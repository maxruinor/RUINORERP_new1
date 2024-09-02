using RUINORERP.Business;
using RUINORERP.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.UI.Common;
using System.Collections.Concurrent;
namespace RUINORERP.UI.ProductEAV
{

    [MenuAttrAssemblyInfo("多属性编辑", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.产品资料)]
    public partial class UCMultiPropertyEditor : UserControl
    {
        public UCMultiPropertyEditor()
        {
            InitializeComponent();
        }

        private void btnQueryForGoods_Click(object sender, EventArgs e)
        {
            Query();
        }


        public void Query()
        {
            tb_ProdController<tb_Prod> dc = Startup.GetFromFac<tb_ProdController<tb_Prod>>();

            Expression<Func<tb_Prod, bool>> exp = Expressionable.Create<tb_Prod>() //创建表达式
           .AndIF(txtNo.Text.Trim().Length > 0, w => w.ProductNo.Contains(txtNo.Text.Trim()))
           .AndIF(txtNo.Text.Trim().Length > 0, w => w.ProductNo.Contains(txtNo.Text.Trim()))
           .AndIF(txtNo.Text.Trim().Length > 0, w => w.ProductNo.Contains(txtNo.Text.Trim()))
           .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.ProductNo.Contains(txtSpecifications.Text.Trim()))
            .ToExpression();
            var list = dc.BaseQueryByWhereTop(exp, 100);
            bindingSourcProperty.DataSource = list.ToBindingSortCollection();
        }
        ConcurrentDictionary<string, KeyValuePair<string, bool>> FieldNameList1;
        private void UCMultiPropertyEditor_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            FieldNameList1 = UIHelper.GetFieldNameColList(typeof(tb_Prod));
            dataGridView1.XmlFileName = "UCMultiPropertyEditor_" + typeof(tb_Prod).Name;
            dataGridView1.FieldNameList = FieldNameList1;
            dataGridView1.DataSource = null;
            bindingSourcProperty.DataSource = new List<tb_Prod>();
            dataGridView1.DataSource = bindingSourcProperty;
        }
    }
}
