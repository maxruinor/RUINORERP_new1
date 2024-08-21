using MySqlEntity;
using SMTAPI.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using unvell.ReoGrid;
using unvell.ReoGrid.CellTypes;

namespace RUINORERP.UI.ProductEAV
{
    //尝试只维护datatable表 kvs去掉
    //表中 也保存 各 optinID valueID sheet 隐藏
    //调整列顺序 ，列排序从x开始  
    //myDt.Columns["num"].SetOrdinal(3); 

    public partial class frmProductEAVEdit : Form
    {



        public frmProductEAVEdit()
        {
            InitializeComponent();
            string filePath = Application.StartupPath + "\\demo.htm";
            this.webBrowser1.Url = new Uri(filePath);
        }



        public internalproductEntity entity = new internalproductEntity();



        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (!DesignMode)
            {
                // 调用JavaScript的messageBox方法，并传入参数
                object[] objects = new object[1];
                objects[0] = entity.ProductDesc;
                this.webBrowser1.Document.InvokeScript("SetText", objects);

                //    this.webBrowser1.Document.InvokeScript("SetText", new string[] { ((tbProductEntity)this.bindingSource1.Current).Description.ToString() });
                // webBrowser1.Document.GetElementById("content1").InnerText = "12313232";
                //  HtmlElementCollection txtarea = webBrowser1.Document.GetElementsByTagName("TEXTAREA");
            }
        }

        List<productsoptionsvaluesEntity> OVlist = new List<productsoptionsvaluesEntity>();


        private void frmProductEAVEdit_Load(object sender, EventArgs e)
        {

            OVlist = new productsoptionsvaluesEntity().GetAll();

            HLH.Lib.Helper.DropDownListHelper.InitDropList<productsunitEntity>(new productsunitEntity().GetAll(), cmbproductUnit, "ID", "UnitName");

            HLH.Lib.Helper.DropDownListHelper.InitDropList<productsunitEntity>(new productsunitEntity().GetAll(), cmbPackedUnit, "ID", "UnitName");

            HLH.Lib.Helper.DropDownListHelper.InitDropList<productsunitEntity>(new productsunitEntity().GetAll(), cmbBatchPackedUnit, "ID", "UnitName");

            HLH.Lib.Helper.DropDownListHelper.InitDropList<producttypeEntity>(new producttypeEntity().GetAll(), cmbProductType, "ID", "TypeName");
            if (txtProductNo.Text.Trim().Length == 0)
            {
                txtProductNo.Text = SMTAPI.Biz.SMTCommonData.GetAutoProductNo();
                BindUI(txtProductNo.Text);
            }
            else
            {
                BindUI(entity.ProductNo);
            }
        }


        private void BindUI(string ProductNo)
        {
            txtProductNo.Text = ProductNo;
            txtSKU.Text = entity.SKU;
            txtProductEnName.Text = entity.ProductName;
            txtProductCNName.Text = entity.ProductNameCN;
            txtProductCode.Text = entity.ProductCode;
            txtTags.Text = entity.Tag;
            txtRemarks.Text = entity.Remarks;
            txtSourcePlatform.Text = entity.SourcePlatform;
            List<productsunitEntity> unitList = new productsunitEntity().GetAll();
            productsunitEntity selectUnit = unitList.Find(delegate (productsunitEntity u) { return u.ID == entity.ProductUnitID; });
            if (selectUnit != null)
            {
                cmbproductUnit.SelectedValue = selectUnit.ID;
            }


            List<producttypeEntity> typeList = new producttypeEntity().GetAll();
            producttypeEntity selectType = typeList.Find(delegate (producttypeEntity u) { return u.ID == entity.ProductTypeID; });
            if (selectType != null)
            {
                cmbProductType.SelectedValue = selectType.ID;

                #region 产品属性时 不同处理方式

                EnumProductType pt = (EnumProductType)selectType.ID;
                switch (pt)
                {
                    case EnumProductType.单属性:
                        #region 提取单属性值时的数据

                        // List<productattributeEntity> attrList = new List<productattributeEntity>();
                        //attrList = new productattributeEntity().GetAllByQuery(" ProductID= " + productID);
                        productattributeEntity proAttr = new productattributeEntity().GetByNaturalKey("ProductID", entity.ID.ToString());
                        if (proAttr != null)
                        {
                            chk是否打包销售.Checked = proAttr.IsPackToSell;
                            txtPackSellQty.Text = proAttr.PackSellQty.Value.ToString();
                            productsunitEntity packedUnit = unitList.Find(delegate (productsunitEntity u) { return u.ID == proAttr.PackedUnitID; });
                            if (packedUnit != null)
                            {
                                cmbPackedUnit.SelectedValue = packedUnit.ID;
                            }

                            txtPackBackUp.Text = proAttr.PackBackUp;

                            if (proAttr.PackWeight.HasValue)
                            {
                                txtPackWeight.Text = proAttr.PackWeight.Value.ToString();
                            }


                            txtPackQty.Text = proAttr.PackQty.ToString();

                            if (proAttr.Stock.HasValue)
                            {
                                txtBatchStock.Text = proAttr.Stock.Value.ToString();
                            }

                            if (proAttr.SellPrice.HasValue)
                            {
                                txtSellPrice.Text = proAttr.SellPrice.Value.ToString();
                            }

                            if (!string.IsNullOrEmpty(proAttr.Images))
                            {
                                pictureBox单属性图片.ImageLocation = System.IO.Path.Combine(MultiUser.Instance.ProductLibraryImagesPath, proAttr.Images);
                            }
                            if (proAttr.PurchasePrice.HasValue)
                            {
                                txtPurchasePrice.Text = proAttr.PurchasePrice.Value.ToString();
                            }
                        }

                        #endregion
                        break;
                    case EnumProductType.可配置多属性:
                        LoadAttrValuesByUC(entity.ID);
                        int counterForUC = 0;
                        foreach (Control cc in flowLayoutPanel多属性.Controls)
                        {
                            if (cc.Visible && cc is UCMultiAttributes)
                            {
                                counterForUC++;
                            }
                        }
                        groupBox多属性配置.Text = string.Format("多属性设置({0})", counterForUC);
                        break;
                    case EnumProductType.捆绑:
                        break;
                    case EnumProductType.虚拟:
                        break;
                    default:
                        break;
                }

                #endregion

            }
            if (entity.CategorieId.HasValue)
            {

                productscategoryEntity sc = new productscategoryEntity();
                sc = sc.GetById(entity.CategorieId.Value);
                txtcategoryId.Text = sc.ID + "|" + sc.CategoryCNName + "|" + sc.CategoryENName; ;
                txtcategoryId.Tag = sc.ID;
                txtcategoryId.Name = sc.CategoryENName;
            }
            //载入图片  先copy到临时文件夹下，再载入
            flowLayoutPanel1.Controls.Clear();
            if (entity.ProductsImages != null)
            {
                string[] images = null;
                //new char[] { '#', '|', '|', '#'
                if (entity.ProductsImages.Contains(";"))
                {
                    images = entity.ProductsImages.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    images = entity.ProductsImages.Split(new char[] { '#', '|', '|', '#' }, StringSplitOptions.RemoveEmptyEntries);
                }

                List<string> imagesTemp = new List<string>();
                foreach (string item in images)
                {
                    imagesTemp.Add(System.IO.Path.Combine(MultiUser.Instance.ProductLibraryImagesPath, item));
                }
                LoadImagesForTemp(imagesTemp.ToArray(), false);
            }

        }



        private void btnOK_Click(object sender, EventArgs e)
        {
            internalproductEntity entity = new internalproductEntity();
            entity = entity.GetByNaturalKey("ProductNo", txtProductNo.Text);
            if (entity == null)
            {
                entity = new internalproductEntity();
                entity.ProductNo = txtProductNo.Text;
                entity.CreateDate = System.DateTime.Now;
            }
            else
            {
                //判断是不是有修改产品类型，这个在编辑时，不能够让他修改。不然太复杂。
                if (entity.ProductTypeID.ToString() != cmbProductType.SelectedValue.ToString())
                {
                    MessageBox.Show("产品编辑时，产品类型不可以修改。请重试", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                entity.ModifyDate = System.DateTime.Now;
            }

            EnumProductType pt = (EnumProductType)int.Parse(cmbProductType.SelectedValue.ToString());
            switch (pt)
            {
                case EnumProductType.单属性:
                    #region 保存单属性值时的数据

                    #endregion
                    break;
                case EnumProductType.可配置多属性:
                    //多属性时，如果没有配置属性 或 SKU为空 或其它值无效，则无法保存
                    int contrls = 0;
                    foreach (Control item in flowLayoutPanel多属性.Controls)
                    {
                        if (item.Visible)
                        {
                            contrls++;
                        }
                    }
                    if (contrls == 0)
                    {
                        MessageBox.Show("请选择要配置的属性。");
                        return;
                    }
                    break;
                case EnumProductType.捆绑:
                    break;
                case EnumProductType.虚拟:
                    break;
                default:
                    break;
            }

            entity.ProductName = txtProductEnName.Text;
            entity.ProductNameCN = txtProductCNName.Text;
            entity.ProductCode = txtProductCode.Text;
            entity.SKU = txtSKU.Text;
            entity.Remarks = txtRemarks.Text;
            entity.SourcePlatform = txtSourcePlatform.Text;
            if (cmbproductUnit.SelectedItem != null && cmbproductUnit.SelectedItem is productsunitEntity)
            {
                entity.ProductUnitID = (cmbproductUnit.SelectedItem as productsunitEntity).ID;
            }




            entity.Tag = txtTags.Text;
            entity.ProductDesc = webBrowser1.Document.GetElementById("content1").InnerText;
            if (txtcategoryId.Text.Trim().Length > 0)
            {
                if (txtcategoryId.Text.Contains("|"))
                {
                    entity.CategorieId = int.Parse(txtcategoryId.Text.Substring(0, txtcategoryId.Text.IndexOf("|")));
                }
                else
                {
                    entity.CategorieId = int.Parse(txtcategoryId.Text);
                }
            }
            if (cmbProductType.SelectedItem != null && cmbProductType.SelectedItem is producttypeEntity)
            {
                entity.ProductTypeID = (cmbProductType.SelectedItem as producttypeEntity).ID;
            }


            try
            {
                #region 处理商品图片

                //保存商品图片 路径规则为 原始文件名+guid去-号+GUID
                //编辑时不需要处理，新增或托进来的图片需要处理，这里判断文件名后面是否有GUID四个字母
                //没有选择主图情况下。按顺序生成

                string productImages = string.Empty;

                foreach (System.Windows.Forms.Control var in flowLayoutPanel1.Controls)
                {
                    PictureBox pb = var as PictureBox;
                    //如果为编码 图片不需要处理 则这里需要标记
                    if (pb.Tag != null && bool.Parse(pb.Tag.ToString()))
                    {
                        System.IO.FileInfo fi = new FileInfo(pb.ImageLocation);
                        if (fi.Exists)
                        {
                            if (!fi.ToString().Replace(fi.Extension, "").EndsWith("GUID"))
                            {
                                string TargerDir = MultiUser.Instance.ProductLibraryImagesPath;
                                if (string.IsNullOrEmpty(TargerDir))
                                {
                                    frmMain.Instance.PrintInfoLog("当前上传用户下的产品上传路径没有设置。", Color.Red);
                                }
                                //复制 重新命名时，用类目名 空格用_ 代替， GUID 缩小长度
                                string NewName = string.Empty;
                                int flagIndex = txtcategoryId.Name.IndexOf("|");

                                if (flagIndex > -1)
                                {
                                    NewName = txtcategoryId.Name.Substring(flagIndex + 1);
                                    NewName = NewName.Replace(" ", "_");
                                }

                                string newPath = System.IO.Path.Combine(TargerDir, DateTime.Parse(entity.CreateDate.ToString()).ToString("yyyyMM") + "\\" + NewName + Guid.NewGuid().ToString("n") + "GUID" + fi.Extension);
                                System.IO.FileInfo newfi = new FileInfo(newPath);
                                if (!System.IO.Directory.Exists(newfi.DirectoryName))
                                {
                                    System.IO.Directory.CreateDirectory(newfi.DirectoryName);
                                }
                                System.IO.File.Copy(pb.ImageLocation, newPath, true);

                                //如果保存成功,防止再次保存时，多次处理图片，因为可能是从旧面拉过去的。路径还是原始的桌面上的，但是数据中已经处理了。另存为了
                                //这里更新为处理后的。将不会重复处理了
                                pb.ImageLocation = newPath;
                                productImages += pb.ImageLocation.Replace(MultiUser.Instance.ProductLibraryImagesPath + "\\", "") + "#||#";
                            }
                            else
                            {
                                //编辑的 只是不复制和 重命名而已
                                productImages += pb.ImageLocation.Replace(MultiUser.Instance.ProductLibraryImagesPath + "\\", "") + "#||#";
                            }

                        }
                        else
                        {
                            frmMain.Instance.PrintInfoLog(entity.ProductName + "商品" + entity.ProductNo.ToString() + "的商品图片不存在:" + fi.Name);
                        }
                    }
                    else
                    {
                        //编辑的 只是不复制和 重命名而已 这里要去掉公共目录
                        productImages += pb.ImageLocation.Replace(MultiUser.Instance.ProductLibraryImagesPath + "\\", "") + "#||#";
                    }
                }
                // productImages = productImages.TrimEnd(new char[] { ';' });
                productImages = productImages.TrimEnd(new char[] { '#', '|', '|', '#' });
                //if (productImages.Trim().Length > 10)
                //{
                entity.ProductsImages = productImages;
                //}
                // entity.v_products_image = productImages.Replace("\\", "/");

                #endregion
            }
            catch (Exception ex)
            {
                frmMain.Instance.PrintInfoLog("产品图片处理失败。" + ex.Message + "|" + ex.Source);
            }
            entity.SaveOrUpdate(entity);

            //保存扩展属性
            if (cmbProductType.SelectedItem != null && cmbProductType.SelectedItem is producttypeEntity)
            {
                #region 处理产品类型

                switch (pt)
                {
                    case EnumProductType.单属性:
                        #region 保存单属性值时的数据

                        productattributeEntity att = new productattributeEntity();
                        //保存前清除旧数据 数据复杂。暂时不做更新处理
                        // att.Delete("  delete from [product_attribute] where ProductID=" + productID);
                        att.OptionValueRelationshipText = "";
                        att.SKU = txtProductNo.Text;//单属性时 SKU 等于产品编码
                        //是不是要更新
                        att = att.GetByNaturalKey("SKU", att.SKU);
                        if (att == null)
                        {
                            att = new productattributeEntity();
                            att.OptionValueRelationshipText = "";
                            att.SKU = txtProductNo.Text;
                        }

                        int tempStock = 0;
                        if (int.TryParse(txtBatchStock.Text, out tempStock))
                        {
                            att.Stock = tempStock;
                        }
                        decimal tempSellPrice = 0;
                        if (decimal.TryParse(txtSellPrice.Text, out tempSellPrice))
                        {
                            att.SellPrice = tempSellPrice;
                        }

                        decimal tempPurchasePrice = 0;
                        if (decimal.TryParse(txtPurchasePrice.Text, out tempPurchasePrice))
                        {
                            att.PurchasePrice = tempPurchasePrice;
                        }



                        //目前只对应一个SKU时，一个图片
                        string productImages = string.Empty;
                        if (!string.IsNullOrEmpty(att.Images))
                        {
                            try
                            {
                                #region 处理商品图片
                                //保存商品图片 路径规则为 原始文件名+guid去-号+GUID
                                //编辑时不需要处理，新增或托进来的图片需要处理，这里判断文件名后面是否有GUID四个字母
                                //没有选择主图情况下。按顺序生成
                                System.IO.FileInfo fi = new FileInfo(pictureBox单属性图片.ImageLocation);
                                if (fi.Exists)
                                {
                                    if (!fi.ToString().Replace(fi.Extension, "").EndsWith("GUID"))
                                    {
                                        string TargerDir = MultiUser.Instance.ProductLibraryImagesPath;
                                        if (string.IsNullOrEmpty(TargerDir))
                                        {
                                            frmMain.Instance.PrintInfoLog("当前上传用户下的产品上传路径没有设置。", Color.Red);
                                        }
                                        //复制
                                        string newPath = System.IO.Path.Combine(TargerDir, DateTime.Parse(entity.CreateDate.ToString()).ToString("yyyyMM") + "\\" + fi.Name.Replace(fi.Extension, Guid.NewGuid().ToString("n") + "GUID" + fi.Extension));
                                        System.IO.FileInfo newfi = new FileInfo(newPath);
                                        if (!System.IO.Directory.Exists(newfi.DirectoryName))
                                        {
                                            System.IO.Directory.CreateDirectory(newfi.DirectoryName);
                                        }
                                        System.IO.File.Copy(pictureBox单属性图片.ImageLocation, newPath, true);

                                        //如果保存成功,防止再次保存时，多次处理图片，因为可能是从旧面拉过去的。路径还是原始的桌面上的，但是数据中已经处理了。另存为了
                                        //这里更新为处理后的。将不会重复处理了
                                        pictureBox单属性图片.ImageLocation = newPath;
                                        productImages = pictureBox单属性图片.ImageLocation.Replace(MultiUser.Instance.ProductLibraryImagesPath, "");
                                    }
                                    else
                                    {
                                        //编辑的 只是不复制和 重命名而已
                                        productImages = pictureBox单属性图片.ImageLocation.Replace(MultiUser.Instance.ProductLibraryImagesPath, "");
                                    }

                                }
                                else
                                {
                                    frmMain.Instance.PrintInfoLog(entity.ProductName + "商品" + entity.ProductNo.ToString() + "的商品图片不存在:" + fi.Name);
                                }

                                #endregion
                            }
                            catch (Exception ex)
                            {
                                frmMain.Instance.PrintInfoLog("产品图片处理失败。" + ex.Message + "|" + ex.Source);
                            }
                        }
                        att.Images = productImages;


                        att.ProductID = entity.ID;
                        att.IsPackToSell = chk是否打包销售.Checked;
                        int tempPackQty = 0;
                        if (int.TryParse(txtPackQty.Text, out tempPackQty))
                        {
                            att.PackQty = tempPackQty;
                        }

                        int tempPackSellQty = 0;
                        if (int.TryParse(txtPackSellQty.Text, out tempPackSellQty) && chk是否打包销售.Checked)
                        {
                            att.PackSellQty = tempPackSellQty;
                        }
                        else
                        {
                            att.PackSellQty = 1;
                        }

                        att.IsPackToSell = chk是否打包销售.Checked;
                        att.PackBackUp = txtPackBackUp.Text;
                        if (cmbPackedUnit.SelectedItem != null && cmbPackedUnit.SelectedItem is productsunitEntity)
                        {
                            att.PackedUnitID = (cmbPackedUnit.SelectedItem as productsunitEntity).ID;
                        }
                        int tempPackWeight = 0;
                        if (int.TryParse(txtPackWeight.Text, out tempPackWeight))
                        {
                            att.PackWeight = tempPackWeight;
                        }

                        att.SaveOrUpdate(att);


                        #endregion
                        break;
                    case EnumProductType.可配置多属性:
                        SaveAttributeInfoByUC(entity.ID);
                        break;
                    case EnumProductType.捆绑:
                        break;
                    case EnumProductType.虚拟:
                        break;
                    default:
                        break;
                }


                #endregion
            }



            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        /// <summary>
        /// 保存扩展属性 属性列会动态变化 增加或减少，这里引出另一个表来维护动态多属性
        /// </summary>
        /// <param name="productID"></param>
        private void SaveAttributeInfoByUC(int productID)
        {
            //当多属性的属性个数有变化时，则原来的数据要全部分清空sku也是重新生成的
            //判断方法是 必须有老数据隐藏，一定有新数据产生。
            productattributeEntity attDelete = new productattributeEntity();
            attDelete.Delete("  delete from product_AttrComb where ProductID=" + productID);

            foreach (Control cc in flowLayoutPanel多属性.Controls)
            {

                if (cc.Visible)
                {
                    UCMultiAttributes myuc = cc as UCMultiAttributes;
                    #region 添加行
                    productattributeEntity att = new productattributeEntity();
                    //保存前清除旧数据 数据复杂。暂时不做更新处理
                    // att.Delete("  delete from [product_attribute] where ProductID=" + productID);
                    att.OptionValueRelationshipText = "";
                    att.SKU = myuc.txtSKU.Text;
                    //是不是要更新
                    att = att.GetByNaturalKey("SKU", att.SKU);
                    if (att == null)
                    {
                        att = new productattributeEntity();
                        att.OptionValueRelationshipText = "";
                        att.SKU = myuc.txtSKU.Text;
                    }

                    int tempStock = 0;
                    if (int.TryParse(myuc.txtStock.Text, out tempStock))
                    {
                        att.Stock = tempStock;
                    }
                    decimal tempSellPrice = 0;
                    if (decimal.TryParse(myuc.txtSellPrice.Text, out tempSellPrice))
                    {
                        att.SellPrice = tempSellPrice;
                    }
                    decimal tempPurchasePrice = 0;
                    if (decimal.TryParse(myuc.txtPurchasePrice.Text, out tempPurchasePrice))
                    {
                        att.PurchasePrice = tempPurchasePrice;
                    }
                    //目前只对应一个SKU时，一个图片
                    string productImages = string.Empty;
                    if (!string.IsNullOrEmpty(myuc.pictureBoxsku.ImageLocation))
                    {
                        try
                        {
                            #region 处理商品图片
                            //保存商品图片 路径规则为 原始文件名+guid去-号+GUID
                            //编辑时不需要处理，新增或托进来的图片需要处理，这里判断文件名后面是否有GUID四个字母
                            //没有选择主图情况下。按顺序生成
                            System.IO.FileInfo fi = new FileInfo(myuc.pictureBoxsku.ImageLocation);
                            if (fi.Exists)
                            {
                                if (!fi.ToString().Replace(fi.Extension, "").EndsWith("GUID"))
                                {
                                    string TargerDir = MultiUser.Instance.ProductLibraryImagesPath;
                                    if (string.IsNullOrEmpty(TargerDir))
                                    {
                                        frmMain.Instance.PrintInfoLog("当前上传用户下的产品上传路径没有设置。", Color.Red);
                                    }
                                    //复制
                                    string newPath = System.IO.Path.Combine(TargerDir, DateTime.Parse(entity.CreateDate.ToString()).ToString("yyyyMM") + "\\" + fi.Name.Replace(fi.Extension, Guid.NewGuid().ToString("n") + "GUID" + fi.Extension));
                                    System.IO.FileInfo newfi = new FileInfo(newPath);
                                    if (!System.IO.Directory.Exists(newfi.DirectoryName))
                                    {
                                        System.IO.Directory.CreateDirectory(newfi.DirectoryName);
                                    }
                                    System.IO.File.Copy(myuc.pictureBoxsku.ImageLocation, newPath, true);

                                    //如果保存成功,防止再次保存时，多次处理图片，因为可能是从旧面拉过去的。路径还是原始的桌面上的，但是数据中已经处理了。另存为了
                                    //这里更新为处理后的。将不会重复处理了
                                    myuc.pictureBoxsku.ImageLocation = newPath;
                                    productImages = myuc.pictureBoxsku.ImageLocation.Replace(MultiUser.Instance.ProductLibraryImagesPath, ""); ;

                                    //如果有旧图片，则判断 直接删除
                                    if (myuc.pictureBoxsku.Tag != null)
                                    {
                                        System.IO.FileInfo fiOld = new FileInfo(myuc.pictureBoxsku.Tag.ToString());
                                        if (fiOld.Exists)
                                        {
                                            System.IO.File.Delete(fiOld.ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    //编辑的 只是不复制和 重命名而已
                                    productImages = myuc.pictureBoxsku.ImageLocation.Replace(MultiUser.Instance.ProductLibraryImagesPath, "");
                                }

                            }
                            else
                            {
                                frmMain.Instance.PrintInfoLog(entity.ProductName + "商品" + entity.ProductNo.ToString() + "的商品图片不存在:" + fi.Name);
                            }

                            #endregion
                        }
                        catch (Exception ex)
                        {
                            frmMain.Instance.PrintInfoLog("产品图片处理失败。" + ex.Message + "|" + ex.Source);
                        }
                    }
                    att.Images = productImages;
                    att.ProductID = productID;
                    att.IsPackToSell = myuc.chk打包销售.Checked;
                    int tempPackQty = 0;
                    if (int.TryParse(myuc.txtPackQty.Text, out tempPackQty))
                    {
                        att.PackQty = tempPackQty;
                    }

                    int tempPackSellQty = 0;
                    if (int.TryParse(myuc.txtPackSellQty.Text, out tempPackSellQty) && myuc.chk打包销售.Checked)
                    {
                        att.PackSellQty = tempPackSellQty;
                    }
                    else
                    {
                        att.PackSellQty = 1;
                    }

                    att.IsPackToSell = myuc.chk打包销售.Checked;
                    att.PackBackUp = myuc.txtPackBackUp.Text;
                    if (myuc.cmbPackedUnit.SelectedItem != null && myuc.cmbPackedUnit.SelectedItem is productsunitEntity)
                    {
                        att.PackedUnitID = (myuc.cmbPackedUnit.SelectedItem as productsunitEntity).ID;
                    }
                    int tempPackWeight = 0;
                    if (int.TryParse(myuc.txtPackWeight.Text, out tempPackWeight))
                    {
                        att.PackWeight = tempPackWeight;
                    }

                    att.SaveOrUpdate(att);
                    #region 保存属性值

                    DataTable dt = myuc.dataGridViewMultiAttributes.DataSource as DataTable;
                    foreach (DataRow dr in dt.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            productAttrCombEntity comb = new productAttrCombEntity();
                            productsoptionsEntity tempOption = (new productsoptionsEntity().GetByNaturalKey("OptionName", dc.ColumnName));
                            //List<productAttrCombEntity> tempList = comb.GetAllByQuery(" [ProductID]=" + productID + " and SKU=" + myuc.txtSKU.Text + " and [OptionID]=" + tempOption.ID);
                            //if (tempList.Count > 0)
                            //{
                            //    comb = tempList[0];
                            //}
                            comb.ProductID = productID;
                            comb.SKU = myuc.txtSKU.Text;
                            comb.OptionID = tempOption.ID;
                            comb.OptionValueID = (new productsoptionsvaluesEntity().GetByNaturalKey("OptionValueName", dr[dc.ColumnName].ToString())).ID;
                            comb.attrID = att.ID;
                            comb.SaveOrUpdate(comb);
                        }

                    }
                    #endregion

                    #endregion

                }
                else
                {
                    //反过来，到达保存 是最后修改机会，如果是隐藏的。则认为不需要的数据。直接删除

                    UCMultiAttributes myuc = cc as UCMultiAttributes;
                    productattributeEntity att = new productattributeEntity();
                    att.SKU = myuc.txtSKU.Text;
                    //是不是要更新
                    att = att.GetByNaturalKey("SKU", att.SKU);
                    if (att != null)
                    {
                        att.Delete(att.ID);
                    }
                }
            }



        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private const byte Ctrl = 8;//表示按住了Ctrl键（类似的还有Alt、Shift、鼠标左键、鼠标右键等键的常数定义，请查看MSDN）
        private const byte Shift = 4;//表示按住了Shift

        private void flowLayoutPanel1_DragDrop(object sender, DragEventArgs e)
        {
            //目标对象
            string[] str = ((string[])e.Data.GetData(DataFormats.FileDrop));
            LoadImagesForTemp(str, true);

        }

        private void flowLayoutPanel1_DragEnter(object sender, DragEventArgs e)
        {
            //选中的对象
            //检查拖动的数据是否适合于目标空间类型；若不适合，则拒绝放置。
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                //检查Ctrl键是否被按住
                if (e.KeyState == Ctrl)
                {
                    //若Ctrl键被按住，则设置拖放类型为拷贝
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    //若Ctrl键没有被按住，则设置拖放类型为移动
                    e.Effect = DragDropEffects.Move;
                }
            }
            else
            {
                //若被拖动的数据不适合于目标控件，则设置拖放类型为拒绝放置
                e.Effect = DragDropEffects.None;
            }
        }


        /// <summary>
        /// 图片上传思路 显示时是不作操作，保存时 将选择中的文件，直接复制到指定文件下 并且用绝对路径保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImagesForProduct_Click(object sender, EventArgs e)
        {
            openFileDialog4Images.Filter = "图片文件(*.jpg)|*.jpg|所有文件(*.*)|*.*";
            if (openFileDialog4Images.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (openFileDialog4Images.FileNames.Length > 0)
                {
                    LoadImagesForTemp(openFileDialog4Images.FileNames, true);
                }

            }
        }

        /// <summary>
        /// 显示临时图片
        /// <param name="isNeedProcess">是否需要处理 如果为新增加的则要复制保存和重新GUID命名，否则为编辑图片不需要处理则为fase</param>
        /// </summary>
        public void LoadImagesForTemp(string[] files, bool isNeedProcess)
        {
            foreach (string file in files)
            {
                //如果不是正常的文件路径，则忽略处理

                if (!System.IO.File.Exists(file))
                {
                    continue;
                }

                System.IO.FileInfo fi = new FileInfo(file);
                if (fi.Extension.ToUpper() == ".JPG")
                {
                    //取表中的列  From Email Address
                    PictureBox pb = new PictureBox();
                    pb.Name = fi.Name;
                    pb.DragDrop += new DragEventHandler(pb_DragDrop);
                    pb.MouseDown += new MouseEventHandler(pb_MouseDown);
                    pb.DragEnter += new DragEventHandler(pb_DragEnter);
                    pb.AllowDrop = true;
                    pb.Width = 100;
                    pb.Height = 100;
                    pb.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.toolTip1.SetToolTip(pb, "同时按住Ctrl键将删除图片,按住Shit则在资源浏览器中打开选中图片。");
                    pb.ContextMenuStrip = contextMenuStrip图片;
                    pb.Tag = isNeedProcess;

                    pb.ImageLocation = file;
                    flowLayoutPanel1.Controls.Add(pb);
                }
            }
        }


        // System.Diagnostics.Process.Start("explorer.exe", "/select," + (sender as PictureBox).ImageLocation.ToString());


        void pb_DragEnter(object sender, DragEventArgs e)
        {
            //选中的对象
            //检查拖动的数据是否适合于目标空间类型；若不适合，则拒绝放置。
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                //检查Ctrl键是否被按住
                if (e.KeyState == Ctrl)
                {
                    //若Ctrl键被按住，则设置拖放类型为拷贝
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    //若Ctrl键没有被按住，则设置拖放类型为移动
                    e.Effect = DragDropEffects.Move;
                }
            }
            else
            {
                //若被拖动的数据不适合于目标控件，则设置拖放类型为拒绝放置
                e.Effect = DragDropEffects.None;
            }
        }

        void pb_MouseDown(object sender, MouseEventArgs e)
        {
            //判断是否按下鼠标左键
            if (e.Button == MouseButtons.Left)
            {

                //从sender对象里取出源事件PictureBox控件
                PictureBox picTemp = sender as PictureBox;
                //aaa
                contextMenuStrip图片.Tag = picTemp;

                //判断事件源对象是否正显示了图片
                if (picTemp.Image != null)
                {
                    //开始拖放操作，并传递要拖动的数据以及拖放的类型（是移动操作还是拷贝操作）
                    picTemp.DoDragDrop(picTemp.Image, DragDropEffects.Move | DragDropEffects.Copy);
                }


            }
        }

        void pb_DragDrop(object sender, DragEventArgs e)
        {
            //目标对象
            //从sender对象里取出目标事件PictureBox控件
            PictureBox picTemp = sender as PictureBox;

            //从事件参数中取出拖动的数据，并转换为图象对象
            //picTemp.Image = e.Data.GetData(DataFormats.Bitmap) as Bitmap;
            //若不是执行的拷贝操作

            //string[] str = ((string[])e.Data.GetData(DataFormats.FileDrop));
            //if (str.Length > 0)
            //{
            //    PictureBox pb = sender as PictureBox;
            //    System.IO.FileInfo fi = new FileInfo(str[0]);
            //    if (fi.Extension.ToUpper() == ".JPG")
            //    {
            //        pb.ImageLocation = str[0];
            //    }
            //}

            //0 表示 没有按其它键  4 shift 8为ctrl

            if (e.KeyState == Shift)
            {
                //string path = string.Empty;
                //PictureBox picold = contextMenuStrip图片.Tag as PictureBox;
                //path = picold.ImageLocation;
                System.Diagnostics.Process.Start("explorer.exe", "/select," + (sender as PictureBox).ImageLocation.ToString());

                frmMain.Instance.PrintInfoLog("正在浏览文件：" + (sender as PictureBox).ImageLocation.ToString());

            }
            if (e.KeyState != Ctrl && e.KeyState != Shift)
            {
                string path = string.Empty;
                PictureBox picold = contextMenuStrip图片.Tag as PictureBox;
                path = picold.ImageLocation;

                picold.ImageLocation = picTemp.ImageLocation;
                picTemp.ImageLocation = path;
            }
            if (e.KeyState == Ctrl)
            {
                //删除图片
                if (picTemp.Tag != null && bool.Parse(picTemp.Tag.ToString()))
                {
                    //第一次新增时 通常手动 则不需要删除，如果是采集 可以删除
                    if (MessageBox.Show("你是否需要真正删除这张图片？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (MessageBox.Show("从硬盘上删除引用的图片？无法恢复。", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                        {
                            System.IO.File.Delete(picTemp.ImageLocation);
                        }
                    }
                    PictureBox picold = contextMenuStrip图片.Tag as PictureBox;
                    flowLayoutPanel1.Controls.Remove(picold);
                }
                else
                {
                    if (MessageBox.Show("你是否需要真正删除这张图片？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                    {
                        System.IO.File.Delete(picTemp.ImageLocation);
                    }
                    PictureBox picold = contextMenuStrip图片.Tag as PictureBox;
                    flowLayoutPanel1.Controls.Remove(picold);
                }
            }
        }

        List<productsunitEntity> unitList = new List<productsunitEntity>();

        private void cmbProductType_SelectedIndexChanged(object sender, EventArgs e)
        {

            EnumProductType pt = (EnumProductType)int.Parse(cmbProductType.SelectedValue.ToString());
            switch (pt)
            {
                case EnumProductType.单属性:
                    btn多属性配置.Visible = false;
                    groupBox多属性配置.Visible = false;

                    groupBox单属性参数.Visible = true;
                    break;
                case EnumProductType.可配置多属性:
                    unitList = new productsunitEntity().GetAll();
                    btn多属性配置.Visible = true;
                    //绑定对应的选项及其值
                    productsoptionsEntity entity = new productsoptionsEntity();
                    List<productsoptionsEntity> listOption = entity.GetAll();
                    HLH.Lib.Helper.DropDownListHelper.InitDropList<productsoptionsEntity>(listOption, cmb属性, "ID", "OptionName");
                    groupBox多属性配置.Visible = true;

                    // grid.Visible = true;

                    groupBox单属性参数.Visible = false;
                    break;
                case EnumProductType.捆绑:
                    break;
                case EnumProductType.虚拟:
                    break;
                default:
                    break;
            }


        }

        private void btnSelectCategory_Click(object sender, EventArgs e)
        {
            frmSelectClassNo sc = new frmSelectClassNo();
            if (sc.ShowDialog() == DialogResult.OK)
            {
                txtcategoryId.Text = sc.ClassNo + "|" + sc.ClassName;
                txtcategoryId.Tag = sc.ClassNo;
                txtcategoryId.Name = sc.ClassName;
            }
        }

        private void btn多属性配置_Click(object sender, EventArgs e)
        {
            frmEavManager eav = new frmEavManager();
            eav.ShowDialog();
        }



        private void cmb属性_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region
            List<string> optionValuesSelect = new List<string>();
            //判断是否已经存在属性
            string modifyIndex = string.Empty;
            //通过UC容器反推
            foreach (Control cc in flowLayoutPanel多属性.Controls)
            {
                //提取属性个数，就是列 只需要提取第一个UC
                if (cc is UCMultiAttributes && cc.Visible == true)
                {
                    UCMultiAttributes ucols = cc as UCMultiAttributes;

                    if (ucols.dataGridViewMultiAttributes.Columns.Contains(cmb属性.Text.ToString()))
                    {
                        //找到修改的列
                        modifyIndex = ucols.dataGridViewMultiAttributes.Columns[cmb属性.Text.ToString()].Index.ToString();
                    }
                }
            }
            #region 从容器中控件DG中取值
            if (!string.IsNullOrEmpty(modifyIndex))
            {
                for (int m = 0; m < flowLayoutPanel多属性.Controls.Count; m++)
                {
                    UCMultiAttributes ucmult = flowLayoutPanel多属性.Controls[m] as UCMultiAttributes;
                    //是去掉重复
                    if (ucmult.Visible && !optionValuesSelect.Contains(ucmult.dataGridViewMultiAttributes[int.Parse(modifyIndex), 0].Value.ToString().Trim()))
                    {
                        optionValuesSelect.Add(ucmult.dataGridViewMultiAttributes[int.Parse(modifyIndex), 0].Value.ToString().Trim());//指定一行。因为每个属性只对应一行
                    }

                }
            }

            #endregion

            List<productsoptionsvaluesEntity> listOptionValue = new productsoptionsvaluesEntity().GetAllByQuery("OptionID=" + cmb属性.SelectedValue.ToString());
            chk全选.Checked = false;
            //HLH.Lib.Helper.DropDownListHelper.InitDropList<productsoptionsvaluesEntity>(listOptionValue, cmb属性值, "ID", "OptionValueName");
            cmbOPvalues.Items.Clear();
            cmbOPvalues.CheckBoxItems.Clear();
            cmbOPvalues.Clear();
            cmbOPvalues.ClearSelection();
            foreach (productsoptionsvaluesEntity item in listOptionValue)
            {
                HLH.Lib.CmbItem it = new HLH.Lib.CmbItem(item.ID.ToString(), item.OptionValueName);
                cmbOPvalues.Items.Add(it);
            }
            for (int i = 0; i < cmbOPvalues.CheckBoxItems.Count; i++)
            {
                cmbOPvalues.CheckBoxItems[i].Checked = false;
            }
            cmbOPvalues.Text = string.Empty;
            foreach (string str in optionValuesSelect)
            {
                for (int i = 0; i < cmbOPvalues.Items.Count; i++)
                {
                    if (cmbOPvalues.CheckBoxItems[str] != null)
                    {
                        cmbOPvalues.CheckBoxItems[str].Checked = true;
                    }

                }
            }




            #endregion
        }





        //List<productsoptionsvaluesEntity> listOptionValue = new productsoptionsvaluesEntity().GetAllByQuery("OptionID=" + cmb属性.SelectedValue.ToString());
        //chk全选.Checked = false;
        ////HLH.Lib.Helper.DropDownListHelper.InitDropList<productsoptionsvaluesEntity>(listOptionValue, cmb属性值, "ID", "OptionValueName");
        //cmbOPvalues.Items.Clear();
        //foreach (productsoptionsvaluesEntity item in listOptionValue)
        //{
        //    HLH.Lib.CmbItem it = new HLH.Lib.CmbItem(item.ID.ToString(), item.OptionValueName);
        //    cmbOPvalues.Items.Add(it);
        //}


        //foreach (DataRow dr in DtOptionValues.Rows)
        //{
        //    for (int i = 0; i < cmbOPvalues.Items.Count; i++)
        //    {
        //        if (DtOptionValues.Columns.Contains(cmb属性.Text.ToString()) && cmbOPvalues.Items[i].ToString() == dr[cmb属性.Text.ToString()].ToString())
        //        {
        //            cmbOPvalues.CheckBoxItems[dr[cmb属性.Text.ToString()].ToString()].Checked = true;
        //        }

        //    }
        //}


        private void cmbOPvalues_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbOPvalues_CheckBoxCheckedChanged(object sender, EventArgs e)
        {
            //foreach (Control item in panel属性配置区.Controls)
            //{
            //    //手动选择时 
            //    if (cmbOPvalues.SelectedItem != null && cmbOPvalues.SelectedItem.ToString() != "")
            //    {
            //        if (cmbOPvalues.SelectedItem.ToString().Contains("white"))
            //        {
            //            //  isShowForCountry = true;
            //        }
            //        else
            //        {
            //            //   isShowForCountry = false;
            //        }

            //    }
            //    else //程序 是否全选
            //    {
            //        for (int i = 0; i < cmbOPvalues.Items.Count; i++)
            //        {
            //            if (!string.IsNullOrEmpty(cmbOPvalues.Items[i].ToString()))
            //            {
            //                //     isShowForCountry = cmbOPvalues.CheckBoxItems[i].Checked;
            //            }
            //        }
            //    }
            //    //oif.Visible = isShowForlogistic && isShowForCountry;
            //    //oif.chkPrint.Checked = isShowForlogistic && isShowForCountry; ;

            //}



        }

        private void LoadAttrValuesByUC(int productID)
        {
            #region 添加属性列

            //从数据库中取用到的属性
            List<productsoptionsEntity> options = new List<productsoptionsEntity>();
            options = new productsoptionsEntity().GetAllByQuery(" ID in ( select distinct OptionID from product_AttrComb where ProductID=" + productID + " ) ");



            #endregion


            #region 组合内存表数据结构

            //准备选择值 用于下面通过 ID来得到字面值
            List<productsoptionsvaluesEntity> optionValuesList = new List<productsoptionsvaluesEntity>();
            optionValuesList = new productsoptionsvaluesEntity().GetAll();

            List<productattributeEntity> attrList = new List<productattributeEntity>();

            attrList = new productattributeEntity().GetAllByQuery(" ProductID= " + productID);

            List<productAttrCombEntity> attrCombList = new List<productAttrCombEntity>();
            attrCombList = new productAttrCombEntity().GetAllByQuery(" ProductID= " + productID);

            List<productsunitEntity> unitList = new productsunitEntity().GetAll();


            flowLayoutPanel多属性.Controls.Clear();
            foreach (productattributeEntity item in attrList)
            {
                #region 添加UC控件 相当于行
                UCMultiAttributes ucm = new UCMultiAttributes();
                #region 添加UC
                ucm.Visible = true;
                ucm.txtSKU.Text = item.SKU;
                if (item.SellPrice.HasValue)
                {
                    ucm.txtSellPrice.Text = item.SellPrice.Value.ToString();
                }

                if (item.Stock.HasValue)
                {
                    ucm.txtStock.Text = item.Stock.Value.ToString();
                }
                if (item.SellPrice.HasValue)
                {
                    ucm.txtSellPrice.Text = item.SellPrice.Value.ToString();
                }
                if (item.PackWeight.HasValue)
                {
                    ucm.txtPackWeight.Text = item.PackWeight.Value.ToString();
                }
                if (item.PackSellQty.HasValue)
                {
                    ucm.txtPackSellQty.Text = item.PackSellQty.Value.ToString();
                }

                if (item.PurchasePrice.HasValue)
                {
                    ucm.txtPurchasePrice.Text = item.PurchasePrice.Value.ToString();
                }

                ucm.txtPackQty.Text = item.PackQty.ToString();

                ucm.txtPackBackUp.Text = item.PackBackUp;

                if (System.IO.File.Exists(System.IO.Path.Combine(MultiUser.Instance.ProductLibraryImagesPath, item.Images)))
                {
                    ucm.pictureBoxsku.ImageLocation = System.IO.Path.Combine(MultiUser.Instance.ProductLibraryImagesPath, item.Images);
                    //备份保存图片路径到tag，为了删除或修改时 删除图片，减少冗余数据
                    ucm.pictureBoxsku.Tag = ucm.pictureBoxsku.ImageLocation;
                }


                ucm.cmbPackedUnit.Items.Clear();
                HLH.Lib.Helper.DropDownListHelper.InitDropList<productsunitEntity>(unitList, ucm.cmbPackedUnit, "ID", "UnitName");

                productsunitEntity selectUnit = unitList.Find(delegate (productsunitEntity u) { return u.ID == item.PackedUnitID; });
                if (selectUnit != null)
                {
                    ucm.cmbPackedUnit.SelectedValue = selectUnit.ID;
                }

                #endregion

                DataTable dtOV = new DataTable();

                for (int i = 0; i < options.Count; i++)
                {
                    if (!dtOV.Columns.Contains(options[i].OptionName))
                    {
                        DataColumn dcNew = new DataColumn();
                        dcNew.ColumnName = options[i].OptionName;
                        dtOV.Columns.Add(dcNew);

                    }
                }

                #region 添加行
                foreach (productattributeEntity drattr in attrList)
                {
                    if (drattr.SKU == item.SKU)
                    {
                        DataRow dr = dtOV.NewRow();
                        foreach (DataColumn dc in dtOV.Columns)
                        {
                            productsoptionsEntity option = options.Find(delegate (productsoptionsEntity p) { return p.OptionName == dc.ColumnName; });
                            productAttrCombEntity AttrComb = attrCombList.Find(delegate (productAttrCombEntity comb) { return comb.OptionID == option.ID && comb.SKU == item.SKU; });
                            if (AttrComb != null)
                            {
                                productsoptionsvaluesEntity optionValue = optionValuesList.Find(delegate (productsoptionsvaluesEntity v) { return v.ID == AttrComb.OptionValueID; });
                                dr[dc.ColumnName] = optionValue.OptionValueName;
                            }

                        }
                        dtOV.Rows.Add(dr);
                        string ucName = string.Empty;
                        foreach (object obj in dr.ItemArray)
                        {
                            ucName += obj.ToString() + ",";
                        }
                        ucName = ucName.TrimEnd(',');
                        ucm.Name = ucName;
                    }
                }

                #endregion

                ucm.dataGridViewMultiAttributes.DataSource = dtOV;
                ucm.pictureBoxsku.DoubleClick -= pictureBoxsku_DoubleClick;
                ucm.pictureBoxsku.DoubleClick += pictureBoxsku_DoubleClick;
                ucm.chk打包销售.Tag = ucm;//打包销售选择时，通过这个来控制另一个txtbox
                ucm.chk打包销售.CheckedChanged -= chk打包销售_CheckedChanged;
                ucm.chk打包销售.CheckedChanged += chk打包销售_CheckedChanged;
                ContextMenuStrip cms = new System.Windows.Forms.ContextMenuStrip();
                cms.Items.Add("浏览图片");
                cms.Items[0].Click += cms_Click;
                cms.Items[0].Tag = ucm.pictureBoxsku;
                //cms.Tag = ucm.pictureBoxsku;
                ucm.pictureBoxsku.ContextMenuStrip = cms;
                flowLayoutPanel多属性.Controls.Add(ucm);

                #endregion
            }

            #endregion


        }






        void frmProductEAVEdit_Click(object sender, EventArgs e)
        {
            openFileDialog4Images.Filter = "图片文件(*.jpg)|*.jpg|所有文件(*.*)|*.*";

            if (openFileDialog4Images.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //LoadImagesForTemp(openFileDialog4Images.FileNames, true);
                foreach (string str in openFileDialog4Images.FileNames)
                {
                    ButtonCell bc = sender as unvell.ReoGrid.CellTypes.ButtonCell;
                    bc.Cell.Tag = str + ",";
                    bc.Cell.Style.TextColor = Color.Blue;

                }
            }
        }




        private void chk全选_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < cmbOPvalues.Items.Count; i++)
            {
                if (!string.IsNullOrEmpty(cmbOPvalues.Items[i].ToString()))
                {
                    cmbOPvalues.CheckBoxItems[i].Checked = chk全选.Checked;
                }
            }

        }





        private void btnAddOptionValues_Click(object sender, EventArgs e)
        {
            //只维护表
            AddattrByUC(new productsoptionsEntity().GetById(int.Parse(cmb属性.SelectedValue.ToString())), cmbOPvalues.Text);
            // AddattrByDV();
            int counterForUC = 0;
            foreach (Control cc in flowLayoutPanel多属性.Controls)
            {
                if (cc.Visible && cc is UCMultiAttributes)
                {
                    counterForUC++;
                }
            }
            groupBox多属性配置.Text = string.Format("多属性设置({0})", counterForUC);
        }


        private void AddattrByUC(productsoptionsEntity option, string optionValues)
        {
            if (flowLayoutPanel多属性.Controls.Count == 0 && optionValues.Trim().Length == 0)
            {
                MessageBox.Show("请选择要添加的属性值。");
                return;
            }
            ///原来属性数
            int OldOptionCount = 0;
            string OptionNames = "";
            #region 绑定到控件

            List<List<string>> para = new List<List<string>>();

            #region 添加维护数据
            //如果容器为空 则为第一次添加
            if (flowLayoutPanel多属性.Controls.Count > 0)
            {
                #region 从容器中还原之前的选项值

                //提取属性个数，就是列 只需要提取第一个UC 可见的
                UCMultiAttributes ucols = new UCMultiAttributes();
                foreach (Control item in flowLayoutPanel多属性.Controls)
                {
                    if (item.Visible)
                    {
                        ucols = item as UCMultiAttributes;
                        break;
                    }
                }
                OldOptionCount = ucols.dataGridViewMultiAttributes.Columns.Count;

                //判断加入的是不是 已经存在属性，只是修改了属性值
                string modifyIndex = string.Empty;
                if (ucols.dataGridViewMultiAttributes.Columns.Contains(option.OptionName))
                {
                    //找到修改的列
                    modifyIndex = ucols.dataGridViewMultiAttributes.Columns[option.OptionName].Index.ToString();
                }
                else
                {
                    if (optionValues.Trim().Length == 0)
                    {
                        MessageBox.Show("请选择要添加的属性值。");
                        return;
                    }
                }
                for (int i = 0; i < ucols.dataGridViewMultiAttributes.Columns.Count; i++)
                {
                    List<string> oldItems = new List<string>();
                    OptionNames += ucols.dataGridViewMultiAttributes.Columns[i].Name + ",";
                    #region 从添加行的属性中取，这个时候 是修改模式

                    if (!string.IsNullOrEmpty(modifyIndex) && int.Parse(modifyIndex) == i)
                    {
                        if (optionValues.Trim().Length == 0)
                        {
                            OptionNames = OptionNames.Replace(ucols.dataGridViewMultiAttributes.Columns[i].Name + ",", "");
                        }
                        string[] tempsz = optionValues.Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                        foreach (string str in tempsz)
                        {
                            if (!oldItems.Contains(str))
                            {
                                oldItems.Add(str);//指定一行。因为每个属性只对应一行
                            }
                        }

                    }
                    else
                    {
                        #region 从容器中控件DG中取值

                        for (int m = 0; m < flowLayoutPanel多属性.Controls.Count; m++)
                        {
                            UCMultiAttributes ucmult = flowLayoutPanel多属性.Controls[m] as UCMultiAttributes;
                            //是去掉重复
                            if (ucmult.Visible && !oldItems.Contains(ucmult.dataGridViewMultiAttributes[i, 0].Value.ToString()))
                            {
                                oldItems.Add(ucmult.dataGridViewMultiAttributes[i, 0].Value.ToString());//指定一行。因为每个属性只对应一行
                            }

                        }
                        #endregion
                    }

                    #endregion
                    if (oldItems.Count > 0)
                    {
                        para.Add(oldItems);
                    }

                }

                #region 添加另一个属性时，新加入

                if (!ucols.dataGridViewMultiAttributes.Columns.Contains(option.OptionName))
                {
                    //新加
                    List<string> currentItem = new List<string>();
                    string[] item = optionValues.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToArray();
                    for (int i = 0; i < item.Length; i++)
                    {
                        item[i] = item[i].Trim();
                    }
                    currentItem.AddRange(item);
                    para.Add(currentItem);
                    OptionNames = OptionNames + option.OptionName;
                }

                #endregion



                #endregion
            }
            else
            {
                //当次新添加的值 完全新的
                List<string> currentItem = new List<string>();
                string[] item = optionValues.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToArray();
                for (int i = 0; i < item.Length; i++)
                {
                    item[i] = item[i].Trim();
                }
                currentItem.AddRange(item);
                para.Add(currentItem);
                OldOptionCount = 1;
                OptionNames = option.OptionName;
            }




            List<string> rs = ArrayCombination.Combination(para);
            #endregion

            #region 组合数据
            int controls = 0;
            foreach (Control item in flowLayoutPanel多属性.Controls)
            {
                if (item.Visible)
                {
                    controls++;
                }
            }
            //属性值变化
            if (controls != rs.Count || OldOptionCount != OptionNames.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length)
            {
                //减少 就隐藏
                if (flowLayoutPanel多属性.Controls.Count > rs.Count)
                {
                    foreach (Control ct in flowLayoutPanel多属性.Controls)
                    {
                        //ct.Visible = false;
                        foreach (string str in rs)
                        {
                            Control[] myuc = flowLayoutPanel多属性.Controls.Find(str.Trim(), true);
                            if (myuc.Length > 0)
                            {
                                (myuc[0] as UCMultiAttributes).Visible = true;
                            }
                        }
                    }

                }
                else
                {
                    //添加 并且属性个数都不一样了 SKU全部生成
                    if (OldOptionCount != para.Count)
                    {
                        //但是这里还是不清空，用隐藏，保存时根据隐藏做删除操作
                        flowLayoutPanel多属性.Tag = "全新生成";
                        foreach (Control cc in flowLayoutPanel多属性.Controls)
                        {
                            if (cc.Visible && cc is UCMultiAttributes)
                            {
                                cc.Visible = false;
                            }
                        }

                    }

                }
            }

            foreach (string str in rs)
            {
                UCMultiAttributes ucm = new UCMultiAttributes();
                Control[] myuc = flowLayoutPanel多属性.Controls.Find(str.Trim(), true);
                if (myuc.Length > 0)
                {
                    ucm = myuc[0] as UCMultiAttributes;
                    ucm.Tag = "更新模式";
                }
                //标记一下这个控件是更新


                #region 添加UC
                ucm.Name = str.Trim();
                ucm.Visible = true;
                if (string.IsNullOrEmpty(ucm.txtSKU.Text))
                {
                    ucm.txtSKU.Text = Biz.SMTCommonData.GetAutoSKUCodeByAtt(txtProductNo.Text); //创建新的SKU
                }

                #endregion

                DataTable dtOV = new DataTable();
                string[] dtOVDc = OptionNames.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string dcStr in dtOVDc)
                {
                    //添加列
                    if (!dtOV.Columns.Contains(dcStr))
                    {
                        DataColumn dc = new DataColumn();
                        dc.ColumnName = dcStr;
                        dtOV.Columns.Add(dc);
                    }

                }

                DataRow drr = dtOV.NewRow();
                string[] sz = str.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < sz.Length; i++)
                {
                    drr[i] = sz[i].Trim();
                }
                dtOV.Rows.Add(drr);
                ucm.dataGridViewMultiAttributes.DataSource = dtOV;
                ucm.pictureBoxsku.DoubleClick -= pictureBoxsku_DoubleClick;
                ucm.pictureBoxsku.DoubleClick += pictureBoxsku_DoubleClick;
                ucm.chk打包销售.Tag = ucm;//打包销售选择时，通过这个来控制另一个txtbox
                ucm.chk打包销售.CheckedChanged -= chk打包销售_CheckedChanged;
                ucm.chk打包销售.CheckedChanged += chk打包销售_CheckedChanged;


                //说明是全新添加的，否则 绑定 事件等 不能重复处理
                if (ucm.Tag == null)
                {
                    ContextMenuStrip cms = new System.Windows.Forms.ContextMenuStrip();
                    cms.Items.Add("浏览图片");
                    cms.Items[0].Click += cms_Click;
                    cms.Items[0].Tag = ucm.pictureBoxsku;
                    // cms.Tag = ucm.pictureBoxsku;
                    ucm.pictureBoxsku.ContextMenuStrip = cms;
                    ucm.cmbPackedUnit.Items.Clear();
                    HLH.Lib.Helper.DropDownListHelper.InitDropList<productsunitEntity>(unitList, ucm.cmbPackedUnit, "ID", "UnitName");
                    flowLayoutPanel多属性.Controls.Add(ucm);
                }

            }

            #endregion
            #endregion

        }

        private void cms_Click(object sender, EventArgs e)
        {
            ToolStripDropDownItem tsddi = sender as ToolStripDropDownItem;
            if (tsddi.Tag != null)
            {
                PictureBox pb = tsddi.Tag as PictureBox;
                System.Diagnostics.Process.Start("explorer.exe", "/select," + pb.ImageLocation.ToString());
                frmMain.Instance.PrintInfoLog("正在浏览文件：" + pb.ImageLocation.ToString());
            }


        }

        private void chk打包销售_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.Tag is UCMultiAttributes)
            {
                UCMultiAttributes uc = cb.Tag as UCMultiAttributes;
                uc.txtPackSellQty.Visible = cb.Checked;
            }
        }

        private void pictureBoxsku_DoubleClick(object sender, EventArgs e)
        {
            openFileDialog4Images.Filter = "图片文件(*.jpg)|*.jpg|所有文件(*.*)|*.*";
            if (openFileDialog4Images.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (openFileDialog4Images.FileNames.Length > 0)
                {
                    PictureBox pb = sender as PictureBox;
                    pb.ImageLocation = openFileDialog4Images.FileNames[0];
                    //LoadImagesForTemp(openFileDialog4Images.FileNames, true);
                }

            }
        }




        private void chk是否打包销售_CheckedChanged(object sender, EventArgs e)
        {
            txtPackSellQty.Visible = chk是否打包销售.Checked;
        }

        private void pictureBox单属性图片_DoubleClick(object sender, EventArgs e)
        {
            openFileDialog4Images.Filter = "图片文件(*.jpg)|*.jpg|所有文件(*.*)|*.*";
            if (openFileDialog4Images.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (openFileDialog4Images.FileNames.Length > 0)
                {
                    PictureBox pb = sender as PictureBox;
                    pb.ImageLocation = openFileDialog4Images.FileNames[0];
                    //LoadImagesForTemp(openFileDialog4Images.FileNames, true);
                }

            }
        }

        private void pictureBox单属性图片_DragDrop(object sender, DragEventArgs e)
        {
            //目标对象
            string[] str = ((string[])e.Data.GetData(DataFormats.FileDrop));
            if (str.Length > 0)
            {
                PictureBox pb = sender as PictureBox;
                System.IO.FileInfo fi = new FileInfo(str[0]);
                if (fi.Extension.ToUpper() == ".JPG")
                {
                    pb.ImageLocation = str[0];
                }
            }
        }

        private void pictureBox单属性图片_DragEnter(object sender, DragEventArgs e)
        {
            //选中的对象
            //检查拖动的数据是否适合于目标空间类型；若不适合，则拒绝放置。
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                //检查Ctrl键是否被按住
                if (e.KeyState == Ctrl)
                {
                    //若Ctrl键被按住，则设置拖放类型为拷贝
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    //若Ctrl键没有被按住，则设置拖放类型为移动
                    e.Effect = DragDropEffects.Move;
                }
            }
            else
            {
                //若被拖动的数据不适合于目标控件，则设置拖放类型为拒绝放置
                e.Effect = DragDropEffects.None;
            }
        }

        private void btn批量设置_Click(object sender, EventArgs e)
        {
            //批量设置产品多属性
            #region

            foreach (Control cc in flowLayoutPanel多属性.Controls)
            {
                if (cc.Visible)
                {
                    UCMultiAttributes myuc = cc as UCMultiAttributes;
                    if (chk批量设置库存.Checked)
                    {
                        myuc.txtStock.Text = txtBatchStock.Text;
                    }
                    if (chk批量设置售价.Checked)
                    {
                        myuc.txtSellPrice.Text = txtBatchSellPrice.Text;
                    }
                    if (chk批量设置进价.Checked)
                    {
                        myuc.txtPurchasePrice.Text = txtBatchPurchasePrice.Text;
                    }
                    if (chk是否批量设置打包销售.Checked)
                    {
                        myuc.chk打包销售.Checked = chk是否批量设置打包销售.Checked;
                        myuc.txtPackSellQty.Text = txtBatchPackSellQty.Text;
                    }
                    if (chk批量设置包装单位.Checked)
                    {
                        if (myuc.cmbPackedUnit.SelectedItem != null && myuc.cmbPackedUnit.SelectedItem is productsunitEntity)
                        {
                            myuc.cmbPackedUnit.SelectedValue = (myuc.cmbPackedUnit.SelectedItem as productsunitEntity).ID;
                        }
                    }

                    if (chk批量设置包装数量.Checked)
                    {
                        myuc.txtPackQty.Text = txtBatchPackQty.Text;
                    }

                    if (chk批量设置包装说明.Checked)
                    {
                        myuc.txtPackBackUp.Text = txtBatchPackBackUp.Text;
                    }

                    if (chk批量设置包装重量.Checked)
                    {
                        myuc.txtPackWeight.Text = txtBatchPackWeight.Text;
                    }
                }
            }
            #endregion



        }

        private void chk是否批量设置打包销售_CheckedChanged(object sender, EventArgs e)
        {
            txtBatchPackSellQty.Visible = chk是否批量设置打包销售.Checked;
        }

        private void 删除图上ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
