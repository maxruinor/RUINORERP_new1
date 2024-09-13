
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:23
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model.Base;

namespace RUINORERP.Model.QueryDto
{
    /// <summary>
    /// 考勤表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Attendance")]
    public partial class tb_AttendanceQueryDto:BaseEntityDto
    {
        public tb_AttendanceQueryDto()
        {

        }

    
     

        private string _badgenumber;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "badgenumber",ColDesc = "")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "badgenumber",Length=30,IsNullable = true,ColumnDescription = "" )]
        public string badgenumber 
        { 
            get{return _badgenumber;}
            set{SetProperty(ref _badgenumber, value);}
        }
     

        private string _username;
        /// <summary>
        /// 姓名
        /// </summary>
        [AdvQueryAttribute(ColName = "username",ColDesc = "姓名")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "username",Length=50,IsNullable = true,ColumnDescription = "姓名" )]
        public string username 
        { 
            get{return _username;}
            set{SetProperty(ref _username, value);}
        }
     

        private string _deptname;
        /// <summary>
        /// 部门
        /// </summary>
        [AdvQueryAttribute(ColName = "deptname",ColDesc = "部门")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "deptname",Length=60,IsNullable = true,ColumnDescription = "部门" )]
        public string deptname 
        { 
            get{return _deptname;}
            set{SetProperty(ref _deptname, value);}
        }
     

        private string _sDate;
        /// <summary>
        /// 开始时间
        /// </summary>
        [AdvQueryAttribute(ColName = "sDate",ColDesc = "开始时间")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "sDate",Length=100,IsNullable = true,ColumnDescription = "开始时间" )]
        public string sDate 
        { 
            get{return _sDate;}
            set{SetProperty(ref _sDate, value);}
        }
     

        private string _stime;
        /// <summary>
        /// 时间组
        /// </summary>
        [AdvQueryAttribute(ColName = "stime",ColDesc = "时间组")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "stime",Length=255,IsNullable = true,ColumnDescription = "时间组" )]
        public string stime 
        { 
            get{return _stime;}
            set{SetProperty(ref _stime, value);}
        }
     

        private DateTime? _eDate;
        /// <summary>
        /// 结束时间
        /// </summary>
        [AdvQueryAttribute(ColName = "eDate",ColDesc = "结束时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "eDate",IsNullable = true,ColumnDescription = "结束时间" )]
        public DateTime? eDate 
        { 
            get{return _eDate;}
            set{SetProperty(ref _eDate, value);}
        }
     

        private DateTime? _t1;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "t1",ColDesc = "")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "t1",IsNullable = true,ColumnDescription = "" )]
        public DateTime? t1 
        { 
            get{return _t1;}
            set{SetProperty(ref _t1, value);}
        }
     

        private DateTime? _t2;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "t2",ColDesc = "")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "t2",IsNullable = true,ColumnDescription = "" )]
        public DateTime? t2 
        { 
            get{return _t2;}
            set{SetProperty(ref _t2, value);}
        }
     

        private DateTime? _t3;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "t3",ColDesc = "")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "t3",IsNullable = true,ColumnDescription = "" )]
        public DateTime? t3 
        { 
            get{return _t3;}
            set{SetProperty(ref _t3, value);}
        }
     

        private DateTime? _t4;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "t4",ColDesc = "")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "t4",IsNullable = true,ColumnDescription = "" )]
        public DateTime? t4 
        { 
            get{return _t4;}
            set{SetProperty(ref _t4, value);}
        }


       
    }
}



