using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Business.LogicaService;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;


namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("付款方式", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.财务资料)]
    public partial class UCPaymentMethodList : BaseForm.BaseListGeneric<tb_PaymentMethod>
    {
        public UCPaymentMethodList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCPaymentMethodEdit);

        }

        protected override async Task<object> Add()
        {
            object result = null;
            if (ListDataSoure.Count == 0)
            {
                //第一次添加付款方式时，添加系统默认的值先
                //循环枚举DefaultPaymentMethod中的值，添加到表中
                List<tb_PaymentMethod> list = new List<tb_PaymentMethod>();
                foreach (var item in Enum.GetValues(typeof(DefaultPaymentMethod)))
                {
                    bool cash = true;
                    if (item.ToString() == "账期")
                    {
                        cash = false;
                    }
                    list.Add(new tb_PaymentMethod() { Paytype_Name = item.ToString(), Cash = cash });
                }
                await MainForm.Instance.AppContext.Db.Insertable<tb_PaymentMethod>(list).ExecuteReturnSnowflakeIdListAsync();
                QueryAsync();
                result = await base.Add();
                base.toolStripButtonModify.Enabled = false;
            }
            else
            {
                //非第一次添加付款方式时。正常处理
                result = await base.Add();
                base.toolStripButtonModify.Enabled = false;
            }
            return result;
        }





    }



}
