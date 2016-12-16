using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeDay16
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "10001001100000001";
            var amountToFill = 35651584;

            var a = input;
            while (a.Length < amountToFill)
            {
                var b = Reverse(a);

                b = b.Replace('1', '-').Replace('0', '1').Replace('-', '0');
                a = string.Concat(string.Concat(a, '0'), b);
            }

            var checksum = string.Concat(a.Take(amountToFill));

            do
            {
                var builder = new StringBuilder();
                for (int i = 0; i < checksum.Length; i += 2)
                {
                    if (i + 1 == checksum.Length)
                    {
                        continue;
                    }

                    builder.Append(checksum[i] == checksum[i + 1] ? '1' : '0');
                }

                checksum = builder.ToString();

            } while (checksum.Length % 2 == 0);

            Console.WriteLine(checksum);
            Console.ReadKey();
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
