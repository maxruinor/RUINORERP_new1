
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:48:47
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
    /// 质检表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_QualityInspection")]
    public partial class tb_QualityInspectionQueryDto:BaseEntityDto
    {
        public tb_QualityInspectionQueryDto()
        {

        }

    
     

        private DateTime? _InspectionDate;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "InspectionDate",ColDesc = "")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "InspectionDate",IsNullable = true,ColumnDescription = "" )]
        public DateTime? InspectionDate 
        { 
            get{return _InspectionDate;}
            set{SetProperty(ref _InspectionDate, value);}
        }
     

        private string _InspectionResult;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "InspectionResult",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "InspectionResult",Length=500,IsNullable = true,ColumnDescription = "" )]
        public string InspectionResult 
        { 
            get{return _InspectionResult;}
            set{SetProperty(ref _InspectionResult, value);}
        }
     

        private int? _ProductID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ProductID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "ProductID",IsNullable = true,ColumnDescription = "" )]
        public int? ProductID 
        { 
            get{return _ProductID;}
            set{SetProperty(ref _ProductID, value);}
        }


       
    }
}



