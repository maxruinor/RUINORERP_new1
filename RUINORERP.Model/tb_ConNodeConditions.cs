
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:46:50
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
    /// 流程步骤 为转移条件集合，Field为条件左参数，Operator为操作操作符如果值类型为String则表达式只能为==或者!=，Value为表达式值
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ConNodeConditions")]
    public partial class tb_ConNodeConditions: BaseEntity, ICloneable
    {
        public tb_ConNodeConditions()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_ConNodeConditions" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ConNodeConditions_Id;
        /// <summary>
        /// 条件
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ConNodeConditions_Id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "条件" , IsPrimaryKey = true)]
        public long ConNodeConditions_Id
        { 
            get{return _ConNodeConditions_Id;}
            set{
            base.PrimaryKeyID = _ConNodeConditions_Id;
            SetProperty(ref _ConNodeConditions_Id, value);
            }
        }

        private string _Field;
        /// <summary>
        /// 表达式
        /// </summary>
        [AdvQueryAttribute(ColName = "Field",ColDesc = "表达式")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Field" ,Length=50,IsNullable = false,ColumnDescription = "表达式" )]
        public string Field
        { 
            get{return _Field;}
            set{
            SetProperty(ref _Field, value);
            }
        }

        private string _Operator;
        /// <summary>
        /// 操作符
        /// </summary>
        [AdvQueryAttribute(ColName = "Operator",ColDesc = "操作符")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Operator" ,Length=50,IsNullable = true,ColumnDescription = "操作符" )]
        public string Operator
        { 
            get{return _Operator;}
            set{
            SetProperty(ref _Operator, value);
            }
        }

        private string _Value;
        /// <summary>
        /// 表达式值
        /// </summary>
        [AdvQueryAttribute(ColName = "Value",ColDesc = "表达式值")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Value" ,Length=50,IsNullable = true,ColumnDescription = "表达式值" )]
        public string Value
        { 
            get{return _Value;}
            set{
            SetProperty(ref _Value, value);
            }
        }

        #endregion

        #region 扩展属性

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_NextNodes.ConNodeConditions_Id))]
        public virtual List<tb_NextNodes> tb_NextNodeses { get; set; }
        //tb_NextNodes.ConNodeConditions_Id)
        //ConNodeConditions_Id.FK_TB_NEXTN_REFERENCE_TB_CONNO)
        //tb_ConNodeConditions.ConNodeConditions_Id)


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
                    Type type = typeof(tb_ConNodeConditions);
                    
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
            tb_ConNodeConditions loctype = (tb_ConNodeConditions)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

