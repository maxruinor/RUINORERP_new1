
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:41:43
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
    /// 目标客户-公海客户CRM系统中使用，给成交客户作外键引用
    /// </summary>
    [Serializable()]
    [Description("目标客户-公海客户CRM系统中使用，给成交客户作外键引用")]
    [SugarTable("tb_CRM_Customer")]
    public partial class tb_CRM_Customer: BaseEntity, ICloneable
    {
        public tb_CRM_Customer()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("目标客户-公海客户CRM系统中使用，给成交客户作外键引用tb_CRM_Customer" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Customer_id;
        /// <summary>
        /// 目标客户
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Customer_id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "目标客户" , IsPrimaryKey = true)]
        public long Customer_id
        { 
            get{return _Customer_id;}
            set{
            SetProperty(ref _Customer_id, value);
                base.PrimaryKeyID = _Customer_id;
            }
        }

        private string _CustomerName;
        /// <summary>
        /// 客户名称
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerName",ColDesc = "客户名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomerName" ,Length=50,IsNullable = true,ColumnDescription = "客户名称" )]
        public string CustomerName
        { 
            get{return _CustomerName;}
            set{
            SetProperty(ref _CustomerName, value);
                        }
        }

        private long? _Employee_ID;
        /// <summary>
        /// 对接人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "对接人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "对接人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long? Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
                        }
        }

        private long? _DepartmentID;
        /// <summary>
        /// 部门
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "部门")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "部门" )]
        [FKRelationAttribute("tb_Department","DepartmentID")]
        public long? DepartmentID
        { 
            get{return _DepartmentID;}
            set{
            SetProperty(ref _DepartmentID, value);
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

        private long? _Region_ID;
        /// <summary>
        /// 地区
        /// </summary>
        [AdvQueryAttribute(ColName = "Region_ID",ColDesc = "地区")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Region_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "地区" )]
        [FKRelationAttribute("tb_CRM_Region","Region_ID")]
        public long? Region_ID
        { 
            get{return _Region_ID;}
            set{
            SetProperty(ref _Region_ID, value);
                        }
        }

        private long? _ProvinceID;
        /// <summary>
        /// 省
        /// </summary>
        [AdvQueryAttribute(ColName = "ProvinceID",ColDesc = "省")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProvinceID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "省" )]
        [FKRelationAttribute("tb_Provinces","ProvinceID")]
        public long? ProvinceID
        { 
            get{return _ProvinceID;}
            set{
            SetProperty(ref _ProvinceID, value);
                        }
        }

        private long? _CityID;
        /// <summary>
        /// 城市
        /// </summary>
        [AdvQueryAttribute(ColName = "CityID",ColDesc = "城市")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CityID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "城市" )]
        [FKRelationAttribute("tb_Cities","CityID")]
        public long? CityID
        { 
            get{return _CityID;}
            set{
            SetProperty(ref _CityID, value);
                        }
        }

        private string _wwSocialTools;
        /// <summary>
        /// 旺旺/IM工具
        /// </summary>
        [AdvQueryAttribute(ColName = "wwSocialTools",ColDesc = "旺旺/IM工具")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "wwSocialTools" ,Length=200,IsNullable = true,ColumnDescription = "旺旺/IM工具" )]
        public string wwSocialTools
        { 
            get{return _wwSocialTools;}
            set{
            SetProperty(ref _wwSocialTools, value);
                        }
        }

        private string _SocialTools;
        /// <summary>
        /// 其他/IM工具
        /// </summary>
        [AdvQueryAttribute(ColName = "SocialTools",ColDesc = "其他/IM工具")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SocialTools" ,Length=200,IsNullable = true,ColumnDescription = "其他/IM工具" )]
        public string SocialTools
        { 
            get{return _SocialTools;}
            set{
            SetProperty(ref _SocialTools, value);
                        }
        }

        private string _Contact_Name;
        /// <summary>
        /// 联系人姓名
        /// </summary>
        [AdvQueryAttribute(ColName = "Contact_Name",ColDesc = "联系人姓名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Contact_Name" ,Length=50,IsNullable = true,ColumnDescription = "联系人姓名" )]
        public string Contact_Name
        { 
            get{return _Contact_Name;}
            set{
            SetProperty(ref _Contact_Name, value);
                        }
        }

        private string _Contact_Email;
        /// <summary>
        /// 邮箱
        /// </summary>
        [AdvQueryAttribute(ColName = "Contact_Email",ColDesc = "邮箱")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Contact_Email" ,Length=100,IsNullable = true,ColumnDescription = "邮箱" )]
        public string Contact_Email
        { 
            get{return _Contact_Email;}
            set{
            SetProperty(ref _Contact_Email, value);
                        }
        }

        private string _Contact_Phone;
        /// <summary>
        /// 电话
        /// </summary>
        [AdvQueryAttribute(ColName = "Contact_Phone",ColDesc = "电话")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Contact_Phone" ,Length=30,IsNullable = true,ColumnDescription = "电话" )]
        public string Contact_Phone
        { 
            get{return _Contact_Phone;}
            set{
            SetProperty(ref _Contact_Phone, value);
                        }
        }

        private string _CustomerAddress;
        /// <summary>
        /// 客户地址
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerAddress",ColDesc = "客户地址")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomerAddress" ,Length=300,IsNullable = true,ColumnDescription = "客户地址" )]
        public string CustomerAddress
        { 
            get{return _CustomerAddress;}
            set{
            SetProperty(ref _CustomerAddress, value);
                        }
        }

        private bool? _RepeatCustomer= false;
        /// <summary>
        /// 重复客户
        /// </summary>
        [AdvQueryAttribute(ColName = "RepeatCustomer",ColDesc = "重复客户")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "RepeatCustomer" ,IsNullable = true,ColumnDescription = "重复客户" )]
        public bool? RepeatCustomer
        { 
            get{return _RepeatCustomer;}
            set{
            SetProperty(ref _RepeatCustomer, value);
                        }
        }

        private int _CustomerStatus;
        /// <summary>
        /// 客户状态
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerStatus",ColDesc = "客户状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "CustomerStatus" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "客户状态" )]
        public int CustomerStatus
        { 
            get{return _CustomerStatus;}
            set{
            SetProperty(ref _CustomerStatus, value);
                        }
        }

        private string _CustomerTags;
        /// <summary>
        /// 客户标签
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerTags",ColDesc = "客户标签")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomerTags" ,Length=500,IsNullable = true,ColumnDescription = "客户标签" )]
        public string CustomerTags
        { 
            get{return _CustomerTags;}
            set{
            SetProperty(ref _CustomerTags, value);
                        }
        }

        private string _CoreProductInfo;
        /// <summary>
        /// 获客来源
        /// </summary>
        [AdvQueryAttribute(ColName = "CoreProductInfo",ColDesc = "获客来源")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CoreProductInfo" ,Length=200,IsNullable = true,ColumnDescription = "获客来源" )]
        public string CoreProductInfo
        { 
            get{return _CoreProductInfo;}
            set{
            SetProperty(ref _CoreProductInfo, value);
                        }
        }

        private string _GetCustomerSource;
        /// <summary>
        /// 主营产品信息
        /// </summary>
        [AdvQueryAttribute(ColName = "GetCustomerSource",ColDesc = "主营产品信息")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "GetCustomerSource" ,Length=250,IsNullable = true,ColumnDescription = "主营产品信息" )]
        public string GetCustomerSource
        { 
            get{return _GetCustomerSource;}
            set{
            SetProperty(ref _GetCustomerSource, value);
                        }
        }

        private string _SalePlatform;
        /// <summary>
        /// 销售平台
        /// </summary>
        [AdvQueryAttribute(ColName = "SalePlatform",ColDesc = "销售平台")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SalePlatform" ,Length=50,IsNullable = true,ColumnDescription = "销售平台" )]
        public string SalePlatform
        { 
            get{return _SalePlatform;}
            set{
            SetProperty(ref _SalePlatform, value);
                        }
        }

        private string _Website;
        /// <summary>
        /// 网址
        /// </summary>
        [AdvQueryAttribute(ColName = "Website",ColDesc = "网址")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Website" ,Length=255,IsNullable = true,ColumnDescription = "网址" )]
        public string Website
        { 
            get{return _Website;}
            set{
            SetProperty(ref _Website, value);
                        }
        }

        private int? _CustomerLevel;
        /// <summary>
        /// 客户级别
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerLevel",ColDesc = "客户级别")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "CustomerLevel" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "客户级别" )]
        public int? CustomerLevel
        { 
            get{return _CustomerLevel;}
            set{
            SetProperty(ref _CustomerLevel, value);
                        }
        }

        private int? _PurchaseCount;
        /// <summary>
        /// 采购次数
        /// </summary>
        [AdvQueryAttribute(ColName = "PurchaseCount",ColDesc = "采购次数")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "PurchaseCount" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "采购次数" )]
        public int? PurchaseCount
        { 
            get{return _PurchaseCount;}
            set{
            SetProperty(ref _PurchaseCount, value);
                        }
        }

        private decimal? _TotalPurchaseAmount;
        /// <summary>
        /// 采购金额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalPurchaseAmount",ColDesc = "采购金额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalPurchaseAmount" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "采购金额" )]
        public decimal? TotalPurchaseAmount
        { 
            get{return _TotalPurchaseAmount;}
            set{
            SetProperty(ref _TotalPurchaseAmount, value);
                        }
        }

        private int? _DaysSinceLastPurchase;
        /// <summary>
        /// 最近距上次采购间隔天
        /// </summary>
        [AdvQueryAttribute(ColName = "DaysSinceLastPurchase",ColDesc = "最近距上次采购间隔天")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DaysSinceLastPurchase" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "最近距上次采购间隔天" )]
        public int? DaysSinceLastPurchase
        { 
            get{return _DaysSinceLastPurchase;}
            set{
            SetProperty(ref _DaysSinceLastPurchase, value);
                        }
        }

        private DateTime? _LastPurchaseDate;
        /// <summary>
        /// 最近采购日期
        /// </summary>
        [AdvQueryAttribute(ColName = "LastPurchaseDate",ColDesc = "最近采购日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "LastPurchaseDate" ,IsNullable = true,ColumnDescription = "最近采购日期" )]
        public DateTime? LastPurchaseDate
        { 
            get{return _LastPurchaseDate;}
            set{
            SetProperty(ref _LastPurchaseDate, value);
                        }
        }

        private DateTime? _FirstPurchaseDate;
        /// <summary>
        /// 首次采购日期
        /// </summary>
        [AdvQueryAttribute(ColName = "FirstPurchaseDate",ColDesc = "首次采购日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "FirstPurchaseDate" ,IsNullable = true,ColumnDescription = "首次采购日期" )]
        public DateTime? FirstPurchaseDate
        { 
            get{return _FirstPurchaseDate;}
            set{
            SetProperty(ref _FirstPurchaseDate, value);
                        }
        }

        private bool? _Converted= false;
        /// <summary>
        /// 已转化
        /// </summary>
        [AdvQueryAttribute(ColName = "Converted",ColDesc = "已转化")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Converted" ,IsNullable = true,ColumnDescription = "已转化" )]
        public bool? Converted
        { 
            get{return _Converted;}
            set{
            SetProperty(ref _Converted, value);
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
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Region_ID))]
        public virtual tb_CRM_Region tb_crm_region { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(DepartmentID))]
        public virtual tb_Department tb_department { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(CityID))]
        public virtual tb_Cities tb_cities { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(LeadID))]
        public virtual tb_CRM_Leads tb_crm_leads { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProvinceID))]
        public virtual tb_Provinces tb_provinces { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_CRM_Contact.Customer_id))]
        public virtual List<tb_CRM_Contact> tb_CRM_Contacts { get; set; }
        //tb_CRM_Contact.Customer_id)
        //Customer_id.FK_CONTACT_REF_CUSTOMER)
        //tb_CRM_Customer.Customer_id)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_CRM_FollowUpPlans.Customer_id))]
        public virtual List<tb_CRM_FollowUpPlans> tb_CRM_FollowUpPlanses { get; set; }
        //tb_CRM_FollowUpPlans.Customer_id)
        //Customer_id.FK_TB_CRM_F_REFERENCE_TB_CRM_C)
        //tb_CRM_Customer.Customer_id)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_CRM_FollowUpRecords.Customer_id))]
        public virtual List<tb_CRM_FollowUpRecords> tb_CRM_FollowUpRecordses { get; set; }
        //tb_CRM_FollowUpRecords.Customer_id)
        //Customer_id.FK_Followuprecoreds_REF_CRM_Customer)
        //tb_CRM_Customer.Customer_id)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_CRM_Collaborator.Customer_id))]
        public virtual List<tb_CRM_Collaborator> tb_CRM_Collaborators { get; set; }
        //tb_CRM_Collaborator.Customer_id)
        //Customer_id.FK_TB_CRM_C_REFERENCE_TB_CRM_C)
        //tb_CRM_Customer.Customer_id)


        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_CRM_Customer loctype = (tb_CRM_Customer)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

