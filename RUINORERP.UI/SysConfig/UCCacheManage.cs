using AutoUpdateTools;
using CacheManager.Core;
using FastReport.DevComponents.DotNetBar.Controls;
using FastReport.Table;
using HLH.Lib.Helper;
using NPOI.SS.Formula.Functions;
using RUINORERP.Business.CommService;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebSockets;
using System.Windows.Forms;
using System.Windows.Media;

namespace RUINORERP.UI.SysConfig
{
    [MenuAttrAssemblyInfo("缓存管理", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统工具)]
    public partial class UCCacheManage : UserControl
    {
        public UCCacheManage()
        {
            InitializeComponent();
        }

        private void 请求缓存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBoxTableList.SelectedItem is SuperValue kv)
            {
                string tableName = kv.superDataTypeName;
                UIBizSrvice.RequestCache(tableName);
            }
        }

        private void UCCacheManage_Load(object sender, EventArgs e)
        {
            //这里添加所有缓存
            LoadCacheToUI();
        }

        private void LoadCacheToUI()
        {
            //加载所有缓存的表
            listBoxTableList.Items.Clear();

            List<string> list = new List<string>();
            foreach (var tableName in BizCacheHelper.Manager.NewTableList.Keys)
            {
                list.Add(tableName);
            }
            list.Sort();
            foreach (var tableName in list)
            {

                var CacheList = BizCacheHelper.Manager.CacheEntityList.Get(tableName);
                if (CacheList == null)
                {
                    SuperValue kv = new SuperValue(tableName + "[" + 0 + "]", tableName);
                    listBoxTableList.Items.Add(kv);
                }
                else
                {
                    var lastlist = ((IEnumerable<dynamic>)CacheList).ToList();
                    if (lastlist != null)
                    {
                        SuperValue kv = new SuperValue(tableName + "[" + lastlist.Count + "]", tableName);
                        listBoxTableList.Items.Add(kv);
                    }

                }
            }

            //添加锁定信息
            if (MainForm.Instance.LockInfoList != null)
            {
                SuperValue kv = new SuperValue("锁定信息列表" + "[" + MainForm.Instance.LockInfoList.Count + "]", "锁定信息列表");
                listBoxTableList.Items.Add(kv);
            }
        }

        private void btnRefreshCache_Click(object sender, EventArgs e)
        {
            LoadCacheToUI();
        }

        /// <summary>
        /// 所有实体表都在这个命名空间下，不需要每次都反射
        /// </summary>
        Assembly assembly = System.Reflection.Assembly.LoadFrom("RUINORERP.Model.dll");
        private void listBoxTableList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxTableList.SelectedItem is SuperValue kv)
            {
                string tableName = kv.superDataTypeName;

                if (tableName == "锁定信息列表")
                {
                    List<BillLockInfo> lockInfoList = new List<BillLockInfo>();
                    foreach (var item in MainForm.Instance.LockInfoList)
                    {
                        lockInfoList.Add(item.Value);
                    }
                    this.dataGridView1.SetUseCustomColumnDisplay(false);
                    dataGridView1.DataSource = lockInfoList;
                }
                else
                {
                    var CacheList = BizCacheHelper.Manager.CacheEntityList.Get(tableName);
                    if (CacheList == null)
                    {
                        dataGridView1.DataSource = null;
                        return;
                    }
                    // 使用 Assembly.Load 加载包含 PrintHelper<T> 类的程序集

                    // 使用 GetType 方法获取 PrintHelper<T> 的类型
                    Type type = assembly.GetType("RUINORERP.Model." + tableName);
                    dataGridView1.FieldNameList = UIHelper.GetFieldNameColList(true, type);
                    dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView1.XmlFileName = "UCCacheManage" + tableName;
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = CacheList;
                }

            }
            else
            {
                dataGridView1.DataSource = null;
                return;
            }

        }

        private void 清空选中缓存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBoxTableList.SelectedItem is SuperValue kv)
            {
                string tableName = kv.superDataTypeName;
                BizCacheHelper.Manager.CacheEntityList.Remove(tableName);
                CacheInfo lastCacheInfo = new CacheInfo(tableName, 0);
                lastCacheInfo.HasExpire = false;
                //看是更新好。还是移除好。主要看后面的更新机制。
                MyCacheManager.Instance.CacheInfoList.AddOrUpdate(tableName, lastCacheInfo, c => lastCacheInfo);


            }
        }
    }
}
