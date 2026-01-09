
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:42:13
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
    /// 行级权限规则
    /// </summary>
    [Serializable()]
    [Description("行级权限规则")]
    [SugarTable("tb_RowAuthPolicy")]
    public partial class tb_RowAuthPolicy: BaseEntity, ICloneable
    {
        public tb_RowAuthPolicy()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("行级权限规则tb_RowAuthPolicy" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PolicyId;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PolicyId" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long PolicyId
        { 
            get{return _PolicyId;}
            set{
            SetProperty(ref _PolicyId, value);
                base.PrimaryKeyID = _PolicyId;
            }
        }

        private string _PolicyName;
        /// <summary>
        /// 规则名称
        /// </summary>
        [AdvQueryAttribute(ColName = "PolicyName",ColDesc = "规则名称")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "PolicyName" ,Length=100,IsNullable = false,ColumnDescription = "规则名称" )]
        public string PolicyName
        { 
            get{return _PolicyName;}
            set{
            SetProperty(ref _PolicyName, value);
                        }
        }

        private string _TargetTable;
        /// <summary>
        /// 查询主表
        /// </summary>
        [AdvQueryAttribute(ColName = "TargetTable",ColDesc = "查询主表")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "TargetTable" ,Length=100,IsNullable = false,ColumnDescription = "查询主表" )]
        public string TargetTable
        { 
            get{return _TargetTable;}
            set{
            SetProperty(ref _TargetTable, value);
                        }
        }

        private string _TargetEntity;
        /// <summary>
        /// 查询实体
        /// </summary>
        [AdvQueryAttribute(ColName = "TargetEntity",ColDesc = "查询实体")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "TargetEntity" ,Length=100,IsNullable = false,ColumnDescription = "查询实体" )]
        public string TargetEntity
        { 
            get{return _TargetEntity;}
            set{
            SetProperty(ref _TargetEntity, value);
                        }
        }

        private bool? _IsJoinRequired;
        /// <summary>
        /// 是否需要联表
        /// </summary>
        [AdvQueryAttribute(ColName = "IsJoinRequired",ColDesc = "是否需要联表")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsJoinRequired" ,IsNullable = true,ColumnDescription = "是否需要联表" )]
        public bool? IsJoinRequired
        { 
            get{return _IsJoinRequired;}
            set{
            SetProperty(ref _IsJoinRequired, value);
                        }
        }

        private string _TargetTableJoinField;
        /// <summary>
        /// 目标表关联字段
        /// </summary>
        [AdvQueryAttribute(ColName = "TargetTableJoinField",ColDesc = "目标表关联字段")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "TargetTableJoinField" ,Length=50,IsNullable = true,ColumnDescription = "目标表关联字段" )]
        public string TargetTableJoinField
        { 
            get{return _TargetTableJoinField;}
            set{
            SetProperty(ref _TargetTableJoinField, value);
                        }
        }

        private string _JoinTableJoinField;
        /// <summary>
        /// 关联表关联字段
        /// </summary>
        [AdvQueryAttribute(ColName = "JoinTableJoinField",ColDesc = "关联表关联字段")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "JoinTableJoinField" ,Length=50,IsNullable = true,ColumnDescription = "关联表关联字段" )]
        public string JoinTableJoinField
        { 
            get{return _JoinTableJoinField;}
            set{
            SetProperty(ref _JoinTableJoinField, value);
                        }
        }

        private string _JoinTable;
        /// <summary>
        /// 需要关联的表名
        /// </summary>
        [AdvQueryAttribute(ColName = "JoinTable",ColDesc = "需要关联的表名")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "JoinTable" ,Length=100,IsNullable = true,ColumnDescription = "需要关联的表名" )]
        public string JoinTable
        { 
            get{return _JoinTable;}
            set{
            SetProperty(ref _JoinTable, value);
                        }
        }

        private string _JoinType;
        /// <summary>
        /// 关联类型
        /// </summary>
        [AdvQueryAttribute(ColName = "JoinType",ColDesc = "关联类型")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "JoinType" ,Length=10,IsNullable = true,ColumnDescription = "关联类型" )]
        public string JoinType
        { 
            get{return _JoinType;}
            set{
            SetProperty(ref _JoinType, value);
                        }
        }

        private string _JoinOnClause;
        /// <summary>
        /// 关联条件
        /// </summary>
        [AdvQueryAttribute(ColName = "JoinOnClause",ColDesc = "关联条件")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "JoinOnClause" ,Length=500,IsNullable = true,ColumnDescription = "关联条件" )]
        public string JoinOnClause
        { 
            get{return _JoinOnClause;}
            set{
            SetProperty(ref _JoinOnClause, value);
                        }
        }

        private string _FilterClause;
        /// <summary>
        /// 过滤条件(静态条件)
        /// </summary>
        [AdvQueryAttribute(ColName = "FilterClause",ColDesc = "过滤条件")]
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "FilterClause" ,Length=1000,IsNullable = true,ColumnDescription = "过滤条件" )]
        public string FilterClause
        {
            get{return _FilterClause;}
            set{
            SetProperty(ref _FilterClause, value);
                        }
        }

        private bool _IsParameterized = false;
        /// <summary>
        /// 是否使用参数化过滤条件
        /// </summary>
        [AdvQueryAttribute(ColName = "IsParameterized",ColDesc = "是否参数化")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsParameterized" ,IsNullable = false,ColumnDescription = "是否参数化")]
        public bool IsParameterized
        {
            get{return _IsParameterized;}
            set{
            SetProperty(ref _IsParameterized, value);
                        }
        }

        private string _ParameterizedFilterClause;
        /// <summary>
        /// 参数化过滤条件模板
        /// 支持占位符: {UserId}, {EmployeeId}, {RoleId}等
        /// </summary>
        [AdvQueryAttribute(ColName = "ParameterizedFilterClause",ColDesc = "参数化过滤条件")]
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "ParameterizedFilterClause" ,Length=1000,IsNullable = true,ColumnDescription = "参数化过滤条件" )]
        public string ParameterizedFilterClause
        {
            get{return _ParameterizedFilterClause;}
            set{
            SetProperty(ref _ParameterizedFilterClause, value);
                        }
        }

        private string _EntityType;
        /// <summary>
        /// 实体的全限定类名
        /// </summary>
        [AdvQueryAttribute(ColName = "EntityType",ColDesc = "实体的全限定类名")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "EntityType" ,Length=200,IsNullable = true,ColumnDescription = "实体的全限定类名" )]
        public string EntityType
        { 
            get{return _EntityType;}
            set{
            SetProperty(ref _EntityType, value);
                        }
        }

        private bool _IsEnabled= true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "IsEnabled",ColDesc = "是否启用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsEnabled" ,IsNullable = false,ColumnDescription = "是否启用" )]
        public bool IsEnabled
        { 
            get{return _IsEnabled;}
            set{
            SetProperty(ref _IsEnabled, value);
                        }
        }

        private string _PolicyDescription;
        /// <summary>
        /// 规则描述
        /// </summary>
        [AdvQueryAttribute(ColName = "PolicyDescription",ColDesc = "规则描述")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "PolicyDescription" ,Length=500,IsNullable = true,ColumnDescription = "规则描述" )]
        public string PolicyDescription
        { 
            get{return _PolicyDescription;}
            set{
            SetProperty(ref _PolicyDescription, value);
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

        private int? _DefaultRuleEnum;
        /// <summary>
        /// 默认规则
        /// </summary>
        [AdvQueryAttribute(ColName = "DefaultRuleEnum",ColDesc = "默认规则")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DefaultRuleEnum" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "默认规则" )]
        public int? DefaultRuleEnum
        { 
            get{return _DefaultRuleEnum;}
            set{
            SetProperty(ref _DefaultRuleEnum, value);
                        }
        }

        #endregion

        #region 扩展属性

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_P4RowAuthPolicyByRole.PolicyId))]
        public virtual List<tb_P4RowAuthPolicyByRole> tb_P4RowAuthPolicyByRoles { get; set; }
        //tb_P4RowAuthPolicyByRole.PolicyId)
        //PolicyId.FK_TB_P4ROW_REFERENCE_TB_ROWAU)
        //tb_RowAuthPolicy.PolicyId)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_P4RowAuthPolicyByUser.PolicyId))]
        public virtual List<tb_P4RowAuthPolicyByUser> tb_P4RowAuthPolicyByUsers { get; set; }
        //tb_P4RowAuthPolicyByUser.PolicyId)
        //PolicyId.FK_TB_P4ROWAUTHPOLICYBYUSER_REF_TB_ROWAUTHPOLICY)
        //tb_RowAuthPolicy.PolicyId)


        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_RowAuthPolicy loctype = (tb_RowAuthPolicy)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

