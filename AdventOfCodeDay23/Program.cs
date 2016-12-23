using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeDay23
{
    class Program
    {
        static void Main(string[] args)
        {
            var registers = new Dictionary<char, int> { {'a', 12 } };

            var text = System.IO.File.ReadAllText(@"..\..\..\Input\Day23.txt");
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

                    var registerValue1 = 0;
                    var res1 = int.TryParse(jumpValues[1], out registerValue1);
                    if (!res1)
                    {
                        var c = jumpValues[1][0];
                        if (registers.ContainsKey(c))
                        {
                            registerValue1 = registers[c];
                        }
                    }

                    var registerValue2 = 0;
                    var res2 = int.TryParse(jumpValues[0], out registerValue2);
                    if (!res2)
                    {
                        var c = jumpValues[0][0];
                        if (registers.ContainsKey(c))
                        {
                            registerValue2 = registers[c];
                        }
                    }

                    if (registerValue2 != 0)
                    {
                        i += registerValue1 - 1;
                    }
                }
                else if (inputArray[i].StartsWith("tgl"))
                {
                    var command = inputArray[i].Replace("jnz ", "");
                    var tglValue = command[command.Length - 1];

                    var offset = 0;
                    var res = int.TryParse(tglValue.ToString(), out offset);
                    if (!res)
                    {
                        offset = registers[tglValue];
                    }

                    if (i + offset >= inputArray.Length)
                    {
                        continue;
                    }

                    var splittedLine = inputArray[i + offset].Split(' ');
                    var arguments = splittedLine.Length - 1;
                    
                    if (arguments == 1)
                    {
                        if (inputArray[i + offset].StartsWith("inc"))
                        {
                            inputArray[i + offset] = inputArray[i + offset].Replace("inc", "dec");
                        }
                        else
                        {
                            var firstWord = splittedLine[0];
                            inputArray[i + offset] = inputArray[i + offset].Replace(firstWord, "inc");
                        }
                    }

                    if (arguments == 2)
                    {
                        if (inputArray[i + offset].StartsWith("jnz"))
                        {
                            inputArray[i + offset] = inputArray[i + offset].Replace("jnz", "cpy");
                        }
                        else
                        {
                            var firstWord = splittedLine[0];
                            inputArray[i + offset] = inputArray[i + offset].Replace(firstWord, "jnz");
                        }
                    }
                }
            }

            Console.WriteLine(registers['a']);
            Console.ReadKey();
        }
    }
}
