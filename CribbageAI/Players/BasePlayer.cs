﻿using Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CribbageAI
{
    public enum PlayerAlgorithm { Easy, Hard, Random, ImprovedCounting, None };
    public enum PlayerName { PlayerOne, PlayerTwo, Uninitialized, None };


    public class Player 
    {
        //
        //  state about the game
        List<Card> _crib = new List<Card>();
        public List<Card> Hand { get; set; } = new List<Card>();
        public List<Card> UncountedCards { get; set; } = new List<Card>();
        public List<Card> Crib
        {
            get
            {
                return _crib;
            }
            set
            {
                _crib = new List<Card>();
                _crib.AddRange(value);
            }
        }

        /// <summary>
        ///     These stats are here to calculate the average score for each of the phases of the game so we can compare algorithms
        /// </summary>
        public int Score { get; set; } = 0;
        public int CountPoints { get; set; } = 0; // number of points gained by counting
        public int HandPoints { get; set; } = 0; // number of points from the hand
        public int CribPoints { get; set; } = 0; // number of points from the crib
        public int CribCount { get; set; } = 0; 
        public int HandCount { get; set; } = 0; 
        public int CountingSessions { get; set; } = 0;

        
        public override string ToString()
        {
            return String.Format($"[{Description}].Score:{Score}");

        }

        //
        //  state about the Player
        public string Description { get; set; }
        public PlayerName PlayerName { get; set; }
        public bool Winner { get; set; }
        public PlayerAlgorithm PlayerAlgorithm { get; set; }

        

        /// <summary>
        ///     Given a list of cards that have been played and the list of cards that are left in the hand
        /// </summary>
        /// <param name="playedCards"> The cards that have already been played </param>
        /// <param name="uncountedCards"> The cards in the players hand that are available to be played</param>
        /// <param name="currentCount">Current count</param>
        /// <returns> the card to play.  NULL if no card is acceptable</returns>
        public virtual Card GetCountCard(List<Card> playedCards, List<Card> uncountedCards, int currentCount)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Initializes the class.  I use this in "default" game to pass in a parameter to tell it to use the drop table.
        ///     this parameter is set in the AvailablePlayer class that is added to the UI ComboBox
        /// </summary>
        /// <param name="parameters"></param>
        //  
        public virtual void Init(string parameters)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        ///     
        /// </summary>
        /// <param name="hand">6 cards that represent a player's hand</param>
        /// <param name="yourCrib">true if the API is returned cards to be added to its own Crib</param>
        /// <returns>2 cards to be added to the crib</returns>
        public virtual List<Card> SelectCribCards(List<Card> hand, bool yourCrib)
        {
            throw new NotImplementedException();
        }
    }
}
