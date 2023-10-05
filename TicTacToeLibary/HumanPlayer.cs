using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeLibrary
{
    public class HumanPlayer : IPlayer
    {
        public PlayerTypes PlayerType => PlayerTypes.Player;

        public Task<int> PlayMove(GameState state, int movPos = -1)
        {
            string? move = "";
            move = movPos == -1 ? Console.ReadLine() : movPos.ToString();
            _ = int.TryParse(move, out var result);
            return Task.FromResult(result);
        }
    }
}
