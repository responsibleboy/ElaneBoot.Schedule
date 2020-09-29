using ElaneBoot.Schedule.Models;
using ElaneBoot.Schedule.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElaneBoot.Schedule.Controllers
{
    public class ScheduleController : ApiController
    {
        private readonly ScheduleService _scheduleService;

        public ScheduleController(ScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet]
        public ScheduleJobInfo GetJob(string jobId)
        {
            return _scheduleService.GetJob(jobId);
        }

        [HttpGet]
        public IList<ScheduleJobInfo> GetJobList()
        {
            return _scheduleService.GetJobList();
        }

        [HttpPut]
        public ScheduleJobInfo AddJob(ScheduleJobInfo dto)
        {
            return _scheduleService.AddJob(dto);
        }

        [HttpDelete]
        public void DeleteJob(string jobId)
        {
            _scheduleService.DeleteJob(jobId);
        }

        [HttpPost]
        public ScheduleJobInfo UpdateJob(ScheduleJobInfo dto)
        {
            return _scheduleService.UpdateJob(dto);
        }

        [HttpPost]
        public void StartJob(string jobId)
        {
            _scheduleService.StartJob(jobId);
        }

        [HttpPost]
        public void StopJob(string jobId)
        {
            _scheduleService.StopJob(jobId);
        }

        [HttpGet]
        public IList<ScheduleLogInfo> GetLogList(string jobId, string statusCode)
        {
            return _scheduleService.GetLogList(jobId, statusCode);
        }

        [HttpGet]
        public PageData<ScheduleLogInfo> GetLogPage(string jobId, string statusCode, int pageIndex, int pageSize)
        {
            return _scheduleService.GetLogPage(jobId, statusCode, pageIndex, pageSize);
        }
    }
}
