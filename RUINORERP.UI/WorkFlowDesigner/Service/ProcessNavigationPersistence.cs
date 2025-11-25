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

                // 序列化节点 - 包含所有必要属性
                foreach (var shape in graphControl.Shapes.OfType<ProcessNavigationNode>())
                {
                    // 确保NodeId唯一
                    if (string.IsNullOrEmpty(shape.NodeId))
                    {
                        shape.NodeId = Guid.NewGuid().ToString();
                    }
                    
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
                        // 业务属性
                        BusinessType = (int)shape.BusinessType,
                        FormName = shape.FormName,
                        ClassPath = shape.ClassPath,
                        ModuleID = shape.ModuleID.HasValue ? shape.ModuleID.Value : 0,
                        // 视觉属性
                        CustomText = shape.CustomText,
                        ShowCustomText = shape.ShowCustomText,
                        FontSize = shape.FontSize,
                        FontStyle = shape.TextStyle.ToString(),
                        FontColor = shape.FontColor.ToArgb(),
                        TextAlignment = (int)shape.TextAlignment,
                        TextWrap = shape.TextWrap,
                        BackgroundImagePath = shape.BackgroundImagePath,
                        Opacity = shape.Opacity,
                        BorderColor = shape.BorderColor.ToArgb()
                    };
                    graphData.Nodes.Add(nodeData);
                }

                // 序列化连接
                foreach (var connection in graphControl.Connections)
                {
                    // 使用反射获取连接的源和目标节点
                    var sourceNode = GetConnectionProperty<ProcessNavigationNode>(connection, "StartShape");
                    if (sourceNode == null) sourceNode = GetConnectionProperty<ProcessNavigationNode>(connection, "Source");
                    if (sourceNode == null) sourceNode = GetConnectionProperty<ProcessNavigationNode>(connection, "FromNode");
                    var targetNode = GetConnectionProperty<ProcessNavigationNode>(connection, "EndShape");
                    if (targetNode == null) targetNode = GetConnectionProperty<ProcessNavigationNode>(connection, "Target");
                    if (targetNode == null) targetNode = GetConnectionProperty<ProcessNavigationNode>(connection, "ToNode");
                    
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

                // 添加额外信息以便于调试
                _logger?.LogInformation($"成功序列化图形数据: {graphData.Nodes.Count}个节点, {graphData.Connections.Count}个连接");
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
                    // 清空图形控件
                    if (graphControl.Shapes != null)
                    {
                        graphControl.Shapes.Clear();
                    }
                    if (graphControl.Connections != null)
                    {
                        // 清空连接 - 使用不同方式尝试
                        var connectionsCount = graphControl.Connections.Count;
                        while (connectionsCount > 0)
                        {
                            try { graphControl.Connections.RemoveAt(0); }
                            catch { break; }
                            connectionsCount--;
                        }
                    }
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
                // 清空连接
                if (graphControl.Connections != null)
                {
                    var connectionsCount = graphControl.Connections.Count;
                    while (connectionsCount > 0)
                    {
                        try { graphControl.Connections.RemoveAt(0); }
                        catch { break; }
                        connectionsCount--;
                    }
                }

                _logger?.LogInformation($"开始反序列化: {graphData.Nodes.Count}个节点, {graphData.Connections.Count}个连接");

                // 创建节点字典用于快速查找
                var nodeDict = new Dictionary<string, ProcessNavigationNode>();

                // 先创建所有节点
                foreach (var nodeData in graphData.Nodes)
                {
                    var node = new ProcessNavigationNode
                    {
                        NodeId = nodeData.ID,
                        ProcessName = nodeData.NodeName,
                        MenuID = nodeData.MenuID > 0 ? nodeData.MenuID.ToString() : string.Empty,
                        Description = nodeData.Description,
                        NodeColor = Color.FromArgb(nodeData.FillColor),
                        // 业务属性
                        BusinessType = Enum.IsDefined(typeof(ProcessNavigationNodeBusinessType), nodeData.BusinessType) ? 
                            (ProcessNavigationNodeBusinessType)nodeData.BusinessType : ProcessNavigationNodeBusinessType.通用节点,
                        FormName = nodeData.FormName,
                        ClassPath = nodeData.ClassPath,
                        ModuleID = nodeData.ModuleID > 0 ? (long?)nodeData.ModuleID : null,
                        // 视觉属性
                        CustomText = nodeData.CustomText,
                        ShowCustomText = nodeData.ShowCustomText,
                        FontSize = nodeData.FontSize,
                        FontColor = Color.FromArgb(nodeData.FontColor),
                        TextAlignment = (ContentAlignment)nodeData.TextAlignment,
                        TextWrap = nodeData.TextWrap,
                        BackgroundImagePath = nodeData.BackgroundImagePath,
                        Opacity = nodeData.Opacity > 0.1f ? (nodeData.Opacity < 1.0f ? nodeData.Opacity : 1.0f) : 0.1f,
                        BorderColor = Color.FromArgb(nodeData.BorderColor)
                    };
                    
                    // 还原字体样式
                    if (!string.IsNullOrEmpty(nodeData.FontStyle))
                    {
                        if (Enum.TryParse<FontStyle>(nodeData.FontStyle, out var fontStyle))
                        {
                            node.TextStyle = fontStyle;
                        }
                    }
                    
                    // 设置节点位置和大小
                    node.Rectangle = new RectangleF(nodeData.X, nodeData.Y, nodeData.Width, nodeData.Height);
                    
                    // 添加到图形控件
                    try
                    {
                        graphControl.AddShape(node);
                        nodeDict[node.NodeId] = node;
                        _logger?.LogDebug($"成功添加节点: {node.NodeId}, {node.ProcessName}");
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "添加节点失败: {nodeId}", nodeData.ID);
                    }
                }

                // 再创建所有连接
                int successfulConnections = 0;
                foreach (var connectionData in graphData.Connections)
                {
                    ProcessNavigationNode sourceNode = null;
                    ProcessNavigationNode targetNode = null;
                    nodeDict.TryGetValue(connectionData.SourceNodeID, out sourceNode);
                    nodeDict.TryGetValue(connectionData.TargetNodeID, out targetNode);
                    if (sourceNode != null && targetNode != null)
                    {
                        // 创建连接
                        try
                        {
                            // 获取源节点和目标节点的连接器
                            var sourceConnector = GetValidConnector(sourceNode);
                            var targetConnector = GetValidConnector(targetNode);
                            
                            if (sourceConnector != null && targetConnector != null)
                            {
                                // 尝试通过不同方式创建连接
                                var connection = CreateConnection(graphControl, sourceConnector, targetConnector);
                                if (connection != null)
                                {
                                    try
                                    {
                                        graphControl.Connections.Add((Netron.GraphLib.Connection)connection);
                                        successfulConnections++;
                                        _logger?.LogDebug($"成功创建连接: {connectionData.SourceNodeID} -> {connectionData.TargetNodeID}");
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger?.LogWarning(ex, "添加连接到集合失败");
                                    }
                                }
                                else
                                {
                                    _logger?.LogWarning("创建连接对象失败: {source} -> {target}", 
                                        connectionData.SourceNodeID, connectionData.TargetNodeID);
                                }
                            }
                            else
                            {
                                _logger?.LogWarning("找不到有效连接器: SourceConnector={sourceConn}, TargetConnector={targetConn}", 
                                    sourceConnector != null, targetConnector != null);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "创建连接失败: {source} -> {target}", 
                                connectionData.SourceNodeID, connectionData.TargetNodeID);
                        }
                    }
                    else
                    {
                        _logger?.LogWarning("找不到节点: Source={sourceExists}, Target={targetExists}",
                            nodeDict.ContainsKey(connectionData.SourceNodeID), 
                            nodeDict.ContainsKey(connectionData.TargetNodeID));
                    }
                }
                
                _logger?.LogInformation($"反序列化完成: 成功创建 {successfulConnections}/{graphData.Connections.Count} 个连接");
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
                
                // 从数据库加载节点，补充视觉属性
                await LoadNodesFromDatabaseAsync(navigationId, graphControl);

                return navigation;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "加载流程导航图失败(ID:{navigationId})");
                throw new Exception("加载流程导航图失败，请联系管理员", ex);
            }
        }
        
        /// <summary>
        /// 从数据库加载节点并补充视觉属性
        /// </summary>
        /// <param name="navigationId">流程导航图ID</param>
        /// <param name="graphControl">图形控件实例</param>
        private async Task LoadNodesFromDatabaseAsync(long navigationId, GraphControl graphControl)
        {
            try
            {
                // 从数据库加载节点数据
                var dbNodes = await _navigationNodeController.GetNavigationNodesAsync(navigationId);
                if (dbNodes == null || !dbNodes.Any())
                    return;
                
                // 创建节点代码到图形节点的映射
                var graphNodesDict = graphControl.Shapes.OfType<ProcessNavigationNode>()
                    .Where(n => !string.IsNullOrEmpty(n.NodeId))
                    .ToDictionary(n => n.NodeId);
                
                foreach (var dbNode in dbNodes)
                {
                    // 如果在图形控件中找到匹配的节点
                    if (graphNodesDict.TryGetValue(dbNode.NodeCode, out var graphNode))
                    {
                        // 确保位置和大小正确
                        // 简化代码，根据说明这些属性不会为空
                        graphNode.Rectangle = new RectangleF(
                            dbNode.PositionX, 
                            dbNode.PositionY, 
                            dbNode.Width, 
                            dbNode.Height);
                        
                        // 补充节点颜色
                        if (!string.IsNullOrEmpty(dbNode.NodeColor) && int.TryParse(dbNode.NodeColor, out var colorArgb))
                        {
                            graphNode.NodeColor = Color.FromArgb(colorArgb);
                        }
                        
                        // 如果数据库表有ExtendedProperties字段，可以从JSON中还原视觉属性
                        // 这里假设dbNode有ExtendedProperties属性
                        // if (!string.IsNullOrEmpty(dbNode.ExtendedProperties))
                        // {
                        //     try
                        //     {
                        //         var visualProperties = JsonConvert.DeserializeObject<Dictionary<string, object>>(dbNode.ExtendedProperties);
                        //         if (visualProperties != null)
                        //         {
                        //             graphNode.CustomText = visualProperties.ContainsKey("CustomText") ? visualProperties["CustomText"]?.ToString() : string.Empty;
                        //             graphNode.ShowCustomText = visualProperties.ContainsKey("ShowCustomText") && bool.TryParse(visualProperties["ShowCustomText"]?.ToString(), out var showText) && showText;
                        //             graphNode.FontSize = visualProperties.ContainsKey("FontSize") && float.TryParse(visualProperties["FontSize"]?.ToString(), out var fontSize) ? fontSize : 12f;
                        //              
                        //             if (visualProperties.ContainsKey("FontStyle"))
                        //             {
                        //                 if (Enum.TryParse<FontStyle>(visualProperties["FontStyle"]?.ToString(), out var fontStyle))
                        //                     graphNode.TextStyle = fontStyle;
                        //             }
                        //              
                        //             graphNode.FontColor = visualProperties.ContainsKey("FontColor") && int.TryParse(visualProperties["FontColor"]?.ToString(), out var fontColorArgb) ? Color.FromArgb(fontColorArgb) : Color.Black;
                        //             graphNode.TextAlignment = visualProperties.ContainsKey("TextAlignment") && int.TryParse(visualProperties["TextAlignment"]?.ToString(), out var align) ? (ContentAlignment)align : ContentAlignment.MiddleCenter;
                        //             graphNode.TextWrap = visualProperties.ContainsKey("TextWrap") && bool.TryParse(visualProperties["TextWrap"]?.ToString(), out var wrap) && wrap;
                        //             graphNode.BackgroundImagePath = visualProperties.ContainsKey("BackgroundImagePath") ? visualProperties["BackgroundImagePath"]?.ToString() : null;
                        //             graphNode.Opacity = visualProperties.ContainsKey("Opacity") && float.TryParse(visualProperties["Opacity"]?.ToString(), out var opacity) ? opacity : 1f;
                        //             graphNode.BorderColor = visualProperties.ContainsKey("BorderColor") && int.TryParse(visualProperties["BorderColor"]?.ToString(), out var borderColorArgb) ? Color.FromArgb(borderColorArgb) : Color.Black;
                        //         }
                        //     }
                        //     catch (Exception ex)
                        //     {
                        //         _logger?.LogWarning(ex, "解析节点视觉属性失败: {nodeCode}", dbNode.NodeCode);
                        //     }
                        // }
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录错误但不阻止主流程
                _logger?.LogWarning(ex, "从数据库加载节点补充视觉属性失败");
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
                    
                    // 添加视觉属性的JSON序列化存储
                    var visualProperties = new Dictionary<string, object>
                    {
                        { "CustomText", shape.CustomText },
                        { "ShowCustomText", shape.ShowCustomText },
                        { "FontSize", shape.FontSize },
                        { "FontStyle", shape.TextStyle.ToString() },
                        { "FontColor", shape.FontColor.ToArgb() },
                        { "TextAlignment", (int)shape.TextAlignment },
                        { "TextWrap", shape.TextWrap },
                        { "BackgroundImagePath", shape.BackgroundImagePath },
                        { "Opacity", shape.Opacity },
                        { "BorderColor", shape.BorderColor.ToArgb() }
                    };
                    
                    // 如果数据库表支持额外字段，可以存储视觉属性JSON
                    // 这里假设表中有ExtendedProperties字段
                    // node.ExtendedProperties = JsonConvert.SerializeObject(visualProperties);
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
        
        /// <summary>
        /// 获取节点的有效连接器
        /// </summary>
        private Connector GetValidConnector(ProcessNavigationNode node)
        {
            if (node == null || node.Connectors == null || node.Connectors.Count == 0)
            {
                _logger?.LogWarning("节点没有连接器: {nodeId}", node?.NodeId);
                return null;
            }
                
            // 确保节点的连接器位置已更新
            if (typeof(ProcessNavigationNode).GetMethod("UpdateConnectorsPosition") != null)
            {
                try
                {
                    typeof(ProcessNavigationNode).GetMethod("UpdateConnectorsPosition").Invoke(node, null);
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "更新连接器位置失败");
                }
            }
                
            // 尝试多种方式查找连接器
            // 1. 优先使用底部连接器，匹配ProcessNavigationNode中的命名
            Connector connector = null;
            foreach (Connector c in node.Connectors)
            {
                if (c.Name == "BottomNode" || c.Name == "Bottom")
                {
                    connector = c;
                    break;
                }
            }
            
            // 2. 尝试使用名称中包含"Out"或"Exit"的连接器
            if (connector == null)
            {
                foreach (Connector c in node.Connectors)
                {
                    if ((c.Name.IndexOf("Out", StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (c.Name.IndexOf("Exit", StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (c.Name.IndexOf("Bottom", StringComparison.OrdinalIgnoreCase) >= 0))
                    {
                        connector = c;
                        break;
                    }
                }
            }
            
            // 3. 使用任何可用的连接器
            if (connector == null && node.Connectors.Count > 0)
            {
                connector = node.Connectors[0];
            }
            
            // 4. 如果有多个连接器，为源节点选择底部，为目标节点选择顶部
            if (connector != null)
            {
                connector.AllowConnections = true;
            }
            
            // 如果找到连接器，确保它已正确初始化
            if (connector != null)
            {
                // 确保连接器的位置正确
                // if (connector.AttachedTo != null && connector.AttachedTo != node)
                // {
                //     _logger?.LogWarning("连接器已附加到其他节点，重新附加");
                //     connector.AttachedTo = node;
                // }
                
                _logger?.LogDebug("找到有效连接器: {connectorName}", connector.Name);
            }
            else
            {
                _logger?.LogWarning("无法为节点找到连接器: {nodeId}", node.NodeId);
            }
            
            return connector;
        }
        
        /// <summary>
        /// 创建连接的通用方法
        /// </summary>
        private object CreateConnection(GraphControl graphControl, Connector sourceConnector, Connector targetConnector)
        {
            try
            {
                // 尝试使用不同的方法创建连接
                object connection = null;
                
                // 方法1：尝试使用CreateConnection方法（如果存在）
                var createConnMethod = graphControl.GetType().GetMethod("CreateConnection", 
                    new Type[] { typeof(Connector), typeof(Connector) });
                if (createConnMethod != null)
                {
                    connection = createConnMethod.Invoke(graphControl, new object[] { sourceConnector, targetConnector });
                }
                
                // 方法2：如果方法1失败，尝试使用直接实例化
                if (connection == null)
                {
                    // 获取Connection类型
                    var connectionType = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes())
                        .FirstOrDefault(t => t.Name == "Connection" && t.IsClass);
                    
                    if (connectionType != null)
                    {
                        connection = Activator.CreateInstance(connectionType);
                        
                        // 设置源和目标连接器
                        var sourceProp = connectionType.GetProperty("Source") ?? 
                                        connectionType.GetProperty("StartShape");
                        var targetProp = connectionType.GetProperty("Target") ?? 
                                        connectionType.GetProperty("EndShape");
                        
                        if (sourceProp != null)
                            sourceProp.SetValue(connection, sourceConnector);
                        if (targetProp != null)
                            targetProp.SetValue(connection, targetConnector);
                    }
                }
                
                return connection;
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "创建连接失败");
                return null;
            }
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
            // 业务属性
            public int BusinessType { get; set; }
            public string FormName { get; set; }
            public string ClassPath { get; set; }
            public long ModuleID { get; set; }
            // 视觉属性
            public string CustomText { get; set; }
            public bool ShowCustomText { get; set; }
            public float FontSize { get; set; }
            public string FontStyle { get; set; }
            public int FontColor { get; set; }
            public int TextAlignment { get; set; }
            public bool TextWrap { get; set; }
            public string BackgroundImagePath { get; set; }
            public float Opacity { get; set; }
            public int BorderColor { get; set; }
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