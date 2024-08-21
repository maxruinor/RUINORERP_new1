
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/10/2024 19:17:32
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
    /// 标准物料表次级产出明细
    /// </summary>
    [Serializable()]
    [SugarTable("tb_BOM_SDetailSecondary")]
    public partial class tb_BOM_SDetailSecondaryQueryDto:BaseEntityDto
    {
        public tb_BOM_SDetailSecondaryQueryDto()
        {

        }

    
     

        private long? _ProdDetailID;
        /// <summary>
        /// 产品详情
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品详情")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProdDetailID",IsNullable = true,ColumnDescription = "产品详情" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long? ProdDetailID 
        { 
            get{return _ProdDetailID;}
            set{SetProperty(ref _ProdDetailID, value);}
        }
     

        private long? _BOM_ID;
        /// <summary>
        /// BOM
        /// </summary>
        [AdvQueryAttribute(ColName = "BOM_ID",ColDesc = "BOM")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "BOM_ID",IsNullable = true,ColumnDescription = "BOM" )]
        [FKRelationAttribute("tb_BOM_S","BOM_ID")]
        public long? BOM_ID 
        { 
            get{return _BOM_ID;}
            set{SetProperty(ref _BOM_ID, value);}
        }
     

        private long _Location_ID;
        /// <summary>
        /// 仓库
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "仓库")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Location_ID",IsNullable = false,ColumnDescription = "仓库" )]
        [FKRelationAttribute("tb_Location","Location_ID")]
        public long Location_ID 
        { 
            get{return _Location_ID;}
            set{SetProperty(ref _Location_ID, value);}
        }
     

        private string _SecondItemName;
        /// <summary>
        /// 副产名称
        /// </summary>
        [AdvQueryAttribute(ColName = "SecondItemName",ColDesc = "副产名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "SecondItemName",Length=200,IsNullable = true,ColumnDescription = "副产名称" )]
        public string SecondItemName 
        { 
            get{return _SecondItemName;}
            set{SetProperty(ref _SecondItemName, value);}
        }
     

        private string _SecondItemSpec;
        /// <summary>
        /// 副产规格
        /// </summary>
        [AdvQueryAttribute(ColName = "SecondItemSpec",ColDesc = "副产规格")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "SecondItemSpec",Length=200,IsNullable = true,ColumnDescription = "副产规格" )]
        public string SecondItemSpec 
        { 
            get{return _SecondItemSpec;}
            set{SetProperty(ref _SecondItemSpec, value);}
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
     

        private decimal _Qty= ((0));
        /// <summary>
        /// 数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Qty",ColDesc = "数量")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Qty",IsNullable = false,ColumnDescription = "数量" )]
        public decimal Qty 
        { 
            get{return _Qty;}
            set{SetProperty(ref _Qty, value);}
        }
     

        private decimal _Scale= ((0));
        /// <summary>
        /// 比例
        /// </summary>
        [AdvQueryAttribute(ColName = "Scale",ColDesc = "比例")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Scale",IsNullable = false,ColumnDescription = "比例" )]
        public decimal Scale 
        { 
            get{return _Scale;}
            set{SetProperty(ref _Scale, value);}
        }
     

        private decimal _UnitCost= ((0));
        /// <summary>
        /// 单位成本
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitCost",ColDesc = "单位成本")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "UnitCost",IsNullable = false,ColumnDescription = "单位成本" )]
        public decimal UnitCost 
        { 
            get{return _UnitCost;}
            set{SetProperty(ref _UnitCost, value);}
        }
     

        private decimal _SubtotalCost= ((0));
        /// <summary>
        /// 成本小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalCost",ColDesc = "成本小计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "SubtotalCost",IsNullable = false,ColumnDescription = "成本小计" )]
        public decimal SubtotalCost 
        { 
            get{return _SubtotalCost;}
            set{SetProperty(ref _SubtotalCost, value);}
        }
     

        private string _Remarks;
        /// <summary>
        /// 备注说明
        /// </summary>
        [AdvQueryAttribute(ColName = "Remarks",ColDesc = "备注说明")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Remarks",Length=200,IsNullable = true,ColumnDescription = "备注说明" )]
        public string Remarks 
        { 
            get{return _Remarks;}
            set{SetProperty(ref _Remarks, value);}
        }


       
    }
}



