using System;
using System.Data;
using System.Threading.Tasks;
using RUINORERP.Business.ImportEngine.Models;
using RUINORERP.Model.ImportEngine.Models;

namespace RUINORERP.Business.ImportEngine
{
    /// <summary>
    /// 智能导入引擎接口
    /// </summary>
    public interface ISmartImportEngine
    {
        /// <summary>
        /// 执行导入任务（通过方案名称）
        /// </summary>
        Task<ImportReport> ExecuteAsync(string filePath, string profileName, bool isDryRun = false);
        
        /// <summary>
        /// 执行导入任务（直接传入配置对象）
        /// </summary>
        Task<ImportReport> ExecuteWithProfileAsync(string filePath, ImportProfile profile);
        
        /// <summary>
        /// 执行导入任务（使用已解析的DataTable）
        /// </summary>
        Task<ImportReport> ExecuteWithDataTableAsync(DataTable data, ImportProfile profile);
        
        /// <summary>
        /// 获取所有可用的导入方案
        /// </summary>
        System.Collections.Generic.List<string> GetAvailableProfiles();
        
        /// <summary>
        /// 预览数据
        /// </summary>
        Task<DataTable> PreviewDataAsync(string filePath, string profileName, int maxRows = 50);
        
        /// <summary>
        /// 解析Excel文件为DataTable
        /// </summary>
        Task<DataTable> ParseExcelAsync(string filePath, int sheetIndex = 0);
        
        /// <summary>
        /// 获取AI智能列映射建议
        /// </summary>
        /// <param name="excelHeaders">Excel表头列表</param>
        /// <param name="targetEntityType">目标实体类型</param>
        /// <returns>包含映射建议和逻辑主键的结果</returns>
        Task<RUINORERP.Business.AIServices.DataImport.IntelligentMappingResult> GetAiMappingSuggestionsAsync(System.Collections.Generic.List<string> excelHeaders, Type targetEntityType);
    }
}
