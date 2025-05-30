﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:32
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
    [Description("基本单位")]
    [SugarTable("tb_Unit")]
    public partial class tb_Unit: BaseEntity, ICloneable
    {
        public tb_Unit()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("基本单位tb_Unit" + "外键ID与对应主主键名称不一致。请修改数据库");
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
            SetProperty(ref _Unit_ID, value);
                base.PrimaryKeyID = _Unit_ID;
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

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FinishedGoodsInvDetail.Unit_ID))]
        public virtual List<tb_FinishedGoodsInvDetail> tb_FinishedGoodsInvDetails { get; set; }
        //tb_FinishedGoodsInvDetail.Unit_ID)
        //Unit_ID.FK_TB_FINISDetail_REF_TB_UNIT)
        //tb_Unit.Unit_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Prod.Unit_ID))]
        public virtual List<tb_Prod> tb_Prods { get; set; }
        //tb_Prod.Unit_ID)
        //Unit_ID.FK_TB_PROD_REFERENCE_TB_UNIT)
        //tb_Unit.Unit_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Unit_Conversion.Target_unit_id))]
        public virtual List<tb_Unit_Conversion> tb_Unit_Conversions_target { get; set; }
        //tb_Unit_Conversion.Unit_ID)
        //Unit_ID.FK_UNIT_Conversions_TB_UNIT_1)
        //tb_Unit.Target_unit_id)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Unit_Conversion.Source_unit_id))]
        public virtual List<tb_Unit_Conversion> tb_Unit_Conversions_source { get; set; }
        //tb_Unit_Conversion.Unit_ID)
        //Unit_ID.FK_UNIT_Conversions_TB_UNIT_2)
        //tb_Unit.Source_unit_id)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Packing.Unit_ID))]
        public virtual List<tb_Packing> tb_Packings { get; set; }
        //tb_Packing.Unit_ID)
        //Unit_ID.FK_PACKIING_REF_UNIT)
        //tb_Unit.Unit_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_BOM_SDetail.Unit_ID))]
        public virtual List<tb_BOM_SDetail> tb_BOM_SDetails { get; set; }
        //tb_BOM_SDetail.Unit_ID)
        //Unit_ID.FK_BOM_SDetail_REF_UNIT)
        //tb_Unit.Unit_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_BOM_SDetailSubstituteMaterial.Unit_ID))]
        public virtual List<tb_BOM_SDetailSubstituteMaterial> tb_BOM_SDetailSubstituteMaterials { get; set; }
        //tb_BOM_SDetailSubstituteMaterial.Unit_ID)
        //Unit_ID.FK_BOM_SDetailSubstitute_REF_UNIT)
        //tb_Unit.Unit_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ManufacturingOrder.Unit_ID))]
        public virtual List<tb_ManufacturingOrder> tb_ManufacturingOrders { get; set; }
        //tb_ManufacturingOrder.Unit_ID)
        //Unit_ID.FK_MANUFACTURINGORDER_REF_UNIT)
        //tb_Unit.Unit_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
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






     
        public override object Clone()
        {
            tb_Unit loctype = (tb_Unit)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

