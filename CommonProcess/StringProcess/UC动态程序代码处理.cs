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
    public partial class UC动态程序代码处理 : UCMyBase, IUCBase
    {
        public UC动态程序代码处理()
        {
            InitializeComponent();
        }

        private List<string> referenceDlls = new List<string>();

        public List<string> ReferenceDlls
        {
            get { return referenceDlls; }
            set { referenceDlls = value; }
        }


        private string dynamicCsharpCode = string.Empty;

        /// <summary>
        /// 针对特殊的字段，需要用c#程序代码进行数据处理，
        /// 这个字段来保存要动态执行的代码
        /// </summary>
        public string DynamicCsharpCode
        {
            get { return dynamicCsharpCode; }
            set { dynamicCsharpCode = value; }
        }


        [Browsable(true), Description("引发外部事件")]
        public event OtherHandler OtherEvent;


        public void SaveDataFromUI(UCBasePara aa)
        {
            UC动态程序代码处理Para para = new UC动态程序代码处理Para();
            para = aa as UC动态程序代码处理Para;
            para.ReferenceDlls = ReferenceDlls;
            para.DynamicCsharpCode = DynamicCsharpCode;

        }

        public void LoadDataToUI(UCBasePara aa)
        {
            UC动态程序代码处理Para para = new UC动态程序代码处理Para();
            para = aa as UC动态程序代码处理Para;
            ReferenceDlls = para.ReferenceDlls;
            DynamicCsharpCode = para.DynamicCsharpCode;
        }



        private void btn设置动态程序代码_Click(object sender, EventArgs e)
        {
            frmDynamicCompilation dc = new frmDynamicCompilation();
            dc.ReferenceDlls = ReferenceDlls;
            dc.DynamicCsharpCode = DynamicCsharpCode;
            if (dc.ShowDialog() == DialogResult.OK)
            {
                ReferenceDlls = dc.ReferenceDlls;
                DynamicCsharpCode = dc.DynamicCsharpCode;
            }
        }
    }
}
