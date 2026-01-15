using Microsoft.Extensions.Logging;
using RUINORERP.Business.BizMapperService;
using RUINORERP.UI.BusinessService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.UserCenter.DataParts
{
    /// <summary>
    /// 工作单元基类，所有工作单元类的统一基类
    /// </summary>
    public partial class UCBaseCell : UserControl
    {
        public UCBaseCell()
        {
            InitializeComponent();
            // 初始化防重复操作服务（延迟初始化，避免设计时错误）
            if (_guardService == null)
            {
                _guardService = Startup.GetFromFac<RepeatOperationGuardService>();
            }
        }
        /// <summary>
        /// 防重复操作服务实例
        /// </summary>
        public RepeatOperationGuardService _guardService;
        /// <summary>
        /// 数据加载方法，子类应重写此方法实现数据加载逻辑
        /// </summary>
        public virtual async Task LoadData()
        {
            // 基类默认实现为空，子类重写
            await Task.CompletedTask;
        }

        /// <summary>
        /// UI初始化方法，子类可重写此方法实现UI初始化逻辑
        /// </summary>
        protected virtual void InitializeUI()
        {
            // 基类默认实现为空，子类可重写
        }
    }
}
