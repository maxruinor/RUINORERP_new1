
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/15/2023 17:39:36
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using Csla;
using RUINORERP.Business.UseCsla;
using RUINORERP.Model;

namespace RUINORERP.Business.UseCsla
{
    /// <summary>
    /// 收藏表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Favorite")]
    public class tb_FavoriteInfo:ReadOnlyBase<tb_FavoriteInfo>
    {
        public tb_FavoriteInfo()
        {
            FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_Favorite" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    
        public static readonly PropertyInfo<long> IDProperty = RegisterProperty<long>(c => c.ID);
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ID",IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        [Display(Name = "")]
        public long ID 
        { 
            get{return GetProperty(IDProperty);}
          private  set{ LoadProperty(IDProperty, value); }
        }

        public static readonly PropertyInfo<long?> ReferenceIDProperty = RegisterProperty<long?>(c => c.ReferenceID);
        
        
        /// <summary>
        /// 引用ID
        /// </summary>
        [SugarColumn(ColumnName = "ReferenceID",IsNullable = true,ColumnDescription = "引用ID" )]
        [Display(Name = "引用ID")]
        public long? ReferenceID 
        { 
            get{return GetProperty(ReferenceIDProperty);}
          private  set{ LoadProperty(ReferenceIDProperty, value); }
        }

        public static readonly PropertyInfo<string> Ref_Table_NameProperty = RegisterProperty<string>(c => c.Ref_Table_Name);
        
        
        /// <summary>
        /// 引用表名
        /// </summary>
        [SugarColumn(ColumnName = "Ref_Table_Name",Length=100,IsNullable = true,ColumnDescription = "引用表名" )]
        [Display(Name = "引用表名")]
        public string Ref_Table_Name 
        { 
            get{return GetProperty(Ref_Table_NameProperty);}
          private  set{ LoadProperty(Ref_Table_NameProperty, value); }
        }

        public static readonly PropertyInfo<string> ModuleNameProperty = RegisterProperty<string>(c => c.ModuleName);
        
        
        /// <summary>
        /// 模块名
        /// </summary>
        [SugarColumn(ColumnName = "ModuleName",Length=255,IsNullable = true,ColumnDescription = "模块名" )]
        [Display(Name = "模块名")]
        public string ModuleName 
        { 
            get{return GetProperty(ModuleNameProperty);}
          private  set{ LoadProperty(ModuleNameProperty, value); }
        }

        public static readonly PropertyInfo<string> BusinessTypeProperty = RegisterProperty<string>(c => c.BusinessType);
        
        
        /// <summary>
        /// 业务类型
        /// </summary>
        [SugarColumn(ColumnName = "BusinessType",Length=255,IsNullable = true,ColumnDescription = "业务类型" )]
        [Display(Name = "业务类型")]
        public string BusinessType 
        { 
            get{return GetProperty(BusinessTypeProperty);}
          private  set{ LoadProperty(BusinessTypeProperty, value); }
        }

        public static readonly PropertyInfo<bool> Publi_enabledProperty = RegisterProperty<bool>(c => c.Publi_enabled);
        
        
        /// <summary>
        /// 是否公开
        /// </summary>
        [SugarColumn(ColumnName = "Publi_enabled",IsNullable = false,ColumnDescription = "是否公开" )]
        [Display(Name = "是否公开")]
        public bool Publi_enabled 
        { 
            get{return GetProperty(Publi_enabledProperty);}
          private  set{ LoadProperty(Publi_enabledProperty, value); }
        }

        public static readonly PropertyInfo<bool> is_enabledProperty = RegisterProperty<bool>(c => c.is_enabled);
        
        
        /// <summary>
        /// 是否启用
        /// </summary>
        [SugarColumn(ColumnName = "is_enabled",IsNullable = false,ColumnDescription = "是否启用" )]
        [Display(Name = "是否启用")]
        public bool is_enabled 
        { 
            get{return GetProperty(is_enabledProperty);}
          private  set{ LoadProperty(is_enabledProperty, value); }
        }

        public static readonly PropertyInfo<bool> is_availableProperty = RegisterProperty<bool>(c => c.is_available);
        
        
        /// <summary>
        /// 是否可用
        /// </summary>
        [SugarColumn(ColumnName = "is_available",IsNullable = false,ColumnDescription = "是否可用" )]
        [Display(Name = "是否可用")]
        public bool is_available 
        { 
            get{return GetProperty(is_availableProperty);}
          private  set{ LoadProperty(is_availableProperty, value); }
        }

        public static readonly PropertyInfo<string> NotesProperty = RegisterProperty<string>(c => c.Notes);
        
        
        /// <summary>
        /// 备注说明
        /// </summary>
        [SugarColumn(ColumnName = "Notes",Length=2147483647,IsNullable = true,ColumnDescription = "备注说明" )]
        [Display(Name = "备注说明")]
        public string Notes 
        { 
            get{return GetProperty(NotesProperty);}
          private  set{ LoadProperty(NotesProperty, value); }
        }

        public static readonly PropertyInfo<DateTime?> Created_atProperty = RegisterProperty<DateTime?>(c => c.Created_at);
        
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "Created_at",IsNullable = true,ColumnDescription = "创建时间" )]
        [Display(Name = "创建时间")]
        public DateTime? Created_at 
        { 
            get{return GetProperty(Created_atProperty);}
          private  set{ LoadProperty(Created_atProperty, value); }
        }

        public static readonly PropertyInfo<long?> Owner_byProperty = RegisterProperty<long?>(c => c.Owner_by);
        
        
        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(ColumnName = "Owner_by",IsNullable = true,ColumnDescription = "创建人" )]
        [Display(Name = "创建人")]
        public long? Owner_by 
        { 
            get{return GetProperty(Owner_byProperty);}
          private  set{ LoadProperty(Owner_byProperty, value); }
        }

        public static readonly PropertyInfo<long?> Created_byProperty = RegisterProperty<long?>(c => c.Created_by);
        
        
        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(ColumnName = "Created_by",IsNullable = true,ColumnDescription = "创建人" )]
        [Display(Name = "创建人")]
        public long? Created_by 
        { 
            get{return GetProperty(Created_byProperty);}
          private  set{ LoadProperty(Created_byProperty, value); }
        }

        public static readonly PropertyInfo<DateTime?> Modified_atProperty = RegisterProperty<DateTime?>(c => c.Modified_at);
        
        
        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(ColumnName = "Modified_at",IsNullable = true,ColumnDescription = "修改时间" )]
        [Display(Name = "修改时间")]
        public DateTime? Modified_at 
        { 
            get{return GetProperty(Modified_atProperty);}
          private  set{ LoadProperty(Modified_atProperty, value); }
        }

        public static readonly PropertyInfo<long?> Modified_byProperty = RegisterProperty<long?>(c => c.Modified_by);
        
        
        /// <summary>
        /// 修改人
        /// </summary>
        [SugarColumn(ColumnName = "Modified_by",IsNullable = true,ColumnDescription = "修改人" )]
        [Display(Name = "修改人")]
        public long? Modified_by 
        { 
            get{return GetProperty(Modified_byProperty);}
          private  set{ LoadProperty(Modified_byProperty, value); }
        }







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
        [Description("列名中文描述"), Category("自定属性"), Browsable(true)]
        [SugarColumn(IsIgnore = true)]
        public ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, string>();
                    SugarColumn entityAttr;
                    Type type = typeof(tb_FavoriteInfo);
                    
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
        
       



    }
}

