
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:14
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
    /// 产品详细表
    /// </summary>
    [Serializable()]
    [Description("产品详细表")]
    [SugarTable("tb_ProdDetail")]
    public partial class tb_ProdDetail: BaseEntity, ICloneable
    {
        public tb_ProdDetail()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("产品详细表tb_ProdDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ProdDetailID;
        /// <summary>
        /// 货品
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "货品" , IsPrimaryKey = true)]
        public long ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
                base.PrimaryKeyID = _ProdDetailID;
            }
        }

        private long? _ProdBaseID;
        /// <summary>
        /// 货品主信息
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdBaseID",ColDesc = "货品主信息")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdBaseID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "货品主信息" )]
        [FKRelationAttribute("tb_Prod","ProdBaseID")]
        public long? ProdBaseID
        { 
            get{return _ProdBaseID;}
            set{
            SetProperty(ref _ProdBaseID, value);
                        }
        }

        private long? _BOM_ID;
        /// <summary>
        /// 标准配方
        /// </summary>
        [AdvQueryAttribute(ColName = "BOM_ID",ColDesc = "标准配方")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BOM_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "标准配方" )]
        [FKRelationAttribute("tb_BOM_S","BOM_ID")]
        public long? BOM_ID
        { 
            get{return _BOM_ID;}
            set{
            SetProperty(ref _BOM_ID, value);
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

        private string _BarCode;
        /// <summary>
        /// 条码
        /// </summary>
        [AdvQueryAttribute(ColName = "BarCode",ColDesc = "条码")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BarCode" ,Length=50,IsNullable = true,ColumnDescription = "条码" )]
        public string BarCode
        { 
            get{return _BarCode;}
            set{
            SetProperty(ref _BarCode, value);
                        }
        }

        private string _ImagesPath;
        /// <summary>
        /// 产品图片
        /// </summary>
        [AdvQueryAttribute(ColName = "ImagesPath",ColDesc = "产品图片")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ImagesPath" ,Length=2000,IsNullable = true,ColumnDescription = "产品图片" )]
        public string ImagesPath
        { 
            get{return _ImagesPath;}
            set{
            SetProperty(ref _ImagesPath, value);
                        }
        }

        private byte[] _Images;
        /// <summary>
        /// 产品图片
        /// </summary>
        [AdvQueryAttribute(ColName = "Images",ColDesc = "产品图片")] 
        [SugarColumn(ColumnDataType = "image", SqlParameterDbType ="Binary",  ColumnName = "Images" ,Length=2147483647,IsNullable = true,ColumnDescription = "产品图片" )]
        public byte[] Images
        { 
            get{return _Images;}
            set{
            SetProperty(ref _Images, value);
                        }
        }

        private decimal? _Weight;
        /// <summary>
        /// 重量（千克）
        /// </summary>
        [AdvQueryAttribute(ColName = "Weight",ColDesc = "重量（千克）")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Weight" , DecimalDigits = 3,IsNullable = true,ColumnDescription = "重量（千克）" )]
        public decimal? Weight
        { 
            get{return _Weight;}
            set{
            SetProperty(ref _Weight, value);
                        }
        }

        private decimal? _Standard_Price;
        /// <summary>
        /// 标准价
        /// </summary>
        [AdvQueryAttribute(ColName = "Standard_Price",ColDesc = "标准价")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Standard_Price" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "标准价" )]
        public decimal? Standard_Price
        { 
            get{return _Standard_Price;}
            set{
            SetProperty(ref _Standard_Price, value);
                        }
        }

        private decimal? _Transfer_Price;
        /// <summary>
        /// 调拨价格
        /// </summary>
        [AdvQueryAttribute(ColName = "Transfer_Price",ColDesc = "调拨价格")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Transfer_Price" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "调拨价格" )]
        public decimal? Transfer_Price
        { 
            get{return _Transfer_Price;}
            set{
            SetProperty(ref _Transfer_Price, value);
                        }
        }

        private decimal? _Wholesale_Price;
        /// <summary>
        /// 批发价格
        /// </summary>
        [AdvQueryAttribute(ColName = "Wholesale_Price",ColDesc = "批发价格")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Wholesale_Price" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "批发价格" )]
        public decimal? Wholesale_Price
        { 
            get{return _Wholesale_Price;}
            set{
            SetProperty(ref _Wholesale_Price, value);
                        }
        }

        private decimal? _Market_Price;
        /// <summary>
        /// 市场零售价
        /// </summary>
        [AdvQueryAttribute(ColName = "Market_Price",ColDesc = "市场零售价")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Market_Price" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "市场零售价" )]
        public decimal? Market_Price
        { 
            get{return _Market_Price;}
            set{
            SetProperty(ref _Market_Price, value);
                        }
        }

        private decimal? _Discount_Price;
        /// <summary>
        /// 折扣价格
        /// </summary>
        [AdvQueryAttribute(ColName = "Discount_Price",ColDesc = "折扣价格")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Discount_Price" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "折扣价格" )]
        public decimal? Discount_Price
        { 
            get{return _Discount_Price;}
            set{
            SetProperty(ref _Discount_Price, value);
                        }
        }

        private byte[] _Image;
        /// <summary>
        /// 产品图片
        /// </summary>
        [AdvQueryAttribute(ColName = "Image",ColDesc = "产品图片")] 
        [SugarColumn(ColumnDataType = "image", SqlParameterDbType ="Binary",  ColumnName = "Image" ,Length=2147483647,IsNullable = true,ColumnDescription = "产品图片" )]
        public byte[] Image
        { 
            get{return _Image;}
            set{
            SetProperty(ref _Image, value);
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

        private bool _SalePublish= true;
        /// <summary>
        /// 参与分销
        /// </summary>
        [AdvQueryAttribute(ColName = "SalePublish",ColDesc = "参与分销")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "SalePublish" ,IsNullable = false,ColumnDescription = "参与分销" )]
        public bool SalePublish
        { 
            get{return _SalePublish;}
            set{
            SetProperty(ref _SalePublish, value);
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
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(BOM_ID))]
        public virtual tb_BOM_S tb_bom_s { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProdBaseID))]
        public virtual tb_Prod tb_prod { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProdSplitDetail.ProdDetailID))]
        public virtual List<tb_ProdSplitDetail> tb_ProdSplitDetails { get; set; }
        //tb_ProdSplitDetail.ProdDetailID)
        //ProdDetailID.FK_PRODSplitDetail_REF_PRODDEtail)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProductionPlanDetail.ProdDetailID))]
        public virtual List<tb_ProductionPlanDetail> tb_ProductionPlanDetails { get; set; }
        //tb_ProductionPlanDetail.ProdDetailID)
        //ProdDetailID.FK_PRODPLANDETAIL_REF_PRODDETAIL)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_StockOutDetail.ProdDetailID))]
        public virtual List<tb_StockOutDetail> tb_StockOutDetails { get; set; }
        //tb_StockOutDetail.ProdDetailID)
        //ProdDetailID.FK_TB_STOCKOUTD_REF_TB_PRODDETAIL)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOutReDetail.ProdDetailID))]
        public virtual List<tb_SaleOutReDetail> tb_SaleOutReDetails { get; set; }
        //tb_SaleOutReDetail.ProdDetailID)
        //ProdDetailID.FK_SOREDETAIL_RE_TB_PRODDDETAIL)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MRP_ReworkReturnDetail.ProdDetailID))]
        public virtual List<tb_MRP_ReworkReturnDetail> tb_MRP_ReworkReturnDetails { get; set; }
        //tb_MRP_ReworkReturnDetail.ProdDetailID)
        //ProdDetailID.FK_MRP_Reworkreturndetail_REF_PRODDetail)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurReturnEntryDetail.ProdDetailID))]
        public virtual List<tb_PurReturnEntryDetail> tb_PurReturnEntryDetails { get; set; }
        //tb_PurReturnEntryDetail.ProdDetailID)
        //ProdDetailID.FK_PURRETRUNENTRYDETAIL_REF_PRODDETAIL)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_BOM_S.ProdDetailID))]
        public virtual List<tb_BOM_S> tb_BOM_Ss { get; set; }
        //tb_BOM_S.ProdDetailID)
        //ProdDetailID.FK_TB_BO_TB_PROD_BOM_S_1)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProduceGoodsRecommendDetail.ProdDetailID))]
        public virtual List<tb_ProduceGoodsRecommendDetail> tb_ProduceGoodsRecommendDetails { get; set; }
        //tb_ProduceGoodsRecommendDetail.ProdDetailID)
        //ProdDetailID.FK_ProduceGoodsRecommendDetail_REF_PRODDETAIL)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MaterialReturnDetail.ProdDetailID))]
        public virtual List<tb_MaterialReturnDetail> tb_MaterialReturnDetails { get; set; }
        //tb_MaterialReturnDetail.ProdDetailID)
        //ProdDetailID.FK_MATERRETURNDETAIL_REB_PRODDETAIL)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FinishedGoodsInvDetail.ProdDetailID))]
        public virtual List<tb_FinishedGoodsInvDetail> tb_FinishedGoodsInvDetails { get; set; }
        //tb_FinishedGoodsInvDetail.ProdDetailID)
        //ProdDetailID.FK_TB_FINISde_REF_TB_PRODDETAIL)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProductionDemandDetail.ProdDetailID))]
        public virtual List<tb_ProductionDemandDetail> tb_ProductionDemandDetails { get; set; }
        //tb_ProductionDemandDetail.ProdDetailID)
        //ProdDetailID.FK_PRODDEMANDDETAIL_REF_PRODDETAIL)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProdMergeDetail.ProdDetailID))]
        public virtual List<tb_ProdMergeDetail> tb_ProdMergeDetails { get; set; }
        //tb_ProdMergeDetail.ProdDetailID)
        //ProdDetailID.FK_PRODMergeDetail_REF_PRODDetail)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOrderDetail.ProdDetailID))]
        public virtual List<tb_SaleOrderDetail> tb_SaleOrderDetails { get; set; }
        //tb_SaleOrderDetail.ProdDetailID)
        //ProdDetailID.FK_TB_SALEODE_REF_TB_PRODDETail)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProdReturningDetail.ProdDetailID))]
        public virtual List<tb_ProdReturningDetail> tb_ProdReturningDetails { get; set; }
        //tb_ProdReturningDetail.ProdDetailID)
        //ProdDetailID.FK_PRODRetruningdetail_REF_PRODDe)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ReturnDetail.ProdDetailID))]
        public virtual List<tb_ReturnDetail> tb_ReturnDetails { get; set; }
        //tb_ReturnDetail.ProdDetailID)
        //ProdDetailID.FK_RETURNDETAIL_PRODDETAIL)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PriceRecord.ProdDetailID))]
        public virtual List<tb_PriceRecord> tb_PriceRecords { get; set; }
        //tb_PriceRecord.ProdDetailID)
        //ProdDetailID.FK_TB_PRICE_REF_TB_PRODDetail)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_StocktakeDetail.ProdDetailID))]
        public virtual List<tb_StocktakeDetail> tb_StocktakeDetails { get; set; }
        //tb_StocktakeDetail.ProdDetailID)
        //ProdDetailID.FK_TB_STOCK_REF_TB_PROD_11)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProductionDemandTargetDetail.ProdDetailID))]
        public virtual List<tb_ProductionDemandTargetDetail> tb_ProductionDemandTargetDetails { get; set; }
        //tb_ProductionDemandTargetDetail.ProdDetailID)
        //ProdDetailID.FK_PRODDEMANDTARGETDETAIL_REF_PRODDETAIL)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurEntryDetail.ProdDetailID))]
        public virtual List<tb_PurEntryDetail> tb_PurEntryDetails { get; set; }
        //tb_PurEntryDetail.ProdDetailID)
        //ProdDetailID.FK_TB_PUREN_REF_TB_PROD_DETAIL)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Prod_Attr_Relation.ProdDetailID))]
        public virtual List<tb_Prod_Attr_Relation> tb_Prod_Attr_Relations { get; set; }
        //tb_Prod_Attr_Relation.ProdDetailID)
        //ProdDetailID.FK_TB_PROD_A_1_TB_PROD_D)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOutReRefurbishedMaterialsDetail.ProdDetailID))]
        public virtual List<tb_SaleOutReRefurbishedMaterialsDetail> tb_SaleOutReRefurbishedMaterialsDetails { get; set; }
        //tb_SaleOutReRefurbishedMaterialsDetail.ProdDetailID)
        //ProdDetailID.FK_TB_SALEOREREMATER_REF_PRODDEtail)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Packing.ProdDetailID))]
        public virtual List<tb_Packing> tb_Packings { get; set; }
        //tb_Packing.ProdDetailID)
        //ProdDetailID.FK_TB_PACKINGSPEC_REF_PRODD_Detail)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurGoodsRecommendDetail.ProdDetailID))]
        public virtual List<tb_PurGoodsRecommendDetail> tb_PurGoodsRecommendDetails { get; set; }
        //tb_PurGoodsRecommendDetail.ProdDetailID)
        //ProdDetailID.FK_PURGOODSEWCOMMENDDETAIL_REF_PRODDETAIL)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProdBorrowingDetail.ProdDetailID))]
        public virtual List<tb_ProdBorrowingDetail> tb_ProdBorrowingDetails { get; set; }
        //tb_ProdBorrowingDetail.ProdDetailID)
        //ProdDetailID.FK_PRODBorrowingdetail_REF_PRODDetail)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProdSplit.ProdDetailID))]
        public virtual List<tb_ProdSplit> tb_ProdSplits { get; set; }
        //tb_ProdSplit.ProdDetailID)
        //ProdDetailID.FK_TB_PRODSplit_REF_PRODDetail)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProdConversionDetail.ProdDetailID_from))]
        public virtual List<tb_ProdConversionDetail> tb_ProdConversionDetails_from { get; set; }
        //tb_ProdConversionDetail.ProdDetailID)
        //ProdDetailID.FK_PRO_REF_TB_PRO_fromID)
        //tb_ProdDetail.ProdDetailID_from)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProdConversionDetail.ProdDetailID_to))]
        public virtual List<tb_ProdConversionDetail> tb_ProdConversionDetails_to { get; set; }
        //tb_ProdConversionDetail.ProdDetailID)
        //ProdDetailID.FK_TB_PRO_REF_TB_PRO_toID)
        //tb_ProdDetail.ProdDetailID_to)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ManufacturingOrderDetail.ProdDetailID))]
        public virtual List<tb_ManufacturingOrderDetail> tb_ManufacturingOrderDetails { get; set; }
        //tb_ManufacturingOrderDetail.ProdDetailID)
        //ProdDetailID.FK_MANUFODETAIL_RE_PRODDETAIL)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_BOM_SDetailSecondary.ProdDetailID))]
        public virtual List<tb_BOM_SDetailSecondary> tb_BOM_SDetailSecondaries { get; set; }
        //tb_BOM_SDetailSecondary.ProdDetailID)
        //ProdDetailID.FK_TB_BOM_S_dede__TB_PROD_D)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOutDetail.ProdDetailID))]
        public virtual List<tb_SaleOutDetail> tb_SaleOutDetails { get; set; }
        //tb_SaleOutDetail.ProdDetailID)
        //ProdDetailID.FK_TB_SALEO_REFERENCE_TB_PRODD)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurOrderDetail.ProdDetailID))]
        public virtual List<tb_PurOrderDetail> tb_PurOrderDetails { get; set; }
        //tb_PurOrderDetail.ProdDetailID)
        //ProdDetailID.FK_PURORDE_TB_PROD_DE)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_BOM_SDetail.ProdDetailID))]
        public virtual List<tb_BOM_SDetail> tb_BOM_SDetails { get; set; }
        //tb_BOM_SDetail.ProdDetailID)
        //ProdDetailID.FK_TB_BOM_S_deta_TB_PROD_D)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_BuyingRequisitionDetail.ProdDetailID))]
        public virtual List<tb_BuyingRequisitionDetail> tb_BuyingRequisitionDetails { get; set; }
        //tb_BuyingRequisitionDetail.ProdDetailID)
        //ProdDetailID.FK_BUYINGREQUISTIONDETAIL_REF_PRODDDETAIL)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProdMerge.ProdDetailID))]
        public virtual List<tb_ProdMerge> tb_ProdMerges { get; set; }
        //tb_ProdMerge.ProdDetailID)
        //ProdDetailID.FK_PRODMerge_REF_PRODDetail)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MRP_ReworkEntryDetail.ProdDetailID))]
        public virtual List<tb_MRP_ReworkEntryDetail> tb_MRP_ReworkEntryDetails { get; set; }
        //tb_MRP_ReworkEntryDetail.ProdDetailID)
        //ProdDetailID.FK_MRP_ReworkEntryDetail_REF_PRODDetail)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurEntryReDetail.ProdDetailID))]
        public virtual List<tb_PurEntryReDetail> tb_PurEntryReDetails { get; set; }
        //tb_PurEntryReDetail.ProdDetailID)
        //ProdDetailID.FK_PURENREDETAIL_RE_PRODDDETAIL)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Inventory.ProdDetailID))]
        public virtual List<tb_Inventory> tb_Inventories { get; set; }
        //tb_Inventory.ProdDetailID)
        //ProdDetailID.FK_TB_INVEN_REFERENCE_TB_PRODD)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_BOM_SDetailSubstituteMaterial.ProdDetailID))]
        public virtual List<tb_BOM_SDetailSubstituteMaterial> tb_BOM_SDetailSubstituteMaterials { get; set; }
        //tb_BOM_SDetailSubstituteMaterial.ProdDetailID)
        //ProdDetailID.FK_TB_BOM_SDetailSubstitue_REF_TB_PRODD)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurOrderReDetail.ProdDetailID))]
        public virtual List<tb_PurOrderReDetail> tb_PurOrderReDetails { get; set; }
        //tb_PurOrderReDetail.ProdDetailID)
        //ProdDetailID.FK_PUROREDETAIL_RE_PRODDETAIL)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PackingDetail.ProdDetailID))]
        public virtual List<tb_PackingDetail> tb_PackingDetails { get; set; }
        //tb_PackingDetail.ProdDetailID)
        //ProdDetailID.FK_PACKINGdETAIL_REF_PRODDETAIL)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ManufacturingOrder.ProdDetailID))]
        public virtual List<tb_ManufacturingOrder> tb_ManufacturingOrders { get; set; }
        //tb_ManufacturingOrder.ProdDetailID)
        //ProdDetailID.FK_MANUFCTURINGORDER_REF_PRODDDETAIL)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_StockInDetail.ProdDetailID))]
        public virtual List<tb_StockInDetail> tb_StockInDetails { get; set; }
        //tb_StockInDetail.ProdDetailID)
        //ProdDetailID.FK_TB_STOCKINDETAIL_REF_PRODDETAIL)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MaterialRequisitionDetail.ProdDetailID))]
        public virtual List<tb_MaterialRequisitionDetail> tb_MaterialRequisitionDetails { get; set; }
        //tb_MaterialRequisitionDetail.ProdDetailID)
        //ProdDetailID.FK_MATEREQUISITIONSDETAIL_REF_PRODDETAIL)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProdBundleDetail.ProdDetailID))]
        public virtual List<tb_ProdBundleDetail> tb_ProdBundleDetails { get; set; }
        //tb_ProdBundleDetail.ProdDetailID)
        //ProdDetailID.FK_PRODBUNDLEDetail_REF_PRODDETAIL)
        //tb_ProdDetail.ProdDetailID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_StockTransferDetail.ProdDetailID))]
        public virtual List<tb_StockTransferDetail> tb_StockTransferDetails { get; set; }
        //tb_StockTransferDetail.ProdDetailID)
        //ProdDetailID.FK_TB_STOCKTRANSFER_REF_PRODDETAIL)
        //tb_ProdDetail.ProdDetailID)


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
                    Type type = typeof(tb_ProdDetail);
                    
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
            tb_ProdDetail loctype = (tb_ProdDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

