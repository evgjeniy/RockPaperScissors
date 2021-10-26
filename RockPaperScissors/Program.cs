using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ConsoleTables;

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
            Console.WriteLine("HMAC: " + ByteArrayToString(hmac));
            
            string choice = Menu.ShowMainMenu(parameters);
            if (choice == "0") return false;
            
            if (choice == "?")
                Menu.ShowHelpMenu(parameters);
            else if (int.TryParse(choice, out int userMove) && userMove <= parameters.Length && userMove >= 0)
                PlayGame(parameters, userMove, computerMove, randomKey);
            else
                Console.WriteLine("Incorrect input");
            
            return true;
        }
        
        private static string ByteArrayToString(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:x2}", b);
            
            return hex.ToString().ToUpper();
        }

        private static void PlayGame(string[] parameters, int userMove, int computerMove, byte[] randomKey)
        {
            Console.WriteLine("\nYour move: " + parameters[userMove - 1]); 
            Console.WriteLine("Computer move: " + parameters[computerMove - 1]);
            Console.WriteLine("Result: " + Game.GameLogic(parameters, userMove, computerMove));
            Console.WriteLine("HMAC key: " + ByteArrayToString(randomKey));
        }
    }
}

namespace RockPaperScissors
{
    static class Generator
    {
        private static RNGCryptoServiceProvider rng = new();
        public static byte[] GetRandomKey()
        {
            byte[] random = new byte[16];
            rng.GetBytes(random);
            
            return random;
        }
        
        public static byte[] GetHmac(byte[] randomKey, string computerMove)
        {
            HMACSHA256 generatorHmac = new HMACSHA256(randomKey);
            byte[] resultHmac = generatorHmac.ComputeHash(Encoding.ASCII.GetBytes(computerMove));
            
            return resultHmac;
        }
    }
}

namespace RockPaperScissors
{
    static class Menu
    {
        private static ConsoleTable _table;

        public static void InitializeHelpMenu(string[] parameters)
        {
            _table = new ConsoleTable("User \\ PC");
            foreach (string parameter in parameters)
                _table.Columns.Add(parameter);

            for (int i = 0; i < parameters.Length; i++)
            {
                string[] row = new string[parameters.Length + 1];
                row[0] = parameters[i];
                for (int j = 0; j < parameters.Length; j++)
                    row[j + 1] = Game.GameLogic(parameters, i, j);
                _table.Rows.Add(row);
            }
        }

        public static void ShowHelpMenu(string[] parameters)
        {
            Console.WriteLine("\nHelp Menu / Game Rules:");
            _table.Write(Format.Alternative);
        }

        public static string ShowMainMenu(string[] parameters)
        {
            Console.WriteLine("Available moves:");
            for (int i = 0; i < parameters.Length; i++)
                Console.WriteLine(i + 1 + " - " + parameters[i]);
            Console.Write("0 - exit\n? - help\nEnter your move: ");

            return Console.ReadLine();
        }
    }
}

namespace RockPaperScissors
{
    static class Game
    {
        public static string GameLogic(string[] parameters, int userMove, int computerMove)
        {
            if (userMove == computerMove) return "Draw";
            int distance = (parameters.Length + 1) / 2;
            if (userMove > computerMove)
                return userMove - computerMove < distance ? "Win" : "Lose";
            return computerMove - userMove < distance ? "Lose" : "Win";
        }
    }
}