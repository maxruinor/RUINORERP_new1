
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:41:57
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
    /// 制令单的原料明细表 明细对应的是一个树，结构同BOM，先把BOM搞好再来实现这里的细节
    /// </summary>
    [Serializable()]
    [Description("制令单明细表")]
    [SugarTable("tb_ManufacturingOrderDetail")]
    public partial class tb_ManufacturingOrderDetail: BaseEntity, ICloneable
    {
        public tb_ManufacturingOrderDetail()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("制令单的原料明细表 明细对应的是一个树，结构同BOM，先把BOM搞好再来实现这里的细节tb_ManufacturingOrderDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _MOCID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "MOCID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long MOCID
        { 
            get{return _MOCID;}
            set{
            SetProperty(ref _MOCID, value);
                base.PrimaryKeyID = _MOCID;
            }
        }

        private long _MOID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "MOID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "MOID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_ManufacturingOrder","MOID")]
        public long MOID
        { 
            get{return _MOID;}
            set{
            SetProperty(ref _MOID, value);
                        }
        }

        private long _ProdDetailID;
        /// <summary>
        /// 货品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "货品")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "货品" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
                        }
        }

        private bool? _IsKeyMaterial;
        /// <summary>
        /// 关键物料
        /// </summary>
        [AdvQueryAttribute(ColName = "IsKeyMaterial",ColDesc = "关键物料")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsKeyMaterial" ,IsNullable = true,ColumnDescription = "关键物料" )]
        public bool? IsKeyMaterial
        { 
            get{return _IsKeyMaterial;}
            set{
            SetProperty(ref _IsKeyMaterial, value);
                        }
        }

        private string _property;
        /// <summary>
        /// 属性
        /// </summary>
        [AdvQueryAttribute(ColName = "property",ColDesc = "属性")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "property" ,Length=255,IsNullable = true,ColumnDescription = "属性" )]
        public string property
        { 
            get{return _property;}
            set{
            SetProperty(ref _property, value);
                        }
        }

        private long _ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        public long ID
        { 
            get{return _ID;}
            set{
            SetProperty(ref _ID, value);
                        }
        }

        private long _ParentId;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ParentId",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ParentId" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        public long ParentId
        { 
            get{return _ParentId;}
            set{
            SetProperty(ref _ParentId, value);
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

        private string _BOM_NO;
        /// <summary>
        /// 配方编号
        /// </summary>
        [AdvQueryAttribute(ColName = "BOM_NO",ColDesc = "配方编号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BOM_NO" ,Length=50,IsNullable = true,ColumnDescription = "配方编号" )]
        public string BOM_NO
        { 
            get{return _BOM_NO;}
            set{
            SetProperty(ref _BOM_NO, value);
                        }
        }

        private decimal _ShouldSendQty= ((0));
        /// <summary>
        /// 应发数
        /// </summary>
        [AdvQueryAttribute(ColName = "ShouldSendQty",ColDesc = "应发数")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "ShouldSendQty" , DecimalDigits = 3,IsNullable = false,ColumnDescription = "应发数" )]
        public decimal ShouldSendQty
        { 
            get{return _ShouldSendQty;}
            set{
            SetProperty(ref _ShouldSendQty, value);
                        }
        }

        private decimal _ActualSentQty= ((0));
        /// <summary>
        /// 实发数
        /// </summary>
        [AdvQueryAttribute(ColName = "ActualSentQty",ColDesc = "实发数")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "ActualSentQty" , DecimalDigits = 3,IsNullable = false,ColumnDescription = "实发数" )]
        public decimal ActualSentQty
        { 
            get{return _ActualSentQty;}
            set{
            SetProperty(ref _ActualSentQty, value);
                        }
        }

        private decimal _OverSentQty= ((0));
        /// <summary>
        /// 超发数
        /// </summary>
        [AdvQueryAttribute(ColName = "OverSentQty",ColDesc = "超发数")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "OverSentQty" , DecimalDigits = 3,IsNullable = false,ColumnDescription = "超发数" )]
        public decimal OverSentQty
        { 
            get{return _OverSentQty;}
            set{
            SetProperty(ref _OverSentQty, value);
                        }
        }

        private decimal _WastageQty= ((0));
        /// <summary>
        /// 损耗量
        /// </summary>
        [AdvQueryAttribute(ColName = "WastageQty",ColDesc = "损耗量")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "WastageQty" , DecimalDigits = 3,IsNullable = false,ColumnDescription = "损耗量" )]
        public decimal WastageQty
        { 
            get{return _WastageQty;}
            set{
            SetProperty(ref _WastageQty, value);
                        }
        }

        private decimal _CurrentIinventory= ((0));
        /// <summary>
        /// 现有库存
        /// </summary>
        [AdvQueryAttribute(ColName = "CurrentIinventory",ColDesc = "现有库存")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "CurrentIinventory" , DecimalDigits = 3,IsNullable = false,ColumnDescription = "现有库存" )]
        public decimal CurrentIinventory
        { 
            get{return _CurrentIinventory;}
            set{
            SetProperty(ref _CurrentIinventory, value);
                        }
        }

        private decimal _UnitCost= ((0));
        /// <summary>
        /// 单位成本
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitCost",ColDesc = "单位成本")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "UnitCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "单位成本" )]
        public decimal UnitCost
        { 
            get{return _UnitCost;}
            set{
            SetProperty(ref _UnitCost, value);
                        }
        }

        private decimal _SubtotalUnitCost= ((0));
        /// <summary>
        /// 成本小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalUnitCost",ColDesc = "成本小计")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalUnitCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "成本小计" )]
        public decimal SubtotalUnitCost
        { 
            get{return _SubtotalUnitCost;}
            set{
            SetProperty(ref _SubtotalUnitCost, value);
                        }
        }

        private long? _BOM_ID;
        /// <summary>
        /// 配方
        /// </summary>
        [AdvQueryAttribute(ColName = "BOM_ID",ColDesc = "配方")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BOM_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "配方" )]
        public long? BOM_ID
        { 
            get{return _BOM_ID;}
            set{
            SetProperty(ref _BOM_ID, value);
                        }
        }

        private bool? _IsExternalProduce;
        /// <summary>
        /// 是否托外
        /// </summary>
        [AdvQueryAttribute(ColName = "IsExternalProduce",ColDesc = "是否托外")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsExternalProduce" ,IsNullable = true,ColumnDescription = "是否托外" )]
        public bool? IsExternalProduce
        { 
            get{return _IsExternalProduce;}
            set{
            SetProperty(ref _IsExternalProduce, value);
                        }
        }

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=255,IsNullable = true,ColumnDescription = "摘要" )]
        public string Summary
        { 
            get{return _Summary;}
            set{
            SetProperty(ref _Summary, value);
                        }
        }

        private string _AssemblyPosition;
        /// <summary>
        /// 组装位置
        /// </summary>
        [AdvQueryAttribute(ColName = "AssemblyPosition",ColDesc = "组装位置")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "AssemblyPosition" ,Length=500,IsNullable = true,ColumnDescription = "组装位置" )]
        public string AssemblyPosition
        { 
            get{return _AssemblyPosition;}
            set{
            SetProperty(ref _AssemblyPosition, value);
                        }
        }

        private string _AlternativeProducts;
        /// <summary>
        /// 替代品
        /// </summary>
        [AdvQueryAttribute(ColName = "AlternativeProducts",ColDesc = "替代品")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "AlternativeProducts" ,Length=50,IsNullable = true,ColumnDescription = "替代品" )]
        public string AlternativeProducts
        { 
            get{return _AlternativeProducts;}
            set{
            SetProperty(ref _AlternativeProducts, value);
                        }
        }

        private string _Prelevel_BOM_Desc;
        /// <summary>
        /// 上级配方名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Prelevel_BOM_Desc",ColDesc = "上级配方名称")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "Prelevel_BOM_Desc" ,Length=100,IsNullable = true,ColumnDescription = "上级配方名称" )]
        public string Prelevel_BOM_Desc
        { 
            get{return _Prelevel_BOM_Desc;}
            set{
            SetProperty(ref _Prelevel_BOM_Desc, value);
                        }
        }

        private long? _Prelevel_BOM_ID;
        /// <summary>
        /// 上级配方
        /// </summary>
        [AdvQueryAttribute(ColName = "Prelevel_BOM_ID",ColDesc = "上级配方")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Prelevel_BOM_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "上级配方" )]
        public long? Prelevel_BOM_ID
        { 
            get{return _Prelevel_BOM_ID;}
            set{
            SetProperty(ref _Prelevel_BOM_ID, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(MOID))]
        public virtual tb_ManufacturingOrder tb_manufacturingorder { get; set; }



        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_ManufacturingOrderDetail loctype = (tb_ManufacturingOrderDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

