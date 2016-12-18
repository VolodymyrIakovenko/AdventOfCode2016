using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeDay18
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = new List<string>();

            var startingMap = "^^.^..^.....^..^..^^...^^.^....^^^.^.^^....^.^^^...^^^^.^^^^.^..^^^^.^^.^.^.^.^.^^...^^..^^^..^.^^^^";
            map.Add(startingMap);

            var safeTiles = startingMap.Count(t => t.Equals('.'));

            for (int i = 1; i < 400000; i++)
            {
                var newRow = string.Empty;
                var lastRow = map[map.Count - 1];

                for (var j = 0; j < lastRow.Length; j++)
                {
                    var leftIsTrap = j - 1 >= 0 && lastRow[j - 1] == '^';
                    var centerIsTrap = lastRow[j] == '^';
                    var righIsTrap = j + 1 < lastRow.Length && lastRow[j + 1] == '^';

                    if ((leftIsTrap && centerIsTrap && !righIsTrap) ||
                        (!leftIsTrap && centerIsTrap && righIsTrap) ||
                        (leftIsTrap && !centerIsTrap && !righIsTrap) ||
                        (!leftIsTrap && !centerIsTrap && righIsTrap))
                    {
                        newRow += '^';
                    }
                    else
                    {
                        newRow += '.';
                        safeTiles++;
                    }
                }

                map.Add(newRow);
            }

            foreach (var row in map)
            {
                Console.WriteLine(row);
            }

            Console.WriteLine();
            Console.WriteLine("Safe tiles: " + safeTiles);

            Console.ReadKey();
        }
    }
}
