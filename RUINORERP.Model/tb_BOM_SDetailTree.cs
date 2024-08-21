
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/14/2024 15:01:02
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{

    public partial class tb_BOM_SDetailTree : tb_BOM_SDetail
    {
        public tb_BOM_SDetailTree()
        {
        }


        private long _ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ID", ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "ID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "")]
        public long ID
        {
            get { return _ID; }
            set
            {
                SetProperty(ref _ID, value);
            }
        }

        private long _ParentId;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "ParentId", DecimalDigits = 0, IsNullable = false, ColumnDescription = "")]
        public long ParentId
        {
            get { return _ParentId; }
            set
            {
                SetProperty(ref _ParentId, value);
            }
        }
    }

}
 

