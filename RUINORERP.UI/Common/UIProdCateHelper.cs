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
    public static class UIProdCateHelper
    {

        /// <summary>
        /// 刷新菜单树
        /// </summary>
        public static void BindToTreeView(List<tb_ProdCategories> list, TreeView tree_MainMenu)
        {

            LoadTree(list, tree_MainMenu);
            if (tree_MainMenu.Nodes.Count > 0)
            {
                tree_MainMenu.Nodes[0].Expand(); //
            }
        }

        /// <summary>
        /// 刷新菜单树
        /// </summary>
        public static void BindToTreeViewNoRootNode(List<tb_ProdCategories> list, TreeView tree_MainMenu)
        {
            LoadTreeNoRootNode(list, tree_MainMenu);
            if (tree_MainMenu.Nodes.Count > 0)
            {
                tree_MainMenu.Nodes[0].Expand(); //
            }
        }

        private static void LoadTreeNoRootNode(List<tb_ProdCategories> list, System.Windows.Forms.TreeView tree_MainMenu)
        {
            tree_MainMenu.Nodes.Clear();
            Bind(tree_MainMenu.Nodes, list, 0);
            tree_MainMenu.ExpandAll();
        }


        private static void LoadTree(List<tb_ProdCategories> list, System.Windows.Forms.TreeView tree_MainMenu)
        {
            tree_MainMenu.Nodes.Clear();
            TreeNode nodeRoot = new TreeNode();
            nodeRoot.Text = "类目根结节";
            nodeRoot.Name = "0";
            tree_MainMenu.Nodes.Add(nodeRoot);
            Bind(nodeRoot, list, 0);
            tree_MainMenu.ExpandAll();
        }




        //递归方法
        private static void Bind(TreeNodeCollection Nodes, List<tb_ProdCategories> list, long nodeId)
        {
            var childList = list.FindAll(t => t.Parent_id == nodeId).OrderBy(t => t.Sort);
            foreach (var nodeObj in childList)
            {
                var node = new TreeNode();
                node.Name = nodeObj.Category_ID.ToString();
                node.Text = nodeObj.Category_name;
                node.Tag = nodeObj;
                Nodes.Add(node);
                Bind(node, list, nodeObj.Category_ID);
            }
        }

        //递归方法1
        private static void Bind(TreeNode parNode, List<tb_ProdCategories> list, long nodeId)
        {
            var childList = list.FindAll(t => t.Parent_id == nodeId).OrderBy(t => t.Sort);
            foreach (var nodeObj in childList)
            {
                var node = new TreeNode();
                node.Name = nodeObj.Category_ID.ToString();
                node.Text = nodeObj.Category_name;
                node.Tag = nodeObj;
                parNode.Nodes.Add(node);
                Bind(node, list, nodeObj.Category_ID);
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
        private static void LoadTree(List<tb_ProdCategories> list, KryptonTreeView tree_MainMenu, bool Expand = false)
        {
            tree_MainMenu.Nodes.Clear();
            TreeNode nodeRoot = new TreeNode();
            nodeRoot.Text = "类目根结节";
            nodeRoot.Name = "0";
            tree_MainMenu.Nodes.Add(nodeRoot);
            Bind(nodeRoot, list, 0);
            if (Expand)
            {
                tree_MainMenu.ExpandAll();
            }
            
        }



    }
}
