using System;
using System.Collections.Generic;
using System.Data;

// Enum: Sum, Prod, Min, Max, Count, Average, None

namespace HierarchicalGrid
{
    public enum GroupTypeEnum
    {
        Sum = 1,
        Product = 2,
        Min = 3,
        Max = 4,
        Count = 5,
        Average = 6,
    }

    public class GroupColumn
    {
        private string _colName;
        private GroupTypeEnum _groupType;

        public GroupColumn(string colName, GroupTypeEnum groupType)
        {
            _colName = colName;
            _groupType = groupType;
        }

        public string ColName
        {
            get { return _colName; }
        }
        public GroupTypeEnum GroupType
        {
            get { return _groupType; }
        }
    }

    public class DataGridSource
    {
        private DataSet _dataSet;
        private List<string> _dsColumns;
        private List<GroupColumn> _dsGroupColumns;

        public DataGridSource(DataSet dataSet, List<string> dsColumns, List<GroupColumn> dsGroupColumns)
        {
            _dataSet = dataSet;
            _dsColumns = dsColumns;
            _dsGroupColumns = dsGroupColumns;
        }

        public DataSet DataSet
        {
            get
            { return _dataSet; }
            set
            { _dataSet = value; }
        }

        public List<String> DisplayColumns
        {
            get
            {
                return _dsColumns;
            }
        }

        public List<GroupColumn> GroupColumns
        {
            get
            {
                return _dsGroupColumns;
            }
        }
    }
}
