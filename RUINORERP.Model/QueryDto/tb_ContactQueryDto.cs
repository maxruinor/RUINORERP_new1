
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:46:51
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
    /// 联系人表，CRM系统中使用
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Contact")]
    public partial class tb_ContactQueryDto:BaseEntityDto
    {
        public tb_ContactQueryDto()
        {

        }

    
     

        private long? _Customer_id;
        /// <summary>
        /// 意向客户
        /// </summary>
        [AdvQueryAttribute(ColName = "Customer_id",ColDesc = "意向客户")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Customer_id",IsNullable = true,ColumnDescription = "意向客户" )]
        [FKRelationAttribute("tb_Customer","Customer_id")]
        public long? Customer_id 
        { 
            get{return _Customer_id;}
            set{SetProperty(ref _Customer_id, value);}
        }
     

        private string _Contact_Name;
        /// <summary>
        /// 名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Contact_Name",ColDesc = "名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Contact_Name",Length=50,IsNullable = true,ColumnDescription = "名称" )]
        public string Contact_Name 
        { 
            get{return _Contact_Name;}
            set{SetProperty(ref _Contact_Name, value);}
        }
     

        private string _Contact_Email;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Contact_Email",ColDesc = "描述")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Contact_Email",Length=100,IsNullable = true,ColumnDescription = "描述" )]
        public string Contact_Email 
        { 
            get{return _Contact_Email;}
            set{SetProperty(ref _Contact_Email, value);}
        }
     

        private string _Contact_Phone;
        /// <summary>
        /// 电话
        /// </summary>
        [AdvQueryAttribute(ColName = "Contact_Phone",ColDesc = "电话")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Contact_Phone",Length=30,IsNullable = true,ColumnDescription = "电话" )]
        public string Contact_Phone 
        { 
            get{return _Contact_Phone;}
            set{SetProperty(ref _Contact_Phone, value);}
        }
     

        private string _Preferences;
        /// <summary>
        /// 爱好
        /// </summary>
        [AdvQueryAttribute(ColName = "Preferences",ColDesc = "爱好")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Preferences",Length=100,IsNullable = true,ColumnDescription = "爱好" )]
        public string Preferences 
        { 
            get{return _Preferences;}
            set{SetProperty(ref _Preferences, value);}
        }


       
    }
}



