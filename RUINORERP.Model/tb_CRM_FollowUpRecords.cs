
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:31:56
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
    /// 跟进记录表
    /// </summary>
    [Serializable()]
    [Description("跟进记录表")]
    [SugarTable("tb_CRM_FollowUpRecords")]
    public partial class tb_CRM_FollowUpRecords: BaseEntity, ICloneable
    {
        public tb_CRM_FollowUpRecords()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("跟进记录表tb_CRM_FollowUpRecords" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _RecordID;
        /// <summary>
        /// 跟进记录
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RecordID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "跟进记录" , IsPrimaryKey = true)]
        public long RecordID
        { 
            get{return _RecordID;}
            set{
            SetProperty(ref _RecordID, value);
                base.PrimaryKeyID = _RecordID;
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

        private long? _LeadID;
        /// <summary>
        /// 线索
        /// </summary>
        [AdvQueryAttribute(ColName = "LeadID",ColDesc = "线索")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "LeadID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "线索" )]
        [FKRelationAttribute("tb_CRM_Leads","LeadID")]
        public long? LeadID
        { 
            get{return _LeadID;}
            set{
            SetProperty(ref _LeadID, value);
                        }
        }

        private long? _PlanID;
        /// <summary>
        /// 跟进计划
        /// </summary>
        [AdvQueryAttribute(ColName = "PlanID",ColDesc = "跟进计划")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PlanID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "跟进计划" )]
        [FKRelationAttribute("tb_CRM_FollowUpPlans","PlanID")]
        public long? PlanID
        { 
            get{return _PlanID;}
            set{
            SetProperty(ref _PlanID, value);
                        }
        }

        private long _Employee_ID;
        /// <summary>
        /// 跟进人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "跟进人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "跟进人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
                        }
        }

        private DateTime _FollowUpDate;
        /// <summary>
        /// 跟进日期
        /// </summary>
        [AdvQueryAttribute(ColName = "FollowUpDate",ColDesc = "跟进日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "FollowUpDate" ,IsNullable = false,ColumnDescription = "跟进日期" )]
        public DateTime FollowUpDate
        { 
            get{return _FollowUpDate;}
            set{
            SetProperty(ref _FollowUpDate, value);
                        }
        }

        private int _FollowUpMethod;
        /// <summary>
        /// 跟进方式
        /// </summary>
        [AdvQueryAttribute(ColName = "FollowUpMethod",ColDesc = "跟进方式")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "FollowUpMethod" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "跟进方式" )]
        public int FollowUpMethod
        { 
            get{return _FollowUpMethod;}
            set{
            SetProperty(ref _FollowUpMethod, value);
                        }
        }

        private string _FollowUpSubject;
        /// <summary>
        /// 跟进主题
        /// </summary>
        [AdvQueryAttribute(ColName = "FollowUpSubject",ColDesc = "跟进主题")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "FollowUpSubject" ,Length=200,IsNullable = false,ColumnDescription = "跟进主题" )]
        public string FollowUpSubject
        { 
            get{return _FollowUpSubject;}
            set{
            SetProperty(ref _FollowUpSubject, value);
                        }
        }

        private string _FollowUpContent;
        /// <summary>
        /// 跟进内容
        /// </summary>
        [AdvQueryAttribute(ColName = "FollowUpContent",ColDesc = "跟进内容")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "FollowUpContent" ,Length=1000,IsNullable = false,ColumnDescription = "跟进内容" )]
        public string FollowUpContent
        { 
            get{return _FollowUpContent;}
            set{
            SetProperty(ref _FollowUpContent, value);
                        }
        }

        private bool? _HasResponse= false;
        /// <summary>
        /// 有回应
        /// </summary>
        [AdvQueryAttribute(ColName = "HasResponse",ColDesc = "有回应")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "HasResponse" ,IsNullable = true,ColumnDescription = "有回应" )]
        public bool? HasResponse
        { 
            get{return _HasResponse;}
            set{
            SetProperty(ref _HasResponse, value);
                        }
        }

        private string _FollowUpResult;
        /// <summary>
        /// 跟进结果
        /// </summary>
        [AdvQueryAttribute(ColName = "FollowUpResult",ColDesc = "跟进结果")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "FollowUpResult" ,Length=100,IsNullable = true,ColumnDescription = "跟进结果")]
        public string FollowUpResult
        { 
            get{return _FollowUpResult;}
            set{
            SetProperty(ref _FollowUpResult, value);
                        }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=255,IsNullable = true,ColumnDescription = "备注")]
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
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(PlanID))]
        public virtual tb_CRM_FollowUpPlans tb_crm_followupplans { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Customer_id))]
        public virtual tb_CRM_Customer tb_crm_customer { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(LeadID))]
        public virtual tb_CRM_Leads tb_crm_leads { get; set; }



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
                    Type type = typeof(tb_CRM_FollowUpRecords);
                    
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
            tb_CRM_FollowUpRecords loctype = (tb_CRM_FollowUpRecords)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

