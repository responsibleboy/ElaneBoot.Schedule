using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WangSql;

namespace ElaneBoot.Schedule.Config.Data
{
    public class ScheduleLogDataConfig : IDataConfig
    {
        public ScheduleLogDataConfig()
        {
            //Fluent API
            var map = TableMap.Entity<Models.ScheduleLogInfo>().ToTable("tb_schedule_log", "定时任务日志", true);
            map.HasColumn(x => x.LogId).Name("LogId").Length(50).Comment("ID").IsPrimaryKey().IsNotNull();
            map.HasColumn(x => x.JobId).Name("JobId").Length(50).Comment("任务ID");
            map.HasColumn(x => x.JobName).Name("JobName").Length(50).Comment("任务名称");
            map.HasColumn(x => x.RequestUrl).Name("RequestUrl").Length(50).Comment("请求地址");
            map.HasColumn(x => x.RequestMethod).Name("RequestMethod").Length(50).Comment("请求方式");
            map.HasColumn(x => x.RequestParam).Name("RequestParam").Length(200).Comment("请求参数");
            map.HasColumn(x => x.StatusCode).Name("StatusCode").Length(50).Comment("状态码");
            map.HasColumn(x => x.ExecuteTime).Name("ExecuteTime").Length(18).Comment("执行时长");
            map.HasColumn(x => x.Result).Name("Result").DataType(SimpleStandardType.Text).Comment("结果");
            map.HasColumn(x => x.Exception).Name("Exception").DataType(SimpleStandardType.Text).Comment("异常");
            map.HasColumn(x => x.DateTime).Name("DateTime").Length(50).Comment("请求时间");

        }
    }
}
