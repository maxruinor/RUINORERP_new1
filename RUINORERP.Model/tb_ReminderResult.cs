
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/26/2025 12:18:32
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
    /// 用户接收提醒内容
    /// </summary>
    [Serializable()]
    [Description("用户接收提醒内容")]
    [SugarTable("tb_ReminderResult")]
    public partial class tb_ReminderResult: BaseEntity, ICloneable
    {
        public tb_ReminderResult()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("用户接收提醒内容tb_ReminderResult" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ResultId;
        /// <summary>
        /// 提醒结果
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ResultId" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "提醒结果" , IsPrimaryKey = true)]
        public long ResultId
        { 
            get{return _ResultId;}
            set{
            SetProperty(ref _ResultId, value);
                base.PrimaryKeyID = _ResultId;
            }
        }

        private long? _RuleId;
        /// <summary>
        /// 提醒规则
        /// </summary>
        [AdvQueryAttribute(ColName = "RuleId",ColDesc = "提醒规则")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RuleId" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "提醒规则" )]
        [FKRelationAttribute("tb_ReminderRule","RuleId")]
        public long? RuleId
        { 
            get{return _RuleId;}
            set{
            SetProperty(ref _RuleId, value);
                        }
        }

        private int _ReminderBizType;
        /// <summary>
        /// 提醒类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ReminderBizType",ColDesc = "提醒类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ReminderBizType" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "提醒类型" )]
        public int ReminderBizType
        { 
            get{return _ReminderBizType;}
            set{
            SetProperty(ref _ReminderBizType, value);
                        }
        }

        private DateTime _TriggerTime;
        /// <summary>
        /// 提醒时间
        /// </summary>
        [AdvQueryAttribute(ColName = "TriggerTime",ColDesc = "提醒时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "TriggerTime" ,IsNullable = false,ColumnDescription = "提醒时间" )]
        public DateTime TriggerTime
        { 
            get{return _TriggerTime;}
            set{
            SetProperty(ref _TriggerTime, value);
                        }
        }

        private string _Message;
        /// <summary>
        /// 提醒内容
        /// </summary>
        [AdvQueryAttribute(ColName = "Message",ColDesc = "提醒内容")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Message" ,Length=200,IsNullable = true,ColumnDescription = "提醒内容" )]
        public string Message
        { 
            get{return _Message;}
            set{
            SetProperty(ref _Message, value);
                        }
        }

        private bool _IsRead= false;
        /// <summary>
        /// 已读
        /// </summary>
        [AdvQueryAttribute(ColName = "IsRead",ColDesc = "已读")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsRead" ,IsNullable = false,ColumnDescription = "已读" )]
        public bool IsRead
        { 
            get{return _IsRead;}
            set{
            SetProperty(ref _IsRead, value);
                        }
        }

        private DateTime? _ReadTime;
        /// <summary>
        /// 读取时间
        /// </summary>
        [AdvQueryAttribute(ColName = "ReadTime",ColDesc = "读取时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "ReadTime" ,IsNullable = true,ColumnDescription = "读取时间" )]
        public DateTime? ReadTime
        { 
            get{return _ReadTime;}
            set{
            SetProperty(ref _ReadTime, value);
                        }
        }

        private string _JsonResult;
        /// <summary>
        /// 扩展JSON结果
        /// </summary>
        [AdvQueryAttribute(ColName = "JsonResult",ColDesc = "扩展JSON结果")] 
        [SugarColumn(ColumnDataType = "text", SqlParameterDbType ="String",  ColumnName = "JsonResult" ,Length=2147483647,IsNullable = true,ColumnDescription = "扩展JSON结果" )]
        public string JsonResult
        { 
            get{return _JsonResult;}
            set{
            SetProperty(ref _JsonResult, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(RuleId))]
        public virtual tb_ReminderRule tb_reminderrule { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_ReminderResult loctype = (tb_ReminderResult)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

