using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeLibrary
{
    public class ConsoleGame
    {
        GameState state;
        GameRenderer renderer;
        GameResultHandler gameResultHandler;
        bool inProgress = true;
        public GameResult gameResult = GameResult.InProgress;

        public ConsoleGame(IPlayer playerOne, IPlayer playerTwo)
        {
            state = new GameState(playerOne, playerTwo, 4, 4);
            renderer = new GameRenderer(state.Board);
            gameResultHandler = new GameResultHandler(state);
        }

        public async Task Start()
        {
            while (inProgress)
            {
                Console.Clear();
                //await renderer.RenderBoard();
                var playerTurn = state.GetPlayersTurnNumber();
                if (playerTurn == PlayerHelper.PlayerOne && state.Board.AvailablePosition.Count != 0)
                {
                    var move = await state.PlayerOne.PlayMove(state);
                    state.MakeMove(move);
                }
                if (playerTurn == PlayerHelper.PlayerTwo && state.Board.AvailablePosition.Count != 0)
                {
                    var move = await state.PlayerTwo.PlayMove(state);
                    state.MakeMove(move);
                }
                gameResult = gameResultHandler.CheckResult();
                if (gameResult == GameResult.Player1Win)
                {
                    state.GameEnded(gameResult);
                    //await renderer.RenderPlayerOneWin();
                    inProgress = false;
                }
                if (gameResult == GameResult.Player2Win)
                {
                    state.GameEnded(gameResult);
                    //await renderer.RenderPlayerTwoWin();
                    inProgress = false;
                }
                if (gameResult == GameResult.Draw)
                {
                    state.GameEnded(gameResult);
                    //await renderer.RenderDraw();
                    inProgress = false;
                }
            }
        }
    }
}
