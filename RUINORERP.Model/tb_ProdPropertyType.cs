
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:16
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
    /// 产品属性类型EVA
    /// </summary>
    [Serializable()]
    [Description("产品属性类型EVA")]
    [SugarTable("tb_ProdPropertyType")]
    public partial class tb_ProdPropertyType: BaseEntity, ICloneable
    {
        public tb_ProdPropertyType()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("产品属性类型EVAtb_ProdPropertyType" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PropertyType_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PropertyType_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long PropertyType_ID
        { 
            get{return _PropertyType_ID;}
            set{
            SetProperty(ref _PropertyType_ID, value);
                base.PrimaryKeyID = _PropertyType_ID;
            }
        }

        private string _PropertyTypeName;
        /// <summary>
        /// 属性类型名称
        /// </summary>
        [AdvQueryAttribute(ColName = "PropertyTypeName",ColDesc = "属性类型名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PropertyTypeName" ,Length=50,IsNullable = false,ColumnDescription = "属性类型名称" )]
        public string PropertyTypeName
        { 
            get{return _PropertyTypeName;}
            set{
            SetProperty(ref _PropertyTypeName, value);
                        }
        }

        private string _PropertyTypeDesc;
        /// <summary>
        /// 属性类型描述
        /// </summary>
        [AdvQueryAttribute(ColName = "PropertyTypeDesc",ColDesc = "属性类型描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PropertyTypeDesc" ,Length=100,IsNullable = true,ColumnDescription = "属性类型描述" )]
        public string PropertyTypeDesc
        { 
            get{return _PropertyTypeDesc;}
            set{
            SetProperty(ref _PropertyTypeDesc, value);
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
                    Type type = typeof(tb_ProdPropertyType);
                    
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
            tb_ProdPropertyType loctype = (tb_ProdPropertyType)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

