
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/07/2024 19:06:30
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
    /// 货品基本信息表
    /// </summary>
    [Serializable()]
    [Description("tb_Prod")]
    [SugarTable("tb_Prod")]
    public partial class tb_Prod: BaseEntity, ICloneable
    {
        public tb_Prod()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_Prod" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ProdBaseID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdBaseID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long ProdBaseID
        { 
            get{return _ProdBaseID;}
            set{
            base.PrimaryKeyID = _ProdBaseID;
            SetProperty(ref _ProdBaseID, value);
            }
        }

        private string _ProductNo;
        /// <summary>
        /// 品号
        /// </summary>
        [AdvQueryAttribute(ColName = "ProductNo",ColDesc = "品号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ProductNo" ,Length=40,IsNullable = false,ColumnDescription = "品号" )]
        public string ProductNo
        { 
            get{return _ProductNo;}
            set{
            SetProperty(ref _ProductNo, value);
            }
        }

        private string _CNName;
        /// <summary>
        /// 品名
        /// </summary>
        [AdvQueryAttribute(ColName = "CNName",ColDesc = "品名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CNName" ,Length=255,IsNullable = false,ColumnDescription = "品名" )]
        public string CNName
        { 
            get{return _CNName;}
            set{
            SetProperty(ref _CNName, value);
            }
        }

        private string _ImagesPath;
        /// <summary>
        /// 图片组
        /// </summary>
        [AdvQueryAttribute(ColName = "ImagesPath",ColDesc = "图片组")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ImagesPath" ,Length=2000,IsNullable = true,ColumnDescription = "图片组" )]
        public string ImagesPath
        { 
            get{return _ImagesPath;}
            set{
            SetProperty(ref _ImagesPath, value);
            }
        }

        private byte[] _Images;
        /// <summary>
        /// 产品图
        /// </summary>
        [AdvQueryAttribute(ColName = "Images",ColDesc = "产品图")] 
        [SugarColumn(ColumnDataType = "image", SqlParameterDbType ="Binary",  ColumnName = "Images" ,Length=2147483647,IsNullable = true,ColumnDescription = "产品图" )]
        public byte[] Images
        { 
            get{return _Images;}
            set{
            SetProperty(ref _Images, value);
            }
        }

        private string _ENName;
        /// <summary>
        /// 英文名称
        /// </summary>
        [AdvQueryAttribute(ColName = "ENName",ColDesc = "英文名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ENName" ,Length=255,IsNullable = true,ColumnDescription = "英文名称" )]
        public string ENName
        { 
            get{return _ENName;}
            set{
            SetProperty(ref _ENName, value);
            }
        }

        private string _Model;
        /// <summary>
        /// 型号
        /// </summary>
        [AdvQueryAttribute(ColName = "Model",ColDesc = "型号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Model" ,Length=50,IsNullable = true,ColumnDescription = "型号" )]
        public string Model
        { 
            get{return _Model;}
            set{
            SetProperty(ref _Model, value);
            }
        }

        private string _ShortCode;
        /// <summary>
        /// 助记码
        /// </summary>
        [AdvQueryAttribute(ColName = "ShortCode",ColDesc = "助记码")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ShortCode" ,Length=50,IsNullable = true,ColumnDescription = "助记码" )]
        public string ShortCode
        { 
            get{return _ShortCode;}
            set{
            SetProperty(ref _ShortCode, value);
            }
        }

        private string _Specifications;
        /// <summary>
        /// 规格
        /// </summary>
        [AdvQueryAttribute(ColName = "Specifications",ColDesc = "规格")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Specifications" ,Length=1000,IsNullable = true,ColumnDescription = "规格" )]
        public string Specifications
        { 
            get{return _Specifications;}
            set{
            SetProperty(ref _Specifications, value);
            }
        }

        private int? _SourceType;
        /// <summary>
        /// 货品来源
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceType",ColDesc = "货品来源")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "SourceType" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "货品来源" )]
        public int? SourceType
        { 
            get{return _SourceType;}
            set{
            SetProperty(ref _SourceType, value);
            }
        }

        private long? _DepartmentID;
        /// <summary>
        /// 所属部门
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "所属部门")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "所属部门" )]
        [FKRelationAttribute("tb_Department","DepartmentID")]
        public long? DepartmentID
        { 
            get{return _DepartmentID;}
            set{
            SetProperty(ref _DepartmentID, value);
            }
        }

        private int _PropertyType;
        /// <summary>
        /// 货品类型
        /// </summary>
        [AdvQueryAttribute(ColName = "PropertyType",ColDesc = "货品类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "PropertyType" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "货品类型" )]
        public int PropertyType
        { 
            get{return _PropertyType;}
            set{
            SetProperty(ref _PropertyType, value);
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

        private long? _Category_ID;
        /// <summary>
        /// 类别
        /// </summary>
        [AdvQueryAttribute(ColName = "Category_ID",ColDesc = "类别")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Category_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "类别" )]
        [FKRelationAttribute("tb_ProdCategories","Category_ID")]
        public long? Category_ID
        { 
            get{return _Category_ID;}
            set{
            SetProperty(ref _Category_ID, value);
            }
        }

        private long _Type_ID;
        /// <summary>
        /// 货品类型
        /// </summary>
        [AdvQueryAttribute(ColName = "Type_ID",ColDesc = "货品类型")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Type_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "货品类型" )]
        [FKRelationAttribute("tb_ProductType","Type_ID")]
        public long Type_ID
        { 
            get{return _Type_ID;}
            set{
            SetProperty(ref _Type_ID, value);
            }
        }

        private long? _CustomerVendor_ID;
        /// <summary>
        /// 厂商
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "厂商")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "厂商" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long? CustomerVendor_ID
        { 
            get{return _CustomerVendor_ID;}
            set{
            SetProperty(ref _CustomerVendor_ID, value);
            }
        }

        private long? _Location_ID;
        /// <summary>
        /// 默认仓库
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "默认仓库")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Location_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "默认仓库" )]
        [FKRelationAttribute("tb_Location","Location_ID")]
        public long? Location_ID
        { 
            get{return _Location_ID;}
            set{
            SetProperty(ref _Location_ID, value);
            }
        }

        private long? _Rack_ID;
        /// <summary>
        /// 默认货架
        /// </summary>
        [AdvQueryAttribute(ColName = "Rack_ID",ColDesc = "默认货架")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Rack_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "默认货架" )]
        [FKRelationAttribute("tb_StorageRack","Rack_ID")]
        public long? Rack_ID
        { 
            get{return _Rack_ID;}
            set{
            SetProperty(ref _Rack_ID, value);
            }
        }

        private long? _Employee_ID;
        /// <summary>
        /// 业务员
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "业务员")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "业务员" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long? Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
            }
        }

        private string _Brand;
        /// <summary>
        /// 品牌
        /// </summary>
        [AdvQueryAttribute(ColName = "Brand",ColDesc = "品牌")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Brand" ,Length=50,IsNullable = true,ColumnDescription = "品牌" )]
        public string Brand
        { 
            get{return _Brand;}
            set{
            SetProperty(ref _Brand, value);
            }
        }

        private string _ProductENDesc;
        /// <summary>
        /// 英文详情
        /// </summary>
        [AdvQueryAttribute(ColName = "ProductENDesc",ColDesc = "英文详情")] 
        [SugarColumn(ColumnDataType = "text", SqlParameterDbType ="String",  ColumnName = "ProductENDesc" ,Length=2147483647,IsNullable = true,ColumnDescription = "英文详情" )]
        public string ProductENDesc
        { 
            get{return _ProductENDesc;}
            set{
            SetProperty(ref _ProductENDesc, value);
            }
        }

        private string _ProductCNDesc;
        /// <summary>
        /// 中文详情
        /// </summary>
        [AdvQueryAttribute(ColName = "ProductCNDesc",ColDesc = "中文详情")] 
        [SugarColumn(ColumnDataType = "text", SqlParameterDbType ="String",  ColumnName = "ProductCNDesc" ,Length=2147483647,IsNullable = true,ColumnDescription = "中文详情" )]
        public string ProductCNDesc
        { 
            get{return _ProductCNDesc;}
            set{
            SetProperty(ref _ProductCNDesc, value);
            }
        }

        private decimal? _TaxRate;
        /// <summary>
        /// 税率
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxRate",ColDesc = "税率")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "TaxRate" , DecimalDigits = 3,IsNullable = true,ColumnDescription = "税率" )]
        public decimal? TaxRate
        { 
            get{return _TaxRate;}
            set{
            SetProperty(ref _TaxRate, value);
            }
        }

        private string _CustomsCode;
        /// <summary>
        /// 海关编码
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomsCode",ColDesc = "海关编码")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomsCode" ,Length=30,IsNullable = true,ColumnDescription = "海关编码" )]
        public string CustomsCode
        { 
            get{return _CustomsCode;}
            set{
            SetProperty(ref _CustomsCode, value);
            }
        }

        private string _Tag;
        /// <summary>
        /// 标签
        /// </summary>
        [AdvQueryAttribute(ColName = "Tag",ColDesc = "标签")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Tag" ,Length=250,IsNullable = true,ColumnDescription = "标签" )]
        public string Tag
        { 
            get{return _Tag;}
            set{
            SetProperty(ref _Tag, value);
            }
        }

        private bool? _SalePublish;
        /// <summary>
        /// 参与分销
        /// </summary>
        [AdvQueryAttribute(ColName = "SalePublish",ColDesc = "参与分销")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "SalePublish" ,IsNullable = true,ColumnDescription = "参与分销" )]
        public bool? SalePublish
        { 
            get{return _SalePublish;}
            set{
            SetProperty(ref _SalePublish, value);
            }
        }

        private bool? _Is_enabled= true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_enabled",ColDesc = "是否启用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Is_enabled" ,IsNullable = true,ColumnDescription = "是否启用" )]
        public bool? Is_enabled
        { 
            get{return _Is_enabled;}
            set{
            SetProperty(ref _Is_enabled, value);
            }
        }

        private bool? _Is_available= true;
        /// <summary>
        /// 是否可用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_available",ColDesc = "是否可用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Is_available" ,IsNullable = true,ColumnDescription = "是否可用" )]
        public bool? Is_available
        { 
            get{return _Is_available;}
            set{
            SetProperty(ref _Is_available, value);
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

        private bool _isdeleted= false;
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "isdeleted" ,IsNullable = false,ColumnDescription = "逻辑删除" )]
        [Browsable(false)]
        public bool isdeleted
        { 
            get{return _isdeleted;}
            set{
            SetProperty(ref _isdeleted, value);
            }
        }

        private int? _DataStatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "DataStatus",ColDesc = "数据状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DataStatus" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "数据状态" )]
        public int? DataStatus
        { 
            get{return _DataStatus;}
            set{
            SetProperty(ref _DataStatus, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(CustomerVendor_ID))]
        public virtual tb_CustomerVendor tb_customervendor { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(DepartmentID))]
        public virtual tb_Department tb_department { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Category_ID))]
        public virtual tb_ProdCategories tb_prodcategories { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Type_ID))]
        public virtual tb_ProductType tb_producttype { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Unit_ID))]
        public virtual tb_Unit tb_unit { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Rack_ID))]
        public virtual tb_StorageRack tb_storagerack { get; set; }


        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProdDetail.ProdBaseID))]
        public virtual List<tb_ProdDetail> tb_ProdDetails { get; set; }
        //tb_ProdDetail.ProdBaseID)
        //ProdBaseID.FK_TB_PRODD_REFERENCE_TB_PROD)
        //tb_Prod.ProdBaseID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Prod_Attr_Relation.ProdBaseID))]
        public virtual List<tb_Prod_Attr_Relation> tb_Prod_Attr_Relations { get; set; }
        //tb_Prod_Attr_Relation.ProdBaseID)
        //ProdBaseID.FK_TB_PROD__REF_TB_PROD_4)
        //tb_Prod.ProdBaseID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Packing.ProdBaseID))]
        public virtual List<tb_Packing> tb_Packings { get; set; }
        //tb_Packing.ProdBaseID)
        //ProdBaseID.FK_PACKingSpec_REF_PROD)
        //tb_Prod.ProdBaseID)


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
                    Type type = typeof(tb_Prod);
                    
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
            tb_Prod loctype = (tb_Prod)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

