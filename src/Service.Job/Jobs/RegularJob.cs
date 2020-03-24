using System;
using Quartz;
using log4net;
using Service.Job.Service;
using Service.Job.ProcessingCenter;

using System.Threading.Tasks;

namespace Service.Job.Jobs
{
    public class RegularJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(RegularJob));
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.Info($"Message：{nameof(RegularJob)} Starting,DateTime:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                await new RegularProcessJob().RunAsync(context);
                _logger.Info($"Message：{nameof(RegularJob)} Complated,DateTime:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            }
            catch (Exception e)
            {
                _logger.Error($"Message：Error，Reexecution will be delayed by {ServiceRunner.RestartTime}minutes", e);
                //转换为可调度异常
                var jobException = new JobExecutionException(e);
                //延迟指定时间
                await Task.Delay(TimeSpan.FromMinutes(ServiceRunner.RestartTime), context.CancellationToken);
                // 立即重新执行
                jobException.RefireImmediately = true;
                throw jobException;
            }
        }
    }
}
