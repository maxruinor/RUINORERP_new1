
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/26/2024 10:52:22
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
    /// 报表打印配置表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_PrintConfig")]
    public partial class tb_PrintConfig: BaseEntity, ICloneable
    {
        public tb_PrintConfig()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_PrintConfig" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PrintConfigID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PrintConfigID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long PrintConfigID
        { 
            get{return _PrintConfigID;}
            set{
            base.PrimaryKeyID = _PrintConfigID;
            SetProperty(ref _PrintConfigID, value);
            }
        }

        private string _Config_Name;
        /// <summary>
        /// 配置名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Config_Name",ColDesc = "配置名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Config_Name" ,Length=100,IsNullable = false,ColumnDescription = "配置名称" )]
        public string Config_Name
        { 
            get{return _Config_Name;}
            set{
            SetProperty(ref _Config_Name, value);
            }
        }

        private int _BizType;
        /// <summary>
        /// 业务类型
        /// </summary>
        [AdvQueryAttribute(ColName = "BizType",ColDesc = "业务类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "BizType" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "业务类型" )]
        public int BizType
        { 
            get{return _BizType;}
            set{
            SetProperty(ref _BizType, value);
            }
        }

        private string _BizName;
        /// <summary>
        /// 业务名称
        /// </summary>
        [AdvQueryAttribute(ColName = "BizName",ColDesc = "业务名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BizName" ,Length=30,IsNullable = false,ColumnDescription = "业务名称" )]
        public string BizName
        { 
            get{return _BizName;}
            set{
            SetProperty(ref _BizName, value);
            }
        }

        private string _PrinterName;
        /// <summary>
        /// 默认打印机名称
        /// </summary>
        [AdvQueryAttribute(ColName = "PrinterName",ColDesc = "默认打印机名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PrinterName" ,Length=200,IsNullable = true,ColumnDescription = "默认打印机名称")]
        public string PrinterName
        { 
            get{return _PrinterName;}
            set{
            SetProperty(ref _PrinterName, value);
            }
        }

        private bool _PrinterSelected = false;
        /// <summary>
        /// 设置默认打印机
        /// </summary>
        [AdvQueryAttribute(ColName = "PrinterSelected", ColDesc = "设置默认打印机")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "PrinterSelected", IsNullable = false, ColumnDescription = "设置默认打印机")]
        [Browsable(false)]
        public bool PrinterSelected
        {
            get { return _PrinterSelected; }
            set
            {
                SetProperty(ref _PrinterSelected, value);
            }
        }


        private bool _Landscape = false;
        /// <summary>
        /// 设置横向打印
        /// </summary>
        [AdvQueryAttribute(ColName = "Landscape", ColDesc = "设置横向打印")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "Landscape", IsNullable = false, ColumnDescription = "设置横向打印")]
        [Browsable(false)]
        public bool Landscape
        {
            get { return _Landscape; }
            set
            {
                SetProperty(ref _Landscape, value);
            }
        }


        #endregion

        #region 扩展属性

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PrintTemplate.PrintConfigID))]
        public virtual List<tb_PrintTemplate> tb_PrintTemplates { get; set; }
        //tb_PrintTemplate.PrintConfigID)
        //PrintConfigID.FK_PRINTTEMPLATE_REF_PRINTCONFIG)
        //tb_PrintConfig.PrintConfigID)


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
                    Type type = typeof(tb_PrintConfig);
                    
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
            tb_PrintConfig loctype = (tb_PrintConfig)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

