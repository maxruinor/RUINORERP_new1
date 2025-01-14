
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/14/2025 18:57:02
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
    /// 产品类别表 与行业相关的产品分类
    /// </summary>
    [Serializable()]
    [Description("产品类别表 与行业相关的产品分类")]
    [SugarTable("tb_ProdCategories")]
    public partial class tb_ProdCategories: BaseEntity, ICloneable
    {
        public tb_ProdCategories()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("产品类别表 与行业相关的产品分类tb_ProdCategories" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Category_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Category_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long Category_ID
        { 
            get{return _Category_ID;}
            set{
            base.PrimaryKeyID = _Category_ID;
            SetProperty(ref _Category_ID, value);
            }
        }

        private string _Category_name;
        /// <summary>
        /// 类别名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Category_name",ColDesc = "类别名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Category_name" ,Length=50,IsNullable = true,ColumnDescription = "类别名称" )]
        public string Category_name
        { 
            get{return _Category_name;}
            set{
            SetProperty(ref _Category_name, value);
            }
        }

        private string _CategoryCode;
        /// <summary>
        /// 类别代码
        /// </summary>
        [AdvQueryAttribute(ColName = "CategoryCode",ColDesc = "类别代码")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CategoryCode" ,Length=20,IsNullable = true,ColumnDescription = "类别代码" )]
        public string CategoryCode
        { 
            get{return _CategoryCode;}
            set{
            SetProperty(ref _CategoryCode, value);
            }
        }

        private bool? _Is_enabled= true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_enabled",ColDesc = "是否启用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Is_enabled" ,IsNullable = true,ColumnDescription = "是否启用" )]
        public bool? Is_enabled
        { 
            get{return _Is_enabled;}
            set{
            SetProperty(ref _Is_enabled, value);
            }
        }

        private int? _Sort;
        /// <summary>
        /// 排序
        /// </summary>
        [AdvQueryAttribute(ColName = "Sort",ColDesc = "排序")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Sort" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "排序" )]
        public int? Sort
        { 
            get{return _Sort;}
            set{
            SetProperty(ref _Sort, value);
            }
        }

        private long? _Parent_id;
        /// <summary>
        /// 父类
        /// </summary>
        [AdvQueryAttribute(ColName = "Parent_id",ColDesc = "父类")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Parent_id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "父类" )]
        public long? Parent_id
        { 
            get{return _Parent_id;}
            set{
            SetProperty(ref _Parent_id, value);
            }
        }

        private byte[] _Images;
        /// <summary>
        /// 类目图片
        /// </summary>
        [AdvQueryAttribute(ColName = "Images",ColDesc = "类目图片")] 
        [SugarColumn(ColumnDataType = "image", SqlParameterDbType ="Binary",  ColumnName = "Images" ,Length=2147483647,IsNullable = true,ColumnDescription = "类目图片" )]
        public byte[] Images
        { 
            get{return _Images;}
            set{
            SetProperty(ref _Images, value);
            }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=200,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
            }
        }

        #endregion

        #region 扩展属性

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Prod.Category_ID))]
        public virtual List<tb_Prod> tb_Prods { get; set; }
        //tb_Prod.Category_ID)
        //Category_ID.FK_TB_PROD_REFERENCE_TB_PRODC)
        //tb_ProdCategories.Category_ID)


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
                    Type type = typeof(tb_ProdCategories);
                    
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
            tb_ProdCategories loctype = (tb_ProdCategories)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

