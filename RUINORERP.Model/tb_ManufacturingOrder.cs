
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:04
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
    /// 制令单-生产工单 ，工单(MO)各种企业的叫法不一样，比如生产单，制令单，生产指导单，裁单，等等。其实都是同一个东西–MO,    来源于 销售订单，计划单，生产需求单，我在下文都以工单简称。https://blog.csdn.net/qq_37365475/article/details/106612960
    /// </summary>
    [Serializable()]
    [Description("制令单-生产工单 ，工单(MO)各种企业的叫法不一样，比如生产单，制令单，生产指导单，裁单，等等。其实都是同一个东西–MO,    来源于 销售订单，计划单，生产需求单，我在下文都以工单简称。https://blog.csdn.net/qq_37365475/article/details/106612960")]
    [SugarTable("tb_ManufacturingOrder")]
    public partial class tb_ManufacturingOrder: BaseEntity, ICloneable
    {
        public tb_ManufacturingOrder()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("制令单-生产工单 ，工单(MO)各种企业的叫法不一样，比如生产单，制令单，生产指导单，裁单，等等。其实都是同一个东西–MO,    来源于 销售订单，计划单，生产需求单，我在下文都以工单简称。https://blog.csdn.net/qq_37365475/article/details/106612960tb_ManufacturingOrder" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _MOID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "MOID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long MOID
        { 
            get{return _MOID;}
            set{
            SetProperty(ref _MOID, value);
                base.PrimaryKeyID = _MOID;
            }
        }

        private string _MONO;
        /// <summary>
        /// 制令单号
        /// </summary>
        [AdvQueryAttribute(ColName = "MONO",ColDesc = "制令单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "MONO" ,Length=100,IsNullable = false,ColumnDescription = "制令单号" )]
        public string MONO
        { 
            get{return _MONO;}
            set{
            SetProperty(ref _MONO, value);
                        }
        }

        private string _PDNO;
        /// <summary>
        /// 需求单号
        /// </summary>
        [AdvQueryAttribute(ColName = "PDNO",ColDesc = "需求单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PDNO" ,Length=100,IsNullable = true,ColumnDescription = "需求单号" )]
        public string PDNO
        { 
            get{return _PDNO;}
            set{
            SetProperty(ref _PDNO, value);
                        }
        }

        private long? _PDCID;
        /// <summary>
        /// 自制品
        /// </summary>
        [AdvQueryAttribute(ColName = "PDCID",ColDesc = "自制品")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PDCID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "自制品" )]
        [FKRelationAttribute("tb_ProduceGoodsRecommendDetail","PDCID")]
        public long? PDCID
        { 
            get{return _PDCID;}
            set{
            SetProperty(ref _PDCID, value);
                        }
        }

        private long? _PDID;
        /// <summary>
        /// 需求单据
        /// </summary>
        [AdvQueryAttribute(ColName = "PDID",ColDesc = "需求单据")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PDID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "需求单据" )]
        [FKRelationAttribute("tb_ProductionDemand","PDID")]
        public long? PDID
        { 
            get{return _PDID;}
            set{
            SetProperty(ref _PDID, value);
                        }
        }

        private int _QuantityDelivered= ((0));
        /// <summary>
        /// 已交付量
        /// </summary>
        [AdvQueryAttribute(ColName = "QuantityDelivered",ColDesc = "已交付量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "QuantityDelivered" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "已交付量" )]
        public int QuantityDelivered
        { 
            get{return _QuantityDelivered;}
            set{
            SetProperty(ref _QuantityDelivered, value);
                        }
        }

        private int _ManufacturingQty= ((0));
        /// <summary>
        /// 生产数量
        /// </summary>
        [AdvQueryAttribute(ColName = "ManufacturingQty",ColDesc = "生产数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ManufacturingQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "生产数量" )]
        public int ManufacturingQty
        { 
            get{return _ManufacturingQty;}
            set{
            SetProperty(ref _ManufacturingQty, value);
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

        private DateTime? _PreStartDate;
        /// <summary>
        /// 预开工日
        /// </summary>
        [AdvQueryAttribute(ColName = "PreStartDate",ColDesc = "预开工日")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "PreStartDate" ,IsNullable = true,ColumnDescription = "预开工日" )]
        public DateTime? PreStartDate
        { 
            get{return _PreStartDate;}
            set{
            SetProperty(ref _PreStartDate, value);
                        }
        }

        private DateTime? _PreEndDate;
        /// <summary>
        /// 预完工日
        /// </summary>
        [AdvQueryAttribute(ColName = "PreEndDate",ColDesc = "预完工日")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "PreEndDate" ,IsNullable = true,ColumnDescription = "预完工日" )]
        public DateTime? PreEndDate
        { 
            get{return _PreEndDate;}
            set{
            SetProperty(ref _PreEndDate, value);
                        }
        }

        private string _SKU;
        /// <summary>
        /// 母件SKU码
        /// </summary>
        [AdvQueryAttribute(ColName = "SKU",ColDesc = "母件SKU码")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SKU" ,Length=80,IsNullable = true,ColumnDescription = "母件SKU码" )]
        public string SKU
        { 
            get{return _SKU;}
            set{
            SetProperty(ref _SKU, value);
                        }
        }

        private string _CNName;
        /// <summary>
        /// 母件品名
        /// </summary>
        [AdvQueryAttribute(ColName = "CNName",ColDesc = "母件品名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CNName" ,Length=255,IsNullable = false,ColumnDescription = "母件品名" )]
        public string CNName
        { 
            get{return _CNName;}
            set{
            SetProperty(ref _CNName, value);
                        }
        }

        private long _ProdDetailID;
        /// <summary>
        /// 货品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "货品")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "货品" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
                        }
        }

        private long _BOM_ID;
        /// <summary>
        /// 配方名称
        /// </summary>
        [AdvQueryAttribute(ColName = "BOM_ID",ColDesc = "配方名称")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BOM_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "配方名称" )]
        [FKRelationAttribute("tb_BOM_S","BOM_ID")]
        public long BOM_ID
        { 
            get{return _BOM_ID;}
            set{
            SetProperty(ref _BOM_ID, value);
                        }
        }

        private string _BOM_No;
        /// <summary>
        /// 配方号
        /// </summary>
        [AdvQueryAttribute(ColName = "BOM_No",ColDesc = "配方号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BOM_No" ,Length=100,IsNullable = false,ColumnDescription = "配方号" )]
        public string BOM_No
        { 
            get{return _BOM_No;}
            set{
            SetProperty(ref _BOM_No, value);
                        }
        }

        private long? _Type_ID;
        /// <summary>
        /// 母件类型
        /// </summary>
        [AdvQueryAttribute(ColName = "Type_ID",ColDesc = "母件类型")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Type_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "母件类型" )]
        [FKRelationAttribute("tb_ProductType","Type_ID")]
        public long? Type_ID
        { 
            get{return _Type_ID;}
            set{
            SetProperty(ref _Type_ID, value);
                        }
        }

        private long? _Unit_ID;
        /// <summary>
        /// 单位
        /// </summary>
        [AdvQueryAttribute(ColName = "Unit_ID",ColDesc = "单位")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Unit_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "单位" )]
        [FKRelationAttribute("tb_Unit","Unit_ID")]
        public long? Unit_ID
        { 
            get{return _Unit_ID;}
            set{
            SetProperty(ref _Unit_ID, value);
                        }
        }

        private string _CustomerPartNo;
        /// <summary>
        /// 客户料号
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerPartNo",ColDesc = "客户料号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomerPartNo" ,Length=100,IsNullable = true,ColumnDescription = "客户料号" )]
        public string CustomerPartNo
        { 
            get{return _CustomerPartNo;}
            set{
            SetProperty(ref _CustomerPartNo, value);
                        }
        }

        private string _Specifications;
        /// <summary>
        /// 母件规格
        /// </summary>
        [AdvQueryAttribute(ColName = "Specifications",ColDesc = "母件规格")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Specifications" ,Length=1000,IsNullable = true,ColumnDescription = "母件规格" )]
        public string Specifications
        { 
            get{return _Specifications;}
            set{
            SetProperty(ref _Specifications, value);
                        }
        }

        private string _property;
        /// <summary>
        /// 母件属性
        /// </summary>
        [AdvQueryAttribute(ColName = "property",ColDesc = "母件属性")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "property" ,Length=255,IsNullable = true,ColumnDescription = "母件属性" )]
        public string property
        { 
            get{return _property;}
            set{
            SetProperty(ref _property, value);
                        }
        }

        private long _Employee_ID;
        /// <summary>
        /// 制单人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "制单人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "制单人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
                        }
        }

        private long _Location_ID;
        /// <summary>
        /// 预入库位
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "预入库位")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Location_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "预入库位" )]
        [FKRelationAttribute("tb_Location","Location_ID")]
        public long Location_ID
        { 
            get{return _Location_ID;}
            set{
            SetProperty(ref _Location_ID, value);
                        }
        }

        private long _DepartmentID;
        /// <summary>
        /// 需求部门
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "需求部门")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "需求部门" )]
        [FKRelationAttribute("tb_Department","DepartmentID")]
        public long DepartmentID
        { 
            get{return _DepartmentID;}
            set{
            SetProperty(ref _DepartmentID, value);
                        }
        }

        private long? _CustomerVendor_ID_Out;
        /// <summary>
        /// 外发厂商
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID_Out",ColDesc = "外发厂商")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID_Out" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "外发厂商" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID_Out")]
        public long? CustomerVendor_ID_Out
        { 
            get{return _CustomerVendor_ID_Out;}
            set{
            SetProperty(ref _CustomerVendor_ID_Out, value);
                        }
        }

        private long? _CustomerVendor_ID;
        /// <summary>
        /// 需求客户
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "需求客户")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "需求客户" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long? CustomerVendor_ID
        { 
            get{return _CustomerVendor_ID;}
            set{
            SetProperty(ref _CustomerVendor_ID, value);
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

        private string _CloseCaseOpinions;
        /// <summary>
        /// 结案情况
        /// </summary>
        [AdvQueryAttribute(ColName = "CloseCaseOpinions",ColDesc = "结案情况")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CloseCaseOpinions" ,Length=200,IsNullable = true,ColumnDescription = "结案情况" )]
        public string CloseCaseOpinions
        { 
            get{return _CloseCaseOpinions;}
            set{
            SetProperty(ref _CloseCaseOpinions, value);
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

        private decimal _ApportionedCost= ((0));
        /// <summary>
        /// 分摊成本
        /// </summary>
        [AdvQueryAttribute(ColName = "ApportionedCost",ColDesc = "分摊成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "ApportionedCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "分摊成本" )]
        public decimal ApportionedCost
        { 
            get{return _ApportionedCost;}
            set{
            SetProperty(ref _ApportionedCost, value);
                        }
        }

        private decimal _TotalManuFee= ((0));
        /// <summary>
        /// 总制造费用
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalManuFee",ColDesc = "总制造费用")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalManuFee" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "总制造费用" )]
        public decimal TotalManuFee
        { 
            get{return _TotalManuFee;}
            set{
            SetProperty(ref _TotalManuFee, value);
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

        private decimal _TotalProductionCost= ((0));
        /// <summary>
        /// 生产总成本
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalProductionCost",ColDesc = "生产总成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalProductionCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "生产总成本" )]
        public decimal TotalProductionCost
        { 
            get{return _TotalProductionCost;}
            set{
            SetProperty(ref _TotalProductionCost, value);
                        }
        }

        private decimal _PeopleQty= ((0));
        /// <summary>
        /// 人数
        /// </summary>
        [AdvQueryAttribute(ColName = "PeopleQty",ColDesc = "人数")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "PeopleQty" , DecimalDigits = 5,IsNullable = false,ColumnDescription = "人数" )]
        public decimal PeopleQty
        { 
            get{return _PeopleQty;}
            set{
            SetProperty(ref _PeopleQty, value);
                        }
        }

        private decimal _WorkingHour= ((0));
        /// <summary>
        /// 工时
        /// </summary>
        [AdvQueryAttribute(ColName = "WorkingHour",ColDesc = "工时")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "WorkingHour" , DecimalDigits = 5,IsNullable = false,ColumnDescription = "工时" )]
        public decimal WorkingHour
        { 
            get{return _WorkingHour;}
            set{
            SetProperty(ref _WorkingHour, value);
                        }
        }

        private decimal _MachineHour= ((0));
        /// <summary>
        /// 机时
        /// </summary>
        [AdvQueryAttribute(ColName = "MachineHour",ColDesc = "机时")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "MachineHour" , DecimalDigits = 5,IsNullable = false,ColumnDescription = "机时" )]
        public decimal MachineHour
        { 
            get{return _MachineHour;}
            set{
            SetProperty(ref _MachineHour, value);
                        }
        }

        private bool? _IncludeSubBOM= false;
        /// <summary>
        /// 上层驱动
        /// </summary>
        [AdvQueryAttribute(ColName = "IncludeSubBOM",ColDesc = "上层驱动")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IncludeSubBOM" ,IsNullable = true,ColumnDescription = "上层驱动" )]
        public bool? IncludeSubBOM
        { 
            get{return _IncludeSubBOM;}
            set{
            SetProperty(ref _IncludeSubBOM, value);
                        }
        }

        private bool _IsOutSourced= false;
        /// <summary>
        /// 是否托工
        /// </summary>
        [AdvQueryAttribute(ColName = "IsOutSourced",ColDesc = "是否托工")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsOutSourced" ,IsNullable = false,ColumnDescription = "是否托工" )]
        public bool IsOutSourced
        { 
            get{return _IsOutSourced;}
            set{
            SetProperty(ref _IsOutSourced, value);
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

        private DateTime? _ApproverTime;
        /// <summary>
        /// 审批时间
        /// </summary>
        [AdvQueryAttribute(ColName = "ApproverTime",ColDesc = "审批时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "ApproverTime" ,IsNullable = true,ColumnDescription = "审批时间" )]
        public DateTime? ApproverTime
        { 
            get{return _ApproverTime;}
            set{
            SetProperty(ref _ApproverTime, value);
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

        private int? _DataStatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "DataStatus",ColDesc = "数据状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DataStatus" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "数据状态" )]
        public int? DataStatus
        { 
            get{return _DataStatus;}
            set{
            SetProperty(ref _DataStatus, value);
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
        [Navigate(NavigateType.OneToOne, nameof(CustomerVendor_ID_Out))]
        public virtual tb_CustomerVendor tb_customervendor_out { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(BOM_ID))]
        public virtual tb_BOM_S tb_bom_s { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(PDCID))]
        public virtual tb_ProduceGoodsRecommendDetail tb_producegoodsrecommenddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(DepartmentID))]
        public virtual tb_Department tb_department { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(PDID))]
        public virtual tb_ProductionDemand tb_productiondemand { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Type_ID))]
        public virtual tb_ProductType tb_producttype { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Unit_ID))]
        public virtual tb_Unit tb_unit { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(CustomerVendor_ID))]
        public virtual tb_CustomerVendor tb_customervendor { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MaterialRequisition.MOID))]
        public virtual List<tb_MaterialRequisition> tb_MaterialRequisitions { get; set; }
        //tb_MaterialRequisition.MOID)
        //MOID.FK_MATEREQUISTITIONS_REF_MANUFCTRUINGORDER)
        //tb_ManufacturingOrder.MOID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FinishedGoodsInv.MOID))]
        public virtual List<tb_FinishedGoodsInv> tb_FinishedGoodsInvs { get; set; }
        //tb_FinishedGoodsInv.MOID)
        //MOID.FK_FINIS_REF_MANUF)
        //tb_ManufacturingOrder.MOID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ManufacturingOrderDetail.MOID))]
        public virtual List<tb_ManufacturingOrderDetail> tb_ManufacturingOrderDetails { get; set; }
        //tb_ManufacturingOrderDetail.MOID)
        //MOID.FK_TB_MANUFODER_REF_TB_MANUFORDERDE)
        //tb_ManufacturingOrder.MOID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MRP_ReworkReturn.MOID))]
        public virtual List<tb_MRP_ReworkReturn> tb_MRP_ReworkReturns { get; set; }
        //tb_MRP_ReworkReturn.MOID)
        //MOID.FK_MRP_Reworkreturn_REF_ManufacturingOrder)
        //tb_ManufacturingOrder.MOID)


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
         if("CustomerVendor_ID"!="CustomerVendor_ID_Out")
        {
        // rs=false;
        }
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
                    Type type = typeof(tb_ManufacturingOrder);
                    
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
            tb_ManufacturingOrder loctype = (tb_ManufacturingOrder)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

