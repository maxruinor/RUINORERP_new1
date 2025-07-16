
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/16/2025 14:14:16
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

namespace RUINORERP.Model
{
    /// <summary>
    /// 售后申请明细统计 用pb文件生成。选择要生成的视图.检查列的描述。描述不能全空
    /// </summary>
    [Serializable()]
    [SugarTable("View_AS_AfterSaleApplyItems")]
    public partial class View_AS_AfterSaleApplyItems:BaseViewEntity
    {
        public View_AS_AfterSaleApplyItems()
        {
            FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("View_AS_AfterSaleApplyItems" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    
        private string _ASApplyNo;
        
        
        /// <summary>
        /// 申请编号
        /// </summary>

        [AdvQueryAttribute(ColName = "ASApplyNo",ColDesc = "申请编号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ASApplyNo" ,Length=50,IsNullable = true,ColumnDescription = "申请编号" )]
        [Display(Name = "申请编号")]
        public string ASApplyNo 
        { 
            get{return _ASApplyNo;}            set{                SetProperty(ref _ASApplyNo, value);                }
        }

        private long? _Employee_ID;


        /// <summary>
        /// 业务员
        /// </summary>

        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "业务员")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" ,IsNullable = true,ColumnDescription = "业务员")]
        [Display(Name = "业务员")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}            set{                SetProperty(ref _Employee_ID, value);                }
        }

        private long? _ProjectGroup_ID;
        
        
        /// <summary>
        /// 项目小组
        /// </summary>

        [AdvQueryAttribute(ColName = "ProjectGroup_ID",ColDesc = "项目小组")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProjectGroup_ID" ,IsNullable = true,ColumnDescription = "项目小组" )]
        [Display(Name = "项目小组")]
        public long? ProjectGroup_ID 
        { 
            get{return _ProjectGroup_ID;}            set{                SetProperty(ref _ProjectGroup_ID, value);                }
        }

        private long? _CustomerVendor_ID;
        
        
        /// <summary>
        /// 外发工厂
        /// </summary>

        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "外发工厂")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" ,IsNullable = true,ColumnDescription = "外发工厂" )]
        [Display(Name = "外发工厂")]
        public long? CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}            set{                SetProperty(ref _CustomerVendor_ID, value);                }
        }

        private DateTime? _ApplyDate;
        
        
        /// <summary>
        /// 申请日期
        /// </summary>

        [AdvQueryAttribute(ColName = "ApplyDate",ColDesc = "申请日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "ApplyDate" ,IsNullable = true,ColumnDescription = "申请日期" )]
        [Display(Name = "申请日期")]
        public DateTime? ApplyDate 
        { 
            get{return _ApplyDate;}            set{                SetProperty(ref _ApplyDate, value);                }
        }

        private DateTime? _Created_at;
        
        
        /// <summary>
        /// 创建时间
        /// </summary>

        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Created_at" ,IsNullable = true,ColumnDescription = "创建时间" )]
        [Display(Name = "创建时间")]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}            set{                SetProperty(ref _Created_at, value);                }
        }

        private long? _Created_by;
        
        
        /// <summary>
        /// 创建人
        /// </summary>

        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Created_by" ,IsNullable = true,ColumnDescription = "创建人" )]
        [Display(Name = "创建人")]
        public long? Created_by 
        { 
            get{return _Created_by;}            set{                SetProperty(ref _Created_by, value);                }
        }

        private string _CustomerSourceNo;
        
        
        /// <summary>
        /// 来源单号
        /// </summary>

        [AdvQueryAttribute(ColName = "CustomerSourceNo",ColDesc = "来源单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomerSourceNo" ,Length=50,IsNullable = true,ColumnDescription = "来源单号" )]
        [Display(Name = "来源单号")]
        public string CustomerSourceNo 
        { 
            get{return _CustomerSourceNo;}            set{                SetProperty(ref _CustomerSourceNo, value);                }
        }

        private int? _Priority;
        
        
        /// <summary>
        /// 紧急程度
        /// </summary>

        [AdvQueryAttribute(ColName = "Priority",ColDesc = "紧急程度")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Priority" ,IsNullable = true,ColumnDescription = "紧急程度" )]
        [Display(Name = "紧急程度")]
        public int? Priority 
        { 
            get{return _Priority;}            set{                SetProperty(ref _Priority, value);                }
        }

        private int? _ASProcessStatus;
        
        
        /// <summary>
        /// 处理状态
        /// </summary>

        [AdvQueryAttribute(ColName = "ASProcessStatus",ColDesc = "处理状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ASProcessStatus" ,IsNullable = true,ColumnDescription = "处理状态" )]
        [Display(Name = "处理状态")]
        public int? ASProcessStatus 
        { 
            get{return _ASProcessStatus;}            set{                SetProperty(ref _ASProcessStatus, value);                }
        }

        private int? _TotalConfirmedQuantity;
        
        
        /// <summary>
        /// 复核数量
        /// </summary>

        [AdvQueryAttribute(ColName = "TotalConfirmedQuantity",ColDesc = "复核数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "TotalConfirmedQuantity" ,IsNullable = true,ColumnDescription = "复核数量" )]
        [Display(Name = "复核数量")]
        public int? TotalConfirmedQuantity 
        { 
            get{return _TotalConfirmedQuantity;}            set{                SetProperty(ref _TotalConfirmedQuantity, value);                }
        }

        private string _RepairEvaluationOpinion;
        
        
        /// <summary>
        /// 维修评估意见
        /// </summary>

        [AdvQueryAttribute(ColName = "RepairEvaluationOpinion",ColDesc = "维修评估意见")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "RepairEvaluationOpinion" ,Length=500,IsNullable = true,ColumnDescription = "维修评估意见" )]
        [Display(Name = "维修评估意见")]
        public string RepairEvaluationOpinion 
        { 
            get{return _RepairEvaluationOpinion;}            set{                SetProperty(ref _RepairEvaluationOpinion, value);                }
        }

        private int? _ExpenseAllocationMode;
        
        
        /// <summary>
        /// 费用承担模式
        /// </summary>

        [AdvQueryAttribute(ColName = "ExpenseAllocationMode",ColDesc = "费用承担模式")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ExpenseAllocationMode" ,IsNullable = true,ColumnDescription = "费用承担模式" )]
        [Display(Name = "费用承担模式")]
        public int? ExpenseAllocationMode 
        { 
            get{return _ExpenseAllocationMode;}            set{                SetProperty(ref _ExpenseAllocationMode, value);                }
        }

        private int? _ExpenseBearerType;
        
        
        /// <summary>
        /// 费用承担方
        /// </summary>

        [AdvQueryAttribute(ColName = "ExpenseBearerType",ColDesc = "费用承担方")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ExpenseBearerType" ,IsNullable = true,ColumnDescription = "费用承担方" )]
        [Display(Name = "费用承担方")]
        public int? ExpenseBearerType 
        { 
            get{return _ExpenseBearerType;}            set{                SetProperty(ref _ExpenseBearerType, value);                }
        }

        private int? _TotalDeliveredQty;
        
        
        /// <summary>
        /// 交付数量
        /// </summary>

        [AdvQueryAttribute(ColName = "TotalDeliveredQty",ColDesc = "交付数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "TotalDeliveredQty" ,IsNullable = true,ColumnDescription = "交付数量" )]
        [Display(Name = "交付数量")]
        public int? TotalDeliveredQty 
        { 
            get{return _TotalDeliveredQty;}            set{                SetProperty(ref _TotalDeliveredQty, value);                }
        }

        private string _FaultDescription;
        
        
        /// <summary>
        /// 问题描述
        /// </summary>

        [AdvQueryAttribute(ColName = "FaultDescription",ColDesc = "问题描述")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "FaultDescription" ,Length=500,IsNullable = true,ColumnDescription = "问题描述" )]
        [Display(Name = "问题描述")]
        public string FaultDescription 
        { 
            get{return _FaultDescription;}            set{                SetProperty(ref _FaultDescription, value);                }
        }

        private long? _ProdDetailID;
        
        
        /// <summary>
        /// 产品详情
        /// </summary>

        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品详情")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" ,IsNullable = true,ColumnDescription = "产品详情" )]
        [Display(Name = "产品详情")]
        public long? ProdDetailID 
        { 
            get{return _ProdDetailID;}            set{                SetProperty(ref _ProdDetailID, value);                }
        }

        private long? _Location_ID;
        
        
        /// <summary>
        /// 库位
        /// </summary>

        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "库位")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Location_ID" ,IsNullable = true,ColumnDescription = "库位" )]
        [Display(Name = "库位")]
        public long? Location_ID 
        { 
            get{return _Location_ID;}            set{                SetProperty(ref _Location_ID, value);                }
        }

        private string _CustomerPartNo;
        
        
        /// <summary>
        /// 客户型号
        /// </summary>

        [AdvQueryAttribute(ColName = "CustomerPartNo",ColDesc = "客户型号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomerPartNo" ,Length=100,IsNullable = true,ColumnDescription = "客户型号" )]
        [Display(Name = "客户型号")]
        public string CustomerPartNo 
        { 
            get{return _CustomerPartNo;}            set{                SetProperty(ref _CustomerPartNo, value);                }
        }

        private int? _ConfirmedQuantity;
        
        
        /// <summary>
        /// 复核数量
        /// </summary>

        [AdvQueryAttribute(ColName = "ConfirmedQuantity",ColDesc = "复核数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ConfirmedQuantity" ,IsNullable = true,ColumnDescription = "复核数量" )]
        [Display(Name = "复核数量")]
        public int? ConfirmedQuantity 
        { 
            get{return _ConfirmedQuantity;}            set{                SetProperty(ref _ConfirmedQuantity, value);                }
        }

        private int? _DeliveredQty;
        
        
        /// <summary>
        /// 交付数量
        /// </summary>

        [AdvQueryAttribute(ColName = "DeliveredQty",ColDesc = "交付数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DeliveredQty" ,IsNullable = true,ColumnDescription = "交付数量" )]
        [Display(Name = "交付数量")]
        public int? DeliveredQty 
        { 
            get{return _DeliveredQty;}            set{                SetProperty(ref _DeliveredQty, value);                }
        }

        private int? _InitialQuantity;
        
        
        /// <summary>
        /// 客户申报数量
        /// </summary>

        [AdvQueryAttribute(ColName = "InitialQuantity",ColDesc = "客户申报数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "InitialQuantity" ,IsNullable = true,ColumnDescription = "客户申报数量" )]
        [Display(Name = "客户申报数量")]
        public int? InitialQuantity 
        { 
            get{return _InitialQuantity;}            set{                SetProperty(ref _InitialQuantity, value);                }
        }

        private string _Summary;
        
        
        /// <summary>
        /// 摘要
        /// </summary>

        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=255,IsNullable = true,ColumnDescription = "摘要" )]
        [Display(Name = "摘要")]
        public string Summary 
        { 
            get{return _Summary;}            set{                SetProperty(ref _Summary, value);                }
        }

        private string _property;
        
        
        /// <summary>
        /// 属性
        /// </summary>

        [AdvQueryAttribute(ColName = "property",ColDesc = "属性")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "property" ,Length=255,IsNullable = true,ColumnDescription = "属性" )]
        [Display(Name = "属性")]
        public string property 
        { 
            get{return _property;}            set{                SetProperty(ref _property, value);                }
        }

        private long? _ProdBaseID;
        
        
        /// <summary>
        /// 产品主信息
        /// </summary>

        [AdvQueryAttribute(ColName = "ProdBaseID",ColDesc = "产品主信息")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdBaseID" ,IsNullable = true,ColumnDescription = "产品主信息" )]
        [Display(Name = "产品主信息")]
        public long? ProdBaseID 
        { 
            get{return _ProdBaseID;}            set{                SetProperty(ref _ProdBaseID, value);                }
        }

        private string _SKU;
        
        
        /// <summary>
        /// SKU码
        /// </summary>

        [AdvQueryAttribute(ColName = "SKU",ColDesc = "SKU码")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SKU" ,Length=80,IsNullable = true,ColumnDescription = "SKU码" )]
        [Display(Name = "SKU码")]
        public string SKU 
        { 
            get{return _SKU;}            set{                SetProperty(ref _SKU, value);                }
        }

        private string _CNName;
        
        
        /// <summary>
        /// 品名
        /// </summary>

        [AdvQueryAttribute(ColName = "CNName",ColDesc = "品名")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CNName" ,Length=255,IsNullable = true,ColumnDescription = "品名" )]
        [Display(Name = "品名")]
        public string CNName 
        { 
            get{return _CNName;}            set{                SetProperty(ref _CNName, value);                }
        }

        private long? _Unit_ID;
        
        
        /// <summary>
        /// 单位
        /// </summary>

        [AdvQueryAttribute(ColName = "Unit_ID",ColDesc = "单位")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Unit_ID" ,IsNullable = true,ColumnDescription = "单位" )]
        [Display(Name = "单位")]
        public long? Unit_ID 
        { 
            get{return _Unit_ID;}            set{                SetProperty(ref _Unit_ID, value);                }
        }

        private string _Specifications;
        
        
        /// <summary>
        /// 规格
        /// </summary>

        [AdvQueryAttribute(ColName = "Specifications",ColDesc = "规格")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Specifications" ,Length=1000,IsNullable = true,ColumnDescription = "规格" )]
        [Display(Name = "规格")]
        public string Specifications 
        { 
            get{return _Specifications;}            set{                SetProperty(ref _Specifications, value);                }
        }

        private int? _Quantity;
        
        
        /// <summary>
        /// 实际库存
        /// </summary>

        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "实际库存")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Quantity" ,IsNullable = true,ColumnDescription = "实际库存" )]
        [Display(Name = "实际库存")]
        public int? Quantity 
        { 
            get{return _Quantity;}            set{                SetProperty(ref _Quantity, value);                }
        }

        private string _prop;
        
        
        /// <summary>
        /// 属性
        /// </summary>

        [AdvQueryAttribute(ColName = "prop",ColDesc = "属性")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "prop" ,Length=255,IsNullable = true,ColumnDescription = "属性" )]
        [Display(Name = "属性")]
        public string prop 
        { 
            get{return _prop;}            set{                SetProperty(ref _prop, value);                }
        }

        private string _ProductNo;
        
        
        /// <summary>
        /// 品号
        /// </summary>

        [AdvQueryAttribute(ColName = "ProductNo",ColDesc = "品号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ProductNo" ,Length=40,IsNullable = true,ColumnDescription = "品号" )]
        [Display(Name = "品号")]
        public string ProductNo 
        { 
            get{return _ProductNo;}            set{                SetProperty(ref _ProductNo, value);                }
        }

        private string _Model;
        
        
        /// <summary>
        /// 型号
        /// </summary>

        [AdvQueryAttribute(ColName = "Model",ColDesc = "型号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Model" ,Length=50,IsNullable = true,ColumnDescription = "型号" )]
        [Display(Name = "型号")]
        public string Model 
        { 
            get{return _Model;}            set{                SetProperty(ref _Model, value);                }
        }

        private long? _Category_ID;
        
        
        /// <summary>
        /// 类别
        /// </summary>

        [AdvQueryAttribute(ColName = "Category_ID",ColDesc = "类别")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Category_ID" ,IsNullable = true,ColumnDescription = "类别" )]
        [Display(Name = "类别")]
        public long? Category_ID 
        { 
            get{return _Category_ID;}            set{                SetProperty(ref _Category_ID, value);                }
        }

        private long? _Type_ID;
        
        
        /// <summary>
        /// 产品类型
        /// </summary>

        [AdvQueryAttribute(ColName = "Type_ID",ColDesc = "产品类型")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Type_ID" ,IsNullable = true,ColumnDescription = "产品类型" )]
        [Display(Name = "产品类型")]
        public long? Type_ID 
        { 
            get{return _Type_ID;}            set{                SetProperty(ref _Type_ID, value);                }
        }

        private int? _DataStatus;
        
        
        /// <summary>
        /// 数据状态
        /// </summary>

        [AdvQueryAttribute(ColName = "DataStatus",ColDesc = "数据状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DataStatus" ,IsNullable = true,ColumnDescription = "数据状态" )]
        [Display(Name = "数据状态")]
        public int? DataStatus 
        { 
            get{return _DataStatus;}            set{                SetProperty(ref _DataStatus, value);                }
        }

        private int? _ApprovalStatus;
        
        
        /// <summary>
        /// 审批状态
        /// </summary>

        [AdvQueryAttribute(ColName = "ApprovalStatus",ColDesc = "审批状态")]
        [SugarColumn(ColumnDataType = "tinyint", SqlParameterDbType ="Byte",  ColumnName = "ApprovalStatus" ,IsNullable = true,ColumnDescription = "审批状态" )]
        [Display(Name = "审批状态")]
        public int? ApprovalStatus 
        { 
            get{return _ApprovalStatus;}            set{                SetProperty(ref _ApprovalStatus, value);                }
        }

        private bool? _ApprovalResults;
        
        
        /// <summary>
        /// 审批结果
        /// </summary>

        [AdvQueryAttribute(ColName = "ApprovalResults",ColDesc = "审批结果")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "ApprovalResults" ,IsNullable = true,ColumnDescription = "审批结果" )]
        [Display(Name = "审批结果")]
        public bool? ApprovalResults 
        { 
            get{return _ApprovalResults;}            set{                SetProperty(ref _ApprovalResults, value);                }
        }

        private string _ApprovalOpinions;
        
        
        /// <summary>
        /// 审批意见
        /// </summary>

        [AdvQueryAttribute(ColName = "ApprovalOpinions",ColDesc = "审批意见")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ApprovalOpinions" ,Length=255,IsNullable = true,ColumnDescription = "审批意见" )]
        [Display(Name = "审批意见")]
        public string ApprovalOpinions 
        { 
            get{return _ApprovalOpinions;}            set{                SetProperty(ref _ApprovalOpinions, value);                }
        }

        private string _Notes;
        
        
        /// <summary>
        /// 备注
        /// </summary>

        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=255,IsNullable = true,ColumnDescription = "备注" )]
        [Display(Name = "备注")]
        public string Notes 
        { 
            get{return _Notes;}            set{                SetProperty(ref _Notes, value);                }
        }







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
        [Description("列名中文描述"), Category("自定属性"), Browsable(true)]
        [SugarColumn(IsIgnore = true)]
        public override ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, string>();
                    SugarColumn entityAttr;
                    Type type = typeof(View_AS_AfterSaleApplyItems);
                    
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
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}

