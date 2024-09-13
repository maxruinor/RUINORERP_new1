
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:33
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
    /// 审计日志表
    /// </summary>
    [Serializable()]
    [Description("tb_AuditLogs")]
    [SugarTable("tb_AuditLogs")]
    public partial class tb_AuditLogs: BaseEntity, ICloneable
    {
        public tb_AuditLogs()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_AuditLogs" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Audit_ID;
        /// <summary>
        /// 审计日志
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Audit_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "审计日志" , IsPrimaryKey = true)]
        public long Audit_ID
        { 
            get{return _Audit_ID;}
            set{
            base.PrimaryKeyID = _Audit_ID;
            SetProperty(ref _Audit_ID, value);
            }
        }

        private long? _Employee_ID;
        /// <summary>
        /// 员工信息
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "员工信息")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "员工信息" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
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

        private string _Notes;
        /// <summary>
        /// 备注说明
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注说明")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=100,IsNullable = true,ColumnDescription = "备注说明" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






        #region 字段描述对应列表
        private ConcurrentDictionary<string, string> fieldNameList;


        /// <summary>
        /// 表列名的中文描述集合
        /// </summary>
        [Description("列名中文描述"), Category("自定属性")]
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public override ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, string>();
                    SugarColumn entityAttr;
                    Type type = typeof(tb_AuditLogs);
                    
                       foreach (PropertyInfo field in type.GetProperties())
                            {
                                foreach (Attribute attr in field.GetCustomAttributes(true))
                                {
                                    entityAttr = attr as SugarColumn;
                                    if (null != entityAttr)
                                    {
                                        if (entityAttr.ColumnDescription == null)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.IsIdentity)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.IsPrimaryKey)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.ColumnDescription.Trim().Length > 0)
                                        {
                                            fieldNameList.TryAdd(field.Name, entityAttr.ColumnDescription);
                                        }
                                    }
                                }
                            }
                }
                
                return fieldNameList;
            }
            set
            {
                fieldNameList = value;
            }

        }
        #endregion
        

        public override object Clone()
        {
            tb_AuditLogs loctype = (tb_AuditLogs)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

