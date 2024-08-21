
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:47:55
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
    /// 期初存货来自期初盘点或业务上首次库存入库
    /// </summary>
    [Serializable()]
    [SugarTable("tb_OpeningInventory")]
    public partial class tb_OpeningInventoryQueryDto:BaseEntityDto
    {
        public tb_OpeningInventoryQueryDto()
        {

        }

    
     

        private long? _Inventory_ID;
        /// <summary>
        /// 库存
        /// </summary>
        [AdvQueryAttribute(ColName = "Inventory_ID",ColDesc = "库存")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Inventory_ID",IsNullable = true,ColumnDescription = "库存" )]
        [FKRelationAttribute("tb_Inventory","Inventory_ID")]
        public long? Inventory_ID 
        { 
            get{return _Inventory_ID;}
            set{SetProperty(ref _Inventory_ID, value);}
        }
     

        private int _InitQty= ((0));
        /// <summary>
        /// 期初库存
        /// </summary>
        [AdvQueryAttribute(ColName = "InitQty",ColDesc = "期初库存")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "InitQty",IsNullable = false,ColumnDescription = "期初库存" )]
        public int InitQty 
        { 
            get{return _InitQty;}
            set{SetProperty(ref _InitQty, value);}
        }
     

        private decimal _Cost_price= ((0));
        /// <summary>
        /// 成本价格
        /// </summary>
        [AdvQueryAttribute(ColName = "Cost_price",ColDesc = "成本价格")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "Cost_price",IsNullable = false,ColumnDescription = "成本价格" )]
        public decimal Cost_price 
        { 
            get{return _Cost_price;}
            set{SetProperty(ref _Cost_price, value);}
        }
     

        private decimal _Subtotal_Cost_Price= ((0));
        /// <summary>
        /// 成本小计
        /// </summary>
        [AdvQueryAttribute(ColName = "Subtotal_Cost_Price",ColDesc = "成本小计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "Subtotal_Cost_Price",IsNullable = false,ColumnDescription = "成本小计" )]
        public decimal Subtotal_Cost_Price 
        { 
            get{return _Subtotal_Cost_Price;}
            set{SetProperty(ref _Subtotal_Cost_Price, value);}
        }
     

        private DateTime? _InitInvDate;
        /// <summary>
        /// 期初日期
        /// </summary>
        [AdvQueryAttribute(ColName = "InitInvDate",ColDesc = "期初日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "InitInvDate",IsNullable = true,ColumnDescription = "期初日期" )]
        public DateTime? InitInvDate 
        { 
            get{return _InitInvDate;}
            set{SetProperty(ref _InitInvDate, value);}
        }
     

        private long? _RefBillID;
        /// <summary>
        /// 转入单
        /// </summary>
        [AdvQueryAttribute(ColName = "RefBillID",ColDesc = "转入单")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "RefBillID",IsNullable = true,ColumnDescription = "转入单" )]
        public long? RefBillID 
        { 
            get{return _RefBillID;}
            set{SetProperty(ref _RefBillID, value);}
        }
     

        private string _RefNO;
        /// <summary>
        /// 引用单据
        /// </summary>
        [AdvQueryAttribute(ColName = "RefNO",ColDesc = "引用单据")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "RefNO",Length=50,IsNullable = true,ColumnDescription = "引用单据" )]
        public string RefNO 
        { 
            get{return _RefNO;}
            set{SetProperty(ref _RefNO, value);}
        }
     

        private string _RefBizType;
        /// <summary>
        /// 单据类型
        /// </summary>
        [AdvQueryAttribute(ColName = "RefBizType",ColDesc = "单据类型")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "RefBizType",Length=50,IsNullable = true,ColumnDescription = "单据类型" )]
        public string RefBizType 
        { 
            get{return _RefBizType;}
            set{SetProperty(ref _RefBizType, value);}
        }
     

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=100,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
        }


       
    }
}



