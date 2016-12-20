using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeDay20
{
    class Program
    {
        public class Range
        {
            public long StartValue;
            public long EndValue;

            public Range(long startValue, long endValue)
            {
                StartValue = startValue;
                EndValue = endValue;
            }

            public bool IsWithin(long number)
            {
                return number >= StartValue && number <= EndValue;
            }

            public override string ToString()
            {
                return StartValue + " - " + EndValue;
            }
        }

        static void Main(string[] args)
        {
            var text = System.IO.File.ReadAllText(@"..\..\..\Input\Day20.txt");
            var input = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var invalidValues = new List<Range>();

            foreach (var range in input)
            {
                var minMaxValues = range.Split('-');
                var startValue = Convert.ToInt64(minMaxValues[0]);
                var endValue = Convert.ToInt64(minMaxValues[1]);


                foreach (var invalidValue in invalidValues)
                {
                    if (invalidValue.IsWithin(startValue))
                    {
                        startValue = invalidValue.EndValue + 1;
                    }
                    
                    if (invalidValue.IsWithin(endValue))
                    {
                        endValue = invalidValue.StartValue - 1;
                    }
                }

                if (startValue <= endValue)
                {
                    invalidValues.Add(new Range(startValue, endValue));
                }
            }

            invalidValues = invalidValues.OrderBy(iv => iv.StartValue).ToList();

            long minInvalid = 0;
            long allowedAmount = 0;
            foreach (var range in invalidValues)
            {
                var diff = range.StartValue - minInvalid;
                if (range.StartValue - minInvalid > 1)
                {
                    allowedAmount += diff - 1;
                }

                if (range.EndValue > minInvalid)
                {
                    minInvalid = range.EndValue;
                }
            }

            const long maxAmount = 4294967295;
            allowedAmount += maxAmount - minInvalid;

            Console.WriteLine(allowedAmount);
            Console.ReadKey();
        }
    }
}
