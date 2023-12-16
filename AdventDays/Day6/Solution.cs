using AdventOfCode2023.AdventDays.Day4Old;
using AdventOfCode2023.AdventDays.Reusables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.AdventDays.Day6
{
    public class Solution : SolutionBase
    {
        #region Constructors
        public Solution()
        {
            string filePath = @".\..\..\..\AdventDays\Day6\Inputs\PuzzleInput.txt";
            PuzzleLines = File.ReadLines(filePath).ToList();
        }
        #endregion

        #region Types
        #endregion

        #region Properties
        protected override List<string> PuzzleLines { get; }
        #endregion

        #region Methods
        public List<int> GetTimes (List<string> puzzleLines)
            => PuzzleLines[0].Split(' ')
                             .Where(x => int.TryParse(x, out _))
                             .Select(x => int.Parse(x))
                             .ToList();

        public List<int> GetDistances (List<string> puzzleLines)
            => PuzzleLines[1].Split(' ')
                             .Where(x => int.TryParse(x, out _))
                             .Select(x => int.Parse(x))
                             .ToList();

        public long GetTotalTime(List<string> puzzleLines)
            => long.Parse(puzzleLines[0].Split(' ')
                                        .Where(x => int.TryParse(x, out _))
                                        .Aggregate((i, j) => i + j));

        public long GetTotalDistance(List<string> puzzleLines)
            => long.Parse(puzzleLines[1].Split(' ')
                                        .Where(x => int.TryParse(x, out _))
                                        .Aggregate((i, j) => i + j));

        public List<long> GetNumbersOfWaysToWin(List<int> times, List<int> distances)
        {
            List<long> numbersOfWaysToWin = new List<long>();

            for (int i = 0; i < times.Count; i++)
                numbersOfWaysToWin.Add(NumberOfWaysToWin(times[i], distances[i]));

            return numbersOfWaysToWin;
        }

        public long NumberOfWaysToWin(long time, long distance)
        {
            long numberOfWaysToWin = 0;
            long totalTime = time;
            long remainingTime = time;
            long distanceToBeat = distance;
            long chargedDistance = 0;

            for (long speed = 0; speed <= totalTime; speed++)
            {
                remainingTime = totalTime - speed;
                chargedDistance = remainingTime * speed;

                if (chargedDistance > distanceToBeat)
                    numberOfWaysToWin++;
            }

            return numberOfWaysToWin;
        }
        #endregion

        #region Parts
        public override long Part1()
        {
            List<int> times = GetTimes(PuzzleLines);
            List<int> distances = GetDistances(PuzzleLines);

            List<long> NumbersOfTimesToWin = GetNumbersOfWaysToWin(times, distances);

            long result = 1;

            foreach (long number in NumbersOfTimesToWin)
                result *= number;

            return result;
        }

        public override long Part2()
        {
            long totalTime = GetTotalTime(PuzzleLines);
            long totalDistance = GetTotalDistance(PuzzleLines);

            long result = NumberOfWaysToWin(totalTime, totalDistance);

            return result;
        }
        #endregion
    }
}
