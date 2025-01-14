
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/14/2025 18:56:47
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
    /// 合同明细
    /// </summary>
    [Serializable()]
    [Description("合同明细")]
    [SugarTable("tb_ContractDetail")]
    public partial class tb_ContractDetail: BaseEntity, ICloneable
    {
        public tb_ContractDetail()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("合同明细tb_ContractDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Sub_ID;
        /// <summary>
        /// 明细
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Sub_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "明细" , IsPrimaryKey = true)]
        public long Sub_ID
        { 
            get{return _Sub_ID;}
            set{
            base.PrimaryKeyID = _Sub_ID;
            SetProperty(ref _Sub_ID, value);
            }
        }

        private long? _ContractID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ContractID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ContractID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_Contract","ContractID")]
        public long? ContractID
        { 
            get{return _ContractID;}
            set{
            SetProperty(ref _ContractID, value);
            }
        }

        private long? _ProdDetailID;
        /// <summary>
        /// 货品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "货品")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "货品" )]
        public long? ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
            }
        }

        private int? _Qty;
        /// <summary>
        /// 数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Qty",ColDesc = "数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Qty" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "数量" )]
        public int? Qty
        { 
            get{return _Qty;}
            set{
            SetProperty(ref _Qty, value);
            }
        }

        private decimal? _Price;
        /// <summary>
        /// 售价
        /// </summary>
        [AdvQueryAttribute(ColName = "Price",ColDesc = "售价")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Price" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "售价" )]
        public decimal? Price
        { 
            get{return _Price;}
            set{
            SetProperty(ref _Price, value);
            }
        }

        private decimal? _Cost;
        /// <summary>
        /// 成本
        /// </summary>
        [AdvQueryAttribute(ColName = "Cost",ColDesc = "成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Cost" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "成本" )]
        public decimal? Cost
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
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=255,IsNullable = true,ColumnDescription = "摘要" )]
        public string Summary
        { 
            get{return _Summary;}
            set{
            SetProperty(ref _Summary, value);
            }
        }

        private int _SubtotalQty;
        /// <summary>
        /// 数量小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalQty",ColDesc = "数量小计")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "SubtotalQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "数量小计" )]
        public int SubtotalQty
        { 
            get{return _SubtotalQty;}
            set{
            SetProperty(ref _SubtotalQty, value);
            }
        }

        private decimal _SubtotalCostAmount;
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

        private decimal _SubtotalPirceAmount;
        /// <summary>
        /// 金额小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalPirceAmount",ColDesc = "金额小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalPirceAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "金额小计" )]
        public decimal SubtotalPirceAmount
        { 
            get{return _SubtotalPirceAmount;}
            set{
            SetProperty(ref _SubtotalPirceAmount, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(ContractID))]
        public virtual tb_Contract tb_contract { get; set; }



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
                    Type type = typeof(tb_ContractDetail);
                    
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
            tb_ContractDetail loctype = (tb_ContractDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

