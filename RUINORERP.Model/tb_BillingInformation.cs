
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/12/2025 21:29:58
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
    /// 开票资料表
    /// </summary>
    [Serializable()]
    [Description("开票资料表")]
    [SugarTable("tb_BillingInformation")]
    public partial class tb_BillingInformation: BaseEntity, ICloneable
    {
        public tb_BillingInformation()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("开票资料表tb_BillingInformation" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _BillingInfo_ID;
        /// <summary>
        /// 开票资料
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BillingInfo_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "开票资料" , IsPrimaryKey = true)]
        public long BillingInfo_ID
        { 
            get{return _BillingInfo_ID;}
            set{
            SetProperty(ref _BillingInfo_ID, value);
                base.PrimaryKeyID = _BillingInfo_ID;
            }
        }

        private long _CustomerVendor_ID;
        /// <summary>
        /// 往来单位
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "往来单位")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "往来单位" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long CustomerVendor_ID
        { 
            get{return _CustomerVendor_ID;}
            set{
            SetProperty(ref _CustomerVendor_ID, value);
                        }
        }

        private string _Title;
        /// <summary>
        /// 抬头
        /// </summary>
        [AdvQueryAttribute(ColName = "Title",ColDesc = "抬头")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Title" ,Length=200,IsNullable = true,ColumnDescription = "抬头" )]
        public string Title
        { 
            get{return _Title;}
            set{
            SetProperty(ref _Title, value);
                        }
        }

        private string _TaxNumber;
        /// <summary>
        /// 税号
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxNumber",ColDesc = "税号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "TaxNumber" ,Length=200,IsNullable = true,ColumnDescription = "税号" )]
        public string TaxNumber
        { 
            get{return _TaxNumber;}
            set{
            SetProperty(ref _TaxNumber, value);
                        }
        }

        private string _Address;
        /// <summary>
        /// 地址
        /// </summary>
        [AdvQueryAttribute(ColName = "Address",ColDesc = "地址")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Address" ,Length=200,IsNullable = true,ColumnDescription = "地址" )]
        public string Address
        { 
            get{return _Address;}
            set{
            SetProperty(ref _Address, value);
                        }
        }

        private string _PITEL;
        /// <summary>
        /// 电话
        /// </summary>
        [AdvQueryAttribute(ColName = "PITEL",ColDesc = "电话")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PITEL" ,Length=50,IsNullable = true,ColumnDescription = "电话" )]
        public string PITEL
        { 
            get{return _PITEL;}
            set{
            SetProperty(ref _PITEL, value);
                        }
        }

        private string _BankAccount;
        /// <summary>
        /// 银行账号
        /// </summary>
        [AdvQueryAttribute(ColName = "BankAccount",ColDesc = "银行账号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BankAccount" ,Length=150,IsNullable = true,ColumnDescription = "银行账号" )]
        public string BankAccount
        { 
            get{return _BankAccount;}
            set{
            SetProperty(ref _BankAccount, value);
                        }
        }

        private string _BankName;
        /// <summary>
        /// 开户行
        /// </summary>
        [AdvQueryAttribute(ColName = "BankName",ColDesc = "开户行")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BankName" ,Length=50,IsNullable = true,ColumnDescription = "开户行" )]
        public string BankName
        { 
            get{return _BankName;}
            set{
            SetProperty(ref _BankName, value);
                        }
        }

        private string _Email;
        /// <summary>
        /// 邮箱
        /// </summary>
        [AdvQueryAttribute(ColName = "Email",ColDesc = "邮箱")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Email" ,Length=150,IsNullable = true,ColumnDescription = "邮箱" )]
        public string Email
        { 
            get{return _Email;}
            set{
            SetProperty(ref _Email, value);
                        }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=255,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
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

        private bool _isdeleted= false;
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "isdeleted" ,IsNullable = false,ColumnDescription = "逻辑删除" )]
        [Browsable(false)]
        public bool isdeleted
        { 
            get{return _isdeleted;}
            set{
            SetProperty(ref _isdeleted, value);
                        }
        }

        private bool _IsActive= true;
        /// <summary>
        /// 激活
        /// </summary>
        [AdvQueryAttribute(ColName = "IsActive",ColDesc = "激活")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsActive" ,IsNullable = false,ColumnDescription = "激活" )]
        public bool IsActive
        { 
            get{return _IsActive;}
            set{
            SetProperty(ref _IsActive, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(CustomerVendor_ID))]
        public virtual tb_CustomerVendor tb_customervendor { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}




 
        

        public override object Clone()
        {
            tb_BillingInformation loctype = (tb_BillingInformation)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

