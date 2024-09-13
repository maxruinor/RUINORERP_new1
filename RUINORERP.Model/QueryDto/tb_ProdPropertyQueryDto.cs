
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:10
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
    /// 产品属性表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ProdProperty")]
    public partial class tb_ProdPropertyQueryDto:BaseEntityDto
    {
        public tb_ProdPropertyQueryDto()
        {

        }

    
     

        private string _PropertyName;
        /// <summary>
        /// 属性名称
        /// </summary>
        [AdvQueryAttribute(ColName = "PropertyName",ColDesc = "属性名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PropertyName",Length=20,IsNullable = false,ColumnDescription = "属性名称" )]
        public string PropertyName 
        { 
            get{return _PropertyName;}
            set{SetProperty(ref _PropertyName, value);}
        }
     

        private string _PropertyDesc;
        /// <summary>
        /// 属性描述
        /// </summary>
        [AdvQueryAttribute(ColName = "PropertyDesc",ColDesc = "属性描述")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PropertyDesc",Length=50,IsNullable = true,ColumnDescription = "属性描述" )]
        public string PropertyDesc 
        { 
            get{return _PropertyDesc;}
            set{SetProperty(ref _PropertyDesc, value);}
        }
     

        private int? _SortOrder;
        /// <summary>
        /// 排序
        /// </summary>
        [AdvQueryAttribute(ColName = "SortOrder",ColDesc = "排序")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "SortOrder",IsNullable = true,ColumnDescription = "排序" )]
        public int? SortOrder 
        { 
            get{return _SortOrder;}
            set{SetProperty(ref _SortOrder, value);}
        }
     

        private string _InputType;
        /// <summary>
        /// 输入类型
        /// </summary>
        [AdvQueryAttribute(ColName = "InputType",ColDesc = "输入类型")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "InputType",Length=50,IsNullable = true,ColumnDescription = "输入类型" )]
        public string InputType 
        { 
            get{return _InputType;}
            set{SetProperty(ref _InputType, value);}
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
     

        private bool _isdeleted= false;
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "isdeleted",IsNullable = false,ColumnDescription = "逻辑删除" )]
        public bool isdeleted 
        { 
            get{return _isdeleted;}
            set{SetProperty(ref _isdeleted, value);}
        }
     

        private int? _DataStatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "DataStatus",ColDesc = "数据状态")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "DataStatus",IsNullable = true,ColumnDescription = "数据状态" )]
        public int? DataStatus 
        { 
            get{return _DataStatus;}
            set{SetProperty(ref _DataStatus, value);}
        }


       
    }
}



