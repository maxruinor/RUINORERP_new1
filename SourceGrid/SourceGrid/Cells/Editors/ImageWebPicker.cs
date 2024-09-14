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
    ///  Web�͵�ͼƬѡ����
    /// </summary>
    [System.ComponentModel.ToolboxItem(false)]
    public class ImageWebPicker : EditorControlBase
    {
        public readonly static ImageWebPicker Default = new ImageWebPicker();

        #region Constructor
        /// <summary>
        /// Construct an Editor of type ImagePicker.
        /// </summary>
        public ImageWebPicker() : base(typeof(byte[]))
        {
        }
        #endregion

        public System.Drawing.Image PickerImage { get; set; }

        /// <summary>
        /// ��������ͼƬ��ϣֵ�����ڱȽ�ͼƬ�Ƿ����
        /// </summary>
        public string Imagehash { get; set; }


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
            DevAge.Windows.Forms.TextBoxUITypeEditor editor = new DevAge.Windows.Forms.TextBoxUITypeEditor();
            editor.BorderStyle = DevAge.Drawing.BorderStyle.None;
            editor.Validator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(System.Drawing.Image));
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
            string NewHash = string.Empty;
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = Guid.NewGuid().ToString() + ".jpg";
            }
            else
            {
                //�Ƿ����
                if (File.Exists(AbsolutelocPath))
                {
                    NewHash = ImagePickerHelper.GenerateHash(image);
                    if (string.IsNullOrEmpty(Imagehash))
                    {
                        Imagehash = NewHash;
                        return;
                    }
                    else if (!ImagePickerHelper.AreHashesEqual(Imagehash, NewHash))
                    {
                        Imagehash = NewHash;
                        return;
                    }
                }
            }
            if (image != null)
            {
                // ����ͼ�����籣�浽�ļ�
                byte[] buffByte = GetByteImage(image);
                //Control.Value = GetByteImage(image);// GetImage(image);
                // �ж�ͼƬ��С�Ƿ񳬹� 10MB
                if (buffByte.Length > 10000 * 1024)
                {
                    // ѹ��ͼƬ
                    ImageCodecInfo jpegCodec = GetEncoderInfo(ImageFormat.Jpeg);
                    EncoderParameters encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 75L);
                    image = (System.Drawing.Image)new System.Drawing.Bitmap(image, new System.Drawing.Size(1000, 1000));
                    image.Save(AbsolutelocPath, jpegCodec, encoderParams);
                    byte[] NewBuffByte =GetByteImage(image);
                    NewHash = ImagePickerHelper.GenerateHash(NewBuffByte);
                }
                else
                {
                    image.Save(AbsolutelocPath, ImageFormat.Jpeg);
                    NewHash = ImagePickerHelper.GenerateHash(buffByte);
                }
         
                if (Imagehash != NewHash)
                {
                    Imagehash = NewHash;
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
                        Control.Value = fileName;
                        ValueType = typeof(string);
                    }
                    else
                    {
                        MessageBox.Show("ֻ�ܽ���ͼƬ�ļ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
        //private System.Drawing.Image GetImage(string pathName)
        //{
        //    System.Drawing.Image img = System.Drawing.Image.FromFile(pathName);
        //    return GetImage(img);
        //}
        //private System.Drawing.Image GetImage(System.Drawing.Image img)
        //{
        //    System.Drawing.Image NewImg = null;
        //    // ����һ���ڴ���
        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        // ��ͼ�񱣴浽�ڴ����У�ָ��ͼ���ʽ
        //        img.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

        //        // ���ڴ����е��ֽ�����ת��Ϊbyte[]
        //        byte[] buffByte = memoryStream.ToArray();
        //        // �ж�ͼƬ��С�Ƿ񳬹� 500KB
        //        if (buffByte.Length > 500 * 1024)
        //        {
        //            // ѹ��ͼƬ
        //            ImageCodecInfo jpegCodec = GetEncoderInfo(ImageFormat.Jpeg);
        //            EncoderParameters encoderParams = new EncoderParameters(1);
        //            encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);
        //            img = (System.Drawing.Image)new System.Drawing.Bitmap(img, new System.Drawing.Size(800, 600));
        //            img.Save("compressed.jpg", jpegCodec, encoderParams);

        //            // ���¶�ȡѹ�����ͼƬ
        //            NewImg = System.Drawing.Image.FromFile("compressed.jpg");
        //            MessageBox.Show("ͼƬ��С���� 500KB�����Զ�ѹ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            // ��ѹ�����ͼƬת��Ϊ byte[] ����
        //            // byte[] compressedImageBytes = File.ReadAllBytes("compressed.jpg");

        //            // �����ｫ compressedImageBytes ���浽���ݿ���
        //            //_EditEntity.Images = compressedImageBytes;
        //        }
        //        else
        //        {
        //            NewImg = img;
        //        }
        //        // ��ʱbuffByte������ͼ����ֽ�����
        //        // ����Զ�buffByte���н�һ���Ĳ��������籣�浽�ļ����͵�����

        //        // �ͷ�Image��Դ
        //        img.Dispose();
        //    }
        //    /*
        //    //��ͼ����뵽�ֽ�����
        //    System.IO.FileStream fs = new System.IO.FileStream(pathName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        //    byte[] buffByte = new byte[fs.Length];
        //    fs.Read(buffByte, 0, (int)fs.Length);
        //    fs.Close();
        //    fs = null;
        //    // �ж�ͼƬ��С�Ƿ񳬹� 500KB
        //    if (buffByte.Length > 500 * 1024)
        //    {
        //        // ѹ��ͼƬ
        //        ImageCodecInfo jpegCodec = GetEncoderInfo(ImageFormat.Jpeg);
        //        EncoderParameters encoderParams = new EncoderParameters(1);
        //        encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);
        //        img = (System.Drawing.Image)new System.Drawing.Bitmap(img, new System.Drawing.Size(800, 600));
        //        img.Save("compressed.jpg", jpegCodec, encoderParams);

        //        // ���¶�ȡѹ�����ͼƬ
        //        img = System.Drawing.Image.FromFile("compressed.jpg");
        //        MessageBox.Show("ͼƬ��С���� 500KB�����Զ�ѹ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        // ��ѹ�����ͼƬת��Ϊ byte[] ����
        //        // byte[] compressedImageBytes = File.ReadAllBytes("compressed.jpg");

        //        // �����ｫ compressedImageBytes ���浽���ݿ���
        //        //_EditEntity.Images = compressedImageBytes;
        //    }
        //    else
        //    {
        //        // _EditEntity.Images = buffByte;
        //    }*/
        //    return NewImg;
        //}

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
                /*// ��ͼ��ת��Ϊ�ֽ�����     
                // ��ͼ��ת��Ϊ�ֽ�����
                byte[] buffByte = GetByteImage(img);

                // �ж�ͼƬ��С�Ƿ񳬹� 500KB
                if (buffByte.Length > 10000 * 1024)
                {
                    // ѹ��ͼƬ
                    ImageCodecInfo jpegCodec = GetEncoderInfo(ImageFormat.Jpeg);
                    EncoderParameters encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 50L);

                    using (var ms = new MemoryStream())
                    {
                        img.Save(ms, jpegCodec, encoderParams);
                        byte[] compressedImageBytes = ms.ToArray();

                        // ��ʾѹ����ʾ
                        MessageBox.Show("ͼƬ��С���� 500KB�����Զ�ѹ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // ����ѹ������ֽ�����
                        return compressedImageBytes;
                    }
                }
                else
                {
                    using (var ms = new MemoryStream())
                    {
                        img.Save(ms, img.RawFormat);
                        return ms.ToArray();
                    }
                }*/
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
            object val = Control.Tag;
            if (val == null)
            {
                return null;
            }

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
            else if (val is string)
            {
                fileName = (string)val;
            }
            return val;
        }
    }




}
