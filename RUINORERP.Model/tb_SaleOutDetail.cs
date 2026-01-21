
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:42:15
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
    /// 销售出库明细
    /// </summary>
    [Serializable()]
    [Description("销售出库明细")]
    [SugarTable("tb_SaleOutDetail")]
    public partial class tb_SaleOutDetail: BaseEntity, ICloneable
    {
        public tb_SaleOutDetail()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("销售出库明细tb_SaleOutDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _SaleOutDetail_ID;
        /// <summary>
        /// 明细
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "SaleOutDetail_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "明细" , IsPrimaryKey = true)]
        public long SaleOutDetail_ID
        { 
            get{return _SaleOutDetail_ID;}
            set{
            SetProperty(ref _SaleOutDetail_ID, value);
                base.PrimaryKeyID = _SaleOutDetail_ID;
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

        private long _SaleOut_MainID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "SaleOut_MainID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "SaleOut_MainID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_SaleOut","SaleOut_MainID")]
        public long SaleOut_MainID
        { 
            get{return _SaleOut_MainID;}
            set{
            SetProperty(ref _SaleOut_MainID, value);
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

        private decimal _UnitPrice;
        /// <summary>
        /// 单价
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitPrice",ColDesc = "单价")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UnitPrice" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "单价" )]
        public decimal UnitPrice
        { 
            get{return _UnitPrice;}
            set{
            SetProperty(ref _UnitPrice, value);
                        }
        }

        private decimal _Discount= ((1));
        /// <summary>
        /// 折扣
        /// </summary>
        [AdvQueryAttribute(ColName = "Discount",ColDesc = "折扣")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Discount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "折扣" )]
        public decimal Discount
        { 
            get{return _Discount;}
            set{
            SetProperty(ref _Discount, value);
                        }
        }

        private decimal _TransactionPrice= ((0));
        /// <summary>
        /// 成交单价
        /// </summary>
        [AdvQueryAttribute(ColName = "TransactionPrice",ColDesc = "成交单价")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TransactionPrice" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "成交单价" )]
        public decimal TransactionPrice
        { 
            get{return _TransactionPrice;}
            set{
            SetProperty(ref _TransactionPrice, value);
                        }
        }

        private decimal _SubtotalTransAmount= ((0));
        /// <summary>
        /// 成交小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalTransAmount",ColDesc = "成交小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalTransAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "成交小计" )]
        public decimal SubtotalTransAmount
        { 
            get{return _SubtotalTransAmount;}
            set{
            SetProperty(ref _SubtotalTransAmount, value);
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

        private string _CustomerPartNo;
        /// <summary>
        /// 客户型号
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerPartNo",ColDesc = "客户型号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomerPartNo" ,Length=50,IsNullable = true,ColumnDescription = "客户型号" )]
        public string CustomerPartNo
        { 
            get{return _CustomerPartNo;}
            set{
            SetProperty(ref _CustomerPartNo, value);
                        }
        }

        private decimal _CustomizedCost= ((0));
        /// <summary>
        /// 定制成本
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomizedCost",ColDesc = "定制成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "CustomizedCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "定制成本" )]
        public decimal CustomizedCost
        { 
            get{return _CustomizedCost;}
            set{
            SetProperty(ref _CustomizedCost, value);
                        }
        }

        private decimal _Cost;
        /// <summary>
        /// 成本
        /// </summary>
        [AdvQueryAttribute(ColName = "Cost",ColDesc = "成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Cost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "成本" )]
        public decimal Cost
        { 
            get{return _Cost;}
            set{
            SetProperty(ref _Cost, value);
                        }
        }

        private decimal _SubtotalCostAmount;
        /// <summary>
        /// 成本小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalCostAmount",ColDesc = "成本小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalCostAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "成本小计" )]
        public decimal SubtotalCostAmount
        { 
            get{return _SubtotalCostAmount;}
            set{
            SetProperty(ref _SubtotalCostAmount, value);
                        }
        }

        private decimal _TaxRate;
        /// <summary>
        /// 税率
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxRate",ColDesc = "税率")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "TaxRate" , DecimalDigits = 3,IsNullable = false,ColumnDescription = "税率" )]
        public decimal TaxRate
        { 
            get{return _TaxRate;}
            set{
            SetProperty(ref _TaxRate, value);
                        }
        }

        private int _TotalReturnedQty= ((0));
        /// <summary>
        /// 订单退回数
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalReturnedQty",ColDesc = "订单退回数")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "TotalReturnedQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "订单退回数" )]
        public int TotalReturnedQty
        { 
            get{return _TotalReturnedQty;}
            set{
            SetProperty(ref _TotalReturnedQty, value);
                        }
        }

        private bool _Gift= false;
        /// <summary>
        /// 赠品
        /// </summary>
        [AdvQueryAttribute(ColName = "Gift",ColDesc = "赠品")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Gift" ,IsNullable = false,ColumnDescription = "赠品" )]
        public bool Gift
        { 
            get{return _Gift;}
            set{
            SetProperty(ref _Gift, value);
                        }
        }

 

        private decimal? _SubtotalUntaxedAmount;
        /// <summary>
        /// 未税本位币
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalUntaxedAmount",ColDesc = "未税本位币")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalUntaxedAmount" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "未税本位币" )]
        public decimal? SubtotalUntaxedAmount
        { 
            get{return _SubtotalUntaxedAmount;}
            set{
            SetProperty(ref _SubtotalUntaxedAmount, value);
                        }
        }

        private decimal _UnitCommissionAmount= ((0));
        /// <summary>
        /// 单品佣金
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitCommissionAmount",ColDesc = "单品佣金")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UnitCommissionAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "单品佣金" )]
        public decimal UnitCommissionAmount
        { 
            get{return _UnitCommissionAmount;}
            set{
            SetProperty(ref _UnitCommissionAmount, value);
                        }
        }

        private decimal _CommissionAmount;
        /// <summary>
        /// 佣金小计
        /// </summary>
        [AdvQueryAttribute(ColName = "CommissionAmount",ColDesc = "佣金小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "CommissionAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "佣金小计" )]
        public decimal CommissionAmount
        { 
            get{return _CommissionAmount;}
            set{
            SetProperty(ref _CommissionAmount, value);
                        }
        }

        private decimal _SubtotalTaxAmount;
        /// <summary>
        /// 税额
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalTaxAmount",ColDesc = "税额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalTaxAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "税额" )]
        public decimal SubtotalTaxAmount
        { 
            get{return _SubtotalTaxAmount;}
            set{
            SetProperty(ref _SubtotalTaxAmount, value);
                        }
        }

        private string _SaleFlagCode;
        /// <summary>
        /// 标识代码
        /// </summary>
        [AdvQueryAttribute(ColName = "SaleFlagCode",ColDesc = "标识代码")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SaleFlagCode" ,Length=200,IsNullable = true,ColumnDescription = "标识代码" )]
        public string SaleFlagCode
        { 
            get{return _SaleFlagCode;}
            set{
            SetProperty(ref _SaleFlagCode, value);
                        }
        }

        private long? _SaleOrderDetail_ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "SaleOrderDetail_ID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "SaleOrderDetail_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        public long? SaleOrderDetail_ID
        { 
            get{return _SaleOrderDetail_ID;}
            set{
            SetProperty(ref _SaleOrderDetail_ID, value);
                        }
        }

        private decimal _AllocatedFreightIncome= ((0));
        /// <summary>
        /// 运费收入分摊
        /// </summary>
        [AdvQueryAttribute(ColName = "AllocatedFreightIncome",ColDesc = "运费收入分摊")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "AllocatedFreightIncome" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "运费收入分摊" )]
        public decimal AllocatedFreightIncome
        { 
            get{return _AllocatedFreightIncome;}
            set{
            SetProperty(ref _AllocatedFreightIncome, value);
                        }
        }

        private decimal _AllocatedFreightCost= ((0));
        /// <summary>
        /// 运费成本分摊
        /// </summary>
        [AdvQueryAttribute(ColName = "AllocatedFreightCost",ColDesc = "运费成本分摊")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "AllocatedFreightCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "运费成本分摊" )]
        public decimal AllocatedFreightCost
        { 
            get{return _AllocatedFreightCost;}
            set{
            SetProperty(ref _AllocatedFreightCost, value);
                        }
        }

        private int? _FreightAllocationRules;
        /// <summary>
        /// 分摊规则
        /// </summary>
        [AdvQueryAttribute(ColName = "FreightAllocationRules",ColDesc = "分摊规则")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "FreightAllocationRules" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "分摊规则" )]
        public int? FreightAllocationRules
        { 
            get{return _FreightAllocationRules;}
            set{
            SetProperty(ref _FreightAllocationRules, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(SaleOut_MainID))]
        public virtual tb_SaleOut tb_saleout { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Rack_ID))]
        public virtual tb_StorageRack tb_storagerack { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }



        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_SaleOutDetail loctype = (tb_SaleOutDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

