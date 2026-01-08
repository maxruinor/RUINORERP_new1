
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:42:22
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
    /// 用户表
    /// </summary>
    [Serializable()]
    [Description("用户表")]
    [SugarTable("tb_UserInfo")]
    public partial class tb_UserInfo: BaseEntity, ICloneable
    {
        public tb_UserInfo()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("用户表tb_UserInfo" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _User_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "User_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long User_ID
        { 
            get{return _User_ID;}
            set{
            SetProperty(ref _User_ID, value);
                base.PrimaryKeyID = _User_ID;
            }
        }

        private long? _Employee_ID;
        /// <summary>
        /// 员工信息
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "员工信息")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "员工信息" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long? Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
                        }
        }

        private string _UserName;
        /// <summary>
        /// 用户名
        /// </summary>
        [AdvQueryAttribute(ColName = "UserName",ColDesc = "用户名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "UserName" ,Length=255,IsNullable = false,ColumnDescription = "用户名" )]
        public string UserName
        { 
            get{return _UserName;}
            set{
            SetProperty(ref _UserName, value);
                        }
        }

        private string _Password;
        /// <summary>
        /// 密码
        /// </summary>
        [AdvQueryAttribute(ColName = "Password",ColDesc = "密码")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Password" ,Length=255,IsNullable = true,ColumnDescription = "密码" )]
        public string Password
        { 
            get{return _Password;}
            set{
            SetProperty(ref _Password, value);
                        }
        }

        private bool _is_enabled= true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "is_enabled",ColDesc = "是否启用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "is_enabled" ,IsNullable = false,ColumnDescription = "是否启用" )]
        public bool is_enabled
        { 
            get{return _is_enabled;}
            set{
            SetProperty(ref _is_enabled, value);
                        }
        }

        private bool _is_available= true;
        /// <summary>
        /// 是否可用
        /// </summary>
        [AdvQueryAttribute(ColName = "is_available",ColDesc = "是否可用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "is_available" ,IsNullable = false,ColumnDescription = "是否可用" )]
        public bool is_available
        { 
            get{return _is_available;}
            set{
            SetProperty(ref _is_available, value);
                        }
        }

        private bool _IsSuperUser= false;
        /// <summary>
        /// 超级用户
        /// </summary>
        [AdvQueryAttribute(ColName = "IsSuperUser",ColDesc = "超级用户")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsSuperUser" ,IsNullable = false,ColumnDescription = "超级用户" )]
        public bool IsSuperUser
        { 
            get{return _IsSuperUser;}
            set{
            SetProperty(ref _IsSuperUser, value);
                        }
        }

        private string _Notes;
        /// <summary>
        /// 备注说明
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注说明")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=100,IsNullable = true,ColumnDescription = "备注说明" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
                        }
        }

        private DateTime? _Lastlogin_at;
        /// <summary>
        /// 最后登录时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Lastlogin_at",ColDesc = "最后登录时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Lastlogin_at" ,IsNullable = true,ColumnDescription = "最后登录时间" )]
        public DateTime? Lastlogin_at
        { 
            get{return _Lastlogin_at;}
            set{
            SetProperty(ref _Lastlogin_at, value);
                        }
        }

        private DateTime? _Lastlogout_at;
        /// <summary>
        /// 最后登出时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Lastlogout_at",ColDesc = "最后登出时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Lastlogout_at" ,IsNullable = true,ColumnDescription = "最后登出时间" )]
        public DateTime? Lastlogout_at
        { 
            get{return _Lastlogout_at;}
            set{
            SetProperty(ref _Lastlogout_at, value);
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
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_User_Role.User_ID))]
        public virtual List<tb_User_Role> tb_User_Roles { get; set; }
        //tb_User_Role.User_ID)
        //User_ID.FK_TB_USER_REFERENCE_TB_USERI)
        //tb_UserInfo.User_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_P4RowAuthPolicyByUser.User_ID))]
        public virtual List<tb_P4RowAuthPolicyByUser> tb_P4RowAuthPolicyByUsers { get; set; }
        //tb_P4RowAuthPolicyByUser.User_ID)
        //User_ID.FK_TB_P4ROWAUTHPOLICYBYUSER_REF_TB_USERINFO)
        //tb_UserInfo.User_ID)


        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

