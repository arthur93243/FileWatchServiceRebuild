using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatchService.Database
{
    public class DBHelper
    {
        public string Name { get; private set; }
        public string Path { get; private set; }
        public string ConnectString { get; set; }

        public DBHelper()
        { }
        
    }
}
