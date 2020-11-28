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
    }
}
