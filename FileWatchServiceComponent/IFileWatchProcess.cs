using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatchServiceComponent
{
    /// <summary>
    /// FileWatc程序介面
    /// </summary>
    public interface IFileWatchProcess : IDisposable
    {
       /// <summary>
       /// 初始化
       /// </summary>
        void Initialize();

        /// <summary>
        /// 取得程序名稱
        /// </summary>
        /// <returns>程序名稱</returns>
        string GetProcessName();
    }
}
