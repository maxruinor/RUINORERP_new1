
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:03
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{
    /// <summary>
    /// 库存表
    /// </summary>
    [Serializable()]
    [Description("库存表")]
    [SugarTable("tb_Inventory")]
    public partial class tb_Inventory: BaseEntity, ICloneable
    {
        public tb_Inventory()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("库存表tb_Inventory" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Inventory_ID;
        /// <summary>
        /// 库存
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Inventory_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "库存" , IsPrimaryKey = true)]
        public long Inventory_ID
        { 
            get{return _Inventory_ID;}
            set{
            SetProperty(ref _Inventory_ID, value);
                base.PrimaryKeyID = _Inventory_ID;
            }
        }

        private long _ProdDetailID;
        /// <summary>
        /// 货品详情
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "货品详情")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "货品详情" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
                        }
        }

        private long? _Rack_ID;
        /// <summary>
        /// 货架
        /// </summary>
        [AdvQueryAttribute(ColName = "Rack_ID",ColDesc = "货架")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Rack_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "货架" )]
        [FKRelationAttribute("tb_StorageRack","Rack_ID")]
        public long? Rack_ID
        { 
            get{return _Rack_ID;}
            set{
            SetProperty(ref _Rack_ID, value);
                        }
        }

        private long _Location_ID;
        /// <summary>
        /// 库位
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "库位")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Location_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "库位" )]
        [FKRelationAttribute("tb_Location","Location_ID")]
        public long Location_ID
        { 
            get{return _Location_ID;}
            set{
            SetProperty(ref _Location_ID, value);
                        }
        }

        private int _Quantity= ((0));
        /// <summary>
        /// 实际库存
        /// </summary>
        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "实际库存")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Quantity" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "实际库存" )]
        public int Quantity
        { 
            get{return _Quantity;}
            set{
            SetProperty(ref _Quantity, value);
                        }
        }

        private int _InitInventory= ((0));
        /// <summary>
        /// 期初数量
        /// </summary>
        [AdvQueryAttribute(ColName = "InitInventory",ColDesc = "期初数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "InitInventory" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "期初数量" )]
        public int InitInventory
        { 
            get{return _InitInventory;}
            set{
            SetProperty(ref _InitInventory, value);
                        }
        }

        private int _Alert_Use= ((0));
        /// <summary>
        /// 使用预警
        /// </summary>
        [AdvQueryAttribute(ColName = "Alert_Use",ColDesc = "使用预警")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Alert_Use" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "使用预警" )]
        public int Alert_Use
        { 
            get{return _Alert_Use;}
            set{
            SetProperty(ref _Alert_Use, value);
                        }
        }

        private int _On_the_way_Qty= ((0));
        /// <summary>
        /// 在途库存
        /// </summary>
        [AdvQueryAttribute(ColName = "On_the_way_Qty",ColDesc = "在途库存")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "On_the_way_Qty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "在途库存" )]
        public int On_the_way_Qty
        { 
            get{return _On_the_way_Qty;}
            set{
            SetProperty(ref _On_the_way_Qty, value);
                        }
        }

        private int _Sale_Qty= ((0));
        /// <summary>
        /// 拟销售量
        /// </summary>
        [AdvQueryAttribute(ColName = "Sale_Qty",ColDesc = "拟销售量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Sale_Qty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "拟销售量" )]
        public int Sale_Qty
        { 
            get{return _Sale_Qty;}
            set{
            SetProperty(ref _Sale_Qty, value);
                        }
        }

        private int _MakingQty= ((0));
        /// <summary>
        /// 在制数量
        /// </summary>
        [AdvQueryAttribute(ColName = "MakingQty",ColDesc = "在制数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "MakingQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "在制数量" )]
        public int MakingQty
        { 
            get{return _MakingQty;}
            set{
            SetProperty(ref _MakingQty, value);
                        }
        }

        private int _NotOutQty= ((0));
        /// <summary>
        /// 未发数量
        /// </summary>
        [AdvQueryAttribute(ColName = "NotOutQty",ColDesc = "未发数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "NotOutQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "未发数量" )]
        public int NotOutQty
        { 
            get{return _NotOutQty;}
            set{
            SetProperty(ref _NotOutQty, value);
                        }
        }

        private int _BatchNumber= ((0));
        /// <summary>
        /// 批次管理
        /// </summary>
        [AdvQueryAttribute(ColName = "BatchNumber",ColDesc = "批次管理")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "BatchNumber" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "批次管理" )]
        public int BatchNumber
        { 
            get{return _BatchNumber;}
            set{
            SetProperty(ref _BatchNumber, value);
                        }
        }

        private int _Alert_Quantity= ((0));
        /// <summary>
        /// 预警值
        /// </summary>
        [AdvQueryAttribute(ColName = "Alert_Quantity",ColDesc = "预警值")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Alert_Quantity" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "预警值" )]
        public int Alert_Quantity
        { 
            get{return _Alert_Quantity;}
            set{
            SetProperty(ref _Alert_Quantity, value);
                        }
        }

        private decimal _CostFIFO= ((0));
        /// <summary>
        /// 先进先出成本
        /// </summary>
        [AdvQueryAttribute(ColName = "CostFIFO",ColDesc = "先进先出成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "CostFIFO" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "先进先出成本" )]
        public decimal CostFIFO
        { 
            get{return _CostFIFO;}
            set{
            SetProperty(ref _CostFIFO, value);
                        }
        }

        private decimal _CostMonthlyWA= ((0));
        /// <summary>
        /// 月加权平均成本
        /// </summary>
        [AdvQueryAttribute(ColName = "CostMonthlyWA",ColDesc = "月加权平均成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "CostMonthlyWA" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "月加权平均成本" )]
        public decimal CostMonthlyWA
        { 
            get{return _CostMonthlyWA;}
            set{
            SetProperty(ref _CostMonthlyWA, value);
                        }
        }

        private decimal _CostMovingWA= ((0));
        /// <summary>
        /// 移动加权平均成本
        /// </summary>
        [AdvQueryAttribute(ColName = "CostMovingWA",ColDesc = "移动加权平均成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "CostMovingWA" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "移动加权平均成本" )]
        public decimal CostMovingWA
        { 
            get{return _CostMovingWA;}
            set{
            SetProperty(ref _CostMovingWA, value);
                        }
        }

        private decimal _Inv_AdvCost= ((0));
        /// <summary>
        /// 实际成本
        /// </summary>
        [AdvQueryAttribute(ColName = "Inv_AdvCost",ColDesc = "实际成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Inv_AdvCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "实际成本")]
        public decimal Inv_AdvCost
        { 
            get{return _Inv_AdvCost;}
            set{
            SetProperty(ref _Inv_AdvCost, value);
                        }
        }

        private decimal _Inv_Cost= ((0));
        /// <summary>
        /// 货品成本
        /// </summary>
        [AdvQueryAttribute(ColName = "Inv_Cost",ColDesc = "货品成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Inv_Cost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "货品成本" )]
        public decimal Inv_Cost
        { 
            get{return _Inv_Cost;}
            set{
            SetProperty(ref _Inv_Cost, value);
                        }
        }

        private decimal _Inv_SubtotalCostMoney= ((0));
        /// <summary>
        /// 成本小计
        /// </summary>
        [AdvQueryAttribute(ColName = "Inv_SubtotalCostMoney",ColDesc = "成本小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Inv_SubtotalCostMoney" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "成本小计" )]
        public decimal Inv_SubtotalCostMoney
        { 
            get{return _Inv_SubtotalCostMoney;}
            set{
            SetProperty(ref _Inv_SubtotalCostMoney, value);
                        }
        }

        private DateTime? _LatestOutboundTime;
        /// <summary>
        /// 最新出库时间
        /// </summary>
        [AdvQueryAttribute(ColName = "LatestOutboundTime",ColDesc = "最新出库时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "LatestOutboundTime" ,IsNullable = true,ColumnDescription = "最新出库时间" )]
        public DateTime? LatestOutboundTime
        { 
            get{return _LatestOutboundTime;}
            set{
            SetProperty(ref _LatestOutboundTime, value);
                        }
        }

        private DateTime? _LatestStorageTime;
        /// <summary>
        /// 最新入库时间
        /// </summary>
        [AdvQueryAttribute(ColName = "LatestStorageTime",ColDesc = "最新入库时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "LatestStorageTime" ,IsNullable = true,ColumnDescription = "最新入库时间" )]
        public DateTime? LatestStorageTime
        { 
            get{return _LatestStorageTime;}
            set{
            SetProperty(ref _LatestStorageTime, value);
                        }
        }

        private DateTime? _LastInventoryDate;
        /// <summary>
        /// 最后盘点时间
        /// </summary>
        [AdvQueryAttribute(ColName = "LastInventoryDate",ColDesc = "最后盘点时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "LastInventoryDate" ,IsNullable = true,ColumnDescription = "最后盘点时间" )]
        public DateTime? LastInventoryDate
        { 
            get{return _LastInventoryDate;}
            set{
            SetProperty(ref _LastInventoryDate, value);
                        }
        }

        private string _Notes;
        /// <summary>
        /// 备注说明
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注说明")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=250,IsNullable = true,ColumnDescription = "备注说明" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
                        }
        }

        private DateTime? _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Created_at" ,IsNullable = true,ColumnDescription = "创建时间" )]
        public DateTime? Created_at
        { 
            get{return _Created_at;}
            set{
            SetProperty(ref _Created_at, value);
                        }
        }

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Created_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "创建人" )]
        public long? Created_by
        { 
            get{return _Created_by;}
            set{
            SetProperty(ref _Created_by, value);
                        }
        }

        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Modified_at" ,IsNullable = true,ColumnDescription = "修改时间" )]
        public DateTime? Modified_at
        { 
            get{return _Modified_at;}
            set{
            SetProperty(ref _Modified_at, value);
                        }
        }

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Modified_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "修改人" )]
        public long? Modified_by
        { 
            get{return _Modified_by;}
            set{
            SetProperty(ref _Modified_by, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Rack_ID))]
        public virtual tb_StorageRack tb_storagerack { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Inv_Alert_Attribute.Inventory_ID))]
        public virtual List<tb_Inv_Alert_Attribute> tb_Inv_Alert_Attributes { get; set; }
        //tb_Inv_Alert_Attribute.Inventory_ID)
        //Inventory_ID.FK_TB_INV_A_REFERENCE_TB_INVEN)
        //tb_Inventory.Inventory_ID)


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






        #region 字段描述对应列表
        private ConcurrentDictionary<string, string> fieldNameList;


        /// <summary>
        /// 表列名的中文描述集合
        /// </summary>
        [Description("列名中文描述"), Category("自定属性")]
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public override ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, string>();
                    SugarColumn entityAttr;
                    Type type = typeof(tb_Inventory);
                    
                       foreach (PropertyInfo field in type.GetProperties())
                            {
                                foreach (Attribute attr in field.GetCustomAttributes(true))
                                {
                                    entityAttr = attr as SugarColumn;
                                    if (null != entityAttr)
                                    {
                                        if (entityAttr.ColumnDescription == null)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.IsIdentity)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.IsPrimaryKey)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.ColumnDescription.Trim().Length > 0)
                                        {
                                            fieldNameList.TryAdd(field.Name, entityAttr.ColumnDescription);
                                        }
                                    }
                                }
                            }
                }
                
                return fieldNameList;
            }
            set
            {
                fieldNameList = value;
            }

        }
        #endregion
        

        public override object Clone()
        {
            tb_Inventory loctype = (tb_Inventory)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

