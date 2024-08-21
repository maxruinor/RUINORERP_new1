
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/13/2023 17:33:37
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model.Base;

namespace RUINORERP.Model.QueryDto
{
    /// <summary>
    /// 联系人表，CRM系统中使用
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Contacts")]
    public partial class tb_ContactsQueryDto:BaseEntityDto
    {
        public tb_ContactsQueryDto()
        {

        }

    
     

        private long? _Customer_id;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Customer_id",ColDesc = "")]
        [SugarColumn(ColumnName = "Customer_id",IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_Customer","Customer_id")]
        public long? Customer_id 
        { 
            get{return _Customer_id;}
            set{SetProperty(ref _Customer_id, value);}
        }
     

        private string _Contact_Name;
        /// <summary>
        /// 名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Contact_Name",ColDesc = "名称")]
        [SugarColumn(ColumnName = "Contact_Name",Length=50,IsNullable = true,ColumnDescription = "名称" )]
        public string Contact_Name 
        { 
            get{return _Contact_Name;}
            set{SetProperty(ref _Contact_Name, value);}
        }
     

        private string _Contact_Email;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Contact_Email",ColDesc = "描述")]
        [SugarColumn(ColumnName = "Contact_Email",Length=100,IsNullable = true,ColumnDescription = "描述" )]
        public string Contact_Email 
        { 
            get{return _Contact_Email;}
            set{SetProperty(ref _Contact_Email, value);}
        }
     

        private string _Contact_Phone;
        /// <summary>
        /// 电话
        /// </summary>
        [AdvQueryAttribute(ColName = "Contact_Phone",ColDesc = "电话")]
        [SugarColumn(ColumnName = "Contact_Phone",Length=30,IsNullable = true,ColumnDescription = "电话" )]
        public string Contact_Phone 
        { 
            get{return _Contact_Phone;}
            set{SetProperty(ref _Contact_Phone, value);}
        }
     

        private string _Preferences;
        /// <summary>
        /// 爱好
        /// </summary>
        [AdvQueryAttribute(ColName = "Preferences",ColDesc = "爱好")]
        [SugarColumn(ColumnName = "Preferences",Length=100,IsNullable = true,ColumnDescription = "爱好" )]
        public string Preferences 
        { 
            get{return _Preferences;}
            set{SetProperty(ref _Preferences, value);}
        }


        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(RUINORERP.Model.tb_Customer.Customer_id))]
        public virtual tb_Customer tb_Customer { get; set; }




/*

        #region 字段描述对应列表
        private ConcurrentDictionary<string, BaseDtoField> fieldNameList;


        /// <summary>
        /// 表列名的中文描述集合
        /// </summary>
        [Description("列名中文描述"), Category("自定属性"), Browsable(true)]
        [SugarColumn(IsIgnore = true)]
        public ConcurrentDictionary<string, BaseDtoField> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, BaseDtoField>();
                    SugarColumn entityAttr;
                    Type type = typeof(tb_ContactsQueryDto);
                    
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

*/


       
    }
}



