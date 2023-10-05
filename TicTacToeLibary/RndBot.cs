using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeLibrary
{
    public class RndBot : IPlayer
    {
        public PlayerTypes PlayerType => PlayerTypes.Bot;

        public Task<int> PlayMove(GameState state, int movPos = -1)
        {
            Random random = new Random();
            var rnd = random.Next(0, state.Board.AvailablePosition.Count);
            var move = state.Board.AvailablePosition.ElementAt(rnd);
            return Task.FromResult(move);
        }
    }
}
