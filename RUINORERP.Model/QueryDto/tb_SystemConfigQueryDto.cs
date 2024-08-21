
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:49:18
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
        /// 调试信息
        /// </summary>
        [AdvQueryAttribute(ColName = "ShowDebugInfo",ColDesc = "调试信息")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "ShowDebugInfo",IsNullable = false,ColumnDescription = "调试信息" )]
        public bool ShowDebugInfo 
        { 
            get{return _ShowDebugInfo;}
            set{SetProperty(ref _ShowDebugInfo, value);}
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
     

        private bool? _CurrencyDataPrecisionAutoAddZero= true;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "CurrencyDataPrecisionAutoAddZero",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "CurrencyDataPrecisionAutoAddZero",IsNullable = true,ColumnDescription = "" )]
        public bool? CurrencyDataPrecisionAutoAddZero 
        { 
            get{return _CurrencyDataPrecisionAutoAddZero;}
            set{SetProperty(ref _CurrencyDataPrecisionAutoAddZero, value);}
        }
     

        private bool _UseBarCode= false;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "UseBarCode",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "UseBarCode",IsNullable = false,ColumnDescription = "" )]
        public bool UseBarCode 
        { 
            get{return _UseBarCode;}
            set{SetProperty(ref _UseBarCode, value);}
        }


       
    }
}



