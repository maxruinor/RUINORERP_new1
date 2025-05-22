
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:09
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
    /// 付款方式 付款方式，后面扩展有关账期 账龄分析的字段
    /// </summary>
    [Serializable()]
    [Description("付款方式 付款方式，后面扩展有关账期 账龄分析的字段")]
    [SugarTable("tb_PaymentMethod")]
    public partial class tb_PaymentMethod : BaseEntity, ICloneable
    {
        public tb_PaymentMethod()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("付款方式 付款方式，后面扩展有关账期 账龄分析的字段tb_PaymentMethod" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Paytype_ID;
        /// <summary>
        /// 
        /// </summary>

        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Paytype_ID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "", IsPrimaryKey = true)]
        public long Paytype_ID
        {
            get { return _Paytype_ID; }
            set
            {
                SetProperty(ref _Paytype_ID, value);
                base.PrimaryKeyID = _Paytype_ID;
            }
        }

        private string _Paytype_Name;
        /// <summary>
        /// 付款方式
        /// </summary>
        [AdvQueryAttribute(ColName = "Paytype_Name", ColDesc = "付款方式")]
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType = "String", ColumnName = "Paytype_Name", Length = 50, IsNullable = true, ColumnDescription = "付款方式")]
        public string Paytype_Name
        {
            get { return _Paytype_Name; }
            set
            {
                SetProperty(ref _Paytype_Name, value);
            }
        }

        private string _Desc;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Desc", ColDesc = "描述")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "Desc", Length = 50, IsNullable = true, ColumnDescription = "描述")]
        public string Desc
        {
            get { return _Desc; }
            set
            {
                SetProperty(ref _Desc, value);
            }
        }

        private int _Sort = 0;
        /// <summary>
        /// 排序
        /// </summary>
        [AdvQueryAttribute(ColName = "Sort", ColDesc = "排序")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "Sort", DecimalDigits = 0, IsNullable = false, ColumnDescription = "排序")]
        public int Sort
        {
            get { return _Sort; }
            set
            {
                SetProperty(ref _Sort, value);
            }
        }

        private bool _Cash = false;
        /// <summary>
        /// 即时收付款
        /// </summary>
        [AdvQueryAttribute(ColName = "Cash", ColDesc = "即时收付款")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "Cash", IsNullable = false, ColumnDescription = "即时收付款")]
        public bool Cash
        {
            get { return _Cash; }
            set
            {
                SetProperty(ref _Cash, value);
            }
        }

        #endregion

        #region 扩展属性

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurOrder.Paytype_ID))]
        public virtual List<tb_PurOrder> tb_PurOrders { get; set; }
        //tb_PurOrder.Paytype_ID)
        //Paytype_ID.FK_PO_REF_PAYMEMETHOD)
        //tb_PaymentMethod.Paytype_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOut.Paytype_ID))]
        public virtual List<tb_SaleOut> tb_SaleOuts { get; set; }
        //tb_SaleOut.Paytype_ID)
        //Paytype_ID.FK_SALEOUT_REF_PAYMENTMETHOD)
        //tb_PaymentMethod.Paytype_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurEntry.Paytype_ID))]
        public virtual List<tb_PurEntry> tb_PurEntries { get; set; }
        //tb_PurEntry.Paytype_ID)
        //Paytype_ID.FK_TB_PUREN_REF_TB_PAYMETHOD)
        //tb_PaymentMethod.Paytype_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurReturnEntry.Paytype_ID))]
        public virtual List<tb_PurReturnEntry> tb_PurReturnEntries { get; set; }
        //tb_PurReturnEntry.Paytype_ID)
        //Paytype_ID.FK_PURRETURNENTRY_REE_PAYMENTMETHOD)
        //tb_PaymentMethod.Paytype_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOrder.Paytype_ID))]
        public virtual List<tb_SaleOrder> tb_SaleOrders { get; set; }
        //tb_SaleOrder.Paytype_ID)
        //Paytype_ID.FK_TB_SALEO_REFERENCE_TB_PAYME)
        //tb_PaymentMethod.Paytype_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOutRe.Paytype_ID))]
        public virtual List<tb_SaleOutRe> tb_SaleOutRes { get; set; }
        //tb_SaleOutRe.Paytype_ID)
        //Paytype_ID.FK_SALEOUTRE_REF_PAYMENTMETHOD)
        //tb_PaymentMethod.Paytype_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurEntryRe.Paytype_ID))]
        public virtual List<tb_PurEntryRe> tb_PurEntryRes { get; set; }
        //tb_PurEntryRe.Paytype_ID)
        //Paytype_ID.FK_PURENTRYRE_PAYMETHOD)
        //tb_PaymentMethod.Paytype_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_CustomerVendor.Paytype_ID))]
        public virtual List<tb_CustomerVendor> tb_CustomerVendors { get; set; }
        //tb_CustomerVendor.Paytype_ID)
        //Paytype_ID.FK_TB_CUSTOVENDOR_RE_PAYMETHOD)
        //tb_PaymentMethod.Paytype_ID)


        #endregion




        //如果为false,则不可以。
        private bool PK_FK_ID_Check()
        {
            bool rs = true;
            return rs;
        }





        public override object Clone()
        {
            tb_PaymentMethod loctype = (tb_PaymentMethod)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

