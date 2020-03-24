using Quartz;
using System.Threading.Tasks;

namespace Service.Job.ProcessingCenter.Interface
{
    public interface IProcessJob
    {
        Task RunAsync(IJobExecutionContext context);
    }
}