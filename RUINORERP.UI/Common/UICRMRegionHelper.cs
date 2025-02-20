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
    public static class UICRMRegionHelper
    {

        /// <summary>
        /// 刷新菜单树
        /// </summary>
        public static void BindToTreeView(List<tb_CRM_Region> list, TreeView tree_MainMenu)
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
        public static void BindToTreeViewNoRootNode(List<tb_CRM_Region> list, TreeView tree_MainMenu)
        {
            LoadTreeNoRootNode(list, tree_MainMenu);
            if (tree_MainMenu.Nodes.Count > 0)
            {
                tree_MainMenu.Nodes[0].Expand(); //
            }
        }

        private static void LoadTreeNoRootNode(List<tb_CRM_Region> list, System.Windows.Forms.TreeView tree_MainMenu)
        {
            tree_MainMenu.Nodes.Clear();
            Bind(tree_MainMenu.Nodes, list, 0);
            tree_MainMenu.ExpandAll();
        }


        private static void LoadTree(List<tb_CRM_Region> list, System.Windows.Forms.TreeView tree_MainMenu)
        {
            tree_MainMenu.Nodes.Clear();
            TreeNode nodeRoot = new TreeNode();
            nodeRoot.Text = "区域根结节";
            nodeRoot.Name = "0";
            tree_MainMenu.Nodes.Add(nodeRoot);
            Bind(nodeRoot, list, 0);
            tree_MainMenu.ExpandAll();
        }




        //递归方法
        private static void Bind(TreeNodeCollection Nodes, List<tb_CRM_Region> list, long nodeId)
        {
            var childList = list.FindAll(t => t.Parent_region_id == nodeId).OrderBy(t => t.Region_code);
            foreach (var nodeObj in childList)
            {
                var node = new TreeNode();
                node.Name = nodeObj.Region_ID.ToString();
                node.Text = nodeObj.Region_code + "【" + nodeObj.Region_Name + "】";
                node.Tag = nodeObj;
                Nodes.Add(node);
                Bind(node, list, nodeObj.Region_ID);
            }
        }

        //递归方法
        private static void Bind(TreeNode parNode, List<tb_CRM_Region> list, long nodeId)
        {
            var childList = list.FindAll(t => t.Parent_region_id == nodeId).OrderBy(t => t.Region_code);
            foreach (var nodeObj in childList)
            {
                var node = new TreeNode();
                node.Name = nodeObj.Region_ID.ToString();
                node.Text = nodeObj.Region_code + "【"+nodeObj.Region_Name + "】";
                node.Tag = nodeObj;
                parNode.Nodes.Add(node);
                Bind(node, list, nodeObj.Region_ID);
            }
        }






        /// <summary>
        /// 刷新菜单树
        /// </summary>
        public static void BindToTreeView(List<tb_CRM_Region> list, KryptonTreeView tree_MainMenu)
        {
            LoadTree(list, tree_MainMenu);
            if (tree_MainMenu.Nodes.Count > 0)
            {
                tree_MainMenu.Nodes[0].Expand(); //
            }
        }
        private static void LoadTree(List<tb_CRM_Region> list, KryptonTreeView tree_MainMenu)
        {
            tree_MainMenu.Nodes.Clear();
            TreeNode nodeRoot = new TreeNode();
            nodeRoot.Text = "区域根结节";
            nodeRoot.Name = "0";
            tree_MainMenu.Nodes.Add(nodeRoot);
            Bind(nodeRoot, list, 0);
            tree_MainMenu.ExpandAll();
        }



    }


}
