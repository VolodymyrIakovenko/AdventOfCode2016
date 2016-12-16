using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeDay12
{
    using System.Text.RegularExpressions;

    class Program
    {
        static void Main(string[] args)
        {
            var registers = new Dictionary<char, int>();

            var text = System.IO.File.ReadAllText(@"..\..\..\Input\Day12.txt");
            var inputArray = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < inputArray.Length; i++)
            {
                //Console.WriteLine(inputArray[i]);

                if (inputArray[i].StartsWith("cpy"))
                {
                    var command = inputArray[i].Replace("cpy ", "");
                    var copyValues = command.Split(' ');

                    var copyValue = 0;
                    var res = int.TryParse(copyValues[0], out copyValue);
                    if (!res)
                    {
                        copyValue = registers[copyValues[0][0]];
                    }

                    var registerToCopy = copyValues[1][0];

                    if (registers.ContainsKey(registerToCopy))
                    {
                        registers[registerToCopy] = copyValue;
                    }
                    else
                    {
                        registers.Add(registerToCopy, copyValue);
                    }
                }
                else if (inputArray[i].StartsWith("inc"))
                {
                    var command = inputArray[i].Replace("inc ", "");
                    var registerToIncrease = command[command.Length - 1];

                    if (registers.ContainsKey(registerToIncrease))
                    {
                        registers[registerToIncrease]++;
                    }
                }
                else if (inputArray[i].StartsWith("dec"))
                {
                    var command = inputArray[i].Replace("dec ", "");
                    var registerToDecrease = command[command.Length - 1];

                    if (registers.ContainsKey(registerToDecrease))
                    {
                        registers[registerToDecrease]--;
                    }

                    if (registerToDecrease == 'd')
                    {
                        Console.WriteLine(registers[registerToDecrease]);
                    }
                }
                else if (inputArray[i].StartsWith("jnz"))
                {
                    var command = inputArray[i].Replace("jnz ", "");
                    var jumpValues = command.Split(' ');

                    var jumpValue = int.Parse(jumpValues[1]);

                    var registerValue = 0;
                    var res = int.TryParse(jumpValues[0], out registerValue);
                    if (!res)
                    {
                        var c = jumpValues[0][0];
                        if (registers.ContainsKey(c))
                        {
                            registerValue = registers[c];
                        }
                    }

                    if (registerValue != 0)
                    {
                        i += jumpValue - 1;
                    }

                    //foreach (var register in registers)
                    //{
                    //    Console.Write(register.Key + ": " + register.Value + "\t");
                    //}

                    //Console.WriteLine();
                    //Console.WriteLine();
                }

                
                //Console.ReadKey();
            }

            Console.WriteLine("!!!!! " + registers['a']);
            Console.ReadKey();
        }
    }
}
