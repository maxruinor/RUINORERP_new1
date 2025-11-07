
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:42:19
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
    /// 业务编号规则
    /// </summary>
    [Serializable()]
    [Description("业务编号规则")]
    [SugarTable("tb_sys_BillNoRule")]
    public partial class tb_sys_BillNoRule: BaseEntity, ICloneable
    {
        public tb_sys_BillNoRule()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("业务编号规则tb_sys_BillNoRule" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _BillNoRuleID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BillNoRuleID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long BillNoRuleID
        { 
            get{return _BillNoRuleID;}
            set{
            SetProperty(ref _BillNoRuleID, value);
                base.PrimaryKeyID = _BillNoRuleID;
            }
        }

        private string _RuleName;
        /// <summary>
        /// 规则名称
        /// </summary>
        [AdvQueryAttribute(ColName = "RuleName",ColDesc = "规则名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "RuleName" ,Length=200,IsNullable = false,ColumnDescription = "规则名称" )]
        public string RuleName
        { 
            get{return _RuleName;}
            set{
            SetProperty(ref _RuleName, value);
                        }
        }

        private int _BizType;
        /// <summary>
        /// 业务类型
        /// </summary>
 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "BizType" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "业务类型" , IsPrimaryKey = true)]
        public int BizType
        { 
            get{return _BizType;}
            set{
            SetProperty(ref _BizType, value);
                base.PrimaryKeyID = _BizType;
            }
        }

        private string _Prefix;
        /// <summary>
        /// 前缀
        /// </summary>
        [AdvQueryAttribute(ColName = "Prefix",ColDesc = "前缀")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Prefix" ,Length=200,IsNullable = false,ColumnDescription = "前缀" )]
        public string Prefix
        { 
            get{return _Prefix;}
            set{
            SetProperty(ref _Prefix, value);
                        }
        }

        private int _DateFormat;
        /// <summary>
        /// 日期格式
        /// </summary>
        [AdvQueryAttribute(ColName = "DateFormat",ColDesc = "日期格式")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DateFormat" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "日期格式" )]
        public int DateFormat
        { 
            get{return _DateFormat;}
            set{
            SetProperty(ref _DateFormat, value);
                        }
        }

        private int _SequenceLength;
        /// <summary>
        /// 流水号长度
        /// </summary>
        [AdvQueryAttribute(ColName = "SequenceLength",ColDesc = "流水号长度")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "SequenceLength" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "流水号长度" )]
        public int SequenceLength
        { 
            get{return _SequenceLength;}
            set{
            SetProperty(ref _SequenceLength, value);
                        }
        }

        private bool _UseCheckDigit;
        /// <summary>
        /// 是否使用校验位
        /// </summary>
        [AdvQueryAttribute(ColName = "UseCheckDigit",ColDesc = "是否使用校验位")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "UseCheckDigit" ,IsNullable = false,ColumnDescription = "是否使用校验位" )]
        public bool UseCheckDigit
        { 
            get{return _UseCheckDigit;}
            set{
            SetProperty(ref _UseCheckDigit, value);
                        }
        }

        private string _RedisKeyPattern;
        /// <summary>
        /// Redis键模式
        /// </summary>
        [AdvQueryAttribute(ColName = "RedisKeyPattern",ColDesc = "Redis键模式")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "RedisKeyPattern" ,Length=3000,IsNullable = true,ColumnDescription = "Redis键模式" )]
        public string RedisKeyPattern
        { 
            get{return _RedisKeyPattern;}
            set{
            SetProperty(ref _RedisKeyPattern, value);
                        }
        }

        private int _ResetMode;
        /// <summary>
        /// 重置模式
        /// </summary>
        [AdvQueryAttribute(ColName = "ResetMode",ColDesc = "重置模式")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ResetMode" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "重置模式" )]
        public int ResetMode
        { 
            get{return _ResetMode;}
            set{
            SetProperty(ref _ResetMode, value);
                        }
        }

        private bool _IsActive= true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "IsActive",ColDesc = "是否启用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsActive" ,IsNullable = false,ColumnDescription = "是否启用" )]
        public bool IsActive
        { 
            get{return _IsActive;}
            set{
            SetProperty(ref _IsActive, value);
                        }
        }

        private string _Description;
        /// <summary>
        /// 规则描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Description",ColDesc = "规则描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Description" ,Length=200,IsNullable = true,ColumnDescription = "规则描述" )]
        public string Description
        { 
            get{return _Description;}
            set{
            SetProperty(ref _Description, value);
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






       
        

        public override object Clone()
        {
            tb_sys_BillNoRule loctype = (tb_sys_BillNoRule)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

