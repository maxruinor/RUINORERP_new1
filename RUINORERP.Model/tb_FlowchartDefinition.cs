
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:31:59
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
    /// 流程图定义
    /// </summary>
    [Serializable()]
    [Description("流程图定义")]
    [SugarTable("tb_FlowchartDefinition")]
    public partial class tb_FlowchartDefinition: BaseEntity, ICloneable
    {
        public tb_FlowchartDefinition()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("流程图定义tb_FlowchartDefinition" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ID;
        /// <summary>
        /// 流程图
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "流程图" , IsPrimaryKey = true)]
        public long ID
        { 
            get{return _ID;}
            set{
            SetProperty(ref _ID, value);
                base.PrimaryKeyID = _ID;
            }
        }

        private long? _ModuleID;
        /// <summary>
        /// 模块
        /// </summary>
        [AdvQueryAttribute(ColName = "ModuleID",ColDesc = "模块")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ModuleID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "模块" )]
        [FKRelationAttribute("tb_ModuleDefinition","ModuleID")]
        public long? ModuleID
        { 
            get{return _ModuleID;}
            set{
            SetProperty(ref _ModuleID, value);
                        }
        }

        private string _FlowchartNo;
        /// <summary>
        /// 流程图编号
        /// </summary>
        [AdvQueryAttribute(ColName = "FlowchartNo",ColDesc = "流程图编号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "FlowchartNo" ,Length=50,IsNullable = false,ColumnDescription = "流程图编号" )]
        public string FlowchartNo
        { 
            get{return _FlowchartNo;}
            set{
            SetProperty(ref _FlowchartNo, value);
                        }
        }

        private string _FlowchartName;
        /// <summary>
        /// 流程图名称
        /// </summary>
        [AdvQueryAttribute(ColName = "FlowchartName",ColDesc = "流程图名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "FlowchartName" ,Length=20,IsNullable = false,ColumnDescription = "流程图名称" )]
        public string FlowchartName
        { 
            get{return _FlowchartName;}
            set{
            SetProperty(ref _FlowchartName, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ModuleID))]
        public virtual tb_ModuleDefinition tb_moduledefinition { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FlowchartItem.ID))]
        public virtual List<tb_FlowchartItem> tb_FlowchartItems { get; set; }
        //tb_FlowchartItem.ID)
        //ID.FK_TB_FLOWC_REF_TB_FLOWCTION)
        //tb_FlowchartDefinition.ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FlowchartLine.ID))]
        public virtual List<tb_FlowchartLine> tb_FlowchartLines { get; set; }
        //tb_FlowchartLine.ID)
        //ID.FK_TB_FLOWC_REFFLOWCLINE)
        //tb_FlowchartDefinition.ID)


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
                    Type type = typeof(tb_FlowchartDefinition);
                    
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
            tb_FlowchartDefinition loctype = (tb_FlowchartDefinition)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

