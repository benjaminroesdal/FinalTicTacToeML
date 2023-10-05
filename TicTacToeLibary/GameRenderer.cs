using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeLibrary
{
    public class GameRenderer
    {
        private readonly Board Board;
        public GameRenderer(Board board) 
        {
            Board = board;
        }

        public async Task RenderBoard()
        {
            int boardSideLength = (int)Math.Sqrt(Board.Positions.Length);
            for (int i = 0; i < Board.Positions.Length; i++)
            {
                if (i % boardSideLength == 0)
                {
                    Console.Write("\n|");
                }
                if (Board.Positions[i] == PlayerHelper.PlayerOne)
                {
                    Console.Write($"{PlayerHelper.PlayerOneSymbol}|");
                }
                else if (Board.Positions[i] == PlayerHelper.PlayerTwo)
                {
                    Console.Write($"{PlayerHelper.PlayerTwoSymbol}|");
                }
                else
                {
                    Console.Write(" |");
                }
            }
        }

        public void WriteEmptyPosition()
        {
            Console.WriteLine("0");
        }

        public async Task RenderPlayerOneWin()
        {
            Console.Clear();
            await RenderBoard();
            Console.WriteLine("\nPlayerOne won the game! Congratz!");
        }

        public async Task RenderPlayerTwoWin()
        {
            Console.Clear();
            await RenderBoard();
            Console.WriteLine("\nPlayerTwo won the game! Congratz!");
        }

        public async Task RenderDraw()
        {
            Console.Clear();
            await RenderBoard();
            Console.WriteLine("\nThe Game ended in a draw :(");
        }
    }
}
