﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:30
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
    /// 调拨单-两个仓库之间的库存转移
    /// </summary>
    [Serializable()]
    [Description("调拨单")]
    [SugarTable("tb_StockTransfer")]
    public partial class tb_StockTransfer: BaseEntity, ICloneable
    {
        public tb_StockTransfer()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("调拨单-两个仓库之间的库存转移tb_StockTransfer" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _StockTransferID;
        /// <summary>
        /// 调拨单
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "StockTransferID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "调拨单" , IsPrimaryKey = true)]
        public long StockTransferID
        { 
            get{return _StockTransferID;}
            set{
            SetProperty(ref _StockTransferID, value);
                base.PrimaryKeyID = _StockTransferID;
            }
        }

        private string _StockTransferNo;
        /// <summary>
        /// 调拨单号
        /// </summary>
        [AdvQueryAttribute(ColName = "StockTransferNo",ColDesc = "调拨单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "StockTransferNo" ,Length=50,IsNullable = true,ColumnDescription = "调拨单号" )]
        public string StockTransferNo
        { 
            get{return _StockTransferNo;}
            set{
            SetProperty(ref _StockTransferNo, value);
                        }
        }

        private long _Location_ID_from;
        /// <summary>
        /// 调出仓库
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID_from",ColDesc = "调出仓库")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Location_ID_from" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "调出仓库" )]
        [FKRelationAttribute("tb_Location","Location_ID_from")]
        public long Location_ID_from
        { 
            get{return _Location_ID_from;}
            set{
            SetProperty(ref _Location_ID_from, value);
                        }
        }

        private long _Location_ID_to;
        /// <summary>
        /// 调入仓库
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID_to",ColDesc = "调入仓库")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Location_ID_to" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "调入仓库" )]
        [FKRelationAttribute("tb_Location","Location_ID_to")]
        public long Location_ID_to
        { 
            get{return _Location_ID_to;}
            set{
            SetProperty(ref _Location_ID_to, value);
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

        private decimal _TotalCost= ((0));
        /// <summary>
        /// 总成本
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalCost",ColDesc = "总成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "总成本" )]
        public decimal TotalCost
        { 
            get{return _TotalCost;}
            set{
            SetProperty(ref _TotalCost, value);
                        }
        }

        private decimal _TotalTransferAmount= ((0));
        /// <summary>
        /// 调拨金额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalTransferAmount",ColDesc = "调拨金额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalTransferAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "调拨金额" )]
        public decimal TotalTransferAmount
        { 
            get{return _TotalTransferAmount;}
            set{
            SetProperty(ref _TotalTransferAmount, value);
                        }
        }

        private DateTime? _Transfer_date;
        /// <summary>
        /// 调拨日期
        /// </summary>
        [AdvQueryAttribute(ColName = "Transfer_date",ColDesc = "调拨日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Transfer_date" ,IsNullable = true,ColumnDescription = "调拨日期" )]
        public DateTime? Transfer_date
        { 
            get{return _Transfer_date;}
            set{
            SetProperty(ref _Transfer_date, value);
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
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ApprovalOpinions" ,Length=500,IsNullable = true,ColumnDescription = "审批意见" )]
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

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Location_ID_from))]
        public virtual tb_Location tb_location_from { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Location_ID_to))]
        public virtual tb_Location tb_location_to { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_StockTransferDetail.StockTransferID))]
        public virtual List<tb_StockTransferDetail> tb_StockTransferDetails { get; set; }
        //tb_StockTransferDetail.StockTransferID)
        //StockTransferID.FK_TB_STOCK_REFERENCE_TB_STOCK)
        //tb_StockTransfer.StockTransferID)


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
         if("Location_ID"!="Location_ID_from")
        {
        // rs=false;
        }
         if("Location_ID"!="Location_ID_to")
        {
        // rs=false;
        }
return rs;
}





 

        public override object Clone()
        {
            tb_StockTransfer loctype = (tb_StockTransfer)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

