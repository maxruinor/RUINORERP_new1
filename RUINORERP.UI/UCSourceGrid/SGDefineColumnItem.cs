using System;
using cenetcom.control;
using System.Reflection;
using SourceGrid.Cells.Editors;
using SourceGrid;
using SqlSugar;
using RUINORERP.Global.CustomAttribute;
using System.Linq.Expressions;
using System.Collections.Concurrent;
using static NPOI.HSSF.Util.HSSFColor;
using HLH.Lib.Helper;
using System.Collections.Generic;
using RUINORERP.UI.UControls;

namespace RUINORERP.UI.UCSourceGrid
{
    /// <summary>
    /// 单个列的定义
    /// 这个列是SourceGrid的，在目前表格控件中，主要是单据明细。
    /// 再加上公共部分和辅助显示部分。
    /// 所以标记
    /// 他会对应实际grid表格控件的列定义
    /// </summary>
    [Serializable]
    public class SGDefineColumnItem
    {
        private string _UniqueId;
        /// <summary>
        /// 唯一标识符
        /// 对应实际grid表格控件的列定义的唯一标识符
        /// </summary>
        public string UniqueId
        {
            get => _UniqueId;
            set
            {
                _UniqueId = value;
                if (this.DisplayController != null)
                {
                    DisplayController.UniqueId = value;
                }
            }
        }


        /// <summary>
        /// 2025-02-12为了能自定义显示逻辑，所以这里增加一个控制器。
        /// </summary>
        public SGColDisplayHandler DisplayController { get; set; } = new SGColDisplayHandler();

        /// <summary>
        /// 代表了主要的、真实的明细数据部分，其命名应该清晰地反映出它的核心地位和实际用途
        /// 与GuideToTargetColumn 重复了。暂时没有去重构优化
        /// </summary>
        public bool IsCoreContent { get; set; } = false;
        /// <summary>
        /// 对某些列的自动列宽特殊设置处理
        /// </summary>
        public SourceGrid.AutoSizeMode ColAutoSizeMode { get; set; } = AutoSizeMode.None;

        /// <summary>
        /// 保存关联列设置的一些参数
        /// 一个字段列可以影响多个目标列的值
        /// </summary>
        public ConcurrentDictionary<string, List<RelatedColumnParameter>> RelatedCols = new ConcurrentDictionary<string, List<RelatedColumnParameter>>();

        /// <summary>
        /// 保存要手动设置编辑器查询的数据源的列
        /// </summary>
        public ConcurrentDictionary<string, List<SourceToTargetMatchCol>> EditorDataSourceCols = new ConcurrentDictionary<string, List<SourceToTargetMatchCol>>();

        /// <summary>
        /// 识别是否为主要的目标列  标识列,只有一个列是主键
        /// </summary>
        public bool IsPrimaryBizKeyColumn { get; set; }

        /// <summary>
        /// 是否为键值类型的列  暂时只用bool来标记。如果后面扩展需要取特性里面的值 也可以在这里扩展
        /// 比方公共部分产品视图带出的实体中有单位。实际单位只是主键值，要显示名称时，这时单位标记录FK外键
        /// </summary>
        public bool IsFKRelationColumn { get; set; }

        /// <summary>
        /// 指定目标的列，意思是公共部分的列也存在于单据明细中。要查出来的值给到明细对象中去
        /// </summary>
        public bool GuideToTargetColumn { get; set; }



        private int _MaxLength = 0;

        /// <summary>
        /// 字符类型的列的最大长度
        /// </summary>
        public int MaxLength { get => _MaxLength; set => _MaxLength = value; }

        public SugarColumn SugarCol { get; set; }

        public FKRelationAttribute FKRelationCol { get; set; }

        // private EditableMode m_EditableMode = EditableMode.Default;

        private EditableMode m_EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey | SourceGrid.EditableMode.SingleClick;
        /// <summary>
        /// Mode to edit the cell.
        /// </summary>
        public EditableMode EditableMode
        {
            get { return m_EditableMode; }
            set { m_EditableMode = value; }
        }


        private string toolTipText;
        public string ToolTipText { get => toolTipText; set => toolTipText = value; }

        private SourceGridDefine _parentGridDefine = null;
        public SourceGridDefine ParentGridDefine { get => _parentGridDefine; set => _parentGridDefine = value; }

        public SGDefineColumnItem(SourceGridDefine parentGridDefine)
        {
            _parentGridDefine = parentGridDefine;
        }

        public SGDefineColumnItem()
        {

        }


        private SourceGrid.Cells.Editors.EditorBase _editorForColumn;
        public EditorBase EditorForColumn { get => _editorForColumn; set => _editorForColumn = value; }


        public SGDefineColumnItem(string name, int width, string selectobject, CustomFormatType formatType = CustomFormatType.DefaultFormat)
        {
            this.ColCaption = name;
            this.CustomFormat = formatType;
            this.selectobject = selectobject;
            this.Width = width;
        }


        private PropertyInfo _colPropertyInfo;
        /// <summary>
        /// 保存列的属性 属性中保存了重要信息 比方 金额 mssqlserver中是 money，c#中是decimal(19,4) 
        /// </summary>
        public PropertyInfo ColPropertyInfo { get => _colPropertyInfo; set => _colPropertyInfo = value; }

        public string fastkey = null;


        //默认值
        private int width = 0;

        /// <summary>
        /// 保存了匹配列的索引
        /// </summary>
        private int colIndex = -1;

        private string colCaption;

        private string colName;

        public string storename;

        /// <summary>
        /// 是否为行头列
        /// </summary>
        public bool IsRowHeaderCol { get; set; }


        private Type _BelongingObjectType = null;


        /// <summary>
        /// 根据大思路 表格数据源是来自公共产品部分和单据明细部分。这里保存了分别所属类型
        /// </summary>
        public Type BelongingObjectType
        {
            get => _BelongingObjectType;
            set
            {
                _BelongingObjectType = value;
                if (this.DisplayController != null && value != null)
                {
                    DisplayController.BelongingObjectName = value.Name;
                }
            }
        }



        /// <summary>
        /// 选中的对象？
        /// </summary>
        public string selectobject;

        public string selectfield;

        public string filter;
        public bool multi;
        private bool summary = false;
        public bool upper = false;


        public bool CanMuliSelect { get; set; } = true;

        private bool neverVisible = false;


        /// <summary>
        ///  设置这个列永远不可见，权限也无法控制。目的是程序控制中要用到。比方产品详情的ID,单据明细ID
        /// </summary>
        public bool NeverVisible
        {
            get => neverVisible;

            set
            {
                neverVisible = value;
                if (this.DisplayController != null)
                {
                    DisplayController.Disable = value;
                }
            }
        }

        /// <summary>
        /// 是否默认隐藏
        /// </summary>
        public bool DefaultHide { get; set; } = false;

        /// <summary>
        /// 是否是统计列 用加
        /// </summary>
        public bool Summary
        {
            get => summary; set => summary = value;
        }


        /// <summary>
        /// 列的格式
        /// </summary>
        public CustomFormatType CustomFormat { get; set; } = CustomFormatType.DefaultFormat;

        /// <summary>
        /// Cell格式化显示 要配合CustomFormatType使用
        /// </summary>
        public string FormatText { get; set; }

        /// <summary>
        ///枚举下拉选项时的类型
        /// </summary>
        public Type TypeForEnumOptions { get; set; }

        /// <summary>
        /// 是否为小计列 用乘 目前只设计了一组小计及结果
        /// </summary>
        public bool SubtotalResult { get; set; } = false;


        /// <summary>
        /// 列的默认值
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// 标记为需要格式化显示的列
        /// 暂时这样，也许可以设置一个keyvaluepair，key为列名，value为格式化显示的格式值的集合
        /// </summary>
        public bool IsDisplayFormatText { get; set; } = false;



        public bool result;
        public bool newline;


        /// <summary>
        /// 只读模式
        /// </summary>
        private bool _ReadOnly = false;

        /// <summary>
        /// 列只读
        /// </summary>
        public bool ReadOnly { get => _ReadOnly; set => _ReadOnly = value; }
        /// <summary>
        /// 列可见优先级次于永不可见
        /// </summary>
        public bool Visible
        {
            get
            {
                if (NeverVisible)
                {
                    visible = false;
                }
                return visible;
            }
            set
            {
                visible = value;
                if (this.DisplayController != null)
                {
                    DisplayController.Visible = value;
                }
            }
        }

        /// <summary>
        /// 列的编辑状态，用于计算列的值时各种关联列的混乱情况(暂时没有使用上)
        /// </summary>
        public bool IsEdit { get; internal set; }
        public string ColCaption
        {
            get => colCaption;
            set
            {
                colCaption = value;
                if (this.DisplayController != null)
                {
                    DisplayController.ColCaption = value;
                }
            }
        }

        public int Width
        {
            get => width;
            set
            {
                width = value;
                if (this.DisplayController != null)
                {
                    DisplayController.ColWidth = value;
                }
            }
        }

        public string ColName
        {
            get => colName;
            set
            {
                colName = value;
                if (this.DisplayController != null)
                {
                    DisplayController.ColName = value;
                }
            }
        }

        public int ColIndex
        {
            get => colIndex;
            set
            {
                colIndex = value;
            }
        }

        private bool visible = true;
        public bool check = false;
        public bool notnull = false;

        public bool addlink = false;
        public string linkprop;

        public bool time = false;
        public string[] enums = null;
        public int span = 1;
        public bool percent;
        public bool select = false;
        public string[] vals = null;
        public int declen = 2;

    }
}
