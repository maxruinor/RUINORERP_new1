using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HLH.WinControl.ControlLib;

namespace CommonProcess.QueryProcess
{

    //https://www.cnblogs.com/lhyqzx/p/7003240.html
    //https://www.cnblogs.com/lhyqzx/p/7007623.html
    //重要的控件属性说明文章,这个人的可以重点看一下其他文章，自定义控件这块。有深度

    /*
     
    自定义控件在工具箱中的图标显示：
可以使用已有控件的图标，
1 [ToolboxBitmap(typeof(System.Windows.Forms.PictureBox))]
2 public partial class UserControl1 : UserControl
  如果不想用系统的图标，要使用自己的图标，可以这样（这部分未测试，源于参考文章）
    [ToolboxBitmap(typeof(MyPanel), "WindowsApplication1.Images.MyPanel.bmp")]
 public class MyPanel : UserControl
     */
    // 自定义控件显示在工具箱的开关：
    //将true改为false就可以不显示了。

    [ToolboxItem(true)]
    [Designer(typeof(ConsumablesButtonDesigner))]
    public partial class UCMultirowkeyselector : Button
    {
        public UCMultirowkeyselector()
        {
            InitializeComponent();
            //this.Text = "...";
            this.Click += UCMultirowkeyselector_Click;
            this.Text = "999";
        }

        private string _Text = "...";

        //[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [Category("Z测试而已"), DefaultValue("...")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public Label label = new Label();

        /// <summary>
        /// 控件的文字显示
        /// </summary>
        private string textEX = "abcd";
        [Description("显示的文字"), DefaultValue("asdfa")]
        public string TextEX
        {
            get { return textEX; }
            set
            {
                textEX = value;
            }
        }

        private string _BtnName = "bt1";
        /*
         5）DesignerSerializationVisibility：代码生成器生成组件相关代码的方式
DesignerSerializationVisibilityAttribute（MSDN）用于指定在设计时序列化组件上的属性时所使用的持久性类型。

参数为DesignerSerializationVisibility类型的枚举：

Hidden：代码生成器不生成对象的代码

Visible：代码生成器生成对象的代码

Content：代码生成器产生对象内容的代码，而不是对象本身的代码
         */
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [Category("Z测试而已"), DefaultValue("***")]
        public string BtnName
        {
            get
            {
                return _BtnName;
            }
            set
            {
                _BtnName = value;
                //这样可以做到生成时，为指定值
                this.Text = _BtnName;
            }
        }



        [Category("Z结果格式")]
        [TypeConverter(typeof(OptionValuesTypeConvertor))]
        [OptionCollection("'x','x'", "x,x", null)]
        public string ResultFormat { get; set; } = "'x','x'";


        [Category("Z测试而已")]
        private String Databasetype = "MySql";
        [TypeConverter(typeof(DataBaseTypeConverter)), CategoryAttribute("数据库")]
        public String DataBaseType
        {
            get { return Databasetype; }
            set { Databasetype = value; }
        }

        //给返回值设置不同的格式

        #region 测试而已

        /// <summary>
        /// 多条件的字段作用对象
        /// </summary>
        [Description("多条件的字段作用对象")]
        public TextBox TargetTextbox { get; set; }

        //[Browsable(true)]
        //[Description("属性描述"), Category("属性类别"), DefaultValue("选择多个单号")]
        //[TypeConverter(typeof(OptionValuesTypeConvertor))]
        //[OptionCollection("...", "选择多个单号", null)]
        //public new string Text { get; set; } = "选择多个单号";

        //[Browsable(true)]
        //[Description("设置控件图片"), Category("setPic"), DefaultValue(" ")]
        //public Bitmap setPic
        //{
        //    get { return (Bitmap)this.pictureBox1.Image; }
        //    set
        //    {
        //        this.pictureBox1.Image = value;
        //    }
        //}

        public enum indexEnum
        {
            a,
            b,
            c
        }
        public indexEnum index;
        [Browsable(true)]
        [Description("设置index"), Category("Z测试而已"), DefaultValue("属性默认值")]
        public indexEnum Index
        {
            get { return index; }
            set { index = value; }
        }
        #endregion


        public string MultiKeys { get; set; }

        private void UCMultirowkeyselector_Click(object sender, EventArgs e)
        {
            frmMultiRowkeyselector frm = new frmMultiRowkeyselector();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //如果返回格式需要处理，这里可以 选择 或传到 窗体再处理
                MultiKeys = frm.MultiKeys;
                this.TargetTextbox.Text = MultiKeys;
            }
        }















    }
}
