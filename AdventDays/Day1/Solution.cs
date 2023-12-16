using AdventOfCode2023.AdventDays.Reusables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.AdventDays.Day1
{
    public class Solution : SolutionBase
    {
        #region Constructors
        public Solution()
        {
            string filePath = @".\..\..\..\AdventDays\Day1\Inputs\PuzzleInput.txt";
            PuzzleLines = File.ReadLines(filePath).ToList();
        }
        #endregion

        #region Enums and Dictionaries
        Dictionary<string, int> IntValueOf = new Dictionary<string, int>()
        {
            { "zero",  0 },
            { "one",   1 },
            { "two",   2 },
            { "three", 3 },
            { "four",  4 },
            { "five",  5 },
            { "six",   6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine",  9 }
        };
        #endregion

        #region Properties
        protected override List<string> PuzzleLines { get; }
        #endregion

        #region Methods
        private bool IsDigit(char c)
            => c >= '0' && c <= '9';

        private int? FirstNumericalDigit(string line)
        { 
            char digitChar = line.FirstOrDefault(c => IsDigit(c), '?');
            return (digitChar == '?') ? null : digitChar - '0';
        }

        private int? LastNumericalDigit(string line)
        {
            char digitChar = line.LastOrDefault(c => IsDigit(c), '?');
            return (digitChar == '?') ? null : digitChar - '0';
        }

        private int? FirstAlphanumericalDigit(string line)
        {
            int firstDigitIndex = line.Length - 1;
            int? firstDigit = FirstNumericalDigit(line);

            if (firstDigit != null)
                firstDigitIndex = line.IndexOf((char)(firstDigit + '0'));

            string lineToSearch = line.Substring(0, firstDigitIndex);

            foreach(string number in IntValueOf.Keys)
            {
                int foundIndex = lineToSearch.IndexOf(number);
                if (foundIndex != -1 && foundIndex < firstDigitIndex)
                {
                    firstDigitIndex = foundIndex;
                    firstDigit = IntValueOf[number];
                }
            }

            return firstDigit;
        }

        private int? LastAlphanumericalDigit(string line)
        {
            int lastDigitIndex = 0;
            int? lastDigit  = LastNumericalDigit(line);

            if (lastDigit != null)
                lastDigitIndex = line.LastIndexOf((char)(lastDigit + '0'));

            string lineToSearch = line.Substring(lastDigitIndex);
            int indexStart = lastDigitIndex;

            foreach (string number in IntValueOf.Keys)
            {
                int foundIndex = lineToSearch.LastIndexOf(number) + indexStart;
                if (foundIndex != -1 && foundIndex > lastDigitIndex)
                {
                    lastDigitIndex = foundIndex;
                    lastDigit = IntValueOf[number];
                }
            }

            return lastDigit;
        }
        #endregion

        #region Parts
        public override long Part1()
        {
            long result = 0;

            foreach (string line in PuzzleLines)
            {
                int? firstDigit = FirstNumericalDigit(line);
                int? lastDigit = LastNumericalDigit(line);

                if (firstDigit == null || lastDigit == null)
                    continue;

                result += (int)firstDigit * 10 + (int)lastDigit;
            }

            return result;
        }

        public override long Part2()
        {
            long result = 0;

            foreach (string line in PuzzleLines)
            {
                int? firstDigit = FirstAlphanumericalDigit(line);
                int? lastDigit = LastAlphanumericalDigit(line);

                if (firstDigit == null || lastDigit == null)
                    continue;

                result += (int)firstDigit * 10 + (int)lastDigit;
            }

            return result;
        }
        #endregion
    }
}