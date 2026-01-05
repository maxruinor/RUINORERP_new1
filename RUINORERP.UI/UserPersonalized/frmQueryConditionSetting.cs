using Krypton.Toolkit;
using RUINORERP.Business.Processor;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Models;
using RUINORERP.UI.Common;
using RUINORERP.UI.UControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.UserPersonalized
{
    /// <summary>
    /// QueryConditionSettings  查询条件设置，可以设置显示行数。条件排序，条件的默认值。条件显示情况
    /// </summary>
    public partial class frmQueryConditionSetting : KryptonForm
    {
        public frmQueryConditionSetting()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        public List<tb_UIQueryCondition> Conditions { get; set; } = new List<tb_UIQueryCondition>();

        public tb_UIQueryCondition[] oldConditions;

        public tb_UIMenuPersonalization Personalization { get; set; }

        ContextMenuStrip contentMenu1;

        private void btnOk_Click(object sender, EventArgs e)
        {
            string shou = string.Empty;

            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Checked)
                {
                    shou += item.Name + ",";
                }
            }
            shou = shou.TrimEnd(',');
            if (shou == "")
            {
                MessageBox.Show("不能隐藏所有查询条件项！", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int sortindex = 0;
            //上面临时保存了一个之前的序列数组
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Tag is tb_UIQueryCondition Condition)
                {
                    if (Condition != null)
                    {
                        tb_UIQueryCondition cdc = Conditions.Where(c => c.FieldName == Condition.FieldName).FirstOrDefault();
                        cdc.IsVisble = item.Checked;
                        cdc.Sort = sortindex;
                    }
                }
                if (string.IsNullOrEmpty(item.Text))
                {
                    sortindex++;
                    continue;
                }

                sortindex++;
            }
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public string MenuPathKey { get; set; }


        /// <summary>
        /// 设置默认值时需要的对象
        /// </summary>
        public BaseEntity QueryDto { get; set; }


        public tb_UIMenuPersonalization uIMenuPersonalization { get; set; }
        /// <summary>
        /// 查询条件 根据这个来生成绑定。选择默认值
        /// </summary>
        public List<QueryField> QueryFields { get; set; }

        private void frmMenuPersonalization_Load(object sender, EventArgs e)
        {
            DataBindingHelper.BindData4CheckBox<tb_UIMenuPersonalization>(Personalization, k => k.EnableQuerySettings, chkEnableQuerySettings, false);
            listView1.AllowDrop = true;
            listView1.ItemDrag += ListView1_ItemDrag;
            listView1.DragEnter += listView1_DragEnter;
            listView1.DragOver += listView1_DragOver;
            listView1.DragDrop += listView1_DragDrop;
            listView1.DragLeave += listView1_DragLeave;


            listView1.Columns.Add("查询条件");
            listView1.Columns[0].TextAlign = HorizontalAlignment.Center;
            listView1.Columns[0].Width = -2; //-1 -2 

            oldConditions = new tb_UIQueryCondition[Conditions.Count];
            Conditions.CopyTo(oldConditions);
            foreach (tb_UIQueryCondition keyValue in Conditions)
            {

                //listView1.Items.Insert(0, new ListViewItem(item.Key.ToString()));
                ListViewItem lvi = new ListViewItem();
                lvi.Checked = keyValue.IsVisble;
                lvi.Name = keyValue.FieldName;
                lvi.Tag = keyValue;
                lvi.Text = keyValue.Caption;
                //用这个来保存
                lvi.ImageKey = keyValue.Sort.ToString();
                listView1.Items.Add(lvi);


            }




            //添加悬浮提示  
            ToolTip tt = new ToolTip();
            tt.InitialDelay = 200;
            tt.AutomaticDelay = 200;
            tt.ReshowDelay = 200;
            tt.ShowAlways = true;

            //tt.SetToolTip(pictureBox_down, "下移选中字段（已选择字段列表）");  
            //tt.SetToolTip(pictureBox_downdown, "置底选中字段（已选择字段列表）");  
            //tt.SetToolTip(pictureBox_left, "左移选中字段");  
            //tt.SetToolTip(pictureBox_right, "右移选中字段");  
            //tt.SetToolTip(pictureBox_up, "上移选中字段（已选择字段列表）");  
            //tt.SetToolTip(pictureBox_upup, "置顶选中字段（已选择字段列表）");  
            //tt.SetToolTip(listView1, "双击从未选择字段列表中移除");  
            //tt.SetToolTip(listView2, "双击从已选择字段列表中移除");  

            contentMenu1 = new ContextMenuStrip();
            contentMenu1.Items.Add("全选");
            contentMenu1.Items.Add("全不选");
            contentMenu1.Items.Add("反选");
            contentMenu1.Items[0].Click += new EventHandler(contentMenu1_CheckAll);
            contentMenu1.Items[1].Click += new EventHandler(contentMenu1_CheckNo);
            contentMenu1.Items[2].Click += new EventHandler(contentMenu1_Inverse);

            listView1.ContextMenuStrip = contentMenu1;
        }

        //启动拖拽，设置拖拽的数据和效果。
        private void ListView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            listView1.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        //拖拽进入ListView，判断拖拽的数据格式，并设置拖拽的效果。
        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            //e.Effect = e.AllowedEffect;
            e.Effect = DragDropEffects.Move;
        }

        //拖动经过ListView时，设置拖动的效果，显示拖放位置线
        private void listView1_DragOver(object sender, DragEventArgs e)
        {
            System.Drawing.Point ptScreen = new System.Drawing.Point(e.X, e.Y);
            System.Drawing.Point pt = listView1.PointToClient(ptScreen);
            ListViewItem item = listView1.GetItemAt(pt.X, pt.Y);
            if (item != null)
                item.Selected = true;


        }

        //拖拽释放，移动行
        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            ListViewItem draggedItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
            System.Drawing.Point ptScreen = new System.Drawing.Point(e.X, e.Y);
            System.Drawing.Point pt = listView1.PointToClient(ptScreen);
            ListViewItem TargetItem = listView1.GetItemAt(pt.X, pt.Y); // 拖动的项将放置于该项之前    
            if (TargetItem == null)
            {
                return;
            }
            listView1.Items.Insert(TargetItem.Index, (ListViewItem)draggedItem.Clone());
            listView1.Items.Remove(draggedItem);

        }


        private void listView1_DragLeave(object sender, EventArgs e)
        {
            listView1.InsertionMark.Index = -1;
        }
        private class ListViewIndexComparer : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                return ((ListViewItem)x).Index - ((ListViewItem)y).Index;
            }
        }


        private void contentMenu1_CheckAll(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
                item.Checked = true;
        }
        private void contentMenu1_CheckNo(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = false;
            }
        }
        private void contentMenu1_Inverse(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Checked == true)
                    item.Checked = false;
                else
                    item.Checked = true;
            }

        }


        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            //获取listview1当前焦点  
            ListViewItem item = listView1.FocusedItem;
            listView1.Items.Remove(item);
            listView1.SelectedItems.Clear();
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
                item.Checked = true;
        }

        private void chkReverseSelection_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = !item.Checked;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
            {
                var entity = listView1.SelectedItems[0].Tag as tb_UIQueryCondition;

                if (panelConditionEdit.Controls.ContainsKey(entity.FieldName.ToString()))
                {
                    var uCQuery = panelConditionEdit.Controls.CastToList<Control>().FirstOrDefault(c => c.Name == entity.FieldName.ToString());
                    if (uCQuery != null)
                    {
                        uCQuery.Visible = true;
                    }
                    //其它隐藏
                    panelConditionEdit.Controls.CastToList<Control>().Where(c => c.Name != entity.FieldName.ToString()).ToList().ForEach(c => c.Visible = false);
                }
                else
                {
                    //有些闪屏。后面优化是不是加载时就全部加进去 
                    UCQueryCondition uCQuery = new UCQueryCondition();
                    uCQuery.Name = entity.FieldName.ToString();
                    uCQuery.OnSynchronizeUI += UCQuery_OnSynchronizeUI;
                    uCQuery.QueryFields = QueryFields;
                    uCQuery.QueryDto = QueryDto;
                    uCQuery.BindData(entity);

                    uCQuery.Visible = true;
                    uCQuery.TopLevel = false;
                    uCQuery.Dock = DockStyle.Fill;
                    panelConditionEdit.Controls.Add(uCQuery as Control);
                    //其它隐藏
                    panelConditionEdit.Controls.CastToList<Control>().Where(c => c.Name != entity.FieldName.ToString()).ToList().ForEach(c => c.Visible = false);
                }




            }

        }

        private void UCQuery_OnSynchronizeUI(object sender, object e)
        {
            //其它就不能有焦点。只能设置一个。
            //QueryFields.Where(c => !c.FieldName.Equals(EditEntity.FieldName)).ForEach(c => c.Focused = false);
            if (e.ToString() == "Focused")
            {
                if (sender.GetPropertyValue(e.ToString()).ToBool())
                {
                    listView1.Items.CastToList<ListViewItem>().Where(c => c.Tag != sender).ToList().ForEach
                        ((x) =>
                        {
                            if (x.Tag is tb_UIQueryCondition Condition)
                            {
                                Condition.Focused = false;
                            }
                        });
                }
            }
            //上面临时保存了一个之前的序列数组
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Tag is tb_UIQueryCondition Condition)
                {
                    if (Condition != null)
                    {
                        item.Checked = Condition.IsVisble;
                        item.Text = Condition.Caption;

                    }
                }
            }
        }

        /// <summary>
        /// 初始化查询条件按钮点击事件
        /// </summary>
        /// <param name="sender">触发事件的控件</param>
        /// <param name="e">事件参数</param>
        private void btnInitialize_Click(object sender, EventArgs e)
        {
            // 添加确认提示，防止误操作
            if (MessageBox.Show("确定要初始化所有查询条件吗？此操作将重置所有查询条件为默认状态。", "确认初始化", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                // 清空当前条件
                Conditions.Clear();
                listView1.Items.Clear();

                // 重新生成默认查询条件
                List<tb_UIQueryCondition> defaultConditions = new List<tb_UIQueryCondition>();
                if (QueryFields != null)
                {
                    int sortIndex = 0;
                    foreach (var item in QueryFields)
                    {
                        tb_UIQueryCondition condition = new tb_UIQueryCondition();
                        condition.FieldName = item.FieldName;
                        condition.Sort = item.DisplayIndex;
                        // 时间区间排最后
                        if (item.AdvQueryFieldType == AdvQueryProcessType.datetimeRange && condition.Sort == 0)
                        {
                            condition.Sort = 100;
                        }
                        condition.IsVisble = true; // 默认显示所有条件
                        condition.Caption = item.Caption;
                        if (item.ColDataType != null)
                        {
                            condition.ValueType = item.ColDataType.Name;
                        }
                        condition.UIMenuPID = Personalization.UIMenuPID;
                        // 重置其他属性为默认值
                        condition.Default1 = string.Empty;
                        condition.Default2 = string.Empty;
                        condition.EnableDefault1 = false;
                        condition.EnableDefault2 = false;
                        condition.Focused = false;
                        condition.UseLike = true;
                        condition.MultiChoice = false;
                        condition.ControlWidth = 120; // 默认控件宽度
                        condition.DiffDays1 = null;
                        condition.DiffDays2 = null;

                        defaultConditions.Add(condition);
                        sortIndex++;
                    }
                }

                // 对默认条件进行排序
                var sortedDefaultConditions = defaultConditions.OrderBy(condition => condition.Sort).ToList();

                // 更新条件集合
                Conditions.AddRange(sortedDefaultConditions);

                // 保存初始条件到oldConditions
                oldConditions = new tb_UIQueryCondition[Conditions.Count];
                Conditions.CopyTo(oldConditions);

                // 更新ListView显示
                foreach (tb_UIQueryCondition keyValue in Conditions)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Checked = keyValue.IsVisble;
                    lvi.Name = keyValue.FieldName;
                    lvi.Tag = keyValue;
                    lvi.Text = keyValue.Caption;
                    lvi.ImageKey = keyValue.Sort.ToString();
                    listView1.Items.Add(lvi);
                }

                // 清空编辑面板
                panelConditionEdit.Controls.Clear();

                MessageBox.Show("查询条件初始化完成。", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化查询条件失败：{ex.Message}", "操作失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
