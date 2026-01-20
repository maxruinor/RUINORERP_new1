using Microsoft.Extensions.Logging;
using RUINORERP.UI.HelpSystem.Core;
using RUINORERP.UI.HelpSystem.Extensions;
using System;

namespace RUINORERP.UI.BaseForm.Helpers
{
    /// <summary>
    /// 帮助系统集成辅助类
    /// 负责处理与帮助系统相关的功能集成
    /// </summary>
    public class HelpSystemIntegration<T, C> where T : RUINORERP.Model.BaseEntity, new() where C : class, new()
    {
        private readonly BaseBillEditGeneric<T, C> _parentForm;
        private readonly ILogger _logger;

        public HelpSystemIntegration(BaseBillEditGeneric<T, C> parentForm, ILogger logger)
        {
            _parentForm = parentForm;
            _logger = logger;
        }

        /// <summary>
        /// 初始化帮助系统
        /// </summary>
        public void Initialize()
        {
            if (!_parentForm.EnableSmartHelp) return;

            try
            {
                // 启用F1帮助
                _parentForm.EnableF1Help();

                // 启用智能提示
                HelpManager.Instance.EnableSmartTooltipForAll(
                    _parentForm,
                    _parentForm.FormHelpKey,
                    typeof(T)  // 传递泛型实体类型
                );
            }
            catch (Exception ex)
            {
                _logger?.LogError($"初始化帮助系统失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 更新帮助系统配置
        /// </summary>
        public void UpdateHelpConfiguration(string formHelpKey = null)
        {
            if (formHelpKey != null)
            {
                _parentForm.FormHelpKey = formHelpKey;
            }
            // 如果 UpdateFormHelp 不存在，直接移除这行或改为其他可用方法
            // HelpManager.Instance.UpdateFormHelp(_parentForm, _parentForm.FormHelpKey);
        }
    }
}