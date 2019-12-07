using System;
using System.Linq;

namespace AdventOfCode2019
{
    class Day4 : Day
    {
        public override string Compute(string[] input)
        {
            int[] range = input[0].Split('-').Select(r => int.Parse(r)).ToArray();
            int validPasswordCounts = 0;

            for (int cursor = range[0]; cursor <= range[1]; cursor++)
            {
                if (isValidPassword($"{cursor}"))
                {
                    validPasswordCounts++;
                }
            }

            return $"{validPasswordCounts}";
        }

        bool isValidPassword(string password)
        {
            int cursor = 0;
            bool validDoublePairsFound = false;
            int currentMatchingStreak = 0;
            bool neverDecreases = true;

            while (cursor < password.Length - 1)
            {
                currentMatchingStreak = 0;
                while (cursor < password.Length - 1 && password[cursor] == password[cursor + 1])
                {
                    cursor++;
                    currentMatchingStreak++;
                }

                if (cursor < password.Length - 1)
                {
                    if (password[cursor] > password[cursor + 1])
                    {
                        neverDecreases = false;
                    }

                    if (currentMatchingStreak == 1)
                    {
                        validDoublePairsFound = true;
                    }
                    else
                    {
                        cursor++;
                    }
                }
            }

            if (currentMatchingStreak == 1)
            {
                validDoublePairsFound = true;
            }

            return cursor == password.Length - 1 && validDoublePairsFound && neverDecreases;
        }
    }
}
