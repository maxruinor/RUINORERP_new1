
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/22/2025 18:02:33
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
    /// 维修物料明细表
    /// </summary>
    [Serializable()]
    [Description("维修物料明细表")]
    [SugarTable("tb_AS_RepairOrderMaterialDetail")]
    public partial class tb_AS_RepairOrderMaterialDetail: BaseEntity, ICloneable
    {
        public tb_AS_RepairOrderMaterialDetail()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("维修物料明细表tb_AS_RepairOrderMaterialDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _RepairMaterialDetailID;
        /// <summary>
        /// 维修物料明细
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RepairMaterialDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "维修物料明细" , IsPrimaryKey = true)]
        public long RepairMaterialDetailID
        { 
            get{return _RepairMaterialDetailID;}
            set{
            SetProperty(ref _RepairMaterialDetailID, value);
                base.PrimaryKeyID = _RepairMaterialDetailID;
            }
        }

        private long? _RepairOrderID;
        /// <summary>
        /// 维修工单
        /// </summary>
        [AdvQueryAttribute(ColName = "RepairOrderID",ColDesc = "维修工单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RepairOrderID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "维修工单" )]
        [FKRelationAttribute("tb_AS_RepairOrder","RepairOrderID")]
        public long? RepairOrderID
        { 
            get{return _RepairOrderID;}
            set{
            SetProperty(ref _RepairOrderID, value);
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

        private long _ProdDetailID;
        /// <summary>
        /// 维修物料
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "维修物料")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "维修物料" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
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

        private decimal _ShouldSendQty= ((0));
        /// <summary>
        /// 需求数量
        /// </summary>
        [AdvQueryAttribute(ColName = "ShouldSendQty",ColDesc = "需求数量")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "ShouldSendQty" , DecimalDigits = 3,IsNullable = false,ColumnDescription = "需求数量" )]
        public decimal ShouldSendQty
        { 
            get{return _ShouldSendQty;}
            set{
            SetProperty(ref _ShouldSendQty, value);
                        }
        }

        private decimal _ActualSentQty= ((0));
        /// <summary>
        /// 实发数量
        /// </summary>
        [AdvQueryAttribute(ColName = "ActualSentQty",ColDesc = "实发数量")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "ActualSentQty" , DecimalDigits = 3,IsNullable = false,ColumnDescription = "实发数量" )]
        public decimal ActualSentQty
        { 
            get{return _ActualSentQty;}
            set{
            SetProperty(ref _ActualSentQty, value);
                        }
        }

        private decimal _SubtotalTransAmount= ((0));
        /// <summary>
        /// 小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalTransAmount",ColDesc = "小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalTransAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "小计" )]
        public decimal SubtotalTransAmount
        { 
            get{return _SubtotalTransAmount;}
            set{
            SetProperty(ref _SubtotalTransAmount, value);
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

        private decimal _SubtotalTaxAmount= ((0));
        /// <summary>
        /// 税额
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalTaxAmount",ColDesc = "税额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalTaxAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "税额" )]
        public decimal SubtotalTaxAmount
        { 
            get{return _SubtotalTaxAmount;}
            set{
            SetProperty(ref _SubtotalTaxAmount, value);
                        }
        }

        private decimal _SubtotalUntaxedAmount= ((0));
        /// <summary>
        /// 未税本位币
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalUntaxedAmount",ColDesc = "未税本位币")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalUntaxedAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "未税本位币" )]
        public decimal SubtotalUntaxedAmount
        { 
            get{return _SubtotalUntaxedAmount;}
            set{
            SetProperty(ref _SubtotalUntaxedAmount, value);
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

        private decimal _SubtotalCost= ((0));
        /// <summary>
        /// 成本小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalCost",ColDesc = "成本小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "成本小计" )]
        public decimal SubtotalCost
        { 
            get{return _SubtotalCost;}
            set{
            SetProperty(ref _SubtotalCost, value);
                        }
        }

        private bool? _IsCritical;
        /// <summary>
        /// 是否关键物料
        /// </summary>
        [AdvQueryAttribute(ColName = "IsCritical",ColDesc = "是否关键物料")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsCritical" ,IsNullable = true,ColumnDescription = "是否关键物料" )]
        public bool? IsCritical
        { 
            get{return _IsCritical;}
            set{
            SetProperty(ref _IsCritical, value);
                        }
        }

        private bool? _Gift= false;
        /// <summary>
        /// 赠品
        /// </summary>
        [AdvQueryAttribute(ColName = "Gift",ColDesc = "赠品")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Gift" ,IsNullable = true,ColumnDescription = "赠品" )]
        public bool? Gift
        { 
            get{return _Gift;}
            set{
            SetProperty(ref _Gift, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(RepairOrderID))]
        public virtual tb_AS_RepairOrder tb_as_repairorder { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_AS_RepairOrderMaterialDetail loctype = (tb_AS_RepairOrderMaterialDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

