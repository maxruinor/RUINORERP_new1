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
            groups.ForEach(g => g.Items.Clear());
            groups.Clear();
            this.Controls.Clear();
        }


        public void SetItemChecked(string _GroupID, string ItemText, bool isChecked)
        {
            //加载属性值时，勾选对应的属性值
            var group = Groups.FirstOrDefault(c => c.GroupID == _GroupID);
            if (group != null)
            {
                group.Items.FirstOrDefault(c => c.Text == ItemText).Checked = true;
            }
        }


        public TileListView()
        {
            this.Dock = DockStyle.Fill;

        }



        private List<TileGroup> groups = new List<TileGroup>();



        public TileGroup[] Groups
        {
            get { return groups.ToArray(); }
        }
        public TileGroup AddGroup(string _GroupID, string title)
        {
            var group = new TileGroup(_GroupID, title);
            groups.Add(group);
            return group;
        }


        public CheckBox AddItemToGroup(string _GroupID, string ItemText, bool isChecked, string chkName = "chk", object tag = null)
        {
            CheckBox checkBox = null;
            var group = groups.Find(g => g.GroupID == _GroupID);
            if (group != null)
            {
                // var item = new TileItem(text, isChecked);
                var item = new CheckBox
                {
                    Name = chkName,
                    Height = 18,
                    Text = ItemText,
                    Checked = isChecked,
                    Tag = tag,
                    //AutoSize = true,
                    //Anchor = AnchorStyles.Left | AnchorStyles.Top,
                    Margin = new Padding(0, 0, 0, 0),
                    //TextAlign = ContentAlignment.MiddleLeft,
                    //FlatStyle = FlatStyle.Flat,
                    //BackColor = Color.AliceBlue
                };
                checkBox = item;
                group.Items.Add(item);
            }
            return checkBox;
        }
        public void UpdateUI()
        {
            // 刷新 UI 界面
            this.Invalidate(); // 请求重绘控件
            this.PerformLayout(); // 强制执行布局逻辑
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
                        AutoSizeMode = AutoSizeMode.GrowAndShrink,
                        Width = this.Width - 30, // 固定宽度，减去一些边距
                                                 // Height =  200, // Adjust height based on item count
                        Top = y,
                        //BackColor = Color.LightSkyBlue,
                        Left = 10,
                        Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
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
                        Padding = new Padding(5, 0, 5, 15),
                        // BackColor = Color.LightBlue,
                        Anchor = AnchorStyles.Top | AnchorStyles.Left
                    };
                    group.GroupBox.Controls.Add(group.GroupFlowLayoutPanel);
                }
                //if (group.GroupFlowLayoutPanel != null)
                //{
                //    group.GroupFlowLayoutPanel.Controls.Add(item);
                //}
                foreach (var item in group.Items)
                {
                    group.GroupFlowLayoutPanel.Controls.Add(item);
                }
                group.GroupBox.Height = group.GroupFlowLayoutPanel.GetPreferredSize(new Size(group.GroupBox.Width, 0)).Height + group.GroupBox.Padding.Vertical;

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
            //this.BackColor = Color.MistyRose;
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
        /// <summary>
        /// 名称唯一标识
        /// </summary>
        public string GroupID { get; set; }
        public string GroupTitle { get; set; }
        // 属性用于保存业务数据，类型为 object 以提供灵活性
        public object BusinessData { get; set; }
        // 示例方法，用于设置业务数据
        public void SetBusinessData(object data)
        {
            BusinessData = data;
        }

        // 示例方法，用于获取业务数据
        public object GetBusinessData()
        {
            return BusinessData;
        }

        public List<CheckBox> Items { get; } = new List<CheckBox>();
        public GroupBox GroupBox { get; set; }
        public FlowLayoutPanel GroupFlowLayoutPanel { get; set; }


        public TileGroup(string _GroupID, string title)
        {
            GroupID = _GroupID;
            GroupTitle = title;
        }

        public TileGroup(string title)
        {
            GroupTitle = title;
        }
    }


}