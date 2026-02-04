using Krypton.Toolkit;
using Netron.GraphLib;
using RUINORERP.Business;
using RUINORERP.Business.Processor;
using RUINORERP.Common;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.ProductEAV
{
    /// <summary>
    /// 智能包装计算工具窗体
    /// 提供双向智能计算：已知成品数量→推荐最优箱规；已知箱规→计算最大容纳数量
    /// </summary>
    [MenuAttrAssemblyInfo("智能包装计算器", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.产品资料)]
    public partial class frmSmartPackagingCalculator : UserControl
    {
        #region 私有字段

        private List<tb_CartoonBox> _availableBoxes; // 可用箱规列表
        private List<tb_CartoonBox> _availableCartonBoxes; // 外箱规格列表
        private List<PackagingSolution> _solutions; // 计算结果方案
        private BindingSource _solutionBindingSource;
        private List<ProductInfo> _productList; // 产品列表（支持混合包装）
        private BindingSource _productBindingSource;
        private List<tb_ProdDetail> _allProducts; // 所有可用产品
        private tb_BoxRules _boxRulesEntity; // 箱规实体（用于数据绑定）

        #endregion

        #region 构造函数

        public frmSmartPackagingCalculator()
        {
            InitializeComponent();
            InitializeForm();
            LoadAvailableBoxes();
        }

        #endregion

        #region 初始化方法

        private void InitializeForm()
        {
            // 设置窗体基本属性
            this.Text = "智能包装计算器";

            // 初始化数据源
            _solutions = new List<PackagingSolution>();
            _solutionBindingSource = new BindingSource();
            _solutionBindingSource.DataSource = _solutions;

            // 初始化包装实体
            _boxRulesEntity = new tb_BoxRules();
            _boxRulesEntity.ActionStatus = ActionStatus.新增;

            // 绑定结果网格
            dgvResults.AutoGenerateColumns = false;
            dgvResults.DataSource = _solutionBindingSource;

            // 设置默认值
            numGap.Value = 0.5m; // cm
            numBoxWeight.Value = 50m; // kg
            numBoxLength.Value = 50.00m; // cm
            numBoxWidth.Value = 40.00m; // cm
            numBoxHeight.Value = 30.00m; // cm

            // 初始化计算模式
            rdoQuantityToBox.Checked = true;
            chkMixedPack.Checked = false;
        }

        private async void LoadAvailableBoxes()
        {
            try
            {
                // 加载外箱数据
                await LoadCartonBoxData();

                // 加载产品数据
                await LoadProductData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载数据失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadCartonBoxData()
        {
            try
            {
                // 创建包装实体用于数据绑定
                _boxRulesEntity = new tb_BoxRules();
                _boxRulesEntity.ActionStatus = ActionStatus.新增;

                // 创建外箱过滤条件（只加载启用的外箱）
                var lambdaCartoonBox = Expressionable.Create<tb_CartoonBox>()
                   .And(t => t.Is_enabled == true)
                   .ToExpression();

                // 获取Processor和QueryFilter
                BaseProcessor baseProcessorCartoonBox = Startup.GetFromFacByName<BaseProcessor>(
                    typeof(tb_CartoonBox).Name + "Processor");
                QueryFilter queryFilterCartoonBox = baseProcessorCartoonBox.GetQueryFilter();
                queryFilterCartoonBox.FilterLimitExpressions.Add(lambdaCartoonBox);

                // 使用DataBindingHelper标准化绑定外箱数据
                DataBindingHelper.BindData4Cmb<tb_CartoonBox>(
                    _boxRulesEntity,
                    t => t.CartonID,
                    t => t.CartonName,
                    cmbBoxSelect,
                    queryFilterCartoonBox.GetFilterExpression<tb_CartoonBox>(),
                    true);

                // 初始化过滤控件（支持查询按钮功能）
                DataBindingHelper.InitFilterForControlByExp<tb_CartoonBox>(
                    _boxRulesEntity,
                    cmbBoxSelect,
                    c => c.CartonName,
                    queryFilterCartoonBox);

                // 添加选择事件处理
                cmbBoxSelect.SelectedIndexChanged += cmbBoxSelect_SelectedIndexChanged;
                numBoxLength.ValueChanged += numBoxLength_ValueChanged;
                numBoxWidth.ValueChanged += numBoxWidth_ValueChanged;
                numBoxHeight.ValueChanged += numBoxHeight_ValueChanged;

                // 默认选择第一个
                if (cmbBoxSelect.Items.Count > 0)
                {
                    cmbBoxSelect.SelectedIndex = 0;
                }

                // 从数据库加载所有启用的外箱规格到列表（用于计算）
                _availableCartonBoxes = await MainForm.Instance.AppContext.Db.Queryable<tb_CartoonBox>()
                    .Where(c => c.Is_enabled == true)
                    .OrderBy(c => c.CartonName)
                    .ToListAsync();

                //DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartment);


                //lblBoxCount.Text = $"共 {_availableCartonBoxes.Count} 种可用箱规";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载外箱数据失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadProductData()
        {
            try
            {
                // 创建包装规格的过滤条件（只加载启用且有产品详情的包装）
                var lambdaPacking = Expressionable.Create<tb_Packing>()
                   .And(t => t.Is_enabled == true)
                   .And(t => t.ProdDetailID.HasValue)
                   .ToExpression();

                // 获取Processor和QueryFilter
                BaseProcessor baseProcessorPacking = Startup.GetFromFacByName<BaseProcessor>(
                    typeof(tb_Packing).Name + "Processor");
                QueryFilter queryFilterPacking = baseProcessorPacking.GetQueryFilter();
                queryFilterPacking.FilterLimitExpressions.Add(lambdaPacking);

                // 使用DataBindingHelper标准化绑定产品数据1
                DataBindingHelper.BindData4Cmb<tb_Packing>(
                    _boxRulesEntity,
                    t => t.Pack_ID,
                    t => t.PackagingName,
                    cmbProductSelect1,
                    queryFilterPacking.GetFilterExpression<tb_Packing>(),
                    true);

                // 使用DataBindingHelper标准化绑定产品数据2
                DataBindingHelper.BindData4Cmb<tb_Packing>(
                    _boxRulesEntity,
                    t => t.Pack_ID,
                    t => t.PackagingName,
                    cmbProductSelect2,
                    queryFilterPacking.GetFilterExpression<tb_Packing>(),
                    true);

                // 初始化过滤控件（支持查询按钮功能）
                DataBindingHelper.InitFilterForControlByExp<tb_Packing>(
                    _boxRulesEntity,
                    cmbProductSelect1,
                    c => c.PackagingName,
                    queryFilterPacking);

                DataBindingHelper.InitFilterForControlByExp<tb_Packing>(
                    _boxRulesEntity,
                    cmbProductSelect2,
                    c => c.PackagingName,
                    queryFilterPacking);

                // 添加选择事件处理
                cmbProductSelect1.SelectedIndexChanged += cmbProductSelect1_SelectedIndexChanged;
                cmbProductSelect2.SelectedIndexChanged += cmbProductSelect2_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载包装数据失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 事件处理

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                btnCalculate.Enabled = false;
                btnCalculate.Text = "计算中...";
                Application.DoEvents();

                if (rdoQuantityToBox.Checked)
                {
                    CalculateBestBoxes();
                }
                else
                {
                    CalculateMaxQuantity();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"计算出错：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnCalculate.Enabled = true;
                btnCalculate.Text = "开始计算";
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ApplySelectedSolution();
        }

        private void rdoQuantityToBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateCalculationMode();
        }

        private void rdoBoxToQuantity_CheckedChanged(object sender, EventArgs e)
        {
            UpdateCalculationMode();
        }

        private void dgvResults_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvResults.SelectedRows.Count > 0)
            {
                var solution = dgvResults.SelectedRows[0].DataBoundItem as PackagingSolution;
                if (solution != null)
                {
                    DrawBoxPreview(solution);
                }
            }
        }

        private void cmbBoxSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBoxSelect.SelectedItem != null)
            {
                var selectedBox = cmbBoxSelect.SelectedItem as tb_CartoonBox;
                if (selectedBox != null)
                {
                    // 自动填充外箱尺寸信息（cm单位）
                    numBoxLength.Value = selectedBox.Length;
                    numBoxWidth.Value = selectedBox.Width;
                    numBoxHeight.Value = selectedBox.Height;
                    // 自动填充外箱重量（kg单位，MaxLoad已经是kg）
                    numBoxWeight.Value = selectedBox.MaxLoad;
                }
            }
        }

        private void numBoxLength_ValueChanged(object sender, EventArgs e)
        {
            // 用户手动修改外箱长度时的处理
            UpdateBoxVolume();
        }

        private void numBoxWidth_ValueChanged(object sender, EventArgs e)
        {
            // 用户手动修改外箱宽度时的处理
            UpdateBoxVolume();
        }

        private void numBoxHeight_ValueChanged(object sender, EventArgs e)
        {
            // 用户手动修改外箱高度时的处理
            UpdateBoxVolume();
        }

        private void UpdateBoxVolume()
        {
            // 更新外箱体积显示（如果需要的话）
            decimal volume = numBoxLength.Value * numBoxWidth.Value * numBoxHeight.Value;
            // 可以在这里更新界面上的体积显示
        }

        private void cmbProductSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 这个方法已不再使用,保留是为了兼容性
        }

        private void cmbProductSelect1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProductSelect1.SelectedItem is tb_Packing packing)
            {
                numProductLength1.Value = packing.Length;
                numProductWidth1.Value = packing.Width;
                numProductHeight1.Value = packing.Height;
                numProductWeight1.Value = packing.NetWeight > 0 ? packing.NetWeight : 5000m;
            }
        }

        private void cmbProductSelect2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProductSelect2.SelectedItem is tb_Packing packing)
            {
                numProductLength2.Value = packing.Length;
                numProductWidth2.Value = packing.Width;
                numProductHeight2.Value = packing.Height;
                numProductWeight2.Value = packing.NetWeight > 0 ? packing.NetWeight : 5000m;
            }
        }

        #endregion

        #region 核心计算逻辑

        /// <summary>
        /// 根据目标数量推荐最优箱规
        /// </summary>
        private void CalculateBestBoxes()
        {
            var config = GetPackagingConfiguration();
            if (config == null) return;

            // 计算平均产品体积
            decimal avgProductVolume = config.Products.Sum(p => p.Volume) / config.Products.Count;
            // 使用智能容差
            decimal smartGap = CalculateSmartTolerance(avgProductVolume);
            // 外箱重量单位是kg，转换为g（乘以1000）
            decimal maxWeight = numBoxWeight.Value * 1000;

            _solutions.Clear();

            // 使用用户输入的外箱尺寸创建临时箱规
            var customBox = new tb_CartoonBox
            {
                CartonName = "自定义箱规",
                Length = numBoxLength.Value,
                Width = numBoxWidth.Value,
                Height = numBoxHeight.Value,
                MaxLoad = maxWeight // kg，直接使用
            };

            // 计算单个方案
            var solution = CalculatePackagingSolution(config, customBox, smartGap, maxWeight);
            if (solution != null)
            {
                solution.UsedGap = smartGap; // 记录实际使用的容差
                _solutions.Add(solution);
            }

            // 对于单个方案,直接使用

            _solutionBindingSource.ResetBindings(false);
            lblResultCount.Text = $"找到 {_solutions.Count} 个推荐方案 (智能容差: {smartGap:F2}cm)";
        }

        /// <summary>
        /// 根据选定箱规计算最大容纳数量
        /// </summary>
        private void CalculateMaxQuantity()
        {
            var config = GetPackagingConfiguration();
            if (config == null) return;

            // 计算平均产品体积
            decimal avgProductVolume = config.Products.Sum(p => p.Volume) / config.Products.Count;
            // 使用智能容差
            decimal smartGap = CalculateSmartTolerance(avgProductVolume);
            // 外箱重量单位是kg，转换为g（乘以1000）
            decimal maxWeight = numBoxWeight.Value * 1000;

            // 使用用户输入的外箱尺寸
            var customBox = new tb_CartoonBox
            {
                CartonName = "自定义箱规",
                Length = numBoxLength.Value,
                Width = numBoxWidth.Value,
                Height = numBoxHeight.Value,
                MaxLoad = maxWeight // kg，直接使用
            };

            var solution = CalculatePackagingSolution(config, customBox, smartGap, maxWeight);

            _solutions.Clear();
            if (solution != null)
            {
                solution.UsedGap = smartGap; // 记录实际使用的容差
                _solutions.Add(solution);
            }

            _solutionBindingSource.ResetBindings(false);
            lblResultCount.Text = $"计算完成 (智能容差: {smartGap:F2}cm)";
        }

        /// <summary>
        /// 获取包装配置（支持单产品和混合包装）
        /// </summary>
        private MixedPackConfiguration GetPackagingConfiguration()
        {
            var config = new MixedPackConfiguration();

            if (chkMixedPack.Checked)
            {
                // 混合包装模式 - 使用两个成品包装信息
                var product1 = CreateProductInfo(cmbProductSelect1, numProductLength1, numProductWidth1, numProductHeight1, numProductWeight1, 100);
                var product2 = CreateProductInfo(cmbProductSelect2, numProductLength2, numProductWidth2, numProductHeight2, numProductWeight2, 100);

                if (product1 == null && product2 == null)
                {
                    MessageBox.Show("请至少选择一个成品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }

                config.Products.Clear();
                if (product1 != null) config.Products.Add(product1);
                if (product2 != null) config.Products.Add(product2);

                config.TotalWeight = config.Products.Sum(p => p.Weight * p.TargetQuantity);
                config.TotalVolume = config.Products.Sum(p => p.Volume * p.TargetQuantity);
            }
            else
            {
                // 单产品模式 - 使用成品包装信息1
                var productInfo = CreateProductInfo(cmbProductSelect1, numProductLength1, numProductWidth1, numProductHeight1, numProductWeight1, (int)numTargetQuantity.Value);

                if (productInfo == null)
                {
                    MessageBox.Show("请选择一个成品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }

                config.Products.Clear();
                config.Products.Add(productInfo);
                config.TotalWeight = productInfo.Weight * productInfo.TargetQuantity;
                config.TotalVolume = productInfo.Volume * productInfo.TargetQuantity;
            }

            return config;
        }

        /// <summary>
        /// 创建产品信息
        /// </summary>
        private ProductInfo CreateProductInfo(KryptonComboBox cmb, KryptonNumericUpDown numLength, KryptonNumericUpDown numWidth, KryptonNumericUpDown numHeight, KryptonNumericUpDown numWeight, int quantity)
        {
            if (cmb.SelectedItem == null)
            {
                // 如果未选择,使用手动输入的值
                return new ProductInfo
                {
                    ProdDetailID = 0,
                    ProductName = "自定义成品",
                    SKU = "",
                    Length = numLength.Value,
                    Width = numWidth.Value,
                    Height = numHeight.Value,
                    Weight = numWeight.Value,
                    TargetQuantity = quantity
                };
            }

            var packing = cmb.SelectedItem as tb_Packing;
            return new ProductInfo
            {
                ProdDetailID = packing.ProdDetailID ?? 0,
                ProductName = packing.tb_proddetail?.tb_prod?.CNName ?? "未知成品",
                SKU = packing.SKU,
                Length = packing.Length,
                Width = packing.Width,
                Height = packing.Height,
                Weight = packing.NetWeight > 0 ? packing.NetWeight : numWeight.Value,
                TargetQuantity = quantity
            };
        }

        /// <summary>
        /// 计算智能间隙容差(根据产品尺寸动态调整)
        /// 成品尺寸越小,容差越小
        /// 成品尺寸越大,容差越大
        /// </summary>
        private decimal CalculateSmartTolerance(decimal productVolume)
        {
            // 基础容差(cm)
            decimal baseGap = 0.5m;

            // 根据产品体积计算容差系数
            // 体积单位: cm³
            // 小尺寸产品(< 1000cm³): 容差 0.2-0.5cm
            // 中等尺寸产品(1000-5000cm³): 容差 0.5-1.0cm
            // 大尺寸产品(> 5000cm³): 容差 1.0-2.0cm

            decimal toleranceFactor;
            if (productVolume < 1000)
            {
                // 小尺寸产品
                toleranceFactor = 0.4m;
            }
            else if (productVolume < 5000)
            {
                // 中等尺寸产品
                toleranceFactor = 0.8m;
            }
            else
            {
                // 大尺寸产品
                toleranceFactor = 1.5m;
            }

            // 最终容差 = 基础容差 × 因子
            decimal smartGap = baseGap * toleranceFactor;

            // 限制容差范围 0.1cm - 3.0cm
            return Math.Max(0.1m, Math.Min(3.0m, smartGap));
        }

        /// <summary>
        /// 计算单个包装方案（支持混合包装）
        /// </summary>
        private PackagingSolution CalculatePackagingSolution(MixedPackConfiguration config, tb_CartoonBox box, decimal gap, decimal maxWeight)
        {
            try
            {
                var solution = new PackagingSolution
                {
                    BoxRule = box,
                    Configuration = config
                };

                // 考虑间隙后的有效尺寸
                decimal effectiveLength = box.Length - 2 * gap;
                decimal effectiveWidth = box.Width - 2 * gap;
                decimal effectiveHeight = box.Height - 2 * gap;

                if (effectiveLength <= 0 || effectiveWidth <= 0 || effectiveHeight <= 0)
                {
                    return null; // 间隙过大，无法放置产品
                }

                // 计算混合包装的最大容纳数量
                int maxQuantity = CalculateMixedPackQuantity(config, effectiveLength, effectiveWidth, effectiveHeight);

                if (maxQuantity == 0)
                {
                    return null; // 无法放入任何产品
                }

                solution.QuantityPerBox = maxQuantity;
                solution.BoxVolume = box.Length * box.Width * box.Height;
                solution.EffectiveVolume = effectiveLength * effectiveWidth * effectiveHeight;
                solution.OccupiedVolume = config.TotalVolume * maxQuantity / config.Products.Sum(p => p.TargetQuantity);
                solution.UtilizationRate = (decimal)(solution.OccupiedVolume / solution.EffectiveVolume * 100);

                // 计算所需箱数
                int totalTargetQuantity = config.Products.Sum(p => p.TargetQuantity);
                solution.RequiredBoxes = (int)Math.Ceiling((decimal)totalTargetQuantity / solution.QuantityPerBox);
                solution.TotalQuantity = solution.RequiredBoxes * solution.QuantityPerBox;
                solution.RemainingSpace = solution.EffectiveVolume - solution.OccupiedVolume;

                // 检查重量限制
                solution.TotalWeight = config.TotalWeight * solution.QuantityPerBox / totalTargetQuantity;
                solution.WeightExceeded = solution.TotalWeight > maxWeight;
                solution.WeightStatus = solution.WeightExceeded ?
                    $"超重({solution.TotalWeight:F0}g/{maxWeight:F0}g)" :
                    $"安全({solution.TotalWeight:F0}g/{maxWeight:F0}g)";

                return solution;
            }
            catch (Exception ex)
            {
                // 记录异常但不中断计算
                System.Diagnostics.Debug.WriteLine($"计算箱规 {box.CartonName} 时出错: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 计算混合包装的最大数量（优化算法）
        /// </summary>
        private int CalculateMixedPackQuantity(MixedPackConfiguration config, decimal effLength, decimal effWidth, decimal effHeight)
        {
            // 优化算法：分层计算，考虑产品尺寸差异
            if (config.Products.Count == 0)
                return 0;

            // 按产品尺寸分组（大、中、小）
            var sortedProducts = config.Products.OrderByDescending(p => p.Volume).ToList();

            // 优先计算最大产品的摆放方式（以最大产品为基准）
            var baseProduct = sortedProducts[0];
            var arrangements = new List<BoxArrangement>();

            // 计算所有可能的摆放方向
            arrangements.Add(CalculateArrangement(baseProduct, effLength, effWidth, effHeight, "原始方向"));
            arrangements.Add(CalculateArrangement(baseProduct, effLength, effWidth, effHeight, "长宽交换", true, false, false));
            arrangements.Add(CalculateArrangement(baseProduct, effLength, effWidth, effHeight, "长高交换", false, false, true));

            // 选择最优摆放方式（容纳数量最多）
            var bestArrangement = arrangements
                .Where(a => a.TotalFit > 0)
                .OrderByDescending(a => a.TotalFit)
                .FirstOrDefault();

            if (bestArrangement == null)
                return 0;

            // 计算每层空间利用率
            int itemsPerLayer = bestArrangement.LengthFit * bestArrangement.WidthFit;
            int totalLayers = bestArrangement.HeightFit;

            // 按体积比例分配各产品数量
            decimal totalTargetVolume = config.TotalVolume;
            int totalCapacity = itemsPerLayer * totalLayers;

            // 计算各产品实际可放置数量（考虑混合比例）
            int totalPlaced = 0;
            foreach (var product in config.Products)
            {
                // 按体积比例计算该产品应占数量
                decimal volumeRatio = product.Volume / totalTargetVolume;
                int productCapacity = (int)(totalCapacity * volumeRatio);

                // 限制不超过目标数量
                productCapacity = Math.Min(productCapacity, product.TargetQuantity);
                totalPlaced += productCapacity;
            }

            // 填充剩余空间（如果有）
            int remainingCapacity = totalCapacity - totalPlaced;
            if (remainingCapacity > 0)
            {
                // 优先填充小体积产品
                foreach (var product in sortedProducts.Reverse<ProductInfo>())
                {
                    if (totalPlaced >= totalCapacity) break;

                    int canAdd = Math.Min(remainingCapacity, product.TargetQuantity);
                    if (canAdd > 0)
                    {
                        totalPlaced += canAdd;
                        remainingCapacity -= canAdd;
                    }
                }
            }

            return Math.Max(0, Math.Min(totalPlaced, totalCapacity));
        }

        /// <summary>
        /// 计算产品在特定方向的摆放数量
        /// </summary>
        private BoxArrangement CalculateArrangement(ProductInfo product, decimal effLength, decimal effWidth, decimal effHeight,
            string orientation, bool swapLengthWidth = false, bool swapLengthHeight = false, bool swapWidthHeight = false)
        {
            decimal length = product.Length;
            decimal width = product.Width;
            decimal height = product.Height;

            // 应用交换
            if (swapLengthWidth)
            {
                decimal temp = length;
                length = width;
                width = temp;
            }
            if (swapLengthHeight)
            {
                decimal temp = length;
                length = height;
                height = temp;
            }
            if (swapWidthHeight)
            {
                decimal temp = width;
                width = height;
                height = temp;
            }

            return new BoxArrangement
            {
                Orientation = orientation,
                LengthFit = (int)(effLength / length),
                WidthFit = (int)(effWidth / width),
                HeightFit = (int)(effHeight / height),
                TotalFit = (int)(effLength / length) * (int)(effWidth / width) * (int)(effHeight / height)
            };
        }

        #endregion

        #region UI更新方法

        private void UpdateCalculationMode()
        {
            if (rdoQuantityToBox.Checked)
            {
                // 数量→箱规模式
                lblModeDescription.Text = "输入产品信息，系统将推荐最适合的箱规";
            }
            else
            {
                // 箱规→数量模式
                lblModeDescription.Text = "选择箱规，系统将计算该箱规最多能装多少产品";
            }

            UpdateMixedPackVisibility();
        }

        private void UpdateMixedPackVisibility()
        {
            bool isMixed = chkMixedPack.Checked;

            // 单产品模式控件 - 隐藏
            lblTargetQuantity.Visible = !isMixed;
            numTargetQuantity.Visible = !isMixed;

            // 混合包装模式控件 - 显示两个成品包装信息GroupBox
            grpProductInfo2.Visible = isMixed;

            if (isMixed)
            {
                lblModeDescription.Text = "输入两种成品信息，计算混合包装方案";
            }
            else
            {
                lblModeDescription.Text = "输入成品数量，推荐最优箱规";
            }
        }

        private void chkMixedPack_CheckedChanged(object sender, EventArgs e)
        {
            UpdateMixedPackVisibility();
        }

        private void ResetForm()
        {
            numGap.Value = 0.5m;
            numBoxWeight.Value = 50m; // kg，默认值
            numTargetQuantity.Value = 100;
            rdoQuantityToBox.Checked = true;
            chkMixedPack.Checked = false;

            _solutions.Clear();
            _solutionBindingSource.ResetBindings(false);

            lblResultCount.Text = "";
            lblBoxCount.Text = "";

            // 清空预览图
            if (picPreview.Image != null)
            {
                picPreview.Image.Dispose();
                picPreview.Image = null;
            }
        }

        private void ApplySelectedSolution()
        {
            if (dgvResults.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选择一个方案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var solution = dgvResults.SelectedRows[0].DataBoundItem as PackagingSolution;
            if (solution != null)
            {
                // 显示详细的包装指导对话框
                ShowPackingInstructionsDialog(solution);
            }
        }

        /// <summary>
        /// 显示详细的包装指导对话框
        /// </summary>
        private void ShowPackingInstructionsDialog(PackagingSolution solution)
        {
            var dialog = new frmPackingInstructions(solution);
            dialog.ShowDialog();
        }

        private void DrawBoxPreview(PackagingSolution solution)
        {
            try
            {
                // 释放旧的Image以避免内存泄漏
                if (picPreview.Image != null)
                {
                    picPreview.Image.Dispose();
                }

                var bitmap = new Bitmap(picPreview.Width, picPreview.Height);
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.Clear(Color.White);

                    // 获取箱子和产品的实际尺寸（cm）
                    decimal boxLength = solution.BoxRule.Length;
                    decimal boxWidth = solution.BoxRule.Width;
                    decimal boxHeight = solution.BoxRule.Height;

                    // 获取产品信息（用于计算显示尺寸）
                    ProductInfo productInfo = solution.Configuration?.Products?.FirstOrDefault();
                    decimal prodLength = productInfo?.Length ?? 10;
                    decimal prodWidth = productInfo?.Width ?? 10;
                    decimal prodHeight = productInfo?.Height ?? 10;

                    // 计算显示比例：确保箱子能完整显示在预览区域内
                    // 预留边距：左侧50，顶部50，右侧150（显示统计信息），底部50
                    int maxDisplayWidth = picPreview.Width - 50 - 150;
                    int maxDisplayHeight = picPreview.Height - 100;

                    // 选择显示方向：长度方向对应X轴，高度方向对应Y轴，宽度方向对应Z轴（深度）
                    decimal scaleX = maxDisplayWidth / boxLength;
                    decimal scaleY = maxDisplayHeight / boxHeight;
                    decimal scale = Math.Min(scaleX, scaleY); // 使用最小缩放比例，确保完整显示

                    // 计算箱子的显示尺寸
                    int displayBoxLength = (int)(boxLength * scale);
                    int displayBoxHeight = (int)(boxHeight * scale);
                    int displayBoxDepth = (int)(boxWidth * scale * 0.3m); // 深度按比例缩小30%用于透视效果

                    // 绘制3D箱子轮廓（透视效果）
                    int boxLeft = 50;
                    int boxTop = 50;

                    // 绘制箱子正面
                    g.DrawRectangle(Pens.Blue, boxLeft, boxTop, displayBoxLength, displayBoxHeight);

                    // 绘制箱子顶部（3D效果）
                    Point[] topPoints = new Point[]
                    {
                        new Point(boxLeft, boxTop),
                        new Point(boxLeft + displayBoxDepth, boxTop - displayBoxDepth),
                        new Point(boxLeft + displayBoxLength + displayBoxDepth, boxTop - displayBoxDepth),
                        new Point(boxLeft + displayBoxLength, boxTop)
                    };
                    g.DrawPolygon(Pens.Blue, topPoints);

                    // 绘制箱子右侧（3D效果）
                    Point[] rightPoints = new Point[]
                    {
                        new Point(boxLeft + displayBoxLength, boxTop),
                        new Point(boxLeft + displayBoxLength + displayBoxDepth, boxTop - displayBoxDepth),
                        new Point(boxLeft + displayBoxLength + displayBoxDepth, boxTop + displayBoxHeight - displayBoxDepth),
                        new Point(boxLeft + displayBoxLength, boxTop + displayBoxHeight)
                    };
                    g.DrawPolygon(Pens.Blue, rightPoints);

                    // 绘制箱子标签
                    g.DrawString($"📦 {solution.BoxRule.CartonName}",
                               new Font(Font.FontFamily, 10, FontStyle.Bold),
                               Brushes.Blue,
                               boxLeft,
                               boxTop - 40);

                    // 绘制尺寸标注
                    g.DrawString($"{boxLength:F1}cm",
                               new Font(Font.FontFamily, 8),
                               Brushes.Gray,
                               boxLeft + displayBoxLength / 2 - 20,
                               boxTop + displayBoxHeight + 5);
                    g.DrawString($"{boxHeight:F1}cm",
                               new Font(Font.FontFamily, 8),
                               Brushes.Gray,
                               boxLeft - 35,
                               boxTop + displayBoxHeight / 2);

                    // 如果有摆放方案，按实际尺寸比例绘制产品
                    if (solution.Arrangement != null && solution.Arrangement.Layers.Count > 0)
                    {
                        DrawLayeredProductsScaled(g, solution, boxLeft, boxTop, displayBoxLength, displayBoxHeight, displayBoxDepth, scale);
                    }
                    else
                    {
                        // 简单绘制产品示意（按比例）
                        DrawSimpleProductGridScaled(g, solution, boxLeft, boxTop, displayBoxLength, displayBoxHeight, scale, prodLength, prodWidth, prodHeight);
                    }

                    // 显示统计信息（带图标）
                    string stats = $"📊 每箱数量: {solution.QuantityPerBox}个\n" +
                                  $"📈 空间利用率: {solution.UtilizationRate:F1}%\n" +
                                  $"⚖️  重量状态: {solution.WeightStatus}\n" +
                                  $"📏 箱规: {boxLength:F1}×{boxWidth:F1}×{boxHeight:F1} cm";

                    g.DrawString(stats,
                               new Font(Font.FontFamily, 9),
                               Brushes.Black,
                               boxLeft + displayBoxLength + 30,
                               boxTop);
                }

                picPreview.Image = bitmap;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"绘制预览图出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 绘制分层产品（按实际尺寸比例）
        /// </summary>
        private void DrawLayeredProductsScaled(Graphics g, PackagingSolution solution, int boxLeft, int boxTop,
            int displayBoxLength, int displayBoxHeight, int displayBoxDepth, decimal scale)
        {
            if (solution.Arrangement.Layers.Count == 0) return;

            var arrangement = solution.Arrangement;
            var productInfo = solution.Configuration?.Products?.FirstOrDefault();
            if (productInfo == null) return;

            // 获取产品的实际尺寸（cm）
            decimal prodLength = productInfo.Length;
            decimal prodWidth = productInfo.Width;
            decimal prodHeight = productInfo.Height;

            // 确定产品摆放方向
            string orientation = arrangement.Orientation ?? "LWH"; // 默认：长=箱长，宽=箱宽，高=箱高

            // 根据摆放方向计算产品在显示区域中的尺寸
            int displayProdLength;
            int displayProdWidth;
            int displayProdHeight;

            if (orientation == "LWH") // 长=箱长，宽=箱宽，高=箱高
            {
                displayProdLength = (int)(prodLength * scale);
                displayProdWidth = (int)(prodWidth * scale * 0.3m); // 宽度对应深度方向
                displayProdHeight = (int)(prodHeight * scale);
            }
            else if (orientation == "WLH") // 宽=箱长，长=箱宽，高=箱高
            {
                displayProdLength = (int)(prodWidth * scale);
                displayProdWidth = (int)(prodLength * scale * 0.3m);
                displayProdHeight = (int)(prodHeight * scale);
            }
            else if (orientation == "LHW") // 长=箱长，高=箱宽，宽=箱高
            {
                displayProdLength = (int)(prodLength * scale);
                displayProdWidth = (int)(prodHeight * scale * 0.3m);
                displayProdHeight = (int)(prodWidth * scale);
            }
            else // 默认使用LWH
            {
                displayProdLength = (int)(prodLength * scale);
                displayProdWidth = (int)(prodWidth * scale * 0.3m);
                displayProdHeight = (int)(prodHeight * scale);
            }

            // 确保产品尺寸不会太小（至少4个像素）
            displayProdLength = Math.Max(4, displayProdLength);
            displayProdWidth = Math.Max(2, displayProdWidth);
            displayProdHeight = Math.Max(4, displayProdHeight);

            // 计算每层能放多少个产品（从Arrangement中获取）
            int itemsPerRow = Math.Max(1, arrangement.LengthFit);
            int itemsPerCol = Math.Max(1, arrangement.HeightFit);

            // 计算间距
            int gapX = (displayBoxLength - itemsPerRow * displayProdLength) / Math.Max(itemsPerRow + 1, 1);
            int gapY = (displayBoxHeight - itemsPerCol * displayProdHeight) / Math.Max(itemsPerCol + 1, 1);

            // 绘制每层产品
            int currentLayer = 0;
            foreach (var layerInfo in arrangement.Layers)
            {
                if (layerInfo.ItemsInLayer <= 0) continue;

                int layerOffsetY = currentLayer * (displayProdHeight / 3); // 层间偏移（3D效果）

                // 绘制该层产品
                for (int i = 0; i < Math.Min(layerInfo.ItemsInLayer, itemsPerRow * itemsPerCol); i++)
                {
                    int row = i / itemsPerRow;
                    int col = i % itemsPerRow;

                    // 计算产品位置（带层偏移）
                    int productX = boxLeft + gapX + col * (displayProdLength + gapX);
                    int productY = boxTop + gapY + row * (displayProdHeight + gapY) + layerOffsetY;

                    // 绘制产品正面
                    var productRect = new Rectangle(productX, productY, displayProdLength, displayProdHeight);

                    // 不同层使用不同颜色
                    Brush productBrush = GetLayerBrush(currentLayer);
                    g.FillRectangle(productBrush, productRect);
                    g.DrawRectangle(Pens.DarkGreen, productRect);

                    // 绘制产品顶部（3D效果）
                    if (displayProdWidth > 2)
                    {
                        Point[] productTop = new Point[]
                        {
                            new Point(productX, productY),
                            new Point(productX + displayProdWidth/2, productY - displayProdWidth/2),
                            new Point(productX + displayProdLength + displayProdWidth/2, productY - displayProdWidth/2),
                            new Point(productX + displayProdLength, productY)
                        };
                        g.FillPolygon(productBrush, productTop);
                        g.DrawPolygon(Pens.DarkGreen, productTop);
                    }

                    // 在产品上显示编号
                    if (displayProdLength > 15 && displayProdHeight > 15)
                    {
                        g.DrawString($"{i + 1}",
                                   new Font(Font.FontFamily, 6),
                                   Brushes.Black,
                                   productX + 2,
                                   productY + 2);
                    }
                }

                currentLayer++;

                // 限制显示的层数（避免重叠过多）
                if (currentLayer >= 3) break;
            }

            // 绘制层数指示器
            if (arrangement.Layers.Count > 1)
            {
                g.DrawString($"📚 共 {arrangement.Layers.Count} 层",
                           new Font(Font.FontFamily, 8, FontStyle.Italic),
                           Brushes.Gray,
                           boxLeft,
                           boxTop + displayBoxHeight + 10);
            }

            // 绘制摆放方向说明
            string orientationText = $"摆放: {orientation} (长×宽×高)";
            g.DrawString(orientationText,
                       new Font(Font.FontFamily, 8),
                       Brushes.DarkGray,
                       boxLeft,
                       boxTop + displayBoxHeight + 25);
        }

        /// <summary>
        /// 绘制简单产品网格（按实际尺寸比例，无分层信息时使用）
        /// </summary>
        private void DrawSimpleProductGridScaled(Graphics g, PackagingSolution solution, int boxLeft, int boxTop,
            int displayBoxLength, int displayBoxHeight, decimal scale, decimal prodLength, decimal prodWidth, decimal prodHeight)
        {
            // 计算产品在显示区域中的尺寸（使用产品长度和高度）
            int displayProdLength = (int)(prodLength * scale);
            int displayProdHeight = (int)(prodHeight * scale);
            int displayProdDepth = (int)(prodWidth * scale * 0.3m); // 深度方向对应产品宽度

            // 确保产品尺寸不会太小
            displayProdLength = Math.Max(4, displayProdLength);
            displayProdHeight = Math.Max(4, displayProdHeight);
            displayProdDepth = Math.Max(2, displayProdDepth);

            // 计算可以放多少行和列的产品
            int productsPerRow = Math.Max(1, displayBoxLength / (displayProdLength + 2));
            int productsPerCol = Math.Max(1, displayBoxHeight / (displayProdHeight + 2));

            // 计算间距
            int gapX = (displayBoxLength - productsPerRow * displayProdLength) / Math.Max(productsPerRow + 1, 1);
            int gapY = (displayBoxHeight - productsPerCol * displayProdHeight) / Math.Max(productsPerCol + 1, 1);

            // 绘制产品网格
            for (int i = 0; i < Math.Min(solution.QuantityPerBox, productsPerRow * productsPerCol); i++)
            {
                int row = i / productsPerRow;
                int col = i % productsPerRow;

                // 计算产品位置
                int productX = boxLeft + gapX + col * (displayProdLength + gapX);
                int productY = boxTop + gapY + row * (displayProdHeight + gapY);

                // 绘制产品正面
                var productRect = new Rectangle(productX, productY, displayProdLength, displayProdHeight);
                g.FillRectangle(Brushes.LightGreen, productRect);
                g.DrawRectangle(Pens.DarkGreen, productRect);

                // 绘制产品顶部（3D效果）
                if (displayProdDepth > 2)
                {
                    Point[] productTop = new Point[]
                    {
                        new Point(productX, productY),
                        new Point(productX + displayProdDepth/2, productY - displayProdDepth/2),
                        new Point(productX + displayProdLength + displayProdDepth/2, productY - displayProdDepth/2),
                        new Point(productX + displayProdLength, productY)
                    };
                    g.FillPolygon(Brushes.LightGreen, productTop);
                    g.DrawPolygon(Pens.DarkGreen, productTop);
                }

                // 在产品上显示编号
                if (displayProdLength > 15 && displayProdHeight > 15)
                {
                    g.DrawString($"{i + 1}",
                               new Font(Font.FontFamily, 6),
                               Brushes.Black,
                               productX + 2,
                               productY + 2);
                }
            }

            // 显示产品尺寸信息
            g.DrawString($"产品尺寸: {prodLength:F1}×{prodWidth:F1}×{prodHeight:F1} cm",
                       new Font(Font.FontFamily, 8),
                       Brushes.DarkGray,
                       boxLeft,
                       boxTop + displayBoxHeight + 10);
        }

        /// <summary>
        /// 根据层数获取不同的产品颜色
        /// </summary>
        private Brush GetLayerBrush(int layerIndex)
        {
            Brush[] brushes = new Brush[]
            {
                new SolidBrush(Color.FromArgb(200, 255, 200)), // 第1层：浅绿
                new SolidBrush(Color.FromArgb(200, 200, 255)), // 第2层：浅蓝
                new SolidBrush(Color.FromArgb(255, 255, 200)), // 第3层：浅黄
                new SolidBrush(Color.FromArgb(255, 200, 200)), // 第4层：浅红
                new SolidBrush(Color.FromArgb(200, 255, 255))  // 第5层：浅青
            };


            return layerIndex < brushes.Length ? brushes[layerIndex] : Brushes.LightGray;
        }


        #endregion

        #region 数据模型类

        /// <summary>
        /// 产品信息（支持混合包装）
        /// </summary>
        public class ProductInfo
        {
            public long ProdDetailID { get; set; }
            public string ProductName { get; set; }
            public string SKU { get; set; }
            public decimal Length { get; set; }  // cm
            public decimal Width { get; set; }   // cm
            public decimal Height { get; set; }  // cm
            public decimal Weight { get; set; }  // g
            public int TargetQuantity { get; set; }
            public decimal Volume => Length * Width * Height;

            public ProductInfo Clone()
            {
                return new ProductInfo
                {
                    ProdDetailID = this.ProdDetailID,
                    ProductName = this.ProductName,
                    SKU = this.SKU,
                    Length = this.Length,
                    Width = this.Width,
                    Height = this.Height,
                    Weight = this.Weight,
                    TargetQuantity = this.TargetQuantity
                };
            }
        }

        /// <summary>
        /// 箱内产品排列方案
        /// </summary>
        public class BoxArrangement
        {
            public string Orientation { get; set; }
            public int LengthFit { get; set; }
            public int WidthFit { get; set; }
            public int HeightFit { get; set; }
            private int _totalFit;
            public int TotalFit
            {
                get { return _totalFit; }
                set { _totalFit = value; }
            }
            public string DetailedInstructions { get; set; } // 详细摆放说明
            public List<LayerInfo> Layers { get; set; } = new List<LayerInfo>(); // 分层信息
        }

        /// <summary>
        /// 分层摆放信息
        /// </summary>
        public class LayerInfo
        {
            public int LayerNumber { get; set; }
            public int ItemsInLayer { get; set; }
            public string LayoutPattern { get; set; } // 如 "5×4 矩阵排列"
            public decimal LayerHeight { get; set; }
        }

        /// <summary>
        /// 混合包装配置
        /// </summary>
        public class MixedPackConfiguration
        {
            public List<ProductInfo> Products { get; set; } = new List<ProductInfo>();
            public Dictionary<long, int> ProductQuantities { get; set; } = new Dictionary<long, int>();
            public decimal TotalWeight { get; set; }
            public decimal TotalVolume { get; set; }
        }

        /// <summary>
        /// 包装方案结果
        /// </summary>
        public class PackagingSolution
        {
            public tb_CartoonBox BoxRule { get; set; }
            public MixedPackConfiguration Configuration { get; set; }
            public BoxArrangement Arrangement { get; set; }
            public int QuantityPerBox { get; set; }
            public decimal BoxVolume { get; set; }
            public decimal EffectiveVolume { get; set; }
            public decimal OccupiedVolume { get; set; }
            public decimal UtilizationRate { get; set; }
            public int RequiredBoxes { get; set; }
            public int TotalQuantity { get; set; }
            public decimal RemainingSpace { get; set; }
            public decimal TotalWeight { get; set; }
            public bool WeightExceeded { get; set; }
            public string WeightStatus { get; set; }
            public decimal UsedGap { get; set; } // 实际使用的智能容差
            public string PackingInstructions { get; set; } // 完整的包装指导
            public List<PackingStep> PackingSteps { get; set; } = new List<PackingStep>(); // 分步指导
        }

        /// <summary>
        /// 包装步骤
        /// </summary>
        public class PackingStep
        {
            public int StepNumber { get; set; }
            public string Description { get; set; }
            public string VisualHint { get; set; } // 可视化提示
        }

        #endregion

   
    }

}