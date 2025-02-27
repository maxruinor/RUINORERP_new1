using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.Common
{
    using RUINORERP.Business.CommService;
    using RUINORERP.Model;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    /// AI代码借鉴 没有使用
    /// </summary>
    public class GridViewDisplayNameResolver
    {
        #region Configuration Classes
        public class ForeignKeyMapping
        {
            public string SourceColumn { get; set; }
            public string TargetTable { get; set; }
            public string KeyColumn { get; set; } = "ID";
            public string DisplayColumn { get; set; } = "Name";
        }

        public class DictionaryMapping
        {
            public string ColumnName { get; set; }
            public List<KeyValuePair<object, string>> KeyValuePairs { get; set; }
        }

        public class ColumnAlias
        {
            public string OriginalName { get; set; }
            public string AliasName { get; set; }
        }
        #endregion

        #region Properties
        public List<ForeignKeyMapping> ForeignKeyMappings { get; } = new List<ForeignKeyMapping>();
        public List<DictionaryMapping> DictionaryMappings { get; } = new List<DictionaryMapping>();
        public List<ColumnAlias> ColumnAliases { get; } = new List<ColumnAlias>();
        public List<string> ImageColumns { get; } = new List<string>();
        public List<string> SpecialEmployeeColumns { get; } = new List<string> { "Created_by", "Modified_by", "Approver_by" };
        #endregion

        #region Public Methods
        public void Attach(DataGridView dataGridView)
        {
            dataGridView.CellFormatting += HandleCellFormatting;
        }

        public void Detach(DataGridView dataGridView)
        {
            dataGridView.CellFormatting -= HandleCellFormatting;
        }
        #endregion

        #region Event Handling
        private void HandleCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid?.Columns[e.ColumnIndex]?.Visible != true) return;

            var columnName = GetOriginalColumnName(grid.Columns[e.ColumnIndex].Name);
            var value = e.Value;
            if (value==null)
            {
                return; 
            }
            if (HandleImageFormatting(grid, e, columnName)) return;
            if (HandleSpecialEmployeeColumns(e, columnName, value)) return;
            if (HandleDictionaryMappings(e, columnName, value)) return;
            HandleForeignKeyMappings(e, columnName, value);
        }
        #endregion

        #region Private Methods
        private string GetOriginalColumnName(string columnName)
        {
            var alias = ColumnAliases.FirstOrDefault(a => a.AliasName == columnName);
            return alias?.OriginalName ?? columnName;
        }

        private bool HandleImageFormatting(DataGridView grid, DataGridViewCellFormattingEventArgs e, string columnName)
        {
            if (!ImageColumns.Contains(columnName) && e.Value?.GetType() != typeof(byte[]))
                return false;

            if (e.Value is byte[] bytes)
            {
                using (var ms = new MemoryStream(bytes))
                {
                    var image = Image.FromStream(ms);
                    e.Value = CreateThumbnail(image, 100, 100);
                }
                return true;
            }
            return false;
        }

        private bool HandleSpecialEmployeeColumns(DataGridViewCellFormattingEventArgs e, string columnName, object value)
        {
            if (!SpecialEmployeeColumns.Contains(columnName))
                return false;

            if (value?.ToString() == "0")
            {
                e.Value = string.Empty;
                return true;
            }

            var displayValue = BizCacheHelper.Instance.GetValue(nameof(tb_Employee), value);
            if (displayValue != null)
            {
                e.Value = displayValue.ToString();
                return true;
            }
            return false;
        }

        private bool HandleDictionaryMappings(DataGridViewCellFormattingEventArgs e, string columnName, object value)
        {
            var mapping = DictionaryMappings.FirstOrDefault(m => m.ColumnName == columnName);
            if (mapping == null) return false;

            var pair = mapping.KeyValuePairs.FirstOrDefault(kv =>
                kv.Key.ToString().Equals(value?.ToString(), StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(pair.Value))
            {
                e.Value = pair.Value;
                return true;
            }
            return false;
        }

        private void HandleForeignKeyMappings(DataGridViewCellFormattingEventArgs e, string columnName, object value)
        {
            var mapping = ForeignKeyMappings.FirstOrDefault(m => m.SourceColumn == columnName);
            if (mapping == null) return;

            var displayValue = BizCacheHelper.Instance.GetValue(
                mapping.TargetTable,
                               mapping.KeyColumn


            );

            if (displayValue != null)
            {
                e.Value = displayValue.ToString();
            }
        }

        private Image CreateThumbnail(Image original, int maxWidth, int maxHeight)
        {
            var ratio = Math.Min((double)maxWidth / original.Width, (double)maxHeight / original.Height);
            var newWidth = (int)(original.Width * ratio);
            var newHeight = (int)(original.Height * ratio);

            var thumbnail = new Bitmap(newWidth, newHeight);
            using (var graphics = Graphics.FromImage(thumbnail))
            {
                graphics.DrawImage(original, 0, 0, newWidth, newHeight);
            }
            return thumbnail;
        }
        #endregion
    }
}
