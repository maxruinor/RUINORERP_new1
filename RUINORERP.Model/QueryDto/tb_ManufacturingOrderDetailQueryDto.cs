
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/28/2024 11:55:42
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
    /// 制令单的原料明细表 明细对应的是一个树，结构同BOM，先把BOM搞好再来实现这里的细节
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ManufacturingOrderDetail")]
    public partial class tb_ManufacturingOrderDetailQueryDto:BaseEntityDto
    {
        public tb_ManufacturingOrderDetailQueryDto()
        {

        }

    
     

        private long _MOID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "MOID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "MOID",IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_ManufacturingOrder","MOID")]
        public long MOID 
        { 
            get{return _MOID;}
            set{SetProperty(ref _MOID, value);}
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
     

        private long _ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ID",IsNullable = false,ColumnDescription = "" )]
        public long ID 
        { 
            get{return _ID;}
            set{SetProperty(ref _ID, value);}
        }
     

        private long _ParentId;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ParentId",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ParentId",IsNullable = false,ColumnDescription = "" )]
        public long ParentId 
        { 
            get{return _ParentId;}
            set{SetProperty(ref _ParentId, value);}
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
     

        private string _BOM_NO;
        /// <summary>
        /// 配方编号
        /// </summary>
        [AdvQueryAttribute(ColName = "BOM_NO",ColDesc = "配方编号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "BOM_NO",Length=50,IsNullable = true,ColumnDescription = "配方编号" )]
        public string BOM_NO 
        { 
            get{return _BOM_NO;}
            set{SetProperty(ref _BOM_NO, value);}
        }
     

        private decimal _ShouldSendQty= ((0));
        /// <summary>
        /// 应发数
        /// </summary>
        [AdvQueryAttribute(ColName = "ShouldSendQty",ColDesc = "应发数")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "ShouldSendQty",IsNullable = false,ColumnDescription = "应发数" )]
        public decimal ShouldSendQty 
        { 
            get{return _ShouldSendQty;}
            set{SetProperty(ref _ShouldSendQty, value);}
        }
     

        private decimal _ActualSentQty= ((0));
        /// <summary>
        /// 实发数
        /// </summary>
        [AdvQueryAttribute(ColName = "ActualSentQty",ColDesc = "实发数")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "ActualSentQty",IsNullable = false,ColumnDescription = "实发数" )]
        public decimal ActualSentQty 
        { 
            get{return _ActualSentQty;}
            set{SetProperty(ref _ActualSentQty, value);}
        }
     

        private decimal _OverSentQty= ((0));
        /// <summary>
        /// 超发数
        /// </summary>
        [AdvQueryAttribute(ColName = "OverSentQty",ColDesc = "超发数")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "OverSentQty",IsNullable = false,ColumnDescription = "超发数" )]
        public decimal OverSentQty 
        { 
            get{return _OverSentQty;}
            set{SetProperty(ref _OverSentQty, value);}
        }
     

        private decimal _WastageQty= ((0));
        /// <summary>
        /// 损耗量
        /// </summary>
        [AdvQueryAttribute(ColName = "WastageQty",ColDesc = "损耗量")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "WastageQty",IsNullable = false,ColumnDescription = "损耗量" )]
        public decimal WastageQty 
        { 
            get{return _WastageQty;}
            set{SetProperty(ref _WastageQty, value);}
        }
     

        private decimal _CurrentIinventory= ((0));
        /// <summary>
        /// 现有库存
        /// </summary>
        [AdvQueryAttribute(ColName = "CurrentIinventory",ColDesc = "现有库存")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "CurrentIinventory",IsNullable = false,ColumnDescription = "现有库存" )]
        public decimal CurrentIinventory 
        { 
            get{return _CurrentIinventory;}
            set{SetProperty(ref _CurrentIinventory, value);}
        }
     

        private decimal _MaterialCost= ((0));
        /// <summary>
        /// 物料成本 
        /// </summary>
        [AdvQueryAttribute(ColName = "MaterialCost",ColDesc = "物料成本 ")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "MaterialCost",IsNullable = false,ColumnDescription = "物料成本 " )]
        public decimal MaterialCost 
        { 
            get{return _MaterialCost;}
            set{SetProperty(ref _MaterialCost, value);}
        }
     

        private long? _BOM_ID;
        /// <summary>
        /// 配方
        /// </summary>
        [AdvQueryAttribute(ColName = "BOM_ID",ColDesc = "配方")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "BOM_ID",IsNullable = true,ColumnDescription = "配方" )]
        public long? BOM_ID 
        { 
            get{return _BOM_ID;}
            set{SetProperty(ref _BOM_ID, value);}
        }
     

        private bool? _IsExternalProduce;
        /// <summary>
        /// 是否托外
        /// </summary>
        [AdvQueryAttribute(ColName = "IsExternalProduce",ColDesc = "是否托外")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsExternalProduce",IsNullable = true,ColumnDescription = "是否托外" )]
        public bool? IsExternalProduce 
        { 
            get{return _IsExternalProduce;}
            set{SetProperty(ref _IsExternalProduce, value);}
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
     

        private string _AssemblyPosition;
        /// <summary>
        /// 组装位置
        /// </summary>
        [AdvQueryAttribute(ColName = "AssemblyPosition",ColDesc = "组装位置")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "AssemblyPosition",Length=500,IsNullable = true,ColumnDescription = "组装位置" )]
        public string AssemblyPosition 
        { 
            get{return _AssemblyPosition;}
            set{SetProperty(ref _AssemblyPosition, value);}
        }
     

        private string _AlternativeProducts;
        /// <summary>
        /// 替代品
        /// </summary>
        [AdvQueryAttribute(ColName = "AlternativeProducts",ColDesc = "替代品")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "AlternativeProducts",Length=10,IsNullable = true,ColumnDescription = "替代品" )]
        public string AlternativeProducts 
        { 
            get{return _AlternativeProducts;}
            set{SetProperty(ref _AlternativeProducts, value);}
        }
     

        private string _Prelevel_BOM_Desc;
        /// <summary>
        /// 上级配方
        /// </summary>
        [AdvQueryAttribute(ColName = "Prelevel_BOM_Desc",ColDesc = "上级配方")]
        [SugarColumn(ColumnDataType = "nvarchar",SqlParameterDbType ="String",ColumnName = "Prelevel_BOM_Desc",Length=100,IsNullable = true,ColumnDescription = "上级配方" )]
        public string Prelevel_BOM_Desc 
        { 
            get{return _Prelevel_BOM_Desc;}
            set{SetProperty(ref _Prelevel_BOM_Desc, value);}
        }


       
    }
}



