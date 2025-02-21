
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:05
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{
    /// <summary>
    /// 退料单(包括生产和托工） 在生产过程中或结束后，我们会根据加工任务（制令单）进行生产退料。这时就需要使用生产退料这个单据进行退料。生产退料单会影响到制令单的直接材料成本，它会冲减该制令单所发生的原料成本
    /// </summary>
    [Serializable()]
    [Description("退料单(包括生产和托工） 在生产过程中或结束后，我们会根据加工任务（制令单）进行生产退料。这时就需要使用生产退料这个单据进行退料。生产退料单会影响到制令单的直接材料成本，它会冲减该制令单所发生的原料成本")]
    [SugarTable("tb_MaterialReturn")]
    public partial class tb_MaterialReturn : BaseEntity, ICloneable
    {
        public tb_MaterialReturn()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("退料单(包括生产和托工） 在生产过程中或结束后，我们会根据加工任务（制令单）进行生产退料。这时就需要使用生产退料这个单据进行退料。生产退料单会影响到制令单的直接材料成本，它会冲减该制令单所发生的原料成本tb_MaterialReturn" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _MRE_ID;
        /// <summary>
        /// 退料单
        /// </summary>

        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "MRE_ID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "退料单", IsPrimaryKey = true)]
        public long MRE_ID
        {
            get { return _MRE_ID; }
            set
            {
                SetProperty(ref _MRE_ID, value);
                base.PrimaryKeyID = _MRE_ID;
            }
        }

        private string _BillNo;
        /// <summary>
        /// 退料单号
        /// </summary>
        [AdvQueryAttribute(ColName = "BillNo", ColDesc = "退料单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "BillNo", Length = 50, IsNullable = true, ColumnDescription = "退料单号")]
        public string BillNo
        {
            get { return _BillNo; }
            set
            {
                SetProperty(ref _BillNo, value);
            }
        }

        private int? _BillType;
        /// <summary>
        /// 单据类别
        /// </summary>
        [AdvQueryAttribute(ColName = "BillType", ColDesc = "单据类别")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "BillType", DecimalDigits = 0, IsNullable = true, ColumnDescription = "单据类别")]
        public int? BillType
        {
            get { return _BillType; }
            set
            {
                SetProperty(ref _BillType, value);
            }
        }



        private long _Employee_ID;
        /// <summary>
        /// 经办人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID", ColDesc = "经办人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Employee_ID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "经办人")]
        [FKRelationAttribute("tb_Employee", "Employee_ID")]
        public long Employee_ID
        {
            get { return _Employee_ID; }
            set
            {
                SetProperty(ref _Employee_ID, value);
            }
        }

        private long? _DepartmentID;
        /// <summary>
        /// 生产部门
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentID", ColDesc = "生产部门")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "DepartmentID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "生产部门")]
        [FKRelationAttribute("tb_Department", "DepartmentID")]
        public long? DepartmentID
        {
            get { return _DepartmentID; }
            set
            {
                SetProperty(ref _DepartmentID, value);
            }
        }

        private bool _Outgoing = false;
        /// <summary>
        /// 外发加工
        /// </summary>
        [AdvQueryAttribute(ColName = "Outgoing", ColDesc = "外发加工")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "Outgoing", IsNullable = false, ColumnDescription = "外发加工")]
        public bool Outgoing
        {
            get { return _Outgoing; }
            set
            {
                SetProperty(ref _Outgoing, value);
            }
        }

        private long? _CustomerVendor_ID;
        /// <summary>
        /// 加工厂商
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID", ColDesc = "加工厂商")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "CustomerVendor_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "加工厂商")]
        [FKRelationAttribute("tb_CustomerVendor", "CustomerVendor_ID")]
        public long? CustomerVendor_ID
        {
            get { return _CustomerVendor_ID; }
            set
            {
                SetProperty(ref _CustomerVendor_ID, value);
            }
        }

        private decimal _TotalQty = ((0));
        /// <summary>
        /// 总数量
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalQty", ColDesc = "总数量")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType = "Decimal", ColumnName = "TotalQty", DecimalDigits = 0, IsNullable = false, ColumnDescription = "总数量")]
        public decimal TotalQty
        {
            get { return _TotalQty; }
            set
            {
                SetProperty(ref _TotalQty, value);
            }
        }

        private decimal _TotalCostAmount = ((0));
        /// <summary>
        /// 总成本
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalCostAmount", ColDesc = "总成本")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType = "Decimal", ColumnName = "TotalCostAmount", DecimalDigits = 0, IsNullable = false, ColumnDescription = "总成本")]
        public decimal TotalCostAmount
        {
            get { return _TotalCostAmount; }
            set
            {
                SetProperty(ref _TotalCostAmount, value);
            }
        }

        private decimal _TotalAmount = ((0));
        /// <summary>
        /// 总金额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalAmount", ColDesc = "总金额")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType = "Decimal", ColumnName = "TotalAmount", DecimalDigits = 0, IsNullable = false, ColumnDescription = "总金额")]
        public decimal TotalAmount
        {
            get { return _TotalAmount; }
            set
            {
                SetProperty(ref _TotalAmount, value);
            }
        }

        private DateTime? _ReturnDate;
        /// <summary>
        /// 退料日期
        /// </summary>
        [AdvQueryAttribute(ColName = "ReturnDate", ColDesc = "退料日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "ReturnDate", IsNullable = true, ColumnDescription = "退料日期")]
        public DateTime? ReturnDate
        {
            get { return _ReturnDate; }
            set
            {
                SetProperty(ref _ReturnDate, value);
            }
        }

        private DateTime? _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at", ColDesc = "创建时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "Created_at", IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime? Created_at
        {
            get { return _Created_at; }
            set
            {
                SetProperty(ref _Created_at, value);
            }
        }

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by", ColDesc = "创建人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Created_by", DecimalDigits = 0, IsNullable = true, ColumnDescription = "创建人")]
        public long? Created_by
        {
            get { return _Created_by; }
            set
            {
                SetProperty(ref _Created_by, value);
            }
        }

        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at", ColDesc = "修改时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "Modified_at", IsNullable = true, ColumnDescription = "修改时间")]
        public DateTime? Modified_at
        {
            get { return _Modified_at; }
            set
            {
                SetProperty(ref _Modified_at, value);
            }
        }

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by", ColDesc = "修改人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Modified_by", DecimalDigits = 0, IsNullable = true, ColumnDescription = "修改人")]
        public long? Modified_by
        {
            get { return _Modified_by; }
            set
            {
                SetProperty(ref _Modified_by, value);
            }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes", ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "Notes", Length = 255, IsNullable = true, ColumnDescription = "备注")]
        public string Notes
        {
            get { return _Notes; }
            set
            {
                SetProperty(ref _Notes, value);
            }
        }

        private long _MR_ID;
        /// <summary>
        /// 领料单
        /// </summary>
        [AdvQueryAttribute(ColName = "MR_ID", ColDesc = "领料单")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "MR_ID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "领料单")]
        [FKRelationAttribute("tb_MaterialRequisition", "MR_ID")]
        public long MR_ID
        {
            get { return _MR_ID; }
            set
            {
                SetProperty(ref _MR_ID, value);
            }
        }

        private string _MaterialRequisitionNO;
        /// <summary>
        /// 领料单号
        /// </summary>
        [AdvQueryAttribute(ColName = "MaterialRequisitionNO", ColDesc = "领料单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "MaterialRequisitionNO", Length = 50, IsNullable = false, ColumnDescription = "领料单号")]
        public string MaterialRequisitionNO
        {
            get { return _MaterialRequisitionNO; }
            set
            {
                SetProperty(ref _MaterialRequisitionNO, value);
            }
        }

        private bool _isdeleted = false;
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [AdvQueryAttribute(ColName = "isdeleted", ColDesc = "逻辑删除")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "isdeleted", IsNullable = false, ColumnDescription = "逻辑删除")]
        [Browsable(false)]
        public bool isdeleted
        {
            get { return _isdeleted; }
            set
            {
                SetProperty(ref _isdeleted, value);
            }
        }

        private int _DataStatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "DataStatus", ColDesc = "数据状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "DataStatus", DecimalDigits = 0, IsNullable = false, ColumnDescription = "数据状态")]
        public int DataStatus
        {
            get { return _DataStatus; }
            set
            {
                SetProperty(ref _DataStatus, value);
            }
        }

        private string _ApprovalOpinions;
        /// <summary>
        /// 审批意见
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalOpinions", ColDesc = "审批意见")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "ApprovalOpinions", Length = 500, IsNullable = true, ColumnDescription = "审批意见")]
        public string ApprovalOpinions
        {
            get { return _ApprovalOpinions; }
            set
            {
                SetProperty(ref _ApprovalOpinions, value);
            }
        }

        private long? _Approver_by;
        /// <summary>
        /// 审批人
        /// </summary>
        [AdvQueryAttribute(ColName = "Approver_by", ColDesc = "审批人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Approver_by", DecimalDigits = 0, IsNullable = true, ColumnDescription = "审批人")]
        public long? Approver_by
        {
            get { return _Approver_by; }
            set
            {
                SetProperty(ref _Approver_by, value);
            }
        }

        private DateTime? _Approver_at;
        /// <summary>
        /// 审批时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Approver_at", ColDesc = "审批时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "Approver_at", IsNullable = true, ColumnDescription = "审批时间")]
        public DateTime? Approver_at
        {
            get { return _Approver_at; }
            set
            {
                SetProperty(ref _Approver_at, value);
            }
        }

        private int? _ApprovalStatus = ((0));
        /// <summary>
        /// 审批状态
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalStatus", ColDesc = "审批状态")]
        [SugarColumn(ColumnDataType = "tinyint", SqlParameterDbType = "SByte", ColumnName = "ApprovalStatus", DecimalDigits = 0, IsNullable = true, ColumnDescription = "审批状态")]
        public int? ApprovalStatus
        {
            get { return _ApprovalStatus; }
            set
            {
                SetProperty(ref _ApprovalStatus, value);
            }
        }

        private bool? _GeneEvidence;
        /// <summary>
        /// 产生凭证
        /// </summary>
        [AdvQueryAttribute(ColName = "GeneEvidence", ColDesc = "产生凭证")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "GeneEvidence", IsNullable = true, ColumnDescription = "产生凭证")]
        public bool? GeneEvidence
        {
            get { return _GeneEvidence; }
            set
            {
                SetProperty(ref _GeneEvidence, value);
            }
        }

        private bool? _ApprovalResults;
        /// <summary>
        /// 审批结果
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalResults", ColDesc = "审批结果")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "ApprovalResults", IsNullable = true, ColumnDescription = "审批结果")]
        public bool? ApprovalResults
        {
            get { return _ApprovalResults; }
            set
            {
                SetProperty(ref _ApprovalResults, value);
            }
        }

        private int _PrintStatus = ((0));
        /// <summary>
        /// 打印状态
        /// </summary>
        [AdvQueryAttribute(ColName = "PrintStatus", ColDesc = "打印状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "PrintStatus", DecimalDigits = 0, IsNullable = false, ColumnDescription = "打印状态")]
        public int PrintStatus
        {
            get { return _PrintStatus; }
            set
            {
                SetProperty(ref _PrintStatus, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(MR_ID))]
        public virtual tb_MaterialRequisition tb_materialrequisition { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(CustomerVendor_ID))]
        public virtual tb_CustomerVendor tb_customervendor { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(DepartmentID))]
        public virtual tb_Department tb_department { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }
 


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MaterialReturnDetail.MRE_ID))]
        public virtual List<tb_MaterialReturnDetail> tb_MaterialReturnDetails { get; set; }
        //tb_MaterialReturnDetail.MRE_ID)
        //MRE_ID.FK_TB_MATER_REFERENCE_TB_MATER)
        //tb_MaterialReturn.MRE_ID)


        #endregion




        //如果为false,则不可以。
        private bool PK_FK_ID_Check()
        {
            bool rs = true;
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
                    Type type = typeof(tb_MaterialReturn);

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
            tb_MaterialReturn loctype = (tb_MaterialReturn)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

