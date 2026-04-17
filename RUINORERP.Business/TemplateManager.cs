using System.Collections.Generic;

namespace RUINORERP.Business
{
    public static class TemplateManager
    {
        private static Dictionary<string, ImportTemplate> _templates = new Dictionary<string, ImportTemplate>();

        static TemplateManager()
        {
            InitializeDefaultTemplates();
        }

        private static void InitializeDefaultTemplates()
        {
            // 1. 产品导入模板
            var prodTemplate = new ImportTemplate
            {
                TemplateName = "标准产品导入 (含图片)",
                TargetTableName = "tb_Prod",
                LogicalKeyField = "ProductNo",
                EnableChildImport = true,
                ColumnMappings = new Dictionary<string, string>
                {
                    { "品号", "ProductNo" },
                    { "品名", "CNName" },
                    { "规格", "Specifications" },
                    { "单位", "Unit_ID" }, // 这里填名称，由 IdRemappingEngine 转换
                    { "类别", "Category_ID" },
                    { "厂商", "CustomerVendor_ID" }
                },
                ImageStrategies = new Dictionary<string, int>
                {
                    { "Images", 1 } // 假设图片在 Excel 对应行的偏移量为 1
                },
                ChildConfig = new ChildTableConfig
                {
                    ChildTableName = "tb_ProdDetail",
                    ForeignKeyField = "ProdBaseID",
                    ChildColumnMappings = new Dictionary<string, string>
                    {
                        { "明细品号", "DetailCode" },
                        { "数量", "Quantity" }
                    }
                }
            };
            _templates["tb_Prod"] = prodTemplate;

            // 2. 供应商/客户导入模板
            var cvTemplate = new ImportTemplate
            {
                TemplateName = "供应商/客户快速导入",
                TargetTableName = "tb_CustomerVendor",
                LogicalKeyField = "Code",
                ColumnMappings = new Dictionary<string, string>
                {
                    { "编码", "Code" },
                    { "全称", "FullName" },
                    { "联系人", "ContactPerson" }
                }
            };
            _templates["tb_CustomerVendor"] = cvTemplate;
        }

        public static ImportTemplate GetTemplate(string tableName)
        {
            return _templates.ContainsKey(tableName) ? _templates[tableName] : null;
        }

        public static List<string> GetAvailableTemplates()
        {
            return new List<string>(_templates.Keys);
        }
    }
}
