using System;
using ConsoleTables;

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