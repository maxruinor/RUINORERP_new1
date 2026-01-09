
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/09/2026 20:34:52
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
    /// 提醒对象链路
    /// </summary>
    [Serializable()]
    [Description("提醒对象链路")]
    [SugarTable("tb_ReminderObjectLink")]
    public partial class tb_ReminderObjectLink: BaseEntity, ICloneable
    {
        public tb_ReminderObjectLink()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("提醒对象链路tb_ReminderObjectLink" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _LinkId;
        /// <summary>
        /// 链路ID
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "LinkId" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "链路ID" , IsPrimaryKey = true)]
        public long LinkId
        { 
            get{return _LinkId;}
            set{
            SetProperty(ref _LinkId, value);
                base.PrimaryKeyID = _LinkId;
            }
        }

        private string _LinkName;
        /// <summary>
        /// 链路名称
        /// </summary>
        [AdvQueryAttribute(ColName = "LinkName",ColDesc = "链路名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "LinkName" ,Length=100,IsNullable = true,ColumnDescription = "链路名称" )]
        public string LinkName
        { 
            get{return _LinkName;}
            set{
            SetProperty(ref _LinkName, value);
                        }
        }

        private string _Description;
        /// <summary>
        /// 链路描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Description",ColDesc = "链路描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Description" ,Length=500,IsNullable = true,ColumnDescription = "链路描述" )]
        public string Description
        { 
            get{return _Description;}
            set{
            SetProperty(ref _Description, value);
                        }
        }

        private int? _SourceType;
        /// <summary>
        /// 提醒源类型
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceType",ColDesc = "提醒源类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "SourceType" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "提醒源类型" )]
        public int? SourceType
        { 
            get{return _SourceType;}
            set{
            SetProperty(ref _SourceType, value);
                        }
        }

        private long? _SourceValue;
        /// <summary>
        /// 提醒源值
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceValue",ColDesc = "提醒源值")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "SourceValue" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "提醒源值" )]
        public long? SourceValue
        { 
            get{return _SourceValue;}
            set{
            SetProperty(ref _SourceValue, value);
                        }
        }

        private long? _TargetValue;
        /// <summary>
        /// 提醒目标值
        /// </summary>
        [AdvQueryAttribute(ColName = "TargetValue",ColDesc = "提醒目标值")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "TargetValue" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "提醒目标值" )]
        public long? TargetValue
        { 
            get{return _TargetValue;}
            set{
            SetProperty(ref _TargetValue, value);
                        }
        }

        private int? _ActionType;
        /// <summary>
        /// 操作类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ActionType",ColDesc = "操作类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ActionType" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "操作类型" )]
        public int? ActionType
        { 
            get{return _ActionType;}
            set{
            SetProperty(ref _ActionType, value);
                        }
        }

        private int? _TargetType;
        /// <summary>
        /// 提醒目标类型
        /// </summary>
        [AdvQueryAttribute(ColName = "TargetType",ColDesc = "提醒目标类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "TargetType" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "提醒目标类型" )]
        public int? TargetType
        { 
            get{return _TargetType;}
            set{
            SetProperty(ref _TargetType, value);
                        }
        }

        private int? _BizType;
        /// <summary>
        /// 单据类型
        /// </summary>
        [AdvQueryAttribute(ColName = "BizType",ColDesc = "单据类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "BizType" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "单据类型" )]
        public int? BizType
        { 
            get{return _BizType;}
            set{
            SetProperty(ref _BizType, value);
                        }
        }

        private int? _BillStatus;
        /// <summary>
        /// 单据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "BillStatus",ColDesc = "单据状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "BillStatus" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "单据状态" )]
        public int? BillStatus
        { 
            get{return _BillStatus;}
            set{
            SetProperty(ref _BillStatus, value);
                        }
        }

        private bool? _IsEnabled;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "IsEnabled",ColDesc = "是否启用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsEnabled" ,IsNullable = true,ColumnDescription = "是否启用" )]
        public bool? IsEnabled
        { 
            get{return _IsEnabled;}
            set{
            SetProperty(ref _IsEnabled, value);
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


        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ReminderLinkRuleRelation.LinkId))]
        public virtual List<tb_ReminderLinkRuleRelation> tb_ReminderLinkRuleRelations { get; set; }


        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_ReminderObjectLink loctype = (tb_ReminderObjectLink)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

