using System;
using System.Windows.Forms;

namespace RUINOR.WinFormsUI.TreeViewColumns.TreeViewColumnsDemo
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			TreeNode treeNode = new TreeNode("test");
			treeNode.Tag = new string[] { "col1", "col2" };

			// Some random node
			this.treeViewColumns1.TreeView.Nodes[0].Nodes[0].Nodes.Add(treeNode);

			this.treeViewColumns1.TreeView.SelectedNode = treeNode;
		}
	}
}