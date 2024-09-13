
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:15
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
    /// 生产需求分析目标对象明细
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ProductionDemandTargetDetail")]
    public partial class tb_ProductionDemandTargetDetailQueryDto:BaseEntityDto
    {
        public tb_ProductionDemandTargetDetailQueryDto()
        {

        }

    
     

        private long _PDID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "PDID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "PDID",IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_ProductionDemand","PDID")]
        public long PDID 
        { 
            get{return _PDID;}
            set{SetProperty(ref _PDID, value);}
        }
     

        private long _ProdDetailID;
        /// <summary>
        /// 货品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "货品")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProdDetailID",IsNullable = false,ColumnDescription = "货品" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID 
        { 
            get{return _ProdDetailID;}
            set{SetProperty(ref _ProdDetailID, value);}
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
     

        private int _NeedQuantity= ((0));
        /// <summary>
        /// 需求数量
        /// </summary>
        [AdvQueryAttribute(ColName = "NeedQuantity",ColDesc = "需求数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "NeedQuantity",IsNullable = false,ColumnDescription = "需求数量" )]
        public int NeedQuantity 
        { 
            get{return _NeedQuantity;}
            set{SetProperty(ref _NeedQuantity, value);}
        }
     

        private DateTime _RequirementDate;
        /// <summary>
        /// 需求日期
        /// </summary>
        [AdvQueryAttribute(ColName = "RequirementDate",ColDesc = "需求日期")]
        [SugarColumn(ColumnDataType = "date",SqlParameterDbType ="DateTime",ColumnName = "RequirementDate",IsNullable = false,ColumnDescription = "需求日期" )]
        public DateTime RequirementDate 
        { 
            get{return _RequirementDate;}
            set{SetProperty(ref _RequirementDate, value);}
        }
     

        private int _BookInventory= ((0));
        /// <summary>
        /// 账面库存
        /// </summary>
        [AdvQueryAttribute(ColName = "BookInventory",ColDesc = "账面库存")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "BookInventory",IsNullable = false,ColumnDescription = "账面库存" )]
        public int BookInventory 
        { 
            get{return _BookInventory;}
            set{SetProperty(ref _BookInventory, value);}
        }
     

        private int _AvailableStock= ((0));
        /// <summary>
        /// 可用库存
        /// </summary>
        [AdvQueryAttribute(ColName = "AvailableStock",ColDesc = "可用库存")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "AvailableStock",IsNullable = false,ColumnDescription = "可用库存" )]
        public int AvailableStock 
        { 
            get{return _AvailableStock;}
            set{SetProperty(ref _AvailableStock, value);}
        }
     

        private int _InTransitInventory= ((0));
        /// <summary>
        /// 在途库存
        /// </summary>
        [AdvQueryAttribute(ColName = "InTransitInventory",ColDesc = "在途库存")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "InTransitInventory",IsNullable = false,ColumnDescription = "在途库存" )]
        public int InTransitInventory 
        { 
            get{return _InTransitInventory;}
            set{SetProperty(ref _InTransitInventory, value);}
        }
     

        private int _MakeProcessInventory= ((0));
        /// <summary>
        /// 在制库存
        /// </summary>
        [AdvQueryAttribute(ColName = "MakeProcessInventory",ColDesc = "在制库存")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "MakeProcessInventory",IsNullable = false,ColumnDescription = "在制库存" )]
        public int MakeProcessInventory 
        { 
            get{return _MakeProcessInventory;}
            set{SetProperty(ref _MakeProcessInventory, value);}
        }
     

        private int _SaleQty= ((0));
        /// <summary>
        /// 拟销售量
        /// </summary>
        [AdvQueryAttribute(ColName = "SaleQty",ColDesc = "拟销售量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "SaleQty",IsNullable = false,ColumnDescription = "拟销售量" )]
        public int SaleQty 
        { 
            get{return _SaleQty;}
            set{SetProperty(ref _SaleQty, value);}
        }
     

        private int _NotIssueMaterialQty= ((0));
        /// <summary>
        /// 未发料量
        /// </summary>
        [AdvQueryAttribute(ColName = "NotIssueMaterialQty",ColDesc = "未发料量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "NotIssueMaterialQty",IsNullable = false,ColumnDescription = "未发料量" )]
        public int NotIssueMaterialQty 
        { 
            get{return _NotIssueMaterialQty;}
            set{SetProperty(ref _NotIssueMaterialQty, value);}
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
     

        private long _PPCID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "PPCID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "PPCID",IsNullable = false,ColumnDescription = "" )]
        public long PPCID 
        { 
            get{return _PPCID;}
            set{SetProperty(ref _PPCID, value);}
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


       
    }
}



