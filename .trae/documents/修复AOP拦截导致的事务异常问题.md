## 问题分析

**现象**：
- 执行事务开始（BeginTran）时被AOP拦截
- 拦截过程中出现序列化异常和类型转换异常
- 最终导致"不允许启动新事务，因为有其他线程正在该会话中运行"的错误

**根本原因**：
- `BaseDataCacheAOP`拦截了`UnitOfWorkManage`的所有方法，包括`GetDbClient`和`BeginTran`
- 当拦截`GetDbClient`方法时，尝试对返回的`SqlSugarScope`对象进行缓存处理
- 这个过程中发生序列化异常，导致数据库连接状态混乱
- 后续执行`BeginTran`时，数据库会话已被其他线程占用

## 修复方案

**方案1：修改BaseDataCacheAOP，排除UnitOfWorkManage相关方法**
1. 在`BaseDataCacheAOP.Intercept`方法中添加类型检查
2. 当拦截目标是`UnitOfWorkManage`类型时，直接执行方法，不进行缓存处理
3. 确保只对需要缓存的业务方法进行拦截

**方案2：优化缓存键生成和类型检查**
1. 修改`CustomCacheKey`方法，对不适合缓存的类型返回空键
2. 在缓存处理前添加类型检查，避免对SqlSugar相关对象进行序列化
3. 确保只有业务数据对象才会被缓存

## 具体修改步骤

1. **修改BaseDataCacheAOP.cs**：
   - 在`Intercept`方法开始处添加类型检查
   - 排除`UnitOfWorkManage`类型的方法拦截
   - 优化缓存键生成逻辑，避免对复杂对象的序列化

2. **验证修复效果**：
   - 执行`tb_FM_ReceivablePayableController.BaseSaveOrUpdateWithChild`方法
   - 检查是否不再出现序列化异常
   - 确认事务能够正常开启和提交

## 预期结果

- AOP不再拦截`UnitOfWorkManage`的方法
- 事务操作能够正常执行，不再出现"不允许启动新事务"的错误
- 业务方法的缓存功能保持正常

## 风险评估

- **低风险**：修改仅影响AOP拦截逻辑，不改变核心业务功能
- **兼容性**：保持对现有缓存功能的支持
- **可观测性**：通过日志和调试信息验证修复效果