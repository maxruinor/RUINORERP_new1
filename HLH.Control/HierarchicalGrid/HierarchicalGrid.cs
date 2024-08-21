//---------------------------------------------------------------------
// 
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// 
//THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
//KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//PARTICULAR PURPOSE.
//---------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace HierarchicalGrid
{
    /// <summary>
    /// 应该是一个主从显示的表格控制。但是不清楚用法可能代码也不成熟
    /// </summary>
    [System.ComponentModel.DesignerCategory("code"),
    Designer(typeof(System.Windows.Forms.Design.ControlDesigner)),
    ComplexBindingProperties(),
    Docking(DockingBehavior.Ask)]
    public class HierarchicalGridView : DataGridView
    {
        private int _indentWidth;
        private HierarchicalGridNode _root;
        private HierarchicalGridColumn _expandableColumn;
        private bool _disposing = false;
        internal ImageList _imageList;
        private bool _inExpandCollapse = false;
        internal bool _inExpandCollapseMouseCapture = false;
        private Control hideScrollBarControl;
        private bool _showLines = true;
        private bool _virtualNodes = false;
        private DataGridSource _dtSource = null;

        internal VisualStyleRenderer rOpen = new VisualStyleRenderer(VisualStyleElement.TreeView.Glyph.Opened);
        internal VisualStyleRenderer rClosed = new VisualStyleRenderer(VisualStyleElement.TreeView.Glyph.Closed);

        #region Constructor
        public HierarchicalGridView()
        {
            // Control when edit occurs because edit mode shouldn't start when expanding/collapsing
            this.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.RowTemplate = new HierarchicalGridNode() as DataGridViewRow;
            // This sample does not support adding or deleting rows by the user.
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this._root = new HierarchicalGridNode(this);
            this._root.IsRoot = true;

            // Ensures that all rows are added unshared by listening to the CollectionChanged event.
            base.Rows.CollectionChanged += delegate (object sender, System.ComponentModel.CollectionChangeEventArgs e) { };
        }
        #endregion

        #region Keyboard F2 to begin edit support
        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Cause edit mode to begin since edit mode is disabled to support 
            // expanding/collapsing 
            base.OnKeyDown(e);
            if (!e.Handled)
            {
                if (e.KeyCode == Keys.F2 && this.CurrentCellAddress.X > -1 && this.CurrentCellAddress.Y > -1)
                {
                    if (!this.CurrentCell.Displayed)
                    {
                        this.FirstDisplayedScrollingRowIndex = this.CurrentCellAddress.Y;
                    }
                    else
                    {
                        // TODO:calculate if the cell is partially offscreen and if so scroll into view
                    }
                    this.SelectionMode = DataGridViewSelectionMode.CellSelect;
                    this.BeginEdit(true);
                }
                else if (e.KeyCode == Keys.Enter && !this.IsCurrentCellInEditMode)
                {
                    this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    this.CurrentCell.OwningRow.Selected = true;
                }
            }
        }
        #endregion

        #region Shadow and hide DGV properties - NEW DATASOURCE

        // This sample does not support databinding
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Never)]
        public new object DataSource
        {
            get { return _dtSource; }
            set
            {
                this.Nodes.Clear();
                this.Columns.Clear();

                if (!(value is DataGridSource))
                    throw new Exception("DataSource must be a DataGridSource object!");

                _dtSource = (DataGridSource)value;

                // Armazena as colunas de relacionamento
                List<String> relColList = new List<string>();
                foreach (DataRelation curRelation in _dtSource.DataSet.Relations)
                {
                    foreach (DataColumn curCol in curRelation.ParentKeyConstraint.Columns)
                    {
                        relColList.Add(curCol.ColumnName);
                    }
                }


                // Adiciona as colunas no grid
                bool relationCol;
                foreach (string curCol in _dtSource.DisplayColumns)
                {
                    relationCol = relColList.Contains(curCol);

                    if (relationCol)
                    {
                        HierarchicalGridColumn newCol = new HierarchicalGridColumn();
                        relationCol = false;
                        newCol.Name = curCol;
                        newCol.HeaderText = curCol;
                        this.Columns.Add(newCol);
                    }
                    else
                    {
                        DataGridViewTextBoxColumn newCol = new DataGridViewTextBoxColumn();
                        newCol.Name = curCol;
                        newCol.HeaderText = curCol;
                        this.Columns.Add(newCol);
                    }
                }

                foreach (GroupColumn curGroup in _dtSource.GroupColumns)
                {
                    DataGridViewTextBoxColumn newCol = new DataGridViewTextBoxColumn();
                    newCol.Name = curGroup.ColName;
                    newCol.HeaderText = curGroup.ColName;
                    this.Columns.Add(newCol);
                }

                // Monta o Grid Hierquico
                DataRelationCollection dsRelations = _dtSource.DataSet.Relations;
                HierarchicalGridNode curNode;

                foreach (DataRelation curRelation in dsRelations)
                {
                    DataTable dtParent = curRelation.ParentTable;
                    DataTable dtChild = curRelation.ChildTable;
                    foreach (DataRow curRow in dtParent.Rows)
                    {
                        string childFilter = "";
                        for (int iCol = 0; iCol < curRelation.ChildKeyConstraint.Columns.Length; iCol++)
                        {
                            if (iCol > 0)
                            {
                                childFilter += " AND ";
                            }
                            childFilter += curRelation.ChildKeyConstraint.Columns[iCol].ColumnName + "='" + curRow[curRelation.ChildKeyConstraint.RelatedColumns[iCol]].ToString() + "'";
                        }

                        object[] valuesToAdd = new object[this.Columns.Count];
                        int curObj = 0;
                        foreach (string curColName in _dtSource.DisplayColumns)
                        {
                            if (dtParent.Columns.Contains(curColName))
                            {
                                valuesToAdd[curObj] = curRow[curColName];
                            }
                            else
                            {
                                valuesToAdd[curObj] = null;
                            }
                            curObj++;
                        }

                        foreach (GroupColumn curCol in _dtSource.GroupColumns)
                        {
                            if (dtChild.Columns.Contains(curCol.ColName))
                            {
                                valuesToAdd[curObj] = dtChild.Compute(curCol.GroupType.ToString() + "(" + curCol.ColName + ")", childFilter);
                            }
                            else
                            {
                                valuesToAdd[curObj] = null;
                            }
                            curObj++;
                        }

                        curNode = this.Nodes.Add(valuesToAdd);
                        DataRow[] childRows = dtChild.Select(childFilter);

                        foreach (DataRow curChildRow in childRows)
                        {
                            valuesToAdd = new object[this.Columns.Count];
                            curObj = 0;
                            foreach (DataGridViewColumn curCol in this.Columns)
                            {
                                if (dtChild.Columns.Contains(curCol.Name))
                                {
                                    valuesToAdd[curObj] = curChildRow[curCol.Name];
                                }
                                else
                                {
                                    valuesToAdd[curObj] = null;
                                }
                                curObj++;
                            }
                            curNode.Nodes.Add(valuesToAdd);
                        }

                    }
                }
            }
        }

        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
    EditorBrowsable(EditorBrowsableState.Never)]
        public new object DataMember
        {
            get { return null; }
            set { /*throw new NotSupportedException("The TreeGridView does not support databinding");*/ }
        }

        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Never)]
        public new DataGridViewRowCollection Rows
        {
            get { return base.Rows; }
        }

        //[Browsable(false),
        //DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        //EditorBrowsable(EditorBrowsableState.Never)]
        public new bool VirtualMode
        {
            get { return false; }
            set { /*throw new NotSupportedException("The TreeGridView does not support virtual mode");*/ }
        }

        // none of the rows/nodes created use the row template, so it is hidden.
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Never)]
        public new DataGridViewRow RowTemplate
        {
            get { return base.RowTemplate; }
            set { base.RowTemplate = value; }
        }

        #endregion

        #region Public methods
        [Description("Returns the TreeGridNode for the given DataGridViewRow")]
        public HierarchicalGridNode GetNodeForRow(DataGridViewRow row)
        {
            return row as HierarchicalGridNode;
        }

        [Description("Returns the TreeGridNode for the given DataGridViewRow")]
        public HierarchicalGridNode GetNodeForRow(int index)
        {
            return GetNodeForRow(base.Rows[index]);
        }
        #endregion

        #region Public properties
        [Category("Data"),
        Description("The collection of root nodes in the treelist."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        public HierarchicalGridNodeCollection Nodes
        {
            get
            {
                return this._root.Nodes;
            }
        }

        public new HierarchicalGridNode CurrentRow
        {
            get
            {
                return base.CurrentRow as HierarchicalGridNode;
            }
        }

        [DefaultValue(false),
        Description("Causes nodes to always show as expandable. Use the NodeExpanding event to add nodes.")]
        public bool VirtualNodes
        {
            get { return _virtualNodes; }
            set { _virtualNodes = value; }
        }

        public HierarchicalGridNode CurrentNode
        {
            get
            {
                return this.CurrentRow;
            }
        }

        [DefaultValue(true)]
        public bool ShowLines
        {
            get { return this._showLines; }
            set
            {
                if (value != this._showLines)
                {
                    this._showLines = value;
                    this.Invalidate();
                }
            }
        }

        public ImageList ImageList
        {
            get { return this._imageList; }
            set
            {
                this._imageList = value;
                //TODO: should we invalidate cell styles when setting the image list?

            }
        }

        public new int RowCount
        {
            get { return this.Nodes.Count; }
            set
            {
                for (int i = 0; i < value; i++)
                    this.Nodes.Add(new HierarchicalGridNode());

            }
        }
        #endregion

        #region Site nodes and collapse/expand support
        protected override void OnRowsAdded(DataGridViewRowsAddedEventArgs e)
        {
            base.OnRowsAdded(e);

            // Notify the row when it is added to the base grid 
            int count = e.RowCount - 1;
            HierarchicalGridNode row;
            while (count >= 0)
            {
                row = base.Rows[e.RowIndex + count] as HierarchicalGridNode;
                if (row != null)
                {
                    row.Sited();
                }
                count--;
            }
        }

        internal protected void UnSiteAll()
        {
            this.UnSiteNode(this._root);
        }

        public override void Sort(DataGridViewColumn dataGridViewColumn, ListSortDirection direction)
        {

            IList<HierarchicalGridNode> nodeList = this.Nodes._list;
            IList<HierarchicalGridNode> expandedNodes = new List<HierarchicalGridNode>();
            int nodeIndex, iNode;

            for (iNode = 0; iNode < nodeList.Count; iNode++)
            {
                if (nodeList[iNode].IsExpanded)
                {
                    expandedNodes.Add(nodeList[iNode]);
                    nodeList[iNode].Collapse();
                }
            }

            base.Sort(dataGridViewColumn, direction);

            for (iNode = 0; iNode < expandedNodes.Count; iNode++) //(TreeGridNode nodeToExpand in expandedNodes)
            {
                nodeIndex = this.Nodes.IndexOf(expandedNodes[iNode]);
                this.Nodes[nodeIndex].Expand();
            }
        }

        internal protected virtual void UnSiteNode(HierarchicalGridNode node)
        {
            if (node.IsSited || node.IsRoot)
            {
                // remove child rows first
                foreach (HierarchicalGridNode childNode in node.Nodes)
                {
                    this.UnSiteNode(childNode);
                }

                // now remove this row except for the root
                if (!node.IsRoot)
                {
                    base.Rows.Remove(node);
                    // Row isn't sited in the grid anymore after remove. Note that we cannot
                    // Use the RowRemoved event since we cannot map from the row index to
                    // the index of the expandable row/node.
                    node.UnSited();
                }
            }
        }

        internal protected virtual bool CollapseNode(HierarchicalGridNode node)
        {
            if (node.IsExpanded)
            {
                CollapsingEventArgs exp = new CollapsingEventArgs(node);
                this.OnNodeCollapsing(exp);

                if (!exp.Cancel)
                {
                    this.LockVerticalScrollBarUpdate(true);
                    this.SuspendLayout();
                    _inExpandCollapse = true;
                    node.IsExpanded = false;

                    foreach (HierarchicalGridNode childNode in node.Nodes)
                    {
                        Debug.Assert(childNode.RowIndex != -1, "Row is NOT in the grid.");
                        this.UnSiteNode(childNode);
                    }

                    CollapsedEventArgs exped = new CollapsedEventArgs(node);
                    this.OnNodeCollapsed(exped);
                    //TODO: Convert this to a specific NodeCell property
                    _inExpandCollapse = false;
                    this.LockVerticalScrollBarUpdate(false);
                    this.ResumeLayout(true);
                    this.InvalidateCell(node.Cells[0]);

                }

                return !exp.Cancel;
            }
            else
            {
                // row isn't expanded, so we didn't do anything.				
                return false;
            }
        }

        internal protected virtual void SiteNode(HierarchicalGridNode node)
        {
            //TODO: Raise exception if parent node is not the root or is not sited.
            int rowIndex = -1;
            HierarchicalGridNode currentRow;
            node._grid = this;

            if (node.Parent != null && node.Parent.IsRoot == false)
            {
                // row is a child
                Debug.Assert(node.Parent != null && node.Parent.IsExpanded == true);

                if (node.Index > 0)
                {
                    currentRow = node.Parent.Nodes[node.Index - 1];
                }
                else
                {
                    currentRow = node.Parent;
                }
            }
            else
            {
                // row is being added to the root
                if (node.Index > 0)
                {
                    currentRow = node.Parent.Nodes[node.Index - 1];
                }
                else
                {
                    currentRow = null;
                }

            }

            if (currentRow != null)
            {
                while (currentRow.Level >= node.Level)
                {
                    if (currentRow.RowIndex < base.Rows.Count - 1)
                    {
                        currentRow = base.Rows[currentRow.RowIndex + 1] as HierarchicalGridNode;
                        Debug.Assert(currentRow != null);
                    }
                    else
                        // no more rows, site this node at the end.
                        break;

                }
                if (currentRow == node.Parent)
                    rowIndex = currentRow.RowIndex + 1;
                else if (currentRow.Level < node.Level)
                    rowIndex = currentRow.RowIndex;
                else
                    rowIndex = currentRow.RowIndex + 1;
            }
            else
                rowIndex = 0;


            Debug.Assert(rowIndex != -1);
            this.SiteNode(node, rowIndex);

            Debug.Assert(node.IsSited);
            if (node.IsExpanded)
            {
                // add all child rows to display
                foreach (HierarchicalGridNode childNode in node.Nodes)
                {
                    //TODO: could use the more efficient SiteRow with index.
                    this.SiteNode(childNode);
                }
            }
        }


        internal protected virtual void SiteNode(HierarchicalGridNode node, int index)
        {
            if (index < base.Rows.Count)
            {
                base.Rows.Insert(index, node);
            }
            else
            {
                // for the last item.
                base.Rows.Add(node);
            }
        }

        internal protected virtual bool ExpandNode(HierarchicalGridNode node)
        {
            if (!node.IsExpanded || this._virtualNodes)
            {
                ExpandingEventArgs exp = new ExpandingEventArgs(node);
                this.OnNodeExpanding(exp);

                if (!exp.Cancel)
                {
                    this.LockVerticalScrollBarUpdate(true);
                    this.SuspendLayout();
                    _inExpandCollapse = true;
                    node.IsExpanded = true;

                    //TODO Convert this to a InsertRange
                    foreach (HierarchicalGridNode childNode in node.Nodes)
                    {
                        Debug.Assert(childNode.RowIndex == -1, "Row is already in the grid.");

                        this.SiteNode(childNode);
                        //this.BaseRows.Insert(rowIndex + 1, childRow);
                        //TODO : remove -- just a test.
                        //childNode.Cells[0].Value = "child";
                    }

                    ExpandedEventArgs exped = new ExpandedEventArgs(node);
                    this.OnNodeExpanded(exped);
                    //TODO: Convert this to a specific NodeCell property
                    _inExpandCollapse = false;
                    this.LockVerticalScrollBarUpdate(false);
                    this.ResumeLayout(true);
                    this.InvalidateCell(node.Cells[0]);
                }

                return !exp.Cancel;
            }
            else
            {
                // row is already expanded, so we didn't do anything.
                return false;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            // used to keep extra mouse moves from selecting more rows when collapsing
            base.OnMouseUp(e);
            this._inExpandCollapseMouseCapture = false;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // while we are expanding and collapsing a node mouse moves are
            // supressed to keep selections from being messed up.
            if (!this._inExpandCollapseMouseCapture)
                base.OnMouseMove(e);

        }
        #endregion

        #region Collapse/Expand events
        public event ExpandingEventHandler NodeExpanding;
        public event ExpandedEventHandler NodeExpanded;
        public event CollapsingEventHandler NodeCollapsing;
        public event CollapsedEventHandler NodeCollapsed;

        protected virtual void OnNodeExpanding(ExpandingEventArgs e)
        {
            if (this.NodeExpanding != null)
            {
                NodeExpanding(this, e);
            }
        }
        protected virtual void OnNodeExpanded(ExpandedEventArgs e)
        {
            if (this.NodeExpanded != null)
            {
                NodeExpanded(this, e);
            }
        }
        protected virtual void OnNodeCollapsing(CollapsingEventArgs e)
        {
            if (this.NodeCollapsing != null)
            {
                NodeCollapsing(this, e);
            }

        }
        protected virtual void OnNodeCollapsed(CollapsedEventArgs e)
        {
            if (this.NodeCollapsed != null)
            {
                NodeCollapsed(this, e);
            }
        }
        #endregion

        #region Helper methods
        protected override void Dispose(bool disposing)
        {
            this._disposing = true;
            base.Dispose(Disposing);
            this.UnSiteAll();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // this control is used to temporarly hide the vertical scroll bar
            hideScrollBarControl = new Control();
            hideScrollBarControl.Visible = false;
            hideScrollBarControl.Enabled = false;
            hideScrollBarControl.TabStop = false;
            // control is disposed automatically when the grid is disposed
            this.Controls.Add(hideScrollBarControl);
        }

        protected override void OnRowEnter(DataGridViewCellEventArgs e)
        {
            // ensure full row select
            base.OnRowEnter(e);
            if (this.SelectionMode == DataGridViewSelectionMode.CellSelect ||
                (this.SelectionMode == DataGridViewSelectionMode.FullRowSelect &&
                base.Rows[e.RowIndex].Selected == false))
            {
                this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                base.Rows[e.RowIndex].Selected = true;
            }
        }

        private void LockVerticalScrollBarUpdate(bool lockUpdate/*, bool delayed*/)
        {
            // Temporarly hide/show the vertical scroll bar by changing its parent
            if (!this._inExpandCollapse)
            {
                if (lockUpdate)
                {
                    this.VerticalScrollBar.Parent = hideScrollBarControl;
                }
                else
                {
                    this.VerticalScrollBar.Parent = this;
                }
            }
        }

        protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
        {
            if (typeof(HierarchicalGridColumn).IsAssignableFrom(e.Column.GetType()))
            {
                if (_expandableColumn == null)
                {
                    // identify the expanding column.			
                    _expandableColumn = (HierarchicalGridColumn)e.Column;
                }
                else
                {
                    // this.Columns.Remove(e.Column);
                    //throw new InvalidOperationException("Only one TreeGridColumn per TreeGridView is supported.");
                }
            }

            // Expandable Grid doesn't support sorting. This is just a limitation of the sample.
            //e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;

            base.OnColumnAdded(e);
        }

        private static class Win32Helper
        {
            public const int WM_SYSKEYDOWN = 0x0104,
                             WM_KEYDOWN = 0x0100,
                             WM_SETREDRAW = 0x000B;

            [System.Runtime.InteropServices.DllImport("USER32.DLL", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            public static extern IntPtr SendMessage(System.Runtime.InteropServices.HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);

            [System.Runtime.InteropServices.DllImport("USER32.DLL", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            public static extern IntPtr SendMessage(System.Runtime.InteropServices.HandleRef hWnd, int msg, int wParam, int lParam);

            [System.Runtime.InteropServices.DllImport("USER32.DLL", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            public static extern bool PostMessage(System.Runtime.InteropServices.HandleRef hwnd, int msg, IntPtr wparam, IntPtr lparam);

        }
        #endregion


    }
}
