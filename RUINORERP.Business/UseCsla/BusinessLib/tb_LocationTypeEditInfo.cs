
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/20/2023 14:44:54
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
    /// 库位类别
    /// </summary>
    [Serializable()]
    [Csla.Server.ObjectFactory(typeof(tb_LocationTypeFactory))]
    public class tb_LocationTypeEditInfo:BusinessBase<tb_LocationTypeEditInfo>
    {
        public tb_LocationTypeEditInfo()
        {
           // FieldNameList = fieldNameList;
        }

        public static readonly PropertyInfo<int> LocationType_IDProperty = RegisterProperty<int>(c => c.LocationType_ID);
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "LocationType_ID",IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true, IsIdentity = true)]
        [Display(Name = "")]
        public int LocationType_ID 
        {    
         get=> GetProperty(LocationType_IDProperty);
         set=> SetProperty(LocationType_IDProperty, value); 
        }
        public static readonly PropertyInfo<string> TypeNameProperty = RegisterProperty<string>(c => c.TypeName);
        
        
        /// <summary>
        /// 类型名称
        /// </summary>
        [SugarColumn(ColumnName = "TypeName",Length=50,IsNullable = false,ColumnDescription = "类型名称" )]
        [Display(Name = "类型名称")]
        public string TypeName 
        {    
         get=> GetProperty(TypeNameProperty);
         set=> SetProperty(TypeNameProperty, value); 
        }
        public static readonly PropertyInfo<string> DescProperty = RegisterProperty<string>(c => c.Desc);
        
        
        /// <summary>
        /// 描述
        /// </summary>
        [SugarColumn(ColumnName = "Desc",Length=100,IsNullable = true,ColumnDescription = "描述" )]
        [Display(Name = "描述")]
        public string Desc 
        {    
         get=> GetProperty(DescProperty);
         set=> SetProperty(DescProperty, value); 
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
      Csla.Rules.BusinessRules.AddRule(typeof(tb_LocationTypeEditInfo),new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.CreateObject,Security.Roles.ProjectManager, Security.Roles.Administrator));
      Csla.Rules.BusinessRules.AddRule(typeof(tb_LocationTypeEditInfo),new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.EditObject, Security.Roles.Administrator));
      Csla.Rules.BusinessRules.AddRule(typeof(tb_LocationTypeEditInfo),new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.DeleteObject, Security.Roles.ProjectManager, Security.Roles.Administrator));
       //暂时指定固定的两个，实现是来自数据库
      Csla.Rules.BusinessRules.AddRule(typeof(tb_LocationTypeEditInfo), new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.ExecuteMethod, Security.Roles.ProjectManager, Security.Roles.Administrator));
      Csla.Rules.BusinessRules.AddRule(typeof(tb_LocationTypeEditInfo), new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.GetObject, Security.Roles.ProjectManager, Security.Roles.Administrator));
      Csla.Rules.BusinessRules.AddRule(typeof(tb_LocationTypeEditInfo), new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.ReadProperty, Security.Roles.ProjectManager, Security.Roles.Administrator));
      Csla.Rules.BusinessRules.AddRule(typeof(tb_LocationTypeEditInfo), new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.WriteProperty, Security.Roles.ProjectManager, Security.Roles.Administrator));
    }
    
    
    
        #region csla mothed
 /*
 
 
 
     [Create, RunLocal]
    private void Create()
    {
    LocationType_ID=-1;
      BusinessRules.CheckRules();
    }

    [Fetch]
    private void Fetch(int id, [Inject]Itb_LocationTypeDal dal)
    {
      var data = dal.Fetch(LocationType_ID);
      using (BypassPropertyChecks)
      {
        Csla.Data.DataMapper.Map(data, this);
      }
      BusinessRules.CheckRules();
    }

    [Insert]
    private void Insert([Inject]Itb_LocationTypeDal dal)
    {
      using (BypassPropertyChecks)
      {
        var data = new tb_LocationType
        {
          LocationType_ID=LocationType_ID,
          TypeName=TypeName,
          Desc=Desc,
         
        };
         dal.Insert(data);
        LocationType_ID = data.LocationType_ID;
      }
    }

    [Update]
    private void Update([Inject]Itb_LocationTypeDal dal)
    {
      using (BypassPropertyChecks)
      {
        var data = new tb_LocationType
        {
          TypeName=TypeName,
          Desc=Desc,

        };
        dal.Update(data);
      }
    }

    [DeleteSelf]
    private void DeleteSelf([Inject]Itb_LocationTypeDal dal)
    {
      Delete(ReadProperty(LocationType_IDProperty), dal);
    }

    [Delete]
    private void Delete(int id, [Inject]Itb_LocationTypeDal dal)
    {
      dal.Delete(LocationType_ID);
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
                    Type type = typeof(tb_LocationTypeEditInfo);
                    
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

