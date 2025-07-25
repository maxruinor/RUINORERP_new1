﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/19/2024 01:04:31
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model.Base;

namespace RUINORERP.Model
{
    /// <summary>
    /// 费用报销统计分析
    /// </summary>
    [Serializable()]
    [SugarTable("View_FM_ExpenseClaimItems")]
    public partial class View_FM_ExpenseClaimItems: BaseViewEntity
    {
        public View_FM_ExpenseClaimItems()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("View_FM_ExpenseClaimItems" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    
        private string _ClaimNo;
        
        
        /// <summary>
        /// 单据编号
        /// </summary>

        [AdvQueryAttribute(ColName = "ClaimNo",ColDesc = "单据编号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ClaimNo" ,Length=30,IsNullable = true,ColumnDescription = "单据编号" )]
        [Display(Name = "单据编号")]
        public string ClaimNo 
        { 
            get{return _ClaimNo;}            set{                SetProperty(ref _ClaimNo, value);                }
        }

        private DateTime? _DocumentDate;
        
        
        /// <summary>
        /// 单据日期
        /// </summary>

        [AdvQueryAttribute(ColName = "DocumentDate",ColDesc = "单据日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "DocumentDate" ,IsNullable = true,ColumnDescription = "单据日期" )]
        [Display(Name = "单据日期")]
        public DateTime? DocumentDate 
        { 
            get{return _DocumentDate;}            set{                SetProperty(ref _DocumentDate, value);                }
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

        private string _Notes;
        
        
        /// <summary>
        /// 备注
        /// </summary>

        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=100,IsNullable = true,ColumnDescription = "备注" )]
        [Display(Name = "备注")]
        public string Notes 
        { 
            get{return _Notes;}            set{                SetProperty(ref _Notes, value);                }
        }

        private string _ClaimName;
        
        
        /// <summary>
        /// 事由
        /// </summary>

        [AdvQueryAttribute(ColName = "ClaimName",ColDesc = "事由")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ClaimName" ,Length=300,IsNullable = true,ColumnDescription = "事由" )]
        [Display(Name = "事由")]
        public string ClaimName 
        { 
            get{return _ClaimName;}            set{                SetProperty(ref _ClaimName, value);                }
        }

        private long? _Employee_ID;


        /// <summary>
        /// 报销人
        /// </summary>

        [AdvQueryAttribute(ColName = "Employee_ID", ColDesc = "报销人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Employee_ID", IsNullable = true, ColumnDescription = "报销人")]
        [Display(Name = "报销人")]
        public long? Employee_ID
        {
            get { return _Employee_ID; }            set
            {                SetProperty(ref _Employee_ID, value);
            }
        }

        private long? _PayeeInfoID;
        
        
        /// <summary>
        /// 收款信息
        /// </summary>

        [AdvQueryAttribute(ColName = "PayeeInfoID",ColDesc = "收款信息")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PayeeInfoID" ,IsNullable = true,ColumnDescription = "收款信息" )]
        [Display(Name = "收款信息")]
        public long? PayeeInfoID 
        { 
            get{return _PayeeInfoID;}            set{                SetProperty(ref _PayeeInfoID, value);                }
        }

        private long? _DepartmentID;
        
        
        /// <summary>
        /// 报销部门
        /// </summary>

        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "报销部门")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" ,IsNullable = true,ColumnDescription = "报销部门" )]
        [Display(Name = "报销部门")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}            set{                SetProperty(ref _DepartmentID, value);                }
        }

        private long? _ExpenseType_id;
        
        
        /// <summary>
        /// 费用类型
        /// </summary>

        [AdvQueryAttribute(ColName = "ExpenseType_id",ColDesc = "费用类型")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ExpenseType_id" ,IsNullable = true,ColumnDescription = "费用类型" )]
        [Display(Name = "费用类型")]
        public long? ExpenseType_id 
        { 
            get{return _ExpenseType_id;}            set{                SetProperty(ref _ExpenseType_id, value);                }
        }

        private long? _Account_id;
        
        
        /// <summary>
        /// 支付账号
        /// </summary>

        [AdvQueryAttribute(ColName = "Account_id",ColDesc = "支付账号")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Account_id" ,IsNullable = true,ColumnDescription = "支付账号" )]
        [Display(Name = "支付账号")]
        public long? Account_id 
        { 
            get{return _Account_id;}            set{                SetProperty(ref _Account_id, value);                }
        }

        private long? _Subject_id;
        
        
        /// <summary>
        /// 会计科目
        /// </summary>

        [AdvQueryAttribute(ColName = "Subject_id",ColDesc = "会计科目")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Subject_id" ,IsNullable = true,ColumnDescription = "会计科目" )]
        [Display(Name = "会计科目")]
        public long? Subject_id 
        { 
            get{return _Subject_id;}            set{                SetProperty(ref _Subject_id, value);                }
        }

        private long? _ProjectGroup_ID;
        
        
        /// <summary>
        /// 所属项目
        /// </summary>

        [AdvQueryAttribute(ColName = "ProjectGroup_ID",ColDesc = "所属项目")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProjectGroup_ID" ,IsNullable = true,ColumnDescription = "所属项目" )]
        [Display(Name = "所属项目")]
        public long? ProjectGroup_ID 
        { 
            get{return _ProjectGroup_ID;}            set{                SetProperty(ref _ProjectGroup_ID, value);                }
        }

        private DateTime? _TranDate;
        
        
        /// <summary>
        /// 发生日期
        /// </summary>

        [AdvQueryAttribute(ColName = "TranDate",ColDesc = "发生日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "TranDate" ,IsNullable = true,ColumnDescription = "发生日期" )]
        [Display(Name = "发生日期")]
        public DateTime? TranDate 
        { 
            get{return _TranDate;}            set{                SetProperty(ref _TranDate, value);                }
        }

        private decimal? _SingleAmount;
        
        
        /// <summary>
        /// 单项金额
        /// </summary>

        [AdvQueryAttribute(ColName = "SingleAmount",ColDesc = "单项金额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SingleAmount" ,IsNullable = true,ColumnDescription = "单项金额" )]
        [Display(Name = "单项金额")]
        public decimal? SingleAmount 
        { 
            get{return _SingleAmount;}            set{                SetProperty(ref _SingleAmount, value);                }
        }

        private string _EvidenceImagePath;
        
        
        /// <summary>
        /// 凭证图
        /// </summary>

        [AdvQueryAttribute(ColName = "EvidenceImagePath",ColDesc = "凭证图")]
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "EvidenceImagePath" ,Length=300,IsNullable = true,ColumnDescription = "凭证图" )]
        [Display(Name = "凭证图")]
        public string EvidenceImagePath 
        { 
            get{return _EvidenceImagePath;}            set{                SetProperty(ref _EvidenceImagePath, value);                }
        }

        private string _Summary;
        
        
        /// <summary>
        /// 摘要
        /// </summary>

        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=1000,IsNullable = true,ColumnDescription = "摘要" )]
        [Display(Name = "摘要")]
        public string Summary 
        { 
            get{return _Summary;}            set{                SetProperty(ref _Summary, value);                }
        }







//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}



        public override object Clone()
        {
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}

