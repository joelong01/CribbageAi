﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cards;
using System.Collections.Concurrent;
using System.Threading;
using System.Diagnostics;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.ApplicationModel.Core;

namespace CribbageAI
{
    interface ILog
    {
        void AddLogLine(int gameID, int sequence, GameState state, LogType logType, PlayerName name, string line);
    }

    public enum LogType { DealerHand, NonDealerHand, SharedCard, DealerCrib, NonDealerCrib, Count, AddScore, ScoreHand, ScoreCrib, CurrentScore, Winner, PlayCountCard };

    public class LogLine
    {
        public LogLine(int gameID, int sequence, GameState state, LogType logType, PlayerName name , string line)
        {
            GameID = gameID;
            GameState = state;
            PlayerName = name;
            Line = line;
            Sequence = sequence;
            LogType = logType;

            
        }

        public int GameID { get; set; }
        public GameState GameState { get; set; }
        public PlayerName PlayerName{ get; set; }
        public String Line { get; set; }
        public int Sequence { get; set; }
        public LogType LogType { get; set; }

        public override string ToString()
        {
            return String.Format($"{GameID}\t{Sequence}\t{GameState}\t{PlayerName}\t{LogType}\t{Line}");
        }

    }

    class Logger : ILog
    {
        ConcurrentQueue<LogLine> _log = new ConcurrentQueue<LogLine>();
        
        StorageFolder _folder = null;
        StorageFile _file = null;
        bool _stop = false;

        public Logger()
        {
            
           
        }

        public async Task Init()
        {
            string saveFileName = DateTime.Now.ToString("hh_mm_ss dd_MM_yyyy") + ".cailog";
            string content = "After clicking on \"Close\" pick the default location for all your CribbageAi saved state";
            _folder = await StaticHelpers.GetSaveFolder(content, "CribbageAI");
            _file = await _folder.CreateFileAsync(saveFileName, CreationCollisionOption.ReplaceExisting);
            string s = "GameID\tSequence\tGamesState\tAlgorithm\tLogType\tLine\r\n";
            await FileIO.AppendTextAsync(_file, s);
        }

        public int Records
        {
            get
            {
                return _log.Count;
            }
        }
        
        public void AddLogLine(int gameID, int sequence, GameState state, LogType logType, PlayerName name, string line)
        {
            _log.Enqueue(new LogLine(gameID, sequence, state, logType, name, line));
        }

        public void Start()
        {
            TimeSpan period = TimeSpan.FromSeconds(5);
            
            _stop = false;
            ThreadPoolTimer PeriodicTimer = ThreadPoolTimer.CreatePeriodicTimer(async (source) =>
            {
                if (_stop)
                    return;

                await DrainQueueAsync();
                

            }, period);

        }

        private async Task DrainQueueAsync()
        {
            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;

            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
              {
                  while (_log.TryDequeue(out LogLine line))
                  {

                      await AppendPersistentLog(line);
                  }
              });
        }

        public async Task Stop(IProgress<int> progress = null)
        {
            _stop = true;
            int count = 1;
            while (_log.TryDequeue(out LogLine line))
            {
                await AppendPersistentLog(line);
                if (progress != null)
                {
                    count++;
                    if (_log.Count > 1000)
                    {
                        if (count % 100 == 0)
                        {
                            progress.Report(count);
                        }
                    }
                    else if (count % 10 ==0)
                    {
                        progress.Report(count);                        
                    }
                }
            }
        }

        public async Task AppendPersistentLog(LogLine line)
        {
            
            if (_file == null)
                return;
            try
            {
                string s = String.Format($"{line}\r\n");

                await FileIO.AppendTextAsync(_file, s);


            }
            catch (Exception exception)
            {

                string s = StaticHelpers.GetErrorMessage($"Error saving to file", exception);
                MessageDialog dlg = new MessageDialog(s);
                await dlg.ShowAsync();

            }
        }
    }
}