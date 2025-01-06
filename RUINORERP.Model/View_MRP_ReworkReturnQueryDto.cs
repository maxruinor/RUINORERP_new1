
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
    public class View_MRP_ReworkReturnQueryDto:BaseEntity, ICloneable
    {
        public View_MRP_ReworkReturnQueryDto()
        {

        }

    
        private long? _ReworkReturnID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ReworkReturnID",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? ReworkReturnID 
        { 
            get{return _ReworkReturnID;}            set{                SetProperty(ref _ReworkReturnID, value);                }
        }

        private string _ReworkReturnNo;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ReworkReturnNo",Length=50,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string ReworkReturnNo 
        { 
            get{return _ReworkReturnNo;}            set{                SetProperty(ref _ReworkReturnNo, value);                }
        }

        private long? _CustomerVendor_ID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "CustomerVendor_ID",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}            set{                SetProperty(ref _CustomerVendor_ID, value);                }
        }

        private long? _Employee_ID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Employee_ID",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}            set{                SetProperty(ref _Employee_ID, value);                }
        }

        private int? _DataStatus;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "DataStatus",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public int? DataStatus 
        { 
            get{return _DataStatus;}            set{                SetProperty(ref _DataStatus, value);                }
        }

        private string _CloseCaseOpinions;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "CloseCaseOpinions",Length=200,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string CloseCaseOpinions 
        { 
            get{return _CloseCaseOpinions;}            set{                SetProperty(ref _CloseCaseOpinions, value);                }
        }

        private string _ApprovalOpinions;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ApprovalOpinions",Length=200,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string ApprovalOpinions 
        { 
            get{return _ApprovalOpinions;}            set{                SetProperty(ref _ApprovalOpinions, value);                }
        }

        private long? _DepartmentID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "DepartmentID",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}            set{                SetProperty(ref _DepartmentID, value);                }
        }

        private int? _TotalQty;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "TotalQty",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public int? TotalQty 
        { 
            get{return _TotalQty;}            set{                SetProperty(ref _TotalQty, value);                }
        }

        private decimal? _TotalReworkFee;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "TotalReworkFee",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal? TotalReworkFee 
        { 
            get{return _TotalReworkFee;}            set{                SetProperty(ref _TotalReworkFee, value);                }
        }

        private decimal? _TotalCost;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "TotalCost",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal? TotalCost 
        { 
            get{return _TotalCost;}            set{                SetProperty(ref _TotalCost, value);                }
        }

        private DateTime? _ReturnDate;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ReturnDate",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public DateTime? ReturnDate 
        { 
            get{return _ReturnDate;}            set{                SetProperty(ref _ReturnDate, value);                }
        }

        private DateTime? _ExpectedReturnDate;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ExpectedReturnDate",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public DateTime? ExpectedReturnDate 
        { 
            get{return _ExpectedReturnDate;}            set{                SetProperty(ref _ExpectedReturnDate, value);                }
        }

        private string _ReasonForRework;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ReasonForRework",Length=500,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string ReasonForRework 
        { 
            get{return _ReasonForRework;}            set{                SetProperty(ref _ReasonForRework, value);                }
        }

        private DateTime? _Approver_at;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Approver_at",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public DateTime? Approver_at 
        { 
            get{return _Approver_at;}            set{                SetProperty(ref _Approver_at, value);                }
        }

        private long? _Approver_by;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Approver_by",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? Approver_by 
        { 
            get{return _Approver_by;}            set{                SetProperty(ref _Approver_by, value);                }
        }

        private int? _ApprovalStatus;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ApprovalStatus",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public int? ApprovalStatus 
        { 
            get{return _ApprovalStatus;}            set{                SetProperty(ref _ApprovalStatus, value);                }
        }

        private bool? _ApprovalResults;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ApprovalResults",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public bool? ApprovalResults 
        { 
            get{return _ApprovalResults;}            set{                SetProperty(ref _ApprovalResults, value);                }
        }

        private DateTime? _Created_at;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Created_at",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}            set{                SetProperty(ref _Created_at, value);                }
        }

        private long? _Created_by;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Created_by",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? Created_by 
        { 
            get{return _Created_by;}            set{                SetProperty(ref _Created_by, value);                }
        }

        private DateTime? _Modified_at;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Modified_at",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public DateTime? Modified_at 
        { 
            get{return _Modified_at;}            set{                SetProperty(ref _Modified_at, value);                }
        }

        private long? _Modified_by;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Modified_by",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? Modified_by 
        { 
            get{return _Modified_by;}            set{                SetProperty(ref _Modified_by, value);                }
        }

        private long? _MOID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "MOID",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? MOID 
        { 
            get{return _MOID;}            set{                SetProperty(ref _MOID, value);                }
        }

        private long _ProdDetailID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ProdDetailID",IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public long ProdDetailID 
        { 
            get{return _ProdDetailID;}            set{                SetProperty(ref _ProdDetailID, value);                }
        }

        private string _SKU;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "SKU",Length=80,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string SKU 
        { 
            get{return _SKU;}            set{                SetProperty(ref _SKU, value);                }
        }

        private string _Specifications;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Specifications",Length=1000,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string Specifications 
        { 
            get{return _Specifications;}            set{                SetProperty(ref _Specifications, value);                }
        }

        private string _CNName;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "CNName",Length=255,IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public string CNName 
        { 
            get{return _CNName;}            set{                SetProperty(ref _CNName, value);                }
        }

        private string _Model;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Model",Length=50,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string Model 
        { 
            get{return _Model;}            set{                SetProperty(ref _Model, value);                }
        }

        private long _Type_ID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Type_ID",IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public long Type_ID 
        { 
            get{return _Type_ID;}            set{                SetProperty(ref _Type_ID, value);                }
        }

        private string _property;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "property",Length=255,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string property 
        { 
            get{return _property;}            set{                SetProperty(ref _property, value);                }
        }

        private long _Location_ID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Location_ID",IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public long Location_ID 
        { 
            get{return _Location_ID;}            set{                SetProperty(ref _Location_ID, value);                }
        }

        private string _Summary;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Summary",Length=1000,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string Summary 
        { 
            get{return _Summary;}            set{                SetProperty(ref _Summary, value);                }
        }

        private int _Quantity;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Quantity",IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public int Quantity 
        { 
            get{return _Quantity;}            set{                SetProperty(ref _Quantity, value);                }
        }

        private int _DeliveredQuantity;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "DeliveredQuantity",IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public int DeliveredQuantity 
        { 
            get{return _DeliveredQuantity;}            set{                SetProperty(ref _DeliveredQuantity, value);                }
        }

        private decimal _ReworkFee;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ReworkFee",IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal ReworkFee 
        { 
            get{return _ReworkFee;}            set{                SetProperty(ref _ReworkFee, value);                }
        }

        private decimal _SubtotalReworkFee;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "SubtotalReworkFee",IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal SubtotalReworkFee 
        { 
            get{return _SubtotalReworkFee;}            set{                SetProperty(ref _SubtotalReworkFee, value);                }
        }

        private decimal _UnitCost;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "UnitCost",IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal UnitCost 
        { 
            get{return _UnitCost;}            set{                SetProperty(ref _UnitCost, value);                }
        }

        private decimal _SubtotalCostAmount;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "SubtotalCostAmount",IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal SubtotalCostAmount 
        { 
            get{return _SubtotalCostAmount;}            set{                SetProperty(ref _SubtotalCostAmount, value);                }
        }

        private string _CustomertModel;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "CustomertModel",Length=50,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string CustomertModel 
        { 
            get{return _CustomertModel;}            set{                SetProperty(ref _CustomertModel, value);                }
        }

        private string _ProductNo;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ProductNo",Length=40,IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public string ProductNo 
        { 
            get{return _ProductNo;}            set{                SetProperty(ref _ProductNo, value);                }
        }

        private string _Notes;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Notes",Length=1500,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string Notes 
        { 
            get{return _Notes;}            set{                SetProperty(ref _Notes, value);                }
        }





  
        

        public override object Clone()
        {
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}

