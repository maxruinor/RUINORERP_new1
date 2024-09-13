
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:05
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
    /// 产品主次及属性关系表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Prod_Attr_Relation")]
    public partial class tb_Prod_Attr_RelationQueryDto:BaseEntityDto
    {
        public tb_Prod_Attr_RelationQueryDto()
        {

        }

    
     

        private long? _PropertyValueID;
        /// <summary>
        /// 属性值
        /// </summary>
        [AdvQueryAttribute(ColName = "PropertyValueID",ColDesc = "属性值")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "PropertyValueID",IsNullable = true,ColumnDescription = "属性值" )]
        [FKRelationAttribute("tb_ProdPropertyValue","PropertyValueID")]
        public long? PropertyValueID 
        { 
            get{return _PropertyValueID;}
            set{SetProperty(ref _PropertyValueID, value);}
        }
     

        private long? _Property_ID;
        /// <summary>
        /// 属性
        /// </summary>
        [AdvQueryAttribute(ColName = "Property_ID",ColDesc = "属性")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Property_ID",IsNullable = true,ColumnDescription = "属性" )]
        [FKRelationAttribute("tb_ProdProperty","Property_ID")]
        public long? Property_ID 
        { 
            get{return _Property_ID;}
            set{SetProperty(ref _Property_ID, value);}
        }
     

        private long? _ProdDetailID;
        /// <summary>
        /// 货品详情
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "货品详情")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProdDetailID",IsNullable = true,ColumnDescription = "货品详情" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long? ProdDetailID 
        { 
            get{return _ProdDetailID;}
            set{SetProperty(ref _ProdDetailID, value);}
        }
     

        private long? _ProdBaseID;
        /// <summary>
        /// 货品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdBaseID",ColDesc = "货品")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProdBaseID",IsNullable = true,ColumnDescription = "货品" )]
        [FKRelationAttribute("tb_Prod","ProdBaseID")]
        public long? ProdBaseID 
        { 
            get{return _ProdBaseID;}
            set{SetProperty(ref _ProdBaseID, value);}
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


       
    }
}



