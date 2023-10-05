using TicTacToeLibrary;

namespace TicTacToeWeb.Server
{
    public class GameStateModel
    {
        public Board Board { get; set; }
        public int NextPlayersTurn { get; set; }
        public GameResult GameResult { get; set; }
    }
}
