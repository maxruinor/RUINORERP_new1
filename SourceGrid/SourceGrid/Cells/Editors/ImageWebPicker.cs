using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Drawing.Imaging;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Collections.Generic;
using SourceGrid.Cells;



namespace SourceGrid.Cells.Editors
{
    /// <summary>
    ///  Web�͵�ͼƬѡ����, ��һ�ж���һ���ġ�����ֻ��һ�����š����ݵ���cellΪ��λ����
    /// </summary>
    [System.ComponentModel.ToolboxItem(false)]
    public class ImageWebPicker : EditorControlBase
    {
        public readonly static ImageWebPicker Default = new ImageWebPicker();

        #region Constructor
        /// <summary>
        /// Construct an Editor of type ImagePicker.
        /// </summary>
        //public ImageWebPicker() : base(typeof(byte[]))
        //{
        //}
        ///web����ͼƬ ֻ����ʾͼƬ����
        public ImageWebPicker() : base(typeof(string))
        {
        }


        #endregion

        public System.Drawing.Image PickerImage { get; set; }



        private string _fileName = string.Empty;


        /// <summary>
        /// �ļ���
        /// </summary>
        public string fileName
        {
            get { return _fileName; }

            set
            {
                _fileName = value;
            }
        }


        /// <summary>
        /// ����URL·��
        /// </summary>
        public string AbsoluteUrlPath { get { return _fileName; } }

        /// <summary>
        /// Temp����·��
        /// </summary>
        public string AbsolutelocPath
        {
            get
            {
                return Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "temp"), _fileName);
            }
        }



        /// <summary>
        /// ���·��
        /// </summary>
        public string RelativePath { get { return _fileName; } }


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
            if (PickerImage != null)
            {
                DevAge.Windows.Forms.frmPictureViewer frm = new DevAge.Windows.Forms.frmPictureViewer();
                frm.PictureBoxViewer.Image = PickerImage;
                frm.ShowDialog();
            }
        }
        #endregion



        #region Edit Control
        /// <summary>
        /// Create the editor control
        /// </summary>
        /// <returns></returns>
        protected override Control CreateControl()
        {
            //DevAge.Windows.Forms.TextBoxUITypeEditor editor = new DevAge.Windows.Forms.TextBoxUITypeEditor();
            DevAge.Windows.Forms.TextBoxUITypeEditorWebImage editor = new DevAge.Windows.Forms.TextBoxUITypeEditorWebImage();

            editor.BorderStyle = DevAge.Drawing.BorderStyle.None;
            //editor.Validator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(System.Drawing.Image));
            editor.Validator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(string));

            editor.ContextMenuStrip = GetContextMenu();
            editor.TextBox.AllowDrop = true;
            editor.TextBox.DragDrop += Editor_DragDrop;
            editor.TextBox.DragEnter += Editor_DragEnter;
            editor.TextBox.KeyDown += Editor_KeyDown;
            return editor;
        }

        private void Editor_KeyDown(object sender, KeyEventArgs e)
        {
            // ����Ƿ����� Ctrl+V
            if ((e.Control && e.KeyCode == Keys.V) || (e.Shift && e.KeyCode == Keys.Insert))
            {
                // �����������Ƿ���ͼ��
                if (Clipboard.ContainsImage())
                {
                    // ��ȡͼ��
                    System.Drawing.Image image = Clipboard.GetImage();
                    SetImageToPath(image);
                    ValueType = typeof(string);
                    Control.Value = fileName;

                }
                else if (Clipboard.ContainsText())
                {

                    // ��ȡ�ı�
                    string text = Clipboard.GetText();
                    // ���ı����뵽 TextBox ��
                    if (Control != null)
                    {
                        //var tb = Control as TextBox;
                        //tb.SelectedText = text;
                    }

                }
            }
        }

        private void SetImageToPath(System.Drawing.Image image)
        {

            var model = this.EditCell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
            SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = (SourceGrid.Cells.Models.ValueImageWeb)model;
            fileName = valueImageWeb.CellImageName;


            if (image != null)
            {           //�Ƿ���� ���ܣ� ����������Ƚϡ���һ���͸���
                        //if (File.Exists(AbsolutelocPath))
                        //{
                        //NewHash = ImageHashHelper.GenerateHash(image);
                        //if (string.IsNullOrEmpty(Imagehash) || !ImageHashHelper.AreHashesEqual(Imagehash, NewHash))
                        //{
                        //    Imagehash = NewHash;
                        //    ImageProcessor.SaveImageAsFile(image, AbsolutelocPath);
                        //    return;
                        //}
                        //}
                        //else
                        //{

                //}
                #region 
                byte[] buffByte = ImageProcessor.CompressImage(image);
                string NewHash = ImageHashHelper.GenerateHash(buffByte);
                #endregion
                if (string.IsNullOrEmpty(valueImageWeb.CellImageHash) || !ImageHashHelper.AreHashesEqual(valueImageWeb.CellImageHash, NewHash))
                {
                    valueImageWeb.CellImageHash = NewHash;
                    //��ͼƬ���浽�ڴ��С����ں������ʾ���򱣴浽������ʱ�ļ����У����ϴ���������
                    byte[] destination = new byte[buffByte.Length];
                    Buffer.BlockCopy(buffByte, 0, destination, 0, buffByte.Length);
                    valueImageWeb.CellImageBytes = destination;
                    //ImageProcessor.SaveBytesAsImage(buffByte, AbsolutelocPath);
                }
                ValueType = typeof(string);
            }
        }

        private void Editor_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Editor_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    string filePath = files[0];
                    if (filePath.ToLower().EndsWith(".png") || filePath.ToLower().EndsWith(".jpg") || filePath.ToLower().EndsWith(".jpeg") || filePath.ToLower().EndsWith(".bmp"))
                    {
                        var img = System.Drawing.Image.FromFile(filePath);

                        SetImageToPath(img);
                        ValueType = typeof(string);
                        Control.Value = fileName;
                    }
                    else
                    {
                        MessageBox.Show("ֻ�ܽ���ͼƬ�ļ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }


        /// <summary>
        /// Gets the control used for editing the cell.
        /// </summary>
        public new DevAge.Windows.Forms.TextBoxUITypeEditorWebImage Control
        {
            get
            {
                return (DevAge.Windows.Forms.TextBoxUITypeEditorWebImage)base.Control;
            }
        }


        #endregion


        public override bool SetCellTagValue(CellContext cellContext, object p_NewTagValue)
        {
            Control.Tag = p_NewTagValue;
            return base.SetCellTagValue(cellContext, p_NewTagValue); ;
        }
        public override object GetEditedValue()
        {
            string path = string.Empty;
            object val = Control.Value;
            if (val == null)
                return null;
            else if (val is System.Drawing.Image img)
            {
                SetImageToPath(img);
                ValueType = typeof(string);
                Control.Value = fileName;

            }
            else if (val is byte[])
            {
                byte[] bytes = val as byte[];
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    PickerImage = System.Drawing.Image.FromStream(ms, true);
                }
                return val;
            }
            else if (val is string)
            {
                //����ΪͼƬ
                //string newIamgeFilePath = @"../temp/" + new Guid().ToString();
                //System.IO.File.Exists(newIamgeFilePath);
                //Control.Value = val;//= newIamgeFilePath;
                return val;
            }
            path = Control.Value.ToString();
            return path;
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
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
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
            var model = this.EditCell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
            SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = (SourceGrid.Cells.Models.ValueImageWeb)model;
            Control.Tag = valueImageWeb.CellImageBytes;
            if (valueImageWeb.CellImageBytes != null)
            {
                return valueImageWeb.CellImageBytes;
            }
            else
            {

            }
            object val = Control.Value;
            if (val is System.Drawing.Image)
            {
                DevAge.ComponentModel.Validator.ValidatorTypeConverter imageValidator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(System.Drawing.Image));
                return imageValidator.ValueToObject(val, typeof(byte[]));
            }
            else if (val is byte[])
                return val;
            else if (val is string)
            {
            }
            return val;  
        }
    }




}
