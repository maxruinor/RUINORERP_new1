using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.WF.UI
{
    /// <summary>
    ///  基类
    /// </summary>
    public partial class UCNodePropEditBase : UserControl
    {

        /// <summary>
        /// 节点属性名称，显示于编辑时的上面
        /// </summary>
        public string NodePropName { get; set; } = string.Empty;


        /// <summary>
        /// 要设置的对象值
        /// </summary>
        public object SetNodeValue { get; set; }

        public UCNodePropEditBase()
        {
            InitializeComponent();
        }
    }
}
