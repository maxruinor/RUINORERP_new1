
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/09/2023 01:03:46
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model.Base;

namespace RUINORERP.Model.QueryDto
{
    /// <summary>
    /// 生产计划表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Prod_Plan")]
    public partial class tb_Prod_PlanQueryDto:BaseEntityDto
    {
        public tb_Prod_PlanQueryDto()
        {

        }

    
     

        private int? _id;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "id",ColDesc = "")]
        [SugarColumn(ColumnName = "id",IsNullable = true,ColumnDescription = "" )]
        public int? id 
        { 
            get{return _id;}
            set{SetProperty(ref _id, value);}
        }





/*

        #region 字段描述对应列表
        private ConcurrentDictionary<string, BaseDtoField> fieldNameList;


        /// <summary>
        /// 表列名的中文描述集合
        /// </summary>
        [Description("列名中文描述"), Category("自定属性"), Browsable(true)]
        [SugarColumn(IsIgnore = true)]
        public ConcurrentDictionary<string, BaseDtoField> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, BaseDtoField>();
                    SugarColumn entityAttr;
                    Type type = typeof(tb_Prod_PlanQueryDto);
                    
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

*/


       
    }
}



