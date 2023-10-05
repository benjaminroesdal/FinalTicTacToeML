using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeLibrary
{
    public interface IPlayer
    {
        Task<int> PlayMove(GameState state, int movPos = -1);
        PlayerTypes PlayerType { get; }
        string GetPlayerName() 
            => PlayerType.ToString();
        void GameEnded(GameState state, GameResult result, int myNumber) { }
    }
}
