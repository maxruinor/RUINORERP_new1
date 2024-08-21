
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/15/2023 17:39:37
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
    [Serializable]
    public class tb_FavoriteEditInfo:BusinessBase<tb_FavoriteEditInfo>
    {
        public tb_FavoriteEditInfo()
        {
           // FieldNameList = fieldNameList;
        }

        public static readonly PropertyInfo<long> IDProperty = RegisterProperty<long>(c => c.ID);
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ID",IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        [Display(Name = "")]
        public long ID 
        {    
         get=> GetProperty(IDProperty);
         set=> SetProperty(IDProperty, value); 
        }
        public static readonly PropertyInfo<long?> ReferenceIDProperty = RegisterProperty<long?>(c => c.ReferenceID);
        
        
        /// <summary>
        /// 引用ID
        /// </summary>
        [SugarColumn(ColumnName = "ReferenceID",IsNullable = true,ColumnDescription = "引用ID" )]
        [Display(Name = "引用ID")]
        public long? ReferenceID 
        {    
         get=> GetProperty(ReferenceIDProperty);
         set=> SetProperty(ReferenceIDProperty, value); 
        }
        public static readonly PropertyInfo<string> Ref_Table_NameProperty = RegisterProperty<string>(c => c.Ref_Table_Name);
        
        
        /// <summary>
        /// 引用表名
        /// </summary>
        [SugarColumn(ColumnName = "Ref_Table_Name",Length=100,IsNullable = true,ColumnDescription = "引用表名" )]
        [Display(Name = "引用表名")]
        public string Ref_Table_Name 
        {    
         get=> GetProperty(Ref_Table_NameProperty);
         set=> SetProperty(Ref_Table_NameProperty, value); 
        }
        public static readonly PropertyInfo<string> ModuleNameProperty = RegisterProperty<string>(c => c.ModuleName);
        
        
        /// <summary>
        /// 模块名
        /// </summary>
        [SugarColumn(ColumnName = "ModuleName",Length=255,IsNullable = true,ColumnDescription = "模块名" )]
        [Display(Name = "模块名")]
        public string ModuleName 
        {    
         get=> GetProperty(ModuleNameProperty);
         set=> SetProperty(ModuleNameProperty, value); 
        }
        public static readonly PropertyInfo<string> BusinessTypeProperty = RegisterProperty<string>(c => c.BusinessType);
        
        
        /// <summary>
        /// 业务类型
        /// </summary>
        [SugarColumn(ColumnName = "BusinessType",Length=255,IsNullable = true,ColumnDescription = "业务类型" )]
        [Display(Name = "业务类型")]
        public string BusinessType 
        {    
         get=> GetProperty(BusinessTypeProperty);
         set=> SetProperty(BusinessTypeProperty, value); 
        }
        public static readonly PropertyInfo<bool> Publi_enabledProperty = RegisterProperty<bool>(c => c.Publi_enabled);
        
        
        /// <summary>
        /// 是否公开
        /// </summary>
        [SugarColumn(ColumnName = "Publi_enabled",IsNullable = false,ColumnDescription = "是否公开" )]
        [Display(Name = "是否公开")]
        public bool Publi_enabled 
        {    
         get=> GetProperty(Publi_enabledProperty);
         set=> SetProperty(Publi_enabledProperty, value); 
        }
        public static readonly PropertyInfo<bool> is_enabledProperty = RegisterProperty<bool>(c => c.is_enabled);
        
        
        /// <summary>
        /// 是否启用
        /// </summary>
        [SugarColumn(ColumnName = "is_enabled",IsNullable = false,ColumnDescription = "是否启用" )]
        [Display(Name = "是否启用")]
        public bool is_enabled 
        {    
         get=> GetProperty(is_enabledProperty);
         set=> SetProperty(is_enabledProperty, value); 
        }
        public static readonly PropertyInfo<bool> is_availableProperty = RegisterProperty<bool>(c => c.is_available);
        
        
        /// <summary>
        /// 是否可用
        /// </summary>
        [SugarColumn(ColumnName = "is_available",IsNullable = false,ColumnDescription = "是否可用" )]
        [Display(Name = "是否可用")]
        public bool is_available 
        {    
         get=> GetProperty(is_availableProperty);
         set=> SetProperty(is_availableProperty, value); 
        }
        public static readonly PropertyInfo<string> NotesProperty = RegisterProperty<string>(c => c.Notes);
        
        
        /// <summary>
        /// 备注说明
        /// </summary>
        [SugarColumn(ColumnName = "Notes",Length=2147483647,IsNullable = true,ColumnDescription = "备注说明" )]
        [Display(Name = "备注说明")]
        public string Notes 
        {    
         get=> GetProperty(NotesProperty);
         set=> SetProperty(NotesProperty, value); 
        }
        public static readonly PropertyInfo<DateTime?> Created_atProperty = RegisterProperty<DateTime?>(c => c.Created_at);
        
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "Created_at",IsNullable = true,ColumnDescription = "创建时间" )]
        [Display(Name = "创建时间")]
        public DateTime? Created_at 
        {    
         get=> GetProperty(Created_atProperty);
         set=> SetProperty(Created_atProperty, value); 
        }
        public static readonly PropertyInfo<long?> Owner_byProperty = RegisterProperty<long?>(c => c.Owner_by);
        
        
        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(ColumnName = "Owner_by",IsNullable = true,ColumnDescription = "创建人" )]
        [Display(Name = "创建人")]
        public long? Owner_by 
        {    
         get=> GetProperty(Owner_byProperty);
         set=> SetProperty(Owner_byProperty, value); 
        }
        public static readonly PropertyInfo<long?> Created_byProperty = RegisterProperty<long?>(c => c.Created_by);
        
        
        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(ColumnName = "Created_by",IsNullable = true,ColumnDescription = "创建人" )]
        [Display(Name = "创建人")]
        public long? Created_by 
        {    
         get=> GetProperty(Created_byProperty);
         set=> SetProperty(Created_byProperty, value); 
        }
        public static readonly PropertyInfo<DateTime?> Modified_atProperty = RegisterProperty<DateTime?>(c => c.Modified_at);
        
        
        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(ColumnName = "Modified_at",IsNullable = true,ColumnDescription = "修改时间" )]
        [Display(Name = "修改时间")]
        public DateTime? Modified_at 
        {    
         get=> GetProperty(Modified_atProperty);
         set=> SetProperty(Modified_atProperty, value); 
        }
        public static readonly PropertyInfo<long?> Modified_byProperty = RegisterProperty<long?>(c => c.Modified_by);
        
        
        /// <summary>
        /// 修改人
        /// </summary>
        [SugarColumn(ColumnName = "Modified_by",IsNullable = true,ColumnDescription = "修改人" )]
        [Display(Name = "修改人")]
        public long? Modified_by 
        {    
         get=> GetProperty(Modified_byProperty);
         set=> SetProperty(Modified_byProperty, value); 
        }



    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      /*
      BusinessRules.AddRule(new InfoText(NameProperty, "Person name (required)"));
      BusinessRules.AddRule(new CheckCase(NameProperty));
      BusinessRules.AddRule(new NoZAllowed(NameProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(NameProperty));
            BusinessRules.AddRule(
        new StartDateGTEndDate { 
          PrimaryProperty = StartedProperty, 
          AffectedProperties = { EndedProperty } });
      BusinessRules.AddRule(
        new StartDateGTEndDate { 
          PrimaryProperty = EndedProperty, 
          AffectedProperties = { StartedProperty } });

      BusinessRules.AddRule(
        new Csla.Rules.CommonRules.IsInRole(
          Csla.Rules.AuthorizationActions.WriteProperty, 
          NameProperty, 
          "ProjectManager"));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.IsInRole(
        Csla.Rules.AuthorizationActions.WriteProperty, StartedProperty, Security.Roles.ProjectManager));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.IsInRole(
        Csla.Rules.AuthorizationActions.WriteProperty, EndedProperty, Security.Roles.ProjectManager));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.IsInRole(
        Csla.Rules.AuthorizationActions.WriteProperty, DescriptionProperty, Security.Roles.ProjectManager));
      BusinessRules.AddRule(new NoDuplicateResource { PrimaryProperty = ResourcesProperty });
      
      */
    }
    


    [EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    [ObjectAuthorizationRules]
    public static void AddObjectAuthorizationRules()
    {
        //暂时指定固定的，实现是来自数据库
      Csla.Rules.BusinessRules.AddRule(typeof(tb_FavoriteEditInfo),new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.CreateObject,Security.Roles.ProjectManager, Security.Roles.Administrator));
      Csla.Rules.BusinessRules.AddRule(typeof(tb_FavoriteEditInfo),new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.EditObject, Security.Roles.Administrator));
      Csla.Rules.BusinessRules.AddRule(typeof(tb_FavoriteEditInfo),new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.DeleteObject, Security.Roles.ProjectManager, Security.Roles.Administrator));
       //暂时指定固定的两个，实现是来自数据库
      Csla.Rules.BusinessRules.AddRule(typeof(tb_FavoriteEditInfo), new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.ExecuteMethod, Security.Roles.ProjectManager, Security.Roles.Administrator));
      Csla.Rules.BusinessRules.AddRule(typeof(tb_FavoriteEditInfo), new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.GetObject, Security.Roles.ProjectManager, Security.Roles.Administrator));
      Csla.Rules.BusinessRules.AddRule(typeof(tb_FavoriteEditInfo), new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.ReadProperty, Security.Roles.ProjectManager, Security.Roles.Administrator));
      Csla.Rules.BusinessRules.AddRule(typeof(tb_FavoriteEditInfo), new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.WriteProperty, Security.Roles.ProjectManager, Security.Roles.Administrator));
    }
    
    
    
        #region csla mothed
 /*
 
 
 
     [Create, RunLocal]
    private void Create()
    {
    ID=-1;
      BusinessRules.CheckRules();
    }

    [Fetch]
    private void Fetch(int id, [Inject]Itb_FavoriteDal dal)
    {
      var data = dal.Fetch(ID);
      using (BypassPropertyChecks)
      {
        Csla.Data.DataMapper.Map(data, this);
      }
      BusinessRules.CheckRules();
    }

    [Insert]
    private void Insert([Inject]Itb_FavoriteDal dal)
    {
      using (BypassPropertyChecks)
      {
        var data = new tb_Favorite
        {
          ID=ID,
          ReferenceID=ReferenceID,
          Ref_Table_Name=Ref_Table_Name,
          ModuleName=ModuleName,
          BusinessType=BusinessType,
          Publi_enabled=Publi_enabled,
          is_enabled=is_enabled,
          is_available=is_available,
          Notes=Notes,
          Created_at=Created_at,
          Owner_by=Owner_by,
          Created_by=Created_by,
          Modified_at=Modified_at,
          Modified_by=Modified_by,
         
        };
         dal.Insert(data);
        ID = data.ID;
      }
    }

    [Update]
    private void Update([Inject]Itb_FavoriteDal dal)
    {
      using (BypassPropertyChecks)
      {
        var data = new tb_Favorite
        {
          ReferenceID=ReferenceID,
          Ref_Table_Name=Ref_Table_Name,
          ModuleName=ModuleName,
          BusinessType=BusinessType,
          Publi_enabled=Publi_enabled,
          is_enabled=is_enabled,
          is_available=is_available,
          Notes=Notes,
          Created_at=Created_at,
          Owner_by=Owner_by,
          Created_by=Created_by,
          Modified_at=Modified_at,
          Modified_by=Modified_by,

        };
        dal.Update(data);
      }
    }

    [DeleteSelf]
    private void DeleteSelf([Inject]Itb_FavoriteDal dal)
    {
      Delete(ReadProperty(IDProperty), dal);
    }

    [Delete]
    private void Delete(int id, [Inject]Itb_FavoriteDal dal)
    {
      dal.Delete(ID);
    }

        */


        #endregion
        
        
        
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
                    Type type = typeof(tb_FavoriteEditInfo);
                    
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

