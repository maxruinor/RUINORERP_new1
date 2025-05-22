
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:31:56
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
    /// 客户关系配置表
    /// </summary>
    [Serializable()]
    [Description("客户关系配置表")]
    [SugarTable("tb_CRMConfig")]
    public partial class tb_CRMConfig: BaseEntity, ICloneable
    {
        public tb_CRMConfig()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("客户关系配置表tb_CRMConfig" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _CRMConfigID;
        /// <summary>
        /// 客户关系配置
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CRMConfigID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "客户关系配置" , IsPrimaryKey = true)]
        public long CRMConfigID
        { 
            get{return _CRMConfigID;}
            set{
            SetProperty(ref _CRMConfigID, value);
                base.PrimaryKeyID = _CRMConfigID;
            }
        }

        private bool _CS_UseLeadsFunction= false;
        /// <summary>
        /// 是否使用线索功能
        /// </summary>
        [AdvQueryAttribute(ColName = "CS_UseLeadsFunction",ColDesc = "是否使用线索功能")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "CS_UseLeadsFunction" ,IsNullable = false,ColumnDescription = "是否使用线索功能" )]
        public bool CS_UseLeadsFunction
        { 
            get{return _CS_UseLeadsFunction;}
            set{
            SetProperty(ref _CS_UseLeadsFunction, value);
                        }
        }

        private int _CS_NewCustToLeadsCustDays= ((30));
        /// <summary>
        /// 新客转潜客天数
        /// </summary>
        [AdvQueryAttribute(ColName = "CS_NewCustToLeadsCustDays",ColDesc = "新客转潜客天数")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "CS_NewCustToLeadsCustDays" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "新客转潜客天数" )]
        public int CS_NewCustToLeadsCustDays
        { 
            get{return _CS_NewCustToLeadsCustDays;}
            set{
            SetProperty(ref _CS_NewCustToLeadsCustDays, value);
                        }
        }

        private int _CS_SleepingCustomerDays= ((180));
        /// <summary>
        /// 定义休眠客户天数
        /// </summary>
        [AdvQueryAttribute(ColName = "CS_SleepingCustomerDays",ColDesc = "定义休眠客户天数")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "CS_SleepingCustomerDays" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "定义休眠客户天数" )]
        public int CS_SleepingCustomerDays
        { 
            get{return _CS_SleepingCustomerDays;}
            set{
            SetProperty(ref _CS_SleepingCustomerDays, value);
                        }
        }

        private int _CS_LostCustomersDays= ((365));
        /// <summary>
        /// 定义流失客户天数
        /// </summary>
        [AdvQueryAttribute(ColName = "CS_LostCustomersDays",ColDesc = "定义流失客户天数")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "CS_LostCustomersDays" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "定义流失客户天数" )]
        public int CS_LostCustomersDays
        { 
            get{return _CS_LostCustomersDays;}
            set{
            SetProperty(ref _CS_LostCustomersDays, value);
                        }
        }

        private int _CS_ActiveCustomers= ((15));
        /// <summary>
        /// 定义活跃客户天数
        /// </summary>
        [AdvQueryAttribute(ColName = "CS_ActiveCustomers",ColDesc = "定义活跃客户天数")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "CS_ActiveCustomers" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "定义活跃客户天数" )]
        public int CS_ActiveCustomers
        { 
            get{return _CS_ActiveCustomers;}
            set{
            SetProperty(ref _CS_ActiveCustomers, value);
                        }
        }

        private int _LS_ConvCustHasFollowUpDays= ((15));
        /// <summary>
        /// 转换为客户后有跟进天数
        /// </summary>
        [AdvQueryAttribute(ColName = "LS_ConvCustHasFollowUpDays",ColDesc = "转换为客户后有跟进天数")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "LS_ConvCustHasFollowUpDays" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "转换为客户后有跟进天数" )]
        public int LS_ConvCustHasFollowUpDays
        { 
            get{return _LS_ConvCustHasFollowUpDays;}
            set{
            SetProperty(ref _LS_ConvCustHasFollowUpDays, value);
                        }
        }

        private int _LS_ConvCustNoTransDays= ((30));
        /// <summary>
        /// 转换为客户后无成交天数
        /// </summary>
        [AdvQueryAttribute(ColName = "LS_ConvCustNoTransDays",ColDesc = "转换为客户后无成交天数")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "LS_ConvCustNoTransDays" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "转换为客户后无成交天数" )]
        public int LS_ConvCustNoTransDays
        { 
            get{return _LS_ConvCustNoTransDays;}
            set{
            SetProperty(ref _LS_ConvCustNoTransDays, value);
                        }
        }

        private int _LS_ConvCustLostDays= ((60));
        /// <summary>
        /// 转换为客户后已丢失天数
        /// </summary>
        [AdvQueryAttribute(ColName = "LS_ConvCustLostDays",ColDesc = "转换为客户后已丢失天数")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "LS_ConvCustLostDays" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "转换为客户后已丢失天数" )]
        public int LS_ConvCustLostDays
        { 
            get{return _LS_ConvCustLostDays;}
            set{
            SetProperty(ref _LS_ConvCustLostDays, value);
                        }
        }

        private int _NoFollToPublicPoolDays= ((90));
        /// <summary>
        /// 无跟进转换到公海的天数
        /// </summary>
        [AdvQueryAttribute(ColName = "NoFollToPublicPoolDays",ColDesc = "无跟进转换到公海的天数")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "NoFollToPublicPoolDays" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "无跟进转换到公海的天数" )]
        public int NoFollToPublicPoolDays
        { 
            get{return _NoFollToPublicPoolDays;}
            set{
            SetProperty(ref _NoFollToPublicPoolDays, value);
                        }
        }

        private int _CustomerNoOrderDays= ((30));
        /// <summary>
        /// 客户无返单间隔提醒天数
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerNoOrderDays",ColDesc = "客户无返单间隔提醒天数")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "CustomerNoOrderDays" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "客户无返单间隔提醒天数" )]
        public int CustomerNoOrderDays
        { 
            get{return _CustomerNoOrderDays;}
            set{
            SetProperty(ref _CustomerNoOrderDays, value);
                        }
        }

        private int _CustomerNoFollowUpDays= ((20));
        /// <summary>
        /// 客户无回访间隔提醒天数
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerNoFollowUpDays",ColDesc = "客户无回访间隔提醒天数")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "CustomerNoFollowUpDays" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "客户无回访间隔提醒天数" )]
        public int CustomerNoFollowUpDays
        { 
            get{return _CustomerNoFollowUpDays;}
            set{
            SetProperty(ref _CustomerNoFollowUpDays, value);
                        }
        }

        private DateTime _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Created_at" ,IsNullable = false,ColumnDescription = "创建时间" )]
        public DateTime Created_at
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





        public override object Clone()
        {
            tb_CRMConfig loctype = (tb_CRMConfig)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

