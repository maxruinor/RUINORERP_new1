﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:25
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
    /// 采购退货入库单明细
    /// </summary>
    [Serializable()]
    [Description("采购退货入库单明细")]
    [SugarTable("tb_PurReturnEntryDetail")]
    public partial class tb_PurReturnEntryDetail: BaseEntity, ICloneable
    {
        public tb_PurReturnEntryDetail()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("采购退货入库单明细tb_PurReturnEntryDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PurReEntry_CID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PurReEntry_CID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long PurReEntry_CID
        { 
            get{return _PurReEntry_CID;}
            set{
            SetProperty(ref _PurReEntry_CID, value);
                base.PrimaryKeyID = _PurReEntry_CID;
            }
        }

        private long _PurReEntry_ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "PurReEntry_ID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PurReEntry_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_PurReturnEntry","PurReEntry_ID")]
        public long PurReEntry_ID
        { 
            get{return _PurReEntry_ID;}
            set{
            SetProperty(ref _PurReEntry_ID, value);
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


        private string _VendorModelCode;
        /// <summary>
        /// 厂商型号
        /// </summary>
        [AdvQueryAttribute(ColName = "VendorModelCode", ColDesc = "厂商型号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "VendorModelCode", Length = 50, IsNullable = true, ColumnDescription = "厂商型号")]
        public string VendorModelCode
        {
            get { return _VendorModelCode; }
            set
            {
                SetProperty(ref _VendorModelCode, value);
            }
        }

        private long _PurEntryRe_CID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "PurEntryRe_CID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PurEntryRe_CID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        public long PurEntryRe_CID
        { 
            get{return _PurEntryRe_CID;}
            set{
            SetProperty(ref _PurEntryRe_CID, value);
                        }
        }

        private string _CustomertModel;
        /// <summary>
        /// 客户型号
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomertModel",ColDesc = "客户型号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomertModel" ,Length=50,IsNullable = true,ColumnDescription = "客户型号" )]
        public string CustomertModel
        { 
            get{return _CustomertModel;}
            set{
            SetProperty(ref _CustomertModel, value);
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

        private long? _Rack_ID;
        /// <summary>
        /// 货架
        /// </summary>
        [AdvQueryAttribute(ColName = "Rack_ID",ColDesc = "货架")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Rack_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "货架" )]
        [FKRelationAttribute("tb_StorageRack","Rack_ID")]
        public long? Rack_ID
        { 
            get{return _Rack_ID;}
            set{
            SetProperty(ref _Rack_ID, value);
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

        private int _Quantity= ((0));
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

        private decimal _UnitPrice= ((0));
        /// <summary>
        /// 单价
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitPrice",ColDesc = "单价")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UnitPrice" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "单价" )]
        public decimal UnitPrice
        { 
            get{return _UnitPrice;}
            set{
            SetProperty(ref _UnitPrice, value);
                        }
        }

         
 

        private bool? _IsGift;
        /// <summary>
        /// 赠品
        /// </summary>
        [AdvQueryAttribute(ColName = "IsGift",ColDesc = "赠品")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsGift" ,IsNullable = true,ColumnDescription = "赠品" )]
        public bool? IsGift
        { 
            get{return _IsGift;}
            set{
            SetProperty(ref _IsGift, value);
                        }
        }

        private decimal _TaxRate= ((0));
        /// <summary>
        /// 税率
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxRate",ColDesc = "税率")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "TaxRate" , DecimalDigits = 2,IsNullable = false,ColumnDescription = "税率" )]
        public decimal TaxRate
        { 
            get{return _TaxRate;}
            set{
            SetProperty(ref _TaxRate, value);
                        }
        }

        private decimal _TaxAmount= ((0));
        /// <summary>
        /// 税额
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxAmount",ColDesc = "税额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TaxAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "税额" )]
        public decimal TaxAmount
        { 
            get{return _TaxAmount;}
            set{
            SetProperty(ref _TaxAmount, value);
                        }
        }

        private decimal _SubtotalTrPriceAmount= ((0));
        /// <summary>
        /// 小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalTrPriceAmount",ColDesc = "小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalTrPriceAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "小计" )]
        public decimal SubtotalTrPriceAmount
        { 
            get{return _SubtotalTrPriceAmount;}
            set{
            SetProperty(ref _SubtotalTrPriceAmount, value);
                        }
        }

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=1000,IsNullable = true,ColumnDescription = "摘要" )]
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
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Rack_ID))]
        public virtual tb_StorageRack tb_storagerack { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(PurReEntry_ID))]
        public virtual tb_PurReturnEntry tb_purreturnentry { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}




 

        public override object Clone()
        {
            tb_PurReturnEntryDetail loctype = (tb_PurReturnEntryDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

