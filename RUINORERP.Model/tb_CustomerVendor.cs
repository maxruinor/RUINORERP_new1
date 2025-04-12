
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:31:57
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
    /// 客户厂商表 开票资料这种与财务有关另外开表
    /// </summary>
    [Serializable()]
    [Description("客户厂商表 开票资料这种与财务有关另外开表")]
    [SugarTable("tb_CustomerVendor")]
    public partial class tb_CustomerVendor: BaseEntity, ICloneable
    {
        public tb_CustomerVendor()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("客户厂商表 开票资料这种与财务有关另外开表tb_CustomerVendor" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _CustomerVendor_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long CustomerVendor_ID
        { 
            get{return _CustomerVendor_ID;}
            set{
            SetProperty(ref _CustomerVendor_ID, value);
                base.PrimaryKeyID = _CustomerVendor_ID;
            }
        }

        private string _CVCode;
        /// <summary>
        /// 编号
        /// </summary>
        [AdvQueryAttribute(ColName = "CVCode",ColDesc = "编号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CVCode" ,Length=50,IsNullable = true,ColumnDescription = "编号" )]
        public string CVCode
        { 
            get{return _CVCode;}
            set{
            SetProperty(ref _CVCode, value);
                        }
        }

        private string _CVName;
        /// <summary>
        /// 全称
        /// </summary>
        [AdvQueryAttribute(ColName = "CVName",ColDesc = "全称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CVName" ,Length=255,IsNullable = false,ColumnDescription = "全称" )]
        public string CVName
        { 
            get{return _CVName;}
            set{
            SetProperty(ref _CVName, value);
                        }
        }

        private string _ShortName;
        /// <summary>
        /// 简称
        /// </summary>
        [AdvQueryAttribute(ColName = "ShortName",ColDesc = "简称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ShortName" ,Length=50,IsNullable = true,ColumnDescription = "简称" )]
        public string ShortName
        { 
            get{return _ShortName;}
            set{
            SetProperty(ref _ShortName, value);
                        }
        }

        private long? _Type_ID;
        /// <summary>
        /// 客户厂商类型
        /// </summary>
        [AdvQueryAttribute(ColName = "Type_ID",ColDesc = "客户厂商类型")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Type_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "客户厂商类型" )]
        [FKRelationAttribute("tb_CustomerVendorType","Type_ID")]
        public long? Type_ID
        { 
            get{return _Type_ID;}
            set{
            SetProperty(ref _Type_ID, value);
                        }
        }

        private long? _Employee_ID;
        /// <summary>
        /// 责任人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "责任人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "责任人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long? Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
                        }
        }

        private bool _IsExclusive= true;
        /// <summary>
        /// 责任人专属
        /// </summary>
        [AdvQueryAttribute(ColName = "IsExclusive",ColDesc = "责任人专属")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsExclusive" ,IsNullable = false,ColumnDescription = "责任人专属" )]
        public bool IsExclusive
        { 
            get{return _IsExclusive;}
            set{
            SetProperty(ref _IsExclusive, value);
                        }
        }

        private long? _Paytype_ID;
        /// <summary>
        /// 默认交易方式
        /// </summary>
        [AdvQueryAttribute(ColName = "Paytype_ID",ColDesc = "默认交易方式")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Paytype_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "默认交易方式" )]
        [FKRelationAttribute("tb_PaymentMethod","Paytype_ID")]
        public long? Paytype_ID
        { 
            get{return _Paytype_ID;}
            set{
            SetProperty(ref _Paytype_ID, value);
                        }
        }

        private long? _Customer_id;
        /// <summary>
        /// 目标客户
        /// </summary>
        [AdvQueryAttribute(ColName = "Customer_id",ColDesc = "目标客户")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Customer_id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "目标客户")]
        [FKRelationAttribute("tb_CRM_Customer", "Customer_id")]
        public long? Customer_id
        { 
            get{return _Customer_id;}
            set{
            SetProperty(ref _Customer_id, value);
                        }
        }

        private string _Area;
        /// <summary>
        /// 所在地区
        /// </summary>
        [AdvQueryAttribute(ColName = "Area",ColDesc = "所在地区")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Area" ,Length=50,IsNullable = true,ColumnDescription = "所在地区" )]
        public string Area
        { 
            get{return _Area;}
            set{
            SetProperty(ref _Area, value);
                        }
        }

        private string _Contact;
        /// <summary>
        /// 联系人
        /// </summary>
        [AdvQueryAttribute(ColName = "Contact",ColDesc = "联系人")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Contact" ,Length=50,IsNullable = true,ColumnDescription = "联系人" )]
        public string Contact
        { 
            get{return _Contact;}
            set{
            SetProperty(ref _Contact, value);
                        }
        }

        private string _Phone;
        /// <summary>
        /// 电话
        /// </summary>
        [AdvQueryAttribute(ColName = "Phone",ColDesc = "电话")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Phone" ,Length=255,IsNullable = true,ColumnDescription = "电话" )]
        public string Phone
        { 
            get{return _Phone;}
            set{
            SetProperty(ref _Phone, value);
                        }
        }

        private string _Address;
        /// <summary>
        /// 地址
        /// </summary>
        [AdvQueryAttribute(ColName = "Address",ColDesc = "地址")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Address" ,Length=255,IsNullable = true,ColumnDescription = "地址" )]
        public string Address
        { 
            get{return _Address;}
            set{
            SetProperty(ref _Address, value);
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

        private bool _IsCustomer= false;
        /// <summary>
        /// 是客户
        /// </summary>
        [AdvQueryAttribute(ColName = "IsCustomer",ColDesc = "是客户")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsCustomer" ,IsNullable = false,ColumnDescription = "是客户" )]
        public bool IsCustomer
        { 
            get{return _IsCustomer;}
            set{
            SetProperty(ref _IsCustomer, value);
                        }
        }

        private bool _IsVendor= false;
        /// <summary>
        /// 是供应商
        /// </summary>
        [AdvQueryAttribute(ColName = "IsVendor",ColDesc = "是供应商")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsVendor" ,IsNullable = false,ColumnDescription = "是供应商" )]
        public bool IsVendor
        { 
            get{return _IsVendor;}
            set{
            SetProperty(ref _IsVendor, value);
                        }
        }

        private bool _IsOther= false;
        /// <summary>
        /// 是其他
        /// </summary>
        [AdvQueryAttribute(ColName = "IsOther",ColDesc = "是其他")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsOther" ,IsNullable = false,ColumnDescription = "是其他" )]
        public bool IsOther
        { 
            get{return _IsOther;}
            set{
            SetProperty(ref _IsOther, value);
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

        private bool? _Is_enabled= true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_enabled",ColDesc = "是否启用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Is_enabled" ,IsNullable = true,ColumnDescription = "是否启用" )]
        public bool? Is_enabled
        { 
            get{return _Is_enabled;}
            set{
            SetProperty(ref _Is_enabled, value);
                        }
        }

        private bool? _Is_available= true;
        /// <summary>
        /// 是否可用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_available",ColDesc = "是否可用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Is_available" ,IsNullable = true,ColumnDescription = "是否可用" )]
        public bool? Is_available
        { 
            get{return _Is_available;}
            set{
            SetProperty(ref _Is_available, value);
                        }
        }

        private bool _isdeleted= false;
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "isdeleted" ,IsNullable = false,ColumnDescription = "逻辑删除" )]
        [Browsable(false)]
        public bool isdeleted
        { 
            get{return _isdeleted;}
            set{
            SetProperty(ref _isdeleted, value);
                        }
        }

        private long? _BankAccount_id;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "BankAccount_id",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BankAccount_id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_BankAccount","BankAccount_id")]
        public long? BankAccount_id
        { 
            get{return _BankAccount_id;}
            set{
            SetProperty(ref _BankAccount_id, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(BankAccount_id))]
        public virtual tb_BankAccount tb_bankaccount { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Customer_id))]
        public virtual tb_CRM_Customer tb_crm_customer { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Type_ID))]
        public virtual tb_CustomerVendorType tb_customervendortype { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Paytype_ID))]
        public virtual tb_PaymentMethod tb_paymentmethod { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurOrder.CustomerVendor_ID))]
        public virtual List<tb_PurOrder> tb_PurOrders { get; set; }
        //tb_PurOrder.CustomerVendor_ID)
        //CustomerVendor_ID.FK_PO_CUSTOMv)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MRP_ReworkEntry.CustomerVendor_ID))]
        public virtual List<tb_MRP_ReworkEntry> tb_MRP_ReworkEntries { get; set; }
        //tb_MRP_ReworkEntry.CustomerVendor_ID)
        //CustomerVendor_ID.FK_MRP_ReworkEntry_REF_CUSTOmerVendor)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_OtherExpenseDetail.CustomerVendor_ID))]
        public virtual List<tb_FM_OtherExpenseDetail> tb_FM_OtherExpenseDetails { get; set; }
        //tb_FM_OtherExpenseDetail.CustomerVendor_ID)
        //CustomerVendor_ID.FK_FM_OTHEREXPENSE_RE_CUSTOVENDOR)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MaterialRequisition.CustomerVendor_ID))]
        public virtual List<tb_MaterialRequisition> tb_MaterialRequisitions { get; set; }
        //tb_MaterialRequisition.CustomerVendor_ID)
        //CustomerVendor_ID.FK_MATEREQUISTIONS_RE_CUSTOVENDOR)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Return.CustomerVendor_ID))]
        public virtual List<tb_Return> tb_Returns { get; set; }
        //tb_Return.CustomerVendor_ID)
        //CustomerVendor_ID.FK_RETURN_RE_CUSTOVENDOR)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOut.CustomerVendor_ID))]
        public virtual List<tb_SaleOut> tb_SaleOuts { get; set; }
        //tb_SaleOut.CustomerVendor_ID)
        //CustomerVendor_ID.FK_SALEOUT_REF_CUSTOVENDOR)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Prod.CustomerVendor_ID))]
        public virtual List<tb_Prod> tb_Prods { get; set; }
        //tb_Prod.CustomerVendor_ID)
        //CustomerVendor_ID.FK_TB_PROD_B_TB_CUSTOME)
        //tb_CustomerVendor.CustomerVendor_ID)


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurEntry.CustomerVendor_ID))]
        public virtual List<tb_PurEntry> tb_PurEntries { get; set; }
        //tb_PurEntry.CustomerVendor_ID)
        //CustomerVendor_ID.FK_TB_PUREN_REF_TB_CUSTOMER)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MaterialReturn.CustomerVendor_ID))]
        public virtual List<tb_MaterialReturn> tb_MaterialReturns { get; set; }
        //tb_MaterialReturn.CustomerVendor_ID)
        //CustomerVendor_ID.FK_MATERIALRETURN_REF_CUSTOMERVENDOR)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FinishedGoodsInv.CustomerVendor_ID))]
        public virtual List<tb_FinishedGoodsInv> tb_FinishedGoodsInvs { get; set; }
        //tb_FinishedGoodsInv.CustomerVendor_ID)
        //CustomerVendor_ID.FK_TB_FINISINV_REF_TB_CUSTO)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProdReturning.CustomerVendor_ID))]
        public virtual List<tb_ProdReturning> tb_ProdReturnings { get; set; }
        //tb_ProdReturning.CustomerVendor_ID)
        //CustomerVendor_ID.FK_PRODRetruning_REF_CUSTOmervendor)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_CustomerVendorFiles.CustomerVendor_ID))]
        public virtual List<tb_CustomerVendorFiles> tb_CustomerVendorFileses { get; set; }
        //tb_CustomerVendorFiles.CustomerVendor_ID)
        //CustomerVendor_ID.FK_TB_CUSTO_REF_TB_CUSTO_F)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurGoodsRecommendDetail.CustomerVendor_ID))]
        public virtual List<tb_PurGoodsRecommendDetail> tb_PurGoodsRecommendDetails { get; set; }
        //tb_PurGoodsRecommendDetail.CustomerVendor_ID)
        //CustomerVendor_ID.FK_PURGOODSEWCOMMENDDETAIL_REF_CUSTOMERVENDOR)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProdBorrowing.CustomerVendor_ID))]
        public virtual List<tb_ProdBorrowing> tb_ProdBorrowings { get; set; }
        //tb_ProdBorrowing.CustomerVendor_ID)
        //CustomerVendor_ID.FK_PRODBorrowing_REF_CUSTOmervendor)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PaymentApplication.CustomerVendor_ID))]
        public virtual List<tb_FM_PaymentApplication> tb_FM_PaymentApplications { get; set; }
        //tb_FM_PaymentApplication.CustomerVendor_ID)
        //CustomerVendor_ID.FK_PAYMENTAPPLICATION_REF_CUSTOMERVENDOR)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurReturnEntry.CustomerVendor_ID))]
        public virtual List<tb_PurReturnEntry> tb_PurReturnEntries { get; set; }
        //tb_PurReturnEntry.CustomerVendor_ID)
        //CustomerVendor_ID.FK_PURReturn_REF_CUSTOMERVENDOR)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_StockOut.CustomerVendor_ID))]
        public virtual List<tb_StockOut> tb_StockOuts { get; set; }
        //tb_StockOut.CustomerVendor_ID)
        //CustomerVendor_ID.FK_TB_STOCKOUT_REF_CUSTOVendor)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOrder.CustomerVendor_ID))]
        public virtual List<tb_SaleOrder> tb_SaleOrders { get; set; }
        //tb_SaleOrder.CustomerVendor_ID)
        //CustomerVendor_ID.FK_TB_SALEO_REFERENCE_TB_CUSTO)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOutRe.CustomerVendor_ID))]
        public virtual List<tb_SaleOutRe> tb_SaleOutRes { get; set; }
        //tb_SaleOutRe.CustomerVendor_ID)
        //CustomerVendor_ID.FK_TB_SOOUTRE_RE_CUSTOVENDOR)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_Initial_PayAndReceivable.CustomerVendor_ID))]
        public virtual List<tb_FM_Initial_PayAndReceivable> tb_FM_Initial_PayAndReceivables { get; set; }
        //tb_FM_Initial_PayAndReceivable.CustomerVendor_ID)
        //CustomerVendor_ID.FK_TB_FM_INITPRBLE_RE_CUSTOVENDOR)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PayeeInfo.CustomerVendor_ID))]
        public virtual List<tb_FM_PayeeInfo> tb_FM_PayeeInfos { get; set; }
        //tb_FM_PayeeInfo.CustomerVendor_ID)
        //CustomerVendor_ID.FK_TB_FM_PA_REFERENCE_TB_CUSTO)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PaymentBill.CustomerVendor_ID))]
        public virtual List<tb_FM_PaymentBill> tb_FM_PaymentBills { get; set; }
        //tb_FM_PaymentBill.CustomerVendor_ID)
        //CustomerVendor_ID.FK_FM_PAYMENTBILL_REF_CUSTOMERVENDOR)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_StockIn.CustomerVendor_ID))]
        public virtual List<tb_StockIn> tb_StockIns { get; set; }
        //tb_StockIn.CustomerVendor_ID)
        //CustomerVendor_ID.FK_TB_STOCKIN_RE_CUSTOMERVENDOR)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurEntryRe.CustomerVendor_ID))]
        public virtual List<tb_PurEntryRe> tb_PurEntryRes { get; set; }
        //tb_PurEntryRe.CustomerVendor_ID)
        //CustomerVendor_ID.FK_PURENRE_CUSTOVENDOR)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PrePaymentBillDetail.CustomerVendor_ID))]
        public virtual List<tb_FM_PrePaymentBillDetail> tb_FM_PrePaymentBillDetails { get; set; }
        //tb_FM_PrePaymentBillDetail.CustomerVendor_ID)
        //CustomerVendor_ID.FK_FM_PREPaymentBDEtail_CUSTOVENDOR)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ManufacturingOrder.CustomerVendor_ID_Out))]
        public virtual List<tb_ManufacturingOrder> tb_ManufacturingOrders_Out { get; set; }
        //tb_ManufacturingOrder.CustomerVendor_ID)
        //CustomerVendor_ID.FK_MANUF_REF_CUSTOVENDOR_OUT)
        //tb_CustomerVendor.CustomerVendor_ID_Out)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ManufacturingOrder.CustomerVendor_ID))]
        public virtual List<tb_ManufacturingOrder> tb_ManufacturingOrders { get; set; }
        //tb_ManufacturingOrder.CustomerVendor_ID)
        //CustomerVendor_ID.FK_MANUFCTRUINGORDER_REF_CUSTOMERVENDOR)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurOrderRe.CustomerVendor_ID))]
        public virtual List<tb_PurOrderRe> tb_PurOrderRes { get; set; }
        //tb_PurOrderRe.CustomerVendor_ID)
        //CustomerVendor_ID.FK_TB_PUROR_REFERENCE_TB_CUSTO)
        //tb_CustomerVendor.CustomerVendor_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MRP_ReworkReturn.CustomerVendor_ID))]
        public virtual List<tb_MRP_ReworkReturn> tb_MRP_ReworkReturns { get; set; }
        //tb_MRP_ReworkReturn.CustomerVendor_ID)
        //CustomerVendor_ID.FK_MRP_ReworkRetrun_REF_TB_CUSTOMERVENDOR)
        //tb_CustomerVendor.CustomerVendor_ID)


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
                    Type type = typeof(tb_CustomerVendor);
                    
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
            tb_CustomerVendor loctype = (tb_CustomerVendor)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

