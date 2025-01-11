
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/11/2025 12:08:36
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
    /// 系统注册信息
    /// </summary>
    [Serializable()]
    [SugarTable("tb_sys_RegistrationInfo")]
    public partial class tb_sys_RegistrationInfoQueryDto:BaseEntityDto
    {
        public tb_sys_RegistrationInfoQueryDto()
        {

        }

    
     

        private string _CompanyName;
        /// <summary>
        /// 注册公司名
        /// </summary>
        [AdvQueryAttribute(ColName = "CompanyName",ColDesc = "注册公司名")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "CompanyName",Length=200,IsNullable = false,ColumnDescription = "注册公司名" )]
        public string CompanyName 
        { 
            get{return _CompanyName;}
            set{SetProperty(ref _CompanyName, value);}
        }
     

        private string _FunctionModule;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "FunctionModule",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "FunctionModule",Length=200,IsNullable = true,ColumnDescription = "备注" )]
        public string FunctionModule 
        { 
            get{return _FunctionModule;}
            set{SetProperty(ref _FunctionModule, value);}
        }
     

        private string _ContactName;
        /// <summary>
        /// 联系人姓名
        /// </summary>
        [AdvQueryAttribute(ColName = "ContactName",ColDesc = "联系人姓名")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ContactName",Length=200,IsNullable = false,ColumnDescription = "联系人姓名" )]
        public string ContactName 
        { 
            get{return _ContactName;}
            set{SetProperty(ref _ContactName, value);}
        }
     

        private string _PhoneNumber;
        /// <summary>
        /// 手机号
        /// </summary>
        [AdvQueryAttribute(ColName = "PhoneNumber",ColDesc = "手机号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PhoneNumber",Length=200,IsNullable = false,ColumnDescription = "手机号" )]
        public string PhoneNumber 
        { 
            get{return _PhoneNumber;}
            set{SetProperty(ref _PhoneNumber, value);}
        }
     

        private string _MachineCode;
        /// <summary>
        /// 机器码
        /// </summary>
        [AdvQueryAttribute(ColName = "MachineCode",ColDesc = "机器码")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "MachineCode",Length=3000,IsNullable = true,ColumnDescription = "机器码" )]
        public string MachineCode 
        { 
            get{return _MachineCode;}
            set{SetProperty(ref _MachineCode, value);}
        }
     

        private string _RegistrationCode;
        /// <summary>
        /// 注册码
        /// </summary>
        [AdvQueryAttribute(ColName = "RegistrationCode",ColDesc = "注册码")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "RegistrationCode",Length=3000,IsNullable = false,ColumnDescription = "注册码" )]
        public string RegistrationCode 
        { 
            get{return _RegistrationCode;}
            set{SetProperty(ref _RegistrationCode, value);}
        }
     

        private int _ConcurrentUsers;
        /// <summary>
        /// 同时在线用户数
        /// </summary>
        [AdvQueryAttribute(ColName = "ConcurrentUsers",ColDesc = "同时在线用户数")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "ConcurrentUsers",IsNullable = false,ColumnDescription = "同时在线用户数" )]
        public int ConcurrentUsers 
        { 
            get{return _ConcurrentUsers;}
            set{SetProperty(ref _ConcurrentUsers, value);}
        }
     

        private DateTime _ExpirationDate;
        /// <summary>
        /// 截止时间
        /// </summary>
        [AdvQueryAttribute(ColName = "ExpirationDate",ColDesc = "截止时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "ExpirationDate",IsNullable = false,ColumnDescription = "截止时间" )]
        public DateTime ExpirationDate 
        { 
            get{return _ExpirationDate;}
            set{SetProperty(ref _ExpirationDate, value);}
        }
     

        private string _ProductVersion;
        /// <summary>
        /// 版本信息
        /// </summary>
        [AdvQueryAttribute(ColName = "ProductVersion",ColDesc = "版本信息")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ProductVersion",Length=200,IsNullable = false,ColumnDescription = "版本信息" )]
        public string ProductVersion 
        { 
            get{return _ProductVersion;}
            set{SetProperty(ref _ProductVersion, value);}
        }
     

        private string _LicenseType;
        /// <summary>
        /// 授权类型
        /// </summary>
        [AdvQueryAttribute(ColName = "LicenseType",ColDesc = "授权类型")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "LicenseType",Length=20,IsNullable = false,ColumnDescription = "授权类型" )]
        public string LicenseType 
        { 
            get{return _LicenseType;}
            set{SetProperty(ref _LicenseType, value);}
        }
     

        private DateTime _PurchaseDate;
        /// <summary>
        /// 购买日期
        /// </summary>
        [AdvQueryAttribute(ColName = "PurchaseDate",ColDesc = "购买日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "PurchaseDate",IsNullable = false,ColumnDescription = "购买日期" )]
        public DateTime PurchaseDate 
        { 
            get{return _PurchaseDate;}
            set{SetProperty(ref _PurchaseDate, value);}
        }
     

        private DateTime _RegistrationDate;
        /// <summary>
        /// 注册时间
        /// </summary>
        [AdvQueryAttribute(ColName = "RegistrationDate",ColDesc = "注册时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "RegistrationDate",IsNullable = false,ColumnDescription = "注册时间" )]
        public DateTime RegistrationDate 
        { 
            get{return _RegistrationDate;}
            set{SetProperty(ref _RegistrationDate, value);}
        }
     

        private bool _IsRegistered;
        /// <summary>
        /// 已注册
        /// </summary>
        [AdvQueryAttribute(ColName = "IsRegistered",ColDesc = "已注册")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsRegistered",IsNullable = false,ColumnDescription = "已注册" )]
        public bool IsRegistered 
        { 
            get{return _IsRegistered;}
            set{SetProperty(ref _IsRegistered, value);}
        }
     

        private string _Remarks;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Remarks",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Remarks",Length=200,IsNullable = true,ColumnDescription = "备注" )]
        public string Remarks 
        { 
            get{return _Remarks;}
            set{SetProperty(ref _Remarks, value);}
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


       
    }
}



