
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/10/2025 15:31:57
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
    /// 用户角色关系表
    /// </summary>
    [Serializable()]
    [Description("用户角色关系表")]
    [SugarTable("tb_User_Role")]
    public partial class tb_User_Role: BaseEntity, ICloneable
    {
        public tb_User_Role()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("用户角色关系表tb_User_Role" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long ID
        { 
            get{return _ID;}
            set{
            base.PrimaryKeyID = _ID;
            SetProperty(ref _ID, value);
            }
        }

        private long _User_ID;
        /// <summary>
        /// 用户
        /// </summary>
        [AdvQueryAttribute(ColName = "User_ID",ColDesc = "用户")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "User_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "用户" )]
        [FKRelationAttribute("tb_UserInfo","User_ID")]
        public long User_ID
        { 
            get{return _User_ID;}
            set{
            SetProperty(ref _User_ID, value);
            }
        }

        private long _RoleID;
        /// <summary>
        /// 角色
        /// </summary>
        [AdvQueryAttribute(ColName = "RoleID",ColDesc = "角色")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RoleID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "角色" )]
        [FKRelationAttribute("tb_RoleInfo","RoleID")]
        public long RoleID
        { 
            get{return _RoleID;}
            set{
            SetProperty(ref _RoleID, value);
            }
        }

        private bool _Authorized= false;
        /// <summary>
        /// 已授权
        /// </summary>
        [AdvQueryAttribute(ColName = "Authorized",ColDesc = "已授权")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Authorized" ,IsNullable = false,ColumnDescription = "已授权" )]
        public bool Authorized
        { 
            get{return _Authorized;}
            set{
            SetProperty(ref _Authorized, value);
            }
        }

        private bool _DefaultRole= false;
        /// <summary>
        /// 默认角色
        /// </summary>
        [AdvQueryAttribute(ColName = "DefaultRole",ColDesc = "默认角色")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "DefaultRole" ,IsNullable = false,ColumnDescription = "默认角色" )]
        public bool DefaultRole
        { 
            get{return _DefaultRole;}
            set{
            SetProperty(ref _DefaultRole, value);
            }
        }

        private string _WorkDefaultLayout;
        /// <summary>
        /// 工作台布局
        /// </summary>
        [AdvQueryAttribute(ColName = "WorkDefaultLayout",ColDesc = "工作台布局")] 
        [SugarColumn(ColumnDataType = "text", SqlParameterDbType ="String",  ColumnName = "WorkDefaultLayout" ,Length=2147483647,IsNullable = true,ColumnDescription = "工作台布局" )]
        public string WorkDefaultLayout
        { 
            get{return _WorkDefaultLayout;}
            set{
            SetProperty(ref _WorkDefaultLayout, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(User_ID))]
        public virtual tb_UserInfo tb_userinfo { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(RoleID))]
        public virtual tb_RoleInfo tb_roleinfo { get; set; }


        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_UserPersonalized.ID))]
        public virtual List<tb_UserPersonalized> tb_UserPersonalizeds { get; set; }
        //tb_UserPersonalized.ID)
        //ID.FK_USERPersonalized_REF_USER_Role)
        //tb_User_Role.ID)


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
                    Type type = typeof(tb_User_Role);
                    
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
            tb_User_Role loctype = (tb_User_Role)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

