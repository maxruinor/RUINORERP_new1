
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:36
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
    /// 盘点表
    /// </summary>
    [Serializable()]
    [Description("tb_Stocktake")]
    [SugarTable("tb_Stocktake")]
    public partial class tb_Stocktake: BaseEntity, ICloneable
    {
        public tb_Stocktake()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_Stocktake" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _MainID;
        /// <summary>
        /// 盘点单
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "MainID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "盘点单" , IsPrimaryKey = true)]
        public long MainID
        { 
            get{return _MainID;}
            set{
            base.PrimaryKeyID = _MainID;
            SetProperty(ref _MainID, value);
            }
        }

        private long _Employee_ID;
        /// <summary>
        /// 盘点负责人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "盘点负责人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "盘点负责人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
            }
        }

        private long _Location_ID;
        /// <summary>
        /// 盘点仓库
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "盘点仓库")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Location_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "盘点仓库" )]
        [FKRelationAttribute("tb_Location","Location_ID")]
        public long Location_ID
        { 
            get{return _Location_ID;}
            set{
            SetProperty(ref _Location_ID, value);
            }
        }

        private string _CheckNo;
        /// <summary>
        /// 盘点单号
        /// </summary>
        [AdvQueryAttribute(ColName = "CheckNo",ColDesc = "盘点单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CheckNo" ,Length=50,IsNullable = false,ColumnDescription = "盘点单号" )]
        public string CheckNo
        { 
            get{return _CheckNo;}
            set{
            SetProperty(ref _CheckNo, value);
            }
        }

        private int _CheckMode;
        /// <summary>
        /// 盘点方式
        /// </summary>
        [AdvQueryAttribute(ColName = "CheckMode",ColDesc = "盘点方式")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "CheckMode" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "盘点方式" )]
        public int CheckMode
        { 
            get{return _CheckMode;}
            set{
            SetProperty(ref _CheckMode, value);
            }
        }

        private int _Adjust_Type;
        /// <summary>
        /// 调整类型
        /// </summary>
        [AdvQueryAttribute(ColName = "Adjust_Type",ColDesc = "调整类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Adjust_Type" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "调整类型" )]
        public int Adjust_Type
        { 
            get{return _Adjust_Type;}
            set{
            SetProperty(ref _Adjust_Type, value);
            }
        }

        private int? _CheckResult;
        /// <summary>
        /// 盘点结果
        /// </summary>
        [AdvQueryAttribute(ColName = "CheckResult",ColDesc = "盘点结果")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "CheckResult" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "盘点结果" )]
        public int? CheckResult
        { 
            get{return _CheckResult;}
            set{
            SetProperty(ref _CheckResult, value);
            }
        }

        private int _CarryingTotalQty= ((0));
        /// <summary>
        /// 载账总数量
        /// </summary>
        [AdvQueryAttribute(ColName = "CarryingTotalQty",ColDesc = "载账总数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "CarryingTotalQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "载账总数量" )]
        public int CarryingTotalQty
        { 
            get{return _CarryingTotalQty;}
            set{
            SetProperty(ref _CarryingTotalQty, value);
            }
        }

        private decimal _CarryingTotalAmount= ((0));
        /// <summary>
        /// 载账总成本
        /// </summary>
        [AdvQueryAttribute(ColName = "CarryingTotalAmount",ColDesc = "载账总成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "CarryingTotalAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "载账总成本" )]
        public decimal CarryingTotalAmount
        { 
            get{return _CarryingTotalAmount;}
            set{
            SetProperty(ref _CarryingTotalAmount, value);
            }
        }

        private DateTime _Check_date;
        /// <summary>
        /// 盘点日期
        /// </summary>
        [AdvQueryAttribute(ColName = "Check_date",ColDesc = "盘点日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Check_date" ,IsNullable = false,ColumnDescription = "盘点日期" )]
        public DateTime Check_date
        { 
            get{return _Check_date;}
            set{
            SetProperty(ref _Check_date, value);
            }
        }

        private DateTime? _CarryingDate;
        /// <summary>
        /// 载账日期
        /// </summary>
        [AdvQueryAttribute(ColName = "CarryingDate",ColDesc = "载账日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "CarryingDate" ,IsNullable = true,ColumnDescription = "载账日期" )]
        public DateTime? CarryingDate
        { 
            get{return _CarryingDate;}
            set{
            SetProperty(ref _CarryingDate, value);
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
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=1000,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
            }
        }

        private int _DiffTotalQty= ((0));
        /// <summary>
        /// 差异总数量
        /// </summary>
        [AdvQueryAttribute(ColName = "DiffTotalQty",ColDesc = "差异总数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DiffTotalQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "差异总数量" )]
        public int DiffTotalQty
        { 
            get{return _DiffTotalQty;}
            set{
            SetProperty(ref _DiffTotalQty, value);
            }
        }

        private decimal _DiffTotalAmount= ((0));
        /// <summary>
        /// 差异总金额
        /// </summary>
        [AdvQueryAttribute(ColName = "DiffTotalAmount",ColDesc = "差异总金额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "DiffTotalAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "差异总金额" )]
        public decimal DiffTotalAmount
        { 
            get{return _DiffTotalAmount;}
            set{
            SetProperty(ref _DiffTotalAmount, value);
            }
        }

        private int _CheckTotalQty= ((0));
        /// <summary>
        /// 盘点总数量
        /// </summary>
        [AdvQueryAttribute(ColName = "CheckTotalQty",ColDesc = "盘点总数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "CheckTotalQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "盘点总数量" )]
        public int CheckTotalQty
        { 
            get{return _CheckTotalQty;}
            set{
            SetProperty(ref _CheckTotalQty, value);
            }
        }

        private decimal _CheckTotalAmount= ((0));
        /// <summary>
        /// 盘点总成本
        /// </summary>
        [AdvQueryAttribute(ColName = "CheckTotalAmount",ColDesc = "盘点总成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "CheckTotalAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "盘点总成本" )]
        public decimal CheckTotalAmount
        { 
            get{return _CheckTotalAmount;}
            set{
            SetProperty(ref _CheckTotalAmount, value);
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

        private int _DataStatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "DataStatus",ColDesc = "数据状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DataStatus" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "数据状态" )]
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
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ApprovalOpinions" ,Length=500,IsNullable = true,ColumnDescription = "审批意见" )]
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
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }


        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_StocktakeDetail.MainID))]
        public virtual List<tb_StocktakeDetail> tb_StocktakeDetails { get; set; }
        //tb_StocktakeDetail.MainID)
        //MainID.FK_TB_STOCK_REF_TB_STOCK_1)
        //tb_Stocktake.MainID)


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
                    Type type = typeof(tb_Stocktake);
                    
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
            tb_Stocktake loctype = (tb_Stocktake)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

