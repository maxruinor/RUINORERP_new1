using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using SqlSugar;

namespace RUINORERP.Business.ImportEngine
{
    /// <summary>
    /// 元数据依赖分析器
    /// 利用实体类中的导航属性和外键特性，自动推导主表与子表、依赖表之间的关联关系
    /// </summary>
    public class MetadataDependencyAnalyzer
    {
        /// <summary>
        /// 分析指定实体类型的依赖关系
        /// </summary>
        public static DependencyAnalysisResult Analyze(Type entityType)
        {
            var result = new DependencyAnalysisResult();
            
            if (entityType == null || !typeof(RUINORERP.Model.BaseEntity).IsAssignableFrom(entityType))
            {
                return result;
            }

            // 1. 获取外键依赖（依赖表）
            var fkRelations = GetFKRelations(entityType);
            foreach (var relation in fkRelations)
            {
                if (!result.DependencyTables.Contains(relation.FKTableName))
                {
                    result.DependencyTables.Add(relation.FKTableName);
                }
            }

            // 2. 尝试识别子表（通过扫描已知实体或通过 SugarTable 特性反向匹配）
            // 注意：由于反射开销，这里仅做简单演示。在实际生产中，建议维护一个全局的 EntityRegistry。
            result.MasterTable = entityType.Name;

            return result;
        }

        /// <summary>
        /// 获取实体的外键关系列表
        /// </summary>
        private static List<RUINORERP.Model.Base.FKRelationInfo> GetFKRelations(Type entityType)
        {
            try
            {
                var instance = Activator.CreateInstance(entityType) as RUINORERP.Model.BaseEntity;
                return instance?.ImportFKRelations ?? new List<RUINORERP.Model.Base.FKRelationInfo>();
            }
            catch
            {
                return new List<RUINORERP.Model.Base.FKRelationInfo>();
            }
        }

        /// <summary>
        /// 依赖分析结果
        /// </summary>
        public class DependencyAnalysisResult
        {
            public string MasterTable { get; set; }
            public List<string> DependencyTables { get; set; } = new List<string>();
            public List<string> ChildTables { get; set; } = new List<string>();
        }
    }
}
