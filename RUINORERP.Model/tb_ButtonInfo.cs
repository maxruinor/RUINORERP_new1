
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:46:48
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
    /// 字段信息表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ButtonInfo")]
    public partial class tb_ButtonInfo: BaseEntity, ICloneable
    {
        public tb_ButtonInfo()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_ButtonInfo" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ButtonInfo_ID;
        /// <summary>
        /// 按钮
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ButtonInfo_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "按钮" , IsPrimaryKey = true)]
        public long ButtonInfo_ID
        { 
            get{return _ButtonInfo_ID;}
            set{
            base.PrimaryKeyID = _ButtonInfo_ID;
            SetProperty(ref _ButtonInfo_ID, value);
            }
        }

        private long? _MenuID;
        /// <summary>
        /// 所属菜单
        /// </summary>
        [AdvQueryAttribute(ColName = "MenuID",ColDesc = "所属菜单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "MenuID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "所属菜单" )]
        [FKRelationAttribute("tb_MenuInfo","MenuID")]
        public long? MenuID
        { 
            get{return _MenuID;}
            set{
            SetProperty(ref _MenuID, value);
            }
        }

        private string _BtnName;
        /// <summary>
        /// 按钮名称
        /// </summary>
        [AdvQueryAttribute(ColName = "BtnName",ColDesc = "按钮名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BtnName" ,Length=255,IsNullable = true,ColumnDescription = "按钮名称" )]
        public string BtnName
        { 
            get{return _BtnName;}
            set{
            SetProperty(ref _BtnName, value);
            }
        }

        private string _BtnText;
        /// <summary>
        /// 按钮文本
        /// </summary>
        [AdvQueryAttribute(ColName = "BtnText",ColDesc = "按钮文本")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BtnText" ,Length=250,IsNullable = true,ColumnDescription = "按钮文本" )]
        public string BtnText
        { 
            get{return _BtnText;}
            set{
            SetProperty(ref _BtnText, value);
            }
        }

        private string _HotKey;
        /// <summary>
        /// 热键
        /// </summary>
        [AdvQueryAttribute(ColName = "HotKey",ColDesc = "热键")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "HotKey" ,Length=50,IsNullable = true,ColumnDescription = "热键" )]
        public string HotKey
        { 
            get{return _HotKey;}
            set{
            SetProperty(ref _HotKey, value);
            }
        }

        private string _FormName;
        /// <summary>
        /// 窗体名称
        /// </summary>
        [AdvQueryAttribute(ColName = "FormName",ColDesc = "窗体名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "FormName" ,Length=255,IsNullable = true,ColumnDescription = "窗体名称" )]
        public string FormName
        { 
            get{return _FormName;}
            set{
            SetProperty(ref _FormName, value);
            }
        }

        private string _ClassPath;
        /// <summary>
        /// 类路径
        /// </summary>
        [AdvQueryAttribute(ColName = "ClassPath",ColDesc = "类路径")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ClassPath" ,Length=500,IsNullable = true,ColumnDescription = "类路径" )]
        public string ClassPath
        { 
            get{return _ClassPath;}
            set{
            SetProperty(ref _ClassPath, value);
            }
        }

        private bool? _IsForm;
        /// <summary>
        /// 是否为窗体
        /// </summary>
        [AdvQueryAttribute(ColName = "IsForm",ColDesc = "是否为窗体")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsForm" ,IsNullable = true,ColumnDescription = "是否为窗体" )]
        public bool? IsForm
        { 
            get{return _IsForm;}
            set{
            SetProperty(ref _IsForm, value);
            }
        }

        private bool _IsEnabled= true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "IsEnabled",ColDesc = "是否启用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsEnabled" ,IsNullable = false,ColumnDescription = "是否启用" )]
        public bool IsEnabled
        { 
            get{return _IsEnabled;}
            set{
            SetProperty(ref _IsEnabled, value);
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
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(MenuID))]
        public virtual tb_MenuInfo tb_menuinfo { get; set; }
        //public virtual tb_MenuInfo tb_MenuID { get; set; }


        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_P4Button.ButtonInfo_ID))]
        public virtual List<tb_P4Button> tb_P4Buttons { get; set; }
        //tb_P4Button.ButtonInfo_ID)
        //ButtonInfo_ID.FK_TB_P4BUT_REFERENCE_TB_BUTTO)
        //tb_ButtonInfo.ButtonInfo_ID)


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
                    Type type = typeof(tb_ButtonInfo);
                    
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
            tb_ButtonInfo loctype = (tb_ButtonInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

