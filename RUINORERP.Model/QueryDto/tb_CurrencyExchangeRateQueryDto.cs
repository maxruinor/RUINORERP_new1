
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/16/2025 12:02:51
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
    /// 币别换算表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_CurrencyExchangeRate")]
    public partial class tb_CurrencyExchangeRateQueryDto:BaseEntityDto
    {
        public tb_CurrencyExchangeRateQueryDto()
        {

        }

    
     

        private string _ConversionName;
        /// <summary>
        /// 换算名称
        /// </summary>
        [AdvQueryAttribute(ColName = "ConversionName",ColDesc = "换算名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ConversionName",Length=50,IsNullable = false,ColumnDescription = "换算名称" )]
        public string ConversionName 
        { 
            get{return _ConversionName;}
            set{SetProperty(ref _ConversionName, value);}
        }
     

        private long _BaseCurrencyID;
        /// <summary>
        /// 基本币别
        /// </summary>
        [AdvQueryAttribute(ColName = "BaseCurrencyID",ColDesc = "基本币别")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "BaseCurrencyID",IsNullable = false,ColumnDescription = "基本币别" )]
        [FKRelationAttribute("tb_Currency","BaseCurrencyID")]
        public long BaseCurrencyID 
        { 
            get{return _BaseCurrencyID;}
            set{SetProperty(ref _BaseCurrencyID, value);}
        }
     

        private long _TargetCurrencyID;
        /// <summary>
        /// 目标币别
        /// </summary>
        [AdvQueryAttribute(ColName = "TargetCurrencyID",ColDesc = "目标币别")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "TargetCurrencyID",IsNullable = false,ColumnDescription = "目标币别" )]
        [FKRelationAttribute("tb_Currency","TargetCurrencyID")]
        public long TargetCurrencyID 
        { 
            get{return _TargetCurrencyID;}
            set{SetProperty(ref _TargetCurrencyID, value);}
        }
     

        private DateTime _EffectiveDate;
        /// <summary>
        /// 生效日期
        /// </summary>
        [AdvQueryAttribute(ColName = "EffectiveDate",ColDesc = "生效日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "EffectiveDate",IsNullable = false,ColumnDescription = "生效日期" )]
        public DateTime EffectiveDate 
        { 
            get{return _EffectiveDate;}
            set{SetProperty(ref _EffectiveDate, value);}
        }
     

        private DateTime? _ExpirationDate;
        /// <summary>
        /// 有效日期
        /// </summary>
        [AdvQueryAttribute(ColName = "ExpirationDate",ColDesc = "有效日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "ExpirationDate",IsNullable = true,ColumnDescription = "有效日期" )]
        public DateTime? ExpirationDate 
        { 
            get{return _ExpirationDate;}
            set{SetProperty(ref _ExpirationDate, value);}
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
     

        private decimal? _ExecuteExchRate;
        /// <summary>
        /// 执行汇率
        /// </summary>
        [AdvQueryAttribute(ColName = "ExecuteExchRate",ColDesc = "执行汇率")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "ExecuteExchRate",IsNullable = true,ColumnDescription = "执行汇率" )]
        public decimal? ExecuteExchRate 
        { 
            get{return _ExecuteExchRate;}
            set{SetProperty(ref _ExecuteExchRate, value);}
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


       
    }
}



