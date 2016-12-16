using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCodeDay14
{
    class Program
    {
        static void Main(string[] args)
        {
            var salt = "cuanljph";
            var counter = 0;

            var possibleKeys = new Dictionary<long, char>();
            var keys = new List<string>();
            var counters = new List<long>();

            while(true)
            {
                var countersToRemove = new List<long>();
                foreach (var possibleKey in possibleKeys)
                {
                    if (counter - possibleKey.Key >= 1000)
                    {
                        countersToRemove.Add(possibleKey.Key);
                    }
                }

                foreach (var c in countersToRemove)
                {
                    possibleKeys.Remove(c);
                }

                var input = CreateMD5(string.Concat(salt, counter));
                for (int i = 0; i < 2016; i++)
                {
                    input = CreateMD5(input);
                }

                var ch = GetCharDuplicated(input, 3);

                var fiveCharsDuplication = GetCharDuplicated(input, 5);

                var keysToRemove = new List<long>();
                foreach (var possibleKey in possibleKeys)
                {
                    if (possibleKey.Value == fiveCharsDuplication)
                    {
                        //keys.Add(CreateMD5(string.Concat(salt, possibleKey.Key)));
                        counters.Add(possibleKey.Key);
                        keysToRemove.Add(possibleKey.Key);

                        if (counters.Count >= 64)
                        {
                            counters = counters.OrderBy(c => c).ToList();
                            Console.WriteLine("!!!!!!!!! " + counters[63]);
                        }
                    }
                }

                foreach (var key in keysToRemove)
                {
                    possibleKeys.Remove(key);
                }
                
                if (ch != default(char))
                {
                    possibleKeys.Add(counter, ch);
                }

                counter++;
            }

            Console.ReadKey();
        }

        private static readonly uint[] _lookup32 = CreateLookup32();

        private static uint[] CreateLookup32()
        {
            var result = new uint[256];
            for (int i = 0; i < 256; i++)
            {
                string s = i.ToString("X2");
                result[i] = ((uint)s[0]) + ((uint)s[1] << 16);
            }
            return result;
        }

        private static string ByteArrayToHexViaLookup32(byte[] bytes)
        {
            var lookup32 = _lookup32;
            var result = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                var val = lookup32[bytes[i]];
                result[2 * i] = (char)val;
                result[2 * i + 1] = (char)(val >> 16);
            }
            return new string(result);
        }

        public static char GetCharDuplicated(string str, int times)
        {
            var equalTimes = 1;
            char equalChar = str[0];

            for (int i = 1; i < str.Length; i++)
            {
                if (str[i] == equalChar)
                {
                    equalTimes++;
                    if (equalTimes == times)
                    {
                        return equalChar;
                        equalTimes = 0;
                    }
                }
                else
                {
                    equalTimes = 1;
                    equalChar = str[i];
                }
            }

            return default(char);
        }


        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }
        }
    }
}
