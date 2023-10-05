
namespace TicTacToeLibrary
{
    public class GameResultHandler
    {
        private Board Board { get; set; }

        private int boardSize;
        private int winningLength;
        public GameResultHandler(GameState gameState)
        {
            Board = gameState.Board;
            boardSize = (int)Math.Sqrt(Board.Positions.Length);
            this.winningLength = Board.WinningLength; 
        }

        public GameResult CheckResult()
        {
            var playerOneWon = false;
            var playerTwoWon = false;
            //Check if playerOne won
            playerOneWon = playerOneWon == true ? true : CheckDiagonal(PlayerHelper.PlayerOne);
            playerOneWon = playerOneWon == true ? true : CheckColumns(PlayerHelper.PlayerOne);
            playerOneWon = playerOneWon == true ? true : CheckRows(PlayerHelper.PlayerOne);

            //Check if playerTwo won
            if (!playerOneWon)
            {
                playerTwoWon = playerTwoWon == true ? true : CheckDiagonal(PlayerHelper.PlayerTwo);
                playerTwoWon = playerTwoWon == true ? true : CheckColumns(PlayerHelper.PlayerTwo);
                playerTwoWon = playerTwoWon == true ? true : CheckRows(PlayerHelper.PlayerTwo);
            }

            if (playerOneWon)
            {
                return GameResult.Player1Win;
            }
            if (playerTwoWon)
            {
                return GameResult.Player2Win;
            }
            if (!playerOneWon && !playerTwoWon && !Board.Positions.Contains(0))
            {
                return GameResult.Draw;
            }
            return GameResult.InProgress;
        }

        private int GetPosition(int row, int column)
        {
            return Board.Positions[row * boardSize + column];
        }

        public bool CheckDiagonal(int playerNr)
        {
            // Check main diagonal
            for (int i = 0; i <= boardSize - winningLength; i++)
            {
                for (int j = 0; j <= boardSize - winningLength; j++)
                {
                    int count = 0;
                    for (int k = 0; k < winningLength; k++)
                    {
                        if (GetPosition(i + k, j + k) == playerNr)
                        {
                            count++;
                            if (count == winningLength) return true;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            // Check anti-diagonal
            for (int i = 0; i <= boardSize - winningLength; i++)
            {
                for (int j = boardSize - 1; j >= winningLength - 1; j--)
                {
                    int count = 0;
                    for (int k = 0; k < winningLength; k++)
                    {
                        if (GetPosition(i + k, j - k) == playerNr)
                        {
                            count++;
                            if (count == winningLength) return true;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return false;
        }

        public bool CheckRows(int playerNr)
        {
            for (int i = 0; i < boardSize; i++)
            {
                int count = 0;
                for (int j = 0; j < boardSize; j++)
                {
                    if (GetPosition(i, j) == playerNr)
                    {
                        count++;
                        if (count == winningLength) return true;
                    }
                    else
                    {
                        count = 0;
                    }
                }
            }
            return false;
        }

        public bool CheckColumns(int playerNr)
        {
            for (int j = 0; j < boardSize; j++)
            {
                int count = 0;
                for (int i = 0; i < boardSize; i++)
                {
                    if (GetPosition(i, j) == playerNr)
                    {
                        count++;
                        if (count == winningLength) return true;
                    }
                    else
                    {
                        count = 0;
                    }
                }
            }
            return false;
        }
    }
}
