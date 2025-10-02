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

using System.Threading;
using RUINORERP.Global;
using RUINORERP.PacketSpec.Models.Core;


namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("公司设置", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统参数)]
    public partial class UCCompanyList : BaseForm.BaseListGeneric<tb_Company>
    {
        public UCCompanyList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCCompanyEdit);
        }


        private async Task<object> QueryData(string msg)
        {
#warning TODO: 这里需要完善具体逻辑，当前仅为占位

            //发送一个数据。等待
            OriginalData gd = new OriginalData();
            byte[] buffer = new byte[0];
            string tempHexStr = string.Empty;
            tempHexStr = "C7E700FFDE565FDB1BD5FC83EA7CB9F64A6EDB0AD5E13F78A67B56FB3CD6477E1A5E0CA094EA07C238B90434C3BA13C26FFD8DCB74489EAFDA9EDB546A74E46188E738489BD09A12DD38A28E0F327DA38939950AE8EB1D89695205108EEC4CF90BE952DD7C13D81CFC460C274D8CFEC44E0D900B5D94034F546A65FD6B35EFA6D04A9EC620BE484E089D1DDF164EBE343DAB53E11F302178083E6F8030357D3EF97F47F536107C54D96B1987B93AC2D8B21857090D98481CA16CBAA4D118374A55F4E4AFFA8D9D52C478CA317AF1BF262FF3AC5C818D66E35C4FEB5949072A5D9E5A96C0200590E6753D626F4E761C9EECB2796CC307584F91FC4E9C921985FC03A771D116F3C53FB61F7B8146D87852B9D3323B64E8C755B891F4CA2DF177FFC570F9180B9F7C165871C4470FD3D1A92851AE849A82E9E14FEC8E1C876EB1FC38510B0380450687496762379D2C7E5C3BBCF2147C51B34043258E00E102B5ABD5015C01FDB4F2C54FF59AD40F";
         //   buffer = HexStrTobyte(tempHexStr.Trim());
            //MainForm.Instance.socketSession.AddSendDataEnqueue(buffer);
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            return await ExecuteAsync(tokenSource.Token);
            //这里查询要不要取消
            //tokenSource.Cancel();
        }

        protected override async Task Add()
        {
            if (ListDataSoure.Count == 0)
            {
                await base.Add();
                base.toolStripButtonModify.Enabled = false;
            }
            else
            {
                if (MainForm.Instance.AppContext.CanUsefunctionModules.Contains(GlobalFunctionModule.多公司经营功能))
                {
                    if (MessageBox.Show("您确定多公司模式运营吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        base.Add();
                        base.toolStripButtonModify.Enabled = false;
                    }
                }
                else
                {
                    //您没有使用多公司模式运营的权限。请购买后使用
                    MessageBox.Show("您没有使用多公司模式运营的权限。请购买后使用", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    base.toolStripButtonModify.Enabled = false;
                    base.toolStripButtonAdd.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 要保留一条记录
        /// </summary>
        /// <returns></returns>
        protected override Task<bool> Delete()
        {
            if (ListDataSoure.Count == 1)
            {
                return Task.FromResult(false);
            }
            else
            {
                return base.Delete();
            }
        }

        public async ValueTask<object> ExecuteAsync(CancellationToken token)
        {
            int counter = 0;
            while (true)
            {
                try
                {
                    //if (MainForm.Instance.socketSession.ServerMsg != null)
                    //{
                    //    return MainForm.Instance.socketSession.ServerMsg;
                    //}
                    //超时等待
                    //await Task.Delay(interval, token);
                    await Task.Delay(1000, token);
                    counter++;
                    if (counter >= 3)
                    {

                    }
                }
                catch (Exception)
                {

                }
                if (token.IsCancellationRequested)
                {
                    break;
                }
            }
            return null;
        }







    }
}
