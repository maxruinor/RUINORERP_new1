
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/20/2025 22:58:10
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
    /// UI表格设置
    /// </summary>
    [Serializable()]
    [Description("UI表格设置")]
    [SugarTable("tb_UIGridSetting")]
    public partial class tb_UIGridSetting: BaseEntity, ICloneable
    {
        public tb_UIGridSetting()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("UI表格设置tb_UIGridSetting" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _UIGID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "UIGID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long UIGID
        { 
            get{return _UIGID;}
            set{
            SetProperty(ref _UIGID, value);
                base.PrimaryKeyID = _UIGID;
            }
        }

        private long? _UIMenuPID;
        /// <summary>
        /// 菜单设置
        /// </summary>
        [AdvQueryAttribute(ColName = "UIMenuPID",ColDesc = "菜单设置")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "UIMenuPID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "菜单设置" )]
        [FKRelationAttribute("tb_UIMenuPersonalization","UIMenuPID")]
        public long? UIMenuPID
        { 
            get{return _UIMenuPID;}
            set{
            SetProperty(ref _UIMenuPID, value);
                        }
        }

        private string _GridKeyName;
        /// <summary>
        /// 表格名称
        /// </summary>
        [AdvQueryAttribute(ColName = "GridKeyName",ColDesc = "表格名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "GridKeyName" ,Length=255,IsNullable = true,ColumnDescription = "表格名称" )]
        public string GridKeyName
        { 
            get{return _GridKeyName;}
            set{
            SetProperty(ref _GridKeyName, value);
                        }
        }

        private string _ColsSetting;
        /// <summary>
        /// 列设置信息
        /// </summary>
        [AdvQueryAttribute(ColName = "ColsSetting",ColDesc = "列设置信息")] 
        [SugarColumn(ColumnDataType = "text", SqlParameterDbType ="String",  ColumnName = "ColsSetting" ,Length=2147483647,IsNullable = true,ColumnDescription = "列设置信息" )]
        public string ColsSetting
        { 
            get{return _ColsSetting;}
            set{
            SetProperty(ref _ColsSetting, value);
                        }
        }

        private string _GridType;
        /// <summary>
        /// 表格类型
        /// </summary>
        [AdvQueryAttribute(ColName = "GridType",ColDesc = "表格类型")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "GridType" ,Length=50,IsNullable = true,ColumnDescription = "表格类型" )]
        public string GridType
        { 
            get{return _GridType;}
            set{
            SetProperty(ref _GridType, value);
                        }
        }

        private int? _ColumnsMode= ((1));
        /// <summary>
        /// 列宽显示模式
        /// </summary>
        [AdvQueryAttribute(ColName = "ColumnsMode",ColDesc = "列宽显示模式")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ColumnsMode" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "列宽显示模式" )]
        public int? ColumnsMode
        { 
            get{return _ColumnsMode;}
            set{
            SetProperty(ref _ColumnsMode, value);
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
        [Navigate(NavigateType.OneToOne, nameof(UIMenuPID))]
        public virtual tb_UIMenuPersonalization tb_uimenupersonalization { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}







        public override object Clone()
        {
            tb_UIGridSetting loctype = (tb_UIGridSetting)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

