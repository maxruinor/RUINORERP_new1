
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:46:55
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
    /// 意向客户，公海客户 CRM系统中使用，给成交客户作外键引用
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Customer")]
    public partial class tb_CustomerQueryDto:BaseEntityDto
    {
        public tb_CustomerQueryDto()
        {

        }

    
     

        private string _CompanyName;
        /// <summary>
        /// 公司名称
        /// </summary>
        [AdvQueryAttribute(ColName = "CompanyName",ColDesc = "公司名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "CompanyName",Length=50,IsNullable = true,ColumnDescription = "公司名称" )]
        public string CompanyName 
        { 
            get{return _CompanyName;}
            set{SetProperty(ref _CompanyName, value);}
        }
     

        private long? _Employee_ID;
        /// <summary>
        /// 对接人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "对接人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Employee_ID",IsNullable = true,ColumnDescription = "对接人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}
            set{SetProperty(ref _Employee_ID, value);}
        }
     

        private string _CustomerName;
        /// <summary>
        /// 客户名称
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerName",ColDesc = "客户名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "CustomerName",Length=50,IsNullable = true,ColumnDescription = "客户名称" )]
        public string CustomerName 
        { 
            get{return _CustomerName;}
            set{SetProperty(ref _CustomerName, value);}
        }
     

        private string _CustomerAddress;
        /// <summary>
        /// 客户地址
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerAddress",ColDesc = "客户地址")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "CustomerAddress",Length=300,IsNullable = true,ColumnDescription = "客户地址" )]
        public string CustomerAddress 
        { 
            get{return _CustomerAddress;}
            set{SetProperty(ref _CustomerAddress, value);}
        }
     

        private string _CustomerDesc;
        /// <summary>
        /// 客户描述
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerDesc",ColDesc = "客户描述")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "CustomerDesc",Length=1000,IsNullable = true,ColumnDescription = "客户描述" )]
        public string CustomerDesc 
        { 
            get{return _CustomerDesc;}
            set{SetProperty(ref _CustomerDesc, value);}
        }


       
    }
}



