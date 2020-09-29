using ElaneBoot.Schedule.Jobs;
using ElaneBoot.Schedule.Models;
using ElaneBoot.Schedule.Services;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WangSql;

namespace ElaneBoot.Schedule
{
    public class ScheduleManager
    {
        private readonly static IScheduler _scheduler;
        private readonly static ISqlMapper _sqlMapper;

        static ScheduleManager()
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            _scheduler = sf.GetScheduler().Result;

            _sqlMapper = new SqlMapper();
        }

        //public static IJobDetail GetJobDetail(string jobId)
        //{
        //    var jobKey = JobKey.Create(jobId);
        //    return _scheduler.GetJobDetail(jobKey).Result;
        //}

        public static void AddJob(ScheduleJobInfo job)
        {
            UpdateOrAddJob(job);
        }

        public static void UpdateJob(ScheduleJobInfo job)
        {
            UpdateOrAddJob(job);
        }

        //public static void PauseJob(string jobId)
        //{
        //    var jobKey = JobKey.Create(jobId);
        //    _scheduler.PauseJob(jobKey).Wait();
        //}

        //public static void ResumeJob(string jobId)
        //{
        //    var jobKey = JobKey.Create(jobId);
        //    _scheduler.ResumeJob(jobKey).Wait();
        //}

        public static void TriggerJob(string jobId)
        {
            var jobKey = JobKey.Create(jobId);
            _scheduler.TriggerJob(jobKey).Wait();
        }

        public static void DeleteJob(string jobId)
        {
            var jobKey = JobKey.Create(jobId);
            _scheduler.DeleteJob(jobKey).Wait();
        }

        public static void Start()
        {
            _sqlMapper.Migrate.Run();

            var jobs = _sqlMapper.Entity<ScheduleJobInfo>().ToList();
            foreach (var item in jobs)
            {
                AddJob(item);
            }
            _scheduler.Start().Wait();
        }

        public static void Shutdown()
        {
            _scheduler.Shutdown().Wait();
        }








        private static void UpdateOrAddJob(ScheduleJobInfo job)
        {
            if (CheckExists(job.JobId))
                DeleteJob(job.JobId);

            if (job.JobState == JobState.Normal)
            {
                var jobDetail = JobBuilder.Create<HttpJob>()
                    .WithIdentity(job.JobId)
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                   .WithIdentity(job.JobId)
                   .WithCronSchedule(job.Cron)
                   .Build();

                ScheduleJob(jobDetail, trigger);
            }
        }
        private static void ScheduleJob(IJobDetail job, ITrigger trigger)
        {
            _scheduler.ScheduleJob(job, trigger).Wait();
        }
        private static bool CheckExists(string jobId)
        {
            var jobKey = JobKey.Create(jobId);
            return _scheduler.CheckExists(jobKey).Result;
        }

    }
}
