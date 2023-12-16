using AdventOfCode2023.AdventDays.Reusables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.AdventDays.Day9
{
    public class Solution : SolutionBase
    {
        #region Constructors
        public Solution()
        {
            string filePath = @".\..\..\..\AdventDays\Day9\Inputs\PuzzleInput.txt";
            PuzzleLines = File.ReadLines(filePath).ToList();
        }
        #endregion

        #region Types
        #endregion

        #region Properties
        protected override List<string> PuzzleLines { get; }
        #endregion

        #region Methods
        private List<List<long>> GetValuesLists(List<string> puzzleLines)
            => puzzleLines.Select(x => x.Split(' ').Select(x => long.Parse(x))
                          .ToList())
                          .ToList();

        private List<List<long>> GetPredictionsLists(List<long> values)
        {
            List<List<long>> predictionsLists = [values];

            List<long> currentList = values;
            do
            {
                currentList = currentList.Zip(currentList.Skip(1), (a, b) => b - a).ToList();
                predictionsLists.Add(currentList);

            } while (currentList.Count != currentList.FindAll(x => x == currentList[0]).Count);

            return predictionsLists;
        }

        private long GetNextValue(List<long> values)
        {
            List<List<long>> listsOfValues = GetPredictionsLists(values);

            long nextValue = listsOfValues.Sum(x => x.Last());

            return nextValue;
        }

        private long GetPreviousValue(List<long> values)
        {
            List<List<long>> listsOfValues = GetPredictionsLists(values);
            listsOfValues.Reverse();

            long previousValue = listsOfValues.Select(x => x.First())
                                              .Aggregate((a, b) => b - a);
            return previousValue;
        }
        #endregion

        #region Parts
        public override long Part1()
        {
            List<List<long>> valuesLists = GetValuesLists(PuzzleLines);

            long result = 0;

            foreach (var valuesList in valuesLists)
                result += GetNextValue(valuesList);

            return result;
        }

        public override long Part2()
        {
            List<List<long>> valuesLists = GetValuesLists(PuzzleLines);

            long result = 0;

            foreach (var valuesList in valuesLists)
                result += GetPreviousValue(valuesList);

            return result;
        }
        #endregion
    }
}
