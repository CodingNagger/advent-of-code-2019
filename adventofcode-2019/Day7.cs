using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    public class Day7 : Day
    {
        long[] program;

        long maxOutput = 0;

        public string Compute(string[] input)
        {
            program = IntCodeProgramParser.Parse(input);

            int[] phaseSettings = new int[] { 5, 6, 7, 8, 9 };

            foreach (var permuation in phaseSettings.Permutations())
            {
                maxOutput = Math.Max(maxOutput, new IntCodeAmplifier(permuation, program).Run());
            }

            return $"{maxOutput}";
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
