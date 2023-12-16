using AdventOfCode2023.AdventDays.Day2Old;
using AdventOfCode2023.AdventDays.Day4Old;
using AdventOfCode2023.AdventDays.Reusables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace AdventOfCode2023.AdventDays.Day4
{
    public class Solution : SolutionBase
    {
        #region Constructors
        public Solution()
        {
            string filePath = @"D:\Home\Projects\Software\AdventOfCode2023\AdventDays\Day4\Inputs\PuzzleInput.txt";
            PuzzleLines = File.ReadLines(filePath).ToList();
        }
        #endregion

        #region Types
        private record Card(int Id, int Score, int WonCards, List<int> GivenNumbers, List<int> WinningNumbers);
        #endregion

        #region Properties
        protected override List<string> PuzzleLines { get; }
        #endregion

        #region Methods
        private List<(Card Card, int Count)> GetUpdatedCardStack(List<(Card Card, int Count)> cardStack)
        {
            List<(Card Card, int Count)> updatedCardStack = new List<(Card Card, int Count)> ();

            List<int> cardCounts = cardStack.Select(x => x.Count).ToList();

            for (int i = 0; i < cardStack.Count; i++)
            {
                for (int j = i + 1; (j < cardStack.Count) && (j <= i + cardStack[i].Card.WonCards); j++)
                    cardCounts[j] += cardCounts[i];

                updatedCardStack.Add((cardStack[i].Card, cardCounts[i]));
            }

            return updatedCardStack;
        }

        private List<(Card Card, int Count)> GetCardStack(List<Card> cards)
            => cards.Select(x => (x, 1)).ToList();

        private List<Card> GetCards(List<string> puzzleLines)
        {
            List<Card> cards = new List<Card>();

            foreach (string puzzleLine in puzzleLines)
                cards.Add(GetCard(puzzleLine));

            return cards;
        }

        private Card GetCard(string puzzleString)
        {
            string[] puzzleData = puzzleString.Split(':');
            int id = int.Parse(puzzleData[0].Substring(5));

            string[] cardData = puzzleData[1].Replace("  ", " ")
                                             .Split('|')
                                             .Select(x => x.Trim())
                                             .ToArray();

            List<int> winningNumbers = cardData[0].Split(' ')
                                                  .Select(x => int.Parse(x.Trim()))
                                                  .ToList();

            List<int> givenNumbers = cardData[1].Split(' ')
                                                .Select(x => int.Parse(x.Trim()))
                                                .ToList();

            int score = 0;
            int wonCards = 0;
            foreach (int winningNumber in winningNumbers)
            {
                if (givenNumbers.Contains(winningNumber))
                {
                    score = (score == 0) ? 1 : score * 2;
                    wonCards++;
                }
            }

            return new Card(id, score, wonCards, givenNumbers, winningNumbers);
        }
        #endregion

        #region Parts
        public override long Part1()
        {
            List<Card> cards = GetCards(PuzzleLines);

            long sum = 0;

            foreach (Card card in cards)
                sum += card.Score;

            return sum;
        }

        public override long Part2()
        {
            List<(Card Card, int Count)> cardStack = GetCardStack(GetCards(PuzzleLines));
            cardStack = GetUpdatedCardStack(cardStack);

            long sum = 0;

            foreach (var cardPile in cardStack)
                sum += cardPile.Count;

            return sum;
        }
        #endregion
    }
}
