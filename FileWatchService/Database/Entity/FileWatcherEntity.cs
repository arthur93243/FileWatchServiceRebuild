using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatchService.Model
{
    public class FileWatcherEntity
    {
        public string ProcessID { get; set; }
        public string WatcherID { get; set; }
        public string WatcherName { get; set; }
        public string WatcherPath { get; set; }
        public bool Enable { get; set; }

    }
}
