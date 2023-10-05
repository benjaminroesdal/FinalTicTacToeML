using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeLibrary
{
    public class Trainer
    {
        public async Task TrainAsync()
        {
            var learningPolicyFilePath = @"C:\Users\benja\Documents\TrainingModel\solid_qlearn_30";
            var savePolicyFilePath = @"C:\Users\benja\Documents\TrainingModel\solid_qlearn";

            //THIS IS TRAINING 4x4 3WL CURRENTLY!
            var numberOfGamesInCycle = 1000000;
            var numberOfCycles = 100;
            var shouldRotate = true;

            const string qLearnTrain = "qLearnTrain";
            var qLearningBot = new QLearningBot(learningPolicyFilePath, true, 50, customName: qLearnTrain);
            var qLearningBot2 = new QLearningBot(learningPolicyFilePath, true, 50, customName: "opponent");
            var randomMoveBot = new RndBot();
            var player = new HumanPlayer();

            var players = new List<IPlayer>()
            {
                qLearningBot,
                qLearningBot2,
                //player
            };

            var resultSet = new QLearningResultSet();
            for (int i = 0; i < numberOfCycles; i++)
            {
                var result = new QLearningResult();
                resultSet.Results[i] = result;

                for (var j = 0; j < numberOfGamesInCycle; j++)
                {
                    var playGame = new ConsoleGame(players[0], players[1]);
                    await playGame.Start();

                    if (playGame.gameResult == GameResult.Draw)
                    {
                        result.Draw();
                    }
                    else
                    {
                        // record the winning type
                        var winningPlayer = playGame.gameResult == GameResult.Player1Win ? players[0] : players[1];
                        var qLearningWon = winningPlayer.GetPlayerName() == "qLearnTrain";

                        if (qLearningWon)
                        {
                            result.Won();
                        }
                        else
                        {
                            result.Lost();
                        }
                    }

                    if (shouldRotate)
                    {
                        // switch order of players around
                        players.Reverse();
                    }
                    //Console.ReadLine();
                }

                await qLearningBot.SavePolicyAsync(savePolicyFilePath + $"_{(i + 1) * numberOfGamesInCycle}");
            }
        }
    }
}
