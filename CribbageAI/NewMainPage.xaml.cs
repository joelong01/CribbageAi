using CribbageAI.Players;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CribbageAI
{

    public class AvailablePlayer
    {
        public string Parameters { get; set; }
        public Type GameType { get; set; }
        public string Description { get; set; }
        public PlayerAlgorithm PlayerAlgorithm { get; set; }


        public AvailablePlayer(string param, Type type, string des, PlayerAlgorithm algorythm)
        {
            Parameters = param;
            GameType = type;
            Description = des;
            PlayerAlgorithm = algorythm;
        }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewMainPage : Page, INotifyPropertyChanged
    {
        private ObservableCollection<AvailablePlayer> AllPlayers = new ObservableCollection<AvailablePlayer>();

        public event PropertyChangedEventHandler PropertyChanged;

        private AvailablePlayer PlayerOne { get; set; } = null;
        private AvailablePlayer PlayerTwo { get; set; } = null;
        private int Iterations { get; set; } = 1;
        private int Loops { get; set; } = 1;
        private int TotalGames { get; set; } = 1;

        private int PlayerOneWin { get; set; } = 0;
        private int PlayerTwoWin { get; set; } = 0;
        private double MSPerGame { get; set; } = 0;
        private string WallClocktime { get; set; } = "";
        public double PlayerOneWinPercent { get; set; } = 0.0;
        public double PlayerTwoWinPercent { get; set; } = 0.0;
        public double AverageScorePlayerOne { get; set; } = 0.0;
        public double AverageScorePlayerTwo { get; set; } = 0.0;
        public bool? AlternateWhoStarts = true;
        public bool? UseLog = true;




        private void UpdateUI()
        {
            NotifyPropertyChanged("PlayerOneWin");
            NotifyPropertyChanged("PlayerTwoWin");
            NotifyPropertyChanged("MSPerGame");
            NotifyPropertyChanged("WallClocktime");
            NotifyPropertyChanged("PlayerOneWinPercent");
            NotifyPropertyChanged("PlayerTwoWinPercent");
            NotifyPropertyChanged("AlternateWhoStarts");
            NotifyPropertyChanged("AverageScorePlayerTwo");
            NotifyPropertyChanged("AverageScorePlayerOne");
            NotifyPropertyChanged("UseLog");


        }
        private void OnClear(object sender, RoutedEventArgs e)
        {
            PlayerOneWin = 0;
            PlayerTwoWin = 0;
            MSPerGame = 0;
            WallClocktime = "";
            PlayerOneWinPercent = 0;
            PlayerTwoWinPercent = 0;
            AlternateWhoStarts = true;
            AverageScorePlayerTwo = 0;
            AverageScorePlayerOne = 0;

            UpdateUI();
        }

        public NewMainPage()
        {
            this.InitializeComponent();

            AvailablePlayer game = new AvailablePlayer("", typeof(DefaultPlayer), "Default - Easy", PlayerAlgorithm.Easy);
            AllPlayers.Add(game);
            PlayerOne = game;
            game = new AvailablePlayer("-usedroptable", typeof(DefaultPlayer), "Default - Hard", PlayerAlgorithm.Hard);
            AllPlayers.Add(game);
            PlayerTwo = game;
            game = new AvailablePlayer("", typeof(RandomPlayer), "Random Player", PlayerAlgorithm.Random);
            AllPlayers.Add(game);
            game = new AvailablePlayer("", typeof(CountingPlayer), "Improved Counting Player", PlayerAlgorithm.ImprovedCounting);
            AllPlayers.Add(game);


        }

        private async void OnGo(object sender, RoutedEventArgs e)
        {
            try
            {


                ((Button)sender).IsEnabled = false;

                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(CoreCursorType.Wait, 1);
                Tuple<int, int, double, string, double, double> ret = await RunGames(Iterations, (bool)UseLog, PlayerOne, PlayerTwo);
                PlayerOneWin = ret.Item1;
                PlayerTwoWin = ret.Item2;
                MSPerGame = (double)Math.Round(ret.Item3, 3);
                WallClocktime = ret.Item4;

                PlayerOneWinPercent = Math.Round((double)PlayerOneWin / (double)(Iterations * Loops) * 100, 2);
                PlayerTwoWinPercent = Math.Round((double)PlayerTwoWin / (double)(Iterations * Loops) * 100, 2);
                AverageScorePlayerOne = Math.Round(ret.Item5, 2);
                AverageScorePlayerTwo = Math.Round(ret.Item6, 2);
                UpdateUI();

                //if (UseLog == true)
                //{
                //    int total = log.Records;

                //    IProgress<int> progress = new Progress<int>((n) =>
                //    {
                //        _uiHint.Text = $"dumped {n} of {total} records in log";
                //    });
                //    await log.Stop(progress);
                //    _uiHint.Text = $"dumped {total} of {total} records in log";
                //}


            }
            finally
            {
                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(CoreCursorType.Arrow, 1);
                ((Button)sender).IsEnabled = true;
                UpdateUI();
            }


        }


        /// <summary>
        ///     Create and run each of the games.  Keep track of how long it takes.  return the stats.
        /// </summary>
        /// <param name="iterations"></param>
        /// <param name="logger"></param>
        /// <param name="playerOne"></param>
        /// <param name="playerTwo"></param>
        /// <returns></returns>

        private async Task<Tuple<int, int, double, string, double, double>> RunGamesAsyncOld(int iterations, bool useLog, AvailablePlayer playerOne, AvailablePlayer playerTwo)
        {


            var watch = System.Diagnostics.Stopwatch.StartNew();


            ConcurrentBag<Game> gameBag = new ConcurrentBag<Game>();
            bool playerOneStarts = true;

            //
            //  this has to happen on the main thread, so it can't happen inside the worker threads
            //
            //  this lets us put the documents in whatever location we want instead of inside the private storage area for the app, which is burried pretty deep in the
            //  disk heirarchy and hard to get to.  You only have to do this once per machine though.
            string content = "After clicking on \"Close\" pick the default location for all your CribbageAi saved state";
            StorageFolder folder = await StaticHelpers.GetSaveFolder(content, "CribbageAI");

            Debug.WriteLine("Starting Parallel.For");

            Parallel.For(0, iterations, async i =>
            {
                IPlayer player1 = (IPlayer)Activator.CreateInstance(playerOne.GameType);
                player1.Init(playerOne.Parameters);
                player1.PlayerAlgorithm = playerOne.PlayerAlgorithm;

                IPlayer player2 = (IPlayer)Activator.CreateInstance(playerTwo.GameType);
                player2.Init(playerTwo.Parameters);
                player2.PlayerAlgorithm = playerTwo.PlayerAlgorithm;


                Game game = new Game(i, player1, player2, playerOneStarts);
                await game.Init(useLog, folder);

                //
                //  alternate who deals - looks like if you use the same algorithm, dealer *loses* 60% of the time!
                if ((bool)AlternateWhoStarts)
                {
                    if (playerOneStarts)
                    {
                        playerOneStarts = false;
                    }
                    else
                    {
                        playerOneStarts = true;
                    }
                }


                gameBag.Add(game);
                await game.PlayGame();



            });

            watch.Stop();
            Debug.WriteLine("Finished Parallel.For");


            int totalP1Score = 0;
            int totalP2Score = 0;
            var elapsedMs = watch.ElapsedMilliseconds;

            double msPerGame = elapsedMs / (double)iterations;



            int p1Win = 0;
            int p2Win = 0;

            foreach (var game in gameBag)
            {
                if (game == null)
                    continue;

                if (game.PlayerOne.IPlayer.Winner)
                    p1Win++;
                else
                    p2Win++;

                totalP1Score += game.PlayerOne.Score;
                totalP2Score += game.PlayerTwo.Score;

            }



            return Tuple.Create(p1Win, p2Win, msPerGame, FormatTime(elapsedMs), totalP1Score / (double)iterations, totalP2Score / (double)iterations);



        }


        private async Task<Tuple<int, int, double, string, double, double>> RunGames(int iterations, bool useLog, AvailablePlayer playerOne, AvailablePlayer playerTwo)
        {


            var watch = System.Diagnostics.Stopwatch.StartNew();


            ConcurrentBag<Game> gameBag = new ConcurrentBag<Game>();
            ConcurrentBag<Task> taskBag = new ConcurrentBag<Task>();
            bool playerOneStarts = true;

            //
            //  this has to happen on the main thread, so it can't happen inside the worker threads
            //
            //  this lets us put the documents in whatever location we want instead of inside the private storage area for the app, which is burried pretty deep in the
            //  disk heirarchy and hard to get to.  You only have to do this once per machine though.
            string content = "After clicking on \"Close\" pick the default location for all your CribbageAi saved state";
            StorageFolder folder = await StaticHelpers.GetSaveFolder(content, "CribbageAI");

            Debug.WriteLine("Starting Parallel.For");

            //
            //  this uses Parallel.For to create 1 thread per processor.  Each thread creates a tasks and starts running it.
            //  The task runs a game to completion, logging everything to a file. I do it this way so that I can wait for the 
            //  logs to finish writing -- I found that if I did it outside of a Task list, it would finish the log writes async
            Parallel.For(0, iterations, i =>
            {
                taskBag.Add(Task.Run(async () =>
                {
                    IPlayer player1 = (IPlayer)Activator.CreateInstance(playerOne.GameType);
                    player1.Init(playerOne.Parameters);
                    player1.PlayerAlgorithm = playerOne.PlayerAlgorithm;

                    IPlayer player2 = (IPlayer)Activator.CreateInstance(playerTwo.GameType);
                    player2.Init(playerTwo.Parameters);
                    player2.PlayerAlgorithm = playerTwo.PlayerAlgorithm;


                    Game game = new Game(i, player1, player2, playerOneStarts);
                    await game.Init(useLog, folder);

                    //
                    //  alternate who deals - looks like if you use the same algorithm, dealer *loses* 60% of the time!
                    if ((bool)AlternateWhoStarts)
                    {
                        if (playerOneStarts)
                        {
                            playerOneStarts = false;
                        }
                        else
                        {
                            playerOneStarts = true;
                        }
                    }


                    gameBag.Add(game);
                    await game.PlayGame();
                }
                ));


            });

            //
            //  wait for all the games to complete -- this includes the log writes
            Task.WaitAll(taskBag.ToArray());
            watch.Stop();


            Debug.WriteLine("Finished Parallel.For");

            //
            //  add up some stats
            int totalP1LostScore = 0;
            int totalP2LostScore = 0;
            var elapsedMs = watch.ElapsedMilliseconds;

            double msPerGame = elapsedMs / (double)iterations;



            int p1Win = 0;
            int p2Win = 0;

            foreach (var game in gameBag)
            {
                if (game == null)
                    continue;
                //
                //  this calculates the number of times each player one and the average of their *losing* score
                //
                if (game.PlayerOne.IPlayer.Winner)
                {
                    totalP2LostScore += game.PlayerTwo.Score;
                    p1Win++;
                }
                else
                {
                    p2Win++;
                    totalP1LostScore += game.PlayerOne.Score;
                }




            }



            return Tuple.Create(p1Win, p2Win, msPerGame, FormatTime(elapsedMs), totalP1LostScore / (double)(p2Win), totalP2LostScore / (double)(p1Win)); // of course, p1win == p2Loss



        }

        private string FormatTime(double ms)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(ms);
            string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                                    t.Hours,
                                    t.Minutes,
                                    t.Seconds,
                                    t.Milliseconds);
            return answer;

        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
    }
}
