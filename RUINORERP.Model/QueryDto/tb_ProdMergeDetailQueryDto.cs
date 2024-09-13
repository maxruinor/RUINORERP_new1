
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
    /// 组合单明细
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ProdMergeDetail")]
    public partial class tb_ProdMergeDetailQueryDto:BaseEntityDto
    {
        public tb_ProdMergeDetailQueryDto()
        {

        }

    
     

        private long _MergeID;
        /// <summary>
        /// 组合单
        /// </summary>
        [AdvQueryAttribute(ColName = "MergeID",ColDesc = "组合单")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "MergeID",IsNullable = false,ColumnDescription = "组合单" )]
        [FKRelationAttribute("tb_ProdMerge","MergeID")]
        public long MergeID 
        { 
            get{return _MergeID;}
            set{SetProperty(ref _MergeID, value);}
        }
     

        private long _Location_ID;
        /// <summary>
        /// 库位
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "库位")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Location_ID",IsNullable = false,ColumnDescription = "库位" )]
        [FKRelationAttribute("tb_Location","Location_ID")]
        public long Location_ID 
        { 
            get{return _Location_ID;}
            set{SetProperty(ref _Location_ID, value);}
        }
     

        private long _ProdDetailID;
        /// <summary>
        /// 子件
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "子件")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProdDetailID",IsNullable = false,ColumnDescription = "子件" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID 
        { 
            get{return _ProdDetailID;}
            set{SetProperty(ref _ProdDetailID, value);}
        }
     

        private string _property;
        /// <summary>
        /// 属性
        /// </summary>
        [AdvQueryAttribute(ColName = "property",ColDesc = "属性")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "property",Length=255,IsNullable = true,ColumnDescription = "属性" )]
        public string property 
        { 
            get{return _property;}
            set{SetProperty(ref _property, value);}
        }
     

        private int _Qty= ((0));
        /// <summary>
        /// 子件数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Qty",ColDesc = "子件数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Qty",IsNullable = false,ColumnDescription = "子件数量" )]
        public int Qty 
        { 
            get{return _Qty;}
            set{SetProperty(ref _Qty, value);}
        }
     

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Summary",Length=1000,IsNullable = true,ColumnDescription = "摘要" )]
        public string Summary 
        { 
            get{return _Summary;}
            set{SetProperty(ref _Summary, value);}
        }


       
    }
}



