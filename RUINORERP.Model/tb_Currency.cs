
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:32
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
    /// 币别资料表
    /// </summary>
    [Serializable()]
    [Description("tb_Currency")]
    [SugarTable("tb_Currency")]
    public partial class tb_Currency: BaseEntity, ICloneable
    {
        public tb_Currency()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_Currency" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Currency_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Currency_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long Currency_ID
        { 
            get{return _Currency_ID;}
            set{
            base.PrimaryKeyID = _Currency_ID;
            SetProperty(ref _Currency_ID, value);
            }
        }

        private string _GroupName;
        /// <summary>
        /// 组合名称
        /// </summary>
        [AdvQueryAttribute(ColName = "GroupName",ColDesc = "组合名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "GroupName" ,Length=50,IsNullable = true,ColumnDescription = "组合名称" )]
        public string GroupName
        { 
            get{return _GroupName;}
            set{
            SetProperty(ref _GroupName, value);
            }
        }

        private string _CurrencyCode;
        /// <summary>
        /// 外币代码
        /// </summary>
        [AdvQueryAttribute(ColName = "CurrencyCode",ColDesc = "外币代码")] 
        [SugarColumn(ColumnDataType = "char", SqlParameterDbType ="String",  ColumnName = "CurrencyCode" ,Length=10,IsNullable = true,ColumnDescription = "外币代码" )]
        public string CurrencyCode
        { 
            get{return _CurrencyCode;}
            set{
            SetProperty(ref _CurrencyCode, value);
            }
        }

        private string _CurrencyName;
        /// <summary>
        /// 外币名称
        /// </summary>
        [AdvQueryAttribute(ColName = "CurrencyName",ColDesc = "外币名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CurrencyName" ,Length=20,IsNullable = false,ColumnDescription = "外币名称" )]
        public string CurrencyName
        { 
            get{return _CurrencyName;}
            set{
            SetProperty(ref _CurrencyName, value);
            }
        }

        private DateTime? _AdjustDate;
        /// <summary>
        /// 调整日期
        /// </summary>
        [AdvQueryAttribute(ColName = "AdjustDate",ColDesc = "调整日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "AdjustDate" ,IsNullable = true,ColumnDescription = "调整日期" )]
        public DateTime? AdjustDate
        { 
            get{return _AdjustDate;}
            set{
            SetProperty(ref _AdjustDate, value);
            }
        }

        private decimal? _DefaultExchRate;
        /// <summary>
        /// 预设汇率
        /// </summary>
        [AdvQueryAttribute(ColName = "DefaultExchRate",ColDesc = "预设汇率")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "DefaultExchRate" , DecimalDigits = 2,IsNullable = true,ColumnDescription = "预设汇率" )]
        public decimal? DefaultExchRate
        { 
            get{return _DefaultExchRate;}
            set{
            SetProperty(ref _DefaultExchRate, value);
            }
        }

        private decimal? _BuyExchRate;
        /// <summary>
        /// 买入汇率
        /// </summary>
        [AdvQueryAttribute(ColName = "BuyExchRate",ColDesc = "买入汇率")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "BuyExchRate" , DecimalDigits = 2,IsNullable = true,ColumnDescription = "买入汇率" )]
        public decimal? BuyExchRate
        { 
            get{return _BuyExchRate;}
            set{
            SetProperty(ref _BuyExchRate, value);
            }
        }

        private decimal? _SellOutExchRate;
        /// <summary>
        /// 卖出汇率
        /// </summary>
        [AdvQueryAttribute(ColName = "SellOutExchRate",ColDesc = "卖出汇率")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "SellOutExchRate" , DecimalDigits = 2,IsNullable = true,ColumnDescription = "卖出汇率" )]
        public decimal? SellOutExchRate
        { 
            get{return _SellOutExchRate;}
            set{
            SetProperty(ref _SellOutExchRate, value);
            }
        }

        private decimal? _MonthEndExchRate;
        /// <summary>
        /// 月末汇率
        /// </summary>
        [AdvQueryAttribute(ColName = "MonthEndExchRate",ColDesc = "月末汇率")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "MonthEndExchRate" , DecimalDigits = 2,IsNullable = true,ColumnDescription = "月末汇率" )]
        public decimal? MonthEndExchRate
        { 
            get{return _MonthEndExchRate;}
            set{
            SetProperty(ref _MonthEndExchRate, value);
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

        private bool? _Is_BaseCurrency= false;
        /// <summary>
        /// 为本位币
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_BaseCurrency",ColDesc = "为本位币")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Is_BaseCurrency" ,IsNullable = true,ColumnDescription = "为本位币" )]
        public bool? Is_BaseCurrency
        { 
            get{return _Is_BaseCurrency;}
            set{
            SetProperty(ref _Is_BaseCurrency, value);
            }
        }

        #endregion

        #region 扩展属性

        [Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_Account.Currency_ID))]
        public virtual List<tb_FM_Account> tb_FM_Accounts { get; set; }
        //tb_FM_Account.Currency_ID)
        //Currency_ID.FK_ACCOUNTS_REF_CURRENCY)
        //tb_Currency.Currency_ID)

        [Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PaymentBill.Currency_ID))]
        public virtual List<tb_FM_PaymentBill> tb_FM_PaymentBills { get; set; }
        //tb_FM_PaymentBill.Currency_ID)
        //Currency_ID.FK_FM_PAYMENTBILL_REF_CURRENCY)
        //tb_Currency.Currency_ID)

        [Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PrePaymentBillDetail.Currency_ID))]
        public virtual List<tb_FM_PrePaymentBillDetail> tb_FM_PrePaymentBillDetails { get; set; }
        //tb_FM_PrePaymentBillDetail.Currency_ID)
        //Currency_ID.FK_FM_PREPAYMENTDETAIL_REF_CURRENCY)
        //tb_Currency.Currency_ID)

        [Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_OtherExpense.Currency_ID))]
        public virtual List<tb_FM_OtherExpense> tb_FM_OtherExpenses { get; set; }
        //tb_FM_OtherExpense.Currency_ID)
        //Currency_ID.FK_OTHEREXPENSE_REF_CURRENCY)
        //tb_Currency.Currency_ID)

        [Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_ExpenseClaim.Currency_ID))]
        public virtual List<tb_FM_ExpenseClaim> tb_FM_ExpenseClaims { get; set; }
        //tb_FM_ExpenseClaim.Currency_ID)
        //Currency_ID.FK_EXPENSECLAIM_REF_CURRENCY)
        //tb_Currency.Currency_ID)


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
                    Type type = typeof(tb_Currency);
                    
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
            tb_Currency loctype = (tb_Currency)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

