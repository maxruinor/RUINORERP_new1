
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:57
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
    /// 网店信息表
    /// </summary>
    [Serializable()]
    [Description("tb_OnlineStoreInfo")]
    [SugarTable("tb_OnlineStoreInfo")]
    public partial class tb_OnlineStoreInfo: BaseEntity, ICloneable
    {
        public tb_OnlineStoreInfo()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_OnlineStoreInfo" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Store_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Store_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long Store_ID
        { 
            get{return _Store_ID;}
            set{
            base.PrimaryKeyID = _Store_ID;
            SetProperty(ref _Store_ID, value);
            }
        }

        private string _StoreCode;
        /// <summary>
        /// 项目代码
        /// </summary>
        [AdvQueryAttribute(ColName = "StoreCode",ColDesc = "项目代码")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "StoreCode" ,Length=50,IsNullable = true,ColumnDescription = "项目代码" )]
        public string StoreCode
        { 
            get{return _StoreCode;}
            set{
            SetProperty(ref _StoreCode, value);
            }
        }

        private string _StoreName;
        /// <summary>
        /// 项目名称
        /// </summary>
        [AdvQueryAttribute(ColName = "StoreName",ColDesc = "项目名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "StoreName" ,Length=50,IsNullable = true,ColumnDescription = "项目名称" )]
        public string StoreName
        { 
            get{return _StoreName;}
            set{
            SetProperty(ref _StoreName, value);
            }
        }

        private string _PlatformName;
        /// <summary>
        /// 平台名称
        /// </summary>
        [AdvQueryAttribute(ColName = "PlatformName",ColDesc = "平台名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PlatformName" ,Length=100,IsNullable = true,ColumnDescription = "平台名称" )]
        public string PlatformName
        { 
            get{return _PlatformName;}
            set{
            SetProperty(ref _PlatformName, value);
            }
        }

        private string _Contact;
        /// <summary>
        /// 联系人
        /// </summary>
        [AdvQueryAttribute(ColName = "Contact",ColDesc = "联系人")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Contact" ,Length=50,IsNullable = true,ColumnDescription = "联系人" )]
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
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Phone" ,Length=255,IsNullable = true,ColumnDescription = "电话" )]
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

        private string _ResponsiblePerson;
        /// <summary>
        /// 负责人
        /// </summary>
        [AdvQueryAttribute(ColName = "ResponsiblePerson",ColDesc = "负责人")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ResponsiblePerson" ,Length=50,IsNullable = true,ColumnDescription = "负责人" )]
        public string ResponsiblePerson
        { 
            get{return _ResponsiblePerson;}
            set{
            SetProperty(ref _ResponsiblePerson, value);
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

        #endregion

        #region 扩展属性


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
                    Type type = typeof(tb_OnlineStoreInfo);
                    
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
            tb_OnlineStoreInfo loctype = (tb_OnlineStoreInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

