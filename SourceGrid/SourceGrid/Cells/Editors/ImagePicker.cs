using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Drawing.Imaging;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Collections.Generic;



namespace SourceGrid.Cells.Editors
{
    /// <summary>
    ///  A model that use a TextBoxButton for Image editing, allowing to select a source image file. Returns null as DisplayString. Write and read byte[] values.
    /// </summary>
    [System.ComponentModel.ToolboxItem(false)]
    public class ImagePicker : EditorControlBase
    {
        public readonly static ImagePicker Default = new ImagePicker();

        #region Constructor
        /// <summary>
        /// Construct an Editor of type ImagePicker.
        /// </summary>
        public ImagePicker() : base(typeof(byte[]))
        {
        }
        #endregion


        #region ������Ҽ��˵�
        public bool Use�Ƿ�ʹ�������Ҽ����� { get; private set; } = true;

        /// <summary>
        /// ��Ϊ��ʱ�¼��޷�ͨ�������е����ݴ��䣬���������ٴ�����������ƥ��
        /// </summary>
        private List<EventHandler> ContextClickList = new List<EventHandler>();
        private List<ContextMenuController> _ContextMenucCnfigurator = new List<ContextMenuController>();

        private ContextMenuStrip _ContextMenuStrip;

        /// <summary>
        /// ��д�Ҽ��˵� Ϊ�˺ϲ�
        /// ���ʹ�����õĲ˵�����������ʱ��Ҫ�ϲ�����
        /// </summary>
        public ContextMenuStrip ContextMenuStrip
        {
            get { return _ContextMenuStrip; }
            set
            {
                _ContextMenuStrip = value;
            }
        }

        /// <summary>
        /// ��ȡ��ǰ�Ƿ��������ģʽ
        /// </summary>
        /// <remarks>
        /// �ڳ����ʼ��ʱ��ȡһ�αȽ�׼ȷ������Ҫʱ��ȡ�������ڲ���Ƕ�׵��»�ȡ����ȷ����GridControl-GridView��ϡ�
        /// </remarks>
        /// <returns>�Ƿ�Ϊ�����ģʽ</returns>
        private bool GetIsDesignMode()
        {
            return (this.GetService(typeof(System.ComponentModel.Design.IDesignerHost)) == null
                || LicenseManager.UsageMode == LicenseUsageMode.Designtime);
        }

        /// <summary>
        /// �����Ҽ��˵������ǶԲ��Բ����������á���Ϊ�����õģ���ı�ֵ
        /// </summary>
        /// <param name="_contextMenuStrip"></param>
        public ContextMenuStrip GetContextMenu(ContextMenuStrip _contextMenuStrip = null)
        {
            // ����һ��ȫ�µ��Ҽ��˵�����
            ContextMenuStrip newContextMenuStrip = new ContextMenuStrip();

            //��ʼ���Ҽ��˵�
            // ��ʼ�������Ҽ��˵�
            ContextMenuStrip internalMenu = new ContextMenuStrip();
            internalMenu.BackColor = System.Drawing.Color.FromArgb(192, 255, 255);

            //�����Ҫͨ�����ʱ������ֵ�޸ġ�
            //������������캯���в�������Ϊ��ʱ����ֵ���ܻ�ȡ
            internalMenu.Items.Clear();

            // �ϲ�����Ĳ˵������ò˵�
            if (_contextMenuStrip != null)
            {
                //����˵����������һ���ָ���
                if (_contextMenuStrip.Items.Count > 0)
                {
                    ToolStripSeparator MyTss = new ToolStripSeparator();
                    _contextMenuStrip.Items.Add(MyTss);
                }

                ToolStripItem[] ts = new ToolStripItem[_contextMenuStrip.Items.Count];
                _contextMenuStrip.Items.CopyTo(ts, 0);

                internalMenu.Items.AddRange(ts);


            }


            //����Ҳ����ָ��������Щ��Ч �ϲ�����ģ�
            if (Use�Ƿ�ʹ�������Ҽ�����)
            {
                #region �������õ��Ҽ��˵�

                if (ContextClickList == null)
                {
                    ContextClickList = new List<EventHandler>();
                }
                ContextClickList.Clear();
                ContextClickList.Add(ContextMenu_�鿴��ͼ);
                if (_ContextMenucCnfigurator == null)
                {
                    _ContextMenucCnfigurator = new List<ContextMenuController>();
                }
                _ContextMenucCnfigurator.Clear();
                //ֻ�ǳ�ʼ�����ظ����
                if (_ContextMenucCnfigurator.Count == 0 && GetIsDesignMode())
                {
                    _ContextMenucCnfigurator.Add(new ContextMenuController("���鿴��ͼ��", true, false, "ContextMenu_�鿴��ͼ"));
                    _ContextMenucCnfigurator.Add(new ContextMenuController("��line��", true, true, ""));
                }
                #endregion

                foreach (var item in _ContextMenucCnfigurator)
                {
                    if (!item.IsShow)
                    {
                        continue;
                    }

                    if (item.IsSeparator)
                    {
                        ToolStripSeparator ts1 = new ToolStripSeparator();
                        internalMenu.Items.Add(ts1);
                    }
                    else
                    {
                        EventHandler ehh = ContextClickList.Find(
                            delegate (EventHandler eh)
                            {
                                return eh.Method.Name == item.ClickEventName;
                            });
                        //����϶���ⲿ�¼�Ҳ������һ������
                        //if (ehh == null && item.ClickEventName == "ɾ��ѡ����")
                        //{
                        //    ehh = ɾ��ѡ����;
                        //}
                        internalMenu.Items.Add(item.MenuText, null, ehh);
                    }
                }
            }
            newContextMenuStrip = internalMenu;
            // �������յ��Ҽ��˵�
            ContextMenuStrip = newContextMenuStrip;
            return newContextMenuStrip;
        }

        private void ContextMenu_�鿴��ͼ(object sender, EventArgs e)
        {
            DevAge.Windows.Forms.frmPictureViewer frm = new DevAge.Windows.Forms.frmPictureViewer();
            frm.ShowDialog();
        }
        #endregion



        #region Edit Control
        /// <summary>
        /// Create the editor control
        /// </summary>
        /// <returns></returns>
        protected override Control CreateControl()
        {
            DevAge.Windows.Forms.TextBoxUITypeEditor editor = new DevAge.Windows.Forms.TextBoxUITypeEditor();
            editor.BorderStyle = DevAge.Drawing.BorderStyle.None;
            editor.Validator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(System.Drawing.Image));
            //editor.ContextMenuStrip = GetContextMenu();

            return editor;
        }

        /// <summary>
        /// Gets the control used for editing the cell.
        /// </summary>
        public new DevAge.Windows.Forms.TextBoxUITypeEditor Control
        {
            get
            {
                return (DevAge.Windows.Forms.TextBoxUITypeEditor)base.Control;
            }
        }


        #endregion

        public override object GetEditedValue()
        {
            object val = Control.Value;
            if (val == null)
                return null;
            else if (val is System.Drawing.Image)
            {
                System.Drawing.Image img = val as System.Drawing.Image;
                #region �ж�ͼƬ��С�Ƿ񳬹� 500KB
                //��ͼ����뵽�ֽ�����

                byte[] buffByte = GetByteImage(val as System.Drawing.Image);
                // �ж�ͼƬ��С�Ƿ񳬹� 500KB
                if (buffByte.Length > 500 * 1024)
                {
                    // ѹ��ͼƬ
                    ImageCodecInfo jpegCodec = GetEncoderInfo(ImageFormat.Jpeg);
                    EncoderParameters encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);
                    img = (System.Drawing.Image)new System.Drawing.Bitmap(img, new System.Drawing.Size(800, 600));
                    img.Save("compressed.jpg", jpegCodec, encoderParams);

                    // ���¶�ȡѹ�����ͼƬ
                    img = System.Drawing.Image.FromFile("compressed.jpg");

                    MessageBox.Show("ͼƬ��С���� 500KB�����Զ�ѹ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // ��ѹ�����ͼƬת��Ϊ byte[] ����
                    byte[] compressedImageBytes = File.ReadAllBytes("compressed.jpg");

                    // �����ｫ compressedImageBytes ���浽���ݿ���
                    return compressedImageBytes;
                    // DevAge.ComponentModel.Validator.ValidatorTypeConverter imageValidator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(System.Drawing.Image));
                    //  return imageValidator.ValueToObject(val, typeof(byte[]));
                }
                else
                {

                    DevAge.ComponentModel.Validator.ValidatorTypeConverter imageValidator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(System.Drawing.Image));
                    return imageValidator.ValueToObject(val, typeof(byte[]));
                }
                #endregion


                //Stranamente questo codice in caso di ico va in eccezione!
                //				System.Drawing.Image img = (System.Drawing.Image)val;
                //				using (System.IO.MemoryStream memStream = new System.IO.MemoryStream())
                //				{
                //					img.Save(memStream, System.Drawing.Imaging.ImageCodecInfo.);
                //
                //					return memStream.ToArray();
                //				}
            }
            else if (val is byte[])
                return val;
            else
                throw new SourceGridException("Invalid edited value, expected byte[] or Image");
        }

        public byte[] GetByteImage(System.Drawing.Image img)
        {
            byte[] bt = null;
            if (!img.Equals(null))
            {
                using (MemoryStream mostream = new MemoryStream())
                {
                    System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(img);
                    bmp.Save(mostream, System.Drawing.Imaging.ImageFormat.Bmp);//��ͼ����ָ���ĸ�ʽ���뻺���ڴ���
                    bt = new byte[mostream.Length];
                    mostream.Position = 0;//�������ĳ�ʼλ��
                    mostream.Read(bt, 0, Convert.ToInt32(bt.Length));
                }
            }
            return bt;
        }

        private ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }


        public override void SetEditValue(object editValue)
        {
            Control.Value = editValue;
            Control.TextBox.SelectAll();
        }

        /// <summary>
        /// Used to returns the display string for a given value. In this case return null.
        /// </summary>
        /// <param name="p_Value"></param>
        /// <returns></returns>
        public override string ValueToDisplayString(object p_Value)
        {
            return null;
        }



        protected override void OnSendCharToEditor(char key)
        {
            //No implementation
        }

        public override object GetEditedTagValue()
        {
            object val = Control.Tag;
            if (val == null)
                return null;
            else if (val is System.Drawing.Image)
            {
                DevAge.ComponentModel.Validator.ValidatorTypeConverter imageValidator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(System.Drawing.Image));
                return imageValidator.ValueToObject(val, typeof(byte[]));

                //Stranamente questo codice in caso di ico va in eccezione!
                //				System.Drawing.Image img = (System.Drawing.Image)val;
                //				using (System.IO.MemoryStream memStream = new System.IO.MemoryStream())
                //				{
                //					img.Save(memStream, System.Drawing.Imaging.ImageCodecInfo.);
                //
                //					return memStream.ToArray();
                //				}
            }
            else if (val is byte[])
                return val;
            else
                throw new SourceGridException("Invalid edited value, expected byte[] or Image");
        }
    }

    [Serializable]
    [TypeConverter(typeof(MenuControllerConverter))]
    public class ContextMenuController
    {
        public ContextMenuController()
        {
            menuText = "menuText1";
        }
        private string menuText = string.Empty;
        private bool isShow = true;
        private string _clickEventName = string.Empty;
        public string MenuText { get => menuText; set => menuText = value; }
        public bool IsShow { get => isShow; set => isShow = value; }
        public string ClickEventName { get => _clickEventName; set => _clickEventName = value; }
        /// <summary>
        /// �Ƿ�Ϊ�ָ���
        /// </summary>
        public bool IsSeparator { get => isSeparator; set => isSeparator = value; }

        private bool isSeparator = false;

        //        public ContextMenuControler(string _menuText, EventHandler _click, bool isSeparatorLine)
        public ContextMenuController(string _menuText, bool isShow, bool isSeparatorLine, string _click)
        {
            menuText = _menuText;
            _clickEventName = _click;
            isSeparator = isSeparatorLine;
            IsShow = isShow;
        }



    }

    #region Converter ��
    /// <summary>
    /// ��������CanConvertFrom��CanConvertTo��ConvertTo��ConvertFrom
    /// </summary>
    [Serializable]
    public class MenuControllerConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(String)) return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(String)) return true;

            if (destinationType == typeof(InstanceDescriptor)) return true;

            return base.CanConvertTo(context, destinationType);
        }

        /*
        * ConvertTo��ʵ�֣����ת����Ŀ��������string��
        * �ҽ�Scope����������ת����string���ͣ�������һ������������������
        * ���������������������￴���ı�����ʽ��
        * 
        * ���ת����Ŀ��������ʵ����������InstanceDescriptor��
        * ����������ʵ�����Ĵ��룩��������Ҫ����һ��ʵ����������
        * ����ʵ����������ʱ������Ҫ���÷�����ƻ��Scope��Ĺ�������Ϣ��
        * ����new��ʱ����Scopeʵ������������ֵ��ʵ����������Ϊ�������������Ĵ��룺
        * this.myListControl1.Scope = new CustomControlSample.Scope(10, 200)��
        * �����Ҫ���ǵ��� base.ConvertTo(context, culture, value, destinationType)��
        * �㲻��Ҫ�����ת�����ͣ���������ȥ�����ˡ�
        */
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            String result;
            if (destinationType == typeof(String))
            {
                ContextMenuController controller = (ContextMenuController)value;
                result = controller.MenuText.ToString() + "," + controller.IsShow.ToString() + "," + controller.IsSeparator.ToString() + "," + controller.ClickEventName.ToString();
                return result;

            }

            if (destinationType == typeof(InstanceDescriptor))
            {
                ConstructorInfo ci = typeof(ContextMenuController).GetConstructor(new Type[] { typeof(string), typeof(bool), typeof(bool), typeof(string) });
                ContextMenuController controller = (ContextMenuController)value;
                return new InstanceDescriptor(ci, new object[] { controller.MenuText, controller.IsShow, controller.IsSeparator, controller.ClickEventName });
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }


        /*
        * ConvertFrom�Ĵ��룬����ϵͳ�ܹ�ֱ�ӽ�ʵ��������ת��Ϊ Ŀ�� ���ͣ�
        * �������Ǿ�û�б�Ҫ��д���룬����ֻ��Ҫ��ע��ν�String��������������ֵ�����ֵ�ı�
        * ���͵�ֵת��Ϊ Ŀ�� ���͡�û�кܸ��ӵ�ת����ֻ�ǽ�����ַ����ԡ������ֲ𿪣�������ΪInt32����
        * ��Ȼ��newһ��Ŀ�� ���ʵ�������ֲ��ת������������ֵ���� Ŀ�� ��ʵ����
        * Ȼ�󷵻�ʵ��������δ��������Ҫ�ж�һ���û��趨������ֵ�Ƿ���Ч��
        * ���磬����û���Scope�������������ˡ�10200����
        * ����û�����롰�����������޷������Ե�ֵ�ֲ�Ϊ�����ַ�����
        * Ҳ���޷����������ת�������ԣ�����Ҫ�׳�һ���쳣��֪ͨ�û��������롣
        */
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                String[] v = ((String)value).Split(',');
                if (v.GetLength(0) != 4)
                {
                    throw new ArgumentException("Invalid parameter format ��Ҫ���ĸ�����");
                }


                ContextMenuController controller = new ContextMenuController();
                controller.MenuText = Convert.ToString(v[0]);
                controller.IsShow = Convert.ToBoolean(v[1]);
                controller.IsSeparator = Convert.ToBoolean(v[2]);
                controller.ClickEventName = Convert.ToString(v[3]);
                //throw new ArgumentException("112121t ��Ҫ���ĸ�����");
                return controller;
            }
            return base.ConvertFrom(context, culture, value);
        }
        /*
        * Ϊ����������������ܹ������ı༭������
        * �����ǻ�Ҫ��д����������GetPropertiesSupported������GetProperties������
        * ������ScopeConverter���������룺
        * ��GetProperties���������TypeDescriptor�����Scope������е����������������ء�
        * ������TypeDescriptor������Ϥ�Ļ������Բο�MSDN��
        * ��д�����������������Ժ��ڲ��Թ�����鿴�ؼ������ԣ�����Կ���Scope�����µ���ʽ
        */

        //���淽��ʵ�ֶ����Ե������Կɽ��б༭
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(ContextMenuController), attributes);
        }
    }
    #endregion

}
