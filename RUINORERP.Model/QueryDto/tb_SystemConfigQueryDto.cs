
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/30/2025 13:26:31
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

namespace RUINORERP.Model.QueryDto
{
    /// <summary>
    /// 系统配置表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_SystemConfig")]
    public partial class tb_SystemConfigQueryDto:BaseEntityDto
    {
        public tb_SystemConfigQueryDto()
        {

        }

    
     

        private int _QtyDataPrecision= ((0));
        /// <summary>
        /// 数量精度
        /// </summary>
        [AdvQueryAttribute(ColName = "QtyDataPrecision",ColDesc = "数量精度")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "QtyDataPrecision",IsNullable = false,ColumnDescription = "数量精度" )]
        public int QtyDataPrecision 
        { 
            get{return _QtyDataPrecision;}
            set{SetProperty(ref _QtyDataPrecision, value);}
        }
     

        private int _TaxRateDataPrecision= ((2));
        /// <summary>
        /// 税率精度
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxRateDataPrecision",ColDesc = "税率精度")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "TaxRateDataPrecision",IsNullable = false,ColumnDescription = "税率精度" )]
        public int TaxRateDataPrecision 
        { 
            get{return _TaxRateDataPrecision;}
            set{SetProperty(ref _TaxRateDataPrecision, value);}
        }
     

        private int _MoneyDataPrecision= ((2));
        /// <summary>
        /// 金额精度
        /// </summary>
        [AdvQueryAttribute(ColName = "MoneyDataPrecision",ColDesc = "金额精度")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "MoneyDataPrecision",IsNullable = false,ColumnDescription = "金额精度" )]
        public int MoneyDataPrecision 
        { 
            get{return _MoneyDataPrecision;}
            set{SetProperty(ref _MoneyDataPrecision, value);}
        }
     

        private bool _CheckNegativeInventory= true;
        /// <summary>
        /// 允许负库存
        /// </summary>
        [AdvQueryAttribute(ColName = "CheckNegativeInventory",ColDesc = "允许负库存")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "CheckNegativeInventory",IsNullable = false,ColumnDescription = "允许负库存" )]
        public bool CheckNegativeInventory 
        { 
            get{return _CheckNegativeInventory;}
            set{SetProperty(ref _CheckNegativeInventory, value);}
        }
     

        private int _CostCalculationMethod;
        /// <summary>
        /// 成本方式
        /// </summary>
        [AdvQueryAttribute(ColName = "CostCalculationMethod",ColDesc = "成本方式")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "CostCalculationMethod",IsNullable = false,ColumnDescription = "成本方式" )]
        public int CostCalculationMethod 
        { 
            get{return _CostCalculationMethod;}
            set{SetProperty(ref _CostCalculationMethod, value);}
        }
     

        private bool _ShowDebugInfo= false;
        /// <summary>
        /// 显示调试信息
        /// </summary>
        [AdvQueryAttribute(ColName = "ShowDebugInfo",ColDesc = "显示调试信息")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "ShowDebugInfo",IsNullable = false,ColumnDescription = "显示调试信息" )]
        public bool ShowDebugInfo 
        { 
            get{return _ShowDebugInfo;}
            set{SetProperty(ref _ShowDebugInfo, value);}
        }
     

        private bool _OwnershipControl= true;
        /// <summary>
        /// 数据归属控制
        /// </summary>
        [AdvQueryAttribute(ColName = "OwnershipControl",ColDesc = "数据归属控制")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "OwnershipControl",IsNullable = false,ColumnDescription = "数据归属控制" )]
        public bool OwnershipControl 
        { 
            get{return _OwnershipControl;}
            set{SetProperty(ref _OwnershipControl, value);}
        }
     

        private bool _SaleBizLimited= false;
        /// <summary>
        /// 销售业务范围限制
        /// </summary>
        [AdvQueryAttribute(ColName = "SaleBizLimited",ColDesc = "销售业务范围限制")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "SaleBizLimited",IsNullable = false,ColumnDescription = "销售业务范围限制" )]
        public bool SaleBizLimited 
        { 
            get{return _SaleBizLimited;}
            set{SetProperty(ref _SaleBizLimited, value);}
        }
     

        private bool _DepartBizLimited= false;
        /// <summary>
        /// 部门范围限制
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartBizLimited",ColDesc = "部门范围限制")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "DepartBizLimited",IsNullable = false,ColumnDescription = "部门范围限制" )]
        public bool DepartBizLimited 
        { 
            get{return _DepartBizLimited;}
            set{SetProperty(ref _DepartBizLimited, value);}
        }
     

        private bool _PurchsaeBizLimited= false;
        /// <summary>
        /// 采购业务范围限制
        /// </summary>
        [AdvQueryAttribute(ColName = "PurchsaeBizLimited",ColDesc = "采购业务范围限制")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "PurchsaeBizLimited",IsNullable = false,ColumnDescription = "采购业务范围限制" )]
        public bool PurchsaeBizLimited 
        { 
            get{return _PurchsaeBizLimited;}
            set{SetProperty(ref _PurchsaeBizLimited, value);}
        }
     

        private bool _CurrencyDataPrecisionAutoAddZero= true;
        /// <summary>
        /// 金额精度自动补零
        /// </summary>
        [AdvQueryAttribute(ColName = "CurrencyDataPrecisionAutoAddZero",ColDesc = "金额精度自动补零")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "CurrencyDataPrecisionAutoAddZero",IsNullable = false,ColumnDescription = "金额精度自动补零" )]
        public bool CurrencyDataPrecisionAutoAddZero 
        { 
            get{return _CurrencyDataPrecisionAutoAddZero;}
            set{SetProperty(ref _CurrencyDataPrecisionAutoAddZero, value);}
        }
     

        private bool _UseBarCode= false;
        /// <summary>
        /// 是否启用条码
        /// </summary>
        [AdvQueryAttribute(ColName = "UseBarCode",ColDesc = "是否启用条码")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "UseBarCode",IsNullable = false,ColumnDescription = "是否启用条码" )]
        public bool UseBarCode 
        { 
            get{return _UseBarCode;}
            set{SetProperty(ref _UseBarCode, value);}
        }
     

        private bool _QueryPageLayoutCustomize;
        /// <summary>
        /// 查询页布局自定义
        /// </summary>
        [AdvQueryAttribute(ColName = "QueryPageLayoutCustomize",ColDesc = "查询页布局自定义")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "QueryPageLayoutCustomize",IsNullable = false,ColumnDescription = "查询页布局自定义" )]
        public bool QueryPageLayoutCustomize 
        { 
            get{return _QueryPageLayoutCustomize;}
            set{SetProperty(ref _QueryPageLayoutCustomize, value);}
        }
     

        private decimal _AutoApprovedSaleOrderAmount;
        /// <summary>
        /// 自动审核销售订单金额
        /// </summary>
        [AdvQueryAttribute(ColName = "AutoApprovedSaleOrderAmount",ColDesc = "自动审核销售订单金额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "AutoApprovedSaleOrderAmount",IsNullable = false,ColumnDescription = "自动审核销售订单金额" )]
        public decimal AutoApprovedSaleOrderAmount 
        { 
            get{return _AutoApprovedSaleOrderAmount;}
            set{SetProperty(ref _AutoApprovedSaleOrderAmount, value);}
        }
     

        private decimal _AutoApprovedPurOrderAmount;
        /// <summary>
        /// 自动审核采购订单金额
        /// </summary>
        [AdvQueryAttribute(ColName = "AutoApprovedPurOrderAmount",ColDesc = "自动审核采购订单金额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "AutoApprovedPurOrderAmount",IsNullable = false,ColumnDescription = "自动审核采购订单金额" )]
        public decimal AutoApprovedPurOrderAmount 
        { 
            get{return _AutoApprovedPurOrderAmount;}
            set{SetProperty(ref _AutoApprovedPurOrderAmount, value);}
        }
     

        private bool _QueryGridColCustomize;
        /// <summary>
        /// 查询表格列自定义
        /// </summary>
        [AdvQueryAttribute(ColName = "QueryGridColCustomize",ColDesc = "查询表格列自定义")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "QueryGridColCustomize",IsNullable = false,ColumnDescription = "查询表格列自定义" )]
        public bool QueryGridColCustomize 
        { 
            get{return _QueryGridColCustomize;}
            set{SetProperty(ref _QueryGridColCustomize, value);}
        }
     

        private bool _BillGridColCustomize;
        /// <summary>
        /// 单据表格列自定义
        /// </summary>
        [AdvQueryAttribute(ColName = "BillGridColCustomize",ColDesc = "单据表格列自定义")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "BillGridColCustomize",IsNullable = false,ColumnDescription = "单据表格列自定义" )]
        public bool BillGridColCustomize 
        { 
            get{return _BillGridColCustomize;}
            set{SetProperty(ref _BillGridColCustomize, value);}
        }
     

        private bool _IsDebug;
        /// <summary>
        /// 调试模式
        /// </summary>
        [AdvQueryAttribute(ColName = "IsDebug",ColDesc = "调试模式")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsDebug",IsNullable = false,ColumnDescription = "调试模式" )]
        public bool IsDebug 
        { 
            get{return _IsDebug;}
            set{SetProperty(ref _IsDebug, value);}
        }
     

        private bool _EnableVoucherModule= true;
        /// <summary>
        /// 启用凭证模块
        /// </summary>
        [AdvQueryAttribute(ColName = "EnableVoucherModule",ColDesc = "启用凭证模块")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "EnableVoucherModule",IsNullable = false,ColumnDescription = "启用凭证模块" )]
        public bool EnableVoucherModule 
        { 
            get{return _EnableVoucherModule;}
            set{SetProperty(ref _EnableVoucherModule, value);}
        }
     

        private bool _EnableContractModule= true;
        /// <summary>
        /// 启用合同模块
        /// </summary>
        [AdvQueryAttribute(ColName = "EnableContractModule",ColDesc = "启用合同模块")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "EnableContractModule",IsNullable = false,ColumnDescription = "启用合同模块" )]
        public bool EnableContractModule 
        { 
            get{return _EnableContractModule;}
            set{SetProperty(ref _EnableContractModule, value);}
        }
     

        private bool _EnableInvoiceModule;
        /// <summary>
        /// 启用发票模块
        /// </summary>
        [AdvQueryAttribute(ColName = "EnableInvoiceModule",ColDesc = "启用发票模块")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "EnableInvoiceModule",IsNullable = false,ColumnDescription = "启用发票模块" )]
        public bool EnableInvoiceModule 
        { 
            get{return _EnableInvoiceModule;}
            set{SetProperty(ref _EnableInvoiceModule, value);}
        }
     

        private bool _EnableMultiCurrency= true;
        /// <summary>
        /// 启用多币种
        /// </summary>
        [AdvQueryAttribute(ColName = "EnableMultiCurrency",ColDesc = "启用多币种")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "EnableMultiCurrency",IsNullable = false,ColumnDescription = "启用多币种" )]
        public bool EnableMultiCurrency 
        { 
            get{return _EnableMultiCurrency;}
            set{SetProperty(ref _EnableMultiCurrency, value);}
        }
     

        private bool _EnableFinancialModule= true;
        /// <summary>
        /// 启用财务模块
        /// </summary>
        [AdvQueryAttribute(ColName = "EnableFinancialModule",ColDesc = "启用财务模块")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "EnableFinancialModule",IsNullable = false,ColumnDescription = "启用财务模块" )]
        public bool EnableFinancialModule 
        { 
            get{return _EnableFinancialModule;}
            set{SetProperty(ref _EnableFinancialModule, value);}
        }


       
    }
}



