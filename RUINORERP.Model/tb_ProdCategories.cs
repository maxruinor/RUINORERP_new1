
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/07/2025 11:46:21
// **************************************
using System;
using SqlSugar;
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
            SetProperty(ref _Category_ID, value);
                base.PrimaryKeyID = _Category_ID;
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

        private int? _CategoryLevel;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "CategoryLevel",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "CategoryLevel" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        public int? CategoryLevel
        { 
            get{return _CategoryLevel;}
            set{
            SetProperty(ref _CategoryLevel, value);
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
        [FKRelationAttribute("tb_ProdCategories","Parent_id")]
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
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(Parent_id))]
        public virtual tb_ProdCategories tb_prodcategories { get; set; }



        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Prod.Category_ID))]
        public virtual List<tb_Prod> tb_Prods { get; set; }


        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProdCategories.Parent_id))]
        public virtual List<tb_ProdCategories> tb_ProdCategorieses { get; set; }


        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
         if("Category_ID"!="Parent_id")
        {
        // rs=false;
        }
return rs;
}






       
        

        public override object Clone()
        {
            tb_ProdCategories loctype = (tb_ProdCategories)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

