
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:02
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
    /// 部门表是否分层
    /// </summary>
    [Serializable()]
    [Description("部门表是否分层")]
    [SugarTable("tb_Department")]
    public partial class tb_Department: BaseEntity, ICloneable
    {
        public tb_Department()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("部门表是否分层tb_Department" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _DepartmentID;
        /// <summary>
        /// 部门
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "部门" , IsPrimaryKey = true)]
        public long DepartmentID
        { 
            get{return _DepartmentID;}
            set{
            SetProperty(ref _DepartmentID, value);
                base.PrimaryKeyID = _DepartmentID;
            }
        }

        private long _ID;
        /// <summary>
        /// 所属公司
        /// </summary>
        [AdvQueryAttribute(ColName = "ID",ColDesc = "所属公司")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "所属公司" )]
        [FKRelationAttribute("tb_Company","ID")]
        public long ID
        { 
            get{return _ID;}
            set{
            SetProperty(ref _ID, value);
                        }
        }

        private string _DepartmentCode;
        /// <summary>
        /// 部门代号
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentCode",ColDesc = "部门代号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "DepartmentCode" ,Length=50,IsNullable = false,ColumnDescription = "部门代号" )]
        public string DepartmentCode
        { 
            get{return _DepartmentCode;}
            set{
            SetProperty(ref _DepartmentCode, value);
                        }
        }

        private string _DepartmentName;
        /// <summary>
        /// 部门名称
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentName",ColDesc = "部门名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "DepartmentName" ,Length=255,IsNullable = false,ColumnDescription = "部门名称" )]
        public string DepartmentName
        { 
            get{return _DepartmentName;}
            set{
            SetProperty(ref _DepartmentName, value);
                        }
        }

        private string _TEL;
        /// <summary>
        /// 电话
        /// </summary>
        [AdvQueryAttribute(ColName = "TEL",ColDesc = "电话")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "TEL" ,Length=20,IsNullable = true,ColumnDescription = "电话" )]
        public string TEL
        { 
            get{return _TEL;}
            set{
            SetProperty(ref _TEL, value);
                        }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=200,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
                        }
        }

        private string _Director;
        /// <summary>
        /// 责任人
        /// </summary>
        [AdvQueryAttribute(ColName = "Director",ColDesc = "责任人")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Director" ,Length=20,IsNullable = true,ColumnDescription = "责任人" )]
        public string Director
        { 
            get{return _Director;}
            set{
            SetProperty(ref _Director, value);
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
        [Navigate(NavigateType.OneToOne, nameof(ID))]
        public virtual tb_Company tb_company { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ManufacturingOrder.DepartmentID))]
        public virtual List<tb_ManufacturingOrder> tb_ManufacturingOrders { get; set; }
        //tb_ManufacturingOrder.DepartmentID)
        //DepartmentID.FK_MANUFACTURINGORDER_REF_DEPARTMENT)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_BOM_S.DepartmentID))]
        public virtual List<tb_BOM_S> tb_BOM_Ss { get; set; }
        //tb_BOM_S.DepartmentID)
        //DepartmentID.FK_TB_BILLO_REF_TB_DEPAR1)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_OtherExpenseDetail.DepartmentID))]
        public virtual List<tb_FM_OtherExpenseDetail> tb_FM_OtherExpenseDetails { get; set; }
        //tb_FM_OtherExpenseDetail.DepartmentID)
        //DepartmentID.FK_FM_OTHEREXPENSES_R_DEPARTMENT)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_ReceivablePayable.DepartmentID))]
        public virtual List<tb_FM_ReceivablePayable> tb_FM_ReceivablePayables { get; set; }
        //tb_FM_ReceivablePayable.DepartmentID)
        //DepartmentID.FK_tb_FM_ReceivablePayable_REFE_TB_DEPARTMENT)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Prod.DepartmentID))]
        public virtual List<tb_Prod> tb_Prods { get; set; }
        //tb_Prod.DepartmentID)
        //DepartmentID.FK_TB_PROD_REFERENCE_TB_DEPAR)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProjectGroup.DepartmentID))]
        public virtual List<tb_ProjectGroup> tb_ProjectGroups { get; set; }
        //tb_ProjectGroup.DepartmentID)
        //DepartmentID.FK_PROJECTGROUP_REF_DEPARTMENT)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MaterialReturn.DepartmentID))]
        public virtual List<tb_MaterialReturn> tb_MaterialReturns { get; set; }
        //tb_MaterialReturn.DepartmentID)
        //DepartmentID.FK_MATERIALRETURN_REF_DEPARTMENT)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_ProfitLoss.DepartmentID))]
        public virtual List<tb_FM_ProfitLoss> tb_FM_ProfitLosses { get; set; }
        //tb_FM_ProfitLoss.DepartmentID)
        //DepartmentID.FK_TB_FM_PROFITLOSS_REF_TB_DEPARTMENT)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PaymentRecordDetail.DepartmentID))]
        public virtual List<tb_FM_PaymentRecordDetail> tb_FM_PaymentRecordDetails { get; set; }
        //tb_FM_PaymentRecordDetail.DepartmentID)
        //DepartmentID.FK_PAYMENTRECORDDETAIL_REF_DEPARTMENT)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_BuyingRequisition.DepartmentID))]
        public virtual List<tb_BuyingRequisition> tb_BuyingRequisitions { get; set; }
        //tb_BuyingRequisition.DepartmentID)
        //DepartmentID.FK_BUYINGREQUISITION_REF_DEPARTMENT)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_Account.DepartmentID))]
        public virtual List<tb_FM_Account> tb_FM_Accounts { get; set; }
        //tb_FM_Account.DepartmentID)
        //DepartmentID.FK_ACCOUNTS_RE_DEPARTMENT)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PaymentApplication.DepartmentID))]
        public virtual List<tb_FM_PaymentApplication> tb_FM_PaymentApplications { get; set; }
        //tb_FM_PaymentApplication.DepartmentID)
        //DepartmentID.FK_PAYMENTAPPLICATION_REF_DEPARTMENT)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurReturnEntry.DepartmentID))]
        public virtual List<tb_PurReturnEntry> tb_PurReturnEntries { get; set; }
        //tb_PurReturnEntry.DepartmentID)
        //DepartmentID.FK_PURRETURNENTRY_REF_DEPARTMENT)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurEntryRe.DepartmentID))]
        public virtual List<tb_PurEntryRe> tb_PurEntryRes { get; set; }
        //tb_PurEntryRe.DepartmentID)
        //DepartmentID.FK_TB_PURENTRYRE__DEPARTMENT)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProductionPlan.DepartmentID))]
        public virtual List<tb_ProductionPlan> tb_ProductionPlans { get; set; }
        //tb_ProductionPlan.DepartmentID)
        //DepartmentID.FK_TB_PRODU_REFERENCE_TB_DEPAR)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Employee.DepartmentID))]
        public virtual List<tb_Employee> tb_Employees { get; set; }
        //tb_Employee.DepartmentID)
        //DepartmentID.FKTB_EMPLOTB_DEPAR_aB)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_ExpenseClaimDetail.DepartmentID))]
        public virtual List<tb_FM_ExpenseClaimDetail> tb_FM_ExpenseClaimDetails { get; set; }
        //tb_FM_ExpenseClaimDetail.DepartmentID)
        //DepartmentID.FK_FMEXCLAIMDETAIL_REF_DEPARTMENT)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MRP_ReworkEntry.DepartmentID))]
        public virtual List<tb_MRP_ReworkEntry> tb_MRP_ReworkEntries { get; set; }
        //tb_MRP_ReworkEntry.DepartmentID)
        //DepartmentID.FK_MRP_ReworkEntry_REF_Department)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MRP_ReworkReturn.DepartmentID))]
        public virtual List<tb_MRP_ReworkReturn> tb_MRP_ReworkReturns { get; set; }
        //tb_MRP_ReworkReturn.DepartmentID)
        //DepartmentID.FK_MRP_Reworkreturnl_REF_Department)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurEntry.DepartmentID))]
        public virtual List<tb_PurEntry> tb_PurEntries { get; set; }
        //tb_PurEntry.DepartmentID)
        //DepartmentID.FK_TB_PUREN_REFERENCE_TB_DEPAR)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PriceAdjustment.DepartmentID))]
        public virtual List<tb_FM_PriceAdjustment> tb_FM_PriceAdjustments { get; set; }
        //tb_FM_PriceAdjustment.DepartmentID)
        //DepartmentID.FK_PRICEADJUSTMENT_REF_DEPARTMENT)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FinishedGoodsInv.DepartmentID))]
        public virtual List<tb_FinishedGoodsInv> tb_FinishedGoodsInvs { get; set; }
        //tb_FinishedGoodsInv.DepartmentID)
        //DepartmentID.FK_TB_FINISINV_REF_TB_DEPAR)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_CRM_Customer.DepartmentID))]
        public virtual List<tb_CRM_Customer> tb_CRM_Customers { get; set; }
        //tb_CRM_Customer.DepartmentID)
        //DepartmentID.FK_CUSTOMER_REF_DEPARTMENT)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PreReceivedPayment.DepartmentID))]
        public virtual List<tb_FM_PreReceivedPayment> tb_FM_PreReceivedPayments { get; set; }
        //tb_FM_PreReceivedPayment.DepartmentID)
        //DepartmentID.FK_TB_FM_PRERECEIVEDPAYMNET_REF_DEPARTMENT)
        //tb_Department.DepartmentID)


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_Department loctype = (tb_Department)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

