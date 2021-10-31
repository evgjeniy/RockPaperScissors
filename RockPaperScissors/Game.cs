using System;

namespace RockPaperScissors
{
    static class Game
    {
        public static void PlayGame(string[] parameters, int userMove, int computerMove, byte[] randomKey)
        {
            Console.WriteLine("\nYour move: " + parameters[userMove - 1]); 
            Console.WriteLine("Computer move: " + parameters[computerMove - 1]);
            Console.WriteLine("Result: " + GameLogic(parameters, userMove, computerMove));
            Console.WriteLine("HMAC key: " + Generator.ByteArrayToString(randomKey));
        }
        
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