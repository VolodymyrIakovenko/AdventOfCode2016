using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCodeDay10
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = System.IO.File.ReadAllText(@"..\..\..\Input\Day10.txt");
            var input = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var listValues = new List<object>();

            foreach (var line in input)
            {
                if (line.StartsWith("value"))
                {
                    var newLine = line.Replace("value ", "");
                    var valueAmount = Regex.Match(newLine, @"\d+").Value;
                    newLine = newLine.Replace(valueAmount + " goes to bot ", "");
                    var botNumber = Regex.Match(newLine, @"\d+").Value;

                    listValues.Add(new GiveValue(int.Parse(valueAmount), new Bot(int.Parse(botNumber))));
                }
                else if (line.StartsWith("bot"))
                {
                    var newLine = line.Remove(0, 4);
                    var botNumber = Regex.Match(newLine, @"\d+").Value;
                    newLine = newLine.Replace(botNumber + " gives low to ", "");

                    if (newLine.StartsWith("output"))
                    {
                        var continueLine = newLine.Remove(0, 7);
                        var low = Regex.Match(continueLine, @"\d+").Value;
                        continueLine = continueLine.Replace(low + " and high to ", "");

                        if (continueLine.StartsWith("output"))
                        {
                            var continueLine2 = continueLine.Remove(0, 7);
                            var high = Regex.Match(continueLine2, @"\d+").Value;

                            listValues.Add(new BotComparison(new Bot(int.Parse(botNumber)), null, null, new Bin(int.Parse(low)), new Bin(int.Parse(high))));
                        }
                        else if (continueLine.StartsWith("bot"))
                        {
                            var continueLine2 = continueLine.Remove(0, 4);
                            var high = Regex.Match(continueLine2, @"\d+").Value;

                            listValues.Add(new BotComparison(new Bot(int.Parse(botNumber)), null, new Bot(int.Parse(high)), new Bin(int.Parse(low)), null));
                        }

                    }
                    else if (newLine.StartsWith("bot"))
                    {
                        var continueLine = newLine.Remove(0, 4);
                        var low = Regex.Match(continueLine, @"\d+").Value;
                        continueLine = continueLine.Replace(low + " and high to ", "");

                        if (continueLine.StartsWith("output"))
                        {
                            var continueLine2 = continueLine.Remove(0, 7);
                            var high = Regex.Match(continueLine2, @"\d+").Value;

                            listValues.Add(new BotComparison(new Bot(int.Parse(botNumber)), new Bot(int.Parse(low)), null, null, new Bin(int.Parse(high))));
                        }
                        else if (continueLine.StartsWith("bot"))
                        {
                            var continueLine2 = continueLine.Remove(0, 4);
                            var high = Regex.Match(continueLine2, @"\d+").Value;

                            listValues.Add(new BotComparison(new Bot(int.Parse(botNumber)), new Bot(int.Parse(low)), new Bot(int.Parse(high)), null, null));
                        }
                    }
                }
            }

            var bots = new List<Bot>();

            var bins = new List<Bin>();
            for (int i = 0; i < listValues.Count; i++)
            {
                if (listValues[i] is GiveValue)
                {
                    var item = (GiveValue)listValues[i];
                    var index = bots.FindIndex(b => b.Number == item.Bot.Number);
                    if (index != -1)
                    {
                        bots[index].Values.Add(item.Val);
                    }
                    else
                    {

                        var bot = item.Bot;
                        bot.Values.Add(item.Val);
                        bots.Add(bot);
                    }
                }
                else
                {
                    var item = (BotComparison)listValues[i];

                    var bot = bots.Find(b => b.Number == item.Bot.Number);
                    if (bot == null || bot.Values.Count < 2)
                    {
                        listValues.Add(item);
                        continue;
                    }

                    var lowValue = bot.Values.Min();
                    var highValue = bot.Values.Max();

                    if (item.LowerValToBin != null)
                    {
                        var index = bins.FindIndex(b => b.Number == item.LowerValToBin.Number);
                        if (index != -1)
                        {
                            bins[index].Values.Add(lowValue);
                        }
                        else
                        {
                            var bin = item.LowerValToBin;
                            bin.Values.Add(lowValue);
                            bins.Add(bin);
                        }
                    }

                    if (item.LowerValToBot != null)
                    {
                        var index = bots.FindIndex(b => b.Number == item.LowerValToBot.Number);
                        if (index != -1)
                        {
                            bots[index].Values.Add(lowValue);
                        }
                        else
                        {
                            var newBot = item.LowerValToBot;
                            newBot.Values.Add(lowValue);
                            bots.Add(newBot);
                        }
                    }

                    if (item.HigherValToBin != null)
                    {
                        var index = bins.FindIndex(b => b.Number == item.HigherValToBin.Number);
                        if (index != -1)
                        {
                            bins[index].Values.Add(highValue);
                        }
                        else
                        {
                            var bin = item.HigherValToBin;
                            bin.Values.Add(highValue);
                            bins.Add(bin);
                        }
                    }

                    if (item.HigherValToBot != null)
                    {
                        var index = bots.FindIndex(b => b.Number == item.HigherValToBot.Number);
                        if (index != -1)
                        {
                            bots[index].Values.Add(highValue);
                        }
                        else
                        {
                            var newBot = item.HigherValToBot;
                            newBot.Values.Add(highValue);
                            bots.Add(newBot);
                        }
                    }

                    bot.Values.Clear();                    
                }
                
            }

            foreach (var bin in bins.Where(b => b.Number == 0 || b.Number == 1 || b.Number == 2))
            {
                Console.Write("Bin " + bin.Number + ": ");
                foreach (var item in bin.Values)
                {
                    Console.Write(item + ", ");
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }

        private class Entity
        {
            public int Number { get; protected set; }
        }

        private class Bot : Entity
        {
            public Bot(int number)
            {
                Number = number;
            }

            public List<int> Values = new List<int>();
        }

        private class Bin : Entity
        {
            public Bin(int number)
            {
                Number = number;
            }

            public List<int> Values = new List<int>();
        }

        private class GiveValue
        {
            public GiveValue(int val, Bot bot)
            {
                Val = val;
                Bot = bot;
            }

            public int Val { get; }

            public Bot Bot { get; }
        }

        private class BotComparison
        {
            public BotComparison(Bot bot, Bot lowerValToBot, Bot higherValToBot, Bin lowerValToBin, Bin higherValToBin)
            {
                Bot = bot;
                HigherValToBot = higherValToBot;
                LowerValToBot = lowerValToBot;
                HigherValToBin = higherValToBin;
                LowerValToBin = lowerValToBin;
            }

            public Bot Bot { get; }

            public Bot LowerValToBot { get; }

            public Bot HigherValToBot { get; }

            public Bin LowerValToBin { get; }

            public Bin HigherValToBin { get; }

        }
    }
}
