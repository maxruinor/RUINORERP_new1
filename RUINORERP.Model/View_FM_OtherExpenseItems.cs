﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/27/2024 19:36:47
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
    /// 其它费用统计分析
    /// </summary>
    [Serializable()]
    [SugarTable("View_FM_OtherExpenseItems")]
    public class View_FM_OtherExpenseItems:BaseEntity, ICloneable
    {
        public View_FM_OtherExpenseItems()
        {
            FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("View_FM_OtherExpenseItems" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    
        private string _ExpenseNo;
        
        
        /// <summary>
        /// 单据编号
        /// </summary>

        [AdvQueryAttribute(ColName = "ExpenseNo",ColDesc = "单据编号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ExpenseNo" ,Length=30,IsNullable = true,ColumnDescription = "单据编号" )]
        [Display(Name = "单据编号")]
        public string ExpenseNo 
        { 
            get{return _ExpenseNo;}
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
            get{return _DocumentDate;}
        }

        private decimal? _TotalAmount;
        
        
        /// <summary>
        /// 总金额
        /// </summary>

        [AdvQueryAttribute(ColName = "TotalAmount",ColDesc = "总金额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalAmount" ,IsNullable = true,ColumnDescription = "总金额" )]
        [Display(Name = "总金额")]
        public decimal? TotalAmount 
        { 
            get{return _TotalAmount;}
        }

        private bool? _EXPOrINC;
        
        
        /// <summary>
        /// 收支标识
        /// </summary>

        [AdvQueryAttribute(ColName = "EXPOrINC",ColDesc = "为收入")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "EXPOrINC" ,IsNullable = true,ColumnDescription = "为收入")]
        [Display(Name = "为收入")]
        public bool? EXPOrINC 
        { 
            get{return _EXPOrINC;}
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
            get{return _Notes;}
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
            get{return _Created_at;}
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
            get{return _isdeleted;}
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
            get{return _DataStatus;}
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
            get{return _ApprovalStatus;}
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
            get{return _ApprovalResults;}
        }

        private decimal? _ApprovedAmount;
        
        
        /// <summary>
        /// 核准金额
        /// </summary>

        [AdvQueryAttribute(ColName = "ApprovedAmount",ColDesc = "核准金额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "ApprovedAmount" ,IsNullable = true,ColumnDescription = "核准金额" )]
        [Display(Name = "核准金额")]
        public decimal? ApprovedAmount 
        { 
            get{return _ApprovedAmount;}
        }

        private long? _Currency_ID;
        
        
        /// <summary>
        /// Currency_ID
        /// </summary>

        [AdvQueryAttribute(ColName = "Currency_ID",ColDesc = "币种")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Currency_ID" ,IsNullable = true,ColumnDescription = "币种")]
        [Display(Name = "币种")]
        public long? Currency_ID 
        { 
            get{return _Currency_ID;}
        }

        private string _ExpenseName;
        
        
        /// <summary>
        /// 事由
        /// </summary>

        [AdvQueryAttribute(ColName = "ExpenseName",ColDesc = "事由")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ExpenseName" ,Length=300,IsNullable = true,ColumnDescription = "事由" )]
        [Display(Name = "事由")]
        public string ExpenseName 
        { 
            get{return _ExpenseName;}
        }

        private long? _Employee_ID;
        
        
        /// <summary>
        /// 经办人
        /// </summary>

        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "经办人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" ,IsNullable = true,ColumnDescription = "经办人" )]
        [Display(Name = "经办人")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}
        }

        private long? _DepartmentID;


        /// <summary>
        /// 归属部门
        /// </summary>

        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "归属部门")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" ,IsNullable = true,ColumnDescription = "归属部门")]
        [Display(Name = "归属部门")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}
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
            get{return _ExpenseType_id;}
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
            get{return _Account_id;}
        }

        private long? _CustomerVendor_ID;
        
        
        /// <summary>
        /// 交易对象
        /// </summary>

        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "交易对象")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" ,IsNullable = true,ColumnDescription = "交易对象" )]
        [Display(Name = "交易对象")]
        public long? CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}
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
            get{return _Subject_id;}
        }

        private DateTime? _CheckOutDate;
        
        
        /// <summary>
        /// 结帐日期
        /// </summary>

        [AdvQueryAttribute(ColName = "CheckOutDate",ColDesc = "结帐日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "CheckOutDate" ,IsNullable = true,ColumnDescription = "结帐日期" )]
        [Display(Name = "结帐日期")]
        public DateTime? CheckOutDate 
        { 
            get{return _CheckOutDate;}
        }

        private bool? _IncludeTax;
        
        
        /// <summary>
        /// 含税
        /// </summary>

        [AdvQueryAttribute(ColName = "IncludeTax",ColDesc = "含税")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IncludeTax" ,IsNullable = true,ColumnDescription = "含税" )]
        [Display(Name = "含税")]
        public bool? IncludeTax 
        { 
            get{return _IncludeTax;}
        }

        private string _Summary;
        
        
        /// <summary>
        /// 备注
        /// </summary>

        [AdvQueryAttribute(ColName = "Summary",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=100,IsNullable = true,ColumnDescription = "备注" )]
        [Display(Name = "备注")]
        public string Summary 
        { 
            get{return _Summary;}
        }

        private decimal? _TaxAmount;
        
        
        /// <summary>
        /// 税额
        /// </summary>

        [AdvQueryAttribute(ColName = "TaxAmount",ColDesc = "税额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TaxAmount" ,IsNullable = true,ColumnDescription = "税额" )]
        [Display(Name = "税额")]
        public decimal? TaxAmount 
        { 
            get{return _TaxAmount;}
        }

        private decimal? _TaxRate;
        
        
        /// <summary>
        /// 税率
        /// </summary>

        [AdvQueryAttribute(ColName = "TaxRate",ColDesc = "税率")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "TaxRate" , DecimalDigits = 5,Length=5,IsNullable = true,ColumnDescription = "税率" )]
        [Display(Name = "税率")]
        public decimal? TaxRate 
        { 
            get{return _TaxRate;}
        }

        private decimal? _UntaxedAmount;
        
        
        /// <summary>
        /// 未税本位币
        /// </summary>

        [AdvQueryAttribute(ColName = "UntaxedAmount",ColDesc = "未税本位币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UntaxedAmount" ,IsNullable = true,ColumnDescription = "未税本位币" )]
        [Display(Name = "未税本位币")]
        public decimal? UntaxedAmount 
        { 
            get{return _UntaxedAmount;}
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
            get{return _ProjectGroup_ID;}
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
            get{return _Created_by;}
        }







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
                    Type type = typeof(View_FM_OtherExpenseItems);
                    
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
