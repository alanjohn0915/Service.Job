using System;
using Quartz;
using log4net;
using System.Threading.Tasks;
using Service.Job.ProcessingCenter.Interface;

namespace Service.Job.ProcessingCenter
{
    public class RegularProcessJob : IProcessJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(RegularProcessJob));
        public async Task RunAsync(IJobExecutionContext context)
        {
            await Task.Run(() =>
            {
                for (var i = 0; i < 10; i++)
                {
                    Console.WriteLine($"Start work task{i}");
                }
            });
        }
    }
}