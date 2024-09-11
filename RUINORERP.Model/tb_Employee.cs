
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:47:00
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
    /// 员工表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Employee")]
    public partial class tb_Employee: BaseEntity, ICloneable
    {
        public tb_Employee()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_Employee" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Employee_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            base.PrimaryKeyID = _Employee_ID;
            SetProperty(ref _Employee_ID, value);
            }
        }

        private long _DepartmentID;
        /// <summary>
        /// 部门
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "部门")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "部门" )]
        [FKRelationAttribute("tb_Department","DepartmentID")]
        public long DepartmentID
        { 
            get{return _DepartmentID;}
            set{
            SetProperty(ref _DepartmentID, value);
            }
        }

        private string _Employee_NO;
        /// <summary>
        /// 员工编号
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_NO",ColDesc = "员工编号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Employee_NO" ,Length=20,IsNullable = false,ColumnDescription = "员工编号" )]
        public string Employee_NO
        { 
            get{return _Employee_NO;}
            set{
            SetProperty(ref _Employee_NO, value);
            }
        }

        private string _Employee_Name;
        /// <summary>
        /// 姓名
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_Name",ColDesc = "姓名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Employee_Name" ,Length=100,IsNullable = false,ColumnDescription = "姓名" )]
        public string Employee_Name
        { 
            get{return _Employee_Name;}
            set{
            SetProperty(ref _Employee_Name, value);
            }
        }

        private bool? _Gender;
        /// <summary>
        /// 性别
        /// </summary>
        [AdvQueryAttribute(ColName = "Gender",ColDesc = "性别")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Gender" ,IsNullable = true,ColumnDescription = "性别" )]
        public bool? Gender
        { 
            get{return _Gender;}
            set{
            SetProperty(ref _Gender, value);
            }
        }

        private string _Position;
        /// <summary>
        /// 职位
        /// </summary>
        [AdvQueryAttribute(ColName = "Position",ColDesc = "职位")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Position" ,Length=20,IsNullable = true,ColumnDescription = "职位" )]
        public string Position
        { 
            get{return _Position;}
            set{
            SetProperty(ref _Position, value);
            }
        }

        private int? _Marriage;
        /// <summary>
        /// 婚姻状况
        /// </summary>
        [AdvQueryAttribute(ColName = "Marriage",ColDesc = "婚姻状况")] 
        [SugarColumn(ColumnDataType = "tinyint", SqlParameterDbType ="SByte",  ColumnName = "Marriage" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "婚姻状况" )]
        public int? Marriage
        { 
            get{return _Marriage;}
            set{
            SetProperty(ref _Marriage, value);
            }
        }

        private DateTime? _Birthday;
        /// <summary>
        /// 生日
        /// </summary>
        [AdvQueryAttribute(ColName = "Birthday",ColDesc = "生日")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Birthday" ,IsNullable = true,ColumnDescription = "生日" )]
        public DateTime? Birthday
        { 
            get{return _Birthday;}
            set{
            SetProperty(ref _Birthday, value);
            }
        }

        private DateTime? _StartDate;
        /// <summary>
        /// 入职时间
        /// </summary>
        [AdvQueryAttribute(ColName = "StartDate",ColDesc = "入职时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "StartDate" ,IsNullable = true,ColumnDescription = "入职时间" )]
        public DateTime? StartDate
        { 
            get{return _StartDate;}
            set{
            SetProperty(ref _StartDate, value);
            }
        }

        private string _JobTitle;
        /// <summary>
        /// 职称
        /// </summary>
        [AdvQueryAttribute(ColName = "JobTitle",ColDesc = "职称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "JobTitle" ,Length=50,IsNullable = true,ColumnDescription = "职称" )]
        public string JobTitle
        { 
            get{return _JobTitle;}
            set{
            SetProperty(ref _JobTitle, value);
            }
        }

        private string _Address;
        /// <summary>
        /// 联络地址
        /// </summary>
        [AdvQueryAttribute(ColName = "Address",ColDesc = "联络地址")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Address" ,Length=255,IsNullable = true,ColumnDescription = "联络地址" )]
        public string Address
        { 
            get{return _Address;}
            set{
            SetProperty(ref _Address, value);
            }
        }

        private string _Email;
        /// <summary>
        /// 邮件
        /// </summary>
        [AdvQueryAttribute(ColName = "Email",ColDesc = "邮件")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Email" ,Length=100,IsNullable = true,ColumnDescription = "邮件" )]
        public string Email
        { 
            get{return _Email;}
            set{
            SetProperty(ref _Email, value);
            }
        }

        private string _Education;
        /// <summary>
        /// 教育程度
        /// </summary>
        [AdvQueryAttribute(ColName = "Education",ColDesc = "教育程度")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Education" ,Length=100,IsNullable = true,ColumnDescription = "教育程度" )]
        public string Education
        { 
            get{return _Education;}
            set{
            SetProperty(ref _Education, value);
            }
        }

        private string _LanguageSkills;
        /// <summary>
        /// 外语能力
        /// </summary>
        [AdvQueryAttribute(ColName = "LanguageSkills",ColDesc = "外语能力")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "LanguageSkills" ,Length=50,IsNullable = true,ColumnDescription = "外语能力" )]
        public string LanguageSkills
        { 
            get{return _LanguageSkills;}
            set{
            SetProperty(ref _LanguageSkills, value);
            }
        }

        private string _University;
        /// <summary>
        /// 毕业院校
        /// </summary>
        [AdvQueryAttribute(ColName = "University",ColDesc = "毕业院校")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "University" ,Length=100,IsNullable = true,ColumnDescription = "毕业院校" )]
        public string University
        { 
            get{return _University;}
            set{
            SetProperty(ref _University, value);
            }
        }

        private string _IDNumber;
        /// <summary>
        /// 身份证号
        /// </summary>
        [AdvQueryAttribute(ColName = "IDNumber",ColDesc = "身份证号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "IDNumber" ,Length=30,IsNullable = true,ColumnDescription = "身份证号" )]
        public string IDNumber
        { 
            get{return _IDNumber;}
            set{
            SetProperty(ref _IDNumber, value);
            }
        }

        private DateTime? _EndDate;
        /// <summary>
        /// 离职日期
        /// </summary>
        [AdvQueryAttribute(ColName = "EndDate",ColDesc = "离职日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "EndDate" ,IsNullable = true,ColumnDescription = "离职日期" )]
        public DateTime? EndDate
        { 
            get{return _EndDate;}
            set{
            SetProperty(ref _EndDate, value);
            }
        }

        private decimal? _salary;
        /// <summary>
        /// 工资
        /// </summary>
        [AdvQueryAttribute(ColName = "salary",ColDesc = "工资")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "salary" , DecimalDigits = 6,IsNullable = true,ColumnDescription = "工资" )]
        public decimal? salary
        { 
            get{return _salary;}
            set{
            SetProperty(ref _salary, value);
            }
        }

        private string _Notes;
        /// <summary>
        /// 备注说明
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注说明")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=200,IsNullable = true,ColumnDescription = "备注说明" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
            }
        }

        private bool _Is_enabled= true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_enabled",ColDesc = "是否启用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Is_enabled" ,IsNullable = false,ColumnDescription = "是否启用" )]
        public bool Is_enabled
        { 
            get{return _Is_enabled;}
            set{
            SetProperty(ref _Is_enabled, value);
            }
        }

        private bool _Is_available= true;
        /// <summary>
        /// 是否可用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_available",ColDesc = "是否可用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Is_available" ,IsNullable = false,ColumnDescription = "是否可用" )]
        public bool Is_available
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

        private string _PhoneNumber;
        /// <summary>
        /// 手机号
        /// </summary>
        [AdvQueryAttribute(ColName = "PhoneNumber",ColDesc = "手机号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PhoneNumber" ,Length=50,IsNullable = true,ColumnDescription = "手机号" )]
        public string PhoneNumber
        { 
            get{return _PhoneNumber;}
            set{
            SetProperty(ref _PhoneNumber, value);
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
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(BankAccount_id))]
        public virtual tb_BankAccount tb_bankaccount { get; set; }
        //public virtual tb_BankAccount tb_BankAccount_id { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(DepartmentID))]
        public virtual tb_Department tb_department { get; set; }
        //public virtual tb_Department tb_DepartmentID { get; set; }


        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_StockIn.Employee_ID))]
        public virtual List<tb_StockIn> tb_StockIns { get; set; }
        //tb_StockIn.Employee_ID)
        //Employee_ID.FK_TB_STOCKIN_RE_EMPLOYEE)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurOrder.Employee_ID))]
        public virtual List<tb_PurOrder> tb_PurOrders { get; set; }
        //tb_PurOrder.Employee_ID)
        //Employee_ID.FK_TB_PUROR_REFERENCE_TB_EMPLO)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_StockOut.Employee_ID))]
        public virtual List<tb_StockOut> tb_StockOuts { get; set; }
        //tb_StockOut.Employee_ID)
        //Employee_ID.FK_TB_STOCKOUT_REF_TB_EMPLOYEE)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Prod.Employee_ID))]
        public virtual List<tb_Prod> tb_Prods { get; set; }
        //tb_Prod.Employee_ID)
        //Employee_ID.FK_PRODBASE_REF_EMPLOYEE)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Stocktake.Employee_ID))]
        public virtual List<tb_Stocktake> tb_Stocktakes { get; set; }
        //tb_Stocktake.Employee_ID)
        //Employee_ID.FK_TB_STOCK_REFERENCE_TB_EMPLO)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Location.Employee_ID))]
        public virtual List<tb_Location> tb_Locations { get; set; }
        //tb_Location.Employee_ID)
        //Employee_ID.FK_TB_LOCAT_REF_TB_EMPLOP_1)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Customer.Employee_ID))]
        public virtual List<tb_Customer> tb_Customers { get; set; }
        //tb_Customer.Employee_ID)
        //Employee_ID.FK_TB_CUSTO_REFERENCE_TB_EMPLO)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_CustomerVendor.Employee_ID))]
        public virtual List<tb_CustomerVendor> tb_CustomerVendors { get; set; }
        //tb_CustomerVendor.Employee_ID)
        //Employee_ID.FK_CUSTVENDOR_REF_EMPLOYEE)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MaterialRequisition.Employee_ID))]
        public virtual List<tb_MaterialRequisition> tb_MaterialRequisitionses { get; set; }
        //tb_MaterialRequisitions.Employee_ID)
        //Employee_ID.FK_MATERREWUIS_REF_TB_EMPLO)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Customer_interaction.Employee_ID))]
        public virtual List<tb_Customer_interaction> tb_Customer_interactions { get; set; }
        //tb_Customer_interaction.Employee_ID)
        //Employee_ID.FK_CUSTOMERINTERACTIONS_RE_EMPLOYEE)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_UserInfo.Employee_ID))]
        public virtual List<tb_UserInfo> tb_UserInfos { get; set; }
        //tb_UserInfo.Employee_ID)
        //Employee_ID.FK_TB_USERI_REFERENCE_TB_EMPLO)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProductionPlan.Employee_ID))]
        public virtual List<tb_ProductionPlan> tb_ProductionPlans { get; set; }
        //tb_ProductionPlan.Employee_ID)
        //Employee_ID.FK_TB_PRODU_REFERENCE_TB_EMPLO)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_Initial_PayAndReceivable.Employee_ID))]
        public virtual List<tb_FM_Initial_PayAndReceivable> tb_FM_Initial_PayAndReceivables { get; set; }
        //tb_FM_Initial_PayAndReceivable.Employee_ID)
        //Employee_ID.FK_TB_FM_INITPR_EMPLOYEE)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MaterialReturn.Employee_ID))]
        public virtual List<tb_MaterialReturn> tb_MaterialReturns { get; set; }
        //tb_MaterialReturn.Employee_ID)
        //Employee_ID.FK_MATERRETURN_RE_EMPLOYEE)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOut.Employee_ID))]
        public virtual List<tb_SaleOut> tb_SaleOuts { get; set; }
        //tb_SaleOut.Employee_ID)
        //Employee_ID.FK_SALEOUT_RE_EMPLOYEE)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurEntry.Employee_ID))]
        public virtual List<tb_PurEntry> tb_PurEntries { get; set; }
        //tb_PurEntry.Employee_ID)
        //Employee_ID.FK_TB_PUREN_REF_TB_EMPLOYEE)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOrder.Employee_ID))]
        public virtual List<tb_SaleOrder> tb_SaleOrders { get; set; }
        //tb_SaleOrder.Employee_ID)
        //Employee_ID.FK_TB_SALEO_REFERENCE_TB_EMPLO)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PaymentBill.Employee_ID))]
        public virtual List<tb_FM_PaymentBill> tb_FM_PaymentBills { get; set; }
        //tb_FM_PaymentBill.Employee_ID)
        //Employee_ID.FK_TB_FM_PAYMENTBILL_RE_EMPLOYEE)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PrePaymentBill.Employee_ID))]
        public virtual List<tb_FM_PrePaymentBill> tb_FM_PrePaymentBills { get; set; }
        //tb_FM_PrePaymentBill.Employee_ID)
        //Employee_ID.FK_PREPAYMENTBILL_RE_EMPLOYEE)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOutRe.Employee_ID))]
        public virtual List<tb_SaleOutRe> tb_SaleOutRes { get; set; }
        //tb_SaleOutRe.Employee_ID)
        //Employee_ID.FK_TB_SARE_REF_TB_EMPLO)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurEntryRe.Employee_ID))]
        public virtual List<tb_PurEntryRe> tb_PurEntryRes { get; set; }
        //tb_PurEntryRe.Employee_ID)
        //Employee_ID.FK_TB_PURENTRYRE_EMPLO)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_OtherExpense.Employee_ID))]
        public virtual List<tb_FM_OtherExpense> tb_FM_OtherExpenses { get; set; }
        //tb_FM_OtherExpense.Employee_ID)
        //Employee_ID.FK_TB_FM_OTHEREXPENSE_REF_EMPLOYEE)
        //tb_Employee.Employee_ID)


        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_OtherExpenseDetail.Employee_ID))]
        public virtual List<tb_FM_OtherExpenseDetail> tb_FM_OtherExpenseDetails { get; set; }

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FinishedGoodsInv.Employee_ID))]
        public virtual List<tb_FinishedGoodsInv> tb_FinishedGoodsInvs { get; set; }
        //tb_FinishedGoodsInv.Employee_ID)
        //Employee_ID.FK_TB_FINISGINV_REF_TB_EMPLO)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_ExpenseClaim.Employee_ID))]
        public virtual List<tb_FM_ExpenseClaim> tb_FM_ExpenseClaims { get; set; }
        //tb_FM_ExpenseClaim.Employee_ID)
        //Employee_ID.FK_EXPENSECLAIM_REF_EMPLOYEE)
        //tb_Employee.Employee_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_ExpenseClaimDetail.Employee_ID))]
        public virtual List<tb_FM_ExpenseClaimDetail> tb_FM_ExpenseClaimDetails { get; set; }
        //tb_FM_ExpenseClaimDetail.Employee_ID)
        //Employee_ID.FK_FMEXPENSECLAIMDETAIL_REF_EMPLOYEE)
        //tb_Employee.Employee_ID)


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
                    Type type = typeof(tb_Employee);
                    
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
            tb_Employee loctype = (tb_Employee)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

