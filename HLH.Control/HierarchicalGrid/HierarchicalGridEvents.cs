//---------------------------------------------------------------------
// 
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// 
//THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
//KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//PARTICULAR PURPOSE.
//---------------------------------------------------------------------

namespace HierarchicalGrid
{
    public class HierarchicalGridNodeEventBase
    {
        private HierarchicalGridNode _node;

        public HierarchicalGridNodeEventBase(HierarchicalGridNode node)
        {
            this._node = node;
        }

        public HierarchicalGridNode Node
        {
            get { return _node; }
        }
    }
    public class CollapsingEventArgs : System.ComponentModel.CancelEventArgs
    {
        private HierarchicalGridNode _node;

        private CollapsingEventArgs() { }
        public CollapsingEventArgs(HierarchicalGridNode node)
            : base()
        {
            this._node = node;
        }
        public HierarchicalGridNode Node
        {
            get { return _node; }
        }

    }
    public class CollapsedEventArgs : HierarchicalGridNodeEventBase
    {
        public CollapsedEventArgs(HierarchicalGridNode node)
            : base(node)
        {
        }
    }

    public class ExpandingEventArgs : System.ComponentModel.CancelEventArgs
    {
        private HierarchicalGridNode _node;

        private ExpandingEventArgs() { }
        public ExpandingEventArgs(HierarchicalGridNode node) : base()
        {
            this._node = node;
        }
        public HierarchicalGridNode Node
        {
            get { return _node; }
        }

    }
    public class ExpandedEventArgs : HierarchicalGridNodeEventBase
    {
        public ExpandedEventArgs(HierarchicalGridNode node) : base(node)
        {
        }
    }

    public delegate void ExpandingEventHandler(object sender, ExpandingEventArgs e);
    public delegate void ExpandedEventHandler(object sender, ExpandedEventArgs e);

    public delegate void CollapsingEventHandler(object sender, CollapsingEventArgs e);
    public delegate void CollapsedEventHandler(object sender, CollapsedEventArgs e);

}
