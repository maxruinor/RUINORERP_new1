
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:25
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model.Base;

namespace RUINORERP.Model.QueryDto
{
    /// <summary>
    /// 返厂售后单
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Return")]
    public partial class tb_ReturnQueryDto:BaseEntityDto
    {
        public tb_ReturnQueryDto()
        {

        }

    
     

        private long _CustomerVendor_ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "CustomerVendor_ID",IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}
            set{SetProperty(ref _CustomerVendor_ID, value);}
        }
     

        private DateTime _ReDate;
        /// <summary>
        /// 返厂日期
        /// </summary>
        [AdvQueryAttribute(ColName = "ReDate",ColDesc = "返厂日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "ReDate",IsNullable = false,ColumnDescription = "返厂日期" )]
        public DateTime ReDate 
        { 
            get{return _ReDate;}
            set{SetProperty(ref _ReDate, value);}
        }
     

        private int _TotalQty= ((0));
        /// <summary>
        /// 总数量
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalQty",ColDesc = "总数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "TotalQty",IsNullable = false,ColumnDescription = "总数量" )]
        public int TotalQty 
        { 
            get{return _TotalQty;}
            set{SetProperty(ref _TotalQty, value);}
        }
     

        private decimal _TotalCost= ((0));
        /// <summary>
        /// 总成本
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalCost",ColDesc = "总成本")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TotalCost",IsNullable = false,ColumnDescription = "总成本" )]
        public decimal TotalCost 
        { 
            get{return _TotalCost;}
            set{SetProperty(ref _TotalCost, value);}
        }
     

        private decimal _TotalAmount= ((0));
        /// <summary>
        /// 总金额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalAmount",ColDesc = "总金额")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "TotalAmount",IsNullable = false,ColumnDescription = "总金额" )]
        public decimal TotalAmount 
        { 
            get{return _TotalAmount;}
            set{SetProperty(ref _TotalAmount, value);}
        }
     

        private string _RetrunNo;
        /// <summary>
        /// 返厂单号
        /// </summary>
        [AdvQueryAttribute(ColName = "RetrunNo",ColDesc = "返厂单号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "RetrunNo",Length=50,IsNullable = false,ColumnDescription = "返厂单号" )]
        public string RetrunNo 
        { 
            get{return _RetrunNo;}
            set{SetProperty(ref _RetrunNo, value);}
        }
     

        private string _Reason;
        /// <summary>
        /// 返厂原因
        /// </summary>
        [AdvQueryAttribute(ColName = "Reason",ColDesc = "返厂原因")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Reason",Length=500,IsNullable = true,ColumnDescription = "返厂原因" )]
        public string Reason 
        { 
            get{return _Reason;}
            set{SetProperty(ref _Reason, value);}
        }
     

        private DateTime? _PreDeliveryDate;
        /// <summary>
        /// 预回日期
        /// </summary>
        [AdvQueryAttribute(ColName = "PreDeliveryDate",ColDesc = "预回日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "PreDeliveryDate",IsNullable = true,ColumnDescription = "预回日期" )]
        public DateTime? PreDeliveryDate 
        { 
            get{return _PreDeliveryDate;}
            set{SetProperty(ref _PreDeliveryDate, value);}
        }
     

        private bool _isdeleted= false;
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "isdeleted",IsNullable = false,ColumnDescription = "逻辑删除" )]
        public bool isdeleted 
        { 
            get{return _isdeleted;}
            set{SetProperty(ref _isdeleted, value);}
        }
     

        private int _DataStatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "DataStatus",ColDesc = "数据状态")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "DataStatus",IsNullable = false,ColumnDescription = "数据状态" )]
        public int DataStatus 
        { 
            get{return _DataStatus;}
            set{SetProperty(ref _DataStatus, value);}
        }
     

        private string _ApprovalOpinions;
        /// <summary>
        /// 审批意见
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalOpinions",ColDesc = "审批意见")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ApprovalOpinions",Length=500,IsNullable = true,ColumnDescription = "审批意见" )]
        public string ApprovalOpinions 
        { 
            get{return _ApprovalOpinions;}
            set{SetProperty(ref _ApprovalOpinions, value);}
        }
     

        private long? _Approver_by;
        /// <summary>
        /// 审批人
        /// </summary>
        [AdvQueryAttribute(ColName = "Approver_by",ColDesc = "审批人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Approver_by",IsNullable = true,ColumnDescription = "审批人" )]
        public long? Approver_by 
        { 
            get{return _Approver_by;}
            set{SetProperty(ref _Approver_by, value);}
        }
     

        private DateTime? _Approver_at;
        /// <summary>
        /// 审批时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Approver_at",ColDesc = "审批时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Approver_at",IsNullable = true,ColumnDescription = "审批时间" )]
        public DateTime? Approver_at 
        { 
            get{return _Approver_at;}
            set{SetProperty(ref _Approver_at, value);}
        }
     

        private int? _ApprovalStatus= ((0));
        /// <summary>
        /// 审批状态
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalStatus",ColDesc = "审批状态")]
        [SugarColumn(ColumnDataType = "tinyint",SqlParameterDbType ="SByte",ColumnName = "ApprovalStatus",IsNullable = true,ColumnDescription = "审批状态" )]
        public int? ApprovalStatus 
        { 
            get{return _ApprovalStatus;}
            set{SetProperty(ref _ApprovalStatus, value);}
        }
     

        private bool? _ApprovalResults;
        /// <summary>
        /// 审批结果
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalResults",ColDesc = "审批结果")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "ApprovalResults",IsNullable = true,ColumnDescription = "审批结果" )]
        public bool? ApprovalResults 
        { 
            get{return _ApprovalResults;}
            set{SetProperty(ref _ApprovalResults, value);}
        }
     

        private int _PrintStatus= ((0));
        /// <summary>
        /// 打印状态
        /// </summary>
        [AdvQueryAttribute(ColName = "PrintStatus",ColDesc = "打印状态")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "PrintStatus",IsNullable = false,ColumnDescription = "打印状态" )]
        public int PrintStatus 
        { 
            get{return _PrintStatus;}
            set{SetProperty(ref _PrintStatus, value);}
        }
     

        private decimal _ShipCost= ((0));
        /// <summary>
        /// 已付运费
        /// </summary>
        [AdvQueryAttribute(ColName = "ShipCost",ColDesc = "已付运费")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "ShipCost",IsNullable = false,ColumnDescription = "已付运费" )]
        public decimal ShipCost 
        { 
            get{return _ShipCost;}
            set{SetProperty(ref _ShipCost, value);}
        }
     

        private DateTime? _DeliveryDate;
        /// <summary>
        /// 发货日期
        /// </summary>
        [AdvQueryAttribute(ColName = "DeliveryDate",ColDesc = "发货日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "DeliveryDate",IsNullable = true,ColumnDescription = "发货日期" )]
        public DateTime? DeliveryDate 
        { 
            get{return _DeliveryDate;}
            set{SetProperty(ref _DeliveryDate, value);}
        }
     

        private string _ShippingAddress;
        /// <summary>
        /// 发货地址
        /// </summary>
        [AdvQueryAttribute(ColName = "ShippingAddress",ColDesc = "发货地址")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ShippingAddress",Length=255,IsNullable = true,ColumnDescription = "发货地址" )]
        public string ShippingAddress 
        { 
            get{return _ShippingAddress;}
            set{SetProperty(ref _ShippingAddress, value);}
        }
     

        private string _ShippingWay;
        /// <summary>
        /// 发货方式
        /// </summary>
        [AdvQueryAttribute(ColName = "ShippingWay",ColDesc = "发货方式")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ShippingWay",Length=50,IsNullable = true,ColumnDescription = "发货方式" )]
        public string ShippingWay 
        { 
            get{return _ShippingWay;}
            set{SetProperty(ref _ShippingWay, value);}
        }
     

        private string _TrackNo;
        /// <summary>
        /// 物流单号
        /// </summary>
        [AdvQueryAttribute(ColName = "TrackNo",ColDesc = "物流单号")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "TrackNo",Length=50,IsNullable = true,ColumnDescription = "物流单号" )]
        public string TrackNo 
        { 
            get{return _TrackNo;}
            set{SetProperty(ref _TrackNo, value);}
        }
     

        private DateTime? _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Created_at",IsNullable = true,ColumnDescription = "创建时间" )]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}
            set{SetProperty(ref _Created_at, value);}
        }
     

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Created_by",IsNullable = true,ColumnDescription = "创建人" )]
        public long? Created_by 
        { 
            get{return _Created_by;}
            set{SetProperty(ref _Created_by, value);}
        }
     

        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Modified_at",IsNullable = true,ColumnDescription = "修改时间" )]
        public DateTime? Modified_at 
        { 
            get{return _Modified_at;}
            set{SetProperty(ref _Modified_at, value);}
        }
     

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Modified_by",IsNullable = true,ColumnDescription = "修改人" )]
        public long? Modified_by 
        { 
            get{return _Modified_by;}
            set{SetProperty(ref _Modified_by, value);}
        }
     

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=500,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
        }


       
    }
}



