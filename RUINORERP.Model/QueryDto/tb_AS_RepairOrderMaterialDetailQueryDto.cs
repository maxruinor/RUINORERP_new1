
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:25:43
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model.Base;

namespace RUINORERP.Model.QueryDto
{
    /// <summary>
    /// 维修物料明细表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_AS_RepairOrderMaterialDetail")]
    public partial class tb_AS_RepairOrderMaterialDetailQueryDto:BaseEntityDto
    {
        public tb_AS_RepairOrderMaterialDetailQueryDto()
        {

        }

    
     

        private long? _RepairOrderID;
        /// <summary>
        /// 维修工单
        /// </summary>
        [AdvQueryAttribute(ColName = "RepairOrderID",ColDesc = "维修工单")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "RepairOrderID",IsNullable = true,ColumnDescription = "维修工单" )]
        [FKRelationAttribute("tb_AS_RepairOrder","RepairOrderID")]
        public long? RepairOrderID 
        { 
            get{return _RepairOrderID;}
            set{SetProperty(ref _RepairOrderID, value);}
        }
     

        private long _Location_ID;
        /// <summary>
        /// 库位
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "库位")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Location_ID",IsNullable = false,ColumnDescription = "库位" )]
        [FKRelationAttribute("tb_Location","Location_ID")]
        public long Location_ID 
        { 
            get{return _Location_ID;}
            set{SetProperty(ref _Location_ID, value);}
        }
     

        private string _property;
        /// <summary>
        /// 属性
        /// </summary>
        [AdvQueryAttribute(ColName = "property",ColDesc = "属性")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "property",Length=255,IsNullable = true,ColumnDescription = "属性" )]
        public string property 
        { 
            get{return _property;}
            set{SetProperty(ref _property, value);}
        }
     

        private long _ProdDetailID;
        /// <summary>
        /// 维修物料
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "维修物料")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProdDetailID",IsNullable = false,ColumnDescription = "维修物料" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID 
        { 
            get{return _ProdDetailID;}
            set{SetProperty(ref _ProdDetailID, value);}
        }
     

        private decimal _UnitPrice= ((0));
        /// <summary>
        /// 单价
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitPrice",ColDesc = "单价")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "UnitPrice",IsNullable = false,ColumnDescription = "单价" )]
        public decimal UnitPrice 
        { 
            get{return _UnitPrice;}
            set{SetProperty(ref _UnitPrice, value);}
        }
     

        private decimal _ShouldSendQty= ((0));
        /// <summary>
        /// 需求数量
        /// </summary>
        [AdvQueryAttribute(ColName = "ShouldSendQty",ColDesc = "需求数量")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "ShouldSendQty",IsNullable = false,ColumnDescription = "需求数量" )]
        public decimal ShouldSendQty 
        { 
            get{return _ShouldSendQty;}
            set{SetProperty(ref _ShouldSendQty, value);}
        }
     

        private decimal _ActualSentQty= ((0));
        /// <summary>
        /// 实发数量
        /// </summary>
        [AdvQueryAttribute(ColName = "ActualSentQty",ColDesc = "实发数量")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "ActualSentQty",IsNullable = false,ColumnDescription = "实发数量" )]
        public decimal ActualSentQty 
        { 
            get{return _ActualSentQty;}
            set{SetProperty(ref _ActualSentQty, value);}
        }
     

        private decimal _SubtotalTransAmount= ((0));
        /// <summary>
        /// 小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalTransAmount",ColDesc = "小计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "SubtotalTransAmount",IsNullable = false,ColumnDescription = "小计" )]
        public decimal SubtotalTransAmount 
        { 
            get{return _SubtotalTransAmount;}
            set{SetProperty(ref _SubtotalTransAmount, value);}
        }
     

        private decimal _TaxRate= ((0));
        /// <summary>
        /// 税率
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxRate",ColDesc = "税率")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "TaxRate",IsNullable = false,ColumnDescription = "税率" )]
        public decimal TaxRate 
        { 
            get{return _TaxRate;}
            set{SetProperty(ref _TaxRate, value);}
        }
     

        private decimal _SubtotalTaxAmount= ((0));
        /// <summary>
        /// 税额
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalTaxAmount",ColDesc = "税额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "SubtotalTaxAmount",IsNullable = false,ColumnDescription = "税额" )]
        public decimal SubtotalTaxAmount 
        { 
            get{return _SubtotalTaxAmount;}
            set{SetProperty(ref _SubtotalTaxAmount, value);}
        }
     

        private decimal _SubtotalUntaxedAmount= ((0));
        /// <summary>
        /// 未税本位币
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalUntaxedAmount",ColDesc = "未税本位币")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "SubtotalUntaxedAmount",IsNullable = false,ColumnDescription = "未税本位币" )]
        public decimal SubtotalUntaxedAmount 
        { 
            get{return _SubtotalUntaxedAmount;}
            set{SetProperty(ref _SubtotalUntaxedAmount, value);}
        }
     

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Summary",Length=255,IsNullable = true,ColumnDescription = "摘要" )]
        public string Summary 
        { 
            get{return _Summary;}
            set{SetProperty(ref _Summary, value);}
        }
     

        private decimal _Cost= ((0));
        /// <summary>
        /// 成本
        /// </summary>
        [AdvQueryAttribute(ColName = "Cost",ColDesc = "成本")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "Cost",IsNullable = false,ColumnDescription = "成本" )]
        public decimal Cost 
        { 
            get{return _Cost;}
            set{SetProperty(ref _Cost, value);}
        }
     

        private decimal _SubtotalCost= ((0));
        /// <summary>
        /// 成本小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalCost",ColDesc = "成本小计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "SubtotalCost",IsNullable = false,ColumnDescription = "成本小计" )]
        public decimal SubtotalCost 
        { 
            get{return _SubtotalCost;}
            set{SetProperty(ref _SubtotalCost, value);}
        }
     

        private bool? _IsCritical;
        /// <summary>
        /// 是否关键物料
        /// </summary>
        [AdvQueryAttribute(ColName = "IsCritical",ColDesc = "是否关键物料")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsCritical",IsNullable = true,ColumnDescription = "是否关键物料" )]
        public bool? IsCritical 
        { 
            get{return _IsCritical;}
            set{SetProperty(ref _IsCritical, value);}
        }
     

        private bool? _Gift= false;
        /// <summary>
        /// 赠品
        /// </summary>
        [AdvQueryAttribute(ColName = "Gift",ColDesc = "赠品")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Gift",IsNullable = true,ColumnDescription = "赠品" )]
        public bool? Gift 
        { 
            get{return _Gift;}
            set{SetProperty(ref _Gift, value);}
        }


       
    }
}



