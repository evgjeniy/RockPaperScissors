using System.Security.Cryptography;
using System.Text;

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

        public static string ByteArrayToString(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:x2}", b);
            
            return hex.ToString().ToUpper();
        }
    }
}