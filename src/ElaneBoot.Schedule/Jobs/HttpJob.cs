using ElaneBoot.Schedule.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WangSql;

namespace ElaneBoot.Schedule.Jobs
{
    public class HttpJob : IJob
    {
        private readonly ISqlMapper _sqlMapper;

        public HttpJob()
        {
            _sqlMapper = new SqlMapper();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var job = _sqlMapper.Entity<ScheduleJobInfo>().Where(x => x.JobId == context.JobDetail.Key.Name).FirstOrDefault();
            if (job == null || job.JobState != JobState.Normal) return;
            if (job.StartDateTime != null && job.StartDateTime > DateTime.Now) return;
            if (job.EndDateTime != null && job.EndDateTime < DateTime.Now) return;

            var dateTime = DateTime.Now;
            var StatusCode = "200";
            string Result = null;
            string Exception = null;
            try
            {
                string r = null;
                switch (job.RequestMethod.ToLower())
                {
                    case "get":
                        {
                            r = HttpHelper.GetStringAsync(job.RequestUrl);
                        }
                        break;
                    case "post":
                        {
                            r = HttpHelper.PostAsync(job.RequestUrl, job.RequestParam);
                        }
                        break;
                    case "put":
                        {
                            r = HttpHelper.PutAsync(job.RequestUrl, job.RequestParam);
                        }
                        break;
                    case "delete":
                        {
                            r = HttpHelper.DeleteAsync(job.RequestUrl);
                        }
                        break;
                }
                Result = r;
            }
            catch (Exception ex)
            {
                StatusCode = "500";
                Exception = ex.Message;
            }

            //记录日志
            _sqlMapper.Entity<ScheduleLogInfo>().Insert(new ScheduleLogInfo()
            {
                LogId = Guid.NewGuid().ToString("N").ToLower(),
                JobId = job.JobId,
                JobName = job.JobName,
                RequestUrl = job.RequestUrl,
                RequestMethod = job.RequestMethod,
                RequestParam = job.RequestParam,
                StatusCode = StatusCode,
                ExecuteTime = (long)(DateTime.Now - dateTime).TotalMilliseconds,
                Result = Result,
                Exception = Exception,
                DateTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }
    }
}
