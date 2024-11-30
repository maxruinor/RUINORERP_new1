
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/30/2024 00:18:30
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
    /// 用户角色个性化设置表
    /// </summary>
    [Serializable()]
    [Description("用户角色个性化设置表")]
    [SugarTable("tb_UserPersonalized")]
    public partial class tb_UserPersonalized: BaseEntity, ICloneable
    {
        public tb_UserPersonalized()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("用户角色个性化设置表tb_UserPersonalized" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PSID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PSID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long PSID
        { 
            get{return _PSID;}
            set{
            base.PrimaryKeyID = _PSID;
            SetProperty(ref _PSID, value);
            }
        }

        private long? _UIPID;
        /// <summary>
        /// 个性化
        /// </summary>
        [AdvQueryAttribute(ColName = "UIPID",ColDesc = "个性化")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "UIPID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "个性化" )]
        [FKRelationAttribute("tb_UIMenuPersonalization","UIPID")]
        public long? UIPID
        { 
            get{return _UIPID;}
            set{
            SetProperty(ref _UIPID, value);
            }
        }

        private string _WorkCellSettings;
        /// <summary>
        /// 工作单元设置
        /// </summary>
        [AdvQueryAttribute(ColName = "WorkCellSettings",ColDesc = "工作单元设置")] 
        [SugarColumn(ColumnDataType = "text", SqlParameterDbType ="String",  ColumnName = "WorkCellSettings" ,Length=2147483647,IsNullable = true,ColumnDescription = "工作单元设置" )]
        public string WorkCellSettings
        { 
            get{return _WorkCellSettings;}
            set{
            SetProperty(ref _WorkCellSettings, value);
            }
        }

        private string _WorkCellLayout;
        /// <summary>
        /// 工作台布局
        /// </summary>
        [AdvQueryAttribute(ColName = "WorkCellLayout",ColDesc = "工作台布局")] 
        [SugarColumn(ColumnDataType = "text", SqlParameterDbType ="String",  ColumnName = "WorkCellLayout" ,Length=2147483647,IsNullable = true,ColumnDescription = "工作台布局" )]
        public string WorkCellLayout
        { 
            get{return _WorkCellLayout;}
            set{
            SetProperty(ref _WorkCellLayout, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(UIPID))]
        public virtual tb_UIMenuPersonalization tb_uimenupersonalization { get; set; }


        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_User_Role.PSID))]
        public virtual List<tb_User_Role> tb_User_Roles { get; set; }
        //tb_User_Role.PSID)
        //PSID.FK_USERPersonalized_REF_USER_Role)
        //tb_UserPersonalized.PSID)


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
                    Type type = typeof(tb_UserPersonalized);
                    
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
            tb_UserPersonalized loctype = (tb_UserPersonalized)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

