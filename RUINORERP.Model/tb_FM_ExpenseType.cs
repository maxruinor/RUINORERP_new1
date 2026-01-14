
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:41:48
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
    [Description("业务类型")]
    [SugarTable("tb_FM_ExpenseType")]
    public partial class tb_FM_ExpenseType: BaseEntity, ICloneable
    {
        public tb_FM_ExpenseType()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("业务类型 报销，员工借支还款，运费tb_FM_ExpenseType" + "外键ID与对应主主键名称不一致。请修改数据库");
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
            SetProperty(ref _ExpenseType_id, value);
                base.PrimaryKeyID = _ExpenseType_id;
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

 

        private int _ReceivePaymentType;
        /// <summary>
        /// 收付类型
        /// </summary>
        [AdvQueryAttribute(ColName = nameof(ReceivePaymentType),ColDesc = "收付类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = nameof(ReceivePaymentType) , DecimalDigits = 0,IsNullable = false,ColumnDescription = "收付类型" )]
        public int ReceivePaymentType
        { 
            get{return _ReceivePaymentType;}
            set{
            SetProperty(ref _ReceivePaymentType, value);
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
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(subject_id))]
        public virtual tb_FM_Subject tb_fm_subject { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_OtherExpenseDetail.ExpenseType_id))]
        public virtual List<tb_FM_OtherExpenseDetail> tb_FM_OtherExpenseDetails { get; set; }
        //tb_FM_OtherExpenseDetail.ExpenseType_id)
        //ExpenseType_id.FK_TB_FM_OT_REFERENCE_TB_FM_EX)
        //tb_FM_ExpenseType.ExpenseType_id)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_ExpenseClaimDetail.ExpenseType_id))]
        public virtual List<tb_FM_ExpenseClaimDetail> tb_FM_ExpenseClaimDetails { get; set; }
        //tb_FM_ExpenseClaimDetail.ExpenseType_id)
        //ExpenseType_id.FK_EXPENSECLAIMDETAIL_REF_EXPENSETYPE)
        //tb_FM_ExpenseType.ExpenseType_id)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_ReceivablePayableDetail.ExpenseType_id))]
        public virtual List<tb_FM_ReceivablePayableDetail> tb_FM_ReceivablePayableDetails { get; set; }
        //tb_FM_ReceivablePayableDetail.ExpenseType_id)
        //ExpenseType_id.FK_TB_FM_RECEIVABLEPAYABLEDETAIL_REF_FM_EXPENSETYPE)
        //tb_FM_ExpenseType.ExpenseType_id)


        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
         if("Subject_id"!="subject_id")
        {
        // rs=false;
        }
return rs;
}






       
        

        public override object Clone()
        {
            tb_FM_ExpenseType loctype = (tb_FM_ExpenseType)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

