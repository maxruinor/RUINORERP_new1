
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/07/2025 21:48:21
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
            base.FieldNameList = fieldNameList;
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
            base.PrimaryKeyID = _ID;
            SetProperty(ref _ID, value);
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
        /// 地址
        /// </summary>
        [AdvQueryAttribute(ColName = "Address",ColDesc = "地址")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Address" ,Length=255,IsNullable = true,ColumnDescription = "地址" )]
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

        [Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Department.ID))]
        public virtual List<tb_Department> tb_Departments { get; set; }
        //tb_Department.ID)
        //ID.FK_DEPARTMENT_REF_COMPANY)
        //tb_Company.ID)


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
                    Type type = typeof(tb_Company);
                    
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
            tb_Company loctype = (tb_Company)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

