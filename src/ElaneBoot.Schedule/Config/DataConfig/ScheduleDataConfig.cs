using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WangSql;

namespace ElaneBoot.Schedule.Config.Data
{
    public class ScheduleDataConfig : IDataConfig
    {
        public ScheduleDataConfig()
        {
            //Fluent API
            var map = TableMap.Entity<Models.ScheduleJobInfo>().ToTable("tb_schedule_job", "定时任务", true);
            map.HasColumn(x => x.JobId).Name("JobId").Length(50).Comment("ID").IsPrimaryKey().IsNotNull();
            map.HasColumn(x => x.JobName).Name("JobName").Length(50).Comment("任务名称").IsUnique().IsNotNull();
            map.HasColumn(x => x.JobType).Name("JobType").Length(50).Comment("任务类型");
            map.HasColumn(x => x.JobDesc).Name("JobDesc").Length(50).Comment("任务描述");
            map.HasColumn(x => x.StartDateTime).Name("StartDateTime").Length(50).Comment("开始时间");
            map.HasColumn(x => x.EndDateTime).Name("EndDateTime").Length(50).Comment("结束时间");
            map.HasColumn(x => x.Cron).Name("Cron").Length(50).Comment("Cron表达式");
            map.HasColumn(x => x.RequestUrl).Name("RequestUrl").Length(50).Comment("请求地址");
            map.HasColumn(x => x.RequestMethod).Name("RequestMethod").Length(50).Comment("请求方式");
            map.HasColumn(x => x.JobState).Name("JobState").Length(50).Comment("任务状态");

        }
    }
}
