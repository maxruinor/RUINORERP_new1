using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace HLH.WinControl
{

    /// <summary>
    /// 带有行号显示的富文本框OK
    /// </summary>
    [Description("带有行号显示的富文本框。")]

    //[ToolboxBitmap(typeof(UserDefindControl), "UserDefindControl.bmp")]//新添加的代码
    public class MyRichTextBox : System.Windows.Forms.UserControl
    {

        /*
         Browsable

适用于属性和事件，指定属性或事件是否应该显示在属性浏览器中。

Category

 适用于属性和事件，指定类别的名称，在该类别中将对属性或事件进行分组。当使用了类别时，组件属性和事件可以按逻辑分组显示在属性浏览器中。

Description

适用于属性和事件，定义一小块文本，该文本将在用户选择属性或事件时显示在属性浏览器底部。

Bindable

适用于属性 指定是否要绑定到该属性。

DefaultProperty

适用于属性，（将此特性插入类声明前。）指定组件的默认属性。当用户单击控件时，将在属性浏览器中选定该属性。

DefaultValue

 适用于属性，为属性设置一个简单的默认值。

Editor

 适用于属性，指定在可视设计器中编辑（更改）属性时要使用的编辑器。

Localizable

 适用于属性，指定属性可本地化。当用户要本地化某个窗体时，任何具有该特性的属性都将自动永久驻留到资源文件中。

DesignerSerializationVisibility

适用于属性，指定显示在属性浏览器中的属性是否应该（以及如何）永久驻留在代码中。

TypeConverter

适用于属性，指定将属性的类型转换为另一个数据类型时要使用的类型转换器。

DefaultEvent

 适用于事件，（将此特性插入类声明前。）指定组件的默认事件。这是当用户单击组件时在属性浏览器中选定的事件。
         * 
         
         */

        /// <summary>
        ///  
        /// </summary>
        public string summaryDescription = "1，带有显示行功能;2,从下到上的显示;";

        [Browsable(true), Category("Appearance")]
        public string SummaryDescription
        {
            get { return summaryDescription; }
            set { summaryDescription = value; }
        }


        public RichTextBox richTextBox1;
        int currentLine = 0;
        private RichTextBox richTextBox2;
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.Container components = null;
        public MyRichTextBox()
        {
            InitializeComponent();
            //richTextBox2.VScroll += new EventHandler(richTextBox1_VScroll);
            //richTextBox2.Font = new Font(richTextBox1.Font.FontFamily, richTextBox1.Font.Size + 1.096f);
        }
        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Component Designer generated code
        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器 
        /// 修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.AcceptsTab = true;
            this.richTextBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.ForeColor = System.Drawing.Color.Green;
            this.richTextBox1.Location = new System.Drawing.Point(22, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(226, 216);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            this.richTextBox1.VScroll += new System.EventHandler(this.richTextBox1_VScroll);
            this.richTextBox1.FontChanged += new System.EventHandler(this.richTextBox1_FontChanged);
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            this.richTextBox1.Resize += new System.EventHandler(this.richTextBox1_Resize);
            // 
            // richTextBox2
            // 
            this.richTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.richTextBox2.Location = new System.Drawing.Point(0, 0);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox2.Size = new System.Drawing.Size(22, 216);
            this.richTextBox2.TabIndex = 2;
            this.richTextBox2.Text = "";
            // 
            // MyRichTextBox
            // 
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.richTextBox2);
            this.Name = "MyRichTextBox";
            this.Size = new System.Drawing.Size(248, 216);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// 当前行
        /// </summary>
        public int CurrentLine
        {
            set
            {
                currentLine = value;
            }
            get
            {
                return currentLine;
            }
        }

        private void UpdateNumberLineLabel()
        {
            if (richTextBox1.Lines.Length > 2000)
            {
                richTextBox1.Clear();
                return;
            }
            richTextBox1.Font = richTextBox2.Font;
            Point pos = new Point(0, 0);
            int FirstIndex = richTextBox1.GetCharIndexFromPosition(pos);
            int FirstLine = richTextBox1.GetLineFromCharIndex(FirstIndex);
            pos.X = richTextBox1.Width;
            pos.Y = richTextBox1.Height;
            int EndIndex = richTextBox1.GetCharIndexFromPosition(pos);
            int EndLine = richTextBox1.GetLineFromCharIndex(EndIndex);
            int MyStart = richTextBox1.SelectionStart;
            int MyLine = richTextBox1.GetLineFromCharIndex(MyStart) + 1;
            pos = richTextBox1.GetPositionFromCharIndex(EndIndex);
            if (EndIndex > currentLine || EndIndex < currentLine)
            {
                richTextBox2.Text = "";
                for (int i = FirstLine; i < EndLine + 1; i++)
                {
                    richTextBox2.Text += i + 1 + "\n";
                }
            }
            currentLine = EndIndex;
            richTextBox1.HideSelection = false;
        }
        private void richTextBox1_TextChanged(object sender, System.EventArgs e)
        {
            UpdateNumberLineLabel();
        }
        private void richTextBox1_VScroll(object sender, System.EventArgs e)
        {

            int p = richTextBox1.GetPositionFromCharIndex(0).Y % (richTextBox1.Font.Height + 1);
            richTextBox2.Location = new Point(0, p);
            UpdateNumberLineLabel();

        }
        private void richTextBox1_Resize(object sender, System.EventArgs e)
        {
            richTextBox1.Font = richTextBox2.Font;
            UpdateNumberLineLabel();
        }
        private void richTextBox1_FontChanged(object sender, System.EventArgs e)
        {
            UpdateNumberLineLabel();
            richTextBox1_VScroll(null, null);
        }

    }
}


