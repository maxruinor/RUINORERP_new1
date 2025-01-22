
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/08/2024 01:40:30
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
    /// View_BOM
    /// </summary>
    [Serializable()]
    [SugarTable("View_BOM")]
    public class View_BOM:BaseEntity, ICloneable
    {
        public View_BOM()
        {
            FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("View_BOM" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    
        private long? _BOM_ID;
        
        
        /// <summary>
        /// 标准配方
        /// </summary>

        [AdvQueryAttribute(ColName = "BOM_ID",ColDesc = "标准配方")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BOM_ID" ,IsNullable = true,ColumnDescription = "标准配方" )]
        [Display(Name = "标准配方")]
        public long? BOM_ID 
        { 
            get{return _BOM_ID;}            set{                SetProperty(ref _BOM_ID, value);                }
        }

        private string _BOM_No;
        
        
        /// <summary>
        /// 配方编号
        /// </summary>

        [AdvQueryAttribute(ColName = "BOM_No",ColDesc = "配方编号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BOM_No" ,Length=50,IsNullable = true,ColumnDescription = "配方编号" )]
        [Display(Name = "配方编号")]
        public string BOM_No 
        { 
            get{return _BOM_No;}            set{                SetProperty(ref _BOM_No, value);                }
        }

        private string _BOM_Name;
        
        
        /// <summary>
        /// 配方名称
        /// </summary>

        [AdvQueryAttribute(ColName = "BOM_Name",ColDesc = "配方名称")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BOM_Name" ,Length=100,IsNullable = true,ColumnDescription = "配方名称" )]
        [Display(Name = "配方名称")]
        public string BOM_Name 
        { 
            get{return _BOM_Name;}            set{                SetProperty(ref _BOM_Name, value);                }
        }

        private string _SKU;
        
        
        /// <summary>
        /// SKU
        /// </summary>

        [AdvQueryAttribute(ColName = "SKU",ColDesc = "SKU")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SKU" ,Length=80,IsNullable = true,ColumnDescription = "SKU")]
        [Display(Name = "SKU")]
        public string SKU 
        { 
            get{return _SKU;}            set{                SetProperty(ref _SKU, value);                }
        }

        private long? _Type_ID;
        
        
        /// <summary>
        /// 产品类型
        /// </summary>

        [AdvQueryAttribute(ColName = "Type_ID",ColDesc = "产品类型")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Type_ID" ,IsNullable = true,ColumnDescription = "产品类型" )]
        [Display(Name = "产品类型")]
        public long? Type_ID 
        { 
            get{return _Type_ID;}            set{                SetProperty(ref _Type_ID, value);                }
        }

        private string _CNName;
        
        
        /// <summary>
        /// 品名
        /// </summary>

        [AdvQueryAttribute(ColName = "CNName",ColDesc = "品名")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CNName" ,Length=255,IsNullable = true,ColumnDescription = "品名" )]
        [Display(Name = "品名")]
        public string CNName 
        { 
            get{return _CNName;}            set{                SetProperty(ref _CNName, value);                }
        }

        private long? _ProdDetailID;
        
        
        /// <summary>
        /// 母件
        /// </summary>

        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "母件SKU")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" ,IsNullable = true,ColumnDescription = "母件SKU")]
        [Display(Name = "母件SKU")]
        [FKRelationAttribute("tb_ProdDetail", "ProdDetailID")]
        public long? ProdDetailID 
        { 
            get{return _ProdDetailID;}            set{                SetProperty(ref _ProdDetailID, value);                }
        }

        private long? _DepartmentID;
        
        
        /// <summary>
        /// 制造部门
        /// </summary>

        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "制造部门")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" ,IsNullable = true,ColumnDescription = "制造部门" )]
        [Display(Name = "制造部门")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}            set{                SetProperty(ref _DepartmentID, value);                }
        }

        private long? _Doc_ID;
        
        
        /// <summary>
        /// 工艺文件
        /// </summary>

        [AdvQueryAttribute(ColName = "Doc_ID",ColDesc = "工艺文件")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Doc_ID" ,IsNullable = true,ColumnDescription = "工艺文件" )]
        [Display(Name = "工艺文件")]
        public long? Doc_ID 
        { 
            get{return _Doc_ID;}            set{                SetProperty(ref _Doc_ID, value);                }
        }


        private long? _Employee_ID;
        /// <summary>
        /// BOM工程师
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID", ColDesc = "BOM工程师")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Employee_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "BOM工程师")]
        [FKRelationAttribute("tb_Employee", "Employee_ID")]
        public long? Employee_ID
        {
            get { return _Employee_ID; }
            set
            {
                SetProperty(ref _Employee_ID, value);
            }
        }
        private long? _BOM_S_VERID;
        
        
        /// <summary>
        /// 版本号
        /// </summary>

        [AdvQueryAttribute(ColName = "BOM_S_VERID",ColDesc = "版本号")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BOM_S_VERID" ,IsNullable = true,ColumnDescription = "版本号" )]
        [Display(Name = "版本号")]
        public long? BOM_S_VERID 
        { 
            get{return _BOM_S_VERID;}            set{                SetProperty(ref _BOM_S_VERID, value);                }
        }

        private DateTime? _Effective_at;
        
        
        /// <summary>
        /// 生效时间
        /// </summary>

        [AdvQueryAttribute(ColName = "Effective_at",ColDesc = "生效时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Effective_at" ,IsNullable = true,ColumnDescription = "生效时间" )]
        [Display(Name = "生效时间")]
        public DateTime? Effective_at 
        { 
            get{return _Effective_at;}            set{                SetProperty(ref _Effective_at, value);                }
        }

        private bool? _is_enabled;
        
        
        /// <summary>
        /// 是否启用
        /// </summary>

        [AdvQueryAttribute(ColName = "is_enabled",ColDesc = "是否启用")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "is_enabled" ,IsNullable = true,ColumnDescription = "是否启用" )]
        [Display(Name = "是否启用")]
        public bool? is_enabled 
        { 
            get{return _is_enabled;}            set{                SetProperty(ref _is_enabled, value);                }
        }

        private bool? _is_available;
        
        
        /// <summary>
        /// 是否可用
        /// </summary>

        [AdvQueryAttribute(ColName = "is_available",ColDesc = "是否可用")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "is_available" ,IsNullable = true,ColumnDescription = "是否可用" )]
        [Display(Name = "是否可用")]
        public bool? is_available 
        { 
            get{return _is_available;}            set{                SetProperty(ref _is_available, value);                }
        }

        private decimal? _OutApportionedCost;
        
        
        /// <summary>
        /// 外发分摊费用
        /// </summary>

        [AdvQueryAttribute(ColName = "OutApportionedCost",ColDesc = "外发分摊费用")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "OutApportionedCost" ,IsNullable = true,ColumnDescription = "外发分摊费用" )]
        [Display(Name = "外发分摊费用")]
        public decimal? OutApportionedCost 
        { 
            get{return _OutApportionedCost;}            set{                SetProperty(ref _OutApportionedCost, value);                }
        }

        private decimal? _SelfApportionedCost;
        
        
        /// <summary>
        /// 自制分摊费用
        /// </summary>

        [AdvQueryAttribute(ColName = "SelfApportionedCost",ColDesc = "自制分摊费用")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SelfApportionedCost" ,IsNullable = true,ColumnDescription = "自制分摊费用" )]
        [Display(Name = "自制分摊费用")]
        public decimal? SelfApportionedCost 
        { 
            get{return _SelfApportionedCost;}            set{                SetProperty(ref _SelfApportionedCost, value);                }
        }

        private decimal? _TotalSelfManuCost;
        
        
        /// <summary>
        /// 自制费用
        /// </summary>

        [AdvQueryAttribute(ColName = "TotalSelfManuCost",ColDesc = "自制费用")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalSelfManuCost" ,IsNullable = true,ColumnDescription = "自制费用" )]
        [Display(Name = "自制费用")]
        public decimal? TotalSelfManuCost 
        { 
            get{return _TotalSelfManuCost;}            set{                SetProperty(ref _TotalSelfManuCost, value);                }
        }

        private decimal? _TotalOutManuCost;
        
        
        /// <summary>
        /// 外发费用
        /// </summary>

        [AdvQueryAttribute(ColName = "TotalOutManuCost",ColDesc = "外发费用")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalOutManuCost" ,IsNullable = true,ColumnDescription = "外发费用" )]
        [Display(Name = "外发费用")]
        public decimal? TotalOutManuCost 
        { 
            get{return _TotalOutManuCost;}            set{                SetProperty(ref _TotalOutManuCost, value);                }
        }

        private decimal? _TotalMaterialCost;
        
        
        /// <summary>
        /// 总物料费用
        /// </summary>

        [AdvQueryAttribute(ColName = "TotalMaterialCost",ColDesc = "总物料费用")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalMaterialCost" ,IsNullable = true,ColumnDescription = "总物料费用" )]
        [Display(Name = "总物料费用")]
        public decimal? TotalMaterialCost 
        { 
            get{return _TotalMaterialCost;}            set{                SetProperty(ref _TotalMaterialCost, value);                }
        }

        private decimal? _TotalMaterialQty;
        
        
        /// <summary>
        /// 用料总量
        /// </summary>

        [AdvQueryAttribute(ColName = "TotalMaterialQty",ColDesc = "用料总量")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "TotalMaterialQty" , DecimalDigits = 15,Length=15,IsNullable = true,ColumnDescription = "用料总量" )]
        [Display(Name = "用料总量")]
        public decimal? TotalMaterialQty 
        { 
            get{return _TotalMaterialQty;}            set{                SetProperty(ref _TotalMaterialQty, value);                }
        }

        private decimal? _OutputQty;
        
        
        /// <summary>
        /// 产出量
        /// </summary>

        [AdvQueryAttribute(ColName = "OutputQty",ColDesc = "产出量")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "OutputQty" , DecimalDigits = 15,Length=15,IsNullable = true,ColumnDescription = "产出量" )]
        [Display(Name = "产出量")]
        public decimal? OutputQty 
        { 
            get{return _OutputQty;}            set{                SetProperty(ref _OutputQty, value);                }
        }

        private decimal? _PeopleQty;
        
        
        /// <summary>
        /// 人数
        /// </summary>

        [AdvQueryAttribute(ColName = "PeopleQty",ColDesc = "人数")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "PeopleQty" , DecimalDigits = 15,Length=15,IsNullable = true,ColumnDescription = "人数" )]
        [Display(Name = "人数")]
        public decimal? PeopleQty 
        { 
            get{return _PeopleQty;}            set{                SetProperty(ref _PeopleQty, value);                }
        }

        private decimal? _WorkingHour;
        
        
        /// <summary>
        /// 工时
        /// </summary>

        [AdvQueryAttribute(ColName = "WorkingHour",ColDesc = "工时")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "WorkingHour" , DecimalDigits = 15,Length=15,IsNullable = true,ColumnDescription = "工时" )]
        [Display(Name = "工时")]
        public decimal? WorkingHour 
        { 
            get{return _WorkingHour;}            set{                SetProperty(ref _WorkingHour, value);                }
        }

        private decimal? _MachineHour;
        
        
        /// <summary>
        /// 机时
        /// </summary>

        [AdvQueryAttribute(ColName = "MachineHour",ColDesc = "机时")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "MachineHour" , DecimalDigits = 15,Length=15,IsNullable = true,ColumnDescription = "机时" )]
        [Display(Name = "机时")]
        public decimal? MachineHour 
        { 
            get{return _MachineHour;}            set{                SetProperty(ref _MachineHour, value);                }
        }

        private DateTime? _ExpirationDate;
        
        
        /// <summary>
        /// 截止日期
        /// </summary>

        [AdvQueryAttribute(ColName = "ExpirationDate",ColDesc = "截止日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "ExpirationDate" ,IsNullable = true,ColumnDescription = "截止日期" )]
        [Display(Name = "截止日期")]
        public DateTime? ExpirationDate 
        { 
            get{return _ExpirationDate;}            set{                SetProperty(ref _ExpirationDate, value);                }
        }

        private decimal? _DailyQty;
        
        
        /// <summary>
        /// 日产量
        /// </summary>

        [AdvQueryAttribute(ColName = "DailyQty",ColDesc = "日产量")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "DailyQty" ,IsNullable = true,ColumnDescription = "日产量" )]
        [Display(Name = "日产量")]
        public decimal? DailyQty 
        { 
            get{return _DailyQty;}            set{                SetProperty(ref _DailyQty, value);                }
        }

        private decimal? _SelfProductionAllCosts;
        
        
        /// <summary>
        /// 自产总成本
        /// </summary>

        [AdvQueryAttribute(ColName = "SelfProductionAllCosts",ColDesc = "自产总成本")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SelfProductionAllCosts" ,IsNullable = true,ColumnDescription = "自产总成本" )]
        [Display(Name = "自产总成本")]
        public decimal? SelfProductionAllCosts 
        { 
            get{return _SelfProductionAllCosts;}            set{                SetProperty(ref _SelfProductionAllCosts, value);                }
        }

        private decimal? _OutProductionAllCosts;
        
        
        /// <summary>
        /// 外发总成本
        /// </summary>

        [AdvQueryAttribute(ColName = "OutProductionAllCosts",ColDesc = "外发总成本")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "OutProductionAllCosts" ,IsNullable = true,ColumnDescription = "外发总成本" )]
        [Display(Name = "外发总成本")]
        public decimal? OutProductionAllCosts 
        { 
            get{return _OutProductionAllCosts;}            set{                SetProperty(ref _OutProductionAllCosts, value);                }
        }

        private byte[] _BOM_Iimage;
        
        
        /// <summary>
        /// BOM图片
        /// </summary>

        [AdvQueryAttribute(ColName = "BOM_Iimage",ColDesc = "BOM图片")]
        [SugarColumn(ColumnDataType = "image", SqlParameterDbType ="Binary",  ColumnName = "BOM_Iimage" ,IsNullable = true,ColumnDescription = "BOM图片" )]
        [Display(Name = "BOM图片")]
        public byte[] BOM_Iimage 
        { 
            get{return _BOM_Iimage;}            set{                SetProperty(ref _BOM_Iimage, value);                }
        }

        private string _Notes;
        
        
        /// <summary>
        /// 备注说明
        /// </summary>

        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注说明")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=500,IsNullable = true,ColumnDescription = "备注说明" )]
        [Display(Name = "备注说明")]
        public string Notes 
        { 
            get{return _Notes;}            set{                SetProperty(ref _Notes, value);                }
        }

        private DateTime? _Created_at;
        
        
        /// <summary>
        /// 创建时间
        /// </summary>

        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Created_at" ,IsNullable = true,ColumnDescription = "创建时间" )]
        [Display(Name = "创建时间")]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}            set{                SetProperty(ref _Created_at, value);                }
        }

        private long? _Created_by;
        
        
        /// <summary>
        /// 创建人
        /// </summary>

        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Created_by" ,IsNullable = true,ColumnDescription = "创建人" )]
        [Display(Name = "创建人")]
        public long? Created_by 
        { 
            get{return _Created_by;}            set{                SetProperty(ref _Created_by, value);                }
        }

        private DateTime? _Modified_at;
        
        
        /// <summary>
        /// 修改时间
        /// </summary>

        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Modified_at" ,IsNullable = true,ColumnDescription = "修改时间" )]
        [Display(Name = "修改时间")]
        public DateTime? Modified_at 
        { 
            get{return _Modified_at;}            set{                SetProperty(ref _Modified_at, value);                }
        }

        private long? _Modified_by;
        
        
        /// <summary>
        /// 修改人
        /// </summary>

        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Modified_by" ,IsNullable = true,ColumnDescription = "修改人" )]
        [Display(Name = "修改人")]
        public long? Modified_by 
        { 
            get{return _Modified_by;}            set{                SetProperty(ref _Modified_by, value);                }
        }

        private bool? _isdeleted;
        
        
        /// <summary>
        /// 逻辑删除
        /// </summary>

        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "isdeleted" ,IsNullable = true,ColumnDescription = "逻辑删除" )]
        [Browsable(false)]
        [Display(Name = "逻辑删除")]
        public bool? isdeleted 
        { 
            get{return _isdeleted;}            set{                SetProperty(ref _isdeleted, value);                }
        }

        private int? _DataStatus;
        
        
        /// <summary>
        /// 数据状态
        /// </summary>

        [AdvQueryAttribute(ColName = "DataStatus",ColDesc = "数据状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DataStatus" ,IsNullable = true,ColumnDescription = "数据状态" )]
        [Display(Name = "数据状态")]
        public int? DataStatus 
        { 
            get{return _DataStatus;}            set{                SetProperty(ref _DataStatus, value);                }
        }

        private string _ApprovalOpinions;
        
        
        /// <summary>
        /// 审批意见
        /// </summary>

        [AdvQueryAttribute(ColName = "ApprovalOpinions",ColDesc = "审批意见")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ApprovalOpinions" ,Length=500,IsNullable = true,ColumnDescription = "审批意见" )]
        [Display(Name = "审批意见")]
        public string ApprovalOpinions 
        { 
            get{return _ApprovalOpinions;}            set{                SetProperty(ref _ApprovalOpinions, value);                }
        }

        private long? _Approver_by;
        
        
        /// <summary>
        /// 审批人
        /// </summary>

        [AdvQueryAttribute(ColName = "Approver_by",ColDesc = "审批人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Approver_by" ,IsNullable = true,ColumnDescription = "审批人" )]
        [Display(Name = "审批人")]
        public long? Approver_by 
        { 
            get{return _Approver_by;}            set{                SetProperty(ref _Approver_by, value);                }
        }

        private DateTime? _Approver_at;
        
        
        /// <summary>
        /// 审批时间
        /// </summary>

        [AdvQueryAttribute(ColName = "Approver_at",ColDesc = "审批时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Approver_at" ,IsNullable = true,ColumnDescription = "审批时间" )]
        [Display(Name = "审批时间")]
        public DateTime? Approver_at 
        { 
            get{return _Approver_at;}            set{                SetProperty(ref _Approver_at, value);                }
        }

        private int? _ApprovalStatus;
        
        
        /// <summary>
        /// 审批状态
        /// </summary>

        [AdvQueryAttribute(ColName = "ApprovalStatus",ColDesc = "审批状态")]
        [SugarColumn(ColumnDataType = "tinyint", SqlParameterDbType ="Byte",  ColumnName = "ApprovalStatus" ,IsNullable = true,ColumnDescription = "审批状态" )]
        [Display(Name = "审批状态")]
        public int? ApprovalStatus 
        { 
            get{return _ApprovalStatus;}            set{                SetProperty(ref _ApprovalStatus, value);                }
        }

        private bool? _ApprovalResults;
        
        
        /// <summary>
        /// 审批结果
        /// </summary>

        [AdvQueryAttribute(ColName = "ApprovalResults",ColDesc = "审批结果")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "ApprovalResults" ,IsNullable = true,ColumnDescription = "审批结果" )]
        [Display(Name = "审批结果")]
        public bool? ApprovalResults 
        { 
            get{return _ApprovalResults;}            set{                SetProperty(ref _ApprovalResults, value);                }
        }


        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(BOM_ID))]
        public virtual tb_BOM_S tb_bom_s { get; set; }




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
        [Description("列名中文描述"), Category("自定属性"), Browsable(true)]
        [SugarColumn(IsIgnore = true)]
        public override ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, string>();
                    SugarColumn entityAttr;
                    Type type = typeof(View_BOM);
                    
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
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}

