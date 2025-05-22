using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.Dto
{
    /// <summary>
    /// 用户授权用的中间实体 暂时作废2023-12-27
    /// </summary>
    [Obsolete]
    public class SelectDto
    {

        /// <summary>
        /// 品号
        /// </summary>
        [SugarColumn(ColumnName = "UserSelect", ColumnDescription = "授权")]
        public bool UserSelect { get; set; }

     

    }
}
