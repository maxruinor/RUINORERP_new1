
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:13
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
    /// 产品套装明细
    /// </summary>
    [Serializable()]
    [Description("产品套装明细")]
    [SugarTable("tb_ProdBundleDetail")]
    public partial class tb_ProdBundleDetail: BaseEntity, ICloneable
    {
        public tb_ProdBundleDetail()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("产品套装明细tb_ProdBundleDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _BundleChildID;
        /// <summary>
        /// 套装明细
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BundleChildID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "套装明细" , IsPrimaryKey = true)]
        public long BundleChildID
        { 
            get{return _BundleChildID;}
            set{
            SetProperty(ref _BundleChildID, value);
                base.PrimaryKeyID = _BundleChildID;
            }
        }

        private long _BundleID;
        /// <summary>
        /// 套装组合
        /// </summary>
        [AdvQueryAttribute(ColName = "BundleID",ColDesc = "套装组合")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BundleID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "套装组合" )]
        [FKRelationAttribute("tb_ProdBundle","BundleID")]
        public long BundleID
        { 
            get{return _BundleID;}
            set{
            SetProperty(ref _BundleID, value);
                        }
        }

        private long _ProdDetailID;
        /// <summary>
        /// 产品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "产品" )]
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
        [AdvQueryAttribute(ColName = "SKU", ColDesc = "SKU")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "SKU", Length = 80, IsNullable = true, ColumnDescription = "SKU")]
        public string SKU
        {
            get { return _SKU; }
            set
            {
                SetProperty(ref _SKU, value);
            }
        }

        private int _Quantity= ((1));
        /// <summary>
        /// 数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Quantity" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "数量" )]
        public int Quantity
        { 
            get{return _Quantity;}
            set{
            SetProperty(ref _Quantity, value);
                        }
        }


        private decimal _SaleUnitPrice=0m;
        /// <summary>
        /// 销售单价
        /// </summary>
        [AdvQueryAttribute(ColName = "SaleUnitPrice", ColDesc = "销售单价")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "SaleUnitPrice", DecimalDigits = 4, IsNullable = false, ColumnDescription = "销售单价")]
        public decimal SaleUnitPrice
        {
            get { return _SaleUnitPrice; }
            set
            {
                SetProperty(ref _SaleUnitPrice, value);
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

        private bool _Is_available= true;
        /// <summary>
        /// 是否可用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_available",ColDesc = "是否可用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Is_available" ,IsNullable = false,ColumnDescription = "是否可用" )]
        public bool Is_available
        { 
            get{return _Is_available;}
            set{
            SetProperty(ref _Is_available, value);
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
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(BundleID))]
        public virtual tb_ProdBundle tb_prodbundle { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
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
                    Type type = typeof(tb_ProdBundleDetail);
                    
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
            tb_ProdBundleDetail loctype = (tb_ProdBundleDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

