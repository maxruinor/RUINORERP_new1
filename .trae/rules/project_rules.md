1. 当前项目使用的框架版本、依赖
2. 请在生成代码时要按代码的规范，格式，缩进,，代码要带注释。
3. 项目是企业级的大开ERP项目，技术：msql,SqlSugar为ORM，WorkflowCore Autofac CacheManager.Core，StackExchange.Redis 
4. MessageBox的提示语，请更加专业
5. 删除整个文件时，要人工确认
6. 保持代码简洁，删除不必要的包装类和示例文件
7. 服务已在依赖注入容器中注册后，直接注入使用，不需要额外的包装层
8. 企业级系统应注重实用性和可维护性，避免过度设计

# 代码创建规则

## 类创建检查
1. 创建新类前，必须搜索项目中是否已存在相似功能的类
2. 使用search_by_regex工具搜索类名、功能关键词
3. 检查相同命名空间下是否已有类似实体

## 实体类规范
1. 统计类统一使用LockInfoStatistics
2. 锁定相关实体统一放在RUINORERP.Server.Network.Services命名空间
3. 新建实体前检查LockInfoManager.cs中是否已有定义

## 搜索命令模板
搜索现有类：search_by_regex -query "class.*Statistics" -search_directory "项目根目录"
搜索功能：search_by_regex -query "锁定|统计|Lock" -search_directory "项目根目录"
 ## 创建新类前的检查清单
- [ ] 搜索项目中是否已有相似类
- [ ] 检查目标命名空间是否已有相关定义
- [ ] 确认功能是否确实需要新类
- [ ] 考虑是否可以复用现有类
- [ ] 评估是否需要重构现有类
## 强制搜索规则
创建任何新类前，必须：
1. 使用search_by_regex搜索类名关键词
2. 使用search_codebase搜索功能描述
3. 查看相关文件夹结构
不满足搜索要求时禁止创建新类