using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Netron.GraphLib;
using RUINORERP.UI.WorkFlowDesigner.Nodes;
using RUINORERP.UI.WorkFlowDesigner.Entities;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace RUINORERP.UI.WorkFlowDesigner.Service
{
    /// <summary>
    /// 工作流定义转换器
    /// 将图形化设计的流程转换为WorkflowCore可识别的XML/JSON格式
    /// </summary>
    public class WorkflowDefinitionConverter
    {
        private GraphAbstract _graph;

        public WorkflowDefinitionConverter(GraphAbstract graph)
        {
            _graph = graph;
        }

        /// <summary>
        /// 将图形化流程转换为WorkflowCore的XML格式
        /// </summary>
        /// <returns></returns>
        public string ConvertToXml()
        {
            // 创建XML文档
            XmlDocument doc = new XmlDocument();
            
            // 创建根节点
            XmlElement root = doc.CreateElement("WorkflowDefinition");
            doc.AppendChild(root);
            
            // 添加ID属性
            XmlAttribute idAttr = doc.CreateAttribute("Id");
            idAttr.Value = "Workflow_" + Guid.NewGuid().ToString();
            root.Attributes.Append(idAttr);
            
            // 添加版本属性
            XmlAttribute versionAttr = doc.CreateAttribute("Version");
            versionAttr.Value = "1";
            root.Attributes.Append(versionAttr);
            
            // 创建步骤节点
            XmlElement stepsElement = doc.CreateElement("Steps");
            root.AppendChild(stepsElement);
            
            // 遍历所有节点
            foreach (Shape shape in _graph.Shapes)
            {
                BaseNode node = shape as BaseNode;
                if (node != null)
                {
                    XmlElement stepElement = CreateStepElement(doc, node);
                    stepsElement.AppendChild(stepElement);
                }
            }
            
            // 创建连接节点
            XmlElement connectionsElement = doc.CreateElement("Connections");
            root.AppendChild(connectionsElement);
            
            // 遍历所有连接
            foreach (Connection connection in _graph.Connections)
            {
                XmlElement connectionElement = CreateConnectionElement(doc, connection);
                connectionsElement.AppendChild(connectionElement);
            }
            
            // 格式化XML输出
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            settings.NewLineChars = "\r\n";
            settings.NewLineHandling = NewLineHandling.Replace;
            
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }
            
            return sb.ToString();
        }
        
        /// <summary>
        /// 创建步骤XML元素
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private XmlElement CreateStepElement(XmlDocument doc, BaseNode node)
        {
            XmlElement stepElement = doc.CreateElement("Step");
            
            // 添加ID属性
            XmlAttribute idAttr = doc.CreateAttribute("Id");
            idAttr.Value = node.UID.ToString();
            stepElement.Attributes.Append(idAttr);
            
            // 添加类型属性
            XmlAttribute typeAttr = doc.CreateAttribute("Type");
            typeAttr.Value = node.GetType().Name;
            stepElement.Attributes.Append(typeAttr);
            
            // 添加名称元素
            XmlElement nameElement = doc.CreateElement("Name");
            nameElement.InnerText = node.Text;
            stepElement.AppendChild(nameElement);
            
            // 根据节点类型添加特定属性
            if (node is CountersignNode countersignNode)
            {
                // 添加会签节点特定属性
                AddCountersignProperties(doc, stepElement, countersignNode.CountersignStep);
            }
            else if (node is OrSignNode orSignNode)
            {
                // 添加或签节点特定属性
                AddOrSignProperties(doc, stepElement, orSignNode.OrSignStep);
            }
            
            return stepElement;
        }
        
        /// <summary>
        /// 添加会签节点属性
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="stepElement"></param>
        /// <param name="countersignStep"></param>
        private void AddCountersignProperties(XmlDocument doc, XmlElement stepElement, CountersignStep countersignStep)
        {
            // 添加审批人员列表
            XmlElement approversElement = doc.CreateElement("Approvers");
            foreach (ApprovalUser user in countersignStep.Approvers)
            {
                XmlElement userElement = doc.CreateElement("Approver");
                
                XmlAttribute idAttr = doc.CreateAttribute("Id");
                idAttr.Value = user.Id.ToString();
                userElement.Attributes.Append(idAttr);
                
                XmlAttribute nameAttr = doc.CreateAttribute("Name");
                nameAttr.Value = user.Name;
                userElement.Attributes.Append(nameAttr);
                
                // TODO: 部门信息需要从实际系统中获取
                XmlAttribute deptIdAttr = doc.CreateAttribute("DepartmentId");
                deptIdAttr.Value = user.DepartmentId.ToString();
                userElement.Attributes.Append(deptIdAttr);
                
                XmlAttribute deptNameAttr = doc.CreateAttribute("DepartmentName");
                deptNameAttr.Value = user.DepartmentName;
                userElement.Attributes.Append(deptNameAttr);
                
                approversElement.AppendChild(userElement);
            }
            stepElement.AppendChild(approversElement);
            
            // 添加其他属性
            AddCommonProperties(doc, stepElement, countersignStep);
        }
        
        /// <summary>
        /// 添加或签节点属性
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="stepElement"></param>
        /// <param name="orSignStep"></param>
        private void AddOrSignProperties(XmlDocument doc, XmlElement stepElement, OrSignStep orSignStep)
        {
            // 添加审批人员列表
            XmlElement approversElement = doc.CreateElement("Approvers");
            foreach (ApprovalUser user in orSignStep.Approvers)
            {
                XmlElement userElement = doc.CreateElement("Approver");
                
                XmlAttribute idAttr = doc.CreateAttribute("Id");
                idAttr.Value = user.Id.ToString();
                userElement.Attributes.Append(idAttr);
                
                XmlAttribute nameAttr = doc.CreateAttribute("Name");
                nameAttr.Value = user.Name;
                userElement.Attributes.Append(nameAttr);
                
                // TODO: 部门信息需要从实际系统中获取
                XmlAttribute deptIdAttr = doc.CreateAttribute("DepartmentId");
                deptIdAttr.Value = user.DepartmentId.ToString();
                userElement.Attributes.Append(deptIdAttr);
                
                XmlAttribute deptNameAttr = doc.CreateAttribute("DepartmentName");
                deptNameAttr.Value = user.DepartmentName;
                userElement.Attributes.Append(deptNameAttr);
                
                approversElement.AppendChild(userElement);
            }
            stepElement.AppendChild(approversElement);
            
            // 添加其他属性
            AddCommonProperties(doc, stepElement, orSignStep);
        }
        
        /// <summary>
        /// 添加通用属性
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="stepElement"></param>
        /// <param name="step"></param>
        private void AddCommonProperties(XmlDocument doc, XmlElement stepElement, object step)
        {
            // 创建时间
            var createTimeProp = step.GetType().GetProperty("CreateTime");
            if (createTimeProp != null)
            {
                DateTime? createTime = (DateTime?)createTimeProp.GetValue(step);
                if (createTime.HasValue)
                {
                    XmlElement createTimeElement = doc.CreateElement("CreateTime");
                    createTimeElement.InnerText = createTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    stepElement.AppendChild(createTimeElement);
                }
            }
            
            // 完成时间
            var completeTimeProp = step.GetType().GetProperty("CompleteTime");
            if (completeTimeProp != null)
            {
                DateTime? completeTime = (DateTime?)completeTimeProp.GetValue(step);
                if (completeTime.HasValue)
                {
                    XmlElement completeTimeElement = doc.CreateElement("CompleteTime");
                    completeTimeElement.InnerText = completeTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    stepElement.AppendChild(completeTimeElement);
                }
            }
            
            // 状态
            var statusProp = step.GetType().GetProperty("Status");
            if (statusProp != null)
            {
                string status = (string)statusProp.GetValue(step);
                if (!string.IsNullOrEmpty(status))
                {
                    XmlElement statusElement = doc.CreateElement("Status");
                    statusElement.InnerText = status;
                    stepElement.AppendChild(statusElement);
                }
            }
        }
        
        /// <summary>
        /// 创建连接XML元素
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private XmlElement CreateConnectionElement(XmlDocument doc, Connection connection)
        {
            XmlElement connectionElement = doc.CreateElement("Connection");
            
            // 添加源节点ID
            XmlAttribute fromAttr = doc.CreateAttribute("From");
            fromAttr.Value = connection.From.BelongsTo.UID.ToString();
            connectionElement.Attributes.Append(fromAttr);
            
            // 添加目标节点ID
            XmlAttribute toAttr = doc.CreateAttribute("To");
            toAttr.Value = connection.To.BelongsTo.UID.ToString();
            connectionElement.Attributes.Append(toAttr);
            
            return connectionElement;
        }
        
        /// <summary>
        /// 将图形化流程转换为WorkflowCore的JSON格式
        /// </summary>
        /// <returns></returns>
        public string ConvertToJson()
        {
            // 创建工作流定义对象
            WorkflowDefinition workflowDefinition = new WorkflowDefinition();
            workflowDefinition.Id = "Workflow_" + Guid.NewGuid().ToString();
            workflowDefinition.Version = 1;
            
            // 转换步骤
            foreach (Shape shape in _graph.Shapes)
            {
                BaseNode node = shape as BaseNode;
                if (node != null)
                {
                    WorkflowStep step = CreateWorkflowStep(node);
                    workflowDefinition.Steps.Add(step);
                }
            }
            
            // 转换连接
            foreach (Connection connection in _graph.Connections)
            {
                WorkflowConnection workflowConnection = CreateWorkflowConnection(connection);
                workflowDefinition.Connections.Add(workflowConnection);
            }
            
            // 序列化为JSON
            string json = JsonConvert.SerializeObject(workflowDefinition, Newtonsoft.Json.Formatting.Indented);
            return json;
        }
        
        /// <summary>
        /// 创建工作流步骤对象
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private WorkflowStep CreateWorkflowStep(BaseNode node)
        {
            WorkflowStep step = new WorkflowStep();
            step.Id = node.UID.ToString();
            step.Name = node.Text;
            step.Type = node.GetType().Name;
            
            // 根据节点类型设置特定属性
            if (node is CountersignNode countersignNode)
            {
                step.Approvers = new List<ApprovalUser>(countersignNode.CountersignStep.Approvers);
                // 将会签步骤的其他属性序列化为JSON存储在StepData中
                step.StepData = JsonConvert.SerializeObject(countersignNode.CountersignStep);
            }
            else if (node is OrSignNode orSignNode)
            {
                step.Approvers = new List<ApprovalUser>(orSignNode.OrSignStep.Approvers);
                // 将或签步骤的其他属性序列化为JSON存储在StepData中
                step.StepData = JsonConvert.SerializeObject(orSignNode.OrSignStep);
            }
            
            return step;
        }
        
        /// <summary>
        /// 创建工作流连接对象
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private WorkflowConnection CreateWorkflowConnection(Connection connection)
        {
            WorkflowConnection workflowConnection = new WorkflowConnection();
            workflowConnection.FromStepId = connection.From.BelongsTo.UID.ToString();
            workflowConnection.ToStepId = connection.To.BelongsTo.UID.ToString();
            
            // 如果连接上有条件，添加条件信息
            if (connection.Tag != null)
            {
                workflowConnection.Condition = connection.Tag.ToString();
            }
            
            return workflowConnection;
        }
    }
}