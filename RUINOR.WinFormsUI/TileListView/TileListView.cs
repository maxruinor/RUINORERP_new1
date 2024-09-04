using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Data;

namespace RUINOR.WinFormsUI.TileListView
{
    public class TileListView : Panel
    {
        public void Clear()
        {
            groups.Clear();
            this.Controls.Clear();
        }

        public FlowLayoutPanel ListViewFlowLayoutPanel { get; set; }
        public TileListView()
        {
            this.Dock = DockStyle.Fill;
            
        }

        private List<TileGroup> groups = new List<TileGroup>();

       

        public void AddGroup(string title)
        {
            var group = new TileGroup(title);
            groups.Add(group);
            // 刷新 UI 界面
            this.Invalidate(); // 请求重绘控件
            this.PerformLayout(); // 强制执行布局逻辑
        }

        public void AddItemToGroup(string groupTitle, string text, bool isChecked)
        {
            var group = groups.Find(g => g.GroupTitle == groupTitle);
            if (group != null)
            {
                // var item = new TileItem(text, isChecked);
                var item = new CheckBox
                {
                    Height = 20,
                    Text = text,
                    Checked = isChecked
                };
                group.Items.Add(item);

            

                // 刷新 UI 界面
                this.Invalidate(); // 请求重绘控件
                this.PerformLayout(); // 强制执行布局逻辑
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
            //if (ListViewFlowLayoutPanel == null)
            //{
            //    this.ListViewFlowLayoutPanel = new FlowLayoutPanel
            //    {
            //        FlowDirection = FlowDirection.LeftToRight,
            //        AutoSize = true,
            //        AutoSizeMode = AutoSizeMode.GrowAndShrink,
            //        WrapContents = true,
            //        Dock = DockStyle.Fill,
            //        Anchor = AnchorStyles.Top | AnchorStyles.Left
            //    };
            //    this.Controls.Add(ListViewFlowLayoutPanel);
            //}
            

            foreach (var group in groups)
            {
                if (group.GroupBox == null)
                {
                    group.GroupBox = new GroupBox
                    {
                        Text = group.GroupTitle,
                       // AutoSize = true,
                        // Dock = DockStyle.Fill, // 或者根据需要设置为 Fill
                       AutoSizeMode = AutoSizeMode.GrowOnly,
                        Width = this.Width-30,
                        Height =  200, // Adjust height based on item count
                        Top = y,
                       Padding = new Padding(20),
                        Left = 10,
                        
                    };

                    // this.Controls.Add(group.GroupBox);

                    
                    this.Controls.Add(group.GroupBox);
                    group.GroupFlowLayoutPanel = new FlowLayoutPanel
                    {
                        FlowDirection = FlowDirection.LeftToRight,
                        AutoSize = true,
                        AutoSizeMode = AutoSizeMode.GrowAndShrink,
                        WrapContents = true,
                        Dock = DockStyle.Fill,
                       Anchor = AnchorStyles.Top | AnchorStyles.Left 
                    };
                    group.GroupBox.Controls.Add(group.GroupFlowLayoutPanel);
                }
                foreach (var item in group.Items)
                {
                    group.GroupFlowLayoutPanel.Controls.Add(item);
                }
                group.GroupBox.BringToFront();
                y += group.GroupBox.Height;
            }
        }
    }


    [Serializable]

public class TileItem : UserControl
    {
        private Label label;
        private CheckBox checkBox;

        public TileItem(string text, bool isChecked)
        {
            this.Height = 20;

            checkBox = new CheckBox();
            checkBox.Text = text;
            checkBox.Checked = isChecked;
            checkBox.Dock = DockStyle.Top;
            checkBox.TextAlign = ContentAlignment.MiddleLeft;

            //label = new Label();
            //label.Text = text;
            //label.Dock = DockStyle.Fill;
            //label.TextAlign = ContentAlignment.MiddleCenter;

            this.Controls.Add(checkBox);
            //this.Controls.Add(label);
            this.BackColor = Color.MistyRose;
            // 调整大小以适应文本
            ResizeToFitText();
        }

        private void ResizeToFitText()
        {
            // 测量文本
            // Size textSize = TextRenderer.MeasureText(label.Text, label.Font);
            Size textSize = TextRenderer.MeasureText(checkBox.Text, checkBox.Font);
            // 增加一些边距
            textSize.Width += 10;
            textSize.Height += 10;

            // 设置最小尺寸
            MinimumSize = new Size(textSize.Width, Math.Max(this.MinimumSize.Height, textSize.Height));

            // 调整控件大小
            this.Size = new Size(textSize.Width, Math.Max(this.Size.Height, textSize.Height));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // 可以在这里添加自定义绘制逻辑
        }
    }

    [Serializable]
    public class TileGroup
    {
        public string GroupTitle { get; set; }
        public List<CheckBox> Items { get; } = new List<CheckBox>();
        public GroupBox GroupBox { get; set; }
        public FlowLayoutPanel GroupFlowLayoutPanel { get; set; }

        public TileGroup(string title)
        {
            GroupTitle = title;
        }
    }


}