
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/14/2025 18:56:45
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
    /// 箱规表
    /// </summary>
    [Serializable()]
    [Description("箱规表")]
    [SugarTable("tb_BoxRules")]
    public partial class tb_BoxRules: BaseEntity, ICloneable
    {
        public tb_BoxRules()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("箱规表tb_BoxRules" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _BoxRules_ID;
        /// <summary>
        /// 箱规
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BoxRules_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "箱规" , IsPrimaryKey = true)]
        public long BoxRules_ID
        { 
            get{return _BoxRules_ID;}
            set{
            base.PrimaryKeyID = _BoxRules_ID;
            SetProperty(ref _BoxRules_ID, value);
            }
        }

        private long? _Pack_ID;
        /// <summary>
        /// 包装信息
        /// </summary>
        [AdvQueryAttribute(ColName = "Pack_ID",ColDesc = "包装信息")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Pack_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "包装信息" )]
        [FKRelationAttribute("tb_Packing","Pack_ID")]
        public long? Pack_ID
        { 
            get{return _Pack_ID;}
            set{
            SetProperty(ref _Pack_ID, value);
            }
        }

        private long _CartonID;
        /// <summary>
        /// 纸箱规格
        /// </summary>
        [AdvQueryAttribute(ColName = "CartonID",ColDesc = "纸箱规格")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CartonID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "纸箱规格" )]
        [FKRelationAttribute("tb_CartoonBox","CartonID")]
        public long CartonID
        { 
            get{return _CartonID;}
            set{
            SetProperty(ref _CartonID, value);
            }
        }

        private string _BoxRuleName;
        /// <summary>
        /// 箱规名称
        /// </summary>
        [AdvQueryAttribute(ColName = "BoxRuleName",ColDesc = "箱规名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BoxRuleName" ,Length=255,IsNullable = true,ColumnDescription = "箱规名称" )]
        public string BoxRuleName
        { 
            get{return _BoxRuleName;}
            set{
            SetProperty(ref _BoxRuleName, value);
            }
        }

        private int _QuantityPerBox= ((0));
        /// <summary>
        ///  每箱数量
        /// </summary>
        [AdvQueryAttribute(ColName = "QuantityPerBox",ColDesc = " 每箱数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "QuantityPerBox" , DecimalDigits = 0,IsNullable = false,ColumnDescription = " 每箱数量" )]
        public int QuantityPerBox
        { 
            get{return _QuantityPerBox;}
            set{
            SetProperty(ref _QuantityPerBox, value);
            }
        }

        private string _PackingMethod;
        /// <summary>
        /// 装箱方式
        /// </summary>
        [AdvQueryAttribute(ColName = "PackingMethod",ColDesc = "装箱方式")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PackingMethod" ,Length=100,IsNullable = true,ColumnDescription = "装箱方式" )]
        public string PackingMethod
        { 
            get{return _PackingMethod;}
            set{
            SetProperty(ref _PackingMethod, value);
            }
        }

        private decimal _Length;
        /// <summary>
        /// 长度(cm)
        /// </summary>
        [AdvQueryAttribute(ColName = "Length",ColDesc = "长度(cm)")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Length" , DecimalDigits = 2,IsNullable = false,ColumnDescription = "长度(cm)" )]
        public decimal Length
        { 
            get{return _Length;}
            set{
            SetProperty(ref _Length, value);
            }
        }

        private decimal _Width;
        /// <summary>
        /// 宽度(cm)
        /// </summary>
        [AdvQueryAttribute(ColName = "Width",ColDesc = "宽度(cm)")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Width" , DecimalDigits = 2,IsNullable = false,ColumnDescription = "宽度(cm)" )]
        public decimal Width
        { 
            get{return _Width;}
            set{
            SetProperty(ref _Width, value);
            }
        }

        private decimal _Height;
        /// <summary>
        /// 高度(cm)
        /// </summary>
        [AdvQueryAttribute(ColName = "Height",ColDesc = "高度(cm)")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Height" , DecimalDigits = 2,IsNullable = false,ColumnDescription = "高度(cm)" )]
        public decimal Height
        { 
            get{return _Height;}
            set{
            SetProperty(ref _Height, value);
            }
        }

        private decimal _Volume;
        /// <summary>
        /// 体积Vol(cm³)
        /// </summary>
        [AdvQueryAttribute(ColName = "Volume",ColDesc = "体积Vol(cm³)")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Volume" , DecimalDigits = 3,IsNullable = false,ColumnDescription = "体积Vol(cm³)" )]
        public decimal Volume
        { 
            get{return _Volume;}
            set{
            SetProperty(ref _Volume, value);
            }
        }

        private decimal _NetWeight;
        /// <summary>
        /// 净重N.Wt.(kg)
        /// </summary>
        [AdvQueryAttribute(ColName = "NetWeight",ColDesc = "净重N.Wt.(kg)")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "NetWeight" , DecimalDigits = 3,IsNullable = false,ColumnDescription = "净重N.Wt.(kg)" )]
        public decimal NetWeight
        { 
            get{return _NetWeight;}
            set{
            SetProperty(ref _NetWeight, value);
            }
        }

        private decimal _GrossWeight;
        /// <summary>
        /// 毛重G.Wt.(kg)
        /// </summary>
        [AdvQueryAttribute(ColName = "GrossWeight",ColDesc = "毛重G.Wt.(kg)")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "GrossWeight" , DecimalDigits = 3,IsNullable = false,ColumnDescription = "毛重G.Wt.(kg)" )]
        public decimal GrossWeight
        { 
            get{return _GrossWeight;}
            set{
            SetProperty(ref _GrossWeight, value);
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

        private bool _Is_enabled= true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_enabled",ColDesc = "是否启用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Is_enabled" ,IsNullable = false,ColumnDescription = "是否启用" )]
        public bool Is_enabled
        { 
            get{return _Is_enabled;}
            set{
            SetProperty(ref _Is_enabled, value);
            }
        }

        private DateTime? _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Created_at" ,IsNullable = true,ColumnDescription = "创建时间" )]
        public DateTime? Created_at
        { 
            get{return _Created_at;}
            set{
            SetProperty(ref _Created_at, value);
            }
        }

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Created_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "创建人" )]
        public long? Created_by
        { 
            get{return _Created_by;}
            set{
            SetProperty(ref _Created_by, value);
            }
        }

        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Modified_at" ,IsNullable = true,ColumnDescription = "修改时间" )]
        public DateTime? Modified_at
        { 
            get{return _Modified_at;}
            set{
            SetProperty(ref _Modified_at, value);
            }
        }

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Modified_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "修改人" )]
        public long? Modified_by
        { 
            get{return _Modified_by;}
            set{
            SetProperty(ref _Modified_by, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(CartonID))]
        public virtual tb_CartoonBox tb_cartoonbox { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Pack_ID))]
        public virtual tb_Packing tb_packing { get; set; }



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
                    Type type = typeof(tb_BoxRules);
                    
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
            tb_BoxRules loctype = (tb_BoxRules)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

