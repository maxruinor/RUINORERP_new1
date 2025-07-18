﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/06/2025 14:52:02
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
    /// 审计日志表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_AuditLogs")]
    public partial class tb_AuditLogsQueryDto:BaseEntityDto
    {
        public tb_AuditLogsQueryDto()
        {

        }

    
     

        private long? _Employee_ID;
        /// <summary>
        /// 员工信息
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "员工信息")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Employee_ID",IsNullable = true,ColumnDescription = "员工信息" )]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}
            set{SetProperty(ref _Employee_ID, value);}
        }
     

        private string _UserName;
        /// <summary>
        /// 用户名
        /// </summary>
        [AdvQueryAttribute(ColName = "UserName",ColDesc = "用户名")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "UserName",Length=255,IsNullable = false,ColumnDescription = "用户名" )]
        public string UserName 
        { 
            get{return _UserName;}
            set{SetProperty(ref _UserName, value);}
        }
     

        private DateTime? _ActionTime;
        /// <summary>
        /// 发生时间
        /// </summary>
        [AdvQueryAttribute(ColName = "ActionTime",ColDesc = "发生时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "ActionTime",IsNullable = true,ColumnDescription = "发生时间" )]
        public DateTime? ActionTime 
        { 
            get{return _ActionTime;}
            set{SetProperty(ref _ActionTime, value);}
        }
     

        private string _ActionType;
        /// <summary>
        /// 动作
        /// </summary>
        [AdvQueryAttribute(ColName = "ActionType",ColDesc = "动作")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ActionType",Length=50,IsNullable = true,ColumnDescription = "动作" )]
        public string ActionType 
        { 
            get{return _ActionType;}
            set{SetProperty(ref _ActionType, value);}
        }
     

        private int? _ObjectType;
        /// <summary>
        /// 单据类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ObjectType",ColDesc = "单据类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "ObjectType",IsNullable = true,ColumnDescription = "单据类型" )]
        public int? ObjectType 
        { 
            get{return _ObjectType;}
            set{SetProperty(ref _ObjectType, value);}
        }
     

        private long? _ObjectId;
        /// <summary>
        /// 单据ID
        /// </summary>
        [AdvQueryAttribute(ColName = "ObjectId",ColDesc = "单据ID")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ObjectId",IsNullable = true,ColumnDescription = "单据ID" )]
        public long? ObjectId 
        { 
            get{return _ObjectId;}
            set{SetProperty(ref _ObjectId, value);}
        }
     

        private string _ObjectNo;
        /// <summary>
        /// 单据编号
        /// </summary>
        [AdvQueryAttribute(ColName = "ObjectNo",ColDesc = "单据编号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ObjectNo",Length=50,IsNullable = true,ColumnDescription = "单据编号" )]
        public string ObjectNo 
        { 
            get{return _ObjectNo;}
            set{SetProperty(ref _ObjectNo, value);}
        }
     

        private string _OldState;
        /// <summary>
        /// 操作前状态
        /// </summary>
        [AdvQueryAttribute(ColName = "OldState",ColDesc = "操作前状态")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "OldState",Length=100,IsNullable = true,ColumnDescription = "操作前状态" )]
        public string OldState 
        { 
            get{return _OldState;}
            set{SetProperty(ref _OldState, value);}
        }
     

        private string _NewState;
        /// <summary>
        /// 操作后状态
        /// </summary>
        [AdvQueryAttribute(ColName = "NewState",ColDesc = "操作后状态")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "NewState",Length=100,IsNullable = true,ColumnDescription = "操作后状态" )]
        public string NewState 
        { 
            get{return _NewState;}
            set{SetProperty(ref _NewState, value);}
        }
     

        private string _DataContent;
        /// <summary>
        /// 数据内容
        /// </summary>
        [AdvQueryAttribute(ColName = "DataContent",ColDesc = "数据内容")]
        [SugarColumn(ColumnDataType = "text",SqlParameterDbType ="String",ColumnName = "DataContent",Length=2147483647,IsNullable = true,ColumnDescription = "数据内容" )]
        public string DataContent 
        { 
            get{return _DataContent;}
            set{SetProperty(ref _DataContent, value);}
        }
     

        private string _Notes;
        /// <summary>
        /// 备注说明
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注说明")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=8000,IsNullable = true,ColumnDescription = "备注说明" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
        }


       
    }
}



