using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Krypton.Toolkit;
using RUINORERP.Model;
using static RUINORERP.UI.ProductEAV.frmSmartPackagingCalculator;

namespace RUINORERP.UI.ProductEAV
{
    /// <summary>
    /// 包装指导详细信息对话框
    /// </summary>
    public partial class frmPackingInstructions : KryptonForm
    {
        private PackagingSolution _solution;
        
        public frmPackingInstructions(PackagingSolution solution)
        {
            InitializeComponent();
            _solution = solution;
            InitializeForm();
        }
        
        private void InitializeForm()
        {
            this.Text = $"包装指导 - {_solution.BoxRule.CartonName}";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(800, 600);
            this.MinimumSize = new Size(600, 400);
            
            // 显示包装指导信息
            txtInstructions.Text = _solution.PackingInstructions;
            txtInstructions.ReadOnly = true;
            txtInstructions.ScrollBars = ScrollBars.Vertical;
            
            // 如果有分步指导，也显示出来
            if (_solution.PackingSteps != null && _solution.PackingSteps.Count > 0)
            {
                var stepsSb = new StringBuilder();
                stepsSb.AppendLine("🔧 详细操作步骤:");
                stepsSb.AppendLine(new string('-', 30));
                
                foreach (var step in _solution.PackingSteps)
                {
                    stepsSb.AppendLine($"{step.StepNumber}. {step.Description}");
                    if (!string.IsNullOrEmpty(step.VisualHint))
                    {
                        stepsSb.AppendLine($"   💡 提示: {step.VisualHint}");
                    }
                    stepsSb.AppendLine();
                }
                
                txtSteps.Text = stepsSb.ToString();
                txtSteps.ReadOnly = true;
                txtSteps.ScrollBars = ScrollBars.Vertical;
            }
            
            // 显示产品信息
            if (_solution.Configuration.Products.Count == 1)
            {
                var product = _solution.Configuration.Products[0];
                lblProductInfo.Text = $"产品: {product.ProductName} ({product.SKU})\n" +
                                    $"尺寸: {product.Length}×{product.Width}×{product.Height} cm\n" +
                                    $"重量: {product.Weight} g/件";
            }
            else
            {
                lblProductInfo.Text = $"混合包装: {_solution.Configuration.Products.Count} 种产品\n" +
                                    $"总重量: {_solution.TotalWeight:F0} g";
            }
            
            // 显示箱规信息
            lblBoxInfo.Text = $"箱规: {_solution.BoxRule.CartonName}\n" +
                             $"尺寸: {_solution.BoxRule.Length}×{_solution.BoxRule.Width}×{_solution.BoxRule.Height} cm\n" +
                             $"承重: {_solution.BoxRule.MaxLoad} kg";
        }
        
        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(_solution.PackingInstructions);
                MessageBox.Show("包装指导已复制到剪贴板", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"复制失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnPrint_Click(object sender, EventArgs e)
        {
            // 这里可以实现打印功能
            MessageBox.Show("打印功能待实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "文本文件|*.txt|所有文件|*.*";
                    saveDialog.FileName = $"包装指导_{_solution.BoxRule.CartonName}_{DateTime.Now:yyyyMMddHHmmss}.txt";
                    
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        System.IO.File.WriteAllText(saveDialog.FileName, _solution.PackingInstructions, Encoding.UTF8);
                        MessageBox.Show("包装指导已保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}