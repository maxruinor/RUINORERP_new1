using RUINORERP.UI.WorkFlowDesigner.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.WorkFlowDesigner.TypeTransfer
{
    public class ListConditionEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            // 创建一个新的对话框或界面来编辑列表条件
            using (ListConditionEditorDialog dialog = new ListConditionEditorDialog((List<WFCondition>)value))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.Conditions;
                }
            }

            return value;
        }
    }
}
