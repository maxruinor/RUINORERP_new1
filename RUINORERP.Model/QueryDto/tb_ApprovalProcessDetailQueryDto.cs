
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
    /// 审核流程明细表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ApprovalProcessDetail")]
    public partial class tb_ApprovalProcessDetailQueryDto:BaseEntityDto
    {
        public tb_ApprovalProcessDetailQueryDto()
        {

        }

    
     

        private long? _ApprovalID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ApprovalID",IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_Approval","ApprovalID")]
        public long? ApprovalID 
        { 
            get{return _ApprovalID;}
            set{SetProperty(ref _ApprovalID, value);}
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
     

        private int? _ApprovalOrder;
        /// <summary>
        /// 审核顺序
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalOrder",ColDesc = "审核顺序")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "ApprovalOrder",IsNullable = true,ColumnDescription = "审核顺序" )]
        public int? ApprovalOrder 
        { 
            get{return _ApprovalOrder;}
            set{SetProperty(ref _ApprovalOrder, value);}
        }


       
    }
}



