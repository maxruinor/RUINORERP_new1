
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/10/2023 00:37:30
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;


namespace RUINORERP.Model
{
    /// <summary>
    /// 流程图线
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FlowchartLines")]
    public partial class tb_FlowchartLines: BaseEntity, ICloneable
    {
        public tb_FlowchartLines()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_FlowchartLines" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    
        
        private int _ID;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ID",IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true, IsIdentity = true)]
        public int ID 
        { 
            get{return _ID;}
            set{
                SetProperty(ref _ID, value);
                }
        }

        
        private string _FlowchartNo;
        /// <summary>
        /// 流程图编号
        /// </summary>
        [SugarColumn(ColumnName = "FlowchartNo",Length=50,IsNullable = false,ColumnDescription = "流程图编号" )]
        public string FlowchartNo 
        { 
            get{return _FlowchartNo;}
            set{
                SetProperty(ref _FlowchartNo, value);
                }
        }

        
        private string _PointToString1;
        /// <summary>
        /// 大小
        /// </summary>
        [SugarColumn(ColumnName = "PointToString1",Length=100,IsNullable = true,ColumnDescription = "大小" )]
        public string PointToString1 
        { 
            get{return _PointToString1;}
            set{
                SetProperty(ref _PointToString1, value);
                }
        }

        
        private string _PointToString2;
        /// <summary>
        /// 位置
        /// </summary>
        [SugarColumn(ColumnName = "PointToString2",Length=100,IsNullable = true,ColumnDescription = "位置" )]
        public string PointToString2 
        { 
            get{return _PointToString2;}
            set{
                SetProperty(ref _PointToString2, value);
                }
        }


        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(RUINORERP.Model.tb_FlowchartDefinition.FlowchartNo))]
        public virtual tb_FlowchartDefinition tb_FlowchartDefinition { get; set; }






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
        [Description("列名中文描述"), Category("自定属性"), Browsable(true)]
        [SugarColumn(IsIgnore = true)]
        public override ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, string>();
                    SugarColumn entityAttr;
                    Type type = typeof(tb_FlowchartLines);
                    
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
            tb_FlowchartLines loctype = (tb_FlowchartLines)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

