
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:58:10
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
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("会计科目表，财务系统中使用tb_FM_Subject" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Subject_id;
        /// <summary>
        /// 会计科目
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Subject_id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "会计科目" , IsPrimaryKey = true)]
        public long Subject_id
        { 
            get{return _Subject_id;}
            set{
            SetProperty(ref _Subject_id, value);
                base.PrimaryKeyID = _Subject_id;
            }
        }

        private long? _Parent_subject_id;
        /// <summary>
        /// 上级科目
        /// </summary>
        [AdvQueryAttribute(ColName = "Parent_subject_id",ColDesc = "上级科目")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Parent_subject_id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "上级科目" )]
        [FKRelationAttribute("tb_FM_Subject","Parent_subject_id")]
        public long? Parent_subject_id
        { 
            get{return _Parent_subject_id;}
            set{
            SetProperty(ref _Parent_subject_id, value);
                        }
        }

        private string _Subject_code;
        /// <summary>
        /// 科目代码
        /// </summary>
        [AdvQueryAttribute(ColName = "Subject_code",ColDesc = "科目代码")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Subject_code" ,Length=50,IsNullable = false,ColumnDescription = "科目代码" )]
        public string Subject_code
        { 
            get{return _Subject_code;}
            set{
            SetProperty(ref _Subject_code, value);
                        }
        }

        private string _Subject_name;
        /// <summary>
        /// 科目名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Subject_name",ColDesc = "科目名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Subject_name" ,Length=100,IsNullable = false,ColumnDescription = "科目名称" )]
        public string Subject_name
        { 
            get{return _Subject_name;}
            set{
            SetProperty(ref _Subject_name, value);
                        }
        }

        private string _Subject_en_name;
        /// <summary>
        /// 英文名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Subject_en_name",ColDesc = "英文名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Subject_en_name" ,Length=100,IsNullable = true,ColumnDescription = "英文名称" )]
        public string Subject_en_name
        { 
            get{return _Subject_en_name;}
            set{
            SetProperty(ref _Subject_en_name, value);
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

        private bool _Balance_direction= true;
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
        /// 科目图片
        /// </summary>
        [AdvQueryAttribute(ColName = "Images",ColDesc = "科目图片")] 
        [SugarColumn(ColumnDataType = "image", SqlParameterDbType ="Binary",  ColumnName = "Images" ,Length=2147483647,IsNullable = true,ColumnDescription = "科目图片" )]
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

        private bool _isdeleted= false;
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "isdeleted" ,IsNullable = false,ColumnDescription = "逻辑删除" )]
        [Browsable(false)]
        public bool isdeleted
        { 
            get{return _isdeleted;}
            set{
            SetProperty(ref _isdeleted, value);
                        }
        }

        #endregion

        #region 扩展属性
        

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_ExpenseClaimDetail.Subject_id))]
        public virtual List<tb_FM_ExpenseClaimDetail> tb_FM_ExpenseClaimDetails { get; set; }
        //tb_FM_ExpenseClaimDetail.Subject_id)
        //Subject_id.FK_EXPENSECLAIMDETAIL_REF_SUBJECT)
        //tb_FM_Subject.Subject_id)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_GeneralLedger.Subject_id))]
        public virtual List<tb_FM_GeneralLedger> tb_FM_GeneralLedgers { get; set; }
        //tb_FM_GeneralLedger.Subject_id)
        //Subject_id.FK_FM_GENERALLEDGER_REF_FM_SUBJECT)
        //tb_FM_Subject.Subject_id)

       

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_ExpenseType.subject_id))]
        public virtual List<tb_FM_ExpenseType> tb_FM_ExpenseTypes { get; set; }
        //tb_FM_ExpenseType.Subject_id)
        //Subject_id.FK_TB_FM_EX_REFERENCE_TB_FM_SU)
        //tb_FM_Subject.subject_id)


   

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_OtherExpenseDetail.Subject_id))]
        public virtual List<tb_FM_OtherExpenseDetail> tb_FM_OtherExpenseDetails { get; set; }
        //tb_FM_OtherExpenseDetail.Subject_id)
        //Subject_id.FK_TB_FM_OT_REFERENCE_TB_FM_SU)
        //tb_FM_Subject.Subject_id)


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
         if("Subject_id"!="Parent_subject_id")
        {
        // rs=false;
        }
return rs;
}





        

        public override object Clone()
        {
            tb_FM_Subject loctype = (tb_FM_Subject)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

