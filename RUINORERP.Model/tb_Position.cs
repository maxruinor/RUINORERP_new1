
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:02
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
    /// 位置信息
    /// </summary>
    [Serializable()]
    [Description("tb_Position")]
    [SugarTable("tb_Position")]
    public partial class tb_Position: BaseEntity, ICloneable
    {
        public tb_Position()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_Position" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Position_Id;
        /// <summary>
        /// 位置信息
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Position_Id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "位置信息" , IsPrimaryKey = true)]
        public long Position_Id
        { 
            get{return _Position_Id;}
            set{
            base.PrimaryKeyID = _Position_Id;
            SetProperty(ref _Position_Id, value);
            }
        }

        private string _Left;
        /// <summary>
        /// 左边距
        /// </summary>
        [AdvQueryAttribute(ColName = "Left",ColDesc = "左边距")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Left" ,Length=50,IsNullable = false,ColumnDescription = "左边距" )]
        public string Left
        { 
            get{return _Left;}
            set{
            SetProperty(ref _Left, value);
            }
        }

        private string _Right;
        /// <summary>
        /// 右边距
        /// </summary>
        [AdvQueryAttribute(ColName = "Right",ColDesc = "右边距")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Right" ,Length=50,IsNullable = true,ColumnDescription = "右边距" )]
        public string Right
        { 
            get{return _Right;}
            set{
            SetProperty(ref _Right, value);
            }
        }

        private string _Bottom;
        /// <summary>
        /// 下边距
        /// </summary>
        [AdvQueryAttribute(ColName = "Bottom",ColDesc = "下边距")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Bottom" ,Length=50,IsNullable = true,ColumnDescription = "下边距" )]
        public string Bottom
        { 
            get{return _Bottom;}
            set{
            SetProperty(ref _Bottom, value);
            }
        }

        private string _Top;
        /// <summary>
        /// 上边距
        /// </summary>
        [AdvQueryAttribute(ColName = "Top",ColDesc = "上边距")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Top" ,Length=50,IsNullable = true,ColumnDescription = "上边距" )]
        public string Top
        { 
            get{return _Top;}
            set{
            SetProperty(ref _Top, value);
            }
        }

        #endregion

        #region 扩展属性

        [Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProcessStep.Position_Id))]
        public virtual List<tb_ProcessStep> tb_ProcessSteps { get; set; }
        //tb_ProcessStep.Position_Id)
        //Position_Id.FK_TB_PROCE_REFERENCE_TB_POSIT)
        //tb_Position.Position_Id)


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
                    Type type = typeof(tb_Position);
                    
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
            tb_Position loctype = (tb_Position)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

