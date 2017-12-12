using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cards;
using Facet.Combinatorics;
using CribbageAI.Cards;

namespace CribbageAI
{

    public enum PlayerAlgorithm { Easy, Hard, Random, ImprovedCounting, None };
    public enum PlayerName { PlayerOne, PlayerTwo, Uninitialized, None };

    public interface IPlayer
    {
        List<Card> SelectCribCards(List<Card> hand, bool yourCrib);
        Card GetCountCard(List<Card> playedCards, List<Card> uncountedCards, int currentCount);
        string Description { get; set; }
        PlayerName PlayerName { get; set; }
        bool Winner { get; set; }
        PlayerAlgorithm PlayerAlgorithm { get; set; }
        void Init(string parameters);
    }

    public class BasePlayer : IPlayer
    {
        public string Description { get; set; }
        public PlayerName PlayerName { get; set; }
        public bool Winner { get; set; }
        public PlayerAlgorithm PlayerAlgorithm { get; set; }

        public virtual Card GetCountCard(List<Card> playedCards, List<Card> uncountedCards, int currentCount)
        {
            throw new NotImplementedException();
        }

        public virtual void Init(string parameters)
        {
            throw new NotImplementedException();
        }

        public virtual List<Card> SelectCribCards(List<Card> hand, bool yourCrib)
        {
            throw new NotImplementedException();
        }
    }

    public class DefaultPlayer : BasePlayer
    {
        public bool UseDropTable { get; set; } = false;
        public DefaultPlayer() { }

        public DefaultPlayer(bool useDropTable)
        {
            UseDropTable = useDropTable;
            if (UseDropTable)
                Description = "Default player using Drop Table";
            else
                Description = "Default player no Drop Table";


        }



        public override Card GetCountCard(List<Card> playedCards, List<Card> uncountedCards, int currentCount)
        {
            int maxScore = -1;
            Card maxCard = null;
            int score = 0;

            if (uncountedCards.Count == 1)
            {
                if (uncountedCards[0].Value + currentCount <= 31)
                    return uncountedCards[0];
                else
                    return null;
            }

            //
            //  see which card we can play that gives us the most points
            foreach (Card c in uncountedCards)
            {
                score = CardScoring.ScoreCountingCardsPlayed(playedCards, c, currentCount);
                if (score > maxScore)
                {
                    maxScore = score;
                    maxCard = c;
                }
            }

            if (maxScore == -1)
                return null; // we have no valid card to play


            if (maxScore == 0) // there isn't a card for us to play that generates points
            {
                //
                //  play a card that we have a pair so we can get 3 of a kind - as long as it isn't a 5 and the 3 of a kind makes > 31
                //

                for (int i = 0; i < uncountedCards.Count - 1; i++)
                {

                    //  dont' do it if it will force us over 31
                    if (uncountedCards[i].Rank * 3 + currentCount > 31)
                        continue;

                    if (uncountedCards[i].Rank == uncountedCards[i + 1].Rank)
                    {
                        if (uncountedCards[i].Rank != 5)
                            return uncountedCards[i];

                    }
                }

                //
                //  make the right choice if assuming they'll play a 10
                //
                Combinations<Card> combinations = new Combinations<Card>(uncountedCards, 2); // at most 6 of these: 4 choose 2
                foreach (List<Card> cards in combinations)
                {
                    int sum = cards[0].Value + cards[1].Value;
                    if (sum + currentCount == 5) // i'll 15 them if they play a 10
                        return cards[1];

                    if (sum + currentCount == 21) // i'll 31 them if they play a 10
                        return cards[1];

                }

            }

            if (maxCard.Rank == 5)
            {
                // try to find a non 5 card to play
                foreach (Card c in uncountedCards)
                {
                    if (c.Rank != 5 && c.Value + currentCount <= 31)
                    {
                        maxCard = c;
                        break;
                    }
                }
            }

            return maxCard;
        }


        public override List<Card> SelectCribCards(List<Card> hand, bool myCrib)
        {
            Combinations<Card> combinations = new Combinations<Card>(hand, 4);
            List<Card> maxCrib = null;
            double maxScore = -1000.0;

            foreach (List<Card> cards in combinations)
            {
                double score = (double)CardScoring.ScoreHand(cards, null, HandType.Regular);
                List<Card> crib = GetCrib(hand, cards);
                if (UseDropTable)
                {
                    double expectedValue = 0.0;
                    if (myCrib)
                    {
                        expectedValue = CribbageStats.dropTableToMyCrib[crib[0].Rank - 1, crib[1].Rank - 1];
                        score += expectedValue;
                    }
                    else
                    {
                        expectedValue = CribbageStats.dropTableToYouCrib[crib[0].Rank - 1, crib[1].Rank - 1];
                        score -= expectedValue;
                    }

                }
                if (score > maxScore)
                {
                    maxScore = score;
                    maxCrib = crib;
                }
            }

            return maxCrib;
        }

        private List<Card> GetCrib(List<Card> hand, List<Card> cards)
        {
            List<Card> crib = new List<Card>(hand);

            foreach (Card card in cards)
            {
                crib.Remove(card);
            }
            return crib;
        }

        public override void Init(string parameters)
        {
            if (parameters == "-usedroptable")
            {
                UseDropTable = true;
                Description = "Default player using Drop Table";
            }
            else
            
                Description = "Default player no Drop Table";
        }
    }


}
