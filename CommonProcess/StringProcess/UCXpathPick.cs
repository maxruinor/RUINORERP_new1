using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonProcess.StringProcess
{
    public partial class UCXpathPick : UCMyBase, IUCBase
    {
        public UCXpathPick()
        {
            InitializeComponent();
        }


        [Browsable(true), Description("引发外部事件")]
        public event OtherHandler OtherEvent;


        public void SaveDataFromUI(UCBasePara aa)
        {
            UCXpathPickPara TagField = new UCXpathPickPara();
            TagField = aa as UCXpathPickPara;
            #region xpath
            TagField.XPath = txtXpath.Text;
            TagField.useInnerHtml = rdb返回值为InnerHtml.Checked;
            TagField.useInnerText = rdb返回值为InnerText.Checked;
            TagField.useOuterHtml = rdb返回值为OuterHtml.Checked;
            TagField.use指定属性值 = rdb返回值为指定属性值.Checked;
            TagField.Xpath指定属性值 = txt指定属性.Text;
            #endregion
        }

        public void LoadDataToUI(UCBasePara aa)
        {
            UCXpathPickPara ConditionsTagField = new UCXpathPickPara();
            ConditionsTagField = aa as UCXpathPickPara;
            #region xpath
            txtXpath.Text = ConditionsTagField.XPath;
            rdb返回值为InnerHtml.Checked = ConditionsTagField.useInnerHtml;
            rdb返回值为InnerText.Checked = ConditionsTagField.useInnerText;
            rdb返回值为OuterHtml.Checked = ConditionsTagField.useOuterHtml;
            rdb返回值为指定属性值.Checked = ConditionsTagField.use指定属性值;
            txt指定属性.Text = ConditionsTagField.Xpath指定属性值;

            #endregion
        }


    }
}
