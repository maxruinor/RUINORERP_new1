﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/12/2025 18:16:13
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
    /// 采购退货统计
    /// </summary>
    [Serializable()]
    [SugarTable("View_PurEntryReItems")]
    public class View_PurEntryReItemsQueryDto:BaseEntity, ICloneable
    {
        public View_PurEntryReItemsQueryDto()
        {

        }

    
        private string _PurEntryReNo;
        
        
        /// <summary>
        /// 退回单号
        /// </summary>
        [SugarColumn(ColumnName = "PurEntryReNo",Length=50,IsNullable = true,ColumnDescription = "退回单号")]
        [Display(Name = "退回单号")]
        public string PurEntryReNo 
        { 
            get{return _PurEntryReNo;}            set{                SetProperty(ref _PurEntryReNo, value);                }
        }

        private string _PurEntryNo;
        
        
        /// <summary>
        /// 入库单号
        /// </summary>
        [SugarColumn(ColumnName = "PurEntryNo",Length=50,IsNullable = true,ColumnDescription = "入库单号")]
        [Display(Name = "入库单号")]
        public string PurEntryNo 
        { 
            get{return _PurEntryNo;}            set{                SetProperty(ref _PurEntryNo, value);                }
        }

        private long? _CustomerVendor_ID;
        
        
        /// <summary>
        /// 供应商
        /// </summary>
        [SugarColumn(ColumnName = "CustomerVendor_ID",IsNullable = true,ColumnDescription = "供应商")]
        [Display(Name = "供应商")]
        public long? CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}            set{                SetProperty(ref _CustomerVendor_ID, value);                }
        }

        private long? _Employee_ID;
        
        
        /// <summary>
        /// 经办人
        /// </summary>
        [SugarColumn(ColumnName = "Employee_ID",IsNullable = true,ColumnDescription = "经办人")]
        [Display(Name = "经办人")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}            set{                SetProperty(ref _Employee_ID, value);                }
        }

        private long? _DepartmentID;
        
        
        /// <summary>
        /// 部门
        /// </summary>
        [SugarColumn(ColumnName = "DepartmentID",IsNullable = true,ColumnDescription = "部门")]
        [Display(Name = "部门")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}            set{                SetProperty(ref _DepartmentID, value);                }
        }

        private long? _Paytype_ID;
        
        
        /// <summary>
        /// 付款方式
        /// </summary>
        [SugarColumn(ColumnName = "Paytype_ID",IsNullable = true,ColumnDescription = "付款方式")]
        [Display(Name = "付款方式")]
        public long? Paytype_ID 
        { 
            get{return _Paytype_ID;}            set{                SetProperty(ref _Paytype_ID, value);                }
        }

        private DateTime? _ReturnDate;
        
        
        /// <summary>
        /// 退回日期
        /// </summary>
        [SugarColumn(ColumnName = "ReturnDate",IsNullable = true,ColumnDescription = "退回日期")]
        [Display(Name = "退回日期")]
        public DateTime? ReturnDate 
        { 
            get{return _ReturnDate;}            set{                SetProperty(ref _ReturnDate, value);                }
        }

        private string _Notes;
        
        
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "Notes",Length=1500,IsNullable = true,ColumnDescription = "备注")]
        [Display(Name = "备注")]
        public string Notes 
        { 
            get{return _Notes;}            set{                SetProperty(ref _Notes, value);                }
        }

        private int? _TaxDeductionType;
        
        
        /// <summary>
        /// 扣税类型
        /// </summary>
        [SugarColumn(ColumnName = "TaxDeductionType",IsNullable = true,ColumnDescription = "扣税类型")]
        [Display(Name = "扣税类型")]
        public int? TaxDeductionType 
        { 
            get{return _TaxDeductionType;}            set{                SetProperty(ref _TaxDeductionType, value);                }
        }

        private long? _ProdDetailID;
        
        
        /// <summary>
        /// 产品
        /// </summary>
        [SugarColumn(ColumnName = "ProdDetailID",IsNullable = true,ColumnDescription = "产品")]
        [Display(Name = "产品")]
        public long? ProdDetailID 
        { 
            get{return _ProdDetailID;}            set{                SetProperty(ref _ProdDetailID, value);                }
        }

        private string _SKU;
        
        
        /// <summary>
        /// SKU码
        /// </summary>
        [SugarColumn(ColumnName = "SKU",Length=80,IsNullable = true,ColumnDescription = "SKU码")]
        [Display(Name = "SKU码")]
        public string SKU 
        { 
            get{return _SKU;}            set{                SetProperty(ref _SKU, value);                }
        }

        private string _Specifications;
        
        
        /// <summary>
        /// 规格
        /// </summary>
        [SugarColumn(ColumnName = "Specifications",Length=1000,IsNullable = true,ColumnDescription = "规格")]
        [Display(Name = "规格")]
        public string Specifications 
        { 
            get{return _Specifications;}            set{                SetProperty(ref _Specifications, value);                }
        }

        private string _CNName;
        
        
        /// <summary>
        /// 品名
        /// </summary>
        [SugarColumn(ColumnName = "CNName",Length=255,IsNullable = true,ColumnDescription = "品名")]
        [Display(Name = "品名")]
        public string CNName 
        { 
            get{return _CNName;}            set{                SetProperty(ref _CNName, value);                }
        }

        private string _Model;
        
        
        /// <summary>
        /// 型号
        /// </summary>
        [SugarColumn(ColumnName = "Model",Length=50,IsNullable = true,ColumnDescription = "型号")]
        [Display(Name = "型号")]
        public string Model 
        { 
            get{return _Model;}            set{                SetProperty(ref _Model, value);                }
        }

        private long? _Type_ID;
        
        
        /// <summary>
        /// 产品类型
        /// </summary>
        [SugarColumn(ColumnName = "Type_ID",IsNullable = true,ColumnDescription = "产品类型")]
        [Display(Name = "产品类型")]
        public long? Type_ID 
        { 
            get{return _Type_ID;}            set{                SetProperty(ref _Type_ID, value);                }
        }

        private string _property;
        
        
        /// <summary>
        /// 属性
        /// </summary>
        [SugarColumn(ColumnName = "property",Length=255,IsNullable = true,ColumnDescription = "属性")]
        [Display(Name = "属性")]
        public string property 
        { 
            get{return _property;}            set{                SetProperty(ref _property, value);                }
        }

        private long? _Location_ID;
        
        
        /// <summary>
        /// 库位
        /// </summary>
        [SugarColumn(ColumnName = "Location_ID",IsNullable = true,ColumnDescription = "库位")]
        [Display(Name = "库位")]
        public long? Location_ID 
        { 
            get{return _Location_ID;}            set{                SetProperty(ref _Location_ID, value);                }
        }

        private int? _Quantity;
        
        
        /// <summary>
        /// 数量
        /// </summary>
        [SugarColumn(ColumnName = "Quantity",IsNullable = true,ColumnDescription = "数量")]
        [Display(Name = "数量")]
        public int? Quantity 
        { 
            get{return _Quantity;}            set{                SetProperty(ref _Quantity, value);                }
        }

        private decimal? _UnitPrice;
        
        
        /// <summary>
        /// 单价
        /// </summary>
        [SugarColumn(ColumnName = "UnitPrice",IsNullable = true,ColumnDescription = "单价")]
        [Display(Name = "单价")]
        public decimal? UnitPrice 
        { 
            get{return _UnitPrice;}            set{                SetProperty(ref _UnitPrice, value);                }
        }

        private decimal? _TaxRate;
        
        
        /// <summary>
        /// 税率
        /// </summary>
        [SugarColumn(ColumnName = "TaxRate",Length=5,IsNullable = true,ColumnDescription = "税率")]
        [Display(Name = "税率")]
        public decimal? TaxRate 
        { 
            get{return _TaxRate;}            set{                SetProperty(ref _TaxRate, value);                }
        }

        private decimal? _TaxAmount;
        
        
        /// <summary>
        /// 税额
        /// </summary>
        [SugarColumn(ColumnName = "TaxAmount",IsNullable = true,ColumnDescription = "税额")]
        [Display(Name = "税额")]
        public decimal? TaxAmount 
        { 
            get{return _TaxAmount;}            set{                SetProperty(ref _TaxAmount, value);                }
        }

        private decimal? _SubtotalTrPriceAmount;
        
        
        /// <summary>
        /// 小计
        /// </summary>
        [SugarColumn(ColumnName = "SubtotalTrPriceAmount",IsNullable = true,ColumnDescription = "小计")]
        [Display(Name = "小计")]
        public decimal? SubtotalTrPriceAmount 
        { 
            get{return _SubtotalTrPriceAmount;}            set{                SetProperty(ref _SubtotalTrPriceAmount, value);                }
        }

        private bool? _IsGift;
        
        
        /// <summary>
        /// 赠品
        /// </summary>
        [SugarColumn(ColumnName = "IsGift",IsNullable = true,ColumnDescription = "赠品")]
        [Display(Name = "赠品")]
        public bool? IsGift 
        { 
            get{return _IsGift;}            set{                SetProperty(ref _IsGift, value);                }
        }

        private string _CustomertModel;
        
        
        /// <summary>
        /// 客户型号
        /// </summary>
        [SugarColumn(ColumnName = "CustomertModel",Length=50,IsNullable = true,ColumnDescription = "客户型号")]
        [Display(Name = "客户型号")]
        public string CustomertModel 
        { 
            get{return _CustomertModel;}            set{                SetProperty(ref _CustomertModel, value);                }
        }

        private string _Summary;
        
        
        /// <summary>
        /// 摘要
        /// </summary>
        [SugarColumn(ColumnName = "Summary",Length=1000,IsNullable = true,ColumnDescription = "摘要")]
        [Display(Name = "摘要")]
        public string Summary 
        { 
            get{return _Summary;}            set{                SetProperty(ref _Summary, value);                }
        }

        private int? _PrintStatus;
        
        
        /// <summary>
        /// 打印状态
        /// </summary>
        [SugarColumn(ColumnName = "PrintStatus",IsNullable = true,ColumnDescription = "打印状态")]
        [Display(Name = "打印状态")]
        public int? PrintStatus 
        { 
            get{return _PrintStatus;}            set{                SetProperty(ref _PrintStatus, value);                }
        }

        private int? _DataStatus;
        
        
        /// <summary>
        /// 数据状态
        /// </summary>
        [SugarColumn(ColumnName = "DataStatus",IsNullable = true,ColumnDescription = "数据状态")]
        [Display(Name = "数据状态")]
        public int? DataStatus 
        { 
            get{return _DataStatus;}            set{                SetProperty(ref _DataStatus, value);                }
        }

        private int? _ApprovalStatus;
        
        
        /// <summary>
        /// 审批状态
        /// </summary>
        [SugarColumn(ColumnName = "ApprovalStatus",IsNullable = true,ColumnDescription = "审批状态")]
        [Display(Name = "审批状态")]
        public int? ApprovalStatus 
        { 
            get{return _ApprovalStatus;}            set{                SetProperty(ref _ApprovalStatus, value);                }
        }

        private bool? _ApprovalResults;
        
        
        /// <summary>
        /// 审批结果
        /// </summary>
        [SugarColumn(ColumnName = "ApprovalResults",IsNullable = true,ColumnDescription = "审批结果")]
        [Display(Name = "审批结果")]
        public bool? ApprovalResults 
        { 
            get{return _ApprovalResults;}            set{                SetProperty(ref _ApprovalResults, value);                }
        }

        private string _ApprovalOpinions;
        
        
        /// <summary>
        /// 审批意见
        /// </summary>
        [SugarColumn(ColumnName = "ApprovalOpinions",Length=200,IsNullable = true,ColumnDescription = "审批意见")]
        [Display(Name = "审批意见")]
        public string ApprovalOpinions 
        { 
            get{return _ApprovalOpinions;}            set{                SetProperty(ref _ApprovalOpinions, value);                }
        }

        private decimal? _TotalAmount;
        
        
        /// <summary>
        /// 合计金额
        /// </summary>
        [SugarColumn(ColumnName = "TotalAmount",IsNullable = true,ColumnDescription = "合计金额")]
        [Display(Name = "合计金额")]
        public decimal? TotalAmount 
        { 
            get{return _TotalAmount;}            set{                SetProperty(ref _TotalAmount, value);                }
        }

        private decimal? _TotalTaxAmount;
        
        
        /// <summary>
        /// 合计税额
        /// </summary>
        [SugarColumn(ColumnName = "TotalTaxAmount",IsNullable = true,ColumnDescription = "合计税额")]
        [Display(Name = "合计税额")]
        public decimal? TotalTaxAmount 
        { 
            get{return _TotalTaxAmount;}            set{                SetProperty(ref _TotalTaxAmount, value);                }
        }

        private int? _TotalQty;
        
        
        /// <summary>
        /// 合计数量
        /// </summary>
        [SugarColumn(ColumnName = "TotalQty",IsNullable = true,ColumnDescription = "合计数量")]
        [Display(Name = "合计数量")]
        public int? TotalQty 
        { 
            get{return _TotalQty;}            set{                SetProperty(ref _TotalQty, value);                }
        }

        private long? _Category_ID;
        
        
        /// <summary>
        /// 类别
        /// </summary>
        [SugarColumn(ColumnName = "Category_ID",IsNullable = true,ColumnDescription = "类别")]
        [Display(Name = "类别")]
        public long? Category_ID 
        { 
            get{return _Category_ID;}            set{                SetProperty(ref _Category_ID, value);                }
        }

        private long? _Rack_ID;
        
        
        /// <summary>
        /// 货架
        /// </summary>
        [SugarColumn(ColumnName = "Rack_ID",IsNullable = true,ColumnDescription = "货架")]
        [Display(Name = "货架")]
        public long? Rack_ID 
        { 
            get{return _Rack_ID;}            set{                SetProperty(ref _Rack_ID, value);                }
        }

        private string _UnitName;
        
        
        /// <summary>
        /// 单位名称
        /// </summary>
        [SugarColumn(ColumnName = "UnitName",Length=255,IsNullable = true,ColumnDescription = "单位名称")]
        [Display(Name = "单位名称")]
        public string UnitName 
        { 
            get{return _UnitName;}            set{                SetProperty(ref _UnitName, value);                }
        }

        private bool? _IsIncludeTax;
        
        
        /// <summary>
        /// 含税
        /// </summary>
        [SugarColumn(ColumnName = "IsIncludeTax",IsNullable = true,ColumnDescription = "含税")]
        [Display(Name = "含税")]
        public bool? IsIncludeTax 
        { 
            get{return _IsIncludeTax;}            set{                SetProperty(ref _IsIncludeTax, value);                }
        }





  
        

        public override object Clone()
        {
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}

