
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:08
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
    /// 字段权限表
    /// </summary>
    [Serializable()]
    [Description("字段权限表")]
    [SugarTable("tb_P4Field")]
    public partial class tb_P4Field: BaseEntity, ICloneable
    {
        public tb_P4Field()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("字段权限表tb_P4Field" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _P4Field_ID;
        /// <summary>
        /// 字段关系
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "P4Field_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "字段关系" , IsPrimaryKey = true)]
        public long P4Field_ID
        { 
            get{return _P4Field_ID;}
            set{
            SetProperty(ref _P4Field_ID, value);
                base.PrimaryKeyID = _P4Field_ID;
            }
        }

        private long? _FieldInfo_ID;
        /// <summary>
        /// 字段
        /// </summary>
        [AdvQueryAttribute(ColName = "FieldInfo_ID",ColDesc = "字段")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "FieldInfo_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "字段" )]
        [FKRelationAttribute("tb_FieldInfo","FieldInfo_ID")]
        public long? FieldInfo_ID
        { 
            get{return _FieldInfo_ID;}
            set{
            SetProperty(ref _FieldInfo_ID, value);
                        }
        }

        private long? _RoleID;
        /// <summary>
        /// 角色
        /// </summary>
        [AdvQueryAttribute(ColName = "RoleID",ColDesc = "角色")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RoleID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "角色" )]
        [FKRelationAttribute("tb_RoleInfo","RoleID")]
        public long? RoleID
        { 
            get{return _RoleID;}
            set{
            SetProperty(ref _RoleID, value);
                        }
        }

        private long? _MenuID;
        /// <summary>
        /// 菜单
        /// </summary>
        [AdvQueryAttribute(ColName = "MenuID",ColDesc = "菜单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "MenuID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "菜单" )]
        [FKRelationAttribute("tb_MenuInfo","MenuID")]
        public long? MenuID
        { 
            get{return _MenuID;}
            set{
            SetProperty(ref _MenuID, value);
                        }
        }

        private bool _IsVisble;
        /// <summary>
        /// 是否可见
        /// </summary>
        [AdvQueryAttribute(ColName = "IsVisble",ColDesc = "是否可见")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsVisble" ,IsNullable = false,ColumnDescription = "是否可见" )]
        public bool IsVisble
        { 
            get{return _IsVisble;}
            set{
            SetProperty(ref _IsVisble, value);
                        }
        }

        private bool _CanReadWrite;
        /// <summary>
        /// 可读写
        /// </summary>
        [AdvQueryAttribute(ColName = "CanReadWrite",ColDesc = "可读写")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "CanReadWrite" ,IsNullable = false,ColumnDescription = "可读写" )]
        public bool CanReadWrite
        { 
            get{return _CanReadWrite;}
            set{
            SetProperty(ref _CanReadWrite, value);
                        }
        }

        private bool _OnlyRead;
        /// <summary>
        /// 只读
        /// </summary>
        [AdvQueryAttribute(ColName = "OnlyRead",ColDesc = "只读")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "OnlyRead" ,IsNullable = false,ColumnDescription = "只读" )]
        public bool OnlyRead
        { 
            get{return _OnlyRead;}
            set{
            SetProperty(ref _OnlyRead, value);
                        }
        }

        private bool _HideValue;
        /// <summary>
        /// 默认隐藏
        /// </summary>
        [AdvQueryAttribute(ColName = "HideValue",ColDesc = "默认隐藏")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "HideValue" ,IsNullable = false,ColumnDescription = "默认隐藏" )]
        public bool HideValue
        { 
            get{return _HideValue;}
            set{
            SetProperty(ref _HideValue, value);
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

        private bool _IsChild;
        /// <summary>
        /// 子表字段
        /// </summary>
        [AdvQueryAttribute(ColName = "IsChild",ColDesc = "子表字段")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsChild" ,IsNullable = false,ColumnDescription = "子表字段" )]
        public bool IsChild
        { 
            get{return _IsChild;}
            set{
            SetProperty(ref _IsChild, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(FieldInfo_ID))]
        public virtual tb_FieldInfo tb_fieldinfo { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(MenuID))]
        public virtual tb_MenuInfo tb_menuinfo { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(RoleID))]
        public virtual tb_RoleInfo tb_roleinfo { get; set; }



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
                    Type type = typeof(tb_P4Field);
                    
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
            tb_P4Field loctype = (tb_P4Field)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

