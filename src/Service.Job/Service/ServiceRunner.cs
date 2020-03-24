using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Quartz;
using Quartz.Impl;
using Quartz.Util;
using Topshelf;

namespace Service.Job.Service
{
    public class ServiceRunner : ServiceControl, ServiceSuspend
    {
        //Delayed start time
        public static int RestartTime;
        private readonly IScheduler _scheduler;
        private readonly ILog _logger = LogManager.GetLogger(typeof(ServiceRunner));

        public ServiceRunner()
        {
            try
            {
                RestartTime = ConfigurationManager.AppSettings["RestartTime"].Trim(' ').StringToInt32(1);
                if (RestartTime < 1) RestartTime = 1;
                var properties = GetProperties();
                var stdSchedulerFactory = new StdSchedulerFactory(properties);
                _scheduler = stdSchedulerFactory.GetScheduler().Result;
            }
            catch (Exception e)
            {
                _logger.Error($"Message：ServiceRunner initialization failure,Error Message:{e.Message},DateTime:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}", e);
                throw;
            }
        }
        /// <summary>
        /// Read the Quartz.Net configuration from the config file
        /// </summary>
        /// <returns></returns>
        public NameValueCollection GetProperties()
        {
            try
            {
                var quartzFile = ConfigurationManager.AppSettings["QuartzFile"].Trim(' ');
                var requestedFile = QuartzEnvironment.GetEnvironmentVariable(quartzFile);
                var propFileName = !string.IsNullOrWhiteSpace(requestedFile) ? requestedFile : $"~/{quartzFile}";
                propFileName = FileUtil.ResolveFile(propFileName);
                var propertiesParser = PropertiesParser.ReadFromFileResource(propFileName);
                var properties = propertiesParser.UnderlyingProperties;
                return properties;
            }
            catch (Exception e)
            {
                _logger.Error($"Message：ServiceRunner Reading the Quartz config file failed,Error Message:{e.Message},DateTime:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}", e);
            }
            return new NameValueCollection();
        }
        public bool Start(HostControl hostControl)
        {
            try
            {
                _scheduler.Start();
            }
            catch (Exception e)
            {
                _logger.Error($"Message：ServiceRunner start failure,Error Message: {e.Message},DateTime:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}", e);
            }
            _logger.Info($"Message：ServiceRunner start success,DateTime:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            try
            {
                _scheduler.Shutdown(false);
            }
            catch (Exception e)
            {
                _logger.Error($"Message：ServiceRunner stop failure:{e.Message},DateTime:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}", e);
            }
            _logger.Info($"Message：ServiceRunner stop success,DateTime:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            return true;
        }

        public bool Continue(HostControl hostControl)
        {
            _scheduler.ResumeAll();
            return true;
        }

        public bool Pause(HostControl hostControl)
        {
            _scheduler.PauseAll();
            return true;
        }
    }
}
