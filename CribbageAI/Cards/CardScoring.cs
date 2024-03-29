﻿using Cards;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CribbageAI.Cards
{
    static class CardScoring
    {
        public static int ScoreCountingCardsPlayed(List<Card> playedCards, Card card, int currentCount)
        {
            if (card.Value + currentCount > 31)
            {
                return -1;
            }

            int score = 0;

            currentCount += card.Value;
            // 
            //  hit 15?
            if (currentCount == 15)
            {
                score += 2;
            }

            // hit 31?
            if (currentCount == 31)
            {
                score += 2;
            }

            List<Card> allCards = new List<Card>(playedCards);
            allCards.Add(card);


            //
            //   search for 2, 3, or 4 of a kind -- this has to happen before the sort!
            int samenessCount = 0;
            for (int i = allCards.Count - 1; i > 0; i--)
            {
                if (allCards[i].Rank == allCards[i - 1].Rank)
                    samenessCount++;
                else break;
            }

            switch (samenessCount)
            {
                case 1: // pair
                    score += 2;
                    break;
                case 2: // 3 of a kind
                    score += 6;
                    break;
                case 3: // 4 of a kind
                    score += 12;
                    break;
                default:
                    break;
            }

            // search for runs

            score += ScoreCountedRun(allCards);

            return score;

        }

        //
        //  just call ScoreRuns to get ths list..
        private static int ScoreRuns(List<Card> list)
        {
            List<List<Card>> cardLists = DemuxPairs(list);
            List<List<Card>> runs = new List<List<Card>>();

            foreach (List<Card> cards in cardLists)
            {
                List<Card> l = GetRuns(cards);
                if (l != null)
                {
                    runs.Add(l);
                }
            }
            //
            //  eliminate duplicate lists - this happens if you have a hand that looks like 5, 5, 7, 8, 9 where the pair is not in the run
            if (runs.Count == 2)
            {
                if (runs[0].Count == runs[1].Count) // same length
                {
                    bool same = false;
                    for (int i = 0; i < runs[0].Count; i++)
                    {
                        if (runs[0][i] != runs[1][i])
                        {
                            same = false;
                            break;
                        }

                        same = true;

                    }

                    if (same)
                    {
                        runs.RemoveAt(1);
                    }
                }
            }


            //
            //  runs now how the list of cards that have runs in them
            int score = 0;
            {

                foreach (List<Card> cards in runs)
                {
                    if (cards.Count > 2)
                    {
                        score += cards.Count;
                    }
                }
            }

            return score;

        }

        public static bool Is3CardRun(Card card1, Card card2, Card card3)
        {
            if (card1.Rank == card2.Rank - 1 &&
                card2.Rank == card3.Rank - 1)
            {
                return true;
            }

            return false;

        }
        private static List<List<Card>> DemuxPairs(List<Card> list)
        {
            List<List<Card>> cardList = new List<List<Card>>();

            Card previousCard = null;
            int consecutive = 0;
            int pairs = 0;
            foreach (Card thisCard in list)
            {
                if (previousCard == null)
                {
                    cardList.Add(new List<Card>());
                    cardList[0].Add(thisCard);
                }
                else if (previousCard.Rank != thisCard.Rank)
                {
                    consecutive = 0;
                    foreach (List<Card> cards in cardList)
                    {
                        cards.Add(thisCard);
                    }
                }
                else if (previousCard.Rank == thisCard.Rank) // pair
                {
                    consecutive++;
                    pairs++;

                    if ((consecutive == 1 && pairs == 1) || (consecutive == 2 && pairs == 2))
                    {
                        int count = cardList.Count;
                        List<Card> newList = new List<Card>(cardList[count - 1]);
                        cardList.Add(newList);
                        newList.Remove(previousCard);
                        newList.Add(thisCard);
                    }
                    else if (consecutive == 1 && pairs == 2)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            List<Card> newList = new List<Card>(cardList[k]);
                            newList.Remove(previousCard);
                            newList.Add(thisCard);
                            cardList.Add(newList);
                        }

                    }

                }

                previousCard = thisCard;

            }

            return cardList;

        }
        //
        //   3, four of 5 cards can be passed in 
        private static List<Card> GetRuns(List<Card> list)
        {
            int count = list.Count;
            if (count < 3)
                return null;


            if (Is3CardRun(list[0], list[1], list[2]))
            {
                if (count > 3 && list[2].Rank == list[3].Rank - 1)
                {
                    if (count > 4 && list[3].Rank == list[4].Rank - 1)
                    {

                        return new List<Card>(list); // 5 card run
                    }
                    else
                    {
                        if (count > 4)
                            list.RemoveAt(4);
                        return new List<Card>(list); // 4 card run
                    }

                }
                else
                {

                    if (count > 4)
                        list.RemoveAt(4);
                    if (count > 3)
                        list.RemoveAt(3);

                    return new List<Card>(list); // 3 card run
                }

            }
            else if (count > 3 && Is3CardRun(list[1], list[2], list[3]))
            {
                if (count > 4 && list[3].Rank == list[4].Rank - 1)
                {
                    list.RemoveAt(0);
                    return new List<Card>(list); // 4 card run
                }
                else
                {
                    if (count > 4)
                        list.RemoveAt(4);

                    if (count > 3)
                        list.RemoveAt(0);

                    return new List<Card>(list); // 3 card run
                }
            }
            else if (count > 4 && Is3CardRun(list[2], list[3], list[4]))
            {
                list.RemoveAt(1);
                list.RemoveAt(0);
                return new List<Card>(list); // 3 card run
            }

            return null;
        }




        // assumes a sorted list
        public static int ScorePairs(List<Card> list)
        {
            int score = 0;
            int pairs = 0;
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (list[i].Rank == list[j].Rank) // pair
                    {
                        score += 2;
                        pairs++;
                    }
                }
            }

            return score;
        }
        public static int ScoreFifteens(List<Card> list)
        {
            int score = 0;
            int iVal = 0;
            int ijVal = 0;
            int ijkVal = 0;
            int ijkxVal = 0;
            for (int i = 0; i < list.Count; i++)
            {
                iVal = list[i].Value;
                for (int j = i + 1; j < list.Count; j++)
                {
                    ijVal = list[j].Value + iVal;
                    if (ijVal > 15)
                        break; //because we are ordered;
                    if (ijVal == 15)
                    {
                        score += 2;

                    }
                    else
                    {
                        //
                        // here because ijVal < 15
                        for (int k = j + 1; k < list.Count; k++)
                        {
                            ijkVal = list[k].Value + ijVal;
                            if (ijkVal > 15)
                                break;

                            if (ijkVal == 15)
                            {
                                score += 2;
                                // don't break here -- it might be a pair like 5555

                            }
                            else
                            {
                                //
                                // here because ijkVal < 15
                                for (int x = k + 1; x < list.Count; x++)
                                {

                                    ijkxVal = list[x].Value + ijkVal;
                                    if (ijkxVal > 15)
                                        break;
                                    if (ijkxVal == 15)
                                    {
                                        score += 2;
                                    }

                                    if (list.Count == 5) // if the shared card is passed in...
                                    {
                                        int sumAll = ijkVal + list[3].Value + list[4].Value;
                                        if (sumAll == 15) // takes all 5...
                                        {
                                            score += 2;
                                            return score;
                                        }

                                        if (sumAll < 15) // not enough points to get to 15 with all 5 cards
                                        {
                                            return 0;
                                        }


                                    }
                                }
                            }
                        }
                    }
                }
            }

            return score;

        }

        //
        //  call this to see if a run is in the cards based on the rules of counting
        public static int ScoreCountedRun(List<Card> playedCards)
        {
            int n = 3;
            int count = playedCards.Count;
            int score = 0;

            if (count > 2) // don't bother if there aren't enough cards
            {
                List<Card> cards = new List<Card>();
                int longestRun = 0;
                int i = 0;
                do
                {
                    cards.Clear();
                    //
                    //  add the last n cards ... starting with 3
                    for (i = 0; i < n; i++)
                    {
                        cards.Add(playedCards[count - i - 1]);
                    }
                    //
                    //  sort them
                    cards.Sort(Card.CompareCardsByRank);

                    //
                    //  check to see if they are in order
                    for (i = 0; i < n - 1; i++)
                    {
                        if (cards[i].Rank != cards[i + 1].Rank - 1)
                        {
                            //
                            //  if not in order, break
                            break;
                        }
                    }
                    if (i >= 2)
                    {
                        //
                        //  we found a run of at least 3
                        longestRun = i;
                    }
                    //
                    //  if we have enough cards, look for the next longest run
                    n++;
                }
                while (n < playedCards.Count);

                if (i > 1)
                {
                    // found a run!!
                    score += i + 1;
                    //Debug.WriteLine($"Found a run: {score} cards: {Card.CardsToString(playedCards)}");
                }

            }
            return score;
        }

        public static int ScoreHand(List<Card> hand, Card sharedCard, HandType handType)
        {
            int score = 0;

            score += ScoreNibs(hand, sharedCard);                    // this is the only one where it matters which particular card is shared

            //
            //   DON't SORT BEFORE NIBS!!!
            List<Card> cards = new List<Card>(hand);
            if (sharedCard != null) // sharedCard null when calculating value of hand prior to seeing the shared card
                cards.Add(sharedCard);

            cards.Sort(Card.CompareCardsByRank);

            score += ScoreFifteens(cards);
            score += ScorePairs(cards);
            score += ScoreRuns(cards);
            score += ScoreFlush(cards, handType);


            return score;

        }

        private static int ScoreFlush(List<Card> cards, HandType handType)
        {
            cards.Sort(Card.CompareCardsBySuit);
            int max = 0;
            int run = 1;
            for (int i = 0; i < cards.Count - 1; i++)
            {
                if (cards[i].Suit == cards[i + 1].Suit)
                {
                    run++;
                }
                else
                {
                    if (run > max) max = run;
                    run = 1;
                }
            }

            if (handType == HandType.Crib && run == 5)
                return run;

            if (handType == HandType.Regular && run > 3)
                return run;

            return 0;
        }

        private static int ScoreNibs(List<Card> hand, Card sharedCard)
        {
            if (sharedCard == null)
                return 0;

            for (int i = 0; i < 4; i++)
            {
                if (hand[i].Rank == 11) //Jack -- 1 indexed
                {
                    if (hand[i].Suit == sharedCard.Suit)
                    {
                        return 1;
                    }
                }

            }
            return 0;

        }



    }

    public static class CribbageStats
    {
        public static int[,] TwoCard = new int[,] { { 0, 1 }, { 0, 2 }, { 0, 3 }, { 0, 4 }, { 1, 2 }, { 1, 3 }, { 1, 4 }, { 2, 3 }, { 3, 4 }, { 3, 4 } };
        public static int[,,] ThreeCard = new int[,,] { { { 0, 1, 2 }, { 0, 1, 3 }, { 0, 1, 4 }, { 0, 2, 3 }, { 0, 2, 4 }, { 0, 3, 4 }, { 1, 2, 3 }, { 1, 2, 4 }, { 1, 3, 4 }, { 2, 3, 4 } } };
        public static int[,,,] FourCard = new int[,,,] { { { { 0, 1, 2, 3 }, { 0, 1, 2, 4 }, { 1, 2, 3, 4 } } } };
        public static int[] FiveCard = new int[] { 0, 1, 2, 3, 4 };
        public static int[] TwoCardList = new int[] { 0, 1, 0, 2, 0, 3, 0, 4, 1, 2, 1, 3, 1, 4, 2, 3, 3, 4 };

        public static double[,] dropTable = new double[,]{
               {5.26, 4.18, 4.47, 5.45, 5.48, 3.80, 3.73, 3.70, 3.33, 3.37, 3.65, 3.39, 3.42},
               {4.18, 5.67, 6.97, 4.51, 5.44, 3.87, 3.81, 3.58, 3.63, 3.51, 3.79, 3.52, 3.55} ,
               {4.47, 6.97, 5.90, 4.88, 6.01, 3.72, 3.67, 3.84, 3.66, 3.61, 3.88, 3.62, 3.66} ,
               {5.45, 4.51, 4.88, 5.65, 6.54, 3.87, 3.74, 3.84, 3.69, 3.62, 3.89, 3.63, 3.67} ,
               {5.48, 5.44, 6.01, 6.54, 8.95, 6.65, 6.04, 5.49, 5.47, 6.68, 7.04, 6.71, 6.70} ,
               {3.80, 3.87, 3.72, 3.87, 6.65, 5.74, 4.94, 4.70, 5.11, 3.15, 3.40, 3.08, 3.13} ,
               {3.73, 3.81, 3.67, 3.74, 6.04, 4.94, 5.98, 6.58, 4.06, 3.10, 3.43, 3.17, 3.21} ,
               {3.70, 3.58, 3.84, 3.84, 5.49, 4.70, 6.58, 5.42, 4.74, 3.86, 3.39, 3.16, 3.20} ,
               {3.33, 3.63, 3.66, 3.69, 5.47, 5.11, 4.06, 4.74, 5.09, 4.27, 3.98, 2.97, 3.05} ,
               {3.37, 3.51, 3.61, 3.62, 6.68, 3.15, 3.10, 3.86, 4.27, 4.73, 4.64, 3.36, 2.86} ,
               {3.65, 3.79, 3.88, 3.89, 7.04, 3.40, 3.43, 3.39, 3.98, 4.64, 5.37, 4.90, 4.07} ,
               {3.39, 3.52, 3.62, 3.63, 6.71, 3.08, 3.17, 3.16, 2.97, 3.36, 4.90, 4.66, 3.50} ,
               {3.42, 3.55, 3.66, 3.67, 6.70, 3.13, 3.21, 3.20, 3.05, 2.86, 4.07, 3.50, 4.62} };


        public static double[,] dropTableToMyCrib = new double[,]{
                {5.38,  4.23,   4.52,   5.43,   5.45,   3.85,   3.85,   3.80,   3.40,   3.42,   3.65,   3.42,   3.41},
                {4.23,  5.72,   7.00,   4.52,   5.45,   3.93,   3.81,   3.66,   3.71,   3.55,   3.84,   3.58,   3.52},
                {4.52,  7.00,   5.94,   4.91,   5.97,   3.81,   3.58,   3.92,   3.78,   3.57,   3.90,   3.59,   3.67},
                {5.43,  4.52,   4.91,   5.63,   6.48,   3.85,   3.72,   3.83,   3.72,   3.59,   3.88,   3.59,   3.60},
                {5.45,  5.45,   5.97,   6.48,   8.79,   6.63,   6.01,   5.48,   5.43,   6.66,   7.00,   6.63,   6.66},
                {3.85,  3.93,   3.81,   3.85,   6.63,   5.76,   4.98,   4.63,   5.13,   3.17,   3.41,   3.23,   3.13},
                {3.85,  3.81,   3.58,   3.72,   6.01,   4.98,   5.92,   6.53,   4.04,   3.23,   3.53,   3.23,   3.26},
                {3.80,  3.66,   3.92,   3.83,   5.48,   4.63,   6.53,   5.45,   4.72,   3.80,   3.52,   3.19,   3.16},
                {3.40,  3.71,   3.78,   3.72,   5.43,   5.13,   4.04,   4.72,   5.16,   4.29,   3.97,   2.99,   3.06},
                {3.42,  3.55,   3.57,   3.59,   6.66,   3.17,   3.23,   3.80,   4.29,   4.76,   4.61,   3.31,   2.84},
                {3.65,  3.84,   3.90,   3.88,   7.00,   3.41,   3.53,   3.52,   3.97,   4.61,   5.33,   4.81,   3.96},
                {3.42,  3.58,   3.59,   3.59,   6.63,   3.23,   3.23,   3.19,   2.99,   3.31,   4.81,   4.79,   3.46},
                {3.41,  3.52,   3.67,   3.60,   6.66,   3.13,   3.26,   3.16,   3.06,   2.84,   3.96,   3.46,   4.58}};

        public static double[,] dropTableToYouCrib = new double[,]{
                {6.02,  5.07,   5.07,   5.72,   6.01,   4.91,   4.89,   4.85,   4.55,   4.48,   4.68,   4.33,   4.30},
                {5.07,  6.38,   7.33,   5.33,   6.11,   4.97,   4.97,   4.94,   4.70,   4.59,   4.81,   4.56,   4.45},
                {5.07,  7.33,   6.68,   5.96,   6.78,   4.87,   5.01,   5.05,   4.87,   4.63,   4.86,   4.59,   4.48},
                {5.72,  5.33,   5.96,   6.53,   7.26,   5.34,   4.88,   4.94,   4.68,   4.53,   4.85,   4.46,   4.36},
                {6.01,  6.11,   6.78,   7.26,   9.37,   7.47,   7.00,   6.30,   6.15,   7.41,   7.76,   7.34,   7.25},
                {4.91,  4.97,   4.87,   5.34,   7.47,   7.08,   6.42,   5.86,   6.26,   4.31,   4.57,   4.22,   4.14},
                {4.89,  4.97,   5.01,   4.88,   7.00,   6.42,   7.14,   7.63,   5.26,   4.31,   4.68,   4.32,   4.27},
                {4.85,  4.94,   5.05,   4.94,   6.30,   5.86,   7.63,   6.82,   5.83,   5.10,   4.59,   4.31,   4.20},
                {4.55,  4.70,   4.87,   4.68,   6.15,   6.26,   5.26,   5.83,   6.39,   5.43,   4.96,   4.11,   4.03},
                {4.48,  4.59,   4.63,   4.53,   7.41,   4.31,   4.31,   5.10,   5.43,   6.08,   5.63,   4.61,   3.88},
                {4.68,  4.81,   4.86,   4.85,   7.76,   4.57,   4.68,   4.59,   4.96,   5.63,   6.42,   5.46,   4.77},
                {4.33,  4.56,   4.59,   4.46,   7.34,   4.22,   4.32,   4.31,   4.11,   4.61,   5.46,   5.79,   4.49},
                {4.30,  4.45,   4.48,   4.36,   7.25,   4.14,   4.27,   4.20,   4.03,   3.88,   4.77,   4.49,   5.65}};



        //                                 A      2      3    4      5     6     7    8     9     10    J    Q     K
        //double[] ace = new double[]     {5.26, 4.18, 4.47, 5.45, 5.48, 3.80, 3.73, 3.70, 3.33, 3.37, 3.65, 3.39, 3.42};
        //double[] two = new double[]     {4.18, 5.67, 6.97, 4.51, 5.44, 3.87, 3.81, 3.58, 3.63, 3.51, 3.79, 3.52, 3.55}; 
        //double[] three = new double[]   {4.47, 6.97, 5.90, 4.88, 6.01, 3.72, 3.67, 3.84, 3.66, 3.61, 3.88, 3.62, 3.66}; 
        //double[] four = new double[]    {5.45, 4.51, 4.88, 5.65, 6.54, 3.87, 3.74, 3.84, 3.69, 3.62, 3.89, 3.63, 3.67};
        //double[] five = new double[]    {5.48, 5.44, 6.01, 6.54, 8.95, 6.65, 6.04, 5.49, 5.47, 6.68, 7.04, 6.71, 6.70};
        //double[] six = new double[]     {3.80, 3.87, 3.72, 3.87, 6.65, 5.74, 4.94, 4.70, 5.11, 3.15, 3.40, 3.08, 3.13};
        //double[] seven = new double[]   {3.73, 3.81, 3.67, 3.74, 6.04, 4.94, 5.98, 6.58, 4.06, 3.10, 3.43, 3.17, 3.21};
        //double[] eight = new double[]   {3.70, 3.58, 3.84, 3.84, 5.49, 4.70, 6.58, 5.42, 4.74, 3.86, 3.39, 3.16, 3.20};
        //double[] nine = new double[]    {3.33, 3.63, 3.66, 3.69, 5.47, 5.11, 4.06, 4.74, 5.09, 4.27, 3.98, 2.97, 3.05};
        //double[] ten = new double[]     {3.37, 3.51, 3.61, 3.62, 6.68, 3.15, 3.10, 3.86, 4.27, 4.73, 4.64, 3.36, 2.86};
        //double[] jack = new double[]    {3.65, 3.79, 3.88, 3.89, 7.04, 3.40, 3.43, 3.39, 3.98, 4.64, 5.37, 4.90, 4.07};
        //double[] queen = new double[]   {3.39, 3.52, 3.62, 3.63, 6.71, 3.08, 3.17, 3.16, 2.97, 3.36, 4.90, 4.66, 3.50}; 
        //double[] king = new double[]    {3.42, 3.55, 3.66, 3.67, 6.70, 3.13, 3.21, 3.20, 3.05, 2.86, 4.07, 3.50, 4.62};



    }
}
