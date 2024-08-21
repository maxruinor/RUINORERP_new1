using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI
{
    public static class dataGridViewExt
    {

        /// <summary>
        /// 只是控制显示中文，并不是列的显示
        /// </summary>
        /// <param name="this"></param>
        /// <param name="fieldNameList"></param>
        [Obsolete]
        public static void ColumnDisplayControl(this DataGridView @this, System.Collections.Concurrent.ConcurrentDictionary<string, string> fieldNameList)
        {
            return;
            if (fieldNameList == null)
            {
                return;
            }
            foreach (DataGridViewColumn var in @this.Columns)
            {

                if (fieldNameList.ContainsKey(var.Name))
                {
                    var.HeaderText = fieldNameList[var.Name];

                    //if (var.HeaderText.Trim().Length == 0)
                    //{
                    //    var.Visible = false;
                    //}
                    //else
                    //{
                    var.Visible = true;
                    //}
                }
                else
                {
                    var.Visible = false;
                }
            }
            /*
            foreach (DataGridViewColumn var in @this.Columns)
            {
                if (fieldNameList.ContainsKey(var.Name))
                {
                    var.HeaderText = fieldNameList[var.Name];
                    if (var.HeaderText.Trim().Length == 0)
                    {
                        var.Visible = false;
                    }
                    else
                    {
                        var.Visible = true;
                    }
                }
                else
                {
                    var.Visible = false;
                }
            }
            */
        }

        #region 实际选择列右键全选不选
        /// <summary>
        /// 控制列的显示
        /// </summary>
        /// <param name="this"></param>
        /// <param name="fieldNameList"></param>
        /// <param name="ShowSelectCol">是否显示选择列</param>
        public static void ColumnDisplayControl(this DataGridView @this, System.Collections.Concurrent.ConcurrentDictionary<string, string> fieldNameList, bool ShowSelectCol)
        {
            if (fieldNameList == null)
            {
                return;
            }
            foreach (DataGridViewColumn var in @this.Columns)
            {
                if (ShowSelectCol && var.Name == "Selected")
                {
                    @this.ReadOnly = false;
                    var.Width = 55;
                    var.DisplayIndex = 0;
                    @this.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    @this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    var.HeaderText = "选择";
                    var.Visible = true;
                    var.Frozen = true;
                    //var.ContextMenuStrip = SetContextMenuStripForSelect();

                    continue;
                }
                if (fieldNameList.ContainsKey(var.Name))
                {
                    var.HeaderText = fieldNameList[var.Name];

                    //if (var.HeaderText.Trim().Length == 0)
                    //{
                    //    var.Visible = false;
                    //}
                    //else
                    //{
                    var.Visible = true;
                    //}
                }
                else
                {
                    var.Visible = false;
                }
            }
            /*
            foreach (DataGridViewColumn var in @this.Columns)
            {
                if (fieldNameList.ContainsKey(var.Name))
                {
                    var.HeaderText = fieldNameList[var.Name];
                    if (var.HeaderText.Trim().Length == 0)
                    {
                        var.Visible = false;
                    }
                    else
                    {
                        var.Visible = true;
                    }
                }
                else
                {
                    var.Visible = false;
                }
            }
            */
        }
        #endregion



        /// <summary>
        /// 2024-2-19不再使用
        /// 只是控制显示中文，并不是列的显示
        /// </summary>
        /// <param name="this"></param>
        /// <param name="fieldNameList"></param>
        [Obsolete]
        public static void ColumnDisplayControlToCaption(this DataGridView @this, System.Collections.Concurrent.ConcurrentDictionary<string, KeyValuePair<string, bool>> fieldNameList)
        {
            return;
            foreach (DataGridViewColumn var in @this.Columns)
            {
                if (fieldNameList.ContainsKey(var.Name))
                {
                    var.HeaderText = fieldNameList[var.Name].Key;
                    var.Visible = fieldNameList[var.Name].Value;
                }
                else
                {
                    var.Visible = false;
                }
            }


            /*
            foreach (DataGridViewColumn var in @this.Columns)
            {
                if (fieldNameList.ContainsKey(var.Name))
                {
                    var.HeaderText = fieldNameList[var.Name];
                    if (var.HeaderText.Trim().Length == 0)
                    {
                        var.Visible = false;
                    }
                    else
                    {
                        var.Visible = true;
                    }
                }
                else
                {
                    var.Visible = false;
                }
            }
            */
        }


    }
}
