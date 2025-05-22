
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/23/2025 23:00:48
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
    /// 系统使用者公司
    /// </summary>
    [Serializable()]
    [Description("系统使用者公司")]
    [SugarTable("tb_Company")]
    public partial class tb_Company: BaseEntity, ICloneable
    {
        public tb_Company()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("系统使用者公司tb_Company" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long ID
        { 
            get{return _ID;}
            set{
            SetProperty(ref _ID, value);
                base.PrimaryKeyID = _ID;
            }
        }

        private string _CompanyCode;
        /// <summary>
        /// 公司代号
        /// </summary>
        [AdvQueryAttribute(ColName = "CompanyCode",ColDesc = "公司代号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CompanyCode" ,Length=30,IsNullable = true,ColumnDescription = "公司代号" )]
        public string CompanyCode
        { 
            get{return _CompanyCode;}
            set{
            SetProperty(ref _CompanyCode, value);
                        }
        }

        private string _CNName;
        /// <summary>
        /// 名称
        /// </summary>
        [AdvQueryAttribute(ColName = "CNName",ColDesc = "名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CNName" ,Length=100,IsNullable = true,ColumnDescription = "名称" )]
        public string CNName
        { 
            get{return _CNName;}
            set{
            SetProperty(ref _CNName, value);
                        }
        }

        private string _ENName;
        /// <summary>
        /// 英语名称
        /// </summary>
        [AdvQueryAttribute(ColName = "ENName",ColDesc = "英语名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ENName" ,Length=100,IsNullable = true,ColumnDescription = "英语名称" )]
        public string ENName
        { 
            get{return _ENName;}
            set{
            SetProperty(ref _ENName, value);
                        }
        }

        private string _ShortName;
        /// <summary>
        /// 简称
        /// </summary>
        [AdvQueryAttribute(ColName = "ShortName",ColDesc = "简称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ShortName" ,Length=50,IsNullable = true,ColumnDescription = "简称" )]
        public string ShortName
        { 
            get{return _ShortName;}
            set{
            SetProperty(ref _ShortName, value);
                        }
        }

        private string _LegalPersonName;
        /// <summary>
        /// 法人姓名
        /// </summary>
        [AdvQueryAttribute(ColName = "LegalPersonName",ColDesc = "法人姓名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "LegalPersonName" ,Length=50,IsNullable = true,ColumnDescription = "法人姓名" )]
        public string LegalPersonName
        { 
            get{return _LegalPersonName;}
            set{
            SetProperty(ref _LegalPersonName, value);
                        }
        }

        private string _UnifiedSocialCreditIdentifier;
        /// <summary>
        /// 公司执照代码
        /// </summary>
        [AdvQueryAttribute(ColName = "UnifiedSocialCreditIdentifier",ColDesc = "公司执照代码")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "UnifiedSocialCreditIdentifier" ,Length=50,IsNullable = true,ColumnDescription = "公司执照代码" )]
        public string UnifiedSocialCreditIdentifier
        { 
            get{return _UnifiedSocialCreditIdentifier;}
            set{
            SetProperty(ref _UnifiedSocialCreditIdentifier, value);
                        }
        }

        private string _Contact;
        /// <summary>
        /// 联系人
        /// </summary>
        [AdvQueryAttribute(ColName = "Contact",ColDesc = "联系人")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Contact" ,Length=100,IsNullable = true,ColumnDescription = "联系人" )]
        public string Contact
        { 
            get{return _Contact;}
            set{
            SetProperty(ref _Contact, value);
                        }
        }

        private string _Phone;
        /// <summary>
        /// 电话
        /// </summary>
        [AdvQueryAttribute(ColName = "Phone",ColDesc = "电话")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Phone" ,Length=100,IsNullable = true,ColumnDescription = "电话" )]
        public string Phone
        { 
            get{return _Phone;}
            set{
            SetProperty(ref _Phone, value);
                        }
        }

        private string _Address;
        /// <summary>
        /// 营业地址
        /// </summary>
        [AdvQueryAttribute(ColName = "Address",ColDesc = "营业地址")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Address" ,Length=255,IsNullable = true,ColumnDescription = "营业地址" )]
        public string Address
        { 
            get{return _Address;}
            set{
            SetProperty(ref _Address, value);
                        }
        }

        private string _ENAddress;
        /// <summary>
        /// 英文地址
        /// </summary>
        [AdvQueryAttribute(ColName = "ENAddress",ColDesc = "英文地址")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ENAddress" ,Length=255,IsNullable = true,ColumnDescription = "英文地址" )]
        public string ENAddress
        { 
            get{return _ENAddress;}
            set{
            SetProperty(ref _ENAddress, value);
                        }
        }

        private string _Website;
        /// <summary>
        /// 网址
        /// </summary>
        [AdvQueryAttribute(ColName = "Website",ColDesc = "网址")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Website" ,Length=255,IsNullable = true,ColumnDescription = "网址" )]
        public string Website
        { 
            get{return _Website;}
            set{
            SetProperty(ref _Website, value);
                        }
        }

        private string _Email;
        /// <summary>
        /// 电子邮件
        /// </summary>
        [AdvQueryAttribute(ColName = "Email",ColDesc = "电子邮件")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Email" ,Length=100,IsNullable = true,ColumnDescription = "电子邮件" )]
        public string Email
        { 
            get{return _Email;}
            set{
            SetProperty(ref _Email, value);
                        }
        }

        private string _InvoiceTitle;
        /// <summary>
        /// 发票抬头
        /// </summary>
        [AdvQueryAttribute(ColName = "InvoiceTitle",ColDesc = "发票抬头")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "InvoiceTitle" ,Length=200,IsNullable = true,ColumnDescription = "发票抬头" )]
        public string InvoiceTitle
        { 
            get{return _InvoiceTitle;}
            set{
            SetProperty(ref _InvoiceTitle, value);
                        }
        }

        private string _InvoiceTaxNumber;
        /// <summary>
        /// 纳税人识别号
        /// </summary>
        [AdvQueryAttribute(ColName = "InvoiceTaxNumber",ColDesc = "纳税人识别号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "InvoiceTaxNumber" ,Length=200,IsNullable = true,ColumnDescription = "纳税人识别号" )]
        public string InvoiceTaxNumber
        { 
            get{return _InvoiceTaxNumber;}
            set{
            SetProperty(ref _InvoiceTaxNumber, value);
                        }
        }

        private string _InvoiceAddress;
        /// <summary>
        /// 发票地址
        /// </summary>
        [AdvQueryAttribute(ColName = "InvoiceAddress",ColDesc = "发票地址")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "InvoiceAddress" ,Length=200,IsNullable = true,ColumnDescription = "发票地址" )]
        public string InvoiceAddress
        { 
            get{return _InvoiceAddress;}
            set{
            SetProperty(ref _InvoiceAddress, value);
                        }
        }

        private string _InvoiceTEL;
        /// <summary>
        /// 发票电话
        /// </summary>
        [AdvQueryAttribute(ColName = "InvoiceTEL",ColDesc = "发票电话")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "InvoiceTEL" ,Length=50,IsNullable = true,ColumnDescription = "发票电话" )]
        public string InvoiceTEL
        { 
            get{return _InvoiceTEL;}
            set{
            SetProperty(ref _InvoiceTEL, value);
                        }
        }

        private string _InvoiceBankAccount;
        /// <summary>
        /// 银行账号
        /// </summary>
        [AdvQueryAttribute(ColName = "InvoiceBankAccount",ColDesc = "银行账号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "InvoiceBankAccount" ,Length=150,IsNullable = true,ColumnDescription = "银行账号" )]
        public string InvoiceBankAccount
        { 
            get{return _InvoiceBankAccount;}
            set{
            SetProperty(ref _InvoiceBankAccount, value);
                        }
        }

        private string _InvoiceBankName;
        /// <summary>
        /// 开户行
        /// </summary>
        [AdvQueryAttribute(ColName = "InvoiceBankName",ColDesc = "开户行")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "InvoiceBankName" ,Length=100,IsNullable = true,ColumnDescription = "开户行" )]
        public string InvoiceBankName
        { 
            get{return _InvoiceBankName;}
            set{
            SetProperty(ref _InvoiceBankName, value);
                        }
        }

        private DateTime? _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Created_at" ,IsNullable = true,ColumnDescription = "创建时间" )]
        public DateTime? Created_at
        { 
            get{return _Created_at;}
            set{
            SetProperty(ref _Created_at, value);
                        }
        }

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Created_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "创建人" )]
        public long? Created_by
        { 
            get{return _Created_by;}
            set{
            SetProperty(ref _Created_by, value);
                        }
        }

        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Modified_at" ,IsNullable = true,ColumnDescription = "修改时间" )]
        public DateTime? Modified_at
        { 
            get{return _Modified_at;}
            set{
            SetProperty(ref _Modified_at, value);
                        }
        }

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Modified_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "修改人" )]
        public long? Modified_by
        { 
            get{return _Modified_by;}
            set{
            SetProperty(ref _Modified_by, value);
                        }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=255,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
                        }
        }

        #endregion

        #region 扩展属性

       

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Department.ID))]
        public virtual List<tb_Department> tb_Departments { get; set; }
        //tb_Department.ID)
        //ID.FK_DEPARTMENT_REF_COMPANY)
        //tb_Company.ID)

       

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_Account.ID))]
        public virtual List<tb_FM_Account> tb_FM_Accounts { get; set; }
        //tb_FM_Account.ID)
        //ID.FK_FM_ACCOUNT_REF_COMPANY)
        //tb_Company.ID)


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






        public override object Clone()
        {
            tb_Company loctype = (tb_Company)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

