﻿using Netron.GraphLib;
using RUINORERP.Common.Extensions;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.UI.ToolForm;
using RUINORERP.UI.UControls;
using SourceGrid;
using SourceGrid.Cells.Editors;
using SourceGrid.Cells.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winista.Text.HtmlParser.Data;
using static RUINORERP.UI.Log.UClog;
using Color = System.Drawing.Color;
using RUINOR.WinFormsUI.CustomPictureBox;
using static FastReport.Design.ToolWindows.DictionaryWindow;
using SqlSugar;

namespace RUINORERP.UI.UCSourceGrid
{

    /// <summary>
    /// 带自定义列的右键菜单。后面也可能合并都有配置性
    /// </summary>
    public class PopupMenuWithCustomColumns : SourceGrid.Cells.Controllers.ControllerBase
    {

        public delegate void ColumnsVisibleDelegate(KeyValuePair<string, SGDefineColumnItem> kv);
        //string bool  cost true-->表示 成本 显示
        //public delegate void ColumnsVisibleDelegate(int colIndex, string colName, bool visible);
        /// <summary>
        /// 验证行数据
        /// </summary>
        public event ColumnsVisibleDelegate OnColumnsVisible;

        /// <summary>
        /// 控件列配置的右键菜单，用于拖拽排序列的顺序和勾选是否显示列。
        /// </summary>
        ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();
        private ToolStripMenuItem draggedItem;
        private Point dragStartPoint;
        private int InitItemsCount = 0;
        private ToolStripMenuItem highlightedItem; // 跟踪当前高亮项
        SourceGridDefine sgdefine;
        public tb_MenuInfo CurMenuInfo { get; set; }
        public PopupMenuWithCustomColumns(string xmlfileName, SourceGridDefine _sgdefine)
        {
            sgdefine = _sgdefine;
            contextMenuStrip1.AllowDrop = true;
            contextMenuStrip1.Opening += ContextMenuStrip1_Opening;
            contextMenuStrip1.MouseDown += ContextMenuStrip_MouseDown;
            contextMenuStrip1.MouseMove += ContextMenuStrip_MouseMove;
            contextMenuStrip1.DragOver += ContextMenuStrip_DragOver;
            contextMenuStrip1.DragDrop += ContextMenuStrip_DragDrop;
            contextMenuStrip1.DragLeave += ContextMenuStrip_DragLeave;
            contextMenuStrip1.Closing += ContextMenuStrip1_Closing;
            contextMenuStrip1.ShowCheckMargin = true;
            _xmlfileName = xmlfileName;
            InitializeContextMenu();
            //ConfigColItems = Common.UIHelper.LoadColumnsList(xmlfileName);
        }


        private void InitializeContextMenu()
        {
            // 初始化菜单项，例如添加分隔线、自定义项等
            ToolStripSeparator ss = new ToolStripSeparator();
            contextMenuStrip1.Items.Add(ss);
            //ToolStripMenuItem siCustom = new ToolStripMenuItem("自定义");
            //siCustom.Click += SiCustom_Click;
            //contextMenuStrip1.Items.Add(siCustom);
            //contextMenuStrip1.Renderer = new HighlightMenuRenderer();
            ToolStripMenuItem LoadDefaultColumns = new ToolStripMenuItem("加载默认配置");
            LoadDefaultColumns.Click += LoadDefaultColumns_Click;
            contextMenuStrip1.Items.Add(LoadDefaultColumns);
            InitItemsCount = contextMenuStrip1.Items.Count;

        }

        // 自定义渲染器
        private class HighlightMenuRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                var menuItem = e.Item as ToolStripMenuItem;
                if (menuItem != null && menuItem.Selected)
                {
                    using (var brush = new SolidBrush(Color.FromArgb(50, 150, 250)))
                    {
                        e.Graphics.FillRectangle(brush, e.Item.ContentRectangle);
                    }
                }
                else
                {
                    base.OnRenderMenuItemBackground(e);
                }
            }
        }


        private void ContextMenuStrip1_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            ClearHighlight();
        }
        private void LoadDefaultColumns_Click(object sender, EventArgs e)
        {
            ColumnDisplays = UIBizSrvice.LoadInitSourceGridSetting(sgdefine, CurMenuInfo);
            //先清掉之前的
            //for (int i = contextMenuStrip1.Items.Count - 1; i >= InitItemsCount; i--)
            //{
            //    if (contextMenuStrip1.Items[0].AllowDrop == true)
            //    {
            //        contextMenuStrip1.Items.RemoveAt(i);
            //    }
            //}
            contextMenuStrip1.Items.Clear();
            InitializeContextMenu();
            foreach (var item in ColumnDisplays)
            {
                AddNewItems(item);
            }
            //显示 宽度 排序 生效
        }

        #region 拖放处理逻辑
        private void ContextMenuStrip_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragStartPoint = e.Location;
                draggedItem = contextMenuStrip1.GetItemAt(e.Location) as ToolStripMenuItem;
            }
        }

        private void ContextMenuStrip_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggedItem == null || e.Button != MouseButtons.Left) return;

            var dragSize = SystemInformation.DragSize;
            var dragRect = new Rectangle(
                dragStartPoint.X - dragSize.Width / 2,
                dragStartPoint.Y - dragSize.Height / 2,
                dragSize.Width,
                dragSize.Height);

            if (!dragRect.Contains(e.Location))
            {
                contextMenuStrip1.DoDragDrop(draggedItem, DragDropEffects.Move);
                draggedItem = null;
            }
        }

        private void ContextMenuStrip_DragOver(object sender, DragEventArgs e)
        {
            var targetItem = contextMenuStrip1.GetItemAt(contextMenuStrip1.PointToClient(new Point(e.X, e.Y)));
            if (targetItem != null)
            {
                var effect = GetDragDropEffect(targetItem);
                e.Effect = effect;

                // 显示插入位置指示
                HighlightInsertPosition(targetItem, effect);
            }
        }

        private void ContextMenuStrip_DragDrop(object sender, DragEventArgs e)
        {
            var sourceItem = e.Data.GetData(typeof(ToolStripMenuItem)) as ToolStripMenuItem;
            var dropPos = contextMenuStrip1.PointToClient(new Point(e.X, e.Y));
            var targetItem = contextMenuStrip1.GetItemAt(dropPos);

            //如果目标不是可拖动的。则返回不处理
            if (!targetItem.AllowDrop)
            {
                return;
            }

            if (sourceItem != null && targetItem != null && sourceItem != targetItem)
            {
                MoveMenuItem(sourceItem, targetItem);
            }
            ClearInsertIndicator();
        }

        private DragDropEffects GetDragDropEffect(ToolStripItem targetItem)
        {
            if (targetItem is ToolStripSeparator)
                return DragDropEffects.None;

            return DragDropEffects.Move;
        }

        private void MoveMenuItem(ToolStripMenuItem source, ToolStripItem target)
        {
            int oldIndex = contextMenuStrip1.Items.IndexOf(source);
            int newIndex = contextMenuStrip1.Items.IndexOf(target);

            contextMenuStrip1.Items.Remove(source);
            contextMenuStrip1.Items.Insert(newIndex, source);

            // 同步数据源
            //var itemData = items[oldIndex];
            //items.RemoveAt(oldIndex);
            //items.Insert(newIndex, itemData);
        }

        /// <summary>
        /// 移动菜单项，但不考虑插入位置可视化。只在拖拽结束时调用此方法。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        private void MoveMenuItem(ToolStripMenuItem source, ToolStripMenuItem target)
        {
            int sourceIndex = contextMenuStrip1.Items.IndexOf(source);
            int targetIndex = contextMenuStrip1.Items.IndexOf(target);
            SGColDisplayHandler sourceCol = source.Tag as SGColDisplayHandler;
            SGColDisplayHandler targetCol = target.Tag as SGColDisplayHandler;

            contextMenuStrip1.Items.Remove(source);
            contextMenuStrip1.Items.Insert(targetIndex, source);
            SynchronizeDefineColumns(source, target);
        }


        #endregion

        #region 插入位置可视化
        private void HighlightInsertPosition(ToolStripItem targetItem, DragDropEffects effect)
        {
            ClearInsertIndicator();
            if (effect != DragDropEffects.Move) return;

            if (targetItem is ToolStripSeparator)
            {
                return;
            }
            //这时应该是拖到了非表格列的位置 
            if (!targetItem.AllowDrop)
            {
                return;
            }

            var index = contextMenuStrip1.Items.IndexOf(targetItem);
            var indicator = new ToolStripSeparator();
            contextMenuStrip1.Items.Insert(index, indicator);
        }

        private void ClearHighlight()
        {
            highlightedItem = null;
            contextMenuStrip1.Invalidate();
        }
        private void ClearInsertIndicator()
        {
            foreach (var item in contextMenuStrip1.Items.OfType<ToolStripSeparator>().Where(s => s.Tag?.ToString() == "indicator"))
            {
                contextMenuStrip1.Items.Remove(item);
            }
        }
        //private void ClearInsertIndicator()
        //{
        //    if (insertIndicator != null && contextMenuStrip1.Items.Contains(insertIndicator))
        //    {
        //        contextMenuStrip1.Items.Remove(insertIndicator);
        //        insertIndicator = null;
        //    }
        //}

        private void ContextMenuStrip_DragLeave(object sender, EventArgs e)
        {
            ClearInsertIndicator();
        }
        #endregion


        private void SynchronizeDefineColumns(ToolStripMenuItem sourceItem, ToolStripMenuItem targetItem)
        {
            // 获取源项和目标项的 DisplayController
            SGColDisplayHandler sourceDisplay = sourceItem.Tag as SGColDisplayHandler;
            SGColDisplayHandler targetDisplay = targetItem.Tag as SGColDisplayHandler;

            if (sourceDisplay == null || targetDisplay == null)
            {
                return; // 如果没有找到 DisplayController，直接返回
            }

            // 在 DefineColumns 中找到对应的列
            SGDefineColumnItem sourceColumn = sgdefine.DefineColumns.FirstOrDefault(c => c.ColName == sourceDisplay.ColName);
            SGDefineColumnItem targetColumn = sgdefine.DefineColumns.FirstOrDefault(c => c.ColName == targetDisplay.ColName);

            if (sourceColumn == null || targetColumn == null)
            {
                return; // 如果没有找到对应的列，直接返回
            }

            // 交换 DefineColumns 中的顺序
            int sourceIndex = sgdefine.DefineColumns.IndexOf(sourceColumn);
            int targetIndex = sgdefine.DefineColumns.IndexOf(targetColumn);

            sgdefine.DefineColumns.RemoveAt(sourceIndex);
            sgdefine.DefineColumns.Insert(targetIndex, sourceColumn);

            List<ColumnInfo> columnInfos = new List<ColumnInfo>();
            for (int i = 0; i < sgdefine.grid.Columns.Count; i++)
            {
                columnInfos.Add(sgdefine.grid.Columns[i]);
            }

            List<ColumnInfo> OtherColumnInfos = new List<ColumnInfo>();

            //根据这些重新更新ColumnDisplays的顺序
            sgdefine.grid.Columns.Clear();

            //项总是第一列

            var ItemColInfo = columnInfos.FirstOrDefault(c => c.Tag as SGDefineColumnItem != null && c.Tag is SGDefineColumnItem columnItem && columnItem.ColName == "项");
            sgdefine.grid.Columns.Add(ItemColInfo);

            for (int i = 0; i < sgdefine.DefineColumns.Count; i++)
            {

                //bool isExist = columnInfos.Any(c =>
                // {
                //     if (c.Tag as SGDefineColumnItem != null && c.Tag is SGDefineColumnItem columnItem)
                //     {
                //         if (columnItem.ColName == sgdefine.DefineColumns[i].ColName)
                //         {
                //             return true;
                //         }
                //         else
                //         {
                //             return false;
                //         }
                //     }
                //     else
                //     {
                //         return false;
                //     }
                // }
                // );

                var colInfo = columnInfos.FirstOrDefault(c => c.Tag as SGDefineColumnItem != null && c.Tag is SGDefineColumnItem columnItem && columnItem.ColName == sgdefine.DefineColumns[i].ColName);
                //如果存在 就直接添加 不需要重新创建列对象
                if (colInfo != null)
                {
                    sgdefine.grid.Columns.Add(colInfo);
                }
                else
                {
                    OtherColumnInfos.Add(sgdefine.grid.Columns[i]);
                }
            }

            foreach (var item in OtherColumnInfos)
            {
                sgdefine.grid.Columns.Add(item);
            }

        }
        /// <summary>
        /// 保存了要控制的列  可以作废
        /// </summary>
        private List<KeyValuePair<string, SGDefineColumnItem>> items = new List<KeyValuePair<string, SGDefineColumnItem>>();
        //private SerializableDictionary<string, bool> items = new SerializableDictionary<string, bool>();

        /// <summary>
        /// 用来保存配置自定义列
        /// </summary>
        private string _xmlfileName;

        private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            // 初始化拖动状态
            draggedItem = null;
        }

        frmShowColumns frm = new frmShowColumns();

        /* 自定义
        private void SiCustom_Click(object sender, EventArgs e)
        {
            frm.XmlFileName = _xmlfileName;
            frm.Items = items;
            frm.ConfigItems = ConfigColItems;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //更新配置变化，体现到这里。思路是?
                foreach (var item in items)
                {
                    OnColumnsVisible(item);
                }
                Common.UIHelper.SaveColumnsList(ConfigColItems, _xmlfileName);
            }

        }
        */

        //private SerializableDictionary<string, bool> _ConfigColItems = new SerializableDictionary<string, bool>();

        ///// <summary>
        ///// 保存自定义列的集合
        ///// </summary>
        //public SerializableDictionary<string, bool> ConfigColItems { get => _ConfigColItems; set => _ConfigColItems = value; }

        /// <summary>
        /// 保存列控制信息的列表 ，这个值设计时不生成
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        public List<SGColDisplayHandler> ColumnDisplays { get; set; } = new List<SGColDisplayHandler>();


        /*
         public void AddItems(KeyValuePair<string, SGDefineColumnItem> item)
         {
             if (!item.Value.NeverVisible)
             {
                 //缓存控制，添加，如果存在则修改状态，如果没有则添加
                 if (ConfigColItems.ContainsKey(item.Key))
                 {
                     ConfigColItems[item.Key] = item.Value.Visible;
                 }
                 else
                 {
                     ConfigColItems.Add(item.Key, item.Value.Visible);
                 }

                 items.Add(item);
                 ToolStripMenuItem menuItem = new ToolStripMenuItem(item.Key)
                 {
                     Checked = item.Value.Visible,
                     CheckOnClick = true,
                     Tag = item // 存储原始数据
                 };
                 menuItem.Checked = item.Value.Visible;
                 menuItem.CheckOnClick = true;
                 menuItem.Click += Si_Click;
                 menuItem.AllowDrop = true;
                 //menuItem.DragEnter += Item_DragEnter;
                 //menuItem.DragOver += Item_DragOver;
                 //menuItem.DragDrop += Item_DragDrop;
                 menuItem.MouseDown += ContextMenuStrip_MouseDown;
                 menuItem.MouseMove += ContextMenuStrip_MouseMove;

                 // 为每个菜单项绑定独立事件
                 menuItem.MouseDown += MenuItem_MouseDown;
                 menuItem.MouseMove += MenuItem_MouseMove;
                 menuItem.DragOver += MenuItem_DragOver;
                 menuItem.DragDrop += MenuItem_DragDrop;
                 menuItem.DragLeave += MenuItem_DragLeave;
                 //menuItem.MouseDown += ContextMenuStrip_MouseDown;
                 //menuItem.MouseMove += ContextMenuStrip_MouseMove;

                 // MyMenu.Items.Add(si);
                 //初始时有几个，就减几
                 contextMenuStrip1.Items.Insert(contextMenuStrip1.Items.Count - InitItemsCount, menuItem);
             }
         }
        */
        /// <summary>
        /// 添加要控制的列,这个时候 就可以保存配置了
        /// </summary>
        /// <param name="item"></param>
        public void AddNewItems(SGColDisplayHandler DisplayController)
        {
            //如果不是禁用状态，则添加到菜单中
            if (!DisplayController.Disable)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem(DisplayController.ColCaption)
                {
                    Checked = DisplayController.Visible,
                    CheckOnClick = true,
                    Tag = DisplayController // 存储原始数据
                };

                menuItem.Checked = DisplayController.Visible;

                menuItem.CheckOnClick = true;
                menuItem.Click += menuItem_Click;
                menuItem.AllowDrop = true;
                //menuItem.DragEnter += Item_DragEnter;
                //menuItem.DragOver += Item_DragOver;
                //menuItem.DragDrop += Item_DragDrop;
                menuItem.MouseDown += ContextMenuStrip_MouseDown;
                menuItem.MouseMove += ContextMenuStrip_MouseMove;

                // 为每个菜单项绑定独立事件
                menuItem.MouseDown += MenuItem_MouseDown;
                menuItem.MouseMove += MenuItem_MouseMove;
                menuItem.DragOver += MenuItem_DragOver;
                menuItem.DragDrop += MenuItem_DragDrop;
                menuItem.DragLeave += MenuItem_DragLeave;
                //menuItem.MouseDown += ContextMenuStrip_MouseDown;
                //menuItem.MouseMove += ContextMenuStrip_MouseMove;

                // MyMenu.Items.Add(si);
                //初始时有几个，就减几
                contextMenuStrip1.Items.Insert(contextMenuStrip1.Items.Count - InitItemsCount, menuItem);
            }
        }

        private void menuItem_Click(object sender, EventArgs e)
        {
            //OnColumnsVisible?.Invoke(itemData);
            //OnColumnsVisible(item);
            //要更新到配置中
            var colDisplay = (sender as ToolStripMenuItem).Tag as SGColDisplayHandler;
            colDisplay.Visible = (sender as ToolStripMenuItem).Checked;
            sgdefine.grid.Columns.GetColumnInfo(colDisplay.UniqueId).Visible = colDisplay.Visible;
            ColumnDisplays.First(x => x.UniqueId == colDisplay.UniqueId)
               .Visible = colDisplay.Visible;
            //Common.UIHelper.SaveColumnsList(ConfigColItems, _xmlfileName);

        }

        #region 拖放事件处理
        private void MenuItem_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                draggedItem = sender as ToolStripMenuItem;
                dragStartPoint = e.Location;
            }
        }

        private void MenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggedItem == null || e.Button != MouseButtons.Left) return;

            // 检测是否达到拖动阈值
            if (IsDragStart(e.Location))
            {
                draggedItem.DoDragDrop(draggedItem, DragDropEffects.Move);
                draggedItem = null;
            }
        }

        private void MenuItem_DragOver(object sender, DragEventArgs e)
        {
            var targetItem = sender as ToolStripMenuItem;
            if (targetItem == null || e.Data.GetData(typeof(ToolStripMenuItem)) == null) return;

            // 如果目标项存在且与拖动项不同，则更新目标项的高亮状态
            //代码不起作用。暂时不处理了
            //if (targetItem != null && targetItem != draggedItem)
            //{
            //    // 更新目标项的高亮状态
            //    targetItem.BackColor = SystemColors.HotTrack;
            //    targetItem.ForeColor = Color.Red;
            //}
            //else
            //{
            //    // 恢复默认颜色
            //    targetItem?.ResetBackColor();
            //    targetItem?.ResetForeColor();
            //}

            // 更新高亮项并重绘
            if (highlightedItem != targetItem)
            {
                highlightedItem = targetItem;
                contextMenuStrip1.Invalidate();
            }
            e.Effect = DragDropEffects.Move;

            // 显示插入位置指示
            // ShowInsertIndicator(targetItem);
        }

        private void MenuItem_DragDrop(object sender, DragEventArgs e)
        {
            var sourceItem = e.Data.GetData(typeof(ToolStripMenuItem)) as ToolStripMenuItem;
            var targetItem = sender as ToolStripMenuItem;

            if (sourceItem == null || targetItem == null || sourceItem == targetItem) return;

            // 如果目标项存在且与拖动项不同，则更新目标项的高亮状态
            if (targetItem != null && targetItem != draggedItem)
            {
                // 更新目标项的高亮状态
                targetItem.BackColor = SystemColors.HotTrack;
                targetItem.ForeColor = SystemColors.Desktop;
            }
            else
            {
                // 恢复默认颜色
                targetItem?.ResetBackColor();
                targetItem?.ResetForeColor();
            }

            // 移动菜单项
            MoveMenuItem(sourceItem, targetItem);
            ClearInsertIndicator();
        }

        private void MenuItem_DragLeave(object sender, EventArgs e)
        {
            ClearInsertIndicator();
            ClearHighlight();
        }
        #endregion

        #region 拖放辅助方法
        private bool IsDragStart(Point currentPos)
        {
            return Math.Abs(currentPos.X - dragStartPoint.X) > SystemInformation.DragSize.Width ||
                   Math.Abs(currentPos.Y - dragStartPoint.Y) > SystemInformation.DragSize.Height;
        }


        private void ShowInsertIndicator(ToolStripMenuItem targetItem)
        {
            ClearInsertIndicator();
            int index = contextMenuStrip1.Items.IndexOf(targetItem);
            ToolStripSeparator insertIndicator = new ToolStripSeparator();
            contextMenuStrip1.Items.Insert(index, insertIndicator);
        }


        #endregion



        private void Item_DragOver(object sender, DragEventArgs e)
        {
            // 更新当前鼠标位置下的放置位置提示
            // 获取鼠标位置
            Point clientPoint = ((Control)sender).PointToClient(new Point(e.X, e.Y));

            // 获取鼠标位置的菜单项
            ToolStripMenuItem targetItem = contextMenuStrip1.GetItemAt(clientPoint) as ToolStripMenuItem;

            // 如果目标项存在且与拖动项不同，则更新目标项的高亮状态
            if (targetItem != null && targetItem != draggedItem)
            {
                // 更新目标项的高亮状态
                targetItem.BackColor = SystemColors.Highlight;
                targetItem.ForeColor = SystemColors.HighlightText;
            }
            else
            {
                // 恢复默认颜色
                targetItem?.ResetBackColor();
                targetItem?.ResetForeColor();
            }
        }

        private void Item_DragDrop(object sender, DragEventArgs e)
        {
            ToolStripMenuItem sourceItem = (ToolStripMenuItem)e.Data.GetData(typeof(ToolStripMenuItem));
            ToolStripMenuItem targetItem = (ToolStripMenuItem)sender;

            if (sourceItem != null && targetItem != null && sourceItem != targetItem)
            {
                int sourceIndex = contextMenuStrip1.Items.IndexOf(sourceItem);
                int targetIndex = contextMenuStrip1.Items.IndexOf(targetItem);

                if (sourceIndex < targetIndex) targetIndex--; // 如果拖动到上方，目标索引需要调整
                contextMenuStrip1.Items.Remove(sourceItem);
                contextMenuStrip1.Items.Insert(targetIndex, sourceItem);
            }
        }

        /*
        private void Si_Click(object sender, EventArgs e)
        {
            KeyValuePair<string, SGDefineColumnItem> item = items.Find(kv => kv.Key == sender.ToString());
            if (item.Value == null)
            {
                return;
            }
            item.Value.Visible = (sender as ToolStripMenuItem).Checked;

            //var itemData = (KeyValuePair<string, SourceGridDefineColumnItem>)menuItem.Tag;
            //OnColumnsVisible?.Invoke(itemData);
            OnColumnsVisible(item);
            //要更新到配置中
            if (ConfigColItems.ContainsKey(item.Key))
            {
                ConfigColItems[item.Key] = item.Value.Visible;
            }
            Common.UIHelper.SaveColumnsList(ConfigColItems, _xmlfileName);
        }
        */


        public override void OnClick(CellContext sender, EventArgs e)
        {
            base.OnClick(sender, e);
        }


        public override void OnMouseUp(SourceGrid.CellContext sender, MouseEventArgs e)
        {
            base.OnMouseUp(sender, e);

            if (e.Button == MouseButtons.Right)
                contextMenuStrip1.Show(sender.Grid, new Point(e.X, e.Y));
        }

    }
}


