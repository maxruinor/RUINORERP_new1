
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:39
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
    /// 基本单位
    /// </summary>
    [Serializable()]
    [Description("tb_Unit")]
    [SugarTable("tb_Unit")]
    public partial class tb_Unit: BaseEntity, ICloneable
    {
        public tb_Unit()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_Unit" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Unit_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Unit_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long Unit_ID
        { 
            get{return _Unit_ID;}
            set{
            base.PrimaryKeyID = _Unit_ID;
            SetProperty(ref _Unit_ID, value);
            }
        }

        private string _UnitName;
        /// <summary>
        /// 单位名称
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitName",ColDesc = "单位名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "UnitName" ,Length=255,IsNullable = false,ColumnDescription = "单位名称" )]
        public string UnitName
        { 
            get{return _UnitName;}
            set{
            SetProperty(ref _UnitName, value);
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

        private bool _is_measurement_unit= false;
        /// <summary>
        /// 是否可换算
        /// </summary>
        [AdvQueryAttribute(ColName = "is_measurement_unit",ColDesc = "是否可换算")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "is_measurement_unit" ,IsNullable = false,ColumnDescription = "是否可换算" )]
        public bool is_measurement_unit
        { 
            get{return _is_measurement_unit;}
            set{
            SetProperty(ref _is_measurement_unit, value);
            }
        }

        #endregion

        #region 扩展属性

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ManufacturingOrder.Unit_ID))]
        public virtual List<tb_ManufacturingOrder> tb_ManufacturingOrders { get; set; }
        //tb_ManufacturingOrder.Unit_ID)
        //Unit_ID.FK_MANUFACTURINGORDER_REF_UNIT)
        //tb_Unit.Unit_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FinishedGoodsInvDetail.Unit_ID))]
        public virtual List<tb_FinishedGoodsInvDetail> tb_FinishedGoodsInvDetails { get; set; }
        //tb_FinishedGoodsInvDetail.Unit_ID)
        //Unit_ID.FK_TB_FINISDetail_REF_TB_UNIT)
        //tb_Unit.Unit_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Prod.Unit_ID))]
        public virtual List<tb_Prod> tb_Prods { get; set; }
        //tb_Prod.Unit_ID)
        //Unit_ID.FK_TB_PROD_REFERENCE_TB_UNIT)
        //tb_Unit.Unit_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Packing.Unit_ID))]
        public virtual List<tb_Packing> tb_Packings { get; set; }
        //tb_Packing.Unit_ID)
        //Unit_ID.FK_PACKIING_REF_UNIT)
        //tb_Unit.Unit_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_BOM_SDetail.Unit_ID))]
        public virtual List<tb_BOM_SDetail> tb_BOM_SDetails { get; set; }
        //tb_BOM_SDetail.Unit_ID)
        //Unit_ID.FK_BOM_SDetail_REF_UNIT)
        //tb_Unit.Unit_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProdBundle.Unit_ID))]
        public virtual List<tb_ProdBundle> tb_ProdBundles { get; set; }
        //tb_ProdBundle.Unit_ID)
        //Unit_ID.FK_PRODBUNDLE_REF_UNIT)
        //tb_Unit.Unit_ID)


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
                    Type type = typeof(tb_Unit);
                    
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
            tb_Unit loctype = (tb_Unit)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

