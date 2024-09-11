
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:47:14
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
    /// 费用报销单明细
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FM_ExpenseClaimDetail")]
    public partial class tb_FM_ExpenseClaimDetail: BaseEntity, ICloneable
    {
        public tb_FM_ExpenseClaimDetail()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_FM_ExpenseClaimDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ClaimSubID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ClaimSubID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long ClaimSubID
        { 
            get{return _ClaimSubID;}
            set{
            base.PrimaryKeyID = _ClaimSubID;
            SetProperty(ref _ClaimSubID, value);
            }
        }

        private long? _ClaimMainID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ClaimMainID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ClaimMainID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_FM_ExpenseClaim","ClaimMainID")]
        public long? ClaimMainID
        { 
            get{return _ClaimMainID;}
            set{
            SetProperty(ref _ClaimMainID, value);
            }
        }

        private string _ClaimName;
        /// <summary>
        /// 事由
        /// </summary>
        [AdvQueryAttribute(ColName = "ClaimName",ColDesc = "事由")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ClaimName" ,Length=300,IsNullable = true,ColumnDescription = "事由" )]
        public string ClaimName
        { 
            get{return _ClaimName;}
            set{
            SetProperty(ref _ClaimName, value);
            }
        }

        private long? _Employee_ID;
        /// <summary>
        /// 报销人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "报销人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "报销人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long? Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
            }
        }

        private long? _DepartmentID;
        /// <summary>
        /// 报销部门
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "报销部门")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "报销部门" )]
        [FKRelationAttribute("tb_Department","DepartmentID")]
        public long? DepartmentID
        { 
            get{return _DepartmentID;}
            set{
            SetProperty(ref _DepartmentID, value);
            }
        }

        private long? _ExpenseType_id;
        /// <summary>
        /// 费用类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ExpenseType_id",ColDesc = "费用类型")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ExpenseType_id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "费用类型" )]
        [FKRelationAttribute("tb_FM_ExpenseType","ExpenseType_id")]
        public long? ExpenseType_id
        { 
            get{return _ExpenseType_id;}
            set{
            SetProperty(ref _ExpenseType_id, value);
            }
        }

        private long? _account_id;
        /// <summary>
        /// 支付账号
        /// </summary>
        [AdvQueryAttribute(ColName = "account_id",ColDesc = "支付账号")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "account_id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "支付账号" )]
        [FKRelationAttribute("tb_FM_Account","account_id")]
        public long? account_id
        { 
            get{return _account_id;}
            set{
            SetProperty(ref _account_id, value);
            }
        }

        private long? _subject_id;
        /// <summary>
        /// 会计科目
        /// </summary>
        [AdvQueryAttribute(ColName = "subject_id",ColDesc = "会计科目")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "subject_id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "会计科目" )]
        [FKRelationAttribute("tb_FM_Subject","subject_id")]
        public long? subject_id
        { 
            get{return _subject_id;}
            set{
            SetProperty(ref _subject_id, value);
            }
        }

        private long? _ProjectGroup_ID;
        /// <summary>
        /// 所属项目
        /// </summary>
        [AdvQueryAttribute(ColName = "ProjectGroup_ID",ColDesc = "所属项目")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProjectGroup_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "所属项目" )]
        [FKRelationAttribute("tb_ProjectGroup","ProjectGroup_ID")]
        public long? ProjectGroup_ID
        { 
            get{return _ProjectGroup_ID;}
            set{
            SetProperty(ref _ProjectGroup_ID, value);
            }
        }

        private DateTime _TranDate;
        /// <summary>
        /// 发生日期
        /// </summary>
        [AdvQueryAttribute(ColName = "TranDate",ColDesc = "发生日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "TranDate" ,IsNullable = false,ColumnDescription = "发生日期" )]
        public DateTime TranDate
        { 
            get{return _TranDate;}
            set{
            SetProperty(ref _TranDate, value);
            }
        }

        private decimal _TotalAmount= ((0));
        /// <summary>
        /// 总金额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalAmount",ColDesc = "总金额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalAmount" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "总金额" )]
        public decimal TotalAmount
        { 
            get{return _TotalAmount;}
            set{
            SetProperty(ref _TotalAmount, value);
            }
        }

        private byte[] _EvidenceImage;
        /// <summary>
        /// 产品图
        /// </summary>
        [AdvQueryAttribute(ColName = "EvidenceImage", ColDesc = "凭证图")]
        [SugarColumn(ColumnDataType = "image", SqlParameterDbType = "Binary", ColumnName = "EvidenceImage", Length = 2147483647, IsNullable = true, ColumnDescription = "凭证图")]
        public byte[] EvidenceImage
        {
            get { return _EvidenceImage; }
            set
            {
                SetProperty(ref _EvidenceImage, value);
            }
        }


        private bool _IncludeTax= false;
        /// <summary>
        /// 含税
        /// </summary>
        [AdvQueryAttribute(ColName = "IncludeTax",ColDesc = "含税")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IncludeTax" ,IsNullable = false,ColumnDescription = "含税" )]
        public bool IncludeTax
        { 
            get{return _IncludeTax;}
            set{
            SetProperty(ref _IncludeTax, value);
            }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=100,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
            }
        }

        private decimal? _TaxRate;
        /// <summary>
        /// 税率
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxRate",ColDesc = "税率")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "TaxRate" , DecimalDigits = 3,IsNullable = true,ColumnDescription = "税率" )]
        public decimal? TaxRate
        { 
            get{return _TaxRate;}
            set{
            SetProperty(ref _TaxRate, value);
            }
        }

        private decimal? _TaxAmount;
        /// <summary>
        /// 税额
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxAmount",ColDesc = "税额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TaxAmount" , DecimalDigits = 6,IsNullable = true,ColumnDescription = "税额" )]
        public decimal? TaxAmount
        { 
            get{return _TaxAmount;}
            set{
            SetProperty(ref _TaxAmount, value);
            }
        }

        private decimal _UntaxedAmount=0;
        /// <summary>
        /// 未税本位币
        /// </summary>
        [AdvQueryAttribute(ColName = "UntaxedAmount",ColDesc = "未税本位币")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UntaxedAmount" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "未税本位币" )]
        public decimal UntaxedAmount
        { 
            get{return _UntaxedAmount;}
            set{
            SetProperty(ref _UntaxedAmount, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(account_id))]
        public virtual tb_FM_Account tb_fm_account { get; set; }
        //public virtual tb_FM_Account tb_account_id { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(ExpenseType_id))]
        public virtual tb_FM_ExpenseType tb_fm_expensetype { get; set; }
        //public virtual tb_FM_ExpenseType tb_ExpenseType_id { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(ProjectGroup_ID))]
        public virtual tb_ProjectGroup tb_projectgroup { get; set; }
        //public virtual tb_ProjectGroup tb_ProjectGroup_ID { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(subject_id))]
        public virtual tb_FM_Subject tb_fm_subject { get; set; }
        //public virtual tb_FM_Subject tb_subject_id { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(DepartmentID))]
        public virtual tb_Department tb_department { get; set; }
        //public virtual tb_Department tb_DepartmentID { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }
        //public virtual tb_Employee tb_Employee_ID { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(ClaimMainID))]
        public virtual tb_FM_ExpenseClaim tb_fm_expenseclaim { get; set; }
        //public virtual tb_FM_ExpenseClaim tb_ClaimMainID { get; set; }



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
                    Type type = typeof(tb_FM_ExpenseClaimDetail);
                    
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
            tb_FM_ExpenseClaimDetail loctype = (tb_FM_ExpenseClaimDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

