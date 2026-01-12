## 问题分析
当前登录窗体在显示欢迎公告时会调用`MoveLoginFormDown()`方法，动态调整登录控件的位置和窗体高度，这与用户要求的固定UI设计不符。

## 解决方案
移除所有与动态UI调整相关的代码，确保登录界面保持固定位置。

## 具体修改点

### 1. 移除动态调整逻辑方法
- **文件**: `RUINORERP.UI\FrmLogin.cs`
- **操作**: 删除`MoveLoginFormDown()`方法（第174-206行）

### 2. 修改公告显示方法
- **文件**: `RUINORERP.UI\FrmLogin.cs`
- **操作**: 修改`DisplayAnnouncement()`方法（第148-167行），移除对`MoveLoginFormDown()`的调用，仅保留公告文本设置和面板显示逻辑

### 3. 确认面板位置设置
- **文件**: `RUINORERP.UI\FrmLogin.Designer.cs`
- **操作**: 确认`panelAnnouncement`控件已处于固定位置（当前位于`(43, 340)`，不会影响其他登录控件）

### 4. 验证其他相关逻辑
- **文件**: `RUINORERP.UI\FrmLogin.cs`
- **操作**: 检查`OnAnnouncementReceived()`、`btnCloseAnnouncement_Click()`等方法，确保没有其他动态调整逻辑

## 预期效果
- 登录窗体的UI元素保持固定位置，不会因公告显示而移动
- 欢迎公告面板在固定位置显示/隐藏，不影响其他控件布局
- 登录界面整体布局保持稳定，符合用户要求的固定设计

## 测试要点
- 验证公告显示时登录控件位置不变
- 验证公告隐藏时登录控件位置不变
- 验证登录流程正常进行
- 验证窗体高度保持固定