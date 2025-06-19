
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:31:59
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
    /// 缴库单明细
    /// </summary>
    [Serializable()]
    [Description("缴库单明细")]
    [SugarTable("tb_FinishedGoodsInvDetail")]
    public partial class tb_FinishedGoodsInvDetail: BaseEntity, ICloneable
    {
        public tb_FinishedGoodsInvDetail()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("成品入库单明细tb_FinishedGoodsInvDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Sub_ID;
        /// <summary>
        /// 缴库明细
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Sub_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "缴库明细" , IsPrimaryKey = true)]
        public long Sub_ID
        { 
            get{return _Sub_ID;}
            set{
            SetProperty(ref _Sub_ID, value);
                base.PrimaryKeyID = _Sub_ID;
            }
        }

        private long? _FG_ID;
        /// <summary>
        /// 缴库单
        /// </summary>
        [AdvQueryAttribute(ColName = "FG_ID",ColDesc = "缴库单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "FG_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "缴库单" )]
        [FKRelationAttribute("tb_FinishedGoodsInv","FG_ID")]
        public long? FG_ID
        { 
            get{return _FG_ID;}
            set{
            SetProperty(ref _FG_ID, value);
                        }
        }

        private long? _Unit_ID;
        /// <summary>
        /// 单位
        /// </summary>
        [AdvQueryAttribute(ColName = "Unit_ID",ColDesc = "单位")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Unit_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "单位" )]
        [FKRelationAttribute("tb_Unit","Unit_ID")]
        public long? Unit_ID
        { 
            get{return _Unit_ID;}
            set{
            SetProperty(ref _Unit_ID, value);
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

        private int _PayableQty= ((0));
        /// <summary>
        /// 应缴数量
        /// </summary>
        [AdvQueryAttribute(ColName = "PayableQty",ColDesc = "应缴数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "PayableQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "应缴数量" )]
        public int PayableQty
        { 
            get{return _PayableQty;}
            set{
            SetProperty(ref _PayableQty, value);
                        }
        }

        private int _Qty= ((0));
        /// <summary>
        /// 实缴数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Qty",ColDesc = "实缴数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Qty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "实缴数量" )]
        public int Qty
        { 
            get{return _Qty;}
            set{
            SetProperty(ref _Qty, value);
                        }
        }

        private decimal _UnitCost= ((0));
        /// <summary>
        /// 单位成本
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitCost",ColDesc = "单位成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UnitCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "单位成本" )]
        public decimal UnitCost
        { 
            get{return _UnitCost;}
            set{
            SetProperty(ref _UnitCost, value);
                        }
        }

        private int _UnpaidQty= ((0));
        /// <summary>
        /// 未缴数量
        /// </summary>
        [AdvQueryAttribute(ColName = "UnpaidQty",ColDesc = "未缴数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "UnpaidQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "未缴数量" )]
        public int UnpaidQty
        { 
            get{return _UnpaidQty;}
            set{
            SetProperty(ref _UnpaidQty, value);
                        }
        }

        private decimal _NetMachineHours= ((0));
        /// <summary>
        /// 单位实际机时
        /// </summary>
        [AdvQueryAttribute(ColName = "NetMachineHours",ColDesc = "单位实际机时")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "NetMachineHours" , DecimalDigits = 5,IsNullable = false,ColumnDescription = "单位实际机时" )]
        public decimal NetMachineHours
        { 
            get{return _NetMachineHours;}
            set{
            SetProperty(ref _NetMachineHours, value);
                        }
        }

        private decimal _NetWorkingHours= ((0));
        /// <summary>
        /// 单位实际工时
        /// </summary>
        [AdvQueryAttribute(ColName = "NetWorkingHours",ColDesc = "单位实际工时")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "NetWorkingHours" , DecimalDigits = 5,IsNullable = false,ColumnDescription = "单位实际工时" )]
        public decimal NetWorkingHours
        { 
            get{return _NetWorkingHours;}
            set{
            SetProperty(ref _NetWorkingHours, value);
                        }
        }

        private decimal _ApportionedCost= ((0));
        /// <summary>
        /// 单位分摊成本
        /// </summary>
        [AdvQueryAttribute(ColName = "ApportionedCost",ColDesc = "单位分摊成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "ApportionedCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "单位分摊成本" )]
        public decimal ApportionedCost
        { 
            get{return _ApportionedCost;}
            set{
            SetProperty(ref _ApportionedCost, value);
                        }
        }

        private decimal _ManuFee= ((0));
        /// <summary>
        /// 单位制造费用
        /// </summary>
        [AdvQueryAttribute(ColName = "ManuFee",ColDesc = "单位制造费用")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "ManuFee" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "单位制造费用" )]
        public decimal ManuFee
        { 
            get{return _ManuFee;}
            set{
            SetProperty(ref _ManuFee, value);
                        }
        }

        private decimal _MaterialCost= ((0));
        /// <summary>
        /// 单位材料成本
        /// </summary>
        [AdvQueryAttribute(ColName = "MaterialCost",ColDesc = "单位材料成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "MaterialCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "单位材料成本" )]
        public decimal MaterialCost
        { 
            get{return _MaterialCost;}
            set{
            SetProperty(ref _MaterialCost, value);
                        }
        }

        //private decimal _SubtotalMaterialCost= ((0));
        ///// <summary>
        ///// 材料小计
        ///// </summary>
        //[AdvQueryAttribute(ColName = "SubtotalMaterialCost",ColDesc = "材料小计")] 
        //[SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalMaterialCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "材料小计" )]
        //public decimal SubtotalMaterialCost
        //{ 
        //    get{return _SubtotalMaterialCost;}
        //    set{
        //    SetProperty(ref _SubtotalMaterialCost, value);
        //                }
        //}

        private decimal _ProductionAllCost= ((0));
        /// <summary>
        /// 成本小计
        /// </summary>
        [AdvQueryAttribute(ColName = "ProductionAllCost",ColDesc = "成本小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "ProductionAllCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "成本小计")]
        public decimal ProductionAllCost
        { 
            get{return _ProductionAllCost;}
            set{
            SetProperty(ref _ProductionAllCost, value);
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

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Rack_ID))]
        public virtual tb_StorageRack tb_storagerack { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(FG_ID))]
        public virtual tb_FinishedGoodsInv tb_finishedgoodsinv { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Unit_ID))]
        public virtual tb_Unit tb_unit { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}





        public override object Clone()
        {
            tb_FinishedGoodsInvDetail loctype = (tb_FinishedGoodsInvDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

