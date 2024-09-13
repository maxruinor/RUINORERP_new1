
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:23
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
    /// 银行账号信息表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_BankAccount")]
    public partial class tb_BankAccountQueryDto:BaseEntityDto
    {
        public tb_BankAccountQueryDto()
        {

        }

    
     

        private string _Account_Name;
        /// <summary>
        /// 账户名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Account_Name",ColDesc = "账户名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Account_Name",Length=100,IsNullable = false,ColumnDescription = "账户名称" )]
        public string Account_Name 
        { 
            get{return _Account_Name;}
            set{SetProperty(ref _Account_Name, value);}
        }
     

        private string _Account_No;
        /// <summary>
        /// 账号
        /// </summary>
        [AdvQueryAttribute(ColName = "Account_No",ColDesc = "账号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Account_No",Length=100,IsNullable = false,ColumnDescription = "账号" )]
        public string Account_No 
        { 
            get{return _Account_No;}
            set{SetProperty(ref _Account_No, value);}
        }
     

        private string _OpeningBank;
        /// <summary>
        /// 开户行
        /// </summary>
        [AdvQueryAttribute(ColName = "OpeningBank",ColDesc = "开户行")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "OpeningBank",Length=100,IsNullable = false,ColumnDescription = "开户行" )]
        public string OpeningBank 
        { 
            get{return _OpeningBank;}
            set{SetProperty(ref _OpeningBank, value);}
        }
     

        private bool _Is_enabled= true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_enabled",ColDesc = "是否启用")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Is_enabled",IsNullable = false,ColumnDescription = "是否启用" )]
        public bool Is_enabled 
        { 
            get{return _Is_enabled;}
            set{SetProperty(ref _Is_enabled, value);}
        }
     

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=200,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
        }


       
    }
}



