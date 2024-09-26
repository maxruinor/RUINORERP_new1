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
    ///  Web型的图片选择器, 第一列都是一样的。所以只是一个过桥。数据得以cell为单位保存
    /// 创建列时的可编辑的列的编辑器。一列共用一个编辑器
    /// 对话框选择图片文件，和内存中粘贴，以及拖入的图片处理。都是通过这个编辑器来实现的
    /// 并且保存在当前的control.tag和model中
    /// </summary>
    [System.ComponentModel.ToolboxItem(false)]
    public class ImageWebPickEditor : EditorControlBase
    {



        public readonly static ImageWebPickEditor Default = new ImageWebPickEditor(typeof(string));

        #region Constructor
        /// <summary>
        /// Construct an Editor of type ImagePicker.
        /// </summary>
        //public ImageWebPicker() : base(typeof(byte[]))
        //{
        //}
        ///web下载图片 只是显示图片名称
        //public ImageWebPickEditor() : base(typeof(string))
        public ImageWebPickEditor(Type p_Type) : base(p_Type)
        {

        }


        #endregion

        public System.Drawing.Image PickerImage { get; set; }



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
        /// Temp绝对路径
        /// </summary>
        public string AbsolutelocPath
        {
            get
            {
                return Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "temp"), _fileName);
            }
        }





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
            // DevAge.Windows.Forms.TextBoxUITypeEditor editor = new DevAge.Windows.Forms.TextBoxUITypeEditor();
            DevAge.Windows.Forms.TextBoxUITypeEditorWebImage editor = new DevAge.Windows.Forms.TextBoxUITypeEditorWebImage();

            editor.BorderStyle = DevAge.Drawing.BorderStyle.None;
            //editor.Validator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(System.Drawing.Image));
            editor.Validator = new DevAge.ComponentModel.Validator.ValidatorImagePathTypeConverter(typeof(string));

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
                    ValueType = typeof(string);
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

        /// <summary>
        /// 拖入或内存中图片转换。
        /// 如果
        /// </summary>
        /// <param name="image"></param>
        private void SetImageToPath(System.Drawing.Image image)
        {
            var model = this.EditCell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
            SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = (SourceGrid.Cells.Models.ValueImageWeb)model;

            if (image != null)
            {
                #region 
                byte[] buffByte = ImageProcessor.CompressImage(image);

                //得到图片的hash值
                string NewHash = ImageHashHelper.GenerateHash(buffByte);
                #endregion
                //if (string.IsNullOrEmpty(valueImageWeb.CellImageHashName) 
                //    || !ImageHashHelper.AreHashesEqual(valueImageWeb.CellImageHashName, NewHash))
                //{
                if (!AreHashesEqual(valueImageWeb.GetImageNewHash(), NewHash))
                {
                    valueImageWeb.SetImageNewHash(NewHash);
                    //将图片保存到内存中。用于后面的显示，或保存到本地临时文件夹中，或上传到服务器
                    byte[] destination = new byte[buffByte.Length];
                    Buffer.BlockCopy(buffByte, 0, destination, 0, buffByte.Length);
                    valueImageWeb.CellImageBytes = destination;
                    Control.Tag = destination;
                    //如果先对话框。再拖拽，则对话框会覆盖拖拽的值。所以这里要清空对话框返回的路径。
                    byte[] bytes = destination as byte[];
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        PickerImage = System.Drawing.Image.FromStream(ms, true);
                    }
                }
                Control.Value = valueImageWeb.CellImageHashName;
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
                        MessageBox.Show("只能接受图片文件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        /// <summary>
        /// This method is called just before the edit start. You can use this method to customize the editor with the cell informations.
        /// </summary>
        /// <param name="cellContext"></param>
        /// <param name="editorControl"></param>
        protected override void OnStartingEdit(CellContext cellContext, Control editorControl)
        {
            base.OnStartingEdit(cellContext, editorControl);
            DevAge.Windows.Forms.TextBoxUITypeEditorWebImage l_TxtBox = (DevAge.Windows.Forms.TextBoxUITypeEditorWebImage)editorControl;
            l_TxtBox.TextBox.WordWrap = cellContext.Cell.View.WordWrap;
            l_TxtBox.TextBox.TextAlign = DevAge.Windows.Forms.Utilities.ContentToHorizontalAlignment(cellContext.Cell.View.TextAlignment);
            //to set the scroll of the textbox to the initial position (otherwise the textbox use the previous scroll position)
            l_TxtBox.TextBox.SelectionStart = 0;
            l_TxtBox.TextBox.SelectionLength = 0;
        }

        /// <summary>
        /// 对话框会直接到这里
        /// </summary>
        /// <returns></returns>
        public override object GetEditedValue()
        {
            var model = this.EditCell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
            SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = (SourceGrid.Cells.Models.ValueImageWeb)model;

            //这里取值。要判断比较哪个最新通过哈希值比较一下。
            if (Control is DevAge.Windows.Forms.TextBoxUITypeEditorWebImage txtWebImage)
            {
                if (!string.IsNullOrEmpty(txtWebImage.SelectedFilePath))
                {
                    byte[] NewbuffByte = ImageProcessor.CompressImage(txtWebImage.SelectedFilePath);
                    string NewHash = ImageHashHelper.GenerateHash(NewbuffByte);

                    //看原来有不有哈希值或新旧是否相同，如果不同则更新
                    if (!AreHashesEqual(valueImageWeb.GetImageNewHash(), NewHash))
                    {
                        //将图片保存到内存中。用于后面的显示，或保存到本地临时文件夹中，或上传到服务器
                        byte[] destination = new byte[NewbuffByte.Length];
                        Buffer.BlockCopy(NewbuffByte, 0, destination, 0, NewbuffByte.Length);
                        valueImageWeb.CellImageBytes = destination;
                        valueImageWeb.SetImageNewHash(NewHash);
                        Control.Tag = destination;
                    }
                    txtWebImage.SelectedFilePath = string.Empty;//用完了清空。
                    //return valueImageWeb.CellImageName;
                }
            }
            //三种形式都将byte[]保存到tag中
            if (Control.Value != null && !string.IsNullOrEmpty(Control.Value.ToString()) && valueImageWeb.CellImageBytes != null && valueImageWeb.CellImageBytes.Length > 0)
            {
                Control.Tag = valueImageWeb.CellImageBytes;
            }

            object val = Control.Tag;
            if (val == null)
                return null;
            else if (val is System.Drawing.Image img)
            {
                SetImageToPath(img);
                //ValueType = typeof(string);
                Control.Tag = null;//清空。让第二个单元格可以选择新的图片。

            }
            else if (val is byte[] buffByte)
            {
                //实际上比较一下。如果还是相同的图片不用赋值
                string NewHash = ImageHashHelper.GenerateHash(buffByte);
                //看原来有不有哈希值或新旧是否相同，如果不同则更新
                if (!AreHashesEqual(valueImageWeb.GetImageNewHash(), NewHash))
                {
                    byte[] NewbuffByte = val as byte[];
                    byte[] destination = new byte[NewbuffByte.Length];
                    Buffer.BlockCopy(NewbuffByte, 0, destination, 0, NewbuffByte.Length);
                    valueImageWeb.CellImageBytes = destination;
                    valueImageWeb.SetImageNewHash(NewHash);
                    Control.Tag = destination;
                    using (MemoryStream ms = new MemoryStream(NewbuffByte))
                    {
                        PickerImage = System.Drawing.Image.FromStream(ms, true);
                    }
                }
            }
            else if (val is string)
            {
                //保存为图片
                //string newIamgeFilePath = @"../temp/" + new Guid().ToString();
                //System.IO.File.Exists(newIamgeFilePath);
                //Control.Value = val;//= newIamgeFilePath;
                return val;
            }
            Control.Value = valueImageWeb.CellImageHashName;
            return Control.Value;
        }


        //比较两个byte[]是否相同，使用哈希值来比较
        /// <summary>
        /// 比较两个byte[]是否相同，使用哈希值来比较
        /// </summary>
        /// <param name="hash1"></param>
        /// <param name="hash2"></param>
        /// <returns></returns>
        public static bool AreHashesEqual(string hash1, string hash2)
        {
            //如果哈希值为空，认为两个byte[]相同
            if (string.IsNullOrEmpty(hash1) && string.IsNullOrEmpty(hash2))
            {
                return true;
            }
            if (!string.IsNullOrEmpty(hash1) && string.IsNullOrEmpty(hash2))
            {
                return false;
            }
            if (string.IsNullOrEmpty(hash1) && !string.IsNullOrEmpty(hash2))
            {
                return false;
            }
            //如果哈希值相同，则认为两个byte[]相同
            return ImageHashHelper.AreHashesEqual(hash1, hash2);
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
            return Control.Tag;
        }

        /*
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
        */
    }




}
