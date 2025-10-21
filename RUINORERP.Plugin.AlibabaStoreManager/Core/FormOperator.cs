using System;
using System.Threading.Tasks;

namespace RUINORERP.Plugin.AlibabaStoreManager.Core
{
    /// <summary>
    /// 表单操作器，负责页面表单操作
    /// </summary>
    public class FormOperator
    {
        private BrowserController browserController;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="browserController">浏览器控制器实例</param>
        public FormOperator(BrowserController browserController)
        {
            this.browserController = browserController ?? throw new ArgumentNullException(nameof(browserController));
        }

        /// <summary>
        /// 填写登录表单
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public async Task FillLoginFormAsync(string username, string password)
        {
            try
            {
                // 填写用户名
                string usernameScript = $@"
                    (function() {{
                        var usernameInput = document.querySelector('#username'); // 示例选择器
                        if (usernameInput) {{
                            usernameInput.value = '{username}';
                        }}
                    }})();
                ";
                await browserController.ExecuteScriptAsync(usernameScript);

                // 填写密码
                string passwordScript = $@"
                    (function() {{
                        var passwordInput = document.querySelector('#password'); // 示例选择器
                        if (passwordInput) {{
                            passwordInput.value = '{password}';
                        }}
                    }})();
                ";
                await browserController.ExecuteScriptAsync(passwordScript);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"填写登录表单时发生错误: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 提交登录表单
        /// </summary>
        public async Task SubmitLoginFormAsync()
        {
            try
            {
                string script = @"
                    (function() {
                        var loginButton = document.querySelector('#login-button'); // 示例选择器
                        if (loginButton) {
                            loginButton.click();
                        }
                    })();
                ";
                await browserController.ExecuteScriptAsync(script);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"提交登录表单时发生错误: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 填写订单发货表单
        /// </summary>
        /// <param name="orderNumber">订单号</param>
        /// <param name="logisticsCompany">物流公司</param>
        /// <param name="trackingNumber">物流单号</param>
        public async Task FillShippingFormAsync(string orderNumber, string logisticsCompany, string trackingNumber)
        {
            try
            {
                // 填写物流公司
                string companyScript = $@"
                    (function() {{
                        var companySelect = document.querySelector('#logistics-company'); // 示例选择器
                        if (companySelect) {{
                            companySelect.value = '{logisticsCompany}';
                        }}
                    }})();
                ";
                await browserController.ExecuteScriptAsync(companyScript);

                // 填写物流单号
                string trackingScript = $@"
                    (function() {{
                        var trackingInput = document.querySelector('#tracking-number'); // 示例选择器
                        if (trackingInput) {{
                            trackingInput.value = '{trackingNumber}';
                        }}
                    }})();
                ";
                await browserController.ExecuteScriptAsync(trackingScript);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"填写发货表单时发生错误: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 提交订单发货表单
        /// </summary>
        public async Task SubmitShippingFormAsync()
        {
            try
            {
                string script = @"
                    (function() {
                        var submitButton = document.querySelector('#submit-shipping'); // 示例选择器
                        if (submitButton) {
                            submitButton.click();
                        }
                    })();
                ";
                await browserController.ExecuteScriptAsync(script);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"提交发货表单时发生错误: {ex.Message}", ex);
            }
        }
    }
}