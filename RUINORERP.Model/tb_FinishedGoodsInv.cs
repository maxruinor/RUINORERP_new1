
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:37
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
    /// 成品入库单 要进一步完善
    /// </summary>
    [Serializable()]
    [Description("tb_FinishedGoodsInv")]
    [SugarTable("tb_FinishedGoodsInv")]
    public partial class tb_FinishedGoodsInv: BaseEntity, ICloneable
    {
        public tb_FinishedGoodsInv()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_FinishedGoodsInv" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _FG_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "FG_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long FG_ID
        { 
            get{return _FG_ID;}
            set{
            base.PrimaryKeyID = _FG_ID;
            SetProperty(ref _FG_ID, value);
            }
        }

        private string _DeliveryBillNo;
        /// <summary>
        /// 缴库单号
        /// </summary>
        [AdvQueryAttribute(ColName = "DeliveryBillNo",ColDesc = "缴库单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "DeliveryBillNo" ,Length=50,IsNullable = true,ColumnDescription = "缴库单号" )]
        public string DeliveryBillNo
        { 
            get{return _DeliveryBillNo;}
            set{
            SetProperty(ref _DeliveryBillNo, value);
            }
        }

        private long _Employee_ID;
        /// <summary>
        /// 经办人员
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "经办人员")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "经办人员" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
            }
        }

        private long? _DepartmentID;
        /// <summary>
        /// 生产部门
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "生产部门")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "生产部门" )]
        [FKRelationAttribute("tb_Department","DepartmentID")]
        public long? DepartmentID
        { 
            get{return _DepartmentID;}
            set{
            SetProperty(ref _DepartmentID, value);
            }
        }

        private long? _CustomerVendor_ID;
        /// <summary>
        /// 生产商
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "生产商")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "生产商" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long? CustomerVendor_ID
        { 
            get{return _CustomerVendor_ID;}
            set{
            SetProperty(ref _CustomerVendor_ID, value);
            }
        }

        private DateTime _DeliveryDate;
        /// <summary>
        /// 缴库日期
        /// </summary>
        [AdvQueryAttribute(ColName = "DeliveryDate",ColDesc = "缴库日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "DeliveryDate" ,IsNullable = false,ColumnDescription = "缴库日期" )]
        public DateTime DeliveryDate
        { 
            get{return _DeliveryDate;}
            set{
            SetProperty(ref _DeliveryDate, value);
            }
        }

        private bool _IsOutSourced = false;
        /// <summary>
        /// 是否托工
        /// </summary>
        [AdvQueryAttribute(ColName = "IsOutSourced", ColDesc = "是否托工")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "IsOutSourced", IsNullable = false, ColumnDescription = "是否托工")]
        public bool IsOutSourced
        {
            get { return _IsOutSourced; }
            set
            {
                SetProperty(ref _IsOutSourced, value);
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
        

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=255,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
            }
        }

        private string _ShippingWay;
        /// <summary>
        /// 发货方式
        /// </summary>
        [AdvQueryAttribute(ColName = "ShippingWay",ColDesc = "发货方式")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ShippingWay" ,Length=50,IsNullable = true,ColumnDescription = "发货方式" )]
        public string ShippingWay
        { 
            get{return _ShippingWay;}
            set{
            SetProperty(ref _ShippingWay, value);
            }
        }

        private string _TrackNo;
        /// <summary>
        /// 物流单号
        /// </summary>
        [AdvQueryAttribute(ColName = "TrackNo",ColDesc = "物流单号")] 
        [SugarColumn(ColumnDataType = "char", SqlParameterDbType ="String",  ColumnName = "TrackNo" ,Length=50,IsNullable = true,ColumnDescription = "物流单号" )]
        public string TrackNo
        { 
            get{return _TrackNo;}
            set{
            SetProperty(ref _TrackNo, value);
            }
        }

        private string _MONo;
        /// <summary>
        /// 制令单号
        /// </summary>
        [AdvQueryAttribute(ColName = "MONo",ColDesc = "制令单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "MONo" ,Length=50,IsNullable = true,ColumnDescription = "制令单号" )]
        public string MONo
        { 
            get{return _MONo;}
            set{
            SetProperty(ref _MONo, value);
            }
        }

        private long? _MOID;
        /// <summary>
        /// 制令单
        /// </summary>
        [AdvQueryAttribute(ColName = "MOID",ColDesc = "制令单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "MOID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "制令单" )]
        [FKRelationAttribute("tb_ManufacturingOrder","MOID")]
        public long? MOID
        { 
            get{return _MOID;}
            set{
            SetProperty(ref _MOID, value);
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

        private int? _DataStatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "DataStatus",ColDesc = "数据状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DataStatus" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "数据状态" )]
        public int? DataStatus
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

        private int? _ApprovalStatus= ((0));
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

        private bool? _GeneEvidence;
        /// <summary>
        /// 产生凭证
        /// </summary>
        [AdvQueryAttribute(ColName = "GeneEvidence",ColDesc = "产生凭证")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "GeneEvidence" ,IsNullable = true,ColumnDescription = "产生凭证" )]
        public bool? GeneEvidence
        { 
            get{return _GeneEvidence;}
            set{
            SetProperty(ref _GeneEvidence, value);
            }
        }


        private int _TotalQty = ((0));
        /// <summary>
        /// 总数量
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalQty", ColDesc = "总数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "TotalQty", DecimalDigits = 0, IsNullable = false, ColumnDescription = "总数量")]
        public int TotalQty
        {
            get { return _TotalQty; }
            set
            {
                SetProperty(ref _TotalQty, value);
            }
        }

        private decimal _TotalNetMachineHours = ((0));
        /// <summary>
        /// 总机时
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalNetMachineHours", ColDesc = "总机时")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType = "Decimal", ColumnName = "TotalNetMachineHours", DecimalDigits = 5, IsNullable = false, ColumnDescription = "总机时")]
        public decimal TotalNetMachineHours
        {
            get { return _TotalNetMachineHours; }
            set
            {
                SetProperty(ref _TotalNetMachineHours, value);
            }
        }


        private decimal _TotalNetWorkingHours = ((0));
        /// <summary>
        /// 总工时
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalNetWorkingHours",ColDesc = "总工时")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType = "Decimal",  ColumnName = "TotalNetWorkingHours" , DecimalDigits = 5,IsNullable = false,ColumnDescription = "总工时" )]
        public decimal TotalNetWorkingHours
        { 
            get{return _TotalNetWorkingHours;}
            set{
            SetProperty(ref _TotalNetWorkingHours, value);
            }
        }

        private decimal _TotalApportionedCost= ((0));
        /// <summary>
        /// 总分摊成本
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalApportionedCost",ColDesc = "总分摊成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalApportionedCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "总分摊成本" )]
        public decimal TotalApportionedCost
        { 
            get{return _TotalApportionedCost;}
            set{
            SetProperty(ref _TotalApportionedCost, value);
            }
        }

        private decimal _TotalManuFee = ((0));
        /// <summary>
        /// 总制造费用
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalManuFee", ColDesc = "总制造费用")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalManuFee", DecimalDigits = 4,IsNullable = false,ColumnDescription = "总制造费用")]
        public decimal TotalManuFee
        { 
            get{return _TotalManuFee; }
            set{
            SetProperty(ref _TotalManuFee, value);
            }
        }
       

        private decimal _TotalMaterialCost= ((0));
        /// <summary>
        /// 总材料成本
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalMaterialCost",ColDesc = "总材料成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalMaterialCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "总材料成本" )]
        public decimal TotalMaterialCost
        { 
            get{return _TotalMaterialCost;}
            set{
            SetProperty(ref _TotalMaterialCost, value);
            }
        }

        private decimal _TotalProductionCost = ((0));
        /// <summary>
        /// 生产总成本
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalProductionCost", ColDesc = "生产总成本")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalProductionCost", DecimalDigits = 4, IsNullable = false, ColumnDescription = "生产总成本")]
        public decimal TotalProductionCost
        {
            get { return _TotalProductionCost; }
            set
            {
                SetProperty(ref _TotalProductionCost, value);
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

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(MOID))]
        public virtual tb_ManufacturingOrder tb_manufacturingorder { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(CustomerVendor_ID))]
        public virtual tb_CustomerVendor tb_customervendor { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(DepartmentID))]
        public virtual tb_Department tb_department { get; set; }


        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FinishedGoodsInvDetail.FG_ID))]
        public virtual List<tb_FinishedGoodsInvDetail> tb_FinishedGoodsInvDetails { get; set; }
        //tb_FinishedGoodsInvDetail.FG_ID)
        //FG_ID.FK_TB_FINIS_REFERENCE_TB_FINIS)
        //tb_FinishedGoodsInv.FG_ID)


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
                    Type type = typeof(tb_FinishedGoodsInv);
                    
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
            tb_FinishedGoodsInv loctype = (tb_FinishedGoodsInv)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

