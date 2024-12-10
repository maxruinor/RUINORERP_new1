
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/10/2024 13:12:13
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
    /// 跟进计划表
    /// </summary>
    [Serializable()]
    [Description("跟进计划表")]
    [SugarTable("tb_CRM_FollowUpPlans")]
    public partial class tb_CRM_FollowUpPlans: BaseEntity, ICloneable
    {
        public tb_CRM_FollowUpPlans()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("跟进计划表tb_CRM_FollowUpPlans" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PlanID;
        /// <summary>
        /// 跟进计划
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PlanID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "跟进计划" , IsPrimaryKey = true)]
        public long PlanID
        { 
            get{return _PlanID;}
            set{
            base.PrimaryKeyID = _PlanID;
            SetProperty(ref _PlanID, value);
            }
        }

        private long? _Customer_id;
        /// <summary>
        /// 目标客户
        /// </summary>
        [AdvQueryAttribute(ColName = "Customer_id",ColDesc = "目标客户")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Customer_id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "目标客户" )]
        [FKRelationAttribute("tb_CRM_Customer","Customer_id")]
        public long? Customer_id
        { 
            get{return _Customer_id;}
            set{
            SetProperty(ref _Customer_id, value);
            }
        }

        private long? _Employee_ID;
        /// <summary>
        /// 执行人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "执行人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "执行人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long? Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
            }
        }

        private DateTime _PlanStartDate;
        /// <summary>
        /// 开始日期
        /// </summary>
        [AdvQueryAttribute(ColName = "PlanStartDate",ColDesc = "开始日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "PlanStartDate" ,IsNullable = false,ColumnDescription = "开始日期" )]
        public DateTime PlanStartDate
        { 
            get{return _PlanStartDate;}
            set{
            SetProperty(ref _PlanStartDate, value);
            }
        }

        private DateTime _PlanEndDate;
        /// <summary>
        /// 结束日期
        /// </summary>
        [AdvQueryAttribute(ColName = "PlanEndDate",ColDesc = "结束日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "PlanEndDate" ,IsNullable = false,ColumnDescription = "结束日期" )]
        public DateTime PlanEndDate
        { 
            get{return _PlanEndDate;}
            set{
            SetProperty(ref _PlanEndDate, value);
            }
        }

        private int? _PlanStatus;
        /// <summary>
        /// 计划状态
        /// </summary>
        [AdvQueryAttribute(ColName = "PlanStatus",ColDesc = "计划状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "PlanStatus" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "计划状态" )]
        public int? PlanStatus
        { 
            get{return _PlanStatus;}
            set{
            SetProperty(ref _PlanStatus, value);
            }
        }

        private string _PlanSubject;
        /// <summary>
        /// 计划主题
        /// </summary>
        [AdvQueryAttribute(ColName = "PlanSubject",ColDesc = "计划主题")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PlanSubject" ,Length=200,IsNullable = true,ColumnDescription = "计划主题" )]
        public string PlanSubject
        { 
            get{return _PlanSubject;}
            set{
            SetProperty(ref _PlanSubject, value);
            }
        }

        private string _PlanContent;
        /// <summary>
        /// 计划内容
        /// </summary>
        [AdvQueryAttribute(ColName = "PlanContent",ColDesc = "计划内容")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PlanContent" ,Length=1000,IsNullable = true,ColumnDescription = "计划内容" )]
        public string PlanContent
        { 
            get{return _PlanContent;}
            set{
            SetProperty(ref _PlanContent, value);
            }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=255,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
            }
        }

        private DateTime? _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Created_at" ,IsNullable = true,ColumnDescription = "创建时间" )]
        public DateTime? Created_at
        { 
            get{return _Created_at;}
            set{
            SetProperty(ref _Created_at, value);
            }
        }

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Created_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "创建人" )]
        public long? Created_by
        { 
            get{return _Created_by;}
            set{
            SetProperty(ref _Created_by, value);
            }
        }

        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Modified_at" ,IsNullable = true,ColumnDescription = "修改时间" )]
        public DateTime? Modified_at
        { 
            get{return _Modified_at;}
            set{
            SetProperty(ref _Modified_at, value);
            }
        }

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Modified_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "修改人" )]
        public long? Modified_by
        { 
            get{return _Modified_by;}
            set{
            SetProperty(ref _Modified_by, value);
            }
        }

        private bool? _isdeleted= false;
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "isdeleted" ,IsNullable = true,ColumnDescription = "逻辑删除" )]
        [Browsable(false)]
        public bool? isdeleted
        { 
            get{return _isdeleted;}
            set{
            SetProperty(ref _isdeleted, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Customer_id))]
        public virtual tb_CRM_Customer tb_crm_customer { get; set; }


        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_CRM_FollowUpRecords.PlanID))]
        public virtual List<tb_CRM_FollowUpRecords> tb_CRM_FollowUpRecordses { get; set; }
        //tb_CRM_FollowUpRecords.PlanID)
        //PlanID.FK_CRM_FOLLOWUPRECORDS_REF_FOLLOWUPPLANS)
        //tb_CRM_FollowUpPlans.PlanID)


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
                    Type type = typeof(tb_CRM_FollowUpPlans);
                    
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
            tb_CRM_FollowUpPlans loctype = (tb_CRM_FollowUpPlans)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

