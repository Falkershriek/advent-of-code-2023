using AdventOfCode2023.AdventDays.Reusables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdventOfCode2023.AdventDays.Day8
{
    public class Solution : SolutionBase
    {
        #region Fields
        private Dictionary<string, (string Left, string Right)> Node;
        #endregion

        #region Constructors
        public Solution()
        {
            string filePath = @".\..\..\..\AdventDays\Day8\Inputs\PuzzleInput.txt";
            PuzzleLines = File.ReadLines(filePath).ToList();
            Node = new Dictionary<string, (string Left, string Right)>();

            foreach (string nodeLine in PuzzleLines.Skip(2))
                Node.Add(nodeLine.Substring(0, 3), (nodeLine.Substring(7, 3), nodeLine.Substring(12, 3)));
        }
        #endregion

        #region Types
        #endregion

        #region Properties
        protected override List<string> PuzzleLines { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Greatest Common Denominator/Divisor
        /// </summary>
        private static long GCD(long a, long b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a | b;
        }

        /// <summary>
        /// Least Common Multiple/Divisor
        /// </summary>
        private static long LCM(long a, long b)
            => (a * b) / GCD(a, b);

        private List<char> GetInstructions(List<string> puzzleLines)
            => puzzleLines[0].ToList();
        #endregion

        #region Parts
        public override long Part1()
        {
            List<char> instructions = GetInstructions(PuzzleLines);

            string currentNode = "AAA";

            long steps = 0;

            for (int i = 0; ; i = (i + 1) % instructions.Count)
            {
                currentNode = (instructions[i] == 'L') ? Node[currentNode].Left : Node[currentNode].Right;
                steps++;

                if (currentNode == "ZZZ")
                    break;
            }

            long result = steps;

            return result;
        }

        public override long Part2()
        {
            List<char> instructions = GetInstructions(PuzzleLines);
            List<string> entryNodes = Node.ToList()
                                          .FindAll(x => x.Key.EndsWith('A'))
                                          .Select(x => x.Key)
                                          .ToList();

            List<long> loopSteps = new List<long>();

            foreach (string entryNode in entryNodes)
            {
                string currentNode = entryNode;
                long currentSteps = 0;

                for (int i = 0; ; i = (i + 1) % instructions.Count)
                {
                    currentNode = (instructions[i] == 'L') ? Node[currentNode].Left : Node[currentNode].Right;
                    currentSteps++;

                    if (currentNode[2] == 'Z')
                        break;
                }

                loopSteps.Add(currentSteps);
            }

            long lcm = loopSteps.Aggregate((a, b) => LCM(a, b));

            long result = lcm;

            return result;
        }
        #endregion
    }
}
