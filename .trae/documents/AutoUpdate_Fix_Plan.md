# AutoUpdate 自动更新系统问题修复计划

## 问题分析

### 问题1: AutoUpdate.exe 自身更新不成功

**根本原因分析：**
1. **进程占用问题**：AutoUpdate.exe 正在运行时无法替换自身文件
2. **AutoUpdateUpdater 启动逻辑问题**：
   - `SelfUpdateHelper.StartAutoUpdateUpdater()` 方法创建配置文件后启动 AutoUpdateUpdater.exe
   - 但 AutoUpdateUpdater 在 `ExecuteUpdate` 中等待的是 AutoUpdate 进程退出，而不是自身
   - 文件复制逻辑在 `ReplaceFiles` 中，但此时 AutoUpdate.exe 可能仍在运行
3. **参数传递问题**：配置文件方式虽然避免了命令行参数解析问题，但文件路径和进程等待逻辑需要修正

**关键代码问题：**
- `SelfUpdateHelper.cs` 第 251-282 行：等待的是 `currentProcessName`（即 AutoUpdate），但当前执行的是 AutoUpdateUpdater
- `AutoUpdateUpdater/Program.cs` 第 208-261 行：`WaitAndKillProcess` 方法等待的是 AutoUpdate 进程，但此时应该确保 AutoUpdate 已经完全退出

### 问题2: 启动主程序后提示"程序正在运行中"

**根本原因分析：**
1. **Mutex 释放时机问题**：
   - `Program.cs` 第 131-153 行：`CheckSingleInstance()` 使用 Mutex 检查单实例
   - `ActivateExistingInstance()` 方法（第 414-517 行）尝试激活已存在的实例
   - 但从更新程序启动时，旧进程可能还未完全释放 Mutex

2. **进程等待时间不足**：
   - `AutoUpdateUpdater/Program.cs` 第 702-747 行的 `WaitForAutoUpdateExit()` 方法等待 15 秒
   - 但 `Program.cs` 第 439-466 行在更新场景只等待 30 秒，且没有正确检测进程退出

3. **启动参数检测问题**：
   - `Program.cs` 第 198-209 行检测 `--updated` 参数
   - 但 `SelfUpdateHelper.cs` 第 510 行使用的是 `--updated-from-auto-update`
   - `AutoUpdateUpdater/Program.cs` 第 666 行使用的是 `--updated`
   - 参数不一致导致 `JustUpdated` 标记未正确设置

## 修复方案

### 修复1: 统一启动参数标识

**文件**: `SelfUpdateHelper.cs`
- 修改第 510 行的启动参数，与 `Program.cs` 保持一致

### 修复2: 优化 AutoUpdateUpdater 的进程等待逻辑

**文件**: `AutoUpdateUpdater/Program.cs`
- 优化 `WaitAndKillProcess` 方法，确保正确等待 AutoUpdate 进程退出
- 增加更详细的日志记录

### 修复3: 修复 Program.cs 的单实例检测逻辑

**文件**: `RUINORERP.UI/Program.cs`
- 修复 `ActivateExistingInstance` 方法中的等待逻辑
- 确保从更新程序启动时正确等待旧进程退出
- 统一参数检测逻辑

### 修复4: 修复 SelfUpdateHelper 的配置文件生成

**文件**: `AutoUpdate/SelfUpdateHelper.cs`
- 确保配置文件正确生成
- 优化进程退出检测逻辑

## 实施步骤

### 步骤1: 修复参数不一致问题
- 修改 `SelfUpdateHelper.cs` 第 510 行参数

### 步骤2: 优化 AutoUpdateUpdater 进程等待
- 修改 `AutoUpdateUpdater/Program.cs` 的 `WaitAndKillProcess` 方法

### 步骤3: 修复 Program.cs 单实例检测
- 修改 `ActivateExistingInstance` 方法的等待逻辑

### 步骤4: 测试验证
- 验证所有修改后的流程

## 风险评估

| 修复项 | 风险等级 | 影响范围 |
|--------|----------|----------|
| 参数统一 | 低 | 仅影响更新后启动 |
| 进程等待优化 | 中 | 影响更新流程 |
| 单实例检测 | 中 | 影响程序启动 |

## 注意事项

1. 所有修改必须保持向后兼容
2. 不能影响正常启动流程
3. 保持日志记录以便调试
4. 确保 Mutex 正确释放
