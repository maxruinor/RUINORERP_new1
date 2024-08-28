using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using WorkflowCore.Interface;
using RUINORERP.UI.WorkFlowTester;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Base;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices;
using RUINORERP.Services;
using RUINORERP.Repository.Base;
using RUINORERP.IRepository.Base;
using RUINORERP.Global;
using RUINORERP.Model.Context;
using System.Linq.Expressions;
using RUINORERP.Extensions.Middlewares;
using SqlSugar;
using RUINORERP.UI.Report;
using System.Reflection;
using RUINORERP.Common.Helper;
using RUINORERP.Business;
using System.Collections.Concurrent;
using Org.BouncyCastle.Math.Field;
using RUINORERP.UI.PSI.PUR;
using FastReport.DevComponents.DotNetBar.Controls;
using RUINORERP.UI.SS;

namespace RUINORERP.UI.SysConfig
{
    [MenuAttrAssemblyInfo("数据校正中心", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统工具)]
    public partial class UCDataCorrectionCenter : UserControl
    {
        public UCDataCorrectionCenter()
        {
            InitializeComponent();
        }


        Type[] ModelTypes;
        /// <summary>
        /// 为了查找明细表名类型，保存所有类型名称方便查找
        /// </summary>
        List<string> typeNames = new List<string>();

        List<SugarTable> stlist = new List<SugarTable>();

        private void UCDataFix_Load(object sender, EventArgs e)
        {
            LoadTree();
        }

        private async void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (treeViewTableList.SelectedNode != null)
            {
                if (treeViewTableList.SelectedNode.Tag != null && treeViewTableList.SelectedNode.Name == typeof(tb_MenuInfo).Name)
                {
                    tb_MenuInfoController<tb_MenuInfo> ctrMenuInfo = Startup.GetFromFac<tb_MenuInfoController<tb_MenuInfo>>();
                    List<tb_MenuInfo> menuInfos = ctrMenuInfo.Query();
                    for (int i = 0; i < menuInfos.Count; i++)
                    {
                        if (menuInfos[i].MenuName == "生产需求分析查询")
                        {

                        }

                        //检查菜单设置中的枚举类型是不是和代码中的一致，因为代码可能会维护修改
                        if (menuInfos[i].BizType != null && menuInfos[i].BizType.HasValue)
                        {
                            int oldid = menuInfos[i].BizType.Value;

                            //新值来自枚举值硬编码
                            #region 取新值

                            // 获取当前正在执行的程序集
                            Assembly currentAssembly = Assembly.GetExecutingAssembly();

                            // 已知类的全名（包括命名空间）
                            string className = menuInfos[i].ClassPath;

                            // 获取类型对象
                            Type type = currentAssembly.GetType(className);

                            if (type != null)
                            {
                                // 使用 Activator 创建类的实例
                                object instance = Activator.CreateInstance(type);
                                var descType = typeof(MenuAttrAssemblyInfo);
                                // 类型是否为窗体，否则跳过，进入下一个循环
                                // 是否为自定义特性，否则跳过，进入下一个循环
                                if (!type.IsDefined(descType, false))
                                    continue;

                                // 强制为自定义特性
                                MenuAttrAssemblyInfo? attribute = type.GetCustomAttribute(descType, false) as MenuAttrAssemblyInfo;
                                // 如果强制失败或者不需要注入的窗体跳过，进入下一个循环
                                if (attribute == null || !attribute.Enabled)
                                    continue;
                                // 域注入
                                //Services.AddScoped(type);
                                MenuAttrAssemblyInfo info = new MenuAttrAssemblyInfo();
                                info = attribute;
                                info.ClassName = type.Name;
                                info.ClassType = type;
                                info.ClassPath = type.FullName;
                                info.Caption = attribute.Describe;
                                info.MenuPath = attribute.MenuPath;
                                info.UiType = attribute.UiType;
                                if (attribute.MenuBizType.HasValue)
                                {
                                    info.MenuBizType = attribute.MenuBizType.Value;
                                }

                                int newid = (int)((BizType)info.MenuBizType.Value);
                                if (oldid == newid)
                                {
                                    //richTextBoxLog.AppendText($"{menuInfos[i].MenuName}=>{menuInfos[i].BizType} ==>" + (BizType)menuInfos[i].BizType.Value + "\r\n");
                                }
                                else
                                {
                                    richTextBoxLog.AppendText($"{menuInfos[i].MenuName}=>{menuInfos[i].BizType} ==========不应该是===={(BizType)menuInfos[i].BizType.Value}=======应该是{newid}" + "\r\n");
                                    if (!chkTestMode.Checked)
                                    {
                                        menuInfos[i].BizType = newid;
                                        await ctrMenuInfo.UpdateMenuInfo(menuInfos[i]);
                                    }
                                }


                            }
                            else
                            {
                                Console.WriteLine("类型未找到");
                            }

                            #endregion


                        }


                    }


                }
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {


        }

        private void LoadTree()
        {
            Assembly dalAssemble = System.Reflection.Assembly.LoadFrom("RUINORERP.Model.dll");
            ModelTypes = dalAssemble.GetExportedTypes();
            typeNames = ModelTypes.Select(m => m.Name).ToList();
            treeViewTableList.Nodes.Clear();
            stlist.Clear();
            foreach (var type in ModelTypes)
            {
                var attrs = type.GetCustomAttributes<SugarTable>();
                foreach (var attr in attrs)
                {
                    if (attr is SugarTable st)
                    {
                        //var t = Startup.ServiceProvider.GetService(type);//SugarColumn 或进一步取字段特性也可以
                        //var t = Startup.ServiceProvider.CreateInstance(type);//SugarColumn 或进一步取字段特性也可以
                        if (st.TableName.Contains("tb_") && !type.Name.Contains("QueryDto"))
                        {
                            //if (txtTableName.Text.Trim().Length > 0 && st.TableName.Contains(txtTableName.Text.Trim()))
                            //{
                            if (st.TableName.Contains("tb_MenuInfo") || st.TableName == "tb_MenuInfo")
                            {

                            }
                            TreeNode node = new TreeNode(st.TableName);
                            node.Name = st.TableName;
                            node.Tag = type;
                            treeViewTableList.Nodes.Add(node);
                            stlist.Add(st);
                            //}
                        }
                        continue;
                    }
                }
            }
        }


        /// <summary>
        /// 树形框-单选模式的实现,放在事件 _AfterCheck下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse && sender is TreeView tv)
            {
                if (e.Node.Checked)
                {
                    tv.SelectedNode = e.Node;
                    CancelCheckedExceptOne(tv.Nodes, e.Node);
                }
            }
            //TreeViewSingleSelectedAndChecked(TreeView1, e);
            e.Node.Checked = true;
            node_AfterCheck(sender, e);

        }



        void node_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // only do it if the node became checked:
            if (e.Node.Checked)
            {
                // for all the nodes in the tree...
                foreach (TreeNode cur_node in e.Node.TreeView.Nodes)
                {
                    // ... which are not the freshly checked one...
                    if (cur_node != e.Node)
                    {
                        // ... uncheck them
                        cur_node.Checked = false;
                    }
                }
            }
        }


        /// <summary>
        /// 树形框-单选模式的实现,放在事件 _AfterCheck下
        /// </summary>
        /// <param name="tv"></param>
        /// <param name="e"></param>
        public static void TreeViewSingleSelectedAndChecked(Krypton.Toolkit.KryptonTreeView tv, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                if (e.Node.Checked)
                {
                    tv.SelectedNode = e.Node;
                    CancelCheckedExceptOne(tv.Nodes, e.Node);
                }
            }
        }

        private static void CancelCheckedExceptOne(TreeNodeCollection tnc, TreeNode tn)
        {
            foreach (TreeNode item in tnc)
            {
                if (item != tn)
                    item.Checked = false;
                if (item.Nodes.Count > 0)
                    CancelCheckedExceptOne(item.Nodes, tn);

            }
        }

    }
}
