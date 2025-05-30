﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:06
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
    /// 返工入库
    /// </summary>
    [Serializable()]
    [Description("返工入库")]
    [SugarTable("tb_MRP_ReworkEntry")]
    public partial class tb_MRP_ReworkEntry: BaseEntity, ICloneable
    {
        public tb_MRP_ReworkEntry()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("返工入库tb_MRP_ReworkEntry" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ReworkEntryID;
        /// <summary>
        /// 返工入库单
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ReworkEntryID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "返工入库单" , IsPrimaryKey = true)]
        public long ReworkEntryID
        { 
            get{return _ReworkEntryID;}
            set{
            SetProperty(ref _ReworkEntryID, value);
                base.PrimaryKeyID = _ReworkEntryID;
            }
        }

        private string _ReworkEntryNo;
        /// <summary>
        /// 返工入库单号
        /// </summary>
        [AdvQueryAttribute(ColName = "ReworkEntryNo",ColDesc = "返工入库单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ReworkEntryNo" ,Length=50,IsNullable = false,ColumnDescription = "返工入库单号" )]
        public string ReworkEntryNo
        { 
            get{return _ReworkEntryNo;}
            set{
            SetProperty(ref _ReworkEntryNo, value);
                        }
        }
        private bool _IsOutSourced = false;
        /// <summary>
        /// 是否托工
        /// </summary>
        [AdvQueryAttribute(ColName = "IsOutSourced", ColDesc = "是否托工")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "IsOutSourced", IsNullable = false, ColumnDescription = "是否托工")]
        public bool IsOutSourced
        {
            get { return _IsOutSourced; }
            set
            {
                SetProperty(ref _IsOutSourced, value);
            }
        }
        private long? _CustomerVendor_ID;
        /// <summary>
        /// 外发工厂
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "外发工厂")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "外发工厂")]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long? CustomerVendor_ID
        { 
            get{return _CustomerVendor_ID;}
            set{
            SetProperty(ref _CustomerVendor_ID, value);
                        }
        }

        private long? _DepartmentID;
        /// <summary>
        /// 需求部门
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "需求部门")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "需求部门" )]
        [FKRelationAttribute("tb_Department","DepartmentID")]
        public long? DepartmentID
        { 
            get{return _DepartmentID;}
            set{
            SetProperty(ref _DepartmentID, value);
                        }
        }

        private long _Employee_ID;
        /// <summary>
        /// 经办人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "经办人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "经办人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
                        }
        }

        private long _ReworkReturnID;
        /// <summary>
        /// 退库单
        /// </summary>
        [AdvQueryAttribute(ColName = "ReworkReturnID",ColDesc = "退库单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ReworkReturnID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "退库单" )]
        [FKRelationAttribute("tb_MRP_ReworkReturn","ReworkReturnID")]
        public long ReworkReturnID
        { 
            get{return _ReworkReturnID;}
            set{
            SetProperty(ref _ReworkReturnID, value);
                        }
        }

        private string _ReworkReturnNo;
        /// <summary>
        /// 退库单号
        /// </summary>
        [AdvQueryAttribute(ColName = "ReworkReturnNo",ColDesc = "退库单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ReworkReturnNo" ,Length=50,IsNullable = false,ColumnDescription = "退库单号" )]
        public string ReworkReturnNo
        { 
            get{return _ReworkReturnNo;}
            set{
            SetProperty(ref _ReworkReturnNo, value);
                        }
        }

        private int _TotalQty= ((0));
        /// <summary>
        /// 合计数量
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalQty",ColDesc = "合计数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "TotalQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "合计数量" )]
        public int TotalQty
        { 
            get{return _TotalQty;}
            set{
            SetProperty(ref _TotalQty, value);
                        }
        }

        private decimal _TotalReworkFee= ((0));
        /// <summary>
        /// 预估费用
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalReworkFee",ColDesc = "预估费用")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalReworkFee" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "预估费用" )]
        public decimal TotalReworkFee
        { 
            get{return _TotalReworkFee;}
            set{
            SetProperty(ref _TotalReworkFee, value);
                        }
        }

        private decimal _TotalCost= ((0));
        /// <summary>
        /// 合计成本
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalCost",ColDesc = "合计成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "合计成本" )]
        public decimal TotalCost
        { 
            get{return _TotalCost;}
            set{
            SetProperty(ref _TotalCost, value);
                        }
        }

        private DateTime _EntryDate;
        /// <summary>
        /// 入库日期
        /// </summary>
        [AdvQueryAttribute(ColName = "EntryDate",ColDesc = "入库日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "EntryDate" ,IsNullable = false,ColumnDescription = "入库日期" )]
        public DateTime EntryDate
        { 
            get{return _EntryDate;}
            set{
            SetProperty(ref _EntryDate, value);
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

        private int? _ApprovalStatus;
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

        private int? _KeepAccountsType;
        /// <summary>
        /// 立帐类型
        /// </summary>
        [AdvQueryAttribute(ColName = "KeepAccountsType",ColDesc = "立帐类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "KeepAccountsType" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "立帐类型" )]
        public int? KeepAccountsType
        { 
            get{return _KeepAccountsType;}
            set{
            SetProperty(ref _KeepAccountsType, value);
                        }
        }

        private bool? _ReceiptInvoiceClosed;
        /// <summary>
        /// 立帐结案
        /// </summary>
        [AdvQueryAttribute(ColName = "ReceiptInvoiceClosed",ColDesc = "立帐结案")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "ReceiptInvoiceClosed" ,IsNullable = true,ColumnDescription = "立帐结案" )]
        public bool? ReceiptInvoiceClosed
        { 
            get{return _ReceiptInvoiceClosed;}
            set{
            SetProperty(ref _ReceiptInvoiceClosed, value);
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

        private bool? _GenerateVouchers;
        /// <summary>
        /// 生成凭证
        /// </summary>
        [AdvQueryAttribute(ColName = "GenerateVouchers",ColDesc = "生成凭证")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "GenerateVouchers" ,IsNullable = true,ColumnDescription = "生成凭证" )]
        public bool? GenerateVouchers
        { 
            get{return _GenerateVouchers;}
            set{
            SetProperty(ref _GenerateVouchers, value);
                        }
        }

        private string _VoucherNO;
        /// <summary>
        /// 凭证号码
        /// </summary>
        [AdvQueryAttribute(ColName = "VoucherNO",ColDesc = "凭证号码")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "VoucherNO" ,Length=50,IsNullable = true,ColumnDescription = "凭证号码" )]
        public string VoucherNO
        { 
            get{return _VoucherNO;}
            set{
            SetProperty(ref _VoucherNO, value);
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
        [Navigate(NavigateType.OneToOne, nameof(DepartmentID))]
        public virtual tb_Department tb_department { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ReworkReturnID))]
        public virtual tb_MRP_ReworkReturn tb_mrp_reworkreturn { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MRP_ReworkEntryDetail.ReworkEntryID))]
        public virtual List<tb_MRP_ReworkEntryDetail> tb_MRP_ReworkEntryDetails { get; set; }
        //tb_MRP_ReworkEntryDetail.ReworkEntryID)
        //ReworkEntryID.FK_TB_MRP_ReworkEntry_REF_MRP_ReworkEntryDetail)
        //tb_MRP_ReworkEntry.ReworkEntryID)


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}







        public override object Clone()
        {
            tb_MRP_ReworkEntry loctype = (tb_MRP_ReworkEntry)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

