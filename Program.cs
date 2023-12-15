// See https://aka.ms/new-console-template for more information

using AdventOfCode2023.AdventDays.Day1;

namespace AdventOfCode
{
    internal class Program
    {
        static void Main()
        {
            Solution solution = new();
            string solutionString = solution.ResultPart2().ToString();
            Console.WriteLine($"{solutionString}");
        }
    }
}

