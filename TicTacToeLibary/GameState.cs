using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeLibrary
{
    public class GameState
    {
        public Board Board { get; set; }
        public IPlayer PlayerOne { get; set; }
        public IPlayer PlayerTwo { get; set; }
        public IPlayer PlayersTurn { get; set; }

        public List<string> States { get; set; }

        public GameState(IPlayer playerOne, IPlayer playerTwo, int boardSize, int winningLength)
        {
            Board = new Board(boardSize, winningLength);
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
            PlayersTurn = playerOne;
            States = new List<string>
            {
                Board.GetState()
            };
        }

        public void MakeMove(int movePos)
        {
            if (!Board.AvailablePosition.Any(e => e == movePos))
            {
                throw new Exception($"'{movePos}' is not an eligible move");
            }
            var playerNr = GetPlayersTurnNumber();
            Board.PlayMove(movePos, playerNr);
            States.Add(Board.GetState());
            ChangePlayerTurn();
        }

        public void GameEnded(GameResult gameResult)
        {
            PlayerOne.GameEnded(this, gameResult, PlayerHelper.PlayerOne);
            PlayerTwo.GameEnded(this, gameResult, PlayerHelper.PlayerTwo);
        }

        public void ChangePlayerTurn()
        {
            PlayersTurn = PlayersTurn == PlayerOne ? PlayerTwo : PlayerOne;
        }

        public int GetPlayersTurnNumber()
        {
            return PlayersTurn == PlayerOne ? PlayerHelper.PlayerOne : PlayerHelper.PlayerTwo;
        }

        public string GetPlayersTurnSymbol()
        {
            return PlayersTurn == PlayerOne ? PlayerHelper.PlayerOneSymbol : PlayerHelper.PlayerTwoSymbol;
        }
    }
}
