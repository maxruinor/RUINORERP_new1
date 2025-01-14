
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/14/2025 18:56:44
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
    /// 银行账号信息表
    /// </summary>
    [Serializable()]
    [Description("银行账号信息表")]
    [SugarTable("tb_BankAccount")]
    public partial class tb_BankAccount: BaseEntity, ICloneable
    {
        public tb_BankAccount()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("银行账号信息表tb_BankAccount" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _BankAccount_id;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BankAccount_id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long BankAccount_id
        { 
            get{return _BankAccount_id;}
            set{
            base.PrimaryKeyID = _BankAccount_id;
            SetProperty(ref _BankAccount_id, value);
            }
        }

        private string _Account_Name;
        /// <summary>
        /// 账户名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Account_Name",ColDesc = "账户名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Account_Name" ,Length=100,IsNullable = false,ColumnDescription = "账户名称" )]
        public string Account_Name
        { 
            get{return _Account_Name;}
            set{
            SetProperty(ref _Account_Name, value);
            }
        }

        private string _Account_No;
        /// <summary>
        /// 账号
        /// </summary>
        [AdvQueryAttribute(ColName = "Account_No",ColDesc = "账号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Account_No" ,Length=100,IsNullable = false,ColumnDescription = "账号" )]
        public string Account_No
        { 
            get{return _Account_No;}
            set{
            SetProperty(ref _Account_No, value);
            }
        }

        private string _OpeningBank;
        /// <summary>
        /// 开户行
        /// </summary>
        [AdvQueryAttribute(ColName = "OpeningBank",ColDesc = "开户行")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "OpeningBank" ,Length=100,IsNullable = false,ColumnDescription = "开户行" )]
        public string OpeningBank
        { 
            get{return _OpeningBank;}
            set{
            SetProperty(ref _OpeningBank, value);
            }
        }

        private bool _Is_enabled= true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_enabled",ColDesc = "是否启用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Is_enabled" ,IsNullable = false,ColumnDescription = "是否启用" )]
        public bool Is_enabled
        { 
            get{return _Is_enabled;}
            set{
            SetProperty(ref _Is_enabled, value);
            }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=200,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
            }
        }

        #endregion

        #region 扩展属性

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Employee.BankAccount_id))]
        public virtual List<tb_Employee> tb_Employees { get; set; }
        //tb_Employee.BankAccount_id)
        //BankAccount_id.FK_EMPLOYEE_REF_BANKACCOUNT)
        //tb_BankAccount.BankAccount_id)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_CustomerVendor.BankAccount_id))]
        public virtual List<tb_CustomerVendor> tb_CustomerVendors { get; set; }
        //tb_CustomerVendor.BankAccount_id)
        //BankAccount_id.FK_CUSTOMERVENDOR_REF_BANKACCOUNT)
        //tb_BankAccount.BankAccount_id)


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
                    Type type = typeof(tb_BankAccount);
                    
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
            tb_BankAccount loctype = (tb_BankAccount)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

