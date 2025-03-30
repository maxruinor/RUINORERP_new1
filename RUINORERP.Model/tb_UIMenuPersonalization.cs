
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/30/2025 15:54:07
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
        private long _UIMenuPID;
        /// <summary>
        /// 菜单设置
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "UIMenuPID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "菜单设置" , IsPrimaryKey = true)]
        public long UIMenuPID
        { 
            get{return _UIMenuPID;}
            set{
            SetProperty(ref _UIMenuPID, value);
                base.PrimaryKeyID = _UIMenuPID;
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

        private long? _UserPersonalizedID;
        /// <summary>
        /// 用户角色设置
        /// </summary>
        [AdvQueryAttribute(ColName = "UserPersonalizedID",ColDesc = "用户角色设置")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "UserPersonalizedID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "用户角色设置" )]
        [FKRelationAttribute("tb_UserPersonalized","UserPersonalizedID")]
        public long? UserPersonalizedID
        { 
            get{return _UserPersonalizedID;}
            set{
            SetProperty(ref _UserPersonalizedID, value);
                        }
        }

        private int _QueryConditionCols= ((4));
        /// <summary>
        /// 条件显示列数量
        /// </summary>
        [AdvQueryAttribute(ColName = "QueryConditionCols",ColDesc = "条件显示列数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "QueryConditionCols" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "条件显示列数量" )]
        public int QueryConditionCols
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

        private int? _BaseWidth= ((0));
        /// <summary>
        /// 排序
        /// </summary>
        [AdvQueryAttribute(ColName = "BaseWidth",ColDesc = "排序")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "BaseWidth" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "排序" )]
        public int? BaseWidth
        { 
            get{return _BaseWidth;}
            set{
            SetProperty(ref _BaseWidth, value);
                        }
        }

        private int _Sort= ((150));
        /// <summary>
        /// 基准宽度
        /// </summary>
        [AdvQueryAttribute(ColName = "Sort",ColDesc = "基准宽度")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Sort" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "基准宽度" )]
        public int Sort
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
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(MenuID))]
        public virtual tb_MenuInfo tb_menuinfo { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(UserPersonalizedID))]
        public virtual tb_UserPersonalized tb_userpersonalized { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_UIQueryCondition.UIMenuPID))]
        public virtual List<tb_UIQueryCondition> tb_UIQueryConditions { get; set; }
        //tb_UIQueryCondition.UIMenuPID)
        //UIMenuPID.FK_UIQUERYCONDITION_REF_UIMENUPERSONALIZATION)
        //tb_UIMenuPersonalization.UIMenuPID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_UIGridSetting.UIMenuPID))]
        public virtual List<tb_UIGridSetting> tb_UIGridSettings { get; set; }
        //tb_UIGridSetting.UIMenuPID)
        //UIMenuPID.FK_UIGRIDSETTING_REF_UIMENUPERSONALIZATION)
        //tb_UIMenuPersonalization.UIMenuPID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_UIDataFieldSetting.UIMenuPID))]
        public virtual List<tb_UIDataFieldSetting> tb_UIDataFieldSettings { get; set; }
        //tb_UIDataFieldSetting.UIMenuPID)
        //UIMenuPID.FK_TB_UIDATAFIELDSETTING_REF_TB_UIMENPERSONLIZATION)
        //tb_UIMenuPersonalization.UIMenuPID)


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

