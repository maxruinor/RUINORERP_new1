
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:28
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
    /// 请购单，可能来自销售订单,也可以来自其它日常需求也可能来自生产需求也可以直接录数据，是一个纯业务性的数据表
   
    /// </summary>
    [Serializable()]
    [SugarTable("tb_BuyingRequisition")]
    public partial class tb_BuyingRequisitionQueryDto:BaseEntityDto
    {
        public tb_BuyingRequisitionQueryDto()
        {

        }

    
     

        private string _PuRequisitionNo;
        /// <summary>
        /// 请购单号
        /// </summary>
        [AdvQueryAttribute(ColName = "PuRequisitionNo",ColDesc = "请购单号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PuRequisitionNo",Length=100,IsNullable = false,ColumnDescription = "请购单号" )]
        public string PuRequisitionNo 
        { 
            get{return _PuRequisitionNo;}
            set{SetProperty(ref _PuRequisitionNo, value);}
        }
     

        private long _Employee_ID;
        /// <summary>
        /// 申请人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "申请人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Employee_ID",IsNullable = false,ColumnDescription = "申请人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long Employee_ID 
        { 
            get{return _Employee_ID;}
            set{SetProperty(ref _Employee_ID, value);}
        }
     

        private long? _Location_ID;
        /// <summary>
        /// 库位
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "库位")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Location_ID",IsNullable = true,ColumnDescription = "库位" )]
        [FKRelationAttribute("tb_Location","Location_ID")]
        public long? Location_ID 
        { 
            get{return _Location_ID;}
            set{SetProperty(ref _Location_ID, value);}
        }
     

        private long? _DepartmentID;
        /// <summary>
        /// 使用部门
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "使用部门")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "DepartmentID",IsNullable = true,ColumnDescription = "使用部门" )]
        [FKRelationAttribute("tb_Department","DepartmentID")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}
            set{SetProperty(ref _DepartmentID, value);}
        }
     

        private long? _RefBillID;
        /// <summary>
        /// 引用单据
        /// </summary>
        [AdvQueryAttribute(ColName = "RefBillID",ColDesc = "引用单据")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "RefBillID",IsNullable = true,ColumnDescription = "引用单据" )]
        public long? RefBillID 
        { 
            get{return _RefBillID;}
            set{SetProperty(ref _RefBillID, value);}
        }
     

        private string _RefBillNO;
        /// <summary>
        /// 引用单据编号
        /// </summary>
        [AdvQueryAttribute(ColName = "RefBillNO",ColDesc = "引用单据编号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "RefBillNO",Length=50,IsNullable = true,ColumnDescription = "引用单据编号" )]
        public string RefBillNO 
        { 
            get{return _RefBillNO;}
            set{SetProperty(ref _RefBillNO, value);}
        }
     

        private int? _RefBizType;
        /// <summary>
        /// 单据类型
        /// </summary>
        [AdvQueryAttribute(ColName = "RefBizType",ColDesc = "单据类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "RefBizType",IsNullable = true,ColumnDescription = "单据类型" )]
        public int? RefBizType 
        { 
            get{return _RefBizType;}
            set{SetProperty(ref _RefBizType, value);}
        }
     

        private DateTime _ApplicationDate;
        /// <summary>
        /// 申请日期
        /// </summary>
        [AdvQueryAttribute(ColName = "ApplicationDate",ColDesc = "申请日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "ApplicationDate",IsNullable = false,ColumnDescription = "申请日期" )]
        public DateTime ApplicationDate 
        { 
            get{return _ApplicationDate;}
            set{SetProperty(ref _ApplicationDate, value);}
        }
     

        private DateTime? _RequirementDate;
        /// <summary>
        /// 需求日期
        /// </summary>
        [AdvQueryAttribute(ColName = "RequirementDate",ColDesc = "需求日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "RequirementDate",IsNullable = true,ColumnDescription = "需求日期" )]
        public DateTime? RequirementDate 
        { 
            get{return _RequirementDate;}
            set{SetProperty(ref _RequirementDate, value);}
        }
     

        private int _TotalQty;
        /// <summary>
        /// 总数量
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalQty",ColDesc = "总数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "TotalQty",IsNullable = false,ColumnDescription = "总数量" )]
        public int TotalQty 
        { 
            get{return _TotalQty;}
            set{SetProperty(ref _TotalQty, value);}
        }
     

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=1500,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
        }
     

        private int _DataStatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "DataStatus",ColDesc = "数据状态")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "DataStatus",IsNullable = false,ColumnDescription = "数据状态" )]
        public int DataStatus 
        { 
            get{return _DataStatus;}
            set{SetProperty(ref _DataStatus, value);}
        }
     

        private string _ApprovalOpinions;
        /// <summary>
        /// 审批意见
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalOpinions",ColDesc = "审批意见")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ApprovalOpinions",Length=200,IsNullable = true,ColumnDescription = "审批意见" )]
        public string ApprovalOpinions 
        { 
            get{return _ApprovalOpinions;}
            set{SetProperty(ref _ApprovalOpinions, value);}
        }
     

        private int? _ApprovalStatus;
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
     

        private string _CloseCaseOpinions;
        /// <summary>
        /// 结案情况
        /// </summary>
        [AdvQueryAttribute(ColName = "CloseCaseOpinions",ColDesc = "结案情况")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "CloseCaseOpinions",Length=200,IsNullable = true,ColumnDescription = "结案情况" )]
        public string CloseCaseOpinions 
        { 
            get{return _CloseCaseOpinions;}
            set{SetProperty(ref _CloseCaseOpinions, value);}
        }


       
    }
}



