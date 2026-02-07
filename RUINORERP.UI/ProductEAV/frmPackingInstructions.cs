using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        #region 摆放指导生成方法

        /// <summary>
        /// 生成详细的包装摆放指导
        /// </summary>
        private void GeneratePackingInstructions(PackagingSolution solution, MixedPackConfiguration config, tb_CartoonBox box,
            decimal effLength, decimal effWidth, decimal effHeight)
        {
            var steps = new List<PackingStep>();
            var sb = new StringBuilder();

            sb.AppendLine($"📦 包装方案指导 - {box.CartonName}");
            sb.AppendLine(new string('=', 50));

            // 基本信息
            sb.AppendLine($"📦 箱规尺寸: {box.Length:F2}×{box.Width:F2}×{box.Height:F2} cm");
            sb.AppendLine($"📏 有效空间: {effLength:F2}×{effWidth:F2}×{effHeight:F2} cm (扣除间隙)");
            sb.AppendLine($"📊 每箱容量: {solution.QuantityPerBox} 件");
            sb.AppendLine($"⚖️  总重量: {solution.TotalWeight:F0}g ({solution.WeightStatus})");
            sb.AppendLine($"📈 空间利用率: {solution.UtilizationRate:F2}%");
            sb.AppendLine();

            if (config.Products.Count == 1)
            {
                // 单产品模式
                var product = config.Products[0];
                var arrangement = CalculateOptimalArrangement(product, effLength, effWidth, effHeight);
                solution.Arrangement = arrangement;

                sb.AppendLine("🔢 摆放方案:");
                sb.AppendLine($"   方向: {arrangement.Orientation}");
                sb.AppendLine($"   排列: {arrangement.LengthFit}×{arrangement.WidthFit}×{arrangement.HeightFit}");
                sb.AppendLine($"   总数: {arrangement.TotalFit} 件");
                sb.AppendLine($"   每层: {arrangement.LengthFit}×{arrangement.WidthFit} = {arrangement.LengthFit * arrangement.WidthFit} 件");
                sb.AppendLine($"   层数: {arrangement.HeightFit} 层");
                sb.AppendLine();

                // 生成分层信息
                GenerateLayerInfo(arrangement, product, steps);

                // 方向示意图
                sb.AppendLine("📐 摆放方向示意图:");
                sb.AppendLine($"   {GetOrientationDiagram(arrangement.Orientation)}");
                sb.AppendLine();

                sb.AppendLine("📋 分步摆放指导:");
                for (int i = 0; i < steps.Count; i++)
                {
                    sb.AppendLine($"   {steps[i].StepNumber}. {steps[i].Description}");
                    if (!string.IsNullOrEmpty(steps[i].VisualHint))
                    {
                        sb.AppendLine($"      💡 {steps[i].VisualHint}");
                    }
                }
            }
            else
            {
                // 混合包装模式
                sb.AppendLine("🔢 混合包装分布:");
                foreach (var product in config.Products)
                {
                    int productQty = (int)(solution.QuantityPerBox * (decimal)product.TargetQuantity /
                                         config.Products.Sum(p => p.TargetQuantity));
                    sb.AppendLine($"   • {product.ProductName} ({product.SKU}): {productQty} 件");
                    sb.AppendLine($"     尺寸: {product.Length}×{product.Width}×{product.Height} cm");
                }
                sb.AppendLine();

                // 混合包装摆放建议
                sb.AppendLine("📋 混合包装建议:");
                sb.AppendLine("   1. 先放置较重或较大的产品作为底层");
                sb.AppendLine("   2. 按产品类别分区域摆放，相同产品集中放置");
                sb.AppendLine("   3. 易碎品放在中间位置，周围用缓冲材料");
                sb.AppendLine("   4. 确保重心居中，避免运输时倾斜");
                sb.AppendLine("   5. 顶层放置轻质产品，填充空隙");
                sb.AppendLine();

                // 添加通用步骤
                steps.Add(new PackingStep
                {
                    StepNumber = 1,
                    Description = "清理箱内杂物，确保底部平整",
                    VisualHint = "检查箱底无尖锐物品"
                });
                steps.Add(new PackingStep
                {
                    StepNumber = 2,
                    Description = "按重量和尺寸分类产品",
                    VisualHint = "重物在下，轻物在上"
                });
                steps.Add(new PackingStep
                {
                    StepNumber = 3,
                    Description = "逐层摆放，保持重心稳定",
                    VisualHint = "每层产品尽量紧密排列"
                });
                steps.Add(new PackingStep
                {
                    StepNumber = 4,
                    Description = "填充空隙，防止运输中移动",
                    VisualHint = "使用填充物或气泡膜"
                });
                steps.Add(new PackingStep
                {
                    StepNumber = 5,
                    Description = "封箱并贴标签",
                    VisualHint = "标明重量、易碎标识和产品种类"
                });
            }

            sb.AppendLine();
            sb.AppendLine("⚠️  注意事项:");
            sb.AppendLine("   • 确保包装间隙均匀分布，避免产品晃动");
            sb.AppendLine("   • 重物靠近箱底中心位置，保持重心稳定");
            sb.AppendLine("   • 易碎品需额外缓冲保护，避免碰撞损伤");
            sb.AppendLine("   • 封箱前检查重心是否平衡，必要时调整摆放");
            sb.AppendLine("   • 确保箱盖能正常关闭，避免过度填充");

            solution.PackingInstructions = sb.ToString();
            solution.PackingSteps = steps;
        }

        /// <summary>
        /// 获取摆放方向示意图
        /// </summary>
        /// <param name="orientation">摆放方向</param>
        /// <returns>方向示意图字符串</returns>
        private string GetOrientationDiagram(string orientation)
        {
            switch (orientation)
            {
                case "原始方向(长×宽×高)":
                    return "   [长] →→→\n   [宽] ↓↓↓\n   [高] ⊥⊥⊥";
                case "长宽交换(宽×长×高)":
                    return "   [宽] →→→\n   [长] ↓↓↓\n   [高] ⊥⊥⊥";
                case "竖直摆放(高×宽×长)":
                    return "   [高] →→→\n   [宽] ↓↓↓\n   [长] ⊥⊥⊥";
                case "侧放(长×高×宽)":
                    return "   [长] →→→\n   [高] ↓↓↓\n   [宽] ⊥⊥⊥";
                default:
                    return "   [长] →→→\n   [宽] ↓↓↓\n   [高] ⊥⊥⊥";
            }
        }

        /// <summary>
        /// 计算最优的产品摆放方向
        /// </summary>
        private BoxArrangement CalculateOptimalArrangement(ProductInfo product, decimal effLength, decimal effWidth, decimal effHeight)
        {
            var arrangements = new List<BoxArrangement>();

            // 原始方向: 长×宽×高
            var arr1 = new BoxArrangement
            {
                Orientation = "原始方向(长×宽×高)",
                LengthFit = (int)(effLength / product.Length),
                WidthFit = (int)(effWidth / product.Width),
                HeightFit = (int)(effHeight / product.Height),
                DetailedInstructions = $"产品按原始方向摆放，长边沿箱长方向"
            };
            arr1.TotalFit = arr1.LengthFit * arr1.WidthFit * arr1.HeightFit;
            arrangements.Add(arr1);

            // 长宽交换: 宽×长×高
            var arr2 = new BoxArrangement
            {
                Orientation = "长宽交换(宽×长×高)",
                LengthFit = (int)(effLength / product.Width),
                WidthFit = (int)(effWidth / product.Length),
                HeightFit = (int)(effHeight / product.Height),
                DetailedInstructions = $"产品旋转90度，宽边沿箱长方向"
            };
            arr2.TotalFit = arr2.LengthFit * arr2.WidthFit * arr2.HeightFit;
            arrangements.Add(arr2);

            // 长高交换: 高×宽×长
            var arr3 = new BoxArrangement
            {
                Orientation = "竖直摆放(高×宽×长)",
                LengthFit = (int)(effLength / product.Height),
                WidthFit = (int)(effWidth / product.Width),
                HeightFit = (int)(effHeight / product.Length),
                DetailedInstructions = $"产品竖直摆放，高边沿箱长方向"
            };
            arr3.TotalFit = arr3.LengthFit * arr3.WidthFit * arr3.HeightFit;
            arrangements.Add(arr3);

            // 宽高交换: 长×高×宽
            var arr4 = new BoxArrangement
            {
                Orientation = "侧放(长×高×宽)",
                LengthFit = (int)(effLength / product.Length),
                WidthFit = (int)(effWidth / product.Height),
                HeightFit = (int)(effHeight / product.Width),
                DetailedInstructions = $"产品侧放，高边沿箱宽方向"
            };
            arr4.TotalFit = arr4.LengthFit * arr4.WidthFit * arr4.HeightFit;
            arrangements.Add(arr4);

            // 找到最优排列方式
            var bestArrangement = arrangements
                .Where(a => a.LengthFit > 0 && a.WidthFit > 0 && a.HeightFit > 0)
                .OrderByDescending(a => a.TotalFit)
                .FirstOrDefault();

            if (bestArrangement != null)
            {
                // 生成详细的分层信息
                GenerateDetailedLayers(bestArrangement, product);
            }

            return bestArrangement ?? new BoxArrangement { TotalFit = 0 };
        }

        /// <summary>
        /// 生成详细的分层摆放信息
        /// </summary>
        private void GenerateDetailedLayers(BoxArrangement arrangement, ProductInfo product)
        {
            arrangement.Layers.Clear();

            for (int layer = 1; layer <= arrangement.HeightFit; layer++)
            {
                int itemsInThisLayer = arrangement.LengthFit * arrangement.WidthFit;
                string layout = $"{arrangement.LengthFit}×{arrangement.WidthFit} 矩阵排列";
                decimal layerHeight = layer * product.Height; // 基于摆放方向的实际高度

                arrangement.Layers.Add(new LayerInfo
                {
                    LayerNumber = layer,
                    ItemsInLayer = itemsInThisLayer,
                    LayoutPattern = layout,
                    LayerHeight = layerHeight
                });
            }
        }

        /// <summary>
        /// 生成分层摆放步骤
        /// </summary>
        private void GenerateLayerInfo(BoxArrangement arrangement, ProductInfo product, List<PackingStep> steps)
        {
            steps.Add(new PackingStep
            {
                StepNumber = 1,
                Description = "清理箱内杂物，确保底部平整",
                VisualHint = "检查箱底无尖锐物品"
            });

            steps.Add(new PackingStep
            {
                StepNumber = 2,
                Description = $"准备按{arrangement.Orientation}摆放产品",
                VisualHint = $"确认产品方向与摆放示意图一致"
            });

            // 逐层添加摆放步骤
            for (int i = 0; i < arrangement.Layers.Count; i++)
            {
                var layer = arrangement.Layers[i];
                steps.Add(new PackingStep
                {
                    StepNumber = i + 3,
                    Description = $"第{layer.LayerNumber}层: 按{layer.LayoutPattern}摆放{layer.ItemsInLayer}件产品",
                    VisualHint = $"横向{arrangement.LengthFit}件，纵向{arrangement.WidthFit}件，排列紧密，间隙≤{product.Length * 0.1m:F1}cm"
                });
            }

            steps.Add(new PackingStep
            {
                StepNumber = steps.Count + 1,
                Description = "检查整体稳定性，必要时添加填充物",
                VisualHint = "摇晃测试，确保无松动"
            });

            steps.Add(new PackingStep
            {
                StepNumber = steps.Count + 1,
                Description = "封箱并标注重量和重心位置",
                VisualHint = "标明'此面朝上'、重量信息和摆放方向"
            });

            steps.Add(new PackingStep
            {
                StepNumber = steps.Count + 1,
                Description = "最终检查包装完整性",
                VisualHint = "确认箱盖能正常关闭，无突出物品"
            });
        }

        #endregion


    }
}