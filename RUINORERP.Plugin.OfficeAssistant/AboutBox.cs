using System;
using System.Drawing;
using System.Windows.Forms;

namespace RUINORERP.Plugin.OfficeAssistant
{
    partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            this.Text = "关于 Excel 对比工具";
            this.labelProductName.Text = "Excel 对比工具";
            this.labelVersion.Text = "版本 1.0.0.0";
            this.textBoxDescription.Text = "Excel 对比工具是一个用于对比两个 Excel 文件差异的工具。\n\n" +
                                          "主要功能：\n" +
                                          "1. 支持 Excel 文件加载和预览\n" +
                                          "2. 支持工作表选择\n" +
                                          "3. 支持列映射配置\n" +
                                          "4. 支持唯一键设置\n" +
                                          "5. 支持三种对比模式：存在性对比、数据差异对比、自定义列对比\n" +
                                          "6. 差异高亮显示和分类查看\n" +
                                          "7. 结果导出功能";
        }
    }
}