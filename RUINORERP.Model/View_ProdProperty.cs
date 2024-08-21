
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/26/2024 11:47:01
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
    /// 产品属性关系
    /// </summary>
    [Serializable()]
    [SugarTable("View_ProdProperty")]
    public class View_ProdProperty:BaseEntity, ICloneable
    {
        public View_ProdProperty()
        {
            FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("View_ProdProperty" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    
        private long? _ProdBaseID;
        
        
        /// <summary>
        /// 产品
        /// </summary>

        [AdvQueryAttribute(ColName = "ProdBaseID",ColDesc = "产品")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdBaseID" ,IsNullable = true,ColumnDescription = "产品" )]
        [Display(Name = "产品")]
        public long? ProdBaseID 
        { 
            get{return _ProdBaseID;}            set{                SetProperty(ref _ProdBaseID, value);                }
        }

        private long? _ProdDetailID;
        
        
        /// <summary>
        /// 产品详情
        /// </summary>

        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品详情")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" ,IsNullable = true,ColumnDescription = "产品详情" )]
        [Display(Name = "产品详情")]
        public long? ProdDetailID 
        { 
            get{return _ProdDetailID;}            set{                SetProperty(ref _ProdDetailID, value);                }
        }

        private long? _Property_ID;
        
        
        /// <summary>
        /// Property_ID
        /// </summary>

        [AdvQueryAttribute(ColName = "Property_ID",ColDesc = "Property_ID")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Property_ID" ,IsNullable = true,ColumnDescription = "Property_ID" )]
        [Display(Name = "Property_ID")]
        public long? Property_ID 
        { 
            get{return _Property_ID;}            set{                SetProperty(ref _Property_ID, value);                }
        }

        private string _PropertyName;
        
        
        /// <summary>
        /// 属性名称
        /// </summary>

        [AdvQueryAttribute(ColName = "PropertyName",ColDesc = "属性名称")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PropertyName" ,Length=20,IsNullable = true,ColumnDescription = "属性名称" )]
        [Display(Name = "属性名称")]
        public string PropertyName 
        { 
            get{return _PropertyName;}            set{                SetProperty(ref _PropertyName, value);                }
        }

        private long? _PropertyValueID;
        
        
        /// <summary>
        /// PropertyValueID
        /// </summary>

        [AdvQueryAttribute(ColName = "PropertyValueID",ColDesc = "PropertyValueID")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PropertyValueID" ,IsNullable = true,ColumnDescription = "PropertyValueID" )]
        [Display(Name = "PropertyValueID")]
        public long? PropertyValueID 
        { 
            get{return _PropertyValueID;}            set{                SetProperty(ref _PropertyValueID, value);                }
        }

        private string _PropertyValueName;
        
        
        /// <summary>
        /// 属性值名称
        /// </summary>

        [AdvQueryAttribute(ColName = "PropertyValueName",ColDesc = "属性值名称")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PropertyValueName" ,Length=20,IsNullable = true,ColumnDescription = "属性值名称" )]
        [Display(Name = "属性值名称")]
        public string PropertyValueName 
        { 
            get{return _PropertyValueName;}            set{                SetProperty(ref _PropertyValueName, value);                }
        }







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
        [Description("列名中文描述"), Category("自定属性"), Browsable(true)]
        [SugarColumn(IsIgnore = true)]
        public override ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, string>();
                    SugarColumn entityAttr;
                    Type type = typeof(View_ProdProperty);
                    
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
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}

