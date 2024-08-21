using RUINORERP.Business;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Model;
using RUINORERP.Model.QueryDto;
using RUINORERP.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.PSI.INV
{
    public partial class frmStocktakeFilterByProd : BaseForm.frmBaseQuery
    {
        public frmStocktakeFilterByProd()
        {
            InitializeComponent();
            InitListData();
            LoadDroplistData();
            this.QueryDto = new tb_Product_BaseQueryDto();
            BindData(this.QueryDto);
        }

        /// <summary>
        /// 初始化列表数据
        /// </summary>
        //internal void InitListData()
        //{
        //    this.dataGridView1.DataSource = null;
        //    // ListDataSoure = bindingSourceList;
        //    //绑定导航
        //    this.bindingNavigatorList.BindingSource = ListDataSoure;

        //    this.dataGridView1.DataSource = ListDataSoure.DataSource;
        //}

        public void LoadDroplistData()
        {
            DataBindingHelper.InitDataToCmb<tb_CustomerVendor>(k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID);
            //DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name,cmbe);
            //DataBindingHelper.InitDataToCmb<tb_Payment_method>(k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
        }



        public tb_Product_BaseQueryDto QueryDto { get; set; }

        /// <summary>
        /// 字段还需要补充完整
        /// </summary>
        /// <param name="entity"></param>
        public void BindData(tb_Product_BaseQueryDto entity)
        {
            QueryDto = entity;


            // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            //  DataBindingHelper.BindData4Cmb<tb_Payment_method>(entity, k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID);

            //  DataBindingHelper.BindData4DataTime<tb_sales_orderQueryDto>(entity, t => t.sale_date, dtpsale_date, false);
            //  DataBindingHelper.BindData4DataTime<tb_sales_orderQueryDto>(entity, t => t.delivery_date, dtpdelivery_date, false);
            DataBindingHelper.BindData4DataTime<tb_SaleOrderQueryDto>(entity, t => t.Created_at, dtpCreated_at, false);

            DataBindingHelper.BindData4TextBox<tb_SaleOrderQueryDto>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);




        }

        tb_ProdController<tb_Prod> ctrMain = Startup.GetFromFac<tb_ProdController<tb_Prod>>();

        private async void btnQuery_Click(object sender, EventArgs e)
        {
            Pagination pagination = new Pagination();
            List<tb_Prod> list = await ctrMain.QueryAsync();// await ctrMain.QueryAsync(QueryDto, pagination);
            dataGridView1.DataSource = null;
            dataGridView1.ReadOnly = true;
            ListDataSoure.DataSource = list.ToBindingSortCollection();
            dataGridView1.DataSource = ListDataSoure;
            bindingNavigatorList.BindingSource = ListDataSoure;
           
        }
    }
}
