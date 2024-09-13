
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:51
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
    /// 系统全局动态配置表 行转列
    /// </summary>
    [Serializable()]
    [Description("tb_SysGlobalDynamicConfig")]
    [SugarTable("tb_SysGlobalDynamicConfig")]
    public partial class tb_SysGlobalDynamicConfig: BaseEntity, ICloneable
    {
        public tb_SysGlobalDynamicConfig()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_SysGlobalDynamicConfig" + "外键ID与对应主主键名称不一致。请修改数据库");
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

        private string _ConfigKey;
        /// <summary>
        /// 配置项
        /// </summary>
        [AdvQueryAttribute(ColName = "ConfigKey",ColDesc = "配置项")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ConfigKey" ,Length=255,IsNullable = false,ColumnDescription = "配置项" )]
        public string ConfigKey
        { 
            get{return _ConfigKey;}
            set{
            SetProperty(ref _ConfigKey, value);
            }
        }

        private string _ConfigValue;
        /// <summary>
        /// 配置值
        /// </summary>
        [AdvQueryAttribute(ColName = "ConfigValue",ColDesc = "配置值")] 
        [SugarColumn(ColumnDataType = "text", SqlParameterDbType ="String",  ColumnName = "ConfigValue" ,Length=2147483647,IsNullable = false,ColumnDescription = "配置值" )]
        public string ConfigValue
        { 
            get{return _ConfigValue;}
            set{
            SetProperty(ref _ConfigValue, value);
            }
        }

        private string _Description;
        /// <summary>
        /// 配置描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Description",ColDesc = "配置描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Description" ,Length=200,IsNullable = false,ColumnDescription = "配置描述" )]
        public string Description
        { 
            get{return _Description;}
            set{
            SetProperty(ref _Description, value);
            }
        }

        private int _ValueType;
        /// <summary>
        /// 配置项的值类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ValueType",ColDesc = "配置项的值类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ValueType" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "配置项的值类型" )]
        public int ValueType
        { 
            get{return _ValueType;}
            set{
            SetProperty(ref _ValueType, value);
            }
        }

        private string _ConfigType;
        /// <summary>
        /// 配置类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ConfigType",ColDesc = "配置类型")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ConfigType" ,Length=100,IsNullable = true,ColumnDescription = "配置类型" )]
        public string ConfigType
        { 
            get{return _ConfigType;}
            set{
            SetProperty(ref _ConfigType, value);
            }
        }

        private bool? _IsActive= true;
        /// <summary>
        /// 启用
        /// </summary>
        [AdvQueryAttribute(ColName = "IsActive",ColDesc = "启用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsActive" ,IsNullable = true,ColumnDescription = "启用" )]
        public bool? IsActive
        { 
            get{return _IsActive;}
            set{
            SetProperty(ref _IsActive, value);
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
                    Type type = typeof(tb_SysGlobalDynamicConfig);
                    
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
            tb_SysGlobalDynamicConfig loctype = (tb_SysGlobalDynamicConfig)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

