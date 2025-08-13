
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/13/2025 17:30:04
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
    /// 库存快照表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_InventorySnapshot")]
    public partial class tb_InventorySnapshotQueryDto:BaseEntityDto
    {
        public tb_InventorySnapshotQueryDto()
        {

        }

    
     

        private long _ProdDetailID;
        /// <summary>
        /// 产品详情
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品详情")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProdDetailID",IsNullable = false,ColumnDescription = "产品详情" )]
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
        public long Location_ID 
        { 
            get{return _Location_ID;}
            set{SetProperty(ref _Location_ID, value);}
        }
     

        private int _Quantity= ((0));
        /// <summary>
        /// 实际库存
        /// </summary>
        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "实际库存")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Quantity",IsNullable = false,ColumnDescription = "实际库存" )]
        public int Quantity 
        { 
            get{return _Quantity;}
            set{SetProperty(ref _Quantity, value);}
        }
     

        private int _InitInventory= ((0));
        /// <summary>
        /// 期初数量
        /// </summary>
        [AdvQueryAttribute(ColName = "InitInventory",ColDesc = "期初数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "InitInventory",IsNullable = false,ColumnDescription = "期初数量" )]
        public int InitInventory 
        { 
            get{return _InitInventory;}
            set{SetProperty(ref _InitInventory, value);}
        }
     

        private long? _Rack_ID;
        /// <summary>
        /// 货架
        /// </summary>
        [AdvQueryAttribute(ColName = "Rack_ID",ColDesc = "货架")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Rack_ID",IsNullable = true,ColumnDescription = "货架" )]
        public long? Rack_ID 
        { 
            get{return _Rack_ID;}
            set{SetProperty(ref _Rack_ID, value);}
        }
     

        private int _On_the_way_Qty= ((0));
        /// <summary>
        /// 在途库存
        /// </summary>
        [AdvQueryAttribute(ColName = "On_the_way_Qty",ColDesc = "在途库存")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "On_the_way_Qty",IsNullable = false,ColumnDescription = "在途库存" )]
        public int On_the_way_Qty 
        { 
            get{return _On_the_way_Qty;}
            set{SetProperty(ref _On_the_way_Qty, value);}
        }
     

        private int _Sale_Qty= ((0));
        /// <summary>
        /// 拟销售量
        /// </summary>
        [AdvQueryAttribute(ColName = "Sale_Qty",ColDesc = "拟销售量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Sale_Qty",IsNullable = false,ColumnDescription = "拟销售量" )]
        public int Sale_Qty 
        { 
            get{return _Sale_Qty;}
            set{SetProperty(ref _Sale_Qty, value);}
        }
     

        private int _MakingQty= ((0));
        /// <summary>
        /// 在制数量
        /// </summary>
        [AdvQueryAttribute(ColName = "MakingQty",ColDesc = "在制数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "MakingQty",IsNullable = false,ColumnDescription = "在制数量" )]
        public int MakingQty 
        { 
            get{return _MakingQty;}
            set{SetProperty(ref _MakingQty, value);}
        }
     

        private int _NotOutQty= ((0));
        /// <summary>
        /// 未发料量
        /// </summary>
        [AdvQueryAttribute(ColName = "NotOutQty",ColDesc = "未发料量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "NotOutQty",IsNullable = false,ColumnDescription = "未发料量" )]
        public int NotOutQty 
        { 
            get{return _NotOutQty;}
            set{SetProperty(ref _NotOutQty, value);}
        }
     

        private decimal _CostFIFO= ((0));
        /// <summary>
        /// 先进先出成本
        /// </summary>
        [AdvQueryAttribute(ColName = "CostFIFO",ColDesc = "先进先出成本")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "CostFIFO",IsNullable = false,ColumnDescription = "先进先出成本" )]
        public decimal CostFIFO 
        { 
            get{return _CostFIFO;}
            set{SetProperty(ref _CostFIFO, value);}
        }
     

        private decimal _CostMonthlyWA= ((0));
        /// <summary>
        /// 月加权平均成本
        /// </summary>
        [AdvQueryAttribute(ColName = "CostMonthlyWA",ColDesc = "月加权平均成本")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "CostMonthlyWA",IsNullable = false,ColumnDescription = "月加权平均成本" )]
        public decimal CostMonthlyWA 
        { 
            get{return _CostMonthlyWA;}
            set{SetProperty(ref _CostMonthlyWA, value);}
        }
     

        private decimal _CostMovingWA= ((0));
        /// <summary>
        /// 移动加权平均成本
        /// </summary>
        [AdvQueryAttribute(ColName = "CostMovingWA",ColDesc = "移动加权平均成本")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "CostMovingWA",IsNullable = false,ColumnDescription = "移动加权平均成本" )]
        public decimal CostMovingWA 
        { 
            get{return _CostMovingWA;}
            set{SetProperty(ref _CostMovingWA, value);}
        }
     

        private decimal _Inv_AdvCost= ((0));
        /// <summary>
        /// 实际成本
        /// </summary>
        [AdvQueryAttribute(ColName = "Inv_AdvCost",ColDesc = "实际成本")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "Inv_AdvCost",IsNullable = false,ColumnDescription = "实际成本" )]
        public decimal Inv_AdvCost 
        { 
            get{return _Inv_AdvCost;}
            set{SetProperty(ref _Inv_AdvCost, value);}
        }
     

        private decimal _Inv_Cost= ((0));
        /// <summary>
        /// 产品成本
        /// </summary>
        [AdvQueryAttribute(ColName = "Inv_Cost",ColDesc = "产品成本")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "Inv_Cost",IsNullable = false,ColumnDescription = "产品成本" )]
        public decimal Inv_Cost 
        { 
            get{return _Inv_Cost;}
            set{SetProperty(ref _Inv_Cost, value);}
        }
     

        private decimal _Inv_SubtotalCostMoney= ((0));
        /// <summary>
        /// 成本小计
        /// </summary>
        [AdvQueryAttribute(ColName = "Inv_SubtotalCostMoney",ColDesc = "成本小计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "Inv_SubtotalCostMoney",IsNullable = false,ColumnDescription = "成本小计" )]
        public decimal Inv_SubtotalCostMoney 
        { 
            get{return _Inv_SubtotalCostMoney;}
            set{SetProperty(ref _Inv_SubtotalCostMoney, value);}
        }
     

        private DateTime? _LatestOutboundTime;
        /// <summary>
        /// 最新出库时间
        /// </summary>
        [AdvQueryAttribute(ColName = "LatestOutboundTime",ColDesc = "最新出库时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "LatestOutboundTime",IsNullable = true,ColumnDescription = "最新出库时间" )]
        public DateTime? LatestOutboundTime 
        { 
            get{return _LatestOutboundTime;}
            set{SetProperty(ref _LatestOutboundTime, value);}
        }
     

        private DateTime? _LatestStorageTime;
        /// <summary>
        /// 最新入库时间
        /// </summary>
        [AdvQueryAttribute(ColName = "LatestStorageTime",ColDesc = "最新入库时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "LatestStorageTime",IsNullable = true,ColumnDescription = "最新入库时间" )]
        public DateTime? LatestStorageTime 
        { 
            get{return _LatestStorageTime;}
            set{SetProperty(ref _LatestStorageTime, value);}
        }
     

        private DateTime? _LastInventoryDate;
        /// <summary>
        /// 最后盘点时间
        /// </summary>
        [AdvQueryAttribute(ColName = "LastInventoryDate",ColDesc = "最后盘点时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "LastInventoryDate",IsNullable = true,ColumnDescription = "最后盘点时间" )]
        public DateTime? LastInventoryDate 
        { 
            get{return _LastInventoryDate;}
            set{SetProperty(ref _LastInventoryDate, value);}
        }
     

        private DateTime? _SnapshotTime;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "SnapshotTime",ColDesc = "修改时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "SnapshotTime",IsNullable = true,ColumnDescription = "修改时间" )]
        public DateTime? SnapshotTime 
        { 
            get{return _SnapshotTime;}
            set{SetProperty(ref _SnapshotTime, value);}
        }
     

        private string _Notes;
        /// <summary>
        /// 备注说明
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注说明")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=250,IsNullable = true,ColumnDescription = "备注说明" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
        }


       
    }
}



