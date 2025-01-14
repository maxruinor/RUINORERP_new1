
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:41
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
    /// 业务类型 报销，员工借支还款，运费
    /// </summary>
    [Serializable()]
    [Description("tb_FM_ExpenseType")]
    [SugarTable("tb_FM_ExpenseType")]
    public partial class tb_FM_ExpenseType: BaseEntity, ICloneable
    {
        public tb_FM_ExpenseType()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_FM_ExpenseType" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ExpenseType_id;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ExpenseType_id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long ExpenseType_id
        { 
            get{return _ExpenseType_id;}
            set{
            base.PrimaryKeyID = _ExpenseType_id;
            SetProperty(ref _ExpenseType_id, value);
            }
        }

        private long? _subject_id;
        /// <summary>
        /// 科目
        /// </summary>
        [AdvQueryAttribute(ColName = "subject_id",ColDesc = "科目")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "subject_id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "科目" )]
        [FKRelationAttribute("tb_FM_Subject","subject_id")]
        public long? subject_id
        { 
            get{return _subject_id;}
            set{
            SetProperty(ref _subject_id, value);
            }
        }

        private string _Expense_name;
        /// <summary>
        /// 费用业务名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Expense_name",ColDesc = "费用业务名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Expense_name" ,Length=50,IsNullable = true,ColumnDescription = "费用业务名称" )]
        public string Expense_name
        { 
            get{return _Expense_name;}
            set{
            SetProperty(ref _Expense_name, value);
            }
        }

        private bool _EXPOrINC= true;
        /// <summary>
        /// 收支标识
        /// </summary>
        [AdvQueryAttribute(ColName = "EXPOrINC",ColDesc = "收支标识")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "EXPOrINC" ,IsNullable = false,ColumnDescription = "收支标识" )]
        public bool EXPOrINC
        { 
            get{return _EXPOrINC;}
            set{
            SetProperty(ref _EXPOrINC, value);
            }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=30,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(subject_id))]
        public virtual tb_FM_Subject tb_fm_subject { get; set; }


        [Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_OtherExpenseDetail.ExpenseType_id))]
        public virtual List<tb_FM_OtherExpenseDetail> tb_FM_OtherExpenseDetails { get; set; }
        //tb_FM_OtherExpenseDetail.ExpenseType_id)
        //ExpenseType_id.FK_TB_FM_OT_REFERENCE_TB_FM_EX)
        //tb_FM_ExpenseType.ExpenseType_id)

        [Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_Initial_PayAndReceivable.ExpenseType_id))]
        public virtual List<tb_FM_Initial_PayAndReceivable> tb_FM_Initial_PayAndReceivables { get; set; }
        //tb_FM_Initial_PayAndReceivable.ExpenseType_id)
        //ExpenseType_id.FK_TB_FM_IN_REFERENCE_TB_FM_EX)
        //tb_FM_ExpenseType.ExpenseType_id)

        [Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_ExpenseClaimDetail.ExpenseType_id))]
        public virtual List<tb_FM_ExpenseClaimDetail> tb_FM_ExpenseClaimDetails { get; set; }
        //tb_FM_ExpenseClaimDetail.ExpenseType_id)
        //ExpenseType_id.FK_EXPENSECLAIMDETAIL_REF_EXPENSETYPE)
        //tb_FM_ExpenseType.ExpenseType_id)


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
                    Type type = typeof(tb_FM_ExpenseType);
                    
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
            tb_FM_ExpenseType loctype = (tb_FM_ExpenseType)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

