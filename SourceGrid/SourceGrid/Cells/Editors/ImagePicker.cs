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
                #region 判断图片大小是否超过 500KB
                //将图像读入到字节数组

                byte[] buffByte = GetByteImage(val as System.Drawing.Image);
                // 判断图片大小是否超过 500KB
                if (buffByte.Length > 500 * 1024)
                {
                    // 压缩图片
                    ImageCodecInfo jpegCodec = GetEncoderInfo(ImageFormat.Jpeg);
                    EncoderParameters encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);
                    img = (System.Drawing.Image)new System.Drawing.Bitmap(img, new System.Drawing.Size(800, 600));
                    img.Save("compressed.jpg", jpegCodec, encoderParams);

                    // 重新读取压缩后的图片
                    img = System.Drawing.Image.FromFile("compressed.jpg");

                    MessageBox.Show("图片大小超过 500KB，已自动压缩。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // 将压缩后的图片转换为 byte[] 数组
                    byte[] compressedImageBytes = File.ReadAllBytes("compressed.jpg");

                    // 在这里将 compressedImageBytes 保存到数据库中
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
        /// 是否为分割线
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

    #region Converter 类
    /// <summary>
    /// 三个方法CanConvertFrom，CanConvertTo，ConvertTo，ConvertFrom
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
        * ConvertTo的实现，如果转换的目标类型是string，
        * 我将Scope的两个属性转换成string类型，并且用一个“，”连接起来，
        * 这就是我们在属性浏览器里看到的表现形式，
        * 
        * 如果转换的目标类型是实例描述器（InstanceDescriptor，
        * 它负责生成实例化的代码），我们需要构造一个实例描述器，
        * 构造实例描述器的时候，我们要利用反射机制获得Scope类的构造器信息，
        * 并在new的时候传入Scope实例的两个属性值。实例描述器会为我们生成这样的代码：
        * this.myListControl1.Scope = new CustomControlSample.Scope(10, 200)；
        * 在最后不要忘记调用 base.ConvertTo(context, culture, value, destinationType)，
        * 你不需要处理的转换类型，交给基类去做好了。
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
        * ConvertFrom的代码，由于系统能够直接将实例描述器转换为 目标 类型，
        * 所以我们就没有必要再写代码，我们只需要关注如何将String（在属性浏览出现的属性值的表达）
        * 类型的值转换为 目标 类型。没有很复杂的转换，只是将这个字符串以“，”分拆开，并串换为Int32类型
        * ，然后new一个目标 类的实例，将分拆后转换的两个整型值赋给 目标 的实例，
        * 然后返回实例。在这段代码里，我们要判断一下用户设定的属性值是否有效。
        * 比如，如果用户在Scope属性那里输入了“10200”，
        * 由于没有输入“，”，我们无法将属性的值分拆为两个字符串，
        * 也就无法进行下面的转换，所以，我们要抛出一个异常，通知用户重新输入。
        */
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                String[] v = ((String)value).Split(',');
                if (v.GetLength(0) != 4)
                {
                    throw new ArgumentException("Invalid parameter format 需要有四个数据");
                }


                ContextMenuController controller = new ContextMenuController();
                controller.MenuText = Convert.ToString(v[0]);
                controller.IsShow = Convert.ToBoolean(v[1]);
                controller.IsSeparator = Convert.ToBoolean(v[2]);
                controller.ClickEventName = Convert.ToString(v[3]);
                //throw new ArgumentException("112121t 需要有四个数据");
                return controller;
            }
            return base.ConvertFrom(context, culture, value);
        }
        /*
        * 为了在属性浏览器里能够独立的编辑子属性
        * ，我们还要重写两个方法：GetPropertiesSupported（）和GetProperties（）；
        * 下面是ScopeConverter的完整代码：
        * 在GetProperties方法里，我用TypeDescriptor获得了Scope类的所有的属性描述器并返回。
        * 如果你对TypeDescriptor还不熟悉的话，可以参考MSDN。
        * 重写这两个方法并编译以后，在测试工程里查看控件的属性，你可以看到Scope是如下的形式
        */

        //下面方法实现对属性的子属性可进行编辑
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
