
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/15/2024 13:50:21
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
    /// 成品入库单明细
    /// </summary>
    [Serializable()]
    [Description("tb_FinishedGoodsInvDetail")]
    [SugarTable("tb_FinishedGoodsInvDetail")]
    public partial class tb_FinishedGoodsInvDetail: BaseEntity, ICloneable
    {
        public tb_FinishedGoodsInvDetail()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_FinishedGoodsInvDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
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
            base.PrimaryKeyID = _Sub_ID;
            SetProperty(ref _Sub_ID, value);
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
        /// 产品详情
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品详情")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "产品详情" )]
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
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UnitCost" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "单位成本" )]
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

        private int _NetWorkingHours= ((0));
        /// <summary>
        /// 实际工时
        /// </summary>
        [AdvQueryAttribute(ColName = "NetWorkingHours",ColDesc = "实际工时")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "NetWorkingHours" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "实际工时" )]
        public int NetWorkingHours
        { 
            get{return _NetWorkingHours;}
            set{
            SetProperty(ref _NetWorkingHours, value);
            }
        }

        private decimal _ApportionedCost= ((0));
        /// <summary>
        /// 分摊成本
        /// </summary>
        [AdvQueryAttribute(ColName = "ApportionedCost",ColDesc = "分摊成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "ApportionedCost" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "分摊成本" )]
        public decimal ApportionedCost
        { 
            get{return _ApportionedCost;}
            set{
            SetProperty(ref _ApportionedCost, value);
            }
        }

        private decimal _TollFees= ((0));
        /// <summary>
        /// 托工费用
        /// </summary>
        [AdvQueryAttribute(ColName = "TollFees",ColDesc = "托工费用")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TollFees" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "托工费用" )]
        public decimal TollFees
        { 
            get{return _TollFees;}
            set{
            SetProperty(ref _TollFees, value);
            }
        }

        private decimal _LaborCost= ((0));
        /// <summary>
        /// 人工成本
        /// </summary>
        [AdvQueryAttribute(ColName = "LaborCost",ColDesc = "人工成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "LaborCost" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "人工成本" )]
        public decimal LaborCost
        { 
            get{return _LaborCost;}
            set{
            SetProperty(ref _LaborCost, value);
            }
        }

        private decimal _MaterialCost= ((0));
        /// <summary>
        /// 材料成本
        /// </summary>
        [AdvQueryAttribute(ColName = "MaterialCost",ColDesc = "材料成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "MaterialCost" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "材料成本" )]
        public decimal MaterialCost
        { 
            get{return _MaterialCost;}
            set{
            SetProperty(ref _MaterialCost, value);
            }
        }

        private decimal _ProductionCost= ((0));
        /// <summary>
        /// 生产总成本
        /// </summary>
        [AdvQueryAttribute(ColName = "ProductionCost",ColDesc = "生产总成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "ProductionCost" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "生产总成本" )]
        public decimal ProductionCost
        { 
            get{return _ProductionCost;}
            set{
            SetProperty(ref _ProductionCost, value);
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
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Rack_ID))]
        public virtual tb_StorageRack tb_storagerack { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(FG_ID))]
        public virtual tb_FinishedGoodsInv tb_finishedgoodsinv { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Unit_ID))]
        public virtual tb_Unit tb_unit { get; set; }



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
                    Type type = typeof(tb_FinishedGoodsInvDetail);
                    
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
            tb_FinishedGoodsInvDetail loctype = (tb_FinishedGoodsInvDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

