using System;
using SourceGrid2.Cells.Real;
using cenetcom.control;
using SourceGrid2;
using System.Reflection;
using SourceGrid.Cells.Editors;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 单个列的定义
    /// </summary>
    public class SourceGridDefineColumnItem
    {
        public SourceGridDefineColumnItem()
        {

        }


        private SourceGrid.Cells.Editors.EditorBase _editorForColumn;
        public EditorBase EditorForColumn { get => _editorForColumn; set => _editorForColumn = value; }


        public SourceGridDefineColumnItem(string name, int width, bool currency, string selectobject)
        {
            this.name = name;
            this.currency = currency;
            this.selectobject = selectobject;
            this.width = width;
        }


        private PropertyInfo _colPropertyInfo;
        /// <summary>
        /// 保存列的属性
        /// </summary>
        public PropertyInfo ColPropertyInfo { get => _colPropertyInfo; set => _colPropertyInfo = value; }

        public string fastkey = null;
        public GridDefine parent = null;

        //默认值
        public int width = 90;

        public string name;
        public string storename;
        public bool currency = false;

        /// <summary>
        /// 选中的对象？
        /// </summary>
        public string selectobject;

        public string selectfield;

        public string filter;
        public bool multi;
        public bool summary = false;
        public bool upper = false;

        public bool Summary
        {
            get
            {
                return summary;
            }
        }
        public bool result;
        public bool newline;


        /// <summary>
        /// 只读模式
        /// </summary>
        public bool ReadOnly = false;

        public bool check = false;
        public bool notnull = false;
        private bool visi = true;
        public bool addlink = false;
        public string linkprop;
        public bool visible
        {
            get
            {
                return visi;
            }
            set
            {
                visi = value;
                if (this.parent != null) parent.VisibleChange();

            }
        }


        public InputType type = InputType.Normal;
        public bool time = false;
        public string[] enums = null;
        public int span = 1;
        public bool percent;
        public bool select = false;
        public string[] vals = null;
        public int declen = 2;

    }
}
