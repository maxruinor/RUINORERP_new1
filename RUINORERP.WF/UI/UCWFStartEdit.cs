using RUINORERP.Common.Helper;
using RUINORERP.Model;
using RUINORERP.WF.BizOperation;
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
using WorkflowCore.Interface;
using WorkflowCore.Services;

namespace RUINORERP.WF.UI
{
    public partial class UCWFStartEdit : UCNodePropEditBase
    {
        public UCWFStartEdit()
        {
            InitializeComponent();
            NodePropName = "流程配置";
        }


        WorkFlowContextData wFStart = new WorkFlowContextData();
        List<CmbItem> dropItemList = new List<CmbItem>();
        private void UCWFStartEdit_Load(object sender, EventArgs e)
        {

            //根据流程参数加载类型
            LoadWorkflowParaDataType(cmbWorkflowType);
            if (SetNodeValue != null && SetNodeValue is WorkFlowContextData)
            {
                wFStart = (WorkFlowContextData)SetNodeValue;
                SetNodeValue = wFStart;
                DataBindHelper.BindData4TextBox<WorkFlowContextData>(wFStart, t => t.Id, txtID, BindDataType.Text, false);
                DataBindHelper.BindData4TextBox<WorkFlowContextData>(wFStart, t => t.Version, txtVer, BindDataType.Text, false);
                DataBindHelper.BindData4TextBox<WorkFlowContextData>(wFStart, t => t.Name, txtName, BindDataType.Text, false);
                DataBindHelper.BindData4TextBox<WorkFlowContextData>(wFStart, t => t.Description, txtDesc, BindDataType.Text, false);

                cmbWorkflowType.SelectedItem = dropItemList.FirstOrDefault(c => c.Key == wFStart.DataType);

            }



        }




        private void LoadWorkflowParaDataType(ComboBox cmb)
        {
            var assembly = System.Reflection.Assembly.LoadFrom("RUINORERP.WF.dll");

            /* 也可以这样找
            // 要搜索的接口类型
            Type interfaceType = typeof(IWorkflowDataMarker);

            // 查找实现了指定接口的所有类
            var implementingTypes = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Contains(interfaceType) && t.IsClass);

            // 打印出找到的类名
            foreach (var type in implementingTypes)
            {
                Console.WriteLine(type.FullName);
            }
            */

            cmb.Items.Clear();
            cmb.Items.Add("请选择");
            var workflowTypes = assembly.GetTypes()
                .Where(t => typeof(IWorkflowDataMarker).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
            foreach (var workflowType in workflowTypes)
            {
                try
                {
                    if (typeof(IWorkflowDataMarker).IsAssignableFrom(workflowType))
                    {
                        var workflowData = (IWorkflowDataMarker)Activator.CreateInstance(workflowType);
                        CmbItem cmbItem = new CmbItem(workflowData.DataType, workflowData.TypeName);
                        cmb.Items.Add(cmbItem);
                        dropItemList.Add(cmbItem);
                    }

                }
                catch (Exception ex)
                {
                    // 处理异常，例如记录日志
                    Console.WriteLine($"Failed to register workflow of type {workflowType.FullName}: {ex.Message}");
                }
            }
        }

        private void cmbWorkflowType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbWorkflowType.SelectedItem != null && cmbWorkflowType.SelectedItem.ToString() != "请选择")
            {
                wFStart.DataType = (cmbWorkflowType.SelectedItem as CmbItem).Key;
            }
            //wFStart.DataType = cmbWorkflowType.SelectedItem.ToString();
        }
    }
}
