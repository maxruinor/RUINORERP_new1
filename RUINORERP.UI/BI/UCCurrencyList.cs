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
using RUINORERP.Business.CommService;

using System.Web.UI;
using System.Windows.Documents;
using SourceGrid2.Win32;
using RUINORERP.Global;
using RUINORERP.PacketSpec.Protocol;

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("币别资料", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.财务资料)]
    public partial class UCCurrencyList : BaseForm.BaseListGeneric<tb_Currency>
    {
        public UCCurrencyList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCCurrencyEdit);
           
        }
        protected override async Task Add()
        {
            if (ListDataSoure.Count == 0)
            {
                //弹出提示框：系统将默认为您添加人民币和美元币别。
                MessageBox.Show("系统将默认为您添加人民币和美元币别。");
                //第一次添加付款方式时，添加系统默认的值  优化
                //循环枚举DefaultPaymentMethod中的值，添加到表中
                List<tb_Currency> list = new List<tb_Currency>();
                foreach (var item in Enum.GetValues(typeof(DefaultCurrency)))
                {
                    tb_Currency currency = new tb_Currency();
                    currency.Is_enabled = true;
                    currency.CurrencyCode = item.ToString();
                    DefaultCurrency defaultCurrency = (DefaultCurrency)item;
                    switch (defaultCurrency)
                    {
                        case DefaultCurrency.RMB:
                            currency.Country="中国";
                            currency.CurrencyName = "人民币";
                            currency.CurrencySymbol = "￥";
                            currency.Is_BaseCurrency = true;
                            break;
                        case DefaultCurrency.USD:
                            currency.Country = "美国";
                            currency.CurrencyName = "美元";
                            currency.CurrencySymbol = "$";
                            currency.Is_BaseCurrency = false;
                            break;
                        default:
                            break;
                    }
                    list.Add(currency);
                }
                List<long> ids = await MainForm.Instance.AppContext.Db.Insertable<tb_Currency>(list).ExecuteReturnSnowflakeIdListAsync();
                Query();
             await   base.Add();
                base.toolStripButtonModify.Enabled = false;
            }
            else
            {
                //非第一次添加付款方式时。正常处理
             await   base.Add();
                base.toolStripButtonModify.Enabled = false;
            }
        }

        /// <summary>
        /// 特殊 处理
        /// 本位币只能一行一个币种有效
        /// </summary>
        public async override Task<List<tb_Currency>> Save()
        {
            tb_Currency currency = new tb_Currency();
            tb_CurrencyController<tb_Currency> pctr = Startup.GetFromFac<tb_CurrencyController<tb_Currency>>();
            //这里是否要用保存列表来处理
            foreach (var item in bindingSourceList.List)
            {
                var entity = item as tb_Currency;
                switch (entity.ActionStatus)
                {
                    case ActionStatus.无操作:
                        break;
                    case ActionStatus.新增:
                    case ActionStatus.修改:

                        ReturnResults<tb_Currency> rr = new ReturnResults<tb_Currency>();
                        rr = await pctr.SaveOrUpdate(entity);
                        if (rr.Succeeded)
                        {
                            currency = rr.ReturnObject;
                            ToolBarEnabledControl(MenuItemEnums.保存);
                            //根据要缓存的列表集合来判断是否需要上传到服务器。让服务器分发到其他客户端
                            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                            //只处理需要缓存的表
                            if (BizCacheHelper.Manager.NewTableList.TryGetValue(typeof(tb_Currency).Name, out pair))
                            {
#warning TODO: 这里需要完善具体逻辑，当前仅为占位
                                //如果有更新变动就上传到服务器再分发到所有客户端
                                //OriginalData odforCache = ActionForClient.更新缓存<tb_Currency>(rr.ReturnObject);
                                //byte[] buffer = CryptoProtocol.EncryptClientPackToServer(odforCache);
                                //MainForm.Instance.ecs.client.Send(buffer);
                            }
                        }
                        else
                        {
                            MainForm.Instance.uclog.AddLog(rr.ErrorMsg, Global.UILogType.错误);
                        }
                        break;
                    case ActionStatus.删除:
                        break;
                    default:
                        break;
                }

                entity.HasChanged = false;
            }

            base.toolStripButtonModify.Enabled = true;

            List<tb_Currency> list = new List<tb_Currency>();
            foreach (var item in bindingSourceList.List)
            {
                var entity = item as tb_Currency;
                list.Add(entity);
            }
            if (currency.Is_BaseCurrency.HasValue && currency.Is_BaseCurrency.Value)
            {
                //其他行是否
                list.Where(t => t.Currency_ID != currency.Currency_ID).ToList().ForEach(w => w.Is_BaseCurrency = false);
                //保存
                await MainForm.Instance.AppContext.Db.Updateable<tb_Currency>(list).ExecuteCommandAsync();
            }

            return list;
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
