
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/19/2025 17:12:33
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
    /// 售后申请单
    /// </summary>
    [Serializable()]
    [Description("售后申请单")]
    [SugarTable("tb_AS_AfterSaleApply")]
    public partial class tb_AS_AfterSaleApply: BaseEntity, ICloneable
    {
        public tb_AS_AfterSaleApply()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("售后申请单tb_AS_AfterSaleApply" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ASApplyID;
        /// <summary>
        /// 售后申请单
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ASApplyID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "售后申请单" , IsPrimaryKey = true)]
        public long ASApplyID
        { 
            get{return _ASApplyID;}
            set{
            SetProperty(ref _ASApplyID, value);
                base.PrimaryKeyID = _ASApplyID;
            }
        }

        private string _ASApplyNo;
        /// <summary>
        /// 申请编号
        /// </summary>
        [AdvQueryAttribute(ColName = "ASApplyNo",ColDesc = "申请编号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ASApplyNo" ,Length=50,IsNullable = false,ColumnDescription = "申请编号" )]
        public string ASApplyNo
        { 
            get{return _ASApplyNo;}
            set{
            SetProperty(ref _ASApplyNo, value);
                        }
        }

        private long _CustomerVendor_ID;
        /// <summary>
        /// 申请客户
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "申请客户")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "申请客户" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long CustomerVendor_ID
        { 
            get{return _CustomerVendor_ID;}
            set{
            SetProperty(ref _CustomerVendor_ID, value);
                        }
        }

        private string _CustomerSourceNo;
        /// <summary>
        /// 来源单号
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerSourceNo",ColDesc = "来源单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomerSourceNo" ,Length=50,IsNullable = true,ColumnDescription = "来源单号" )]
        public string CustomerSourceNo
        { 
            get{return _CustomerSourceNo;}
            set{
            SetProperty(ref _CustomerSourceNo, value);
                        }
        }

        private long _Location_ID;
        /// <summary>
        /// 售后暂存仓
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "售后暂存仓")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Location_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "售后暂存仓" )]
        [FKRelationAttribute("tb_Location","Location_ID")]
        public long Location_ID
        { 
            get{return _Location_ID;}
            set{
            SetProperty(ref _Location_ID, value);
                        }
        }

        private int _Priority= ((0));
        /// <summary>
        /// 紧急程度
        /// </summary>
        [AdvQueryAttribute(ColName = "Priority",ColDesc = "紧急程度")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Priority" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "紧急程度" )]
        public int Priority
        { 
            get{return _Priority;}
            set{
            SetProperty(ref _Priority, value);
                        }
        }

        private int _ASProcessStatus;
        /// <summary>
        /// 处理状态
        /// </summary>
        [AdvQueryAttribute(ColName = "ASProcessStatus",ColDesc = "处理状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ASProcessStatus" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "处理状态" )]
        public int ASProcessStatus
        { 
            get{return _ASProcessStatus;}
            set{
            SetProperty(ref _ASProcessStatus, value);
                        }
        }

        private long _Employee_ID;
        /// <summary>
        /// 业务员
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "业务员")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "业务员" )]
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

        private int _TotalInitialQuantity= ((0));
        /// <summary>
        /// 登记数量
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalInitialQuantity",ColDesc = "登记数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "TotalInitialQuantity" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "登记数量" )]
        public int TotalInitialQuantity
        { 
            get{return _TotalInitialQuantity;}
            set{
            SetProperty(ref _TotalInitialQuantity, value);
                        }
        }

        private int _TotalConfirmedQuantity= ((0));
        /// <summary>
        /// 复核数量
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalConfirmedQuantity",ColDesc = "复核数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "TotalConfirmedQuantity" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "复核数量" )]
        public int TotalConfirmedQuantity
        { 
            get{return _TotalConfirmedQuantity;}
            set{
            SetProperty(ref _TotalConfirmedQuantity, value);
                        }
        }

        private DateTime _ApplyDate;
        /// <summary>
        /// 申请日期
        /// </summary>
        [AdvQueryAttribute(ColName = "ApplyDate",ColDesc = "申请日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "ApplyDate" ,IsNullable = false,ColumnDescription = "申请日期" )]
        public DateTime ApplyDate
        { 
            get{return _ApplyDate;}
            set{
            SetProperty(ref _ApplyDate, value);
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

        private string _ShippingAddress;
        /// <summary>
        /// 收货地址
        /// </summary>
        [AdvQueryAttribute(ColName = "ShippingAddress",ColDesc = "收货地址")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ShippingAddress" ,Length=500,IsNullable = true,ColumnDescription = "收货地址" )]
        public string ShippingAddress
        { 
            get{return _ShippingAddress;}
            set{
            SetProperty(ref _ShippingAddress, value);
                        }
        }

        private string _ShippingWay;
        /// <summary>
        /// 发货方式
        /// </summary>
        [AdvQueryAttribute(ColName = "ShippingWay",ColDesc = "发货方式")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ShippingWay" ,Length=50,IsNullable = true,ColumnDescription = "发货方式" )]
        public string ShippingWay
        { 
            get{return _ShippingWay;}
            set{
            SetProperty(ref _ShippingWay, value);
                        }
        }

        private bool? _InWarrantyPeriod;
        /// <summary>
        /// 保修期内
        /// </summary>
        [AdvQueryAttribute(ColName = "InWarrantyPeriod",ColDesc = "保修期内")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "InWarrantyPeriod" ,IsNullable = true,ColumnDescription = "保修期内" )]
        public bool? InWarrantyPeriod
        { 
            get{return _InWarrantyPeriod;}
            set{
            SetProperty(ref _InWarrantyPeriod, value);
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

        private string _RepairEvaluationOpinion;
        /// <summary>
        /// 维修评估意见
        /// </summary>
        [AdvQueryAttribute(ColName = "RepairEvaluationOpinion",ColDesc = "维修评估意见")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "RepairEvaluationOpinion" ,Length=500,IsNullable = true,ColumnDescription = "维修评估意见" )]
        public string RepairEvaluationOpinion
        { 
            get{return _RepairEvaluationOpinion;}
            set{
            SetProperty(ref _RepairEvaluationOpinion, value);
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

        private bool _MaterialFeeConfirmed= false;
        /// <summary>
        /// 费用确认状态
        /// </summary>
        [AdvQueryAttribute(ColName = "MaterialFeeConfirmed",ColDesc = "费用确认状态")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "MaterialFeeConfirmed" ,IsNullable = false,ColumnDescription = "费用确认状态" )]
        public bool MaterialFeeConfirmed
        { 
            get{return _MaterialFeeConfirmed;}
            set{
            SetProperty(ref _MaterialFeeConfirmed, value);
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

        private string _ApprovalOpinions;
        /// <summary>
        /// 审批意见
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalOpinions",ColDesc = "审批意见")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ApprovalOpinions" ,Length=255,IsNullable = true,ColumnDescription = "审批意见" )]
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
        [Navigate(NavigateType.OneToOne, nameof(ProjectGroup_ID))]
        public virtual tb_ProjectGroup tb_projectgroup { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_AS_AfterSaleDelivery.ASApplyID))]
        public virtual List<tb_AS_AfterSaleDelivery> tb_AS_AfterSaleDeliveries { get; set; }
        //tb_AS_AfterSaleDelivery.ASApplyID)
        //ASApplyID.FK_AS_AFTERSALEDELIVERY_REF_AF_AFTERSALEAPPLY)
        //tb_AS_AfterSaleApply.ASApplyID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_AS_RepairOrder.ASApplyID))]
        public virtual List<tb_AS_RepairOrder> tb_AS_RepairOrders { get; set; }
        //tb_AS_RepairOrder.ASApplyID)
        //ASApplyID.FK_TB_AS_RE_REFERENCE_TB_AS_AF)
        //tb_AS_AfterSaleApply.ASApplyID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_AS_AfterSaleApplyDetail.ASApplyID))]
        public virtual List<tb_AS_AfterSaleApplyDetail> tb_AS_AfterSaleApplyDetails { get; set; }
        //tb_AS_AfterSaleApplyDetail.ASApplyID)
        //ASApplyID.FK_TB_AS_AF_REFERENCE_TB_AS_AF)
        //tb_AS_AfterSaleApply.ASApplyID)


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_AS_AfterSaleApply loctype = (tb_AS_AfterSaleApply)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

