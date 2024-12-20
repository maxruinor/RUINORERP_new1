
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 18:16:35
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
    /// 项目组信息 用于业务分组小团队
    /// </summary>
    [Serializable()]
    [Description("项目组信息 用于业务分组小团队")]
    [SugarTable("tb_ProjectGroup")]
    public partial class tb_ProjectGroup: BaseEntity, ICloneable
    {
        public tb_ProjectGroup()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("项目组信息 用于业务分组小团队tb_ProjectGroup" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ProjectGroup_ID;
        /// <summary>
        /// 项目组
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProjectGroup_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "项目组" , IsPrimaryKey = true)]
        public long ProjectGroup_ID
        { 
            get{return _ProjectGroup_ID;}
            set{
            base.PrimaryKeyID = _ProjectGroup_ID;
            SetProperty(ref _ProjectGroup_ID, value);
            }
        }

        private long? _DepartmentID;
        /// <summary>
        /// 部门
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "部门")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "部门" )]
        [FKRelationAttribute("tb_Department","DepartmentID")]
        public long? DepartmentID
        { 
            get{return _DepartmentID;}
            set{
            SetProperty(ref _DepartmentID, value);
            }
        }

        private string _ProjectGroupCode;
        /// <summary>
        /// 项目组代号
        /// </summary>
        [AdvQueryAttribute(ColName = "ProjectGroupCode",ColDesc = "项目组代号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ProjectGroupCode" ,Length=50,IsNullable = true,ColumnDescription = "项目组代号" )]
        public string ProjectGroupCode
        { 
            get{return _ProjectGroupCode;}
            set{
            SetProperty(ref _ProjectGroupCode, value);
            }
        }

        private string _ProjectGroupName;
        /// <summary>
        /// 项目组名称
        /// </summary>
        [AdvQueryAttribute(ColName = "ProjectGroupName",ColDesc = "项目组名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ProjectGroupName" ,Length=50,IsNullable = true,ColumnDescription = "项目组名称" )]
        public string ProjectGroupName
        { 
            get{return _ProjectGroupName;}
            set{
            SetProperty(ref _ProjectGroupName, value);
            }
        }

        private string _ResponsiblePerson;
        /// <summary>
        /// 负责人
        /// </summary>
        [AdvQueryAttribute(ColName = "ResponsiblePerson",ColDesc = "负责人")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ResponsiblePerson" ,Length=50,IsNullable = true,ColumnDescription = "负责人" )]
        public string ResponsiblePerson
        { 
            get{return _ResponsiblePerson;}
            set{
            SetProperty(ref _ResponsiblePerson, value);
            }
        }

        private string _Phone;
        /// <summary>
        /// 电话
        /// </summary>
        [AdvQueryAttribute(ColName = "Phone",ColDesc = "电话")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Phone" ,Length=255,IsNullable = true,ColumnDescription = "电话" )]
        public string Phone
        { 
            get{return _Phone;}
            set{
            SetProperty(ref _Phone, value);
            }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=255,IsNullable = true,ColumnDescription = "备注" )]
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

        private DateTime? _StartDate;
        /// <summary>
        /// 启动时间
        /// </summary>
        [AdvQueryAttribute(ColName = "StartDate",ColDesc = "启动时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "StartDate" ,IsNullable = true,ColumnDescription = "启动时间" )]
        public DateTime? StartDate
        { 
            get{return _StartDate;}
            set{
            SetProperty(ref _StartDate, value);
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
        /// 结束时间
        /// </summary>
        [AdvQueryAttribute(ColName = "EndDate",ColDesc = "结束时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "EndDate" ,IsNullable = true,ColumnDescription = "结束时间" )]
        public DateTime? EndDate
        { 
            get{return _EndDate;}
            set{
            SetProperty(ref _EndDate, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(DepartmentID))]
        public virtual tb_Department tb_department { get; set; }


        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_OtherExpenseDetail.ProjectGroup_ID))]
        public virtual List<tb_FM_OtherExpenseDetail> tb_FM_OtherExpenseDetails { get; set; }
        //tb_FM_OtherExpenseDetail.ProjectGroup_ID)
        //ProjectGroup_ID.FK_OTHEREXPENSEDETAIL_REF_PROJEGROUP)
        //tb_ProjectGroup.ProjectGroup_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MaterialRequisition.ProjectGroup_ID))]
        public virtual List<tb_MaterialRequisition> tb_MaterialRequisitions { get; set; }
        //tb_MaterialRequisition.ProjectGroup_ID)
        //ProjectGroup_ID.FK_MATEREQUISITIONS_REF_PROJECTGROUP)
        //tb_ProjectGroup.ProjectGroup_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOut.ProjectGroup_ID))]
        public virtual List<tb_SaleOut> tb_SaleOuts { get; set; }
        //tb_SaleOut.ProjectGroup_ID)
        //ProjectGroup_ID.FK_TB_SALEOUT_REF_PROJECTGROUP)
        //tb_ProjectGroup.ProjectGroup_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProductionPlan.ProjectGroup_ID))]
        public virtual List<tb_ProductionPlan> tb_ProductionPlans { get; set; }
        //tb_ProductionPlan.ProjectGroup_ID)
        //ProjectGroup_ID.FK_TB_PRODUPLAN_REF_PROJECTGROUP)
        //tb_ProjectGroup.ProjectGroup_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_ExpenseClaimDetail.ProjectGroup_ID))]
        public virtual List<tb_FM_ExpenseClaimDetail> tb_FM_ExpenseClaimDetails { get; set; }
        //tb_FM_ExpenseClaimDetail.ProjectGroup_ID)
        //ProjectGroup_ID.FK_EXPENSECLAIMDETAIL_REF_PROJECTGROUP)
        //tb_ProjectGroup.ProjectGroup_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOrder.ProjectGroup_ID))]
        public virtual List<tb_SaleOrder> tb_SaleOrders { get; set; }
        //tb_SaleOrder.ProjectGroup_ID)
        //ProjectGroup_ID.FK_SALEORDER_REF_PROJECTGROUP)
        //tb_ProjectGroup.ProjectGroup_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOutRe.ProjectGroup_ID))]
        public virtual List<tb_SaleOutRe> tb_SaleOutRes { get; set; }
        //tb_SaleOutRe.ProjectGroup_ID)
        //ProjectGroup_ID.FK_SALEOUTRE_REF_PROJECTGROUP)
        //tb_ProjectGroup.ProjectGroup_ID)


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
                    Type type = typeof(tb_ProjectGroup);
                    
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
            tb_ProjectGroup loctype = (tb_ProjectGroup)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

