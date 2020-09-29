using ElaneBoot.Schedule.Models;
using System;
using System.Collections.Generic;
using System.Text;
using WangSql;

namespace ElaneBoot.Schedule.Services
{
    public class ScheduleService : ApiService
    {
        public ScheduleJobInfo GetJob(string jobId)
        {
            var r = SqlMapper.Entity<Models.ScheduleJobInfo>().Where(x => x.JobId == jobId).FirstOrDefault();
            return r;
        }

        public IList<ScheduleJobInfo> GetJobList()
        {
            var r = SqlMapper.Entity<Models.ScheduleJobInfo>().ToList();
            return r;
        }

        public ScheduleJobInfo AddJob(ScheduleJobInfo dto)
        {
            var model = dto;
            model.JobId = Guid.NewGuid().ToString("N").ToLower();

            using (var trans = SqlMapper.BeginTransaction())
            {
                try
                {
                    trans.Entity<Models.ScheduleJobInfo>().Insert(model);
                    ScheduleManager.AddJob(model);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }

            return GetJob(model.JobId);
        }

        public void DeleteJob(string jobId)
        {
            var job = SqlMapper.Entity<Models.ScheduleJobInfo>().Where(x => x.JobId == jobId).FirstOrDefault();
            using (var trans = SqlMapper.BeginTransaction())
            {
                try
                {
                    job.JobState = JobState.Deleted;
                    trans.Entity<Models.ScheduleJobInfo>().Update(job);
                    ScheduleManager.DeleteJob(jobId);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public ScheduleJobInfo UpdateJob(ScheduleJobInfo dto)
        {
            var model = dto;

            using (var trans = SqlMapper.BeginTransaction())
            {
                try
                {
                    trans.Entity<Models.ScheduleJobInfo>().Set(x => new { x.JobState }, false).Update(model);
                    ScheduleManager.UpdateJob(model);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }

            return GetJob(model.JobId);
        }

        public void StartJob(string jobId)
        {
            var job = SqlMapper.Entity<Models.ScheduleJobInfo>().Where(x => x.JobId == jobId).FirstOrDefault();
            using (var trans = SqlMapper.BeginTransaction())
            {
                try
                {
                    job.JobState = JobState.Normal;
                    trans.Entity<Models.ScheduleJobInfo>().Update(job);
                    ScheduleManager.AddJob(job);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public void StopJob(string jobId)
        {
            var job = SqlMapper.Entity<Models.ScheduleJobInfo>().Where(x => x.JobId == jobId).FirstOrDefault();
            using (var trans = SqlMapper.BeginTransaction())
            {
                try
                {
                    job.JobState = JobState.Disabled;
                    trans.Entity<Models.ScheduleJobInfo>().Update(job);
                    ScheduleManager.DeleteJob(jobId);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public IList<ScheduleLogInfo> GetLogList(string jobId, string statusCode)
        {
            var list = SqlMapper.Entity<ScheduleLogInfo>();
            if (!string.IsNullOrEmpty(jobId))
            {
                list.Where(x => x.JobId == jobId);
            }
            if (!string.IsNullOrEmpty(statusCode))
            {
                list.Where(x => x.StatusCode == statusCode);
            }
            return list.OrderByDescending(x => x.DateTime).ToList();
        }

        public PageData<ScheduleLogInfo> GetLogPage(string jobId, string statusCode, int pageIndex, int pageSize)
        {
            var pageInfo = new PageInfo();
            if (pageIndex > 0) pageInfo.PageIndex = pageIndex;
            if (pageSize > 0) pageInfo.PageSize = pageSize;

            var list = SqlMapper.Entity<ScheduleLogInfo>();
            if (!string.IsNullOrEmpty(jobId))
            {
                list.Where(x => x.JobId == jobId);
            }
            if (!string.IsNullOrEmpty(statusCode))
            {
                list.Where(x => x.StatusCode == statusCode);
            }

            PageData<ScheduleLogInfo> result = new PageData<ScheduleLogInfo>();
            var r = list.OrderByDescending(x => x.DateTime).ToPaged(pageInfo.PageIndex, pageInfo.PageSize, out int total);
            result.Items = r;
            result.PageInfo = pageInfo;
            result.PageInfo.TotalCount = total;
            return result;
        }
    }
}
