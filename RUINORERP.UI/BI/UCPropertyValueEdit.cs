using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.UI.UCToolBar;
using RUINORERP.Model;
using Krypton.Toolkit;
using RUINORERP.UI.BaseForm;
using RUINORERP.Business.LogicaService;
using RUINORERP.UI.Common;
using RUINORERP.Business;
using RUINORERP.AI;
using Newtonsoft.Json;

namespace RUINORERP.UI.BI
{


    [MenuAttrAssemblyInfo("属性值编辑", true, UIType.单表数据)]
    public partial class UCPropertyValueEdit : BaseEditGeneric<tb_ProdPropertyValue>
    {
        private OllamaService _ollamaService;
        private string _defaultModelName;
        
        public UCPropertyValueEdit()
        {
            InitializeComponent();
            // 获取系统全局配置
            var config = MainForm.Instance.AppContext.GetRequiredService<RUINORERP.Model.ConfigModel.SystemGlobalConfig>();
            // 使用配置中的API地址和默认模型名称
            _ollamaService = new OllamaService(config.OllamaApiAddress);
            _defaultModelName = config.OllamaDefaultModel;
            // 添加文本框编辑完成事件
            txtPropertyValueName.Leave += TxtPropertyValueName_Leave;
        }

        public override void BindData(BaseEntity entity)
        {
            DataBindingHelper.BindData4Cmb<tb_ProdProperty>(entity, k => k.Property_ID, v => v.PropertyName, cmbProperty_ID);
            DataBindingHelper.BindData4TextBox<tb_ProdPropertyValue>(entity, t => t.PropertyValueName, txtPropertyValueName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProdPropertyValue>(entity, t => t.PropertyValueCode, txtPropertyValueCode, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProdPropertyValue>(entity, t => t.PropertyValueDesc, txtPropertyValueDesc, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProdProperty>(entity, t => t.SortOrder.ToString(), txtSortOrder, BindDataType4TextBox.Text, false);
            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_ProdPropertyValueValidator> (), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }
            base.BindData(entity);
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        Business.LogicaService.UnitController mc = Startup.GetFromFac<UnitController>();

        /// <summary>
        /// 属性值名称编辑完成事件，触发AI生成编码
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private async void TxtPropertyValueName_Leave(object sender, EventArgs e)
        {
            string propertyValueName = txtPropertyValueName.Text.Trim();
            if (!string.IsNullOrEmpty(propertyValueName))
            {
                try
                {
                    // 显示加载状态
                    this.Cursor = Cursors.WaitCursor;
                    
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
输出：{{""Code"": ""R""}}

输入：Large Size
输出：{{""Code"": ""L""}}

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
                                    // 这里可以将生成的编码设置到对应的字段
                                    // 假设存在txtPropertyValueCode文本框
                                    // txtPropertyValueCode.Text = result.Code;
                                    
                                    // 由于当前界面可能没有编码字段，这里可以在控制台输出或弹出提示
                                    Console.WriteLine($"生成的编码：{result.Code}");
                                    // 或者显示提示信息
                                    // MessageBox.Show($"生成的编码：{result.Code}", "AI编码生成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"解析AI返回结果失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
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

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (base.Validator())
            {
                bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }


    }
}
