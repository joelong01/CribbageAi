
using CribbageAI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Cards
{
    public enum CardNames
    {
        AceOfClubs = 0, TwoOfClubs = 1, ThreeOfClubs = 2, FourOfClubs = 3, FiveOfClubs = 4, SixOfClubs = 5, SevenOfClubs = 6, EightOfClubs = 7, NineOfClubs = 8, TenOfClubs = 9, JackOfClubs = 10, QueenOfClubs = 11, KingOfClubs = 12,
        AceOfDiamonds = 13, TwoOfDiamonds = 14, ThreeOfDiamonds = 15, FourOfDiamonds = 16, FiveOfDiamonds = 17, SixOfDiamonds = 18, SevenOfDiamonds = 19, EightOfDiamonds = 20, NineOfDiamonds = 21, TenOfDiamonds = 22, JackOfDiamonds = 23, QueenOfDiamonds = 24, KingOfDiamonds = 25,
        AceOfHearts = 26, TwoOfHearts = 27, ThreeOfHearts = 28, FourOfHearts = 29, FiveOfHearts = 30, SixOfHearts = 31, SevenOfHearts = 32, EightOfHearts = 33, NineOfHearts = 34, TenOfHearts = 35, JackOfHearts = 36, QueenOfHearts = 37, KingOfHearts = 38,
        AceOfSpades = 39, TwoOfSpades = 40, ThreeOfSpades = 41, FourOfSpades = 42, FiveOfSpades = 43, SixOfSpades = 44, SevenOfSpades = 45, EightOfSpades = 46, NineOfSpades = 47, TenOfSpades = 48, JackOfSpades = 49, QueenOfSpades = 50, KingOfSpades = 51,
        BlackJoker = 52, RedJoker = 53, BackOfCard = 54, Uninitialized = 55
    };
    public enum CardOrdinal
    {
        Uninitialized = 0, Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King
    };

    public enum Location { Unintialized, Deck, Discarded, Computer, Player, Crib };
    public enum Suit { Uninitialized = 0, Clubs = 1, Diamonds = 2, Hearts = 3, Spades = 4 };
    


    public class Card
    {
        public int Index { get; set; } = 0;         // the index into the array of 52 cars (0...51)        
        public int Rank { get; set; } = 0;          // the number of the card in the suit -- e.g. 1...13 for A...K - used for counting runs and sorting   
        public int Value
        {
            get
            {
                if (Rank <= 10)
                    return Rank;

                return 10;
            }
        }

        public Card(CardNames name)
        {
            CardName = name;
        }
        public CardNames CardName
        {
            get
            {
                return (CardNames)(((int)(Suit - 1) * 13 + Rank - 1));

            }
            set
            {
                //
                //  given a number of 0-51, set the suit and the rank

                if ((int)value < 0 || (int)value > 51)
                    throw new InvalidDataException($"The value {(int)value} is an invalid CardNumber");

                int val = (int)value;
                Index = val;
                Suit = (Suit)((int)(val / 13) + 1);
                Rank = val % 13 + 1;
                Ordinal = (CardOrdinal)Rank;
            }
        }
        public Suit Suit { get; set; } = Suit.Uninitialized;
        public CardOrdinal Ordinal { get; set; } = CardOrdinal.Uninitialized;
        public Player Owner { get; set; } = null;

        public override string ToString()
        {
            return CardName.ToString();
        }

        public static string CardsToString(List<Card> cards)
        {

            string s = "";
            foreach (Card c in cards)
            {
                s += c.ToString() + "-";
            }

            return s;

        }

        public static int CompareCardsByRank(Card x, Card y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're 
                    // equal.  
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y 
                    // is greater.  
                    return -1;
                }
            }
            else
            {
                // If x is not null... 
                // 
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the  
                    // lengths of the two Ranks. 
                    // 
                    return x.Rank - y.Rank;


                }
            }


        }
        public static int CompareCardsByIndex(Card x, Card y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're 
                    // equal.  
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y 
                    // is greater.  
                    return -1;
                }
            }
            else
            {
                // If x is not null... 
                // 
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {

                    //
                    //  sorted largest to smallest for easier destructive iterations
                    return (y.Index - x.Index);


                }
            }


        }

        public static int CompareCardsBySuit(Card x, Card y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're 
                    // equal.  
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y 
                    // is greater.  
                    return -1;
                }
            }
            else
            {
                // If x is not null... 
                // 
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {

                    return ((int)y.Suit - (int)x.Suit);


                }
            }


        }

        public static int CompareCardNamesByValue(CardNames x, CardNames y)
        {

            return ((int)x - (int)y);


        }



    }



    public class Deck
    {
        
        int[] _randomIndeces = new int[52];
        int _index = 0; // the index of the cards handed out

        public Deck(int seed)
        {
            Shuffle(seed);
        }

        public void Shuffle(int seed)
        {
            
            MersenneTwister twist = new MersenneTwister(seed);


            for (int i = 0; i < 52; i++)
            {
                _randomIndeces[i] = i;

            }

            int temp = 0;
            for (int n = 0; n < 52; n++)
            {
                int k = twist.Next(n + 1);
                temp = _randomIndeces[n];
                _randomIndeces[n] = _randomIndeces[k];
                _randomIndeces[k] = temp;
            }

            _index = 0;
        }

        public List<Card> GetCards(int number)
        {
            List<Card> cards = new List<Card>();
            for (int i = _index; i < number + _index; i++)
            {
                Card c = new Card((CardNames)_randomIndeces[i]);

                cards.Add(c);
            }

            _index += number;

            return cards;
        }


    }
}
