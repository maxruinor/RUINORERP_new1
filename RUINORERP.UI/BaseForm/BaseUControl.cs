using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Model;

namespace RUINORERP.UI.BaseForm
{
    public partial class BaseUControl : UserControl
    {

        public BaseUControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 关联的菜单信息 实际是可以从点击时传入
        /// </summary>
        public tb_MenuInfo CurMenuInfo { get; set; }


        /// <summary>
        /// 子类T的类型
        /// </summary>
        public Type ClassGenericType { get; set; }

        private BaseListRunWay _runway;
        /// <summary>
        /// 窗体运行方式  在关联编辑功能时 这个好像没有起到作用。实际是在frmBaseEditList 这个中实现显示与隐藏。
        /// </summary>
        public BaseListRunWay Runway { get => _runway; set => _runway = value; }


        public virtual void SetSelect()
        {
            Runway = BaseListRunWay.选中模式;
        }


    }
}
