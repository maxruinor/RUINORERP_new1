
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:56:54
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
    /// 币别资料表-备份第一行数据后删除重建 如果不行则直接修改字段删除字段
    /// </summary>
    [Serializable()]
    [Description("币别资料表-备份第一行数据后删除重建 如果不行则直接修改字段删除字段")]
    [SugarTable("tb_Currency")]
    public partial class tb_Currency: BaseEntity, ICloneable
    {
        public tb_Currency()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("币别资料表-备份第一行数据后删除重建 如果不行则直接修改字段删除字段tb_Currency" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Currency_ID;
        /// <summary>
        /// 币别
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Currency_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "币别" , IsPrimaryKey = true)]
        public long Currency_ID
        { 
            get{return _Currency_ID;}
            set{
            SetProperty(ref _Currency_ID, value);
                base.PrimaryKeyID = _Currency_ID;
            }
        }

        private string _Country;
        /// <summary>
        /// 国家
        /// </summary>
        [AdvQueryAttribute(ColName = "Country",ColDesc = "国家")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Country" ,Length=50,IsNullable = true,ColumnDescription = "国家" )]
        public string Country
        { 
            get{return _Country;}
            set{
            SetProperty(ref _Country, value);
                        }
        }

        private string _CurrencyCode;
        /// <summary>
        /// 币别代码
        /// </summary>
        [AdvQueryAttribute(ColName = "CurrencyCode",ColDesc = "币别代码")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CurrencyCode" ,Length=10,IsNullable = true,ColumnDescription = "币别代码" )]
        public string CurrencyCode
        { 
            get{return _CurrencyCode;}
            set{
            SetProperty(ref _CurrencyCode, value);
                        }
        }

        private string _CurrencyName;
        /// <summary>
        /// 币别名称
        /// </summary>
        [AdvQueryAttribute(ColName = "CurrencyName",ColDesc = "币别名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CurrencyName" ,Length=20,IsNullable = false,ColumnDescription = "币别名称" )]
        public string CurrencyName
        { 
            get{return _CurrencyName;}
            set{
            SetProperty(ref _CurrencyName, value);
                        }
        }

        private string _CurrencySymbol;
        /// <summary>
        /// 币别符号
        /// </summary>
        [AdvQueryAttribute(ColName = "CurrencySymbol",ColDesc = "币别符号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CurrencySymbol" ,Length=10,IsNullable = true,ColumnDescription = "币别符号" )]
        public string CurrencySymbol
        { 
            get{return _CurrencySymbol;}
            set{
            SetProperty(ref _CurrencySymbol, value);
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

        #endregion

        #region 扩展属性

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_Account.Currency_ID))]
        public virtual List<tb_FM_Account> tb_FM_Accounts { get; set; }
        //tb_FM_Account.Currency_ID)
        //Currency_ID.FK_ACCOUNTS_REF_CURRENCY)
        //tb_Currency.Currency_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_ExpenseClaim.Currency_ID))]
        public virtual List<tb_FM_ExpenseClaim> tb_FM_ExpenseClaims { get; set; }
        //tb_FM_ExpenseClaim.Currency_ID)
        //Currency_ID.FK_EXPENSECLAIM_REF_CURRENCY)
        //tb_Currency.Currency_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_GeneralLedger.Currency_ID))]
        public virtual List<tb_FM_GeneralLedger> tb_FM_GeneralLedgers { get; set; }
        //tb_FM_GeneralLedger.Currency_ID)
        //Currency_ID.FK_FM_GE_REFERENCEDGER_TB_CURRENCY)
        //tb_Currency.Currency_ID)

      

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PaymentReceipt.Currency_ID))]
        public virtual List<tb_FM_PaymentReceipt> tb_FM_PaymentReceipts { get; set; }
        //tb_FM_PaymentReceipt.Currency_ID)
        //Currency_ID.FK_FM_PAYMENTRECEIPT_REF_CURREncy)
        //tb_Currency.Currency_ID)

       

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_OtherExpense.Currency_ID))]
        public virtual List<tb_FM_OtherExpense> tb_FM_OtherExpenses { get; set; }
        //tb_FM_OtherExpense.Currency_ID)
        //Currency_ID.FK_OTHEREXPENSE_REF_CURRENCY)
        //tb_Currency.Currency_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PaymentApplication.Currency_ID))]
        public virtual List<tb_FM_PaymentApplication> tb_FM_PaymentApplications { get; set; }
        //tb_FM_PaymentApplication.Currency_ID)
        //Currency_ID.FK_PAYMENTAPPLICATION_REF_CURRENCY)
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

