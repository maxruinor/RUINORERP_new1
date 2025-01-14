
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/14/2025 18:57:12
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
    /// 步骤变量
    /// </summary>
    [Serializable()]
    [Description("步骤变量")]
    [SugarTable("tb_StepBodyPara")]
    public partial class tb_StepBodyPara: BaseEntity, ICloneable
    {
        public tb_StepBodyPara()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("步骤变量tb_StepBodyPara" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Para_Id;
        /// <summary>
        /// 参数
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Para_Id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "参数" , IsPrimaryKey = true)]
        public long Para_Id
        { 
            get{return _Para_Id;}
            set{
            base.PrimaryKeyID = _Para_Id;
            SetProperty(ref _Para_Id, value);
            }
        }

        private string _Key;
        /// <summary>
        /// 参数key
        /// </summary>
        [AdvQueryAttribute(ColName = "Key",ColDesc = "参数key")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Key" ,Length=50,IsNullable = false,ColumnDescription = "参数key" )]
        public string Key
        { 
            get{return _Key;}
            set{
            SetProperty(ref _Key, value);
            }
        }

        private string _Name;
        /// <summary>
        /// 参数名
        /// </summary>
        [AdvQueryAttribute(ColName = "Name",ColDesc = "参数名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Name" ,Length=50,IsNullable = true,ColumnDescription = "参数名" )]
        public string Name
        { 
            get{return _Name;}
            set{
            SetProperty(ref _Name, value);
            }
        }

        private string _DisplayName;
        /// <summary>
        /// 显示名称
        /// </summary>
        [AdvQueryAttribute(ColName = "DisplayName",ColDesc = "显示名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "DisplayName" ,Length=50,IsNullable = true,ColumnDescription = "显示名称" )]
        public string DisplayName
        { 
            get{return _DisplayName;}
            set{
            SetProperty(ref _DisplayName, value);
            }
        }

        private string _Value;
        /// <summary>
        /// 参数值
        /// </summary>
        [AdvQueryAttribute(ColName = "Value",ColDesc = "参数值")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Value" ,Length=50,IsNullable = true,ColumnDescription = "参数值" )]
        public string Value
        { 
            get{return _Value;}
            set{
            SetProperty(ref _Value, value);
            }
        }

        private string _StepBodyParaType;
        /// <summary>
        /// 参数类型
        /// </summary>
        [AdvQueryAttribute(ColName = "StepBodyParaType",ColDesc = "参数类型")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "StepBodyParaType" ,Length=50,IsNullable = true,ColumnDescription = "参数类型" )]
        public string StepBodyParaType
        { 
            get{return _StepBodyParaType;}
            set{
            SetProperty(ref _StepBodyParaType, value);
            }
        }

        #endregion

        #region 扩展属性

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_StepBody.Para_Id))]
        public virtual List<tb_StepBody> tb_StepBodies { get; set; }
        //tb_StepBody.Para_Id)
        //Para_Id.FK_TB_STEPB_REFERENCE_TB_STEPB)
        //tb_StepBodyPara.Para_Id)


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
                    Type type = typeof(tb_StepBodyPara);
                    
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
            tb_StepBodyPara loctype = (tb_StepBodyPara)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

