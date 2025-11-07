
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:41:52
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
    /// 损益费用单
    /// </summary>
    [Serializable()]
    [Description("损益费用单")]
    [SugarTable("tb_FM_ProfitLossDetail")]
    public partial class tb_FM_ProfitLossDetail: BaseEntity, ICloneable
    {
        public tb_FM_ProfitLossDetail()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("损益费用单tb_FM_ProfitLossDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ProfitLossDetail_ID;
        /// <summary>
        /// 损益费用单明细
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProfitLossDetail_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "损益费用单明细" , IsPrimaryKey = true)]
        public long ProfitLossDetail_ID
        { 
            get{return _ProfitLossDetail_ID;}
            set{
            SetProperty(ref _ProfitLossDetail_ID, value);
                base.PrimaryKeyID = _ProfitLossDetail_ID;
            }
        }

        private long? _ProfitLossId;
        /// <summary>
        /// 损益费用单
        /// </summary>
        [AdvQueryAttribute(ColName = "ProfitLossId",ColDesc = "损益费用单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProfitLossId" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "损益费用单" )]
        [FKRelationAttribute("tb_FM_ProfitLoss","ProfitLossId")]
        public long? ProfitLossId
        { 
            get{return _ProfitLossId;}
            set{
            SetProperty(ref _ProfitLossId, value);
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

        private int _ProfitLossType;
        /// <summary>
        /// 损溢类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ProfitLossType",ColDesc = "损溢类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ProfitLossType" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "损溢类型" )]
        public int ProfitLossType
        { 
            get{return _ProfitLossType;}
            set{
            SetProperty(ref _ProfitLossType, value);
                        }
        }

        private int _IncomeExpenseDirection;
        /// <summary>
        /// 收支方向
        /// </summary>
        [AdvQueryAttribute(ColName = "IncomeExpenseDirection",ColDesc = "收支方向")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "IncomeExpenseDirection" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "收支方向" )]
        public int IncomeExpenseDirection
        { 
            get{return _IncomeExpenseDirection;}
            set{
            SetProperty(ref _IncomeExpenseDirection, value);
                        }
        }

        private string _ExpenseDescription;
        /// <summary>
        /// 费用说明
        /// </summary>
        [AdvQueryAttribute(ColName = "ExpenseDescription",ColDesc = "费用说明")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ExpenseDescription" ,Length=300,IsNullable = true,ColumnDescription = "费用说明" )]
        public string ExpenseDescription
        { 
            get{return _ExpenseDescription;}
            set{
            SetProperty(ref _ExpenseDescription, value);
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

        private decimal _SubtotalAmont= ((0));
        /// <summary>
        /// 金额小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalAmont",ColDesc = "金额小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalAmont" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "金额小计" )]
        public decimal SubtotalAmont
        { 
            get{return _SubtotalAmont;}
            set{
            SetProperty(ref _SubtotalAmont, value);
                        }
        }

        private decimal _UntaxedSubtotalAmont= ((0));
        /// <summary>
        /// 未税小计
        /// </summary>
        [AdvQueryAttribute(ColName = "UntaxedSubtotalAmont",ColDesc = "未税小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UntaxedSubtotalAmont" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "未税小计" )]
        public decimal UntaxedSubtotalAmont
        { 
            get{return _UntaxedSubtotalAmont;}
            set{
            SetProperty(ref _UntaxedSubtotalAmont, value);
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

        private decimal _TaxSubtotalAmont= ((0));
        /// <summary>
        /// 税额
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxSubtotalAmont",ColDesc = "税额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TaxSubtotalAmont" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "税额" )]
        public decimal TaxSubtotalAmont
        { 
            get{return _TaxSubtotalAmont;}
            set{
            SetProperty(ref _TaxSubtotalAmont, value);
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
        [Navigate(NavigateType.OneToOne, nameof(ProfitLossId))]
        public virtual tb_FM_ProfitLoss tb_fm_profitloss { get; set; }

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
            tb_FM_ProfitLossDetail loctype = (tb_FM_ProfitLossDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

