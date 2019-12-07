using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    public class Day7 : Day
    {
        long[] baseValues;

        long maxOutput = 0;

        public override string Compute(string[] input)
        {
            baseValues = ParseInput(input);

            int[] phaseSettings = new int[] { 5, 6, 7, 8, 9 };


            foreach (var permuation in phaseSettings.Permutations())
            {
                Console.WriteLine("New permutation");
                var a = new IntCodeComputer(permuation[0], baseValues);
                var b = new IntCodeComputer(permuation[1], baseValues);
                var c = new IntCodeComputer(permuation[2], baseValues);
                var d = new IntCodeComputer(permuation[3], baseValues);
                var e = new IntCodeComputer(permuation[4], baseValues);

                long? outputE = 0;

                while (e.NotHalted) {
                    var outputA = a.RunIntcodeProgram(outputE.HasValue ? outputE.Value : 0 ) ?? 0;
                    var outputB = b.RunIntcodeProgram(outputA) ?? 0;
                    var outputC = c.RunIntcodeProgram(outputB) ?? 0;
                    var outputD = d.RunIntcodeProgram(outputC) ?? 0;
                    outputE = e.RunIntcodeProgram(outputD);
                }

                if (outputE.HasValue) {
                    maxOutput = Math.Max(maxOutput, outputE.Value);
                }
               
            }

            return $"{maxOutput}";
        }

        private long[] ParseInput(string[] input)
        {
            return input[0].Split(',').Select(s => long.Parse(s.Trim())).ToArray();
        }
    }

    public static class PermutationExtension
    {
        public static IEnumerable<T[]> Permutations<T>(this IEnumerable<T> source)
        {
            var sourceArray = source.ToArray();
            var results = new List<T[]>();
            Permute(sourceArray, 0, sourceArray.Length - 1, results);
            return results;
        }

        private static void Swap<T>(ref T a, ref T b)
        {
            T tmp = a;
            a = b;
            b = tmp;
        }

        private static void Permute<T>(T[] elements, int recursionDepth, int maxDepth, ICollection<T[]> results)
        {
            if (recursionDepth == maxDepth)
            {
                results.Add(elements.ToArray());
                return;
            }

            for (var i = recursionDepth; i <= maxDepth; i++)
            {
                Swap(ref elements[recursionDepth], ref elements[i]);
                Permute(elements, recursionDepth + 1, maxDepth, results);
                Swap(ref elements[recursionDepth], ref elements[i]);
            }
        }
    }
}
