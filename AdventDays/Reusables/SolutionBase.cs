using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.AdventDays.Reusables
{
    public abstract class SolutionBase
    {
        #region Fields
        #endregion

        #region Constructors
        public SolutionBase()
        {
            //string filePath = @".\..\..\..\AdventDays\DayX\Inputs\PuzzleInput.txt";
            //PuzzleLines = File.ReadLines(filePath).ToList();
        }
        #endregion

        #region Enums, Dictionaries and Types

        #endregion

        #region Properties
        protected abstract List<string> PuzzleLines { get; }
        #endregion

        #region Methods

        #endregion

        #region Parts
        public abstract long Part1();

        public abstract long Part2();
        #endregion

        #region Results
        public long ResultPart1() => Part1();

        public long ResultPart2() => Part2();
        #endregion
    }
}
