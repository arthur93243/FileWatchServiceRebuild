using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProcess
{
    /// <summary>
    /// FileWatchService事件介面
    /// </summary>
    public interface IFileWatchEvent
    {
        /// <summary>
        /// 檔案初始化時事件
        /// </summary>
        void OnFileInitialEvent(string _fileName, string _fullPath);

        /// <summary>
        /// 檔案建立時事件
        /// </summary>
        void OnFileCreateEvent(string _fileName, string _fullPath);

        /// <summary>
        /// 檔案建立完成時事件
        /// </summary>
        void OnFileCreateCompleteEvent(string _fileName, string _fullPath);

        /// <summary>
        /// 檔案刪除時事件
        /// </summary>
        //void OnFileDeletedEvent();

        /// <summary>
        /// 檔案重新命名時事件
        /// </summary>
        //void OnFileRenamedEvent();

        /// <summary>
        /// 檔案變更時事件
        /// </summary>
        //void OnFileChangedEvent();
    }
}
