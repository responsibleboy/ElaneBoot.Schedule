using System;
using System.Collections.Generic;
using System.Text;

namespace ElaneBoot.Schedule.Models
{
    public class ScheduleLogInfo
    {
        public string LogId { get; set; }
        public string JobId { get; set; }
        public string JobName { get; set; }
        public string RequestUrl { get; set; }
        public string RequestMethod { get; set; }
        public string RequestParam { get; set; }
        public string StatusCode { get; set; }
        public long? ExecuteTime { get; set; }
        public string Result { get; set; }
        public string Exception { get; set; }
        public string DateTime { get; set; }
    }
}
