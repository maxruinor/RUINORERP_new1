using FastReport.DevComponents.DotNetBar;
using Krypton.Docking;
using Krypton.Navigator;
using Krypton.Toolkit;
using RUINORERP.UI.BaseForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.Common
{
    public class TabCloseHandler
    {

        /// <summary>
        /// 关闭的一个方法
        /// 关闭事件 由tab左上的小x决定的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bs_Click(object sender, EventArgs e)
        {
            if (sender is ButtonSpecAny btn && btn.Owner is KryptonPage kpage)
            {
                if (kpage.Controls.Count == 1)
                {
                    var control = kpage.Controls[0];
                    ProcessControlOnClose(control);
                    MainForm.Instance.kryptonDockingManager1.RemovePage(kpage.UniqueName, true);
                    kpage.Dispose();
                }
            }
        }

 

        /// <summary>
        /// 处理关闭时的控件操作
        /// </summary>
        /// <param name="control">要处理的控件</param>
        public void ProcessControlOnClose(Control control)
        {
            if (control == null) return;

            Type controlType = control.GetType();
            if (IsBaseListGeneric(controlType))
            {
                SaveBaseListGenericGridSettings(control);
            }
            else if (IsBaseBillEditGeneric(controlType))
            {
                 
                var baseBillEdit = (BaseBillEdit)control;
                baseBillEdit.UNLock(false);
       
            }
            else if (IsBaseBillQueryMC(controlType))
            {
                SaveBaseBillQueryMCGridSettings(control);
            }
            else if (IsBaseNavigatorGeneric(controlType))
            {
                SaveBaseNavigatorGenericGridSettings(control);
            }
        }

        /// <summary>
        /// 检查是否为BaseListGeneric类型
        /// </summary>
        private bool IsBaseListGeneric(Type type)
        {
            return type != null && type.BaseType?.Name == "BaseListGeneric`1";
        }

        /// <summary>
        /// 检查是否为BaseBillEditGeneric类型
        /// </summary>
        private bool IsBaseBillEditGeneric(Type type)
        {
            return type != null &&
                   (type.BaseType?.Name == "BaseBillEditGeneric`2" ||
                    type.BaseType?.BaseType?.Name == "BaseBillEditGeneric`2");
        }

        /// <summary>
        /// 检查是否为BaseBillQueryMC类型
        /// </summary>
        private bool IsBaseBillQueryMC(Type type)
        {
            return type != null &&
                   (type.BaseType.Name.Contains("BaseBillQueryMC") ||
                    type.BaseType.BaseType.Name.Contains("BaseBillQueryMC"));
        }

        /// <summary>
        /// 检查是否为BaseNavigatorGeneric类型
        /// </summary>
        private bool IsBaseNavigatorGeneric(Type type)
        {
            return type != null && type.BaseType.Name.Contains("BaseNavigatorGeneric");
        }

        /// <summary>
        /// 保存BaseListGeneric类型的网格设置
        /// </summary>
        private void SaveBaseListGenericGridSettings(Control control)
        {
            Type[] genericArguments = control.GetType().BaseType.GetGenericArguments();
            if (genericArguments.Length > 0)
            {
                Type genericParameterType = genericArguments[0];
                var baseUControl = (BaseUControl)control;
                UIBizService.SaveGridSettingData(baseUControl.CurMenuInfo, baseUControl.BaseDataGridView1, genericParameterType);
            }
        }

        /// <summary>
        /// 保存BaseBillQueryMC类型的网格设置
        /// </summary>
        private void SaveBaseBillQueryMCGridSettings(Control control)
        {
            Type[] genericArguments = control.GetType().BaseType.GetGenericArguments();
            if (genericArguments.Length > 0)
            {
                Type genericParameterType = genericArguments[0];
                var baseUControl = (BaseQuery)control;
                UIBizService.SaveGridSettingData(baseUControl.CurMenuInfo, baseUControl.BaseMainDataGridView, genericParameterType);

                if (genericArguments.Length == 2 && !genericArguments[0].Name.Equals(genericArguments[1].Name))
                {
                    UIBizService.SaveGridSettingData(baseUControl.CurMenuInfo, baseUControl.BaseSubDataGridView, genericArguments[1]);
                }
            }
            else
            {
                genericArguments = control.GetType().BaseType.BaseType.GetGenericArguments();
                if (genericArguments.Length > 0)
                {
                    Type genericParameterType = genericArguments[0];
                    var baseUControl = (BaseQuery)control;
                    UIBizService.SaveGridSettingData(baseUControl.CurMenuInfo, baseUControl.BaseMainDataGridView, genericParameterType);

                    if (genericArguments.Length == 2 && !genericArguments[0].Name.Equals(genericArguments[1].Name))
                    {
                        UIBizService.SaveGridSettingData(baseUControl.CurMenuInfo, baseUControl.BaseSubDataGridView, genericArguments[1]);
                    }
                }
            }
        }

        /// <summary>
        /// 保存BaseNavigatorGeneric类型的网格设置
        /// </summary>
        private void SaveBaseNavigatorGenericGridSettings(Control control)
        {
            Type[] genericArguments = control.GetType().BaseType.GetGenericArguments();
            if (genericArguments.Length > 0)
            {
                Type genericParameterType = genericArguments[0];
                var baseUControl = (BaseNavigator)control;
                UIBizService.SaveGridSettingData(baseUControl.CurMenuInfo, baseUControl.BaseMainDataGridView, genericParameterType);
            }
        }
    }
}
