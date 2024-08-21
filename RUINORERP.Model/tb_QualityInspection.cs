
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:48:46
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
    /// 质检表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_QualityInspection")]
    public partial class tb_QualityInspection: BaseEntity, ICloneable
    {
        public tb_QualityInspection()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_QualityInspection" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _InspectionID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "InspectionID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long InspectionID
        { 
            get{return _InspectionID;}
            set{
            base.PrimaryKeyID = _InspectionID;
            SetProperty(ref _InspectionID, value);
            }
        }

        private DateTime? _InspectionDate;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "InspectionDate",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "InspectionDate" ,IsNullable = true,ColumnDescription = "" )]
        public DateTime? InspectionDate
        { 
            get{return _InspectionDate;}
            set{
            SetProperty(ref _InspectionDate, value);
            }
        }

        private string _InspectionResult;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "InspectionResult",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "InspectionResult" ,Length=500,IsNullable = true,ColumnDescription = "" )]
        public string InspectionResult
        { 
            get{return _InspectionResult;}
            set{
            SetProperty(ref _InspectionResult, value);
            }
        }

        private int? _ProductID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ProductID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ProductID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        public int? ProductID
        { 
            get{return _ProductID;}
            set{
            SetProperty(ref _ProductID, value);
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
                    Type type = typeof(tb_QualityInspection);
                    
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
            tb_QualityInspection loctype = (tb_QualityInspection)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

