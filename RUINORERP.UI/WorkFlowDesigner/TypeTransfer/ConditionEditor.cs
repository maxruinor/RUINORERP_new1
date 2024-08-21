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
    public class ConditionEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            // 创建一个新的 WFCondition 对象
            WFCondition condition = new WFCondition();

            // 使用对话框或其他方式让用户编辑条件
            using (ConditionEditorDialog dialog = new ConditionEditorDialog(condition))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // 将用户编辑后的条件返回
                    return dialog.Condition;
                }
            }

            // 如果用户取消编辑，返回原始值
            return value;
        }
    }
}
