
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:29
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
    /// 步骤定义
    /// </summary>
    [Serializable()]
    [Description("步骤定义")]
    [SugarTable("tb_StepBody")]
    public partial class tb_StepBody: BaseEntity, ICloneable
    {
        public tb_StepBody()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("步骤定义tb_StepBody" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _StepBodyld;
        /// <summary>
        /// 步骤定义
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "StepBodyld" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "步骤定义" , IsPrimaryKey = true)]
        public long StepBodyld
        { 
            get{return _StepBodyld;}
            set{
            SetProperty(ref _StepBodyld, value);
                base.PrimaryKeyID = _StepBodyld;
            }
        }

        private long? _Para_Id;
        /// <summary>
        /// 输入参数
        /// </summary>
        [AdvQueryAttribute(ColName = "Para_Id",ColDesc = "输入参数")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Para_Id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "输入参数" )]
        [FKRelationAttribute("tb_StepBodyPara","Para_Id")]
        public long? Para_Id
        { 
            get{return _Para_Id;}
            set{
            SetProperty(ref _Para_Id, value);
                        }
        }

        private string _Name;
        /// <summary>
        /// 名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Name",ColDesc = "名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Name" ,Length=50,IsNullable = true,ColumnDescription = "名称" )]
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

        private string _TypeFullName;
        /// <summary>
        /// 类型全名
        /// </summary>
        [AdvQueryAttribute(ColName = "TypeFullName",ColDesc = "类型全名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "TypeFullName" ,Length=50,IsNullable = true,ColumnDescription = "类型全名" )]
        public string TypeFullName
        { 
            get{return _TypeFullName;}
            set{
            SetProperty(ref _TypeFullName, value);
                        }
        }

        private string _AssemblyFullName;
        /// <summary>
        /// 标题
        /// </summary>
        [AdvQueryAttribute(ColName = "AssemblyFullName",ColDesc = "标题")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "AssemblyFullName" ,Length=50,IsNullable = true,ColumnDescription = "标题" )]
        public string AssemblyFullName
        { 
            get{return _AssemblyFullName;}
            set{
            SetProperty(ref _AssemblyFullName, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Para_Id))]
        public virtual tb_StepBodyPara tb_stepbodypara { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProcessStep.StepBodyld))]
        public virtual List<tb_ProcessStep> tb_ProcessSteps { get; set; }
        //tb_ProcessStep.StepBodyld)
        //StepBodyld.FK_TB_PROCE_REFERENCE_TB_STEPB)
        //tb_StepBody.StepBodyld)


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
                    Type type = typeof(tb_StepBody);
                    
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
            tb_StepBody loctype = (tb_StepBody)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

