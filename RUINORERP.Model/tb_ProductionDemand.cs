
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
    /// 生产需求分析表 是一个中间表，由计划生产单或销售订单带入数据来分析，产生采购订单再产生制令单，分析时有三步，库存不足项（包括有成品材料所有项），采购商品建议，自制品成品建议,中间表保存记录而已，操作UI上会有生成采购订单，或生产单等操作
    /// </summary>
    [Serializable()]
    [Description("生产需求分析表 是一个中间表，由计划生产单或销售订单带入数据来分析，产生采购订单再产生制令单，分析时有三步，库存不足项（包括有成品材料所有项），采购商品建议，自制品成品建议,中间表保存记录而已，操作UI上会有生成采购订单，或生产单等操作")]
    [SugarTable("tb_ProductionDemand")]
    public partial class tb_ProductionDemand: BaseEntity, ICloneable
    {
        public tb_ProductionDemand()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("生产需求分析表 是一个中间表，由计划生产单或销售订单带入数据来分析，产生采购订单再产生制令单，分析时有三步，库存不足项（包括有成品材料所有项），采购商品建议，自制品成品建议,中间表保存记录而已，操作UI上会有生成采购订单，或生产单等操作tb_ProductionDemand" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PDID;
        /// <summary>
        /// 需求分析单
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PDID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "需求分析单" , IsPrimaryKey = true)]
        public long PDID
        { 
            get{return _PDID;}
            set{
            SetProperty(ref _PDID, value);
                base.PrimaryKeyID = _PDID;
            }
        }

        private string _PDNo;
        /// <summary>
        /// 需要分析单号
        /// </summary>
        [AdvQueryAttribute(ColName = "PDNo",ColDesc = "需要分析单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PDNo" ,Length=100,IsNullable = false,ColumnDescription = "需要分析单号" )]
        public string PDNo
        { 
            get{return _PDNo;}
            set{
            SetProperty(ref _PDNo, value);
                        }
        }

        private DateTime _AnalysisDate;
        /// <summary>
        /// 分析日期
        /// </summary>
        [AdvQueryAttribute(ColName = "AnalysisDate",ColDesc = "分析日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "AnalysisDate" ,IsNullable = false,ColumnDescription = "分析日期" )]
        public DateTime AnalysisDate
        { 
            get{return _AnalysisDate;}
            set{
            SetProperty(ref _AnalysisDate, value);
                        }
        }

        private string _PPNo;
        /// <summary>
        /// 计划单号
        /// </summary>
        [AdvQueryAttribute(ColName = "PPNo",ColDesc = "计划单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PPNo" ,Length=100,IsNullable = false,ColumnDescription = "计划单号" )]
        public string PPNo
        { 
            get{return _PPNo;}
            set{
            SetProperty(ref _PPNo, value);
                        }
        }

        private long _PPID;
        /// <summary>
        /// 计划单号
        /// </summary>
        [AdvQueryAttribute(ColName = "PPID",ColDesc = "计划单号")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PPID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "计划单号" )]
        [FKRelationAttribute("tb_ProductionPlan","PPID")]
        public long PPID
        { 
            get{return _PPID;}
            set{
            SetProperty(ref _PPID, value);
                        }
        }

        private long _Employee_ID;
        /// <summary>
        /// 经办人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "经办人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "经办人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
                        }
        }

        private int _DataStatus;
        /// <summary>
        /// 单据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "DataStatus",ColDesc = "单据状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DataStatus" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "单据状态" )]
        public int DataStatus
        { 
            get{return _DataStatus;}
            set{
            SetProperty(ref _DataStatus, value);
                        }
        }

        private string _ApprovalOpinions;
        /// <summary>
        /// 审批意见
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalOpinions",ColDesc = "审批意见")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ApprovalOpinions" ,Length=200,IsNullable = true,ColumnDescription = "审批意见" )]
        public string ApprovalOpinions
        { 
            get{return _ApprovalOpinions;}
            set{
            SetProperty(ref _ApprovalOpinions, value);
                        }
        }

        private int? _ApprovalStatus;
        /// <summary>
        /// 审批状态
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalStatus",ColDesc = "审批状态")] 
        [SugarColumn(ColumnDataType = "tinyint", SqlParameterDbType ="SByte",  ColumnName = "ApprovalStatus" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "审批状态" )]
        public int? ApprovalStatus
        { 
            get{return _ApprovalStatus;}
            set{
            SetProperty(ref _ApprovalStatus, value);
                        }
        }

        private bool? _ApprovalResults;
        /// <summary>
        /// 审批结果
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalResults",ColDesc = "审批结果")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "ApprovalResults" ,IsNullable = true,ColumnDescription = "审批结果" )]
        public bool? ApprovalResults
        { 
            get{return _ApprovalResults;}
            set{
            SetProperty(ref _ApprovalResults, value);
                        }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=1500,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
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

        private long? _Approver_by;
        /// <summary>
        /// 审批人
        /// </summary>
        [AdvQueryAttribute(ColName = "Approver_by",ColDesc = "审批人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Approver_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "审批人" )]
        public long? Approver_by
        { 
            get{return _Approver_by;}
            set{
            SetProperty(ref _Approver_by, value);
                        }
        }

        private DateTime? _Approver_at;
        /// <summary>
        /// 审批时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Approver_at",ColDesc = "审批时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Approver_at" ,IsNullable = true,ColumnDescription = "审批时间" )]
        public DateTime? Approver_at
        { 
            get{return _Approver_at;}
            set{
            SetProperty(ref _Approver_at, value);
                        }
        }

        private int _PrintStatus= ((0));
        /// <summary>
        /// 打印状态
        /// </summary>
        [AdvQueryAttribute(ColName = "PrintStatus",ColDesc = "打印状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "PrintStatus" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "打印状态" )]
        public int PrintStatus
        { 
            get{return _PrintStatus;}
            set{
            SetProperty(ref _PrintStatus, value);
                        }
        }

        private bool _PurAllItems= false;
        /// <summary>
        /// 采购建议含全部物料
        /// </summary>
        [AdvQueryAttribute(ColName = "PurAllItems",ColDesc = "采购建议含全部物料")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "PurAllItems" ,IsNullable = false,ColumnDescription = "采购建议含全部物料" )]
        public bool PurAllItems
        { 
            get{return _PurAllItems;}
            set{
            SetProperty(ref _PurAllItems, value);
                        }
        }

        private bool _SuggestBasedOn= false;
        /// <summary>
        /// 建议依据
        /// </summary>
        [AdvQueryAttribute(ColName = "SuggestBasedOn",ColDesc = "建议依据")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "SuggestBasedOn" ,IsNullable = false,ColumnDescription = "建议依据" )]
        public bool SuggestBasedOn
        { 
            get{return _SuggestBasedOn;}
            set{
            SetProperty(ref _SuggestBasedOn, value);
                        }
        }

        private bool _isdeleted= false;
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "isdeleted" ,IsNullable = false,ColumnDescription = "逻辑删除" )]
        [Browsable(false)]
        public bool isdeleted
        { 
            get{return _isdeleted;}
            set{
            SetProperty(ref _isdeleted, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(PPID))]
        public virtual tb_ProductionPlan tb_productionplan { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ManufacturingOrder.PDID))]
        public virtual List<tb_ManufacturingOrder> tb_ManufacturingOrders { get; set; }
        //tb_ManufacturingOrder.PDID)
        //PDID.FK_MANUFACTURINGORDER_REF_PRODUCTIONDEMAND)
        //tb_ProductionDemand.PDID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProduceGoodsRecommendDetail.PDID))]
        public virtual List<tb_ProduceGoodsRecommendDetail> tb_ProduceGoodsRecommendDetails { get; set; }
        //tb_ProduceGoodsRecommendDetail.PDID)
        //PDID.FK_TB_PRODU_REFERENCE_TB_PRODU)
        //tb_ProductionDemand.PDID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProductionDemandDetail.PDID))]
        public virtual List<tb_ProductionDemandDetail> tb_ProductionDemandDetails { get; set; }
        //tb_ProductionDemandDetail.PDID)
        //PDID.FK_PRODUdemanddetail_REF_PRODUdemand)
        //tb_ProductionDemand.PDID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProductionDemandTargetDetail.PDID))]
        public virtual List<tb_ProductionDemandTargetDetail> tb_ProductionDemandTargetDetails { get; set; }
        //tb_ProductionDemandTargetDetail.PDID)
        //PDID.FK_TB_PRODUDEMANDTARGETDETAIL_REF_TB_PRODUDEMAND)
        //tb_ProductionDemand.PDID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurGoodsRecommendDetail.PDID))]
        public virtual List<tb_PurGoodsRecommendDetail> tb_PurGoodsRecommendDetails { get; set; }
        //tb_PurGoodsRecommendDetail.PDID)
        //PDID.FK_TB_PURGO_REFERENCE_TB_PRODU)
        //tb_ProductionDemand.PDID)


        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_ProductionDemand loctype = (tb_ProductionDemand)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

