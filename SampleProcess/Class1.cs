using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProcess
{
    /// <summary>
    /// FileWatch事件介面
    /// </summary>
    public interface IFileWatchEvent 
    {
        void OnFileCreate();
        void OnFileCreateComplete();
        void OnFileDeleted();
        void OnFileRenamed();
        void OnFileChanged();
    }
}
