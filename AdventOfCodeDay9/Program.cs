using System;

namespace AdventOfCodeDay9
{
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;

    class Program
    {
        public static long decompressedText = 0;
        

        static void Main(string[] args)
        {
            var text = System.IO.File.ReadAllText(@"..\..\..\Input\Day9.txt");
            var aStringBuilder = new StringBuilder();

            var newText = SplitText(text);

            //var inputArray = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var previousK = 0;

            for (var k = 0; k < newText.Count; k++)
            {
                if (previousK != k)
                {
                    newText[previousK] = null;
                }

                previousK = k;
                text = newText[k];


                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i].Equals('('))
                    {
                        var expression = "";
                        for (int j = i; j < text.Length; j++)
                        {
                            if ((char.IsLetter(text[j]) && text[j] != 'x'))
                            {
                                Console.WriteLine("!!!!!!! " + expression);
                                Console.ReadKey();
                            }
                            else if (text[j].Equals(')'))
                            {
                                expression = text.Substring(i, j - i + 1);

                                break;
                            }
                        }

                        var textRangeAmount = int.Parse(Regex.Match(expression, @"\d+").Value);

                        var textRangeStartIndex = i + expression.Length;
                        var textRange = text.Substring(textRangeStartIndex, textRangeAmount);

                        var textRepeatAmount = int.Parse(Regex.Match(expression, @"x\d+").Value.Replace("x", ""));

                        var builder = new StringBuilder();
                        for (int j = 0; j < textRepeatAmount; j++)
                        {
                            builder.Append(textRange);
                        }

                        var result = builder.ToString();

                        text = text.Replace(expression + textRange, result);
                        i--;

                        //Console.WriteLine(text.Length);

                        if (text.Length > 100000)
                        {
                            var moreNewText = SplitText(text);
                            newText.RemoveAt(k);

                            newText.InsertRange(k, moreNewText);

                            decompressedText -= text.Length;

                            k--;
                            break;

                        }

                    }
                }

                decompressedText += text.Length;
                Console.WriteLine(decompressedText + " " + (newText.Count - k));
            }
                        
            Console.WriteLine(decompressedText);
            Console.ReadKey();
        }

        private static List<string> SplitText(string text)
        {
            var newText = new List<string>();
            var builder = new StringBuilder();

            newText.Add("");

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].Equals('('))
                {
                    var expression = "";
                    for (int j = i; j < text.Length; j++)
                    {
                        if (text[j].Equals(')'))
                        {
                            expression = text.Substring(i, j - i + 1);

                            break;
                        }
                    }

                    var textRangeAmount = int.Parse(Regex.Match(expression, @"\d+").Value);

                    var textRangeStartIndex = i + expression.Length;
                    var textRange = text.Substring(textRangeStartIndex, textRangeAmount);
                    //textRange = Regex.Replace(textRange, @" ?\(.*?\)", "");

                    if (builder.Length > 0)
                    {
                        newText.Add(builder.ToString());
                    }

                    builder.Clear();

                    //if (textRange.Contains("("))
                    //{
                    //    newText[newText.Count - 1] += expression + textRange;
                    //}
                    //else
                    {
                        newText.Add(expression + textRange);
                    }

                    i += expression.Length + textRangeAmount - 1;
                }
                else
                {
                    builder.Append(text[i]);
                }
            }
                        
            if (builder.Length > 0)
            {
                newText[newText.Count - 1] += builder.ToString();
            }

            builder.Clear();

            return newText;
        }
    }
}
