
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:41:36
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
    /// 
    /// </summary>
    [Serializable()]
    [Description("")]
    [SugarTable("SequenceNumbers")]
    public partial class SequenceNumbers: BaseEntity, ICloneable
    {
        public SequenceNumbers()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("SequenceNumbers" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private int _Id;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        { 
            get{return _Id;}
            set{
            SetProperty(ref _Id, value);
                base.PrimaryKeyID = _Id;
            }
        }

        private string _SequenceKey;
        /// <summary>
        /// 序号键，唯一标识一个序号序列
        /// </summary>
        [AdvQueryAttribute(ColName = "SequenceKey",ColDesc = "序号键，唯一标识一个序号序列")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "SequenceKey" ,Length=255,IsNullable = false,ColumnDescription = "序号键，唯一标识一个序号序列" )]
        public string SequenceKey
        { 
            get{return _SequenceKey;}
            set{
            SetProperty(ref _SequenceKey, value);
                        }
        }

        private long _CurrentValue;
        /// <summary>
        /// 当前序号值
        /// </summary>
        [AdvQueryAttribute(ColName = "CurrentValue",ColDesc = "当前序号值")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CurrentValue" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "当前序号值" )]
        public long CurrentValue
        { 
            get{return _CurrentValue;}
            set{
            SetProperty(ref _CurrentValue, value);
                        }
        }

        private DateTime _LastUpdated;
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [AdvQueryAttribute(ColName = "LastUpdated",ColDesc = "最后更新时间")] 
        [SugarColumn(ColumnDataType = "datetime2", SqlParameterDbType ="DateTime",  ColumnName = "LastUpdated" ,IsNullable = false,ColumnDescription = "最后更新时间" )]
        public DateTime LastUpdated
        { 
            get{return _LastUpdated;}
            set{
            SetProperty(ref _LastUpdated, value);
                        }
        }

        private DateTime _CreatedAt;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "CreatedAt",ColDesc = "创建时间")] 
        [SugarColumn(ColumnDataType = "datetime2", SqlParameterDbType ="DateTime",  ColumnName = "CreatedAt" ,IsNullable = false,ColumnDescription = "创建时间" )]
        public DateTime CreatedAt
        { 
            get{return _CreatedAt;}
            set{
            SetProperty(ref _CreatedAt, value);
                        }
        }

        private string _ResetType;
        /// <summary>
        /// 重置类型: None, Daily, Monthly, Yearly
        /// </summary>
        [AdvQueryAttribute(ColName = "ResetType",ColDesc = "重置类型: None, Daily, Monthly, Yearly")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "ResetType" ,Length=20,IsNullable = true,ColumnDescription = "重置类型: None, Daily, Monthly, Yearly" )]
        public string ResetType
        { 
            get{return _ResetType;}
            set{
            SetProperty(ref _ResetType, value);
                        }
        }

        private string _FormatMask;
        /// <summary>
        /// 格式化掩码，如 000
        /// </summary>
        [AdvQueryAttribute(ColName = "FormatMask",ColDesc = "格式化掩码，如 000")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "FormatMask" ,Length=50,IsNullable = true,ColumnDescription = "格式化掩码，如 000" )]
        public string FormatMask
        { 
            get{return _FormatMask;}
            set{
            SetProperty(ref _FormatMask, value);
                        }
        }

        private string _Description;
        /// <summary>
        /// 序列描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Description",ColDesc = "序列描述")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "Description" ,Length=255,IsNullable = true,ColumnDescription = "序列描述" )]
        public string Description
        { 
            get{return _Description;}
            set{
            SetProperty(ref _Description, value);
                        }
        }

        private string _BusinessType;
        /// <summary>
        /// 业务类型
        /// </summary>
        [AdvQueryAttribute(ColName = "BusinessType",ColDesc = "业务类型")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "BusinessType" ,Length=100,IsNullable = true,ColumnDescription = "业务类型" )]
        public string BusinessType
        { 
            get{return _BusinessType;}
            set{
            SetProperty(ref _BusinessType, value);
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
            SequenceNumbers loctype = (SequenceNumbers)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

