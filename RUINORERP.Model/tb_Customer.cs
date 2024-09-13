
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:32
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
    /// 意向客户，公海客户 CRM系统中使用，给成交客户作外键引用
    /// </summary>
    [Serializable()]
    [Description("tb_Customer")]
    [SugarTable("tb_Customer")]
    public partial class tb_Customer: BaseEntity, ICloneable
    {
        public tb_Customer()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_Customer" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Customer_id;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Customer_id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long Customer_id
        { 
            get{return _Customer_id;}
            set{
            base.PrimaryKeyID = _Customer_id;
            SetProperty(ref _Customer_id, value);
            }
        }

        private string _CompanyName;
        /// <summary>
        /// 公司名称
        /// </summary>
        [AdvQueryAttribute(ColName = "CompanyName",ColDesc = "公司名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CompanyName" ,Length=50,IsNullable = true,ColumnDescription = "公司名称" )]
        public string CompanyName
        { 
            get{return _CompanyName;}
            set{
            SetProperty(ref _CompanyName, value);
            }
        }

        private long? _Employee_ID;
        /// <summary>
        /// 对接人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "对接人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "对接人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long? Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
            }
        }

        private string _CustomerName;
        /// <summary>
        /// 客户名称
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerName",ColDesc = "客户名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomerName" ,Length=50,IsNullable = true,ColumnDescription = "客户名称" )]
        public string CustomerName
        { 
            get{return _CustomerName;}
            set{
            SetProperty(ref _CustomerName, value);
            }
        }

        private string _CustomerAddress;
        /// <summary>
        /// 客户地址
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerAddress",ColDesc = "客户地址")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomerAddress" ,Length=300,IsNullable = true,ColumnDescription = "客户地址" )]
        public string CustomerAddress
        { 
            get{return _CustomerAddress;}
            set{
            SetProperty(ref _CustomerAddress, value);
            }
        }

        private string _CustomerDesc;
        /// <summary>
        /// 客户描述
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerDesc",ColDesc = "客户描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomerDesc" ,Length=1000,IsNullable = true,ColumnDescription = "客户描述" )]
        public string CustomerDesc
        { 
            get{return _CustomerDesc;}
            set{
            SetProperty(ref _CustomerDesc, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }


        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Region.Customer_id))]
        public virtual List<tb_Region> tb_Regions { get; set; }
        //tb_Region.Customer_id)
        //Customer_id.FK_REGIONS_REF_CUSTOMER)
        //tb_Customer.Customer_id)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Contact.Customer_id))]
        public virtual List<tb_Contact> tb_Contacts { get; set; }
        //tb_Contact.Customer_id)
        //Customer_id.FK_CONTACTS_REF_CUSTOMER)
        //tb_Customer.Customer_id)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Customer_interaction.Customer_id))]
        public virtual List<tb_Customer_interaction> tb_Customer_interactions { get; set; }
        //tb_Customer_interaction.Customer_id)
        //Customer_id.FK_CUSTOMERINTERACTIONS_REF_CUSTOMER)
        //tb_Customer.Customer_id)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_CustomerVendor.Customer_id))]
        public virtual List<tb_CustomerVendor> tb_CustomerVendors { get; set; }
        //tb_CustomerVendor.Customer_id)
        //Customer_id.FK_CUSTOMERVENDOR_REF_CUSTOMER)
        //tb_Customer.Customer_id)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Sales_Chance.Customer_id))]
        public virtual List<tb_Sales_Chance> tb_Sales_Chances { get; set; }
        //tb_Sales_Chance.Customer_id)
        //Customer_id.FK_SALESOPPORTUNI_REF_CUSTOMER)
        //tb_Customer.Customer_id)


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
                    Type type = typeof(tb_Customer);
                    
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
            tb_Customer loctype = (tb_Customer)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

