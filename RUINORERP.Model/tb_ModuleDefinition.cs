
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:41:58
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
    /// 功能模块定义（仅限部分已经硬码并体现于菜单表中）
    /// </summary>
    [Serializable()]
    [Description("功能模块定义（仅限部分已经硬码并体现于菜单表中）")]
    [SugarTable("tb_ModuleDefinition")]
    public partial class tb_ModuleDefinition: BaseEntity, ICloneable
    {
        public tb_ModuleDefinition()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("功能模块定义（仅限部分已经硬码并体现于菜单表中）tb_ModuleDefinition" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ModuleID;
        /// <summary>
        /// 模块
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ModuleID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "模块" , IsPrimaryKey = true)]
        public long ModuleID
        { 
            get{return _ModuleID;}
            set{
            SetProperty(ref _ModuleID, value);
                base.PrimaryKeyID = _ModuleID;
            }
        }

        private string _ModuleNo;
        /// <summary>
        /// 模块编号
        /// </summary>
        [AdvQueryAttribute(ColName = "ModuleNo",ColDesc = "模块编号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ModuleNo" ,Length=50,IsNullable = false,ColumnDescription = "模块编号" )]
        public string ModuleNo
        { 
            get{return _ModuleNo;}
            set{
            SetProperty(ref _ModuleNo, value);
                        }
        }

        private string _ModuleName;
        /// <summary>
        /// 模块名称
        /// </summary>
        [AdvQueryAttribute(ColName = "ModuleName",ColDesc = "模块名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ModuleName" ,Length=20,IsNullable = false,ColumnDescription = "模块名称" )]
        public string ModuleName
        { 
            get{return _ModuleName;}
            set{
            SetProperty(ref _ModuleName, value);
                        }
        }

        private bool _Visible;
        /// <summary>
        /// 是否可见
        /// </summary>
        [AdvQueryAttribute(ColName = "Visible",ColDesc = "是否可见")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Visible" ,IsNullable = false,ColumnDescription = "是否可见" )]
        public bool Visible
        { 
            get{return _Visible;}
            set{
            SetProperty(ref _Visible, value);
                        }
        }

        private bool _Available;
        /// <summary>
        /// 是否可用
        /// </summary>
        [AdvQueryAttribute(ColName = "Available",ColDesc = "是否可用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Available" ,IsNullable = false,ColumnDescription = "是否可用" )]
        public bool Available
        { 
            get{return _Available;}
            set{
            SetProperty(ref _Available, value);
                        }
        }

        private string _IconFile_Path;
        /// <summary>
        /// 图标路径
        /// </summary>
        [AdvQueryAttribute(ColName = "IconFile_Path",ColDesc = "图标路径")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "IconFile_Path" ,Length=100,IsNullable = true,ColumnDescription = "图标路径" )]
        public string IconFile_Path
        { 
            get{return _IconFile_Path;}
            set{
            SetProperty(ref _IconFile_Path, value);
                        }
        }

        #endregion

        #region 扩展属性

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MenuInfo.ModuleID))]
        public virtual List<tb_MenuInfo> tb_MenuInfos { get; set; }
        //tb_MenuInfo.ModuleID)
        //ModuleID.FK_TB_MENUI_REFERENCE_TB_MODUL)
        //tb_ModuleDefinition.ModuleID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FlowchartDefinition.ModuleID))]
        public virtual List<tb_FlowchartDefinition> tb_FlowchartDefinitions { get; set; }
        //tb_FlowchartDefinition.ModuleID)
        //ModuleID.FK_TB_FLOWC_REF_TB_MODULEDe)
        //tb_ModuleDefinition.ModuleID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_P4Menu.ModuleID))]
        public virtual List<tb_P4Menu> tb_P4Menus { get; set; }
        //tb_P4Menu.ModuleID)
        //ModuleID.FK_TB_P4MEN_REFERENCE_TB_MODUL)
        //tb_ModuleDefinition.ModuleID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_P4Module.ModuleID))]
        public virtual List<tb_P4Module> tb_P4Modules { get; set; }
        //tb_P4Module.ModuleID)
        //ModuleID.FK_TB_P4MOD_REFERENCE_TB_MODUL)
        //tb_ModuleDefinition.ModuleID)


        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_ModuleDefinition loctype = (tb_ModuleDefinition)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

