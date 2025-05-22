
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:09
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
    /// 包装规格表
    /// </summary>
    [Serializable()]
    [Description("包装规格表")]
    [SugarTable("tb_Packing")]
    public partial class tb_Packing: BaseEntity, ICloneable
    {
        public tb_Packing()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("包装规格表tb_Packing" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Pack_ID;
        /// <summary>
        /// 包装
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Pack_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "包装" , IsPrimaryKey = true)]
        public long Pack_ID
        { 
            get{return _Pack_ID;}
            set{
            SetProperty(ref _Pack_ID, value);
                base.PrimaryKeyID = _Pack_ID;
            }
        }

        private string _PackagingName;
        /// <summary>
        /// 包装名称
        /// </summary>
        [AdvQueryAttribute(ColName = "PackagingName",ColDesc = "包装名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PackagingName" ,Length=255,IsNullable = true,ColumnDescription = "包装名称" )]
        public string PackagingName
        { 
            get{return _PackagingName;}
            set{
            SetProperty(ref _PackagingName, value);
                        }
        }

        private long? _ProdDetailID;
        /// <summary>
        /// 产品详情
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品详情")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "产品详情" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long? ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
                        }
        }

        private long? _BundleID;
        /// <summary>
        /// 套装组合
        /// </summary>
        [AdvQueryAttribute(ColName = "BundleID",ColDesc = "套装组合")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BundleID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "套装组合" )]
        [FKRelationAttribute("tb_ProdBundle","BundleID")]
        public long? BundleID
        { 
            get{return _BundleID;}
            set{
            SetProperty(ref _BundleID, value);
                        }
        }

        private long? _ProdBaseID;
        /// <summary>
        /// 产品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdBaseID",ColDesc = "产品")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdBaseID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "产品" )]
        [FKRelationAttribute("tb_Prod","ProdBaseID")]
        public long? ProdBaseID
        { 
            get{return _ProdBaseID;}
            set{
            SetProperty(ref _ProdBaseID, value);
                        }
        }

        private long _Unit_ID;
        /// <summary>
        /// 包装单位
        /// </summary>
        [AdvQueryAttribute(ColName = "Unit_ID",ColDesc = "包装单位")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Unit_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "包装单位" )]
        [FKRelationAttribute("tb_Unit","Unit_ID")]
        public long Unit_ID
        { 
            get{return _Unit_ID;}
            set{
            SetProperty(ref _Unit_ID, value);
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

        private byte[] _PackImage;
        /// <summary>
        /// 包装图
        /// </summary>
        [AdvQueryAttribute(ColName = "PackImage",ColDesc = "包装图")] 
        [SugarColumn(ColumnDataType = "image", SqlParameterDbType ="Binary",  ColumnName = "PackImage" ,Length=2147483647,IsNullable = true,ColumnDescription = "包装图" )]
        public byte[] PackImage
        { 
            get{return _PackImage;}
            set{
            SetProperty(ref _PackImage, value);
                        }
        }

        private string _BoxMaterial;
        /// <summary>
        /// 盒子材质
        /// </summary>
        [AdvQueryAttribute(ColName = "BoxMaterial",ColDesc = "盒子材质")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BoxMaterial" ,Length=200,IsNullable = true,ColumnDescription = "盒子材质" )]
        public string BoxMaterial
        { 
            get{return _BoxMaterial;}
            set{
            SetProperty(ref _BoxMaterial, value);
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
        /// 净重N.Wt.(g)
        /// </summary>
        [AdvQueryAttribute(ColName = "NetWeight",ColDesc = "净重N.Wt.(g)")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "NetWeight" , DecimalDigits = 3,IsNullable = false,ColumnDescription = "净重N.Wt.(g)" )]
        public decimal NetWeight
        { 
            get{return _NetWeight;}
            set{
            SetProperty(ref _NetWeight, value);
                        }
        }

        private decimal _GrossWeight;
        /// <summary>
        /// 毛重G.Wt.(g)
        /// </summary>
        [AdvQueryAttribute(ColName = "GrossWeight",ColDesc = "毛重G.Wt.(g)")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "GrossWeight" , DecimalDigits = 3,IsNullable = false,ColumnDescription = "毛重G.Wt.(g)" )]
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
        [Navigate(NavigateType.OneToOne, nameof(Unit_ID))]
        public virtual tb_Unit tb_unit { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProdBaseID))]
        public virtual tb_Prod tb_prod { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_BoxRules.Pack_ID))]
        public virtual List<tb_BoxRules> tb_BoxRuleses { get; set; }
        //tb_BoxRules.Pack_ID)
        //Pack_ID.FK_TB_BOXRU_REFERENCE_TB_PACKI)
        //tb_Packing.Pack_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PackingDetail.Pack_ID))]
        public virtual List<tb_PackingDetail> tb_PackingDetails { get; set; }
        //tb_PackingDetail.Pack_ID)
        //Pack_ID.FK_TB_PACKI_REFERENCE_TB_PACKI)
        //tb_Packing.Pack_ID)


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}



        public override object Clone()
        {
            tb_Packing loctype = (tb_Packing)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

