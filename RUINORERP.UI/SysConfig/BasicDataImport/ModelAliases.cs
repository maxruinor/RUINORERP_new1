// 此文件用于将Model层的模型类引入UI层命名空间（BasicDataImport）

using ColumnMapping = RUINORERP.Model.ImportEngine.Models.ColumnMapping;
using ForeignRelatedConfig = RUINORERP.Model.ImportEngine.Models.ForeignRelatedConfig;
using ForeignKeySourceColumnConfig = RUINORERP.Model.ImportEngine.Models.ForeignKeySourceColumnConfig;
using SerializableKeyValuePair = RUINORERP.Model.ImportEngine.Models.SerializableKeyValuePair<string>;
using DataSourceType = RUINORERP.Model.ImportEngine.Enums.DataSourceType;
using DeduplicationStrategy = RUINORERP.Model.ImportEngine.Enums.DeduplicationStrategy;
using ImportProfile = RUINORERP.Model.ImportEngine.Models.ImportProfile;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    // 通过using别名，让UI层可以使用Model层的类型
    // 注意：UI层原有的ColumnMapping等类需要逐步迁移或废弃
}
