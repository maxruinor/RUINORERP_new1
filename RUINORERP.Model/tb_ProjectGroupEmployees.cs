
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/24/2025 14:14:55
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
    /// 项目及成员关系表
    /// </summary>
    [Serializable()]
    [Description("项目及成员关系表")]
    [SugarTable("tb_ProjectGroupEmployees")]
    public partial class tb_ProjectGroupEmployees: BaseEntity, ICloneable
    {
        public tb_ProjectGroupEmployees()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("项目及成员关系表tb_ProjectGroupEmployees" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ProjectGroupEmpID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProjectGroupEmpID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long ProjectGroupEmpID
        { 
            get{return _ProjectGroupEmpID;}
            set{
            SetProperty(ref _ProjectGroupEmpID, value);
                base.PrimaryKeyID = _ProjectGroupEmpID;
            }
        }

        private long _ProjectGroup_ID;
        /// <summary>
        /// 项目组
        /// </summary>
        [AdvQueryAttribute(ColName = "ProjectGroup_ID",ColDesc = "项目组")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProjectGroup_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "项目组" )]
        [FKRelationAttribute("tb_ProjectGroup","ProjectGroup_ID")]
        public long ProjectGroup_ID
        { 
            get{return _ProjectGroup_ID;}
            set{
            SetProperty(ref _ProjectGroup_ID, value);
                        }
        }

        private long _Employee_ID;
        /// <summary>
        /// 员工
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "员工")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "员工" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
                        }
        }

        private bool _Assigned= false;
        /// <summary>
        /// 已分配
        /// </summary>
        [AdvQueryAttribute(ColName = "Assigned",ColDesc = "已分配")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Assigned" ,IsNullable = false,ColumnDescription = "已分配" )]
        public bool Assigned
        { 
            get{return _Assigned;}
            set{
            SetProperty(ref _Assigned, value);
                        }
        }

        private bool _DefaultGroup= false;
        /// <summary>
        /// 默认组
        /// </summary>
        [AdvQueryAttribute(ColName = "DefaultGroup",ColDesc = "默认组")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "DefaultGroup" ,IsNullable = false,ColumnDescription = "默认组" )]
        public bool DefaultGroup
        { 
            get{return _DefaultGroup;}
            set{
            SetProperty(ref _DefaultGroup, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProjectGroup_ID))]
        public virtual tb_ProjectGroup tb_projectgroup { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}


 
        public override object Clone()
        {
            tb_ProjectGroupEmployees loctype = (tb_ProjectGroupEmployees)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

