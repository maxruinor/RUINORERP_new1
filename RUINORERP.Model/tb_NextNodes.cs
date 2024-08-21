
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:47:53
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
    /// 流程步骤 转移条件集合
    /// </summary>
    [Serializable()]
    [SugarTable("tb_NextNodes")]
    public partial class tb_NextNodes: BaseEntity, ICloneable
    {
        public tb_NextNodes()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_NextNodes" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _NextNode_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "NextNode_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long NextNode_ID
        { 
            get{return _NextNode_ID;}
            set{
            base.PrimaryKeyID = _NextNode_ID;
            SetProperty(ref _NextNode_ID, value);
            }
        }

        private long? _ConNodeConditions_Id;
        /// <summary>
        /// 条件
        /// </summary>
        [AdvQueryAttribute(ColName = "ConNodeConditions_Id",ColDesc = "条件")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ConNodeConditions_Id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "条件" )]
        [FKRelationAttribute("tb_ConNodeConditions","ConNodeConditions_Id")]
        public long? ConNodeConditions_Id
        { 
            get{return _ConNodeConditions_Id;}
            set{
            SetProperty(ref _ConNodeConditions_Id, value);
            }
        }

        private string _NexNodeName;
        /// <summary>
        /// 下节点名称
        /// </summary>
        [AdvQueryAttribute(ColName = "NexNodeName",ColDesc = "下节点名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "NexNodeName" ,Length=50,IsNullable = false,ColumnDescription = "下节点名称" )]
        public string NexNodeName
        { 
            get{return _NexNodeName;}
            set{
            SetProperty(ref _NexNodeName, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(ConNodeConditions_Id))]
        public virtual tb_ConNodeConditions tb_connodeconditions { get; set; }
        //public virtual tb_ConNodeConditions tb_ConNodeConditions_Id { get; set; }


        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProcessStep.NextNode_ID))]
        public virtual List<tb_ProcessStep> tb_ProcessSteps { get; set; }
        //tb_ProcessStep.NextNode_ID)
        //NextNode_ID.FK_TB_PROCE_REFERENCE_TB_NEXTN)
        //tb_NextNodes.NextNode_ID)


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
                    Type type = typeof(tb_NextNodes);
                    
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
            tb_NextNodes loctype = (tb_NextNodes)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

