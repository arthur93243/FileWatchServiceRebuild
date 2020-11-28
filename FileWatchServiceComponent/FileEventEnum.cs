using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatchServiceComponent
{
    public enum FileEventEnum
    {
        /// <summary>
        /// 初始化
        /// </summary>
        Initialize,

        /// <summary>
        /// 已建立
        /// </summary>
        Created,

        /// <summary>
        /// 已建立完成
        /// </summary>
        Complete,

        /// <summary>
        /// 已刪除
        /// </summary>
        Deleted
    }
}
