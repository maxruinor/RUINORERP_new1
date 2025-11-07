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
    public static class UIFMSubjectHelper
    {

        /// <summary>
        /// 刷新菜单树
        /// </summary>
        public static void BindToTreeView(List<tb_FM_Subject> list, TreeView tree_MainMenu)
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
        public static void BindToTreeViewNoRootNode(List<tb_FM_Subject> list, TreeView tree_MainMenu)
        {
            LoadTreeNoRootNode(list, tree_MainMenu);
            if (tree_MainMenu.Nodes.Count > 0)
            {
                tree_MainMenu.Nodes[0].Expand(); //
            }
        }

        private static void LoadTreeNoRootNode(List<tb_FM_Subject> list, System.Windows.Forms.TreeView tree_MainMenu)
        {
            tree_MainMenu.Nodes.Clear();
            Bind(tree_MainMenu.Nodes, list, 0);
            tree_MainMenu.ExpandAll();
        }


        private static void LoadTree(List<tb_FM_Subject> list, System.Windows.Forms.TreeView tree_MainMenu)
        {
            tree_MainMenu.Nodes.Clear();
            TreeNode nodeRoot = new TreeNode();
            nodeRoot.Text = "科目根结节";
            nodeRoot.Name = "0";
            tree_MainMenu.Nodes.Add(nodeRoot);
            Bind(nodeRoot, list, 0);
            tree_MainMenu.ExpandAll();
        }




        //递归方法
        private static void Bind(TreeNodeCollection Nodes, List<tb_FM_Subject> list, long nodeId)
        {
            var childList = list.FindAll(t => t.Parent_subject_id == nodeId).OrderBy(t => t.Subject_Type);
            foreach (var nodeObj in childList)
            {
                var node = new TreeNode();
                node.Name = nodeObj.Subject_id.ToString();
                node.Text = nodeObj.subject_code + "【" + nodeObj.subject_name + "】";
                node.Tag = nodeObj;
                Nodes.Add(node);
                Bind(node, list, nodeObj.Subject_id);
            }
        }

        //递归方法
        private static void Bind(TreeNode parNode, List<tb_FM_Subject> list, long nodeId)
        {
            var childList = list.FindAll(t => t.Parent_subject_id == nodeId).OrderBy(t => t.Subject_Type);
            foreach (var nodeObj in childList)
            {
                var node = new TreeNode();
                node.Name = nodeObj.Subject_id.ToString();
                node.Text = nodeObj.subject_code+"【"+nodeObj.subject_name+"】";
                node.Tag = nodeObj;
                parNode.Nodes.Add(node);
                Bind(node, list, nodeObj.Subject_id);
            }
        }






        /// <summary>
        /// 刷新菜单树
        /// </summary>
        public static void BindToTreeView(List<tb_FM_Subject> list, KryptonTreeView tree_MainMenu)
        {
            LoadTree(list, tree_MainMenu);
            if (tree_MainMenu.Nodes.Count > 0)
            {
                tree_MainMenu.Nodes[0].Expand(); //
            }
        }
        private static void LoadTree(List<tb_FM_Subject> list, KryptonTreeView tree_MainMenu)
        {
            tree_MainMenu.Nodes.Clear();
            TreeNode nodeRoot = new TreeNode();
            nodeRoot.Text = "科目根结节";
            nodeRoot.Name = "0";
            tree_MainMenu.Nodes.Add(nodeRoot);
            Bind(nodeRoot, list, 0);
            tree_MainMenu.ExpandAll();
        }



    }


}
