using log4net;
using System;
using Service.Job.Service;
using System.IO;
using Service.Job.Jobs;
using Topshelf;

namespace Service.Job
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LogManager.GetLogger(typeof(Program));
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
            try
            {
                HostFactory.Run(x => {
                    x.UseLog4Net();
                    x.SetServiceName("Service.Job");
                    x.SetDisplayName("Service.Job");
                    x.SetDescription("Service.Job后端任务处理");
                    x.Service<ServiceRunner>();
                    x.EnablePauseAndContinue();
                });
            }
            catch (Exception e)
            {
                logger.Error($"Message：Start failure,DateTime:{DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss")}", e);
            }
        }
    }
}
