﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:12
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{
    /// <summary>
    /// 产品借出单
    /// </summary>
    [Serializable()]
    [Description("产品借出单")]
    [SugarTable("tb_ProdBorrowing")]
    public partial class tb_ProdBorrowing : BaseEntity, ICloneable
    {
        public tb_ProdBorrowing()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("产品借出单tb_ProdBorrowing" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _BorrowID;
        /// <summary>
        /// 
        /// </summary>

        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "BorrowID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "", IsPrimaryKey = true)]
        public long BorrowID
        {
            get { return _BorrowID; }
            set
            {
                SetProperty(ref _BorrowID, value);
                base.PrimaryKeyID = _BorrowID;
            }
        }

        private long? _CustomerVendor_ID;
        /// <summary>
        /// 接收单位
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID", ColDesc = "接收单位")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "CustomerVendor_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "接收单位")]
        [FKRelationAttribute("tb_CustomerVendor", "CustomerVendor_ID")]
        public long? CustomerVendor_ID
        {
            get { return _CustomerVendor_ID; }
            set
            {
                SetProperty(ref _CustomerVendor_ID, value);
            }
        }


        private bool _IsVendor = false;
        /// <summary>
        /// 是供应商
        /// </summary>
        [AdvQueryAttribute(ColName = "IsVendor", ColDesc = "是供应商")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "IsVendor", IsNullable = false, ColumnDescription = "是供应商")]
        public bool IsVendor
        {
            get { return _IsVendor; }
            set
            {
                SetProperty(ref _IsVendor, value);
            }
        }

        private long _Employee_ID;
        /// <summary>
        /// 借出人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID", ColDesc = "借出人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Employee_ID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "借出人")]
        [FKRelationAttribute("tb_Employee", "Employee_ID")]
        public long Employee_ID
        {
            get { return _Employee_ID; }
            set
            {
                SetProperty(ref _Employee_ID, value);
            }
        }

        private string _BorrowNo;
        /// <summary>
        /// 借出单号
        /// </summary>
        [AdvQueryAttribute(ColName = "BorrowNo", ColDesc = "借出单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "BorrowNo", Length = 50, IsNullable = true, ColumnDescription = "借出单号")]
        public string BorrowNo
        {
            get { return _BorrowNo; }
            set
            {
                SetProperty(ref _BorrowNo, value);
            }
        }

        private int _TotalQty = ((0));
        /// <summary>
        /// 总数量
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalQty", ColDesc = "总数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "TotalQty", DecimalDigits = 0, IsNullable = false, ColumnDescription = "总数量")]
        public int TotalQty
        {
            get { return _TotalQty; }
            set
            {
                SetProperty(ref _TotalQty, value);
            }
        }

        private decimal _TotalCost = ((0));
        /// <summary>
        /// 总成本
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalCost", ColDesc = "总成本")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalCost", DecimalDigits = 4, IsNullable = false, ColumnDescription = "总成本")]
        public decimal TotalCost
        {
            get { return _TotalCost; }
            set
            {
                SetProperty(ref _TotalCost, value);
            }
        }

        private decimal _TotalAmount = ((0));
        /// <summary>
        /// 总金额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalAmount", ColDesc = "总金额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "总金额")]
        public decimal TotalAmount
        {
            get { return _TotalAmount; }
            set
            {
                SetProperty(ref _TotalAmount, value);
            }
        }

        private DateTime? _DueDate;
        /// <summary>
        /// 到期日期
        /// </summary>
        [AdvQueryAttribute(ColName = "DueDate", ColDesc = "到期日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "DueDate", IsNullable = true, ColumnDescription = "到期日期")]
        public DateTime? DueDate
        {
            get { return _DueDate; }
            set
            {
                SetProperty(ref _DueDate, value);
            }
        }

        private DateTime? _Out_date;
        /// <summary>
        /// 出库日期
        /// </summary>
        [AdvQueryAttribute(ColName = "Out_date", ColDesc = "出库日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "Out_date", IsNullable = true, ColumnDescription = "出库日期")]
        public DateTime? Out_date
        {
            get { return _Out_date; }
            set
            {
                SetProperty(ref _Out_date, value);
            }
        }

        private DateTime? _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at", ColDesc = "创建时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "Created_at", IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime? Created_at
        {
            get { return _Created_at; }
            set
            {
                SetProperty(ref _Created_at, value);
            }
        }

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by", ColDesc = "创建人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Created_by", DecimalDigits = 0, IsNullable = true, ColumnDescription = "创建人")]
        public long? Created_by
        {
            get { return _Created_by; }
            set
            {
                SetProperty(ref _Created_by, value);
            }
        }

        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at", ColDesc = "修改时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "Modified_at", IsNullable = true, ColumnDescription = "修改时间")]
        public DateTime? Modified_at
        {
            get { return _Modified_at; }
            set
            {
                SetProperty(ref _Modified_at, value);
            }
        }

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by", ColDesc = "修改人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Modified_by", DecimalDigits = 0, IsNullable = true, ColumnDescription = "修改人")]
        public long? Modified_by
        {
            get { return _Modified_by; }
            set
            {
                SetProperty(ref _Modified_by, value);
            }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes", ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "Notes", Length = 1500, IsNullable = true, ColumnDescription = "备注")]
        public string Notes
        {
            get { return _Notes; }
            set
            {
                SetProperty(ref _Notes, value);
            }
        }

        private bool _isdeleted = false;
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [AdvQueryAttribute(ColName = "isdeleted", ColDesc = "逻辑删除")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "isdeleted", IsNullable = false, ColumnDescription = "逻辑删除")]
        [Browsable(false)]
        public bool isdeleted
        {
            get { return _isdeleted; }
            set
            {
                SetProperty(ref _isdeleted, value);
            }
        }

        private int _DataStatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "DataStatus", ColDesc = "数据状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "DataStatus", DecimalDigits = 0, IsNullable = false, ColumnDescription = "数据状态")]
        public int DataStatus
        {
            get { return _DataStatus; }
            set
            {
                SetProperty(ref _DataStatus, value);
            }
        }

        private string _Reason;
        /// <summary>
        /// 借出原因
        /// </summary>
        [AdvQueryAttribute(ColName = "Reason", ColDesc = "借出原因")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "Reason", Length = 500, IsNullable = true, ColumnDescription = "借出原因")]
        public string Reason
        {
            get { return _Reason; }
            set
            {
                SetProperty(ref _Reason, value);
            }
        }

        private string _ApprovalOpinions;
        /// <summary>
        /// 审批意见
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalOpinions", ColDesc = "审批意见")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "ApprovalOpinions", Length = 500, IsNullable = true, ColumnDescription = "审批意见")]
        public string ApprovalOpinions
        {
            get { return _ApprovalOpinions; }
            set
            {
                SetProperty(ref _ApprovalOpinions, value);
            }
        }

        private long? _Approver_by;
        /// <summary>
        /// 审批人
        /// </summary>
        [AdvQueryAttribute(ColName = "Approver_by", ColDesc = "审批人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Approver_by", DecimalDigits = 0, IsNullable = true, ColumnDescription = "审批人")]
        public long? Approver_by
        {
            get { return _Approver_by; }
            set
            {
                SetProperty(ref _Approver_by, value);
            }
        }

        private DateTime? _Approver_at;
        /// <summary>
        /// 审批时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Approver_at", ColDesc = "审批时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "Approver_at", IsNullable = true, ColumnDescription = "审批时间")]
        public DateTime? Approver_at
        {
            get { return _Approver_at; }
            set
            {
                SetProperty(ref _Approver_at, value);
            }
        }

        private int? _ApprovalStatus = ((0));
        /// <summary>
        /// 审批状态
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalStatus", ColDesc = "审批状态")]
        [SugarColumn(ColumnDataType = "tinyint", SqlParameterDbType = "SByte", ColumnName = "ApprovalStatus", DecimalDigits = 0, IsNullable = true, ColumnDescription = "审批状态")]
        public int? ApprovalStatus
        {
            get { return _ApprovalStatus; }
            set
            {
                SetProperty(ref _ApprovalStatus, value);
            }
        }

        private bool? _ApprovalResults;
        /// <summary>
        /// 审批结果
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalResults", ColDesc = "审批结果")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "ApprovalResults", IsNullable = true, ColumnDescription = "审批结果")]
        public bool? ApprovalResults
        {
            get { return _ApprovalResults; }
            set
            {
                SetProperty(ref _ApprovalResults, value);
            }
        }

        private int _PrintStatus = ((0));
        /// <summary>
        /// 打印状态
        /// </summary>
        [AdvQueryAttribute(ColName = "PrintStatus", ColDesc = "打印状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "PrintStatus", DecimalDigits = 0, IsNullable = false, ColumnDescription = "打印状态")]
        public int PrintStatus
        {
            get { return _PrintStatus; }
            set
            {
                SetProperty(ref _PrintStatus, value);
            }
        }

        private string _CloseCaseOpinions;
        /// <summary>
        /// 审批意见
        /// </summary>
        [AdvQueryAttribute(ColName = "CloseCaseOpinions", ColDesc = "审批意见")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "CloseCaseOpinions", Length = 200, IsNullable = true, ColumnDescription = "审批意见")]
        public string CloseCaseOpinions
        {
            get { return _CloseCaseOpinions; }
            set
            {
                SetProperty(ref _CloseCaseOpinions, value);
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


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProdReturning.BorrowID))]
        public virtual List<tb_ProdReturning> tb_ProdReturnings { get; set; }
        //tb_ProdReturning.BorrowID)
        //BorrowID.FK_TB_PRODR_REFERENCE_TB_PRODB)
        //tb_ProdBorrowing.BorrowID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProdBorrowingDetail.BorrowID))]
        public virtual List<tb_ProdBorrowingDetail> tb_ProdBorrowingDetails { get; set; }
        //tb_ProdBorrowingDetail.BorrowID)
        //BorrowID.FK_TB_PRODB_REFERENCE_TB_PRODB)
        //tb_ProdBorrowing.BorrowID)


        #endregion




        //如果为false,则不可以。
        private bool PK_FK_ID_Check()
        {
            bool rs = true;
            return rs;
        }






        public override object Clone()
        {
            tb_ProdBorrowing loctype = (tb_ProdBorrowing)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

