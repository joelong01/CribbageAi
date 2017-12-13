using Cards;
using CribbageAI.Cards;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace CribbageAI
{

   
    public enum HandType { Crib = 0, Regular = 1 };

    public enum ScoreType { Hand, Crib, Count};



    public enum GameState
    {
        Uninitialized,
        Start,
        Deal,
        SelectCrib,
        GiveToCrib,
        Count,
        ScoreHand,
        CountingEnded,
        ScoreCrib,
        ShowCrib,
        EndOfHand,
        GameOver,
        None
    }

  public class GameStats
    {
        public int PlayerOneWin { get; set; } = 0;
        public int PlayerTwoWin { get; set; } = 0;
        public double    MSPerGame { get; set; } = 0;
        public double WallClocktime { get; set; } = 0;
        public double PlayerOneWinPercent { get; set; } = 0;
        public double PlayerTwoWinPercent { get; set; } = 0;
        
        public double AverageScorePlayerTwo { get; set; } = 0;
        public double AverageScorePlayerOne { get; set; } = 0;
        public double AverageHandPointsPlayerOne { get; set; } = 0;
        public double AverageHandPointsPlayerTwo { get; set; } = 0;
        public double AverageCribPointsPlayerOne { get; set; } = 0;
        public double AverageCribPointsPlayerTwo { get; set; } = 0;
        public double AverageCountPointsPlayerOne { get; set; } = 0;
        public double AverageCountPointsPlayerTwo { get; set; } = 0;
        
    }

    public class PlayerState
    {

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

        public int Score { get; set; } = 0;
        public int CountPoints { get; set; } = 0; // number of points gained by counting
        public int HandPoints { get; set; } = 0;
        public int CribPoints { get; set; } = 0;

        public int CribCount { get; set; } = 0;
        public int HandCount { get; set; } = 0;
        public int CountingSessions { get; set; } = 0;

        public IPlayer IPlayer { get; set; } = null;
        public override string ToString()
        {
            return String.Format($"[{IPlayer?.Description}].Score:{Score}");

        }


    }

    class Game
    {
        //
        //  these are what we are initialized with and shouldn't change after being set
        PlayerState _playerTwo = null;
        PlayerState _playerOne = null;

        //
        //  these toggle back and forth 
        PlayerState _dealer = null;
        PlayerState _nondealer = null;

        //
        // these keep track of whose turn it is to play a card during counting
        PlayerState _currentCountPlayer = null;
        PlayerState _nextCountPlayer = null;

        int _gameID = -1;
        GameState _state = GameState.Uninitialized;
     
        Card _sharedCard = null;

        int _logSequence = 0;

        private static int _deckShuffleSeed = Environment.TickCount;
        Logger _log = null;

        public PlayerState PlayerOne
        {
            get
            {
                return _playerOne;
            }
        }

        public PlayerState PlayerTwo
        {
            get
            {
                return _playerTwo;
            }
        }
        
        public Game(int gameID,  IPlayer player1, IPlayer player2, bool player1Deals)
        {
            _gameID = gameID;
            
            _playerOne = new PlayerState
            {
                Score = 0,
                IPlayer = player1,               

            };
            player1.PlayerName = PlayerName.PlayerOne;

            _playerTwo = new PlayerState
            {
                Score = 0,
                IPlayer = player2,

            };

            player2.PlayerName = PlayerName.PlayerTwo;


            if (player1Deals)
            {
                _dealer = _playerOne;
                _nondealer = _playerTwo;
            }
            else
            {
                _dealer = _playerTwo;
                _nondealer = _playerOne;
            }
            
        }

        public async Task Init(bool useLog, StorageFolder folder)
        {
            if (useLog)
            {
                _log = new Logger();
                await _log.Init(_gameID, folder);
            }

        }

        public PlayerState Winner
        {
            get
            {
                if (_playerOne.Score > 120)
                    return _playerOne;

                if (_playerTwo.Score > 120)
                    return _playerTwo;

                return null;
            }
        }

        private async Task LogLine(PlayerName name, LogType logType, string toLog)
        {
            if (_log != null)
            {
              //  _log.AddLogLineQueued(_gameID, _logSequence++, _state, logType, name, toLog);
              await  _log.AddLogLine(_gameID, _logSequence++, _state, logType, name, toLog);
            }


        }


        private void SwapPlayers(ref PlayerState o1, ref PlayerState o2)
        {
            PlayerState temp = o1;
            o1 = o2;
            o2 = temp;
        }

        public Task<PlayerState> StartAsync()
        {
            return Task.Run(() => PlayGame());
        }

        public async Task<PlayerState> PlayGame()
        {
            
            while (_dealer.Score < 121 && _playerTwo.Score < 121)
            {
                switch (_state)
                {
                    case GameState.Uninitialized:
                        _state = GameState.Deal;
                        break;
                    case GameState.Start:
                        //skipped
                        break;
                    case GameState.Deal:
                        await DoDeal();
                        _state = GameState.SelectCrib;
                        break;
                    case GameState.SelectCrib:
                        await DoSelectCrib();
                        _state = GameState.Count;
                        break;
                    case GameState.GiveToCrib:
                        //skipped
                        break;
                    case GameState.Count:
                        {
                            PlayerState ret = await DoCount();
                            if (ret != null)
                                return ret;

                        }
                        _state = GameState.ScoreHand;
                        break;
                    case GameState.ScoreHand:
                        {
                            PlayerState ret = await DoScoreHand();
                            if (ret != null)
                                return ret;
                        }
                        _state = GameState.EndOfHand;
                        break;
                    case GameState.CountingEnded:
                        break;
                    case GameState.ScoreCrib:
                        break;
                    case GameState.ShowCrib:
                        break;
                    case GameState.EndOfHand:
                        SwapPlayers(ref _dealer, ref _nondealer);
                        _state = GameState.Deal;
                        break;
                    case GameState.GameOver:
                        break;
                    case GameState.None:
                        break;
                    default:
                        break;
                }
            }

           

            return null;
        }

        private async Task<PlayerState> DoScoreHand()
        {
            int score = 0;

            await LogLine(_nondealer.IPlayer.PlayerName, LogType.ScoreHand, $"{Card.CardsToString(_nondealer.Hand)} Shared: {_sharedCard}");
            score = CardScoring.ScoreHand(_nondealer.Hand, _sharedCard, HandType.Regular);
            _nondealer.HandCount++;
            if (await AddScore(_nondealer, score, ScoreType.Hand)) return _nondealer;  // has side effects...                        

            await LogLine(_dealer.IPlayer.PlayerName, LogType.ScoreHand, $"{Card.CardsToString(_dealer.Hand)} Shared: {_sharedCard}");
            score = CardScoring.ScoreHand(_dealer.Hand, _sharedCard, HandType.Regular);
            _dealer.HandCount++;
            if (await AddScore(_dealer, score, ScoreType.Hand)) return _dealer;  // has side effects...      

            await LogLine(_dealer.IPlayer.PlayerName, LogType.ScoreCrib, $"{Card.CardsToString(_dealer.Crib)} Shared: {_sharedCard}");
            score = CardScoring.ScoreHand(_dealer.Crib, _sharedCard, HandType.Crib);
            _dealer.CribCount++;
            if (await AddScore(_dealer, score, ScoreType.Crib)) return _dealer;  // has side effects...            

            return null;
        }

        private async Task<PlayerState> DoCount()
        {
            List<Card> playedCards = new List<Card>();
            _dealer.UncountedCards.Clear();
            _dealer.UncountedCards.AddRange(_dealer.Hand);
            _nondealer.UncountedCards.Clear();
            _nondealer.UncountedCards.AddRange(_nondealer.Hand);
            _currentCountPlayer = _nondealer; // nondealer always starts the count
            _nextCountPlayer = _dealer;
            bool winner = false;
            int currentCount = 0;
            int score = 0;
            _dealer.CountingSessions++;
            _nondealer.CountingSessions++;
            while (_currentCountPlayer.UncountedCards.Count > 0 || _nextCountPlayer.UncountedCards.Count > 0)
            {
                Card card = _currentCountPlayer.IPlayer.GetCountCard(playedCards, _currentCountPlayer.UncountedCards, currentCount);

                if (card != null)
                {
                    score = await ScoreCountedCard(_currentCountPlayer, playedCards, card, currentCount);


                    if (score > 120)
                    {
                        score = 121;
                        return _currentCountPlayer;
                    }



                    currentCount += card.Value;


                    //
                    //  if there are no cards left, grant last card

                    if (_currentCountPlayer.UncountedCards.Count == 0 && _nextCountPlayer.UncountedCards.Count == 0)
                    {
                        winner = await AddScore(_currentCountPlayer, 1, ScoreType.Count);
                        if (winner)
                        {
                            score = 121;
                            return _currentCountPlayer;
                        }
                        break;
                    }


                    SwapPlayers(ref _currentCountPlayer, ref _nextCountPlayer);
                }
                else  // the Player is smart enough to not pass back garbage, and told us there is no card to play
                {
                    SwapPlayers(ref _currentCountPlayer, ref _nextCountPlayer);

                    do
                    {
                        // when a player can't go in counting phase, the other player gets to keep playing until they too can't go. 
                        // The terminating conditions are the player returns a null card or the count lands on 31

                        card = _currentCountPlayer.IPlayer.GetCountCard(playedCards, _currentCountPlayer.UncountedCards, currentCount);

                        if (card != null)
                        {
                            score = await ScoreCountedCard(_currentCountPlayer, playedCards, card, currentCount);
                            if (score > 120)
                            {
                                return _currentCountPlayer;
                            }

                            currentCount += card.Value;

                        }


                    } while (card != null && currentCount < 31);

                    // next turn can't play either - we scored LastCard
                    //                      
                    if (currentCount != 31) // if we hit 31, we scored last card
                    {
                        winner = await AddScore(playedCards.Last().Owner, 1, ScoreType.Count);
                        if (winner)
                        {
                            return playedCards.Last().Owner;
                        }

                    }

                    //
                    //  whoever didn't get last card gets to go first

                    if (playedCards.Last().Owner == _currentCountPlayer)
                    {
                        SwapPlayers(ref _currentCountPlayer, ref _nextCountPlayer);
                    }
                    else
                    {
                        //
                        //   no else -- we already toggeld

                        //Debug.WriteLine("Strange result after last card");
                    }

                    currentCount = 0;
                    playedCards.Clear();
                }



            }

            return null;
        }

        private async Task DoSelectCrib()
        {
            _dealer.Crib.Clear();
            _nondealer.Crib.Clear();
            List<Card> crib = _dealer.IPlayer.SelectCribCards(_dealer.Hand, true);
            _dealer.Hand.Remove(crib[0]);
            _dealer.Hand.Remove(crib[1]);
            _dealer.Crib.AddRange(crib);
            await LogLine(_dealer.IPlayer.PlayerName, LogType.DealerCrib, Card.CardsToString(crib));

            crib = _nondealer.IPlayer.SelectCribCards(_nondealer.Hand, false);
            _nondealer.Hand.Remove(crib[0]);
            _nondealer.Hand.Remove(crib[1]);
            await LogLine(_nondealer.IPlayer.PlayerName, LogType.NonDealerHand, Card.CardsToString(crib));

            _dealer.Crib.AddRange(crib);


            
        }

        private async Task DoDeal()
        {
            {
                //
                // need to have a different seed across all the threads or we'll end up playing the same hand over and over
                // 
                Interlocked.Increment(ref _deckShuffleSeed);
                Deck deck = new Deck(_deckShuffleSeed);
                
                List<Card> cards = deck.GetCards(13); // 6 for each player and 1 shared
                _dealer.Hand.Clear();
                _nondealer.Hand.Clear();
                for (int i = 0; i < 6; i++)
                {
                    _dealer.Hand.Add(cards[i]);
                    cards[i].Owner = _dealer;
                    _nondealer.Hand.Add(cards[i + 6]);
                    cards[i + 6].Owner = _nondealer;

                }
                _sharedCard = cards[12];
                _sharedCard.Owner = null;

               await LogLine(_dealer.IPlayer.PlayerName, LogType.DealerHand, $"{Card.CardsToString(_dealer.Hand)}");
               await LogLine(_nondealer.IPlayer.PlayerName, LogType.NonDealerHand, $"{Card.CardsToString(_nondealer.Hand)}");
               await LogLine(PlayerName.None, LogType.SharedCard, $"{_sharedCard.ToString()}");
                
                
            }
        }

        private async Task<bool> AddScore(PlayerState player, int score, ScoreType scoreType)
        {
            if (score == 0)
                return false;

            player.Score += score; //can and is often 0        
            await LogLine(player.IPlayer.PlayerName, LogType.AddScore, $"{score}");
            await LogLine(player.IPlayer.PlayerName, LogType.CurrentScore, $"PlayerOne: {_playerOne.Score} PlayerTwo: {_playerTwo.Score}");
            switch (scoreType)
            {
                case ScoreType.Hand:
                    player.HandPoints += score;
                    break;
                case ScoreType.Crib:
                    player.CribPoints += score;
                    break;
                case ScoreType.Count:
                    player.CountPoints += score;
                    break;
                default:
                    break;
            }
            if (player.Score > 120)
            {
                player.Score = 121;
                player.IPlayer.Winner = true;
                await LogLine(player.IPlayer.PlayerName, LogType.Winner, $"Player1: {_playerOne.Score} vs Player2: {_playerTwo.Score}");
                return true;
            }
            return false;
        }

        private async Task<int> ScoreCountedCard(PlayerState currentTurn, List<Card> playedCards, Card card, int currentCount)
        {

            int score = CardScoring.ScoreCountingCardsPlayed(playedCards, card, currentCount);
            if (score != -1)
            {
                await LogLine(currentTurn.IPlayer.PlayerName, LogType.PlayCountCard, $"{card}");
                await LogLine(currentTurn.IPlayer.PlayerName, LogType.Count, $"{currentCount + card.Value}");
                await AddScore(currentTurn, score, ScoreType.Count);

                // mark the card as counted
                currentTurn.UncountedCards.Remove(card);
                playedCards.Add(card);


            }
            return score;
        }

        public double AverageScore(PlayerName name, ScoreType type)
        {
            PlayerState state = _playerOne;
            if (name == PlayerName.PlayerTwo)
            {
                state = _playerTwo;
            }
            
            switch (type)
            {
                case ScoreType.Crib:
                    return state.CribPoints / state.CribCount;
                case ScoreType.Hand:
                    return state.HandPoints / state.HandCount;
                case ScoreType.Count:
                    return state.CountPoints / state.CountingSessions;
                default:
                    throw new InvalidProgramException("what the heck??");
                    
            }
        }


    }
}

