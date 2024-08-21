using RUINORERP.Business;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
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
    

    //期初盘点 就从产品表是，其他就是从库存表。因为期初时库存可能为空
    public partial class frmStocktakeFilterByStock : BaseForm.frmBaseQuery
    {
        public frmStocktakeFilterByStock()
        {
            InitializeComponent();
            InitListData();
            LoadDroplistData();
            this.QueryDto = new tb_Product_BaseQueryDto();
            this.FieldNameList = UIHelper.GetFieldNameColList(typeof(tb_Prod));
            dataGridView1.XmlFileName = "frmStocktakeFilterByStock";
            BindData(this.QueryDto);
        }
        /// <summary>
        /// 初始化列表数据
        /// </summary>
        //internal void InitListData()
        //{
        //    this.dataGridView1.DataSource = null;
        //     ListDataSoure = bindingSourceList;
        //    //绑定导航
        //    this.bindingNavigatorList.BindingSource = ListDataSoure;

        //    this.dataGridView1.DataSource = ListDataSoure.DataSource;
        //}

        public void LoadDroplistData()
        {
            DataBindingHelper.InitDataToCmb<tb_ProductType>(k => k.Type_ID, v => v.TypeName, cmbType_ID);
            //https://www.cnblogs.com/cdaniu/p/15236857.html
            EnumBindingHelper bindingHelper = new EnumBindingHelper();
            List<int> exclude = new List<int>();
            exclude.Add((int)ProductAttributeType.虚拟);
            bindingHelper.InitDataToCmbByEnumOnWhere<tb_ProdQueryDto>(typeof(ProductAttributeType).GetListByEnum(2, exclude.ToArray()), e => e.PropertyType, cmbPropertyType);
            // InitDataToCmbByEnumDynamicGeneratedDataSource<tb_Prod>(typeof(Global.ProductAttributeType), e => e.PropertyType, cmbPropertyType);
        }



        public tb_Product_BaseQueryDto QueryDto { get; set; }

        /// <summary>
        /// 字段还需要补充完整
        /// </summary>
        /// <param name="entity"></param>
        public void BindData(tb_Product_BaseQueryDto entity)
        {
            QueryDto = entity;

            DataBindingHelper.BindData4Cmb<tb_ProductType>(entity, k => k.Type_ID, v => v.TypeName, cmbType_ID);
            // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            //  DataBindingHelper.BindData4Cmb<tb_Payment_method>(entity, k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
            // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID);
            DataBindingHelper.BindData4CmbByEnumData<tb_Product_BaseQueryDto>(entity, k => k.PropertyType_ID, cmbPropertyType);
            //  DataBindingHelper.BindData4DataTime<tb_sales_orderQueryDto>(entity, t => t.sale_date, dtpsale_date, false);
            //  DataBindingHelper.BindData4DataTime<tb_sales_orderQueryDto>(entity, t => t.delivery_date, dtpdelivery_date, false);
          //  DataBindingHelper.BindData4DataTime<tb_Product_BaseQueryDto>(entity, t => t.Created_at, dtpCreated_at, false);

            DataBindingHelper.BindData4TextBox<tb_Product_BaseQueryDto>(entity, t => t.Name, txtName, BindDataType4TextBox.Text, false);




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

        private void frmStocktakeFilterByStock_Load(object sender, EventArgs e)
        {

        }
    }
}
