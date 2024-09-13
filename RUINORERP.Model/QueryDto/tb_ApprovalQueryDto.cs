
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:22
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
    /// 审核配置表 对于所有单据审核，并且提供明细，每个明细通过则主表通过主表中对应一个业务单据的主ID https://www.likecs.com/show-747870.html 
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Approval")]
    public partial class tb_ApprovalQueryDto:BaseEntityDto
    {
        public tb_ApprovalQueryDto()
        {

        }

    
     

        private string _BillType;
        /// <summary>
        /// 单据类型
        /// </summary>
        [AdvQueryAttribute(ColName = "BillType",ColDesc = "单据类型")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "BillType",Length=50,IsNullable = true,ColumnDescription = "单据类型" )]
        public string BillType 
        { 
            get{return _BillType;}
            set{SetProperty(ref _BillType, value);}
        }
     

        private string _BillName;
        /// <summary>
        /// 单据名称
        /// </summary>
        [AdvQueryAttribute(ColName = "BillName",ColDesc = "单据名称")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "BillName",Length=100,IsNullable = true,ColumnDescription = "单据名称" )]
        public string BillName 
        { 
            get{return _BillName;}
            set{SetProperty(ref _BillName, value);}
        }
     

        private string _BillEntityClassName;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "BillEntityClassName",ColDesc = "")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "BillEntityClassName",Length=50,IsNullable = true,ColumnDescription = "" )]
        public string BillEntityClassName 
        { 
            get{return _BillEntityClassName;}
            set{SetProperty(ref _BillEntityClassName, value);}
        }
     

        private int? _ApprovalResults;
        /// <summary>
        /// 审核结果
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalResults",ColDesc = "审核结果")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "ApprovalResults",IsNullable = true,ColumnDescription = "审核结果" )]
        public int? ApprovalResults 
        { 
            get{return _ApprovalResults;}
            set{SetProperty(ref _ApprovalResults, value);}
        }
     

        private bool? _GradedAudit;
        /// <summary>
        /// 分级审核
        /// </summary>
        [AdvQueryAttribute(ColName = "GradedAudit",ColDesc = "分级审核")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "GradedAudit",IsNullable = true,ColumnDescription = "分级审核" )]
        public bool? GradedAudit 
        { 
            get{return _GradedAudit;}
            set{SetProperty(ref _GradedAudit, value);}
        }
     

        private int? _Module;
        /// <summary>
        /// 程序模块
        /// </summary>
        [AdvQueryAttribute(ColName = "Module",ColDesc = "程序模块")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Module",IsNullable = true,ColumnDescription = "程序模块" )]
        public int? Module 
        { 
            get{return _Module;}
            set{SetProperty(ref _Module, value);}
        }


       
    }
}



