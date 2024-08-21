using System.Drawing;
using System.Windows.Forms;
namespace WindowsApplication23
{
    public class ComboBoxTreeView : ComboBox
    {
        private const int WM_LBUTTONDOWN = 0x201, WM_LBUTTONDBLCLK = 0x203;
        ToolStripControlHost treeViewHost;
        ToolStripDropDown dropDown;
        public ComboBoxTreeView()
        {
            TreeView treeView = new TreeView();
            treeView.AfterSelect += new TreeViewEventHandler(treeView_AfterSelect);
            treeView.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(treeView_NodeMouseDoubleClick);
            treeView.BorderStyle = BorderStyle.None;

            treeViewHost = new ToolStripControlHost(treeView);
            dropDown = new ToolStripDropDown();
            dropDown.Width = this.Width;
            dropDown.Items.Add(treeViewHost);
        }

        public void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (TreeView.SelectedNode != null)
            {
                this.Text = TreeView.SelectedNode.Text;
            }
            dropDown.Close();
            this.Focus();
        }
        public void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //if (TreeView.SelectedNode.Tag != null)
            //{
            //    this.Tag = TreeView.SelectedNode.Tag;
            //    this.Text = this.Tag.ToString()+"-"+TreeView.SelectedNode.Text;
            //}
            //else
            //{
            //    this.Text = TreeView.SelectedNode.Text;
            //}

            //if (TreeView.SelectedNode.Name != null)
            //{
            //    this.Tag = TreeView.SelectedNode.Name;
            //    this.Text = this.Tag + "-" + TreeView.SelectedNode.Text;
            //}
            //else
            //{
            //    this.Text = TreeView.SelectedNode.Text;
            //}
            this.Text = TreeView.SelectedNode.Text;
            dropDown.Close();
            this.Focus();
        }
        public TreeView TreeView
        {
            get { return treeViewHost.Control as TreeView; }
        }
        private void ShowDropDown()
        {
            if (dropDown != null)
            {
                treeViewHost.Size = new Size(DropDownWidth - 2, DropDownHeight);
                dropDown.Show(this, 0, this.Height);
            }
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDBLCLK || m.Msg == WM_LBUTTONDOWN)
            {
                ShowDropDown();
                return;
            }
            base.WndProc(ref m);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (dropDown != null)
                {
                    dropDown.Dispose();
                    dropDown = null;
                }
            }
            base.Dispose(disposing);
        }
    }

}