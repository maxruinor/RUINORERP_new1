using FastReport.Editor.Dialogs;
using Krypton.Toolkit;
using Microsoft.Extensions.Logging;
using Netron.GraphLib;
using RUINORERP.Business;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Common;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.ClientCmdService;
using RUINORERP.UI.Common;
using RUINORERP.UI.UControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.UserPersonalized
{
    [MenuAttrAssemblyInfo("表格列设置编辑", true, UIType.单表数据)]
    public partial class UCGridColSetting : BaseEditGeneric<ColumnDisplayController>
    {
        public UCGridColSetting()
        {
            InitializeComponent();
        }

        public NewSumDataGridView dataGridView { get; set; }
        public ColumnDisplayController EditEntity { get; set; }

        public void BindData(ColumnDisplayController entity)
        {
            EditEntity = entity;
            DataBindingHelper.BindData4TextBox<ColumnDisplayController>(entity, t => t.ColDisplayText, txtColCaption, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<ColumnDisplayController>(entity, t => t.Visible, chkVisble, false);
            DataBindingHelper.BindData4CheckBox<ColumnDisplayController>(entity, t => t.IsFixed, chkisFixed, false);
            DataBindingHelper.BindData4TrackBar<ColumnDisplayController>(entity, nameof(entity.ColWidth), trackBarColWidth, false);
            DataBindingHelper.BindData4NumericUpDown<ColumnDisplayController>(entity, nameof(entity.ColDisplayIndex), txtSort, false);
            lblColWidthShow.Text = entity.ColWidth.ToString();
            entity.PropertyChanged += (sender, s2) =>
            {
                if (s2.PropertyName == nameof(entity.Visible))
                {
                    if (OnSynchronizeUI != null) OnSynchronizeUI(sender, nameof(entity.Visible));
                }
                if (s2.PropertyName == nameof(entity.ColDisplayIndex))
                {
                    if (OnSynchronizeUI != null) OnSynchronizeUI(sender, nameof(entity.ColDisplayIndex));
                }
                if (s2.PropertyName == nameof(entity.IsFixed))
                {
                    if (OnSynchronizeUI != null) OnSynchronizeUI(sender, nameof(entity.IsFixed));
                }

                if (s2.PropertyName == nameof(entity.ColWidth))
                {
                    if (OnSynchronizeUI != null) OnSynchronizeUI(sender, nameof(entity.ColWidth));
                }
            };
        }

        public event SynchronizeUIEventHandler OnSynchronizeUI;
        public delegate void SynchronizeUIEventHandler(object sender, object e);

        private void UCQueryCondition_Leave(object sender, EventArgs e)
        {
            //操作前将数据收集
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);

        }

        private void trackBarColWidth_Scroll(object sender, EventArgs e)
        {
            lblColWidthShow.Text = trackBarColWidth.Value.ToString();
            if (dataGridView == null) return;
            try
            {
                if (dataGridView.Columns.Contains(EditEntity.ColName))
                {
                    dataGridView.Columns[EditEntity.ColName].Width = trackBarColWidth.Value;
                }
            }
            catch (Exception)
            {

                 
            }
          
        }
    }
}
