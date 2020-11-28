using SampleProcess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileWatchService.FileWatcher
{
    public class FileSystemWatcherControl : FileSystemWatcher
    {
        private static int _BUFFERSIXE = 64 * 1024; //64KB
        private static NotifyFilters _NOTIFYFILTER = NotifyFilters.Attributes | NotifyFilters.FileName | NotifyFilters.Size;

        public string Name { get; private set; }
        public bool IsIncludeSub { get; set; }
        public WatcherChangeTypes WatcherType { get; private set; }
        public string ControlName => (this.GetType().Name);
        public bool IsInitialize { get; set; }

        private const int RELOAD_WATCH_CHANGE = 10;

        private IFileWatchEvent FileWatchEvent;

        public delegate void ErrorEventHandler(ErrorEventArgs _eventArgs);
        public ErrorEventHandler ErrorEvent { get; set; }

        private Dictionary<string, WatcherChangeInfo> WatcherChangedTimes = null;
        private Timer RefreshWatcherChangesTimer;

        /// <summary>
        /// 監聽事件類型枚舉
        /// </summary>
        public enum WatcherEventType
        {
            Create, Change, Delete
        }

        /// <summary>
        /// 建構子
        /// </summary>
        public FileSystemWatcherControl(string _name, string _path, IFileWatchEvent _fileWatchEvent)
        {
            this.Name = _name;
            this.IsIncludeSub = true;
            this.Path = _path;
            this.NotifyFilter = _NOTIFYFILTER;
            this.InternalBufferSize = _BUFFERSIXE;
            this.EnableRaisingEvents = true;

            this.FileWatchEvent = _fileWatchEvent;
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="_name">名稱</param>
        /// <param name="_path">目標路經</param>
        /// <param name="_filter">篩選</param>
        /// <param name="_type">類型</param>
        public FileSystemWatcherControl(string _name, string _path, string _filter, WatcherChangeTypes _type)
        {
            this.Name = _name;
            this.IsIncludeSub = false;
            this.WatcherType = _type;

            //create FileSystemWatcher object
            this.Path = _path;
            this.Filter = _filter;
            this.NotifyFilter = _NOTIFYFILTER;
            this.InternalBufferSize = _BUFFERSIXE;
            this.EnableRaisingEvents = true;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            this.IncludeSubdirectories = this.IsIncludeSub;
            this.WatcherChangedTimes = new Dictionary<string, WatcherChangeInfo>();
            this.RefreshWatcherChangesTimer = new Timer(new TimerCallback(this.ClearWatcherChangedTimes), null, 5, 5);

            if ((this.WatcherType & WatcherChangeTypes.Created) == WatcherChangeTypes.Created) this.Created += new FileSystemEventHandler(WatcherOnChanges);
            if ((this.WatcherType & WatcherChangeTypes.Changed) == WatcherChangeTypes.Changed) this.Changed += new FileSystemEventHandler(WatcherOnChanges);
            if ((this.WatcherType & WatcherChangeTypes.Renamed) == WatcherChangeTypes.Renamed) this.Renamed += new RenamedEventHandler(WatcherOnChanges);
            if ((this.WatcherType & WatcherChangeTypes.Deleted) == WatcherChangeTypes.Deleted) this.Deleted += new FileSystemEventHandler(WatcherOnChanges);

            this.Error += new System.IO.ErrorEventHandler(OnError);

            this.IsInitialize = true;
        }

        /// <summary>
        /// 監聽事件
        /// </summary>
        /// <param name="source">FileSystemWatcher</param>
        /// <param name="e">監聽事件</param>
        private void WatcherOnChanges(object source, FileSystemEventArgs e)
        {
            try
            {
                //判斷事件多次觸發
                if (!this.WatcherChangeTimesCheck(e.FullPath)) return;

                //監聽器實體
                var watcher = source as FileSystemWatcher;

                //啟動執行緒跑單一檔案
                Thread thread = new Thread(WatcherOnChangesThread);
                thread.Start(e);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 例外監聽事件
        /// </summary>
        /// <param name="source">FileSystemWatcher</param>
        /// <param name="e">監聽事件</param>
        private void OnError(object source, ErrorEventArgs e)
        {
            this.ErrorEvent?.Invoke(e);

            /*
            //  Show that an error has been detected.
            Console.WriteLine("The FileSystemWatcher has detected an error");
            //  Give more information if the error is due to an internal buffer overflow.
            if (e.GetException().GetType() == typeof(InternalBufferOverflowException))
            {
                //  This can happen if Windows is reporting many file system events quickly
                //  and internal buffer of the  FileSystemWatcher is not large enough to handle this
                //  rate of events. The InternalBufferOverflowException error informs the application
                //  that some of the file system events are being lost.
                Console.WriteLine(("The file system watcher experienced an internal buffer overflow: " + e.GetException().Message));
            }
            */
        }

        /// <summary>
        /// 檔案異動方法
        /// </summary>
        /// <param name="obj">監聽事件</param>
        private void WatcherOnChangesThread(object obj)
        {
            //檔案監聽事件
            FileSystemEventArgs eventArgs = obj as FileSystemEventArgs;
            if (eventArgs == null) return;

            var info = new FileInfo(eventArgs.FullPath);

            switch (eventArgs.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    {
                        //On Initial
                        this.FileWatchEvent.OnFileInitialEvent(info.Name, info.FullName);

                        //On Create
                        this.FileWatchEvent.OnFileCreateEvent(info.Name, info.FullName);
                    }
                    break;
                case WatcherChangeTypes.Changed:
                case WatcherChangeTypes.Renamed:
                    {
                        //等待檔案處理完畢
                        if (!WaitForFile(new FileInfo(eventArgs.FullPath)))
                        {
                            this.RefreshWatcherChangedTimes(eventArgs.FullPath);
                            return;
                        }

                        this.FileWatchEvent.OnFileCreateCompleteEvent(info.Name, info.FullName);
                    }
                    break;

                case WatcherChangeTypes.Deleted:
                    {
                        //Nothing
                    }
                    break;
            }

            this.RefreshWatcherChangedTimes(eventArgs.FullPath);
        }

        /// <summary>
        /// 判斷事件多次觸發方法
        /// </summary>
        /// <param name="_fullpath">檔案路徑</param>
        private bool WatcherChangeTimesCheck(string _fullpath)
        {
            if (this.WatcherChangedTimes.ContainsKey(_fullpath))
            {
                return false;
            }
            else
            {
                this.WatcherChangedTimes[_fullpath] = new WatcherChangeInfo();
            }

            return true;
        }

        /// <summary>
        /// 更新觸發事件紀錄
        /// </summary>
        /// <param name="_fullpath">檔案路徑</param>
        private void RefreshWatcherChangedTimes(string _fullpath)
        {
            //將該路徑檔案設為合法
            if (this.WatcherChangedTimes.ContainsKey(_fullpath) && !this.WatcherChangedTimes[_fullpath].IsVaild)
            {
                this.WatcherChangedTimes[_fullpath].IsVaild = true;
                this.WatcherChangedTimes[_fullpath].ChangeTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 清除暫存觸發事件紀錄
        /// </summary>
        /// <param name="_obj">空物件</param>
        private void ClearWatcherChangedTimes(object _obj)
        {
            //清除過時路徑
            foreach (var k in this.WatcherChangedTimes.ToList())
            {
                if (k.Value.IsVaild && DateTime.Now.Subtract(k.Value.ChangeTime).TotalSeconds >= RELOAD_WATCH_CHANGE)
                {
                    this.WatcherChangedTimes.Remove(k.Key);
                }
            }
        }

        /// <summary>
        /// 等待檔案異動完成
        /// </summary>
        /// <param name="_fi">檔案資訊</param>
        /// <param name="_count">等待次數</param>
        private bool WaitForFile(FileInfo _fi, int _count = -1)
        {
            var cnt = 0;

            while (_count < 0 || _count > cnt)
            {
                try
                {
                    cnt++;

                    using (Stream stream = _fi.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        stream.Flush();
                        stream.Close();
                        return isFileCanRead(_fi);
                    }
                }
                catch (IOException e)
                {
                    //File isn't ready yet, So we need to keep on waiting until it is.
                    var sleeptime = (cnt * 500);
                    Thread.Sleep(sleeptime);  //延遲
                    if (File.Exists(_fi.FullName)) continue;

                    break;
                }
                catch (Exception e)
                {
                    break;
                }
            }

            return false;
        }

        /// <summary>
        /// 判斷檔案是否可讀
        /// </summary>
        /// <param name="_fi">檔案資訊</param>
        private bool isFileCanRead(FileInfo _fi)
        {
            try
            {
                var fs = new FileStream(_fi.FullName, FileMode.Open);

                fs.Flush();
                fs.Close();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public class WatcherChangeInfo
        {
            public DateTime ChangeTime { get; set; }
            public bool IsVaild { get; set; }

            public WatcherChangeInfo()
            {
                this.ChangeTime = DateTime.Now;
                this.IsVaild = false;
            }
        }
    }
}
