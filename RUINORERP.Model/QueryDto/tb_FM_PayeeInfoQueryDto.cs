
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/30/2025 15:18:03
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
    /// 收款信息，供应商报销人的收款账号
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FM_PayeeInfo")]
    public partial class tb_FM_PayeeInfoQueryDto:BaseEntityDto
    {
        public tb_FM_PayeeInfoQueryDto()
        {

        }

    
     

        private long? _Employee_ID;
        /// <summary>
        /// 员工
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "员工")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Employee_ID",IsNullable = true,ColumnDescription = "员工" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}
            set{SetProperty(ref _Employee_ID, value);}
        }
     

        private long? _CustomerVendor_ID;
        /// <summary>
        /// 往来单位
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "往来单位")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "CustomerVendor_ID",IsNullable = true,ColumnDescription = "往来单位" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long? CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}
            set{SetProperty(ref _CustomerVendor_ID, value);}
        }
     

        private int _Account_type;
        /// <summary>
        /// 账户类型
        /// </summary>
        [AdvQueryAttribute(ColName = "Account_type",ColDesc = "账户类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Account_type",IsNullable = false,ColumnDescription = "账户类型" )]
        public int Account_type 
        { 
            get{return _Account_type;}
            set{SetProperty(ref _Account_type, value);}
        }
     

        private string _Account_name;
        /// <summary>
        /// 账户名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Account_name",ColDesc = "账户名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Account_name",Length=50,IsNullable = true,ColumnDescription = "账户名称" )]
        public string Account_name 
        { 
            get{return _Account_name;}
            set{SetProperty(ref _Account_name, value);}
        }
     

        private string _Account_No;
        /// <summary>
        /// 账号
        /// </summary>
        [AdvQueryAttribute(ColName = "Account_No",ColDesc = "账号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Account_No",Length=100,IsNullable = true,ColumnDescription = "账号" )]
        public string Account_No 
        { 
            get{return _Account_No;}
            set{SetProperty(ref _Account_No, value);}
        }
     

        private string _PaymentCodeImagePath;
        /// <summary>
        /// 收款码
        /// </summary>
        [AdvQueryAttribute(ColName = "PaymentCodeImagePath",ColDesc = "收款码")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PaymentCodeImagePath",Length=300,IsNullable = true,ColumnDescription = "收款码" )]
        public string PaymentCodeImagePath 
        { 
            get{return _PaymentCodeImagePath;}
            set{SetProperty(ref _PaymentCodeImagePath, value);}
        }
     

        private string _BelongingBank;
        /// <summary>
        /// 所属银行
        /// </summary>
        [AdvQueryAttribute(ColName = "BelongingBank",ColDesc = "所属银行")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "BelongingBank",Length=50,IsNullable = true,ColumnDescription = "所属银行" )]
        public string BelongingBank 
        { 
            get{return _BelongingBank;}
            set{SetProperty(ref _BelongingBank, value);}
        }
     

        private string _OpeningBank;
        /// <summary>
        /// 开户行
        /// </summary>
        [AdvQueryAttribute(ColName = "OpeningBank",ColDesc = "开户行")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "OpeningBank",Length=60,IsNullable = true,ColumnDescription = "开户行" )]
        public string OpeningBank 
        { 
            get{return _OpeningBank;}
            set{SetProperty(ref _OpeningBank, value);}
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
     

        private bool _IsDefault= false;
        /// <summary>
        /// 默认账号
        /// </summary>
        [AdvQueryAttribute(ColName = "IsDefault",ColDesc = "默认账号")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsDefault",IsNullable = false,ColumnDescription = "默认账号" )]
        public bool IsDefault 
        { 
            get{return _IsDefault;}
            set{SetProperty(ref _IsDefault, value);}
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


       
    }
}



