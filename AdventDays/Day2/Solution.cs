using AdventOfCode2023.AdventDays.Reusables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.AdventDays.Day2
{
    public class Solution : SolutionBase
    {
        #region Constructors
        public Solution()
        {
            string filePath = @"D:\Home\Projects\Software\AdventOfCode2023\AdventDays\Day2\Inputs\PuzzleInput.txt";
            PuzzleLines = File.ReadLines(filePath).ToList();
        }
        #endregion

        #region Enums, Dictionaries and Records
        private record Game(int Id, int MaxRedCubes, int MaxGreenCubes, int MaxBlueCubes);
        #endregion

        #region Properties
        protected override List<string> PuzzleLines { get; }
        #endregion

        #region Methods
        private List<Game> GetGames(List<string> puzzleLines)
        {
            List<Game> games = new List<Game>();
            char[] cubeSeparators = { ',', ';' };

            foreach (string puzzleLine in puzzleLines)
            {
                string[] gameData = puzzleLine.Split(':');

                int GameId = int.Parse(gameData[0].Substring(5));

                List<string> cubes = gameData[1].Split(cubeSeparators).Select(x => x.Trim()).ToList();
                cubes.RemoveAll(x => string.IsNullOrWhiteSpace(x));

                (int MaxRedCubes, int MaxGreenCubes, int MaxBlueCubes) = GetMaxCubes(cubes);

                games.Add(new Game(GameId, MaxRedCubes, MaxGreenCubes, MaxBlueCubes));
            }

            return games;
        }

        private (int MaxRedCubes, int MaxGreenCubes, int MaxBlueCubes) GetMaxCubes(List<string> cubes)
        {
            int MaxRedCubes = 0;
            int MaxGreenCubes = 0;
            int MaxBlueCubes = 0;

            foreach (string cube in cubes)
            {
                string[] cubeStrings = cube.Split(' ');
                int cubesCount = int.Parse(cubeStrings[0]);
                string cubesColor = cubeStrings[1];

                switch (cubesColor)
                {
                    case "red":
                        MaxRedCubes = (cubesCount > MaxRedCubes) ? cubesCount : MaxRedCubes;
                        break;
                    case "green":
                        MaxGreenCubes = (cubesCount > MaxGreenCubes) ? cubesCount : MaxGreenCubes;
                        break;
                    case "blue":
                        MaxBlueCubes = (cubesCount > MaxBlueCubes) ? cubesCount : MaxBlueCubes;
                        break;
                    default:
                        break;
                }
            }

            return (MaxRedCubes, MaxGreenCubes, MaxBlueCubes);
        }

        private bool GameIsPossible(Game game, int RedCubes, int GreenCubes, int BlueCubes)
            => RedCubes >= game.MaxRedCubes && GreenCubes >= game.MaxGreenCubes && BlueCubes >= game.MaxBlueCubes;

        private int PowerOf(Game game)
            => game.MaxRedCubes * game.MaxGreenCubes * game.MaxBlueCubes;
        #endregion

        #region Parts
        public override long Part1()
        {
            List<Game> games = GetGames(PuzzleLines);

            long sum = 0;

            int RedCubes = 12;
            int GreenCubes = 13;
            int BlueCubes = 14;

            foreach (Game game in games)
            {
                if (GameIsPossible(game, RedCubes, GreenCubes, BlueCubes))
                    sum += game.Id;
            }

            return sum;
        }

        public override long Part2()
        {
            List<Game> games = GetGames(PuzzleLines);

            long sum = 0;

            foreach (Game game in games)
                sum += PowerOf(game);

            return sum;
        }
        #endregion
    }
}
