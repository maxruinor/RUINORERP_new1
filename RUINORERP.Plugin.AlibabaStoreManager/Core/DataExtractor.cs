using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.Plugin.AlibabaStoreManager.Core
{
    /// <summary>
    /// 数据提取器，负责从1688页面提取数据
    /// </summary>
    public class DataExtractor
    {
        private BrowserController browserController;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="browserController">浏览器控制器实例</param>
        public DataExtractor(BrowserController browserController)
        {
            this.browserController = browserController ?? throw new ArgumentNullException(nameof(browserController));
        }

        /// <summary>
        /// 提取订单列表数据
        /// </summary>
        /// <returns>订单数据列表</returns>
        public async Task<List<Dictionary<string, string>>> ExtractOrderListAsync()
        {
            var orders = new List<Dictionary<string, string>>();

            try
            {
                // 这里是示例代码，实际的选择器需要根据1688的具体页面结构调整
                string script = @"
                    (function() {
                        var orders = [];
                        // 查找订单表格行
                        var rows = document.querySelectorAll('.order-item'); // 示例选择器
                        for (var i = 0; i < rows.length; i++) {
                            var order = {};
                            // 提取订单号
                            var orderNumberElement = rows[i].querySelector('.order-number');
                            if (orderNumberElement) {
                                order['orderNumber'] = orderNumberElement.innerText.trim();
                            }
                            
                            // 提取订单金额
                            var amountElement = rows[i].querySelector('.order-amount');
                            if (amountElement) {
                                order['amount'] = amountElement.innerText.trim();
                            }
                            
                            // 提取下单时间
                            var timeElement = rows[i].querySelector('.order-time');
                            if (timeElement) {
                                order['orderTime'] = timeElement.innerText.trim();
                            }
                            
                            // 提取买家信息
                            var buyerElement = rows[i].querySelector('.buyer-info');
                            if (buyerElement) {
                                order['buyer'] = buyerElement.innerText.trim();
                            }
                            
                            orders.push(order);
                        }
                        return JSON.stringify(orders);
                    })();
                ";

                string result = await browserController.ExecuteScriptAsync(script);
                
                // 移除JavaScript返回的字符串引号
                if (!string.IsNullOrEmpty(result) && result.Length > 1)
                {
                    result = result.Substring(1, result.Length - 2).Replace("\\\"", "\"");
                    orders = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(result);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"提取订单数据时发生错误: {ex.Message}", ex);
            }

            return orders;
        }

        /// <summary>
        /// 提取订单详情数据
        /// </summary>
        /// <param name="orderNumber">订单号</param>
        /// <returns>订单详情数据</returns>
        public async Task<Dictionary<string, string>> ExtractOrderDetailAsync(string orderNumber)
        {
            var orderDetail = new Dictionary<string, string>();

            try
            {
                // 这里是示例代码，实际的选择器需要根据1688的具体页面结构调整
                string script = $@"
                    (function() {{
                        var detail = {{}};
                        // 查找订单详情元素
                        var detailElement = document.querySelector('.order-detail'); // 示例选择器
                        if (detailElement) {{
                            detail['orderNumber'] = '{orderNumber}';
                            detail['status'] = detailElement.querySelector('.order-status')?.innerText.trim() || '';
                            detail['receiver'] = detailElement.querySelector('.receiver-info')?.innerText.trim() || '';
                            detail['address'] = detailElement.querySelector('.delivery-address')?.innerText.trim() || '';
                            detail['phone'] = detailElement.querySelector('.contact-phone')?.innerText.trim() || '';
                        }}
                        return JSON.stringify(detail);
                    }})();
                ";

                string result = await browserController.ExecuteScriptAsync(script);

                // 移除JavaScript返回的字符串引号
                if (!string.IsNullOrEmpty(result) && result.Length > 1)
                {
                    result = result.Substring(1, result.Length - 2).Replace("\\\"", "\"");
                    orderDetail = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"提取订单详情时发生错误: {ex.Message}", ex);
            }

            return orderDetail;
        }
    }
}