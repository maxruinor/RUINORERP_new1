
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/19/2024 11:25:36
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
    /// 领料统计
    /// </summary>
    [Serializable()]
    [SugarTable("View_MaterialRequisitionItems")]
    public class View_MaterialRequisitionItems:BaseEntity, ICloneable
    {
        public View_MaterialRequisitionItems()
        {
            FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("View_MaterialRequisitionItems" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    
 

        private string _MaterialRequisitionNO;
        
        
        /// <summary>
        /// 领料单号
        /// </summary>

        [AdvQueryAttribute(ColName = "MaterialRequisitionNO",ColDesc = "领料单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "MaterialRequisitionNO" ,Length=50,IsNullable = true,ColumnDescription = "领料单号" )]
        [Display(Name = "领料单号")]
        public string MaterialRequisitionNO 
        { 
            get{return _MaterialRequisitionNO;}            set{                SetProperty(ref _MaterialRequisitionNO, value);                }
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

        private DateTime? _DeliveryDate;
        
        
        /// <summary>
        /// 发货日期
        /// </summary>

        [AdvQueryAttribute(ColName = "DeliveryDate",ColDesc = "发货日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "DeliveryDate" ,IsNullable = true,ColumnDescription = "发货日期" )]
        [Display(Name = "发货日期")]
        public DateTime? DeliveryDate 
        { 
            get{return _DeliveryDate;}            set{                SetProperty(ref _DeliveryDate, value);                }
        }

        private long? _Employee_ID;
        
        
        /// <summary>
        /// 业务员
        /// </summary>

        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "业务员")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" ,IsNullable = true,ColumnDescription = "业务员" )]
        [Display(Name = "业务员")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}            set{                SetProperty(ref _Employee_ID, value);                }
        }

        private long? _DepartmentID;
        
        
        /// <summary>
        /// 生产部门
        /// </summary>

        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "生产部门")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" ,IsNullable = true,ColumnDescription = "生产部门" )]
        [Display(Name = "生产部门")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}            set{                SetProperty(ref _DepartmentID, value);                }
        }

        private long? _CustomerVendor_ID;
        
        
        /// <summary>
        /// 客户
        /// </summary>

        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "客户")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" ,IsNullable = true,ColumnDescription = "客户" )]
        [Display(Name = "客户")]
        public long? CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}            set{                SetProperty(ref _CustomerVendor_ID, value);                }
        }

 

        private long? _ProjectGroup_ID;
        
        
        /// <summary>
        /// 项目组
        /// </summary>

        [AdvQueryAttribute(ColName = "ProjectGroup_ID",ColDesc = "项目组")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProjectGroup_ID" ,IsNullable = true,ColumnDescription = "项目组" )]
        [Display(Name = "项目组")]
        public long? ProjectGroup_ID 
        { 
            get{return _ProjectGroup_ID;}            set{                SetProperty(ref _ProjectGroup_ID, value);                }
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

        private int? _ShouldSendQty;
        
        
        /// <summary>
        /// 应发数
        /// </summary>

        [AdvQueryAttribute(ColName = "ShouldSendQty",ColDesc = "应发数")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ShouldSendQty" ,IsNullable = true,ColumnDescription = "应发数" )]
        [Display(Name = "应发数")]
        public int? ShouldSendQty 
        { 
            get{return _ShouldSendQty;}            set{                SetProperty(ref _ShouldSendQty, value);                }
        }

        private int? _ActualSentQty;
        
        
        /// <summary>
        /// 实发数
        /// </summary>

        [AdvQueryAttribute(ColName = "ActualSentQty",ColDesc = "实发数")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ActualSentQty" ,IsNullable = true,ColumnDescription = "实发数" )]
        [Display(Name = "实发数")]
        public int? ActualSentQty 
        { 
            get{return _ActualSentQty;}            set{                SetProperty(ref _ActualSentQty, value);                }
        }

        private int? _CanQuantity;
        
        
        /// <summary>
        /// 可发数
        /// </summary>

        [AdvQueryAttribute(ColName = "CanQuantity",ColDesc = "可发数")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "CanQuantity" ,IsNullable = true,ColumnDescription = "可发数" )]
        [Display(Name = "可发数")]
        public int? CanQuantity 
        { 
            get{return _CanQuantity;}            set{                SetProperty(ref _CanQuantity, value);                }
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

        private string _CustomerPartNo;
        
        
        /// <summary>
        /// 客户型号
        /// </summary>

        [AdvQueryAttribute(ColName = "CustomerPartNo",ColDesc = "客户型号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomerPartNo" ,Length=50,IsNullable = true,ColumnDescription = "客户型号" )]
        [Display(Name = "客户型号")]
        public string CustomerPartNo 
        { 
            get{return _CustomerPartNo;}            set{                SetProperty(ref _CustomerPartNo, value);                }
        }

        private decimal? _Cost;
        
        
        /// <summary>
        /// 成本
        /// </summary>

        [AdvQueryAttribute(ColName = "Cost",ColDesc = "成本")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Cost" ,IsNullable = true,ColumnDescription = "成本" )]
        [Display(Name = "成本")]
        public decimal? Cost 
        { 
            get{return _Cost;}            set{                SetProperty(ref _Cost, value);                }
        }

        private decimal? _Price;
        
        
        /// <summary>
        /// 价格
        /// </summary>

        [AdvQueryAttribute(ColName = "Price",ColDesc = "价格")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Price" ,IsNullable = true,ColumnDescription = "价格" )]
        [Display(Name = "价格")]
        public decimal? Price 
        { 
            get{return _Price;}            set{                SetProperty(ref _Price, value);                }
        }

        private decimal? _SubtotalPrice;
        
        
        /// <summary>
        /// 金额小计
        /// </summary>

        [AdvQueryAttribute(ColName = "SubtotalPrice",ColDesc = "金额小计")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalPrice" ,IsNullable = true,ColumnDescription = "金额小计" )]
        [Display(Name = "金额小计")]
        public decimal? SubtotalPrice 
        { 
            get{return _SubtotalPrice;}            set{                SetProperty(ref _SubtotalPrice, value);                }
        }

        private decimal? _SubtotalCost;
        
        
        /// <summary>
        /// 成本小计
        /// </summary>

        [AdvQueryAttribute(ColName = "SubtotalCost",ColDesc = "成本小计")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalCost" ,IsNullable = true,ColumnDescription = "成本小计" )]
        [Display(Name = "成本小计")]
        public decimal? SubtotalCost 
        { 
            get{return _SubtotalCost;}            set{                SetProperty(ref _SubtotalCost, value);                }
        }

        private int? _ReturnQty;
        
        
        /// <summary>
        /// 退回数量
        /// </summary>

        [AdvQueryAttribute(ColName = "ReturnQty",ColDesc = "退回数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ReturnQty" ,IsNullable = true,ColumnDescription = "退回数量" )]
        [Display(Name = "退回数量")]
        public int? ReturnQty 
        { 
            get{return _ReturnQty;}            set{                SetProperty(ref _ReturnQty, value);                }
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

        private string _ShortCode;
        
        
        /// <summary>
        /// 短码
        /// </summary>

        [AdvQueryAttribute(ColName = "ShortCode",ColDesc = "短码")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ShortCode" ,Length=50,IsNullable = true,ColumnDescription = "短码" )]
        [Display(Name = "短码")]
        public string ShortCode 
        { 
            get{return _ShortCode;}            set{                SetProperty(ref _ShortCode, value);                }
        }

        private string _BarCode;
        
        
        /// <summary>
        /// 条码
        /// </summary>

        [AdvQueryAttribute(ColName = "BarCode",ColDesc = "条码")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BarCode" ,Length=50,IsNullable = true,ColumnDescription = "条码" )]
        [Display(Name = "条码")]
        public string BarCode 
        { 
            get{return _BarCode;}            set{                SetProperty(ref _BarCode, value);                }
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
                    Type type = typeof(View_MaterialRequisitionItems);
                    
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

