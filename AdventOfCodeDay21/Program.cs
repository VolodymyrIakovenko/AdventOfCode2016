using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeDay21
{
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;

    class Program
    {
        static void Main(string[] args)
        {
            var text = System.IO.File.ReadAllText(@"..\..\..\Input\Day21.txt");
            var input = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine(Scrumble("abcdefgh", input));
            Console.WriteLine(Unscrumble("fbgdceah", input.Reverse().ToArray()));

            Console.ReadKey();
        }

        public static string Scrumble(string str, string[] input)
        {
            var stringBuilder = new StringBuilder(str);

            foreach (var line in input)
            {
                if (line.StartsWith("swap"))
                {
                    var newLine = line.Replace("swap ", "");
                    if (newLine.StartsWith("position"))
                    {
                        newLine = newLine.Replace("position ", "");
                        var firstPosition = int.Parse(Regex.Match(newLine, @"\d+").Value);
                        newLine = newLine.Replace(firstPosition + " with ", "");
                        var secondPosition = int.Parse(Regex.Match(newLine, @"\d+").Value);

                        var tmpChar = stringBuilder[firstPosition];
                        stringBuilder[firstPosition] = stringBuilder[secondPosition];
                        stringBuilder[secondPosition] = tmpChar;
                    }

                    else if (newLine.StartsWith("letter"))
                    {
                        newLine = newLine.Replace("letter ", "");
                        var firstLetter = newLine.IndexOf(" ") > -1
                            ? newLine.Substring(0, newLine.IndexOf(" "))
                            : newLine;
                        newLine = newLine.Replace(firstLetter + " with ", "");
                        var secondLetter = newLine.IndexOf(" ") > -1
                            ? newLine.Substring(0, newLine.IndexOf(" "))
                            : newLine;

                        stringBuilder = stringBuilder.Replace(firstLetter, "_");
                        stringBuilder = stringBuilder.Replace(secondLetter, firstLetter);
                        stringBuilder = stringBuilder.Replace("_", secondLetter);
                    }
                }

                else if (line.StartsWith("rotate"))
                {
                    var newLine = line.Replace("rotate ", "");
                    if (newLine.StartsWith("left"))
                    {
                        newLine = newLine.Replace("left ", "");
                        var rotation = int.Parse(Regex.Match(newLine, @"\d+").Value);

                        var currentString = stringBuilder.ToString();
                        stringBuilder = new StringBuilder(LeftRotate(currentString, rotation));
                    }

                    else if (newLine.StartsWith("right"))
                    {
                        newLine = newLine.Replace("right ", "");
                        var rotation = int.Parse(Regex.Match(newLine, @"\d+").Value);

                        var currentString = stringBuilder.ToString();
                        stringBuilder = new StringBuilder(RightRotate(currentString, rotation));
                    }

                    else if (newLine.StartsWith("based"))
                    {
                        newLine = newLine.Replace("based on position of letter ", "");

                        var letter = newLine[0];
                        var currentString = stringBuilder.ToString();

                        var index = currentString.IndexOf(letter);
                        var rotation = index + 1;
                        if (index >= 4)
                        {
                            rotation++;
                        }

                        stringBuilder = new StringBuilder(RightRotate(currentString, rotation));
                    }
                }

                else if (line.StartsWith("reverse"))
                {
                    var newLine = line.Replace("reverse positions ", "");
                    var firstPosition = int.Parse(Regex.Match(newLine, @"\d+").Value);
                    newLine = newLine.Replace(firstPosition + " through ", "");
                    var secondPosition = int.Parse(Regex.Match(newLine, @"\d+").Value);

                    var strArray = stringBuilder.ToString().ToCharArray();
                    Array.Reverse(strArray, firstPosition, secondPosition - firstPosition + 1);
                    stringBuilder = new StringBuilder(new string(strArray));
                }

                else if (line.StartsWith("move"))
                {
                    var newLine = line.Replace("move position ", "");
                    var firstPosition = int.Parse(Regex.Match(newLine, @"\d+").Value);
                    newLine = newLine.Replace(firstPosition + " to position ", "");
                    var secondPosition = int.Parse(Regex.Match(newLine, @"\d+").Value);

                    var tmpChar = stringBuilder[firstPosition];
                    stringBuilder.Remove(firstPosition, 1);
                    stringBuilder.Insert(secondPosition, tmpChar);
                }
            }

            return stringBuilder.ToString();
        }

        public static string Unscrumble(string str, string[] input)
        {
            var stringBuilder = new StringBuilder(str);

            foreach (var line in input)
            {
                if (line.StartsWith("swap"))
                {
                    var newLine = line.Replace("swap ", "");
                    if (newLine.StartsWith("position"))
                    {
                        newLine = newLine.Replace("position ", "");
                        var firstPosition = int.Parse(Regex.Match(newLine, @"\d+").Value);
                        newLine = newLine.Replace(firstPosition + " with ", "");
                        var secondPosition = int.Parse(Regex.Match(newLine, @"\d+").Value);

                        var tmpChar = stringBuilder[secondPosition];
                        stringBuilder[secondPosition] = stringBuilder[firstPosition];
                        stringBuilder[firstPosition] = tmpChar;
                    }

                    else if (newLine.StartsWith("letter"))
                    {
                        newLine = newLine.Replace("letter ", "");
                        var firstLetter = newLine.IndexOf(" ") > -1
                            ? newLine.Substring(0, newLine.IndexOf(" "))
                            : newLine;
                        newLine = newLine.Replace(firstLetter + " with ", "");
                        var secondLetter = newLine.IndexOf(" ") > -1
                            ? newLine.Substring(0, newLine.IndexOf(" "))
                            : newLine;

                        stringBuilder = stringBuilder.Replace(secondLetter, "_");
                        stringBuilder = stringBuilder.Replace(firstLetter, secondLetter);
                        stringBuilder = stringBuilder.Replace("_", firstLetter);
                    }
                }

                else if (line.StartsWith("rotate"))
                {
                    var newLine = line.Replace("rotate ", "");
                    if (newLine.StartsWith("left"))
                    {
                        newLine = newLine.Replace("left ", "");
                        var rotation = int.Parse(Regex.Match(newLine, @"\d+").Value);

                        var currentString = stringBuilder.ToString();
                        stringBuilder = new StringBuilder(RightRotate(currentString, rotation));
                    }

                    else if (newLine.StartsWith("right"))
                    {
                        newLine = newLine.Replace("right ", "");
                        var rotation = int.Parse(Regex.Match(newLine, @"\d+").Value);

                        var currentString = stringBuilder.ToString();
                        stringBuilder = new StringBuilder(LeftRotate(currentString, rotation));
                    }

                    else if (newLine.StartsWith("based"))
                    {
                        newLine = newLine.Replace("based on position of letter ", "");

                        var letter = newLine[0];
                        var currentString = stringBuilder.ToString();

                        var newString = string.Empty;

                        for (int i = 0; i < stringBuilder.Length; i++)
                        {
                            newString = new StringBuilder(LeftRotate(currentString, i)).ToString();

                            var index = newString.IndexOf(letter);
                            var rotation = index + 1;
                            if (index >= 4)
                            {
                                rotation++;
                            }


                            if (rotation % newString.Length == i)
                            {
                                break;
                            }
                        }
                        

                        stringBuilder = new StringBuilder(newString);
                    }
                }

                else if (line.StartsWith("reverse"))
                {
                    var newLine = line.Replace("reverse positions ", "");
                    var firstPosition = int.Parse(Regex.Match(newLine, @"\d+").Value);
                    newLine = newLine.Replace(firstPosition + " through ", "");
                    var secondPosition = int.Parse(Regex.Match(newLine, @"\d+").Value);

                    var strArray = stringBuilder.ToString().ToCharArray();
                    Array.Reverse(strArray, firstPosition, secondPosition - firstPosition + 1);
                    stringBuilder = new StringBuilder(new string(strArray));
                }

                else if (line.StartsWith("move"))
                {
                    var newLine = line.Replace("move position ", "");
                    var firstPosition = int.Parse(Regex.Match(newLine, @"\d+").Value);
                    newLine = newLine.Replace(firstPosition + " to position ", "");
                    var secondPosition = int.Parse(Regex.Match(newLine, @"\d+").Value);

                    var tmpChar = stringBuilder[secondPosition];
                    stringBuilder.Remove(secondPosition, 1);
                    stringBuilder.Insert(firstPosition, tmpChar);
                }
            }

            return stringBuilder.ToString();
        }

        private static string LeftRotate(string key, int shift)
        {
            shift %= key.Length;
            return key.Substring(shift) + key.Substring(0, shift);
        }

        private static string RightRotate(string key, int shift)
        {
            shift %= key.Length;
            return key.Substring(key.Length - shift) + key.Substring(0, key.Length - shift);
        }
    }
}
