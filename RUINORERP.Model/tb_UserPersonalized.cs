
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/23/2025 14:00:47
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
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("用户角色个性化设置表tb_UserPersonalized" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _UserPersonalizedID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "UserPersonalizedID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long UserPersonalizedID
        { 
            get{return _UserPersonalizedID;}
            set{
            SetProperty(ref _UserPersonalizedID, value);
                base.PrimaryKeyID = _UserPersonalizedID;
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

        private long _ID;
        /// <summary>
        /// 用户角色
        /// </summary>
        [AdvQueryAttribute(ColName = "ID",ColDesc = "用户角色")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "用户角色" )]
        [FKRelationAttribute("tb_User_Role","ID")]
        public long ID
        { 
            get{return _ID;}
            set{
            SetProperty(ref _ID, value);
                        }
        }

        private bool? _UseUserOwnPrinter;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "UseUserOwnPrinter",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "UseUserOwnPrinter" ,IsNullable = true,ColumnDescription = "" )]
        public bool? UseUserOwnPrinter
        { 
            get{return _UseUserOwnPrinter;}
            set{
            SetProperty(ref _UseUserOwnPrinter, value);
                        }
        }

        private string _PrinterName;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "PrinterName",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PrinterName" ,Length=200,IsNullable = true,ColumnDescription = "" )]
        public string PrinterName
        { 
            get{return _PrinterName;}
            set{
            SetProperty(ref _PrinterName, value);
                        }
        }

        private bool? _SelectTemplatePrint;
        /// <summary>
        /// 选择模板打印
        /// </summary>
        [AdvQueryAttribute(ColName = "SelectTemplatePrint",ColDesc = "选择模板打印")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "SelectTemplatePrint" ,IsNullable = true,ColumnDescription = "选择模板打印" )]
        public bool? SelectTemplatePrint
        { 
            get{return _SelectTemplatePrint;}
            set{
            SetProperty(ref _SelectTemplatePrint, value);
                        }
        }

        private string _UserFavoriteMenu;
        /// <summary>
        /// 用户工具栏
        /// </summary>
        [AdvQueryAttribute(ColName = "UserFavoriteMenu",ColDesc = "用户工具栏")] 
        [SugarColumn(ColumnDataType = "text", SqlParameterDbType ="String",  ColumnName = "UserFavoriteMenu" ,Length=2147483647,IsNullable = true,ColumnDescription = "用户工具栏" )]
        public string UserFavoriteMenu
        { 
            get{return _UserFavoriteMenu;}
            set{
            SetProperty(ref _UserFavoriteMenu, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ID))]
        public virtual tb_User_Role tb_user_role { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_UIMenuPersonalization.UserPersonalizedID))]
        public virtual List<tb_UIMenuPersonalization> tb_UIMenuPersonalizations { get; set; }
        //tb_UIMenuPersonalization.UserPersonalizedID)
        //UserPersonalizedID.FK_UIMENUPERSONALIZATION_REF_USERPERSONALIZED)
        //tb_UserPersonalized.UserPersonalizedID)


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_UserPersonalized loctype = (tb_UserPersonalized)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

