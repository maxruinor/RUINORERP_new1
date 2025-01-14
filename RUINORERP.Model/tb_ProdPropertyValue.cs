
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/14/2025 18:57:04
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
    /// 产品属性值表
    /// </summary>
    [Serializable()]
    [Description("产品属性值表")]
    [SugarTable("tb_ProdPropertyValue")]
    public partial class tb_ProdPropertyValue: BaseEntity, ICloneable
    {
        public tb_ProdPropertyValue()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("产品属性值表tb_ProdPropertyValue" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PropertyValueID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PropertyValueID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long PropertyValueID
        { 
            get{return _PropertyValueID;}
            set{
            base.PrimaryKeyID = _PropertyValueID;
            SetProperty(ref _PropertyValueID, value);
            }
        }

        private long _Property_ID;
        /// <summary>
        /// 属性
        /// </summary>
        [AdvQueryAttribute(ColName = "Property_ID",ColDesc = "属性")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Property_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "属性" )]
        [FKRelationAttribute("tb_ProdProperty","Property_ID")]
        public long Property_ID
        { 
            get{return _Property_ID;}
            set{
            SetProperty(ref _Property_ID, value);
            }
        }

        private string _PropertyValueName;
        /// <summary>
        /// 属性值名称
        /// </summary>
        [AdvQueryAttribute(ColName = "PropertyValueName",ColDesc = "属性值名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PropertyValueName" ,Length=20,IsNullable = false,ColumnDescription = "属性值名称" )]
        public string PropertyValueName
        { 
            get{return _PropertyValueName;}
            set{
            SetProperty(ref _PropertyValueName, value);
            }
        }

        private string _PropertyValueDesc;
        /// <summary>
        /// 属性值描述
        /// </summary>
        [AdvQueryAttribute(ColName = "PropertyValueDesc",ColDesc = "属性值描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PropertyValueDesc" ,Length=50,IsNullable = true,ColumnDescription = "属性值描述" )]
        public string PropertyValueDesc
        { 
            get{return _PropertyValueDesc;}
            set{
            SetProperty(ref _PropertyValueDesc, value);
            }
        }

        private int? _SortOrder;
        /// <summary>
        /// 排序
        /// </summary>
        [AdvQueryAttribute(ColName = "SortOrder",ColDesc = "排序")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "SortOrder" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "排序" )]
        public int? SortOrder
        { 
            get{return _SortOrder;}
            set{
            SetProperty(ref _SortOrder, value);
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

        private bool _isdeleted= false;
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "isdeleted" ,IsNullable = false,ColumnDescription = "逻辑删除" )]
        [Browsable(false)]
        public bool isdeleted
        { 
            get{return _isdeleted;}
            set{
            SetProperty(ref _isdeleted, value);
            }
        }

        private int? _DataStatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "DataStatus",ColDesc = "数据状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DataStatus" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "数据状态" )]
        public int? DataStatus
        { 
            get{return _DataStatus;}
            set{
            SetProperty(ref _DataStatus, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Property_ID))]
        public virtual tb_ProdProperty tb_prodproperty { get; set; }


        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Prod_Attr_Relation.PropertyValueID))]
        public virtual List<tb_Prod_Attr_Relation> tb_Prod_Attr_Relations { get; set; }
        //tb_Prod_Attr_Relation.PropertyValueID)
        //PropertyValueID.FK_TB_PROD_REF_TB_PROD_2)
        //tb_ProdPropertyValue.PropertyValueID)


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
                    Type type = typeof(tb_ProdPropertyValue);
                    
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
            tb_ProdPropertyValue loctype = (tb_ProdPropertyValue)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

