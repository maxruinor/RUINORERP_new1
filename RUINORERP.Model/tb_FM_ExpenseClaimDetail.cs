
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:00
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
    [Description("费用报销单明细")]
    [SugarTable("tb_FM_ExpenseClaimDetail")]
    public partial class tb_FM_ExpenseClaimDetail: BaseEntity, ICloneable
    {
        public tb_FM_ExpenseClaimDetail()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("费用报销单明细tb_FM_ExpenseClaimDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
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
            SetProperty(ref _ClaimSubID, value);
                base.PrimaryKeyID = _ClaimSubID;
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

        private long? _Account_id;
        /// <summary>
        /// 公司账号
        /// </summary>
        [AdvQueryAttribute(ColName = "Account_id",ColDesc = "公司账号")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Account_id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "公司账号")]
        [FKRelationAttribute("tb_FM_Account","Account_id")]
        public long? Account_id
        { 
            get{return _Account_id;}
            set{
            SetProperty(ref _Account_id, value);
                        }
        }

        private long? _Subject_id;
        /// <summary>
        /// 会计科目
        /// </summary>
        [AdvQueryAttribute(ColName = "Subject_id",ColDesc = "会计科目")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Subject_id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "会计科目" )]
        [FKRelationAttribute("tb_FM_Subject","Subject_id")]
        public long? Subject_id
        { 
            get{return _Subject_id;}
            set{
            SetProperty(ref _Subject_id, value);
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

        private decimal _SingleAmount= ((0));
        /// <summary>
        /// 单项金额
        /// </summary>
        [AdvQueryAttribute(ColName = "SingleAmount",ColDesc = "单项金额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SingleAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "单项金额")]
        public decimal SingleAmount
        { 
            get{return _SingleAmount;}
            set{
            SetProperty(ref _SingleAmount, value);
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

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=500,IsNullable = true,ColumnDescription = "摘要" )]
        public string Summary
        { 
            get{return _Summary;}
            set{
            SetProperty(ref _Summary, value);
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
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TaxAmount" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "税额" )]
        public decimal? TaxAmount
        { 
            get{return _TaxAmount;}
            set{
            SetProperty(ref _TaxAmount, value);
                        }
        }

        private decimal _UntaxedAmount;
        /// <summary>
        /// 未税本位币
        /// </summary>
        [AdvQueryAttribute(ColName = "UntaxedAmount",ColDesc = "未税本位币")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UntaxedAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "未税本位币" )]
        public decimal UntaxedAmount
        { 
            get{return _UntaxedAmount;}
            set{
            SetProperty(ref _UntaxedAmount, value);
                        }
        }

        private string _EvidenceImagePath;
        /// <summary>
        /// 凭证图
        /// </summary>
        [AdvQueryAttribute(ColName = "EvidenceImagePath",ColDesc = "凭证图")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "EvidenceImagePath" ,Length=300,IsNullable = true,ColumnDescription = "凭证图" )]
        public string EvidenceImagePath
        { 
            get{return _EvidenceImagePath;}
            set{
            SetProperty(ref _EvidenceImagePath, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Account_id))]
        public virtual tb_FM_Account tb_fm_account { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ExpenseType_id))]
        public virtual tb_FM_ExpenseType tb_fm_expensetype { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProjectGroup_ID))]
        public virtual tb_ProjectGroup tb_projectgroup { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Subject_id))]
        public virtual tb_FM_Subject tb_fm_subject { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(DepartmentID))]
        public virtual tb_Department tb_department { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ClaimMainID))]
        public virtual tb_FM_ExpenseClaim tb_fm_expenseclaim { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
         if("subject_id"!="Subject_id")
        {
        // rs=false;
        }
return rs;
}




        public override object Clone()
        {
            tb_FM_ExpenseClaimDetail loctype = (tb_FM_ExpenseClaimDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

