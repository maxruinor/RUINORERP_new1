
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:51
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
    /// 领料单(包括生产和托工)
    /// </summary>
    [Serializable()]
    [SugarTable("tb_MaterialRequisition")]
    public partial class tb_MaterialRequisitionQueryDto:BaseEntityDto
    {
        public tb_MaterialRequisitionQueryDto()
        {

        }

    
     

        private string _MaterialRequisitionNO;
        /// <summary>
        /// 领料单号
        /// </summary>
        [AdvQueryAttribute(ColName = "MaterialRequisitionNO",ColDesc = "领料单号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "MaterialRequisitionNO",Length=50,IsNullable = false,ColumnDescription = "领料单号" )]
        public string MaterialRequisitionNO 
        { 
            get{return _MaterialRequisitionNO;}
            set{SetProperty(ref _MaterialRequisitionNO, value);}
        }
     

        private string _MONO;
        /// <summary>
        /// 制令单号
        /// </summary>
        [AdvQueryAttribute(ColName = "MONO",ColDesc = "制令单号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "MONO",Length=100,IsNullable = false,ColumnDescription = "制令单号" )]
        public string MONO 
        { 
            get{return _MONO;}
            set{SetProperty(ref _MONO, value);}
        }
     

        private DateTime? _DeliveryDate;
        /// <summary>
        /// 领取日期
        /// </summary>
        [AdvQueryAttribute(ColName = "DeliveryDate",ColDesc = "领取日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "DeliveryDate",IsNullable = true,ColumnDescription = "领取日期" )]
        public DateTime? DeliveryDate 
        { 
            get{return _DeliveryDate;}
            set{SetProperty(ref _DeliveryDate, value);}
        }
     

        private long _Employee_ID;
        /// <summary>
        /// 经办人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "经办人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Employee_ID",IsNullable = false,ColumnDescription = "经办人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long Employee_ID 
        { 
            get{return _Employee_ID;}
            set{SetProperty(ref _Employee_ID, value);}
        }
     

        private long? _DepartmentID;
        /// <summary>
        /// 生产部门
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "生产部门")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "DepartmentID",IsNullable = true,ColumnDescription = "生产部门" )]
        [FKRelationAttribute("tb_Department","DepartmentID")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}
            set{SetProperty(ref _DepartmentID, value);}
        }
     

        private long? _CustomerVendor_ID;
        /// <summary>
        /// 外发厂商
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "外发厂商")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "CustomerVendor_ID",IsNullable = true,ColumnDescription = "外发厂商" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long? CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}
            set{SetProperty(ref _CustomerVendor_ID, value);}
        }
     

        private long _MOID;
        /// <summary>
        /// 制令单
        /// </summary>
        [AdvQueryAttribute(ColName = "MOID",ColDesc = "制令单")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "MOID",IsNullable = false,ColumnDescription = "制令单" )]
        [FKRelationAttribute("tb_ManufacturingOrder","MOID")]
        public long MOID 
        { 
            get{return _MOID;}
            set{SetProperty(ref _MOID, value);}
        }
     

        private long _Location_ID;
        /// <summary>
        /// 库位
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "库位")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Location_ID",IsNullable = false,ColumnDescription = "库位" )]
        [FKRelationAttribute("tb_Location","Location_ID")]
        public long Location_ID 
        { 
            get{return _Location_ID;}
            set{SetProperty(ref _Location_ID, value);}
        }
     

        private long? _ProjectGroup_ID;
        /// <summary>
        /// 项目组
        /// </summary>
        [AdvQueryAttribute(ColName = "ProjectGroup_ID",ColDesc = "项目组")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProjectGroup_ID",IsNullable = true,ColumnDescription = "项目组" )]
        [FKRelationAttribute("tb_ProjectGroup","ProjectGroup_ID")]
        public long? ProjectGroup_ID 
        { 
            get{return _ProjectGroup_ID;}
            set{SetProperty(ref _ProjectGroup_ID, value);}
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
     

        private string _shippingWay;
        /// <summary>
        /// 发货方式
        /// </summary>
        [AdvQueryAttribute(ColName = "shippingWay",ColDesc = "发货方式")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "shippingWay",Length=50,IsNullable = true,ColumnDescription = "发货方式" )]
        public string shippingWay 
        { 
            get{return _shippingWay;}
            set{SetProperty(ref _shippingWay, value);}
        }
     

        private decimal _TotalPrice= ((0));
        /// <summary>
        /// 总金额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalPrice",ColDesc = "总金额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TotalPrice",IsNullable = false,ColumnDescription = "总金额" )]
        public decimal TotalPrice 
        { 
            get{return _TotalPrice;}
            set{SetProperty(ref _TotalPrice, value);}
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
     

        private int _TotalSendQty= ((0));
        /// <summary>
        /// 实发总数
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalSendQty",ColDesc = "实发总数")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "TotalSendQty",IsNullable = false,ColumnDescription = "实发总数" )]
        public int TotalSendQty 
        { 
            get{return _TotalSendQty;}
            set{SetProperty(ref _TotalSendQty, value);}
        }
     

        private int _TotalReQty= ((0));
        /// <summary>
        /// 退回总数
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalReQty",ColDesc = "退回总数")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "TotalReQty",IsNullable = false,ColumnDescription = "退回总数" )]
        public int TotalReQty 
        { 
            get{return _TotalReQty;}
            set{SetProperty(ref _TotalReQty, value);}
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
     

        private decimal? _ShipCost= ((0));
        /// <summary>
        /// 运费
        /// </summary>
        [AdvQueryAttribute(ColName = "ShipCost",ColDesc = "运费")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "ShipCost",IsNullable = true,ColumnDescription = "运费" )]
        public decimal? ShipCost 
        { 
            get{return _ShipCost;}
            set{SetProperty(ref _ShipCost, value);}
        }
     

        private bool _ReApply= false;
        /// <summary>
        /// 是否补领
        /// </summary>
        [AdvQueryAttribute(ColName = "ReApply",ColDesc = "是否补领")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "ReApply",IsNullable = false,ColumnDescription = "是否补领" )]
        public bool ReApply 
        { 
            get{return _ReApply;}
            set{SetProperty(ref _ReApply, value);}
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
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=255,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
        }
     

        private string _ApprovalOpinions;
        /// <summary>
        /// 审批意见
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalOpinions",ColDesc = "审批意见")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ApprovalOpinions",Length=200,IsNullable = true,ColumnDescription = "审批意见" )]
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
     

        private bool? _GeneEvidence;
        /// <summary>
        /// 产生凭证
        /// </summary>
        [AdvQueryAttribute(ColName = "GeneEvidence",ColDesc = "产生凭证")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "GeneEvidence",IsNullable = true,ColumnDescription = "产生凭证" )]
        public bool? GeneEvidence 
        { 
            get{return _GeneEvidence;}
            set{SetProperty(ref _GeneEvidence, value);}
        }
     

        private bool? _Outgoing= false;
        /// <summary>
        /// 外发加工
        /// </summary>
        [AdvQueryAttribute(ColName = "Outgoing",ColDesc = "外发加工")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Outgoing",IsNullable = true,ColumnDescription = "外发加工" )]
        public bool? Outgoing 
        { 
            get{return _Outgoing;}
            set{SetProperty(ref _Outgoing, value);}
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


       
    }
}



