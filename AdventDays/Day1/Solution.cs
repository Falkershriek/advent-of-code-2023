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
            string filePath = @"D:\Home\Projects\Software\AdventOfCode2023\AdventDays\Day1\Inputs\PuzzleInput.txt";
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

        private (int? Digit, int Index) FirstNumericalDigit(string line)
        { 
            char digitChar = line.FirstOrDefault(c => IsDigit(c), '?');
            int? digit = (digitChar == '?') ? null : digitChar - '0';
            int index = (digit == null) ? - 1 : line.IndexOf(digitChar);
            return (digit, index);
        }

        private (int? Digit, int Index) LastNumericalDigit(string line)
        {
            char digitChar = line.LastOrDefault(c => IsDigit(c), '?');
            int? digit = (digitChar == '?') ? null : digitChar - '0';
            int index = (digit == null) ? -1 : line.LastIndexOf(digitChar);
            return (digit, index);
        }

        private (int? Digit, int Index) FirstAlphanumericalDigit(string line)
        {
            int firstDigitIndex = line.Length - 1;
            (int? firstDigit, var firstIndex) = FirstNumericalDigit(line);

            if (firstDigit != null)
                firstDigitIndex = firstIndex;

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

            return (firstDigit, firstDigitIndex);
        }

        private (int? Digit, int Index) LastAlphanumericalDigit(string line)
        {
            int lastDigitIndex = 0;
            (int? lastDigit, var lastIndex)  = LastNumericalDigit(line);

            if (lastDigit != null)
                lastDigitIndex = lastIndex;

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

            return (lastDigit, lastDigitIndex);
        }
        #endregion

        #region Parts
        public override long Part1()
        {
            long sum = 0;

            foreach (string line in PuzzleLines)
            {
                (int? firstDigit, int firstIndex) = FirstNumericalDigit(line);
                (int? lastDigit, int lastIndex) = LastNumericalDigit(line);

                if (firstDigit == null || lastDigit == null)
                    continue;

                sum += (int)firstDigit * 10 + (int)lastDigit;
            }

            return sum;
        }

        public override long Part2()
        {
            long sum = 0;

            foreach (string line in PuzzleLines)
            {
                (int? firstDigit, int firstIndex) = FirstAlphanumericalDigit(line);
                (int? lastDigit, int lastIndex) = LastAlphanumericalDigit(line);

                if (firstDigit == null || lastDigit == null)
                    continue;

                sum += (int)firstDigit * 10 + (int)lastDigit;
            }

            return sum;
        }
        #endregion
    }
}