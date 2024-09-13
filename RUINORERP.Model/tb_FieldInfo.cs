
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:36
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
    /// 字段信息表
    /// </summary>
    [Serializable()]
    [Description("tb_FieldInfo")]
    [SugarTable("tb_FieldInfo")]
    public partial class tb_FieldInfo: BaseEntity, ICloneable
    {
        public tb_FieldInfo()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_FieldInfo" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _FieldInfo_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "FieldInfo_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long FieldInfo_ID
        { 
            get{return _FieldInfo_ID;}
            set{
            base.PrimaryKeyID = _FieldInfo_ID;
            SetProperty(ref _FieldInfo_ID, value);
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

        private string _EntityName;
        /// <summary>
        /// 实体名称
        /// </summary>
        [AdvQueryAttribute(ColName = "EntityName",ColDesc = "实体名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "EntityName" ,Length=50,IsNullable = true,ColumnDescription = "实体名称" )]
        public string EntityName
        { 
            get{return _EntityName;}
            set{
            SetProperty(ref _EntityName, value);
            }
        }

        private string _FieldName;
        /// <summary>
        /// 字段名称
        /// </summary>
        [AdvQueryAttribute(ColName = "FieldName",ColDesc = "字段名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "FieldName" ,Length=50,IsNullable = true,ColumnDescription = "字段名称" )]
        public string FieldName
        { 
            get{return _FieldName;}
            set{
            SetProperty(ref _FieldName, value);
            }
        }

        private string _FieldText;
        /// <summary>
        /// 字段显示
        /// </summary>
        [AdvQueryAttribute(ColName = "FieldText",ColDesc = "字段显示")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "FieldText" ,Length=50,IsNullable = true,ColumnDescription = "字段显示" )]
        public string FieldText
        { 
            get{return _FieldText;}
            set{
            SetProperty(ref _FieldText, value);
            }
        }

        private string _ClassPath;
        /// <summary>
        /// 类路径
        /// </summary>
        [AdvQueryAttribute(ColName = "ClassPath",ColDesc = "类路径")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ClassPath" ,Length=500,IsNullable = true,ColumnDescription = "类路径" )]
        public string ClassPath
        { 
            get{return _ClassPath;}
            set{
            SetProperty(ref _ClassPath, value);
            }
        }

        private bool? _IsForm;
        /// <summary>
        /// 是否为窗体
        /// </summary>
        [AdvQueryAttribute(ColName = "IsForm",ColDesc = "是否为窗体")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsForm" ,IsNullable = true,ColumnDescription = "是否为窗体" )]
        public bool? IsForm
        { 
            get{return _IsForm;}
            set{
            SetProperty(ref _IsForm, value);
            }
        }

        private bool _IsEnabled= true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "IsEnabled",ColDesc = "是否启用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsEnabled" ,IsNullable = false,ColumnDescription = "是否启用" )]
        public bool IsEnabled
        { 
            get{return _IsEnabled;}
            set{
            SetProperty(ref _IsEnabled, value);
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

        private string _ChildEntityName;
        /// <summary>
        /// 子表名称
        /// </summary>
        [AdvQueryAttribute(ColName = "ChildEntityName",ColDesc = "子表名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ChildEntityName" ,Length=50,IsNullable = false,ColumnDescription = "子表名称" )]
        public string ChildEntityName
        { 
            get{return _ChildEntityName;}
            set{
            SetProperty(ref _ChildEntityName, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(MenuID))]
        public virtual tb_MenuInfo tb_menuinfo { get; set; }


        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_P4Field.FieldInfo_ID))]
        public virtual List<tb_P4Field> tb_P4Fields { get; set; }
        //tb_P4Field.FieldInfo_ID)
        //FieldInfo_ID.FK_TB_P4FIE_REFERENCE_TB_FIELD)
        //tb_FieldInfo.FieldInfo_ID)


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
                    Type type = typeof(tb_FieldInfo);
                    
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
            tb_FieldInfo loctype = (tb_FieldInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

