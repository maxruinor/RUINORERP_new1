
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/16/2024 11:43:12
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
    /// 销售退货统计分析
    /// </summary>
    [Serializable()]
    [SugarTable("View_SaleOutReItems")]
    public partial class View_SaleOutReItems:BaseViewEntity
    {
        public View_SaleOutReItems()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("View_SaleOutReItems" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
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

        private long? _ProjectGroup_ID;
        /// <summary>
        /// 项目组
        /// </summary>
        [AdvQueryAttribute(ColName = "ProjectGroup_ID", ColDesc = "项目组")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "ProjectGroup_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "项目组")]
        [FKRelationAttribute("tb_ProjectGroup", "ProjectGroup_ID")]
        public long? ProjectGroup_ID
        {
            get { return _ProjectGroup_ID; }
            set
            {
                SetProperty(ref _ProjectGroup_ID, value);
            }
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

        private string _ReturnNo;
        
        
        /// <summary>
        /// 退回单号
        /// </summary>

        [AdvQueryAttribute(ColName = "ReturnNo",ColDesc = "退回单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ReturnNo" ,Length=50,IsNullable = true,ColumnDescription = "退回单号" )]
        [Display(Name = "退回单号")]
        public string ReturnNo 
        { 
            get{return _ReturnNo;}            set{                SetProperty(ref _ReturnNo, value);                }
        }

        private int? _PayStatus;
        
        
        /// <summary>
        /// 付款状态
        /// </summary>

        [AdvQueryAttribute(ColName = "PayStatus",ColDesc = "付款状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "PayStatus" ,IsNullable = true,ColumnDescription = "付款状态" )]
        [Display(Name = "付款状态")]
        public int? PayStatus 
        { 
            get{return _PayStatus;}            set{                SetProperty(ref _PayStatus, value);                }
        }

        private long? _Paytype_ID;
        
        
        /// <summary>
        /// 付款方式
        /// </summary>

        [AdvQueryAttribute(ColName = "Paytype_ID",ColDesc = "付款方式")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Paytype_ID" ,IsNullable = true,ColumnDescription = "付款方式" )]
        [Display(Name = "付款方式")]
        public long? Paytype_ID 
        { 
            get{return _Paytype_ID;}            set{                SetProperty(ref _Paytype_ID, value);                }
        }

        private DateTime? _ReturnDate;
        
        
        /// <summary>
        /// 退货日期
        /// </summary>

        [AdvQueryAttribute(ColName = "ReturnDate",ColDesc = "退货日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "ReturnDate" ,IsNullable = true,ColumnDescription = "退货日期" )]
        [Display(Name = "退货日期")]
        public DateTime? ReturnDate 
        { 
            get{return _ReturnDate;}            set{                SetProperty(ref _ReturnDate, value);                }
        }

        private string _SaleOut_NO;
        
        
        /// <summary>
        /// 销售出库单号
        /// </summary>

        [AdvQueryAttribute(ColName = "SaleOut_NO",ColDesc = "销售出库单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SaleOut_NO" ,Length=50,IsNullable = true,ColumnDescription = "销售出库单号" )]
        [Display(Name = "销售出库单号")]
        public string SaleOut_NO 
        { 
            get{return _SaleOut_NO;}            set{                SetProperty(ref _SaleOut_NO, value);                }
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

        private long? _Rack_ID;
        
        
        /// <summary>
        /// 货架
        /// </summary>

        [AdvQueryAttribute(ColName = "Rack_ID",ColDesc = "货架")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Rack_ID" ,IsNullable = true,ColumnDescription = "货架" )]
        [Display(Name = "货架")]
        public long? Rack_ID 
        { 
            get{return _Rack_ID;}            set{                SetProperty(ref _Rack_ID, value);                }
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

        private decimal? _TransactionPrice;


        /// <summary>
        /// 实际退款单价
        /// </summary>

        [AdvQueryAttribute(ColName = "TransactionPrice",ColDesc = "实际退款单价")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TransactionPrice" ,IsNullable = true,ColumnDescription = "实际退款单价")]
        [Display(Name = "实际退款单价")]
        public decimal? TransactionPrice 
        { 
            get{return _TransactionPrice;}            set{                SetProperty(ref _TransactionPrice, value);                }
        }

        private decimal? _SubtotalTransAmount;
        
        
        /// <summary>
        /// 成交小计
        /// </summary>

        [AdvQueryAttribute(ColName = "SubtotalTransAmount",ColDesc = "成交小计")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalTransAmount" ,IsNullable = true,ColumnDescription = "成交小计" )]
        [Display(Name = "成交小计")]
        public decimal? SubtotalTransAmount 
        { 
            get{return _SubtotalTransAmount;}            set{                SetProperty(ref _SubtotalTransAmount, value);                }
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

        private decimal? _SubtotalCostAmount;
        
        
        /// <summary>
        /// 成本小计
        /// </summary>

        [AdvQueryAttribute(ColName = "SubtotalCostAmount",ColDesc = "成本小计")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalCostAmount" ,IsNullable = true,ColumnDescription = "成本小计" )]
        [Display(Name = "成本小计")]
        public decimal? SubtotalCostAmount 
        { 
            get{return _SubtotalCostAmount;}            set{                SetProperty(ref _SubtotalCostAmount, value);                }
        }

        private decimal? _TaxRate;
        
        
        /// <summary>
        /// 税率
        /// </summary>

        [AdvQueryAttribute(ColName = "TaxRate",ColDesc = "税率")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "TaxRate" , DecimalDigits = 8,Length=8,IsNullable = true,ColumnDescription = "税率" )]
        [Display(Name = "税率")]
        public decimal? TaxRate 
        { 
            get{return _TaxRate;}            set{                SetProperty(ref _TaxRate, value);                }
        }

        private bool? _Gift;
        
        
        /// <summary>
        /// 赠品
        /// </summary>

        [AdvQueryAttribute(ColName = "Gift",ColDesc = "赠品")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Gift" ,IsNullable = true,ColumnDescription = "赠品" )]
        [Display(Name = "赠品")]
        public bool? Gift 
        { 
            get{return _Gift;}            set{                SetProperty(ref _Gift, value);                }
        }

       

        private decimal? _SubtotalUntaxedAmount;
        
        
        /// <summary>
        /// 未税本位币
        /// </summary>

        [AdvQueryAttribute(ColName = "SubtotalUntaxedAmount",ColDesc = "未税本位币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalUntaxedAmount" ,IsNullable = true,ColumnDescription = "未税本位币" )]
        [Display(Name = "未税本位币")]
        public decimal? SubtotalUntaxedAmount 
        { 
            get{return _SubtotalUntaxedAmount;}            set{                SetProperty(ref _SubtotalUntaxedAmount, value);                }
        }

        private decimal? _SubtotalTaxAmount;
        
        
        /// <summary>
        /// 税额
        /// </summary>

        [AdvQueryAttribute(ColName = "SubtotalTaxAmount",ColDesc = "税额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalTaxAmount" ,IsNullable = true,ColumnDescription = "税额" )]
        [Display(Name = "税额")]
        public decimal? SubtotalTaxAmount 
        { 
            get{return _SubtotalTaxAmount;}            set{                SetProperty(ref _SubtotalTaxAmount, value);                }
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

