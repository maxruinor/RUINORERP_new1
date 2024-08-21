
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:46:54
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
    /// 币别资料表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Currency")]
    public partial class tb_CurrencyQueryDto:BaseEntityDto
    {
        public tb_CurrencyQueryDto()
        {

        }

    
     

        private string _GroupName;
        /// <summary>
        /// 组合名称
        /// </summary>
        [AdvQueryAttribute(ColName = "GroupName",ColDesc = "组合名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "GroupName",Length=50,IsNullable = true,ColumnDescription = "组合名称" )]
        public string GroupName 
        { 
            get{return _GroupName;}
            set{SetProperty(ref _GroupName, value);}
        }
     

        private string _CurrencyCode;
        /// <summary>
        /// 外币代码
        /// </summary>
        [AdvQueryAttribute(ColName = "CurrencyCode",ColDesc = "外币代码")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "CurrencyCode",Length=10,IsNullable = true,ColumnDescription = "外币代码" )]
        public string CurrencyCode 
        { 
            get{return _CurrencyCode;}
            set{SetProperty(ref _CurrencyCode, value);}
        }
     

        private string _CurrencyName;
        /// <summary>
        /// 外币名称
        /// </summary>
        [AdvQueryAttribute(ColName = "CurrencyName",ColDesc = "外币名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "CurrencyName",Length=20,IsNullable = false,ColumnDescription = "外币名称" )]
        public string CurrencyName 
        { 
            get{return _CurrencyName;}
            set{SetProperty(ref _CurrencyName, value);}
        }
     

        private DateTime? _AdjustDate;
        /// <summary>
        /// 调整日期
        /// </summary>
        [AdvQueryAttribute(ColName = "AdjustDate",ColDesc = "调整日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "AdjustDate",IsNullable = true,ColumnDescription = "调整日期" )]
        public DateTime? AdjustDate 
        { 
            get{return _AdjustDate;}
            set{SetProperty(ref _AdjustDate, value);}
        }
     

        private decimal? _DefaultExchRate;
        /// <summary>
        /// 预设汇率
        /// </summary>
        [AdvQueryAttribute(ColName = "DefaultExchRate",ColDesc = "预设汇率")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "DefaultExchRate",IsNullable = true,ColumnDescription = "预设汇率" )]
        public decimal? DefaultExchRate 
        { 
            get{return _DefaultExchRate;}
            set{SetProperty(ref _DefaultExchRate, value);}
        }
     

        private decimal? _BuyExchRate;
        /// <summary>
        /// 买入汇率
        /// </summary>
        [AdvQueryAttribute(ColName = "BuyExchRate",ColDesc = "买入汇率")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "BuyExchRate",IsNullable = true,ColumnDescription = "买入汇率" )]
        public decimal? BuyExchRate 
        { 
            get{return _BuyExchRate;}
            set{SetProperty(ref _BuyExchRate, value);}
        }
     

        private decimal? _SellOutExchRate;
        /// <summary>
        /// 卖出汇率
        /// </summary>
        [AdvQueryAttribute(ColName = "SellOutExchRate",ColDesc = "卖出汇率")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "SellOutExchRate",IsNullable = true,ColumnDescription = "卖出汇率" )]
        public decimal? SellOutExchRate 
        { 
            get{return _SellOutExchRate;}
            set{SetProperty(ref _SellOutExchRate, value);}
        }
     

        private decimal? _MonthEndExchRate;
        /// <summary>
        /// 月末汇率
        /// </summary>
        [AdvQueryAttribute(ColName = "MonthEndExchRate",ColDesc = "月末汇率")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "MonthEndExchRate",IsNullable = true,ColumnDescription = "月末汇率" )]
        public decimal? MonthEndExchRate 
        { 
            get{return _MonthEndExchRate;}
            set{SetProperty(ref _MonthEndExchRate, value);}
        }
     

        private bool? _Is_enabled= true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_enabled",ColDesc = "是否启用")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Is_enabled",IsNullable = true,ColumnDescription = "是否启用" )]
        public bool? Is_enabled 
        { 
            get{return _Is_enabled;}
            set{SetProperty(ref _Is_enabled, value);}
        }
     

        private bool? _Is_available= true;
        /// <summary>
        /// 是否可用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_available",ColDesc = "是否可用")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Is_available",IsNullable = true,ColumnDescription = "是否可用" )]
        public bool? Is_available 
        { 
            get{return _Is_available;}
            set{SetProperty(ref _Is_available, value);}
        }
     

        private DateTime? _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Created_at",IsNullable = true,ColumnDescription = "创建时间" )]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}
            set{SetProperty(ref _Created_at, value);}
        }
     

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Created_by",IsNullable = true,ColumnDescription = "创建人" )]
        public long? Created_by 
        { 
            get{return _Created_by;}
            set{SetProperty(ref _Created_by, value);}
        }
     

        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Modified_at",IsNullable = true,ColumnDescription = "修改时间" )]
        public DateTime? Modified_at 
        { 
            get{return _Modified_at;}
            set{SetProperty(ref _Modified_at, value);}
        }
     

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Modified_by",IsNullable = true,ColumnDescription = "修改人" )]
        public long? Modified_by 
        { 
            get{return _Modified_by;}
            set{SetProperty(ref _Modified_by, value);}
        }
     

        private bool? _Is_BaseCurrency= false;
        /// <summary>
        /// 为本位币
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_BaseCurrency",ColDesc = "为本位币")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Is_BaseCurrency",IsNullable = true,ColumnDescription = "为本位币" )]
        public bool? Is_BaseCurrency 
        { 
            get{return _Is_BaseCurrency;}
            set{SetProperty(ref _Is_BaseCurrency, value);}
        }


       
    }
}



