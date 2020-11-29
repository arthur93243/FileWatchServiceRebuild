using FileWatchService.Model;
using SampleProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatchService
{
    public class FileWatcher
    {
        public string ProcessID { get; }
        public string WatcherID { get; }
        public string WatcherName { get; }
        public string WatcherPath { get; }
        public bool Enable { get; set; }

        private FileSystemWatcherControl FileSystemWatcher;
        private IFileWatchEvent WatchEvent;

        public FileWatcher(FileWatcherEntity _fileWatcher, IFileWatchEvent _fileWatchEvent)
        {
            this.ProcessID = _fileWatcher.ProcessID;
            this.WatcherName = _fileWatcher.WatcherName;
            this.WatcherPath = _fileWatcher.WatcherPath;
            this.WatcherID = _fileWatcher.WatcherID;
            this.WatchEvent = _fileWatchEvent;

            Initialize();
        }

        private void Initialize()
        {
            this.FileSystemWatcher = new FileSystemWatcherControl(this.WatcherID, this.WatcherPath);
            this.FileSystemWatcher.OnFileCreateEvent = this.OnFileCreateEvent;
            this.FileSystemWatcher.OnFileCreateCompleteEvent = this.OnFileCreateCompleteEvent;
            this.FileSystemWatcher.OnFileDeletedEvent = this.OnFileDeletedEvent;
            this.FileSystemWatcher.OnFileRenamedEvent = this.OnFileRenamedEvent;
            this.FileSystemWatcher.OnFileChangedEvent = this.OnFileChangedEvent;
        }

        private void OnFileCreateEvent(string _fileName, string _fullPath)
        {
            if (!this.Enable) return;
        }

        private void OnFileCreateCompleteEvent(string _fileName, string _fullPath)
        {
            if (!this.Enable) return;
        }

        private void OnFileDeletedEvent(string _fileName, string _fullPath)
        {
            if (!this.Enable) return;
        }

        private void OnFileRenamedEvent(string _fileName, string _fullPath)
        {
            if (!this.Enable) return;
        }

        private void OnFileChangedEvent(string _fileName, string _fullPath)
        {
            if (!this.Enable) return;
        }

    }
}
