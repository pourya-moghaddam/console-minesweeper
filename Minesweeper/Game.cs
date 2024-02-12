using System;
using System.Text;

namespace Minesweeper
{
    public class Game
    {
        private int _gameSize;
        private int _bombsCount;

        public Game(int gameSize, int bombsCount)
        {
            _gameSize = gameSize;
            _bombsCount = bombsCount;

            Dug = new HashSet<Tuple<int, int>>();

            Board = MakeNewBoard();
            SetBoardValues();
        }

        public HashSet<Tuple<int, int>> Dug { get; set; }
        public object?[,] Board { get; private set; }

        public bool Dig(int row, int col)
        {
            Dug.Add(Tuple.Create(row, col));

            if (Convert.ToChar(Board[row, col]) == '*')
            {
                return false;
            }
            else if (Convert.ToInt32(Board[row, col]) > 0)
            {
                return true;
            }

            // Board[row, col] == 0
            for (int i = Math.Max(0, row - 1);
                i < Math.Min(_gameSize - 1, row + 1) + 1;
                i++)
            {
                for (int j = Math.Max(0, col - 1);
                    j < Math.Min(_gameSize - 1, col + 1) + 1;
                    j++)
                {
                    if (Dug.Contains(Tuple.Create(i, j)))
                    {
                        continue;
                    }
                    Dig(i, j);
                }
            }

            return true;
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            string?[,] visibleBoard = new string?[_gameSize, _gameSize];
            for (int i = 0; i < visibleBoard.GetLength(0); i++)
            {
                for (int j = 0; j < visibleBoard.GetLength(1); j++)
                {
                    visibleBoard[i, j] = null;
                }
            }

            for (int row = 0; row < visibleBoard.GetLength(0); row++)
            {
                for (int col = 0; col < visibleBoard.GetLength(1); col++)
                {
                    if (Dug.Contains(Tuple.Create(row, col)))
                    {
                        visibleBoard[row, col] = Convert.ToString(Board[row, col]);
                    }
                    else
                    {
                        visibleBoard[row, col] = " ";
                    }
                }
            }

            stringBuilder.AppendLine(
                "  " + String.Join(' ', Enumerable.Range(0, visibleBoard.GetLength(0)))
            );
            for (int i = 0; i < visibleBoard.GetLength(0); i++)
            {
                stringBuilder.Append(i + " ");
                for (int j = 0; j < visibleBoard.GetLength(1); j++)
                {
                    stringBuilder.Append(visibleBoard[i, j] + " ");
                }
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
        private void SetBoardValues()
        {
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    if (Convert.ToChar(Board[i, j]) == '*')
                    {
                        continue;
                    }
                    Board[i, j] = GetNeighboringBombsCount(i, j);
                }
            }
        }
        private int GetNeighboringBombsCount(int row, int col)
        {
            int neighboringBombsCount = 0;
            for (int i = Math.Max(0, row - 1);
                i < Math.Min(_gameSize - 1, row + 1) + 1;
                i++)
            {
                for (int j = Math.Max(0, col - 1);
                    j < Math.Min(_gameSize - 1, col + 1) + 1;
                    j++)
                {
                    if ((i == row) && (j == col))
                    {
                        continue;
                    }
                    if (Convert.ToChar(Board[i, j]) == '*')
                    {
                        neighboringBombsCount++;
                    }
                }
            }

            return neighboringBombsCount;
        }
        private object?[,] MakeNewBoard()
        {
            Random random = new Random();

            object?[,] board = new object?[_gameSize, _gameSize];
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] = null;
                }
            }

            int bombsPlanted = 0;
            while (bombsPlanted < _bombsCount)
            {
                int row = random.Next(board.GetLength(0));
                int col = random.Next(board.GetLength(1));
                if (Convert.ToChar(board[row, col]) == '*')
                {
                    continue;
                }
                board[row, col] = '*';
                bombsPlanted++;
            }

            return board;
        }
    }
}
