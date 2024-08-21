using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms.Design;

namespace RUINOR.WinFormsUI.RegTextBox
{
    public class RegDropDownEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            // 编辑属性值时，在右侧显示...更多按钮  
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            var edSvc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (edSvc != null)
            {
                RegEditorSelect select = new RegEditorSelect();
                // 还有ShowDialog这种方式，可以弹出一个窗体来进行编辑  
                // edSvc.DropDownControl(select);
                //如果值不为空，则给之前设置的值为默认值
                if (value is RegularAuthenticationSettings && value != null)
                {
                    select.regSelect = value as RegularAuthenticationSettings;
                }
                edSvc.ShowDialog(select);
                value = select.regSelect;
            }
            return base.EditValue(context, provider, value);
        }
    }
}
