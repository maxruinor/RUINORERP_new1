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
    ///  Web型的图片选择器
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
        /// 用来保存图片哈希值，用于比较图片是否更改
        /// </summary>
        public string Imagehash { get; set; }


        private string _fileName = string.Empty;


        /// <summary>
        /// 文件名
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
        /// 绝对URL路径
        /// </summary>
        public string AbsoluteUrlPath { get { return _fileName; } }

        /// <summary>
        /// Temp绝对路径
        /// </summary>
        public string AbsolutelocPath
        {
            get
            {
                return Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "temp"), _fileName);
            }
        }



        /// <summary>
        /// 相对路径
        /// </summary>
        public string RelativePath { get { return _fileName; } }


        #region 添加了右键菜单
        public bool Use是否使用内置右键功能 { get; private set; } = true;

        /// <summary>
        /// 因为暂时事件无法通过属性中的数据传输，先用名称再从这里搜索来匹配
        /// </summary>
        private List<EventHandler> ContextClickList = new List<EventHandler>();
        private List<ContextMenuController> _ContextMenucCnfigurator = new List<ContextMenuController>();

        private ContextMenuStrip _ContextMenuStrip;

        /// <summary>
        /// 重写右键菜单 为了合并
        /// 如果使用内置的菜单则这里设置时就要合并处理
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
        /// 获取当前是否处于设计器模式
        /// </summary>
        /// <remarks>
        /// 在程序初始化时获取一次比较准确，若需要时获取可能由于布局嵌套导致获取不正确，如GridControl-GridView组合。
        /// </remarks>
        /// <returns>是否为设计器模式</returns>
        private bool GetIsDesignMode()
        {
            return (this.GetService(typeof(System.ComponentModel.Design.IDesignerHost)) == null
                || LicenseManager.UsageMode == LicenseUsageMode.Designtime);
        }

        /// <summary>
        /// 设置右键菜单，但是对不对参数进行设置。因为是引用的，会改变值
        /// </summary>
        /// <param name="_contextMenuStrip"></param>
        public ContextMenuStrip GetContextMenu(ContextMenuStrip _contextMenuStrip = null)
        {
            // 创建一个全新的右键菜单副本
            ContextMenuStrip newContextMenuStrip = new ContextMenuStrip();

            //初始化右键菜单
            // 初始化内置右键菜单
            ContextMenuStrip internalMenu = new ContextMenuStrip();
            internalMenu.BackColor = System.Drawing.Color.FromArgb(192, 255, 255);

            //如果需要通过设计时对属性值修改。
            //则不能在这个构造函数中操作。因为这时属性值不能获取
            internalMenu.Items.Clear();

            // 合并传入的菜单和内置菜单
            if (_contextMenuStrip != null)
            {
                //外面菜单有设置则加一个分隔符
                if (_contextMenuStrip.Items.Count > 0)
                {
                    ToolStripSeparator MyTss = new ToolStripSeparator();
                    _contextMenuStrip.Items.Add(MyTss);
                }

                ToolStripItem[] ts = new ToolStripItem[_contextMenuStrip.Items.Count];
                _contextMenuStrip.Items.CopyTo(ts, 0);
                internalMenu.Items.AddRange(ts);
            }


            //或者也可以指定内置哪些生效 合并外面的？
            if (Use是否使用内置右键功能)
            {
                #region 生成内置的右键菜单

                if (ContextClickList == null)
                {
                    ContextClickList = new List<EventHandler>();
                }
                ContextClickList.Clear();
                ContextClickList.Add(ContextMenu_查看大图);
                if (_ContextMenucCnfigurator == null)
                {
                    _ContextMenucCnfigurator = new List<ContextMenuController>();
                }
                _ContextMenucCnfigurator.Clear();
                //只是初始化不重复添加
                if (_ContextMenucCnfigurator.Count == 0 && GetIsDesignMode())
                {
                    _ContextMenucCnfigurator.Add(new ContextMenuController("【查看大图】", true, false, "ContextMenu_查看大图"));
                    _ContextMenucCnfigurator.Add(new ContextMenuController("【line】", true, true, ""));
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
                        //如果较多的外部事件也可以做一个集合
                        //if (ehh == null && item.ClickEventName == "删除选中行")
                        //{
                        //    ehh = 删除选中行;
                        //}
                        internalMenu.Items.Add(item.MenuText, null, ehh);
                    }
                }
            }
            newContextMenuStrip = internalMenu;
            // 设置最终的右键菜单
            ContextMenuStrip = newContextMenuStrip;
            return newContextMenuStrip;
        }

        private void ContextMenu_查看大图(object sender, EventArgs e)
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
            // 检查是否按下了 Ctrl+V
            if ((e.Control && e.KeyCode == Keys.V) || (e.Shift && e.KeyCode == Keys.Insert))
            {
                // 检查剪贴板中是否有图像
                if (Clipboard.ContainsImage())
                {


                    // 获取图像
                    System.Drawing.Image image = Clipboard.GetImage();
                    SetImageToPath(image);
                    Control.Value = fileName;
                }
                else if (Clipboard.ContainsText())
                {

                    // 获取文本
                    string text = Clipboard.GetText();
                    // 将文本插入到 TextBox 中
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
                //是否存在
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
                // 处理图像，例如保存到文件
                byte[] buffByte = GetByteImage(image);
                //Control.Value = GetByteImage(image);// GetImage(image);
                // 判断图片大小是否超过 10MB
                if (buffByte.Length > 10000 * 1024)
                {
                    // 压缩图片
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
                        MessageBox.Show("只能接受图片文件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        //    // 创建一个内存流
        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        // 将图像保存到内存流中，指定图像格式
        //        img.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

        //        // 将内存流中的字节数组转换为byte[]
        //        byte[] buffByte = memoryStream.ToArray();
        //        // 判断图片大小是否超过 500KB
        //        if (buffByte.Length > 500 * 1024)
        //        {
        //            // 压缩图片
        //            ImageCodecInfo jpegCodec = GetEncoderInfo(ImageFormat.Jpeg);
        //            EncoderParameters encoderParams = new EncoderParameters(1);
        //            encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);
        //            img = (System.Drawing.Image)new System.Drawing.Bitmap(img, new System.Drawing.Size(800, 600));
        //            img.Save("compressed.jpg", jpegCodec, encoderParams);

        //            // 重新读取压缩后的图片
        //            NewImg = System.Drawing.Image.FromFile("compressed.jpg");
        //            MessageBox.Show("图片大小超过 500KB，已自动压缩。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            // 将压缩后的图片转换为 byte[] 数组
        //            // byte[] compressedImageBytes = File.ReadAllBytes("compressed.jpg");

        //            // 在这里将 compressedImageBytes 保存到数据库中
        //            //_EditEntity.Images = compressedImageBytes;
        //        }
        //        else
        //        {
        //            NewImg = img;
        //        }
        //        // 此时buffByte包含了图像的字节数据
        //        // 你可以对buffByte进行进一步的操作，比如保存到文件或发送到网络

        //        // 释放Image资源
        //        img.Dispose();
        //    }
        //    /*
        //    //将图像读入到字节数组
        //    System.IO.FileStream fs = new System.IO.FileStream(pathName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        //    byte[] buffByte = new byte[fs.Length];
        //    fs.Read(buffByte, 0, (int)fs.Length);
        //    fs.Close();
        //    fs = null;
        //    // 判断图片大小是否超过 500KB
        //    if (buffByte.Length > 500 * 1024)
        //    {
        //        // 压缩图片
        //        ImageCodecInfo jpegCodec = GetEncoderInfo(ImageFormat.Jpeg);
        //        EncoderParameters encoderParams = new EncoderParameters(1);
        //        encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);
        //        img = (System.Drawing.Image)new System.Drawing.Bitmap(img, new System.Drawing.Size(800, 600));
        //        img.Save("compressed.jpg", jpegCodec, encoderParams);

        //        // 重新读取压缩后的图片
        //        img = System.Drawing.Image.FromFile("compressed.jpg");
        //        MessageBox.Show("图片大小超过 500KB，已自动压缩。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        // 将压缩后的图片转换为 byte[] 数组
        //        // byte[] compressedImageBytes = File.ReadAllBytes("compressed.jpg");

        //        // 在这里将 compressedImageBytes 保存到数据库中
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
                /*// 将图像转换为字节数组     
                // 将图像转换为字节数组
                byte[] buffByte = GetByteImage(img);

                // 判断图片大小是否超过 500KB
                if (buffByte.Length > 10000 * 1024)
                {
                    // 压缩图片
                    ImageCodecInfo jpegCodec = GetEncoderInfo(ImageFormat.Jpeg);
                    EncoderParameters encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 50L);

                    using (var ms = new MemoryStream())
                    {
                        img.Save(ms, jpegCodec, encoderParams);
                        byte[] compressedImageBytes = ms.ToArray();

                        // 显示压缩提示
                        MessageBox.Show("图片大小超过 500KB，已自动压缩。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 返回压缩后的字节数组
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
                //保存为图片
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
                    bmp.Save(mostream, System.Drawing.Imaging.ImageFormat.Bmp);//将图像以指定的格式存入缓存内存流
                    bt = new byte[mostream.Length];
                    mostream.Position = 0;//设置留的初始位置
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
