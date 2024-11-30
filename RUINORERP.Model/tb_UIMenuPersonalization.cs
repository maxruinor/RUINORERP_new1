
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/30/2024 00:18:29
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
    /// 用户角色菜单个性化设置表一个角色用户菜单 三个字段为联合主键 就一行数据
    /// </summary>
    [Serializable()]
    [Description("用户角色菜单个性化设置表一个角色用户菜单 三个字段为联合主键 就一行数据")]
    [SugarTable("tb_UIMenuPersonalization")]
    public partial class tb_UIMenuPersonalization: BaseEntity, ICloneable
    {
        public tb_UIMenuPersonalization()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("用户角色菜单个性化设置表一个角色用户菜单 三个字段为联合主键 就一行数据tb_UIMenuPersonalization" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _UIPID;
        /// <summary>
        /// 个性化
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "UIPID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "个性化" , IsPrimaryKey = true)]
        public long UIPID
        { 
            get{return _UIPID;}
            set{
            base.PrimaryKeyID = _UIPID;
            SetProperty(ref _UIPID, value);
            }
        }

        private long _MenuID;
        /// <summary>
        /// 关联菜单
        /// </summary>
        [AdvQueryAttribute(ColName = "MenuID",ColDesc = "关联菜单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "MenuID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "关联菜单" )]
        [FKRelationAttribute("tb_MenuInfo","MenuID")]
        public long MenuID
        { 
            get{return _MenuID;}
            set{
            SetProperty(ref _MenuID, value);
            }
        }

        private long? _UIQCID;
        /// <summary>
        /// 查询条件
        /// </summary>
        [AdvQueryAttribute(ColName = "UIQCID",ColDesc = "查询条件")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "UIQCID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "查询条件" )]
        [FKRelationAttribute("tb_UIQueryCondition","UIQCID")]
        public long? UIQCID
        { 
            get{return _UIQCID;}
            set{
            SetProperty(ref _UIQCID, value);
            }
        }

        private long? _UIGID;
        /// <summary>
        /// 表格设置
        /// </summary>
        [AdvQueryAttribute(ColName = "UIGID",ColDesc = "表格设置")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "UIGID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "表格设置" )]
        [FKRelationAttribute("tb_UIGridSetting","UIGID")]
        public long? UIGID
        { 
            get{return _UIGID;}
            set{
            SetProperty(ref _UIGID, value);
            }
        }

        private int? _QueryConditionCols;
        /// <summary>
        /// 条件显示列数量
        /// </summary>
        [AdvQueryAttribute(ColName = "QueryConditionCols",ColDesc = "条件显示列数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "QueryConditionCols" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "条件显示列数量" )]
        public int? QueryConditionCols
        { 
            get{return _QueryConditionCols;}
            set{
            SetProperty(ref _QueryConditionCols, value);
            }
        }

        private bool? _IsRelatedQuerySettings= false;
        /// <summary>
        /// 是关联查询设置
        /// </summary>
        [AdvQueryAttribute(ColName = "IsRelatedQuerySettings",ColDesc = "是关联查询设置")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsRelatedQuerySettings" ,IsNullable = true,ColumnDescription = "是关联查询设置" )]
        public bool? IsRelatedQuerySettings
        { 
            get{return _IsRelatedQuerySettings;}
            set{
            SetProperty(ref _IsRelatedQuerySettings, value);
            }
        }

        private int? _FavoritesMenu= ((0));
        /// <summary>
        /// 收藏菜单
        /// </summary>
        [AdvQueryAttribute(ColName = "FavoritesMenu",ColDesc = "收藏菜单")] 
        [SugarColumn(ColumnDataType = "tinyint", SqlParameterDbType ="SByte",  ColumnName = "FavoritesMenu" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "收藏菜单" )]
        public int? FavoritesMenu
        { 
            get{return _FavoritesMenu;}
            set{
            SetProperty(ref _FavoritesMenu, value);
            }
        }

        private string _MenuType;
        /// <summary>
        /// 菜单类型
        /// </summary>
        [AdvQueryAttribute(ColName = "MenuType",ColDesc = "菜单类型")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "MenuType" ,Length=20,IsNullable = false,ColumnDescription = "菜单类型" )]
        public string MenuType
        { 
            get{return _MenuType;}
            set{
            SetProperty(ref _MenuType, value);
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

        private string _DefaultLayout;
        /// <summary>
        /// 默认布局
        /// </summary>
        [AdvQueryAttribute(ColName = "DefaultLayout",ColDesc = "默认布局")] 
        [SugarColumn(ColumnDataType = "text", SqlParameterDbType ="String",  ColumnName = "DefaultLayout" ,Length=2147483647,IsNullable = true,ColumnDescription = "默认布局" )]
        public string DefaultLayout
        { 
            get{return _DefaultLayout;}
            set{
            SetProperty(ref _DefaultLayout, value);
            }
        }

        private string _DefaultLayout2;
        /// <summary>
        /// 默认布局
        /// </summary>
        [AdvQueryAttribute(ColName = "DefaultLayout2",ColDesc = "默认布局")] 
        [SugarColumn(ColumnDataType = "text", SqlParameterDbType ="String",  ColumnName = "DefaultLayout2" ,Length=2147483647,IsNullable = true,ColumnDescription = "默认布局" )]
        public string DefaultLayout2
        { 
            get{return _DefaultLayout2;}
            set{
            SetProperty(ref _DefaultLayout2, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(MenuID))]
        public virtual tb_MenuInfo tb_menuinfo { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(UIQCID))]
        public virtual tb_UIQueryCondition tb_uiquerycondition { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(UIGID))]
        public virtual tb_UIGridSetting tb_uigridsetting { get; set; }


        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_UserPersonalized.UIPID))]
        public virtual List<tb_UserPersonalized> tb_UserPersonalizeds { get; set; }
        //tb_UserPersonalized.UIPID)
        //UIPID.FK_TB_USERPERSONALIX_REF_UIMENPERSONALIZ)
        //tb_UIMenuPersonalization.UIPID)


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
                    Type type = typeof(tb_UIMenuPersonalization);
                    
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
            tb_UIMenuPersonalization loctype = (tb_UIMenuPersonalization)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

