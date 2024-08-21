
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/20/2023 14:24:53
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
    /// 用户表
    /// </summary>
    [Serializable()]
    [Csla.Server.ObjectFactory(typeof(tb_UserInfoFactory))]
    public class tb_UserInfoEditInfo:BusinessBase<tb_UserInfoEditInfo>
    {
        public tb_UserInfoEditInfo()
        {
           // FieldNameList = fieldNameList;
        }

        public static readonly PropertyInfo<int> User_IDProperty = RegisterProperty<int>(c => c.User_ID);
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "User_ID",IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true, IsIdentity = true)]
        [Display(Name = "")]
        public int User_ID 
        {    
         get=> GetProperty(User_IDProperty);
         set=> SetProperty(User_IDProperty, value); 
        }
        public static readonly PropertyInfo<int> Employee_IDProperty = RegisterProperty<int>(c => c.Employee_ID);
        
        
        /// <summary>
        /// 员工
        /// </summary>
        [SugarColumn(ColumnName = "Employee_ID",IsNullable = false,ColumnDescription = "员工" )]
        [Display(Name = "员工")]
        public int Employee_ID 
        {    
         get=> GetProperty(Employee_IDProperty);
         set=> SetProperty(Employee_IDProperty, value); 
        }
        public static readonly PropertyInfo<string> UserNameProperty = RegisterProperty<string>(c => c.UserName);
        
        
        /// <summary>
        /// 用户名
        /// </summary>
        [SugarColumn(ColumnName = "UserName",Length=255,IsNullable = false,ColumnDescription = "用户名" )]
        [Display(Name = "用户名")]
        public string UserName 
        {    
         get=> GetProperty(UserNameProperty);
         set=> SetProperty(UserNameProperty, value); 
        }
        public static readonly PropertyInfo<string> PasswordProperty = RegisterProperty<string>(c => c.Password);
        
        
        /// <summary>
        /// 密码
        /// </summary>
        [SugarColumn(ColumnName = "Password",Length=255,IsNullable = true,ColumnDescription = "密码" )]
        [Display(Name = "密码")]
        public string Password 
        {    
         get=> GetProperty(PasswordProperty);
         set=> SetProperty(PasswordProperty, value); 
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
        public static readonly PropertyInfo<int?> Created_byProperty = RegisterProperty<int?>(c => c.Created_by);
        
        
        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(ColumnName = "Created_by",IsNullable = true,ColumnDescription = "创建人" )]
        [Display(Name = "创建人")]
        public int? Created_by 
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
        public static readonly PropertyInfo<int?> Modified_byProperty = RegisterProperty<int?>(c => c.Modified_by);
        
        
        /// <summary>
        /// 修改人
        /// </summary>
        [SugarColumn(ColumnName = "Modified_by",IsNullable = true,ColumnDescription = "修改人" )]
        [Display(Name = "修改人")]
        public int? Modified_by 
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
      Csla.Rules.BusinessRules.AddRule(typeof(tb_UserInfoEditInfo),new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.CreateObject,Security.Roles.ProjectManager, Security.Roles.Administrator));
      Csla.Rules.BusinessRules.AddRule(typeof(tb_UserInfoEditInfo),new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.EditObject, Security.Roles.Administrator));
      Csla.Rules.BusinessRules.AddRule(typeof(tb_UserInfoEditInfo),new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.DeleteObject, Security.Roles.ProjectManager, Security.Roles.Administrator));
       //暂时指定固定的两个，实现是来自数据库
      Csla.Rules.BusinessRules.AddRule(typeof(tb_UserInfoEditInfo), new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.ExecuteMethod, Security.Roles.ProjectManager, Security.Roles.Administrator));
      Csla.Rules.BusinessRules.AddRule(typeof(tb_UserInfoEditInfo), new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.GetObject, Security.Roles.ProjectManager, Security.Roles.Administrator));
      Csla.Rules.BusinessRules.AddRule(typeof(tb_UserInfoEditInfo), new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.ReadProperty, Security.Roles.ProjectManager, Security.Roles.Administrator));
      Csla.Rules.BusinessRules.AddRule(typeof(tb_UserInfoEditInfo), new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.WriteProperty, Security.Roles.ProjectManager, Security.Roles.Administrator));
    }
    
    
    
        #region csla mothed
 
     [Create, RunLocal]
    private void Create()
    {
    User_ID=-1;
      BusinessRules.CheckRules();
    }

    [Fetch]
    private void Fetch(int id, [Inject]Itb_UserInfoDal dal)
    {
      var data = dal.Fetch(User_ID);
      using (BypassPropertyChecks)
      {
        Csla.Data.DataMapper.Map(data, this);
      }
      BusinessRules.CheckRules();
    }

    [Insert]
    private void Insert([Inject]Itb_UserInfoDal dal)
    {
      using (BypassPropertyChecks)
      {
        var data = new tb_UserInfo
        {
          User_ID=User_ID,
          Employee_ID=Employee_ID,
          UserName=UserName,
          Password=Password,
          is_enabled=is_enabled,
          is_available=is_available,
          Notes=Notes,
          Created_at=Created_at,
          Created_by=Created_by,
          Modified_at=Modified_at,
          Modified_by=Modified_by,
         
        };
         dal.Insert(data);
        User_ID = data.User_ID;
      }
    }

    [Update]
    private void Update([Inject]Itb_UserInfoDal dal)
    {
      using (BypassPropertyChecks)
      {
        var data = new tb_UserInfo
        {
          Employee_ID=Employee_ID,
          UserName=UserName,
          Password=Password,
          is_enabled=is_enabled,
          is_available=is_available,
          Notes=Notes,
          Created_at=Created_at,
          Created_by=Created_by,
          Modified_at=Modified_at,
          Modified_by=Modified_by,

        };
        dal.Update(data);
      }
    }

    [DeleteSelf]
    private void DeleteSelf([Inject]Itb_UserInfoDal dal)
    {
      Delete(ReadProperty(User_IDProperty), dal);
    }

    [Delete]
    private void Delete(int id, [Inject]Itb_UserInfoDal dal)
    {
      dal.Delete(User_ID);
    }

        


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
                    Type type = typeof(tb_UserInfoInfo);
                    
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

