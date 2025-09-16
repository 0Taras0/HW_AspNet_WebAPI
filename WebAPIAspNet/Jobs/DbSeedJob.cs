using Core.Interfaces;
using Quartz;

namespace WebAPIAspNet.Jobs
{
    public class DbSeedJob(IDbSeederService dbSeederService) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await dbSeederService.SeedData();
        }
    }
}
