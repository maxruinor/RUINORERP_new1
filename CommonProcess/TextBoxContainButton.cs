using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonProcess
{

    //https://www.cnblogs.com/yepoint/p/7238667.html
    //http://blog.chinaunix.net/uid-25498312-id-3658927.html
    public partial class TextBoxContainButton : UserControl
    {
        public TextBoxContainButton()
        {
            InitializeComponent();
        }



    [Category("自定义"), Description("按钮图片")]

    public Image ButtonImage

    {

        set

        {

            button.Image = value;

        }

        get

        {

            return button.Image;

        }

    }

    [Category("自定义"), Description("文本框鼠标悬停提示文本")]

    public string TextBoxToolTipText

    {

        set

        {

            toolTip1.SetToolTip(this.textBoxFront, value);

        }

        get

        {

            return toolTip1.GetToolTip(this.textBoxFront);

        }

    }

    [Category("自定义"), Description("按钮鼠标悬停提示文本")]

    public string ButtonToolTipText

    {

        set

        {

            toolTip1.SetToolTip(this.textBoxFront, value);

        }

        get

        {

            return toolTip1.GetToolTip(this.textBoxFront);

        }

    }

    [Category("自定义"), Description("文本框内文本")]

    public string TextBoxText

    {

        set

        {

            textBoxFront.Text = value;

        }

        get

        {

            return textBoxFront.Text;

        }

    }

    private bool textBoxReadOnly = false;

    [Category("自定义"), Description("文本框只读")]

    public bool TextBoxReadOnly

    {

        set

        {

            textBoxReadOnly = value;

            textBoxFront.ReadOnly = textBoxReadOnly;

            textBoxFront.ReadOnly = textBoxReadOnly;

        }

        get

        {

            return textBoxReadOnly;

        }

    }

    [Category("自定义"), Description("文本框背景色")]

    public Color TextBoxBackColor

    {

        set

        {

            textBoxBack.BackColor = value;

            textBoxFront.BackColor = value;

        }

        get

        {

            return textBoxBack.BackColor;

        }

    }

    public delegate void ButtonClickEventHandler(Object sender, EventArgs e);

    public event ButtonClickEventHandler ButtonClick; //声明事件

    protected virtual void OnButtonClick(EventArgs e)

    {

        if (ButtonClick != null)

        { // 如果有对象注册

            ButtonClick(this, e); // 调用所有注册对象的方法

        }

    }

    private void button_Click(object sender, EventArgs e)

    {

        OnButtonClick(e); // 调用 OnButtonClick方法

    }
}
}