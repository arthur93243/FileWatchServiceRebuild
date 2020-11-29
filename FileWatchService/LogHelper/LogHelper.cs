using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatchService
{
    public static class LogHelper
    {
        public static ILogger Logger = LogManager.GetCurrentClassLogger();


        public static void SetTraceLog(string _message, params object[] _objs)
        {
            Logger.Trace(String.Format(_message, _objs));
        }

        public static void SetDebugLog(string _message, params object[] _objs)
        {
            Logger.Debug(String.Format(_message, _objs));
        }

        public static void SetInfoLog(string _message, params object[] _objs)
        {
            Logger.Info(String.Format(_message, _objs));
        }
    }   
}
