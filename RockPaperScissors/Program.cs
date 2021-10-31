using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RockPaperScissors
{
    static class Program
    {
        private static void Main(string[] args)
        {
            if (CorrectlyInput(args))
            {
                Menu.InitializeHelpMenu(args);
                while (GameCycle(args))
                {
                    Console.Write("\nPress any key to continue...");
                    Console.ReadKey();
                }
                Console.Clear();
            }
            else
            {
                Console.ReadKey();
            }
        }
        
        private static bool CorrectlyInput(string[] args)
        {
            string example = "Example 1: ...>start RockPaperScissors.exe Rock Paper Scissors\n" +
                             "Example 2: ...>start RockPaperScissors.exe 1 2 3 4 5\n";
            if (args.Length < 3)
            {
                Console.WriteLine("The number of parameters must be at least 3\n" + example +
                                  "BUT NOT: ...>start RockPaperScissors.exe Rock Scissors\n");
                return false;
            }
            if (args.Length % 2 == 0)
            {
                Console.WriteLine("The number of parameters must be odd\n" + example +
                                  "BUT NOT: ...>start RockPaperScissors.exe Rock Paper Scissors Pit\n");
                return false;
            }
            if (args.Length != args.Distinct().Count())
            {
                Console.WriteLine("Input parameters should not be repeated\n" + example +
                                  "BUT NOT: ...>start RockPaperScissors.exe Rock Paper Rock\n");
                return false;
            }
            return true;
        }

        private static bool GameCycle(string[] parameters)
        {
            Console.Clear();
            
            byte[] randomKey = Generator.GetRandomKey();
            int computerMove = RandomNumberGenerator.GetInt32(1, parameters.Length + 1);
            byte[] hmac = Generator.GetHmac(randomKey, parameters[computerMove - 1]);
            Console.WriteLine("HMAC: " + Generator.ByteArrayToString(hmac));
            
            string choice = Menu.ShowMainMenu(parameters);
            if (choice == "0") return false;
            
            if (choice == "?")
                Menu.ShowHelpMenu(parameters);
            else if (int.TryParse(choice, out int userMove) && userMove <= parameters.Length && userMove >= 0)
                Game.PlayGame(parameters, userMove, computerMove, randomKey);
            else
                Console.WriteLine("Incorrect input");
            
            return true;
        }
    }
}