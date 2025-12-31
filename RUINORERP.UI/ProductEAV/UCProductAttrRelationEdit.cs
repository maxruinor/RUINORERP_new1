using FastReport.Utils;
using Krypton.Toolkit;
using RUINORERP.Business;
using RUINORERP.Business.LogicaService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Common;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.UI.ProductEAV.Core;
using RUINORERP.UI.UCToolBar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.ProductEAV
{
    [MenuAttrAssemblyInfo("产品属性关联编辑", true, UIType.单表数据)]
    public partial class UCProductAttrRelationEdit : BaseEditGeneric<tb_Prod_Attr_Relation>
    {
        private ProductAttrService _attrService;

        public UCProductAttrRelationEdit()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                _attrService = new ProductAttrService();
            }
            
        }

        private tb_Prod_Attr_Relation _EditEntity;

        /// <summary>
        /// 绑定数据到控件
        /// </summary>
        /// <param name="entity">要绑定的实体</param>
        public override async void BindData(BaseEntity entity)
        {
            _EditEntity = entity as tb_Prod_Attr_Relation;
            if (_EditEntity.RAR_ID == 0)
            {
                // 新增时的初始化
                _EditEntity.isdeleted = false;
            }
            
            // 绑定基本数据
            DataBindingHelper.BindData4TextBox<tb_Prod_Attr_Relation>(entity, t => t.RAR_ID.ToString(), txtRAR_ID, BindDataType4TextBox.Text, true);

            // 判断是新增还是编辑模式
            if (entity.ActionStatus == ActionStatus.新增)
            {
                // 新增模式下加载产品下拉框
                DataBindingHelper.BindData4Cmb<tb_ProdProperty>(entity, k => k.Property_ID, v => v.PropertyName, cmbProperty);
                DataBindingHelper.BindData4Cmb<tb_ProdPropertyValue>(entity, k => k.PropertyValueID, v => v.PropertyValueName, cmbPropertyValue);

                // 异步初始化下拉框数据
                await InitComboBoxes();

                // 产品和产品详情可以编辑
                cmbProduct.Enabled = true;
                cmbProdDetail.Enabled = true;
            }
            else
            {
                // 编辑模式下只显示已关联的数据，不加载所有产品
                // 显示产品信息
                if (_EditEntity.ProdBaseID.HasValue)
                {
                    var product = _EditEntity.tb_prod;
                    if (product != null)
                    {
                        cmbProduct.Items.Add(product);
                        cmbProduct.SelectedItem = product;
                        cmbProduct.Enabled = false; // 编辑模式下产品不可编辑
                    }
                }

                // 显示产品详情信息
                if (_EditEntity.ProdDetailID.HasValue)
                {
                    var prodDetail = _EditEntity.tb_proddetail;
                    if (prodDetail != null)
                    {
                        cmbProdDetail.Items.Add(prodDetail);
                        cmbProdDetail.SelectedItem = prodDetail;
                        cmbProdDetail.Enabled = false; // 编辑模式下产品详情不可编辑
                    }
                }

                // 加载属性和属性值，允许修改
                DataBindingHelper.BindData4Cmb<tb_ProdProperty>(entity, k => k.Property_ID, v => v.PropertyName, cmbProperty);
                DataBindingHelper.BindData4Cmb<tb_ProdPropertyValue>(entity, k => k.PropertyValueID, v => v.PropertyValueName, cmbPropertyValue);

       
                // 如果已经有属性，异步加载对应的属性值
                if (_EditEntity.Property_ID.HasValue)
                {
                    // 使用异步加载属性值，避免阻塞UI线程
                    await LoadPropertyValuesAsync(_EditEntity.Property_ID.Value);
                }

                // 属性和属性值可以编辑
                cmbProperty.Enabled = true;
                cmbPropertyValue.Enabled = true;
            }

            // 初始化必填项和编辑项
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_Prod_Attr_RelationValidator>(), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }

            entity.PropertyChanged += (sender, s2) =>
            {
                //属性变化后。重新加载对应的属性值
                if (s2.PropertyName == entity.GetPropertyName<tb_Prod_Attr_Relation>(c => c.Property_ID))
                {
                    //创建表达式
                    var lambda = Expressionable.Create<tb_ProdPropertyValue>()
                                    .And(t => t.Property_ID == _EditEntity.Property_ID)
                                    .ToExpression();//注意 这一句 不能少

                    BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ProdPropertyValue).Name + "Processor");
                    QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
                    queryFilterC.FilterLimitExpressions.Add(lambda);
                    DataBindingHelper.BindData4Cmb<tb_ProdPropertyValue>(entity, k => k.PropertyValueID, v => v.PropertyValueName, cmbPropertyValue, queryFilterC.GetFilterExpression<tb_ProdPropertyValue>(), true);
                    DataBindingHelper.InitFilterForControlByExp<tb_ProdPropertyValue>(entity, cmbPropertyValue, c => c.PropertyValueName, queryFilterC);
                }
            }
                ;


            base.BindData(entity);
        }

        /// <summary>
        /// 初始化下拉框数据（仅在新增模式下调用）
        /// </summary>
        private async Task InitComboBoxes()
        {
            try
            {
                // 并行加载产品数据和属性数据
                var productsTask = _attrService.GetAllProductsAsync();
                var propertiesTask = _attrService.GetAllPropertiesAsync();

                await Task.WhenAll(productsTask, propertiesTask);

                var products = productsTask.Result;
                var properties = propertiesTask.Result;

                // 绑定产品数据
                DataBindingHelper.BindData4Cmb<tb_Prod>(products, k => k.ProdBaseID, v => v.ProductNo + " - " + v.CNName, cmbProduct);

                // 如果已经选择了产品，加载该产品的详情
                if (_EditEntity.ProdBaseID.HasValue)
                {
                    await LoadProdDetailsAsync(_EditEntity.ProdBaseID.Value);
                }

                // 绑定属性数据
                DataBindingHelper.BindData4Cmb<tb_ProdProperty>(properties, k => k.Property_ID, v => v.PropertyName, cmbProperty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化下拉框失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 根据产品加载产品详情（异步版本）
        /// </summary>
        /// <param name="prodBaseId">产品基础ID</param>
        private async Task LoadProdDetailsAsync(long prodBaseId)
        {
            if (prodBaseId <= 0)
            {
                return;
            }

            try
            {
                // 清除现有绑定，但保留控件状态
                if (cmbProdDetail.DataBindings.Count > 0)
                {
                    cmbProdDetail.DataBindings.Clear();
                }

                // 使用Service加载产品详情
                var details = await _attrService.GetProductDetailsAsync(prodBaseId);

                if (details != null && details.Count > 0)
                {
                    // 直接绑定数据源，避免使用BindingSource
                    cmbProdDetail.DataSource = details;
                    cmbProdDetail.ValueMember = "ProdDetailID";
                    cmbProdDetail.DisplayMember = "SKU";

                    // 创建数据绑定
                    var binding = new Binding("SelectedValue", _EditEntity, "ProdDetailID", true, DataSourceUpdateMode.OnValidation);

                    // 添加数据转换处理
                    binding.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
                    binding.Parse += (s, args) => args.Value = args.Value == null || (long)args.Value == -1 ? null : args.Value;

                    // 添加绑定到下拉框
                    cmbProdDetail.DataBindings.Add(binding);

                    // 启用下拉框
                    cmbProdDetail.Enabled = true;
                }
                else
                {
                    // 如果没有找到相关产品详情，清空下拉框
                    cmbProdDetail.DataSource = null;
                    cmbProdDetail.Items.Clear();
                    // 添加一个空选项，提示用户
                    cmbProdDetail.Items.Add("暂无产品详情(SKU)");
                    cmbProdDetail.SelectedIndex = 0;
                    cmbProdDetail.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载产品详情失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // 确保界面状态一致
                cmbProdDetail.DataSource = null;
                cmbProdDetail.Items.Clear();
                cmbProdDetail.Items.Add("加载失败");
                cmbProdDetail.SelectedIndex = 0;
                cmbProdDetail.Enabled = false;
            }
        }

        /// <summary>
        /// 根据属性加载属性值（异步版本）1
        /// </summary>
        /// <param name="propertyId">属性ID</param>
        private async Task LoadPropertyValuesAsync(long propertyId)
        {
            if (propertyId <= 0)
            {
                return;
            }

            try
            {
                // 清除现有绑定，但保留控件状态
                if (cmbPropertyValue.DataBindings.Count > 0)
                {
                    cmbPropertyValue.DataBindings.Clear();
                }

                // 使用Service加载属性值
                var values = await _attrService.GetPropertyValuesAsync(propertyId);

                if (values != null && values.Count > 0)
                {
                    // 直接绑定数据源，避免使用BindingSource
                    cmbPropertyValue.DataSource = values;
                    cmbPropertyValue.ValueMember = "PropertyValueID";
                    cmbPropertyValue.DisplayMember = "PropertyValueName";

                    // 创建数据绑定
                    var binding = new Binding("SelectedValue", _EditEntity, "PropertyValueID", true, DataSourceUpdateMode.OnValidation);

                    // 添加数据转换处理
                    binding.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
                    binding.Parse += (s, args) => args.Value = args.Value == null || (long)args.Value == -1 ? null : args.Value;

                    // 添加绑定到下拉框
                    cmbPropertyValue.DataBindings.Add(binding);

                    // 启用下拉框
                    cmbPropertyValue.Enabled = true;
                }
                else
                {
                    // 如果没有找到相关属性值，清空下拉框
                    cmbPropertyValue.DataSource = null;
                    cmbPropertyValue.Items.Clear();
                    // 添加一个空选项，提示用户
                    cmbPropertyValue.Items.Add("暂无属性值");
                    cmbPropertyValue.SelectedIndex = 0;
                    cmbPropertyValue.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载属性值失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // 确保界面状态一致
                cmbPropertyValue.DataSource = null;
                cmbPropertyValue.Items.Clear();
                cmbPropertyValue.Items.Add("加载失败");
                cmbPropertyValue.SelectedIndex = 0;
                cmbPropertyValue.Enabled = false;
            }
        }





        /// <summary>
        /// 清空属性和属性值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearAttrs_Click(object sender, EventArgs e)
        {
            cmbProperty.SelectedItem = null;
            cmbPropertyValue.SelectedItem = null;
            _EditEntity.Property_ID = null;
            _EditEntity.PropertyValueID = null;
        }

        /// <summary>
        /// 替换当前属性和属性值
        /// </summary>
        private void btnReplaceAttrs_Click(object sender, EventArgs e)
        {
            // 验证是否选择了新产品和详情（仅在新增模式下需要）
            if (_EditEntity.ActionStatus == ActionStatus.新增 && (!_EditEntity.ProdBaseID.HasValue || !_EditEntity.ProdDetailID.HasValue))
            {
                MessageBox.Show("请先选择产品和产品详情！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 验证是否选择了属性和属性值
            if (!_EditEntity.Property_ID.HasValue || !_EditEntity.PropertyValueID.HasValue)
            {
                MessageBox.Show("请选择要替换的属性和属性值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // 执行保存操作，实际上就是更新当前的属性关联
                bindingSourceEdit.EndEdit();

                // 显示成功消息
                MessageBox.Show("属性和属性值替换成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 如果是编辑模式，可以考虑关闭窗口
                if (_EditEntity.ActionStatus != ActionStatus.新增)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("属性和属性值替换失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 产品选择变化时加载对应的产品详情
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private async void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 避免在数据绑定过程中触发事件
            if (cmbProduct.SelectedIndex < 0 || !(cmbProduct.SelectedItem is tb_Prod selectedProduct))
            {
                return;
            }

            try
            {
                // 更新绑定实体的产品ID
                _EditEntity.ProdBaseID = selectedProduct.ProdBaseID;
                _EditEntity.ProdDetailID = null;
                _EditEntity.Property_ID = null;
                _EditEntity.PropertyValueID = null;

                // 清除产品详情下拉框的数据绑定和数据源
                if (cmbProdDetail.DataBindings.Count > 0)
                {
                    cmbProdDetail.DataBindings.Clear();
                }
                cmbProdDetail.DataSource = null;
                cmbProdDetail.Items.Clear();

                // 清除属性和属性值下拉框
                if (cmbProperty.DataBindings.Count > 0)
                {
                    cmbProperty.DataBindings.Clear();
                }
                cmbProperty.SelectedIndex = -1;

                if (cmbPropertyValue.DataBindings.Count > 0)
                {
                    cmbPropertyValue.DataBindings.Clear();
                }
                cmbPropertyValue.DataSource = null;
                cmbPropertyValue.Items.Clear();

                // 加载对应产品的详情列表
                await LoadProdDetailsAsync(selectedProduct.ProdBaseID);

                // 通知数据绑定源更新
                bindingSourceEdit.EndEdit();
                bindingSourceEdit.ResetBindings(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载产品详情失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 属性选择变化时加载对应的属性值
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private async void cmbProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 避免在数据绑定过程中触发事件
            if (cmbProperty.SelectedIndex < 0 || !(cmbProperty.SelectedItem is tb_ProdProperty selectedProperty))
            {
                return;
            }

            try
            {
                // 更新绑定实体的属性ID
                _EditEntity.Property_ID = selectedProperty.Property_ID;
                _EditEntity.PropertyValueID = null;

                // 清除属性值下拉框的数据绑定和数据源
                if (cmbPropertyValue.DataBindings.Count > 0)
                {
                    cmbPropertyValue.DataBindings.Clear();
                }
                cmbPropertyValue.DataSource = null;
                cmbPropertyValue.Items.Clear();

                // 加载对应属性的属性值列表
                await LoadPropertyValuesAsync(selectedProperty.Property_ID);

                // 通知数据绑定源更新
                bindingSourceEdit.EndEdit();
                bindingSourceEdit.ResetBindings(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载属性值失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (base.Validator(_EditEntity))
            {
                bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}