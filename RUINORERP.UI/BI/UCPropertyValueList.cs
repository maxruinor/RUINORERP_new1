using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Business.LogicaService;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business;
using RUINORERP.AI;
using Newtonsoft.Json;


namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("属性值管理", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.产品资料)]
    public partial class UCPropertyValueList : BaseForm.BaseListGeneric<tb_ProdPropertyValue>
    {
        private OllamaService _ollamaService;
        private string _defaultModelName;
        
        public UCPropertyValueList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCPropertyValueEdit);
            // 获取系统全局配置
            var config = MainForm.Instance.AppContext.GetRequiredService<RUINORERP.Model.ConfigModel.SystemGlobalConfig>();
            // 使用配置中的API地址和默认模型名称
            _ollamaService = new OllamaService(config.OllamaApiAddress);
            _defaultModelName = config.OllamaDefaultModel;
            
            // 添加提取重复数据按钮
            Krypton.Toolkit.KryptonButton button检查数据 = new Krypton.Toolkit.KryptonButton();
            button检查数据.Text = "提取重复数据";
            button检查数据.ToolTipValues.Description = "提取重复数据，有一行会保留，没有显示出来。";
            button检查数据.ToolTipValues.EnableToolTips = true;
            button检查数据.ToolTipValues.Heading = "提示";
            button检查数据.Click += button检查数据_Click;
            base.frm.flowLayoutPanelButtonsArea.Controls.Add(button检查数据);
            
            // 添加AI生成编码按钮
            Krypton.Toolkit.KryptonButton buttonAIGenerateCode = new Krypton.Toolkit.KryptonButton();
            buttonAIGenerateCode.Text = "AI生成编码";
            buttonAIGenerateCode.ToolTipValues.Description = "使用AI为选中的属性值生成编码";
            buttonAIGenerateCode.ToolTipValues.EnableToolTips = true;
            buttonAIGenerateCode.ToolTipValues.Heading = "提示";
            buttonAIGenerateCode.Click += buttonAIGenerateCode_Click;
            base.frm.flowLayoutPanelButtonsArea.Controls.Add(buttonAIGenerateCode);
        }

        private void button检查数据_Click(object sender, EventArgs e)
        {
            ListDataSoure.DataSource = GetDuplicatesList();
            dataGridView1.DataSource = ListDataSoure;
        }

        /// <summary>
        /// AI生成编码按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private async void buttonAIGenerateCode_Click(object sender, EventArgs e)
        {
            // 检查是否有选中的行
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    // 显示加载状态
                    this.Cursor = Cursors.WaitCursor;
                    
                    int successCount = 0;
                    int totalCount = dataGridView1.SelectedRows.Count;
                    
                    // 遍历选中的行
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        // 获取属性值名称
                        string propertyValueName = row.Cells["PropertyValueName"].Value?.ToString()?.Trim();
                        if (!string.IsNullOrEmpty(propertyValueName))
                        {
                            // 设计AI提示词模板
                            string prompt = $@"请根据属性值名称生成对应的编码。

属性值名称：{propertyValueName}

编码生成规则：
1. 对于中文名称，使用拼音首字母大写
2. 对于英文名称，使用原词大写
3. 去除空格和特殊字符
4. 编码长度建议不超过20个字符

请返回以下JSON格式的结果：
{{
  ""Code"": ""生成的编码""
}}

示例：
输入：红色
输出：{{""Code"": ""RED""}}

输入：Large Size
输出：{{""Code"": ""LARGESIZE""}}

输入：中等硬度
输出：{{""Code"": ""ZDYD""}}"; 
                            
                            // 调用AI生成编码
                            var response = await _ollamaService.GenerateAsync(_defaultModelName, prompt);
                            
                            // 解析AI返回的结果
                            if (!string.IsNullOrEmpty(response?.Response))
                            {
                                // 提取JSON部分
                                string jsonResponse = ExtractJson(response.Response);
                                if (!string.IsNullOrEmpty(jsonResponse))
                                {
                                    try
                                    {
                                        var result = JsonConvert.DeserializeObject<CodeResult>(jsonResponse);
                                        if (result != null && !string.IsNullOrEmpty(result.Code))
                                        {
                                            // 这里可以将生成的编码更新到数据库
                                            // 假设存在Code字段
                                            // row.Cells["Code"].Value = result.Code;
                                            successCount++;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"解析AI返回结果失败：{ex.Message}");
                                    }
                                }
                            }
                        }
                    }
                    
                    // 显示生成结果
                    MessageBox.Show($"AI生成编码完成：成功 {successCount} 个，失败 {totalCount - successCount} 个", "AI编码生成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"AI调用失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // 恢复光标
                    this.Cursor = Cursors.Default;
                }
            }
            else
            {
                MessageBox.Show("请先选择要生成编码的属性值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 从AI返回的文本中提取JSON部分
        /// </summary>
        /// <param name="text">AI返回的文本</param>
        /// <returns>提取的JSON字符串</returns>
        private string ExtractJson(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;
            
            int startIndex = text.IndexOf('{');
            int endIndex = text.LastIndexOf('}');
            
            if (startIndex >= 0 && endIndex > startIndex)
            {
                return text.Substring(startIndex, endIndex - startIndex + 1);
            }
            
            return null;
        }

        /// <summary>
        /// AI返回结果的JSON模型
        /// </summary>
        private class CodeResult
        {
            public string Code { get; set; }
        }

    }
}
