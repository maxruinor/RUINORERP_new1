
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/19/2024 11:25:41
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
    /// 计划单明细统计
    /// </summary>
    [Serializable()]
    [SugarTable("View_ProductionPlanItems")]
    public partial class View_ProductionPlanItems: BaseViewEntity
    {
        public View_ProductionPlanItems()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("View_ProductionPlanItems" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    


        private string _SaleOrderNo;
        
        
        /// <summary>
        /// 销售单号
        /// </summary>

        [AdvQueryAttribute(ColName = "SaleOrderNo",ColDesc = "销售单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SaleOrderNo" ,Length=50,IsNullable = true,ColumnDescription = "销售单号" )]
        [Display(Name = "销售单号")]
        public string SaleOrderNo 
        { 
            get{return _SaleOrderNo;}            set{                SetProperty(ref _SaleOrderNo, value);                }
        }

        private string _PPNo;
        
        
        /// <summary>
        /// 计划单号
        /// </summary>

        [AdvQueryAttribute(ColName = "PPNo",ColDesc = "计划单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PPNo" ,Length=100,IsNullable = true,ColumnDescription = "计划单号" )]
        [Display(Name = "计划单号")]
        public string PPNo 
        { 
            get{return _PPNo;}            set{                SetProperty(ref _PPNo, value);                }
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

        private DateTime? _RequirementDate;
        
        
        /// <summary>
        /// 需求日期
        /// </summary>

        [AdvQueryAttribute(ColName = "RequirementDate",ColDesc = "需求日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "RequirementDate" ,IsNullable = true,ColumnDescription = "需求日期" )]
        [Display(Name = "需求日期")]
        public DateTime? RequirementDate 
        { 
            get{return _RequirementDate;}            set{                SetProperty(ref _RequirementDate, value);                }
        }

        private DateTime? _PlanDate;
        
        
        /// <summary>
        /// 制单日期
        /// </summary>

        [AdvQueryAttribute(ColName = "PlanDate",ColDesc = "制单日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "PlanDate" ,IsNullable = true,ColumnDescription = "制单日期" )]
        [Display(Name = "制单日期")]
        public DateTime? PlanDate 
        { 
            get{return _PlanDate;}            set{                SetProperty(ref _PlanDate, value);                }
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
        /// 库位
        /// </summary>

        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "库位")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Location_ID" ,IsNullable = true,ColumnDescription = "库位" )]
        [Display(Name = "库位")]
        public long? Location_ID 
        { 
            get{return _Location_ID;}            set{                SetProperty(ref _Location_ID, value);                }
        }

        private int? _Quantity;
        
        
        /// <summary>
        /// 计划数量
        /// </summary>

        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "计划数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Quantity" ,IsNullable = true,ColumnDescription = "计划数量" )]
        [Display(Name = "计划数量")]
        public int? Quantity 
        { 
            get{return _Quantity;}            set{                SetProperty(ref _Quantity, value);                }
        }

        private long? _BOM_ID;
        
        
        /// <summary>
        /// 配方名称
        /// </summary>

        [AdvQueryAttribute(ColName = "BOM_ID",ColDesc = "配方名称")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BOM_ID" ,IsNullable = true,ColumnDescription = "配方名称" )]
        [Display(Name = "配方名称")]
        public long? BOM_ID 
        { 
            get{return _BOM_ID;}            set{                SetProperty(ref _BOM_ID, value);                }
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

        private bool? _isdeleted;
        
        
        /// <summary>
        /// 逻辑删除
        /// </summary>

        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "isdeleted" ,IsNullable = true,ColumnDescription = "逻辑删除" )]
        [Browsable(false)]
        [Display(Name = "逻辑删除")]
        public bool? isdeleted 
        { 
            get{return _isdeleted;}            set{                SetProperty(ref _isdeleted, value);                }
        }

        private bool? _IsAnalyzed;
        
        
        /// <summary>
        /// 已分析
        /// </summary>

        [AdvQueryAttribute(ColName = "IsAnalyzed",ColDesc = "已分析")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsAnalyzed" ,IsNullable = true,ColumnDescription = "已分析" )]
        [Display(Name = "已分析")]
        public bool? IsAnalyzed 
        { 
            get{return _IsAnalyzed;}            set{                SetProperty(ref _IsAnalyzed, value);                }
        }

        private int? _AnalyzedQuantity;
        
        
        /// <summary>
        /// 分析数量
        /// </summary>

        [AdvQueryAttribute(ColName = "AnalyzedQuantity",ColDesc = "分析数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "AnalyzedQuantity" ,IsNullable = true,ColumnDescription = "分析数量" )]
        [Display(Name = "分析数量")]
        public int? AnalyzedQuantity 
        { 
            get{return _AnalyzedQuantity;}            set{                SetProperty(ref _AnalyzedQuantity, value);                }
        }

        private int? _CompletedQuantity;
        
        
        /// <summary>
        /// 完成数量
        /// </summary>

        [AdvQueryAttribute(ColName = "CompletedQuantity",ColDesc = "完成数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "CompletedQuantity" ,IsNullable = true,ColumnDescription = "完成数量" )]
        [Display(Name = "完成数量")]
        public int? CompletedQuantity 
        { 
            get{return _CompletedQuantity;}            set{                SetProperty(ref _CompletedQuantity, value);                }
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







//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}


  

        public override object Clone()
        {
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}

