﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:24
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
    /// 采购退货入库单
    /// </summary>
    [Serializable()]
    [Description("采购退货入库单")]
    [SugarTable("tb_PurReturnEntry")]
    public partial class tb_PurReturnEntry: BaseEntity, ICloneable
    {
        public tb_PurReturnEntry()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("采购退货入库单tb_PurReturnEntry" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PurReEntry_ID;
        /// <summary>
        /// 采购退货入库单
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PurReEntry_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "采购退货入库单" , IsPrimaryKey = true)]
        public long PurReEntry_ID
        { 
            get{return _PurReEntry_ID;}
            set{
            SetProperty(ref _PurReEntry_ID, value);
                base.PrimaryKeyID = _PurReEntry_ID;
            }
        }

        private string _PurReEntryNo;
        /// <summary>
        /// 入库单号
        /// </summary>
        [AdvQueryAttribute(ColName = "PurReEntryNo",ColDesc = "入库单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PurReEntryNo" ,Length=50,IsNullable = true,ColumnDescription = "入库单号" )]
        public string PurReEntryNo
        { 
            get{return _PurReEntryNo;}
            set{
            SetProperty(ref _PurReEntryNo, value);
                        }
        }

        private long _CustomerVendor_ID;
        /// <summary>
        /// 供应商
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "供应商")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "供应商" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long CustomerVendor_ID
        { 
            get{return _CustomerVendor_ID;}
            set{
            SetProperty(ref _CustomerVendor_ID, value);
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

        private long? _Paytype_ID;
        /// <summary>
        /// 付款方式
        /// </summary>
        [AdvQueryAttribute(ColName = "Paytype_ID",ColDesc = "付款方式")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Paytype_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "付款方式" )]
        [FKRelationAttribute("tb_PaymentMethod","Paytype_ID")]
        public long? Paytype_ID
        { 
            get{return _Paytype_ID;}
            set{
            SetProperty(ref _Paytype_ID, value);
                        }
        }

        private long? _PurEntryRe_ID;
        /// <summary>
        /// 采购退货单
        /// </summary>
        [AdvQueryAttribute(ColName = "PurEntryRe_ID",ColDesc = "采购退货单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PurEntryRe_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "采购退货单" )]
        [FKRelationAttribute("tb_PurEntryRe","PurEntryRe_ID")]
        public long? PurEntryRe_ID
        { 
            get{return _PurEntryRe_ID;}
            set{
            SetProperty(ref _PurEntryRe_ID, value);
                        }
        }

        private string _PurEntryReNo;
        /// <summary>
        /// 采购退货单号
        /// </summary>
        [AdvQueryAttribute(ColName = "PurEntryReNo",ColDesc = "采购退货单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PurEntryReNo" ,Length=50,IsNullable = true,ColumnDescription = "采购退货单号" )]
        public string PurEntryReNo
        { 
            get{return _PurEntryReNo;}
            set{
            SetProperty(ref _PurEntryReNo, value);
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

        private decimal _TotalTaxAmount= ((0));
        /// <summary>
        /// 合计税额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalTaxAmount",ColDesc = "合计税额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalTaxAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "合计税额" )]
        public decimal TotalTaxAmount
        { 
            get{return _TotalTaxAmount;}
            set{
            SetProperty(ref _TotalTaxAmount, value);
                        }
        }

        private decimal _TotalAmount= ((0));
        /// <summary>
        /// 合计金额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalAmount",ColDesc = "合计金额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "合计金额" )]
        public decimal TotalAmount
        { 
            get{return _TotalAmount;}
            set{
            SetProperty(ref _TotalAmount, value);
                        }
        }

        private DateTime _BillDate;
        /// <summary>
        /// 单据日期
        /// </summary>
        [AdvQueryAttribute(ColName = "BillDate",ColDesc = "单据日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "BillDate" ,IsNullable = false,ColumnDescription = "单据日期" )]
        public DateTime BillDate
        { 
            get{return _BillDate;}
            set{
            SetProperty(ref _BillDate, value);
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

        private string _TrackNo;
        /// <summary>
        /// 物流单号
        /// </summary>
        [AdvQueryAttribute(ColName = "TrackNo",ColDesc = "物流单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "TrackNo" ,Length=50,IsNullable = true,ColumnDescription = "物流单号" )]
        public string TrackNo
        { 
            get{return _TrackNo;}
            set{
            SetProperty(ref _TrackNo, value);
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

        private bool? _IsIncludeTax;
        /// <summary>
        /// 含税
        /// </summary>
        [AdvQueryAttribute(ColName = "IsIncludeTax",ColDesc = "含税")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsIncludeTax" ,IsNullable = true,ColumnDescription = "含税" )]
        public bool? IsIncludeTax
        { 
            get{return _IsIncludeTax;}
            set{
            SetProperty(ref _IsIncludeTax, value);
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

 

        private int? _TaxDeductionType;
        /// <summary>
        /// 扣税类型
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxDeductionType",ColDesc = "扣税类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "TaxDeductionType" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "扣税类型" )]
        public int? TaxDeductionType
        { 
            get{return _TaxDeductionType;}
            set{
            SetProperty(ref _TaxDeductionType, value);
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
        [Navigate(NavigateType.OneToOne, nameof(PurEntryRe_ID))]
        public virtual tb_PurEntryRe tb_purentryre { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(CustomerVendor_ID))]
        public virtual tb_CustomerVendor tb_customervendor { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Paytype_ID))]
        public virtual tb_PaymentMethod tb_paymentmethod { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(DepartmentID))]
        public virtual tb_Department tb_department { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurReturnEntryDetail.PurReEntry_ID))]
        public virtual List<tb_PurReturnEntryDetail> tb_PurReturnEntryDetails { get; set; }
        //tb_PurReturnEntryDetail.PurReEntry_ID)
        //PurReEntry_ID.FK_TB_PURRE_REFERENCE_TB_PURRE)
        //tb_PurReturnEntry.PurReEntry_ID)


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}




 

        public override object Clone()
        {
            tb_PurReturnEntry loctype = (tb_PurReturnEntry)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

