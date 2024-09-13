
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:35
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
    /// 标准物料表BOM明细-要适当冗余
    /// </summary>
    [Serializable()]
    [Description("tb_BOM_SDetail")]
    [SugarTable("tb_BOM_SDetail")]
    public partial class tb_BOM_SDetail: BaseEntity, ICloneable
    {
        public tb_BOM_SDetail()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_BOM_SDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _SubID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "SubID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long SubID
        { 
            get{return _SubID;}
            set{
            base.PrimaryKeyID = _SubID;
            SetProperty(ref _SubID, value);
            }
        }

        private long _ProdDetailID;
        /// <summary>
        /// 货品详情
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "货品详情")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "货品详情" )]
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
        /// SKU码
        /// </summary>
        [AdvQueryAttribute(ColName = "SKU",ColDesc = "SKU码")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SKU" ,Length=80,IsNullable = true,ColumnDescription = "SKU码" )]
        public string SKU
        { 
            get{return _SKU;}
            set{
            SetProperty(ref _SKU, value);
            }
        }

        private long _BOM_ID;
        /// <summary>
        /// 对应BOM
        /// </summary>
        [AdvQueryAttribute(ColName = "BOM_ID",ColDesc = "对应BOM")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BOM_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "对应BOM" )]
        [FKRelationAttribute("tb_BOM_S","BOM_ID")]
        public long BOM_ID
        { 
            get{return _BOM_ID;}
            set{
            SetProperty(ref _BOM_ID, value);
            }
        }

        private string _Remarks;
        /// <summary>
        /// 备注说明
        /// </summary>
        [AdvQueryAttribute(ColName = "Remarks",ColDesc = "备注说明")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Remarks" ,Length=200,IsNullable = true,ColumnDescription = "备注说明" )]
        public string Remarks
        { 
            get{return _Remarks;}
            set{
            SetProperty(ref _Remarks, value);
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

        private int _Radix= ((1));
        /// <summary>
        /// 基数
        /// </summary>
        [AdvQueryAttribute(ColName = "Radix",ColDesc = "基数")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Radix" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "基数" )]
        public int Radix
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

        private decimal _MaterialCost= ((0));
        /// <summary>
        /// 物料成本
        /// </summary>
        [AdvQueryAttribute(ColName = "MaterialCost",ColDesc = "物料成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "MaterialCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "物料成本" )]
        public decimal MaterialCost
        { 
            get{return _MaterialCost;}
            set{
            SetProperty(ref _MaterialCost, value);
            }
        }

        private decimal _SubtotalMaterialCost= ((0));
        /// <summary>
        /// 物料小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalMaterialCost",ColDesc = "物料小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalMaterialCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "物料小计" )]
        public decimal SubtotalMaterialCost
        { 
            get{return _SubtotalMaterialCost;}
            set{
            SetProperty(ref _SubtotalMaterialCost, value);
            }
        }

        private decimal _ManufacturingCost;
        /// <summary>
        /// 制造费
        /// </summary>
        [AdvQueryAttribute(ColName = "ManufacturingCost",ColDesc = "制造费")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "ManufacturingCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "制造费" )]
        public decimal ManufacturingCost
        { 
            get{return _ManufacturingCost;}
            set{
            SetProperty(ref _ManufacturingCost, value);
            }
        }

        private decimal _OutManuCost;
        /// <summary>
        /// 托工费
        /// </summary>
        [AdvQueryAttribute(ColName = "OutManuCost",ColDesc = "托工费")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "OutManuCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "托工费" )]
        public decimal OutManuCost
        { 
            get{return _OutManuCost;}
            set{
            SetProperty(ref _OutManuCost, value);
            }
        }

        private decimal _SubtotalManufacturingCost;
        /// <summary>
        /// 制造费小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalManufacturingCost",ColDesc = "制造费小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalManufacturingCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "制造费小计" )]
        public decimal SubtotalManufacturingCost
        { 
            get{return _SubtotalManufacturingCost;}
            set{
            SetProperty(ref _SubtotalManufacturingCost, value);
            }
        }

        private decimal _SubtotalOutManuCost;
        /// <summary>
        /// 托工费小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalOutManuCost",ColDesc = "托工费小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalOutManuCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "托工费小计" )]
        public decimal SubtotalOutManuCost
        { 
            get{return _SubtotalOutManuCost;}
            set{
            SetProperty(ref _SubtotalOutManuCost, value);
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

        private bool? _IsOutWork;
        /// <summary>
        /// 是否托外
        /// </summary>
        [AdvQueryAttribute(ColName = "IsOutWork",ColDesc = "是否托外")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsOutWork" ,IsNullable = true,ColumnDescription = "是否托外" )]
        public bool? IsOutWork
        { 
            get{return _IsOutWork;}
            set{
            SetProperty(ref _IsOutWork, value);
            }
        }

        private long? _Child_BOM_Node_ID;
        /// <summary>
        /// 子件配方
        /// </summary>
        [AdvQueryAttribute(ColName = "Child_BOM_Node_ID",ColDesc = "子件配方")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Child_BOM_Node_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "子件配方" )]
        public long? Child_BOM_Node_ID
        { 
            get{return _Child_BOM_Node_ID;}
            set{
            SetProperty(ref _Child_BOM_Node_ID, value);
            }
        }

        private decimal _TotalSelfProductionAllCost;
        /// <summary>
        /// 自产总成本
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalSelfProductionAllCost",ColDesc = "自产总成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalSelfProductionAllCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "自产总成本" )]
        public decimal TotalSelfProductionAllCost
        { 
            get{return _TotalSelfProductionAllCost;}
            set{
            SetProperty(ref _TotalSelfProductionAllCost, value);
            }
        }

        private decimal _TotalOutsourcingAllCost;
        /// <summary>
        /// 外发总成本
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalOutsourcingAllCost",ColDesc = "外发总成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalOutsourcingAllCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "外发总成本" )]
        public decimal TotalOutsourcingAllCost
        { 
            get{return _TotalOutsourcingAllCost;}
            set{
            SetProperty(ref _TotalOutsourcingAllCost, value);
            }
        }

        private long? _Substitute;
        /// <summary>
        /// 替代品
        /// </summary>
        [AdvQueryAttribute(ColName = "Substitute",ColDesc = "替代品")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Substitute" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "替代品" )]
        public long? Substitute
        { 
            get{return _Substitute;}
            set{
            SetProperty(ref _Substitute, value);
            }
        }

        private decimal? _OutputRate= ((1));
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

        #endregion

        #region 扩展属性

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual View_ProdDetail view_ProdDetail { get; set; }


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
        [Navigate(NavigateType.OneToOne, nameof(BOM_ID))]
        public virtual tb_BOM_S tb_bom_s { get; set; }

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
                    Type type = typeof(tb_BOM_SDetail);
                    
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
            tb_BOM_SDetail loctype = (tb_BOM_SDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

