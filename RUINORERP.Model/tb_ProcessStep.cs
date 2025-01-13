
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:03
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
    /// 流程步骤
    /// </summary>
    [Serializable()]
    [Description("tb_ProcessStep")]
    [SugarTable("tb_ProcessStep")]
    public partial class tb_ProcessStep: BaseEntity, ICloneable
    {
        public tb_ProcessStep()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_ProcessStep" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Step_Id;
        /// <summary>
        /// 步骤
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Step_Id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "步骤" , IsPrimaryKey = true)]
        public long Step_Id
        { 
            get{return _Step_Id;}
            set{
            base.PrimaryKeyID = _Step_Id;
            SetProperty(ref _Step_Id, value);
            }
        }

        private long? _StepBodyld;
        /// <summary>
        /// 流程定义
        /// </summary>
        [AdvQueryAttribute(ColName = "StepBodyld",ColDesc = "流程定义")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "StepBodyld" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "流程定义" )]
        [FKRelationAttribute("tb_StepBody","StepBodyld")]
        public long? StepBodyld
        { 
            get{return _StepBodyld;}
            set{
            SetProperty(ref _StepBodyld, value);
            }
        }

        private long? _Position_Id;
        /// <summary>
        /// 位置信息
        /// </summary>
        [AdvQueryAttribute(ColName = "Position_Id",ColDesc = "位置信息")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Position_Id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "位置信息" )]
        [FKRelationAttribute("tb_Position","Position_Id")]
        public long? Position_Id
        { 
            get{return _Position_Id;}
            set{
            SetProperty(ref _Position_Id, value);
            }
        }

        private long? _NextNode_ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "NextNode_ID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "NextNode_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_NextNodes","NextNode_ID")]
        public long? NextNode_ID
        { 
            get{return _NextNode_ID;}
            set{
            SetProperty(ref _NextNode_ID, value);
            }
        }

        private string _Version;
        /// <summary>
        /// 版本
        /// </summary>
        [AdvQueryAttribute(ColName = "Version",ColDesc = "版本")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Version" ,Length=50,IsNullable = false,ColumnDescription = "版本" )]
        public string Version
        { 
            get{return _Version;}
            set{
            SetProperty(ref _Version, value);
            }
        }

        private string _Name;
        /// <summary>
        /// 标题
        /// </summary>
        [AdvQueryAttribute(ColName = "Name",ColDesc = "标题")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Name" ,Length=50,IsNullable = true,ColumnDescription = "标题" )]
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

        private string _StepNodeType;
        /// <summary>
        /// 节点类型
        /// </summary>
        [AdvQueryAttribute(ColName = "StepNodeType",ColDesc = "节点类型")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "StepNodeType" ,Length=50,IsNullable = true,ColumnDescription = "节点类型" )]
        public string StepNodeType
        { 
            get{return _StepNodeType;}
            set{
            SetProperty(ref _StepNodeType, value);
            }
        }

        private string _Description;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Description",ColDesc = "描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Description" ,Length=255,IsNullable = true,ColumnDescription = "描述" )]
        public string Description
        { 
            get{return _Description;}
            set{
            SetProperty(ref _Description, value);
            }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=255,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(NextNode_ID))]
        public virtual tb_NextNodes tb_nextnodes { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Position_Id))]
        public virtual tb_Position tb_position { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(StepBodyld))]
        public virtual tb_StepBody tb_stepbody { get; set; }


        [Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProcessDefinition.Step_Id))]
        public virtual List<tb_ProcessDefinition> tb_ProcessDefinitions { get; set; }
        //tb_ProcessDefinition.Step_Id)
        //Step_Id.FK_TB_PROCE_REFERENCE_TB_PROCE)
        //tb_ProcessStep.Step_Id)


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
                    Type type = typeof(tb_ProcessStep);
                    
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
            tb_ProcessStep loctype = (tb_ProcessStep)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

