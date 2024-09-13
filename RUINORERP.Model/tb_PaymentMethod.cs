
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:01
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
    /// 付款方式 交易方式，后面扩展有关账期 账龄分析的字段
    /// </summary>
    [Serializable()]
    [Description("tb_PaymentMethod")]
    [SugarTable("tb_PaymentMethod")]
    public partial class tb_PaymentMethod: BaseEntity, ICloneable
    {
        public tb_PaymentMethod()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_PaymentMethod" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Paytype_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Paytype_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long Paytype_ID
        { 
            get{return _Paytype_ID;}
            set{
            base.PrimaryKeyID = _Paytype_ID;
            SetProperty(ref _Paytype_ID, value);
            }
        }

        private string _Paytype_Name;
        /// <summary>
        /// 付款方式
        /// </summary>
        [AdvQueryAttribute(ColName = "Paytype_Name",ColDesc = "付款方式")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "Paytype_Name" ,Length=50,IsNullable = true,ColumnDescription = "付款方式" )]
        public string Paytype_Name
        { 
            get{return _Paytype_Name;}
            set{
            SetProperty(ref _Paytype_Name, value);
            }
        }

        private string _Desc;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Desc",ColDesc = "描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Desc" ,Length=50,IsNullable = true,ColumnDescription = "描述" )]
        public string Desc
        { 
            get{return _Desc;}
            set{
            SetProperty(ref _Desc, value);
            }
        }

        private bool? _Cash;
        /// <summary>
        /// 现金
        /// </summary>
        [AdvQueryAttribute(ColName = "Cash",ColDesc = "现金")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Cash" ,IsNullable = true,ColumnDescription = "现金" )]
        public bool? Cash
        { 
            get{return _Cash;}
            set{
            SetProperty(ref _Cash, value);
            }
        }

        #endregion

        #region 扩展属性

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurOrder.Paytype_ID))]
        public virtual List<tb_PurOrder> tb_PurOrders { get; set; }
        //tb_PurOrder.Paytype_ID)
        //Paytype_ID.FK_PO_REF_PAYMEMETHOD)
        //tb_PaymentMethod.Paytype_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurEntryRe.Paytype_ID))]
        public virtual List<tb_PurEntryRe> tb_PurEntryRes { get; set; }
        //tb_PurEntryRe.Paytype_ID)
        //Paytype_ID.FK_PURENTRYRE_PAYMETHOD)
        //tb_PaymentMethod.Paytype_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurEntry.Paytype_ID))]
        public virtual List<tb_PurEntry> tb_PurEntries { get; set; }
        //tb_PurEntry.Paytype_ID)
        //Paytype_ID.FK_TB_PUREN_REF_TB_PAYMETHOD)
        //tb_PaymentMethod.Paytype_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOut.Paytype_ID))]
        public virtual List<tb_SaleOut> tb_SaleOuts { get; set; }
        //tb_SaleOut.Paytype_ID)
        //Paytype_ID.FK_SALEOUT_REF_PAYMENTMETHOD)
        //tb_PaymentMethod.Paytype_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOrder.Paytype_ID))]
        public virtual List<tb_SaleOrder> tb_SaleOrders { get; set; }
        //tb_SaleOrder.Paytype_ID)
        //Paytype_ID.FK_TB_SALEO_REFERENCE_TB_PAYME)
        //tb_PaymentMethod.Paytype_ID)

        //[Browsable(false)]
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
                    Type type = typeof(tb_PaymentMethod);
                    
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
            tb_PaymentMethod loctype = (tb_PaymentMethod)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

