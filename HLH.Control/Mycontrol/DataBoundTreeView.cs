using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace DataBoundTreeView
{
    public class DataBoundTreeView : System.Windows.Forms.UserControl
    {
        private System.ComponentModel.Container components = null;

        public DataBoundTreeView()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._treeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // _treeView
            // 
            this._treeView.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right);
            this._treeView.ImageIndex = -1;
            this._treeView.Name = "m_treeView";
            this._treeView.SelectedImageIndex = -1;
            this._treeView.Size = new System.Drawing.Size(224, 384);
            this._treeView.TabIndex = 0;
            // 
            // DataViewTree
            // 
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this._treeView});
            this.Name = "DataViewTree";
            this.Size = new System.Drawing.Size(224, 384);
            this.ResumeLayout(false);

        }
        #endregion

        #region Public Properties and Methods

        // The method that initiates the loading of a tree..
        public void LoadTree(DataSet dataSet, TableBinding[] tableBindings)
        {
            // See the TableBinding class for description...
            _tableBindings = tableBindings;

            // Always start with an empty TreeView
            _treeView.Nodes.Clear();

            // There are three events we are going to handle..
            // 1. When the currency manager changes position (this may be initiated from other controls)
            handlerPositionChanged = new EventHandler(cm_PositionChanged);
            // 2. When our treeview's nodes are selected manually, we'll want to update the currency managers
            handlerAfterSelect = new TreeViewEventHandler(tv_AfterSelect);
            // 3. When data is changed we'll need to update the treeview display
            handlerListChanged = new ListChangedEventHandler(cm_ListChanged);

            // This wires and unwires the PositionChanged event for the currency managers
            SetEvents(dataSet, false);

            // We will start loading from all the "Parent" tables in the DataSet
            foreach (DataTable dt in dataSet.Tables)
            {
                // If this is a "Parent Only" table (no ancestors)
                if (dt.ParentRelations.Count == 0)
                {
                    // Add the parent nodes and it's children
                    AddParentNodes(dt, _treeView.Nodes);
                }
            }

            // Wire back up the PositionChanged events
            SetEvents(dataSet, true);

            // Wire up the AfterSelect for our newly loaded TreeView
            _treeView.AfterSelect += handlerAfterSelect;

            // Start the UI with the first node selected.
            if (_treeView.Nodes.Count > 0)
            {
                _treeView.SelectedNode = _treeView.Nodes[0];

                TreeNode node = _treeView.SelectedNode;

                // Since the child lists (IBindingList) are always changing, handle the ListChanged for each
                while (node.Nodes.Count > 0)
                {
                    ((IBindingList)((BoundTreeNode)node.Nodes[0]).CurrencyManager.List).ListChanged -= handlerListChanged;
                    ((IBindingList)((BoundTreeNode)node.Nodes[0]).CurrencyManager.List).ListChanged += handlerListChanged;
                    node = node.Nodes[0];
                }

            }

        }

        // Allow access to client to Wire and Unwire the PositionChanged event for the currency managers.
        // This will help if you ever need to put two or more BoundTreeView controls on the same form.
        public void SetEvents(DataSet dataSet, bool on)
        {
            // For every "Parent Only" table
            foreach (DataTable dt in dataSet.Tables)
            {
                // No ancestors
                if (dt.ParentRelations.Count == 0)
                {
                    if (on)
                    {
                        WirePositionChanged(dt, dt.TableName);
                    }
                    else
                    {
                        UnwirePositionChanged(dt, dt.TableName);
                    }
                }
            }
        }

        // Allow access to the underlying base TreeView
        public TreeView TreeView
        {
            get { return _treeView; }
        }

        #endregion

        #region Private Properties and Methods

        private TreeView _treeView;
        private TableBinding[] _tableBindings;
        private bool DisablePositionChanged = true;
        private TreeViewEventHandler handlerAfterSelect = null;
        private EventHandler handlerPositionChanged = null;
        private ListChangedEventHandler handlerListChanged = null;

        // Wire up the PositionChanged by calling recursively through the navigation path
        // Take special note of how the navigation path is built on each successive call
        private void WirePositionChanged(DataTable dt, String navigationPath)
        {
            // Get the CurrencyManager and apply the handler
            CurrencyManager cm = (CurrencyManager)BindingContext[dt.DataSet, navigationPath];
            cm.PositionChanged += handlerPositionChanged;

            // Do the same for each child relation 
            foreach (DataRelation relation in dt.ChildRelations)
            {
                WirePositionChanged(relation.ChildTable, navigationPath + "." + relation.RelationName);
            }
        }

        // Unwire the PositionChanged by calling recursively through the navigation path
        // Take special note of how the navigation path is built on each successive call
        private void UnwirePositionChanged(DataTable dt, String navigationPath)
        {
            // Get the CurrencyManager and remove the handler
            CurrencyManager cm = (CurrencyManager)BindingContext[dt.DataSet, navigationPath];
            cm.PositionChanged -= handlerPositionChanged;

            // Do the smae for each child relation
            foreach (DataRelation relation in dt.ChildRelations)
            {
                UnwirePositionChanged(relation.ChildTable, navigationPath + "." + relation.RelationName);
            }
        }

        // Start building the tree nodes with the Parent tables...
        private void AddParentNodes(DataTable dt, TreeNodeCollection nodes)
        {
            // Get the CurrencyManager for the Table...
            CurrencyManager cmParent = (CurrencyManager)BindingContext[dt.DataSet, dt.TableName];

            // The variable "i" provides a position counter
            int i = 0;
            foreach (object rowParent in cmParent.List)
            {
                // Cast out the DataRowView, create the custom node (BoundTreeNode) and add it to the TreeView.
                DataRowView drvParent = (DataRowView)rowParent;
                TreeNode nodeParent = CreateNode(drvParent, cmParent, i);
                nodes.Add(nodeParent);
                cmParent.Position = i;

                // Now we add the children for the current Parent row...
                AddChildNodes(dt, nodeParent, dt.TableName);

                i++;
            }

        }

        // Build the child nodes for each parent and then the subsequent children 
        private void AddChildNodes(DataTable dt, TreeNode nodeParent, String relationName)
        {
            // Add the child rows for every relation on this table
            foreach (DataRelation relation in dt.ChildRelations)
            {
                // Get the currency manager for the current navigation path
                CurrencyManager cmChild = (CurrencyManager)BindingContext[dt.DataSet, relationName + "." + relation.RelationName];

                // Again "i" will provide a position counter
                int i = 0;
                foreach (object rowChild in cmChild.List)
                {
                    // Cast out the DataRowView, create the custom node (BoundTreeNode) and add it to the TreeView.
                    DataRowView drvChild = (DataRowView)rowChild;
                    TreeNode nodeChild = CreateNode(drvChild, cmChild, i);

                    // Add it to the parent node's node collection
                    nodeParent.Nodes.Add(nodeChild);
                    cmChild.Position = i;

                    // Now we add the children for the current Child row (Grandchildren)...
                    AddChildNodes(relation.ChildTable, nodeChild, relationName + "." + relation.RelationName);

                    i++;
                }

            }
        }


        // Mechanism to build a custom BoundTreeNode, supplies the data, currency and position...
        private BoundTreeNode CreateNode(DataRowView drv, CurrencyManager cm, int position)
        {
            // TableBinding object allows us to customize the DisplayMember and ValueMember for each binding
            TableBinding tableBinding = GetBinding(drv.Row.Table.TableName);

            BoundTreeNode node = null;

            if (tableBinding != null)
            {
                node = new BoundTreeNode(drv[tableBinding.DisplayMember].ToString(), drv[tableBinding.ValueMember], cm, position, tableBinding.ImageIndex, tableBinding.SelectedImageIndex);
            }
            else
            {
                // when no binding is supplied, default to the first datarow column...
                node = new BoundTreeNode(drv[0].ToString(), drv[0], cm, position, -1, -1);
            }

            return node;
        }


        // Retrieve the TableBinding object from the private collection... 
        private TableBinding GetBinding(String tableName)
        {
            // Each table can have one and only one binding
            foreach (TableBinding binding in _tableBindings)
            {
                if (binding.TableName == tableName)
                    return binding;
            }

            return null;
        }


        #endregion

        // When the BoundTreeView's nodes are selected, we must synchronize the CurrencyManagers...
        private void tv_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // We have to move the currency manager positions for every node in the
            // selected heirarchy because the parent node selection determines the
            // currency manager "list" contents for the children
            ArrayList nodeList = new ArrayList();

            // Start with the node that has been selected
            BoundTreeNode node = (BoundTreeNode)((TreeView)sender).SelectedNode;
            nodeList.Add(node);

            // Recursively add all the parent nodes
            node = (BoundTreeNode)node.Parent;
            while (node != null)
            {
                nodeList.Add(node);
                node = (BoundTreeNode)node.Parent;
            }

            // Don't fire the our own position change event other controls bound to the
            // currency managers will move accordingly because we are setting the position
            // explicitly
            DisablePositionChanged = true;

            // Start at the highest parent node
            for (int i = nodeList.Count; i > 0; i--)
            {
                node = (BoundTreeNode)nodeList[i - 1];

                ((IBindingList)node.CurrencyManager.List).ListChanged -= handlerListChanged;
                node.CurrencyManager.Position = node.Position;
                ((IBindingList)node.CurrencyManager.List).ListChanged += handlerListChanged;

            }
            DisablePositionChanged = false;

        }

        // When the CurrencyManagers change position we must reposition the TreeView...
        private void cm_PositionChanged(object sender, EventArgs e)
        {
            // We manually disable this if we are changing position from tv_AfterSelect
            if (!DisablePositionChanged)
            {
                CurrencyManager cm = (CurrencyManager)sender;

                // The position may be -1 if the currency manager list is empty
                if (cm.Position >= 0)
                {
                    DataRowView drv = (DataRowView)((DataView)cm.List)[cm.Position];
                    DataRow dr = drv.Row;

                    // other controls (DataGrid) may allow adding rows that are unaccessible
                    if (dr.RowState != DataRowState.Detached)
                    {
                        // Start with the data row that was selected
                        ArrayList dataRows = new ArrayList();
                        dataRows.Add(dr);

                        // We have to select the parents first so that we only search the 
                        // specific lineage when we call SelectNode
                        while (dr.Table.ParentRelations.Count > 0)
                        {
                            dr = dr.GetParentRow(dr.Table.ParentRelations[0]);
                            dataRows.Add(dr);
                        }

                        // Start searching the tree with the base nodes collection
                        TreeNodeCollection nodes = _treeView.Nodes;
                        TreeNode node = null;
                        TableBinding tableBinding;

                        // Select the highest parent and then the subsequent children from
                        // the returned node's collection of children

                        // Start with the highest datarow in the heirarchy
                        for (int i = dataRows.Count; i > 0; i--)
                        {
                            dr = (DataRow)dataRows[i - 1];

                            // TableBinding tells us what the field in the datarow is that will be
                            // compared to the tag value in the node
                            tableBinding = GetBinding(dr.Table.TableName);

                            // Find the node and then search it's children for the next datarow
                            if (tableBinding != null)
                                node = SelectNode(dr[tableBinding.ValueMember], nodes);
                            else
                                node = SelectNode(dr[0], nodes);

                            // The next nodes collection to search
                            nodes = node.Nodes;
                        }

                        // We're going to move the tree node selection here, but we don't want
                        // the AfterSelect event to be handled because it would fire the
                        // currency manager PositionChanged event reciprocally
                        _treeView.AfterSelect -= handlerAfterSelect;
                        _treeView.SelectedNode = node;
                        _treeView.AfterSelect += handlerAfterSelect;

                        // The (IBindingList) has changed, so wire up the child lists to the handler
                        while (node.Nodes.Count > 0)
                        {
                            ((IBindingList)((BoundTreeNode)node.Nodes[0]).CurrencyManager.List).ListChanged -= handlerListChanged;
                            ((IBindingList)((BoundTreeNode)node.Nodes[0]).CurrencyManager.List).ListChanged += handlerListChanged;
                            node = node.Nodes[0];
                        }

                    }
                }
            }
        }

        // Some data in the lists has changed, we may need to update the TreeView Display
        private void cm_ListChanged(object sender, ListChangedEventArgs e)
        {
            // Cast the sender to a DataView
            DataView dv = (DataView)sender;

            // Get the DataRowView of the newly selected row in the "list".
            DataRowView drv = (DataRowView)dv[e.NewIndex];
            DataRow dr = drv.Row;

            // Start searching the tree with the base nodes collection
            TreeNodeCollection nodes = _treeView.Nodes;
            TreeNode node = null;
            TableBinding tableBinding;

            // TableBinding tells us what the field in the datarow is that will be
            // compared to the tag value in the node and what the display value is
            tableBinding = GetBinding(dr.Table.TableName);

            // Find the node
            if (tableBinding != null)
            {
                node = SelectNode(dr[tableBinding.ValueMember], nodes);
                node.Text = dr[tableBinding.DisplayMember].ToString();
            }
            else
            {
                node = SelectNode(dr[0], nodes);
                node.Text = dr[0].ToString();
            }

        }


        // Called recursively. The search is performed left to right and then down the tree.
        private TreeNode SelectNode(object item, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                // If this is the node return it
                if (((BoundTreeNode)node).Tag.Equals(item))
                {
                    // It is so select this node and return it
                    return node;
                }

                // Current node isn't the one so search it's children (there may be none)
                TreeNode foundNode = SelectNode(item, node.Nodes);

                // If it was found in the child collection return it
                if (foundNode != null)
                    return foundNode;
            }

            // There may be no nodes in the collection ie. it's a leaf node or it wasn't found at all
            return null;

        }

    }

    // Custom class that allows us to store more details on the TreeNode object
    public class BoundTreeNode : TreeNode
    {
        // Name of the Table that corresponds to the node's data
        private string _tableName;

        // The position of the row in the CurrencyManager.List
        private int _position;

        // The CurrencyManager of the Table or Navigation Path
        private CurrencyManager _cm;

        // Construct the node
        public BoundTreeNode(string nodeLabel, object nodeValue, CurrencyManager cm, int position, int imageIndex, int selectedImageIndex)
        {
            _tableName = ((DataView)cm.List).Table.TableName;
            _position = position;
            _cm = cm;
            this.Text = nodeLabel;

            // Node value could be any data type, so we store it as an object in the tag field
            this.Tag = nodeValue;

            // Unnecessary, but I know everyone will ask for it... These are set in the client code.
            this.ImageIndex = imageIndex;
            this.SelectedImageIndex = selectedImageIndex;
        }

        public object Value
        {
            get
            {
                return this.Tag;
            }
        }

        public String TableName
        {
            get
            {
                return _tableName;
            }
        }

        public int Position
        {
            get
            {
                return _position;
            }
        }

        public CurrencyManager CurrencyManager
        {
            get
            {
                return _cm;
            }
        }

    }

    // Simple class to store Table Binding information such as DisplayMember and ValueMember
    public class TableBinding
    {
        public string TableName;
        public string ValueMember;
        public string DisplayMember;
        public int ImageIndex;
        public int SelectedImageIndex;

        public TableBinding(string tableName, string valueMember, string displayMember)
        {
            TableName = tableName;
            ValueMember = valueMember;
            DisplayMember = displayMember;
            ImageIndex = -1;
            SelectedImageIndex = -1;
        }

        public TableBinding(string tableName, string valueMember, string displayMember, int imageIndex, int selectedImageIndex)
        {
            TableName = tableName;
            ValueMember = valueMember;
            DisplayMember = displayMember;
            ImageIndex = imageIndex;
            SelectedImageIndex = selectedImageIndex;
        }
    }

}
