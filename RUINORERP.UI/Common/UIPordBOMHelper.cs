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
    public static class UIPordBOMHelper
    {

        /// <summary>
        /// 刷新菜单树
        /// </summary>
        public static void BindToTreeView(tb_BOM_S main, List<tb_BOM_SDetail> list, TreeView tree_MainMenu)
        {
            tree_MainMenu.Nodes.Clear();
            LoadTree(main,list, tree_MainMenu);
            if (tree_MainMenu.Nodes.Count > 0)
            {
                tree_MainMenu.Nodes[0].Expand(); //
            }
        }

        /// <summary>
        /// 刷新菜单树
        /// </summary>
        public static void BindToTreeViewNoRootNode(List<tb_BOM_SDetail> list, TreeView tree_MainMenu)
        {
            LoadTreeNoRootNode(list, tree_MainMenu);
            if (tree_MainMenu.Nodes.Count > 0)
            {
                tree_MainMenu.Nodes[0].Expand(); //
            }
        }

        private static void LoadTreeNoRootNode(List<tb_BOM_SDetail> list, System.Windows.Forms.TreeView tree_MainMenu)
        {
            tree_MainMenu.Nodes.Clear();
            Bind(tree_MainMenu.Nodes, list, 0);
            tree_MainMenu.ExpandAll();
        }


        private static void LoadTree(tb_BOM_S main, List<tb_BOM_SDetail> list, System.Windows.Forms.TreeView tree_MainMenu)
        {
            tree_MainMenu.Nodes.Clear();
            TreeNode nodeRoot = new TreeNode();
            nodeRoot.Text =main.BOM_No + "【" + main.BOM_Name + "】"; 
            nodeRoot.Name = main.BOM_ID.ToString();
            tree_MainMenu.Nodes.Add(nodeRoot);
            Bind(nodeRoot, list, main.BOM_ID);
            tree_MainMenu.ExpandAll();
        }




        //递归方法
        private static void Bind(TreeNodeCollection Nodes, List<tb_BOM_SDetail> list, long nodeId)
        {
            var childList = list.FindAll(t => t.BOM_ID == nodeId).OrderBy(t => t.Sort);
            foreach (var nodeObj in childList)
            {
                var node = new TreeNode();
                node.Name = nodeObj.SubID.ToString();
                node.Text = nodeObj.SubItemName;
                node.Tag = nodeObj;
                Nodes.Add(node);
                if (nodeObj.tb_proddetail.BOM_ID.HasValue)
                {
                    Bind(node, list, nodeObj.tb_proddetail.BOM_ID.Value);
                }
            }
        }

        //递归方法
        private static void Bind(TreeNode parNode, List<tb_BOM_SDetail> list, long nodeId)
        {
            var childList = list.FindAll(t => t.BOM_ID == nodeId).OrderBy(t => t.Type_ID).ThenBy(c => c.Sort);
            foreach (var nodeObj in childList)
            {
                var node = new TreeNode();
                node.Name = nodeObj.SubID.ToString();
                node.Text = nodeObj.SubItemName+"【"+nodeObj.SubItemSpec+"】";
                node.ToolTipText = nodeObj.SubItemSpec;
                node.Tag = nodeObj;
                parNode.Nodes.Add(node);
                //实际看是不能导航出来 如果有子件。带不出就要查询 一下
                if (nodeObj.tb_proddetail!=null)
                {
                    if (nodeObj.tb_proddetail.BOM_ID.HasValue)
                    {
                        Bind(node, list, nodeObj.tb_proddetail.BOM_ID.Value);
                    }
                }
                
            }
        }






        /// <summary>
        /// 刷新菜单树
        /// </summary>
        public static void BindToTreeView(List<tb_BOM_SDetail> list, KryptonTreeView tree_MainMenu)
        {
            LoadTree(list, tree_MainMenu);
            if (tree_MainMenu.Nodes.Count > 0)
            {
                tree_MainMenu.Nodes[0].Expand(); //
            }
        }
        private static void LoadTree(List<tb_BOM_SDetail> list, KryptonTreeView tree_MainMenu)
        {
            tree_MainMenu.Nodes.Clear();
            TreeNode nodeRoot = new TreeNode();
            nodeRoot.Text = "类目根结节";
            nodeRoot.Name = "0";
            tree_MainMenu.Nodes.Add(nodeRoot);
            Bind(nodeRoot, list, 0);
            tree_MainMenu.ExpandAll();
        }



    }
}
