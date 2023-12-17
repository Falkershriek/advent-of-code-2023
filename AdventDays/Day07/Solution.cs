using AdventOfCode2023.AdventDays.Day4Old;
using AdventOfCode2023.AdventDays.Reusables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.AdventDays.Day07
{
    public class Solution : SolutionBase
    {
        #region Constructors
        public Solution()
        {
            string filePath = @".\..\..\..\AdventDays\Day07\Inputs\PuzzleInput.txt";
            PuzzleLines = File.ReadLines(filePath).ToList();
        }
        #endregion

        #region Types
        private enum HandType
        {
            HighCard,
            OnePair,
            TwoPair,
            ThreeOfAKind,
            FullHouse,
            FourOfAKind,
            FiveOfAKind
        }

        private Dictionary<HandType, int> HandRanks = new Dictionary<HandType, int>()
        {
            { HandType.HighCard,     1 },
            { HandType.OnePair,      2 },
            { HandType.TwoPair,      3 },
            { HandType.ThreeOfAKind, 4 },
            { HandType.FullHouse,    5 },
            { HandType.FourOfAKind,  6 },
            { HandType.FiveOfAKind,  7 },
        };

        private Dictionary<char, int> RankOf = new Dictionary<char, int>()
        {
            { '2', 2  },
            { '3', 3  },
            { '4', 4  },
            { '5', 5  },
            { '6', 6  },
            { '7', 7  },
            { '8', 8  },
            { '9', 9  },
            { 'T', 10 },
            { 'J', 11 },
            { 'Q', 12 },
            { 'K', 13 },
            { 'A', 14 },
        };

        private record Hand(string Cards, HandType Type, long Strength, int Bid);
        #endregion

        #region Properties
        protected override List<string> PuzzleLines { get; }
        #endregion

        #region Methods
        private List<int> RanksOf(string cards)
            => cards.Select(x => RankOf[x]).ToList();

        private List<int> RanksOfPt2(string cards)
            => cards.Select(x => (x == 'J') ? 1 : RankOf[x]).ToList();

        private List<int> CardCountsOf(string cards)
        {
            List<int> cardCounts = new int[14].Select(x => x = 0)
                                              .ToList();

            foreach (char card in cards)
            {
                int cardRank = RankOf[card];
                cardCounts[cardRank - 1]++;
            }

            return cardCounts;
        }

        private List<Hand> GetHands(List<string> puzzleLines)
            => puzzleLines.Select(x => GetHand(x)).ToList();

        private Hand GetHand(string puzzleLine)
            => new Hand
            (
                puzzleLine.Substring(0, 5),                  // Cards
                GetHandType(puzzleLine.Substring(0, 5)),     // Type
                GetHandStrength(puzzleLine.Substring(0, 5)), // Strength
                int.Parse(puzzleLine.Substring(6))           // Bid
            );

        private List<Hand> GetHandsPt2(List<string> puzzleLines)
            => puzzleLines.Select(x => GetHandPt2(x)).ToList();

        private Hand GetHandPt2(string puzzleLine)
            => new Hand
            (
                puzzleLine.Substring(0, 5),                     // Cards
                GetHandTypePt2(puzzleLine.Substring(0, 5)),     // Type
                GetHandStrengthPt2(puzzleLine.Substring(0, 5)), // Strength
                int.Parse(puzzleLine.Substring(6))              // Bid
            );

        private HandType GetHandType(string cards)
        {
            List<int> cardCounts = CardCountsOf(cards);

            if (cardCounts.Contains(5))
                return HandType.FiveOfAKind;
            if (cardCounts.Contains(4))
                return HandType.FourOfAKind;
            if (cardCounts.Contains(3))
                return (cardCounts.Contains(2)) ? HandType.FullHouse : HandType.ThreeOfAKind;
            if (cardCounts.Contains(2))
                return (cardCounts.FindAll(x => x == 2).Count == 2) ? HandType.TwoPair : HandType.OnePair;

            return HandType.HighCard;
        }

        private HandType GetHandTypePt2(string cards)
        {
            List<int> cardCounts = CardCountsOf(cards);

            if (cardCounts[10] > 0)
            {
                int jokerCount = cardCounts[10];
                cardCounts[10] = 0;

                int maxCount = cardCounts.Max();
                int highestCountIndex = cardCounts.IndexOf(maxCount);

                cardCounts[highestCountIndex] += jokerCount;
            }

            if (cardCounts.Contains(5))
                return HandType.FiveOfAKind;
            if (cardCounts.Contains(4))
                return HandType.FourOfAKind;
            if (cardCounts.Contains(3))
                return (cardCounts.Contains(2)) ? HandType.FullHouse : HandType.ThreeOfAKind;
            if (cardCounts.Contains(2))
                return (cardCounts.FindAll(x => x == 2).Count == 2) ? HandType.TwoPair : HandType.OnePair;

            return HandType.HighCard;
        }

        private long GetHandStrength(string cards)
        {
            List<int> cardRanks = RanksOf(cards);

            int multiplier = RankOf.Count + 1;

            long score = 0;
            score += cardRanks[0] * multiplier * multiplier * multiplier * multiplier;
            score += cardRanks[1] * multiplier * multiplier * multiplier;
            score += cardRanks[2] * multiplier * multiplier;
            score += cardRanks[3] * multiplier;
            score += cardRanks[4];

            return score;
        }

        private long GetHandStrengthPt2(string cards)
        {
            List<int> cardRanks = RanksOfPt2(cards);

            int multiplier = RankOf.Count + 1;

            long score = 0;
            score += cardRanks[0] * multiplier * multiplier * multiplier * multiplier;
            score += cardRanks[1] * multiplier * multiplier * multiplier;
            score += cardRanks[2] * multiplier * multiplier;
            score += cardRanks[3] * multiplier;
            score += cardRanks[4];

            return score;
        }

        private Hand GetWorstCardByStrength(List<Hand> hands)
        {
            long minStrength = hands.Min(x => x.Strength);
            return hands.Find(x => x.Strength == minStrength);
        }

        private List<Hand> GetWorstHandsByType(List<Hand> hands)
        {
            HandType worstType = hands.Min(x => x.Type);
            return hands.FindAll(x => x.Type == worstType);
        }

        private List<(Hand Hand, int Rank)> GetRankedHands(List<Hand> hands)
        {
            List<(Hand Hand, int Rank)> rankedHands = new List<(Hand Hand, int Rank)>();

            List<Hand> remainingHands = hands;

            for (int i = 1; i <= hands.Count; i++)
            {
                remainingHands = remainingHands.ExceptBy(rankedHands.Select(x => x.Hand), x => x).ToList();
                if (remainingHands.Count == 0)
                    break;

                List<Hand> worstCards = GetWorstHandsByType(remainingHands);

                rankedHands.Add((GetWorstCardByStrength(worstCards), i));
            }

            return rankedHands;
        }
        #endregion

        #region Parts
        public override long Part1()
        {
            List<Hand> hands = GetHands(PuzzleLines);

            List<(Hand Hand, int Rank)> rankedHands = GetRankedHands(hands);

            long result = 0;

            foreach (var rankedHand in rankedHands)
                result += rankedHand.Hand.Bid * rankedHand.Rank;

            return result;
        }

        public override long Part2()
        {
            List<Hand> hands = GetHandsPt2(PuzzleLines);

            List<(Hand Hand, int Rank)> rankedHands = GetRankedHands(hands);

            long result = 0;

            foreach (var rankedHand in rankedHands)
                result += rankedHand.Hand.Bid * rankedHand.Rank;

            return result;
        }
        #endregion
    }
}
