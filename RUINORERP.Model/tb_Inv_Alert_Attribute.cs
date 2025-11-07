
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:41:55
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
    /// 存货预警特性表
    /// </summary>
    [Serializable()]
    [Description("存货预警特性表")]
    [SugarTable("tb_Inv_Alert_Attribute")]
    public partial class tb_Inv_Alert_Attribute: BaseEntity, ICloneable
    {
        public tb_Inv_Alert_Attribute()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("存货预警特性表tb_Inv_Alert_Attribute" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Inv_Alert_ID;
        /// <summary>
        /// 库存预警
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Inv_Alert_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "库存预警" , IsPrimaryKey = true)]
        public long Inv_Alert_ID
        { 
            get{return _Inv_Alert_ID;}
            set{
            SetProperty(ref _Inv_Alert_ID, value);
                base.PrimaryKeyID = _Inv_Alert_ID;
            }
        }

        private long? _Inventory_ID;
        /// <summary>
        /// 库存
        /// </summary>
        [AdvQueryAttribute(ColName = "Inventory_ID",ColDesc = "库存")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Inventory_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "库存" )]
        [FKRelationAttribute("tb_Inventory","Inventory_ID")]
        public long? Inventory_ID
        { 
            get{return _Inventory_ID;}
            set{
            SetProperty(ref _Inventory_ID, value);
                        }
        }

        private int? _AlertPeriod;
        /// <summary>
        /// 预警周期
        /// </summary>
        [AdvQueryAttribute(ColName = "AlertPeriod",ColDesc = "预警周期")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "AlertPeriod" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "预警周期" )]
        public int? AlertPeriod
        { 
            get{return _AlertPeriod;}
            set{
            SetProperty(ref _AlertPeriod, value);
                        }
        }

        private int? _Max_quantity;
        /// <summary>
        /// 库存上限
        /// </summary>
        [AdvQueryAttribute(ColName = "Max_quantity",ColDesc = "库存上限")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Max_quantity" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "库存上限" )]
        public int? Max_quantity
        { 
            get{return _Max_quantity;}
            set{
            SetProperty(ref _Max_quantity, value);
                        }
        }

        private int? _Min_quantity;
        /// <summary>
        /// 库存下限
        /// </summary>
        [AdvQueryAttribute(ColName = "Min_quantity",ColDesc = "库存下限")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Min_quantity" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "库存下限" )]
        public int? Min_quantity
        { 
            get{return _Min_quantity;}
            set{
            SetProperty(ref _Min_quantity, value);
                        }
        }

        private bool? _Alert_Activation;
        /// <summary>
        /// 预警激活
        /// </summary>
        [AdvQueryAttribute(ColName = "Alert_Activation",ColDesc = "预警激活")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Alert_Activation" ,IsNullable = true,ColumnDescription = "预警激活" )]
        public bool? Alert_Activation
        { 
            get{return _Alert_Activation;}
            set{
            SetProperty(ref _Alert_Activation, value);
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
        [Navigate(NavigateType.OneToOne, nameof(Inventory_ID))]
        public virtual tb_Inventory tb_inventory { get; set; }



        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_Inv_Alert_Attribute loctype = (tb_Inv_Alert_Attribute)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

