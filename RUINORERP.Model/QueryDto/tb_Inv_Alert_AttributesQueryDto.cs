
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/13/2023 17:34:15
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
    [SugarTable("tb_Inv_Alert_Attributes")]
    public partial class tb_Inv_Alert_AttributesQueryDto:BaseEntityDto
    {
        public tb_Inv_Alert_AttributesQueryDto()
        {

        }

    
     

        private long? _Inventory_ID;
        /// <summary>
        /// 库存
        /// </summary>
        [AdvQueryAttribute(ColName = "Inventory_ID",ColDesc = "库存")]
        [SugarColumn(ColumnName = "Inventory_ID",IsNullable = true,ColumnDescription = "库存" )]
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
        [SugarColumn(ColumnName = "AlertPeriod",IsNullable = true,ColumnDescription = "预警周期" )]
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
        [SugarColumn(ColumnName = "Max_quantity",IsNullable = true,ColumnDescription = "库存上限" )]
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
        [SugarColumn(ColumnName = "Min_quantity",IsNullable = true,ColumnDescription = "库存下限" )]
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
        [SugarColumn(ColumnName = "Alert_Activation",IsNullable = true,ColumnDescription = "预警激活" )]
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
        [SugarColumn(ColumnName = "Created_at",IsNullable = true,ColumnDescription = "创建时间" )]
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
        [SugarColumn(ColumnName = "Created_by",IsNullable = true,ColumnDescription = "创建人" )]
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
        [SugarColumn(ColumnName = "Modified_at",IsNullable = true,ColumnDescription = "修改时间" )]
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
        [SugarColumn(ColumnName = "Modified_by",IsNullable = true,ColumnDescription = "修改人" )]
        public long? Modified_by 
        { 
            get{return _Modified_by;}
            set{SetProperty(ref _Modified_by, value);}
        }


        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(RUINORERP.Model.tb_Inventory.Inventory_ID))]
        public virtual tb_Inventory tb_Inventory { get; set; }




/*

        #region 字段描述对应列表
        private ConcurrentDictionary<string, BaseDtoField> fieldNameList;


        /// <summary>
        /// 表列名的中文描述集合
        /// </summary>
        [Description("列名中文描述"), Category("自定属性"), Browsable(true)]
        [SugarColumn(IsIgnore = true)]
        public ConcurrentDictionary<string, BaseDtoField> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, BaseDtoField>();
                    SugarColumn entityAttr;
                    Type type = typeof(tb_Inv_Alert_AttributesQueryDto);
                    
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

*/


       
    }
}



