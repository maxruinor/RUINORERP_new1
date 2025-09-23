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
using SourceGrid2.Win32;
using Newtonsoft.Json;
using RUINORERP.Model.ConfigModel;

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("系统参数设置", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统参数)]
    public partial class UCSystemConfigList : BaseForm.BaseListGeneric<tb_SystemConfig>
    {

        protected override async Task Add()
        {
            if (ListDataSoure.Count == 0)
            {
              await  base.Add();
                base.toolStripButtonModify.Enabled = false;
            }
            else
            {
                base.toolStripButtonModify.Enabled = false;
                base.toolStripButtonAdd.Enabled = false;
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


        public override async Task<List<tb_SystemConfig>> Save()
        {
            BindingSortCollection<tb_SystemConfig> list = new BindingSortCollection<tb_SystemConfig>();
            list = bindingSourceList.List as BindingSortCollection<tb_SystemConfig>;
            foreach (var item in list)
            {
                if (item.CheckNegativeInventory)
                {
                    if (MessageBox.Show("不建议开启负库存。\r\n你确定要开启吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        await base.Save();
                    }
                    else
                    {
                        MainForm.Instance.PrintInfoLog("配置信息未保存。");
                    }
                }

                else
                {
                    await base.Save();
                }
            }

            if (list.Count == 1)
            {
                MainForm.Instance.AppContext.SysConfig = list[0];
                //财务模块。直接解析一下
                #region 自定义类的配置

                try
                {
                    MainForm.Instance.AppContext.FMConfig = JsonConvert.DeserializeObject<FMConfiguration>(list[0].FMConfig);
                    MainForm.Instance.AppContext.FunctionConfig = JsonConvert.DeserializeObject<FunctionConfiguration>(list[0].FunctionConfiguration);
                }
                catch (Exception)
                {

                }

                #endregion
         
            }

            return list.ToList();
        }



        public UCSystemConfigList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCSystemConfigEdit);
            //如果有一行数据 就无法增加，
            if (base.bindingSourceList.Count > 0)
            {

            }
        }
    }
}
