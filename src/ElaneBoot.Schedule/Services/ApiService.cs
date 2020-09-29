using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using WangSql;

namespace ElaneBoot.Schedule.Services
{
    public abstract class ApiService
    {
        private ISqlMapper sqlMapper;

        public ISqlMapper SqlMapper
        {
            get
            {
                return this.sqlMapper;
            }
        }

        public ApiService()
        {
            sqlMapper = new SqlMapper();
        }
    }
}
