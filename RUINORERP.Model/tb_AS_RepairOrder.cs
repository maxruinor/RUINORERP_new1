﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:25:42
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
    /// 维修工单  工时费 材料费
    /// </summary>
    [Serializable()]
    [Description("维修工单  工时费 材料费")]
    [SugarTable("tb_AS_RepairOrder")]
    public partial class tb_AS_RepairOrder: BaseEntity, ICloneable
    {
        public tb_AS_RepairOrder()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("维修工单  工时费 材料费tb_AS_RepairOrder" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _RepairOrderID;
        /// <summary>
        /// 维修工单
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RepairOrderID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "维修工单" , IsPrimaryKey = true)]
        public long RepairOrderID
        { 
            get{return _RepairOrderID;}
            set{
            SetProperty(ref _RepairOrderID, value);
                base.PrimaryKeyID = _RepairOrderID;
            }
        }

        private string _RepairOrderNo;
        /// <summary>
        /// 维修工单号
        /// </summary>
        [AdvQueryAttribute(ColName = "RepairOrderNo",ColDesc = "维修工单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "RepairOrderNo" ,Length=50,IsNullable = true,ColumnDescription = "维修工单号" )]
        public string RepairOrderNo
        { 
            get{return _RepairOrderNo;}
            set{
            SetProperty(ref _RepairOrderNo, value);
                        }
        }

        private long? _ASApplyID;
        /// <summary>
        /// 售后申请单
        /// </summary>
        [AdvQueryAttribute(ColName = "ASApplyID",ColDesc = "售后申请单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ASApplyID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "售后申请单" )]
        [FKRelationAttribute("tb_AS_AfterSaleApply","ASApplyID")]
        public long? ASApplyID
        { 
            get{return _ASApplyID;}
            set{
            SetProperty(ref _ASApplyID, value);
                        }
        }

        private string _ASApplyNo;
        /// <summary>
        /// 售后申请编号
        /// </summary>
        [AdvQueryAttribute(ColName = "ASApplyNo",ColDesc = "售后申请编号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ASApplyNo" ,Length=50,IsNullable = true,ColumnDescription = "售后申请编号" )]
        public string ASApplyNo
        { 
            get{return _ASApplyNo;}
            set{
            SetProperty(ref _ASApplyNo, value);
                        }
        }

        private long _Employee_ID;
        /// <summary>
        /// 经办人员
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "经办人员")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "经办人员" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
                        }
        }

        private long? _ProjectGroup_ID;
        /// <summary>
        /// 项目小组
        /// </summary>
        [AdvQueryAttribute(ColName = "ProjectGroup_ID",ColDesc = "项目小组")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProjectGroup_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "项目小组" )]
        [FKRelationAttribute("tb_ProjectGroup","ProjectGroup_ID")]
        public long? ProjectGroup_ID
        { 
            get{return _ProjectGroup_ID;}
            set{
            SetProperty(ref _ProjectGroup_ID, value);
                        }
        }

        private long? _CustomerVendor_ID;
        /// <summary>
        /// 所属客户
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "所属客户")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "所属客户" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long? CustomerVendor_ID
        { 
            get{return _CustomerVendor_ID;}
            set{
            SetProperty(ref _CustomerVendor_ID, value);
                        }
        }

        private int? _RepairStatus= ((0));
        /// <summary>
        /// 维修状态
        /// </summary>
        [AdvQueryAttribute(ColName = "RepairStatus",ColDesc = "维修状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "RepairStatus" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "维修状态" )]
        public int? RepairStatus
        { 
            get{return _RepairStatus;}
            set{
            SetProperty(ref _RepairStatus, value);
                        }
        }

        private int _PayStatus;
        /// <summary>
        /// 付款状态
        /// </summary>
        [AdvQueryAttribute(ColName = "PayStatus",ColDesc = "付款状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "PayStatus" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "付款状态" )]
        public int PayStatus
        { 
            get{return _PayStatus;}
            set{
            SetProperty(ref _PayStatus, value);
                        }
        }

        private long _Paytype_ID;
        /// <summary>
        /// 付款方式
        /// </summary>
        [AdvQueryAttribute(ColName = "Paytype_ID",ColDesc = "付款方式")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Paytype_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "付款方式" )]
        [FKRelationAttribute("tb_PaymentMethod","Paytype_ID")]
        public long Paytype_ID
        { 
            get{return _Paytype_ID;}
            set{
            SetProperty(ref _Paytype_ID, value);
                        }
        }

        private int _TotalQty= ((0));
        /// <summary>
        /// 总数量
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalQty",ColDesc = "总数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "TotalQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "总数量" )]
        public int TotalQty
        { 
            get{return _TotalQty;}
            set{
            SetProperty(ref _TotalQty, value);
                        }
        }

        private int _TotalDeliveredQty= ((0));
        /// <summary>
        /// 交付数量
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalDeliveredQty",ColDesc = "交付数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "TotalDeliveredQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "交付数量" )]
        public int TotalDeliveredQty
        { 
            get{return _TotalDeliveredQty;}
            set{
            SetProperty(ref _TotalDeliveredQty, value);
                        }
        }

        private decimal _LaborCost;
        /// <summary>
        /// 总人工成本
        /// </summary>
        [AdvQueryAttribute(ColName = "LaborCost",ColDesc = "总人工成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "LaborCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "总人工成本" )]
        public decimal LaborCost
        { 
            get{return _LaborCost;}
            set{
            SetProperty(ref _LaborCost, value);
                        }
        }

        private decimal _TotalMaterialAmount= ((0));
        /// <summary>
        /// 总材料费用
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalMaterialAmount",ColDesc = "总材料费用")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalMaterialAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "总材料费用" )]
        public decimal TotalMaterialAmount
        { 
            get{return _TotalMaterialAmount;}
            set{
            SetProperty(ref _TotalMaterialAmount, value);
                        }
        }

        private decimal _TotalAmount= ((0));
        /// <summary>
        /// 总费用
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalAmount",ColDesc = "总费用")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "总费用" )]
        public decimal TotalAmount
        { 
            get{return _TotalAmount;}
            set{
            SetProperty(ref _TotalAmount, value);
                        }
        }

        private decimal _CustomerPaidAmount;
        /// <summary>
        /// 客户支付金额
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerPaidAmount",ColDesc = "客户支付金额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "CustomerPaidAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "客户支付金额" )]
        public decimal CustomerPaidAmount
        { 
            get{return _CustomerPaidAmount;}
            set{
            SetProperty(ref _CustomerPaidAmount, value);
                        }
        }

        private int? _ExpenseAllocationMode;
        /// <summary>
        /// 费用承担模式
        /// </summary>
        [AdvQueryAttribute(ColName = "ExpenseAllocationMode",ColDesc = "费用承担模式")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ExpenseAllocationMode" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "费用承担模式" )]
        public int? ExpenseAllocationMode
        { 
            get{return _ExpenseAllocationMode;}
            set{
            SetProperty(ref _ExpenseAllocationMode, value);
                        }
        }

        private int? _ExpenseBearerType;
        /// <summary>
        /// 费用承担方
        /// </summary>
        [AdvQueryAttribute(ColName = "ExpenseBearerType",ColDesc = "费用承担方")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ExpenseBearerType" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "费用承担方" )]
        public int? ExpenseBearerType
        { 
            get{return _ExpenseBearerType;}
            set{
            SetProperty(ref _ExpenseBearerType, value);
                        }
        }

        private DateTime _RepairStartDate;
        /// <summary>
        /// 开始日期
        /// </summary>
        [AdvQueryAttribute(ColName = "RepairStartDate",ColDesc = "开始日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "RepairStartDate" ,IsNullable = false,ColumnDescription = "开始日期" )]
        public DateTime RepairStartDate
        { 
            get{return _RepairStartDate;}
            set{
            SetProperty(ref _RepairStartDate, value);
                        }
        }

        private DateTime? _PreDeliveryDate;
        /// <summary>
        /// 预交日期
        /// </summary>
        [AdvQueryAttribute(ColName = "PreDeliveryDate",ColDesc = "预交日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "PreDeliveryDate" ,IsNullable = true,ColumnDescription = "预交日期" )]
        public DateTime? PreDeliveryDate
        { 
            get{return _PreDeliveryDate;}
            set{
            SetProperty(ref _PreDeliveryDate, value);
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

        private int _DataStatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "DataStatus",ColDesc = "数据状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DataStatus" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "数据状态" )]
        public int DataStatus
        { 
            get{return _DataStatus;}
            set{
            SetProperty(ref _DataStatus, value);
                        }
        }

        private string _ApprovalOpinions;
        /// <summary>
        /// 审批意见
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalOpinions",ColDesc = "审批意见")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ApprovalOpinions" ,Length=200,IsNullable = true,ColumnDescription = "审批意见" )]
        public string ApprovalOpinions
        { 
            get{return _ApprovalOpinions;}
            set{
            SetProperty(ref _ApprovalOpinions, value);
                        }
        }

        private long? _Approver_by;
        /// <summary>
        /// 审批人
        /// </summary>
        [AdvQueryAttribute(ColName = "Approver_by",ColDesc = "审批人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Approver_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "审批人" )]
        public long? Approver_by
        { 
            get{return _Approver_by;}
            set{
            SetProperty(ref _Approver_by, value);
                        }
        }

        private DateTime? _Approver_at;
        /// <summary>
        /// 审批时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Approver_at",ColDesc = "审批时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Approver_at" ,IsNullable = true,ColumnDescription = "审批时间" )]
        public DateTime? Approver_at
        { 
            get{return _Approver_at;}
            set{
            SetProperty(ref _Approver_at, value);
                        }
        }

        private int? _ApprovalStatus= ((0));
        /// <summary>
        /// 审批状态
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalStatus",ColDesc = "审批状态")] 
        [SugarColumn(ColumnDataType = "tinyint", SqlParameterDbType ="SByte",  ColumnName = "ApprovalStatus" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "审批状态" )]
        public int? ApprovalStatus
        { 
            get{return _ApprovalStatus;}
            set{
            SetProperty(ref _ApprovalStatus, value);
                        }
        }

        private bool? _ApprovalResults;
        /// <summary>
        /// 审批结果
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalResults",ColDesc = "审批结果")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "ApprovalResults" ,IsNullable = true,ColumnDescription = "审批结果" )]
        public bool? ApprovalResults
        { 
            get{return _ApprovalResults;}
            set{
            SetProperty(ref _ApprovalResults, value);
                        }
        }

        private int _PrintStatus= ((0));
        /// <summary>
        /// 打印状态
        /// </summary>
        [AdvQueryAttribute(ColName = "PrintStatus",ColDesc = "打印状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "PrintStatus" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "打印状态" )]
        public int PrintStatus
        { 
            get{return _PrintStatus;}
            set{
            SetProperty(ref _PrintStatus, value);
                        }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=1500,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
                        }
        }

        private decimal _TotalMaterialCost= ((0));
        /// <summary>
        /// 总材料成本
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalMaterialCost",ColDesc = "总材料成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalMaterialCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "总材料成本" )]
        public decimal TotalMaterialCost
        { 
            get{return _TotalMaterialCost;}
            set{
            SetProperty(ref _TotalMaterialCost, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(CustomerVendor_ID))]
        public virtual tb_CustomerVendor tb_customervendor { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Paytype_ID))]
        public virtual tb_PaymentMethod tb_paymentmethod { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProjectGroup_ID))]
        public virtual tb_ProjectGroup tb_projectgroup { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ASApplyID))]
        public virtual tb_AS_AfterSaleApply tb_as_aftersaleapply { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_AS_RepairInStock.RepairOrderID))]
        public virtual List<tb_AS_RepairInStock> tb_AS_RepairInStocks { get; set; }
        //tb_AS_RepairInStock.RepairOrderID)
        //RepairOrderID.FK_AS_REPAIRINSTOCK_REF_TB_AS_REPAIRORDER)
        //tb_AS_RepairOrder.RepairOrderID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_AS_RepairOrderMaterialDetail.RepairOrderID))]
        public virtual List<tb_AS_RepairOrderMaterialDetail> tb_AS_RepairOrderMaterialDetails { get; set; }
        //tb_AS_RepairOrderMaterialDetail.RepairOrderID)
        //RepairOrderID.FK_TB_AS_REPAIRMATERIALDETAIL_REF_AS_REPAIRORDER)
        //tb_AS_RepairOrder.RepairOrderID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_AS_RepairOrderDetail.RepairOrderID))]
        public virtual List<tb_AS_RepairOrderDetail> tb_AS_RepairOrderDetails { get; set; }
        //tb_AS_RepairOrderDetail.RepairOrderID)
        //RepairOrderID.FK_tb_AS_RepairOrder_REF_AS_RepairOrderDetail)
        //tb_AS_RepairOrder.RepairOrderID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_AS_RepairMaterialPickup.RepairOrderID))]
        public virtual List<tb_AS_RepairMaterialPickup> tb_AS_RepairMaterialPickups { get; set; }
        //tb_AS_RepairMaterialPickup.RepairOrderID)
        //RepairOrderID.FK_TB_AS_REPAIRMATERIALPICKUP_REF_TB_AS_REPAIRORDER)
        //tb_AS_RepairOrder.RepairOrderID)


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_AS_RepairOrder loctype = (tb_AS_RepairOrder)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

