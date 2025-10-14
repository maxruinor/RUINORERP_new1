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
using RUINORERP.Business.Processor;
using RUINORERP.Global.EnumExt;
using RUINORERP.Common.Extensions;
using SqlSugar;
using RUINORERP.Business.Security;
using RUINORERP.Global;
using RUINORERP.Business.CommService;
using RUINORERP.Extensions.Middlewares;



namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("收款账号管理", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.财务资料)]
    public partial class UCFMPayeeInfoList : BaseForm.BaseListGeneric<tb_FM_PayeeInfo>
    {
        public UCFMPayeeInfoList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCFMPayeeInfoEdit);
            //固定值也包括枚举值,也可以将没有缓存的提前查询出来给

            System.Linq.Expressions.Expression<Func<tb_FM_PayeeInfo, int?>> exprCheckMode;
            exprCheckMode = (p) => p.Account_type;
            base.ColNameDataDictionary.TryAdd(exprCheckMode.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(AccountType)));
            DisplayTextResolver.AddFixedDictionaryMappingByEnum(t => t.Account_type, typeof(AccountType));
        }


        public override void QueryConditionBuilder()
        {

            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_PayeeInfo).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

            //创建表达式
            var lambda = Expressionable.Create<tb_FM_PayeeInfo>()
                             //.And(t => t.Is_enabled == true)
                             .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制只看到自己的
                            .ToExpression();//注意 这一句 不能少
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);

            //清空过滤条件，因为这个基本数据 需要显示出来 
            //QueryConditionFilter.FilterLimitExpressions.Clear();

        }

        protected override Task<bool> Delete()
        {
            tb_FM_PayeeInfo payinfo = (tb_FM_PayeeInfo)this.bindingSourceList.Current;
            //指向员工收款信息时。只能自己删除 或超级用户来删除
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                if ((payinfo.Employee_ID.HasValue && payinfo.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID))
                {
                    //只能删除自己的收款信息。
                    MessageBox.Show("只能删除自己的收款信息。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return Task.FromResult(false);
                }
            }

            return base.Delete();
        }



        /// <summary>
        /// 特殊 处理
        /// 本位币只能一行一个币种有效
        /// </summary>
        public async override Task<List<tb_FM_PayeeInfo>> Save()
        {
            tb_FM_PayeeInfo currency = new tb_FM_PayeeInfo();
            tb_FM_PayeeInfoController<tb_FM_PayeeInfo> pctr = Startup.GetFromFac<tb_FM_PayeeInfoController<tb_FM_PayeeInfo>>();
            //这里是否要用保存列表来处理
            foreach (var item in bindingSourceList.List)
            {
                var entity = item as tb_FM_PayeeInfo;
                switch (entity.ActionStatus)
                {
                    case ActionStatus.无操作:
                        break;
                    case ActionStatus.新增:
                    case ActionStatus.修改:

                        ReturnResults<tb_FM_PayeeInfo> rr = new ReturnResults<tb_FM_PayeeInfo>();
                        rr = await pctr.SaveOrUpdate(entity);
                        if (rr.Succeeded)
                        {
                            currency = rr.ReturnObject;
                            ToolBarEnabledControl(MenuItemEnums.保存);
                            //根据要缓存的列表集合来判断是否需要上传到服务器。让服务器分发到其他客户端
                            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                            //只处理需要缓存的表
                            if (MyCacheManager.Instance.NewTableList.TryGetValue(typeof(tb_FM_PayeeInfo).Name, out pair))
                            {
                                //如果有更新变动就上传到服务器再分发到所有客户端
#warning TODO: 这里需要完善具体逻辑，当前仅为占位
                                //OriginalData odforCache = ActionForClient.更新缓存<tb_FM_PayeeInfo>(rr.ReturnObject);
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


            // 这里应该要去查询，集合再修改。不是在当前集合中去找。如果当前集合的条件 找到的一行数据。没有出现这个客户名下的其它的收款信息。则不会修改到
            //TODO by watson!!!!!!
            List<tb_FM_PayeeInfo> list = new List<tb_FM_PayeeInfo>();
            foreach (var item in bindingSourceList.List)
            {
                var entity = item as tb_FM_PayeeInfo;
                list.Add(entity);
            }
            if (currency.IsDefault)
            {
                //其他行是否
                var customersPayeeInfo = list.Where(t => t.CustomerVendor_ID == currency.CustomerVendor_ID && t.PayeeInfoID != currency.PayeeInfoID).ToList();

                customersPayeeInfo.ForEach(w => w.IsDefault = false);
                //保存
                await MainForm.Instance.AppContext.Db.Updateable<tb_FM_PayeeInfo>(customersPayeeInfo)
                      .UpdateColumns(it => new { it.IsDefault })
                    .ExecuteCommandHasChangeAsync();
                for (int i = 0; i < customersPayeeInfo.Count; i++)
                {
                    //根据要缓存的列表集合来判断是否需要上传到服务器。让服务器分发到其他客户端
                    KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                    //只处理需要缓存的表
                    if (MyCacheManager.Instance.NewTableList.TryGetValue(typeof(tb_FM_PayeeInfo).Name, out pair))
                    {
                        //如果有更新变动就上传到服务器再分发到所有客户端
#warning TODO: 这里需要完善具体逻辑，当前仅为占位
                        /*
                        OriginalData odforCache = ActionForClient.更新缓存<tb_FM_PayeeInfo>(customersPayeeInfo[i]);
                        byte[] buffer = CryptoProtocol.EncryptClientPackToServer(odforCache);
                        MainForm.Instance.ecs.client.Send(buffer);*/
                    }
                }
            }

            return list;
        }

        private void frm_Load(object sender, EventArgs e)
        {

        }
    }
}
