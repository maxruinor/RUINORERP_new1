using System;
using System.Collections.Generic;
using System.Windows.Forms.Design;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Collections;
using WinLib.PushPanelEx.Win32.Const;

namespace WinLib.PushPanelEx.Design
{
    /* 作者：Starts_2000
     * 日期：2010-08-10
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */
    #region PushPanelItemDesigner

    internal class PushPanelItemDesigner : PanelDesigner
    {
        public PushPanelItemDesigner()
            : base()
        {
            base.AutoResizeHandles = true;
        }

        public override SelectionRules SelectionRules
        {
            get
            {
                return SelectionRules.Locked;
            }
        }

        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == WM.WM_LBUTTONDOWN)
            {
                Point point = Control.PointToClient(Cursor.Position);
                PushPanelItem item = Control as PushPanelItem;

                if (item != null)
                {
                    if (item.CaptionRect.Contains(point))
                    {
                        if (item.Owner != null)
                        {
                            item.Owner.OnItemCaptionClick(
                                new PushPanelItemCaptionClickEventArgs(item));
                        }

                        ISelectionService selectionService =
                            (ISelectionService)GetService(typeof(ISelectionService));
                        ArrayList selection = new ArrayList();
                        selection.Add(item);
                        selectionService.SetSelectedComponents(
                            selection,
                            SelectionTypes.Remove | SelectionTypes.Add);
                    }
                }
            }
            base.WndProc(ref msg);
        }

        protected override void PostFilterProperties(IDictionary properties)
        {
            base.PreFilterAttributes(properties);
            properties.Remove("BorderStyle");
            properties.Remove("Locked");
            properties.Remove("AccessibilityObject");
            properties.Remove("AccessibleDefaultActionDescription");
            properties.Remove("AccessibleDescription");
            properties.Remove("AccessibleName");
            properties.Remove("AccessibleRole");
            properties.Remove("Anchor");
            properties.Remove("AntiAliasing");
            properties.Remove("AutoScroll");
            properties.Remove("AutoScrollMargin");
            properties.Remove("AutoScrollMinSize");
            properties.Remove("BackgroundImage");
            properties.Remove("BackgroundImageLayout");
            properties.Remove("CausesValidation");
            properties.Remove("Dock");
            properties.Remove("GenerateMember");
            properties.Remove("ImeMode");
            properties.Remove("MaximumSize");
            properties.Remove("MinimumSize");
            properties.Remove("BackGroundImage");
            properties.Remove("BackGroundImageLayout");
            properties.Remove("Margin");
            properties.Remove("TabIndex");
            properties.Remove("TabStop");
            properties.Remove("Visible");
            properties.Remove("Enabled");
            properties.Remove("AutoSize");
            properties.Remove("AutoSizeMode");
        }
    }

    #endregion
}
