
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:42:07
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
    /// 自制成品建议
    /// </summary>
    [Serializable()]
    [Description("自制成品建议")]
    [SugarTable("tb_ProduceGoodsRecommendDetail")]
    public partial class tb_ProduceGoodsRecommendDetail: BaseEntity, ICloneable
    {
        public tb_ProduceGoodsRecommendDetail()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("自制成品建议tb_ProduceGoodsRecommendDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PDCID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PDCID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long PDCID
        { 
            get{return _PDCID;}
            set{
            SetProperty(ref _PDCID, value);
                base.PrimaryKeyID = _PDCID;
            }
        }

        private long? _PDID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "PDID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PDID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_ProductionDemand","PDID")]
        public long? PDID
        { 
            get{return _PDID;}
            set{
            SetProperty(ref _PDID, value);
                        }
        }

        private long _ProdDetailID;
        /// <summary>
        /// 货品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "货品")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "货品" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
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

        private long? _ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        public long? ID
        { 
            get{return _ID;}
            set{
            SetProperty(ref _ID, value);
                        }
        }

        private long? _ParentId;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ParentId",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ParentId" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        public long? ParentId
        { 
            get{return _ParentId;}
            set{
            SetProperty(ref _ParentId, value);
                        }
        }

        private string _Specifications;
        /// <summary>
        /// 规格
        /// </summary>
        [AdvQueryAttribute(ColName = "Specifications",ColDesc = "规格")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Specifications" ,Length=1000,IsNullable = true,ColumnDescription = "规格" )]
        public string Specifications
        { 
            get{return _Specifications;}
            set{
            SetProperty(ref _Specifications, value);
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

        private long? _BOM_ID;
        /// <summary>
        /// 标准配方
        /// </summary>
        [AdvQueryAttribute(ColName = "BOM_ID",ColDesc = "标准配方")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BOM_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "标准配方" )]
        [FKRelationAttribute("tb_BOM_S","BOM_ID")]
        public long? BOM_ID
        { 
            get{return _BOM_ID;}
            set{
            SetProperty(ref _BOM_ID, value);
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

        private decimal _UnitCost= ((0));
        /// <summary>
        /// 单位成本
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitCost",ColDesc = "单位成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UnitCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "单位成本" )]
        public decimal UnitCost
        { 
            get{return _UnitCost;}
            set{
            SetProperty(ref _UnitCost, value);
                        }
        }

        private int _RequirementQty= ((0));
        /// <summary>
        /// 请制量
        /// </summary>
        [AdvQueryAttribute(ColName = "RequirementQty",ColDesc = "请制量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "RequirementQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "请制量" )]
        public int RequirementQty
        { 
            get{return _RequirementQty;}
            set{
            SetProperty(ref _RequirementQty, value);
                        }
        }

        private int _RecommendQty= ((0));
        /// <summary>
        /// 建议量
        /// </summary>
        [AdvQueryAttribute(ColName = "RecommendQty",ColDesc = "建议量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "RecommendQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "建议量" )]
        public int RecommendQty
        { 
            get{return _RecommendQty;}
            set{
            SetProperty(ref _RecommendQty, value);
                        }
        }

        private int _PlanNeedQty= ((0));
        /// <summary>
        /// 计划需求数
        /// </summary>
        [AdvQueryAttribute(ColName = "PlanNeedQty",ColDesc = "计划需求数")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "PlanNeedQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "计划需求数" )]
        public int PlanNeedQty
        { 
            get{return _PlanNeedQty;}
            set{
            SetProperty(ref _PlanNeedQty, value);
                        }
        }

        private DateTime? _PreStartDate;
        /// <summary>
        /// 预开工日
        /// </summary>
        [AdvQueryAttribute(ColName = "PreStartDate",ColDesc = "预开工日")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "PreStartDate" ,IsNullable = true,ColumnDescription = "预开工日" )]
        public DateTime? PreStartDate
        { 
            get{return _PreStartDate;}
            set{
            SetProperty(ref _PreStartDate, value);
                        }
        }

        private DateTime? _PreEndDate;
        /// <summary>
        /// 预完工日
        /// </summary>
        [AdvQueryAttribute(ColName = "PreEndDate",ColDesc = "预完工日")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "PreEndDate" ,IsNullable = true,ColumnDescription = "预完工日" )]
        public DateTime? PreEndDate
        { 
            get{return _PreEndDate;}
            set{
            SetProperty(ref _PreEndDate, value);
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

        private string _RefBillNO;
        /// <summary>
        /// 生成单号
        /// </summary>
        [AdvQueryAttribute(ColName = "RefBillNO",ColDesc = "生成单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "RefBillNO" ,Length=100,IsNullable = true,ColumnDescription = "生成单号" )]
        public string RefBillNO
        { 
            get{return _RefBillNO;}
            set{
            SetProperty(ref _RefBillNO, value);
                        }
        }

        private int? _RefBillType;
        /// <summary>
        /// 单据类型
        /// </summary>
        [AdvQueryAttribute(ColName = "RefBillType",ColDesc = "单据类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "RefBillType" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "单据类型" )]
        public int? RefBillType
        { 
            get{return _RefBillType;}
            set{
            SetProperty(ref _RefBillType, value);
                        }
        }

        private long? _RefBillID;
        /// <summary>
        /// 生成单据
        /// </summary>
        [AdvQueryAttribute(ColName = "RefBillID",ColDesc = "生成单据")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RefBillID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "生成单据" )]
        public long? RefBillID
        { 
            get{return _RefBillID;}
            set{
            SetProperty(ref _RefBillID, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(BOM_ID))]
        public virtual tb_BOM_S tb_bom_s { get; set; }

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
        [Navigate(NavigateType.OneToOne, nameof(PDID))]
        public virtual tb_ProductionDemand tb_productiondemand { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ManufacturingOrder.PDCID))]
        public virtual List<tb_ManufacturingOrder> tb_ManufacturingOrders { get; set; }
        //tb_ManufacturingOrder.PDCID)
        //PDCID.FK_MANUFACTRUINGORDRDER_REF_PRODUCEGROODSRECOMMENDDETAIL)
        //tb_ProduceGoodsRecommendDetail.PDCID)


        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_ProduceGoodsRecommendDetail loctype = (tb_ProduceGoodsRecommendDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

