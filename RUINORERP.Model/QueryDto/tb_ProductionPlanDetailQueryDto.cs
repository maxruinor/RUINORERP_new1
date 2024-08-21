
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2024 17:20:32
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
    /// 生产计划明细
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ProductionPlanDetail")]
    public partial class tb_ProductionPlanDetailQueryDto:BaseEntityDto
    {
        public tb_ProductionPlanDetailQueryDto()
        {

        }

    
     

        private long _PPID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "PPID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "PPID",IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_ProductionPlan","PPID")]
        public long PPID 
        { 
            get{return _PPID;}
            set{SetProperty(ref _PPID, value);}
        }
     

        private long _ProdDetailID;
        /// <summary>
        /// 产品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProdDetailID",IsNullable = false,ColumnDescription = "产品" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID 
        { 
            get{return _ProdDetailID;}
            set{SetProperty(ref _ProdDetailID, value);}
        }
     

        private string _Specifications;
        /// <summary>
        /// 规格
        /// </summary>
        [AdvQueryAttribute(ColName = "Specifications",ColDesc = "规格")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Specifications",Length=1000,IsNullable = true,ColumnDescription = "规格" )]
        public string Specifications 
        { 
            get{return _Specifications;}
            set{SetProperty(ref _Specifications, value);}
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
     

        private int _Quantity= ((0));
        /// <summary>
        /// 计划数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "计划数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Quantity",IsNullable = false,ColumnDescription = "计划数量" )]
        public int Quantity 
        { 
            get{return _Quantity;}
            set{SetProperty(ref _Quantity, value);}
        }
     

        private long _BOM_ID;
        /// <summary>
        /// 配方名称
        /// </summary>
        [AdvQueryAttribute(ColName = "BOM_ID",ColDesc = "配方名称")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "BOM_ID",IsNullable = false,ColumnDescription = "配方名称" )]
        [FKRelationAttribute("tb_BOM_S","BOM_ID")]
        public long BOM_ID 
        { 
            get{return _BOM_ID;}
            set{SetProperty(ref _BOM_ID, value);}
        }
     

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Summary",Length=255,IsNullable = true,ColumnDescription = "摘要" )]
        public string Summary 
        { 
            get{return _Summary;}
            set{SetProperty(ref _Summary, value);}
        }
     

        private int _CompletedQuantity= ((0));
        /// <summary>
        /// 完成数量
        /// </summary>
        [AdvQueryAttribute(ColName = "CompletedQuantity",ColDesc = "完成数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "CompletedQuantity",IsNullable = false,ColumnDescription = "完成数量" )]
        public int CompletedQuantity 
        { 
            get{return _CompletedQuantity;}
            set{SetProperty(ref _CompletedQuantity, value);}
        }
     

        private int _AnalyzedQuantity= ((0));
        /// <summary>
        /// 已分析数量
        /// </summary>
        [AdvQueryAttribute(ColName = "AnalyzedQuantity",ColDesc = "已分析数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "AnalyzedQuantity",IsNullable = false,ColumnDescription = "已分析数量" )]
        public int AnalyzedQuantity 
        { 
            get{return _AnalyzedQuantity;}
            set{SetProperty(ref _AnalyzedQuantity, value);}
        }
     

        private bool? _IsAnalyzed= false;
        /// <summary>
        /// 已分析
        /// </summary>
        [AdvQueryAttribute(ColName = "IsAnalyzed",ColDesc = "已分析")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsAnalyzed",IsNullable = true,ColumnDescription = "已分析" )]
        public bool? IsAnalyzed 
        { 
            get{return _IsAnalyzed;}
            set{SetProperty(ref _IsAnalyzed, value);}
        }


       
    }
}



