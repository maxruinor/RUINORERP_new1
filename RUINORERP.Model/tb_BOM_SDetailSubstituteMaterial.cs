﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/25/2024 20:07:13
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
    /// 标准物料表BOM明细的替代材料表-使用优化级按库存量-成本-保质时间在配置来确定
    /// </summary>
    [Serializable()]
    [Description("标准物料表BOM明细的替代材料表-使用优化级按库存量-成本-保质时间在配置来确定")]
    [SugarTable("tb_BOM_SDetailSubstituteMaterial")]
    public partial class tb_BOM_SDetailSubstituteMaterial: BaseEntity, ICloneable
    {
        public tb_BOM_SDetailSubstituteMaterial()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("标准物料表BOM明细的替代材料表-使用优化级按库存量-成本-保质时间在配置来确定tb_BOM_SDetailSubstituteMaterial" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _SubstituteMaterialID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "SubstituteMaterialID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long SubstituteMaterialID
        { 
            get{return _SubstituteMaterialID;}
            set{
            base.PrimaryKeyID = _SubstituteMaterialID;
            SetProperty(ref _SubstituteMaterialID, value);
            }
        }

        private long _SubID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "SubID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "SubID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_BOM_SDetail","SubID")]
        public long SubID
        { 
            get{return _SubID;}
            set{
            SetProperty(ref _SubID, value);
            }
        }

        private long _ProdDetailID;
        /// <summary>
        /// 产品详情
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品详情")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "产品详情" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
            }
        }

        private string _SKU;
        /// <summary>
        /// SKU
        /// </summary>
        [AdvQueryAttribute(ColName = "SKU",ColDesc = "SKU")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SKU" ,Length=80,IsNullable = true,ColumnDescription = "SKU" )]
        public string SKU
        { 
            get{return _SKU;}
            set{
            SetProperty(ref _SKU, value);
            }
        }

        private string _property;
        /// <summary>
        /// 子件属性
        /// </summary>
        [AdvQueryAttribute(ColName = "property",ColDesc = "子件属性")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "property" ,Length=255,IsNullable = true,ColumnDescription = "子件属性" )]
        public string property
        { 
            get{return _property;}
            set{
            SetProperty(ref _property, value);
            }
        }

        private long _Unit_ID;
        /// <summary>
        /// 单位
        /// </summary>
        [AdvQueryAttribute(ColName = "Unit_ID",ColDesc = "单位")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Unit_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "单位" )]
        [FKRelationAttribute("tb_Unit","Unit_ID")]
        public long Unit_ID
        { 
            get{return _Unit_ID;}
            set{
            SetProperty(ref _Unit_ID, value);
            }
        }

        private long? _UnitConversion_ID;
        /// <summary>
        /// 单位换算
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitConversion_ID",ColDesc = "单位换算")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "UnitConversion_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "单位换算" )]
        [FKRelationAttribute("tb_Unit_Conversion","UnitConversion_ID")]
        public long? UnitConversion_ID
        { 
            get{return _UnitConversion_ID;}
            set{
            SetProperty(ref _UnitConversion_ID, value);
            }
        }

        private decimal _UsedQty= ((1));
        /// <summary>
        /// 用量
        /// </summary>
        [AdvQueryAttribute(ColName = "UsedQty",ColDesc = "用量")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "UsedQty" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "用量" )]
        public decimal UsedQty
        { 
            get{return _UsedQty;}
            set{
            SetProperty(ref _UsedQty, value);
            }
        }

        private int? _Radix= ((1));
        /// <summary>
        /// 基数
        /// </summary>
        [AdvQueryAttribute(ColName = "Radix",ColDesc = "基数")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Radix" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "基数" )]
        public int? Radix
        { 
            get{return _Radix;}
            set{
            SetProperty(ref _Radix, value);
            }
        }

        private decimal _LossRate= ((0));
        /// <summary>
        /// 损耗率
        /// </summary>
        [AdvQueryAttribute(ColName = "LossRate",ColDesc = "损耗率")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "LossRate" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "损耗率" )]
        public decimal LossRate
        { 
            get{return _LossRate;}
            set{
            SetProperty(ref _LossRate, value);
            }
        }

        private string _InstallPosition;
        /// <summary>
        /// 组装位置
        /// </summary>
        [AdvQueryAttribute(ColName = "InstallPosition",ColDesc = "组装位置")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "InstallPosition" ,Length=50,IsNullable = true,ColumnDescription = "组装位置" )]
        public string InstallPosition
        { 
            get{return _InstallPosition;}
            set{
            SetProperty(ref _InstallPosition, value);
            }
        }

        private string _PositionNo;
        /// <summary>
        /// 位号
        /// </summary>
        [AdvQueryAttribute(ColName = "PositionNo",ColDesc = "位号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PositionNo" ,Length=50,IsNullable = true,ColumnDescription = "位号" )]
        public string PositionNo
        { 
            get{return _PositionNo;}
            set{
            SetProperty(ref _PositionNo, value);
            }
        }

        private decimal _UnitlCost= ((0));
        /// <summary>
        /// 单位成本
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitlCost",ColDesc = "单位成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UnitlCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "单位成本" )]
        public decimal UnitlCost
        { 
            get{return _UnitlCost;}
            set{
            SetProperty(ref _UnitlCost, value);
            }
        }

        private decimal _SubtotalUnitCost= ((0));
        /// <summary>
        /// 成本小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalUnitCost",ColDesc = "成本小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalUnitCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "成本小计" )]
        public decimal SubtotalUnitCost
        { 
            get{return _SubtotalUnitCost;}
            set{
            SetProperty(ref _SubtotalUnitCost, value);
            }
        }

        private string _PositionDesc;
        /// <summary>
        /// 位号描述
        /// </summary>
        [AdvQueryAttribute(ColName = "PositionDesc",ColDesc = "位号描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PositionDesc" ,Length=100,IsNullable = true,ColumnDescription = "位号描述" )]
        public string PositionDesc
        { 
            get{return _PositionDesc;}
            set{
            SetProperty(ref _PositionDesc, value);
            }
        }

        private long? _ManufacturingProcessID;
        /// <summary>
        /// 制程
        /// </summary>
        [AdvQueryAttribute(ColName = "ManufacturingProcessID",ColDesc = "制程")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ManufacturingProcessID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "制程" )]
        public long? ManufacturingProcessID
        { 
            get{return _ManufacturingProcessID;}
            set{
            SetProperty(ref _ManufacturingProcessID, value);
            }
        }

        private decimal? _OutputRate;
        /// <summary>
        /// 产出率
        /// </summary>
        [AdvQueryAttribute(ColName = "OutputRate",ColDesc = "产出率")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "OutputRate" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "产出率" )]
        public decimal? OutputRate
        { 
            get{return _OutputRate;}
            set{
            SetProperty(ref _OutputRate, value);
            }
        }

        private int? _PriorityUseType= ((0));
        /// <summary>
        /// 优先使用类型
        /// </summary>
        [AdvQueryAttribute(ColName = "PriorityUseType",ColDesc = "优先使用类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "PriorityUseType" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "优先使用类型" )]
        public int? PriorityUseType
        { 
            get{return _PriorityUseType;}
            set{
            SetProperty(ref _PriorityUseType, value);
            }
        }

        private int _Sort= ((0));
        /// <summary>
        /// 排序
        /// </summary>
        [AdvQueryAttribute(ColName = "Sort",ColDesc = "排序")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Sort" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "排序" )]
        public int Sort
        { 
            get{return _Sort;}
            set{
            SetProperty(ref _Sort, value);
            }
        }

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=200,IsNullable = true,ColumnDescription = "摘要" )]
        public string Summary
        { 
            get{return _Summary;}
            set{
            SetProperty(ref _Summary, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(SubID))]
        public virtual tb_BOM_SDetail tb_bom_sdetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Unit_ID))]
        public virtual tb_Unit tb_unit { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(UnitConversion_ID))]
        public virtual tb_Unit_Conversion tb_unit_conversion { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }



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
                    Type type = typeof(tb_BOM_SDetailSubstituteMaterial);
                    
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
            tb_BOM_SDetailSubstituteMaterial loctype = (tb_BOM_SDetailSubstituteMaterial)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}
