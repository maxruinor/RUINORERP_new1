
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/28/2024 18:55:33
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{
    /// <summary>
    /// 系统注册信息
    /// </summary>
    [Serializable()]
    [Description("系统注册信息")]
    [SugarTable("tb_sys_RegistrationInfo")]
    public partial class tb_sys_RegistrationInfo: BaseEntity, ICloneable
    {
        public tb_sys_RegistrationInfo()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("系统注册信息tb_sys_RegistrationInfo" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _RegistrationInfoD;
        /// <summary>
        /// 注册信息
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RegistrationInfoD" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "注册信息" , IsPrimaryKey = true)]
        public long RegistrationInfoD
        { 
            get{return _RegistrationInfoD;}
            set{
            base.PrimaryKeyID = _RegistrationInfoD;
            SetProperty(ref _RegistrationInfoD, value);
            }
        }

        private string _CompanyName;
        /// <summary>
        /// 注册公司名
        /// </summary>
        [AdvQueryAttribute(ColName = "CompanyName",ColDesc = "注册公司名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CompanyName" ,Length=200,IsNullable = false,ColumnDescription = "注册公司名" )]
        public string CompanyName
        { 
            get{return _CompanyName;}
            set{
            SetProperty(ref _CompanyName, value);
            }
        }

        private string _ContactName;
        /// <summary>
        /// 联系人姓名
        /// </summary>
        [AdvQueryAttribute(ColName = "ContactName",ColDesc = "联系人姓名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ContactName" ,Length=200,IsNullable = false,ColumnDescription = "联系人姓名" )]
        public string ContactName
        { 
            get{return _ContactName;}
            set{
            SetProperty(ref _ContactName, value);
            }
        }

        private string _PhoneNumber;
        /// <summary>
        /// 手机号
        /// </summary>
        [AdvQueryAttribute(ColName = "PhoneNumber",ColDesc = "手机号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PhoneNumber" ,Length=200,IsNullable = false,ColumnDescription = "手机号" )]
        public string PhoneNumber
        { 
            get{return _PhoneNumber;}
            set{
            SetProperty(ref _PhoneNumber, value);
            }
        }

        private string _MachineCode;
        /// <summary>
        /// 机器码
        /// </summary>
        [AdvQueryAttribute(ColName = "MachineCode",ColDesc = "机器码")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "MachineCode" ,Length=1000,IsNullable = true,ColumnDescription = "机器码" )]
        public string MachineCode
        { 
            get{return _MachineCode;}
            set{
            SetProperty(ref _MachineCode, value);
            }
        }

        private string _RegistrationCode;
        /// <summary>
        /// 注册码
        /// </summary>
        [AdvQueryAttribute(ColName = "RegistrationCode",ColDesc = "注册码")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "RegistrationCode" ,Length=100,IsNullable = false,ColumnDescription = "注册码" )]
        public string RegistrationCode
        { 
            get{return _RegistrationCode;}
            set{
            SetProperty(ref _RegistrationCode, value);
            }
        }

        private int _ConcurrentUsers;
        /// <summary>
        /// 同时在线用户数
        /// </summary>
        [AdvQueryAttribute(ColName = "ConcurrentUsers",ColDesc = "同时在线用户数")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ConcurrentUsers" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "同时在线用户数" )]
        public int ConcurrentUsers
        { 
            get{return _ConcurrentUsers;}
            set{
            SetProperty(ref _ConcurrentUsers, value);
            }
        }

        private DateTime _ExpirationDate;
        /// <summary>
        /// 截止时间
        /// </summary>
        [AdvQueryAttribute(ColName = "ExpirationDate",ColDesc = "截止时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "ExpirationDate" ,IsNullable = false,ColumnDescription = "截止时间" )]
        public DateTime ExpirationDate
        { 
            get{return _ExpirationDate;}
            set{
            SetProperty(ref _ExpirationDate, value);
            }
        }

        private string _ProductVersion;
        /// <summary>
        /// 版本信息
        /// </summary>
        [AdvQueryAttribute(ColName = "ProductVersion",ColDesc = "版本信息")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ProductVersion" ,Length=200,IsNullable = false,ColumnDescription = "版本信息" )]
        public string ProductVersion
        { 
            get{return _ProductVersion;}
            set{
            SetProperty(ref _ProductVersion, value);
            }
        }

        private int _LicenseType;
        /// <summary>
        /// 授权类型
        /// </summary>
        [AdvQueryAttribute(ColName = "LicenseType",ColDesc = "授权类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "LicenseType" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "授权类型" )]
        public int LicenseType
        { 
            get{return _LicenseType;}
            set{
            SetProperty(ref _LicenseType, value);
            }
        }

        private DateTime _PurchaseDate;
        /// <summary>
        /// 购买日期
        /// </summary>
        [AdvQueryAttribute(ColName = "PurchaseDate",ColDesc = "购买日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "PurchaseDate" ,IsNullable = false,ColumnDescription = "购买日期" )]
        public DateTime PurchaseDate
        { 
            get{return _PurchaseDate;}
            set{
            SetProperty(ref _PurchaseDate, value);
            }
        }

        private DateTime _RegistrationDate;
        /// <summary>
        /// 注册时间
        /// </summary>
        [AdvQueryAttribute(ColName = "RegistrationDate",ColDesc = "注册时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "RegistrationDate" ,IsNullable = false,ColumnDescription = "注册时间" )]
        public DateTime RegistrationDate
        { 
            get{return _RegistrationDate;}
            set{
            SetProperty(ref _RegistrationDate, value);
            }
        }

        private bool _IsRegistered;
        /// <summary>
        /// 已注册
        /// </summary>
        [AdvQueryAttribute(ColName = "IsRegistered",ColDesc = "已注册")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsRegistered" ,IsNullable = false,ColumnDescription = "已注册" )]
        public bool IsRegistered
        { 
            get{return _IsRegistered;}
            set{
            SetProperty(ref _IsRegistered, value);
            }
        }

        private string _Remarks;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Remarks",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Remarks" ,Length=200,IsNullable = true,ColumnDescription = "备注" )]
        public string Remarks
        { 
            get{return _Remarks;}
            set{
            SetProperty(ref _Remarks, value);
            }
        }

        private DateTime? _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Created_at" ,IsNullable = true,ColumnDescription = "创建时间" )]
        public DateTime? Created_at
        { 
            get{return _Created_at;}
            set{
            SetProperty(ref _Created_at, value);
            }
        }

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Created_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "创建人" )]
        public long? Created_by
        { 
            get{return _Created_by;}
            set{
            SetProperty(ref _Created_by, value);
            }
        }

        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Modified_at" ,IsNullable = true,ColumnDescription = "修改时间" )]
        public DateTime? Modified_at
        { 
            get{return _Modified_at;}
            set{
            SetProperty(ref _Modified_at, value);
            }
        }

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Modified_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "修改人" )]
        public long? Modified_by
        { 
            get{return _Modified_by;}
            set{
            SetProperty(ref _Modified_by, value);
            }
        }

        #endregion

        #region 扩展属性


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






        #region 字段描述对应列表
        private ConcurrentDictionary<string, string> fieldNameList;


        /// <summary>
        /// 表列名的中文描述集合
        /// </summary>
        [Description("列名中文描述"), Category("自定属性")]
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public override ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, string>();
                    SugarColumn entityAttr;
                    Type type = typeof(tb_sys_RegistrationInfo);
                    
                       foreach (PropertyInfo field in type.GetProperties())
                            {
                                foreach (Attribute attr in field.GetCustomAttributes(true))
                                {
                                    entityAttr = attr as SugarColumn;
                                    if (null != entityAttr)
                                    {
                                        if (entityAttr.ColumnDescription == null)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.IsIdentity)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.IsPrimaryKey)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.ColumnDescription.Trim().Length > 0)
                                        {
                                            fieldNameList.TryAdd(field.Name, entityAttr.ColumnDescription);
                                        }
                                    }
                                }
                            }
                }
                
                return fieldNameList;
            }
            set
            {
                fieldNameList = value;
            }

        }
        #endregion
        

        public override object Clone()
        {
            tb_sys_RegistrationInfo loctype = (tb_sys_RegistrationInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

