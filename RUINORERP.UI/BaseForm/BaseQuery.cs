using RUINORERP.Business.Processor;
using RUINORERP.Model;
using RUINORERP.UI.UControls;
using RUINORERP.UI.UserCenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.BaseForm
{
    public partial class BaseQuery : UserControl
    {
        public BaseQuery()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 当前窗体的菜单信息
        /// </summary>
        public tb_MenuInfo CurMenuInfo { get; set; } = new tb_MenuInfo();

        public NewSumDataGridView BaseMainDataGridView { get; set; }
        public NewSumDataGridView BaseSubDataGridView { get; set; }
        /// <summary>
        /// 从工作台点击过来的时候，这个保存初始化时的查询参数
        ///  这个可用可不用。
        /// </summary>
        public object QueryDtoProxy { get; set; }

        /// <summary>
        /// 传查询参数对象，对象已经给了查询参数具体值，具体在窗体那边判断
        /// </summary>
        /// <param name="QueryParameters"></param>
        internal virtual void LoadQueryParametersToUI(object loadItems)
        {
             
        }

        /// <summary>
        /// 传查询参数对象，对象已经给了查询参数具体值，具体在窗体那边判断
        /// </summary>
        /// <param name="QueryParameters"></param>
        internal virtual void LoadQueryParametersToUI(object QueryParameters, QueryParameter nodeParameter = null)
        {
            //
        }


        /// <returns></returns>
        internal virtual object LoadQueryConditionToUI(decimal QueryConditionShowColQty)
        {
            return null;
        }



        /// <summary>
        /// 设置GridView显示配置
        /// </summary>
        public virtual void SetGridViewDisplayConfig()
        {

        }

    }
}
