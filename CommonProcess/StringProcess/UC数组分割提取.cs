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
    public partial class UC数组分割提取 : UCMyBase, IUCBase
    {
        public UC数组分割提取()
        {
            InitializeComponent();
        }


        [Browsable(true), Description("引发外部事件")]
        public event OtherHandler OtherEvent;


        public void SaveDataFromUI(UCBasePara aa)
        {
            UC数组分割提取Para para = new UC数组分割提取Para();
            para = aa as UC数组分割提取Para;
            para.delimiter = txt分割字符.Text;
            para.GetIndex = int.Parse(txtGetIndex.Text);
        }

        public void LoadDataToUI(UCBasePara aa)
        {
            UC数组分割提取Para para = new UC数组分割提取Para();
            para = aa as UC数组分割提取Para;
            txt分割字符.Text = para.Delimiter;
            txtGetIndex.Text = para.GetIndex.ToString();
        }


    }
}
