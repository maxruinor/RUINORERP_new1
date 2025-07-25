
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:27:02
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
    /// 收付款记录明细表
    /// </summary>
    [Serializable()]
    [Description("收付款记录明细表")]
    [SugarTable("tb_FM_PaymentRecordDetail")]
    public partial class tb_FM_PaymentRecordDetail : BaseEntity, ICloneable
    {
        public tb_FM_PaymentRecordDetail()
        {

            if (!PK_FK_ID_Check())
            {
                throw new Exception("收付款记录明细表tb_FM_PaymentRecordDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long? _PaymentId;
        /// <summary>
        /// 付款单
        /// </summary>
        [AdvQueryAttribute(ColName = "PaymentId", ColDesc = "付款单")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "PaymentId", DecimalDigits = 0, IsNullable = true, ColumnDescription = "付款单")]
        [FKRelationAttribute("tb_FM_PaymentRecord", "PaymentId")]
        public long? PaymentId
        {
            get { return _PaymentId; }
            set
            {
                SetProperty(ref _PaymentId, value);
            }
        }

        private long _PaymentDetailId;
        /// <summary>
        /// 付款明细
        /// </summary>

        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "PaymentDetailId", DecimalDigits = 0, IsNullable = false, ColumnDescription = "付款明细", IsPrimaryKey = true)]
        public long PaymentDetailId
        {
            get { return _PaymentDetailId; }
            set
            {
                SetProperty(ref _PaymentDetailId, value);
                base.PrimaryKeyID = _PaymentDetailId;
            }
        }

        private int _SourceBizType;
        /// <summary>
        /// 来源业务
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBizType", ColDesc = "来源业务")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "SourceBizType", DecimalDigits = 0, IsNullable = false, ColumnDescription = "来源业务")]
        public int SourceBizType
        {
            get { return _SourceBizType; }
            set
            {
                SetProperty(ref _SourceBizType, value);
            }
        }

        private long _SourceBilllId;
        /// <summary>
        /// 来源单据
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBilllId", ColDesc = "来源单据")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "SourceBilllId", DecimalDigits = 0, IsNullable = false, ColumnDescription = "来源单据")]
        public long SourceBilllId
        {
            get { return _SourceBilllId; }
            set
            {
                SetProperty(ref _SourceBilllId, value);
            }
        }

        private string _SourceBillNo;
        /// <summary>
        /// 来源单号
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBillNo", ColDesc = "来源单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "SourceBillNo", Length = 30, IsNullable = false, ColumnDescription = "来源单号")]
        public string SourceBillNo
        {
            get { return _SourceBillNo; }
            set
            {
                SetProperty(ref _SourceBillNo, value);
            }
        }

        private bool? _IsFromPlatform;
        /// <summary>
        /// 平台单
        /// </summary>
        [AdvQueryAttribute(ColName = "IsFromPlatform", ColDesc = "平台单")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "IsFromPlatform", IsNullable = true, ColumnDescription = "平台单")]
        public bool? IsFromPlatform
        {
            get { return _IsFromPlatform; }
            set
            {
                SetProperty(ref _IsFromPlatform, value);
            }
        }

        private long? _DepartmentID;
        /// <summary>
        /// 部门
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentID", ColDesc = "部门")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "DepartmentID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "部门")]
        [FKRelationAttribute("tb_Department", "DepartmentID")]
        public long? DepartmentID
        {
            get { return _DepartmentID; }
            set
            {
                SetProperty(ref _DepartmentID, value);
            }
        }

        private long? _ProjectGroup_ID;
        /// <summary>
        /// 项目组
        /// </summary>
        [AdvQueryAttribute(ColName = "ProjectGroup_ID", ColDesc = "项目组")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "ProjectGroup_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "项目组")]
        [FKRelationAttribute("tb_ProjectGroup", "ProjectGroup_ID")]
        public long? ProjectGroup_ID
        {
            get { return _ProjectGroup_ID; }
            set
            {
                SetProperty(ref _ProjectGroup_ID, value);
            }
        }

        private long _Currency_ID;
        /// <summary>
        /// 币别
        /// </summary>
        [AdvQueryAttribute(ColName = "Currency_ID", ColDesc = "币别")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Currency_ID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "币别")]
        [FKRelationAttribute("tb_Currency", "Currency_ID")]
        public long Currency_ID
        {
            get { return _Currency_ID; }
            set
            {
                SetProperty(ref _Currency_ID, value);
            }
        }

        private decimal _ExchangeRate = ((1));
        /// <summary>
        /// 汇率
        /// </summary>
        [AdvQueryAttribute(ColName = "ExchangeRate", ColDesc = "汇率")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType = "Decimal", ColumnName = "ExchangeRate", DecimalDigits = 4, IsNullable = false, ColumnDescription = "汇率")]
        public decimal ExchangeRate
        {
            get { return _ExchangeRate; }
            set
            {
                SetProperty(ref _ExchangeRate, value);
            }
        }

        private decimal _ForeignAmount = ((0));
        /// <summary>
        /// 支付金额外币
        /// </summary>
        [AdvQueryAttribute(ColName = "ForeignAmount", ColDesc = "支付金额外币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "ForeignAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "支付金额外币")]
        public decimal ForeignAmount
        {
            get { return _ForeignAmount; }
            set
            {
                SetProperty(ref _ForeignAmount, value);
            }
        }

        private decimal _LocalAmount = ((0));
        /// <summary>
        /// 支付金额本币
        /// </summary>
        [AdvQueryAttribute(ColName = "LocalAmount", ColDesc = "支付金额本币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "LocalAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "支付金额本币")]
        public decimal LocalAmount
        {
            get { return _LocalAmount; }
            set
            {
                SetProperty(ref _LocalAmount, value);
            }
        }

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary", ColDesc = "摘要")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "Summary", Length = 300, IsNullable = true, ColumnDescription = "摘要")]
        public string Summary
        {
            get { return _Summary; }
            set
            {
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
        [Navigate(NavigateType.OneToOne, nameof(ProjectGroup_ID))]
        public virtual tb_ProjectGroup tb_projectgroup { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(PaymentId))]
        public virtual tb_FM_PaymentRecord tb_fm_paymentrecord { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(DepartmentID))]
        public virtual tb_Department tb_department { get; set; }



        #endregion




        //如果为false,则不可以。
        private bool PK_FK_ID_Check()
        {
            bool rs = true;
            return rs;
        }









        public override object Clone()
        {
            tb_FM_PaymentRecordDetail loctype = (tb_FM_PaymentRecordDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

