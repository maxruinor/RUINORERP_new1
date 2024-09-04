using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINOR.WinFormsUI.TileListView
{
    public class TileListView : Panel
    {

        public void Clear()
        {
            groups.Clear();
            this.Controls.Clear();
        }
        private List<TileGroup> groups = new List<TileGroup>();

        public void AddGroup(string title)
        {
            var group = new TileGroup(title);
            groups.Add(group);
        }

        public void AddItemToGroup(string groupTitle, string text, bool isChecked)
        {
            var group = groups.Find(g => g.GroupTitle == groupTitle);
            if (group != null)
            {
                var item = new TileItem(text, isChecked);
                group.Items.Add(item);
                this.Controls.Add(item);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            PerformLayout();
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            int y = 0;

            foreach (var group in groups)
            {
                // Create and position the GroupBox
                GroupBox groupBox = new GroupBox();
                groupBox.Text = group.GroupTitle;
                groupBox.Width = this.Width - 10; // Reduced width to account for padding
                groupBox.Height = group.Items.Count * 35;
                groupBox.Top = y;
                groupBox.Left = 5;
                groupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                this.Controls.Add(groupBox);

                int x = 0;
                foreach (var item in group.Items)
                {
                    item.Left = x;
                    item.Top = 0;
                    item.Width = groupBox.Width - 10;
                    item.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                    groupBox.Controls.Add(item);
                    x += item.Width;
                }

                y += groupBox.Height + 5;
            }
        }
    }

    public class TileItem : UserControl
    {
        private Label label;
        private CheckBox checkBox;

        public TileItem(string text, bool isChecked)
        {
            this.Height = 30;

            checkBox = new CheckBox();
            checkBox.Text = text;
            checkBox.Checked = isChecked;
            checkBox.Dock = DockStyle.Left;

            label = new Label();
            label.Text = text;
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleCenter;

            this.Controls.Add(checkBox);
            this.Controls.Add(label);
        }
    }

    public class TileGroup
    {
        public string GroupTitle { get; set; }
        public List<TileItem> Items { get; } = new List<TileItem>();

        public TileGroup(string title)
        {
            GroupTitle = title;
        }
    }
}