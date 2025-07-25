﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:28
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{
    /// <summary>
    /// 销售出库退回明细
    /// </summary>
    [Serializable()]
    [Description("销售出库退回明细")]
    [SugarTable("tb_SaleOutReDetail")]
    public partial class tb_SaleOutReDetail : BaseEntity, ICloneable
    {
        public tb_SaleOutReDetail()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("销售出库退回明细tb_SaleOutReDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _SOutReturnDetail_ID;
        /// <summary>
        /// 明细
        /// </summary>

        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "SOutReturnDetail_ID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "明细", IsPrimaryKey = true)]
        public long SOutReturnDetail_ID
        {
            get { return _SOutReturnDetail_ID; }
            set
            {
                SetProperty(ref _SOutReturnDetail_ID, value);
                base.PrimaryKeyID = _SOutReturnDetail_ID;
            }
        }

        private long _Location_ID;
        /// <summary>
        /// 库位
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID", ColDesc = "库位")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Location_ID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "库位")]
        [FKRelationAttribute("tb_Location", "Location_ID")]
        public long Location_ID
        {
            get { return _Location_ID; }
            set
            {
                SetProperty(ref _Location_ID, value);
            }
        }

        private long? _Rack_ID;
        /// <summary>
        /// 货架
        /// </summary>
        [AdvQueryAttribute(ColName = "Rack_ID", ColDesc = "货架")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Rack_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "货架")]
        [FKRelationAttribute("tb_StorageRack", "Rack_ID")]
        public long? Rack_ID
        {
            get { return _Rack_ID; }
            set
            {
                SetProperty(ref _Rack_ID, value);
            }
        }

        private long _ProdDetailID;
        /// <summary>
        /// 货品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID", ColDesc = "货品")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "ProdDetailID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "货品")]
        [FKRelationAttribute("tb_ProdDetail", "ProdDetailID")]
        public long ProdDetailID
        {
            get { return _ProdDetailID; }
            set
            {
                SetProperty(ref _ProdDetailID, value);
            }
        }

        private long _SaleOutRe_ID;
        /// <summary>
        /// 销售退回单
        /// </summary>
        [AdvQueryAttribute(ColName = "SaleOutRe_ID", ColDesc = "销售退回单")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "SaleOutRe_ID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "销售退回单")]
        [FKRelationAttribute("tb_SaleOutRe", "SaleOutRe_ID")]
        public long SaleOutRe_ID
        {
            get { return _SaleOutRe_ID; }
            set
            {
                SetProperty(ref _SaleOutRe_ID, value);
            }
        }

        private string _property;
        /// <summary>
        /// 属性
        /// </summary>
        [AdvQueryAttribute(ColName = "property", ColDesc = "属性")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "property", Length = 255, IsNullable = true, ColumnDescription = "属性")]
        public string property
        {
            get { return _property; }
            set
            {
                SetProperty(ref _property, value);
            }
        }

        private int _Quantity = ((0));
        /// <summary>
        /// 退回数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Quantity", ColDesc = "退回数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "Quantity", DecimalDigits = 0, IsNullable = false, ColumnDescription = "退回数量")]
        public int Quantity
        {
            get { return _Quantity; }
            set
            {
                SetProperty(ref _Quantity, value);
            }
        }

        private decimal _TransactionPrice = ((0));
        /// <summary>
        /// 实际退款单价
        /// </summary>
        [AdvQueryAttribute(ColName = "TransactionPrice", ColDesc = "实际退款单价")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TransactionPrice", DecimalDigits = 4, IsNullable = false, ColumnDescription = "实际退款单价")]
        public decimal TransactionPrice
        {
            get { return _TransactionPrice; }
            set
            {
                SetProperty(ref _TransactionPrice, value);
            }
        }

        private decimal _SubtotalTransAmount = ((0));
        /// <summary>
        /// 小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalTransAmount", ColDesc = "小计")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "SubtotalTransAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "小计")]
        public decimal SubtotalTransAmount
        {
            get { return _SubtotalTransAmount; }
            set
            {
                SetProperty(ref _SubtotalTransAmount, value);
            }
        }

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary", ColDesc = "摘要")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "Summary", Length = 255, IsNullable = true, ColumnDescription = "摘要")]
        public string Summary
        {
            get { return _Summary; }
            set
            {
                SetProperty(ref _Summary, value);
            }
        }

        private string _CustomerPartNo;
        /// <summary>
        /// 客户型号
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerPartNo", ColDesc = "客户型号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "CustomerPartNo", Length = 50, IsNullable = true, ColumnDescription = "客户型号")]
        public string CustomerPartNo
        {
            get { return _CustomerPartNo; }
            set
            {
                SetProperty(ref _CustomerPartNo, value);
            }
        }

        private decimal _Cost = ((0));
        /// <summary>
        /// 成本
        /// </summary>
        [AdvQueryAttribute(ColName = "Cost", ColDesc = "成本")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "Cost", DecimalDigits = 4, IsNullable = false, ColumnDescription = "成本")]
        public decimal Cost
        {
            get { return _Cost; }
            set
            {
                SetProperty(ref _Cost, value);
            }
        }

        private decimal _CustomizedCost = ((0));
        /// <summary>
        /// 定制成本
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomizedCost", ColDesc = "定制成本")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "CustomizedCost", DecimalDigits = 4, IsNullable = false, ColumnDescription = "定制成本")]
        public decimal CustomizedCost
        {
            get { return _CustomizedCost; }
            set
            {
                SetProperty(ref _CustomizedCost, value);
            }
        }

        private decimal _SubtotalCostAmount = ((0));
        /// <summary>
        /// 成本小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalCostAmount", ColDesc = "成本小计")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "SubtotalCostAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "成本小计")]
        public decimal SubtotalCostAmount
        {
            get { return _SubtotalCostAmount; }
            set
            {
                SetProperty(ref _SubtotalCostAmount, value);
            }
        }

        private decimal _SubtotalUntaxedAmount = ((0));
        /// <summary>
        /// 未税本位币
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalUntaxedAmount", ColDesc = "未税本位币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "SubtotalUntaxedAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "未税本位币")]
        public decimal SubtotalUntaxedAmount
        {
            get { return _SubtotalUntaxedAmount; }
            set
            {
                SetProperty(ref _SubtotalUntaxedAmount, value);
            }
        }

        private bool? _Gift = false;
        /// <summary>
        /// 赠品
        /// </summary>
        [AdvQueryAttribute(ColName = "Gift", ColDesc = "赠品")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "Gift", IsNullable = true, ColumnDescription = "赠品")]
        public bool? Gift
        {
            get { return _Gift; }
            set
            {
                SetProperty(ref _Gift, value);
            }
        }

        private decimal _TaxRate = ((0));
        /// <summary>
        /// 税率
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxRate", ColDesc = "税率")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType = "Decimal", ColumnName = "TaxRate", DecimalDigits = 2, IsNullable = false, ColumnDescription = "税率")]
        public decimal TaxRate
        {
            get { return _TaxRate; }
            set
            {
                SetProperty(ref _TaxRate, value);
            }
        }

        private decimal _SubtotalTaxAmount = ((0));
        /// <summary>
        /// 税额
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalTaxAmount", ColDesc = "税额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "SubtotalTaxAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "税额")]
        public decimal SubtotalTaxAmount
        {
            get { return _SubtotalTaxAmount; }
            set
            {
                SetProperty(ref _SubtotalTaxAmount, value);
            }
        }

        private decimal _UnitCommissionAmount;
        /// <summary>
        /// 单品返还佣金
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitCommissionAmount", ColDesc = "单品返还佣金")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "UnitCommissionAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "单品返还佣金")]
        public decimal UnitCommissionAmount
        {
            get { return _UnitCommissionAmount; }
            set
            {
                SetProperty(ref _UnitCommissionAmount, value);
            }
        }

        private decimal _CommissionAmount=0;
        /// <summary>
        /// 返还佣金小计
        /// </summary>
        [AdvQueryAttribute(ColName = "CommissionAmount", ColDesc = "返还佣金小计")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "CommissionAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "返还佣金小计")]
        public decimal CommissionAmount
        {
            get { return _CommissionAmount; }
            set
            {
                SetProperty(ref _CommissionAmount, value);
            }
        }

        private string _SaleFlagCode;
        /// <summary>
        /// 标识代码
        /// </summary>
        [AdvQueryAttribute(ColName = "SaleFlagCode", ColDesc = "标识代码")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "SaleFlagCode", Length = 100, IsNullable = true, ColumnDescription = "标识代码")]
        public string SaleFlagCode
        {
            get { return _SaleFlagCode; }
            set
            {
                SetProperty(ref _SaleFlagCode, value);
            }
        }

        private long? _SaleOutDetail_ID;
        /// <summary>
        /// 出库行号
        /// </summary>
        [AdvQueryAttribute(ColName = "SaleOutDetail_ID", ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "SaleOutDetail_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "")]
        public long? SaleOutDetail_ID
        {
            get { return _SaleOutDetail_ID; }
            set
            {
                SetProperty(ref _SaleOutDetail_ID, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(SaleOutRe_ID))]
        public virtual tb_SaleOutRe tb_saleoutre { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Rack_ID))]
        public virtual tb_StorageRack tb_storagerack { get; set; }

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
            bool rs = true;
            return rs;
        }



 


        public override object Clone()
        {
            tb_SaleOutReDetail loctype = (tb_SaleOutReDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

