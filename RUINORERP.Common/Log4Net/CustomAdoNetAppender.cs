using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Common.Log4Net
{
    using log4net.Appender;

    using System.Configuration;

    namespace DataStatisticsApi.Log
    {

        /// <summary>
        /// 到时加密时会用到
        /// </summary>
        public class CustomAdoNetAppender : AdoNetAppender
        {
            public new string ConnectionString
            {
                set
                {
                    this.ConnectionString = ConfigurationManager.ConnectionStrings[value].ConnectionString;
                }
            }
            public new string ConnectionStringName
            {
                set
                {
                    this.ConnectionString = ConfigurationManager.ConnectionStrings[value].ConnectionString;
                }
            }
        }

    }
}
