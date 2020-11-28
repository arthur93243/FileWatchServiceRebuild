using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatchService.Model
{
    public class ProcessInfoEntity
    {
        public string ProcessID { get; set; }
        public string ProcessName { get; set; }
        public string ProcessLibraryName { get; set; }
        public string ProcessLibraryFullName { get; set; }
        public string ProcessMainClassFullName { get; set; }
        public bool IsValid { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
