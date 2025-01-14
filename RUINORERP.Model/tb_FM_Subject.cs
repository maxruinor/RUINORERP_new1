
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/14/2025 18:56:54
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
    /// 会计科目表，财务系统中使用
    /// </summary>
    [Serializable()]
    [Description("会计科目表，财务系统中使用")]
    [SugarTable("tb_FM_Subject")]
    public partial class tb_FM_Subject: BaseEntity, ICloneable
    {
        public tb_FM_Subject()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("会计科目表，财务系统中使用tb_FM_Subject" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _subject_id;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "subject_id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long subject_id
        { 
            get{return _subject_id;}
            set{
            base.PrimaryKeyID = _subject_id;
            SetProperty(ref _subject_id, value);
            }
        }

        private long? _parent_subject_id;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "parent_subject_id",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "parent_subject_id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        public long? parent_subject_id
        { 
            get{return _parent_subject_id;}
            set{
            SetProperty(ref _parent_subject_id, value);
            }
        }

        private string _subject_code;
        /// <summary>
        /// 科目代码
        /// </summary>
        [AdvQueryAttribute(ColName = "subject_code",ColDesc = "科目代码")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "subject_code" ,Length=50,IsNullable = false,ColumnDescription = "科目代码" )]
        public string subject_code
        { 
            get{return _subject_code;}
            set{
            SetProperty(ref _subject_code, value);
            }
        }

        private string _subject_name;
        /// <summary>
        /// 科目名称
        /// </summary>
        [AdvQueryAttribute(ColName = "subject_name",ColDesc = "科目名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "subject_name" ,Length=100,IsNullable = false,ColumnDescription = "科目名称" )]
        public string subject_name
        { 
            get{return _subject_name;}
            set{
            SetProperty(ref _subject_name, value);
            }
        }

        private string _subject_en_name;
        /// <summary>
        /// 科目名称
        /// </summary>
        [AdvQueryAttribute(ColName = "subject_en_name",ColDesc = "科目名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "subject_en_name" ,Length=100,IsNullable = true,ColumnDescription = "科目名称" )]
        public string subject_en_name
        { 
            get{return _subject_en_name;}
            set{
            SetProperty(ref _subject_en_name, value);
            }
        }

        private int _Subject_Type;
        /// <summary>
        /// 科目类型
        /// </summary>
        [AdvQueryAttribute(ColName = "Subject_Type",ColDesc = "科目类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Subject_Type" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "科目类型" )]
        public int Subject_Type
        { 
            get{return _Subject_Type;}
            set{
            SetProperty(ref _Subject_Type, value);
            }
        }

        private bool _Balance_direction;
        /// <summary>
        /// 余额方向
        /// </summary>
        [AdvQueryAttribute(ColName = "Balance_direction",ColDesc = "余额方向")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Balance_direction" ,IsNullable = false,ColumnDescription = "余额方向" )]
        public bool Balance_direction
        { 
            get{return _Balance_direction;}
            set{
            SetProperty(ref _Balance_direction, value);
            }
        }

        private bool? _Is_enabled= true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_enabled",ColDesc = "是否启用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Is_enabled" ,IsNullable = true,ColumnDescription = "是否启用" )]
        public bool? Is_enabled
        { 
            get{return _Is_enabled;}
            set{
            SetProperty(ref _Is_enabled, value);
            }
        }

        private DateTime? _EndDate;
        /// <summary>
        /// 离职日期
        /// </summary>
        [AdvQueryAttribute(ColName = "EndDate",ColDesc = "离职日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "EndDate" ,IsNullable = true,ColumnDescription = "离职日期" )]
        public DateTime? EndDate
        { 
            get{return _EndDate;}
            set{
            SetProperty(ref _EndDate, value);
            }
        }

        private int? _Sort;
        /// <summary>
        /// 排序
        /// </summary>
        [AdvQueryAttribute(ColName = "Sort",ColDesc = "排序")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Sort" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "排序" )]
        public int? Sort
        { 
            get{return _Sort;}
            set{
            SetProperty(ref _Sort, value);
            }
        }

        private byte[] _Images;
        /// <summary>
        /// 类目图片
        /// </summary>
        [AdvQueryAttribute(ColName = "Images",ColDesc = "类目图片")] 
        [SugarColumn(ColumnDataType = "image", SqlParameterDbType ="Binary",  ColumnName = "Images" ,Length=2147483647,IsNullable = true,ColumnDescription = "类目图片" )]
        public byte[] Images
        { 
            get{return _Images;}
            set{
            SetProperty(ref _Images, value);
            }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=200,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
            }
        }

        #endregion

        #region 扩展属性

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_OtherExpenseDetail.Subject_id))]
        public virtual List<tb_FM_OtherExpenseDetail> tb_FM_OtherExpenseDetails { get; set; }
        //tb_FM_OtherExpenseDetail.subject_id)
        //subject_id.FK_TB_FM_OT_REFERENCE_TB_FM_SU)
        //tb_FM_Subject.Subject_id)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_ExpenseClaimDetail.Subject_id))]
        public virtual List<tb_FM_ExpenseClaimDetail> tb_FM_ExpenseClaimDetails { get; set; }
        //tb_FM_ExpenseClaimDetail.subject_id)
        //subject_id.FK_EXPENSECLAIMDETAIL_REF_SUBJECT)
        //tb_FM_Subject.Subject_id)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_Account.Subject_id))]
        public virtual List<tb_FM_Account> tb_FM_Accounts { get; set; }
        //tb_FM_Account.subject_id)
        //subject_id.FK_TB_FM_AC_REFERENCE_TB_FM_SU)
        //tb_FM_Subject.Subject_id)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_ExpenseType.subject_id))]
        public virtual List<tb_FM_ExpenseType> tb_FM_ExpenseTypes { get; set; }
        //tb_FM_ExpenseType.subject_id)
        //subject_id.FK_TB_FM_EX_REFERENCE_TB_FM_SU)
        //tb_FM_Subject.subject_id)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_Initial_PayAndReceivable.subject_id))]
        public virtual List<tb_FM_Initial_PayAndReceivable> tb_FM_Initial_PayAndReceivables { get; set; }
        //tb_FM_Initial_PayAndReceivable.subject_id)
        //subject_id.FK_TB_FM_IN_REFERENCE_TB_FM_SU)
        //tb_FM_Subject.subject_id)


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
                    Type type = typeof(tb_FM_Subject);
                    
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
            tb_FM_Subject loctype = (tb_FM_Subject)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

