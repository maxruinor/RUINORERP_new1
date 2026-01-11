## 问题分析

MainForm中存在两个锁定状态变量：
- `IsLocked`属性：用于控制系统的登录状态
- `_isLocked`字段：仅用于更新状态栏显示，但实际上没有被任何代码使用

这种设计存在缺陷，导致状态管理不一致。

## 解决方案

1. **移除冗余的`_isLocked`字段**：由于该字段仅在`UpdateLockStatus`方法中被赋值，没有其他地方使用，可以完全移除
2. **修改`UpdateLockStatus`方法**：只使用`IsLocked`属性，不再更新冗余字段
3. **确保锁定状态一致性**：所有锁定状态的更新都通过`IsLocked`属性进行

## 具体修改

1. **移除`_isLocked`字段声明**（第1544行）
2. **修改`UpdateLockStatus`方法**（第1550-1578行）：
   - 移除`_isLocked = isLocked;`赋值
   - 保留`IsLocked = isLocked;`赋值
   - 保留UI更新逻辑

## 预期效果

- 统一了锁定状态管理
- 简化了代码结构
- 消除了状态不一致的风险
- 显示状态直接由真实的授权、登录、断开等状态决定

## 修改文件

- `e:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.UI\MainForm.cs`