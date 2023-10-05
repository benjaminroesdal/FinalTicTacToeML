using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeLibrary
{
    public class QLearningBot : IPlayer
    {
        private const double LEARNING_RATE = 0.2d; // alpha
        private const double DISCOUNT_RATE = 0.9d; // decay gamma

        protected readonly Dictionary<string, double> Policy;
        private readonly int _minExplorationRate;
        private readonly int _decreaseExplorationAfter;
        private readonly bool _shouldTrain;
        private readonly string _botName;
        private readonly int _decreaseExplorationCounter;
        private readonly int _turnDelay;
        private int _gamesPlayed;
        private int _explorationRate; // epsilon greedy

        public QLearningBot(
            string? policyFilePath = null,
            bool shouldTrain = false,
            int explorationRate = 30,
            int minExplorationRate = 5,
            int decreaseExplorationAfter = 1000,
            int decreaseExplorationCounter = 1,
            string customName = "",
            int turnDelay = 0,
            Dictionary<string, double>? apiLoadedPolicy = null,
            Dictionary<string, double>? predefinedPolicy = null)
        {
            _shouldTrain = shouldTrain;
            Policy = LoadPolicy(policyFilePath, predefinedPolicy, apiLoadedPolicy);
            _explorationRate = explorationRate;
            _minExplorationRate = minExplorationRate;
            _decreaseExplorationAfter = decreaseExplorationAfter;
            _decreaseExplorationCounter = decreaseExplorationCounter;
            _turnDelay = turnDelay;
            _botName = string.IsNullOrWhiteSpace(customName)
                ? PlayerType.ToString()
                : customName;
        }

        public PlayerTypes PlayerType => PlayerTypes.QLearningBot;

        public string GetPlayerName()
        {
            return _botName;
        }

        public async Task<int> PlayMove(GameState state, int movPos = -1)
        {
            if (_turnDelay > 0)
            {
                await Task.Delay(_turnDelay);
            }

            var myNumber = state.GetPlayersTurnNumber();
            var rnd = new Random();
            if (rnd.Next(1, 100) <= _explorationRate)
            {
                // explore 
                var allAvailableMoves = state.Board.AvailablePosition;
                var randomIndex = rnd.Next(0, allAvailableMoves.Count - 1);
                return allAvailableMoves.ElementAt(randomIndex);
            }

            // exploit
            var topScore = double.MinValue;
            var topSpot = -1;
            var boardState = state.Board.Positions.ToArray();
            foreach (var spot in state.Board.AvailablePosition)
            {
                int[] copiedBoardState = boardState.ToArray();
                copiedBoardState[spot] = myNumber;
                var nextMoveState = $"{myNumber}{string.Join("", copiedBoardState)}";
                if (!Policy.TryGetValue(nextMoveState, out var score))
                {
                    score = 0;
                }

                if (score > topScore)
                {
                    topScore = score;
                    topSpot = spot;
                }
            }

            return topSpot;
        }

        public void GameEnded(GameState state, GameResult result, int myNumber)
        {
            if (_shouldTrain)
            {
                GiveRewardsForGame(state, result, myNumber);
            }

            PolicyUpdatedAsync(CancellationToken.None);
        }

        protected virtual Task PolicyUpdatedAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void GiveRewardsForGame(GameState state, GameResult result, int myNumber)
        {
            var opponentNumber = myNumber == PlayerHelper.PlayerOne
                ? PlayerHelper.PlayerTwo
                : PlayerHelper.PlayerOne;

            var startedFirst = myNumber == PlayerHelper.PlayerOne;

            int myReward;
            int opponentReward;
            if (result == GameResult.Draw)
            {
                //draw       
                myReward = startedFirst ? 20 : 50;
                opponentReward = startedFirst ? 50 : 20;
            }
            else
            {
                var isWin =
                    result == GameResult.Player1Win && myNumber == PlayerHelper.PlayerOne ||
                    result == GameResult.Player2Win && myNumber == PlayerHelper.PlayerTwo;

                if (isWin)
                {
                    // win
                    myReward = 100;
                    opponentReward = -100;
                }
                else
                {
                    // lose
                    myReward = -100;
                    opponentReward = 100;
                }
            }

            FeedReward(state.States, myReward, myNumber);
            FeedReward(state.States, opponentReward, opponentNumber);

            AdjustExplorationRate();
        }

        private void AdjustExplorationRate()
        {
            if (_decreaseExplorationAfter <= 0)
            {
                return;
            }

            // The more trained qLearn bot becomes, the more exploitative it becomes
            _gamesPlayed++;
            if (_gamesPlayed % _decreaseExplorationAfter == _decreaseExplorationCounter)
            {
                _explorationRate -= 1;
                if (_explorationRate < _minExplorationRate)
                {
                    _explorationRate = _minExplorationRate;
                }
            }
        }

        private void FeedReward(List<string> allStates, int reward, int number)
        {
            for (var i = allStates.Count - 1; i >= 0; i--)
            {
                var state = $"{number}{allStates[i]}";
                if (!Policy.ContainsKey(state))
                {
                    Policy[state] = 0;
                }
                var qScore = Policy[state] + LEARNING_RATE * (reward * DISCOUNT_RATE - Policy[state]);
                Policy[state] = qScore;
            }
        }

        public async Task SavePolicyAsync(string policyFilePath)
        {
            var serializedPolicy = JsonConvert.SerializeObject(Policy);
            await File.WriteAllTextAsync(policyFilePath, serializedPolicy);
        }

        private Dictionary<string, double> LoadPolicy(string? policyFilePath, Dictionary<string, double>? predefinedPolicy, Dictionary<string, double>? apiLoadedPolicy)
        {
            if (string.IsNullOrWhiteSpace(policyFilePath) && apiLoadedPolicy == null)
            {
                return predefinedPolicy ?? new Dictionary<string, double>();
            }
            if (predefinedPolicy == null && apiLoadedPolicy == null)
            {
                try
                {
                    var policyStr = File.ReadAllText(policyFilePath);
                    if (string.IsNullOrWhiteSpace(policyStr))
                    {
                        return new Dictionary<string, double>();
                    }

                    var loadedPolicy = JsonConvert.DeserializeObject<Dictionary<string, double>>(policyStr);
                    if (loadedPolicy == null)
                    {
                        return new Dictionary<string, double>();
                    }

                    return loadedPolicy;
                }
                catch (Exception)
                {
                    return new Dictionary<string, double>();
                }
            }
            return apiLoadedPolicy;
        }
    }
}
