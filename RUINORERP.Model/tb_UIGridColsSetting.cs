
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
    /// UI表格列设置
    /// </summary>
    [Serializable()]
    [Description("UI表格列设置")]
    [SugarTable("tb_UIGridColsSetting")]
    public partial class tb_UIGridColsSetting: BaseEntity, ICloneable
    {
        public tb_UIGridColsSetting()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("UI表格列设置tb_UIGridColsSetting" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _UIGCID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "UIGCID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long UIGCID
        { 
            get{return _UIGCID;}
            set{
            base.PrimaryKeyID = _UIGCID;
            SetProperty(ref _UIGCID, value);
            }
        }

        private long? _FieldInfo_ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "FieldInfo_ID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "FieldInfo_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        public long? FieldInfo_ID
        { 
            get{return _FieldInfo_ID;}
            set{
            SetProperty(ref _FieldInfo_ID, value);
            }
        }

        private int _ColDisplayIndex;
        /// <summary>
        /// 显示排序
        /// </summary>
        [AdvQueryAttribute(ColName = "ColDisplayIndex",ColDesc = "显示排序")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ColDisplayIndex" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "显示排序" )]
        public int ColDisplayIndex
        { 
            get{return _ColDisplayIndex;}
            set{
            SetProperty(ref _ColDisplayIndex, value);
            }
        }

        private int? _Sort;
        /// <summary>
        /// 数据排序
        /// </summary>
        [AdvQueryAttribute(ColName = "Sort",ColDesc = "数据排序")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Sort" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "数据排序" )]
        public int? Sort
        { 
            get{return _Sort;}
            set{
            SetProperty(ref _Sort, value);
            }
        }

        private bool _Visible;
        /// <summary>
        /// 是否可见
        /// </summary>
        [AdvQueryAttribute(ColName = "Visible",ColDesc = "是否可见")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Visible" ,IsNullable = false,ColumnDescription = "是否可见" )]
        public bool Visible
        { 
            get{return _Visible;}
            set{
            SetProperty(ref _Visible, value);
            }
        }

        private int? _ColWith;
        /// <summary>
        /// 值类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ColWith",ColDesc = "值类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ColWith" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "值类型" )]
        public int? ColWith
        { 
            get{return _ColWith;}
            set{
            SetProperty(ref _ColWith, value);
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
                    Type type = typeof(tb_UIGridColsSetting);
                    
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
            tb_UIGridColsSetting loctype = (tb_UIGridColsSetting)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

