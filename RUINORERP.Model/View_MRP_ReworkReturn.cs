﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/06/2025 12:07:03
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
    /// 返工退库统计
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
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "ReworkReturnID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ReworkReturnID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public long? ReworkReturnID 
        { 
            get{return _ReworkReturnID;}
        }

        private string _ReworkReturnNo;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "ReworkReturnNo",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ReworkReturnNo" ,Length=50,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public string ReworkReturnNo 
        { 
            get{return _ReworkReturnNo;}
        }

        private long? _CustomerVendor_ID;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public long? CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}
        }

        private long? _Employee_ID;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}
        }

        private int? _DataStatus;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "DataStatus",ColDesc = "")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DataStatus" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public int? DataStatus 
        { 
            get{return _DataStatus;}
        }

        private string _CloseCaseOpinions;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "CloseCaseOpinions",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CloseCaseOpinions" ,Length=200,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public string CloseCaseOpinions 
        { 
            get{return _CloseCaseOpinions;}
        }

        private string _ApprovalOpinions;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "ApprovalOpinions",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ApprovalOpinions" ,Length=200,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public string ApprovalOpinions 
        { 
            get{return _ApprovalOpinions;}
        }

        private long? _DepartmentID;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}
        }

        private int? _TotalQty;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "TotalQty",ColDesc = "")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "TotalQty" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public int? TotalQty 
        { 
            get{return _TotalQty;}
        }

        private decimal? _TotalReworkFee;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "TotalReworkFee",ColDesc = "")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalReworkFee" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public decimal? TotalReworkFee 
        { 
            get{return _TotalReworkFee;}
        }

        private decimal? _TotalCost;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "TotalCost",ColDesc = "")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalCost" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public decimal? TotalCost 
        { 
            get{return _TotalCost;}
        }

        private DateTime? _ReturnDate;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "ReturnDate",ColDesc = "")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "ReturnDate" ,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public DateTime? ReturnDate 
        { 
            get{return _ReturnDate;}
        }

        private DateTime? _ExpectedReturnDate;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "ExpectedReturnDate",ColDesc = "")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "ExpectedReturnDate" ,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public DateTime? ExpectedReturnDate 
        { 
            get{return _ExpectedReturnDate;}
        }

        private string _ReasonForRework;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "ReasonForRework",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ReasonForRework" ,Length=500,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public string ReasonForRework 
        { 
            get{return _ReasonForRework;}
        }

        private DateTime? _Approver_at;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "Approver_at",ColDesc = "")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Approver_at" ,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public DateTime? Approver_at 
        { 
            get{return _Approver_at;}
        }

        private long? _Approver_by;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "Approver_by",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Approver_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public long? Approver_by 
        { 
            get{return _Approver_by;}
        }

        private int? _ApprovalStatus;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "ApprovalStatus",ColDesc = "")]
        [SugarColumn(ColumnDataType = "tinyint", SqlParameterDbType ="SByte",  ColumnName = "ApprovalStatus" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public int? ApprovalStatus 
        { 
            get{return _ApprovalStatus;}
        }

        private bool? _ApprovalResults;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "ApprovalResults",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "ApprovalResults" ,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public bool? ApprovalResults 
        { 
            get{return _ApprovalResults;}
        }

        private DateTime? _Created_at;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Created_at" ,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}
        }

        private long? _Created_by;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Created_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public long? Created_by 
        { 
            get{return _Created_by;}
        }

        private DateTime? _Modified_at;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Modified_at" ,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public DateTime? Modified_at 
        { 
            get{return _Modified_at;}
        }

        private long? _Modified_by;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Modified_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public long? Modified_by 
        { 
            get{return _Modified_by;}
        }

        private long? _MOID;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "MOID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "MOID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public long? MOID 
        { 
            get{return _MOID;}
        }

        private long _ProdDetailID;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        [Display(Name = "")]
        public long ProdDetailID 
        { 
            get{return _ProdDetailID;}
        }

        private string _SKU;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "SKU",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SKU" ,Length=80,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public string SKU 
        { 
            get{return _SKU;}
        }

        private string _Specifications;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "Specifications",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Specifications" ,Length=1000,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public string Specifications 
        { 
            get{return _Specifications;}
        }

        private string _CNName;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "CNName",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CNName" ,Length=255,IsNullable = false,ColumnDescription = "" )]
        [Display(Name = "")]
        public string CNName 
        { 
            get{return _CNName;}
        }

        private string _Model;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "Model",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Model" ,Length=50,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public string Model 
        { 
            get{return _Model;}
        }

        private long _Type_ID;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "Type_ID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Type_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        [Display(Name = "")]
        public long Type_ID 
        { 
            get{return _Type_ID;}
        }

        private string _property;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "property",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "property" ,Length=255,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public string property 
        { 
            get{return _property;}
        }

        private long _Location_ID;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Location_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        [Display(Name = "")]
        public long Location_ID 
        { 
            get{return _Location_ID;}
        }

        private string _Summary;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "Summary",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=1000,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public string Summary 
        { 
            get{return _Summary;}
        }

        private int _Quantity;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Quantity" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        [Display(Name = "")]
        public int Quantity 
        { 
            get{return _Quantity;}
        }

        private int _DeliveredQuantity;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "DeliveredQuantity",ColDesc = "")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DeliveredQuantity" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        [Display(Name = "")]
        public int DeliveredQuantity 
        { 
            get{return _DeliveredQuantity;}
        }

        private decimal _ReworkFee;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "ReworkFee",ColDesc = "")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "ReworkFee" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "" )]
        [Display(Name = "")]
        public decimal ReworkFee 
        { 
            get{return _ReworkFee;}
        }

        private decimal _SubtotalReworkFee;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "SubtotalReworkFee",ColDesc = "")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalReworkFee" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "" )]
        [Display(Name = "")]
        public decimal SubtotalReworkFee 
        { 
            get{return _SubtotalReworkFee;}
        }

        private decimal _UnitCost;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "UnitCost",ColDesc = "")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UnitCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "" )]
        [Display(Name = "")]
        public decimal UnitCost 
        { 
            get{return _UnitCost;}
        }

        private decimal _SubtotalCostAmount;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "SubtotalCostAmount",ColDesc = "")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalCostAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "" )]
        [Display(Name = "")]
        public decimal SubtotalCostAmount 
        { 
            get{return _SubtotalCostAmount;}
        }

        private string _CustomertModel;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "CustomertModel",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomertModel" ,Length=50,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public string CustomertModel 
        { 
            get{return _CustomertModel;}
        }

        private string _ProductNo;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "ProductNo",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ProductNo" ,Length=40,IsNullable = false,ColumnDescription = "" )]
        [Display(Name = "")]
        public string ProductNo 
        { 
            get{return _ProductNo;}
        }

        private string _Notes;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "Notes",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=1500,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public string Notes 
        { 
            get{return _Notes;}
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
