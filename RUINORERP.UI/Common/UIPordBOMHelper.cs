using Krypton.Toolkit;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 配方（BOM）帮助类，提供配方树形结构绑定功能
    /// </summary>
    public static class UIPordBOMHelper
    {
        /// <summary>
        /// 最大BOM层级深度限制，防止循环引用导致的死循环
        /// </summary>
        private const int MAX_BOM_DEPTH = 10;

        /// <summary>
        /// 刷新BOM树
        /// </summary>
        /// <param name="main">BOM主表信息</param>
        /// <param name="list">BOM明细列表</param>
        /// <param name="tree_MainMenu">树形控件</param>
        public static void BindToTreeView(tb_BOM_S main, List<tb_BOM_SDetail> list, TreeView tree_MainMenu)
        {
            tree_MainMenu.Nodes.Clear();
            LoadTree(main, list, tree_MainMenu);
            if (tree_MainMenu.Nodes.Count > 0)
            {
                tree_MainMenu.Nodes[0].Expand();
            }
        }

        /// <summary>
        /// 刷新BOM树（无根节点）
        /// </summary>
        /// <param name="list">BOM明细列表</param>
        /// <param name="tree_MainMenu">树形控件</param>
        public static void BindToTreeViewNoRootNode(List<tb_BOM_SDetail> list, TreeView tree_MainMenu)
        {
            LoadTreeNoRootNode(list, tree_MainMenu);
            if (tree_MainMenu.Nodes.Count > 0)
            {
                tree_MainMenu.Nodes[0].Expand();
            }
        }

        /// <summary>
        /// 加载BOM树（无根节点）
        /// </summary>
        /// <param name="list">BOM明细列表</param>
        /// <param name="tree_MainMenu">树形控件</param>
        private static void LoadTreeNoRootNode(List<tb_BOM_SDetail> list, System.Windows.Forms.TreeView tree_MainMenu)
        {
            tree_MainMenu.Nodes.Clear();
            Bind(tree_MainMenu.Nodes, list, 0, 1);
            tree_MainMenu.ExpandAll();
        }

        /// <summary>
        /// 加载BOM树
        /// </summary>
        /// <param name="main">BOM主表信息</param>
        /// <param name="list">BOM明细列表</param>
        /// <param name="tree_MainMenu">树形控件</param>
        private static void LoadTree(tb_BOM_S main, List<tb_BOM_SDetail> list, System.Windows.Forms.TreeView tree_MainMenu)
        {
            tree_MainMenu.Nodes.Clear();
            TreeNode nodeRoot = new TreeNode();
            nodeRoot.Text = main.BOM_No + "【" + main.BOM_Name + "】";
            nodeRoot.Name = main.BOM_ID.ToString();
            tree_MainMenu.Nodes.Add(nodeRoot);
            Bind(nodeRoot, list, main.BOM_ID, 1);
            tree_MainMenu.ExpandAll();
        }

        /// <summary>
        /// 递归绑定BOM节点到树形控件（TreeNodeCollection 版本）
        /// </summary>
        /// <param name="nodes">节点集合</param>
        /// <param name="list">BOM明细列表</param>
        /// <param name="nodeId">当前节点ID</param>
        /// <param name="depth">当前层级深度</param>
        private static void Bind(TreeNodeCollection nodes, List<tb_BOM_SDetail> list, long nodeId, int depth)
        {
            // 检查层级深度限制
            if (depth > MAX_BOM_DEPTH)
            {
                System.Diagnostics.Debug.WriteLine($"警告：BOM层级超过 {MAX_BOM_DEPTH} 级，已停止继续加载子节点。父节点BOM_ID：{nodeId}");
                return;
            }

            var childList = list.FindAll(t => t.BOM_ID == nodeId).OrderBy(t => t.Sort);
            foreach (var nodeObj in childList)
            {
                var node = new TreeNode();
                node.Name = nodeObj.SubID.ToString();
                node.Text = nodeObj.view_ProdInfo.Specifications;
                node.Tag = nodeObj;
                nodes.Add(node);
                if (nodeObj.tb_proddetail?.BOM_ID.HasValue == true)
                {
                    Bind(node, list, nodeObj.tb_proddetail.BOM_ID.Value, depth + 1);
                }
            }
        }

        /// <summary>
        /// 递归绑定BOM节点到树形控件（TreeNode 版本）
        /// </summary>
        /// <param name="parentNode">父节点</param>
        /// <param name="list">BOM明细列表</param>
        /// <param name="nodeId">当前节点ID</param>
        /// <param name="depth">当前层级深度</param>
        private static void Bind(TreeNode parentNode, List<tb_BOM_SDetail> list, long nodeId, int depth)
        {
            // 检查层级深度限制
            if (depth > MAX_BOM_DEPTH)
            {
                System.Diagnostics.Debug.WriteLine($"警告：BOM层级超过 {MAX_BOM_DEPTH} 级，已停止继续加载子节点。父节点BOM_ID：{nodeId}");
                return;
            }

            var childList = list.FindAll(t => t.BOM_ID == nodeId).OrderBy(t => t.view_ProdInfo.Type_ID).ThenBy(c => c.Sort);
            foreach (var nodeObj in childList)
            {
                var node = new TreeNode();
                node.Name = nodeObj.SubID.ToString();
                node.Text = nodeObj.view_ProdInfo.CNName + "【" + nodeObj.view_ProdInfo.Specifications + "】";
                node.ToolTipText = nodeObj.view_ProdInfo.Specifications;
                node.Tag = nodeObj;
                parentNode.Nodes.Add(node);

                // 如果有子BOM，递归加载子节点
                if (nodeObj.tb_proddetail?.BOM_ID.HasValue == true)
                {
                    Bind(node, list, nodeObj.tb_proddetail.BOM_ID.Value, depth + 1);
                }
            }
        }






        /// <summary>
        /// 刷新BOM树（KryptonTreeView 版本）
        /// </summary>
        /// <param name="list">BOM明细列表</param>
        /// <param name="tree_MainMenu">树形控件</param>
        public static void BindToTreeView(List<tb_BOM_SDetail> list, KryptonTreeView tree_MainMenu)
        {
            LoadTree(list, tree_MainMenu);
            if (tree_MainMenu.Nodes.Count > 0)
            {
                tree_MainMenu.Nodes[0].Expand();
            }
        }

        /// <summary>
        /// 加载BOM树（KryptonTreeView 版本）
        /// </summary>
        /// <param name="list">BOM明细列表</param>
        /// <param name="tree_MainMenu">树形控件</param>
        private static void LoadTree(List<tb_BOM_SDetail> list, KryptonTreeView tree_MainMenu)
        {
            tree_MainMenu.Nodes.Clear();
            TreeNode nodeRoot = new TreeNode();
            nodeRoot.Text = "BOM根节点";
            nodeRoot.Name = "0";
            tree_MainMenu.Nodes.Add(nodeRoot);
            Bind(nodeRoot, list, 0, 1);
            tree_MainMenu.ExpandAll();
        }
    }
}
