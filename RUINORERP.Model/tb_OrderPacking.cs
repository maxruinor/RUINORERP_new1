
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:58
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
    /// 订单装箱单，针对特别订单详细处理 也用主子表来做。暂时不搞
    /// </summary>
    [Serializable()]
    [Description("tb_OrderPacking")]
    [SugarTable("tb_OrderPacking")]
    public partial class tb_OrderPacking: BaseEntity, ICloneable
    {
        public tb_OrderPacking()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_OrderPacking" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _OrderPackaging_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "OrderPackaging_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long OrderPackaging_ID
        { 
            get{return _OrderPackaging_ID;}
            set{
            base.PrimaryKeyID = _OrderPackaging_ID;
            SetProperty(ref _OrderPackaging_ID, value);
            }
        }

        private long _SOrder_ID;
        /// <summary>
        /// 订单
        /// </summary>
        [AdvQueryAttribute(ColName = "SOrder_ID",ColDesc = "订单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "SOrder_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "订单" )]
        [FKRelationAttribute("tb_SaleOrder","SOrder_ID")]
        public long SOrder_ID
        { 
            get{return _SOrder_ID;}
            set{
            SetProperty(ref _SOrder_ID, value);
            }
        }

        private string _BoxNo;
        /// <summary>
        /// 箱号
        /// </summary>
        [AdvQueryAttribute(ColName = "BoxNo",ColDesc = "箱号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BoxNo" ,Length=50,IsNullable = false,ColumnDescription = "箱号" )]
        public string BoxNo
        { 
            get{return _BoxNo;}
            set{
            SetProperty(ref _BoxNo, value);
            }
        }

        private string _BoxMark;
        /// <summary>
        /// 箱唛
        /// </summary>
        [AdvQueryAttribute(ColName = "BoxMark",ColDesc = "箱唛")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BoxMark" ,Length=100,IsNullable = true,ColumnDescription = "箱唛" )]
        public string BoxMark
        { 
            get{return _BoxMark;}
            set{
            SetProperty(ref _BoxMark, value);
            }
        }

        private string _Remarks;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Remarks",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Remarks" ,Length=255,IsNullable = true,ColumnDescription = "备注" )]
        public string Remarks
        { 
            get{return _Remarks;}
            set{
            SetProperty(ref _Remarks, value);
            }
        }

        private int _QuantityPerBox;
        /// <summary>
        /// 数量
        /// </summary>
        [AdvQueryAttribute(ColName = "QuantityPerBox",ColDesc = "数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "QuantityPerBox" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "数量" )]
        public int QuantityPerBox
        { 
            get{return _QuantityPerBox;}
            set{
            SetProperty(ref _QuantityPerBox, value);
            }
        }

        private decimal _Length= ((0));
        /// <summary>
        /// 长度(CM)
        /// </summary>
        [AdvQueryAttribute(ColName = "Length",ColDesc = "长度(CM)")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Length" , DecimalDigits = 2,IsNullable = false,ColumnDescription = "长度(CM)" )]
        public decimal Length
        { 
            get{return _Length;}
            set{
            SetProperty(ref _Length, value);
            }
        }

        private decimal _Width= ((0));
        /// <summary>
        /// 宽度(CM)
        /// </summary>
        [AdvQueryAttribute(ColName = "Width",ColDesc = "宽度(CM)")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Width" , DecimalDigits = 2,IsNullable = false,ColumnDescription = "宽度(CM)" )]
        public decimal Width
        { 
            get{return _Width;}
            set{
            SetProperty(ref _Width, value);
            }
        }

        private decimal _Height= ((0));
        /// <summary>
        /// 高度(CM)
        /// </summary>
        [AdvQueryAttribute(ColName = "Height",ColDesc = "高度(CM)")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Height" , DecimalDigits = 2,IsNullable = false,ColumnDescription = "高度(CM)" )]
        public decimal Height
        { 
            get{return _Height;}
            set{
            SetProperty(ref _Height, value);
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

        private string _BoxMaterial;
        /// <summary>
        /// 箱子材质
        /// </summary>
        [AdvQueryAttribute(ColName = "BoxMaterial",ColDesc = "箱子材质")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BoxMaterial" ,Length=200,IsNullable = true,ColumnDescription = "箱子材质" )]
        public string BoxMaterial
        { 
            get{return _BoxMaterial;}
            set{
            SetProperty(ref _BoxMaterial, value);
            }
        }

        private decimal _Volume= ((0));
        /// <summary>
        /// 体积(CM)
        /// </summary>
        [AdvQueryAttribute(ColName = "Volume",ColDesc = "体积(CM)")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Volume" , DecimalDigits = 2,IsNullable = false,ColumnDescription = "体积(CM)" )]
        public decimal Volume
        { 
            get{return _Volume;}
            set{
            SetProperty(ref _Volume, value);
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

        private decimal? _GrossWeight;
        /// <summary>
        /// 毛重(KG)
        /// </summary>
        [AdvQueryAttribute(ColName = "GrossWeight",ColDesc = "毛重(KG)")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "GrossWeight" , DecimalDigits = 3,IsNullable = true,ColumnDescription = "毛重(KG)" )]
        public decimal? GrossWeight
        { 
            get{return _GrossWeight;}
            set{
            SetProperty(ref _GrossWeight, value);
            }
        }

        private decimal? _NetWeight;
        /// <summary>
        /// 净重(KG)
        /// </summary>
        [AdvQueryAttribute(ColName = "NetWeight",ColDesc = "净重(KG)")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "NetWeight" , DecimalDigits = 3,IsNullable = true,ColumnDescription = "净重(KG)" )]
        public decimal? NetWeight
        { 
            get{return _NetWeight;}
            set{
            SetProperty(ref _NetWeight, value);
            }
        }

        private string _PackingMethod;
        /// <summary>
        /// 打包方式
        /// </summary>
        [AdvQueryAttribute(ColName = "PackingMethod",ColDesc = "打包方式")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PackingMethod" ,Length=100,IsNullable = true,ColumnDescription = "打包方式" )]
        public string PackingMethod
        { 
            get{return _PackingMethod;}
            set{
            SetProperty(ref _PackingMethod, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(SOrder_ID))]
        public virtual tb_SaleOrder tb_saleorder { get; set; }



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
                    Type type = typeof(tb_OrderPacking);
                    
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
            tb_OrderPacking loctype = (tb_OrderPacking)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

