using System;
using System.Collections.Generic;
using System.Linq;

namespace WebViewWithSendingPrint.Maui.Services
{
    public class OnScreenLogEventArgs : EventArgs
    {
        public string LastLogEntry { get; set; }
    }

    public class OnScreenLogs
    {
        //private List<string> log = new List<string>();

        //public List<string> Log
        //{
        //    get { return log; }
        //    set 
        //    {
        //        log = value;
        //        OnLogUpdated(new OnScreenLogEventArgs { LastLogEntry = log.Last() });
        //    }
        //}

        private string lastLogEntry;

        public string LastLogEntry
        {
            get { return lastLogEntry; }
            set 
            { 
                if (value != lastLogEntry)
                {
                    lastLogEntry = value;
                    OnLogUpdated(new OnScreenLogEventArgs { LastLogEntry = lastLogEntry });
                }
            }
        }


        protected virtual void OnLogUpdated(OnScreenLogEventArgs e)
        {
            LogUpdated?.Invoke(this, e);
        }

        public event EventHandler LogUpdated;

    }
}
