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
    /// 产品类目帮助类，提供树形结构绑定功能
    /// </summary>
    public static class UIProdCateHelper
    {
        /// <summary>
        /// 最大类目层级深度限制，防止循环引用导致的死循环
        /// </summary>
        private const int MAX_CATEGORY_DEPTH = 10;

        /// <summary>
        /// 刷新菜单树
        /// </summary>
        /// <param name="list">类目列表</param>
        /// <param name="tree_MainMenu">树形控件</param>
        public static void BindToTreeView(List<tb_ProdCategories> list, TreeView tree_MainMenu)
        {
            LoadTree(list, tree_MainMenu);
            if (tree_MainMenu.Nodes.Count > 0)
            {
                tree_MainMenu.Nodes[0].Expand(); //
            }
        }

        /// <summary>
        /// 刷新菜单树（无根节点）
        /// </summary>
        /// <param name="list">类目列表</param>
        /// <param name="tree_MainMenu">树形控件</param>
        public static void BindToTreeViewNoRootNode(List<tb_ProdCategories> list, TreeView tree_MainMenu)
        {
            LoadTreeNoRootNode(list, tree_MainMenu);
            if (tree_MainMenu.Nodes.Count > 0)
            {
                tree_MainMenu.Nodes[0].Expand(); //
            }
        }

        /// <summary>
        /// 加载树形结构（无根节点）
        /// </summary>
        /// <param name="list">类目列表</param>
        /// <param name="tree_MainMenu">树形控件</param>
        private static void LoadTreeNoRootNode(List<tb_ProdCategories> list, System.Windows.Forms.TreeView tree_MainMenu)
        {
            tree_MainMenu.Nodes.Clear();
            Bind(tree_MainMenu.Nodes, list, 0, 1);
            tree_MainMenu.ExpandAll();
        }

        /// <summary>
        /// 加载树形结构
        /// </summary>
        /// <param name="list">类目列表</param>
        /// <param name="tree_MainMenu">树形控件</param>
        private static void LoadTree(List<tb_ProdCategories> list, System.Windows.Forms.TreeView tree_MainMenu)
        {
            tree_MainMenu.Nodes.Clear();
            TreeNode nodeRoot = new TreeNode();
            nodeRoot.Text = "类目根结节";
            nodeRoot.Name = "0";
            tree_MainMenu.Nodes.Add(nodeRoot);
            Bind(nodeRoot, list, 0, 1);
            tree_MainMenu.ExpandAll();
        }

        /// <summary>
        /// 递归绑定节点到树形控件（TreeNodeCollection 版本）
        /// </summary>
        /// <param name="nodes">节点集合</param>
        /// <param name="list">类目列表</param>
        /// <param name="nodeId">当前节点ID</param>
        /// <param name="depth">当前层级深度</param>
        private static void Bind(TreeNodeCollection nodes, List<tb_ProdCategories> list, long nodeId, int depth)
        {
            // 检查层级深度限制
            if (depth > MAX_CATEGORY_DEPTH)
            {
                System.Diagnostics.Debug.WriteLine($"警告：类目层级超过 {MAX_CATEGORY_DEPTH} 级，已停止继续加载子节点。父节点ID：{nodeId}");
                return;
            }

            var childList = list.FindAll(t => t.Parent_id == nodeId).OrderBy(t => t.Sort);
            foreach (var nodeObj in childList)
            {
                var node = new TreeNode();
                node.Name = nodeObj.Category_ID.ToString();
                node.Text = nodeObj.Category_name;
                node.Tag = nodeObj;
                nodes.Add(node);
                Bind(node, list, nodeObj.Category_ID, depth + 1);
            }
        }

        /// <summary>
        /// 递归绑定节点到树形控件（TreeNode 版本）
        /// </summary>
        /// <param name="parentNode">父节点</param>
        /// <param name="list">类目列表</param>
        /// <param name="nodeId">当前节点ID</param>
        /// <param name="depth">当前层级深度</param>
        private static void Bind(TreeNode parentNode, List<tb_ProdCategories> list, long nodeId, int depth)
        {
            // 检查层级深度限制
            if (depth > MAX_CATEGORY_DEPTH)
            {
                System.Diagnostics.Debug.WriteLine($"警告：类目层级超过 {MAX_CATEGORY_DEPTH} 级，已停止继续加载子节点。父节点ID：{nodeId}");
                return;
            }

            var childList = list.FindAll(t => t.Parent_id == nodeId).OrderBy(t => t.Sort);
            foreach (var nodeObj in childList)
            {
                var node = new TreeNode();
                node.Name = nodeObj.Category_ID.ToString();
                node.Text = nodeObj.Category_name;
                node.Tag = nodeObj;
                parentNode.Nodes.Add(node);
                Bind(node, list, nodeObj.Category_ID, depth + 1);
            }
        }






        /// <summary>
        /// 刷新菜单树
        /// </summary>
        public static void BindToTreeView(List<tb_ProdCategories> list, KryptonTreeView tree_MainMenu, bool Expand = false)
        {
            LoadTree(list, tree_MainMenu);
            if (tree_MainMenu.Nodes.Count > 0)
            {
                if (Expand)
                {
                    tree_MainMenu.Nodes[0].Expand(); //
                }
                tree_MainMenu.TopNode = tree_MainMenu.Nodes[0];
            }
        }
        /// <summary>
        /// 加载树形结构（KryptonTreeView 版本）
        /// </summary>
        /// <param name="list">类目列表</param>
        /// <param name="tree_MainMenu">树形控件</param>
        /// <param name="Expand">是否展开所有节点</param>
        private static void LoadTree(List<tb_ProdCategories> list, KryptonTreeView tree_MainMenu, bool Expand = false)
        {
            tree_MainMenu.Nodes.Clear();
            TreeNode nodeRoot = new TreeNode();
            nodeRoot.Text = "类目根结节";
            nodeRoot.Name = "0";
            tree_MainMenu.Nodes.Add(nodeRoot);
            Bind(nodeRoot, list, 0, 1);
            if (Expand)
            {
                tree_MainMenu.ExpandAll();
            }
        }



    }
}
