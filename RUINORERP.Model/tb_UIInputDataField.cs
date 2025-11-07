
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:42:21
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
    /// UI录入数据预设值表
    /// </summary>
    [Serializable()]
    [Description("UI录入数据预设值表")]
    [SugarTable("tb_UIInputDataField")]
    public partial class tb_UIInputDataField: BaseEntity, ICloneable
    {
        public tb_UIInputDataField()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("UI录入数据预设值表tb_UIInputDataField" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PresetValueID;
        /// <summary>
        /// 预设值
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PresetValueID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "预设值" , IsPrimaryKey = true)]
        public long PresetValueID
        { 
            get{return _PresetValueID;}
            set{
            SetProperty(ref _PresetValueID, value);
                base.PrimaryKeyID = _PresetValueID;
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

        private string _Caption;
        /// <summary>
        /// 字段标题
        /// </summary>
        [AdvQueryAttribute(ColName = "Caption",ColDesc = "字段标题")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Caption" ,Length=100,IsNullable = true,ColumnDescription = "字段标题" )]
        public string Caption
        { 
            get{return _Caption;}
            set{
            SetProperty(ref _Caption, value);
                        }
        }

        private string _FieldName;
        /// <summary>
        /// 字段名
        /// </summary>
        [AdvQueryAttribute(ColName = "FieldName",ColDesc = "字段名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "FieldName" ,Length=100,IsNullable = true,ColumnDescription = "字段名" )]
        public string FieldName
        { 
            get{return _FieldName;}
            set{
            SetProperty(ref _FieldName, value);
                        }
        }

        private string _BelongingObjectType;
        /// <summary>
        /// 所属实体
        /// </summary>
        [AdvQueryAttribute(ColName = "BelongingObjectType",ColDesc = "所属实体")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BelongingObjectType" ,Length=80,IsNullable = true,ColumnDescription = "所属实体" )]
        public string BelongingObjectType
        { 
            get{return _BelongingObjectType;}
            set{
            SetProperty(ref _BelongingObjectType, value);
                        }
        }

        private string _ValueType;
        /// <summary>
        /// 值类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ValueType",ColDesc = "值类型")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ValueType" ,Length=50,IsNullable = true,ColumnDescription = "值类型" )]
        public string ValueType
        { 
            get{return _ValueType;}
            set{
            SetProperty(ref _ValueType, value);
                        }
        }

        private int _ControlWidth= ((0));
        /// <summary>
        /// 控件宽度
        /// </summary>
        [AdvQueryAttribute(ColName = "ControlWidth",ColDesc = "控件宽度")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ControlWidth" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "控件宽度" )]
        public int ControlWidth
        { 
            get{return _ControlWidth;}
            set{
            SetProperty(ref _ControlWidth, value);
                        }
        }

        private int _Sort= ((0));
        /// <summary>
        /// 排序
        /// </summary>
        [AdvQueryAttribute(ColName = "Sort",ColDesc = "排序")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Sort" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "排序" )]
        public int Sort
        { 
            get{return _Sort;}
            set{
            SetProperty(ref _Sort, value);
                        }
        }

        private bool _IsVisble;
        /// <summary>
        /// 是否可见
        /// </summary>
        [AdvQueryAttribute(ColName = "IsVisble",ColDesc = "是否可见")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsVisble" ,IsNullable = false,ColumnDescription = "是否可见" )]
        public bool IsVisble
        { 
            get{return _IsVisble;}
            set{
            SetProperty(ref _IsVisble, value);
                        }
        }

        private string _Default1;
        /// <summary>
        /// 默认值1
        /// </summary>
        [AdvQueryAttribute(ColName = "Default1",ColDesc = "默认值1")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Default1" ,Length=255,IsNullable = true,ColumnDescription = "默认值1" )]
        public string Default1
        { 
            get{return _Default1;}
            set{
            SetProperty(ref _Default1, value);
                        }
        }

        private string _Default2;
        /// <summary>
        /// 默认值2
        /// </summary>
        [AdvQueryAttribute(ColName = "Default2",ColDesc = "默认值2")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Default2" ,Length=255,IsNullable = true,ColumnDescription = "默认值2" )]
        public string Default2
        { 
            get{return _Default2;}
            set{
            SetProperty(ref _Default2, value);
                        }
        }

        private bool? _EnableDefault1;
        /// <summary>
        /// 启用默认值1
        /// </summary>
        [AdvQueryAttribute(ColName = "EnableDefault1",ColDesc = "启用默认值1")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "EnableDefault1" ,IsNullable = true,ColumnDescription = "启用默认值1" )]
        public bool? EnableDefault1
        { 
            get{return _EnableDefault1;}
            set{
            SetProperty(ref _EnableDefault1, value);
                        }
        }

        private bool? _EnableDefault2;
        /// <summary>
        /// 启用默认值2
        /// </summary>
        [AdvQueryAttribute(ColName = "EnableDefault2",ColDesc = "启用默认值2")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "EnableDefault2" ,IsNullable = true,ColumnDescription = "启用默认值2" )]
        public bool? EnableDefault2
        { 
            get{return _EnableDefault2;}
            set{
            SetProperty(ref _EnableDefault2, value);
                        }
        }

        private bool? _Focused= false;
        /// <summary>
        /// 默认焦点
        /// </summary>
        [AdvQueryAttribute(ColName = "Focused",ColDesc = "默认焦点")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Focused" ,IsNullable = true,ColumnDescription = "默认焦点" )]
        public bool? Focused
        { 
            get{return _Focused;}
            set{
            SetProperty(ref _Focused, value);
                        }
        }

        private int? _DiffDays1;
        /// <summary>
        /// 差异天数1
        /// </summary>
        [AdvQueryAttribute(ColName = "DiffDays1",ColDesc = "差异天数1")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DiffDays1" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "差异天数1" )]
        public int? DiffDays1
        { 
            get{return _DiffDays1;}
            set{
            SetProperty(ref _DiffDays1, value);
                        }
        }

        private int? _DiffDays2;
        /// <summary>
        /// 差异天数2
        /// </summary>
        [AdvQueryAttribute(ColName = "DiffDays2",ColDesc = "差异天数2")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DiffDays2" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "差异天数2" )]
        public int? DiffDays2
        { 
            get{return _DiffDays2;}
            set{
            SetProperty(ref _DiffDays2, value);
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
            tb_UIInputDataField loctype = (tb_UIInputDataField)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

