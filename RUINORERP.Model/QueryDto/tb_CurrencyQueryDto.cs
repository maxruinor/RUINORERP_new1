
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:56:54
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
    /// 币别资料表-备份第一行数据后删除重建 如果不行则直接修改字段删除字段
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Currency")]
    public partial class tb_CurrencyQueryDto:BaseEntityDto
    {
        public tb_CurrencyQueryDto()
        {

        }

    
     

        private string _Country;
        /// <summary>
        /// 国家
        /// </summary>
        [AdvQueryAttribute(ColName = "Country",ColDesc = "国家")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Country",Length=50,IsNullable = true,ColumnDescription = "国家" )]
        public string Country 
        { 
            get{return _Country;}
            set{SetProperty(ref _Country, value);}
        }
     

        private string _CurrencyCode;
        /// <summary>
        /// 币别代码
        /// </summary>
        [AdvQueryAttribute(ColName = "CurrencyCode",ColDesc = "币别代码")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "CurrencyCode",Length=10,IsNullable = true,ColumnDescription = "币别代码" )]
        public string CurrencyCode 
        { 
            get{return _CurrencyCode;}
            set{SetProperty(ref _CurrencyCode, value);}
        }
     

        private string _CurrencyName;
        /// <summary>
        /// 币别名称
        /// </summary>
        [AdvQueryAttribute(ColName = "CurrencyName",ColDesc = "币别名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "CurrencyName",Length=20,IsNullable = false,ColumnDescription = "币别名称" )]
        public string CurrencyName 
        { 
            get{return _CurrencyName;}
            set{SetProperty(ref _CurrencyName, value);}
        }
     

        private string _CurrencySymbol;
        /// <summary>
        /// 币别符号
        /// </summary>
        [AdvQueryAttribute(ColName = "CurrencySymbol",ColDesc = "币别符号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "CurrencySymbol",Length=10,IsNullable = true,ColumnDescription = "币别符号" )]
        public string CurrencySymbol 
        { 
            get{return _CurrencySymbol;}
            set{SetProperty(ref _CurrencySymbol, value);}
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



