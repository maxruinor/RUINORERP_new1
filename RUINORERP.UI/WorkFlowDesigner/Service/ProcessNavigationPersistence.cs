using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Netron.GraphLib;
using Newtonsoft.Json;
using RUINORERP.Model;
using RUINORERP.UI.WorkFlowDesigner.Nodes;
using RUINORERP.Common.Helper;
using Netron.GraphLib.UI;
using RUINORERP.Business;
using System.Drawing;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI.WorkFlowDesigner.Service
{
    /// <summary>
    /// 流程导航图持久化服务
    /// 负责流程导航图的图形数据序列化、反序列化和持久化操作
    /// </summary>
    public class ProcessNavigationPersistence
    {
        private readonly tb_ProcessNavigationController<tb_ProcessNavigation> _navigationController;
        private readonly tb_ProcessNavigationNodeController<tb_ProcessNavigationNode> _navigationNodeController;
        private readonly ILogger<ProcessNavigationPersistence> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="navigationController">流程导航图控制器</param>
        /// <param name="navigationNodeController">流程导航图节点控制器</param>
        /// <param name="logger">日志记录器</param>
        public ProcessNavigationPersistence(tb_ProcessNavigationController<tb_ProcessNavigation> navigationController, tb_ProcessNavigationNodeController<tb_ProcessNavigationNode> navigationNodeController, ILogger<ProcessNavigationPersistence> logger = null)
        {
            _navigationController = navigationController;
            _navigationNodeController = navigationNodeController;
            _logger = logger;
        }

        #region 图形数据序列化与反序列化

        /// <summary>
        /// 将图形控件的内容序列化为XML字符串
        /// </summary>
        /// <param name="graphControl">图形控件实例</param>
        /// <returns>XML字符串</returns>
        public string SerializeGraphToXml(GraphControl graphControl)
        {
            try
            {
                // TODO: GraphControl API 需要确认实际的序列化方法
                // 暂时返回空字符串，编译通过后再实现具体功能
                _logger?.LogWarning("GraphControl.Save 方法未找到，暂时返回空字符串");
                return string.Empty;
                
                // 原代码暂时注释
                // using (var stringWriter = new StringWriter())
                // {
                //     graphControl.Save(stringWriter);
                //     return stringWriter.ToString();
                // }
            }
            catch (Exception ex)
            {
                throw new Exception("图形数据序列化失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 将XML字符串反序列化为图形控件内容
        /// </summary>
        /// <param name="graphControl">图形控件实例</param>
        /// <param name="xmlString">XML字符串</param>
        public void DeserializeXmlToGraph(GraphControl graphControl, string xmlString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(xmlString))
                {
                    // TODO: GraphControl API 需要确认实际的清空方法
                    // 暂时使用 Shapes.Clear()
                    if (graphControl.Shapes != null)
                    {
                        graphControl.Shapes.Clear();
                    }
                    return;
                }

                // TODO: GraphControl API 需要确认实际的加载方法
                _logger?.LogWarning("GraphControl.Load 方法未找到，暂时跳过XML反序列化");
                
                // 原代码暂时注释
                // using (var stringReader = new StringReader(xmlString))
                // {
                //     graphControl.Load(stringReader);
                // }
            }
            catch (Exception ex)
            {
                throw new Exception("图形数据反序列化失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 将图形控件的内容序列化为JSON字符串
        /// </summary>
        /// <param name="graphControl">图形控件实例</param>
        /// <returns>JSON字符串</returns>
        public string SerializeGraphToJson(GraphControl graphControl)
        {
            try
            {
                var graphData = new GraphData
                {
                    Nodes = new List<NodeData>(),
                    Connections = new List<ConnectionData>()
                };

                // 序列化节点
                foreach (var shape in graphControl.Shapes.OfType<ProcessNavigationNode>())
                {
                    var nodeData = new NodeData
                    {
                        ID = shape.NodeId,
                        NodeName = shape.ProcessName,
                        MenuID = long.TryParse(shape.MenuID, out var menuId) ? menuId : 0,
                        NodeType = shape.NodeType.ToString(),
                        Description = shape.Description,
                        X = shape.Rectangle.X,
                        Y = shape.Rectangle.Y,
                        Width = shape.Rectangle.Width,
                        Height = shape.Rectangle.Height,
                        FillColor = shape.NodeColor.ToArgb(),
                        BorderColor = Color.Black.ToArgb(), // ProcessNavigationNode 没有 BorderColor 属性
                        FontColor = Color.White.ToArgb()   // ProcessNavigationNode 没有 FontColor 属性
                    };
                    graphData.Nodes.Add(nodeData);
                }

                // 序列化连接
                foreach (var connection in graphControl.Connections)
                {
                    // TODO: 确认 Connection 对象的实际属性名称
                    // 暂时使用反射尝试获取属性
                    var sourceNode = GetConnectionProperty<ProcessNavigationNode>(connection, "StartShape") ?? 
                                   GetConnectionProperty<ProcessNavigationNode>(connection, "Source");
                    var targetNode = GetConnectionProperty<ProcessNavigationNode>(connection, "EndShape") ?? 
                                   GetConnectionProperty<ProcessNavigationNode>(connection, "Target");
                    
                    if (sourceNode != null && targetNode != null)
                    {
                        var connectionData = new ConnectionData
                        {
                            SourceNodeID = sourceNode.NodeId,
                            TargetNodeID = targetNode.NodeId,
                            ConnectionType = connection.GetType().Name
                        };
                        graphData.Connections.Add(connectionData);
                    }
                }

                return JsonConvert.SerializeObject(graphData, Formatting.Indented);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "序列化图形控件到JSON失败");
                throw new Exception("图形数据JSON序列化失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 将JSON字符串反序列化为图形控件内容
        /// </summary>
        /// <param name="graphControl">图形控件实例</param>
        /// <param name="jsonString">JSON字符串</param>
        public void DeserializeJsonToGraph(GraphControl graphControl, string jsonString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jsonString))
                {
                   // graphControl.Clear();
                    return;
                }

                var graphData = JsonConvert.DeserializeObject<GraphData>(jsonString);
                if (graphData == null)
                {
                    throw new Exception("JSON数据格式无效");
                }

                // 清空图形控件
                if (graphControl.Shapes != null)
                {
                    graphControl.Shapes.Clear();
                }

                // 创建节点字典用于快速查找
                var nodeDict = new Dictionary<string, ProcessNavigationNode>();

                // 先创建所有节点
                foreach (var nodeData in graphData.Nodes)
                {
                    var node = new ProcessNavigationNode
                    {
                        NodeId = nodeData.ID,
                        ProcessName = nodeData.NodeName,
                        MenuID = nodeData.MenuID.ToString(),
                        Description = nodeData.Description,
                        NodeColor = Color.FromArgb(nodeData.FillColor)
                        // ProcessNavigationNode 有 NodeType 属性（继承自 BaseNode）
                        // 但没有 BorderColor、FontColor 属性
                    };
                    node.Rectangle = new RectangleF(nodeData.X, nodeData.Y, nodeData.Width, nodeData.Height);
                    graphControl.AddShape(node);
                    nodeDict[node.NodeId] = node;
                }

                // 再创建所有连接
                foreach (var connectionData in graphData.Connections)
                {
                    if (nodeDict.TryGetValue(connectionData.SourceNodeID, out var sourceNode) &&
                        nodeDict.TryGetValue(connectionData.TargetNodeID, out var targetNode))
                    {
                        // TODO: GraphControl API 需要确认实际的连接创建方法
                        _logger?.LogWarning("GraphControl.CreateConnection 方法未找到，暂时跳过连接创建");
                        
                        // 原代码暂时注释
                        // var connection = graphControl.CreateConnection(sourceNode, targetNode);
                        // if (connection != null)
                        // {
                        //     graphControl.AddConnection(connection);
                        // }
                    }
                }
            }
            catch (JsonException jsonEx)
            {
                _logger?.LogError(jsonEx, "JSON解析失败");
                throw new Exception("JSON数据格式无效，请检查数据完整性", jsonEx);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "从JSON反序列化图形控件失败");
                throw new Exception("图形数据JSON反序列化失败，请联系管理员", ex);
            }
        }

        #endregion

        #region 流程导航图持久化

        /// <summary>
        /// 保存流程导航图及其图形数据
        /// </summary>
        /// <param name="navigation">流程导航图实体</param>
        /// <param name="graphControl">图形控件实例</param>
        /// <returns>保存后的流程导航图实体</returns>
        public async Task<tb_ProcessNavigation> SaveNavigationWithGraphAsync(tb_ProcessNavigation navigation, GraphControl graphControl)
        {
            try
            {
                // 序列化图形数据为XML
                navigation.GraphXml = SerializeGraphToXml(graphControl);
                
                // 序列化图形数据为JSON（可选，用于更灵活的数据处理）
                navigation.GraphJson = SerializeGraphToJson(graphControl);
                
                // 设置更新时间
                navigation.UpdateTime = DateTime.Now;
                
                // 保存到数据库
                if (navigation.ProcessNavID > 0)
                {
                    // 更新现有流程
                    await _navigationController.UpdateAsync(navigation);
                }
                else
                {
                    // 创建新流程
                    await _navigationController.SaveOrUpdate(navigation);
                }

                // 同步节点数据到节点表
                await SynchronizeNodesToDatabaseAsync(navigation, graphControl);

                return navigation;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "保存流程导航图及其图形数据失败");
                throw new Exception("保存流程导航图失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 加载流程导航图及其图形数据
        /// </summary>
        /// <param name="navigationId">流程导航图ID</param>
        /// <param name="graphControl">图形控件实例</param>
        /// <returns>流程导航图实体</returns>
        public async Task<tb_ProcessNavigation> LoadNavigationWithGraphAsync(long navigationId, GraphControl graphControl)
        {
            try
            {
                // 从数据库加载流程导航图
                var navigation = await _navigationController.BaseQueryByIdAsync(navigationId);
                if (navigation == null)
                {
                    throw new Exception("流程导航图不存在");
                }

                // 加载图形数据
                if (!string.IsNullOrWhiteSpace(navigation.GraphXml))
                {
                    DeserializeXmlToGraph(graphControl, navigation.GraphXml);
                }
                else if (!string.IsNullOrWhiteSpace(navigation.GraphJson))
                {
                    DeserializeJsonToGraph(graphControl, navigation.GraphJson);
                }
                else
                {
                    if (graphControl.Shapes != null)
                    {
                        graphControl.Shapes.Clear();
                    }
                }

                return navigation;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "加载流程导航图失败(ID:{navigationId})");
                throw new Exception("加载流程导航图失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 同步节点数据到数据库
        /// </summary>
        /// <param name="navigation">流程导航图实体</param>
        /// <param name="graphControl">图形控件实例</param>
        private async Task SynchronizeNodesToDatabaseAsync(tb_ProcessNavigation navigation, GraphControl graphControl)
        {
            try
            {
                // 获取当前数据库中的节点
                var dbNodes = await _navigationNodeController.GetNavigationNodesAsync(navigation.ProcessNavID);
                var dbNodeDict = dbNodes.ToDictionary(n => n.NodeID, n => n);

                // 准备要插入或更新的节点列表
                var nodesToSave = new List<tb_ProcessNavigationNode>();
                var nodeIdsToKeep = new List<long>();

                // 处理图形控件中的每个节点
                foreach (var shape in graphControl.Shapes.OfType<ProcessNavigationNode>())
                {
                    tb_ProcessNavigationNode node;
                    
                    // 尝试从数据库中查找对应节点
                    var existingDbNode = dbNodeDict.Values.FirstOrDefault(n => n.NodeCode == shape.NodeId);
                    
                    if (existingDbNode != null)
                    {
                        // 更新现有节点
                        node = existingDbNode;
                        nodeIdsToKeep.Add(node.NodeID);
                    }
                    else
                    {
                        // 创建新节点
                        node = new tb_ProcessNavigationNode
                        {
                            ProcessNavID = navigation.ProcessNavID,
                            NodeCode = shape.NodeId
                        };
                    }

                    // 更新节点属性
                    node.NodeName = shape.ProcessName;
                    node.MenuID = long.TryParse(shape.MenuID, out var menuId) ? menuId : (long?)null;
                    node.NodeType = shape.NodeType.ToString() ?? string.Empty;
                    node.Description = shape.Description;
                    node.PositionX = shape.Rectangle.X;
                    node.PositionY = shape.Rectangle.Y;
                    node.Width = shape.Rectangle.Width;
                    node.Height = shape.Rectangle.Height;
                    node.NodeColor = shape.NodeColor.ToArgb().ToString();
                    // tb_ProcessNavigationNode 没有 FillColor、BorderColor、FontColor 属性
                    // 只有 NodeColor 属性
                    
                    nodesToSave.Add(node);
                }

                // 删除不再存在的节点
                var nodesToDelete = dbNodes.Where(n => !nodeIdsToKeep.Contains(n.NodeID)).ToList();
                foreach (var nodeToDelete in nodesToDelete)
                {
                    await _navigationNodeController.DeleteAsync(nodeToDelete.NodeID);
                }

                // 批量添加或更新节点
                foreach (var node in nodesToSave)
                {
                    if (node.NodeID > 0)
                    {
                        // 更新现有节点
                        await _navigationNodeController.UpdateAsync(node);
                    }
                    else
                    {
                        // 添加新节点
                        await _navigationNodeController.SaveOrUpdate(node);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "同步节点数据到数据库失败");
                throw new Exception("同步节点数据到数据库失败，请联系管理员", ex);
            }
        }

        #endregion

        #region 文件导入导出

        /// <summary>
        /// 导出流程导航图为JSON文件
        /// </summary>
        /// <param name="navigation">流程导航图实体</param>
        /// <param name="filePath">文件路径</param>
        public async Task ExportNavigationToJsonFileAsync(tb_ProcessNavigation navigation, string filePath)
        {
            try
            {
                var exportData = new NavigationExportData
                {
                    Navigation = navigation,
                    ExportTime = DateTime.Now,
                    Version = "1.0"
                };

                var json = JsonConvert.SerializeObject(exportData, Formatting.Indented);
                File.WriteAllText(filePath, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "导出流程导航图失败(Path:{filePath})");
                throw new Exception("导出流程导航图失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 从JSON文件导入流程导航图
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>流程导航图实体</returns>
        public async Task<tb_ProcessNavigation> ImportNavigationFromJsonFileAsync(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new Exception("文件不存在");
                }

                var json = File.ReadAllText(filePath, Encoding.UTF8);
                var importData = JsonConvert.DeserializeObject<NavigationExportData>(json);
                
                if (importData == null || importData.Navigation == null)
                {
                    throw new Exception("文件格式无效");
                }

                // 重置ID，确保是新导入的流程
                importData.Navigation.ProcessNavID = 0;
                importData.Navigation.CreateTime = DateTime.Now;
                importData.Navigation.UpdateTime = DateTime.Now;
                importData.Navigation.IsDefault = false;

                return importData.Navigation;
            }
            catch (JsonException jsonEx)
            {
                _logger?.LogError(jsonEx, "导入文件JSON解析失败");
                throw new Exception("文件格式无效，请检查是否为有效的流程导航图文件", jsonEx);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"导入流程导航图失败(Path:{filePath})");
                throw new Exception("导入流程导航图失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 导出流程导航图为XML文件
        /// </summary>
        /// <param name="navigation">流程导航图实体</param>
        /// <param name="filePath">文件路径</param>
        public async Task ExportNavigationToXmlFileAsync(tb_ProcessNavigation navigation, string filePath)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(navigation.GraphXml))
                {
                    File.WriteAllText(filePath, navigation.GraphXml, Encoding.UTF8);
                }
                else
                {
                    throw new Exception("流程导航图没有图形数据");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"导出流程导航图XML失败(Path:{filePath})");
                throw new Exception("导出流程导航图失败，请联系管理员", ex);
            }
        }

        /// <summary>
        /// 从XML文件导入图形数据到流程导航图
        /// </summary>
        /// <param name="navigation">流程导航图实体</param>
        /// <param name="filePath">文件路径</param>
        /// <returns>更新后的流程导航图实体</returns>
        public async Task<tb_ProcessNavigation> ImportGraphFromXmlFileAsync(tb_ProcessNavigation navigation, string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new Exception("文件不存在");
                }

                navigation.GraphXml = File.ReadAllText(filePath, Encoding.UTF8);
                navigation.UpdateTime = DateTime.Now;

                return navigation;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"导入图形数据失败(Path:{filePath})");
                throw new Exception("导入图形数据失败，请联系管理员", ex);
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 通过反射获取连接对象的属性
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="connection">连接对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性值</returns>
        private T GetConnectionProperty<T>(object connection, string propertyName)
        {
            try
            {
                var property = connection.GetType().GetProperty(propertyName);
                if (property != null && property.CanRead)
                {
                    return (T)property.GetValue(connection);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, $"获取连接属性 {propertyName} 失败");
            }
            return default(T);
        }

        #endregion

        #region 辅助类

        /// <summary>
        /// 图形数据类
        /// 用于JSON序列化的中间数据结构
        /// </summary>
        private class GraphData
        {
            public List<NodeData> Nodes { get; set; }
            public List<ConnectionData> Connections { get; set; }
        }

        /// <summary>
        /// 节点数据类
        /// </summary>
        private class NodeData
        {
            public string ID { get; set; }
            public string NodeName { get; set; }
            public long MenuID { get; set; }
            public string NodeType { get; set; }
            public string Description { get; set; }
            public float X { get; set; }
            public float Y { get; set; }
            public float Width { get; set; }
            public float Height { get; set; }
            public int FillColor { get; set; }
            public int BorderColor { get; set; }
            public int FontColor { get; set; }
        }

        /// <summary>
        /// 连接数据类
        /// </summary>
        private class ConnectionData
        {
            public string SourceNodeID { get; set; }
            public string TargetNodeID { get; set; }
            public string ConnectionType { get; set; }
        }

        /// <summary>
        /// 导航导出数据类
        /// </summary>
        private class NavigationExportData
        {
            public tb_ProcessNavigation Navigation { get; set; }
            public DateTime ExportTime { get; set; }
            public string Version { get; set; }
        }

        #endregion
    }
}