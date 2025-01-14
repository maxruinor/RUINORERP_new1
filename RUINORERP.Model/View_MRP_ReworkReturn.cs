
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/14/2025 22:02:41
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
    /// 返工退库统计 用pb文件生成。选择要生成的视图.检查列的描述。描述不能全空
    /// </summary>
    [Serializable()]
    [SugarTable("View_MRP_ReworkReturn")]
    public class View_MRP_ReworkReturn:BaseEntity, ICloneable
    {
        public View_MRP_ReworkReturn()
        {
            FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("View_MRP_ReworkReturn" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    
        private long? _ReworkReturnID;
        
        
        /// <summary>
        /// 返工退库单
        /// </summary>

        [AdvQueryAttribute(ColName = "ReworkReturnID",ColDesc = "返工退库单")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ReworkReturnID" ,IsNullable = true,ColumnDescription = "返工退库单" )]
        [Display(Name = "返工退库单")]
        public long? ReworkReturnID 
        { 
            get{return _ReworkReturnID;}            set{                SetProperty(ref _ReworkReturnID, value);                }
        }

        private string _ReworkReturnNo;
        
        
        /// <summary>
        /// 退库单号
        /// </summary>

        [AdvQueryAttribute(ColName = "ReworkReturnNo",ColDesc = "退库单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ReworkReturnNo" ,Length=50,IsNullable = true,ColumnDescription = "退库单号" )]
        [Display(Name = "退库单号")]
        public string ReworkReturnNo 
        { 
            get{return _ReworkReturnNo;}            set{                SetProperty(ref _ReworkReturnNo, value);                }
        }

        private long? _MOID;
        
        
        /// <summary>
        /// 制令单
        /// </summary>

        [AdvQueryAttribute(ColName = "MOID",ColDesc = "制令单")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "MOID" ,IsNullable = true,ColumnDescription = "制令单" )]
        [Display(Name = "制令单")]
        public long? MOID 
        { 
            get{return _MOID;}            set{                SetProperty(ref _MOID, value);                }
        }

        private string _MONO;
        
        
        /// <summary>
        /// 制令单号
        /// </summary>

        [AdvQueryAttribute(ColName = "MONO",ColDesc = "制令单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "MONO" ,Length=100,IsNullable = true,ColumnDescription = "制令单号" )]
        [Display(Name = "制令单号")]
        public string MONO 
        { 
            get{return _MONO;}            set{                SetProperty(ref _MONO, value);                }
        }

        private long? _CustomerVendor_ID;
        
        
        /// <summary>
        /// 生产单位
        /// </summary>

        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "生产单位")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" ,IsNullable = true,ColumnDescription = "生产单位" )]
        [Display(Name = "生产单位")]
        public long? CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}            set{                SetProperty(ref _CustomerVendor_ID, value);                }
        }

        private long? _Employee_ID;
        
        
        /// <summary>
        /// 经办人
        /// </summary>

        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "经办人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" ,IsNullable = true,ColumnDescription = "经办人" )]
        [Display(Name = "经办人")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}            set{                SetProperty(ref _Employee_ID, value);                }
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

        private string _CloseCaseOpinions;
        
        
        /// <summary>
        /// 结案情况
        /// </summary>

        [AdvQueryAttribute(ColName = "CloseCaseOpinions",ColDesc = "结案情况")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CloseCaseOpinions" ,Length=200,IsNullable = true,ColumnDescription = "结案情况" )]
        [Display(Name = "结案情况")]
        public string CloseCaseOpinions 
        { 
            get{return _CloseCaseOpinions;}            set{                SetProperty(ref _CloseCaseOpinions, value);                }
        }

        private string _ApprovalOpinions;
        
        
        /// <summary>
        /// 审批意见
        /// </summary>

        [AdvQueryAttribute(ColName = "ApprovalOpinions",ColDesc = "审批意见")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ApprovalOpinions" ,Length=200,IsNullable = true,ColumnDescription = "审批意见" )]
        [Display(Name = "审批意见")]
        public string ApprovalOpinions 
        { 
            get{return _ApprovalOpinions;}            set{                SetProperty(ref _ApprovalOpinions, value);                }
        }

        private long? _DepartmentID;
        
        
        /// <summary>
        /// 需求部门
        /// </summary>

        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "需求部门")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" ,IsNullable = true,ColumnDescription = "需求部门" )]
        [Display(Name = "需求部门")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}            set{                SetProperty(ref _DepartmentID, value);                }
        }

        private DateTime? _ReturnDate;
        
        
        /// <summary>
        /// 退回日期
        /// </summary>

        [AdvQueryAttribute(ColName = "ReturnDate",ColDesc = "退回日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "ReturnDate" ,IsNullable = true,ColumnDescription = "退回日期" )]
        [Display(Name = "退回日期")]
        public DateTime? ReturnDate 
        { 
            get{return _ReturnDate;}            set{                SetProperty(ref _ReturnDate, value);                }
        }

        private DateTime? _ExpectedReturnDate;
        
        
        /// <summary>
        /// 预完工期
        /// </summary>

        [AdvQueryAttribute(ColName = "ExpectedReturnDate",ColDesc = "预完工期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "ExpectedReturnDate" ,IsNullable = true,ColumnDescription = "预完工期" )]
        [Display(Name = "预完工期")]
        public DateTime? ExpectedReturnDate 
        { 
            get{return _ExpectedReturnDate;}            set{                SetProperty(ref _ExpectedReturnDate, value);                }
        }

        private string _ReasonForRework;
        
        
        /// <summary>
        /// 返工原因
        /// </summary>

        [AdvQueryAttribute(ColName = "ReasonForRework",ColDesc = "返工原因")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ReasonForRework" ,Length=500,IsNullable = true,ColumnDescription = "返工原因" )]
        [Display(Name = "返工原因")]
        public string ReasonForRework 
        { 
            get{return _ReasonForRework;}            set{                SetProperty(ref _ReasonForRework, value);                }
        }

        private DateTime? _Approver_at;
        
        
        /// <summary>
        /// 审批时间
        /// </summary>

        [AdvQueryAttribute(ColName = "Approver_at",ColDesc = "审批时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Approver_at" ,IsNullable = true,ColumnDescription = "审批时间" )]
        [Display(Name = "审批时间")]
        public DateTime? Approver_at 
        { 
            get{return _Approver_at;}            set{                SetProperty(ref _Approver_at, value);                }
        }

        private long? _Approver_by;
        
        
        /// <summary>
        /// 审批人
        /// </summary>

        [AdvQueryAttribute(ColName = "Approver_by",ColDesc = "审批人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Approver_by" ,IsNullable = true,ColumnDescription = "审批人" )]
        [Display(Name = "审批人")]
        public long? Approver_by 
        { 
            get{return _Approver_by;}            set{                SetProperty(ref _Approver_by, value);                }
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

        private DateTime? _Modified_at;
        
        
        /// <summary>
        /// 修改时间
        /// </summary>

        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Modified_at" ,IsNullable = true,ColumnDescription = "修改时间" )]
        [Display(Name = "修改时间")]
        public DateTime? Modified_at 
        { 
            get{return _Modified_at;}            set{                SetProperty(ref _Modified_at, value);                }
        }

        private long? _Modified_by;
        
        
        /// <summary>
        /// 修改人
        /// </summary>

        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Modified_by" ,IsNullable = true,ColumnDescription = "修改人" )]
        [Display(Name = "修改人")]
        public long? Modified_by 
        { 
            get{return _Modified_by;}            set{                SetProperty(ref _Modified_by, value);                }
        }

        private long? _ProdDetailID;
        
        
        /// <summary>
        /// 产品
        /// </summary>

        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" ,IsNullable = true,ColumnDescription = "产品" )]
        [Display(Name = "产品")]
        public long? ProdDetailID 
        { 
            get{return _ProdDetailID;}            set{                SetProperty(ref _ProdDetailID, value);                }
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

        private long? _Location_ID;
        
        
        /// <summary>
        /// 所在仓位
        /// </summary>

        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "所在仓位")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Location_ID" ,IsNullable = true,ColumnDescription = "所在仓位" )]
        [Display(Name = "所在仓位")]
        public long? Location_ID 
        { 
            get{return _Location_ID;}            set{                SetProperty(ref _Location_ID, value);                }
        }

        private string _Summary;
        
        
        /// <summary>
        /// 摘要
        /// </summary>

        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=1000,IsNullable = true,ColumnDescription = "摘要" )]
        [Display(Name = "摘要")]
        public string Summary 
        { 
            get{return _Summary;}            set{                SetProperty(ref _Summary, value);                }
        }

        private int? _Quantity;
        
        
        /// <summary>
        /// 数量
        /// </summary>

        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Quantity" ,IsNullable = true,ColumnDescription = "数量" )]
        [Display(Name = "数量")]
        public int? Quantity 
        { 
            get{return _Quantity;}            set{                SetProperty(ref _Quantity, value);                }
        }

        private int? _DeliveredQuantity;
        
        
        /// <summary>
        /// 已交数量
        /// </summary>

        [AdvQueryAttribute(ColName = "DeliveredQuantity",ColDesc = "已交数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DeliveredQuantity" ,IsNullable = true,ColumnDescription = "已交数量" )]
        [Display(Name = "已交数量")]
        public int? DeliveredQuantity 
        { 
            get{return _DeliveredQuantity;}            set{                SetProperty(ref _DeliveredQuantity, value);                }
        }

        private decimal? _ReworkFee;
        
        
        /// <summary>
        /// 预估费用
        /// </summary>

        [AdvQueryAttribute(ColName = "ReworkFee",ColDesc = "预估费用")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "ReworkFee" ,IsNullable = true,ColumnDescription = "预估费用" )]
        [Display(Name = "预估费用")]
        public decimal? ReworkFee 
        { 
            get{return _ReworkFee;}            set{                SetProperty(ref _ReworkFee, value);                }
        }

        private decimal? _SubtotalReworkFee;
        
        
        /// <summary>
        /// 预估费用小计
        /// </summary>

        [AdvQueryAttribute(ColName = "SubtotalReworkFee",ColDesc = "预估费用小计")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalReworkFee" ,IsNullable = true,ColumnDescription = "预估费用小计" )]
        [Display(Name = "预估费用小计")]
        public decimal? SubtotalReworkFee 
        { 
            get{return _SubtotalReworkFee;}            set{                SetProperty(ref _SubtotalReworkFee, value);                }
        }

        private decimal? _UnitCost;
        
        
        /// <summary>
        /// 成本
        /// </summary>

        [AdvQueryAttribute(ColName = "UnitCost",ColDesc = "成本")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UnitCost" ,IsNullable = true,ColumnDescription = "成本" )]
        [Display(Name = "成本")]
        public decimal? UnitCost 
        { 
            get{return _UnitCost;}            set{                SetProperty(ref _UnitCost, value);                }
        }

        private decimal? _SubtotalCostAmount;
        
        
        /// <summary>
        /// 小计
        /// </summary>

        [AdvQueryAttribute(ColName = "SubtotalCostAmount",ColDesc = "小计")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalCostAmount" ,IsNullable = true,ColumnDescription = "小计" )]
        [Display(Name = "小计")]
        public decimal? SubtotalCostAmount 
        { 
            get{return _SubtotalCostAmount;}            set{                SetProperty(ref _SubtotalCostAmount, value);                }
        }

        private string _CustomertModel;
        
        
        /// <summary>
        /// 客户型号
        /// </summary>

        [AdvQueryAttribute(ColName = "CustomertModel",ColDesc = "客户型号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomertModel" ,Length=50,IsNullable = true,ColumnDescription = "客户型号" )]
        [Display(Name = "客户型号")]
        public string CustomertModel 
        { 
            get{return _CustomertModel;}            set{                SetProperty(ref _CustomertModel, value);                }
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

        private string _Notes;
        
        
        /// <summary>
        /// 备注
        /// </summary>

        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=1500,IsNullable = true,ColumnDescription = "备注" )]
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
                    Type type = typeof(View_MRP_ReworkReturn);
                    
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

