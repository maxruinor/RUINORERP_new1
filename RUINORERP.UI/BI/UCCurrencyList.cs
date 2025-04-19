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
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("币别资料", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.财务资料)]
    public partial class UCCurrencyList : BaseForm.BaseListGeneric<tb_Currency>
    {
        public UCCurrencyList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCCurrencyEdit);
            /*
            List<KeyValuePair<object, string>> kvlist = new List<KeyValuePair<object, string>>();
            kvlist.Add(new KeyValuePair<object, string>(true, "是"));
            kvlist.Add(new KeyValuePair<object, string>(false, "否"));
            Expression<Func<tb_Currency, bool?>> expr1;
            expr1 = (p) => p.Is_available;// == name;
            Expression<Func<tb_Currency, bool?>> expr2;
            expr2 = (p) => p.Is_enabled;// == name;
            string colName1 = expr1.GetMemberInfo().Name;
            string colName2 = expr2.GetMemberInfo().Name;
            base.ColNameDataDictionary.TryAdd(colName1, kvlist);
            base.ColNameDataDictionary.TryAdd(colName2, kvlist);*/
        }
        protected override void Add()
        {
            if (ListDataSoure.Count == 0)
            {
                //第一次添加付款方式时，添加系统默认的值  优化
                //循环枚举DefaultPaymentMethod中的值，添加到表中
                List<tb_Currency> list = new List<tb_Currency>();
                foreach (var item in Enum.GetValues(typeof(DefaultCurrency)))
                { 
                    tb_Currency currency = new tb_Currency();
                    currency.Is_enabled = true;
                    currency.CurrencyCode = item.ToString();
                    DefaultCurrency defaultCurrency= (DefaultCurrency)item;
                    switch (defaultCurrency)
                    {
                        case DefaultCurrency.RMB:
                            currency.CurrencyName = "中国";
                            currency.CurrencySymbol = "￥";
                            currency.Is_BaseCurrency=true;
                            break;
                        case DefaultCurrency.USD:
                            currency.CurrencyName = "美国";
                            currency.CurrencySymbol = "$";
                            currency.Is_BaseCurrency = false;
                            break;
                        default:
                            break;
                    }
                    list.Add(currency);
                }
                MainForm.Instance.AppContext.Db.Insertable<tb_PaymentMethod>(list).ExecuteCommandAsync();
                Query();
                base.Add();
                base.toolStripButtonModify.Enabled = false;
            }
            else
            {
                //非第一次添加付款方式时。正常处理
                base.Add();
                base.toolStripButtonModify.Enabled = false;
            }
        }



        /*
protected override void Add()
        {
            UCCurrencyEdit frmadd = new UCCurrencyEdit();
            frmadd.StartPosition = FormStartPosition.CenterScreen;
            frmadd.bindingSourceEdit = bindingSourceList;
            object obj = frmadd.bindingSourceEdit.AddNew();
            frmadd.BindData(obj as tb_Currency);
            if (frmadd.ShowDialog() == DialogResult.OK)
            {
                base.ToolBarEnabledControl(MenuItemEnums.新增);
            }
            else
            {
                frmadd.bindingSourceEdit.CancelEdit();
                //command.Undo();
            }
        }
        */


        /*
   protected override void Modify()
   {
       UCCurrencyEdit frmadd = new UCCurrencyEdit();
       base.Modify<tb_Currency>(frmadd);

       if (base.bindingSourceList.Current != null)
       {
           RevertCommand command = new RevertCommand();
           UCCurrencyEdit frmadd = new UCCurrencyEdit();
           frmadd.bindingSourceEdit = bindingSourceList;
           frmadd.BindData(base.bindingSourceList.Current as tb_Currency);
           //缓存当前编辑的对象。如果撤销就回原来的值

           object oldobj = CloneHelper.DeepCloneObject<tb_Currency>(base.bindingSourceList.Current as tb_Currency);
           int UpdateIndex = base.bindingSourceList.Position;
           command.UndoOperation = delegate ()
           {
               //Undo操作会执行到的代码
               CloneHelper.SetValues<tb_Currency>(base.bindingSourceList[UpdateIndex], oldobj);
           };
           if (frmadd.ShowDialog() == DialogResult.Cancel)
           {
               command.Undo();
           }
           else
           {
               base.ToolBarEnabledControl(MenuItemEnums.修改);

           }
           dataGridView1.Refresh();
       }
    }*/






    }
}
