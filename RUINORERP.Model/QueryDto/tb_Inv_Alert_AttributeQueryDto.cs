﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:46
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
    /// 存货预警特性表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Inv_Alert_Attribute")]
    public partial class tb_Inv_Alert_AttributeQueryDto:BaseEntityDto
    {
        public tb_Inv_Alert_AttributeQueryDto()
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
     

        private int? _AlertPeriod;
        /// <summary>
        /// 预警周期
        /// </summary>
        [AdvQueryAttribute(ColName = "AlertPeriod",ColDesc = "预警周期")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "AlertPeriod",IsNullable = true,ColumnDescription = "预警周期" )]
        public int? AlertPeriod 
        { 
            get{return _AlertPeriod;}
            set{SetProperty(ref _AlertPeriod, value);}
        }
     

        private int? _Max_quantity;
        /// <summary>
        /// 库存上限
        /// </summary>
        [AdvQueryAttribute(ColName = "Max_quantity",ColDesc = "库存上限")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Max_quantity",IsNullable = true,ColumnDescription = "库存上限" )]
        public int? Max_quantity 
        { 
            get{return _Max_quantity;}
            set{SetProperty(ref _Max_quantity, value);}
        }
     

        private int? _Min_quantity;
        /// <summary>
        /// 库存下限
        /// </summary>
        [AdvQueryAttribute(ColName = "Min_quantity",ColDesc = "库存下限")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Min_quantity",IsNullable = true,ColumnDescription = "库存下限" )]
        public int? Min_quantity 
        { 
            get{return _Min_quantity;}
            set{SetProperty(ref _Min_quantity, value);}
        }
     

        private bool? _Alert_Activation;
        /// <summary>
        /// 预警激活
        /// </summary>
        [AdvQueryAttribute(ColName = "Alert_Activation",ColDesc = "预警激活")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Alert_Activation",IsNullable = true,ColumnDescription = "预警激活" )]
        public bool? Alert_Activation 
        { 
            get{return _Alert_Activation;}
            set{SetProperty(ref _Alert_Activation, value);}
        }
     

        private DateTime? _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Created_at",IsNullable = true,ColumnDescription = "创建时间" )]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}
            set{SetProperty(ref _Created_at, value);}
        }
     

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Created_by",IsNullable = true,ColumnDescription = "创建人" )]
        public long? Created_by 
        { 
            get{return _Created_by;}
            set{SetProperty(ref _Created_by, value);}
        }
     

        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Modified_at",IsNullable = true,ColumnDescription = "修改时间" )]
        public DateTime? Modified_at 
        { 
            get{return _Modified_at;}
            set{SetProperty(ref _Modified_at, value);}
        }
     

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Modified_by",IsNullable = true,ColumnDescription = "修改人" )]
        public long? Modified_by 
        { 
            get{return _Modified_by;}
            set{SetProperty(ref _Modified_by, value);}
        }


       
    }
}



