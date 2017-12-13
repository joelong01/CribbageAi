using System;
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
   
    public enum LogType { DealerHand, NonDealerHand, SharedCard, DealerCrib, NonDealerCrib, Count, AddScore, ScoreHand, ScoreCrib, CurrentScore, Winner, PlayCountCard };

  

    class Logger 
    {
      
        
        
        StorageFile _file = null;
        
        public Logger()
        {
            
           
        }

        public async Task Init(int gameID, StorageFolder folder)
        {
           
            string saveFileName = String.Format($"game_{gameID}.cailog");            
            _file = await folder.CreateFileAsync(saveFileName, CreationCollisionOption.ReplaceExisting);
            if (gameID == 0)
            {
                string s = "GameID\tSequence\tGamesState\tAlgorithm\tLogType\tLine\r\n";
                await FileIO.AppendTextAsync(_file, s);
            }
        }

     

        public async Task AddLogLine(int gameID, int sequence, GameState state, LogType logType, PlayerName name, string line)
        {
            
            if (_file == null)
                return;

            string s = String.Format($"{gameID}\t{sequence}\t{state}\t{name}\t{logType}\t{line}\r\n");
            
            try
            {
                await FileIO.AppendTextAsync(_file, s);

            }
            catch (Exception exception)
            {
                //
                // this will likely be running on a threadpool thread so to show the messagebox we have to hop back over to the UI thread
                var ignored = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    string error = StaticHelpers.GetErrorMessage($"Error saving to file\n{_file.Path}\n writing\n {s}", exception);
                    MessageDialog dlg = new MessageDialog(error);
                    await dlg.ShowAsync();
                });
            }
        }

        
       
    }
}
