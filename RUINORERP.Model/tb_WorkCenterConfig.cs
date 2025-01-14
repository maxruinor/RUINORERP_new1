
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/14/2025 18:57:16
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
    /// 工作台配置表
    /// </summary>
    [Serializable()]
    [Description("工作台配置表")]
    [SugarTable("tb_WorkCenterConfig")]
    public partial class tb_WorkCenterConfig: BaseEntity, ICloneable
    {
        public tb_WorkCenterConfig()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("工作台配置表tb_WorkCenterConfig" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ConfigID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ConfigID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long ConfigID
        { 
            get{return _ConfigID;}
            set{
            base.PrimaryKeyID = _ConfigID;
            SetProperty(ref _ConfigID, value);
            }
        }

        private long _RoleID;
        /// <summary>
        /// 角色
        /// </summary>
        [AdvQueryAttribute(ColName = "RoleID",ColDesc = "角色")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RoleID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "角色" )]
        public long RoleID
        { 
            get{return _RoleID;}
            set{
            SetProperty(ref _RoleID, value);
            }
        }

        private long? _User_ID;
        /// <summary>
        /// 用户
        /// </summary>
        [AdvQueryAttribute(ColName = "User_ID",ColDesc = "用户")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "User_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "用户" )]
        public long? User_ID
        { 
            get{return _User_ID;}
            set{
            SetProperty(ref _User_ID, value);
            }
        }

        private bool _Operable= false;
        /// <summary>
        /// 可操作
        /// </summary>
        [AdvQueryAttribute(ColName = "Operable",ColDesc = "可操作")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Operable" ,IsNullable = false,ColumnDescription = "可操作" )]
        public bool Operable
        { 
            get{return _Operable;}
            set{
            SetProperty(ref _Operable, value);
            }
        }

        private bool _OnlyDisplay= false;
        /// <summary>
        /// 仅展示
        /// </summary>
        [AdvQueryAttribute(ColName = "OnlyDisplay",ColDesc = "仅展示")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "OnlyDisplay" ,IsNullable = false,ColumnDescription = "仅展示" )]
        public bool OnlyDisplay
        { 
            get{return _OnlyDisplay;}
            set{
            SetProperty(ref _OnlyDisplay, value);
            }
        }

        private string _ToDoList;
        /// <summary>
        /// 待办事项
        /// </summary>
        [AdvQueryAttribute(ColName = "ToDoList",ColDesc = "待办事项")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "ToDoList" ,Length=500,IsNullable = true,ColumnDescription = "待办事项" )]
        public string ToDoList
        { 
            get{return _ToDoList;}
            set{
            SetProperty(ref _ToDoList, value);
            }
        }

        private string _FrequentlyMenus;
        /// <summary>
        /// 常用菜单
        /// </summary>
        [AdvQueryAttribute(ColName = "FrequentlyMenus",ColDesc = "常用菜单")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "FrequentlyMenus" ,Length=200,IsNullable = true,ColumnDescription = "常用菜单" )]
        public string FrequentlyMenus
        { 
            get{return _FrequentlyMenus;}
            set{
            SetProperty(ref _FrequentlyMenus, value);
            }
        }

        private string _DataOverview;
        /// <summary>
        /// 数据概览
        /// </summary>
        [AdvQueryAttribute(ColName = "DataOverview",ColDesc = "数据概览")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "DataOverview" ,Length=500,IsNullable = true,ColumnDescription = "数据概览" )]
        public string DataOverview
        { 
            get{return _DataOverview;}
            set{
            SetProperty(ref _DataOverview, value);
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
                    Type type = typeof(tb_WorkCenterConfig);
                    
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
            tb_WorkCenterConfig loctype = (tb_WorkCenterConfig)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

