﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/06/2025 12:06:59
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
    /// 返工入库统计
    /// </summary>
    [Serializable()]
    [SugarTable("View_MRP_ReworkEntry")]
    public class View_MRP_ReworkEntryQueryDto:BaseEntity, ICloneable
    {
        public View_MRP_ReworkEntryQueryDto()
        {

        }

    
        private long? _ReworkEntryID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ReworkEntryID",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? ReworkEntryID 
        { 
            get{return _ReworkEntryID;}
        }

        private string _ReworkEntryNo;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ReworkEntryNo",Length=50,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string ReworkEntryNo 
        { 
            get{return _ReworkEntryNo;}
        }

        private long? _ReworkReturnID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ReworkReturnID",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? ReworkReturnID 
        { 
            get{return _ReworkReturnID;}
        }

        private DateTime? _EntryDate;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "EntryDate",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public DateTime? EntryDate 
        { 
            get{return _EntryDate;}
        }

        private long? _CustomerVendor_ID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "CustomerVendor_ID",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}
        }

        private long? _Employee_ID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Employee_ID",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}
        }

        private string _Notes;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Notes",Length=1500,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string Notes 
        { 
            get{return _Notes;}
        }

        private int? _DataStatus;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "DataStatus",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public int? DataStatus 
        { 
            get{return _DataStatus;}
        }

        private string _ApprovalOpinions;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ApprovalOpinions",Length=200,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string ApprovalOpinions 
        { 
            get{return _ApprovalOpinions;}
        }

        private long _ProdDetailID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ProdDetailID",IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public long ProdDetailID 
        { 
            get{return _ProdDetailID;}
        }

        private string _SKU;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "SKU",Length=80,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string SKU 
        { 
            get{return _SKU;}
        }

        private string _Specifications;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Specifications",Length=1000,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string Specifications 
        { 
            get{return _Specifications;}
        }

        private string _CNName;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "CNName",Length=255,IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public string CNName 
        { 
            get{return _CNName;}
        }

        private string _Model;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Model",Length=50,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string Model 
        { 
            get{return _Model;}
        }

        private long _Type_ID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Type_ID",IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public long Type_ID 
        { 
            get{return _Type_ID;}
        }

        private string _property;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "property",Length=255,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string property 
        { 
            get{return _property;}
        }

        private long _Location_ID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Location_ID",IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public long Location_ID 
        { 
            get{return _Location_ID;}
        }

        private string _Summary;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Summary",Length=1000,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string Summary 
        { 
            get{return _Summary;}
        }

        private long? _DepartmentID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "DepartmentID",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}
        }

        private int? _TotalQty;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "TotalQty",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public int? TotalQty 
        { 
            get{return _TotalQty;}
        }

        private decimal? _TotalReworkFee;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "TotalReworkFee",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal? TotalReworkFee 
        { 
            get{return _TotalReworkFee;}
        }

        private decimal? _TotalCost;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "TotalCost",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal? TotalCost 
        { 
            get{return _TotalCost;}
        }

        private DateTime? _Approver_at;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Approver_at",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public DateTime? Approver_at 
        { 
            get{return _Approver_at;}
        }

        private long? _Approver_by;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Approver_by",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? Approver_by 
        { 
            get{return _Approver_by;}
        }

        private int? _ApprovalStatus;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ApprovalStatus",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public int? ApprovalStatus 
        { 
            get{return _ApprovalStatus;}
        }

        private bool? _ApprovalResults;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ApprovalResults",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public bool? ApprovalResults 
        { 
            get{return _ApprovalResults;}
        }

        private DateTime? _Created_at;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Created_at",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}
        }

        private long? _Created_by;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Created_by",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? Created_by 
        { 
            get{return _Created_by;}
        }

        private DateTime? _Modified_at;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Modified_at",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public DateTime? Modified_at 
        { 
            get{return _Modified_at;}
        }

        private long? _Modified_by;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Modified_by",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? Modified_by 
        { 
            get{return _Modified_by;}
        }

        private int _Quantity;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Quantity",IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public int Quantity 
        { 
            get{return _Quantity;}
        }

        private decimal _ReworkFee;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ReworkFee",IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal ReworkFee 
        { 
            get{return _ReworkFee;}
        }

        private decimal _SubtotalReworkFee;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "SubtotalReworkFee",IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal SubtotalReworkFee 
        { 
            get{return _SubtotalReworkFee;}
        }

        private decimal _UnitCost;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "UnitCost",IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal UnitCost 
        { 
            get{return _UnitCost;}
        }

        private decimal _SubtotalCostAmount;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "SubtotalCostAmount",IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal SubtotalCostAmount 
        { 
            get{return _SubtotalCostAmount;}
        }

        private string _CustomertModel;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "CustomertModel",Length=50,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string CustomertModel 
        { 
            get{return _CustomertModel;}
        }

        private string _ProductNo;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ProductNo",Length=40,IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public string ProductNo 
        { 
            get{return _ProductNo;}
        }





  
        

        public override object Clone()
        {
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}
