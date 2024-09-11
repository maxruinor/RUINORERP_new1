
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:47:24
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
    /// 其它费用记录表，账户管理，财务系统中使用,像基础资料一样单表操作简单
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FM_OtherExpenseDetail")]
    public partial class tb_FM_OtherExpenseDetail: BaseEntity, ICloneable
    {
        public tb_FM_OtherExpenseDetail()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_FM_OtherExpenseDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ExpenseSubID;
        /// <summary>
        /// 费用
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ExpenseSubID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long ExpenseSubID
        { 
            get{return _ExpenseSubID;}
            set{
            base.PrimaryKeyID = _ExpenseSubID;
            SetProperty(ref _ExpenseSubID, value);
            }
        }

        private long _ExpenseMainID;
        /// <summary>
        /// 费用
        /// </summary>
        [AdvQueryAttribute(ColName = "ExpenseMainID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ExpenseMainID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_FM_OtherExpense","ExpenseMainID")]
        public long ExpenseMainID
        { 
            get{return _ExpenseMainID;}
            set{
            SetProperty(ref _ExpenseMainID, value);
            }
        }

        private string _ExpenseName;
        /// <summary>
        /// 事由
        /// </summary>
        [AdvQueryAttribute(ColName = "ExpenseName",ColDesc = "事由")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ExpenseName" ,Length=300,IsNullable = false,ColumnDescription = "事由" )]
        public string ExpenseName
        { 
            get{return _ExpenseName;}
            set{
            SetProperty(ref _ExpenseName, value);
            }
        }

        private long _Employee_ID;
        /// <summary>
        /// 经办人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "经办人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "经办人" )]
        [FKRelationAttribute("tb_Employee", "Employee_ID")]
        public long Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
            }
        }

        private long? _DepartmentID;
        /// <summary>
        /// 发生部门
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "发生部门")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "发生部门" )]
        [FKRelationAttribute("tb_Department","DepartmentID")]
        public long? DepartmentID
        { 
            get{return _DepartmentID;}
            set{
            SetProperty(ref _DepartmentID, value);
            }
        }

        private long? _ExpenseType_id;
        /// <summary>
        /// 费用类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ExpenseType_id",ColDesc = "费用类型")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ExpenseType_id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "费用类型" )]
        [FKRelationAttribute("tb_FM_ExpenseType","ExpenseType_id")]
        public long? ExpenseType_id
        { 
            get{return _ExpenseType_id;}
            set{
            SetProperty(ref _ExpenseType_id, value);
            }
        }

        private long? _account_id;
        /// <summary>
        /// 交易账号
        /// </summary>
        [AdvQueryAttribute(ColName = "account_id",ColDesc = "交易账号")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "account_id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "交易账号" )]
        [FKRelationAttribute("tb_FM_Account","account_id")]
        public long? account_id
        { 
            get{return _account_id;}
            set{
            SetProperty(ref _account_id, value);
            }
        }

        private long? _CustomerVendor_ID;
        /// <summary>
        /// 交易对象
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "交易对象")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "交易对象" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long? CustomerVendor_ID
        { 
            get{return _CustomerVendor_ID;}
            set{
            SetProperty(ref _CustomerVendor_ID, value);
            }
        }

        private long? _subject_id;
        /// <summary>
        /// 会计科目
        /// </summary>
        [AdvQueryAttribute(ColName = "subject_id",ColDesc = "会计科目")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "subject_id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "会计科目" )]
        [FKRelationAttribute("tb_FM_Subject","subject_id")]
        public long? subject_id
        { 
            get{return _subject_id;}
            set{
            SetProperty(ref _subject_id, value);
            }
        }

        private DateTime _CheckOutDate;
        /// <summary>
        /// 交易日期
        /// </summary>
        [AdvQueryAttribute(ColName = "CheckOutDate",ColDesc = "交易日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "CheckOutDate" ,IsNullable = false,ColumnDescription = "交易日期" )]
        public DateTime CheckOutDate
        { 
            get{return _CheckOutDate;}
            set{
            SetProperty(ref _CheckOutDate, value);
            }
        }

        private decimal _TotalAmount= ((0));
        /// <summary>
        /// 总金额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalAmount",ColDesc = "总金额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalAmount" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "总金额" )]
        public decimal TotalAmount
        { 
            get{return _TotalAmount;}
            set{
            SetProperty(ref _TotalAmount, value);
            }
        }

        private bool _IncludeTax= false;
        /// <summary>
        /// 含税
        /// </summary>
        [AdvQueryAttribute(ColName = "IncludeTax",ColDesc = "含税")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IncludeTax" ,IsNullable = false,ColumnDescription = "含税" )]
        public bool IncludeTax
        { 
            get{return _IncludeTax;}
            set{
            SetProperty(ref _IncludeTax, value);
            }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=100,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
            }
        }

        private decimal? _TaxAmount;
        /// <summary>
        /// 税额
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxAmount",ColDesc = "税额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TaxAmount" , DecimalDigits = 6,IsNullable = true,ColumnDescription = "税额" )]
        public decimal? TaxAmount
        { 
            get{return _TaxAmount;}
            set{
            SetProperty(ref _TaxAmount, value);
            }
        }

        private decimal? _TaxRate;
        /// <summary>
        /// 税率
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxRate",ColDesc = "税率")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "TaxRate" , DecimalDigits = 3,IsNullable = true,ColumnDescription = "税率" )]
        public decimal? TaxRate
        { 
            get{return _TaxRate;}
            set{
            SetProperty(ref _TaxRate, value);
            }
        }

        private decimal _UntaxedAmount;
        /// <summary>
        /// 未税本位币
        /// </summary>
        [AdvQueryAttribute(ColName = "UntaxedAmount",ColDesc = "未税本位币")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UntaxedAmount" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "未税本位币" )]
        public decimal UntaxedAmount
        { 
            get{return _UntaxedAmount;}
            set{
            SetProperty(ref _UntaxedAmount, value);
            }
        }

        private long? _ProjectGroup_ID;
        /// <summary>
        /// 所属项目
        /// </summary>
        [AdvQueryAttribute(ColName = "ProjectGroup_ID",ColDesc = "所属项目")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProjectGroup_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "所属项目" )]
        [FKRelationAttribute("tb_ProjectGroup","ProjectGroup_ID")]
        public long? ProjectGroup_ID
        { 
            get{return _ProjectGroup_ID;}
            set{
            SetProperty(ref _ProjectGroup_ID, value);
            }
        }

        #endregion

        #region 扩展属性

        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }


        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(CustomerVendor_ID))]
        public virtual tb_CustomerVendor tb_customervendor { get; set; }
        //public virtual tb_CustomerVendor tb_CustomerVendor_ID { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(ExpenseMainID))]
        public virtual tb_FM_OtherExpense tb_fm_otherexpense { get; set; }
        //public virtual tb_FM_OtherExpense tb_ExpenseMainID { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(DepartmentID))]
        public virtual tb_Department tb_department { get; set; }
        //public virtual tb_Department tb_DepartmentID { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(ProjectGroup_ID))]
        public virtual tb_ProjectGroup tb_projectgroup { get; set; }
        //public virtual tb_ProjectGroup tb_ProjectGroup_ID { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(account_id))]
        public virtual tb_FM_Account tb_fm_account { get; set; }
        //public virtual tb_FM_Account tb_account_id { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(ExpenseType_id))]
        public virtual tb_FM_ExpenseType tb_fm_expensetype { get; set; }
        //public virtual tb_FM_ExpenseType tb_ExpenseType_id { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(subject_id))]
        public virtual tb_FM_Subject tb_fm_subject { get; set; }
        //public virtual tb_FM_Subject tb_subject_id { get; set; }



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
                    Type type = typeof(tb_FM_OtherExpenseDetail);
                    
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
            tb_FM_OtherExpenseDetail loctype = (tb_FM_OtherExpenseDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

