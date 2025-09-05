
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/04/2025 18:02:23
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
    /// 对账单明细
    /// </summary>
    [Serializable()]
    [Description("对账单明细")]
    [SugarTable("tb_FM_StatementDetail")]
    public partial class tb_FM_StatementDetail: BaseEntity, ICloneable
    {
        public tb_FM_StatementDetail()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("对账单明细tb_FM_StatementDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _StatementDetailId;
        /// <summary>
        /// 对账明细
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "StatementDetailId" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "对账明细" , IsPrimaryKey = true)]
        public long StatementDetailId
        { 
            get{return _StatementDetailId;}
            set{
            SetProperty(ref _StatementDetailId, value);
                base.PrimaryKeyID = _StatementDetailId;
            }
        }

        private long _StatementId;
        /// <summary>
        /// 对账单
        /// </summary>
        [AdvQueryAttribute(ColName = "StatementId",ColDesc = "对账单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "StatementId" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "对账单" )]
        [FKRelationAttribute("tb_FM_Statement","StatementId")]
        public long StatementId
        { 
            get{return _StatementId;}
            set{
            SetProperty(ref _StatementId, value);
                        }
        }

        private long _ARAPId;
        /// <summary>
        /// 应收付款单
        /// </summary>
        [AdvQueryAttribute(ColName = "ARAPId",ColDesc = "应收付款单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ARAPId" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "应收付款单" )]
        [FKRelationAttribute("tb_FM_ReceivablePayable","ARAPId")]
        public long ARAPId
        { 
            get{return _ARAPId;}
            set{
            SetProperty(ref _ARAPId, value);
                        }
        }

        private int _ReceivePaymentType;
        /// <summary>
        /// 收付类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ReceivePaymentType",ColDesc = "收付类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ReceivePaymentType" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "收付类型" )]
        public int ReceivePaymentType
        { 
            get{return _ReceivePaymentType;}
            set{
            SetProperty(ref _ReceivePaymentType, value);
                        }
        }

        private DateTime? _DocumentDate;
        /// <summary>
        /// 单据日期
        /// </summary>
        [AdvQueryAttribute(ColName = "DocumentDate",ColDesc = "单据日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "DocumentDate" ,IsNullable = true,ColumnDescription = "单据日期" )]
        public DateTime? DocumentDate
        { 
            get{return _DocumentDate;}
            set{
            SetProperty(ref _DocumentDate, value);
                        }
        }

        private DateTime? _BusinessDate;
        /// <summary>
        /// 业务日期
        /// </summary>
        [AdvQueryAttribute(ColName = "BusinessDate",ColDesc = "业务日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "BusinessDate" ,IsNullable = true,ColumnDescription = "业务日期" )]
        public DateTime? BusinessDate
        { 
            get{return _BusinessDate;}
            set{
            SetProperty(ref _BusinessDate, value);
                        }
        }

        private long _Currency_ID;
        /// <summary>
        /// 币别
        /// </summary>
        [AdvQueryAttribute(ColName = "Currency_ID",ColDesc = "币别")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Currency_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "币别" )]
        [FKRelationAttribute("tb_Currency","Currency_ID")]
        public long Currency_ID
        { 
            get{return _Currency_ID;}
            set{
            SetProperty(ref _Currency_ID, value);
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

        private decimal _IncludedLocalAmount= ((0));
        /// <summary>
        /// 对账金额本币
        /// </summary>
        [AdvQueryAttribute(ColName = "IncludedLocalAmount",ColDesc = "对账金额本币")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "IncludedLocalAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "对账金额本币" )]
        public decimal IncludedLocalAmount
        { 
            get{return _IncludedLocalAmount;}
            set{
            SetProperty(ref _IncludedLocalAmount, value);
                        }
        }

        private decimal _IncludedForeignAmount= ((0));
        /// <summary>
        /// 对账金额外币
        /// </summary>
        [AdvQueryAttribute(ColName = "IncludedForeignAmount",ColDesc = "对账金额外币")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "IncludedForeignAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "对账金额外币" )]
        public decimal IncludedForeignAmount
        { 
            get{return _IncludedForeignAmount;}
            set{
            SetProperty(ref _IncludedForeignAmount, value);
                        }
        }

        private decimal _WrittenOffLocalAmount= ((0));
        /// <summary>
        /// 本次已核销本币金额
        /// </summary>
        [AdvQueryAttribute(ColName = "WrittenOffLocalAmount",ColDesc = "本次已核销本币金额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "WrittenOffLocalAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "本次已核销本币金额" )]
        public decimal WrittenOffLocalAmount
        { 
            get{return _WrittenOffLocalAmount;}
            set{
            SetProperty(ref _WrittenOffLocalAmount, value);
                        }
        }

        private decimal _WrittenOffForeignAmount= ((0));
        /// <summary>
        /// 本次已核销原币金额
        /// </summary>
        [AdvQueryAttribute(ColName = "WrittenOffForeignAmount",ColDesc = "本次已核销原币金额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "WrittenOffForeignAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "本次已核销原币金额" )]
        public decimal WrittenOffForeignAmount
        { 
            get{return _WrittenOffForeignAmount;}
            set{
            SetProperty(ref _WrittenOffForeignAmount, value);
                        }
        }

        private decimal _RemainingLocalAmount= ((0));
        /// <summary>
        /// 剩余未核销本币金额
        /// </summary>
        [AdvQueryAttribute(ColName = "RemainingLocalAmount",ColDesc = "剩余未核销本币金额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "RemainingLocalAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "剩余未核销本币金额" )]
        public decimal RemainingLocalAmount
        { 
            get{return _RemainingLocalAmount;}
            set{
            SetProperty(ref _RemainingLocalAmount, value);
                        }
        }

        private decimal _RemainingForeignAmount= ((0));
        /// <summary>
        /// 剩余未核销原币金额
        /// </summary>
        [AdvQueryAttribute(ColName = "RemainingForeignAmount",ColDesc = "剩余未核销原币金额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "RemainingForeignAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "剩余未核销原币金额" )]
        public decimal RemainingForeignAmount
        { 
            get{return _RemainingForeignAmount;}
            set{
            SetProperty(ref _RemainingForeignAmount, value);
                        }
        }

        private int _ARAPWriteOffStatus;
        /// <summary>
        /// 核销状态
        /// </summary>
        [AdvQueryAttribute(ColName = "ARAPWriteOffStatus",ColDesc = "核销状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ARAPWriteOffStatus" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "核销状态" )]
        public int ARAPWriteOffStatus
        { 
            get{return _ARAPWriteOffStatus;}
            set{
            SetProperty(ref _ARAPWriteOffStatus, value);
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
        [Navigate(NavigateType.OneToOne, nameof(Currency_ID))]
        public virtual tb_Currency tb_currency { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ARAPId))]
        public virtual tb_FM_ReceivablePayable tb_fm_receivablepayable { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(StatementId))]
        public virtual tb_FM_Statement tb_fm_statement { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_FM_StatementDetail loctype = (tb_FM_StatementDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

