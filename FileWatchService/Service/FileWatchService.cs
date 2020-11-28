using FileWatchService.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FileWatchService
{
    public partial class FileWatchService : ServiceBase
    {
        private MainService mainService;

        public FileWatchService()
        {
            InitializeComponent();
            LogHelper.Logger.Trace(this.ServiceName + " Created.");
        }

        /// <summary>
        /// 啟動服務
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            try
            {
                LogHelper.Logger.Trace(this.ServiceName + " OnStart.");
                LogHelper.Logger.Trace("Create MainService.");
                //建立執行物件
                this.mainService = new MainService();

                LogHelper.Logger.Trace("Initial MainService.");
                //初始化執行物件
                this.mainService.Initialize();
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Fatal(ex, "Exception on OnStart()");
            }
        }

        /// <summary>
        /// 停止服務
        /// </summary>
        protected override void OnStop()
        {
            try
            {
                LogHelper.Logger.Trace(this.ServiceName + " OnStop.");
                LogHelper.Logger.Trace("Dispose MainService.");
                this.mainService.Dispose();
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Fatal(ex, "Exception on OnStop()");
            }
        }
    }
}
