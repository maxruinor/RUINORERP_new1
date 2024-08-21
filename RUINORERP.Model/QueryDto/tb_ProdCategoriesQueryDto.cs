
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:48:14
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
    /// 产品类别表 与行业相关的产品分类
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ProdCategories")]
    public partial class tb_ProdCategoriesQueryDto:BaseEntityDto
    {
        public tb_ProdCategoriesQueryDto()
        {

        }

    
     

        private string _Category_name;
        /// <summary>
        /// 类别名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Category_name",ColDesc = "类别名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Category_name",Length=50,IsNullable = true,ColumnDescription = "类别名称" )]
        public string Category_name 
        { 
            get{return _Category_name;}
            set{SetProperty(ref _Category_name, value);}
        }
     

        private string _CategoryCode;
        /// <summary>
        /// 类别代码
        /// </summary>
        [AdvQueryAttribute(ColName = "CategoryCode",ColDesc = "类别代码")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "CategoryCode",Length=20,IsNullable = true,ColumnDescription = "类别代码" )]
        public string CategoryCode 
        { 
            get{return _CategoryCode;}
            set{SetProperty(ref _CategoryCode, value);}
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
     

        private int? _Sort;
        /// <summary>
        /// 排序
        /// </summary>
        [AdvQueryAttribute(ColName = "Sort",ColDesc = "排序")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Sort",IsNullable = true,ColumnDescription = "排序" )]
        public int? Sort 
        { 
            get{return _Sort;}
            set{SetProperty(ref _Sort, value);}
        }
     

        private long? _Parent_id;
        /// <summary>
        /// 父类
        /// </summary>
        [AdvQueryAttribute(ColName = "Parent_id",ColDesc = "父类")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Parent_id",IsNullable = true,ColumnDescription = "父类" )]
        public long? Parent_id 
        { 
            get{return _Parent_id;}
            set{SetProperty(ref _Parent_id, value);}
        }
     

        private byte[] _Images;
        /// <summary>
        /// 类目图片
        /// </summary>
        [AdvQueryAttribute(ColName = "Images",ColDesc = "类目图片")]
        [SugarColumn(ColumnDataType = "image",SqlParameterDbType ="Binary",ColumnName = "Images",Length=2147483647,IsNullable = true,ColumnDescription = "类目图片" )]
        public byte[] Images 
        { 
            get{return _Images;}
            set{SetProperty(ref _Images, value);}
        }
     

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=200,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
        }


       
    }
}



