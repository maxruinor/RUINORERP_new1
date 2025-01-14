
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/14/2025 18:57:07
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
    /// 采购商品建议
    /// </summary>
    [Serializable()]
    [Description("采购商品建议")]
    [SugarTable("tb_PurGoodsRecommendDetail")]
    public partial class tb_PurGoodsRecommendDetail: BaseEntity, ICloneable
    {
        public tb_PurGoodsRecommendDetail()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("采购商品建议tb_PurGoodsRecommendDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PGRCID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PGRCID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long PGRCID
        { 
            get{return _PGRCID;}
            set{
            base.PrimaryKeyID = _PGRCID;
            SetProperty(ref _PGRCID, value);
            }
        }

        private long? _PDID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "PDID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PDID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_ProductionDemand","PDID")]
        public long? PDID
        { 
            get{return _PDID;}
            set{
            SetProperty(ref _PDID, value);
            }
        }

        private long _ProdDetailID;
        /// <summary>
        /// 货品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "货品")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "货品" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
            }
        }

        private long _Location_ID;
        /// <summary>
        /// 库位
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "库位")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Location_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "库位" )]
        [FKRelationAttribute("tb_Location","Location_ID")]
        public long Location_ID
        { 
            get{return _Location_ID;}
            set{
            SetProperty(ref _Location_ID, value);
            }
        }

        private string _property;
        /// <summary>
        /// 属性
        /// </summary>
        [AdvQueryAttribute(ColName = "property",ColDesc = "属性")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "property" ,Length=255,IsNullable = true,ColumnDescription = "属性" )]
        public string property
        { 
            get{return _property;}
            set{
            SetProperty(ref _property, value);
            }
        }

        private long? _CustomerVendor_ID;
        /// <summary>
        /// 供应商
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "供应商")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "供应商" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long? CustomerVendor_ID
        { 
            get{return _CustomerVendor_ID;}
            set{
            SetProperty(ref _CustomerVendor_ID, value);
            }
        }

        private decimal _RecommendPurPrice= ((0));
        /// <summary>
        /// 建议采购价
        /// </summary>
        [AdvQueryAttribute(ColName = "RecommendPurPrice",ColDesc = "建议采购价")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "RecommendPurPrice" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "建议采购价" )]
        public decimal RecommendPurPrice
        { 
            get{return _RecommendPurPrice;}
            set{
            SetProperty(ref _RecommendPurPrice, value);
            }
        }

        private int _ActualRequiredQty;
        /// <summary>
        /// 需求数量
        /// </summary>
        [AdvQueryAttribute(ColName = "ActualRequiredQty",ColDesc = "需求数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ActualRequiredQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "需求数量" )]
        public int ActualRequiredQty
        { 
            get{return _ActualRequiredQty;}
            set{
            SetProperty(ref _ActualRequiredQty, value);
            }
        }

        private int _RecommendQty= ((0));
        /// <summary>
        /// 建议量
        /// </summary>
        [AdvQueryAttribute(ColName = "RecommendQty",ColDesc = "建议量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "RecommendQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "建议量" )]
        public int RecommendQty
        { 
            get{return _RecommendQty;}
            set{
            SetProperty(ref _RecommendQty, value);
            }
        }

        private int _RequirementQty= ((0));
        /// <summary>
        /// 请购量
        /// </summary>
        [AdvQueryAttribute(ColName = "RequirementQty",ColDesc = "请购量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "RequirementQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "请购量" )]
        public int RequirementQty
        { 
            get{return _RequirementQty;}
            set{
            SetProperty(ref _RequirementQty, value);
            }
        }

        private DateTime _RequirementDate;
        /// <summary>
        /// 需求日期
        /// </summary>
        [AdvQueryAttribute(ColName = "RequirementDate",ColDesc = "需求日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "RequirementDate" ,IsNullable = false,ColumnDescription = "需求日期" )]
        public DateTime RequirementDate
        { 
            get{return _RequirementDate;}
            set{
            SetProperty(ref _RequirementDate, value);
            }
        }

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=255,IsNullable = true,ColumnDescription = "摘要" )]
        public string Summary
        { 
            get{return _Summary;}
            set{
            SetProperty(ref _Summary, value);
            }
        }

        private string _RefBillNO;
        /// <summary>
        /// 生成单号
        /// </summary>
        [AdvQueryAttribute(ColName = "RefBillNO",ColDesc = "生成单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "RefBillNO" ,Length=100,IsNullable = true,ColumnDescription = "生成单号" )]
        public string RefBillNO
        { 
            get{return _RefBillNO;}
            set{
            SetProperty(ref _RefBillNO, value);
            }
        }

        private long? _RefBillType;
        /// <summary>
        /// 单据类型
        /// </summary>
        [AdvQueryAttribute(ColName = "RefBillType",ColDesc = "单据类型")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RefBillType" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "单据类型" )]
        public long? RefBillType
        { 
            get{return _RefBillType;}
            set{
            SetProperty(ref _RefBillType, value);
            }
        }

        private long? _RefBillID;
        /// <summary>
        /// 生成单据
        /// </summary>
        [AdvQueryAttribute(ColName = "RefBillID",ColDesc = "生成单据")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RefBillID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "生成单据" )]
        public long? RefBillID
        { 
            get{return _RefBillID;}
            set{
            SetProperty(ref _RefBillID, value);
            }
        }

        private long? _PDCID_RowID;
        /// <summary>
        /// 生成单据
        /// </summary>
        [AdvQueryAttribute(ColName = "PDCID_RowID",ColDesc = "生成单据")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PDCID_RowID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "生成单据" )]
        public long? PDCID_RowID
        { 
            get{return _PDCID_RowID;}
            set{
            SetProperty(ref _PDCID_RowID, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(CustomerVendor_ID))]
        public virtual tb_CustomerVendor tb_customervendor { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(PDID))]
        public virtual tb_ProductionDemand tb_productiondemand { get; set; }



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
                    Type type = typeof(tb_PurGoodsRecommendDetail);
                    
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
            tb_PurGoodsRecommendDetail loctype = (tb_PurGoodsRecommendDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

