
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/09/2026 20:34:51
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{
    /// <summary>
    /// 链路与规则关联表
    /// </summary>
    [Serializable()]
    [Description("链路与规则关联表")]
    [SugarTable("tb_ReminderLinkRuleRelation")]
    public partial class tb_ReminderLinkRuleRelation: BaseEntity, ICloneable
    {
        public tb_ReminderLinkRuleRelation()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("链路与规则关联表tb_ReminderLinkRuleRelation" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _RelationId;
        /// <summary>
        /// 关联ID
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RelationId" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "关联ID" , IsPrimaryKey = true)]
        public long RelationId
        { 
            get{return _RelationId;}
            set{
            SetProperty(ref _RelationId, value);
                base.PrimaryKeyID = _RelationId;
            }
        }

        private long? _LinkId;
        /// <summary>
        /// 链路ID
        /// </summary>
        [AdvQueryAttribute(ColName = "LinkId",ColDesc = "链路ID")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "LinkId" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "链路ID" )]
        [FKRelationAttribute("tb_ReminderObjectLink","LinkId")]
        public long? LinkId
        { 
            get{return _LinkId;}
            set{
            SetProperty(ref _LinkId, value);
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

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(LinkId))]
        public virtual tb_ReminderObjectLink tb_reminderobjectlink { get; set; }

        [SugarColumn(IsIgnore = true)]
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
            tb_ReminderLinkRuleRelation loctype = (tb_ReminderLinkRuleRelation)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

