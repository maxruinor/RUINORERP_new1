
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:26
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
    /// 角色表
    /// </summary>
    [Serializable()]
    [Description("tb_RoleInfo")]
    [SugarTable("tb_RoleInfo")]
    public partial class tb_RoleInfo: BaseEntity, ICloneable
    {
        public tb_RoleInfo()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_RoleInfo" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _RoleID;
        /// <summary>
        /// 角色属性
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RoleID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "角色属性" , IsPrimaryKey = true)]
        public long RoleID
        { 
            get{return _RoleID;}
            set{
            base.PrimaryKeyID = _RoleID;
            SetProperty(ref _RoleID, value);
            }
        }

        private string _RoleName;
        /// <summary>
        /// 角色名称
        /// </summary>
        [AdvQueryAttribute(ColName = "RoleName",ColDesc = "角色名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "RoleName" ,Length=50,IsNullable = false,ColumnDescription = "角色名称" )]
        public string RoleName
        { 
            get{return _RoleName;}
            set{
            SetProperty(ref _RoleName, value);
            }
        }

        private string _Desc;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Desc",ColDesc = "描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Desc" ,Length=250,IsNullable = true,ColumnDescription = "描述" )]
        public string Desc
        { 
            get{return _Desc;}
            set{
            SetProperty(ref _Desc, value);
            }
        }

        private long? _RolePropertyID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "RolePropertyID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RolePropertyID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_RolePropertyConfig","RolePropertyID")]
        public long? RolePropertyID
        { 
            get{return _RolePropertyID;}
            set{
            SetProperty(ref _RolePropertyID, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(RolePropertyID))]
        public virtual tb_RolePropertyConfig tb_rolepropertyconfig { get; set; }


        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_User_Role.RoleID))]
        public virtual List<tb_User_Role> tb_User_Roles { get; set; }
        //tb_User_Role.RoleID)
        //RoleID.FKTB_USER_TB_ROLEI_Info)
        //tb_RoleInfo.RoleID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_P4Field.RoleID))]
        public virtual List<tb_P4Field> tb_P4Fields { get; set; }
        //tb_P4Field.RoleID)
        //RoleID.FK_TB_P4FIE_REFERENCE_TB_ROLEI)
        //tb_RoleInfo.RoleID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_P4Button.RoleID))]
        public virtual List<tb_P4Button> tb_P4Buttons { get; set; }
        //tb_P4Button.RoleID)
        //RoleID.FK_TB_P4BUT_REFERENCE_TB_ROLEI)
        //tb_RoleInfo.RoleID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_P4Menu.RoleID))]
        public virtual List<tb_P4Menu> tb_P4Menus { get; set; }
        //tb_P4Menu.RoleID)
        //RoleID.FK_TB_P4MEN_REFERENCE_TB_ROLEI)
        //tb_RoleInfo.RoleID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_P4Module.RoleID))]
        public virtual List<tb_P4Module> tb_P4Modules { get; set; }
        //tb_P4Module.RoleID)
        //RoleID.FK_TB_P4MOD_REFERENCE_TB_ROLEI)
        //tb_RoleInfo.RoleID)


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
                    Type type = typeof(tb_RoleInfo);
                    
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
            tb_RoleInfo loctype = (tb_RoleInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

