
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:09
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

namespace RUINORERP.Model.QueryDto
{
    /// <summary>
    /// 应收应付表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FM_ReceivablePayable")]
    public partial class tb_FM_ReceivablePayableQueryDto:BaseEntityDto
    {
        public tb_FM_ReceivablePayableQueryDto()
        {

        }

    
     

        private string _ARAPNo;
        /// <summary>
        /// 单据编号
        /// </summary>
        [AdvQueryAttribute(ColName = "ARAPNo",ColDesc = "单据编号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ARAPNo",Length=30,IsNullable = true,ColumnDescription = "单据编号" )]
        public string ARAPNo 
        { 
            get{return _ARAPNo;}
            set{SetProperty(ref _ARAPNo, value);}
        }
     

        private int? _SourceBizType;
        /// <summary>
        /// 来源业务
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBizType",ColDesc = "来源业务")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "SourceBizType",IsNullable = true,ColumnDescription = "来源业务" )]
        public int? SourceBizType 
        { 
            get{return _SourceBizType;}
            set{SetProperty(ref _SourceBizType, value);}
        }
     

        private long? _SourceBillId;
        /// <summary>
        /// 来源单据
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBillId",ColDesc = "来源单据")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "SourceBillId",IsNullable = true,ColumnDescription = "来源单据" )]
        public long? SourceBillId 
        { 
            get{return _SourceBillId;}
            set{SetProperty(ref _SourceBillId, value);}
        }
     

        private string _SourceBillNo;
        /// <summary>
        /// 来源单号
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBillNo",ColDesc = "来源单号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "SourceBillNo",Length=30,IsNullable = true,ColumnDescription = "来源单号" )]
        public string SourceBillNo 
        { 
            get{return _SourceBillNo;}
            set{SetProperty(ref _SourceBillNo, value);}
        }
     

        private long _CustomerVendor_ID;
        /// <summary>
        /// 往来单位
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "往来单位")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "CustomerVendor_ID",IsNullable = false,ColumnDescription = "往来单位" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}
            set{SetProperty(ref _CustomerVendor_ID, value);}
        }
     

        private long _Currency_ID;
        /// <summary>
        /// 币别
        /// </summary>
        [AdvQueryAttribute(ColName = "Currency_ID",ColDesc = "币别")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Currency_ID",IsNullable = false,ColumnDescription = "币别" )]
        [FKRelationAttribute("tb_Currency","Currency_ID")]
        public long Currency_ID 
        { 
            get{return _Currency_ID;}
            set{SetProperty(ref _Currency_ID, value);}
        }
     

        private bool? _IsExpenseType;
        /// <summary>
        /// 费用单据
        /// </summary>
        [AdvQueryAttribute(ColName = "IsExpenseType",ColDesc = "费用单据")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsExpenseType",IsNullable = true,ColumnDescription = "费用单据" )]
        public bool? IsExpenseType 
        { 
            get{return _IsExpenseType;}
            set{SetProperty(ref _IsExpenseType, value);}
        }
     

        private bool? _IsForCommission;
        /// <summary>
        /// 用于佣金
        /// </summary>
        [AdvQueryAttribute(ColName = "IsForCommission",ColDesc = "用于佣金")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsForCommission",IsNullable = true,ColumnDescription = "用于佣金" )]
        public bool? IsForCommission 
        { 
            get{return _IsForCommission;}
            set{SetProperty(ref _IsForCommission, value);}
        }
     

        private bool? _IsFromPlatform;
        /// <summary>
        /// 平台单
        /// </summary>
        [AdvQueryAttribute(ColName = "IsFromPlatform",ColDesc = "平台单")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsFromPlatform",IsNullable = true,ColumnDescription = "平台单" )]
        public bool? IsFromPlatform 
        { 
            get{return _IsFromPlatform;}
            set{SetProperty(ref _IsFromPlatform, value);}
        }
     

        private string _PlatformOrderNo;
        /// <summary>
        /// 平台单号
        /// </summary>
        [AdvQueryAttribute(ColName = "PlatformOrderNo",ColDesc = "平台单号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PlatformOrderNo",Length=100,IsNullable = true,ColumnDescription = "平台单号" )]
        public string PlatformOrderNo 
        { 
            get{return _PlatformOrderNo;}
            set{SetProperty(ref _PlatformOrderNo, value);}
        }
     

        private long? _Account_id;
        /// <summary>
        /// 公司账户
        /// </summary>
        [AdvQueryAttribute(ColName = "Account_id",ColDesc = "公司账户")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Account_id",IsNullable = true,ColumnDescription = "公司账户" )]
        [FKRelationAttribute("tb_FM_Account","Account_id")]
        public long? Account_id 
        { 
            get{return _Account_id;}
            set{SetProperty(ref _Account_id, value);}
        }
     

        private long? _PayeeInfoID;
        /// <summary>
        /// 收款信息
        /// </summary>
        [AdvQueryAttribute(ColName = "PayeeInfoID",ColDesc = "收款信息")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "PayeeInfoID",IsNullable = true,ColumnDescription = "收款信息" )]
        [FKRelationAttribute("tb_FM_PayeeInfo","PayeeInfoID")]
        public long? PayeeInfoID 
        { 
            get{return _PayeeInfoID;}
            set{SetProperty(ref _PayeeInfoID, value);}
        }
     

        private string _PayeeAccountNo;
        /// <summary>
        /// 收款账号
        /// </summary>
        [AdvQueryAttribute(ColName = "PayeeAccountNo",ColDesc = "收款账号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PayeeAccountNo",Length=100,IsNullable = true,ColumnDescription = "收款账号" )]
        public string PayeeAccountNo 
        { 
            get{return _PayeeAccountNo;}
            set{SetProperty(ref _PayeeAccountNo, value);}
        }
     

        private decimal _ExchangeRate= ((1));
        /// <summary>
        /// 汇率
        /// </summary>
        [AdvQueryAttribute(ColName = "ExchangeRate",ColDesc = "汇率")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "ExchangeRate",IsNullable = false,ColumnDescription = "汇率" )]
        public decimal ExchangeRate 
        { 
            get{return _ExchangeRate;}
            set{SetProperty(ref _ExchangeRate, value);}
        }
     

        private int _ReceivePaymentType;
        /// <summary>
        /// 收付类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ReceivePaymentType",ColDesc = "收付类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "ReceivePaymentType",IsNullable = false,ColumnDescription = "收付类型" )]
        public int ReceivePaymentType 
        { 
            get{return _ReceivePaymentType;}
            set{SetProperty(ref _ReceivePaymentType, value);}
        }
     

        private decimal _ShippingFee= ((0));
        /// <summary>
        /// 运费
        /// </summary>
        [AdvQueryAttribute(ColName = "ShippingFee",ColDesc = "运费")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "ShippingFee",IsNullable = false,ColumnDescription = "运费" )]
        public decimal ShippingFee 
        { 
            get{return _ShippingFee;}
            set{SetProperty(ref _ShippingFee, value);}
        }
     

        private decimal _TotalForeignPayableAmount= ((0));
        /// <summary>
        /// 总金额外币
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalForeignPayableAmount",ColDesc = "总金额外币")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TotalForeignPayableAmount",IsNullable = false,ColumnDescription = "总金额外币" )]
        public decimal TotalForeignPayableAmount 
        { 
            get{return _TotalForeignPayableAmount;}
            set{SetProperty(ref _TotalForeignPayableAmount, value);}
        }
     

        private decimal _TotalLocalPayableAmount= ((0));
        /// <summary>
        /// 总金额本币
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalLocalPayableAmount",ColDesc = "总金额本币")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TotalLocalPayableAmount",IsNullable = false,ColumnDescription = "总金额本币" )]
        public decimal TotalLocalPayableAmount 
        { 
            get{return _TotalLocalPayableAmount;}
            set{SetProperty(ref _TotalLocalPayableAmount, value);}
        }
     

        private decimal _ForeignPaidAmount= ((0));
        /// <summary>
        /// 已核销外币
        /// </summary>
        [AdvQueryAttribute(ColName = "ForeignPaidAmount",ColDesc = "已核销外币")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "ForeignPaidAmount",IsNullable = false,ColumnDescription = "已核销外币" )]
        public decimal ForeignPaidAmount 
        { 
            get{return _ForeignPaidAmount;}
            set{SetProperty(ref _ForeignPaidAmount, value);}
        }
     

        private decimal _LocalPaidAmount= ((0));
        /// <summary>
        /// 已核销本币
        /// </summary>
        [AdvQueryAttribute(ColName = "LocalPaidAmount",ColDesc = "已核销本币")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "LocalPaidAmount",IsNullable = false,ColumnDescription = "已核销本币" )]
        public decimal LocalPaidAmount 
        { 
            get{return _LocalPaidAmount;}
            set{SetProperty(ref _LocalPaidAmount, value);}
        }
     

        private decimal _ForeignBalanceAmount= ((0));
        /// <summary>
        /// 未核销外币
        /// </summary>
        [AdvQueryAttribute(ColName = "ForeignBalanceAmount",ColDesc = "未核销外币")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "ForeignBalanceAmount",IsNullable = false,ColumnDescription = "未核销外币" )]
        public decimal ForeignBalanceAmount 
        { 
            get{return _ForeignBalanceAmount;}
            set{SetProperty(ref _ForeignBalanceAmount, value);}
        }
     

        private decimal _LocalBalanceAmount= ((0));
        /// <summary>
        /// 未核销本币
        /// </summary>
        [AdvQueryAttribute(ColName = "LocalBalanceAmount",ColDesc = "未核销本币")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "LocalBalanceAmount",IsNullable = false,ColumnDescription = "未核销本币" )]
        public decimal LocalBalanceAmount 
        { 
            get{return _LocalBalanceAmount;}
            set{SetProperty(ref _LocalBalanceAmount, value);}
        }
     

        private DateTime? _DueDate;
        /// <summary>
        /// 到期日
        /// </summary>
        [AdvQueryAttribute(ColName = "DueDate",ColDesc = "到期日")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "DueDate",IsNullable = true,ColumnDescription = "到期日" )]
        public DateTime? DueDate 
        { 
            get{return _DueDate;}
            set{SetProperty(ref _DueDate, value);}
        }
     

        private long? _DepartmentID;
        /// <summary>
        /// 部门
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "部门")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "DepartmentID",IsNullable = true,ColumnDescription = "部门" )]
        [FKRelationAttribute("tb_Department","DepartmentID")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}
            set{SetProperty(ref _DepartmentID, value);}
        }
     

        private long? _ProjectGroup_ID;
        /// <summary>
        /// 项目组
        /// </summary>
        [AdvQueryAttribute(ColName = "ProjectGroup_ID",ColDesc = "项目组")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProjectGroup_ID",IsNullable = true,ColumnDescription = "项目组" )]
        [FKRelationAttribute("tb_ProjectGroup","ProjectGroup_ID")]
        public long? ProjectGroup_ID 
        { 
            get{return _ProjectGroup_ID;}
            set{SetProperty(ref _ProjectGroup_ID, value);}
        }
     

        private long _Employee_ID;
        /// <summary>
        /// 经办人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "经办人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Employee_ID",IsNullable = false,ColumnDescription = "经办人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long Employee_ID 
        { 
            get{return _Employee_ID;}
            set{SetProperty(ref _Employee_ID, value);}
        }
     

        private long? _InvoiceId;
        /// <summary>
        /// 发票
        /// </summary>
        [AdvQueryAttribute(ColName = "InvoiceId",ColDesc = "发票")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "InvoiceId",IsNullable = true,ColumnDescription = "发票" )]
        [FKRelationAttribute("tb_FM_Invoice","InvoiceId")]
        public long? InvoiceId 
        { 
            get{return _InvoiceId;}
            set{SetProperty(ref _InvoiceId, value);}
        }
     

        private bool _Invoiced= false;
        /// <summary>
        /// 已开票
        /// </summary>
        [AdvQueryAttribute(ColName = "Invoiced",ColDesc = "已开票")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Invoiced",IsNullable = false,ColumnDescription = "已开票" )]
        public bool Invoiced 
        { 
            get{return _Invoiced;}
            set{SetProperty(ref _Invoiced, value);}
        }
     

        private bool _IsIncludeTax= false;
        /// <summary>
        /// 含税
        /// </summary>
        [AdvQueryAttribute(ColName = "IsIncludeTax",ColDesc = "含税")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsIncludeTax",IsNullable = false,ColumnDescription = "含税" )]
        public bool IsIncludeTax 
        { 
            get{return _IsIncludeTax;}
            set{SetProperty(ref _IsIncludeTax, value);}
        }
     

        private decimal _TaxTotalAmount= ((0));
        /// <summary>
        /// 税额总计
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxTotalAmount",ColDesc = "税额总计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TaxTotalAmount",IsNullable = false,ColumnDescription = "税额总计" )]
        public decimal TaxTotalAmount 
        { 
            get{return _TaxTotalAmount;}
            set{SetProperty(ref _TaxTotalAmount, value);}
        }
     

        private decimal _UntaxedTotalAmont= ((0));
        /// <summary>
        /// 未税总计
        /// </summary>
        [AdvQueryAttribute(ColName = "UntaxedTotalAmont",ColDesc = "未税总计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "UntaxedTotalAmont",IsNullable = false,ColumnDescription = "未税总计" )]
        public decimal UntaxedTotalAmont 
        { 
            get{return _UntaxedTotalAmont;}
            set{SetProperty(ref _UntaxedTotalAmont, value);}
        }
     

        private int? _ARAPStatus;
        /// <summary>
        /// 支付状态
        /// </summary>
        [AdvQueryAttribute(ColName = "ARAPStatus",ColDesc = "支付状态")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "ARAPStatus",IsNullable = true,ColumnDescription = "支付状态" )]
        public int? ARAPStatus 
        { 
            get{return _ARAPStatus;}
            set{SetProperty(ref _ARAPStatus, value);}
        }
     

        private string _Remark;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Remark",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Remark",Length=300,IsNullable = true,ColumnDescription = "备注" )]
        public string Remark 
        { 
            get{return _Remark;}
            set{SetProperty(ref _Remark, value);}
        }
     

        private DateTime? _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Created_at",IsNullable = true,ColumnDescription = "创建时间" )]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}
            set{SetProperty(ref _Created_at, value);}
        }
     

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Created_by",IsNullable = true,ColumnDescription = "创建人" )]
        public long? Created_by 
        { 
            get{return _Created_by;}
            set{SetProperty(ref _Created_by, value);}
        }
     

        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Modified_at",IsNullable = true,ColumnDescription = "修改时间" )]
        public DateTime? Modified_at 
        { 
            get{return _Modified_at;}
            set{SetProperty(ref _Modified_at, value);}
        }
     

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Modified_by",IsNullable = true,ColumnDescription = "修改人" )]
        public long? Modified_by 
        { 
            get{return _Modified_by;}
            set{SetProperty(ref _Modified_by, value);}
        }
     

        private bool _isdeleted= false;
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "isdeleted",IsNullable = false,ColumnDescription = "逻辑删除" )]
        public bool isdeleted 
        { 
            get{return _isdeleted;}
            set{SetProperty(ref _isdeleted, value);}
        }
     

        private string _ApprovalOpinions;
        /// <summary>
        /// 审批意见
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalOpinions",ColDesc = "审批意见")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ApprovalOpinions",Length=255,IsNullable = true,ColumnDescription = "审批意见" )]
        public string ApprovalOpinions 
        { 
            get{return _ApprovalOpinions;}
            set{SetProperty(ref _ApprovalOpinions, value);}
        }
     

        private long? _Approver_by;
        /// <summary>
        /// 审批人
        /// </summary>
        [AdvQueryAttribute(ColName = "Approver_by",ColDesc = "审批人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Approver_by",IsNullable = true,ColumnDescription = "审批人" )]
        public long? Approver_by 
        { 
            get{return _Approver_by;}
            set{SetProperty(ref _Approver_by, value);}
        }
     

        private DateTime? _Approver_at;
        /// <summary>
        /// 审批时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Approver_at",ColDesc = "审批时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Approver_at",IsNullable = true,ColumnDescription = "审批时间" )]
        public DateTime? Approver_at 
        { 
            get{return _Approver_at;}
            set{SetProperty(ref _Approver_at, value);}
        }
     

        private int? _ApprovalStatus= ((0));
        /// <summary>
        /// 审批状态
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalStatus",ColDesc = "审批状态")]
        [SugarColumn(ColumnDataType = "tinyint",SqlParameterDbType ="SByte",ColumnName = "ApprovalStatus",IsNullable = true,ColumnDescription = "审批状态" )]
        public int? ApprovalStatus 
        { 
            get{return _ApprovalStatus;}
            set{SetProperty(ref _ApprovalStatus, value);}
        }
     

        private bool? _ApprovalResults;
        /// <summary>
        /// 审批结果
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalResults",ColDesc = "审批结果")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "ApprovalResults",IsNullable = true,ColumnDescription = "审批结果" )]
        public bool? ApprovalResults 
        { 
            get{return _ApprovalResults;}
            set{SetProperty(ref _ApprovalResults, value);}
        }
     

        private int _PrintStatus= ((0));
        /// <summary>
        /// 打印状态
        /// </summary>
        [AdvQueryAttribute(ColName = "PrintStatus",ColDesc = "打印状态")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "PrintStatus",IsNullable = false,ColumnDescription = "打印状态" )]
        public int PrintStatus 
        { 
            get{return _PrintStatus;}
            set{SetProperty(ref _PrintStatus, value);}
        }


       
    }
}



