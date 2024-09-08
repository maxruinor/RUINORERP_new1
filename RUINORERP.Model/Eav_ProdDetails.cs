using RUINORERP.Global.CustomAttribute;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model
{
    /// <summary>
    /// 一个中间实体
    /// 这样 如果修改了表，这忘记修改了，不好。可以做一个视图？
    /// </summary>
    public partial class Eav_ProdDetails : BaseEntity, ICloneable
    {
        public Eav_ProdDetails()
        {
            base.FieldNameList = fieldNameList;
        }

        private string _GroupName;
        /// <summary>
        /// 属性组名称
        /// </summary>
        [SugarColumn(ColumnName = "GroupName", IsNullable = true, ColumnDescription = "特性组合")]
        public string GroupName
        {
            get { return _GroupName; }
            set
            {
                SetProperty(ref _GroupName, value);
            }
        }

        #region 字段描述对应列表
        private ConcurrentDictionary<string, string> fieldNameList;


        /// <summary>
        /// 表列名的中文描述集合
        /// </summary>
        [Description("列名中文描述"), Category("自定属性"), Browsable(true)]
        [SugarColumn(IsIgnore = true)]
        public override ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, string>();
                    SugarColumn entityAttr;
                    Type type = typeof(Eav_ProdDetails);

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
            Eav_ProdDetails prodproptype = (Eav_ProdDetails)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return prodproptype;
        }



        private long _ProdDetailID;
        /// <summary>
        /// 产品详情
        /// </summary>
        [SugarColumn(ColumnName = "Prod_Detail_ID", IsNullable = false, ColumnDescription = "产品详情", IsPrimaryKey = true, IsIdentity = true)]
        public long ProdDetailID
        {
            get { return _ProdDetailID; }
            set
            {
                SetProperty(ref _ProdDetailID, value);
            }
        }


        private string _SKU;
        /// <summary>
        /// SKU码
        /// </summary>
        [SugarColumn(ColumnName = "SKU", Length = 80, IsNullable = true, ColumnDescription = "SKU码")]
        public string SKU
        {
            get { return _SKU; }
            set
            {
                SetProperty(ref _SKU, value);
            }
        }


        private string _BarCode;
        /// <summary>
        /// 条码
        /// </summary>
        [SugarColumn(ColumnName = "BarCode", Length = 50, IsNullable = true, ColumnDescription = "条码")]
        public string BarCode
        {
            get { return _BarCode; }
            set
            {
                SetProperty(ref _BarCode, value);
            }
        }


        private string _ImagesPath;
        /// <summary>
        /// 产品图片
        /// </summary>
        [SugarColumn(ColumnName = "ImagesPath", Length = 500, IsNullable = true, ColumnDescription = "产品图片")]
        public string ImagesPath
        {
            get { return _ImagesPath; }
            set
            {
                SetProperty(ref _ImagesPath, value);
            }
        }

        private decimal? _Weight;
        /// <summary>
        /// 重量
        /// </summary>
        [SugarColumn(ColumnName = "Weight", IsNullable = true, ColumnDescription = "重量")]
        public decimal? Weight
        {
            get { return _Weight; }
            set
            {
                SetProperty(ref _Weight, value);
            }
        }


        private decimal? _Purchase_price;
        /// <summary>
        /// 采购价格
        /// </summary>
        [Browsable(false)]
        [SugarColumn(ColumnName = "Purchase_price", IsNullable = true, ColumnDescription = "采购价格")]
        public decimal? Purchase_price
        {
            get { return _Purchase_price; }
            set
            {
                SetProperty(ref _Purchase_price, value);
            }
        }


        private decimal? _Standard_Price;
        /// <summary>
        /// 销售价格
        /// </summary>
        [SugarColumn(ColumnName = "Standard_Price", IsNullable = true, ColumnDescription = "销售价格")]
        public decimal? Standard_Price
        {
            get { return _Standard_Price; }
            set
            {
                SetProperty(ref _Standard_Price, value);
            }
        }


        private decimal? _Transfer_price;
        /// <summary>
        /// 调拨价格
        /// </summary>
        [SugarColumn(ColumnName = "Transfer_price", IsNullable = true, ColumnDescription = "调拨价格")]
        public decimal? Transfer_price
        {
            get { return _Transfer_price; }
            set
            {
                SetProperty(ref _Transfer_price, value);
            }
        }


        private decimal? _Cost_price;
        /// <summary>
        /// 成本价格
        /// </summary>
        [Browsable(false)]
        [SugarColumn(ColumnName = "Cost_price", IsNullable = true, ColumnDescription = "成本价格")]
        public decimal? Cost_price
        {
            get { return _Cost_price; }
            set
            {
                SetProperty(ref _Cost_price, value);
            }
        }


        private decimal? _Market_price;
        /// <summary>
        /// 市场价格
        /// </summary>
        [SugarColumn(ColumnName = "Market_price", IsNullable = true, ColumnDescription = "市场价格")]
        public decimal? Market_price
        {
            get { return _Market_price; }
            set
            {
                SetProperty(ref _Market_price, value);
            }
        }


        private decimal? _Discount_price;
        /// <summary>
        /// 折扣价格
        /// </summary>
        [SugarColumn(ColumnName = "Discount_price", IsNullable = true, ColumnDescription = "折扣价格")]
        public decimal? Discount_price
        {
            get { return _Discount_price; }
            set
            {
                SetProperty(ref _Discount_price, value);
            }
        }


        private byte[] _Image;
        /// <summary>
        /// 产品图片
        /// </summary>
        [SugarColumn(ColumnName = "Image", Length = 2147483647, IsNullable = true, ColumnDescription = "产品图片")]
        public byte[] Image
        {
            get { return _Image; }
            set
            {
                SetProperty(ref _Image, value);
            }
        }


        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "Notes", Length = 255, IsNullable = true, ColumnDescription = "备注")]
        public string Notes
        {
            get { return _Notes; }
            set
            {
                SetProperty(ref _Notes, value);
            }
        }


        private bool _Is_enabled = true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [SugarColumn(ColumnName = "Is_enabled", IsNullable = true, ColumnDescription = "是否启用")]
        public bool Is_enabled
        {
            get { return _Is_enabled; }
            set
            {
                SetProperty(ref _Is_enabled, value);
            }
        }


        private bool _Is_available = true;
        /// <summary>
        /// 是否可用
        /// </summary>
        [SugarColumn(ColumnName = "Is_available", IsNullable = true, ColumnDescription = "是否可用")]
        public bool Is_available
        {
            get { return _Is_available; }
            set
            {
                SetProperty(ref _Is_available, value);
            }
        }


        private DateTime? _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [Browsable(false)]
        [SugarColumn(ColumnName = "Created_at", IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime? Created_at
        {
            get { return _Created_at; }
            set
            {
                SetProperty(ref _Created_at, value);
            }
        }


        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by", ColDesc = "创建人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Created_by", DecimalDigits = 0, IsNullable = true, ColumnDescription = "创建人")]
        public long? Created_by
        {
            get { return _Created_by; }
            set
            {
                SetProperty(ref _Created_by, value);
            }
        }


        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [Browsable(false)]
        [SugarColumn(IsIgnore = true, ColumnName = "Modified_at", IsNullable = true, ColumnDescription = "修改时间")]
        public DateTime? Modified_at
        {
            get { return _Modified_at; }
            set
            {
                SetProperty(ref _Modified_at, value);
            }
        }


        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by", ColDesc = "修改人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Modified_by", DecimalDigits = 0, IsNullable = true, ColumnDescription = "修改人")]
        public long? Modified_by
        {
            get { return _Modified_by; }
            set
            {
                SetProperty(ref _Modified_by, value);
            }
        }

        #region 中间数据 编辑时 临时保存关系，用于删除更新前后

        private List<tb_Prod_Attr_Relation> _tb_Prod_Attr_Relations;

        /// <summary>
        /// 一行SKU明细 可能有多行关系，多属性时
        /// </summary>
        public List<tb_Prod_Attr_Relation> tb_Prod_Attr_Relations
        {
            get { return _tb_Prod_Attr_Relations; }
            set
            {
                SetProperty(ref _tb_Prod_Attr_Relations, value);
            }
        }

        /// <summary>
        /// 一行SKU明细只会有一行明细。暂存数据
        /// </summary>
        public tb_ProdDetail tb_ProdDetail { get; set; }

        #endregion


    }
}
