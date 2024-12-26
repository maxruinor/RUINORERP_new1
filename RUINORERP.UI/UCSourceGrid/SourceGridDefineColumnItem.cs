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

namespace RUINORERP.UI.UCSourceGrid
{
    /// <summary>
    /// �����еĶ���
    /// �������SourceGrid�ģ���Ŀǰ���ؼ��У���Ҫ�ǵ�����ϸ���ټ��Ϲ������ֺ͸�����ʾ���֡�
    /// ���Ա��
    /// </summary>
    [Serializable]
    public class SourceGridDefineColumnItem
    {
        /// <summary>
        /// ��������Ҫ�ġ���ʵ����ϸ���ݲ��֣�������Ӧ�������ط�ӳ�����ĺ��ĵ�λ��ʵ����;
        /// ��GuideToTargetColumn �ظ��ˡ���ʱû��ȥ�ع��Ż�
        /// </summary>
        public bool IsCoreContent { get; set; } = false;
        /// <summary>
        /// ��ĳЩ�е��Զ��п��������ô���
        /// </summary>
        public SourceGrid.AutoSizeMode ColAutoSizeMode { get; set; } = AutoSizeMode.None;

        /// <summary>
        /// ������������õ�һЩ����
        /// һ���ֶ��п���Ӱ����Ŀ���е�ֵ
        /// </summary>
        public ConcurrentDictionary<string, List<RelatedColumnParameter>> RelatedCols = new ConcurrentDictionary<string, List<RelatedColumnParameter>>();

        /// <summary>
        /// ����Ҫ�ֶ����ñ༭����ѯ������Դ����
        /// </summary>
        public ConcurrentDictionary<string, List<SourceToTargetMatchCol>> EditorDataSourceCols = new ConcurrentDictionary<string, List<SourceToTargetMatchCol>>();

        /// <summary>
        /// ʶ���Ƿ�Ϊ��Ҫ��Ŀ����  ��ʶ��,ֻ��һ����������
        /// </summary>
        public bool IsPrimaryBizKeyColumn { get; set; }

        /// <summary>
        /// �Ƿ�Ϊ��ֵ���͵���  ��ʱֻ��bool����ǡ����������չ��Ҫȡ���������ֵ Ҳ������������չ
        /// �ȷ��������ֲ�Ʒ��ͼ������ʵ�����е�λ��ʵ�ʵ�λֻ������ֵ��Ҫ��ʾ����ʱ����ʱ��λ���¼FK���
        /// </summary>
        public bool IsFKRelationColumn { get; set; }

        /// <summary>
        /// ָ��Ŀ����У���˼�ǹ������ֵ���Ҳ�����ڵ�����ϸ�С�Ҫ�������ֵ������ϸ������ȥ
        /// </summary>
        public bool GuideToTargetColumn { get; set; }



        private int _MaxLength = 0;

        /// <summary>
        /// �ַ����͵��е���󳤶�
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

        public SourceGridDefineColumnItem(SourceGridDefine parentGridDefine)
        {
            _parentGridDefine = parentGridDefine;
        }

        public SourceGridDefineColumnItem()
        {

        }


        private SourceGrid.Cells.Editors.EditorBase _editorForColumn;
        public EditorBase EditorForColumn { get => _editorForColumn; set => _editorForColumn = value; }


        public SourceGridDefineColumnItem(string name, int width, string selectobject, CustomFormatType formatType = CustomFormatType.DefaultFormat)
        {
            this.ColCaption = name;
            this.CustomFormat = formatType;
            this.selectobject = selectobject;
            this.width = width;
        }


        private PropertyInfo _colPropertyInfo;
        /// <summary>
        /// �����е����� �����б�������Ҫ��Ϣ �ȷ� ��� mssqlserver���� money��c#����decimal(19,4) 
        /// </summary>
        public PropertyInfo ColPropertyInfo { get => _colPropertyInfo; set => _colPropertyInfo = value; }

        public string fastkey = null;


        //Ĭ��ֵ
        public int width = 0;

        /// <summary>
        /// ������ƥ���е�����
        /// </summary>
        public int ColIndex = -1;


        public string ColCaption;
        public string ColName;
        public string storename;

        /// <summary>
        /// �Ƿ�Ϊ��ͷ��
        /// </summary>
        public bool IsRowHeaderCol { get; set; }





        /// <summary>
        /// ���ݴ�˼· �������Դ�����Թ�����Ʒ���ֺ͵�����ϸ���֡����ﱣ���˷ֱ���������
        /// </summary>
        public Type BelongingObjectType { get; set; }



        /// <summary>
        /// ѡ�еĶ���
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
        ///  �����������Զ���ɼ���Ȩ��Ҳ�޷����ơ�Ŀ���ǳ��������Ҫ�õ����ȷ���Ʒ�����ID,������ϸID
        /// </summary>
        public bool NeverVisible { get => neverVisible; set => neverVisible = value; }

        /// <summary>
        /// �Ƿ�Ĭ������
        /// </summary>
        public bool DefaultHide { get; set; } = false;

        /// <summary>
        /// �Ƿ���ͳ���� �ü�
        /// </summary>
        public bool Summary
        {
            get => summary; set => summary = value;
        }


        /// <summary>
        /// �еĸ�ʽ
        /// </summary>
        public CustomFormatType CustomFormat { get; set; } = CustomFormatType.DefaultFormat;

        /// <summary>
        /// Cell��ʽ����ʾ Ҫ���CustomFormatTypeʹ��
        /// </summary>
        public string FormatText { get; set; }

        /// <summary>
        ///ö������ѡ��ʱ������
        /// </summary>
        public Type TypeForEnumOptions { get; set; }

        /// <summary>
        /// �Ƿ�ΪС���� �ó� Ŀǰֻ�����һ��С�Ƽ����
        /// </summary>
        public bool SubtotalResult { get; set; } = false;


        /// <summary>
        /// �е�Ĭ��ֵ
        /// </summary>
        public object DefaultValue { get; set; }

        public bool result;
        public bool newline;


        /// <summary>
        /// ֻ��ģʽ
        /// </summary>
        private bool _ReadOnly = false;

        /// <summary>
        /// ��ֻ��
        /// </summary>
        public bool ReadOnly { get => _ReadOnly; set => _ReadOnly = value; }
        /// <summary>
        /// �пɼ����ȼ����������ɼ�
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
            }
        }

        /// <summary>
        /// �еı༭״̬�����ڼ����е�ֵʱ���ֹ����еĻ������(��ʱû��ʹ����)
        /// </summary>
        public bool IsEdit { get; internal set; }

        private bool visible = true;
        public bool check = false;
        public bool notnull = false;

        public bool addlink = false;
        public string linkprop;
        //public bool visible
        //{
        //    get
        //    {
        //        return visi;
        //    }
        //    set
        //    {
        //        visi = value;
        //        if (this.parent != null) parent.VisibleChange();

        //    }
        //}


        //public InputType type = InputType.Normal;
        public bool time = false;
        public string[] enums = null;
        public int span = 1;
        public bool percent;
        public bool select = false;
        public string[] vals = null;
        public int declen = 2;

    }
}
