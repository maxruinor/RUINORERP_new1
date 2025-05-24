
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/24/2025 18:28:29
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
    /// 价格调整单明细
    /// </summary>
    [Serializable()]
    [Description("价格调整单明细")]
    [SugarTable("tb_FM_PriceAdjustmentDetail")]
    public partial class tb_FM_PriceAdjustmentDetail: BaseEntity, ICloneable
    {
        public tb_FM_PriceAdjustmentDetail()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("价格调整单明细tb_FM_PriceAdjustmentDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _AdjustDetailID;
        /// <summary>
        /// 价格调整明细
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "AdjustDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "价格调整明细" , IsPrimaryKey = true)]
        public long AdjustDetailID
        { 
            get{return _AdjustDetailID;}
            set{
            SetProperty(ref _AdjustDetailID, value);
                base.PrimaryKeyID = _AdjustDetailID;
            }
        }

        private long? _AdjustId;
        /// <summary>
        /// 价格调整单
        /// </summary>
        [AdvQueryAttribute(ColName = "AdjustId",ColDesc = "价格调整单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "AdjustId" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "价格调整单" )]
        [FKRelationAttribute("tb_FM_PriceAdjustment","AdjustId")]
        public long? AdjustId
        { 
            get{return _AdjustId;}
            set{
            SetProperty(ref _AdjustId, value);
                        }
        }

        private long? _ProdDetailID;
        /// <summary>
        /// 产品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "产品" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long? ProdDetailID
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

        private string _Specifications;
        /// <summary>
        /// 规格
        /// </summary>
        [AdvQueryAttribute(ColName = "Specifications",ColDesc = "规格")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Specifications" ,Length=1000,IsNullable = true,ColumnDescription = "规格" )]
        public string Specifications
        { 
            get{return _Specifications;}
            set{
            SetProperty(ref _Specifications, value);
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

        private decimal _ExchangeRate= ((1));
        /// <summary>
        /// 汇率
        /// </summary>
        [AdvQueryAttribute(ColName = "ExchangeRate",ColDesc = "汇率")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "ExchangeRate" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "汇率" )]
        public decimal ExchangeRate
        { 
            get{return _ExchangeRate;}
            set{
            SetProperty(ref _ExchangeRate, value);
                        }
        }

        private decimal _OriginalUnitPrice= ((0));
        /// <summary>
        /// 原始单价
        /// </summary>
        [AdvQueryAttribute(ColName = "OriginalUnitPrice",ColDesc = "原始单价")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "OriginalUnitPrice" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "原始单价" )]
        public decimal OriginalUnitPrice
        { 
            get{return _OriginalUnitPrice;}
            set{
            SetProperty(ref _OriginalUnitPrice, value);
                        }
        }

        private decimal _AdjustedUnitPrice= ((0));
        /// <summary>
        /// 调整后单价
        /// </summary>
        [AdvQueryAttribute(ColName = "AdjustedUnitPrice",ColDesc = "调整后单价")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "AdjustedUnitPrice" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "调整后单价" )]
        public decimal AdjustedUnitPrice
        { 
            get{return _AdjustedUnitPrice;}
            set{
            SetProperty(ref _AdjustedUnitPrice, value);
                        }
        }

        private decimal _DiffUnitPrice= ((0));
        /// <summary>
        /// 差异单价
        /// </summary>
        [AdvQueryAttribute(ColName = "DiffUnitPrice",ColDesc = "差异单价")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "DiffUnitPrice" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "差异单价" )]
        public decimal DiffUnitPrice
        { 
            get{return _DiffUnitPrice;}
            set{
            SetProperty(ref _DiffUnitPrice, value);
                        }
        }

        private decimal _Quantity= ((0));
        /// <summary>
        /// 数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "数量")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Quantity" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "数量" )]
        public decimal Quantity
        { 
            get{return _Quantity;}
            set{
            SetProperty(ref _Quantity, value);
                        }
        }

        private string _CustomerPartNo;
        /// <summary>
        /// 往来单位料号
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerPartNo",ColDesc = "往来单位料号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomerPartNo" ,Length=100,IsNullable = true,ColumnDescription = "往来单位料号" )]
        public string CustomerPartNo
        { 
            get{return _CustomerPartNo;}
            set{
            SetProperty(ref _CustomerPartNo, value);
                        }
        }

        private decimal _SubtotalDiffLocalAmount= ((0));
        /// <summary>
        /// 差异金额小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalDiffLocalAmount",ColDesc = "差异金额小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalDiffLocalAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "差异金额小计" )]
        public decimal SubtotalDiffLocalAmount
        { 
            get{return _SubtotalDiffLocalAmount;}
            set{
            SetProperty(ref _SubtotalDiffLocalAmount, value);
                        }
        }

        private string _Description;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Description",ColDesc = "描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Description" ,Length=300,IsNullable = true,ColumnDescription = "描述" )]
        public string Description
        { 
            get{return _Description;}
            set{
            SetProperty(ref _Description, value);
                        }
        }

        private decimal _TaxRate= ((0));
        /// <summary>
        /// 税率
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxRate",ColDesc = "税率")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "TaxRate" , DecimalDigits = 2,IsNullable = false,ColumnDescription = "税率" )]
        public decimal TaxRate
        { 
            get{return _TaxRate;}
            set{
            SetProperty(ref _TaxRate, value);
                        }
        }

        private decimal _TaxDiffLocalAmount= ((0));
        /// <summary>
        /// 税额差异
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxDiffLocalAmount",ColDesc = "税额差异")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TaxDiffLocalAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "税额差异" )]
        public decimal TaxDiffLocalAmount
        { 
            get{return _TaxDiffLocalAmount;}
            set{
            SetProperty(ref _TaxDiffLocalAmount, value);
                        }
        }

        private decimal _TaxSubtotalDiffLocalAmount= ((0));
        /// <summary>
        /// 税额差异小计
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxSubtotalDiffLocalAmount",ColDesc = "税额差异小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TaxSubtotalDiffLocalAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "税额差异小计" )]
        public decimal TaxSubtotalDiffLocalAmount
        { 
            get{return _TaxSubtotalDiffLocalAmount;}
            set{
            SetProperty(ref _TaxSubtotalDiffLocalAmount, value);
                        }
        }

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=300,IsNullable = true,ColumnDescription = "摘要" )]
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
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Unit_ID))]
        public virtual tb_Unit tb_unit { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(AdjustId))]
        public virtual tb_FM_PriceAdjustment tb_fm_priceadjustment { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_FM_PriceAdjustmentDetail loctype = (tb_FM_PriceAdjustmentDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

