
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/21/2025 19:17:36
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
    /// 单位换算表
    /// </summary>
    [Serializable()]
    [Description("单位换算表")]
    [SugarTable("tb_Unit_Conversion")]
    public partial class tb_Unit_Conversion: BaseEntity, ICloneable
    {
        public tb_Unit_Conversion()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("单位换算表tb_Unit_Conversion" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _UnitConversion_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "UnitConversion_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long UnitConversion_ID
        { 
            get{return _UnitConversion_ID;}
            set{
            base.PrimaryKeyID = _UnitConversion_ID;
            SetProperty(ref _UnitConversion_ID, value);
            }
        }

        private string _UnitConversion_Name;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitConversion_Name",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "UnitConversion_Name" ,Length=100,IsNullable = false,ColumnDescription = "备注" )]
        public string UnitConversion_Name
        { 
            get{return _UnitConversion_Name;}
            set{
            SetProperty(ref _UnitConversion_Name, value);
            }
        }

        private long _Source_unit_id;
        /// <summary>
        /// 来源单位
        /// </summary>
        [AdvQueryAttribute(ColName = "Source_unit_id",ColDesc = "来源单位")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Source_unit_id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "来源单位" )]
        [FKRelationAttribute("tb_Unit","Source_unit_id")]
        public long Source_unit_id
        { 
            get{return _Source_unit_id;}
            set{
            SetProperty(ref _Source_unit_id, value);
            }
        }

        private long _Target_unit_id;
        /// <summary>
        /// 目标单位
        /// </summary>
        [AdvQueryAttribute(ColName = "Target_unit_id",ColDesc = "目标单位")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Target_unit_id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "目标单位" )]
        [FKRelationAttribute("tb_Unit","Target_unit_id")]
        public long Target_unit_id
        { 
            get{return _Target_unit_id;}
            set{
            SetProperty(ref _Target_unit_id, value);
            }
        }

        private decimal _Conversion_ratio= ((0));
        /// <summary>
        /// 换算比例
        /// </summary>
        [AdvQueryAttribute(ColName = "Conversion_ratio",ColDesc = "换算比例")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Conversion_ratio" , DecimalDigits = 5,IsNullable = false,ColumnDescription = "换算比例" )]
        public decimal Conversion_ratio
        { 
            get{return _Conversion_ratio;}
            set{
            SetProperty(ref _Conversion_ratio, value);
            }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=200,IsNullable = true,ColumnDescription = "备注" )]
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
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Target_unit_id))]
        public virtual tb_Unit tb_unit_Target { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Source_unit_id))]
        public virtual tb_Unit tb_unit_source { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_BOM_SDetail.UnitConversion_ID))]
        public virtual List<tb_BOM_SDetail> tb_BOM_SDetails { get; set; }
        //tb_BOM_SDetail.UnitConversion_ID)
        //UnitConversion_ID.FK_BOM_SDetail_REF_UNIT_Conversion)
        //tb_Unit_Conversion.UnitConversion_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_BOM_SDetailSubstituteMaterial.UnitConversion_ID))]
        public virtual List<tb_BOM_SDetailSubstituteMaterial> tb_BOM_SDetailSubstituteMaterials { get; set; }
        //tb_BOM_SDetailSubstituteMaterial.UnitConversion_ID)
        //UnitConversion_ID.FK_BOM_Substitute_REF_UNIT_Conversion)
        //tb_Unit_Conversion.UnitConversion_ID)


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
         if("Unit_ID"!="Target_unit_id")
        {
        // rs=false;
        }
         if("Unit_ID"!="Source_unit_id")
        {
        // rs=false;
        }
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
                    Type type = typeof(tb_Unit_Conversion);
                    
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
            tb_Unit_Conversion loctype = (tb_Unit_Conversion)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

