
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:26
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
    /// 角色属性配置不同角色权限功能等不一样
    /// </summary>
    [Serializable()]
    [SugarTable("tb_RolePropertyConfig")]
    public partial class tb_RolePropertyConfigQueryDto:BaseEntityDto
    {
        public tb_RolePropertyConfigQueryDto()
        {

        }

    
     

        private string _RolePropertyName;
        /// <summary>
        /// 角色名称
        /// </summary>
        [AdvQueryAttribute(ColName = "RolePropertyName",ColDesc = "角色名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "RolePropertyName",Length=255,IsNullable = true,ColumnDescription = "角色名称" )]
        public string RolePropertyName 
        { 
            get{return _RolePropertyName;}
            set{SetProperty(ref _RolePropertyName, value);}
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
     

        private bool _OwnershipControl= false;
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
     

        private bool _ExclusiveLimited;
        /// <summary>
        /// 启用责任人独占
        /// </summary>
        [AdvQueryAttribute(ColName = "ExclusiveLimited",ColDesc = "启用责任人独占")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "ExclusiveLimited",IsNullable = false,ColumnDescription = "启用责任人独占" )]
        public bool ExclusiveLimited 
        { 
            get{return _ExclusiveLimited;}
            set{SetProperty(ref _ExclusiveLimited, value);}
        }
     

        private string _DataBoardUnits;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "DataBoardUnits",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "DataBoardUnits",Length=500,IsNullable = true,ColumnDescription = "" )]
        public string DataBoardUnits 
        { 
            get{return _DataBoardUnits;}
            set{SetProperty(ref _DataBoardUnits, value);}
        }


       
    }
}



