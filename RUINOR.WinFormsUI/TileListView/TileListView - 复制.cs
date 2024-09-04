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
        public TileListView()
        {
            //this.Dock = DockStyle.Fill;
        }

        private List<TileGroup> groups = new List<TileGroup>();

        /// <summary>
        /// 获取或设置 TileGroup 列表
        /// </summary>
       // [Browsable(false)]
        //public TileGroup[] Groups { get => groups; set => groups = value; }

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
                var item = new TileItem(text, isChecked);
                group.Items.Add(item);
                /*
                // Ensure the item is added to the group's flow layout panel
                if (group.FlowLayoutPanel == null)
                {
                    group.FlowLayoutPanel = new FlowLayoutPanel
                    {
                        FlowDirection = FlowDirection.LeftToRight,
                        AutoSize = true,
                        AutoSizeMode = AutoSizeMode.GrowAndShrink,
                        Dock = DockStyle.Fill
                    };

                  
                    #region add group to control
                    if (group.GroupBox == null)
                    {
                        group.GroupBox = new GroupBox
                        {
                            Text = group.GroupTitle,
                            Width = this.Width,
                            Height = group.Items.Count * 10, // Adjust height based on item count
                            Left = 0,
                            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                        };
                        this.Controls.Add(group.GroupBox);

                    }
                    group.GroupBox.BringToFront();

                    #endregion
                   

                    group.GroupBox.Controls.Add(group.FlowLayoutPanel);
                }
                group.FlowLayoutPanel.Controls.Add(item);*/
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

            foreach (var group in groups)
            {
                if (group.GroupBox == null)
                {
                    group.GroupBox = new GroupBox
                    {
                        Text = group.GroupTitle,
                        AutoSize = true,
                        // Dock = DockStyle.Fill, // 或者根据需要设置为 Fill
                       // AutoSizeMode = AutoSizeMode.GrowAndShrink,
                        //Width = this.Width-10,
                        //Height = Groups.Count * 200, // Adjust height based on item count
                        Top = y,
                       // Padding = new Padding(3),
                        Left = 10,
                        Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                    };
                    this.Controls.Add(group.GroupBox);

                    group.FlowLayoutPanel = new FlowLayoutPanel
                    {
                        FlowDirection = FlowDirection.LeftToRight,
                        AutoSize = true,
                        AutoSizeMode = AutoSizeMode.GrowAndShrink,
                        WrapContents = true,
                        Dock = DockStyle.Fill,
                        AutoScroll= true
                    };
                    group.GroupBox.Controls.Add(group.FlowLayoutPanel);
                }
                foreach (var item in group.Items)
                {
                    group.FlowLayoutPanel.Controls.Add(item);
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
        public List<TileItem> Items { get; } = new List<TileItem>();
        public GroupBox GroupBox { get; set; }
        public FlowLayoutPanel FlowLayoutPanel { get; set; }

        public TileGroup(string title)
        {
            GroupTitle = title;
        }
    }


}