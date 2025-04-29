
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/29/2025 11:22:31
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
    /// 应收应付明细
    /// </summary>
    [Serializable()]
    [Description("应收应付明细")]
    [SugarTable("tb_FM_ReceivablePayableDetail")]
    public partial class tb_FM_ReceivablePayableDetail: BaseEntity, ICloneable
    {
        public tb_FM_ReceivablePayableDetail()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("应收应付明细tb_FM_ReceivablePayableDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long? _ARAPId;
        /// <summary>
        /// 应收付款单
        /// </summary>
        [AdvQueryAttribute(ColName = "ARAPId",ColDesc = "应收付款单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ARAPId" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "应收付款单" )]
        [FKRelationAttribute("tb_FM_ReceivablePayable","ARAPId")]
        public long? ARAPId
        { 
            get{return _ARAPId;}
            set{
            SetProperty(ref _ARAPId, value);
                        }
        }

        private long _ARAPDetailID;
        /// <summary>
        /// 应收付明细
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ARAPDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "应收付明细" , IsPrimaryKey = true)]
        public long ARAPDetailID
        { 
            get{return _ARAPDetailID;}
            set{
            SetProperty(ref _ARAPDetailID, value);
                base.PrimaryKeyID = _ARAPDetailID;
            }
        }

        private int? _BizType;
        /// <summary>
        /// 来源业务
        /// </summary>
        [AdvQueryAttribute(ColName = "BizType",ColDesc = "来源业务")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "BizType" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "来源业务" )]
        public int? BizType
        { 
            get{return _BizType;}
            set{
            SetProperty(ref _BizType, value);
                        }
        }

        private long? _SourceBill_ID;
        /// <summary>
        /// 来源单据
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBill_ID",ColDesc = "来源单据")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "SourceBill_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "来源单据" )]
        public long? SourceBill_ID
        { 
            get{return _SourceBill_ID;}
            set{
            SetProperty(ref _SourceBill_ID, value);
                        }
        }

        private string _SourceBillNO;
        /// <summary>
        /// 来源单号
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBillNO",ColDesc = "来源单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SourceBillNO" ,Length=30,IsNullable = true,ColumnDescription = "来源单号" )]
        public string SourceBillNO
        { 
            get{return _SourceBillNO;}
            set{
            SetProperty(ref _SourceBillNO, value);
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

        private bool _IncludeTax= false;
        /// <summary>
        /// 含税
        /// </summary>
        [AdvQueryAttribute(ColName = "IncludeTax",ColDesc = "含税")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IncludeTax" ,IsNullable = false,ColumnDescription = "含税" )]
        public bool IncludeTax
        { 
            get{return _IncludeTax;}
            set{
            SetProperty(ref _IncludeTax, value);
                        }
        }

        private decimal? _ExchangeRate;
        /// <summary>
        /// 汇率
        /// </summary>
        [AdvQueryAttribute(ColName = "ExchangeRate",ColDesc = "汇率")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "ExchangeRate" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "汇率" )]
        public decimal? ExchangeRate
        { 
            get{return _ExchangeRate;}
            set{
            SetProperty(ref _ExchangeRate, value);
                        }
        }

        private decimal? _UnitPrice;
        /// <summary>
        /// 单价
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitPrice",ColDesc = "单价")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UnitPrice" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "单价" )]
        public decimal? UnitPrice
        { 
            get{return _UnitPrice;}
            set{
            SetProperty(ref _UnitPrice, value);
                        }
        }

        private decimal? _Quantity;
        /// <summary>
        /// 数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "数量")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Quantity" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "数量" )]
        public decimal? Quantity
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

        private decimal _TaxLocalAmount= ((0));
        /// <summary>
        /// 税额
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxLocalAmount",ColDesc = "税额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TaxLocalAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "税额" )]
        public decimal TaxLocalAmount
        { 
            get{return _TaxLocalAmount;}
            set{
            SetProperty(ref _TaxLocalAmount, value);
                        }
        }

        private decimal _LocalPayableAmount= ((0));
        /// <summary>
        /// 金额小计
        /// </summary>
        [AdvQueryAttribute(ColName = "LocalPayableAmount",ColDesc = "金额小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "LocalPayableAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "金额小计" )]
        public decimal LocalPayableAmount
        { 
            get{return _LocalPayableAmount;}
            set{
            SetProperty(ref _LocalPayableAmount, value);
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
        [Navigate(NavigateType.OneToOne, nameof(ARAPId))]
        public virtual tb_FM_ReceivablePayable tb_fm_receivablepayable { get; set; }



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
                    Type type = typeof(tb_FM_ReceivablePayableDetail);
                    
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
            tb_FM_ReceivablePayableDetail loctype = (tb_FM_ReceivablePayableDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

