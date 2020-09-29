using System;
using System.Collections.Generic;
using System.Text;

namespace ElaneBoot.Schedule.Models
{
    public enum JobType
    {
        Plan,
        Queue
    }
    public enum JobState
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal,
        /// <summary>
        /// 已完成
        /// </summary>
        Finished,
        /// <summary>
        /// 已禁用
        /// </summary>
        Disabled,
        /// <summary>
        /// 已删除
        /// </summary>
        Deleted
    }
    public class ScheduleJobInfo
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public string JobId { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string JobName { get; set; }
        /// <summary>
        /// 任务类型
        /// </summary>
        public JobType JobType { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string JobDesc { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDateTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDateTime { get; set; }
        /// <summary>
        /// Cron表达式
        /// </summary>
        public string Cron { get; set; }
        /// <summary>
        /// 请求url
        /// </summary>
        public string RequestUrl { get; set; }
        /// <summary>
        /// 请求方式
        /// </summary>
        public string RequestMethod { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        public string RequestParam { get; set; }
        /// <summary>
        /// 任务状态
        /// </summary>
        public JobState JobState { get; set; }
    }
}
