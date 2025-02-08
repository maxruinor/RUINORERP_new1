
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
    /// 部门表是否分层
    /// </summary>
    [Serializable()]
    [Description("部门表是否分层")]
    [SugarTable("tb_Department")]
    public partial class tb_Department: BaseEntity, ICloneable
    {
        public tb_Department()
        {
            base.FieldNameList = fieldNameList;
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
        [SugarColumn(ColumnDataType = "char", SqlParameterDbType ="String",  ColumnName = "DepartmentCode" ,Length=20,IsNullable = false,ColumnDescription = "部门代号" )]
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

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ID))]
        public virtual tb_Company tb_company { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurOrder.DepartmentID))]
        public virtual List<tb_PurOrder> tb_PurOrders { get; set; }
        //tb_PurOrder.DepartmentID)
        //DepartmentID.FK_TB_PUROR_REFERENCE_TB_DEPAR)
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
        [Navigate(NavigateType.OneToMany, nameof(tb_MaterialRequisition.DepartmentID))]
        public virtual List<tb_MaterialRequisition> tb_MaterialRequisitions { get; set; }
        //tb_MaterialRequisition.DepartmentID)
        //DepartmentID.FK_MATERREQUI_REF_DEPAR)
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
        [Navigate(NavigateType.OneToMany, nameof(tb_PurEntry.DepartmentID))]
        public virtual List<tb_PurEntry> tb_PurEntries { get; set; }
        //tb_PurEntry.DepartmentID)
        //DepartmentID.FK_TB_PUREN_REFERENCE_TB_DEPAR)
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
        [Navigate(NavigateType.OneToMany, nameof(tb_FinishedGoodsInv.DepartmentID))]
        public virtual List<tb_FinishedGoodsInv> tb_FinishedGoodsInvs { get; set; }
        //tb_FinishedGoodsInv.DepartmentID)
        //DepartmentID.FK_TB_FINISINV_REF_TB_DEPAR)
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
        [Navigate(NavigateType.OneToMany, nameof(tb_BuyingRequisition.DepartmentID))]
        public virtual List<tb_BuyingRequisition> tb_BuyingRequisitions { get; set; }
        //tb_BuyingRequisition.DepartmentID)
        //DepartmentID.FK_BUYINGREQUISITION_REF_DEPARTMENT)
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
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_Account.DepartmentID))]
        public virtual List<tb_FM_Account> tb_FM_Accounts { get; set; }
        //tb_FM_Account.DepartmentID)
        //DepartmentID.FK_ACCOUNTS_RE_DEPARTMENT)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_Initial_PayAndReceivable.DepartmentID))]
        public virtual List<tb_FM_Initial_PayAndReceivable> tb_FM_Initial_PayAndReceivables { get; set; }
        //tb_FM_Initial_PayAndReceivable.DepartmentID)
        //DepartmentID.FK_FM_INITPR_RE_DEPARTMENT)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PaymentBill.DepartmentID))]
        public virtual List<tb_FM_PaymentBill> tb_FM_PaymentBills { get; set; }
        //tb_FM_PaymentBill.DepartmentID)
        //DepartmentID.FK_FM_PAYMENTBILL_RE_DEPARMENT)
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
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PrePaymentBill.DepartmentID))]
        public virtual List<tb_FM_PrePaymentBill> tb_FM_PrePaymentBills { get; set; }
        //tb_FM_PrePaymentBill.DepartmentID)
        //DepartmentID.FK_FM_PREPAYMENTBILL_DEPARTMENT)
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
        [Navigate(NavigateType.OneToMany, nameof(tb_ManufacturingOrder.DepartmentID))]
        public virtual List<tb_ManufacturingOrder> tb_ManufacturingOrders { get; set; }
        //tb_ManufacturingOrder.DepartmentID)
        //DepartmentID.FK_MANUFACTURINGORDER_REF_DEPARTMENT)
        //tb_Department.DepartmentID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MRP_ReworkReturn.DepartmentID))]
        public virtual List<tb_MRP_ReworkReturn> tb_MRP_ReworkReturns { get; set; }
        //tb_MRP_ReworkReturn.DepartmentID)
        //DepartmentID.FK_MRP_Reworkreturnl_REF_Department)
        //tb_Department.DepartmentID)


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
                    Type type = typeof(tb_Department);
                    
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
            tb_Department loctype = (tb_Department)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

