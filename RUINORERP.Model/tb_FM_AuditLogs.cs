
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/27/2025 18:00:25
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{
    /// <summary>
    /// 财务审计日志
    /// </summary>
    [Serializable()]
    [Description("财务审计日志")]
    [SugarTable("tb_FM_AuditLogs")]
    public partial class tb_FM_AuditLogs: BaseEntity, ICloneable
    {
        public tb_FM_AuditLogs()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("财务审计日志tb_FM_AuditLogs" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _FMAudit_ID;
        /// <summary>
        /// 审计日志
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "FMAudit_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "审计日志" , IsPrimaryKey = true)]
        public long FMAudit_ID
        { 
            get{return _FMAudit_ID;}
            set{
            SetProperty(ref _FMAudit_ID, value);
                base.PrimaryKeyID = _FMAudit_ID;
            }
        }

        private long? _Employee_ID;
        /// <summary>
        /// 员工信息
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "员工信息")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "员工信息" )]
        public long? Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
                        }
        }

        private string _UserName;
        /// <summary>
        /// 用户名
        /// </summary>
        [AdvQueryAttribute(ColName = "UserName",ColDesc = "用户名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "UserName" ,Length=255,IsNullable = false,ColumnDescription = "用户名" )]
        public string UserName
        { 
            get{return _UserName;}
            set{
            SetProperty(ref _UserName, value);
                        }
        }

        private DateTime? _ActionTime;
        /// <summary>
        /// 发生时间
        /// </summary>
        [AdvQueryAttribute(ColName = "ActionTime",ColDesc = "发生时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "ActionTime" ,IsNullable = true,ColumnDescription = "发生时间" )]
        public DateTime? ActionTime
        { 
            get{return _ActionTime;}
            set{
            SetProperty(ref _ActionTime, value);
                        }
        }

        private string _ActionType;
        /// <summary>
        /// 动作
        /// </summary>
        [AdvQueryAttribute(ColName = "ActionType",ColDesc = "动作")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ActionType" ,Length=50,IsNullable = true,ColumnDescription = "动作" )]
        public string ActionType
        { 
            get{return _ActionType;}
            set{
            SetProperty(ref _ActionType, value);
                        }
        }

        private int? _ObjectType;
        /// <summary>
        /// 单据类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ObjectType",ColDesc = "单据类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ObjectType" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "单据类型" )]
        public int? ObjectType
        { 
            get{return _ObjectType;}
            set{
            SetProperty(ref _ObjectType, value);
                        }
        }

        private long? _ObjectId;
        /// <summary>
        /// 单据ID
        /// </summary>
        [AdvQueryAttribute(ColName = "ObjectId",ColDesc = "单据ID")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ObjectId" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "单据ID" )]
        public long? ObjectId
        { 
            get{return _ObjectId;}
            set{
            SetProperty(ref _ObjectId, value);
                        }
        }

        private string _ObjectNo;
        /// <summary>
        /// 单据编号
        /// </summary>
        [AdvQueryAttribute(ColName = "ObjectNo",ColDesc = "单据编号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ObjectNo" ,Length=50,IsNullable = true,ColumnDescription = "单据编号" )]
        public string ObjectNo
        { 
            get{return _ObjectNo;}
            set{
            SetProperty(ref _ObjectNo, value);
                        }
        }

        private string _OldState;
        /// <summary>
        /// 操作前状态
        /// </summary>
        [AdvQueryAttribute(ColName = "OldState",ColDesc = "操作前状态")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "OldState" ,Length=100,IsNullable = true,ColumnDescription = "操作前状态" )]
        public string OldState
        { 
            get{return _OldState;}
            set{
            SetProperty(ref _OldState, value);
                        }
        }

        private string _NewState;
        /// <summary>
        /// 操作后状态
        /// </summary>
        [AdvQueryAttribute(ColName = "NewState",ColDesc = "操作后状态")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "NewState" ,Length=100,IsNullable = true,ColumnDescription = "操作后状态" )]
        public string NewState
        { 
            get{return _NewState;}
            set{
            SetProperty(ref _NewState, value);
                        }
        }

        private string _DataContent;
        /// <summary>
        /// 审计日志
        /// </summary>
        [AdvQueryAttribute(ColName = "DataContent",ColDesc = "审计日志")] 
        [SugarColumn(ColumnDataType = "text", SqlParameterDbType ="String",  ColumnName = "DataContent" ,Length=2147483647,IsNullable = true,ColumnDescription = "审计日志" )]
        public string DataContent
        { 
            get{return _DataContent;}
            set{
            SetProperty(ref _DataContent, value);
                        }
        }

        private string _Notes;
        /// <summary>
        /// 备注说明
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注说明")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=8000,IsNullable = true,ColumnDescription = "备注说明" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
                        }
        }

        #endregion

        #region 扩展属性


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_FM_AuditLogs loctype = (tb_FM_AuditLogs)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

