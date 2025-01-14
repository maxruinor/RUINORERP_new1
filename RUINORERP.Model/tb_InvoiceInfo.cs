
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/14/2025 18:56:55
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
    /// 开票资料
    /// </summary>
    [Serializable()]
    [Description("开票资料")]
    [SugarTable("tb_InvoiceInfo")]
    public partial class tb_InvoiceInfo: BaseEntity, ICloneable
    {
        public tb_InvoiceInfo()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("开票资料tb_InvoiceInfo" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _InvoiceInfo_ID;
        /// <summary>
        /// 开票信息
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "InvoiceInfo_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "开票信息" , IsPrimaryKey = true)]
        public long InvoiceInfo_ID
        { 
            get{return _InvoiceInfo_ID;}
            set{
            base.PrimaryKeyID = _InvoiceInfo_ID;
            SetProperty(ref _InvoiceInfo_ID, value);
            }
        }

        private long? _CustomerVendor_ID;
        /// <summary>
        /// 往来单位
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "往来单位")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "往来单位" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long? CustomerVendor_ID
        { 
            get{return _CustomerVendor_ID;}
            set{
            SetProperty(ref _CustomerVendor_ID, value);
            }
        }

        private string _PICompanyName;
        /// <summary>
        /// 公司名称
        /// </summary>
        [AdvQueryAttribute(ColName = "PICompanyName",ColDesc = "公司名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PICompanyName" ,Length=200,IsNullable = true,ColumnDescription = "公司名称" )]
        public string PICompanyName
        { 
            get{return _PICompanyName;}
            set{
            SetProperty(ref _PICompanyName, value);
            }
        }

        private string _PITaxID;
        /// <summary>
        /// 税号
        /// </summary>
        [AdvQueryAttribute(ColName = "PITaxID",ColDesc = "税号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PITaxID" ,Length=100,IsNullable = true,ColumnDescription = "税号" )]
        public string PITaxID
        { 
            get{return _PITaxID;}
            set{
            SetProperty(ref _PITaxID, value);
            }
        }

        private string _PIAddress;
        /// <summary>
        /// 地址
        /// </summary>
        [AdvQueryAttribute(ColName = "PIAddress",ColDesc = "地址")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PIAddress" ,Length=200,IsNullable = true,ColumnDescription = "地址" )]
        public string PIAddress
        { 
            get{return _PIAddress;}
            set{
            SetProperty(ref _PIAddress, value);
            }
        }

        private string _PITEL;
        /// <summary>
        /// 电话
        /// </summary>
        [AdvQueryAttribute(ColName = "PITEL",ColDesc = "电话")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PITEL" ,Length=50,IsNullable = true,ColumnDescription = "电话" )]
        public string PITEL
        { 
            get{return _PITEL;}
            set{
            SetProperty(ref _PITEL, value);
            }
        }

        private string _PIBankName;
        /// <summary>
        /// 开户行
        /// </summary>
        [AdvQueryAttribute(ColName = "PIBankName",ColDesc = "开户行")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PIBankName" ,Length=150,IsNullable = true,ColumnDescription = "开户行" )]
        public string PIBankName
        { 
            get{return _PIBankName;}
            set{
            SetProperty(ref _PIBankName, value);
            }
        }

        private string _PIBankNo;
        /// <summary>
        /// 银行帐号
        /// </summary>
        [AdvQueryAttribute(ColName = "PIBankNo",ColDesc = "银行帐号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PIBankNo" ,Length=50,IsNullable = true,ColumnDescription = "银行帐号" )]
        public string PIBankNo
        { 
            get{return _PIBankNo;}
            set{
            SetProperty(ref _PIBankNo, value);
            }
        }

        private string _Notes;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=255,IsNullable = true,ColumnDescription = "" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
            }
        }

        private string _信用天数;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "信用天数",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "char", SqlParameterDbType ="String",  ColumnName = "信用天数" ,Length=10,IsNullable = true,ColumnDescription = "" )]
        public string 信用天数
        { 
            get{return _信用天数;}
            set{
            SetProperty(ref _信用天数, value);
            }
        }

        private string _往来余额;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "往来余额",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "char", SqlParameterDbType ="String",  ColumnName = "往来余额" ,Length=10,IsNullable = true,ColumnDescription = "" )]
        public string 往来余额
        { 
            get{return _往来余额;}
            set{
            SetProperty(ref _往来余额, value);
            }
        }

        private string _应收款;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "应收款",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "char", SqlParameterDbType ="String",  ColumnName = "应收款" ,Length=10,IsNullable = true,ColumnDescription = "" )]
        public string 应收款
        { 
            get{return _应收款;}
            set{
            SetProperty(ref _应收款, value);
            }
        }

        private string _预收款;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "预收款",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "char", SqlParameterDbType ="String",  ColumnName = "预收款" ,Length=10,IsNullable = true,ColumnDescription = "" )]
        public string 预收款
        { 
            get{return _预收款;}
            set{
            SetProperty(ref _预收款, value);
            }
        }

        private string _纳税号;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "纳税号",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "char", SqlParameterDbType ="String",  ColumnName = "纳税号" ,Length=10,IsNullable = true,ColumnDescription = "" )]
        public string 纳税号
        { 
            get{return _纳税号;}
            set{
            SetProperty(ref _纳税号, value);
            }
        }

        private string _开户行;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "开户行",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "char", SqlParameterDbType ="String",  ColumnName = "开户行" ,Length=10,IsNullable = true,ColumnDescription = "" )]
        public string 开户行
        { 
            get{return _开户行;}
            set{
            SetProperty(ref _开户行, value);
            }
        }

        private string _银行帐号;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "银行帐号",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "char", SqlParameterDbType ="String",  ColumnName = "银行帐号" ,Length=10,IsNullable = true,ColumnDescription = "" )]
        public string 银行帐号
        { 
            get{return _银行帐号;}
            set{
            SetProperty(ref _银行帐号, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(CustomerVendor_ID))]
        public virtual tb_CustomerVendor tb_customervendor { get; set; }


        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Contract.InvoiceInfo_ID))]
        public virtual List<tb_Contract> tb_Contracts { get; set; }
        //tb_Contract.InvoiceInfo_ID)
        //InvoiceInfo_ID.FK_TB_CONTR_REF_TB_INVOINFO)
        //tb_InvoiceInfo.InvoiceInfo_ID)


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
                    Type type = typeof(tb_InvoiceInfo);
                    
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
            tb_InvoiceInfo loctype = (tb_InvoiceInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

