using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using RUINORERP.UI.WorkFlowDesigner.Entities;
using SqlSugar;

namespace RUINORERP.UI.WorkFlowDesigner.Service
{
    /// <summary>
    /// 工作流定义服务类
    /// 负责工作流定义的保存和加载功能
    /// </summary>
    public class WorkflowDefinitionService
    {
        private SqlSugarClient _db;

        public WorkflowDefinitionService(SqlSugarClient db)
        {
            _db = db;
        }

        /// <summary>
        /// 保存工作流定义
        /// </summary>
        /// <param name="definition"></param>
        /// <returns></returns>
        public bool SaveWorkflowDefinition(WorkflowDefinition definition)
        {
            try
            {
                // 开启事务
                _db.Ado.BeginTran();
                
                // 保存工作流定义主表
                var definitionJson = JsonConvert.SerializeObject(definition);
                var existingDefinition = _db.Queryable<WorkflowDefinitionEntity>()
                    .Where(x => x.Id == definition.Id)
                    .First();
                
                if (existingDefinition != null)
                {
                    // 更新现有记录
                    existingDefinition.Name = definition.Name;
                    existingDefinition.Description = definition.Description;
                    existingDefinition.Version = definition.Version;
                    existingDefinition.DefinitionJson = definitionJson;
                    existingDefinition.UpdateTime = DateTime.Now;
                    existingDefinition.IsActive = definition.IsActive;
                    
                    _db.Updateable(existingDefinition).ExecuteCommand();
                }
                else
                {
                    // 插入新记录
                    var definitionEntity = new WorkflowDefinitionEntity
                    {
                        Id = definition.Id,
                        Name = definition.Name,
                        Description = definition.Description,
                        Version = definition.Version,
                        DefinitionJson = definitionJson,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                        IsActive = definition.IsActive
                    };
                    
                    _db.Insertable(definitionEntity).ExecuteCommand();
                }
                
                // 提交事务
                _db.Ado.CommitTran();
                return true;
            }
            catch (Exception ex)
            {
                // 回滚事务
                _db.Ado.RollbackTran();
                throw new Exception($"保存工作流定义失败: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 加载工作流定义
        /// </summary>
        /// <param name="definitionId"></param>
        /// <returns></returns>
        public WorkflowDefinition LoadWorkflowDefinition(string definitionId)
        {
            try
            {
                var definitionEntity = _db.Queryable<WorkflowDefinitionEntity>()
                    .Where(x => x.Id == definitionId)
                    .First();
                
                if (definitionEntity == null)
                {
                    return null;
                }
                
                return JsonConvert.DeserializeObject<WorkflowDefinition>(definitionEntity.DefinitionJson);
            }
            catch (Exception ex)
            {
                throw new Exception($"加载工作流定义失败: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 获取所有工作流定义列表
        /// </summary>
        /// <returns></returns>
        public List<WorkflowDefinition> GetAllWorkflowDefinitions()
        {
            try
            {
                var definitionEntities = _db.Queryable<WorkflowDefinitionEntity>()
                    .Where(x => x.IsActive)
                    .ToList();
                
                var definitions = new List<WorkflowDefinition>();
                foreach (var entity in definitionEntities)
                {
                    var definition = JsonConvert.DeserializeObject<WorkflowDefinition>(entity.DefinitionJson);
                    definitions.Add(definition);
                }
                
                return definitions;
            }
            catch (Exception ex)
            {
                throw new Exception($"获取工作流定义列表失败: {ex.Message}", ex);
            }
        }
    }
    
    /// <summary>
    /// 工作流定义实体类（数据库表对应实体）
    /// </summary>
    [SugarTable("WorkflowDefinitions")]
    public class WorkflowDefinitionEntity
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; }
        
        /// <summary>
        /// 工作流名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 工作流描述
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 版本号
        /// </summary>
        public int Version { get; set; }
        
        /// <summary>
        /// 工作流定义JSON内容
        /// </summary>
        public string DefinitionJson { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }
    }
}