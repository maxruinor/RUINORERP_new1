
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:30
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
    /// 联系人表，CRM系统中使用
    /// </summary>
    [Serializable()]
    [Description("tb_Contact")]
    [SugarTable("tb_Contact")]
    public partial class tb_Contact: BaseEntity, ICloneable
    {
        public tb_Contact()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_Contact" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Contact_id;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Contact_id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long Contact_id
        { 
            get{return _Contact_id;}
            set{
            base.PrimaryKeyID = _Contact_id;
            SetProperty(ref _Contact_id, value);
            }
        }

        private long? _Customer_id;
        /// <summary>
        /// 意向客户
        /// </summary>
        [AdvQueryAttribute(ColName = "Customer_id",ColDesc = "意向客户")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Customer_id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "意向客户" )]
        [FKRelationAttribute("tb_Customer","Customer_id")]
        public long? Customer_id
        { 
            get{return _Customer_id;}
            set{
            SetProperty(ref _Customer_id, value);
            }
        }

        private string _Contact_Name;
        /// <summary>
        /// 名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Contact_Name",ColDesc = "名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Contact_Name" ,Length=50,IsNullable = true,ColumnDescription = "名称" )]
        public string Contact_Name
        { 
            get{return _Contact_Name;}
            set{
            SetProperty(ref _Contact_Name, value);
            }
        }

        private string _Contact_Email;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Contact_Email",ColDesc = "描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Contact_Email" ,Length=100,IsNullable = true,ColumnDescription = "描述" )]
        public string Contact_Email
        { 
            get{return _Contact_Email;}
            set{
            SetProperty(ref _Contact_Email, value);
            }
        }

        private string _Contact_Phone;
        /// <summary>
        /// 电话
        /// </summary>
        [AdvQueryAttribute(ColName = "Contact_Phone",ColDesc = "电话")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Contact_Phone" ,Length=30,IsNullable = true,ColumnDescription = "电话" )]
        public string Contact_Phone
        { 
            get{return _Contact_Phone;}
            set{
            SetProperty(ref _Contact_Phone, value);
            }
        }

        private string _Preferences;
        /// <summary>
        /// 爱好
        /// </summary>
        [AdvQueryAttribute(ColName = "Preferences",ColDesc = "爱好")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Preferences" ,Length=100,IsNullable = true,ColumnDescription = "爱好" )]
        public string Preferences
        { 
            get{return _Preferences;}
            set{
            SetProperty(ref _Preferences, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Customer_id))]
        public virtual tb_Customer tb_customer { get; set; }



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
                    Type type = typeof(tb_Contact);
                    
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
            tb_Contact loctype = (tb_Contact)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

