using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using RUINORERP.UI.WorkFlowDesigner.Entities;

namespace RUINORERP.UI.WorkFlowDesigner.UI
{
    /// <summary>
    /// 审批用户列表编辑器
    /// 用于在属性面板中可视化编辑审批用户列表
    /// </summary>
    public class ApprovalUserListEditor : UITypeEditor
    {
        /// <summary>
        /// 获取编辑器的编辑样式
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            // 指定使用模态对话框进行编辑
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// 编辑值
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context == null || provider == null)
            {
                return value;
            }

            // 获取Windows Forms设计器服务
            IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (editorService == null)
            {
                return value;
            }

            // 获取当前的审批用户列表
            List<ApprovalUser> users = value as List<ApprovalUser>;
            if (users == null)
            {
                users = new List<ApprovalUser>();
            }

            // 显示编辑对话框
            using (ApprovalUserListEditorForm editorForm = new ApprovalUserListEditorForm(users))
            {
                if (editorService.ShowDialog(editorForm) == DialogResult.OK)
                {
                    // 更新审批用户列表
                    return editorForm.ApprovalUsers;
                }
            }

            return value;
        }
    }
}