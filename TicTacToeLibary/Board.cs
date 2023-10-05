using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeLibrary
{
    public class Board
    {
        public int[] Positions { get; set; }
        public List<int> AvailablePosition { get; set; }
        public int WinningLength;
        public Board(int boardSize, int winningLength)
        {
            Positions = new int[boardSize * boardSize];
            WinningLength = winningLength;
            ResetBoard();
        }

        public void ResetBoard()
        {
            AvailablePosition = Enumerable.Range(0, Positions.Length).ToList();
            for (int i = 0; i < Positions.Length; i++)
            {
                Positions[i] = 0;
            }
        }

        public void PlayMove(int movePos, int player)
        {
            Positions[movePos] = player;
            AvailablePosition.Remove(movePos);
        }

        public string GetState()
        {
            return string.Join("", Positions);
        }
    }
}
