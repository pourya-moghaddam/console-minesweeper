using System;

namespace Minesweeper
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Play();
                Console.Write("\nWanna play again? (y/n) ");
            } while (Console.ReadLine().ToLower() == "y");
        }

        static void Play()
        {
            int gameSize = 10;
            int bombsCount = 10;
            Game game = new Game(gameSize, bombsCount);

            bool safe = true;
            while (game.Dug.Count < (Math.Pow(gameSize, 2) - bombsCount))
            {
                Console.WriteLine(game);

                try
                {
                    Tuple<int, int> playerChoice = GetUserChoice();
                    int rowChoice = playerChoice.Item1;
                    int colChoice = playerChoice.Item2;
                    bool isRowInvalid = (rowChoice < 0) || (rowChoice >= gameSize);
                    bool isColInvalid = (colChoice < 0) || (colChoice >= gameSize);
                    if ((isRowInvalid) || (isColInvalid))
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid position. Try again.");
                        continue;
                    }
                    else if (game.Dug.Contains(Tuple.Create(rowChoice, colChoice)))
                    {
                        Console.Clear();
                        Console.WriteLine("You've dug that spot. Try again.");
                        continue;
                    }

                    Console.Clear();

                    safe = game.Dig(rowChoice, colChoice);
                    if (!safe)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("error: " + ex.Message);
                }
            }

            if (safe)
            {
                Console.WriteLine("You WON!!!");
            }
            else
            {
                Console.WriteLine("GaMe OvEr :(((");
                // Reveal the whole board
                game.Dug.Clear();
                for (int i = 0; i < gameSize; i++)
                {
                    for (int j = 0; j < gameSize; j++)
                    {
                        game.Dug.Add(Tuple.Create(i, j));
                    }
                }
                Console.WriteLine(game);
            }
        }

        static Tuple<int, int> GetUserChoice()
        {
            // Receive user input in 'x y' format and split it to row and column number.
            Console.Write("Choose position ([row] [column]): ");
            string? choice = Console.ReadLine();
            string[] splitedChoice = new string[2];
            if (choice != null)
            {
                splitedChoice = choice.Split(' ');
            }
            int[] myInts = Array.ConvertAll(splitedChoice, int.Parse);
            Tuple<int, int> moveChoice = Tuple.Create(myInts[0], myInts[1]);

            return moveChoice;
        }
    }
}
