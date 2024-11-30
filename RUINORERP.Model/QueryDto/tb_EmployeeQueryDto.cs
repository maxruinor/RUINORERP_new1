
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/29/2024 23:20:19
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model.Base;

namespace RUINORERP.Model.QueryDto
{
    /// <summary>
    /// 员工表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Employee")]
    public partial class tb_EmployeeQueryDto:BaseEntityDto
    {
        public tb_EmployeeQueryDto()
        {

        }

    
     

        private long _DepartmentID;
        /// <summary>
        /// 部门
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "部门")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "DepartmentID",IsNullable = false,ColumnDescription = "部门" )]
        [FKRelationAttribute("tb_Department","DepartmentID")]
        public long DepartmentID 
        { 
            get{return _DepartmentID;}
            set{SetProperty(ref _DepartmentID, value);}
        }
     

        private string _Employee_NO;
        /// <summary>
        /// 员工编号
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_NO",ColDesc = "员工编号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Employee_NO",Length=20,IsNullable = false,ColumnDescription = "员工编号" )]
        public string Employee_NO 
        { 
            get{return _Employee_NO;}
            set{SetProperty(ref _Employee_NO, value);}
        }
     

        private string _Employee_Name;
        /// <summary>
        /// 姓名
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_Name",ColDesc = "姓名")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Employee_Name",Length=100,IsNullable = false,ColumnDescription = "姓名" )]
        public string Employee_Name 
        { 
            get{return _Employee_Name;}
            set{SetProperty(ref _Employee_Name, value);}
        }
     

        private bool? _Gender;
        /// <summary>
        /// 性别
        /// </summary>
        [AdvQueryAttribute(ColName = "Gender",ColDesc = "性别")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Gender",IsNullable = true,ColumnDescription = "性别" )]
        public bool? Gender 
        { 
            get{return _Gender;}
            set{SetProperty(ref _Gender, value);}
        }
     

        private string _Position;
        /// <summary>
        /// 职位
        /// </summary>
        [AdvQueryAttribute(ColName = "Position",ColDesc = "职位")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Position",Length=20,IsNullable = true,ColumnDescription = "职位" )]
        public string Position 
        { 
            get{return _Position;}
            set{SetProperty(ref _Position, value);}
        }
     

        private int? _Marriage;
        /// <summary>
        /// 婚姻状况
        /// </summary>
        [AdvQueryAttribute(ColName = "Marriage",ColDesc = "婚姻状况")]
        [SugarColumn(ColumnDataType = "tinyint",SqlParameterDbType ="SByte",ColumnName = "Marriage",IsNullable = true,ColumnDescription = "婚姻状况" )]
        public int? Marriage 
        { 
            get{return _Marriage;}
            set{SetProperty(ref _Marriage, value);}
        }
     

        private DateTime? _Birthday;
        /// <summary>
        /// 生日
        /// </summary>
        [AdvQueryAttribute(ColName = "Birthday",ColDesc = "生日")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Birthday",IsNullable = true,ColumnDescription = "生日" )]
        public DateTime? Birthday 
        { 
            get{return _Birthday;}
            set{SetProperty(ref _Birthday, value);}
        }
     

        private DateTime? _StartDate;
        /// <summary>
        /// 入职时间
        /// </summary>
        [AdvQueryAttribute(ColName = "StartDate",ColDesc = "入职时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "StartDate",IsNullable = true,ColumnDescription = "入职时间" )]
        public DateTime? StartDate 
        { 
            get{return _StartDate;}
            set{SetProperty(ref _StartDate, value);}
        }
     

        private string _JobTitle;
        /// <summary>
        /// 职称
        /// </summary>
        [AdvQueryAttribute(ColName = "JobTitle",ColDesc = "职称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "JobTitle",Length=50,IsNullable = true,ColumnDescription = "职称" )]
        public string JobTitle 
        { 
            get{return _JobTitle;}
            set{SetProperty(ref _JobTitle, value);}
        }
     

        private string _Address;
        /// <summary>
        /// 联络地址
        /// </summary>
        [AdvQueryAttribute(ColName = "Address",ColDesc = "联络地址")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Address",Length=255,IsNullable = true,ColumnDescription = "联络地址" )]
        public string Address 
        { 
            get{return _Address;}
            set{SetProperty(ref _Address, value);}
        }
     

        private string _Email;
        /// <summary>
        /// 邮件
        /// </summary>
        [AdvQueryAttribute(ColName = "Email",ColDesc = "邮件")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Email",Length=100,IsNullable = true,ColumnDescription = "邮件" )]
        public string Email 
        { 
            get{return _Email;}
            set{SetProperty(ref _Email, value);}
        }
     

        private string _Education;
        /// <summary>
        /// 教育程度
        /// </summary>
        [AdvQueryAttribute(ColName = "Education",ColDesc = "教育程度")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Education",Length=100,IsNullable = true,ColumnDescription = "教育程度" )]
        public string Education 
        { 
            get{return _Education;}
            set{SetProperty(ref _Education, value);}
        }
     

        private string _LanguageSkills;
        /// <summary>
        /// 外语能力
        /// </summary>
        [AdvQueryAttribute(ColName = "LanguageSkills",ColDesc = "外语能力")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "LanguageSkills",Length=50,IsNullable = true,ColumnDescription = "外语能力" )]
        public string LanguageSkills 
        { 
            get{return _LanguageSkills;}
            set{SetProperty(ref _LanguageSkills, value);}
        }
     

        private string _University;
        /// <summary>
        /// 毕业院校
        /// </summary>
        [AdvQueryAttribute(ColName = "University",ColDesc = "毕业院校")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "University",Length=100,IsNullable = true,ColumnDescription = "毕业院校" )]
        public string University 
        { 
            get{return _University;}
            set{SetProperty(ref _University, value);}
        }
     

        private string _IDNumber;
        /// <summary>
        /// 身份证号
        /// </summary>
        [AdvQueryAttribute(ColName = "IDNumber",ColDesc = "身份证号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "IDNumber",Length=30,IsNullable = true,ColumnDescription = "身份证号" )]
        public string IDNumber 
        { 
            get{return _IDNumber;}
            set{SetProperty(ref _IDNumber, value);}
        }
     

        private DateTime? _EndDate;
        /// <summary>
        /// 离职日期
        /// </summary>
        [AdvQueryAttribute(ColName = "EndDate",ColDesc = "离职日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "EndDate",IsNullable = true,ColumnDescription = "离职日期" )]
        public DateTime? EndDate 
        { 
            get{return _EndDate;}
            set{SetProperty(ref _EndDate, value);}
        }
     

        private decimal? _salary;
        /// <summary>
        /// 工资
        /// </summary>
        [AdvQueryAttribute(ColName = "salary",ColDesc = "工资")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "salary",IsNullable = true,ColumnDescription = "工资" )]
        public decimal? salary 
        { 
            get{return _salary;}
            set{SetProperty(ref _salary, value);}
        }
     

        private string _Notes;
        /// <summary>
        /// 备注说明
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注说明")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=200,IsNullable = true,ColumnDescription = "备注说明" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
        }
     

        private bool? _Is_enabled= true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_enabled",ColDesc = "是否启用")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Is_enabled",IsNullable = true,ColumnDescription = "是否启用" )]
        public bool? Is_enabled 
        { 
            get{return _Is_enabled;}
            set{SetProperty(ref _Is_enabled, value);}
        }
     

        private bool? _Is_available= true;
        /// <summary>
        /// 是否可用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_available",ColDesc = "是否可用")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Is_available",IsNullable = true,ColumnDescription = "是否可用" )]
        public bool? Is_available 
        { 
            get{return _Is_available;}
            set{SetProperty(ref _Is_available, value);}
        }
     

        private DateTime? _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Created_at",IsNullable = true,ColumnDescription = "创建时间" )]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}
            set{SetProperty(ref _Created_at, value);}
        }
     

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Created_by",IsNullable = true,ColumnDescription = "创建人" )]
        public long? Created_by 
        { 
            get{return _Created_by;}
            set{SetProperty(ref _Created_by, value);}
        }
     

        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Modified_at",IsNullable = true,ColumnDescription = "修改时间" )]
        public DateTime? Modified_at 
        { 
            get{return _Modified_at;}
            set{SetProperty(ref _Modified_at, value);}
        }
     

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Modified_by",IsNullable = true,ColumnDescription = "修改人" )]
        public long? Modified_by 
        { 
            get{return _Modified_by;}
            set{SetProperty(ref _Modified_by, value);}
        }
     

        private string _PhoneNumber;
        /// <summary>
        /// 手机号
        /// </summary>
        [AdvQueryAttribute(ColName = "PhoneNumber",ColDesc = "手机号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PhoneNumber",Length=50,IsNullable = true,ColumnDescription = "手机号" )]
        public string PhoneNumber 
        { 
            get{return _PhoneNumber;}
            set{SetProperty(ref _PhoneNumber, value);}
        }
     

        private long? _BankAccount_id;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "BankAccount_id",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "BankAccount_id",IsNullable = true,ColumnDescription = "" )]
        public long? BankAccount_id 
        { 
            get{return _BankAccount_id;}
            set{SetProperty(ref _BankAccount_id, value);}
        }


       
    }
}



