﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/19/2024 15:29:26
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
    /// 盘点明细统计
    /// </summary>
    [Serializable()]
    [SugarTable("View_StocktakeItems")]
    public class View_StocktakeItems:BaseEntity, ICloneable
    {
        public View_StocktakeItems()
        {
            FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("View_StocktakeItems" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    
        private string _CheckNo;
        
        
        /// <summary>
        /// 盘点单号
        /// </summary>

        [AdvQueryAttribute(ColName = "CheckNo",ColDesc = "盘点单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CheckNo" ,Length=50,IsNullable = true,ColumnDescription = "盘点单号" )]
        [Display(Name = "盘点单号")]
        public string CheckNo 
        { 
            get{return _CheckNo;}
        }

        private long? _Location_ID;
        
        
        /// <summary>
        /// 盘点仓库
        /// </summary>

        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "盘点仓库")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Location_ID" ,IsNullable = true,ColumnDescription = "盘点仓库" )]
        [Display(Name = "盘点仓库")]
        public long? Location_ID 
        { 
            get{return _Location_ID;}
        }

        private long? _Employee_ID;
        
        
        /// <summary>
        /// 内部来源人员
        /// </summary>

        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "内部来源人员")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" ,IsNullable = true,ColumnDescription = "内部来源人员" )]
        [Display(Name = "内部来源人员")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}
        }

        private int? _CheckMode;
        
        
        /// <summary>
        /// 盘点方式
        /// </summary>

        [AdvQueryAttribute(ColName = "CheckMode",ColDesc = "盘点方式")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "CheckMode" ,IsNullable = true,ColumnDescription = "盘点方式" )]
        [Display(Name = "盘点方式")]
        public int? CheckMode 
        { 
            get{return _CheckMode;}
        }

        private int? _Adjust_Type;
        
        
        /// <summary>
        /// 调整类型
        /// </summary>

        [AdvQueryAttribute(ColName = "Adjust_Type",ColDesc = "调整类型")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Adjust_Type" ,IsNullable = true,ColumnDescription = "调整类型" )]
        [Display(Name = "调整类型")]
        public int? Adjust_Type 
        { 
            get{return _Adjust_Type;}
        }

        private int? _CheckResult;
        
        
        /// <summary>
        /// 盘点结果
        /// </summary>

        [AdvQueryAttribute(ColName = "CheckResult",ColDesc = "盘点结果")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "CheckResult" ,IsNullable = true,ColumnDescription = "盘点结果" )]
        [Display(Name = "盘点结果")]
        public int? CheckResult 
        { 
            get{return _CheckResult;}
        }

        private DateTime? _Check_date;
        
        
        /// <summary>
        /// 盘点日期
        /// </summary>

        [AdvQueryAttribute(ColName = "Check_date",ColDesc = "盘点日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Check_date" ,IsNullable = true,ColumnDescription = "盘点日期" )]
        [Display(Name = "盘点日期")]
        public DateTime? Check_date 
        { 
            get{return _Check_date;}
        }

        private DateTime? _CarryingDate;
        
        
        /// <summary>
        /// 载账日期
        /// </summary>

        [AdvQueryAttribute(ColName = "CarryingDate",ColDesc = "载账日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "CarryingDate" ,IsNullable = true,ColumnDescription = "载账日期" )]
        [Display(Name = "载账日期")]
        public DateTime? CarryingDate 
        { 
            get{return _CarryingDate;}
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
            get{return _Created_by;}
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
            get{return _Created_at;}
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
            get{return _Notes;}
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
            get{return _DataStatus;}
        }

        private string _ApprovalOpinions;
        
        
        /// <summary>
        /// 审批意见
        /// </summary>

        [AdvQueryAttribute(ColName = "ApprovalOpinions",ColDesc = "审批意见")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ApprovalOpinions" ,Length=500,IsNullable = true,ColumnDescription = "审批意见" )]
        [Display(Name = "审批意见")]
        public string ApprovalOpinions 
        { 
            get{return _ApprovalOpinions;}
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
            get{return _ApprovalResults;}
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
            get{return _ApprovalStatus;}
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
            get{return _SKU;}
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
            get{return _CNName;}
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
            get{return _Specifications;}
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
            get{return _ProductNo;}
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
            get{return _Model;}
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
            get{return _Category_ID;}
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
            get{return _Type_ID;}
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
            get{return _Unit_ID;}
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
            get{return _ProdDetailID;}
        }

        private long? _Rack_ID;
        
        
        /// <summary>
        /// 货架
        /// </summary>

        [AdvQueryAttribute(ColName = "Rack_ID",ColDesc = "货架")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Rack_ID" ,IsNullable = true,ColumnDescription = "货架" )]
        [Display(Name = "货架")]
        public long? Rack_ID 
        { 
            get{return _Rack_ID;}
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
            get{return _Cost;}
        }

        private int? _CarryinglQty;
        
        
        /// <summary>
        /// 载账数量
        /// </summary>

        [AdvQueryAttribute(ColName = "CarryinglQty",ColDesc = "载账数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "CarryinglQty" ,IsNullable = true,ColumnDescription = "载账数量" )]
        [Display(Name = "载账数量")]
        public int? CarryinglQty 
        { 
            get{return _CarryinglQty;}
        }

        private decimal? _CarryingSubtotalAmount;
        
        
        /// <summary>
        /// 载账小计
        /// </summary>

        [AdvQueryAttribute(ColName = "CarryingSubtotalAmount",ColDesc = "载账小计")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "CarryingSubtotalAmount" ,IsNullable = true,ColumnDescription = "载账小计" )]
        [Display(Name = "载账小计")]
        public decimal? CarryingSubtotalAmount 
        { 
            get{return _CarryingSubtotalAmount;}
        }

        private int? _DiffQty;
        
        
        /// <summary>
        /// 差异数量
        /// </summary>

        [AdvQueryAttribute(ColName = "DiffQty",ColDesc = "差异数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DiffQty" ,IsNullable = true,ColumnDescription = "差异数量" )]
        [Display(Name = "差异数量")]
        public int? DiffQty 
        { 
            get{return _DiffQty;}
        }

        private decimal? _DiffSubtotalAmount;
        
        
        /// <summary>
        /// 差异小计
        /// </summary>

        [AdvQueryAttribute(ColName = "DiffSubtotalAmount",ColDesc = "差异小计")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "DiffSubtotalAmount" ,IsNullable = true,ColumnDescription = "差异小计" )]
        [Display(Name = "差异小计")]
        public decimal? DiffSubtotalAmount 
        { 
            get{return _DiffSubtotalAmount;}
        }

        private int? _CheckQty;
        
        
        /// <summary>
        /// 盘点数量
        /// </summary>

        [AdvQueryAttribute(ColName = "CheckQty",ColDesc = "盘点数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "CheckQty" ,IsNullable = true,ColumnDescription = "盘点数量" )]
        [Display(Name = "盘点数量")]
        public int? CheckQty 
        { 
            get{return _CheckQty;}
        }

        private decimal? _CheckSubtotalAmount;
        
        
        /// <summary>
        /// 盘点小计
        /// </summary>

        [AdvQueryAttribute(ColName = "CheckSubtotalAmount",ColDesc = "盘点小计")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "CheckSubtotalAmount" ,IsNullable = true,ColumnDescription = "盘点小计" )]
        [Display(Name = "盘点小计")]
        public decimal? CheckSubtotalAmount 
        { 
            get{return _CheckSubtotalAmount;}
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
            get{return _property;}
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
                    Type type = typeof(View_StocktakeItems);
                    
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
