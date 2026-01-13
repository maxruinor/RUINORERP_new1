
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:41:47
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
    /// 付款账号管理
    /// </summary>
    [Serializable()]
    [Description("付款账号管理")]
    [SugarTable("tb_FM_Account")]
    public partial class tb_FM_Account: BaseEntity, ICloneable
    {
        public tb_FM_Account()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("付款账号管理tb_FM_Account" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Account_id;
        /// <summary>
        /// 公司账户
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Account_id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "公司账户" , IsPrimaryKey = true)]
        public long Account_id
        { 
            get{return _Account_id;}
            set{
            SetProperty(ref _Account_id, value);
                base.PrimaryKeyID = _Account_id;
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

        
        private long? _ID;
        /// <summary>
        /// 所属公司
        /// </summary>
        [AdvQueryAttribute(ColName = "ID",ColDesc = "所属公司")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "所属公司" )]
        [FKRelationAttribute("tb_Company","ID")]
        public long? ID
        { 
            get{return _ID;}
            set{
            SetProperty(ref _ID, value);
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

        private string _Account_name;
        /// <summary>
        /// 账户名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Account_name",ColDesc = "账户名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Account_name" ,Length=50,IsNullable = true,ColumnDescription = "账户名称" )]
        public string Account_name
        { 
            get{return _Account_name;}
            set{
            SetProperty(ref _Account_name, value);
                        }
        }

        private string _Account_No;
        /// <summary>
        /// 账号
        /// </summary>
        [AdvQueryAttribute(ColName = "Account_No",ColDesc = "账号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Account_No" ,Length=100,IsNullable = true,ColumnDescription = "账号" )]
        public string Account_No
        { 
            get{return _Account_No;}
            set{
            SetProperty(ref _Account_No, value);
                        }
        }

        private int? _Account_type;
        /// <summary>
        /// 账户类型
        /// </summary>
        [AdvQueryAttribute(ColName = "Account_type",ColDesc = "账户类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Account_type" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "账户类型" )]
        public int? Account_type
        { 
            get{return _Account_type;}
            set{
            SetProperty(ref _Account_type, value);
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

        private decimal _OpeningBalance= ((0));
        /// <summary>
        /// 初始余额
        /// </summary>
        [AdvQueryAttribute(ColName = "OpeningBalance",ColDesc = "初始余额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "OpeningBalance" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "初始余额" )]
        public decimal OpeningBalance
        { 
            get{return _OpeningBalance;}
            set{
            SetProperty(ref _OpeningBalance, value);
                        }
        }

        private decimal _CurrentBalance= ((0));
        /// <summary>
        /// 当前余额
        /// </summary>
        [AdvQueryAttribute(ColName = "CurrentBalance",ColDesc = "当前余额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "CurrentBalance" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "当前余额" )]
        public decimal CurrentBalance
        { 
            get{return _CurrentBalance;}
            set{
            SetProperty(ref _CurrentBalance, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(DepartmentID))]
        public virtual tb_Department tb_department { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Currency_ID))]
        public virtual tb_Currency tb_currency { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ID))]
        public virtual tb_Company tb_company { get; set; }

 

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_OtherExpenseDetail.Account_id))]
        public virtual List<tb_FM_OtherExpenseDetail> tb_FM_OtherExpenseDetails { get; set; }
        //tb_FM_OtherExpenseDetail.Account_id)
        //Account_id.FK_TB_FM_OT_REFERENCE_TB_FM_AC)
        //tb_FM_Account.Account_id)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_Statement.Account_id))]
        public virtual List<tb_FM_Statement> tb_FM_Statements { get; set; }
        //tb_FM_Statement.Account_id)
        //Account_id.FK_FM_STATEMENT_REF_FM_ACCOUNT)
        //tb_FM_Account.Account_id)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PaymentRecord.Account_id))]
        public virtual List<tb_FM_PaymentRecord> tb_FM_PaymentRecords { get; set; }
        //tb_FM_PaymentRecord.Account_id)
        //Account_id.FK_TB_FM_PA_REFERENCE_TB_FM_AC)
        //tb_FM_Account.Account_id)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PaymentApplication.Account_id))]
        public virtual List<tb_FM_PaymentApplication> tb_FM_PaymentApplications { get; set; }
        //tb_FM_PaymentApplication.Account_id)
        //Account_id.FK_PAYMENTAPPLICATION_REF_ACCOUNT)
        //tb_FM_Account.Account_id)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_ExpenseClaimDetail.Account_id))]
        public virtual List<tb_FM_ExpenseClaimDetail> tb_FM_ExpenseClaimDetails { get; set; }
        //tb_FM_ExpenseClaimDetail.Account_id)
        //Account_id.FK_EXPENSECLAIMDETAIL_REF_ACCOUNT)
        //tb_FM_Account.Account_id)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PaymentSettlement.Account_id))]
        public virtual List<tb_FM_PaymentSettlement> tb_FM_PaymentSettlements { get; set; }
        //tb_FM_PaymentSettlement.Account_id)
        //Account_id.FK_FM_PAYMENTSETTLEMENT_REF_FM_ACCOUNT)
        //tb_FM_Account.Account_id)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProjectGroupAccountMapper.Account_id))]
        public virtual List<tb_ProjectGroupAccountMapper> tb_ProjectGroupAccountMappers { get; set; }
        //tb_ProjectGroupAccountMapper.Account_id)
        //Account_id.FK_PROJECTGROUPACCOUNTMAPPER_REF_FM_ACCOUNT)
        //tb_FM_Account.Account_id)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PreReceivedPayment.Account_id))]
        public virtual List<tb_FM_PreReceivedPayment> tb_FM_PreReceivedPayments { get; set; }
        //tb_FM_PreReceivedPayment.Account_id)
        //Account_id.FK_FM_PRERECEIVEDPAYMENT_REF_FM_ACCOUNT)
        //tb_FM_Account.Account_id)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PayMethodAccountMapper.Account_id))]
        public virtual List<tb_PayMethodAccountMapper> tb_PayMethodAccountMappers { get; set; }
        //tb_PayMethodAccountMapper.Account_id)
        //Account_id.FK_PAYMETHODACCOUNTMAPPER_REF_FM_ACCOUNT)
        //tb_FM_Account.Account_id)


        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_FM_Account loctype = (tb_FM_Account)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

