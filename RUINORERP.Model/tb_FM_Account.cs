
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:47:10
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
    /// 账户管理，财务系统中使用
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FM_Account")]
    public partial class tb_FM_Account: BaseEntity, ICloneable
    {
        public tb_FM_Account()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_FM_Account" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _account_id;
        /// <summary>
        /// 账户
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "account_id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "账户" , IsPrimaryKey = true)]
        public long account_id
        { 
            get{return _account_id;}
            set{
            base.PrimaryKeyID = _account_id;
            SetProperty(ref _account_id, value);
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

        private long? _Currency_ID;
        /// <summary>
        /// 币种
        /// </summary>
        [AdvQueryAttribute(ColName = "Currency_ID",ColDesc = "币种")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Currency_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "币种" )]
        [FKRelationAttribute("tb_Currency","Currency_ID")]
        public long? Currency_ID
        { 
            get{return _Currency_ID;}
            set{
            SetProperty(ref _Currency_ID, value);
            }
        }

        private string _account_name;
        /// <summary>
        /// 账户名称
        /// </summary>
        [AdvQueryAttribute(ColName = "account_name",ColDesc = "账户名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "account_name" ,Length=50,IsNullable = true,ColumnDescription = "账户名称" )]
        public string account_name
        { 
            get{return _account_name;}
            set{
            SetProperty(ref _account_name, value);
            }
        }

        private string _account_No;
        /// <summary>
        /// 账号
        /// </summary>
        [AdvQueryAttribute(ColName = "account_No",ColDesc = "账号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "account_No" ,Length=100,IsNullable = true,ColumnDescription = "账号" )]
        public string account_No
        { 
            get{return _account_No;}
            set{
            SetProperty(ref _account_No, value);
            }
        }

        private int? _account_type;
        /// <summary>
        /// 账户类型
        /// </summary>
        [AdvQueryAttribute(ColName = "account_type",ColDesc = "账户类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "account_type" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "账户类型" )]
        public int? account_type
        { 
            get{return _account_type;}
            set{
            SetProperty(ref _account_type, value);
            }
        }

        private string _Bank;
        /// <summary>
        /// 所属银行
        /// </summary>
        [AdvQueryAttribute(ColName = "Bank",ColDesc = "所属银行")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Bank" ,Length=30,IsNullable = true,ColumnDescription = "所属银行" )]
        public string Bank
        { 
            get{return _Bank;}
            set{
            SetProperty(ref _Bank, value);
            }
        }

        private decimal? _OpeningBalance;
        /// <summary>
        /// 初始余额
        /// </summary>
        [AdvQueryAttribute(ColName = "OpeningBalance",ColDesc = "初始余额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "OpeningBalance" , DecimalDigits = 6,IsNullable = true,ColumnDescription = "初始余额" )]
        public decimal? OpeningBalance
        { 
            get{return _OpeningBalance;}
            set{
            SetProperty(ref _OpeningBalance, value);
            }
        }

        private decimal? _CurrentBalance;
        /// <summary>
        /// 当前余额
        /// </summary>
        [AdvQueryAttribute(ColName = "CurrentBalance",ColDesc = "当前余额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "CurrentBalance" , DecimalDigits = 6,IsNullable = true,ColumnDescription = "当前余额" )]
        public decimal? CurrentBalance
        { 
            get{return _CurrentBalance;}
            set{
            SetProperty(ref _CurrentBalance, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(DepartmentID))]
        public virtual tb_Department tb_department { get; set; }
        //public virtual tb_Department tb_DepartmentID { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Currency_ID))]
        public virtual tb_Currency tb_currency { get; set; }
        //public virtual tb_Currency tb_Currency_ID { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(subject_id))]
        public virtual tb_FM_Subject tb_fm_subject { get; set; }
        //public virtual tb_FM_Subject tb_subject_id { get; set; }


        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_Initial_PayAndReceivable.account_id))]
        public virtual List<tb_FM_Initial_PayAndReceivable> tb_FM_Initial_PayAndReceivables { get; set; }
        //tb_FM_Initial_PayAndReceivable.account_id)
        //account_id.FK_TB_FM_IN_REFERENCE_TB_FM_AC)
        //tb_FM_Account.account_id)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PaymentBill.account_id))]
        public virtual List<tb_FM_PaymentBill> tb_FM_PaymentBills { get; set; }
        //tb_FM_PaymentBill.account_id)
        //account_id.FK_FM_PAYMENTBILL_REF_ACCOUNTS)
        //tb_FM_Account.account_id)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_OtherExpenseDetail.account_id))]
        public virtual List<tb_FM_OtherExpenseDetail> tb_FM_OtherExpenseDetails { get; set; }
        //tb_FM_OtherExpenseDetail.account_id)
        //account_id.FK_TB_FM_OT_REFERENCE_TB_FM_AC)
        //tb_FM_Account.account_id)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PrePaymentBillDetail.account_id))]
        public virtual List<tb_FM_PrePaymentBillDetail> tb_FM_PrePaymentBillDetails { get; set; }
        //tb_FM_PrePaymentBillDetail.account_id)
        //account_id.FK_PREPAYMENTBILLDETAIL_R_ACCOUNTS)
        //tb_FM_Account.account_id)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_ExpenseClaimDetail.account_id))]
        public virtual List<tb_FM_ExpenseClaimDetail> tb_FM_ExpenseClaimDetails { get; set; }
        //tb_FM_ExpenseClaimDetail.account_id)
        //account_id.FK_EXPENSECLAIMDETAIL_REF_ACCOUNT)
        //tb_FM_Account.account_id)


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
                    Type type = typeof(tb_FM_Account);
                    
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
            tb_FM_Account loctype = (tb_FM_Account)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

