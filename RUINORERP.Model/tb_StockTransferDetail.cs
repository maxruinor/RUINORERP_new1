﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:30
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
    /// 调拨单明细
    /// </summary>
    [Serializable()]
    [Description("调拨单明细")]
    [SugarTable("tb_StockTransferDetail")]
    public partial class tb_StockTransferDetail: BaseEntity, ICloneable
    {
        public tb_StockTransferDetail()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("调拨单明细tb_StockTransferDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _StockTransferDetaill_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "StockTransferDetaill_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long StockTransferDetaill_ID
        { 
            get{return _StockTransferDetaill_ID;}
            set{
            SetProperty(ref _StockTransferDetaill_ID, value);
                base.PrimaryKeyID = _StockTransferDetaill_ID;
            }
        }

        private long _StockTransferID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "StockTransferID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "StockTransferID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_StockTransfer","StockTransferID")]
        public long StockTransferID
        { 
            get{return _StockTransferID;}
            set{
            SetProperty(ref _StockTransferID, value);
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

        private int _Qty= ((0));
        /// <summary>
        /// 数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Qty",ColDesc = "数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Qty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "数量" )]
        public int Qty
        { 
            get{return _Qty;}
            set{
            SetProperty(ref _Qty, value);
                        }
        }

        private decimal _TransPrice= ((0));
        /// <summary>
        /// 调拨价
        /// </summary>
        [AdvQueryAttribute(ColName = "TransPrice",ColDesc = "调拨价")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TransPrice" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "调拨价" )]
        public decimal TransPrice
        { 
            get{return _TransPrice;}
            set{
            SetProperty(ref _TransPrice, value);
                        }
        }

        private decimal _Cost= ((0));
        /// <summary>
        /// 成本
        /// </summary>
        [AdvQueryAttribute(ColName = "Cost",ColDesc = "成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Cost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "成本" )]
        public decimal Cost
        { 
            get{return _Cost;}
            set{
            SetProperty(ref _Cost, value);
                        }
        }

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=500,IsNullable = true,ColumnDescription = "摘要" )]
        public string Summary
        { 
            get{return _Summary;}
            set{
            SetProperty(ref _Summary, value);
                        }
        }

        private decimal _SubtotalCostAmount= ((0));
        /// <summary>
        /// 成本小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalCostAmount",ColDesc = "成本小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalCostAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "成本小计" )]
        public decimal SubtotalCostAmount
        { 
            get{return _SubtotalCostAmount;}
            set{
            SetProperty(ref _SubtotalCostAmount, value);
                        }
        }

        private decimal _SubtotalTransferPirceAmount= ((0));
        /// <summary>
        /// 调拨小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalTransferPirceAmount",ColDesc = "调拨小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalTransferPirceAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "调拨小计" )]
        public decimal SubtotalTransferPirceAmount
        { 
            get{return _SubtotalTransferPirceAmount;}
            set{
            SetProperty(ref _SubtotalTransferPirceAmount, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(StockTransferID))]
        public virtual tb_StockTransfer tb_stocktransfer { get; set; }

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





 

        public override object Clone()
        {
            tb_StockTransferDetail loctype = (tb_StockTransferDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

