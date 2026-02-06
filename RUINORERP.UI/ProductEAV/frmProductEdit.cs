using AutoMapper;
using FastReport.DevComponents.DotNetBar.Controls;
using FluentValidation;
using Krypton.Navigator;
using Krypton.Toolkit;
using Microsoft.International.Converters.PinYinConverter;
using Netron.GraphLib;
using NPOI.SS.Formula.Functions;
using ObjectsComparer;
using RUINOR.WinFormsUI.CustomPictureBox;
using RUINOR.WinFormsUI.TileListView;
using RUINORERP.Business;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Processor;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model;
using RUINORERP.Model.ProductAttribute;
using RUINORERP.Model.ReminderModel.ReminderRules;
using RUINORERP.PacketSpec.Models.FileManagement;
using RUINORERP.SecurityTool;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.UI.Network.Services;
using RUINORERP.UI.UControls;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RUINORERP.UI.Common.DataBindingHelper;

namespace RUINORERP.UI.ProductEAV
{

    /// <summary>
    /// 产品编辑特别，修改要保存后进行。不可以新建后就修改
    /// </summary>
    public partial class frmProductEdit : BaseEditGeneric<tb_Prod>
    {
        List<tb_ProdProperty> prodpropList = new List<tb_ProdProperty>();
        List<tb_ProdPropertyValue> prodpropValueList = new List<tb_ProdPropertyValue>();
        tb_ProdController<tb_Prod> mcProdBase = Startup.GetFromFac<tb_ProdController<tb_Prod>>();
        tb_ProdDetailController<tb_ProdDetail> mcDetail = Startup.GetFromFac<tb_ProdDetailController<tb_ProdDetail>>();
        tb_ProdPropertyValueController<tb_ProdPropertyValue> mcPropertyValue = Startup.GetFromFac<tb_ProdPropertyValueController<tb_ProdPropertyValue>>();

        tb_BoxRulesController<tb_BoxRules> ctrBoxRules = Startup.GetFromFac<tb_BoxRulesController<tb_BoxRules>>();

        /// <summary>
        /// 网格显示文本解析器，用于设置特殊的映射关系
        /// </summary>
        public GridViewDisplayTextResolverGeneric<tb_ProdDetail> DisplayTextResolver { get; set; }
        private ClientBizCodeService clientBizCodeService;

        /// <summary>
        /// 临时存储SKU图片数据（key: tb_ProdDetail对象引用, value: 图片数据列表）
        /// 使用对象引用作为key，支持未保存的SKU（ProdDetailID为0的情况）
        /// </summary>
        private Dictionary<tb_ProdDetail, List<Tuple<byte[], ImageInfo>>> skuImageDataCache = new Dictionary<tb_ProdDetail, List<Tuple<byte[], ImageInfo>>>();

        /// <summary>
        /// 按ProdDetailID存储图片数据的缓存（key: ProdDetailID, value: 图片数据列表）
        /// 用于在对象引用不一致时通过ID查找图片数据
        /// </summary>
        private Dictionary<long, List<Tuple<byte[], ImageInfo>>> skuImageDataCacheById = new Dictionary<long, List<Tuple<byte[], ImageInfo>>>();

        /// <summary>
        /// 临时存储SKU需要删除的图片信息（key: tb_ProdDetail对象引用, value: 需要删除的图片信息列表）
        /// </summary>
        private Dictionary<tb_ProdDetail, List<ImageInfo>> skuImageDeletedCache = new Dictionary<tb_ProdDetail, List<ImageInfo>>();

        //定义两个值，为了计算listview的高宽，高是属性的倍数 假设一个属性一行 是50px，有三组则x3
        //宽取每组属性中值的最多个数,的字长，一个字算20px?
        int PropertyCounter = 0;

        #region

        private int _count = 0;

        // Colors used when hot tracking over tabs
        private Color _hotMain = Color.FromArgb(255, 240, 200);
        private Color _hotEmbedSelected = Color.FromArgb(255, 241, 224);
        private Color _hotEmbedTracking = Color.FromArgb(255, 231, 162);

        // 8 example titles for the tabs
        private string[] _titleMain = new string[] { "Personal",    "Online",
                                                     "Books",       "Travel",
                                                     "Movies",      "Music",
                                                     "Recipes",     "Shopping" };

        private string[] _titleEmbedded = new string[]{ "Financial information", "Credit card accounts",
                                                        "Website logins",        "Medical information",
                                                        "Frequent flyer points", "Activities",
                                                        "Sightseeing",           "Transportation",
                                                        "Hotel information",     "Trip schedule",
                                                        "Searching",             "Take notes",
                                                        "Diary entry",           "Bug reports",
                                                        "Release schedule",      "Shared resources",
                                                        "Screen shots",          "Book list" };


        // 8 colors for when the tab is not selected
        private Color[] _normal = new Color[]{ Color.FromArgb(156, 193, 182), Color.FromArgb(247, 184, 134),
                                               Color.FromArgb(217, 173, 194), Color.FromArgb(165, 194, 215),
                                               Color.FromArgb(179, 166, 190), Color.FromArgb(234, 214, 163),
                                               Color.FromArgb(246, 250, 125), Color.FromArgb(188, 168, 225) };

        // 8 colors for when the tab is selected
        private Color[] _select = new Color[]{ Color.FromArgb(200, 221, 215), Color.FromArgb(251, 216, 188),
                                               Color.FromArgb(234, 210, 221), Color.FromArgb(205, 221, 233),
                                               Color.FromArgb(213, 206, 219), Color.FromArgb(244, 232, 204),
                                               Color.FromArgb(250, 252, 183), Color.FromArgb(218, 207, 239) };
        #endregion

        /// <summary>
        /// 加载状态标志，用于跟踪异步数据是否已加载完成
        /// </summary>
        private bool _isDataLoaded = false;

        /// <summary>
        /// 用于同步异步加载完成事件
        /// </summary>
        private readonly TaskCompletionSource<bool> _dataLoadedTcs = new TaskCompletionSource<bool>();

        public frmProductEdit()
        {
            InitializeComponent();
            if (!this.DesignMode)
            {
                // 初始化属性列表，避免空引用异常
                prodpropList = new List<tb_ProdProperty>();
                prodpropValueList = new List<tb_ProdPropertyValue>();
                Categorylist = new List<tb_ProdCategories>();
                Categorylist = _cacheManager.GetEntityList<tb_ProdCategories>();
                if (Categorylist == null || Categorylist.Count == 0)
                {
                    Categorylist = MainForm.Instance.AppContext.Db.Queryable<tb_ProdCategories>().ToList();
                }

                // 绑定表单Load事件
                this.Load += async (sender, e) => await UCProductEdit_LoadAsync(sender, e);

                // 异步加载数据
                LoadDataAsync();

                kryptonNavigator1.Button.ButtonDisplayLogic = ButtonDisplayLogic.None;
                kryptonPageMain.ClearFlags(KryptonPageFlags.All);
                kryptonPage2.ClearFlags(KryptonPageFlags.All);
                kryptonPage3.ClearFlags(KryptonPageFlags.All);
                kryptonPageImage.ClearFlags(KryptonPageFlags.All);
                //this.OnShowHelp += FrmProductEdit_OnShowHelp;
                clientBizCodeService = Startup.GetFromFac<ClientBizCodeService>();
                InitListData();
                SetBaseValue<tb_ProdDetail>();
                InitDataTocmbbox();
                // 初始化网格显示文本解析器
                DisplayTextResolver = new GridViewDisplayTextResolverGeneric<tb_ProdDetail>();
                DisplayTextResolver.Initialize(dataGridView1);
                // 配置ImagesPath列为图片列（显示缩略图，双击可查看原图）
                DisplayTextResolver.AddColumnDisplayType<tb_ProdDetail>(d => d.ImagesPath, ColumnDisplayTypeEnum.Image);
            }

            pictureBox1.AllowDrop = true;
            pictureBox1.DragEnter += new DragEventHandler(pictureBox1_DragEnter);
            pictureBox1.DragDrop += new DragEventHandler(pictureBox1_DragDrop);

        }

        /// <summary>
        /// 异步加载数据
        /// </summary>
        private async Task LoadDataAsync()
        {
            try
            {
                prodpropList = await MainForm.Instance.AppContext.Db.Queryable<tb_ProdProperty>()
                    .Includes(c => c.tb_ProdPropertyValues)
                    .Includes(c => c.tb_ProdPropertyValues, a => a.tb_prodproperty)
                    .ToListAsync();

                foreach (var item in prodpropList)
                {
                    prodpropValueList.AddRange(item.tb_ProdPropertyValues);
                }
                // 设置加载状态为完成
                _isDataLoaded = true;
                _dataLoadedTcs.TrySetResult(true);

                // 数据加载完成后，更新UI
                if (EditEntity != null)
                {
                    // 如果已经调用了BindData，重新加载SKU列表
                    LoadBaseInfoSKUList(EditEntity);
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog("错误", $"加载数据失败: {ex.Message}");
                _dataLoadedTcs.TrySetResult(false);
            }
        }




        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    string filePath = files[0];
                    if (filePath.ToLower().EndsWith(".png") || filePath.ToLower().EndsWith(".jpg") || filePath.ToLower().EndsWith(".jpeg") || filePath.ToLower().EndsWith(".bmp"))
                    {
                        pictureBox1.Image = SetImageToEntity(filePath);
                    }
                    else
                    {
                        MessageBox.Show("只能接受图片文件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }


        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    string filePath = files[0];
                    if (filePath.ToLower().EndsWith(".png") || filePath.ToLower().EndsWith(".jpg") || filePath.ToLower().EndsWith(".jpeg") || filePath.ToLower().EndsWith(".bmp"))
                    {
                        pictureBox1.Image = SetImageToEntity(filePath);
                        //pictureBox1.Imag= Image.FromFile(filePath);
                    }
                    else
                    {
                        MessageBox.Show("只能接受图片文件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }


        List<tb_ProdDetail> removeSkuList = new List<tb_ProdDetail>();

        private void DataGridView1_删除选中行(object sender, EventArgs e)
        {

            if (dataGridView1.CurrentRow.Index != -1)
            {
                tb_ProdDetail sukProd = dataGridView1.CurrentRow.DataBoundItem as tb_ProdDetail;
                bindingSourceList.Remove(sukProd);
                //将删除的sku行 暂时加入一个临时列表中
                removeSkuList.Add(sukProd);
            }

        }


        private async Task UCProductEdit_LoadAsync(object sender, EventArgs e)
        {
            // 等待数据加载完成
            await _dataLoadedTcs.Task;
            UIProdCateHelper.BindToTreeViewNoRootNode(Categorylist, txtcategory_ID.TreeView);
            // AddTopPage();
            // Do not allow the document pages to be closed or made auto hidden/docked
            kryptonNavigator1.Button.CloseButtonDisplay = ButtonDisplay.Hide;
            UIProdCateHelper.BindToTreeView(Categorylist, kryptonTreeView1, false);
            ExpandOrCollapseNodes(true);
            this.chkexpandAll.CheckedChanged += new System.EventHandler(this.chkexpandAll_CheckedChanged);
        }
        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            DataBindingHelper.InitDataToCmb<tb_Unit>(k => k.Unit_ID, v => v.UnitName, txtUnitID);
            DataBindingHelper.InitDataToCmb<tb_CustomerVendor>(k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID);
            DataBindingHelper.InitDataToCmb<tb_Location>(k => k.Location_ID, v => v.Name, txtLocation_ID);
            DataBindingHelper.InitDataToCmb<tb_ProductType>(k => k.Type_ID, v => v.TypeName, cmbType_ID);
            DataBindingHelper.InitDataToCmb<tb_StorageRack>(k => k.Rack_ID, v => v.RackName, cmbRack_ID);
            DataBindingHelper.InitDataToCmb<tb_Department>(k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            //InitDataToCmbByEnumDynamicGeneratedDataSource<tb_Prod>(typeof(GoodsSource), e => e.SourceType.ToString(), cmbSourceType);
            //EnumBindingHelper bindingHelper = new EnumBindingHelper();
            //https://www.cnblogs.com/cdaniu/p/15236857.html

        }




        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private async void btnOk_Click(object sender, EventArgs e)
        {

            //比方 暂时没有供应商  又是外键，则是如何处理的？
            bool vb = UIHelper.ShowInvalidMessage(mcProdBase.Validator(EditEntity));
            if (!vb)
            {
                return;
            }

            if (EditEntity.Is_enabled.HasValue && !EditEntity.Is_enabled.Value)
            {
                if (MessageBox.Show("产品没有启用，确定保存吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                {
                    return;
                }
            }


            if (EditEntity.Is_available.HasValue && !EditEntity.Is_available.Value)
            {
                if (MessageBox.Show("产品设置为不可用，确定保存吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                {
                    return;
                }
            }

            //给出SKU关系列表
            //一个产品明细SKU 可能对应多行关系，所以 主->sku->relation
            EditEntity.tb_ProdDetails = GetDetailsAndRelations(EditEntity, removeSkuList);

            if (EditEntity.tb_ProdDetails.Count == 0)
            {
                MessageBox.Show("产品明细不能为空！");
                return;
            }
            List<bool> subrs = new List<bool>();
            foreach (var item in EditEntity.tb_ProdDetails)
            {
                bool vd = UIHelper.ShowInvalidMessage(mcDetail.Validator(item));
                subrs.Add(vd);
            }

            if (subrs.Where(c => c.Equals(false)).Any())
            {
                return;
            }

            var _comparer = new ObjectsComparer.Comparer<tb_Prod>(
               new ComparisonSettings
               {
                   //Null and empty error lists are equal
                   EmptyAndNullEnumerablesEqual = true
               });
            if (EditEntity.tb_ProdDetails == null)
            {
                EditEntity.tb_ProdDetails = new List<tb_ProdDetail>();
            }

            //var detail = from p in EditEntity.tb_Prod_Attr_Relations select p;
            _comparer.AddComparerOverride(() => new tb_Prod().tb_Prod_Attr_Relations, DoNotCompareValueComparer.Instance);

            IEnumerable<Difference> differences;
            var isEqual = _comparer.Compare(oldOjb, EditEntity, out differences);

            if (isEqual)
            {
                #region 

                foreach (var item in EditEntity.tb_Prod_Attr_Relations)
                {
                    if (item.ActionStatus == ActionStatus.修改)
                    {
                        EditEntity.ActionStatus = ActionStatus.修改;
                        break;
                    }
                }

                foreach (var item in EditEntity.tb_ProdDetails)
                {
                    if (item.ActionStatus == ActionStatus.修改)
                    {
                        EditEntity.ActionStatus = ActionStatus.修改;
                        break;
                    }
                }

                #endregion
            }
            else
            {
                //变了
                if (EditEntity.ActionStatus == ActionStatus.无操作)
                {
                    EditEntity.ActionStatus = ActionStatus.修改;
                }
            }




            //然后再写保存逻辑
            bindingSourceEdit.EndEdit();

            //要先保存再修改才起作用

            // 检查是否有图片需要上传或删除
            if (HasImagesToUpload())
            {
                bool imageUploadSuccess = await UploadImagesIfNeeded();
                if (!imageUploadSuccess)
                {
                    MessageBox.Show("图片处理失败，请重试。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // 检查是否有SKU图片需要上传或删除
            if (HasSKUImagesToUpload())
            {
                bool skuImageUploadSuccess = await UploadSKUImagesIfNeeded();
                if (!skuImageUploadSuccess)
                {
                    MessageBox.Show("SKU图片处理失败，请重试。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        /// <summary>
        /// 获取产品明细和属性关系列表
        /// </summary>
        /// <param name="baseInfo">产品基本信息</param>
        /// <param name="removeList">需要删除的产品明细列表</param>
        /// <returns>包含完整属性关系的产品明细列表</returns>
        private List<tb_ProdDetail> GetDetailsAndRelations(tb_Prod baseInfo, List<tb_ProdDetail> removeList)
        {
            List<tb_ProdDetail> details = new List<tb_ProdDetail>();


            foreach (var item in bindingSourceList)
            {
                if (item is tb_ProdDetail)
                {
                    tb_ProdDetail epd = item as tb_ProdDetail;
                    tb_ProdDetail detail = new tb_ProdDetail();
                    //为null的不需要，不然会覆盖
                    detail = MainForm.Instance.mapper.Map<tb_ProdDetail>(epd);
                    detail.tb_Prod_Attr_Relations = new List<tb_Prod_Attr_Relation>();
                    // 设置创建/修改信息
                    if (detail.ProdDetailID == 0)
                    {
                        // 新增产品明细
                        detail.Created_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID;
                        detail.Created_at = DateTime.Now;
                        detail.ActionStatus = ActionStatus.新增;
                    }
                    else
                    {
                        // 编辑现有产品明细
                        detail.Modified_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID;
                        detail.Modified_at = DateTime.Now;
                        if (detail.ActionStatus == ActionStatus.加载)
                        {
                            detail.ActionStatus = ActionStatus.修改;
                        }
                    }

                    if (detail.ActionStatus == ActionStatus.无操作)
                    {
                        details.Add(detail);
                        continue;
                    }

                    #region 生成属性关系
                    //多属性
                    if (!string.IsNullOrEmpty(epd.PropertyGroupName))
                    {
                        //明细超过一行，则为多属性。否则是单属性，或groupName有值就是多属性了
                        List<tb_Prod_Attr_Relation> RelationList = new List<tb_Prod_Attr_Relation>();
                        //有多个属性值是，则是复合特性
                        if (epd.PropertyGroupName.Contains(","))
                        {
                            foreach (string propertyValueName in epd.PropertyGroupName.Split(','))
                            {
                                tb_Prod_Attr_Relation rela = SKUDetailToRelateion(item as tb_ProdDetail, prodpropValueList, propertyValueName);
                                rela.ActionStatus = detail.ActionStatus;
                                if (baseInfo.ProdBaseID > 0)
                                {
                                    rela.ProdBaseID = baseInfo.ProdBaseID;
                                }
                                if (detail.ProdDetailID > 0)
                                {
                                    rela.ProdDetailID = detail.ProdDetailID;
                                }
                                RelationList.Add(rela);
                            }
                        }
                        else
                        {
                            tb_Prod_Attr_Relation rela = SKUDetailToRelateion(item as tb_ProdDetail, prodpropValueList, epd.PropertyGroupName);
                            rela.ActionStatus = detail.ActionStatus;
                            if (baseInfo.ProdBaseID > 0)
                            {
                                rela.ProdBaseID = baseInfo.ProdBaseID;
                            }
                            if (detail.ProdDetailID > 0)
                            {
                                rela.ProdDetailID = detail.ProdDetailID;
                            }
                            RelationList.Add(rela);
                        }
                        detail.tb_Prod_Attr_Relations.AddRange(RelationList);
                    }
                    else
                    {
                        #region 单属性
                        if (detail.tb_Prod_Attr_Relations.Count == 0)
                        {
                            tb_Prod_Attr_Relation rela = new tb_Prod_Attr_Relation();
                            //单属性要保持为null 不然db外键冲突
                            //rela.Property_ID = -1;
                            //rela.PropertyValueID = -1;
                            rela.ActionStatus = detail.ActionStatus;
                            if (baseInfo.ProdBaseID > 0)
                            {
                                rela.ProdBaseID = baseInfo.ProdBaseID;
                            }
                            if (detail.ProdDetailID > 0)
                            {
                                rela.ProdDetailID = detail.ProdDetailID;
                            }
                            detail.tb_Prod_Attr_Relations.Add(rela);
                        }
                        #endregion
                    }
                    #endregion

                    details.Add(detail);
                }
            }


            //处理需要删除的产品明细
            foreach (var item in removeList)
            {
                if (item.ProdDetailID > 0)
                {
                    tb_ProdDetail removeDetail = new tb_ProdDetail();
                    removeDetail = MainForm.Instance.mapper.Map<tb_ProdDetail>(item);
                    removeDetail.ActionStatus = ActionStatus.删除;
                    //设置所有关联的属性关系为删除状态
                    foreach (var relation in removeDetail.tb_Prod_Attr_Relations)
                    {
                        relation.ActionStatus = ActionStatus.删除;
                    }
                    details.Add(removeDetail);
                }
            }
            return details;
        }



        /// <summary>
        /// 将SKU表格中显示的明细转换为属性关系
        /// 确保返回的属性关系包含完整的属性信息
        /// </summary>
        /// <param name="item">产品详情对象</param>
        /// <param name="prodPropertyValues">属性值列表</param>
        /// <param name="propertyValueName">属性值名称</param>
        /// <returns>包含完整属性信息的属性关系</returns>
        private tb_Prod_Attr_Relation SKUDetailToRelateion(tb_ProdDetail item, List<tb_ProdPropertyValue> prodPropertyValues, string propertyValueName)
        {
            // 确保参数有效
            if (item == null || string.IsNullOrEmpty(propertyValueName))
            {
                return new tb_Prod_Attr_Relation();
            }

            tb_Prod_Attr_Relation relation = new tb_Prod_Attr_Relation();

            // 根据属性值名称查找属性值对象
            tb_ProdPropertyValue propertyValue = prodPropertyValues.FirstOrDefault(pv => pv.PropertyValueName == propertyValueName);
            if (propertyValue == null)
            {
                return relation;
            }

            // 设置属性关系的基本信息
            relation.Property_ID = propertyValue.Property_ID;
            relation.PropertyValueID = propertyValue.PropertyValueID;

            // 设置属性关系的操作状态
            relation.ActionStatus = item.ActionStatus;

            // 确保属性对象已加载
            relation.tb_prodpropertyvalue = propertyValue;

            // 加载属性对象
            relation.tb_prodproperty = prodpropList.FirstOrDefault(p => p.Property_ID == propertyValue.Property_ID);

            // 检查现有关系中是否已存在相同的属性关系
            var existingRelation = item.tb_Prod_Attr_Relations.FirstOrDefault(r =>
                r.Property_ID == propertyValue.Property_ID && r.PropertyValueID == propertyValue.PropertyValueID);

            if (existingRelation != null)
            {
                // 如果存在现有关系，使用其ID
                relation.RAR_ID = existingRelation.RAR_ID;
            }

            return relation;
        }

        private tb_Prod _EditEntity;
        public tb_Prod EditEntity { get => _EditEntity; set => _EditEntity = value; }

        List<tb_ProdCategories> categorylist = new List<tb_ProdCategories>(0);

        public List<tb_ProdCategories> Categorylist { get => categorylist; set => categorylist = value; }

        tb_Prod oldOjb = null;


        /// <summary>
        /// 验证生成产品编号规则所需的前置条件
        ///  1. 产品类型必须选择
        ///  2. 产品类目必须选择
        /// </summary>
        /// <param name="product">产品实体</param>
        /// <returns>验证结果消息，如果为空表示验证通过</returns>
        private (string, bool) ValidateProductCodeGeneration(tb_Prod product)
        {
            //改为元组返回值

            if (product.Type_ID <= 0)
            {
                return ("请选择产品类型", false);
            }

            if (product.Category_ID <= 0)
            {
                return ("请选择产品类目", false);
            }

            return (string.Empty, true);
        }

        /// <summary>
        /// 生成产品相关编号
        /// </summary>
        private async Task GenerateProductCodes()
        {
            // 验证生成产品编号规则所需的前置条件
            var (validationMessage, isValid) = ValidateProductCodeGeneration(EditEntity);
            if (!isValid)
            {
                // 显示验证失败消息并返回
                MessageBox.Show(validationMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var bizCodeService = Startup.GetFromFac<ClientBizCodeService>();
            var obj = _cacheManager.GetEntity<tb_ProdCategories>(EditEntity.Category_ID);
            if (obj != null && obj.ToString() != "System.Object")
            {
                if (obj is tb_ProdCategories Cate)
                {
                    string constpara = string.Empty;
                    var para = Cate.Category_name;
                    if (para.Length < 5)
                    {
                        constpara = para.ToPinYin(true).Substring(0, para.Length);
                    }
                    else
                    {
                        constpara = para.ToPinYin(true).Substring(0, 5);
                    }

                    _EditEntity.ShortCode = await bizCodeService.GenerateProductRelatedCodeAsync(BaseInfoType.ShortCode, EditEntity, constpara); //推荐
                }
            }
            if (EditEntity.PropertyType == (int)ProductAttributeType.单属性)
            {
                //EditEntity.tb_ProdDetails  = await bizCodeService.GenerateProductRelatedCodeAsync(BaseInfoType.SKU_No); //推荐
            }

            if (bizCodeService != null)
            {
                _EditEntity.ProductNo = await bizCodeService.GenerateProductRelatedCodeAsync(BaseInfoType.ProductNo, EditEntity);
            }
        }

        public async override void BindData(BaseEntity entity)
        {
            oldOjb = CloneHelper.DeepCloneObject<tb_Prod>(entity);
            _EditEntity = entity as tb_Prod;
            if (_EditEntity.ProdBaseID == 0)
            {
                _EditEntity.DataStatus = (int)RUINORERP.Global.DataStatus.草稿;
                _EditEntity.ActionStatus = ActionStatus.新增;
                //助记码要在类目选择后生成，要有规律
                //详情直接清空，因为是新增 ，属性这块不清楚。后面再优化：TODO:
                _EditEntity.tb_ProdDetails = new List<tb_ProdDetail>();

                _EditEntity.tb_Prod_Attr_Relations = new List<tb_Prod_Attr_Relation>();
                var (validationMessage, isValid) = ValidateProductCodeGeneration(EditEntity);
                if (isValid)
                {
                    //当用户使用复制性新增加时，如果已经选择了产品类型和产品类别， 助记码为空时， 则自动生成助记码。
                    if (string.IsNullOrEmpty(_EditEntity.ShortCode))
                    {
                        await GenerateProductCodes();
                    }
                    //同时也生成产品编码
                    if (string.IsNullOrEmpty(_EditEntity.ProductNo))
                    {
                        await GenerateProductCodes();
                    }
                }
            }
            else
            {
                // 显示图片
                // 使用现有的pictureBox1控件显示单张图片
                if (_EditEntity.Images != null && _EditEntity.Images.Length > 0)
                {
                    try
                    {
                        using (var ms = new MemoryStream(_EditEntity.Images))
                        {
                            pictureBox1.Image = Image.FromStream(ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.uclog.AddLog($"加载图片失败: {ex.Message}", UILogType.错误);
                    }
                }
                if (EditEntity != null && EditEntity.PropertyType == (int)ProductAttributeType.单属性)
                {
                    this.dataGridView1.HideColumn<tb_ProdDetail>(c => c.PropertyGroupName);
                }
            }

            #region 类别
            var parent_categorie = new Binding("Text", entity, "category_ID", true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            parent_categorie.Format += new ConvertEventHandler(DataSourceToControl);
            //将控件的数据类型转换为数据源要求的数据类型。
            parent_categorie.Parse += new ConvertEventHandler(ControlToDataSource);
            txtcategory_ID.DataBindings.Add(parent_categorie);

            #endregion

            DataBindingHelper.BindData4CmbByEnum<tb_Prod, GoodsSource>(entity, k => k.SourceType, cmbSourceType, false);
            DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.ProductNo, txtNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.ShortCode, txtShortCode, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.VendorModelCode, txtVendorModelCode, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text, false);

            #region 产品图片
            if (_EditEntity.Images != null && _EditEntity.Images.Length > 0)
            {
                try
                {
                    //Image 是指图片属性，Images是数据库中存储的图片的字段
                    Binding img = new Binding("Image", entity, "Images", true, DataSourceUpdateMode.OnValidation);
                    img.Format += new ConvertEventHandler(PictureFormat);
                    img.Parse += new ConvertEventHandler(Img_Parse);
                    pictureBox1.DataBindings.Add(img);
                }
                catch (Exception exx)
                {


                }
            }

            #endregion

            DataBindingHelper.BindData4Cmb<tb_Unit>(entity, k => k.Unit_ID, v => v.UnitName, txtUnitID);
            //DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID);
            DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v => v.Name, txtLocation_ID);
            DataBindingHelper.BindData4Cmb<tb_ProductType>(entity, k => k.Type_ID, v => v.TypeName, cmbType_ID);

            #region 产品基本图片加载
            // 下载并加载产品基本图片到 magicPictureBox产品基本图片
            if (_EditEntity.ProdBaseID > 0)
            {
                try
                {
                    await DownloadProductImagesAsync(_EditEntity, magicPictureBox产品基本图片);
                }
                catch (Exception ex)
                {
                    MainForm.Instance.uclog.AddLog($"下载产品基本图片失败: {ex.Message}", Global.UILogType.错误);
                }
            }
            #endregion
            DataBindingHelper.BindData4CmbByEntity<tb_StorageRack>(entity, k => k.Rack_ID, cmbRack_ID);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);

            DataBindingHelper.BindData4CmbByEnum<tb_Prod, Global.ProductAttributeType>(entity, k => k.PropertyType, cmbPropertyType, true, Global.ProductAttributeType.捆绑, Global.ProductAttributeType.虚拟);
            DataBindingHelper.BindData4TextBox<tb_Prod>(entity, k => k.CNName, txtName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_Prod>(entity, exp => exp.Is_enabled, txtis_enabled, false);
            DataBindingHelper.BindData4CheckBox<tb_Prod>(entity, exp => exp.Is_available, txtis_available, false);
            DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.ENName, txtENName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.ProductCNDesc, txtProductCNDesc, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.ProductENDesc, txtProductENDesc, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.Brand, txtBrand, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.CustomsCode, txtCustomsCode, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.Tag, txtTag, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4CheckBox<tb_Prod>(entity, exp => exp.SalePublish, chkSalePublish, false);

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_ProdValidator>(), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }

            base.BindData(entity);
            dataGridView1.NeedSaveColumnsXml = true;

            // 等待数据加载完成，然后再加载SKU列表
            await _dataLoadedTcs.Task;

            // 数据加载完成后，执行依赖数据的操作
            LoadBaseInfoSKUList(_EditEntity);

            // 加载SKU图片（如果是编辑模式）
            if (entity.ActionStatus == ActionStatus.加载 || entity.ActionStatus == ActionStatus.修改 || entity.ActionStatus == ActionStatus.无操作)
            {
                await LoadSKUImagesAsync(_EditEntity);
            }

            listView1.UpdateUI();
            Task task_2 = Task.Run(task_Help);
            //task_2.Wait();  //注释打开则等待task_2延时，注释掉则不等待

            EditEntity.PropertyChanged += async (sender, s2) =>
            {
                // 监听产品类目变更事件
                if (EditEntity.Category_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_Prod>(c => c.Category_ID))
                {
                    await GenerateProductCodes();
                }
                // 监听产品类型变更事件
                else if (EditEntity.Type_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_Prod>(c => c.Type_ID))
                {
                    if (EditEntity.Category_ID > 0)
                    {
                        await GenerateProductCodes();
                    }
                }
                if (EditEntity.PropertyType > 0 && s2.PropertyName == entity.GetPropertyName<tb_Prod>(c => c.PropertyType))
                {
                    #region  产品属性联动

                    if (cmbPropertyType.Tag == null)
                    {
                        return;
                    }
                    object selectObj = cmbPropertyType.SelectedValue;
                    if (cmbPropertyType.SelectedItem != null && selectObj.ObjToInt() != -1)
                    {
                        EditEntity.PropertyType = (int)cmbPropertyType.SelectedValue;
                        ProductAttributeType pt = (ProductAttributeType)cmbPropertyType.SelectedValue;
                        switch (pt)
                        {
                            case ProductAttributeType.单属性:
                                ControlBtn(pt, EditEntity.ActionStatus);
                                #region 新增修改式
                                if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ProdBaseID == 0)
                                {
                                    bindingSourceList.Clear();
                                    //listView1.ShowGroups = true;  //记得要设置ShowGroups属性为true（默认是false），否则显示不出分组

                                    if (dataGridView1.Rows.Count == 0)
                                    {
                                        BindToSkulistGrid(new List<tb_ProdDetail>());
                                    }
                                    if (EditEntity.ActionStatus != ActionStatus.加载)
                                    {
                                        tb_ProdDetail ppg = new tb_ProdDetail();
                                        ppg.PropertyGroupName = "";
                                        // 验证生成产品编号规则所需的前置条件
                                        var (validationMessage3, isValid3) = ValidateProductCodeGeneration(EditEntity);
                                        if (!isValid3)
                                        {
                                            // 显示验证失败消息并返回
                                            MessageBox.Show(validationMessage3, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            return;
                                        }

                                        if (EditEntity.ProductNo.Trim().Length > 0)
                                        {
                                            EditEntity.tb_ProdDetails.Add(ppg);
                                            ppg.SKU = await clientBizCodeService.GenerateProductSKUCodeAsync(BaseInfoType.SKU_No, EditEntity, ppg);
                                        }
                                        bindingSourceList.Add(ppg);
                                    }

                                }
                                #endregion

                                if (dataGridView1.Columns.Contains("GroupName"))
                                {
                                    dataGridView1.Columns["GroupName"].Visible = false;
                                }
                                // 检查产品是否为多属性类型
                                if (EditEntity != null && EditEntity.PropertyType == (int)ProductAttributeType.单属性)
                                {
                                    this.dataGridView1.HideColumn<tb_ProdDetail>(c => c.PropertyGroupName);
                                }
                                break;
                            case ProductAttributeType.可配置多属性:
                                ControlBtn(pt, EditEntity.ActionStatus);
                                cmb属性.Enabled = true;
                                btnAddProperty.Enabled = true;
                                bindingSourceList.Clear();
                                // listView1.ItemCheck -= ListView1_ItemCheck;
                                //  listView1.ItemCheck += ListView1_ItemCheck;
                                //绑定对应的选项及其值
                                DataBindingHelper.InitDataToCmb<tb_ProdProperty>(p => p.Property_ID, t => t.PropertyName, cmb属性);
                                cmb属性.SelectedIndex = -1;
                                if (EditEntity.tb_Prod_Attr_Relations != null)
                                {
                                    //编辑性加载
                                }
                                // 检查产品是否为多属性类型
                                if (EditEntity != null && EditEntity.PropertyType == (int)ProductAttributeType.可配置多属性)
                                {
                                    this.dataGridView1.ShowColumn<tb_ProdDetail>(c => c.PropertyGroupName);
                                }

                                break;
                            case ProductAttributeType.捆绑:
                                break;
                            case ProductAttributeType.虚拟:
                                break;
                            default:
                                break;
                        }

                    }

                    #endregion
                }
            };

            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, c => c.IsVendor == true);

            //创建表达式
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.IsVendor == true)
                            .ToExpression();//注意 这一句 不能少
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
            queryFilterC.FilterLimitExpressions.Add(lambda);

            DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterC);

        }

        private void Img_Parse(object sender, ConvertEventArgs e)
        {

        }


        public async Task task_Help()
        {
            await Task.Delay(1000);
            System.Diagnostics.Debug.WriteLine("2秒后执行，方式二 输出语句...");
            base.InitHelpInfoToControl(kryptonPanel1.Controls);
            if (_EditEntity.tb_Prod_Attr_Relations != null)
            {
                //编辑多属性时才需要编辑
                if (_EditEntity.tb_Prod_Attr_Relations.Count > 1)
                {
                    //listView1.ItemCheck -= ListView1_ItemCheck;
                    // listView1.ItemCheck += ListView1_ItemCheck;

                    AttrGoups = GetAttrGoups(listView1);
                }

            }
            kryptonNavigator1.SelectedPage = kryptonPageMain;
        }

        void PictureFormat(Object sender, ConvertEventArgs e)
        {
            if (e.Value == null)
            {
                e.Value = Image.FromStream(Common.DataBindingHelper.GetResource("help4"));
                return;
            }
            // e.Value is the original value
            Byte[] img = (Byte[])e.Value;
            e.Value = ImageHelper.ByteArrayToImage(img);
        }

        private void DataSourceToControl(object sender, ConvertEventArgs cevent)
        {
            // 该方法仅转换为字符串类型。使用DesiredType进行测试。
            if (cevent.DesiredType != typeof(string)) return;
            if (cevent.Value == null || cevent.Value.ToString() == "0")
            {
                //cevent.Value = ((decimal)cevent.Value).ToString("c");
                cevent.Value = "";
            }
            else
            {
                cevent.Value = _cacheManager.GetEntity<tb_ProdCategories>(cevent.Value.ToString()).Category_name;
            }

        }

        private async void ControlToDataSource(object sender, ConvertEventArgs cevent)
        {

            if (string.IsNullOrEmpty(cevent.Value.ToString()) || cevent.Value.ToString() == "类目根结节")
            {
                cevent.Value = null;
            }
            else
            {
                tb_ProdCategories entity = Categorylist.Find(t => t.Category_name == cevent.Value.ToString());
                if (entity != null)
                {
                    cevent.Value = entity.Category_ID;

                }
                else
                {
                    cevent.Value = 0;
                }
            }
        }

        private async void chkAutoCreateProdNo_CheckedChanged(object sender, EventArgs e)
        {
            txtNo.ReadOnly = chkAutoCreateProdNo.Checked;
            if (chkAutoCreateProdNo.Checked)
            {
                // 使用新的BizCodeService生成产品编号
                try
                {
                    if (EditEntity.Category_ID <= 0)
                    {
                        MessageBox.Show("警告：请选选择产品类目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    var bizCodeService = Startup.GetFromFac<RUINORERP.UI.Network.Services.ClientBizCodeService>();
                    if (bizCodeService != null)
                    {
                        txtNo.Text = await bizCodeService.GenerateProductRelatedCodeAsync(BaseInfoType.ProductNo, EditEntity);
                    }
                    else
                    {
                        // 降级方案：如果服务不可用，使用原来的方法
                        txtNo.Text = Common.BarCodeCreator.CreateProductBarCode(EditEntity.Category_ID.ToString(), true, "32324");
                        MessageBox.Show("警告：无法使用服务器生成产品编码，已使用本地生成方式", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    // 异常处理：发生错误时使用本地生成方式作为备用
                    txtNo.Text = Common.BarCodeCreator.CreateProductBarCode(EditEntity.Category_ID.ToString(), true, "32324");
                    MessageBox.Show($"生成产品编码时发生错误：{ex.Message}\n已使用本地生成方式作为备用", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                txtNo.Text = SqlSugar.SnowFlakeSingle.Instance.NextId().ToString();
            }
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {
            ToolTip tool = (ToolTip)sender;
            if (e.AssociatedControl.Name == "textBox1")//e代表我们要触发的事件，我们是在textBox1触发
            {
                tool.ToolTipTitle = "提示信息";//修改标题
                tool.ToolTipIcon = ToolTipIcon.Info;//修改图标
            }
            else
            {
                tool.ToolTipTitle = "警告信息";
                tool.ToolTipIcon = ToolTipIcon.Warning;
            }
        }



        private void btnAddImage_Click(object sender, EventArgs e)
        {
            FindImage();
        }

        private void btnClearImage_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            EditEntity.Images = null;
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            FindImage();
        }

        private void FindImage()
        {
            if (openFileDialog4Img.ShowDialog() == DialogResult.OK)
            {
                string pathName = openFileDialog4Img.FileName;
                this.pictureBox1.Image = SetImageToEntity(pathName);
            }
        }


        private Image SetImageToEntity(string pathName)
        {
            // 先读取原始图片字节
            byte[] originalBytes = File.ReadAllBytes(pathName);

            Image img = null;
            try
            {
                // 使用MemoryStream避免文件锁定
                using (var ms = new MemoryStream(originalBytes))
                {
                    img = Image.FromStream(ms);

                    // 检查大小是否超过200KB
                    if (originalBytes.Length > 200 * 1024)
                    {
                        // 压缩图片（直接在内存中操作）
                        using (var compressedImg = new Bitmap(img, new Size(800, 600)))
                        using (var msOut = new MemoryStream())
                        {
                            ImageCodecInfo jpegCodec = GetEncoderInfo(ImageFormat.Jpeg);
                            EncoderParameters encoderParams = new EncoderParameters(1);
                            encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);

                            compressedImg.Save(msOut, jpegCodec, encoderParams);
                            byte[] compressedBytes = msOut.ToArray();

                            _EditEntity.Images = compressedBytes;
                            //this.pictureBox1.Image = (Image)compressedImg.Clone();

                            MessageBox.Show("图片大小超过200KB，已自动压缩。", "提示",
                                           MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        _EditEntity.Images = originalBytes;
                        //this.pictureBox1.Image = (Image)img.Clone();
                    }

                    return (Image)img.Clone();
                }
            }
            finally
            {
                img?.Dispose();
            }
        }

        private ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }




        private void ControlBtn(ProductAttributeType pat, ActionStatus action)
        {
            switch (pat)
            {
                case ProductAttributeType.单属性:
                    cmb属性.Enabled = false;
                    btnAddProperty.Enabled = false;
                    btnClear.Enabled = false;

                    if (action == ActionStatus.加载)
                    {

                    }
                    break;
                case ProductAttributeType.可配置多属性:
                    cmb属性.Enabled = true;
                    btnAddProperty.Enabled = true;
                    btnClear.Enabled = true;


                    if (action == ActionStatus.加载)
                    {

                    }
                    break;
                case ProductAttributeType.捆绑:
                    break;
                case ProductAttributeType.虚拟:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 加载多属性关系列表，先统计出属性，加载，再加载数据行
        /// 编辑的时候不能再添加属性了，最多删除？
        /// </summary>
        /// <param name="relations"></param>
        private void LoadBaseInfoSKUList(tb_Prod entityProdBase)
        {
            List<tb_Prod_Attr_Relation> relations = entityProdBase.tb_Prod_Attr_Relations;
            if (relations == null || relations.Count == 0)
            {
                //正常数据都不会为空
                return;
            }
            else
            {
                dataGridView1.删除选中行 += DataGridView1_删除选中行;
                btnClear.Enabled = false;
                cmbPropertyType.Enabled = false;
            }
            //单个属性 支付添加删除修改，
            //多个属性  不支持添加属性，只能删除修改内容，在属性不变时可以添加没有加入的属性值的情况？
            //不显示属性下拉，不支持添加？
            var attrs = relations.GroupBy(e => e.Property_ID);
            int propertyCounter = attrs.ToList().Count;
            listView1.Clear();

            if (propertyCounter == 1)
            {
                // listView1.ItemCheck += ListView1_ItemCheck;
            }
            else
            {
                listView1.Enabled = true;
                string tips = "【产品编辑时，不可以对属性修改】";
                //listView1.ItemCheck += ListView1_ItemCheck;
                MainForm.Instance.uclog.AddLog("消息", tips);
            }

            #region 属性UI处理
            foreach (var item in attrs)
            {
                if (item.Key == null)
                {
                    //单属性
                    btnAddProperty.Enabled = false;
                }
                else
                {
                    //多属性
                    tb_ProdProperty prodProperty = prodpropList.FirstOrDefault(x => x.Property_ID.ToString() == item.Key.Value.ToString());
                    List<tb_ProdPropertyValue> listOptionValue = prodpropValueList.FindAll(x => x.Property_ID == prodProperty.Property_ID);
                    List<tb_Prod_Attr_Relation> oneGroup = relations.Where(w => w.Property_ID == prodProperty.Property_ID).ToList();
                    LoadProdProperty(propertyCounter, prodProperty, listOptionValue, oneGroup);
                    btnAddProperty.Enabled = true;
                }

            }
            #endregion

            #region SKU表格

            if (dataGridView1.Rows.Count == 0)
            {
                //先绑一个空的架构
                BindToSkulistGrid(new List<tb_ProdDetail>());
                bindingSourceList.ListChanged += BindingSourceList_ListChanged;
            }

            // 清空现有数据，避免重复加载
            bindingSourceList.Clear();

            //显示表格内容 根据sku更新
            LoadRelationToEavSku(entityProdBase);
            #endregion


        }




        private void cmb属性_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb属性.SelectedItem is tb_ProdProperty)
            {
                tb_ProdProperty ppv = cmb属性.SelectedItem as tb_ProdProperty;
                if (!listView1.Groups.Cast<TileGroup>().Any(i => i.GroupID == ppv.Property_ID.ToString().Trim()))
                {
                    btnAddProperty.Enabled = true;
                }
            }
            return;
        }

        private ConcurrentDictionary<string, string> propertyEavList = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 保存属性及对应选中的值
        /// </summary>
        public ConcurrentDictionary<string, string> PropertyEavList { get => propertyEavList; set => propertyEavList = value; }
        /// <summary>
        /// 属性原始组合队列
        /// </summary>
        public List<KeyValuePair<long, string[]>> AttrGoups { get => _attrGoups; set => _attrGoups = value; }

        private List<KeyValuePair<long, string[]>> _attrGoups = new List<KeyValuePair<long, string[]>>();

        //UCSKUlist ucskulist = new UCSKUlist();

        #region 属性组合辅助类

        /// <summary>
        /// 属性值对，包含属性和属性值的完整信息
        /// </summary>

        #endregion
        private void btnAddProperty_Click(object sender, EventArgs e)
        {
            if (cmb属性.SelectedItem == null)
            {
                return;
            }
            if (cmb属性.SelectedItem is tb_ProdProperty)
            {
                tb_ProdProperty ppv = cmb属性.SelectedItem as tb_ProdProperty;
                if (ppv.Property_ID == -1)
                {
                    return;
                }

                // listView1.ItemCheck -= ListView1_ItemCheck;
                AddProdProperty(ppv, prodpropValueList);
                listView1.UpdateUI();
                // listView1.ItemCheck += ListView1_ItemCheck;
                PropertyCounter = listView1.Groups.Length;
                //PropertyValueMaxCounter=prodpropValueList.Where(w=>w.)
                ControlBtn(ProductAttributeType.可配置多属性, EditEntity.ActionStatus);
                btnAddProperty.Enabled = false;
            }

            AttrGoups = GetAttrGoups(listView1);
            CreateSKUList();

        }
        /// <summary>
        /// 加载产品属性
        /// </summary>
        /// <param name="propertyCounter">特性组的个数</param>
        /// <param name="ppv">特性</param>
        /// <param name="listOptionValue">属性值列表</param>
        /// <param name="oneGroup">一个组对应的关系列表</param>
        private void LoadProdProperty(int propertyCounter, tb_ProdProperty ppv, List<tb_ProdPropertyValue> listOptionValue, List<tb_Prod_Attr_Relation> oneGroup)
        {
            #region 新增修改式
            tb_ProdProperty pp = ppv;
            //create goups

            if (!contextMenuStrip1.Items.ContainsKey("【" + pp.PropertyName + "】全选") && propertyCounter == 1)
            {
                //加入分割线 美观一下
                if (contextMenuStrip1.Items.Count > 0)
                {
                    AddContextMenu("-", contextMenuStrip1.Items, menuClicked);
                    //属性都多了，之前的值全是不需要的
                    bindingSourceList.Clear();
                }
                //添加菜单   
                var yes = AddContextMenu("【" + pp.PropertyName + "】全选", contextMenuStrip1.Items, menuClicked);
                yes.Tag = pp;
                var no = AddContextMenu("【" + pp.PropertyName + "】全不选", contextMenuStrip1.Items, menuClicked);
                no.Tag = pp;
            }


            if (!listView1.Groups.Cast<TileGroup>().Any(i => i.GroupID == pp.Property_ID.ToString().Trim()))
            {

                TileGroup tileGroup = listView1.AddGroup(ppv.Property_ID.ToString(), ppv.PropertyName.Trim());
                tileGroup.BusinessData = ppv;
                //KeyValuePair<string, string> kv = new KeyValuePair<string, string>();
                string keys = string.Empty;
                string names = string.Empty;
                foreach (var item in listOptionValue)
                {
                    keys += item.PropertyValueID + ",";
                    names += item.PropertyValueName + ",";


                    bool isChecked = oneGroup.Exists(v => v.PropertyValueID == item.PropertyValueID);
                    CheckBox checkBox = listView1.AddItemToGroup(ppv.Property_ID.ToString(), item.PropertyValueName, isChecked, item.PropertyValueID.ToString(), item);
                    checkBox.CheckStateChanged += CheckBox_CheckStateChanged;

                }
                keys = keys.Trim(',');
                names = names.Trim(',');
                if (!string.IsNullOrEmpty(names))
                {
                    propertyEavList.TryAdd(pp.Property_ID.ToString(), names);
                }
            }

            #endregion
        }

        private void BindingSourceList_ListChanged(object sender, ListChangedEventArgs e)
        {
            tb_ProdDetail entity = new tb_ProdDetail();
            switch (e.ListChangedType)
            {
                case ListChangedType.Reset:
                    break;
                case ListChangedType.ItemAdded:
                    entity = bindingSourceList.List[e.NewIndex] as tb_ProdDetail;
                    if (entity.ActionStatus != ActionStatus.加载)
                    {
                        entity.ActionStatus = ActionStatus.新增;
                    }
                    break;
                case ListChangedType.ItemDeleted:
                    if (e.NewIndex < bindingSourceList.Count)
                    {
                        entity = bindingSourceList.List[e.NewIndex] as tb_ProdDetail;
                        entity.ActionStatus = ActionStatus.删除;
                    }
                    break;
                case ListChangedType.ItemMoved:
                    break;
                case ListChangedType.ItemChanged:
                    entity = bindingSourceList.List[e.NewIndex] as tb_ProdDetail;
                    if (entity.ActionStatus == ActionStatus.无操作)
                    {
                        entity.ActionStatus = ActionStatus.修改;
                    }
                    if (entity.ActionStatus == ActionStatus.新增)
                    {
                        entity.ActionStatus = ActionStatus.修改;
                    }
                    if (entity.ActionStatus == ActionStatus.加载)
                    {
                        entity.ActionStatus = ActionStatus.修改;
                    }
                    break;
                case ListChangedType.PropertyDescriptorAdded:
                    break;
                case ListChangedType.PropertyDescriptorDeleted:
                    break;
                case ListChangedType.PropertyDescriptorChanged:
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// 加载SKU表格
        /// </summary>
        /// <param name="isMultProperty"></param>
        /// <param name="entityProdBase"></param>
        private void LoadRelationToEavSku(tb_Prod entityProdBase)
        {
            bool isMultProperty = false;
            List<tb_Prod_Attr_Relation> relations = entityProdBase.tb_Prod_Attr_Relations;
            List<tb_ProdDetail> propGroups = new List<tb_ProdDetail>();
            if (bindingSourceList.DataSource is List<tb_ProdDetail>)
            {
                propGroups = bindingSourceList.DataSource as List<tb_ProdDetail>;
            }

            //为了显示属性值中文
            //根据关系明细中的详情id分组 相同明细有多行属性值时，
            #region 新思路
            foreach (tb_ProdDetail detail in entityProdBase.tb_ProdDetails)
            {
                tb_ProdDetail ppg = MainForm.Instance.mapper.Map<tb_ProdDetail>(detail);
                ppg.tb_Prod_Attr_Relations = new List<tb_Prod_Attr_Relation>();

                List<tb_Prod_Attr_Relation> pars = relations.FindAll(w => w.ProdDetailID == detail.ProdDetailID).ToList();
                string groupName = string.Empty;
                foreach (tb_Prod_Attr_Relation par in pars)
                {
                    ppg.tb_Prod_Attr_Relations.Add(par);
                    if (par.Property_ID != null && par.PropertyValueID != null)
                    {
                        tb_ProdPropertyValue ppv = prodpropValueList.FirstOrDefault(f => f.PropertyValueID == par.PropertyValueID);
                        groupName += ppv.PropertyValueName + ",";
                    }
                    else
                    {
                        groupName = "";
                    }
                }
                groupName = groupName.TrimEnd(',');
                if (groupName.Split(',').Length > 0)
                {
                    isMultProperty = true;
                }

                //使用现有的tb_ProdDetail对象，添加GroupName临时字段
                ppg.PropertyGroupName = groupName;
                bindingSourceList.Add(ppg);
                ppg.ActionStatus = ActionStatus.加载;
            }

            #endregion

            if (!isMultProperty)
            {
                btnAddProperty.Enabled = false;
                btnClear.Enabled = false;
            }

        }



        /// <summary>
        /// 添加产品特性
        /// </summary>
        private void AddProdProperty(tb_ProdProperty ppv, List<tb_ProdPropertyValue> listOptionValue)
        {
            #region 新增修改式

            //create goups
            //ListViewGroup lvg = new ListViewGroup();  //创建男生分组
            //lvg.Header = ppv.PropertyName;  //设置组的标题。
            //lvg.Name = ppv.Property_ID.ToString();
            //lvg.Tag = ppv;
            //lvg.HeaderAlignment = HorizontalAlignment.Left;   //设置组标题文本的对齐方式。（默认为Left）

            if (!contextMenuStrip1.Items.ContainsKey("【" + ppv.PropertyName + "】全选"))
            {
                //加入分割线 美观一下
                if (contextMenuStrip1.Items.Count > 0)
                {
                    AddContextMenu("-", contextMenuStrip1.Items, menuClicked);
                    //属性都多了，之前的值全是不需要的
                    bindingSourceList.Clear();
                }
                //添加菜单   
                var yes = AddContextMenu("【" + ppv.PropertyName + "】全选", contextMenuStrip1.Items, menuClicked);
                yes.Tag = ppv;
                var no = AddContextMenu("【" + ppv.PropertyName + "】全不选", contextMenuStrip1.Items, menuClicked);
                no.Tag = ppv;
            }

            if (!listView1.Groups.Cast<TileGroup>().Any(i => i.GroupID == ppv.Property_ID.ToString().Trim()))
            {
                TileGroup tileGroup = listView1.AddGroup(ppv.Property_ID.ToString(), ppv.PropertyName.Trim());
                tileGroup.BusinessData = ppv;

                string keys = string.Empty;
                string names = string.Empty;
                foreach (var item in listOptionValue.Where(w => w.Property_ID == ppv.Property_ID).ToList())
                {
                    keys += item.PropertyValueID + ",";
                    names += item.PropertyValueName + ",";
                    CheckBox checkBox = listView1.AddItemToGroup(ppv.Property_ID.ToString(), item.PropertyValueName, false, item.PropertyValueID.ToString(), item);
                    checkBox.CheckStateChanged += CheckBox_CheckStateChanged;
                }
                keys = keys.Trim(',');
                names = names.Trim(',');
                if (!string.IsNullOrEmpty(names))
                {
                    propertyEavList.TryAdd(ppv.Property_ID.ToString(), names);
                }
            }
            if (dataGridView1.Rows.Count == 0)
            {
                BindToSkulistGrid(new List<tb_ProdDetail>());
            }


            #endregion
        }

        private void CheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            #region 思路 与GetAttrGoups(listView1) 不一样，因为选择状态问题
            AttrGoups = GetAttrGoups(listView1);
            CheckBox cb = sender as CheckBox;
            if (cb.Tag is tb_ProdPropertyValue ppv)
            {
                tb_ProdProperty tpp = ppv.tb_prodproperty;
                //先找到这个属性组
                List<KeyValuePair<long, string[]>> exitkvps = AttrGoups.Where(t => t.Key == tpp.Property_ID).ToList();
                if (exitkvps.Count > 0)
                {
                    #region

                    List<string> text = exitkvps[0].Value.ToList();
                    if (cb.CheckState == CheckState.Checked)//添加
                    {
                        if (!text.Contains(cb.Text))
                        {
                            text.Add(cb.Text);
                            //联动下拉
                        }
                    }
                    else//取消
                    {
                        text.Remove(cb.Text);
                        //联动下拉
                        #region

                        #endregion
                    }
                    KeyValuePair<long, string[]> kvp = new KeyValuePair<long, string[]>(tpp.Property_ID, text.ToArray());
                    AttrGoups.Remove(exitkvps[0]);
                    AttrGoups.Add(kvp);
                    #endregion
                }
                else
                {
                    //不存在这个情况
                }


            }
            #endregion

            //编辑时的添加
            if (listView1.Enabled)
            {
                CreateSKUList();
            }
        }

        private void ListView1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            #region 思路 与GetAttrGoups(listView1) 不一样，因为选择状态问题

            //attrGoups
            ListView lv = sender as ListView;
            if (lv.Items[e.Index].Tag is tb_ProdPropertyValue)
            {
                //tb_ProdPropertyValue ci = lv.Items[e.Index].Tag as tb_ProdPropertyValue;
                CheckState nv = e.NewValue;
                if (lv.Items[e.Index].Group.Tag is tb_ProdProperty)
                {
                    tb_ProdProperty tpp = lv.Items[e.Index].Group.Tag as tb_ProdProperty;
                    //先找到这个属性组
                    List<KeyValuePair<long, string[]>> exitkvps = AttrGoups.Where(t => t.Key == tpp.Property_ID).ToList();
                    if (exitkvps.Count > 0)
                    {
                        #region

                        List<string> text = exitkvps[0].Value.ToList();
                        if (nv == CheckState.Checked)//添加
                        {
                            if (!text.Contains(lv.Items[e.Index].Text))
                            {
                                text.Add(lv.Items[e.Index].Text);
                                //联动下拉
                            }
                        }
                        else//取消
                        {
                            text.Remove(lv.Items[e.Index].Text);
                            //联动下拉
                            #region

                            #endregion
                        }
                        KeyValuePair<long, string[]> kvp = new KeyValuePair<long, string[]>(tpp.Property_ID, text.ToArray());
                        AttrGoups.Remove(exitkvps[0]);
                        AttrGoups.Add(kvp);
                        #endregion
                    }
                    else
                    {
                        //不存在这个情况
                    }


                }
            }

            #endregion


            //编辑时的添加
            if (listView1.Enabled)
            {
                CreateSKUList();
            }
        }

        private void BindToSkulistGrid(List<tb_ProdDetail> propGroups)
        {

            dataGridView1.RowHeadersVisible = false;
            bindingSourceList.DataSource = propGroups;
            dataGridView1.DataSource = bindingSourceList;

            // 配置SKU图片列 - 使用SKUImageColumn显示图片
            ConfigureSKUImageColumn();
        }

        /// <summary>
        /// 配置SKU图片列 - 使用SKUImageColumn显示图片缩略图
        /// </summary>
        private void ConfigureSKUImageColumn()
        {
            try
            {
                // 检查是否存在ImagesPath列
                if (!dataGridView1.Columns.Contains("ImagesPath"))
                    return;

                var imageColumn = dataGridView1.Columns["ImagesPath"];

                // 如果已经是SKUImageColumn，直接配置
                if (imageColumn is SKUImageColumn skuColumn)
                {
                    ConfigureSKUImageColumnProperties(skuColumn);
                }
                else
                {
                    // 替换为SKUImageColumn
                    int columnIndex = imageColumn.Index;
                    var newColumn = new SKUImageColumn();

                    // 复制原列属性
                    newColumn.Name = imageColumn.Name;
                    newColumn.DataPropertyName = imageColumn.DataPropertyName;
                    newColumn.HeaderText = imageColumn.HeaderText;
                    newColumn.Width = imageColumn.Width;
                    newColumn.Visible = imageColumn.Visible;

                    // 移除原列，添加新列
                    dataGridView1.Columns.RemoveAt(columnIndex);
                    dataGridView1.Columns.Insert(columnIndex, newColumn);

                    ConfigureSKUImageColumnProperties(newColumn);
                }

                MainForm.Instance.uclog.AddLog("SKU图片列已配置为SKUImageColumn");
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"配置SKU图片列失败: {ex.Message}", Global.UILogType.警告);
            }
        }

        /// <summary>
        /// 配置SKUImageColumn列属性
        /// </summary>
        private void ConfigureSKUImageColumnProperties(SKUImageColumn skuColumn)
        {
            skuColumn.HeaderText = "SKU图片";
            skuColumn.Width = 80;
            skuColumn.ReadOnly = true;
            skuColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            skuColumn.ToolTipText = "双击查看或编辑SKU图片";

            // 设置显示格式
            skuColumn.DefaultCellStyle.NullValue = null;
        }



        /// <summary>
        /// 将listview的UI值转为属性组
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        private List<KeyValuePair<long, string[]>> GetAttrGoups(TileListView lv)
        {
            List<KeyValuePair<long, string[]>> attrGoups = new List<KeyValuePair<long, string[]>>();
            foreach (TileGroup g in lv.Groups)
            {
                tb_ProdProperty pp = g.BusinessData as tb_ProdProperty;
                long key = pp.Property_ID;
                string values = string.Empty;
                foreach (CheckBox lvitem in g.Items)
                {
                    if (lvitem.Checked)
                    {
                        values += lvitem.Text + ",";
                    }

                }
                values = values.TrimEnd(',');
                if (values.Trim().Length == 0)
                {
                    continue;
                }
                KeyValuePair<long, string[]> kvp = new KeyValuePair<long, string[]>(key, values.Split(','));
                List<KeyValuePair<long, string[]>> exitkvps = attrGoups.Where(t => t.Key == key).ToList();
                if (exitkvps.Count == 0)
                {
                    attrGoups.Add(kvp);
                }
                else
                {
                    KeyValuePair<long, string[]> kvpf = exitkvps.FirstOrDefault();
                    attrGoups.Remove(kvpf);
                    attrGoups.Add(kvp);
                }
            }
            return attrGoups;
        }

        /// <summary>
        /// 生成完整的属性组合，包含属性ID、属性名称、属性值ID和属性值名称
        /// </summary>
        /// <param name="lv">属性选择列表视图</param>
        /// <returns>完整的属性组合列表</returns>
        private List<AttributeCombination> GenerateAttributeCombinations(TileListView lv)
        {
            // 获取所有选中的属性组和属性值
            List<List<AttributeValuePair>> attributeGroups = new List<List<AttributeValuePair>>();

            foreach (TileGroup g in lv.Groups)
            {
                tb_ProdProperty property = g.BusinessData as tb_ProdProperty;
                if (property == null)
                    continue;

                List<AttributeValuePair> attributeValues = new List<AttributeValuePair>();

                foreach (CheckBox lvitem in g.Items)
                {
                    if (lvitem.Checked && lvitem.Tag is tb_ProdPropertyValue propertyValue)
                    {
                        attributeValues.Add(new AttributeValuePair
                        {
                            Property = property,
                            PropertyValue = propertyValue
                        });
                    }
                }

                if (attributeValues.Count > 0)
                {
                    attributeGroups.Add(attributeValues);
                }
            }

            // 如果没有选中的属性组，返回空列表
            if (attributeGroups.Count == 0)
            {
                return new List<AttributeCombination>();
            }

            // 生成所有可能的属性组合
            List<AttributeCombination> combinations = new List<AttributeCombination>();

            // 初始化第一个属性组的组合
            foreach (var value in attributeGroups[0])
            {
                combinations.Add(new AttributeCombination
                {
                    Properties = new List<AttributeValuePair> { value }
                });
            }

            // 处理后续属性组，生成所有组合
            for (int i = 1; i < attributeGroups.Count; i++)
            {
                var tempCombinations = new List<AttributeCombination>();

                foreach (var existingCombination in combinations)
                {
                    foreach (var value in attributeGroups[i])
                    {
                        var newCombination = new AttributeCombination
                        {
                            Properties = new List<AttributeValuePair>(existingCombination.Properties)
                        };

                        newCombination.Properties.Add(value);

                        tempCombinations.Add(newCombination);
                    }
                }

                combinations = tempCombinations;
            }

            return combinations;
        }

        /// <summary>
        /// 从现有产品详情中获取属性组合
        /// </summary>
        /// <returns>现有属性组合列表</returns>
        private List<AttributeCombination> GetExistingAttributeCombinations()
        {
            List<AttributeCombination> existingCombinations = new List<AttributeCombination>();

            foreach (var item in bindingSourceList)
            {
                if (item is tb_ProdDetail detail)
                {
                    var combination = new AttributeCombination
                    {
                        ProductDetail = detail,
                        Properties = new List<AttributeValuePair>()
                    };

                    // 如果有属性关系，从属性关系中获取完整信息
                    if (detail.tb_Prod_Attr_Relations != null && detail.tb_Prod_Attr_Relations.Count > 0)
                    {
                        foreach (var relation in detail.tb_Prod_Attr_Relations)
                        {
                            if (relation.tb_prodproperty != null && relation.tb_prodpropertyvalue != null)
                            {
                                combination.Properties.Add(new AttributeValuePair
                                {
                                    Property = relation.tb_prodproperty,
                                    PropertyValue = relation.tb_prodpropertyvalue
                                });
                            }
                        }
                    }

                    existingCombinations.Add(combination);
                }
            }

            return existingCombinations;
        }


        private async void CreateSKUList()
        {
            // 从依赖注入容器中获取服务实例
            var bizCodeService = Startup.GetFromFac<ClientBizCodeService>();
            var clientBizCodeService = Startup.GetFromFac<ClientBizCodeService>();

            // 生成新的属性组合
            List<AttributeCombination> newCombinations = GenerateAttributeCombinations(listView1);

            // 获取现有属性组合
            List<AttributeCombination> existingCombinations = GetExistingAttributeCombinations();

            // 比较新旧组合，找出需要添加和删除的组合
            var combinationsToAdd = newCombinations.Except(existingCombinations, new AttributeCombinationComparer()).ToList();
            var combinationsToRemove = existingCombinations.Except(newCombinations, new AttributeCombinationComparer()).ToList();
            // 处理需要删除的组合
            foreach (var combination in combinationsToRemove)
            {
                // 查找对应的产品详情
                tb_ProdDetail detailToRemove = combination.ProductDetail;

                if (detailToRemove != null)
                {
                    // 从绑定源中移除
                    bindingSourceList.Remove(detailToRemove);
                    // 将删除的SKU行暂时加入临时列表
                    removeSkuList.Add(detailToRemove);
                }
            }

            // 处理需要添加的组合
            foreach (var combination in combinationsToAdd)
            {
                // 生成GroupName
                string groupName = string.Join(",", combination.Properties.Select(p => p.PropertyValue.PropertyValueName));

                // 创建新的产品详情
                tb_ProdDetail newDetail = new tb_ProdDetail
                {
                    PropertyGroupName = groupName,
                    Created_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID,
                    Created_at = DateTime.Now,
                    ActionStatus = ActionStatus.新增,
                    Is_enabled = true,
                    Is_available = true,
                    tb_Prod_Attr_Relations = new List<tb_Prod_Attr_Relation>()
                };

                // 添加属性关系
                foreach (var attrValuePair in combination.Properties)
                {
                    // 直接使用AttributeValuePair中的属性和属性值对象
                    if (attrValuePair.Property != null && attrValuePair.PropertyValue != null)
                    {
                        tb_Prod_Attr_Relation relation = new tb_Prod_Attr_Relation
                        {
                            Property_ID = attrValuePair.Property.Property_ID,
                            PropertyValueID = attrValuePair.PropertyValue.PropertyValueID,
                            ActionStatus = ActionStatus.新增,
                            tb_prodproperty = attrValuePair.Property,
                            tb_prodpropertyvalue = attrValuePair.PropertyValue
                        };

                        newDetail.tb_Prod_Attr_Relations.Add(relation);
                    }
                }

                // 添加到产品详情列表
                EditEntity.tb_ProdDetails.Add(newDetail);

                // 生成SKU编码
                newDetail.SKU = await bizCodeService.GenerateProductSKUCodeAsync(BaseInfoType.SKU_No, EditEntity, newDetail);

                // 如果需要生成条码
                if (MainForm.Instance.AppContext.SysConfig.UseBarCode)
                {
                    // 使用SKU编号作为条码生成的原始编码，确保条码与产品的唯一性关联
                    newDetail.BarCode = await clientBizCodeService.GenerateBarCodeAsync(newDetail.SKU);
                }

                // 添加到绑定源
                bindingSourceList.Add(newDetail);
            }
        }




        private void btnClear_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            bindingSourceList.Clear();
            AttrGoups.Clear();
            propertyEavList.Clear();
            contextMenuStrip1.Items.Clear();
            if (cmbPropertyType.SelectedValue.ToString() == "-1")
            {
                return;
            }
            #region 单属性

            ProductAttributeType pt = (ProductAttributeType)(int.Parse(cmbPropertyType.SelectedValue.ToString()));// EnumHelper.GetEnumByString<ProductAttributeType>(cmbPropertyType.SelectedItem.ToString());
            switch (pt)
            {
                case ProductAttributeType.单属性:
                    if (cmb属性.SelectedItem == null)
                    {
                        //添加单属性时的SKU
                        #region
                        btnAddProperty.Enabled = false;

                        //UCSKUlist ucskulist = new UCSKUlist();

                        //tableLayoutPanel1.Controls.Remove(ucskulist);
                        #endregion
                    }
                    break;
                case ProductAttributeType.可配置多属性:
                    btnAddProperty.Enabled = true;
                    break;
                case ProductAttributeType.捆绑:
                    break;
                case ProductAttributeType.虚拟:
                    break;
                default:
                    break;
            }

            #endregion


        }






        #region 右键菜单的动态添加   
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (listView1.Groups.Length == 0) //有选中项时才会弹出右键菜单   
            {
                e.Cancel = true;
                return;
            }
        }
        /// <summary>    
        /// 添加右键菜单项   
        /// </summary>    
        /// <param name="text">要显示的文字，如果为 - 则显示为分割线</param>    
        /// <param name="cms">要添加到的子菜单集合</param>    
        /// <param name="callback">点击时触发的事件</param>    
        /// <returns>生成的子菜单，如果为分隔条则返回null</returns>    
        ToolStripMenuItem AddContextMenu(string text, ToolStripItemCollection cms, EventHandler callback)
        {
            if (text == "-")
            {
                ToolStripSeparator tsp = new ToolStripSeparator();
                cms.Add(tsp);

                return null;
            }
            else if (!string.IsNullOrEmpty(text))
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem(text);// Image.FromFile("图标路径"));
                tsmi.Name = text;
                if (callback != null)
                {
                    tsmi.Click += callback;
                }
                cms.Add(tsmi);

                tsmi.Font = new Font("Arial", 9, FontStyle.Regular); //字体设置   
                                                                     //tsmi.Image = Image.FromFile("图标路径"); //菜单图标设置   

                return tsmi;
            }
            return null;
        }
        /// <summary>   
        /// 菜单事件   
        /// </summary>   
        void menuClicked(object sender, EventArgs e)
        {
            ToolStripMenuItem stripItem = (ToolStripMenuItem)sender;
            tb_ProdProperty tpp = stripItem.Tag as tb_ProdProperty;
            bool isAll = false;
            if (stripItem.Text.Contains("全选"))
            {
                isAll = true;
            }
            foreach (TileGroup g in listView1.Groups)
            {
                tb_ProdProperty pp = g.BusinessData as tb_ProdProperty;
                if (pp.Property_ID == tpp.Property_ID)
                {
                    foreach (CheckBox lvitem in g.Items)
                    {

                        if (lvitem.Checked != isAll)
                        {
                            lvitem.Checked = isAll;
                        }

                    }
                    break;
                }
            }

        }



        #endregion



        #region SKU列表的显示控制

        private string db_EntityName = string.Empty;
        /// <summary>
        /// 对应的实体类-实际来自数据库 用来关联菜单相关操作
        /// </summary>
        public string Db_EntityName { get => db_EntityName; set => db_EntityName = value; }

        /// <summary>
        /// 用来保存外键表名与外键主键列名  通过这个打到对应的名称。
        /// </summary>
        public static ConcurrentDictionary<string, string> FKValueColNameTableNameList = new ConcurrentDictionary<string, string>();

        public void SetBaseValue<T>()
        {
            string tableName = typeof(T).Name;
            db_EntityName = tableName;
            foreach (var field in typeof(T).GetProperties())
            {
                //获取指定类型的自定义特性
                object[] attrs = field.GetCustomAttributes(false);
                foreach (var attr in attrs)
                {
                    if (attr is FKRelationAttribute)
                    {
                        FKRelationAttribute fkrattr = attr as FKRelationAttribute;
                        FKValueColNameTableNameList.TryAdd(fkrattr.FK_IDColName, fkrattr.FKTableName);
                    }
                }
            }

            //重构？
            dataGridView1.XmlFileName = tableName;
            this.dataGridView1.FieldNameList = UIHelper.GetFieldNameColList(typeof(T));

            //同样为dataGridView1添加PropertyGroupName属性
            if (typeof(T) == typeof(tb_ProdDetail))
            {

                //因为是第一次加载时。实体还没有数据。先默认添加。再根据数据决定是否显示。数据是用户手动选择的
                // 多属性组组合
                KeyValuePair<string, bool> fieldInfo = new KeyValuePair<string, bool>("多属性组合", true);

                // 确保PropertyGroupName列位于所有列的最前面
                // 创建一个新的字典，先添加PropertyGroupName列，然后再添加其他列
                var newFieldNameList = new ConcurrentDictionary<string, KeyValuePair<string, bool>>();
                newFieldNameList.TryAdd("PropertyGroupName", fieldInfo);

                // 添加其他列
                foreach (var item in this.dataGridView1.FieldNameList)
                {
                    if (item.Key != "PropertyGroupName")
                    {
                        newFieldNameList.TryAdd(item.Key, item.Value);
                    }
                }

                // 替换原有的FieldNameList
                this.dataGridView1.FieldNameList = newFieldNameList;
            }


            HashSet<string> InvisibleCols = new HashSet<string>();

            List<Expression<Func<tb_ProdDetail, object>>> ExpInvisibleCols = new List<Expression<Func<tb_ProdDetail, object>>>();
            ExpInvisibleCols.Add(c => c.ProdBaseID);
            ExpInvisibleCols.Add(c => c.ProdDetailID);
            ExpInvisibleCols.Add(c => c.DataStatus);
            //如果不启用条码。则不显示
            if (!MainForm.Instance.AppContext.SysConfig.UseBarCode)
            {
                ExpInvisibleCols.Add(c => c.BarCode);
            }

            InvisibleCols = RuinorExpressionHelper.ExpressionListToHashSet(ExpInvisibleCols);
            foreach (var item in InvisibleCols)
            {
                KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                dataGridView1.FieldNameList.TryRemove(item, out kv);
            }
            dataGridView1.BizInvisibleCols = InvisibleCols;
        }


        //三级 还是两级呢。  反向来 一是 KEY VALUE  然后是列名
        ConcurrentDictionary<string, List<KeyValuePair<object, string>>> _DataDictionary = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();

        /// <summary>
        /// 固定的值显示，入库ture 出库false
        /// 每个列表对应的值 ，单独设置
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ColNameDataDictionary { get => _DataDictionary; set => _DataDictionary = value; }


        private void BindingSourceList_ListChangedSku(object sender, ListChangedEventArgs e)
        {
            tb_ProdDetail entity = new tb_ProdDetail();
            switch (e.ListChangedType)
            {
                case ListChangedType.Reset:
                    break;
                case ListChangedType.ItemAdded:
                    entity = bindingSourceList.List[e.NewIndex] as tb_ProdDetail;
                    if (entity.ActionStatus != ActionStatus.加载)
                    {
                        entity.ActionStatus = ActionStatus.新增;
                    }
                    break;
                case ListChangedType.ItemDeleted:
                    if (e.NewIndex < bindingSourceList.Count)
                    {
                        entity = bindingSourceList.List[e.NewIndex] as tb_ProdDetail;
                        entity.ActionStatus = ActionStatus.删除;
                    }
                    break;
                case ListChangedType.ItemMoved:
                    break;
                case ListChangedType.ItemChanged:
                    entity = bindingSourceList.List[e.NewIndex] as tb_ProdDetail;
                    if (entity.ActionStatus == ActionStatus.无操作)
                    {
                        entity.ActionStatus = ActionStatus.修改;
                    }
                    if (entity.ActionStatus == ActionStatus.新增)
                    {
                        entity.ActionStatus = ActionStatus.修改;
                    }
                    if (entity.ActionStatus == ActionStatus.加载)
                    {
                        entity.ActionStatus = ActionStatus.修改;
                    }
                    break;
                case ListChangedType.PropertyDescriptorAdded:
                    break;
                case ListChangedType.PropertyDescriptorDeleted:
                    break;
                case ListChangedType.PropertyDescriptorChanged:
                    break;
                default:
                    break;
            }
        }



        #region 画行号

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // 处理ImagesPath列的图片预览显示11
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
                if (columnName == "ImagesPath")
                {
                    var detail = dataGridView1.Rows[e.RowIndex].DataBoundItem as tb_ProdDetail;
                    if (detail != null)
                    {
                        // 检查是否有缓存的图片数据需要显示（包括新添加但未上传的图片）
                        bool hasCachedData = skuImageDataCache.ContainsKey(detail) && skuImageDataCache[detail].Count > 0;

                        // 调试日志（仅在第一次绘制时记录）
                        if (e.RowIndex == dataGridView1.CurrentCell?.RowIndex)
                        {
                            MainForm.Instance.uclog.AddLog($"CellPainting: SKU={detail.SKU}, ID={detail.ProdDetailID}, HasCachedData={hasCachedData}, HasUnsavedChanges={detail.HasUnsavedImageChanges}, ImagesPath={detail.ImagesPath}");
                        }

                        if (hasCachedData)
                        {
                            // 绘制背景（不绘制前景，我们自己绘制）
                            e.PaintBackground(e.ClipBounds, false);
                            e.Handled = true;

                            // 显示第一张图片的缩略图
                            var imageDataList = skuImageDataCache[detail];
                            try
                            {
                                using (var ms = new MemoryStream(imageDataList[0].Item1))
                                using (var originalImage = Image.FromStream(ms))
                                {
                                    // 计算缩略图尺寸（保持宽高比）
                                    int maxWidth = e.CellBounds.Width - 4;
                                    int maxHeight = e.CellBounds.Height - 4;
                                    float ratio = Math.Min((float)maxWidth / originalImage.Width, (float)maxHeight / originalImage.Height);
                                    int thumbWidth = (int)(originalImage.Width * ratio);
                                    int thumbHeight = (int)(originalImage.Height * ratio);

                                    // 居中绘制缩略图
                                    int x = e.CellBounds.X + (e.CellBounds.Width - thumbWidth) / 2;
                                    int y = e.CellBounds.Y + (e.CellBounds.Height - thumbHeight) / 2;
                                    var thumbRect = new Rectangle(x, y, thumbWidth, thumbHeight);

                                    // 直接绘制图片到单元格，不创建中间的缩略图Bitmap
                                    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                                    e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                                    e.Graphics.DrawImage(originalImage, thumbRect);

                                    // 如果有多张图片，显示数量标签
                                    if (imageDataList.Count > 1)
                                    {
                                        string countText = $"+{imageDataList.Count - 1}";
                                        using (var countBrush = new SolidBrush(Color.FromArgb(200, 0, 120, 215)))
                                        using (var textBrush = new SolidBrush(Color.White))
                                        using (var countFont = new Font(e.CellStyle.Font.FontFamily, 8, FontStyle.Bold))
                                        {
                                            var textSize = e.Graphics.MeasureString(countText, countFont);
                                            var countRect = new Rectangle(
                                                e.CellBounds.Right - (int)textSize.Width - 8,
                                                e.CellBounds.Bottom - (int)textSize.Height - 4,
                                                (int)textSize.Width + 6,
                                                (int)textSize.Height + 2);
                                            e.Graphics.FillRectangle(countBrush, countRect);
                                            e.Graphics.DrawString(countText, countFont, textBrush, countRect,
                                                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                                        }
                                    }

                                    // 如果图片有未保存的更改，显示标记
                                    if (detail.HasUnsavedImageChanges)
                                    {
                                        string unsavedText = "*";
                                        using (var unsavedBrush = new SolidBrush(Color.Orange))
                                        using (var unsavedFont = new Font(e.CellStyle.Font.FontFamily, 10, FontStyle.Bold))
                                        {
                                            var textSize = e.Graphics.MeasureString(unsavedText, unsavedFont);
                                            e.Graphics.DrawString(unsavedText, unsavedFont, unsavedBrush,
                                                new PointF(e.CellBounds.Right - textSize.Width - 4, e.CellBounds.Top + 2));
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                // 图片加载失败，显示文本
                                using (var brush = new SolidBrush(Color.Red))
                                {
                                    e.Graphics.DrawString("图片加载失败", e.CellStyle.Font, brush,
                                        e.CellBounds, StringFormat.GenericDefault);
                                }
                            }
                            return;
                        }
                        // 如果有未保存的更改但没有缓存数据（可能是删除了所有图片），显示特殊提示
                        else if (detail.HasUnsavedImageChanges)
                        {
                            using (var brush = new SolidBrush(Color.Orange))
                            {
                                e.Graphics.DrawString("* 图片已更改（待保存）", e.CellStyle.Font, brush,
                                    e.CellBounds, StringFormat.GenericDefault);
                            }
                        }
                        // 显示已有图片数量（无法加载已上传图片的缩略图，因为没有缓存数据）
                        else if (!string.IsNullOrEmpty(detail.ImagesPath))
                        {
                            using (var brush = new SolidBrush(e.CellStyle.ForeColor))
                            {
                                e.Graphics.DrawString(detail.ImagesPath, e.CellStyle.Font, brush,
                                    e.CellBounds, StringFormat.GenericDefault);
                            }
                        }
                        else
                        {
                            using (var brush = new SolidBrush(Color.Gray))
                            {
                                e.Graphics.DrawString("双击添加图片", e.CellStyle.Font, brush,
                                    e.CellBounds, StringFormat.GenericDefault);
                            }
                        }
                    }
                    else
                    {
                        using (var brush = new SolidBrush(Color.Gray))
                        {
                            e.Graphics.DrawString("双击添加图片", e.CellStyle.Font, brush,
                                e.CellBounds, StringFormat.GenericDefault);
                        }
                    }

                    e.Handled = true;
                    return;
                }

                // 其他列的正常绘制
                DataGridViewPaintParts paintParts =
                    e.PaintParts & ~DataGridViewPaintParts.Focus;

                e.Paint(e.ClipBounds, paintParts);
                e.Handled = true;
            }

            if (e.ColumnIndex < 0 && e.RowIndex >= 0)
            {
                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                Rectangle indexRect = e.CellBounds;
                indexRect.Inflate(-2, -2);

                TextRenderer.DrawText(e.Graphics,
                    (e.RowIndex + 1).ToString(),
                    e.CellStyle.Font,
                    indexRect,
                    e.CellStyle.ForeColor,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                e.Handled = true;
            }
        }

        #endregion



        public System.Windows.Forms.BindingSource _ListDataSoure = null;

        [Description("列表中的要显示的数据来源[BindingSource]"), Category("自定属性"), Browsable(true)]
        /// <summary>
        /// 列表的数据源(实际要显示的)
        /// </summary>
        public System.Windows.Forms.BindingSource ListDataSoure
        {
            get { return _ListDataSoure; }
            set { _ListDataSoure = value; }
        }
        private bool editflag;


        /// <summary>
        /// 初始化列表数据
        /// </summary>
        internal void InitListData()
        {
            this.dataGridView1.DataSource = null;

            ListDataSoure = bindingSourceList;

            this.dataGridView1.DataSource = ListDataSoure.DataSource;
        }


        private async void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // 检查是否点击了有效的行和列
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            // 检查是否点击了ImagesPath列
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (columnName == "ImagesPath")
            {
                // 获取当前行的tb_ProdDetail对象
                var detail = dataGridView1.Rows[e.RowIndex].DataBoundItem as tb_ProdDetail;
                if (detail == null)
                {
                    return;
                }

                try
                {
                    // 创建并显示SKU图片编辑对话框
                    using (var imageEditForm = new frmSKUImageEdit(detail))
                    {
                        // 异步加载图片并等待完成
                        await imageEditForm.DownloadImagesAsync();

                        // 显示对话框
                        var dialogResult = imageEditForm.ShowDialog(this);

                        if (dialogResult == DialogResult.OK)
                        {
                            // 在窗体关闭前提取图片数据，避免窗体释放后无法访问
                            // 使用对象引用作为key，支持未保存的SKU（ProdDetailID为0）
                            // 获取需要上传的图片数据
                            var updatedImages = imageEditForm.GetUpdatedImages();
                            if (updatedImages != null && updatedImages.Count > 0)
                            {
                                skuImageDataCache[detail] = updatedImages;
                                MainForm.Instance.uclog.AddLog($"已缓存 {updatedImages.Count} 张SKU图片到内存，SKU: {detail.SKU}, ID: {detail.ProdDetailID}");
                            }
                            else
                            {
                                skuImageDataCache.Remove(detail);
                            }

                            // 获取需要删除的图片信息
                            var deletedImages = imageEditForm.GetDeletedImages();
                            if (deletedImages != null && deletedImages.Count > 0)
                            {
                                skuImageDeletedCache[detail] = deletedImages;
                            }
                            else
                            {
                                skuImageDeletedCache.Remove(detail);
                            }

                            // 刷新当前单元格显示
                            dataGridView1.InvalidateCell(e.ColumnIndex, e.RowIndex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MainForm.Instance.uclog.AddLog($"打开SKU图片编辑器失败: {ex.Message}", Global.UILogType.错误);
                    MessageBox.Show($"打开SKU图片编辑器失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }





        #endregion

        private void txtcategory_ID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void kryptonNavigator1_SelectedPageChanged(object sender, EventArgs e)
        {
            if (kryptonNavigator1.SelectedPage.Name == "kp箱规信息")
            {

            }

        }

        private void kryptonTreeView1_DoubleClick(object sender, EventArgs e)
        {
            if (kryptonTreeView1.SelectedNode != null)
            {
                if (sender is KryptonTreeView treeView)
                {
                    if (treeView.SelectedNode.Tag is tb_ProdCategories prodCategorie)
                    {
                        EditEntity.Category_ID = prodCategorie.Category_ID;
                        txtcategory_ID.Text = prodCategorie.Category_name;
                        TreeNode node = txtcategory_ID.TreeView.Nodes.Find(prodCategorie.Category_ID.ToString(), true).FirstOrDefault();
                        if (node != null)
                        {
                            txtcategory_ID.TreeView.SelectedNode = node;
                            txtcategory_ID.TreeView.SelectedNode.Tag = prodCategorie;
                        }

                    }
                }
            }

        }

        private void chkexpandAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkexpandAll.Checked)
            {
                ExpandOrCollapseNodes(true);
            }
            else
            {
                ExpandOrCollapseNodes(false);
            }
        }
        public void ExpandOrCollapseNodes(bool expand)
        {
            // 展开或收拢根节点（0级）
            foreach (TreeNode rootNode in kryptonTreeView1.Nodes)
            {
                // 如果需要展开，则展开根节点和所有子节点
                if (expand)
                {
                    rootNode.Expand();

                }
                // 如果需要收拢，则只收拢根节点，保留子节点的当前状态
                else
                {
                    rootNode.Collapse();
                }
            }

            // 展开或收拢二级节点（1级）
            foreach (TreeNode rootNode in kryptonTreeView1.Nodes)
            {
                ExpandCollapseSecondLevel(rootNode, expand);
            }
            // 选中第一个节点
            if (expand && kryptonTreeView1.Nodes.Count > 0 && kryptonTreeView1.Nodes[0].Nodes.Count > 0)
            {
                kryptonTreeView1.SelectedNode = kryptonTreeView1.Nodes[0].Nodes[0];
                kryptonTreeView1.SelectedNode.EnsureVisible(); // 确保节点可见
            }
        }

        private void ExpandCollapseSecondLevel(TreeNode parentNode, bool expand)
        {
            foreach (TreeNode childNode in parentNode.Nodes)
            {
                // 展开或收拢二级节点
                if (expand)
                {
                    childNode.Expand();
                }
                else
                {
                    childNode.Collapse();
                }

                // 如果需要收拢二级节点，同时收拢三级节点（如果有）
                if (!expand && childNode.Nodes.Count > 0)
                {
                    foreach (TreeNode grandChildNode in childNode.Nodes)
                    {
                        grandChildNode.Collapse();
                    }
                }
            }
        }

        #region 图片管理功能

        /// <summary>
        /// 检查是否有图片需要上传或删除
        /// </summary>
        /// <returns>如果有图片需要上传或删除返回true，否则返回false</returns>
        private bool HasImagesToUpload()
        {
            // 检查产品图片控件是否有需要上传的图片
            if (magicPictureBox产品基本图片 != null)
            {
                var updatedImages = magicPictureBox产品基本图片.GetImagesNeedingUpdate();
                var deletedImages = magicPictureBox产品基本图片.GetDeletedImages();

                // 检查是否有需要上传的图片或需要删除的图片
                return (updatedImages != null && updatedImages.Count > 0) ||
                       (deletedImages != null && deletedImages.Count > 0);
            }
            return false;
        }

        /// <summary>
        /// 上传或删除图片（如果需要）
        /// 实现产品基本图片的上传和删除逻辑
        /// </summary>
        /// <returns>操作是否成功</returns>
        private async Task<bool> UploadImagesIfNeeded()
        {
            try
            {
                // 检查实体是否已保存（必须有主键ID）
                if (EditEntity == null || EditEntity.ProdBaseID <= 0)
                {
                    MainForm.Instance.uclog.AddLog("产品尚未保存，无法操作图片");
                    return false;
                }

                if (magicPictureBox产品基本图片 != null)
                {
                    var updatedImages = magicPictureBox产品基本图片.GetImagesNeedingUpdate();
                    var deletedImages = magicPictureBox产品基本图片.GetDeletedImages();

                    // 第一步：处理需要删除的图片
                    if (deletedImages != null && deletedImages.Count > 0)
                    {
                        MainForm.Instance.uclog.AddLog($"检测到 {deletedImages.Count} 张图片需要删除");

                        // 删除图片
                        foreach (var deletedImage in deletedImages)
                        {
                            // 只有有FileId的图片才是已上传到服务器的，需要删除
                            if (deletedImage != null && deletedImage.FileId > 0)
                            {
                                try
                                {
                                    var fileService = Startup.GetFromFac<FileManagementService>();
                                    var deleteRequest = new FileDeleteRequest();
                                    deleteRequest.BusinessNo = EditEntity.ProductNo ?? EditEntity.ProdBaseID.ToString();
                                    deleteRequest.BusinessId = EditEntity.ProdBaseID;
                                    deleteRequest.BusinessType = (int)BizType.产品档案;
                                    deleteRequest.PhysicalDelete = false;

                                    var ctrpay = Startup.GetFromFac<FileBusinessService>();
                                    var fileStorageInfo = ctrpay.ConvertToFileStorageInfo(deletedImage);
                                    if (fileStorageInfo != null)
                                    {
                                        deleteRequest.AddDeleteFileStorageInfo(fileStorageInfo);
                                    }

                                    var deleteResponse = await fileService.DeleteFileAsync(deleteRequest);
                                    if (deleteResponse.IsSuccess)
                                    {
                                        MainForm.Instance.uclog.AddLog($"图片删除成功：{deletedImage.OriginalFileName}", UILogType.普通消息);
                                    }
                                    else
                                    {
                                        MainForm.Instance.uclog.AddLog($"图片删除失败：{deletedImage.OriginalFileName}，原因：{deleteResponse.ErrorMessage}", UILogType.错误);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MainForm.Instance.uclog.AddLog($"删除图片出错：{deletedImage.OriginalFileName}，{ex.Message}", UILogType.错误);
                                }
                            }
                        }

                        // 删除成功后清空删除列表
                        magicPictureBox产品基本图片.ClearDeletedImagesList();
                    }

                    // 第二步：处理需要上传的图片
                    if (updatedImages != null && updatedImages.Count > 0)
                    {
                        MainForm.Instance.uclog.AddLog($"检测到 {updatedImages.Count} 张图片需要上传");

                        var ctrpay = Startup.GetFromFac<FileBusinessService>();
                        int successCount = 0;

                        foreach (var imageDataWithInfo in updatedImages)
                        {
                            byte[] imageData = imageDataWithInfo.Item1;
                            var imageInfo = imageDataWithInfo.Item2;

                            if (imageData == null || imageData.Length == 0)
                            {
                                continue;
                            }

                            // 准备参数
                            long? existingFileId = imageInfo.FileId > 0 ? imageInfo.FileId : null;

                            // 上传图片
                            var response = await ctrpay.UploadImageAsync(EditEntity, imageInfo.OriginalFileName, imageData, "ImagesPath", existingFileId);

                            if (response != null && response.IsSuccess)
                            {
                                successCount++;
                                MainForm.Instance.uclog.AddLog($"产品图片上传成功：{imageInfo.OriginalFileName}");
                                imageInfo.IsUpdated = false;
                                imageInfo.IsDeleted = false;
                            }
                            else
                            {
                                MainForm.Instance.uclog.AddLog($"产品图片上传失败：{imageInfo.OriginalFileName}，原因：{response?.Message ?? "未知错误"}", UILogType.错误);
                            }
                        }

                        if (successCount > 0)
                        {
                            MainForm.Instance.uclog.AddLog($"成功更新 {successCount} 张产品图片");
                        }
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"上传图片时发生异常：{ex.Message}", Global.UILogType.错误);
                return false;
            }
        }

        /// <summary>
        /// 异步加载所有SKU明细的图片
        /// 在编辑产品时调用，加载每个SKU已保存的图片
        /// </summary>
        /// <param name="entity">产品实体</param>
        private async Task LoadSKUImagesAsync(tb_Prod entity)
        {
            try
            {
                if (entity?.tb_ProdDetails == null || entity.tb_ProdDetails.Count == 0)
                {
                    return;
                }

                MainForm.Instance.uclog.AddLog($"开始加载 {entity.tb_ProdDetails.Count} 个SKU的图片...");
                int loadedCount = 0;

                foreach (var detail in entity.tb_ProdDetails)
                {
                    try
                    {
                        // 只加载有图片路径的SKU

                        await LoadSKUImageAsync(detail);
                        loadedCount++;

                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.uclog.AddLog($"加载SKU {detail.SKU} 图片失败: {ex.Message}", Global.UILogType.警告);
                    }
                }

                if (loadedCount > 0)
                {
                    MainForm.Instance.uclog.AddLog($"成功加载 {loadedCount} 个SKU的图片");
                    // 刷新网格显示
                    dataGridView1.Invalidate();
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"加载SKU图片时发生错误: {ex.Message}", Global.UILogType.错误);
            }
        }

        /// <summary>
        /// 加载单个SKU的图片
        /// </summary>
        /// <param name="detail">SKU明细</param>
        private async Task LoadSKUImageAsync(tb_ProdDetail detail)
        {
            try
            {
                var ctrpay = Startup.GetFromFac<FileBusinessService>();

                // 下载该SKU关联的图片
                var list = await ctrpay.DownloadImageAsync<tb_ProdDetail>(detail, c => c.ImagesPath);

                if (list == null || list.Count == 0)
                {
                    return;
                }

                // 将下载的图片转换为缓存格式
                var imageDataList = new List<Tuple<byte[], RUINOR.WinFormsUI.CustomPictureBox.ImageInfo>>();

                foreach (var downloadResponse in list)
                {
                    if (downloadResponse.IsSuccess && downloadResponse.FileStorageInfos != null)
                    {
                        foreach (var fileStorageInfo in downloadResponse.FileStorageInfos)
                        {
                            if (fileStorageInfo.FileData != null && fileStorageInfo.FileData.Length > 0)
                            {
                                var imageInfo = ctrpay.ConvertToImageInfo(fileStorageInfo);
                                imageDataList.Add(new Tuple<byte[], RUINOR.WinFormsUI.CustomPictureBox.ImageInfo>(
                                    fileStorageInfo.FileData, imageInfo));
                            }
                        }
                    }
                }

                // 缓存图片数据
                if (imageDataList.Count > 0)
                {
                    skuImageDataCache[detail] = imageDataList;
                    if (detail.ProdDetailID > 0)
                    {
                        skuImageDataCacheById[detail.ProdDetailID] = imageDataList;
                    }
                    MainForm.Instance.uclog.AddLog($"SKU {detail.SKU} 已缓存 {imageDataList.Count} 张图片");
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"加载SKU {detail.SKU} 图片失败: {ex.Message}", Global.UILogType.警告);
            }
        }

        /// <summary>
        /// 下载并显示产品基本图片到MagicPictureBox
        /// </summary>
        /// <param name="entity">产品实体</param>
        /// <param name="magicPicBox">MagicPictureBox控件</param>
        private async Task DownloadProductImagesAsync(tb_Prod entity, RUINOR.WinFormsUI.CustomPictureBox.MagicPictureBox magicPicBox)
        {
            magicPicBox.MultiImageSupport = true;
            var ctrpay = Startup.GetFromFac<FileBusinessService>();
            try
            {
                // 下载ImagesPath字段关联的图片
                var list = await ctrpay.DownloadImageAsync(entity, "ImagesPath");

                if (list == null || list.Count == 0)
                {
                    return;
                }

                // 简化处理逻辑，直接处理文件存储信息
                List<byte[]> imageDataList = new List<byte[]>();
                List<RUINOR.WinFormsUI.CustomPictureBox.ImageInfo> imageInfos = new List<RUINOR.WinFormsUI.CustomPictureBox.ImageInfo>();

                foreach (var downloadResponse in list)
                {
                    if (downloadResponse.IsSuccess && downloadResponse.FileStorageInfos != null)
                    {
                        foreach (var fileStorageInfo in downloadResponse.FileStorageInfos)
                        {
                            if (fileStorageInfo.FileData != null && fileStorageInfo.FileData.Length > 0)
                            {
                                imageDataList.Add(fileStorageInfo.FileData);
                                imageInfos.Add(ctrpay.ConvertToImageInfo(fileStorageInfo));
                            }
                        }
                    }
                }

                if (imageDataList.Count > 0)
                {
                    try
                    {
                        // 使用统一的LoadImages方法，自动处理单张和多张图片
                        magicPicBox.LoadImages(imageDataList, imageInfos, true);
                        MainForm.Instance.uclog.AddLog($"成功加载 {imageDataList.Count} 张产品基本图片");
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.uclog.AddLog($"加载图片失败: {ex.Message}", Global.UILogType.错误);
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"下载产品图片出错：{ex.Message}", Global.UILogType.错误);
            }
        }

        /// <summary>
        /// 检查是否有SKU图片需要上传或删除
        /// </summary>
        /// <returns>如果有SKU图片需要上传或删除返回true，否则返回false</returns>
        private bool HasSKUImagesToUpload()
        {
            if (EditEntity == null || EditEntity.tb_ProdDetails == null)
            {
                return false;
            }

            // 检查每个SKU明细是否有未保存的图片更改
            foreach (var detail in EditEntity.tb_ProdDetails)
            {
                // 检查是否有缓存的图片数据需要上传
                if (detail.HasUnsavedImageChanges)
                {
                    return true;
                }

                // 检查是否有缓存的图片数据
                if (skuImageDataCache.ContainsKey(detail) && skuImageDataCache[detail] != null && skuImageDataCache[detail].Count > 0)
                {
                    return true;
                }

                if (skuImageDeletedCache.ContainsKey(detail) && skuImageDeletedCache[detail] != null && skuImageDeletedCache[detail].Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取指定SKU需要更新的图片列表
        /// </summary>
        /// <param name="detail">SKU明细对象</param>
        /// <returns>需要更新的图片列表</returns>
        private List<Tuple<byte[], RUINOR.WinFormsUI.CustomPictureBox.ImageInfo>> GetSKUImagesNeedingUpdate(tb_ProdDetail detail)
        {
            // 检查缓存中是否有待上传的图片
            if (skuImageDataCache.ContainsKey(detail) && skuImageDataCache[detail] != null)
            {
                return skuImageDataCache[detail];
            }

            // 如果没有缓存数据，返回null
            return null;
        }

        /// <summary>
        /// 获取指定SKU需要删除的图片列表
        /// </summary>
        /// <param name="detail">SKU明细对象</param>
        /// <returns>需要删除的图片列表</returns>
        private List<RUINOR.WinFormsUI.CustomPictureBox.ImageInfo> GetSKUImagesToDelete(tb_ProdDetail detail)
        {
            // 检查缓存中是否有待删除的图片
            if (skuImageDeletedCache.ContainsKey(detail) && skuImageDeletedCache[detail] != null)
            {
                return skuImageDeletedCache[detail];
            }

            // 如果没有缓存数据，返回null
            return null;
        }

        /// <summary>
        /// 上传或删除图片（如果需要）- 通用方法
        /// 支持同时处理新增/更新和删除的图片
        /// </summary>
        /// <typeparam name="Target">目标实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="updatedImages">需要更新的图片列表</param>
        /// <param name="deletedImages">需要删除的图片列表</param>
        /// <param name="TargetField">关联字段表达式</param>
        /// <returns>操作是否成功</returns>
        public async Task<bool> UploadUpdatedImagesAsync<Target>(
            Target entity,
            List<Tuple<byte[], RUINOR.WinFormsUI.CustomPictureBox.ImageInfo>> updatedImages,
            List<RUINOR.WinFormsUI.CustomPictureBox.ImageInfo> deletedImages,
            Expression<Func<Target, object>> TargetField)
        {
            var ctrpay = Startup.GetFromFac<FileBusinessService>();
            try
            {
                bool allSuccess = true;
                MemberInfo memberInfo = TargetField.GetMemberInfo();
                string columnName = memberInfo.Name;

                // 获取实体信息 - 对于产品明细，使用具体字段
                string billNo = "";
                long billId = 0;

                // 通过反射获取ID字段值
                if (entity is tb_ProdDetail prodDetailEntity)
                {
                    billNo = prodDetailEntity.SKU ?? prodDetailEntity.ProdDetailID.ToString();
                    billId = prodDetailEntity.ProdDetailID;
                }
                else
                {
                    // 如果不是预期类型，尝试通过反射获取
                    billId = Convert.ToInt64(entity.GetPropertyValue("ProdDetailID"));
                    billNo = entity.GetPropertyValue("SKU")?.ToString() ?? billId.ToString();
                }

                // ========== 第一步：处理已删除的图片 ==========
                if (deletedImages != null && deletedImages.Count > 0)
                {
                    MainForm.Instance.uclog.AddLog($"开始处理 {deletedImages.Count} 张待删除的图片");

                    var fileService = Startup.GetFromFac<FileManagementService>();
                    var deleteRequest = new FileDeleteRequest();
                    deleteRequest.BusinessNo = billNo;
                    deleteRequest.BusinessId = billId;
                    deleteRequest.BusinessType = (int)BizType.产品档案; // 产品明细使用产品档案业务类型
                    deleteRequest.PhysicalDelete = false; // 逻辑删除

                    // 添加要删除的文件信息
                    foreach (var deletedImage in deletedImages)
                    {
                        if (deletedImage != null && deletedImage.FileId > 0)
                        {
                            var fileStorageInfo = ctrpay.ConvertToFileStorageInfo(deletedImage);
                            if (fileStorageInfo != null)
                            {
                                deleteRequest.AddDeleteFileStorageInfo(fileStorageInfo);
                            }
                        }
                    }

                    // 调用文件管理服务删除文件
                    var deleteResponse = await fileService.DeleteFileAsync(deleteRequest);

                    if (deleteResponse.IsSuccess)
                    {
                        MainForm.Instance.uclog.AddLog($"图片删除成功：共{deletedImages.Count}张", Global.UILogType.普通消息);
                    }
                    else
                    {
                        allSuccess = false;
                        MainForm.Instance.uclog.AddLog($"图片删除失败：{deleteResponse.ErrorMessage}", Global.UILogType.错误);
                    }
                }

                // ========== 第二步：处理新上传或更新的图片 ==========
                if (updatedImages != null && updatedImages.Count > 0)
                {
                    MainForm.Instance.uclog.AddLog($"开始处理 {updatedImages.Count} 张需要更新的图片");

                    int successCount = 0;

                    // 遍历上传所有需要更新的图片
                    foreach (var imageDataWithInfo in updatedImages)
                    {
                        byte[] imageData = imageDataWithInfo.Item1;
                        var imageInfo = imageDataWithInfo.Item2;

                        if (imageData == null || imageData.Length == 0)
                        {
                            MainForm.Instance.uclog.AddLog($"跳过空图片数据: {imageInfo.OriginalFileName}");
                            continue;
                        }

                        // 检查文件大小限制
                        if (imageData.Length > 10 * 1024 * 1024) // 10MB限制
                        {
                            MainForm.Instance.uclog.AddLog($"图片 {imageInfo.OriginalFileName} 超过大小限制(10MB)");
                            allSuccess = false;
                            continue;
                        }

                        // 准备参数
                        // 如果图片有FileId，说明这是替换操作，服务器会更新现有文件
                        long? existingFileId = imageInfo.FileId > 0 ? imageInfo.FileId : null;

                        // 上传图片
                        var response = await ctrpay.UploadImageAsync(entity as BaseEntity, imageInfo.OriginalFileName, imageData, columnName, existingFileId);

                        // 检查响应是否为空
                        if (response == null)
                        {
                            allSuccess = false;
                            MainForm.Instance.uclog.AddLog($"图片上传返回空响应：{imageInfo.OriginalFileName}");
                            continue;
                        }

                        if (response.IsSuccess)
                        {
                            successCount++;
                            MainForm.Instance.uclog.AddLog($"图片上传成功：{imageInfo.OriginalFileName}");
                            // 上传成功后，将图片标记为未更新
                            imageInfo.IsUpdated = false;
                            imageInfo.IsDeleted = false; // 重置删除标记
                        }
                        else
                        {
                            allSuccess = false;
                            MainForm.Instance.uclog.AddLog($"图片上传失败：{imageInfo.OriginalFileName}，原因：{response.Message}");
                        }
                    }

                    if (successCount > 0)
                    {
                        MainForm.Instance.uclog.AddLog($"成功上传 {successCount} 张图片");
                    }
                }

                return allSuccess;
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"处理图片时发生异常：{ex.Message}", Global.UILogType.错误);
                return false;
            }
        }

        /// <summary>
        /// 上传或删除SKU图片（如果需要）
        /// 实现SKU明细图片的上传和删除逻辑
        /// </summary>
        /// <returns>操作是否成功</returns>
        private async Task<bool> UploadSKUImagesIfNeeded()
        {
            try
            {
                if (EditEntity == null || EditEntity.tb_ProdDetails == null)
                {
                    return false;
                }

                int totalSuccessCount = 0;
                int totalFailCount = 0;

                // 遍历所有SKU明细，检查是否有需要上传或删除的图片
                foreach (var detail in EditEntity.tb_ProdDetails)
                {
                    try
                    {
                        // 从SKU图片编辑对话框获取需要上传和删除的图片
                        // 使用frmSKUImageEdit类中的方法来获取图片变更
                        var updatedImages = GetSKUImagesNeedingUpdate(detail);
                        var deletedImages = GetSKUImagesToDelete(detail);

                        // 检查是否有需要处理的图片（上传或删除）
                        if ((updatedImages != null && updatedImages.Count > 0) ||
                            (deletedImages != null && deletedImages.Count > 0))
                        {
                            MainForm.Instance.uclog.AddLog($"处理SKU {detail.SKU ?? "未命名"} 的图片变更");

                            // 使用通用的图片上传方法，支持同时处理上传和删除
                            var uploadSuccess = await UploadUpdatedImagesAsync<tb_ProdDetail>(
                                detail,
                                updatedImages,
                                deletedImages,
                                d => d.ImagesPath);

                            if (uploadSuccess)
                            {
                                // 统计成功上传的图片数量
                                if (updatedImages != null)
                                {
                                    totalSuccessCount += updatedImages.Count;
                                    MainForm.Instance.uclog.AddLog($"SKU {detail.SKU} 成功处理 {updatedImages.Count} 张图片");
                                }
                                if (deletedImages != null)
                                {
                                    MainForm.Instance.uclog.AddLog($"SKU {detail.SKU} 成功删除 {deletedImages.Count} 张图片");
                                }
                            }
                            else
                            {
                                // 统计失败的图片数量
                                if (updatedImages != null)
                                {
                                    totalFailCount += updatedImages.Count;
                                }
                                if (deletedImages != null)
                                {
                                    totalFailCount += deletedImages.Count;
                                }
                                MainForm.Instance.uclog.AddLog($"SKU {detail.SKU} 图片处理失败", UILogType.错误);
                            }

                            // 如果处理了图片，更新SKU的HasUnsavedImageChanges标志
                            detail.HasUnsavedImageChanges = false; // 重置标记，因为已处理

                            // 清空相关缓存
                            if (skuImageDataCache.ContainsKey(detail))
                            {
                                skuImageDataCache.Remove(detail);
                            }
                            if (skuImageDeletedCache.ContainsKey(detail))
                            {
                                skuImageDeletedCache.Remove(detail);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.uclog.AddLog($"处理SKU图片时出错（SKU: {detail.SKU ?? "未命名"}）：{ex.Message}", Global.UILogType.错误);
                        totalFailCount++;
                    }
                }

                // 处理完成后清空所有缓存
                skuImageDataCache.Clear();
                skuImageDeletedCache.Clear();

                if (totalFailCount > 0)
                {
                    MainForm.Instance.uclog.AddLog($"SKU图片处理完成，成功 {totalSuccessCount} 张，失败 {totalFailCount} 张", Global.UILogType.警告);
                }
                else if (totalSuccessCount > 0)
                {
                    MainForm.Instance.uclog.AddLog($"成功处理 {totalSuccessCount} 张SKU图片");
                }

                return totalFailCount == 0;
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"上传SKU图片时发生异常：{ex.Message}", Global.UILogType.错误);
                return false;
            }
        }

        #endregion

    }
}
