using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

using System.Windows.Forms;
using System.Drawing.Design;

namespace DevAge.Windows.Forms
{
    /// <summary>
    /// TextBoxTypedButton����Ϊ�����͹�����UITypeEditor��������һ������ʱ��
    /// ���ǹ����ĵ�һ����������ע������ֵֻ����ʱ��ת,��Ϊ�������ڴ�����ʱ�����ġ�
    /// </summary>
    public class TextBoxUITypeEditorWebImage : DevAgeTextBoxButton, IServiceProvider, System.Windows.Forms.Design.IWindowsFormsEditorService, ITypeDescriptorContext
    {
        private System.ComponentModel.IContainer components = null;

        //public string seed;

        public TextBoxUITypeEditorWebImage()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
            this.DialogOpen += new EventHandler(Control_DialogOpen);
           // seed = Guid.NewGuid().ToString();
        }


        private void Control_DialogOpen(object sender, EventArgs e)
        {
            // ���� OpenFileDialog ʵ��
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // ���������û�ѡ����ļ�����
            openFileDialog.Filter = "Image Files(*.jpg;*.jpeg;*.gif;*.bmp;*.png)|*.jpg;*.jpeg;*.gif;*.bmp;*.png";

            // ���ñ���
            openFileDialog.Title = "��ѡ��ͼƬ�ļ���";

            // ��ʾ�Ի���
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // ��ȡѡ�е��ļ�·��
                SelectedFilePath = openFileDialog.FileName;

                // var model = this.ImagesBytes.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                // SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = (SourceGrid.Cells.Models.ValueImageWeb)model;

                //this.Tag = System.IO.File.ReadAllBytes(selectedFilePath);
                //this.Tag = System.Drawing.Image.FromFile(selectedFilePath);
            }
        }
 

        public string SelectedFilePath = string.Empty;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        public override void ShowDialog()
        {
            try
            {
                OnDialogOpen(EventArgs.Empty);

                /*
                if (m_UITypeEditor != null)
                {
                    //UITypeEditorEditStyle style = m_UITypeEditor.Get();
                    //if (style == UITypeEditorEditStyle.DropDown ||
                    //    style == UITypeEditorEditStyle.Modal)
                    //{
                    object editObject;
                    //Try to read the actual value, if the function failed I edit the default value
                    if (IsValidValue(out editObject) == false)
                    {
                        if (Validator != null)
                            editObject = Validator.DefaultValue;
                        else
                            editObject = null;
                    }

                    //object tmp = m_UITypeEditor.EditValue(this, this, editObject);
                    object tmp = editObject;
                    Value = tmp;
                    //}
                }
                */
                OnDialogClosed(EventArgs.Empty);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }
        }

        //private UITypeEditor m_UITypeEditor;

        ///// <summary>
        ///// ��ȡ������Ҫʹ�õ�UITypeEditor�����ָ������֤������ָ��TypeDescriptor��GetEditor����������֤��ʹ�á�ֵ���͡�
        ///// </summary>
        //[DefaultValue(null), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public UITypeEditor UITypeEditor
        //{
        //    get { return m_UITypeEditor; }
        //    set { m_UITypeEditor = value; }
        //}

        private TextBoxUITypeEditorWebImage m_UITypeEditor;

        /// <summary>
        /// ��ȡ������Ҫʹ�õ�UITypeEditor�����ָ������֤������ָ��TypeDescriptor��GetEditor����������֤��ʹ�á�ֵ���͡�
        /// </summary>
        [DefaultValue(null), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TextBoxUITypeEditorWebImage UITypeEditor
        {
            get { return m_UITypeEditor; }
            set { m_UITypeEditor = value; }
        }

        //public bool ShouldSerializeUITypeEditor()
        //{
        //    return m_UITypeEditor != m_DefaultUITypeEditor;
        //}

        protected override void ApplyValidatorRules()
        {
            base.ApplyValidatorRules();

            if (m_UITypeEditor == null && Validator != null)
            {
                //�����web���ص��Ǹ� ʵ����string��Ҫ���⴦��
                //object tmp = System.ComponentModel.TypeDescriptor.GetEditor(Validator.ValueType, typeof(UITypeEditor));
                //object tmp = System.ComponentModel.TypeDescriptor.GetEditor(typeof(System.Drawing.Image), typeof(UITypeEditor));

                //  var model = .Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                //  SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = (SourceGrid.Cells.Models.ValueImageWeb)model;


                m_UITypeEditor = new TextBoxUITypeEditorWebImage();
                //object tmp = new TextBoxUITypeEditorWebImage();

                //    if (tmp is TextBoxUITypeEditorWebImage)
                //    {
                //        m_UITypeEditor = (TextBoxUITypeEditorWebImage)tmp;
                //    }
                //if (tmp is UITypeEditor)
                //    if (tmp is UITypeEditor)
                //    {
                //        m_UITypeEditor = (UITypeEditor)tmp;
                //    }


            }
        }

        #region IServiceProvider Members
        System.Object IServiceProvider.GetService(System.Type serviceType)
        {
            //modal
            if (serviceType == typeof(System.Windows.Forms.Design.IWindowsFormsEditorService))
                return this;

            return null;
        }
        #endregion

        #region System.Windows.Forms.Design.IWindowsFormsEditorService
        private DevAge.Windows.Forms.DropDown m_dropDown = null;
        public virtual void CloseDropDown()
        {
            if (m_dropDown != null)
            {
                m_dropDown.CloseDropDown();
            }
        }

        public virtual void DropDownControl(System.Windows.Forms.Control control)
        {
            using (m_dropDown = new DevAge.Windows.Forms.DropDown(control, this, this.ParentForm))
            {
                m_dropDown.DropDownFlags = DevAge.Windows.Forms.DropDownFlags.CloseOnEscape;

                m_dropDown.ShowDropDown();

                m_dropDown.Close();
            }
            m_dropDown = null;
        }

        public virtual System.Windows.Forms.DialogResult ShowDialog(System.Windows.Forms.Form dialog)
        {
            return dialog.ShowDialog(this);
        }
        #endregion

        #region ITypeDescriptorContext Members

        void ITypeDescriptorContext.OnComponentChanged()
        {

        }

        IContainer ITypeDescriptorContext.Container
        {
            get
            {
                return base.Container;
            }
        }

        bool ITypeDescriptorContext.OnComponentChanging()
        {
            return true;
        }

        object ITypeDescriptorContext.Instance
        {
            get
            {
                return Value;
            }
        }

        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
        {
            get
            {
                return null;
            }
        }

        #endregion
    }
}

