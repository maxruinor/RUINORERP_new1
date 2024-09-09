using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.UI.BaseForm;
using RUINORERP.Common;
using Krypton.Navigator;
using Krypton.Toolkit;
using RUINORERP.Model;
using RUINORERP.Business;
using RUINORERP.UI.Common;
using RUINORERP.Common.Helper;
using BNR;
using System.IO;
using FluentValidation;
using RUINORERP.Global;
using System.Collections.Concurrent;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using ObjectsComparer;
using RUINORERP.Global.CustomAttribute;
using static RUINORERP.UI.Common.DataBindingHelper;
using Microsoft.International.Converters.PinYinConverter;
using RUINORERP.Common.Extensions;
using System.Drawing.Imaging;
using NPOI.SS.Formula.Functions;
using SqlSugar;
using RUINORERP.Business.Processor;
using Netron.GraphLib;
using RUINOR.WinFormsUI.TileListView;

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
        tb_ProdPropertyController<tb_ProdProperty> mcProperty = Startup.GetFromFac<tb_ProdPropertyController<tb_ProdProperty>>();
        tb_ProdPropertyValueController<tb_ProdPropertyValue> mcPropertyValue = Startup.GetFromFac<tb_ProdPropertyValueController<tb_ProdPropertyValue>>();
        tb_ProdCategoriesController<tb_ProdCategories> mca = Startup.GetFromFac<tb_ProdCategoriesController<tb_ProdCategories>>();

        tb_BoxRulesController<tb_BoxRules> ctrBoxRules = Startup.GetFromFac<tb_BoxRulesController<tb_BoxRules>>();


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
        public frmProductEdit()
        {
            InitializeComponent();
            if (!this.DesignMode)
            {
                kryptonNavigator1.Button.ButtonDisplayLogic = ButtonDisplayLogic.None;
                kryptonPageMain.ClearFlags(KryptonPageFlags.All);
                kryptonPage2.ClearFlags(KryptonPageFlags.All);
                kryptonPage3.ClearFlags(KryptonPageFlags.All);
                kryptonPageImage.ClearFlags(KryptonPageFlags.All);
                //this.OnShowHelp += FrmProductEdit_OnShowHelp;
                categorylist = mca.Query();
                prodpropValueList = mcPropertyValue.QueryByNav(c => true);
                prodpropList = mcProperty.Query();
                // this.bindingSourceList.ListChanged += BindingSourceList_ListChanged;
                InitListData();
                SetBaseValue<Eav_ProdDetails>();
                InitDataTocmbbox();
            }

        }

        List<Eav_ProdDetails> removeSkuList = new List<Eav_ProdDetails>();

        private void DataGridView1_删除选中行(object sender, EventArgs e)
        {

            if (dataGridView1.CurrentRow.Index != -1)
            {
                Eav_ProdDetails sukProd = dataGridView1.CurrentRow.DataBoundItem as Eav_ProdDetails;
                bindingSourceList.Remove(sukProd);
                //将删除的sku行 暂时加入一个临时列表中
                removeSkuList.Add(sukProd);
            }

        }

        private void FrmProductEdit_OnShowHelp()
        {
            base.toolTipBase.Hide(this);
            if (ActiveControl.GetType().ToString() == "ComponentFactory.Krypton.Toolkit.KryptonTextBox+InternalTextBox")
            {
                KryptonTextBox txt = ActiveControl.Parent as KryptonTextBox;
                if (txt.DataBindings.Count > 0)
                {
                    //txt.DataBindings[0].BindingMemberInfo;
                }
            }
            else
            {

            }
            base.toolTipBase.SetToolTip(ActiveControl, "提示");
            base.toolTipBase.Show(ActiveControl.Name + "asdf", ActiveControl);
        }

        private void AddTopPage()
        {
            // Create a new krypton page to be added
            KryptonPage page = new KryptonPage();

            page.ClearFlags(KryptonPageFlags.DockingAllowClose);
            // Set the page title
            page.Text = "产品资料";

            // Remove the default image for the page
            page.ImageSmall = null;

            // Set the padding so contained controls are indented
            page.Padding = new Padding(7);

            // Get the colors to use for this new page
            Color normal = _normal[_count % _normal.Length];
            Color select = _select[_count % _select.Length];

            // Set the page colors
            page.StateNormal.Page.Color1 = select;
            page.StateNormal.Page.Color2 = normal;
            page.StateNormal.Tab.Back.Color2 = normal;
            page.StateSelected.Tab.Back.Color2 = select;
            page.StateTracking.Tab.Back.Color2 = _hotMain;
            page.StatePressed.Tab.Back.Color2 = _hotMain;

            // We want the page drawn as a gradient with colors relative to its own area
            page.StateCommon.Page.ColorAlign = PaletteRectangleAlign.Local;
            page.StateCommon.Page.ColorStyle = PaletteColorStyle.Sigma;

            // We add an embedded navigator with its own pages to mimic OneNote operation
            AddEmbeddedNavigator(page);
            page.ClearFlags(KryptonPageFlags.All);
            // Add page to end of the navigator collection
            kryptonNavigator1.Pages.Add(page);

            // Bump the page index to use next
            _count++;


        }


        private void UCProductEdit_Load(object sender, EventArgs e)
        {
            //flowLayoutPanel1.AutoScroll = true;
            UIProdCateHelper.BindToTreeViewNoRootNode(categorylist, txtcategory_ID.TreeView);
            // AddTopPage();
            // Do not allow the document pages to be closed or made auto hidden/docked
            kryptonNavigator1.Button.CloseButtonDisplay = ButtonDisplay.Hide;

            //设置导航子集合加载时没有变化过
            if (EditEntity != null)
            {
                //if (EditEntity.tb_BoxRuleses != null)
                //{
                //    EditEntity.tb_BoxRuleses.ForEach(c => c.HasChanged = false);
                //}
                //else
                //{
                //    EditEntity.tb_BoxRuleses = new List<tb_BoxRules>();
                //    //因为后面会添加到这个集合中
                //}
            }

            //kryptonNavigator1.Pages.Add(NewDocument());

            //txtBarCode.Text = BizCodeGenerationHelper.GetBizNo(BizType.销售订单);
            // txtNo.Text = BizCodeGenerationHelper.GetProdNo(BizType.);
            //txtBarCode.Text = Common.BarCodeCreator.CreateProductBarCode("2", "3232423");

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
            EnumBindingHelper bindingHelper = new EnumBindingHelper();
            //https://www.cnblogs.com/cdaniu/p/15236857.html
            //加载枚举，并且可以过虑不需要的项
            List<int> exclude = new List<int>();
            exclude.Add((int)ProductAttributeType.虚拟);
            bindingHelper.InitDataToCmbByEnumOnWhere<tb_Prod>(typeof(ProductAttributeType).GetListByEnum(2, exclude.ToArray()), e => e.PropertyType, cmbPropertyType);
            // InitDataToCmbByEnumDynamicGeneratedDataSource<tb_Prod>(typeof(Global.ProductAttributeType), e => e.PropertyType, cmbPropertyType);
            DataBindingHelper.InitDataToCmbByEnumDynamicGeneratedDataSource<tb_Prod>(typeof(GoodsSource), e => e.SourceType, cmbSourceType, false);
        }





        private KryptonPage NewPage(string name, int image, Control content)
        {
            // Create new page with title and image
            KryptonPage p = new KryptonPage();
            p.Text = name + _count.ToString();
            p.TextTitle = name + _count.ToString();
            p.TextDescription = name + _count.ToString();
            p.UniqueName = p.Text;


            // Add the control for display inside the page
            content.Dock = DockStyle.Fill;
            p.Controls.Add(content);

            _count++;
            return p;
        }
        private KryptonPage NewDocument()
        {
            KryptonPage page = NewPage("Document ", 0, new UControls.UCLocList());

            // Do not allow the document pages to be closed or made auto hidden/docked
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden |
                            KryptonPageFlags.DockingAllowDocked |
                            KryptonPageFlags.DockingAllowClose);
            page.AllowDrop = false;
            return page;
        }

        private void AddEmbeddedNavigator(KryptonPage page)
        {
            // Create a navigator to embed inside the page
            KryptonNavigator nav = new KryptonNavigator();

            // We want the navigator to fill the entire page area
            nav.Dock = DockStyle.Fill;

            // Remove the close and context buttons
            nav.Button.CloseButtonDisplay = ButtonDisplay.Hide;
            nav.Button.ButtonDisplayLogic = ButtonDisplayLogic.None;

            // Set the required tab and bar settings
            nav.Bar.BarOrientation = VisualOrientation.Right;
            nav.Bar.ItemOrientation = ButtonOrientation.FixedTop;
            nav.Bar.ItemSizing = BarItemSizing.SameWidthAndHeight;
            nav.Bar.TabBorderStyle = TabBorderStyle.RoundedEqualSmall;
            nav.Bar.TabStyle = TabStyle.StandardProfile;

            // Do not draw the bar area background, let parent page show through
            nav.StateCommon.Panel.Draw = InheritBool.False;

            // Use same font for all tab states and we want text aligned to near
            nav.StateCommon.Tab.Content.ShortText.Font = SystemFonts.IconTitleFont;
            nav.StateCommon.Tab.Content.ShortText.TextH = PaletteRelativeAlign.Near;

            // Set the page colors
            nav.StateCommon.Tab.Content.Padding = new Padding(4);
            nav.StateNormal.Tab.Back.ColorStyle = PaletteColorStyle.Linear;
            nav.StateNormal.Tab.Back.Color1 = _select[_count % _select.Length];
            nav.StateNormal.Tab.Back.Color2 = Color.White;
            nav.StateNormal.Tab.Back.ColorAngle = 270;
            nav.StateSelected.Tab.Back.ColorStyle = PaletteColorStyle.Linear;
            nav.StateSelected.Tab.Back.Color2 = _hotEmbedSelected;
            nav.StateSelected.Tab.Back.ColorAngle = 270;
            nav.StateTracking.Tab.Back.ColorStyle = PaletteColorStyle.Solid;
            nav.StateTracking.Tab.Back.Color1 = _hotEmbedTracking;
            nav.StatePressed.Tab.Back.ColorStyle = PaletteColorStyle.Solid;
            nav.StatePressed.Tab.Back.Color1 = _hotEmbedTracking;

            // Add a random number of pages
            Random rand = new Random();
            int numPages = 3 + rand.Next(5);

            for (int i = 0; i < numPages; i++)
                nav.Pages.Add(NewEmbeddedPage(_titleEmbedded[rand.Next(_titleEmbedded.Length - 1)]));

            page.Controls.Add(nav);
        }

        private KryptonPage NewEmbeddedPage(string title)
        {
            KryptonPage page = new KryptonPage();
            page.Text = title;
            page.ImageSmall = null;
            return page;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void btnOk_Click(object sender, EventArgs e)
        {

            //比方 暂时没有供应商  又是外键，则是如何处理的？
            bool vb = base.ShowInvalidMessage(mcProdBase.Validator(EditEntity, mcProdBase));
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
                if (MessageBox.Show("产品设置为不可用用，确定保存吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
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
                bool vd = base.ShowInvalidMessage(mcDetail.Validator(item));
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
                //MessageBox.Show("数据未修改");
                //如果主表没动，sku详情变也。也需要将主表设置为修改状态
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






            //前后比较是否变化
            //ComPareResult result = UITools.ComPare(oldOjb, EditEntity);
            //if (result.IsEqual)
            //{
            //    //MessageBox.Show("数据未修改");
            //    //return;
            //}
            //else
            //{
            //    if (EditEntity.actionStatus == ActionStatus.无操作)
            //    {
            //        EditEntity.actionStatus = ActionStatus.修改;
            //    }
            //}

            //然后再写保存逻辑
            bindingSourceEdit.EndEdit();

            //要先保存再修改才起作用

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 得到关系列表
        /// </summary>
        /// <param name="removeList">如果有删除的行，则标识出来为删除状态</param>
        /// <returns></returns>
        private List<tb_Prod_Attr_Relation> GetRelations(List<Eav_ProdDetails> removeList)
        {
            //明细超过一行，则为多属性。否则是单属性，或groupName有值就是多属性了
            List<tb_Prod_Attr_Relation> RelationList = new List<tb_Prod_Attr_Relation>();
            foreach (var item in bindingSourceList)
            {
                if (item is Eav_ProdDetails)
                {
                    Eav_ProdDetails epd = item as Eav_ProdDetails;
                    //多属性
                    if (!string.IsNullOrEmpty(epd.GroupName))
                    {
                        //有多个属性值是，则是复合特性
                        if (epd.GroupName.Contains(","))
                        {
                            foreach (string propertyValueName in epd.GroupName.Split(','))
                            {
                                tb_Prod_Attr_Relation rela = SKUDetailToRelateion(item as Eav_ProdDetails, prodpropValueList, propertyValueName);
                                rela.ActionStatus = epd.ActionStatus;
                                RelationList.Add(rela);
                            }
                        }
                        else
                        {
                            tb_Prod_Attr_Relation rela = SKUDetailToRelateion(item as Eav_ProdDetails, prodpropValueList, epd.GroupName);
                            rela.ActionStatus = epd.ActionStatus;
                            RelationList.Add(rela);
                        }

                    }
                    else
                    {
                        //单属性

                        #region
                        tb_Prod_Attr_Relation rela = new tb_Prod_Attr_Relation();
                        IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                        var detail = mapper.Map<tb_ProdDetail>(item);
                        //rela.Property_ID = -1;
                        //rela.PropertyValueID = -1;
                        //tb_Prod_Attr_Relation relaTemp = detail.tb_Prod_Attr_Relations.FirstOrDefault(w => w.PropertyValueID == ppv.PropertyValueID);
                        //if (relaTemp != null)
                        //{
                        //    rela = relaTemp;
                        //}
                        rela.ActionStatus = detail.ActionStatus;
                        //详情中才保存了要更新的数据
                        rela.tb_proddetail = detail;

                        //关系表中 只保存了 属性及值。
                        //var ra = detail.tb_Prod_Attr_Relations.FirstOrDefault(w => w.Property_ID == ppv.Property_ID && w.PropertyValueID == ppv.PropertyValueID);
                        //if (ra != null)
                        //{
                        //    rela.RAR_ID = ra.RAR_ID;
                        //}

                        #endregion



                        RelationList.Add(rela);

                    }

                }
            }

            foreach (var item in removeList)
            {
                Eav_ProdDetails epd = item as Eav_ProdDetails;
                foreach (string propertyValueName in epd.GroupName.Split(','))
                {
                    //关系表中 只保存了 属性及值。
                    tb_Prod_Attr_Relation rela = SKUDetailToRelateion(item as Eav_ProdDetails, prodpropValueList, propertyValueName);
                    rela.ActionStatus = ActionStatus.删除;
                    RelationList.Add(rela);
                }

            }
            return RelationList;
        }




        private List<tb_ProdDetail> GetDetailsAndRelations(tb_Prod baseInfo, List<Eav_ProdDetails> removeList)
        {
            List<tb_ProdDetail> details = new List<tb_ProdDetail>();


            foreach (var item in bindingSourceList)
            {
                if (item is Eav_ProdDetails)
                {
                    Eav_ProdDetails epd = item as Eav_ProdDetails;
                    tb_ProdDetail detail = new tb_ProdDetail();
                    IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                    //为null的不需要，不然会覆盖
                    detail = mapper.Map<tb_ProdDetail>(epd);

                    if (detail.ProdDetailID == 0)
                    {
                        detail.Created_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID;
                        detail.Created_at = DateTime.Now;
                    }
                    else
                    {
                        detail.Modified_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID;
                        detail.Modified_at = DateTime.Now;
                    }
                    if (epd.tb_ProdDetail != null)
                    {
                        detail.ProdBaseID = epd.tb_ProdDetail.ProdBaseID;
                        if (detail.ActionStatus == ActionStatus.无操作 || detail.ActionStatus == ActionStatus.加载)
                        {
                            details.Add(detail);
                            continue;
                        }
                    }

                    #region 生成关系
                    //多属性
                    if (!string.IsNullOrEmpty(epd.GroupName))
                    {
                        //明细超过一行，则为多属性。否则是单属性，或groupName有值就是多属性了
                        List<tb_Prod_Attr_Relation> RelationList = new List<tb_Prod_Attr_Relation>();
                        //有多个属性值是，则是复合特性
                        if (epd.GroupName.Contains(","))
                        {
                            foreach (string propertyValueName in epd.GroupName.Split(','))
                            {
                                tb_Prod_Attr_Relation rela = SKUDetailToRelateion(item as Eav_ProdDetails, prodpropValueList, propertyValueName);
                                rela.ActionStatus = epd.ActionStatus;
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
                            tb_Prod_Attr_Relation rela = SKUDetailToRelateion(item as Eav_ProdDetails, prodpropValueList, epd.GroupName);
                            rela.ActionStatus = epd.ActionStatus;
                            RelationList.Add(rela);
                        }
                        detail.tb_Prod_Attr_Relations.AddRange(RelationList);
                    }
                    else
                    {
                        #region 单属性
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
                        #endregion
                    }
                    #endregion

                    details.Add(detail);
                }
            }


            //多属性才可能被删除一些明细,这里实际是将 删除的。和没有删除的分成两部分，前面是保留的，这里是删除移除的。
            //重新合并。保持状态，后面看是实际删除还是逻辑删除
            foreach (var item in removeList)
            {
                if (item.ProdDetailID > 0)
                {
                    tb_ProdDetail removeDetail = new tb_ProdDetail();
                    Eav_ProdDetails epd = item as Eav_ProdDetails;
                    removeDetail = item.tb_ProdDetail;
                    removeDetail.ActionStatus = ActionStatus.删除;
                    details.Add(removeDetail);
                }
            }
            return details;
        }


        /// <summary>
        /// 得到SKU明细
        /// </summary>
        /// <returns></returns>
        private List<tb_ProdDetail> GetDetails(List<Eav_ProdDetails> removeList)
        {
            List<tb_ProdDetail> details = new List<tb_ProdDetail>();
            foreach (var item in bindingSourceList)
            {
                if (item is Eav_ProdDetails)
                {
                    tb_ProdDetail detail = new tb_ProdDetail();
                    Eav_ProdDetails epd = item as Eav_ProdDetails;
                    IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                    //为null的不需要，不然会覆盖
                    detail = mapper.Map<tb_ProdDetail>(epd);
                    if (epd.tb_ProdDetail != null)
                    {
                        detail.ProdBaseID = epd.tb_ProdDetail.ProdBaseID;
                    }
                    details.Add(detail);
                }
            }

            //多属性才可能被删除一些明细
            foreach (var item in removeList)
            {
                Eav_ProdDetails epd = item as Eav_ProdDetails;
                //关系表中 只保存了 属性及值。
                throw new Exception("完善删除逻辑");
                //details.Add(rela);
            }
            return details;
        }

        /// <summary>
        /// 将SKU表格中显示的明细转换为关系行
        /// </summary>
        /// <param name="item"></param>
        /// <param name="prodPropertyValues"></param>
        /// <param name="propertyValueName"></param>
        /// <returns></returns>
        private tb_Prod_Attr_Relation SKUDetailToRelateion(Eav_ProdDetails item, List<tb_ProdPropertyValue> prodPropertyValues, string propertyValueName)
        {
            tb_Prod_Attr_Relation rela = new tb_Prod_Attr_Relation();
            IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
            var detail = mapper.Map<tb_ProdDetail>(item);
            tb_ProdPropertyValue ppv = prodPropertyValues.FirstOrDefault(p => p.PropertyValueName == propertyValueName);
            rela.Property_ID = ppv.Property_ID;
            rela.PropertyValueID = ppv.PropertyValueID;
            tb_Prod_Attr_Relation relaTemp = detail.tb_Prod_Attr_Relations.FirstOrDefault(w => w.PropertyValueID == ppv.PropertyValueID);
            if (relaTemp != null)
            {
                rela = relaTemp;
            }
            rela.ActionStatus = detail.ActionStatus;
            //详情中才保存了要更新的数据
            //rela.tb_ProdDetail = detail;

            //关系表中 只保存了 属性及值。
            var ra = detail.tb_Prod_Attr_Relations.FirstOrDefault(w => w.Property_ID == ppv.Property_ID && w.PropertyValueID == ppv.PropertyValueID);
            if (ra != null)
            {
                rela.RAR_ID = ra.RAR_ID;
            }
            return rela;
        }

        private tb_Prod _EditEntity;
        public tb_Prod EditEntity { get => _EditEntity; set => _EditEntity = value; }

        List<tb_ProdCategories> categorylist = new List<tb_ProdCategories>(0);

        tb_Prod oldOjb = null;


        public async override void BindData(BaseEntity entity)
        {
            oldOjb = CloneHelper.DeepCloneObject<tb_Prod>(entity);
            _EditEntity = entity as tb_Prod;
            if (_EditEntity.ProdBaseID == 0)
            {
                _EditEntity.DataStatus = (int)RUINORERP.Global.DataStatus.草稿;
                _EditEntity.ActionStatus = ActionStatus.新增;
                long maxid = await mcProdBase.GetMaxID();
                //生成编号
                _EditEntity.ProductNo = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.ProductNo);
                //_EditEntity.ShortCode = maxid.ToString().PadLeft(4, '0');//推荐
                //助记码要在类目选择后生成，要有规律
                //详情直接清空，因为是新增 ，属性这块不清楚。后面再优化：TODO:
                _EditEntity.tb_ProdDetails = new List<tb_ProdDetail>();
                _EditEntity.PropertyType = 1;// cmbPropertyType   1为单属性
                _EditEntity.tb_Prod_Attr_Relations = new List<tb_Prod_Attr_Relation>();
                // 在类目属性选择后
                //if (_EditEntity.tb_ProdDetails != null && _EditEntity.tb_ProdDetails.Count > 0)
                //{
                //    _EditEntity.tb_ProdDetails.ForEach(c => c.ProdBaseID = 0);
                //    _EditEntity.tb_ProdDetails.ForEach(c => c.ProdDetailID = 0);
                //    //_EditEntity.tb_ProdDetails.ForEach(c => c.SKU = 0);
                //}
            }
            else
            {
                //显示图片
            }

            #region 类别
            var parent_categorie = new Binding("Text", entity, "category_ID", true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            parent_categorie.Format += new ConvertEventHandler(DataSourceToControl);
            //将控件的数据类型转换为数据源要求的数据类型。
            parent_categorie.Parse += new ConvertEventHandler(ControlToDataSource);
            txtcategory_ID.DataBindings.Add(parent_categorie);

            #endregion

            DataBindingHelper.BindData4CmbByEnum<tb_Prod>(entity, k => k.SourceType, typeof(GoodsSource), cmbSourceType, false);
            DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.ProductNo, txtNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.ShortCode, txtShortCode, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text, false);
            //DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.SourceType, txtSourceType, BindDataType4TextBox.Text, false);

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
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID);
            DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v => v.Name, txtLocation_ID);
            DataBindingHelper.BindData4Cmb<tb_ProductType>(entity, k => k.Type_ID, v => v.TypeName, cmbType_ID);
            DataBindingHelper.BindData4CmbByEntity<tb_StorageRack>(entity, k => k.Rack_ID, cmbRack_ID);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);

            //DataBindingHelper.BindData4CmbByEnum<tb_Prod>(entity, k => k.PropertyType, typeof(Global.ProductAttributeType), cmbPropertyType);
            DataBindingHelper.BindData4CmbByEnumData<tb_Prod>(entity, k => k.PropertyType, cmbPropertyType);
            // DataBindingHelper.BindData4Cmb<tb_ProdPropertyType>(entity, k => k.PropertyType_ID, v => v.PropertyTypeName, cmbPropertyType);
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
                base.InitRequiredToControl(new tb_ProdValidator(), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }
            base.BindData(entity);
            LoadBaseInfoSKUList(_EditEntity);
            listView1.UpdateUI();
            Task task_2 = Task.Run(task_Help);
            //task_2.Wait();  //注释打开则等待task_2延时，注释掉则不等待


            EditEntity.PropertyChanged += (sender, s2) =>
            {
                if (EditEntity.Category_ID.HasValue && EditEntity.Category_ID.Value > 0 && s2.PropertyName == entity.GetPropertyName<tb_Prod>(c => c.Category_ID))
                {
                    var obj = CacheHelper.Instance.GetEntity<tb_ProdCategories>(EditEntity.Category_ID.Value);
                    if (obj != null && obj.ToString() != "System.Object")
                    {
                        if (obj is tb_ProdCategories Cate)
                        {
                            string constpara = string.Empty;
                            var para = Cate.Category_name;
                            if (para.Length < 3)
                            {
                                constpara = para.ToPinYin(true).Substring(0, para.Length);
                            }
                            else
                            {
                                constpara = para.ToPinYin(true).Substring(0, 3);
                            }

                            _EditEntity.ShortCode = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.ShortCode, constpara); //推荐
                        }
                    }
                }

            };

        }

        private void Img_Parse(object sender, ConvertEventArgs e)
        {

        }


        public async Task task_Help()
        {
            await Task.Delay(1000);
            Console.WriteLine("2秒后执行，方式二 输出语句...");
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
            e.Value = Common.ImageHelper.byteArrayToImage(img);
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
                cevent.Value = CacheHelper.Instance.GetEntity<tb_ProdCategories>(cevent.Value.ToString()).Category_name;
                //cevent.Value = CacheHelper.Instance.GetValue<tb_ProductCategories>(cevent.Value);

                //显示名称
                //tb_ProductCategories entity = list.Find(t => t.category_ID.ToString() == cevent.Value.ToString());
                //if (entity != null)
                //{
                //    cevent.Value = entity.Category_name;
                //}
                //else
                //{
                //    cevent.Value = 0;
                //}
            }

        }

        private void ControlToDataSource(object sender, ConvertEventArgs cevent)
        {
            // The method converts back to decimal type only. 
            //if (cevent.DesiredType != typeof(decimal)) return;
            if (string.IsNullOrEmpty(cevent.Value.ToString()) || cevent.Value.ToString() == "类目根结节")
            {
                cevent.Value = null;
            }
            else
            {
                tb_ProdCategories entity = categorylist.Find(t => t.Category_name == cevent.Value.ToString());
                if (entity != null)
                {
                    cevent.Value = entity.Category_ID;
                    //同时给出品号的建议类目代码
                    txtNo.Text = BNRFactory.Default.Create("{CN:" + entity.Category_name + "}");
                    //txtNo.Text = BNRFactory.Default.Create("{S:OD}{CN:广州}");

                }
                else
                {
                    cevent.Value = 0;
                }
            }



        }

        private void chkAutoCreateProdNo_CheckedChanged(object sender, EventArgs e)
        {
            txtNo.ReadOnly = chkAutoCreateProdNo.Checked;
            if (chkAutoCreateProdNo.Checked)
            {
                txtNo.Text = Common.BarCodeCreator.CreateProductBarCode(EditEntity.Category_ID.ToString(), true, "32324");
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
                System.Drawing.Image img = System.Drawing.Image.FromFile(pathName);
                this.pictureBox1.Image = img;

                //将图像读入到字节数组
                System.IO.FileStream fs = new System.IO.FileStream(pathName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                byte[] buffByte = new byte[fs.Length];
                fs.Read(buffByte, 0, (int)fs.Length);
                fs.Close();
                fs = null;
                // 判断图片大小是否超过 500KB
                if (buffByte.Length > 500 * 1024)
                {
                    // 压缩图片
                    ImageCodecInfo jpegCodec = GetEncoderInfo(ImageFormat.Jpeg);
                    EncoderParameters encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);
                    img = (Image)new Bitmap(img, new Size(800, 600));
                    img.Save("compressed.jpg", jpegCodec, encoderParams);

                    // 重新读取压缩后的图片
                    img = System.Drawing.Image.FromFile("compressed.jpg");
                    this.pictureBox1.Image = img;

                    MessageBox.Show("图片大小超过 500KB，已自动压缩。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // 将压缩后的图片转换为 byte[] 数组
                    byte[] compressedImageBytes = File.ReadAllBytes("compressed.jpg");

                    // 在这里将 compressedImageBytes 保存到数据库中
                    _EditEntity.Images = compressedImageBytes;
                }
                else
                {
                    _EditEntity.Images = buffByte;
                }

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

        private void cmbPropertyType_SelectedIndexChanged(object sender, EventArgs e)
        {
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
                                BindToSkulistGrid(new List<Eav_ProdDetails>());
                            }
                            if (EditEntity.ActionStatus != ActionStatus.加载)
                            {
                                Eav_ProdDetails ppg = new Eav_ProdDetails();
                                ppg.GroupName = "";
                                ppg.SKU = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.SKU_No);
                                bindingSourceList.Add(ppg);
                            }

                        }
                        #endregion

                        if (dataGridView1.Columns.Contains("GroupName"))
                        {
                            dataGridView1.Columns["GroupName"].Visible = false;
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


                        break;
                    case ProductAttributeType.捆绑:
                        break;
                    case ProductAttributeType.虚拟:
                        break;
                    default:
                        break;
                }

            }
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

                    //  listView1.Height = 80 * PropertyCounter;



                    //  kryptonGroupBoxDataGridView.Width = flowLayoutPanel1.Width;
                    //  kryptonGroupBoxDataGridView.Height = flowLayoutPanel1.Height - kryptonGroupBoxListView.Height;

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

            //显示属性  加载勾选框
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
                BindToSkulistGrid(new List<Eav_ProdDetails>());
                bindingSourceList.ListChanged += BindingSourceList_ListChanged;
            }
            //显示表格内容 根据sku更新
            LoadRelationToEavSku(entityProdBase);
            #endregion

            //加载也能编辑



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
            Eav_ProdDetails entity = new Eav_ProdDetails();
            switch (e.ListChangedType)
            {
                case ListChangedType.Reset:
                    break;
                case ListChangedType.ItemAdded:
                    entity = bindingSourceList.List[e.NewIndex] as Eav_ProdDetails;
                    if (entity.ActionStatus != ActionStatus.加载)
                    {
                        entity.ActionStatus = ActionStatus.新增;
                    }
                    break;
                case ListChangedType.ItemDeleted:
                    if (e.NewIndex < bindingSourceList.Count)
                    {
                        entity = bindingSourceList.List[e.NewIndex] as Eav_ProdDetails;
                        entity.ActionStatus = ActionStatus.删除;
                    }
                    break;
                case ListChangedType.ItemMoved:
                    break;
                case ListChangedType.ItemChanged:
                    entity = bindingSourceList.List[e.NewIndex] as Eav_ProdDetails;
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
        /// 2023-10-31
        /// </summary>
        /// <param name="isMultProperty"></param>
        /// <param name="details"></param>
        /// <param name="relations"></param>
        private async void LoadRelationToEavSku(List<tb_ProdDetail> details, List<tb_Prod_Attr_Relation> relations)
        {
            bool isMultProperty = false;
            List<Eav_ProdDetails> propGroups = new List<Eav_ProdDetails>();
            if (bindingSourceList.DataSource is List<Eav_ProdDetails>)
            {
                propGroups = bindingSourceList.DataSource as List<Eav_ProdDetails>;
            }
            if (isMultProperty)
            {
                //为了显示属性值中文
                //根据关系明细中的详情id分组
                var detailIds = relations.GroupBy(d => d.ProdDetailID);
                foreach (var did in detailIds)
                {
                    tb_ProdDetail detail = details.Where(d => d.ProdDetailID == did.Key.Value).FirstOrDefault();
                    //找到对应的
                    List<tb_Prod_Attr_Relation> pars = relations.FindAll(w => w.ProdDetailID == detail.ProdDetailID).ToList();
                    string groupName = string.Empty;
                    foreach (tb_Prod_Attr_Relation par in pars)
                    {
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
                    IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                    Eav_ProdDetails ppg = mapper.Map<Eav_ProdDetails>(detail);
                    ppg.GroupName = groupName;
                    ppg.tb_Prod_Attr_Relations = pars;
                    ppg.tb_ProdDetail = detail;
                    bindingSourceList.Add(ppg);
                    ppg.ActionStatus = ActionStatus.加载;
                }
            }
            else
            {
                //单属性的话，这里应该只有一行详情
                var detailIds = relations.GroupBy(d => d.ProdDetailID);
                foreach (var did in detailIds)
                {
                    tb_ProdDetail detail = new tb_ProdDetail();
                    if (did.Key != null)
                    {
                        detail = await mcDetail.BaseQueryByIdAsync(did.Key);
                    }
                    else
                    {
                        // detail= .
                    }
                    IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                    Eav_ProdDetails ppg = mapper.Map<Eav_ProdDetails>(detail);
                    ppg.GroupName = "";
                    ppg.tb_Prod_Attr_Relations = relations;
                    bindingSourceList.Add(ppg);
                    ppg.ActionStatus = ActionStatus.加载;
                }
                btnAddProperty.Enabled = false;
                btnClear.Enabled = false;
            }

            return;
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
            List<Eav_ProdDetails> propGroups = new List<Eav_ProdDetails>();
            if (bindingSourceList.DataSource is List<Eav_ProdDetails>)
            {
                propGroups = bindingSourceList.DataSource as List<Eav_ProdDetails>;
            }

            //为了显示属性值中文
            //根据关系明细中的详情id分组 相同明细有多行属性值时，
            #region 新思路
            foreach (tb_ProdDetail detail in entityProdBase.tb_ProdDetails)
            {
                List<tb_Prod_Attr_Relation> pars = relations.FindAll(w => w.ProdDetailID == detail.ProdDetailID).ToList();
                string groupName = string.Empty;
                foreach (tb_Prod_Attr_Relation par in pars)
                {
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
                if (groupName.Trim().Length > 0)
                {
                    isMultProperty = true;
                }
                IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                //明细转为中间数据视图
                Eav_ProdDetails ppg = mapper.Map<Eav_ProdDetails>(detail);
                ppg.GroupName = groupName;
                ppg.tb_Prod_Attr_Relations = pars;//暂存
                ppg.tb_ProdDetail = detail;//暂存
                bindingSourceList.Add(ppg);
                ppg.ActionStatus = ActionStatus.加载;
            }

            #endregion

            #region 旧思路
            /*
            var detailIds = relations.GroupBy(d => d.ProdDetailID);
            foreach (var did in detailIds)
            {
                tb_ProdDetail detail = entityProdBase.tb_ProdDetails.FirstOrDefault(t => t.ProdDetailID == did.Key.Value);// await mcDetail.BaseQueryByIdAsync(did.Key);
                List<tb_Prod_Attr_Relation> pars = relations.FindAll(w => w.ProdDetailID == detail.ProdDetailID).ToList();

                string groupName = string.Empty;
                foreach (tb_Prod_Attr_Relation par in pars)
                {
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
                //if (groupName.Trim().Length < 1 )
                //{
                //    MessageBox.Show("组合名不可能为空");
                //}
                IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                Eav_ProdDetails ppg = mapper.Map<Eav_ProdDetails>(detail);
                ppg.GroupName = groupName;
                ppg.tb_Prod_Attr_Relations = pars;
                bindingSourceList.Add(ppg);
                ppg.actionStatus = ActionStatus.加载;
            }*/
            #endregion

            if (!isMultProperty)
            {
                btnAddProperty.Enabled = false;
                btnClear.Enabled = false;
            }

        }

        /*
        /// <summary>
        /// 添加产品特性
        /// </summary>
        private void AddProdProperty(tb_ProdProperty ppv, List<tb_ProdPropertyValue> listOptionValue)
        {
            #region 新增修改式
            listView1.Visible = true;
            listView1.CheckBoxes = true;

            listView1.ShowGroups = true;  //记得要设置ShowGroups属性为true（默认是false），否则显示不出分组
            listView1.View = View.LargeIcon;
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, 25);
            listView1.SmallImageList = imgList;

            //create goups
            ListViewGroup lvg = new ListViewGroup();  //创建男生分组
            lvg.Header = ppv.PropertyName;  //设置组的标题。
            lvg.Name = ppv.Property_ID.ToString();
            lvg.Tag = ppv;
            lvg.HeaderAlignment = HorizontalAlignment.Left;   //设置组标题文本的对齐方式。（默认为Left）

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

            if (!listView1.Groups.Cast<ListViewGroup>().Any(i => i.Header == ppv.PropertyName.Trim()))
            {
                listView1.Groups.Add(lvg);
                string keys = string.Empty;
                string names = string.Empty;
                foreach (var item in listOptionValue.Where(w => w.Property_ID == ppv.Property_ID).ToList())
                {
                    keys += item.PropertyValueID + ",";
                    names += item.PropertyValueName + ",";
                    ListViewItem lvi = new ListViewItem();
                    // lvi.ImageIndex = i;
                    lvi.Name = item.PropertyValueID.ToString();
                    lvi.Tag = item;
                    lvi.Text = item.PropertyValueName;
                    lvi.Checked = true;
                    lvi.ForeColor = Color.Blue;  //设置行颜色
                    lvg.Items.Add(lvi);   //分组添加子项
                    listView1.Items.Add(lvi);
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
                BindToSkulistGrid(new List<Eav_ProdDetails>());
            }


            #endregion
        }
        */

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
                BindToSkulistGrid(new List<Eav_ProdDetails>());
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

        private void BindToSkulistGrid(List<Eav_ProdDetails> propGroups)
        {
            //ucskulist.dataGridView1.RowHeadersVisible = false;
            //ucskulist.bindingSourceList.DataSource = propGroups;
            //ucskulist.dataGridView1.DataSource = ucskulist.bindingSourceList;
            //ucskulist.dataGridView1.ColumnDisplayControl(ucskulist.FieldNameList);

            dataGridView1.RowHeadersVisible = false;
            bindingSourceList.DataSource = propGroups;
            dataGridView1.DataSource = bindingSourceList;
            //dataGridView1.ColumnDisplayControl(ucskulist.FieldNameList);?????

        }

        /*
        /// <summary>
        /// 将listview的UI值转为属性组
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        private List<KeyValuePair<long, string[]>> GetAttrGoups(ListView lv)
        {
            List<KeyValuePair<long, string[]>> attrGoups = new List<KeyValuePair<long, string[]>>();
            foreach (ListViewGroup g in lv.Groups)
            {
                tb_ProdProperty pp = g.Tag as tb_ProdProperty;
                long key = pp.Property_ID;
                string values = string.Empty;
                foreach (ListViewItem lvitem in g.Items)
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
        */

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


        private void CreateSKUList()
        {
            List<Eav_ProdDetails> propGroups = new List<Eav_ProdDetails>();
            if (bindingSourceList.DataSource is List<Eav_ProdDetails>)
            {
                propGroups = bindingSourceList.DataSource as List<Eav_ProdDetails>;
            }

            //如果存在则更新，
            // List<string> rs = ArrayCombination.Combination4Table(para);
            //目前在数据库端控制属性值表中的名称不能重复。再通过对应的值名找对应的属性值ID和属性ID
            List<string> newMix = ArrayCombination.Combination4Table(AttrGoups);
            ////Intersect 交集，Except 差集，Union 并集 Distinct去重 如果是值类型可以直接用，如果是引用类型则要重写 
            //参考凯旋游戏中的 差集合等处理再加上排序
            //按组合先后排序？
            List<string> oldMix = new List<string>();
            foreach (Eav_ProdDetails epd in propGroups)
            {
                oldMix.Add(epd.GroupName);
            }

            var Item交集 = newMix.Intersect(oldMix);// 交集
                                                  //如果交集没有，则认为新的，与旧的完全不一样。把旧的全删除
            if (Item交集.Count() == 0)
            {
                foreach (var old in oldMix)
                {
                    //更新删除
                    Eav_ProdDetails ep = propGroups.Cast<Eav_ProdDetails>().FirstOrDefault(w => w.GroupName == old.Trim());
                    bindingSourceList.Remove(ep);
                    //将删除的sku行 暂时加入一个临时列表中
                    removeSkuList.Add(ep);
                }
                //添加新的
                foreach (var newItem in newMix)
                {
                    Eav_ProdDetails ppg = new Eav_ProdDetails();
                    ppg.GroupName = newItem;
                    ppg.SKU = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.SKU_No);
                    if (MainForm.Instance.AppContext.SysConfig.UseBarCode)
                    {
                        //补码
                        ppg.BarCode = BizCodeGenerator.Instance.GetBarCode(ppg.SKU, EditEntity.CNName.Substring(0).ToCharArray()[0]);
                    }
                    bindingSourceList.Add(ppg);
                }
            }
            else
            {
                //如果组合少了，则删除？
                if (newMix.Count < oldMix.Count)
                {
                    var Item差集 = oldMix.Except(newMix);
                    foreach (var item in Item差集)
                    {
                        //更新删除
                        Eav_ProdDetails ep = propGroups.Cast<Eav_ProdDetails>().FirstOrDefault(w => w.GroupName == item.Trim());
                        bindingSourceList.Remove(ep);
                        //将删除的sku行 暂时加入一个临时列表中
                        removeSkuList.Add(ep);
                    }
                }
                else
                {
                    var Item差集 = newMix.Except(oldMix);
                    foreach (var item in Item差集)
                    {
                        Eav_ProdDetails ppg = new Eav_ProdDetails();
                        ppg.GroupName = item;
                        ppg.SKU = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.SKU_No);
                        bindingSourceList.Add(ppg);

                    }
                }

            }
        }



        private void CombinationProperties(KryptonDataGridView dg)
        {
            //属性组合+产品详情 
            Type combinedType = Common.EmitHelper.MergeTypes(true, typeof(Eav_ProdDetails), typeof(tb_ProdDetail));
            object groupdg = Activator.CreateInstance(combinedType);
            dg.DataSource = groupdg;
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

            // tableLayoutPanel1.Controls.Remove(ucskulist);

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



        /// <summary>
        /// 用来保存外键表名与外键主键列名  通过这个打到对应的名称。
        /// </summary>
        // public static ConcurrentDictionary<string, string> FKValueColNameTableNameList = new ConcurrentDictionary<string, string>();


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

            //这里是不是与那个缓存 初始化时的那个字段重复？
            ///显示列表对应的中文
            FieldNameList = UIHelper.GetFieldNameList<T>();

            //重构？
            dataGridView1.XmlFileName = tableName;
            this.dataGridView1.FieldNameList = UIHelper.GetFieldNameColList(typeof(T));
        }


        //三级 还是两级呢。  反向来 一是 KEY VALUE  然后是列名
        ConcurrentDictionary<string, List<KeyValuePair<object, string>>> _DataDictionary = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();

        /// <summary>
        /// 固定的值显示，入库ture 出库false
        /// 每个列表对应的值 ，单独设置
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ColNameDataDictionary { get => _DataDictionary; set => _DataDictionary = value; }



        /// <summary>
        /// 动态字典值显示 外键才有显示
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        //protected virtual string ShowDataGridViewColumnsNameValue(int columnIndex, object value)
        //{
        //    string NameValue = string.Empty;
        //    string tableName = string.Empty;
        //    if (FKValueColNameTableNameList.TryGetValue(dataGridView1.Columns[columnIndex].Name, out tableName))
        //    {
        //        string ValueDisplayColName = string.Empty;
        //        var obj = CacheHelper.Instance.GetValue(tableName, value);
        //        NameValue = obj.ToString();

        //    }
        //    return NameValue;
        //}

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null)
            {
                e.Value = "";
                return;
            }
            //固定字典值显示
            string colDbName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (ColNameDataDictionary.ContainsKey(colDbName))
            {
                List<KeyValuePair<object, string>> kvlist = new List<KeyValuePair<object, string>>();
                //意思是通过列名找，再通过值找到对应的文本
                ColNameDataDictionary.TryGetValue(colDbName, out kvlist);
                if (kvlist != null)
                {
                    KeyValuePair<object, string> kv = kvlist.FirstOrDefault(t => t.Key.ToString().ToLower() == e.Value.ToString().ToLower());
                    if (kv.Value != null)
                    {
                        e.Value = kv.Value;
                    }

                }
            }



            //动态字典值显示
            string colName = UIHelper.ShowGridColumnsNameValue<tb_ProdDetail>(colDbName, e.Value);
            if (!string.IsNullOrEmpty(colName))
            {
                e.Value = colName;
            }

            //图片特殊处理
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Image")
            {
                if (e.Value != null)
                {
                    if (((byte[])e.Value).Length == 0)
                    {
                        e.Value = null;
                        return;
                    }
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    Image image = Image.FromStream(buf, true);
                    e.Value = image;
                    //这里用缓存
                }
            }


        }



        private void BindingSourceList_ListChangedSku(object sender, ListChangedEventArgs e)
        {
            Eav_ProdDetails entity = new Eav_ProdDetails();
            switch (e.ListChangedType)
            {
                case ListChangedType.Reset:
                    break;
                case ListChangedType.ItemAdded:
                    entity = bindingSourceList.List[e.NewIndex] as Eav_ProdDetails;
                    if (entity.ActionStatus != ActionStatus.加载)
                    {
                        entity.ActionStatus = ActionStatus.新增;
                    }
                    break;
                case ListChangedType.ItemDeleted:
                    if (e.NewIndex < bindingSourceList.Count)
                    {
                        entity = bindingSourceList.List[e.NewIndex] as Eav_ProdDetails;
                        entity.ActionStatus = ActionStatus.删除;
                    }
                    break;
                case ListChangedType.ItemMoved:
                    break;
                case ListChangedType.ItemChanged:
                    entity = bindingSourceList.List[e.NewIndex] as Eav_ProdDetails;
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

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
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
        private System.Collections.Concurrent.ConcurrentDictionary<string, string> fieldNameList;

        /// <summary>
        /// 表列名的中文描述集合
        /// </summary>
        [Description("表列名的中文描述集合"), Category("自定属性"), Browsable(true)]
        public System.Collections.Concurrent.ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                return fieldNameList;
            }
            set
            {
                fieldNameList = value;
            }

        }


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
        /// 是否为编辑 如果为是则true
        /// </summary>
        public bool Edited
        {
            get { return editflag; }
            set { editflag = value; }
        }




        /// <summary>
        /// 初始化列表数据
        /// </summary>
        internal void InitListData()
        {
            this.dataGridView1.DataSource = null;

            ListDataSoure = bindingSourceList;

            this.dataGridView1.DataSource = ListDataSoure.DataSource;
        }






        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

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

    }
}
