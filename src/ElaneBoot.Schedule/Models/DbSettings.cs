using System;
using System.Collections.Generic;
using System.Text;

namespace ElaneBoot.Schedule.Models
{
    public class DbSettings
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string ConnectionType { get; set; }
        public bool UseParameterPrefixInSql { get; set; }
        public bool UseParameterPrefixInParameter { get; set; }
        public string ParameterPrefix { get; set; }
        public bool UseQuotationInSql { get; set; }
        public bool Debug { get; set; }
    }
}
