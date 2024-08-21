using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace RUINORERP.UI.WorkFlowDesigner.TypeTransfer
{
    public class ConditionTypeEditor : UITypeEditor
    {
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            // UITypeEditorEditStyle有三种，Modal是弹出式，DropDown是下拉式，None是没有。
            return UITypeEditorEditStyle.Modal;
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            // 得到editor service，可由其创建弹出窗口。
            IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            // context.Instance —— 可以得到当前的Demo3对象。
            // ((Demo3)context.Instance).Grade —— 可以得到当前Grade的值。

            ConditionEditor1 dialog = new ConditionEditor1();
            editorService.ShowDialog(dialog);
            String grade = dialog.Condition;
            dialog.Dispose();

            return grade;
        }
    }
}
