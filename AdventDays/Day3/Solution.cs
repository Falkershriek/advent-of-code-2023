using AdventOfCode2023.AdventDays.Day2Old;
using AdventOfCode2023.AdventDays.Reusables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.AdventDays.Day3
{
    public class Solution : SolutionBase
    {
        #region Constructors
        public Solution()
        {
            string filePath = @"D:\Home\Projects\Software\AdventOfCode2023\AdventDays\Day3\Inputs\PuzzleInput.txt";
            PuzzleLines = File.ReadLines(filePath).ToList();
        }
        #endregion

        #region Enums, Dictionaries and Records
        private record struct PartNumber(int PosX, int PosY, int Value);
        private record struct AsteriskPart(int PosX, int PosY);
        #endregion

        #region Properties
        protected override List<string> PuzzleLines { get; }
        #endregion

        #region Methods
        private char[,] GetPuzzlePlane(List<string> puzzleLines)
        {
            char[,] puzzlePlane = new char[puzzleLines.Count(), puzzleLines[0].Count()];

            for (int y = 0; y < puzzlePlane.GetLength(1); y++)
            {
                for (int x = 0; x < puzzlePlane.GetLength(0); x++)
                {
                    puzzlePlane[x, y] = puzzleLines[y][x]; // swap x and y?
                }
            }

            return puzzlePlane;
        }

        private List<PartNumber> GetPartNumbers(char[,] puzzlePlane)
        {
            List<PartNumber> partNumbers = new List<PartNumber>();

            for (int y = 0; y < puzzlePlane.GetLength(1); y++)
            {
                for (int x = 0; x < puzzlePlane.GetLength(0); x++)
                {
                    char c = puzzlePlane[x, y];
                    if (c >= '0' && c <= '9')
                    {
                        int value = 0;

                        while (c >= '0' && c <= '9')
                        {
                            value = value * 10 + c - '0';
                            x++;
                            if (x >= puzzlePlane.GetLength(0))
                                break;
                            c = puzzlePlane[x, y];
                        }

                        partNumbers.Add(new PartNumber(x - value.ToString().Length, y, value));
                    }
                }
            }

            return partNumbers;
        }

        private List<AsteriskPart> GetAsteriskParts(char[,] puzzlePlane)
        {
            List<AsteriskPart> asteriskParts = new List<AsteriskPart>();

            for (int y = 0; y < puzzlePlane.GetLength(1); y++)
                for (int x = 0; x < puzzlePlane.GetLength(0); x++)
                    if (puzzlePlane[x, y] == '*')
                        asteriskParts.Add(new AsteriskPart(x, y));

            return asteriskParts;
        }

        private int GetGearRatio(AsteriskPart part, List<PartNumber> partNumbers)
        {
            List<PartNumber> adjacentNumbers = partNumbers.FindAll(x => AreAdjacent(part, x));

            if (adjacentNumbers.Count < 2)
                return -1;

            return adjacentNumbers[0].Value * adjacentNumbers[1].Value;
        }

        private bool AreAdjacent(AsteriskPart part, PartNumber partNumber)
        {
            int minX = partNumber.PosX - 1;
            int maxX = partNumber.PosX + partNumber.Value.ToString().Length;

            int minY = partNumber.PosY - 1;
            int maxY = partNumber.PosY + 1;

            return part.PosX >= minX && part.PosX <= maxX && part.PosY >= minY && part.PosY <= maxY;
        }

        private bool IsSymbol(char c)
            => c != '.' && (c < '0' || c > '9');

        private bool IsAdjacentToSymbol(PartNumber partNumber, char[,] puzzlePlane)
        {
            int length = partNumber.Value.ToString().Length;

            int minX = (partNumber.PosX > 0) ? partNumber.PosX - 1 : 0;
            int maxX = (partNumber.PosX + length < puzzlePlane.GetLength(0)) ? partNumber.PosX + length : puzzlePlane.GetLength(0) - 1;

            int minY = (partNumber.PosY > 0) ? partNumber.PosY - 1 : 0;
            int maxY = (partNumber.PosY + length < puzzlePlane.GetLength(1)) ? partNumber.PosY + 1 : puzzlePlane.GetLength(1) - 1;

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                    if (IsSymbol(puzzlePlane[x, y]))
                        return true;
            }
            return false;
        }
        #endregion

        #region Parts
        public override long Part1()
        {
            char[,] puzzlePlane = GetPuzzlePlane(PuzzleLines);
            List<PartNumber> partNumbers = GetPartNumbers(puzzlePlane);

            long sum = 0;

            foreach (PartNumber partNumber in partNumbers)
                if (IsAdjacentToSymbol(partNumber, puzzlePlane))
                    sum += partNumber.Value;

            return sum;
        }

        public override long Part2()
        {
            char[,] puzzlePlane = GetPuzzlePlane(PuzzleLines);
            List<PartNumber> adjacentPartNumbers = GetPartNumbers(puzzlePlane).FindAll(x => IsAdjacentToSymbol(x, puzzlePlane));
            List<AsteriskPart> AsteriskParts = GetAsteriskParts(puzzlePlane);

            long sum = 0;

            foreach (AsteriskPart asteriskPart in AsteriskParts)
            {
                int gearRatio = GetGearRatio(asteriskPart, adjacentPartNumbers);
                if (gearRatio > 0)
                    sum += gearRatio;
            }

            return sum;
        }
        #endregion
    }
}
