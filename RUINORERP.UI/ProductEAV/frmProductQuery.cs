using HLH.Lib.Draw;
using MySqlEntity;
using SMTAPI.Entity;
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


    public partial class frmProductQuery : Form
    {
        public frmProductQuery()
        {
            InitializeComponent();
        }

        DataGridViewImageColumn columnIMG = new DataGridViewImageColumn();

        private void btnQuery_Click(object sender, EventArgs e)
        {



            List<internalproductEntity> list = new internalproductEntity().GetAllByQuery(" ProductTypeID= " + ((cmbProductType.SelectedItem) as producttypeEntity).ID + Query());
            dataGridView1.DataSource = list;

            if (chk显示图片.Checked)
            {
                dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
                dataGridView1.DataSource = list;
                if (!dataGridView1.Columns.Contains(columnIMG))
                {
                    dataGridView1.Columns.Insert(0, columnIMG);
                }

            }
            else
            {
                if (dataGridView1.Columns.Contains(columnIMG))
                {
                    dataGridView1.Columns.Remove(columnIMG);
                }
                dataGridView1.DataSource = list;
            }


        }

        private string Query()
        {
            StringBuilder sb = new StringBuilder();
            if (txtProductID.Text.Trim().Length > 0)
            {
                sb.Append(" and ProductNo='" + txtProductID.Text + "'");
            }
            return sb.ToString();
        }
        private void 编辑选中产品ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProductEAVEdit edit = new frmProductEAVEdit();
            edit.txtProductNo.Text = dataGridView1.CurrentRow.Cells[0].ToString();
            internalproductEntity entity = new internalproductEntity();
            entity = entity.GetById(int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString()));
            edit.entity = entity;
            edit.ShowDialog();
        }

        private void 导入到ZencartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //要改进的，如果远程网站类目编号等于本地数据编号 ，则可以不用再次选择类目
            //SKU判断 如果存在。刚匹配一下 用更新模式。（小心操作）

            frmSelectClassNo sc = new frmSelectClassNo();
            if (sc.ShowDialog() == DialogResult.OK)
            {
                int locProdID = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                //这里操作都不做更新操作
                internalproductEntity entity = new internalproductEntity();
                entity = entity.GetById(locProdID);

                productattributeEntity prodAttr = new productattributeEntity();
                if (entity.ProductTypeID == (int)EnumProductType.单属性)
                {
                    prodAttr = prodAttr.GetByNaturalKey("SKU", entity.ProductNo);
                }


                Zencart.Entity.productsEntity pro = new Zencart.Entity.productsEntity();
                pro = pro.GetByNaturalKey("products_model", entity.ProductNo);
                if (pro == null)
                {
                    pro = new Zencart.Entity.productsEntity();
                }
                pro.products_model = entity.ProductNo;
                pro.products_status = 1;
                pro.products_type = 1;

                string myImgs = string.Empty;
                string[] images = entity.ProductsImages.Split(new char[] { '#', '|', '|', '#' }, StringSplitOptions.RemoveEmptyEntries);
                List<string> imagesTemp = new List<string>();
                foreach (string item in images)
                {
                    if (item.Trim().Length > 0)
                    {
                        myImgs += "s/" + item + ",";
                    }

                }
                myImgs = myImgs.TrimEnd(',');
                pro.products_image = myImgs.Replace("\\", "/");
                pro.products_date_added = System.DateTime.Now;
                pro.products_price = decimal.Parse(prodAttr.SellPrice.Value.ToString());
                pro.products_price_retail = decimal.Parse(prodAttr.SellPrice.ToString());
                pro.products_weight = 0.102f;
                pro.products_quantity = 5000;
                pro.products_virtual = 0;
                pro.products_tax_class_id = 0;
                pro.Save();

                //需要得到新加产品的ID
                // string selectInsertID = "select last_insert_id() ";
                //  pro.products_id = pro.ExecuteScalar(selectInsertID);
                pro = pro.GetByNaturalKey("products_model", entity.ProductNo);

                //保存
                //sc.ClassNo
                //保存类目

                //要指定 具体 是保存 下面两个表都不用更新，通常是。
                Zencart.Entity.productstocategoriesEntity pcate = new Zencart.Entity.productstocategoriesEntity();
                pcate.products_id = pro.products_id;
                pcate.categories_id = int.Parse(sc.ClassNo);
                pcate.Save();

                //保存产品描述
                Zencart.Entity.productsdescriptionEntity desc = new Zencart.Entity.productsdescriptionEntity();
                desc.products_description = entity.ProductDesc;
                desc.language_id = 1;//英语
                desc.products_id = pro.products_id; ;
                desc.products_name = entity.ProductName;
                desc.products_viewed = 0;
                desc.Save();


                AddProductEAVService.UploadImages(entity);


            }
        }



        private void frmProductQuery_Load(object sender, EventArgs e)
        {
            HLH.Lib.Helper.DropDownListHelper.InitDropList<producttypeEntity>(new producttypeEntity().GetAll(), cmbProductType, "ID", "TypeName", true);
            columnIMG.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            columnIMG.Name = "IMG";
            columnIMG.HeaderText = "图片";
            columnIMG.Width = 50;
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                frmProductEAVEdit edit = new frmProductEAVEdit();
                edit.txtProductNo.Text = dataGridView1.CurrentRow.Cells[0].ToString();
                internalproductEntity entity = new internalproductEntity();

                entity = entity.GetById(int.Parse(dataGridView1.CurrentRow.Cells["ID"].Value.ToString()));
                edit.entity = entity;
                edit.ShowDialog();
            }
        }

        private void 更新到ZencartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //单品操作 不更新图片和类目 
            int locProdID = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            //这里操作都不做更新操作
            internalproductEntity entity = new internalproductEntity();
            entity = entity.GetById(locProdID);

            productattributeEntity prodAttr = new productattributeEntity();
            if (entity.ProductTypeID == (int)EnumProductType.单属性)
            {
                prodAttr = prodAttr.GetByNaturalKey("SKU", entity.ProductNo);
            }

            Zencart.Entity.productsEntity pro = new Zencart.Entity.productsEntity();
            pro = pro.GetByNaturalKey("products_model", entity.ProductNo);
            if (pro == null)
            {
                pro = new Zencart.Entity.productsEntity();
            }
            pro.products_model = entity.ProductNo;
            pro.products_status = 1;
            pro.products_type = 1;
            string myImgs = string.Empty;
            string[] images = entity.ProductsImages.Split(new char[] { '#', '|', '|', '#' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> imagesTemp = new List<string>();
            foreach (string item in images)
            {
                if (item.Trim().Length > 0)
                {
                    myImgs += "s/" + item + ",";
                }

            }
            myImgs = myImgs.TrimEnd(',');
            pro.products_image = myImgs.Replace("\\", "/");
            pro.products_date_added = System.DateTime.Now;
            pro.products_price = decimal.Parse(prodAttr.SellPrice.Value.ToString());
            pro.products_price_retail = decimal.Parse(prodAttr.SellPrice.Value.ToString());
            pro.products_weight = float.Parse(prodAttr.PackWeight.Value.ToString());
            pro.products_quantity = 5000;
            pro.products_virtual = 0;
            pro.products_tax_class_id = 0;
            pro.Update();

            //需要得到新加产品的ID
            // string selectInsertID = "select last_insert_id() ";
            //  pro.products_id = pro.ExecuteScalar(selectInsertID);
            //pro = pro.GetByNaturalKey("products_model", entity.ProductNo);

            //保存
            //sc.ClassNo
            //保存类目

            ////要指定 具体 是保存 下面两个表都不用更新，通常是。
            //Zencart.Entity.productstocategoriesEntity pcate = new Zencart.Entity.productstocategoriesEntity();
            //pcate.products_id = pro.products_id;
            //pcate.categories_id = int.Parse(sc.ClassNo);
            //pcate.Save();

            //保存产品描述
            Zencart.Entity.productsdescriptionEntity desc = new Zencart.Entity.productsdescriptionEntity();
            desc = desc.GetByNaturalKey("products_id", pro.products_id.ToString());
            desc.products_description = entity.ProductDesc;
            desc.language_id = 1;//英语
            desc.products_id = pro.products_id; ;
            desc.products_name = entity.ProductName;
            if (!desc.products_viewed.HasValue)
            {
                desc.products_viewed = 0;
            }
            desc.Update();
            frmMain.Instance.PrintInfoLog("更新完成。");
        }

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

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            #region 显示图片

            if (dataGridView1.Columns[e.ColumnIndex].Name.Equals("IMG") && chk显示图片.Checked && this.dataGridView1.Rows[e.RowIndex].Cells["IMG"].Value == null)
            {
                if (this.dataGridView1.Rows[e.RowIndex].Cells["IMG"].Tag == "error")
                {
                    return;
                }
                try
                {
                    //如果本地图片列有值，则直接显示，否则则下载
                    string LocPath = dataGridView1["ProductsImages", e.RowIndex].Value.ToString();
                    if (!string.IsNullOrEmpty(LocPath))
                    {
                        //分割符为#||#时
                        if (LocPath.Contains("#||#"))
                        {
                            LocPath = LocPath.Split(new string[] { "#||#" }, StringSplitOptions.RemoveEmptyEntries)[0];
                        }
                        //绝对地址时
                        if (LocPath.Contains(";"))
                        {
                            LocPath = LocPath.Split(';')[0];
                        }
                        Image img = ImageHelper.GetImage(System.IO.Path.Combine(MultiUser.Instance.ProductLibraryImagesPath, LocPath), int.Parse("100"), int.Parse("100"));
                        if (img != null)
                        {
                            this.dataGridView1.Rows[e.RowIndex].Cells["IMG"].Value = img;
                            this.dataGridView1.Rows[e.RowIndex].Cells["IMG"].Tag = true;
                        }
                    }
                    else
                    {

                    }

                }
                catch (Exception ex)
                {
                    this.dataGridView1.Rows[e.RowIndex].Cells["IMG"].Tag = "error";
                    frmMain.Instance.PrintInfoLog(ex.Message, Color.Red);
                }
            }

            #endregion
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void 更新到ConnshopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (MultiUser.Instance.WoocommerceCategories == null || MultiUser.Instance.WoocommerceCategories.Count == 0)
            //{
            //    frmMain.Instance.PrintInfoLog("请先提取远程类目。");
            //    return;
            //}
            //Wordpress.ServiceForProduct.GetProduct("94");
            //要改进的，如果远程网站类目编号等于本地数据编号 ，则可以不用再次选择类目
            //SKU判断 如果存在。刚匹配一下 用更新模式。（小心操作）
            //frmSelectClassNo sc = new frmSelectClassNo();
            //if (sc.ShowDialog() == DialogResult.OK)
            //{
            int locProdID = int.Parse(dataGridView1.CurrentRow.Cells["ID"].Value.ToString());

            Wordpress.ServiceForProduct.publishProductfromEAV(locProdID);

            //}
        }

        private void 标记为已经更新到connshopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int locProdID = int.Parse(dataGridView1.CurrentRow.Cells["ID"].Value.ToString());
                internalproductEntity entity = new internalproductEntity();
                entity = entity.GetById(locProdID);
                if (entity.PublishTargets.ToLower().Contains("connshop"))
                {
                    frmMain.Instance.PrintInfoLog("当前商品已经上传到connshop,SKU:" + entity.SKU);
                    return;
                }
                else
                {
                    entity.PublishTargets += "connshop";
                    entity.SaveOrUpdate(entity);
                }

            }
        }
    }
}
