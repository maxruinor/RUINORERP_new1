
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/25/2025 10:38:53
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
    /// 客户厂商表 开票资料这种与财务有关另外开表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_CustomerVendor")]
    public partial class tb_CustomerVendorQueryDto:BaseEntityDto
    {
        public tb_CustomerVendorQueryDto()
        {

        }

    
     

        private string _CVCode;
        /// <summary>
        /// 编号
        /// </summary>
        [AdvQueryAttribute(ColName = "CVCode",ColDesc = "编号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "CVCode",Length=50,IsNullable = true,ColumnDescription = "编号" )]
        public string CVCode 
        { 
            get{return _CVCode;}
            set{SetProperty(ref _CVCode, value);}
        }
     

        private string _CVName;
        /// <summary>
        /// 全称
        /// </summary>
        [AdvQueryAttribute(ColName = "CVName",ColDesc = "全称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "CVName",Length=255,IsNullable = false,ColumnDescription = "全称" )]
        public string CVName 
        { 
            get{return _CVName;}
            set{SetProperty(ref _CVName, value);}
        }
     

        private string _ShortName;
        /// <summary>
        /// 简称
        /// </summary>
        [AdvQueryAttribute(ColName = "ShortName",ColDesc = "简称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ShortName",Length=50,IsNullable = true,ColumnDescription = "简称" )]
        public string ShortName 
        { 
            get{return _ShortName;}
            set{SetProperty(ref _ShortName, value);}
        }
     

        private long? _Type_ID;
        /// <summary>
        /// 客户厂商类型
        /// </summary>
        [AdvQueryAttribute(ColName = "Type_ID",ColDesc = "客户厂商类型")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Type_ID",IsNullable = true,ColumnDescription = "客户厂商类型" )]
        [FKRelationAttribute("tb_CustomerVendorType","Type_ID")]
        public long? Type_ID 
        { 
            get{return _Type_ID;}
            set{SetProperty(ref _Type_ID, value);}
        }
     

        private long? _Employee_ID;
        /// <summary>
        /// 责任人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "责任人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Employee_ID",IsNullable = true,ColumnDescription = "责任人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}
            set{SetProperty(ref _Employee_ID, value);}
        }
     

        private bool _IsExclusive= true;
        /// <summary>
        /// 责任人专属
        /// </summary>
        [AdvQueryAttribute(ColName = "IsExclusive",ColDesc = "责任人专属")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsExclusive",IsNullable = false,ColumnDescription = "责任人专属" )]
        public bool IsExclusive 
        { 
            get{return _IsExclusive;}
            set{SetProperty(ref _IsExclusive, value);}
        }
     

        private long? _Paytype_ID;
        /// <summary>
        /// 默认交易方式
        /// </summary>
        [AdvQueryAttribute(ColName = "Paytype_ID",ColDesc = "默认交易方式")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Paytype_ID",IsNullable = true,ColumnDescription = "默认交易方式" )]
        [FKRelationAttribute("tb_PaymentMethod","Paytype_ID")]
        public long? Paytype_ID 
        { 
            get{return _Paytype_ID;}
            set{SetProperty(ref _Paytype_ID, value);}
        }
     

        private long? _Customer_id;
        /// <summary>
        /// 目标客户
        /// </summary>
        [AdvQueryAttribute(ColName = "Customer_id",ColDesc = "目标客户")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Customer_id",IsNullable = true,ColumnDescription = "目标客户" )]
        public long? Customer_id 
        { 
            get{return _Customer_id;}
            set{SetProperty(ref _Customer_id, value);}
        }
     

        private string _Area;
        /// <summary>
        /// 所在地区
        /// </summary>
        [AdvQueryAttribute(ColName = "Area",ColDesc = "所在地区")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Area",Length=50,IsNullable = true,ColumnDescription = "所在地区" )]
        public string Area 
        { 
            get{return _Area;}
            set{SetProperty(ref _Area, value);}
        }
     

        private string _Contact;
        /// <summary>
        /// 联系人
        /// </summary>
        [AdvQueryAttribute(ColName = "Contact",ColDesc = "联系人")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Contact",Length=50,IsNullable = true,ColumnDescription = "联系人" )]
        public string Contact 
        { 
            get{return _Contact;}
            set{SetProperty(ref _Contact, value);}
        }
     

        private string _Phone;
        /// <summary>
        /// 电话
        /// </summary>
        [AdvQueryAttribute(ColName = "Phone",ColDesc = "电话")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Phone",Length=255,IsNullable = true,ColumnDescription = "电话" )]
        public string Phone 
        { 
            get{return _Phone;}
            set{SetProperty(ref _Phone, value);}
        }
     

        private string _Address;
        /// <summary>
        /// 地址
        /// </summary>
        [AdvQueryAttribute(ColName = "Address",ColDesc = "地址")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Address",Length=255,IsNullable = true,ColumnDescription = "地址" )]
        public string Address 
        { 
            get{return _Address;}
            set{SetProperty(ref _Address, value);}
        }
     

        private string _Website;
        /// <summary>
        /// 网址
        /// </summary>
        [AdvQueryAttribute(ColName = "Website",ColDesc = "网址")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Website",Length=255,IsNullable = true,ColumnDescription = "网址" )]
        public string Website 
        { 
            get{return _Website;}
            set{SetProperty(ref _Website, value);}
        }
     

        private decimal? _CreditLimit= ((0));
        /// <summary>
        /// 信用额度
        /// </summary>
        [AdvQueryAttribute(ColName = "CreditLimit",ColDesc = "信用额度")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "CreditLimit",IsNullable = true,ColumnDescription = "信用额度" )]
        public decimal? CreditLimit 
        { 
            get{return _CreditLimit;}
            set{SetProperty(ref _CreditLimit, value);}
        }
     

        private int? _CreditDays= ((0));
        /// <summary>
        /// 账期天数
        /// </summary>
        [AdvQueryAttribute(ColName = "CreditDays",ColDesc = "账期天数")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "CreditDays",IsNullable = true,ColumnDescription = "账期天数" )]
        public int? CreditDays 
        { 
            get{return _CreditDays;}
            set{SetProperty(ref _CreditDays, value);}
        }
     

        private bool _IsCustomer= false;
        /// <summary>
        /// 是客户
        /// </summary>
        [AdvQueryAttribute(ColName = "IsCustomer",ColDesc = "是客户")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsCustomer",IsNullable = false,ColumnDescription = "是客户" )]
        public bool IsCustomer 
        { 
            get{return _IsCustomer;}
            set{SetProperty(ref _IsCustomer, value);}
        }
     

        private bool _IsVendor= false;
        /// <summary>
        /// 是供应商
        /// </summary>
        [AdvQueryAttribute(ColName = "IsVendor",ColDesc = "是供应商")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsVendor",IsNullable = false,ColumnDescription = "是供应商" )]
        public bool IsVendor 
        { 
            get{return _IsVendor;}
            set{SetProperty(ref _IsVendor, value);}
        }
     

        private bool _IsOther= false;
        /// <summary>
        /// 是其他
        /// </summary>
        [AdvQueryAttribute(ColName = "IsOther",ColDesc = "是其他")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsOther",IsNullable = false,ColumnDescription = "是其他" )]
        public bool IsOther 
        { 
            get{return _IsOther;}
            set{SetProperty(ref _IsOther, value);}
        }
     

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=255,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
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
     

        private bool? _Is_enabled= true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_enabled",ColDesc = "是否启用")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Is_enabled",IsNullable = true,ColumnDescription = "是否启用" )]
        public bool? Is_enabled 
        { 
            get{return _Is_enabled;}
            set{SetProperty(ref _Is_enabled, value);}
        }
     

        private bool? _Is_available= true;
        /// <summary>
        /// 是否可用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_available",ColDesc = "是否可用")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Is_available",IsNullable = true,ColumnDescription = "是否可用" )]
        public bool? Is_available 
        { 
            get{return _Is_available;}
            set{SetProperty(ref _Is_available, value);}
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


       
    }
}



