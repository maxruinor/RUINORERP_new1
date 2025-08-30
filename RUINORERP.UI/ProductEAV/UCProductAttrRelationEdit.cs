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
using RUINORERP.Business;
using RUINORERP.Common.Helper;
using RUINORERP.UI.Common;
using RUINORERP.Global;
using FastReport.Utils;
using RUINORERP.Common.Extensions;

namespace RUINORERP.UI.ProductEAV
{
    [MenuAttrAssemblyInfo("产品属性关联编辑", true, UIType.单表数据)]
    public partial class UCProductAttrRelationEdit : BaseEditGeneric<tb_Prod_Attr_Relation>
    {
        public UCProductAttrRelationEdit()
        {
            InitializeComponent();
        }

        private tb_Prod_Attr_Relation _EditEntity;

        public override void BindData(BaseEntity entity)
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
                DataBindingHelper.BindData4Cmb<tb_Prod>(entity, k => k.ProdBaseID, v => v.ProductNo + " - " + v.CNName, cmbProduct);
                DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v => v.SKU, cmbProdDetail);
                DataBindingHelper.BindData4Cmb<tb_ProdProperty>(entity, k => k.Property_ID, v => v.PropertyName, cmbProperty);
                DataBindingHelper.BindData4Cmb<tb_ProdPropertyValue>(entity, k => k.PropertyValueID, v => v.PropertyValueName, cmbPropertyValue);
                
                // 初始化下拉框数据
                InitComboBoxes();
                
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
                
                // 加载属性列表
                var properties = MainForm.Instance.AppContext.Db.Queryable<tb_ProdProperty>().ToList();
                DataBindingHelper.BindData4Cmb<tb_ProdProperty>(properties, k => k.Property_ID, v => v.PropertyName, cmbProperty);
                
                // 如果已经有属性，加载对应的属性值
                if (_EditEntity.Property_ID.HasValue)
                {
                    LoadPropertyValues(_EditEntity.Property_ID.Value);
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

            base.BindData(entity);
        }

        /// <summary>
        /// 初始化下拉框数据（仅在新增模式下调用）
        /// </summary>
        private void InitComboBoxes()
        {
            // 加载产品数据
            var products = MainForm.Instance.AppContext.Db.Queryable<tb_Prod>().ToList();
            DataBindingHelper.BindData4Cmb<tb_Prod>(products, k => k.ProdBaseID, v => v.ProductNo + " - " + v.CNName, cmbProduct);

            // 如果已经选择了产品，加载该产品的详情
            if (_EditEntity.ProdBaseID.HasValue)
            {
                LoadProdDetails(_EditEntity.ProdBaseID.Value);
            }

            // 加载所有属性
            var properties = MainForm.Instance.AppContext.Db.Queryable<tb_ProdProperty>().ToList();
            DataBindingHelper.BindData4Cmb<tb_ProdProperty>(properties, k => k.Property_ID, v => v.PropertyName, cmbProperty);
        }

        /// <summary>
        /// 根据产品加载产品详情
        /// </summary>
        /// <param name="prodBaseId"></param>
        private void LoadProdDetails(long prodBaseId)
        {
            if (prodBaseId <= 0)
            {
                return;
            }
            
            try
            {
                // 查询与指定产品相关的所有详情
                var details = MainForm.Instance.AppContext.Db.Queryable<tb_ProdDetail>()
                    .Where(pd => pd.ProdBaseID == prodBaseId)
                    .OrderBy(pd => pd.SKU) // 按SKU排序
                    .ToList();
                
                if (details != null && details.Count > 0)
                {
                    // 创建一个BindingSource来管理数据绑定
                    BindingSource bs = new BindingSource();
                    bs.DataSource = details;
                    
                    // 使用ComboBoxHelper初始化下拉列表，确保正确的数据绑定
                    ComboBoxHelper.InitDropList(bs, cmbProdDetail, "ProdDetailID", "SKU", ComboBoxStyle.DropDownList, false);
                    
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
        /// 根据属性加载属性值
        /// </summary>
        /// <param name="propertyId"></param>
        private void LoadPropertyValues(long propertyId)
        {
            if (propertyId <= 0)
            {
                return;
            }
            
            try
            {
                // 查询与指定属性相关的所有属性值
                var values = MainForm.Instance.AppContext.Db.Queryable<tb_ProdPropertyValue>()
                    .Where(pv => pv.Property_ID == propertyId)
                    .OrderBy(pv => pv.SortOrder) // 按排序顺序加载
                    .ToList();
                
                if (values != null && values.Count > 0)
                {
                    // 创建一个BindingSource来管理数据绑定
                    BindingSource bs = new BindingSource();
                    bs.DataSource = values;
                    
                    // 使用ComboBoxHelper初始化下拉列表，确保正确的数据绑定
                    ComboBoxHelper.InitDropList(bs, cmbPropertyValue, "PropertyValueID", "PropertyValueName", ComboBoxStyle.DropDownList, false);
                    
                    // 创建数据绑定
                    var binding = new Binding("SelectedValue", _EditEntity, "PropertyValueID", true, DataSourceUpdateMode.OnValidation);
                    
                    // 添加数据转换处理
                    binding.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
                    binding.Parse += (s, args) => args.Value = args.Value == null || (long)args.Value == -1 ? null : args.Value;
                    
                    // 添加绑定到下拉框
                    cmbPropertyValue.DataBindings.Add(binding);
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedIndex >= 0 && cmbProduct.SelectedItem is tb_Prod selectedProduct)
            {
                try
                {
                    // 清除之前的数据绑定和数据源，避免数据混乱
                    cmbProdDetail.DataSource = null;
                    cmbProdDetail.DataBindings.Clear();
                    
                    // 加载对应产品的详情列表
                    LoadProdDetails(selectedProduct.ProdBaseID);
                    
                    // 清空之前选择的产品详情和相关属性
                    cmbProdDetail.SelectedIndex = -1;
                    cmbProperty.SelectedIndex = -1;
                    cmbPropertyValue.SelectedIndex = -1;
                    
                    // 更新绑定实体的产品ID
                    _EditEntity.ProdBaseID = selectedProduct.ProdBaseID;
                    _EditEntity.ProdDetailID = null;
                    _EditEntity.Property_ID = null;
                    _EditEntity.PropertyValueID = null;
                    
                    // 通知数据绑定源更新
                    bindingSourceEdit.EndEdit();
                    bindingSourceEdit.ResetBindings(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("加载产品详情失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 属性选择变化时加载对应的属性值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProperty.SelectedIndex >= 0 && cmbProperty.SelectedItem is tb_ProdProperty selectedProperty)
            {
                try
                {
                    // 清除之前的数据绑定和数据源，避免数据混乱
                    cmbPropertyValue.DataSource = null;
                    cmbPropertyValue.DataBindings.Clear();
                    
                    // 加载对应属性的属性值列表
                    LoadPropertyValues(selectedProperty.Property_ID);
                    
                    // 清空之前选择的属性值
                    cmbPropertyValue.SelectedIndex = -1;
                    
                    // 更新绑定实体的属性ID
                    _EditEntity.Property_ID = selectedProperty.Property_ID;
                    _EditEntity.PropertyValueID = null;
                    
                    // 通知数据绑定源更新
                    bindingSourceEdit.EndEdit();
                    bindingSourceEdit.ResetBindings(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("加载属性值失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
            if (base.Validator())
            {
                bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}