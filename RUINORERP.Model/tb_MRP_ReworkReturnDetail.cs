﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:07
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
    /// 返工退库明细
    /// </summary>
    [Serializable()]
    [Description("返工退库明细")]
    [SugarTable("tb_MRP_ReworkReturnDetail")]
    public partial class tb_MRP_ReworkReturnDetail: BaseEntity, ICloneable
    {
        public tb_MRP_ReworkReturnDetail()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("返工退库明细tb_MRP_ReworkReturnDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ReworkReturnCID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ReworkReturnCID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long ReworkReturnCID
        { 
            get{return _ReworkReturnCID;}
            set{
            SetProperty(ref _ReworkReturnCID, value);
                base.PrimaryKeyID = _ReworkReturnCID;
            }
        }

        private long? _ReworkReturnID;
        /// <summary>
        /// 返工退库单
        /// </summary>
        [AdvQueryAttribute(ColName = "ReworkReturnID",ColDesc = "返工退库单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ReworkReturnID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "返工退库单" )]
        [FKRelationAttribute("tb_MRP_ReworkReturn","ReworkReturnID")]
        public long? ReworkReturnID
        { 
            get{return _ReworkReturnID;}
            set{
            SetProperty(ref _ReworkReturnID, value);
                        }
        }

        private long _Location_ID;
        /// <summary>
        /// 所在仓位
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "所在仓位")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Location_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "所在仓位" )]
        [FKRelationAttribute("tb_Location","Location_ID")]
        public long Location_ID
        { 
            get{return _Location_ID;}
            set{
            SetProperty(ref _Location_ID, value);
                        }
        }

        private long _ProdDetailID;
        /// <summary>
        /// 产品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "产品" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
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

        private int _Quantity= ((0));
        /// <summary>
        /// 数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Quantity" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "数量" )]
        public int Quantity
        { 
            get{return _Quantity;}
            set{
            SetProperty(ref _Quantity, value);
                        }
        }

        private int _DeliveredQuantity= ((0));
        /// <summary>
        /// 已交数量
        /// </summary>
        [AdvQueryAttribute(ColName = "DeliveredQuantity",ColDesc = "已交数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DeliveredQuantity" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "已交数量" )]
        public int DeliveredQuantity
        { 
            get{return _DeliveredQuantity;}
            set{
            SetProperty(ref _DeliveredQuantity, value);
                        }
        }

        private decimal _UnitCost= ((0));
        /// <summary>
        /// 成本
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitCost",ColDesc = "成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UnitCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "成本" )]
        public decimal UnitCost
        { 
            get{return _UnitCost;}
            set{
            SetProperty(ref _UnitCost, value);
                        }
        }

        private decimal _SubtotalReworkFee= ((0));
        /// <summary>
        /// 预估费用小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalReworkFee",ColDesc = "预估费用小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalReworkFee" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "预估费用小计" )]
        public decimal SubtotalReworkFee
        { 
            get{return _SubtotalReworkFee;}
            set{
            SetProperty(ref _SubtotalReworkFee, value);
                        }
        }

        private decimal _ReworkFee= ((0));
        /// <summary>
        /// 预估费用
        /// </summary>
        [AdvQueryAttribute(ColName = "ReworkFee",ColDesc = "预估费用")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "ReworkFee" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "预估费用" )]
        public decimal ReworkFee
        { 
            get{return _ReworkFee;}
            set{
            SetProperty(ref _ReworkFee, value);
                        }
        }

        private decimal _SubtotalCostAmount= ((0));
        /// <summary>
        /// 小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalCostAmount",ColDesc = "小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalCostAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "小计" )]
        public decimal SubtotalCostAmount
        { 
            get{return _SubtotalCostAmount;}
            set{
            SetProperty(ref _SubtotalCostAmount, value);
                        }
        }

        private string _CustomertModel;
        /// <summary>
        /// 客户型号
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomertModel",ColDesc = "客户型号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomertModel" ,Length=50,IsNullable = true,ColumnDescription = "客户型号" )]
        public string CustomertModel
        { 
            get{return _CustomertModel;}
            set{
            SetProperty(ref _CustomertModel, value);
                        }
        }

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=1000,IsNullable = true,ColumnDescription = "摘要" )]
        public string Summary
        { 
            get{return _Summary;}
            set{
            SetProperty(ref _Summary, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ReworkReturnID))]
        public virtual tb_MRP_ReworkReturn tb_mrp_reworkreturn { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






        public override object Clone()
        {
            tb_MRP_ReworkReturnDetail loctype = (tb_MRP_ReworkReturnDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

