﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:56
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
    [Description("tb_ModuleDefinition")]
    [SugarTable("tb_ModuleDefinition")]
    public partial class tb_ModuleDefinition: BaseEntity, ICloneable
    {
        public tb_ModuleDefinition()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_ModuleDefinition" + "外键ID与对应主主键名称不一致。请修改数据库");
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
            base.PrimaryKeyID = _ModuleID;
            SetProperty(ref _ModuleID, value);
            }
        }

        private string _ModuleNo;
        /// <summary>
        /// 模块编号
        /// </summary>
        [AdvQueryAttribute(ColName = "ModuleNo",ColDesc = "模块编号")] 
        [SugarColumn(ColumnDataType = "char", SqlParameterDbType ="String",  ColumnName = "ModuleNo" ,Length=50,IsNullable = false,ColumnDescription = "模块编号" )]
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

        private bool? _Available;
        /// <summary>
        /// 是否可用
        /// </summary>
        [AdvQueryAttribute(ColName = "Available",ColDesc = "是否可用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Available" ,IsNullable = true,ColumnDescription = "是否可用" )]
        public bool? Available
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

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FlowchartDefinition.ModuleID))]
        public virtual List<tb_FlowchartDefinition> tb_FlowchartDefinitions { get; set; }
        //tb_FlowchartDefinition.ModuleID)
        //ModuleID.FK_TB_FLOWC_REF_TB_MODULEDe)
        //tb_ModuleDefinition.ModuleID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MenuInfo.ModuleID))]
        public virtual List<tb_MenuInfo> tb_MenuInfos { get; set; }
        //tb_MenuInfo.ModuleID)
        //ModuleID.FK_TB_MENUI_REFERENCE_TB_MODUL)
        //tb_ModuleDefinition.ModuleID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_P4Menu.ModuleID))]
        public virtual List<tb_P4Menu> tb_P4Menus { get; set; }
        //tb_P4Menu.ModuleID)
        //ModuleID.FK_TB_P4MEN_REFERENCE_TB_MODUL)
        //tb_ModuleDefinition.ModuleID)

        //[Browsable(false)]
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
                    Type type = typeof(tb_ModuleDefinition);
                    
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
            tb_ModuleDefinition loctype = (tb_ModuleDefinition)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

