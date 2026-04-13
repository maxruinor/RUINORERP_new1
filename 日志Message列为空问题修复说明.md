		2026-04-13 0:11:10	ERROR	RUINORERP.Server.Network.Services.FileStorageMonitorService			系统服务						WATSON
		2026-04-13 0:41:10	ERROR	RUINORERP.Server.Network.Services.FileStorageMonitorService			系统服务						WATSON
		2026-04-13 1:11:10	ERROR	RUINORERP.Server.Network.Services.FileStorageMonitorService			系统服务						WATSON
		2026-04-13 1:41:10	ERROR	RUINORERP.Server.Network.Services.FileStorageMonitorService			系统服务						WATSON
		2026-04-13 2:11:10	ERROR	RUINORERP.Server.Network.Services.FileStorageMonitorService			系统服务						WATSON
		2026-04-13 2:41:10	ERROR	RUINORERP.Server.Network.Services.FileStorageMonitorService			系统服务						WATSON
		2026-04-13 3:11:10	ERROR	RUINORERP.Server.Network.Services.FileStorageMonitorService			系统服务						WATSON
		2026-04-13 3:41:10	ERROR	RUINORERP.Server.Network.Services.FileStorageMonitorService			系统服务						WATSON
		2026-04-13 4:11:10	ERROR	RUINORERP.Server.Network.Services.FileStorageMonitorService			系统服务						WATSON
		2026-04-13 4:41:10	ERROR	RUINORERP.Server.Network.Services.FileStorageMonitorService			系统服务						WATSON
		2026-04-13 5:11:10	ERROR	RUINORERP.Server.Network.Services.FileStorageMonitorService			系统服务						WATSON
		2026-04-13 5:41:10	ERROR	RUINORERP.Server.Network.Services.FileStorageMonitorService			系统服务						WATSON
		2026-04-13 6:11:10	ERROR	RUINORERP.Server.Network.Services.FileStorageMonitorService			系统服务						WATSON
		2026-04-13 6:41:10	ERROR	RUINORERP.Server.Network.Services.FileStorageMonitorService			系统服务						WATSON
		2026-04-13 7:11:10	ERROR	RUINORERP.Server.Network.Services.FileStorageMonitorService			系统服务						WATSON
		2026-04-13 7:41:10	ERROR	RUINORERP.Server.Network.Services.FileStorageMonitorService			系统服务						WATSON
	龙成华	2026-04-13 8:03:04	WARN	RUINORERP.UI.MainForm	欢迎流程验证超时,但连接已建立						192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 8:03:07	ERROR	RUINORERP.Extensions.SqlsugarSetup	SQL 执行错误：中文提示 :  连接数据库过程中发生错误，检查服务器是否正常连接字符串是否正确，错误信息：连接未关闭。 连接的当前状态为正在连接。DbType="SqlServer";ConfigId="".
English Message : Connection open error . 连接未关闭。 连接的当前状态为正在连接。DbType="SqlServer";ConfigId="" , SQL: SELECT [ConfigID],[ConfigKey],[ConfigValue],[Description],[ValueType],[ConfigType],[IsActive],[Created_at],[Created_by],[Modified_at],[Modified_by] FROM [tb_SysGlobalDynamicConfig] WITH(NOLOCK)  		系统服务				192.168.1.250
		CHINAMI-3LUO0MU
	樊仕雯	2026-04-13 8:03:24	WARN	RUINORERP.UI.MainForm	欢迎流程验证超时,但连接已建立						192.168.1.250
		-CHINAMI-3LUO0MU-Administrator
	龙成华	2026-04-13 8:06:21	ERROR	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-29f6bd0c-1e02-4eca-b5dd-491091aa5ded] 事务提交失败	类型: System.Data.SqlClient.SqlException
消息: 无法执行该事务操作，因为有挂起请求正在此事务上运行。
堆栈:    在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.TdsParser.Run(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj)
   在 System.Data.SqlClient.TdsParser.TdsExecuteTransactionManagerRequest(Byte[] buffer, TransactionManagerRequestType request, String transactionName, TransactionManagerIsolationLevel isoLevel, Int32 timeout, SqlInternalTransaction transaction, TdsParserStateObject stateObj, Boolean isDelegateControlRequest)
   在 System.Data.SqlClient.SqlInternalConnectionTds.ExecuteTransactionYukon(TransactionRequest transactionRequest, String transactionName, IsolationLevel iso, SqlInternalTransaction internalTransaction, Boolean isDelegateControlRequest)
   在 System.Data.SqlClient.SqlInternalTransaction.Commit()
   在 System.Data.SqlClient.SqlTransaction.Commit()
   在 SqlSugar.AdoProvider.CommitTran()
   在 RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage.CommitTranInternal()
		生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:06:21	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-Unknown] 强制回滚：事务连接已关闭或无效			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:06:21	ERROR	RUINORERP.Extensions.SqlsugarSetup	SQL 执行错误：INSERT 语句与 FOREIGN KEY 约束"FK_TB_MATERM_REF_TB_MATERDETAIL"冲突。该冲突发生于数据库"erpnew"，表"dbo.tb_MaterialRequisition", column 'MR_ID'。
语句已终止。, SQL: INSERT [tb_MaterialRequisitionDetail]  ([Detail_ID],[MR_ID],[Location_ID],[ProdDetailID],[property],[ShouldSendQty],[ActualSentQty],[CanQuantity],[Summary],[CustomerPartNo],[Cost],[Price],[SubtotalPrice],[SubtotalCost],[ReturnQty],[ManufacturingOrderDetailRowID])
 SELECT N'2043480875215228928' AS [Detail_ID],N'2043480874955182080' AS [MR_ID],N'1740618960329641984' AS [Location_ID],N'1959189752607543296' AS [ProdDetailID],NULL AS [property],N'125' AS [ShouldSendQty],N'50' AS [ActualSentQty],N'5' AS [CanQuantity],NULL AS [Summary],NULL AS [CustomerPartNo],N'0' AS [Cost],N'0' AS [Price],N'0' AS [SubtotalPrice],N'0' AS [SubtotalCost],N'0' AS [ReturnQty],N'2010698458113118208' AS [ManufacturingOrderDetailRowID] 
UNION ALL 
 SELECT N'2043480875215228929' AS [Detail_ID],N'2043480874955182080' AS [MR_ID],N'1740618960329641984' AS [Location_ID],N'1816765388504043520' AS [ProdDetailID],NULL AS [property],N'125' AS [ShouldSendQty],N'50' AS [ActualSentQty],N'1983' AS [CanQuantity],NULL AS [Summary],NULL AS [CustomerPartNo],N'0' AS [Cost],N'0' AS [Price],N'0' AS [SubtotalPrice],N'0' AS [SubtotalCost],N'0' AS [ReturnQty],N'2010698458113118209' AS [ManufacturingOrderDetailRowID] 
UNION ALL 
 SELECT N'2043480875215228930' AS [Detail_ID],N'2043480874955182080' AS [MR_ID],N'1740618960329641984' AS [Location_ID],N'1955930073064411136' AS [ProdDetailID],NULL AS [property],N'125' AS [ShouldSendQty],N'50' AS [ActualSentQty],N'2036' AS [CanQuantity],NULL AS [Summary],NULL AS [CustomerPartNo],N'0' AS [Cost],N'0' AS [Price],N'0' AS [SubtotalPrice],N'0' AS [SubtotalCost],N'0' AS [ReturnQty],N'2010698458113118210' AS [ManufacturingOrderDetailRowID] 
UNION ALL 
 SELECT N'2043480875215228931' AS [Detail_ID],N'2043480874955182080' AS [MR_ID],N'1740618960329641984' AS [Location_ID],N'1955921198219137024' AS [ProdDetailID],NULL AS [property],N'125' AS [ShouldSendQty],N'50' AS [ActualSentQty],N'429' AS [CanQuantity],NULL AS [Summary],NULL AS [CustomerPartNo],N'0' AS [Cost],N'0' AS [Price],N'0' AS [SubtotalPrice],N'0' AS [SubtotalCost],N'0' AS [ReturnQty],N'2010698458113118211' AS [ManufacturingOrderDetailRowID] 
UNION ALL 
 SELECT N'2043480875215228932' AS [Detail_ID],N'2043480874955182080' AS [MR_ID],N'1740618960329641984' AS [Location_ID],N'1955921479795347456' AS [ProdDetailID],NULL AS [property],N'125' AS [ShouldSendQty],N'50' AS [ActualSentQty],N'382' AS [CanQuantity],NULL AS [Summary],NULL AS [CustomerPartNo],N'0' AS [Cost],N'0' AS [Price],N'0' AS [SubtotalPrice],N'0' AS [SubtotalCost],N'0' AS [ReturnQty],N'2010698458113118212' AS [ManufacturingOrderDetailRowID] 
UNION ALL 
 SELECT N'2043480875215228933' AS [Detail_ID],N'2043480874955182080' AS [MR_ID],N'1740618960329641984' AS [Location_ID],N'1955923691464429568' AS [ProdDetailID],NULL AS [property],N'125' AS [ShouldSendQty],N'50' AS [ActualSentQty],N'69' AS [CanQuantity],NULL AS [Summary],NULL AS [CustomerPartNo],N'0' AS [Cost],N'0' AS [Price],N'0' AS [SubtotalPrice],N'0' AS [SubtotalCost],N'0' AS [ReturnQty],N'2010698458113118214' AS [ManufacturingOrderDetailRowID] 
UNION ALL 
 SELECT N'2043480875215228934' AS [Detail_ID],N'2043480874955182080' AS [MR_ID],N'1740618960329641984' AS [Location_ID],N'1955932903099731968' AS [ProdDetailID],NULL AS [property],N'125' AS [ShouldSendQty],N'50' AS [ActualSentQty],N'105' AS [CanQuantity],NULL AS [Summary],NULL AS [CustomerPartNo],N'0' AS [Cost],N'0' AS [Price],N'0' AS [SubtotalPrice],N'0' AS [SubtotalCost],N'0' AS [ReturnQty],N'2010698458113118215' AS [ManufacturingOrderDetailRowID] 
UNION ALL 
 SELECT N'2043480875215228935' AS [Detail_ID],N'2043480874955182080' AS [MR_ID],N'1740618960329641984' AS [Location_ID],N'1742056025806213120' AS [ProdDetailID],NULL AS [property],N'125' AS [ShouldSendQty],N'50' AS [ActualSentQty],N'5784' AS [CanQuantity],NULL AS [Summary],NULL AS [CustomerPartNo],N'0' AS [Cost],N'0' AS [Price],N'0' AS [SubtotalPrice],N'0' AS [SubtotalCost],N'0' AS [ReturnQty],N'2010698458113118216' AS [ManufacturingOrderDetailRowID] 
UNION ALL 
 SELECT N'2043480875215228936' AS [Detail_ID],N'2043480874955182080' AS [MR_ID],N'1740618960329641984' AS [Location_ID],N'1750052384282906624' AS [ProdDetailID],NULL AS [property],N'125' AS [ShouldSendQty],N'50' AS [ActualSentQty],N'819' AS [CanQuantity],NULL AS [Summary],NULL AS [CustomerPartNo],N'0' AS [Cost],N'0' AS [Price],N'0' AS [SubtotalPrice],N'0' AS [SubtotalCost],N'0' AS [ReturnQty],N'2010698458113118217' AS [ManufacturingOrderDetailRowID] 
UNION ALL 
 SELECT N'2043480875215228937' AS [Detail_ID],N'2043480874955182080' AS [MR_ID],N'1740618960329641984' AS [Location_ID],N'1750071787464560640' AS [ProdDetailID],NULL AS [property],N'1' AS [ShouldSendQty],N'1' AS [ActualSentQty],N'11' AS [CanQuantity],NULL AS [Summary],NULL AS [CustomerPartNo],N'0' AS [Cost],N'0' AS [Price],N'0' AS [SubtotalPrice],N'0' AS [SubtotalCost],N'0' AS [ReturnQty],N'2010698458113118218' AS [ManufacturingOrderDetailRowID] 
UNION ALL 
 SELECT N'2043480875215228938' AS [Detail_ID],N'2043480874955182080' AS [MR_ID],N'1740618960329641984' AS [Location_ID],N'1743105004602003456' AS [ProdDetailID],NULL AS [property],N'1' AS [ShouldSendQty],N'1' AS [ActualSentQty],N'28' AS [CanQuantity],NULL AS [Summary],NULL AS [CustomerPartNo],N'0' AS [Cost],N'0' AS [Price],N'0' AS [SubtotalPrice],N'0' AS [SubtotalCost],N'0' AS [ReturnQty],N'2010698458113118219' AS [ManufacturingOrderDetailRowID] 
UNION ALL 
 SELECT N'2043480875215228939' AS [Detail_ID],N'2043480874955182080' AS [MR_ID],N'1740618960329641984' AS [Location_ID],N'1810209518236340224' AS [ProdDetailID],NULL AS [property],N'4' AS [ShouldSendQty],N'1' AS [ActualSentQty],N'9' AS [CanQuantity],NULL AS [Summary],NULL AS [CustomerPartNo],N'0' AS [Cost],N'0' AS [Price],N'0' AS [SubtotalPrice],N'0' AS [SubtotalCost],N'0' AS [ReturnQty],N'2010698458113118220' AS [ManufacturingOrderDetailRowID]
;
select SCOPE_IDENTITY();			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:06:21	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[CommitTran] 提交请求但无事务上下文			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:06:21	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-30627391-e2f6-47c8-9b5a-55343073f833] 事务连接已关闭或无效，跳过回滚			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:06:21	ERROR	RUINORERP.Business.tb_MaterialRequisitionController	INSERT 语句与 FOREIGN KEY 约束"FK_TB_MATERM_REF_TB_MATERDETAIL"冲突。该冲突发生于数据库"erpnew"，表"dbo.tb_MaterialRequisition", column 'MR_ID'。
语句已终止。	类型: System.Data.SqlClient.SqlException
消息: INSERT 语句与 FOREIGN KEY 约束"FK_TB_MATERM_REF_TB_MATERDETAIL"冲突。该冲突发生于数据库"erpnew"，表"dbo.tb_MaterialRequisition", column 'MR_ID'。
语句已终止。
堆栈:    在 SqlSugar.AdoProvider.ExecuteCommand(String sql, SugarParameter[] parameters)
   在 SqlSugar.InsertableProvider`1.ExecuteCommand()
   在 SqlSugar.UpdateNavProvider`2.SetValue[TChild](EntityColumnInfo pkColumn, List`1 UpdateData, Func`1 value)
   在 SqlSugar.UpdateNavProvider`2.InitData[TChild](EntityColumnInfo pkColumn, List`1 UpdateData)
   在 SqlSugar.UpdateNavProvider`2.InsertDatas[TChild](List`1 children, EntityColumnInfo pkColumn, EntityColumnInfo NavColumn)
   在 SqlSugar.UpdateNavProvider`2.DeleteInsert[TChild](String name, EntityColumnInfo nav)
   在 SqlSugar.UpdateNavProvider`2.UpdateOneToMany[TChild](String name, EntityColumnInfo nav)
   在 SqlSugar.UpdateNavProvider`2._ThenInclude[TChild](Expression`1 expression)
   在 SqlSugar.UpdateNavProvider`2.ThenInclude[TChild](Expression`1 expression)
   在 SqlSugar.UpdateNavTask`2.<>c__DisplayClass13_0`1.<ThenInclude>b__0()
   在 SqlSugar.UpdateNavTask`2.ExecuteCommand()
   在 SqlSugar.UpdateNavTask`2.<<ExecuteCommandAsync>b__21_0>d.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 SqlSugar.UpdateNavTask`2.<ExecuteCommandAsync>d__21.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.Business.tb_MaterialRequisitionController`1.<BaseSaveOrUpdateWithChild>d__16`1.MoveNext()
		生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:06:21	ERROR	RUINORERP.Business.BaseController	保存失败，请重试;或联系管理员。INSERT 语句与 FOREIGN KEY 约束"FK_TB_MATERM_REF_TB_MATERDETAIL"冲突。该冲突发生于数据库"erpnew"，表"dbo.tb_MaterialRequisition", column 'MR_ID'。
语句已终止。			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:06:26	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-148e9000-06d4-4080-b747-66bd2c900004] 事务连接已关闭或无效，无法提交			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:06:45	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-5ad370ec-3b93-458b-8229-8f8cdd65d809] 事务连接已关闭或无效，无法提交			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:06:48	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-9af1339e-b775-46d4-acb4-760a560270e1] 事务连接已关闭或无效，无法提交			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:06:57	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043480977078095872, 原因: 单据未被锁定或不存在			产品转换单	RUINORERP.UI.PSI.INV.UCProdConversion	新增	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:07:01	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043480732181073920, 原因: 单据未被锁定或不存在			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	新增	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:07:02	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043480874955182080, 原因: 单据未被锁定或不存在			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	新增	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	樊仕雯	2026-04-13 8:08:14	WARN	RUINORERP.Business.tb_SaleOutController	销售出库单【SOD7EA4D0950】自动审核失败:sku:SK7E99112142库存为：0，拟销售量为：2
 系统设置不允许负库存， 请检查出库数量与库存相关数据			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	提交	192.168.1.250
		-CHINAMI-3LUO0MU-Administrator
	龙成华	2026-04-13 8:08:24	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-5424de49-8c9a-4db5-a1ab-f66f75d16c25] 事务连接已关闭或无效，无法提交			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:08:26	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-5c841e24-c7f6-4290-9290-74e431aafc9a] 事务连接已关闭或无效，无法提交			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:08:45	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-41fec018-cb46-4adf-8b0a-a2248475fc67] 事务连接已关闭或无效，无法提交			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:08:48	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-0e218e44-5d82-4e42-82c1-1def144a22e9] 事务连接已关闭或无效，无法提交			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	樊仕雯	2026-04-13 8:08:53	ERROR	RUINORERP.UI.MainForm	销售出库单:SOD7EA4D0952审核失败销售订单:SO2604111783的【磁吸挂绳支架磁吸挂绳支架】的出库数量不能大于订单中对应行的数量，
" 或存在当前销售订单重复录入了销售出库单，审核失败！			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.250
		-CHINAMI-3LUO0MU-Administrator
	樊仕雯	2026-04-13 8:09:04	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043481489403940864, 原因: 单据未被锁定或不存在			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.250
		-CHINAMI-3LUO0MU-Administrator
	龙成华	2026-04-13 8:09:23	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043481480562348032, 原因: 单据未被锁定或不存在			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:09:26	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043481390275760128, 原因: 单据未被锁定或不存在			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	新增	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	樊仕雯	2026-04-13 8:09:30	ERROR	RUINORERP.UI.MainForm	销售出库单:SOD7EA4D0954审核失败sku:SK7E9B112367库存为：0，拟销售量为：1
 系统设置不允许负库存， 请检查出库数量与库存相关数据			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.250
		-CHINAMI-3LUO0MU-Administrator
	龙成华	2026-04-13 8:10:10	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-6f58d2b3-cc6d-48b1-bba1-c8efbc279df7] 事务连接已关闭或无效，无法提交			行为菜单=>缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	反审	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	樊仕雯	2026-04-13 8:10:19	WARN	RUINORERP.Business.tb_SaleOutController	销售出库单【SOD7EA4D0957】自动审核失败:sku:SK7E931810848库存为：0，拟销售量为：1
 系统设置不允许负库存， 请检查出库数量与库存相关数据			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	提交	192.168.1.250
		-CHINAMI-3LUO0MU-Administrator
	樊仕雯	2026-04-13 8:10:34	WARN	RUINORERP.Business.tb_SaleOutController	销售出库单【SOD7EA4D0958】自动审核失败:sku:SKU7E811801304库存为：0，拟销售量为：1
 系统设置不允许负库存， 请检查出库数量与库存相关数据			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	提交	192.168.1.250
		-CHINAMI-3LUO0MU-Administrator
	龙成华	2026-04-13 8:11:13	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-a270818f-bd04-4a98-9003-32a3da003eb7] 事务连接已关闭或无效，无法提交			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:11:15	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-e59a249a-f85f-449f-95f2-ac928c9e75c2] 事务连接已关闭或无效，无法提交			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 8:11:10	ERROR	RUINORERP.Server.Network.Services.FileStorageMonitorService			系统服务						WATSON
	龙成华	2026-04-13 8:11:24	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043481390275760128, 原因: 单据未被锁定或不存在			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	樊仕雯	2026-04-13 8:11:30	ERROR	RUINORERP.UI.MainForm	销售出库单:SOD7EA4D0962审核失败 销售订单SO2604111772状态为【完结】
请确认状态为【确认】已审核，并且审核结果为已通过!
请检查订单状态数据是否正确，或当前为相同订单重复出库！			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.250
		-CHINAMI-3LUO0MU-Administrator
		2026-04-13 8:11:39	ERROR	RUINORERP.Extensions.SqlsugarSetup			系统服务				192.168.1.150
		SK-20241121HYUW
	陈巧	2026-04-13 8:11:59	WARN	RUINORERP.UI.MainForm			-2.0.0.5-2026/4/3 0:23:54-http://smarterp.7766.org:1688/test/updaterFiler/				192.168.1.150
		-SK-20241121HYUW-Administrator
	樊仕雯	2026-04-13 8:12:25	WARN	RUINORERP.Business.tb_SaleOutController	销售出库单【SOD7EA4D0966】自动审核失败:sku:SK7E94710975库存为：0，拟销售量为：1
 系统设置不允许负库存， 请检查出库数量与库存相关数据			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	提交	192.168.1.250
		-CHINAMI-3LUO0MU-Administrator
	龙成华	2026-04-13 8:14:21	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-082097cb-ec2f-419c-8c70-12dae9e303b2] 事务连接已关闭或无效，无法提交			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:14:24	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-176db819-1074-43b9-a9c2-ec230de78b31] 事务连接已关闭或无效，无法提交			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:14:44	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-b8ee8b6a-bbfc-49b2-9116-a8d2318d89b9] 事务连接已关闭或无效，无法提交			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:14:47	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-90c5b56e-5146-42df-bf5e-a7a1a8662ae4] 事务连接已关闭或无效，无法提交			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	樊仕雯	2026-04-13 8:14:47	ERROR	RUINORERP.UI.MainForm	销售出库单:SOD7EA4D0976审核失败sku:SK7E99312156库存为：18，拟销售量为：30
 系统设置不允许负库存， 请检查出库数量与库存相关数据			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.250
		-CHINAMI-3LUO0MU-Administrator
	龙成华	2026-04-13 8:15:38	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-9b993889-1c40-4d3b-ad69-9e70c472c6f6] 事务连接已关闭或无效，无法提交			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:15:39	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-a900dab2-c5fc-4f35-b973-efb2a44bf59a] 事务连接已关闭或无效，无法提交			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:16:06	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-4393b47b-d995-48f2-b53e-7e289475acbf] 事务连接已关闭或无效，无法提交			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:16:07	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-cc7e1ab8-5efc-4c23-98f0-ff6adc1991ba] 事务连接已关闭或无效，无法提交			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	樊仕雯	2026-04-13 8:16:11	WARN	RUINORERP.Business.tb_SaleOutController	销售出库单【SOD7EA4D0981】自动审核失败:sku:SK7E931810848库存为：0，拟销售量为：1
 系统设置不允许负库存， 请检查出库数量与库存相关数据			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	提交	192.168.1.250
		-CHINAMI-3LUO0MU-Administrator
	龙成华	2026-04-13 8:16:50	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-c571c958-67ec-419f-ac91-b1dee1ff66b2] 事务连接已关闭或无效，无法提交			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:16:52	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-946e8395-22df-4326-83db-ec1909656f35] 事务连接已关闭或无效，无法提交			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:16:54	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043482983972868096, 原因: 单据未被锁定或不存在			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:16:55	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043482891001925632, 原因: 单据未被锁定或不存在			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	樊仕雯	2026-04-13 8:16:56	WARN	RUINORERP.Business.tb_SaleOutController	销售出库单【SOD7EA4D0984】自动审核失败:sku:SKU7E8B88902库存为：129，拟销售量为：200
 系统设置不允许负库存， 请检查出库数量与库存相关数据			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	提交	192.168.1.250
		-CHINAMI-3LUO0MU-Administrator
	龙成华	2026-04-13 8:17:11	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-e6c32e14-e393-4e5f-9961-52e3e584a66e] 事务连接已关闭或无效，无法提交			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:17:12	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-84bff106-cc89-4b33-99e2-7791ca92f682] 事务连接已关闭或无效，跳过回滚			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:17:12	WARN	RUINORERP.Business.tb_SaleOutController	销售出库单【SOD7EA4D0986】自动审核失败:sku:SKU7E8B18739库存为：3，拟销售量为：40
 系统设置不允许负库存， 请检查出库数量与库存相关数据			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	樊仕雯	2026-04-13 8:17:16	ERROR	RUINORERP.UI.MainForm	销售出库单:SOD7EA4D0985审核失败sku:SK7E95A11272库存为：0，拟销售量为：1
 系统设置不允许负库存， 请检查出库数量与库存相关数据			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.250
		-CHINAMI-3LUO0MU-Administrator
	樊仕雯	2026-04-13 8:17:28	ERROR	RUINORERP.UI.MainForm	获取到最新的验证配置LogError			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	审核	192.168.1.250
		-CHINAMI-3LUO0MU-Administrator
		2026-04-13 8:17:44	ERROR	RUINORERP.Extensions.SqlsugarSetup	SQL 执行错误：中文提示 :  连接数据库过程中发生错误，检查服务器是否正常连接字符串是否正确，错误信息：连接未关闭。 连接的当前状态为正在连接。DbType="SqlServer";ConfigId="".
English Message : Connection open error . 连接未关闭。 连接的当前状态为正在连接。DbType="SqlServer";ConfigId="" , SQL: SELECT [ConfigID],[ConfigKey],[ConfigValue],[Description],[ValueType],[ConfigType],[IsActive],[Created_at],[Created_by],[Modified_at],[Modified_by] FROM [tb_SysGlobalDynamicConfig] WITH(NOLOCK)  		系统服务				192.168.1.250
		CHINAMI-3LUO0MU
	樊仕雯	2026-04-13 8:18:03	WARN	RUINORERP.UI.MainForm	欢迎流程验证超时,但连接已建立						192.168.1.250
		-CHINAMI-3LUO0MU-Administrator
	龙成华	2026-04-13 8:23:18	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-4e81a3fb-8965-42c6-a5bc-30c90da373d7] 事务连接已关闭或无效，无法提交			销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:23:18	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-4241e9b4-d903-49a0-bbf2-7d3828f482a7] 事务连接已关闭或无效，无法提交			销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:26:07	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-cffedb5f-bed2-4813-a9b3-d45c57bbd154] 事务连接已关闭或无效，无法提交			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:26:08	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-be02ae38-86ea-47bd-9920-39ebfa986440] 事务连接已关闭或无效，无法提交			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:28:03	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-1905e500-2c8d-49cf-8897-35b306dc339a] 事务连接已关闭或无效，无法提交			其他出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:28:07	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-66c44d02-31b9-4a72-a070-3dfcd45bae7f] 事务连接已关闭或无效，无法提交			其他出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:29:10	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043486338673676288, 原因: 单据未被锁定或不存在			其他出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:29:49	WARN	RUINORERP.UI.Network.Services.FileBusinessService	业务ID无效: 0, 删除操作可能无法正确执行			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	删除	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:31:11	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-9a8997ae-4451-40f3-84c1-df9384cc1c0a] 事务连接已关闭或无效，无法提交			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:31:20	ERROR	RUINORERP.UI.MainForm	生产领料单:PRD260413178审核失败非补料时，制令单:MO26033187的【醋酸布（黑布）醋酸布  20mm*33m】的领料数量2.000不能大于制令单对应行的应发数量0.300。			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:32:18	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-e0c4e0ad-ff7e-4fdb-aa09-6a23e702f01d] 事务连接已关闭或无效，无法提交			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:32:21	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-ba573de1-3fec-4df1-b8e1-58478f33607d] 事务连接已关闭或无效，无法提交			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:32:50	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-80551cbf-91e5-48c8-9288-6a85a0a051dd] 事务连接已关闭或无效，无法提交			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:32:54	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-7b86673d-cadc-4a96-a5af-23b9e2c772d5] 事务连接已关闭或无效，无法提交			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:33:03	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043487405843025920, 原因: 单据未被锁定或不存在			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	新增	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 8:33:10	WARN	RUINORERP.UI.MainForm	欢迎流程验证超时,但连接已建立						192.168.1.24
		WIN-BIJ6DP95VLQ
	龙成华	2026-04-13 8:33:18	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-320ee1fb-f57c-488b-97d3-9133a4893784] 事务连接已关闭或无效，无法提交			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:33:20	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-fe5e1089-df29-42d8-ac23-aab87f042045] 事务连接已关闭或无效，无法提交			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:33:32	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-424bcf2f-b0bc-421b-bbb9-3d9959fd9028] 事务连接已关闭或无效，无法提交			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:33:38	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-a850d4b3-b239-43ca-aac8-c1f51c8f97c8] 事务连接已关闭或无效，跳过回滚			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:33:38	ERROR	RUINORERP.UI.MainForm	销售出库单:SOD7EA4D0993审核失败sku:SK7E9B112367库存为：0，拟销售量为：1
 系统设置不允许负库存， 请检查出库数量与库存相关数据			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:34:09	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043487538668244992, 原因: 单据未被锁定或不存在			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	新增	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:37:04	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-24fa30b9-3dae-4b31-9e2c-487033ac4cf0] 事务连接已关闭或无效，无法提交			商品分割单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:37:07	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-7d4fc237-a0d0-4d1e-baa3-9cba0e9b4401] 事务连接已关闭或无效，无法提交			商品分割单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:37:20	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-411e8968-a1d2-46a2-b86e-8c5e2f15b915] 事务连接已关闭或无效，无法提交			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:37:26	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-f95b7f5a-99a6-4a13-a0e2-95c1d605be1e] 事务连接已关闭或无效，无法提交			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:38:04	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-24f7c351-29bc-4bec-afb4-49719fac91f9] 事务连接已关闭或无效，无法提交			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:38:07	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-91c78481-546c-4bdc-8600-002475044b07] 事务连接已关闭或无效，无法提交			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:38:28	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-b1011007-0c87-4308-9e6e-2512cf798586] 事务连接已关闭或无效，无法提交			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:38:30	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-57aa9117-8da9-49d2-afb0-5b34453aa345] 事务连接已关闭或无效，无法提交			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:38:39	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-b2db4283-8f0e-4eed-9024-204db2181a22] 事务连接已关闭或无效，跳过回滚			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:38:39	ERROR	RUINORERP.UI.MainForm	销售出库单:SOD7EA4D0993审核失败销售订单:SO2604111780的【鲨鱼鳍\HF-AHD-3030J-LSC2336,IP67防水,本色,角度可调，带1.5米扁平线】的出库数量不能大于订单中对应行的数量，
" 或存在当前销售订单重复录入了销售出库单，审核失败！			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:38:58	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043488859383271424, 原因: 单据未被锁定或不存在			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	删除	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:39:13	WARN	RUINORERP.UI.Network.Services.FileBusinessService	业务ID无效: 0, 删除操作可能无法正确执行			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	删除	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:39:28	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-3fb9203d-f59b-4ab6-ab53-e2960abe4231] 事务连接已关闭或无效，跳过回滚			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:39:28	ERROR	RUINORERP.UI.MainForm	销售出库单:SOD7EA4D0954审核失败sku:SK7E9B112367库存为：0，拟销售量为：1
 系统设置不允许负库存， 请检查出库数量与库存相关数据			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:40:37	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043487538668244992, 原因: 单据未被锁定或不存在			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	新增	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:41:00	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-f3977fd8-8895-4a9f-8b6b-e385583e9c01] 事务连接已关闭或无效，无法提交			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:41:02	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-fc9d3298-5174-48b0-8140-b7286302a613] 事务连接已关闭或无效，无法提交			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:41:09	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043488958503063552, 原因: 单据未被锁定或不存在			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	新增	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 8:41:10	ERROR	RUINORERP.Server.Network.Services.FileStorageMonitorService			系统服务						WATSON
	龙成华	2026-04-13 8:41:21	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-8c76d552-9717-4a55-89b0-0100fa96932c] 事务连接已关闭或无效，无法提交			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:41:26	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-e185f4e6-a347-4b8b-bd07-6ff060965f24] 事务连接已关闭或无效，无法提交			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:41:36	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-7bc30e22-6212-454f-8f25-5080b80f29a7] 事务连接已关闭或无效，无法提交			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:41:55	WARN	RUINORERP.UI.Network.Services.FileBusinessService	业务ID无效: 0, 删除操作可能无法正确执行			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	删除	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:43:12	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-90a0433f-be85-4bef-9ef5-4c9fa20fa511] 事务连接已关闭或无效，无法提交			生产计划单	RUINORERP.UI.MRP.MP.UCProductionPlan	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:43:16	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-a064702a-dd0a-46b9-b3ae-49f7b3401de7] 事务连接已关闭或无效，无法提交			生产计划单	RUINORERP.UI.MRP.MP.UCProductionPlan	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:43:44	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-245dd60e-c888-46ed-adc4-c3e55bdceb1e] 事务连接已关闭或无效，无法提交			生产需求分析	RUINORERP.UI.MRP.MP.UCProduceRequirement	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:43:46	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-c4526486-8187-4453-88d1-638c60561f36] 事务连接已关闭或无效，无法提交			生产需求分析	RUINORERP.UI.MRP.MP.UCProduceRequirement	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:44:25	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-c47730c7-ad54-4ab0-ad00-289027f8dd4e] 事务连接已关闭或无效，无法提交			制令单	RUINORERP.UI.MRP.MP.UCManufacturingOrder	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:44:27	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-0862cf2a-3785-4968-b38b-a227bda738a3] 事务连接已关闭或无效，无法提交			制令单	RUINORERP.UI.MRP.MP.UCManufacturingOrder	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:45:01	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-e1feaa48-4dd2-4661-8cd4-252db7962085] 事务连接已关闭或无效，无法提交			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:45:06	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-be1fb7c1-e72c-4241-8031-efd2ae7c6442] 事务连接已关闭或无效，无法提交			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:45:28	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-8288df83-9000-4613-ac20-549fd8045dcc] 事务连接已关闭或无效，无法提交			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:45:30	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-1efd2a71-770d-4e58-a151-8d0b53c39b92] 事务连接已关闭或无效，无法提交			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:46:04	WARN	RUINORERP.UI.Network.Services.FileBusinessService	业务ID无效: 0, 删除操作可能无法正确执行			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	删除	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:46:18	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-84cabd07-0682-40f2-8c5f-4d72bb4187cd] 事务连接已关闭或无效，无法提交			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:46:42	WARN	RUINORERP.UI.Network.Services.FileBusinessService	业务ID无效: 0, 删除操作可能无法正确执行			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	删除	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:49:47	WARN	RUINORERP.UI.Network.Services.FileBusinessService	业务ID无效: 0, 删除操作可能无法正确执行			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	删除	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:50:35	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-6a1d08b4-81b7-4b1d-ad9d-26e4fd421f52] 事务连接已关闭或无效，无法提交			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:50:37	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-b2bb937d-360e-447a-bebb-cf7f2b5b1724] 事务连接已关闭或无效，无法提交			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:50:51	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-75d7a1d1-4b04-4816-ae12-7c4c522d9537] 事务连接已关闭或无效，无法提交			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:50:56	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-c4c29ec3-1a26-49ae-ac4f-6e50e477ac12] 事务连接已关闭或无效，无法提交			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:50:58	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-fb77fc67-a80d-4173-ac6e-8bc54bc0005a] 事务连接已关闭或无效，跳过回滚			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:50:58	ERROR	RUINORERP.UI.MainForm	缴库单:PR260413256审核失败制令单:MO26040204的【Y9--carplay盒子Y9--carplay盒子】的缴库数不能大于制令单中发出物料能生产的最小数量。
SKU7E881D4640:TYPE-C转接头实发数量不够生产3.000			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:51:15	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043492074103377920, 原因: 单据未被锁定或不存在			缴库单	RUINORERP.UI.PSI.PUR.UCFinishedGoodsInv	删除	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:51:18	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043492006675746816, 原因: 单据未被锁定或不存在			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	删除	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:51:22	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-b2f88502-cd65-4cbd-b088-506976e50edd] 事务连接已关闭或无效，无法提交			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:52:40	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-ea578f37-37a1-4081-94d1-7bb656f62af1] 事务连接已关闭或无效，无法提交			产品转换单	RUINORERP.UI.PSI.INV.UCProdConversion	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:52:43	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-a6dc159c-271d-41e1-99ef-2d3070a27b84] 事务连接已关闭或无效，无法提交			产品转换单	RUINORERP.UI.PSI.INV.UCProdConversion	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:52:50	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-49251b27-87f0-4795-b656-17f9dd26f328] 事务连接已关闭或无效，无法提交			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:52:50	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-13b9543e-4ab9-4139-bcdf-d20c272c7d0d] 事务连接已关闭或无效，跳过回滚			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:52:50	WARN	RUINORERP.Business.tb_SaleOutController	销售出库单【SOD7EA4D1008】自动审核失败:销售订单:SO2604111764的【CD05--摄像头SH-X35，2.5MM耳机头，5.5M高清后拉线+0.5M摄像头线】的出库数量不能大于订单中对应行的数量，
" 或存在当前销售订单重复录入了销售出库单，审核失败！			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:53:07	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-bdad04e5-6e0b-4089-99f6-27b8541a8cb1] 事务连接已关闭或无效，跳过回滚			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:53:08	ERROR	RUINORERP.UI.MainForm	销售出库单:SOD7EA4D1008审核失败sku:SK7E94710975库存为：0，拟销售量为：1
 系统设置不允许负库存， 请检查出库数量与库存相关数据			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:53:51	WARN	RUINORERP.UI.Network.Services.FileBusinessService	业务ID无效: 0, 删除操作可能无法正确执行			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	删除	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:54:32	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043492531945213952, 原因: 单据未被锁定或不存在			产品转换单	RUINORERP.UI.PSI.INV.UCProdConversion	新增	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:54:49	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-51234fad-35fb-4803-a549-e4d84dce062c] 事务连接已关闭或无效，无法提交			产品转换单	RUINORERP.UI.PSI.INV.UCProdConversion	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:54:52	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-34c6e549-77d2-4540-ac1f-d41cb6bbf916] 事务连接已关闭或无效，无法提交			产品转换单	RUINORERP.UI.PSI.INV.UCProdConversion	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:55:07	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-b8e5bffb-a64a-4e44-a298-527870ef865d] 事务连接已关闭或无效，无法提交			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:55:20	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043493074268721152, 原因: 单据未被锁定或不存在			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:57:15	WARN	RUINORERP.UI.Network.Services.FileBusinessService	业务ID无效: 0, 删除操作可能无法正确执行			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	删除	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 8:57:26	WARN	RUINORERP.UI.MainForm	欢迎流程验证超时,但连接已建立						192.168.0.42
		DESKTOP-DUFLFQ6
	龙成华	2026-04-13 8:57:31	WARN	RUINORERP.UI.Network.Services.FileBusinessService	业务ID无效: 0, 删除操作可能无法正确执行			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	删除	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	陈晓荣	2026-04-13 8:57:40	ERROR	RUINORERP.UI.MainForm	PUR采购工作台数据查询出错	类型: System.InvalidOperationException
消息: 阅读器关闭时尝试调用 FieldCount 无效。
堆栈:    在 System.Data.SqlClient.SqlDataReader.get_FieldCount()
   在 SqlSugar.DbBindAccessory.GetDataReaderNames(IDataReader dataReader, String& types)
   在 SqlSugar.DbBindAccessory.<GetEntityListAsync>d__5`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 SqlSugar.DbBindProvider.<DataReaderToListAsync>d__12`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 SqlSugar.QueryableProvider`1.<GetDataAsync>d__261`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 SqlSugar.QueryableProvider`1.<GetDataAsync>d__259`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 SqlSugar.QueryableProvider`1.<_ToListAsync>d__256`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.UserCenter.DataParts.UCPUR.<QueryData>d__6.MoveNext()
		工作台			192.168.0.42
	D8-43-AE-57-D8-A7	-DESKTOP-DUFLFQ6-pc
	龙成华	2026-04-13 8:58:23	WARN	RUINORERP.UI.Network.Services.FileBusinessService	业务ID无效: 0, 删除操作可能无法正确执行			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	删除	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 8:58:47	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-eed5b493-f404-4f0e-8559-5c9fee504c2e] 事务连接已关闭或无效，无法提交			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:00:26	WARN	RUINORERP.UI.Network.Services.FileBusinessService	业务ID无效: 0, 删除操作可能无法正确执行			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	删除	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:34	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-61384a29-b5c6-4748-af8f-71e79f5316f9] 事务连接已关闭或无效，无法提交			商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 9:01:22	ERROR	IConnection		类型: System.Exception
消息: Package cannot be larger than 1048576.
堆栈: 
	系统服务						WIN-FL1VGS1FS2F
	龙成华	2026-04-13 9:01:36	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:36	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:37	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0802:CheckLockStatus, 未连接到服务器	类型: System.InvalidOperationException
消息: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.SuperSocketClient.<SendAsync>d__45.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
		商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:37	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0802:CheckLockStatus, RequestId=CheckLockStatus_090137457_01KP25QESH6H19GNCRJ7KMN62Z, RetryCount=0, Error=请求处理失败，指令类型：0x0802:CheckLockStatus，请求ID: CheckLockStatus_090137457_01KP25QESH6H19GNCRJ7KMN62Z: 发送请求失败: 未连接到服务器	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0802:CheckLockStatus，请求ID: CheckLockStatus_090137457_01KP25QESH6H19GNCRJ7KMN62Z: 发送请求失败: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.InvalidOperationException
消息: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.SuperSocketClient.<SendAsync>d__45.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
		商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:37	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=CheckLockStatus, RequestId=CheckLockStatus_090137457_01KP25QESH6H19GNCRJ7KMN62Z, 错误信息=请求处理失败，指令类型：0x0802:CheckLockStatus，请求ID: CheckLockStatus_090137457_01KP25QESH6H19GNCRJ7KMN62Z: 发送请求失败: 未连接到服务器	类型: System.Exception
消息: 带响应命令发送失败: CommandId=CheckLockStatus, RequestId=CheckLockStatus_090137457_01KP25QESH6H19GNCRJ7KMN62Z, 错误信息=请求处理失败，指令类型：0x0802:CheckLockStatus，请求ID: CheckLockStatus_090137457_01KP25QESH6H19GNCRJ7KMN62Z: 发送请求失败: 未连接到服务器
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0802:CheckLockStatus，请求ID: CheckLockStatus_090137457_01KP25QESH6H19GNCRJ7KMN62Z: 发送请求失败: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.InvalidOperationException
消息: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.SuperSocketClient.<SendAsync>d__45.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
		商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:37	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0802:CheckLockStatus, 未连接到服务器	类型: System.InvalidOperationException
消息: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.SuperSocketClient.<SendAsync>d__45.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
		商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:37	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0802:CheckLockStatus, RequestId=CheckLockStatus_090137505_01KP25QEV1TH0C2D2YXZ4ZVCV9, RetryCount=0, Error=请求处理失败，指令类型：0x0802:CheckLockStatus，请求ID: CheckLockStatus_090137505_01KP25QEV1TH0C2D2YXZ4ZVCV9: 发送请求失败: 未连接到服务器	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0802:CheckLockStatus，请求ID: CheckLockStatus_090137505_01KP25QEV1TH0C2D2YXZ4ZVCV9: 发送请求失败: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.InvalidOperationException
消息: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.SuperSocketClient.<SendAsync>d__45.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
		商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:37	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=CheckLockStatus, RequestId=CheckLockStatus_090137505_01KP25QEV1TH0C2D2YXZ4ZVCV9, 错误信息=请求处理失败，指令类型：0x0802:CheckLockStatus，请求ID: CheckLockStatus_090137505_01KP25QEV1TH0C2D2YXZ4ZVCV9: 发送请求失败: 未连接到服务器	类型: System.Exception
消息: 带响应命令发送失败: CommandId=CheckLockStatus, RequestId=CheckLockStatus_090137505_01KP25QEV1TH0C2D2YXZ4ZVCV9, 错误信息=请求处理失败，指令类型：0x0802:CheckLockStatus，请求ID: CheckLockStatus_090137505_01KP25QEV1TH0C2D2YXZ4ZVCV9: 发送请求失败: 未连接到服务器
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0802:CheckLockStatus，请求ID: CheckLockStatus_090137505_01KP25QEV1TH0C2D2YXZ4ZVCV9: 发送请求失败: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.InvalidOperationException
消息: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.SuperSocketClient.<SendAsync>d__45.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
		商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:37	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0802:CheckLockStatus, 未连接到服务器	类型: System.InvalidOperationException
消息: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.SuperSocketClient.<SendAsync>d__45.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
		商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:37	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0802:CheckLockStatus, RequestId=CheckLockStatus_090137546_01KP25QEWADDV4NV790EQA7NXT, RetryCount=0, Error=请求处理失败，指令类型：0x0802:CheckLockStatus，请求ID: CheckLockStatus_090137546_01KP25QEWADDV4NV790EQA7NXT: 发送请求失败: 未连接到服务器	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0802:CheckLockStatus，请求ID: CheckLockStatus_090137546_01KP25QEWADDV4NV790EQA7NXT: 发送请求失败: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.InvalidOperationException
消息: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.SuperSocketClient.<SendAsync>d__45.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
		商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:37	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=CheckLockStatus, RequestId=CheckLockStatus_090137546_01KP25QEWADDV4NV790EQA7NXT, 错误信息=请求处理失败，指令类型：0x0802:CheckLockStatus，请求ID: CheckLockStatus_090137546_01KP25QEWADDV4NV790EQA7NXT: 发送请求失败: 未连接到服务器	类型: System.Exception
消息: 带响应命令发送失败: CommandId=CheckLockStatus, RequestId=CheckLockStatus_090137546_01KP25QEWADDV4NV790EQA7NXT, 错误信息=请求处理失败，指令类型：0x0802:CheckLockStatus，请求ID: CheckLockStatus_090137546_01KP25QEWADDV4NV790EQA7NXT: 发送请求失败: 未连接到服务器
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0802:CheckLockStatus，请求ID: CheckLockStatus_090137546_01KP25QEWADDV4NV790EQA7NXT: 发送请求失败: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.InvalidOperationException
消息: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.SuperSocketClient.<SendAsync>d__45.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
		商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:37	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0802:CheckLockStatus, 未连接到服务器	类型: System.InvalidOperationException
消息: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.SuperSocketClient.<SendAsync>d__45.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
		商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:37	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0802:CheckLockStatus, RequestId=CheckLockStatus_090137593_01KP25QEXSNS32CWHEE3DKE9K6, RetryCount=0, Error=请求处理失败，指令类型：0x0802:CheckLockStatus，请求ID: CheckLockStatus_090137593_01KP25QEXSNS32CWHEE3DKE9K6: 发送请求失败: 未连接到服务器	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0802:CheckLockStatus，请求ID: CheckLockStatus_090137593_01KP25QEXSNS32CWHEE3DKE9K6: 发送请求失败: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.InvalidOperationException
消息: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.SuperSocketClient.<SendAsync>d__45.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
		商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:37	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=CheckLockStatus, RequestId=CheckLockStatus_090137593_01KP25QEXSNS32CWHEE3DKE9K6, 错误信息=请求处理失败，指令类型：0x0802:CheckLockStatus，请求ID: CheckLockStatus_090137593_01KP25QEXSNS32CWHEE3DKE9K6: 发送请求失败: 未连接到服务器	类型: System.Exception
消息: 带响应命令发送失败: CommandId=CheckLockStatus, RequestId=CheckLockStatus_090137593_01KP25QEXSNS32CWHEE3DKE9K6, 错误信息=请求处理失败，指令类型：0x0802:CheckLockStatus，请求ID: CheckLockStatus_090137593_01KP25QEXSNS32CWHEE3DKE9K6: 发送请求失败: 未连接到服务器
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0802:CheckLockStatus，请求ID: CheckLockStatus_090137593_01KP25QEXSNS32CWHEE3DKE9K6: 发送请求失败: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.InvalidOperationException
消息: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.SuperSocketClient.<SendAsync>d__45.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
		商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:37	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:37	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0800:Lock, 未连接到服务器	类型: System.InvalidOperationException
消息: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.SuperSocketClient.<SendAsync>d__45.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
		商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:37	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0800:Lock, RequestId=Lock_090137633_01KP25QEZ1JVN4DJAKTRJ53QQP, RetryCount=0, Error=请求处理失败，指令类型：0x0800:Lock，请求ID: Lock_090137633_01KP25QEZ1JVN4DJAKTRJ53QQP: 发送请求失败: 未连接到服务器	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0800:Lock，请求ID: Lock_090137633_01KP25QEZ1JVN4DJAKTRJ53QQP: 发送请求失败: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.InvalidOperationException
消息: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.SuperSocketClient.<SendAsync>d__45.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
		商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:37	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=Lock, RequestId=Lock_090137633_01KP25QEZ1JVN4DJAKTRJ53QQP, 错误信息=请求处理失败，指令类型：0x0800:Lock，请求ID: Lock_090137633_01KP25QEZ1JVN4DJAKTRJ53QQP: 发送请求失败: 未连接到服务器	类型: System.Exception
消息: 带响应命令发送失败: CommandId=Lock, RequestId=Lock_090137633_01KP25QEZ1JVN4DJAKTRJ53QQP, 错误信息=请求处理失败，指令类型：0x0800:Lock，请求ID: Lock_090137633_01KP25QEZ1JVN4DJAKTRJ53QQP: 发送请求失败: 未连接到服务器
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0800:Lock，请求ID: Lock_090137633_01KP25QEZ1JVN4DJAKTRJ53QQP: 发送请求失败: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.InvalidOperationException
消息: 未连接到服务器
堆栈:    在 RUINORERP.UI.Network.SuperSocketClient.<SendAsync>d__45.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
		商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:37	ERROR	RUINORERP.UI.MainForm	单据 2043494771766464512 锁定失败：响应为空			商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:37	ERROR	RUINORERP.Business.BaseController	单据 2043494771766464512 锁定失败：响应为空			商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:38	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-e9873751-434e-4025-974d-8c8c9ed4cd79] 事务连接已关闭或无效，无法提交			商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 9:01:25	ERROR	IConnection		类型: System.Exception
消息: Package cannot be larger than 1048576.
堆栈: 
	系统服务						WIN-FL1VGS1FS2F
	龙成华	2026-04-13 9:01:39	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:39	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:40	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			商品组合单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:01:43	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-0e90f79a-5049-403f-b7fa-74c0849f1b0c] 事务连接已关闭或无效，无法提交			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:02:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x030A:SendTodoNotification, RequestId=SendTodoNotification_090136435_01KP25QDSKJKKG4V38ZKCE98XX, RetryCount=0, Error=请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090136435_01KP25QDSKJKKG4V38ZKCE98XX	类型: System.TimeoutException
消息: 请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090136435_01KP25QDSKJKKG4V38ZKCE98XX
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()
		销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	打印	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:02:06	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=SendTodoNotification, RequestId=SendTodoNotification_090136435_01KP25QDSKJKKG4V38ZKCE98XX, 错误信息=请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090136435_01KP25QDSKJKKG4V38ZKCE98XX	类型: System.Exception
消息: 带响应命令发送失败: CommandId=SendTodoNotification, RequestId=SendTodoNotification_090136435_01KP25QDSKJKKG4V38ZKCE98XX, 错误信息=请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090136435_01KP25QDSKJKKG4V38ZKCE98XX
堆栈: 

--- InnerException ---
类型: System.TimeoutException
消息: 请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090136435_01KP25QDSKJKKG4V38ZKCE98XX
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()
		销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	打印	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 9:01:55	ERROR	IConnection		类型: System.Exception
消息: Package cannot be larger than 1048576.
堆栈: 
	系统服务						WIN-FL1VGS1FS2F
	龙成华	2026-04-13 9:02:08	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	打印	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:02:08	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	打印	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:02:08	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x030A:SendTodoNotification, RequestId=SendTodoNotification_090138874_01KP25QG5TYERNEG1EZMAQS124, RetryCount=0, Error=请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090138874_01KP25QG5TYERNEG1EZMAQS124	类型: System.TimeoutException
消息: 请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090138874_01KP25QG5TYERNEG1EZMAQS124
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()
		销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	打印	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:02:08	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=SendTodoNotification, RequestId=SendTodoNotification_090138874_01KP25QG5TYERNEG1EZMAQS124, 错误信息=请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090138874_01KP25QG5TYERNEG1EZMAQS124	类型: System.Exception
消息: 带响应命令发送失败: CommandId=SendTodoNotification, RequestId=SendTodoNotification_090138874_01KP25QG5TYERNEG1EZMAQS124, 错误信息=请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090138874_01KP25QG5TYERNEG1EZMAQS124
堆栈: 

--- InnerException ---
类型: System.TimeoutException
消息: 请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090138874_01KP25QG5TYERNEG1EZMAQS124
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()
		销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	打印	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:02:09	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	打印	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 9:01:56	ERROR	IConnection		类型: System.Exception
消息: Package cannot be larger than 1048576.
堆栈: 
	系统服务						WIN-FL1VGS1FS2F
	龙成华	2026-04-13 9:02:10	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	打印	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:02:10	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	打印	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:02:10	WARN	RUINORERP.UI.Network.ClientCommandHandlers.WelcomeCommandHandler	欢迎响应发送失败			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 9:02:24	ERROR	RUINORERP.Extensions.SqlsugarSetup	SQL 执行错误：中文提示 :  连接数据库过程中发生错误，检查服务器是否正常连接字符串是否正确，错误信息：连接未关闭。 连接的当前状态为正在连接。DbType="SqlServer";ConfigId="".
English Message : Connection open error . 连接未关闭。 连接的当前状态为正在连接。DbType="SqlServer";ConfigId="" , SQL: SELECT [PolicyId],[PolicyName],[TargetTable],[TargetEntity],[IsJoinRequired],[TargetTableJoinField],[JoinTableJoinField],[JoinTable],[JoinType],[JoinOnClause],[FilterClause],[IsParameterized],[ParameterizedFilterClause],[EntityType],[IsEnabled],[PolicyDescription],[Created_at],[Created_by],[Modified_at],[Modified_by],[DefaultRuleEnum] FROM [tb_RowAuthPolicy] WITH(NOLOCK)   WHERE ( [IsEnabled]=1 )		系统服务				192.168.0.25
		PURE-20241005RV
		2026-04-13 9:02:24	WARN	RUINORERP.Business.RowLevelAuthService.RowAuthPolicyLoaderService	加载行级权限策略失败，但不影响系统运行。权限过滤将降级为默认行为	类型: SqlSugar.SqlSugarException
消息: 中文提示 :  连接数据库过程中发生错误，检查服务器是否正常连接字符串是否正确，错误信息：连接未关闭。 连接的当前状态为正在连接。DbType="SqlServer";ConfigId="".
English Message : Connection open error . 连接未关闭。 连接的当前状态为正在连接。DbType="SqlServer";ConfigId="" 
堆栈:    在 SqlSugar.Check.Exception(Boolean isException, String message, String[] args)
   在 SqlSugar.AdoProvider.CheckConnection()
   在 SqlSugar.SqlServerProvider.GetCommand(String sql, SugarParameter[] parameters)
   在 SqlSugar.AdoProvider.<GetDataReaderAsync>d__128.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 SqlSugar.QueryableProvider`1.<GetDataAsync>d__259`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 SqlSugar.QueryableProvider`1.<_ToListAsync>d__256`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.Business.RowLevelAuthService.RowAuthPolicyLoaderService.<LoadAllPoliciesAsync>d__9.MoveNext()
	系统服务				192.168.0.25
		PURE-20241005RV
	龙成华	2026-04-13 9:02:38	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x030A:SendTodoNotification, RequestId=SendTodoNotification_090208664_01KP25RD8RV9HHTXGT0NW63RXD, RetryCount=1, Error=请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090208664_01KP25RD8RV9HHTXGT0NW63RXD	类型: System.TimeoutException
消息: 请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090208664_01KP25RD8RV9HHTXGT0NW63RXD
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()
		销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:02:38	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=SendTodoNotification, RequestId=SendTodoNotification_090208664_01KP25RD8RV9HHTXGT0NW63RXD, 错误信息=请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090208664_01KP25RD8RV9HHTXGT0NW63RXD	类型: System.Exception
消息: 带响应命令发送失败: CommandId=SendTodoNotification, RequestId=SendTodoNotification_090208664_01KP25RD8RV9HHTXGT0NW63RXD, 错误信息=请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090208664_01KP25RD8RV9HHTXGT0NW63RXD
堆栈: 

--- InnerException ---
类型: System.TimeoutException
消息: 请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090208664_01KP25RD8RV9HHTXGT0NW63RXD
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()
		销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:02:40	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x030A:SendTodoNotification, RequestId=SendTodoNotification_090210494_01KP25RF1Y45M1V44JR18ZVDK2, RetryCount=1, Error=请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090210494_01KP25RF1Y45M1V44JR18ZVDK2	类型: System.TimeoutException
消息: 请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090210494_01KP25RF1Y45M1V44JR18ZVDK2
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()
		销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:02:40	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=SendTodoNotification, RequestId=SendTodoNotification_090210494_01KP25RF1Y45M1V44JR18ZVDK2, 错误信息=请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090210494_01KP25RF1Y45M1V44JR18ZVDK2	类型: System.Exception
消息: 带响应命令发送失败: CommandId=SendTodoNotification, RequestId=SendTodoNotification_090210494_01KP25RF1Y45M1V44JR18ZVDK2, 错误信息=请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090210494_01KP25RF1Y45M1V44JR18ZVDK2
堆栈: 

--- InnerException ---
类型: System.TimeoutException
消息: 请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090210494_01KP25RF1Y45M1V44JR18ZVDK2
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()
		销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	凌圳华	2026-04-13 9:02:41	WARN	RUINORERP.UI.MainForm	欢迎流程验证超时,但连接已建立						192.168.0.25
	C8-1F-66-31-BE-45	-PURE-20241005RV-Administrator
		2026-04-13 9:02:29	ERROR	IConnection		类型: System.Exception
消息: Package cannot be larger than 1048576.
堆栈: 
	系统服务						WIN-FL1VGS1FS2F
	龙成华	2026-04-13 9:02:43	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:02:43	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:02:44	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 9:02:32	ERROR	IConnection		类型: System.Exception
消息: Package cannot be larger than 1048576.
堆栈: 
	系统服务						WIN-FL1VGS1FS2F
	龙成华	2026-04-13 9:02:46	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:02:46	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:03:13	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x030A:SendTodoNotification, RequestId=SendTodoNotification_090243063_01KP25SEVQT9SHZ2E8RW6VNSEK, RetryCount=2, Error=请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090243063_01KP25SEVQT9SHZ2E8RW6VNSEK	类型: System.TimeoutException
消息: 请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090243063_01KP25SEVQT9SHZ2E8RW6VNSEK
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()
		销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:03:13	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=SendTodoNotification, RequestId=SendTodoNotification_090243063_01KP25SEVQT9SHZ2E8RW6VNSEK, 错误信息=请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090243063_01KP25SEVQT9SHZ2E8RW6VNSEK	类型: System.Exception
消息: 带响应命令发送失败: CommandId=SendTodoNotification, RequestId=SendTodoNotification_090243063_01KP25SEVQT9SHZ2E8RW6VNSEK, 错误信息=请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090243063_01KP25SEVQT9SHZ2E8RW6VNSEK
堆栈: 

--- InnerException ---
类型: System.TimeoutException
消息: 请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090243063_01KP25SEVQT9SHZ2E8RW6VNSEK
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()
		销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:03:13	WARN	RUINORERP.UI.MainForm	发送任务状态通知失败,响应为空RUINORERP.PacketSpec.Models.Message.MessageData			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:03:14	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x030A:SendTodoNotification, RequestId=SendTodoNotification_090244575_01KP25SGAZKK05QC3GAYKE8RNN, RetryCount=2, Error=请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090244575_01KP25SGAZKK05QC3GAYKE8RNN	类型: System.TimeoutException
消息: 请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090244575_01KP25SGAZKK05QC3GAYKE8RNN
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()
		销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:03:14	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=SendTodoNotification, RequestId=SendTodoNotification_090244575_01KP25SGAZKK05QC3GAYKE8RNN, 错误信息=请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090244575_01KP25SGAZKK05QC3GAYKE8RNN	类型: System.Exception
消息: 带响应命令发送失败: CommandId=SendTodoNotification, RequestId=SendTodoNotification_090244575_01KP25SGAZKK05QC3GAYKE8RNN, 错误信息=请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090244575_01KP25SGAZKK05QC3GAYKE8RNN
堆栈: 

--- InnerException ---
类型: System.TimeoutException
消息: 请求超时（30000ms），指令类型：0x030A:SendTodoNotification，请求ID: SendTodoNotification_090244575_01KP25SGAZKK05QC3GAYKE8RNN
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()
		销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:03:14	WARN	RUINORERP.UI.MainForm	发送任务状态通知失败,响应为空RUINORERP.PacketSpec.Models.Message.MessageData			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:04:07	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:04:07	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:04:07	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:04:07	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:04:08	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:05:28	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:05:28	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:05:28	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:05:28	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:05:29	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:06:49	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:06:49	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:06:49	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:06:49	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:06:50	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:08:11	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:08:11	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:08:11	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:08:11	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:08:12	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:09:32	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:09:32	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:09:32	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:09:32	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:09:33	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:10:53	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:10:53	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:10:53	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:10:53	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:10:54	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 9:11:10	ERROR	RUINORERP.Server.Network.Services.FileStorageMonitorService			系统服务						WATSON
	龙成华	2026-04-13 9:12:14	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:12:14	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:12:14	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:12:14	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:12:15	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:13:38	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:13:38	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:13:38	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:13:38	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:13:39	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:14:59	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:14:59	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:14:59	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:14:59	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:15:00	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:16:20	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:16:20	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:16:20	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:16:20	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:16:21	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:17:41	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:17:41	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:17:41	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:17:41	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:17:42	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 9:18:11	WARN	RUINORERP.UI.MainForm	欢迎流程验证超时,但连接已建立						192.168.0.47
		MS-VVOVTLDQGMIB
	黄时富	2026-04-13 9:18:35	ERROR	RUINORERP.Extensions.SqlsugarSetup	SQL 执行错误：无效操作。连接被关闭。, SQL: UPDATE [tb_UserPersonalized]  SET
             [UserFavoriteMenu] = '[{"MenuId":1914258141663596544,"Frequency":1,"LastClickTime":"2025-08-21T10:30:43.8675085+08:00"},{"MenuId":1914258234185748480,"Frequency":1,"LastClickTime":"2026-03-28T11:17:04.4785607+08:00"},{"MenuId":1740601448724566016,"Frequency":70,"LastClickTime":"2026-04-09T16:26:43.0608838+08:00"},{"MenuId":1782569790846668800,"Frequency":18,"LastClickTime":"2026-02-25T10:26:28.4439026+08:00"},{"MenuId":1876158831579500544,"Frequency":1,"LastClickTime":"2025-08-14T14:14:36.7834834+08:00"},{"MenuId":1866114618347360256,"Frequency":2,"LastClickTime":"2026-04-11T13:28:43.6396569+08:00"},{"MenuId":1740601462322499584,"Frequency":42,"LastClickTime":"2026-03-25T09:23:55.7350931+08:00"},{"MenuId":1866131076863365120,"Frequency":1,"LastClickTime":"2026-01-19T17:48:29.4339905+08:00"},{"MenuId":1740601439438376960,"Frequency":834,"LastClickTime":"2026-04-10T13:10:45.0297969+08:00"},{"MenuId":1815282988863328256,"Frequency":5,"LastClickTime":"2025-08-21T15:05:14.221687+08:00"},{"MenuId":1914518614514470912,"Frequency":49,"LastClickTime":"2026-03-28T11:16:31.5427319+08:00"},{"MenuId":1748360871442255872,"Frequency":4,"LastClickTime":"2026-04-09T14:01:50.9612666+08:00"},{"MenuId":1745140988508246016,"Frequency":13,"LastClickTime":"2026-04-09T13:57:10.8175139+08:00"},{"MenuId":1866044630219493376,"Frequency":3,"LastClickTime":"2026-01-19T17:34:29.4322811+08:00"},{"MenuId":1749374743036956672,"Frequency":10,"LastClickTime":"2026-03-30T14:05:37.0759719+08:00"},{"MenuId":1740601429267189760,"Frequency":52,"LastClickTime":"2026-03-17T16:55:41.5003356+08:00"},{"MenuId":1914518505424818176,"Frequency":1,"LastClickTime":"2026-03-28T11:16:17.3243772+08:00"},{"MenuId":1815645753721360384,"Frequency":2,"LastClickTime":"2025-08-21T15:04:51.3022318+08:00"},{"MenuId":1913223393675710464,"Frequency":1,"LastClickTime":"2026-01-23T18:05:34.5905689+08:00"},{"MenuId":1769751574357348352,"Frequency":11,"LastClickTime":"2026-02-03T10:30:18.9202539+08:00"},{"MenuId":1914518561066455040,"Frequency":18,"LastClickTime":"2026-03-28T11:17:53.2697594+08:00"},{"MenuId":1782569786539118592,"Frequency":1,"LastClickTime":"2025-10-30T13:52:49.3159565+08:00"},{"MenuId":1865983270806753280,"Frequency":2,"LastClickTime":"2026-04-11T13:28:52.2027797+08:00"},{"MenuId":1866032293034987520,"Frequency":1,"LastClickTime":"2026-01-19T17:34:51.3830175+08:00"},{"MenuId":1780085880032202752,"Frequency":1,"LastClickTime":"2026-04-03T11:18:59.8614932+08:00"},{"MenuId":1914518420326584320,"Frequency":2,"LastClickTime":"2026-03-28T11:16:56.2411008+08:00"},{"MenuId":1821406487508029440,"Frequency":10,"LastClickTime":"2026-01-26T16:48:10.030252+08:00"},{"MenuId":1740601443896922112,"Frequency":17,"LastClickTime":"2026-02-25T10:26:42.17657+08:00"},{"MenuId":1947937520721465344,"Frequency":289,"LastClickTime":"2026-04-13T09:18:33.9336326+08:00"},{"MenuId":1780421532242284544,"Frequency":15,"LastClickTime":"2026-04-11T15:32:39.8524088+08:00"},{"MenuId":1749375982491537408,"Frequency":2,"LastClickTime":"2026-03-30T14:05:31.3740309+08:00"},{"MenuId":1769751463099240448,"Frequency":11,"LastClickTime":"2025-11-17T14:25:31.7431896+08:00"},{"MenuId":1740601454059720704,"Frequency":288,"LastClickTime":"2026-04-10T13:10:27.3462087+08:00"},{"MenuId":1859509347760082944,"Frequency":2,"LastClickTime":"2025-10-30T13:51:03.4324175+08:00"},{"MenuId":1748351955329224704,"Frequency":3,"LastClickTime":"2026-04-09T14:02:07.1816526+08:00"},{"MenuId":1947937494687420416,"Frequency":188,"LastClickTime":"2026-04-09T16:26:45.6979189+08:00"},{"MenuId":1772144307243978752,"Frequency":1,"LastClickTime":"2025-11-10T18:07:30.6789724+08:00"},{"MenuId":1913189554274308096,"Frequency":6,"LastClickTime":"2025-11-29T17:07:25.7126805+08:00"}]'    WHERE ( [UserPersonalizedID] = 1867489666220036096 )			蓄水查询	RUINORERP.UI.EOP.UCEOPWaterStorageQuery	RUINORERP.UI.EOP.UCEOPWaterStorageQuery	192.168.0.47
	40-16-7E-E7-AB-C3	-MS-VVOVTLDQGMIB-Administrator
	黄时富	2026-04-13 9:18:36	ERROR	RUINORERP.UI.MainForm	保存用户菜单偏好时出错: 无效操作。连接被关闭。			蓄水查询	RUINORERP.UI.EOP.UCEOPWaterStorageQuery	RUINORERP.UI.EOP.UCEOPWaterStorageQuery	192.168.0.47
	40-16-7E-E7-AB-C3	-MS-VVOVTLDQGMIB-Administrator
	龙成华	2026-04-13 9:19:02	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:19:02	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:19:02	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:19:02	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:19:03	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:20:24	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:20:24	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:20:24	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:20:24	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:20:25	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:21:45	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:21:45	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:21:45	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:21:45	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:21:46	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:23:08	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:23:08	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:23:08	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:23:08	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:23:09	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	李懋	2026-04-13 9:24:06	WARN	RUINORERP.UI.MainForm	欢迎流程验证超时,但连接已建立						192.168.0.152
	98-90-96-CC-A2-46	-WIN-20260116PLU-Administrator
	龙成华	2026-04-13 9:24:29	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:24:29	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:24:29	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:24:29	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:24:30	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:25:50	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:25:50	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:25:50	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:25:50	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:25:51	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:27:12	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:27:12	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:27:12	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:27:12	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:27:13	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:28:33	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:28:33	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:28:33	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:28:33	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:28:34	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:29:54	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:29:54	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:29:54	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:29:54	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:29:55	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:31:15	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:31:15	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:31:15	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:31:15	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:31:16	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	李懋	2026-04-13 9:31:18	ERROR	RUINORERP.UI.MainForm	获取到最新的验证配置LogError			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	保存	192.168.0.152
	98-90-96-CC-A2-46	-WIN-20260116PLU-Administrator
	李懋	2026-04-13 9:31:20	ERROR	RUINORERP.UI.MainForm	获取到最新的验证配置LogError			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	保存	192.168.0.152
	98-90-96-CC-A2-46	-WIN-20260116PLU-Administrator
	龙成华	2026-04-13 9:32:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:32:36	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:32:36	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:32:36	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:32:37	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:33:59	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:33:59	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:33:59	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:33:59	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:34:00	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:35:21	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:35:21	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:35:21	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:35:21	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:35:22	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:36:42	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:36:42	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:36:42	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:36:42	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:36:43	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	朱鹏飞	2026-04-13 9:37:21	WARN	RUINORERP.UI.MainForm	欢迎流程验证超时,但连接已建立						192.168.0.27
	64-00-6A-09-04-3F	-PURE-20250527UB-Administrator
	龙成华	2026-04-13 9:38:03	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:38:03	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:38:03	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:38:03	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:38:04	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:39:24	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:39:24	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:39:24	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:39:24	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:39:25	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:40:45	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:40:45	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:40:45	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:40:45	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:40:46	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 9:41:10	ERROR	RUINORERP.Server.Network.Services.FileStorageMonitorService			系统服务						WATSON
		2026-04-13 9:41:54	WARN	RUINORERP.UI.MainForm	欢迎流程验证超时,但连接已建立						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:42:03	WARN	RUINORERP.UI.MainForm			-2.0.0.5-2026/4/8 23:22:44-http://smarterp.7766.org:1688/test/updaterFiler/				192.168.0.165
		USER-20190625HT
	龙成华	2026-04-13 9:42:06	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:42:06	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:42:06	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:42:06	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:42:07	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 9:42:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:42:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:42:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094236451_01KP282G53PNWA9KYYW0W5RRE5, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094236451_01KP282G53PNWA9KYYW0W5RRE5: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094236451_01KP282G53PNWA9KYYW0W5RRE5: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:42:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094236451_01KP282G53PNWA9KYYW0W5RRE5, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094236451_01KP282G53PNWA9KYYW0W5RRE5: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094236451_01KP282G53PNWA9KYYW0W5RRE5, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094236451_01KP282G53PNWA9KYYW0W5RRE5: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094236451_01KP282G53PNWA9KYYW0W5RRE5: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:42:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:42:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:42:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:42:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094236493_01KP282G6D5B0G06SMHAAC0RCF, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094236493_01KP282G6D5B0G06SMHAAC0RCF: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094236493_01KP282G6D5B0G06SMHAAC0RCF: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:42:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094236493_01KP282G6D5B0G06SMHAAC0RCF, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094236493_01KP282G6D5B0G06SMHAAC0RCF: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094236493_01KP282G6D5B0G06SMHAAC0RCF, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094236493_01KP282G6D5B0G06SMHAAC0RCF: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094236493_01KP282G6D5B0G06SMHAAC0RCF: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:42:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:42:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:42:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:42:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094236502_01KP282G6PSP3QTY3JPK41VGZG, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094236502_01KP282G6PSP3QTY3JPK41VGZG: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094236502_01KP282G6PSP3QTY3JPK41VGZG: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:42:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094236502_01KP282G6PSP3QTY3JPK41VGZG, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094236502_01KP282G6PSP3QTY3JPK41VGZG: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094236502_01KP282G6PSP3QTY3JPK41VGZG, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094236502_01KP282G6PSP3QTY3JPK41VGZG: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094236502_01KP282G6PSP3QTY3JPK41VGZG: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:42:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
	龙成华	2026-04-13 9:43:30	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:43:30	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:43:30	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:43:30	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:43:31	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 9:43:33	WARN	RUINORERP.UI.MainForm	欢迎流程验证超时,但连接已建立						192.168.0.165
		USER-20190625HT
		2026-04-13 9:43:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:43:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:43:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094336432_01KP284AQG25Q25053VTMD401V, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094336432_01KP284AQG25Q25053VTMD401V: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094336432_01KP284AQG25Q25053VTMD401V: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:43:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094336432_01KP284AQG25Q25053VTMD401V, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094336432_01KP284AQG25Q25053VTMD401V: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094336432_01KP284AQG25Q25053VTMD401V, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094336432_01KP284AQG25Q25053VTMD401V: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094336432_01KP284AQG25Q25053VTMD401V: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:43:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:43:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:43:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:43:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094336443_01KP284AQVX8GC03ZBGZNACF91, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094336443_01KP284AQVX8GC03ZBGZNACF91: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094336443_01KP284AQVX8GC03ZBGZNACF91: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:43:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094336443_01KP284AQVX8GC03ZBGZNACF91, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094336443_01KP284AQVX8GC03ZBGZNACF91: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094336443_01KP284AQVX8GC03ZBGZNACF91, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094336443_01KP284AQVX8GC03ZBGZNACF91: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094336443_01KP284AQVX8GC03ZBGZNACF91: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:43:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:43:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:43:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:43:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094336460_01KP284ARCPZCVC7PZGMRXA437, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094336460_01KP284ARCPZCVC7PZGMRXA437: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094336460_01KP284ARCPZCVC7PZGMRXA437: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:43:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094336460_01KP284ARCPZCVC7PZGMRXA437, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094336460_01KP284ARCPZCVC7PZGMRXA437: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094336460_01KP284ARCPZCVC7PZGMRXA437, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094336460_01KP284ARCPZCVC7PZGMRXA437: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094336460_01KP284ARCPZCVC7PZGMRXA437: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:43:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:06	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094406448_01KP28581GC83KJV9ZKQ98G0TR, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094406448_01KP28581GC83KJV9ZKQ98G0TR: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094406448_01KP28581GC83KJV9ZKQ98G0TR: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:06	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094406448_01KP28581GC83KJV9ZKQ98G0TR, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094406448_01KP28581GC83KJV9ZKQ98G0TR: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094406448_01KP28581GC83KJV9ZKQ98G0TR, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094406448_01KP28581GC83KJV9ZKQ98G0TR: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094406448_01KP28581GC83KJV9ZKQ98G0TR: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:06	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:06	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094406464_01KP28582086TWPZ4PHM802F5C, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094406464_01KP28582086TWPZ4PHM802F5C: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094406464_01KP28582086TWPZ4PHM802F5C: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:06	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094406464_01KP28582086TWPZ4PHM802F5C, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094406464_01KP28582086TWPZ4PHM802F5C: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094406464_01KP28582086TWPZ4PHM802F5C, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094406464_01KP28582086TWPZ4PHM802F5C: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094406464_01KP28582086TWPZ4PHM802F5C: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:06	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:06	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094406473_01KP2858298X6K1Y1S39NMKVM0, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094406473_01KP2858298X6K1Y1S39NMKVM0: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094406473_01KP2858298X6K1Y1S39NMKVM0: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:06	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094406473_01KP2858298X6K1Y1S39NMKVM0, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094406473_01KP2858298X6K1Y1S39NMKVM0: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094406473_01KP2858298X6K1Y1S39NMKVM0, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094406473_01KP2858298X6K1Y1S39NMKVM0: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094406473_01KP2858298X6K1Y1S39NMKVM0: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:06	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094436459_01KP2865BBR50D30M5CAD86NTE, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094436459_01KP2865BBR50D30M5CAD86NTE: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094436459_01KP2865BBR50D30M5CAD86NTE: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094436459_01KP2865BBR50D30M5CAD86NTE, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094436459_01KP2865BBR50D30M5CAD86NTE: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094436459_01KP2865BBR50D30M5CAD86NTE, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094436459_01KP2865BBR50D30M5CAD86NTE: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094436459_01KP2865BBR50D30M5CAD86NTE: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094436470_01KP2865BP4K44J0QZ8JBV1FDQ, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094436470_01KP2865BP4K44J0QZ8JBV1FDQ: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094436470_01KP2865BP4K44J0QZ8JBV1FDQ: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094436470_01KP2865BP4K44J0QZ8JBV1FDQ, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094436470_01KP2865BP4K44J0QZ8JBV1FDQ: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094436470_01KP2865BP4K44J0QZ8JBV1FDQ, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094436470_01KP2865BP4K44J0QZ8JBV1FDQ: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094436470_01KP2865BP4K44J0QZ8JBV1FDQ: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094436482_01KP2865C2VACP0RR7KH75F5E1, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094436482_01KP2865C2VACP0RR7KH75F5E1: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094436482_01KP2865C2VACP0RR7KH75F5E1: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094436482_01KP2865C2VACP0RR7KH75F5E1, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094436482_01KP2865C2VACP0RR7KH75F5E1: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094436482_01KP2865C2VACP0RR7KH75F5E1, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094436482_01KP2865C2VACP0RR7KH75F5E1: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094436482_01KP2865C2VACP0RR7KH75F5E1: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:44:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
	龙成华	2026-04-13 9:44:51	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:44:51	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:44:51	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:44:51	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:44:52	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 9:45:06	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094506463_01KP2872MZ8RDHN8KK2CPBC62X, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094506463_01KP2872MZ8RDHN8KK2CPBC62X: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094506463_01KP2872MZ8RDHN8KK2CPBC62X: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:06	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094506463_01KP2872MZ8RDHN8KK2CPBC62X, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094506463_01KP2872MZ8RDHN8KK2CPBC62X: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094506463_01KP2872MZ8RDHN8KK2CPBC62X, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094506463_01KP2872MZ8RDHN8KK2CPBC62X: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094506463_01KP2872MZ8RDHN8KK2CPBC62X: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:06	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:06	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094506478_01KP2872NE9QZA27292ZAAX2A4, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094506478_01KP2872NE9QZA27292ZAAX2A4: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094506478_01KP2872NE9QZA27292ZAAX2A4: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:06	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094506478_01KP2872NE9QZA27292ZAAX2A4, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094506478_01KP2872NE9QZA27292ZAAX2A4: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094506478_01KP2872NE9QZA27292ZAAX2A4, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094506478_01KP2872NE9QZA27292ZAAX2A4: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094506478_01KP2872NE9QZA27292ZAAX2A4: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:06	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:06	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094506487_01KP2872NQGYQV3J50FB4T0F1F, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094506487_01KP2872NQGYQV3J50FB4T0F1F: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094506487_01KP2872NQGYQV3J50FB4T0F1F: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:06	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094506487_01KP2872NQGYQV3J50FB4T0F1F, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094506487_01KP2872NQGYQV3J50FB4T0F1F: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094506487_01KP2872NQGYQV3J50FB4T0F1F, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094506487_01KP2872NQGYQV3J50FB4T0F1F: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094506487_01KP2872NQGYQV3J50FB4T0F1F: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:06	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094536469_01KP287ZYN3ZTT4BCQDJWSTKY1, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094536469_01KP287ZYN3ZTT4BCQDJWSTKY1: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094536469_01KP287ZYN3ZTT4BCQDJWSTKY1: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094536469_01KP287ZYN3ZTT4BCQDJWSTKY1, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094536469_01KP287ZYN3ZTT4BCQDJWSTKY1: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094536469_01KP287ZYN3ZTT4BCQDJWSTKY1, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094536469_01KP287ZYN3ZTT4BCQDJWSTKY1: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094536469_01KP287ZYN3ZTT4BCQDJWSTKY1: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094536481_01KP287ZZ1BHY7B12RTRA0ZB9Z, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094536481_01KP287ZZ1BHY7B12RTRA0ZB9Z: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094536481_01KP287ZZ1BHY7B12RTRA0ZB9Z: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094536481_01KP287ZZ1BHY7B12RTRA0ZB9Z, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094536481_01KP287ZZ1BHY7B12RTRA0ZB9Z: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094536481_01KP287ZZ1BHY7B12RTRA0ZB9Z, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094536481_01KP287ZZ1BHY7B12RTRA0ZB9Z: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094536481_01KP287ZZ1BHY7B12RTRA0ZB9Z: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094536490_01KP287ZZANASSJYF5Z6C2AXA6, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094536490_01KP287ZZANASSJYF5Z6C2AXA6: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094536490_01KP287ZZANASSJYF5Z6C2AXA6: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094536490_01KP287ZZANASSJYF5Z6C2AXA6, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094536490_01KP287ZZANASSJYF5Z6C2AXA6: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094536490_01KP287ZZANASSJYF5Z6C2AXA6, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094536490_01KP287ZZANASSJYF5Z6C2AXA6: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094536490_01KP287ZZANASSJYF5Z6C2AXA6: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:45:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:06	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094606477_01KP288X8DJHPME3K3DZ8P1GB9, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094606477_01KP288X8DJHPME3K3DZ8P1GB9: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094606477_01KP288X8DJHPME3K3DZ8P1GB9: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:06	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094606477_01KP288X8DJHPME3K3DZ8P1GB9, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094606477_01KP288X8DJHPME3K3DZ8P1GB9: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094606477_01KP288X8DJHPME3K3DZ8P1GB9, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094606477_01KP288X8DJHPME3K3DZ8P1GB9: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094606477_01KP288X8DJHPME3K3DZ8P1GB9: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:06	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:06	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094606488_01KP288X8RT5FGN99GW9T3FS0K, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094606488_01KP288X8RT5FGN99GW9T3FS0K: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094606488_01KP288X8RT5FGN99GW9T3FS0K: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:06	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094606488_01KP288X8RT5FGN99GW9T3FS0K, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094606488_01KP288X8RT5FGN99GW9T3FS0K: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094606488_01KP288X8RT5FGN99GW9T3FS0K, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094606488_01KP288X8RT5FGN99GW9T3FS0K: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094606488_01KP288X8RT5FGN99GW9T3FS0K: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:06	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:06	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094606497_01KP288X91388X85WCH1V0G996, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094606497_01KP288X91388X85WCH1V0G996: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094606497_01KP288X91388X85WCH1V0G996: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:06	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094606497_01KP288X91388X85WCH1V0G996, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094606497_01KP288X91388X85WCH1V0G996: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094606497_01KP288X91388X85WCH1V0G996, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094606497_01KP288X91388X85WCH1V0G996: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094606497_01KP288X91388X85WCH1V0G996: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:06	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
	龙成华	2026-04-13 9:46:12	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:46:12	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:46:12	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:46:12	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:46:13	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 9:46:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094636491_01KP289TJBF7S9XPGMYYHFY61B, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094636491_01KP289TJBF7S9XPGMYYHFY61B: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094636491_01KP289TJBF7S9XPGMYYHFY61B: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094636491_01KP289TJBF7S9XPGMYYHFY61B, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094636491_01KP289TJBF7S9XPGMYYHFY61B: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094636491_01KP289TJBF7S9XPGMYYHFY61B, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094636491_01KP289TJBF7S9XPGMYYHFY61B: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094636491_01KP289TJBF7S9XPGMYYHFY61B: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094636501_01KP289TJNAGMSBAHAT5DRES1X, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094636501_01KP289TJNAGMSBAHAT5DRES1X: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094636501_01KP289TJNAGMSBAHAT5DRES1X: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094636501_01KP289TJNAGMSBAHAT5DRES1X, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094636501_01KP289TJNAGMSBAHAT5DRES1X: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094636501_01KP289TJNAGMSBAHAT5DRES1X, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094636501_01KP289TJNAGMSBAHAT5DRES1X: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094636501_01KP289TJNAGMSBAHAT5DRES1X: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094636515_01KP289TK3WQFQ7D2BWWYV88SZ, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094636515_01KP289TK3WQFQ7D2BWWYV88SZ: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094636515_01KP289TK3WQFQ7D2BWWYV88SZ: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094636515_01KP289TK3WQFQ7D2BWWYV88SZ, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094636515_01KP289TK3WQFQ7D2BWWYV88SZ: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094636515_01KP289TK3WQFQ7D2BWWYV88SZ, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094636515_01KP289TK3WQFQ7D2BWWYV88SZ: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094636515_01KP289TK3WQFQ7D2BWWYV88SZ: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:46:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:06	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094706500_01KP28AQW47CPP7KSSX8VSQZ26, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094706500_01KP28AQW47CPP7KSSX8VSQZ26: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094706500_01KP28AQW47CPP7KSSX8VSQZ26: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:06	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094706500_01KP28AQW47CPP7KSSX8VSQZ26, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094706500_01KP28AQW47CPP7KSSX8VSQZ26: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094706500_01KP28AQW47CPP7KSSX8VSQZ26, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094706500_01KP28AQW47CPP7KSSX8VSQZ26: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094706500_01KP28AQW47CPP7KSSX8VSQZ26: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:06	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:06	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094706511_01KP28AQWFVG8J6TCFR6TQJDKE, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094706511_01KP28AQWFVG8J6TCFR6TQJDKE: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094706511_01KP28AQWFVG8J6TCFR6TQJDKE: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:06	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094706511_01KP28AQWFVG8J6TCFR6TQJDKE, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094706511_01KP28AQWFVG8J6TCFR6TQJDKE: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094706511_01KP28AQWFVG8J6TCFR6TQJDKE, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094706511_01KP28AQWFVG8J6TCFR6TQJDKE: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094706511_01KP28AQWFVG8J6TCFR6TQJDKE: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:06	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:06	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094706522_01KP28AQWTBZJZMBXJV70BANFD, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094706522_01KP28AQWTBZJZMBXJV70BANFD: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094706522_01KP28AQWTBZJZMBXJV70BANFD: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:06	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094706522_01KP28AQWTBZJZMBXJV70BANFD, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094706522_01KP28AQWTBZJZMBXJV70BANFD: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094706522_01KP28AQWTBZJZMBXJV70BANFD, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094706522_01KP28AQWTBZJZMBXJV70BANFD: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094706522_01KP28AQWTBZJZMBXJV70BANFD: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:06	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
	龙成华	2026-04-13 9:47:33	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:47:33	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:47:33	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:47:33	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:47:34	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 9:47:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094736516_01KP28BN64VF20YJCXZ6G4R9TY, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094736516_01KP28BN64VF20YJCXZ6G4R9TY: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094736516_01KP28BN64VF20YJCXZ6G4R9TY: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094736516_01KP28BN64VF20YJCXZ6G4R9TY, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094736516_01KP28BN64VF20YJCXZ6G4R9TY: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094736516_01KP28BN64VF20YJCXZ6G4R9TY, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094736516_01KP28BN64VF20YJCXZ6G4R9TY: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094736516_01KP28BN64VF20YJCXZ6G4R9TY: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094736529_01KP28BN6HD19DPRVFEY2D21MY, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094736529_01KP28BN6HD19DPRVFEY2D21MY: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094736529_01KP28BN6HD19DPRVFEY2D21MY: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094736529_01KP28BN6HD19DPRVFEY2D21MY, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094736529_01KP28BN6HD19DPRVFEY2D21MY: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094736529_01KP28BN6HD19DPRVFEY2D21MY, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094736529_01KP28BN6HD19DPRVFEY2D21MY: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094736529_01KP28BN6HD19DPRVFEY2D21MY: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094736539_01KP28BN6V3KGJPJ4EBK34CX0D, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094736539_01KP28BN6V3KGJPJ4EBK34CX0D: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094736539_01KP28BN6V3KGJPJ4EBK34CX0D: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094736539_01KP28BN6V3KGJPJ4EBK34CX0D, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094736539_01KP28BN6V3KGJPJ4EBK34CX0D: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094736539_01KP28BN6V3KGJPJ4EBK34CX0D, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094736539_01KP28BN6V3KGJPJ4EBK34CX0D: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094736539_01KP28BN6V3KGJPJ4EBK34CX0D: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:47:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:06	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094806531_01KP28CJG3GP4E1YZXQBA1CMSH, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094806531_01KP28CJG3GP4E1YZXQBA1CMSH: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094806531_01KP28CJG3GP4E1YZXQBA1CMSH: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:06	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094806531_01KP28CJG3GP4E1YZXQBA1CMSH, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094806531_01KP28CJG3GP4E1YZXQBA1CMSH: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094806531_01KP28CJG3GP4E1YZXQBA1CMSH, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094806531_01KP28CJG3GP4E1YZXQBA1CMSH: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094806531_01KP28CJG3GP4E1YZXQBA1CMSH: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:06	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:06	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094806541_01KP28CJGDWM5SJ2GZ9990BXEJ, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094806541_01KP28CJGDWM5SJ2GZ9990BXEJ: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094806541_01KP28CJGDWM5SJ2GZ9990BXEJ: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:06	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094806541_01KP28CJGDWM5SJ2GZ9990BXEJ, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094806541_01KP28CJGDWM5SJ2GZ9990BXEJ: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094806541_01KP28CJGDWM5SJ2GZ9990BXEJ, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094806541_01KP28CJGDWM5SJ2GZ9990BXEJ: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094806541_01KP28CJGDWM5SJ2GZ9990BXEJ: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:06	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:06	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094806554_01KP28CJGT7A5V2A2CXMYV7D5S, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094806554_01KP28CJGT7A5V2A2CXMYV7D5S: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094806554_01KP28CJGT7A5V2A2CXMYV7D5S: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:06	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094806554_01KP28CJGT7A5V2A2CXMYV7D5S, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094806554_01KP28CJGT7A5V2A2CXMYV7D5S: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094806554_01KP28CJGT7A5V2A2CXMYV7D5S, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094806554_01KP28CJGT7A5V2A2CXMYV7D5S: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094806554_01KP28CJGT7A5V2A2CXMYV7D5S: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:06	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094836533_01KP28DFSNVBR98EBM2QEGJDB6, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094836533_01KP28DFSNVBR98EBM2QEGJDB6: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094836533_01KP28DFSNVBR98EBM2QEGJDB6: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094836533_01KP28DFSNVBR98EBM2QEGJDB6, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094836533_01KP28DFSNVBR98EBM2QEGJDB6: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094836533_01KP28DFSNVBR98EBM2QEGJDB6, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094836533_01KP28DFSNVBR98EBM2QEGJDB6: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094836533_01KP28DFSNVBR98EBM2QEGJDB6: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094836545_01KP28DFT1XN3KAFSHH28CAK9V, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094836545_01KP28DFT1XN3KAFSHH28CAK9V: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094836545_01KP28DFT1XN3KAFSHH28CAK9V: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094836545_01KP28DFT1XN3KAFSHH28CAK9V, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094836545_01KP28DFT1XN3KAFSHH28CAK9V: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094836545_01KP28DFT1XN3KAFSHH28CAK9V, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094836545_01KP28DFT1XN3KAFSHH28CAK9V: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094836545_01KP28DFT1XN3KAFSHH28CAK9V: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094836554_01KP28DFTAWZSAEKGVQRB7AEKD, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094836554_01KP28DFTAWZSAEKGVQRB7AEKD: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094836554_01KP28DFTAWZSAEKGVQRB7AEKD: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094836554_01KP28DFTAWZSAEKGVQRB7AEKD, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094836554_01KP28DFTAWZSAEKGVQRB7AEKD: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094836554_01KP28DFTAWZSAEKGVQRB7AEKD, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094836554_01KP28DFTAWZSAEKGVQRB7AEKD: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094836554_01KP28DFTAWZSAEKGVQRB7AEKD: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:48:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
	龙成华	2026-04-13 9:48:55	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:48:55	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:48:55	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:48:55	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:48:56	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 9:49:06	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094906540_01KP28ED3CNJ5C53K5FKVEH7Y3, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094906540_01KP28ED3CNJ5C53K5FKVEH7Y3: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094906540_01KP28ED3CNJ5C53K5FKVEH7Y3: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:06	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094906540_01KP28ED3CNJ5C53K5FKVEH7Y3, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094906540_01KP28ED3CNJ5C53K5FKVEH7Y3: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094906540_01KP28ED3CNJ5C53K5FKVEH7Y3, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094906540_01KP28ED3CNJ5C53K5FKVEH7Y3: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094906540_01KP28ED3CNJ5C53K5FKVEH7Y3: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:06	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:06	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094906569_01KP28ED49PPZ4FJ9EGA3ZK8ST, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094906569_01KP28ED49PPZ4FJ9EGA3ZK8ST: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094906569_01KP28ED49PPZ4FJ9EGA3ZK8ST: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:06	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094906569_01KP28ED49PPZ4FJ9EGA3ZK8ST, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094906569_01KP28ED49PPZ4FJ9EGA3ZK8ST: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094906569_01KP28ED49PPZ4FJ9EGA3ZK8ST, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094906569_01KP28ED49PPZ4FJ9EGA3ZK8ST: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094906569_01KP28ED49PPZ4FJ9EGA3ZK8ST: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:06	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:06	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:06	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094906580_01KP28ED4MYB7PJ19WPRY6GKYY, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094906580_01KP28ED4MYB7PJ19WPRY6GKYY: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094906580_01KP28ED4MYB7PJ19WPRY6GKYY: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:06	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094906580_01KP28ED4MYB7PJ19WPRY6GKYY, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094906580_01KP28ED4MYB7PJ19WPRY6GKYY: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094906580_01KP28ED4MYB7PJ19WPRY6GKYY, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094906580_01KP28ED4MYB7PJ19WPRY6GKYY: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094906580_01KP28ED4MYB7PJ19WPRY6GKYY: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:06	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094936554_01KP28FADAG1S1K0Z1BS6SVQ0Y, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094936554_01KP28FADAG1S1K0Z1BS6SVQ0Y: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094936554_01KP28FADAG1S1K0Z1BS6SVQ0Y: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094936554_01KP28FADAG1S1K0Z1BS6SVQ0Y, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094936554_01KP28FADAG1S1K0Z1BS6SVQ0Y: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094936554_01KP28FADAG1S1K0Z1BS6SVQ0Y, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094936554_01KP28FADAG1S1K0Z1BS6SVQ0Y: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094936554_01KP28FADAG1S1K0Z1BS6SVQ0Y: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094936564_01KP28FADMKRFZD34TSJBC9095, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094936564_01KP28FADMKRFZD34TSJBC9095: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094936564_01KP28FADMKRFZD34TSJBC9095: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094936564_01KP28FADMKRFZD34TSJBC9095, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094936564_01KP28FADMKRFZD34TSJBC9095: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094936564_01KP28FADMKRFZD34TSJBC9095, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094936564_01KP28FADMKRFZD34TSJBC9095: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094936564_01KP28FADMKRFZD34TSJBC9095: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	Token附加失败: CommandId=0x0012:PerformanceDataUpload, TokenInfo=已初始化, TokenManager=可用						192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送数据包时发生错误: CommandId=0x0012:PerformanceDataUpload, 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:36	ERROR	RUINORERP.UI.Network.ClientCommunicationService	发送带响应命令失败: CommandId=0x0012:PerformanceDataUpload, RequestId=PerformanceDataUpload_094936573_01KP28FADXHMCP45TBEACY5E3D, RetryCount=0, Error=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094936573_01KP28FADXHMCP45TBEACY5E3D: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094936573_01KP28FADXHMCP45TBEACY5E3D: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:36	ERROR	RUINORERP.UI.Network.ClientEventManager	未处理的异常:带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094936573_01KP28FADXHMCP45TBEACY5E3D, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094936573_01KP28FADXHMCP45TBEACY5E3D: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload	类型: System.Exception
消息: 带响应命令发送失败: CommandId=PerformanceDataUpload, RequestId=PerformanceDataUpload_094936573_01KP28FADXHMCP45TBEACY5E3D, 错误信息=请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094936573_01KP28FADXHMCP45TBEACY5E3D: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈: 

--- InnerException ---
类型: System.InvalidOperationException
消息: 请求处理失败，指令类型：0x0012:PerformanceDataUpload，请求ID: PerformanceDataUpload_094936573_01KP28FADXHMCP45TBEACY5E3D: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendCommandWithResponseAsync>d__131`1.MoveNext()

--- InnerException ---
类型: RUINORERP.UI.Network.Exceptions.NetworkCommunicationException
消息: 发送请求失败: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 RUINORERP.UI.Network.ClientCommunicationService.<SendRequestAsync>d__99`2.MoveNext()

--- InnerException ---
类型: System.Exception
消息: 发送请求失败: 没有合法授权令牌, 指令：0x0012:PerformanceDataUpload
堆栈:    在 RUINORERP.UI.Network.ClientCommunicationService.<SendPacketCoreAsync>d__115`1.MoveNext()
					192.168.0.20
		WIN-20230415PBO
		2026-04-13 9:49:36	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 						192.168.0.20
		WIN-20230415PBO
	龙成华	2026-04-13 9:50:16	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:50:16	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:50:16	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:50:16	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:50:17	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:51:37	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:51:37	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:51:37	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:51:37	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:51:38	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:52:58	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:52:58	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:52:58	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:52:58	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:52:59	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:54:21	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:54:21	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:54:21	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:54:21	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:54:22	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:55:42	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:55:42	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:55:42	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:55:42	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:55:43	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:57:03	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:57:03	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:57:03	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:57:03	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:57:04	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:58:24	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:58:24	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:58:24	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:58:25	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:58:26	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:59:46	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:59:46	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:59:46	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:59:46	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 9:59:47	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 9:59:53	ERROR	RUINORERP.Extensions.SqlsugarSetup	SQL 执行错误：BeginExecuteReader 要求已打开且可用的 Connection。连接的当前状态为已关闭。, SQL: SELECT [PolicyId],[PolicyName],[TargetTable],[TargetEntity],[IsJoinRequired],[TargetTableJoinField],[JoinTableJoinField],[JoinTable],[JoinType],[JoinOnClause],[FilterClause],[IsParameterized],[ParameterizedFilterClause],[EntityType],[IsEnabled],[PolicyDescription],[Created_at],[Created_by],[Modified_at],[Modified_by],[DefaultRuleEnum] FROM [tb_RowAuthPolicy] WITH(NOLOCK)   WHERE ( [IsEnabled]=1 )		系统服务				192.168.0.27
		PURE-20250527UB
		2026-04-13 9:59:53	ERROR	RUINORERP.Extensions.SqlsugarSetup	SQL 执行错误：中文提示 :  连接数据库过程中发生错误，检查服务器是否正常连接字符串是否正确，错误信息：未将对象引用设置到对象的实例。DbType="SqlServer";ConfigId="".
English Message : Connection open error . 未将对象引用设置到对象的实例。DbType="SqlServer";ConfigId="" , SQL: SELECT [ConfigID],[ConfigKey],[ConfigValue],[Description],[ValueType],[ConfigType],[IsActive],[Created_at],[Created_by],[Modified_at],[Modified_by] FROM [tb_SysGlobalDynamicConfig] WITH(NOLOCK)  		系统服务				192.168.0.27
		PURE-20250527UB
		2026-04-13 9:59:53	WARN	RUINORERP.Business.RowLevelAuthService.RowAuthPolicyLoaderService	加载行级权限策略失败，但不影响系统运行。权限过滤将降级为默认行为	类型: System.InvalidOperationException
消息: BeginExecuteReader 要求已打开且可用的 Connection。连接的当前状态为已关闭。
堆栈:    在 System.Data.SqlClient.SqlCommand.<>c.<ExecuteDbDataReaderAsync>b__180_0(Task`1 result)
   在 System.Threading.Tasks.ContinuationResultTaskFromResultTask`2.InnerInvoke()
   在 System.Threading.Tasks.Task.Execute()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 SqlSugar.AdoProvider.<GetDataReaderAsync>d__128.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 SqlSugar.QueryableProvider`1.<GetDataAsync>d__259`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 SqlSugar.QueryableProvider`1.<_ToListAsync>d__256`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.Business.RowLevelAuthService.RowAuthPolicyLoaderService.<LoadAllPoliciesAsync>d__9.MoveNext()
	系统服务				192.168.0.27
		PURE-20250527UB
	朱鹏飞	2026-04-13 10:00:09	WARN	RUINORERP.UI.MainForm	欢迎流程验证超时,但连接已建立						192.168.0.27
	64-00-6A-09-04-3F	-PURE-20250527UB-Administrator
	龙成华	2026-04-13 10:01:07	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:01:07	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:01:07	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:01:07	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:01:08	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:02:28	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:02:28	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:02:28	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:02:28	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:02:29	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:03:49	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:03:49	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:03:49	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:03:49	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:03:50	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:05:12	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:05:12	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:05:12	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:05:12	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:05:13	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:06:33	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:06:33	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:06:33	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:06:33	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:06:34	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:07:54	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:07:54	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:07:54	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:07:54	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:07:55	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:09:15	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:09:15	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:09:15	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:09:15	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:09:16	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:10:37	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:10:37	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:10:37	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:10:37	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:10:38	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 10:11:10	ERROR	RUINORERP.Server.Network.Services.FileStorageMonitorService			系统服务						WATSON
	龙成华	2026-04-13 10:11:58	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:11:58	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:11:58	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:11:58	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:11:59	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:13:19	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:13:19	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:13:19	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:13:19	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:13:20	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:13:20	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:13:20	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:13:20	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:13:31	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-7480d12e-9a4c-4ccb-9a19-0f89e9a85ea8] 事务连接已关闭或无效，无法提交			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:13:41	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-a2be556b-c07b-40da-9e8b-71e2d17fc08a] 事务连接已关闭或无效，无法提交			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:14:34	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-292e1695-0c7e-42a8-a4a7-2d844176285b] 事务连接已关闭或无效，无法提交			销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:14:34	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-ac57bf32-48c2-44c3-8cec-c7e77e5bf51d] 事务连接已关闭或无效，无法提交			销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:14:40	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-b689091a-f714-4b4d-8f11-5e8b2a99d1e3] 事务连接已关闭或无效，无法提交			销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:14:40	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:14:40	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:14:40	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:14:40	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:14:40	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-fa9b29b5-6596-4717-acd8-1033687c0e23] 事务连接已关闭或无效，无法提交			销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:14:40	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:14:40	WARN	RUINORERP.UI.Network.Services.ClientBizCodeService	业务编码生成失败：未连接到服务器			销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:14:40	ERROR	RUINORERP.UI.Network.Services.ClientBizCodeService	业务编码生成过程中发生未预期的异常 - 命令ID: 0x0F01:GenerateBizBillNo	类型: System.Exception
消息: 未连接到服务器，请检查网络连接后重试
堆栈:    在 RUINORERP.UI.Network.Services.ClientBizCodeService.<SendBizCodeCommandAsync>d__15.MoveNext()
		销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:16:04	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:16:04	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:16:04	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:16:04	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:16:05	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 10:16:45	ERROR	RUINORERP.Extensions.SqlsugarSetup			系统服务				192.168.0.40
		WIN-02TNTIALIEE
	龙成华	2026-04-13 10:17:25	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:17:25	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:17:25	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:17:25	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:17:26	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售退回单	RUINORERP.UI.PSI.SAL.UCSaleOutRe	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	周永琼	2026-04-13 10:18:12	WARN	RUINORERP.UI.MainForm	欢迎流程验证超时,但连接已建立						192.168.0.40
	C8-7F-54-C6-13-D3	-WIN-02TNTIALIEE-Administrator
	龙成华	2026-04-13 10:18:47	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:18:47	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:18:47	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:18:47	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:18:48	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:20:08	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:20:08	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:20:08	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:20:08	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:20:09	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:21:30	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:21:30	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:21:30	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:21:30	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:21:31	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售出库单	RUINORERP.UI.PSI.SAL.UCSaleOut	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:22:26	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043488243357454336, 原因: 单据未被锁定或不存在			采购工作台	RUINORERP.UI.PUR.UCPURWorkbench	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:22:54	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:22:54	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:22:54	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:22:54	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:22:55	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:24:06	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043512879193395200, 原因: 单据未被锁定或不存在			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:24:16	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:24:16	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:24:16	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:24:16	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:24:17	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 10:24:45	ERROR	RUINORERP.Extensions.SqlsugarSetup	SQL 执行错误：中文提示 :  连接数据库过程中发生错误，检查服务器是否正常连接字符串是否正确，错误信息：连接未关闭。 连接的当前状态为正在连接。DbType="SqlServer";ConfigId="".
English Message : Connection open error . 连接未关闭。 连接的当前状态为正在连接。DbType="SqlServer";ConfigId="" , SQL: SELECT [PolicyId],[PolicyName],[TargetTable],[TargetEntity],[IsJoinRequired],[TargetTableJoinField],[JoinTableJoinField],[JoinTable],[JoinType],[JoinOnClause],[FilterClause],[IsParameterized],[ParameterizedFilterClause],[EntityType],[IsEnabled],[PolicyDescription],[Created_at],[Created_by],[Modified_at],[Modified_by],[DefaultRuleEnum] FROM [tb_RowAuthPolicy] WITH(NOLOCK)   WHERE ( [IsEnabled]=1 )		系统服务				192.168.0.25
		PURE-20241005RV
		2026-04-13 10:24:45	WARN	RUINORERP.Business.RowLevelAuthService.RowAuthPolicyLoaderService	加载行级权限策略失败，但不影响系统运行。权限过滤将降级为默认行为	类型: SqlSugar.SqlSugarException
消息: 中文提示 :  连接数据库过程中发生错误，检查服务器是否正常连接字符串是否正确，错误信息：连接未关闭。 连接的当前状态为正在连接。DbType="SqlServer";ConfigId="".
English Message : Connection open error . 连接未关闭。 连接的当前状态为正在连接。DbType="SqlServer";ConfigId="" 
堆栈:    在 SqlSugar.Check.Exception(Boolean isException, String message, String[] args)
   在 SqlSugar.AdoProvider.CheckConnection()
   在 SqlSugar.SqlServerProvider.GetCommand(String sql, SugarParameter[] parameters)
   在 SqlSugar.AdoProvider.<GetDataReaderAsync>d__128.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 SqlSugar.QueryableProvider`1.<GetDataAsync>d__259`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 SqlSugar.QueryableProvider`1.<_ToListAsync>d__256`1.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.Business.RowLevelAuthService.RowAuthPolicyLoaderService.<LoadAllPoliciesAsync>d__9.MoveNext()
	系统服务				192.168.0.25
		PURE-20241005RV
	凌圳华	2026-04-13 10:25:02	WARN	RUINORERP.UI.MainForm	欢迎流程验证超时,但连接已建立						192.168.0.25
	C8-1F-66-31-BE-45	-PURE-20241005RV-Administrator
	龙成华	2026-04-13 10:25:37	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:25:37	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:25:37	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:25:37	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:25:38	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	覃道孔	2026-04-13 10:26:36	WARN	RUINORERP.UI.MainForm	欢迎流程验证超时,但连接已建立		已登录用户				192.168.0.43
		-USER-20190708YI-Administrator
	龙成华	2026-04-13 10:26:58	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:26:59	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:26:59	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:26:59	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:27:00	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	覃道孔	2026-04-13 10:28:01	WARN	RUINORERP.UI.Network.Services.FileBusinessService	关联字段 VoucherImage 的值为 null		-3.0.0.0-2025/11/25 14:28:52-http://smarterp.7766.org:1688/test/updaterFiler/	销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	新增	192.168.0.43
	F8-BC-12-6E-3A-35	-USER-20190708YI-Administrator
	龙成华	2026-04-13 10:28:20	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:28:20	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:28:20	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:28:20	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:28:21	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:28:21	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:28:21	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:28:21	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:29:41	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:29:41	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:29:41	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:29:41	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:29:42	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:31:04	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:31:04	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:31:04	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:31:04	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:31:05	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			行为菜单=>销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:32:26	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:32:26	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:32:26	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:32:26	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:32:27	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			销售订单查询	RUINORERP.UI.PSI.SAL.UCSaleOrderQuery	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 10:32:32	WARN	RUINORERP.UI.MainForm	欢迎流程验证超时,但连接已建立						192.168.0.99
		WATSONNEWPC
	龙成华	2026-04-13 10:33:47	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:33:47	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:33:47	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:33:47	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:33:48	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:35:08	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:35:08	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:35:08	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:35:08	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:35:09	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:36:29	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:36:29	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:36:29	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:36:29	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:36:30	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:37:50	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:37:50	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:37:50	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:37:50	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:37:51	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:39:12	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:39:12	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:39:12	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:39:12	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:39:13	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:40:36	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:40:36	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:40:36	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:40:36	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:40:37	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 10:41:10	ERROR	RUINORERP.Server.Network.Services.FileStorageMonitorService			系统服务						WATSON
	龙成华	2026-04-13 10:41:57	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:41:57	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:41:57	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:41:57	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:41:58	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:43:19	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:43:19	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:43:19	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:43:19	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:43:20	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	张莉	2026-04-13 10:43:29	WARN	RUINORERP.UI.MainForm			-2.0.0.5-2026/3/30 17:24:53-http://smarterp.7766.org:1688/test/updaterFiler/				192.168.0.45
	48-4D-7E-BF-70-E1	-CUSTOMER-Administrator
	龙成华	2026-04-13 10:44:40	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:44:40	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:44:40	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:44:40	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:44:41	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:46:01	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:46:01	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:46:01	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:46:01	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:46:02	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
		2026-04-13 10:46:31	ERROR	RUINORERP.Server.frmMainNew	[StartServerAsync]		系统服务						WATSONNEWPC
	龙成华	2026-04-13 10:47:22	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:47:22	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:47:22	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:47:22	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:47:23	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:48:43	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:48:43	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:48:43	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:48:43	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:48:44	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:50:07	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:50:07	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:50:07	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:50:07	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:50:08	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:51:28	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:51:28	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:51:28	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:51:28	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:51:29	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:52:49	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:52:49	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:52:49	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:52:49	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:52:50	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:54:10	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:54:10	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:54:10	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:54:10	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:54:11	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:55:31	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:55:31	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:55:31	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:55:31	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:55:32	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:56:53	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:56:53	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:56:53	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:56:53	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:56:54	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:58:14	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:58:14	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:58:14	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:58:14	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:58:15	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:59:37	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:59:37	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:59:37	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:59:37	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 10:59:38	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:00:59	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:00:59	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:00:59	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:00:59	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:01:00	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:02:20	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:02:20	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:02:20	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:02:20	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:02:21	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:02:22	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:02:22	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:02:22	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:03:42	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:03:42	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:03:42	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:03:42	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:03:43	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:05:03	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:05:04	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:05:04	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:05:04	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:05:05	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:06:25	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:06:25	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:06:25	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:06:25	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:06:26	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:07:50	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			归还单查询	RUINORERP.UI.PSI.INV.UCProdReturningQuery	RUINORERP.UI.PSI.INV.UCProdReturningQuery	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:07:50	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			归还单查询	RUINORERP.UI.PSI.INV.UCProdReturningQuery	RUINORERP.UI.PSI.INV.UCProdReturningQuery	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:07:50	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			归还单查询	RUINORERP.UI.PSI.INV.UCProdReturningQuery	RUINORERP.UI.PSI.INV.UCProdReturningQuery	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:07:50	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			归还单查询	RUINORERP.UI.PSI.INV.UCProdReturningQuery	RUINORERP.UI.PSI.INV.UCProdReturningQuery	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:07:51	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			归还单查询	RUINORERP.UI.PSI.INV.UCProdReturningQuery	RUINORERP.UI.PSI.INV.UCProdReturningQuery	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:07:52	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 			行为菜单=>归还单	RUINORERP.UI.PSI.INV.UCProdReturning	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:07:52	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 			行为菜单=>归还单	RUINORERP.UI.PSI.INV.UCProdReturning	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:07:52	WARN	RUINORERP.UI.Network.Services.ClientPerformanceMonitorService	性能数据上报失败: 			行为菜单=>归还单	RUINORERP.UI.PSI.INV.UCProdReturning	加载菜单	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:07:56	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-4ca1b035-e996-4e93-8be6-b8c96000ee32] 事务连接已关闭或无效，无法提交			归还单	RUINORERP.UI.PSI.INV.UCProdReturning	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:08:58	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2033839391042048000, 原因: 单据未被锁定或不存在			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	RUINORERP.UI.PSI.INV.UCProdBorrowingQuery	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:09:12	WARN	RUINORERP.UI.Network.ClientCommunicationService	?? 心跳失败达到阈值 8，连续失败次数: 8，主动断开并触发重连			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	RUINORERP.UI.PSI.INV.UCProdBorrowingQuery	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:09:12	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 开始断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	RUINORERP.UI.PSI.INV.UCProdBorrowingQuery	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:09:12	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	RUINORERP.UI.PSI.INV.UCProdBorrowingQuery	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:09:12	WARN	RUINORERP.UI.Network.SuperSocketClient	[主动断开连接] 已成功断开与服务器的连接 - 服务器: smarterp.7766.org:3099			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	RUINORERP.UI.PSI.INV.UCProdBorrowingQuery	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:09:13	WARN	RUINORERP.UI.MainForm	检测到连接断开，启动自动重连机制			工作台	RUINORERP.UI.UserCenter.UCWorkbenches	RUINORERP.UI.PSI.INV.UCProdBorrowingQuery	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:09:54	WARN	RUINORERP.UI.MainForm	欢迎流程验证超时,但连接已建立						192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:14:15	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2042040878104711168, 原因: 单据未被锁定或不存在			销售退回单查询	RUINORERP.UI.PSI.SAL.UCSaleOutReQuery	查询	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:18:53	ERROR	RUINORERP.Extensions.SqlsugarSetup	SQL 执行错误：违反了 PRIMARY KEY 约束 'PK_TB_PURENTRY'。不能在对象 'dbo.tb_PurEntry' 中插入重复键。
语句已终止。, SQL: INSERT INTO [tb_PurEntry]  
           ([PurEntryID],[PurEntryNo],[CustomerVendor_ID],[DepartmentID],[ProjectGroup_ID],[Employee_ID],[Paytype_ID],[PurOrder_ID],[PurOrder_NO],[PayStatus],[IsCustomizedOrder],[Currency_ID],[ExchangeRate],[TotalQty],[ForeignTotalAmount],[TotalAmount],[TotalTaxAmount],[TotalUntaxedAmount],[EntryDate],[Notes],[isdeleted],[Created_at],[Created_by],[Modified_at],[Modified_by],[ApprovalOpinions],[ApprovalStatus],[ApprovalResults],[DataStatus],[Approver_by],[Approver_at],[PrintStatus],[IsIncludeTax],[KeepAccountsType],[ForeignShipCost],[ShipCost],[TaxDeductionType],[ReceiptInvoiceClosed],[GenerateVouchers],[VoucherNO])
     VALUES
           (2043529326196035584,'PIR260413251',1743467637846970368,1740610089557037056,,1740614448411971584,1740969004765417472,2039933874380869632,'PO7EA43035',1,0,1746556578401751040,1,1000,0,1200.0000,0,1200.0000,'2026/4/13 11:18:50','',0,'2026/4/13 11:18:52',1740614448411971584,'',,'',0,NULL,1,,'',0,0,,0,0,,NULL,NULL,'') ;			采购入库单	RUINORERP.UI.PSI.PUR.UCPurEntry	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:18:54	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-43b3a733-ba22-4356-ae18-163b3da87b54] 事务对象已为空，跳过回滚			采购入库单	RUINORERP.UI.PSI.PUR.UCPurEntry	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:18:54	ERROR	RUINORERP.Business.tb_PurEntryController	违反了 PRIMARY KEY 约束 'PK_TB_PURENTRY'。不能在对象 'dbo.tb_PurEntry' 中插入重复键。
语句已终止。	类型: System.Data.SqlClient.SqlException
消息: 违反了 PRIMARY KEY 约束 'PK_TB_PURENTRY'。不能在对象 'dbo.tb_PurEntry' 中插入重复键。
语句已终止。
堆栈:    在 SqlSugar.AdoProvider.ExecuteCommand(String sql, SugarParameter[] parameters)
   在 SqlSugar.InsertableProvider`1.ExecuteCommand()
   在 SqlSugar.InsertNavProvider`2.SetValue[TChild](EntityColumnInfo pkColumn, List`1 insertData, Func`1 value)
   在 SqlSugar.InsertNavProvider`2.InitData[TChild](EntityColumnInfo pkColumn, List`1 insertData)
   在 SqlSugar.InsertNavProvider`2.InsertDatas[TChild](List`1 children, EntityColumnInfo pkColumn, EntityColumnInfo NavColumn)
   在 SqlSugar.InsertNavProvider`2.GetRootList[Type](List`1 datas)
   在 SqlSugar.InsertNavProvider`2.InitParentList()
   在 SqlSugar.InsertNavProvider`2._ThenInclude[TChild](Expression`1 expression)
   在 SqlSugar.InsertNavProvider`2.ThenInclude[TChild](Expression`1 expression)
   在 SqlSugar.InsertNavTaskInit`2.<>c__DisplayClass16_0`1.<Include>b__0()
   在 SqlSugar.InsertNavTask`2.<AsNav>b__24_0()
   在 SqlSugar.InsertNavTask`2.<>c__DisplayClass13_0`1.<ThenInclude>b__0()
   在 SqlSugar.InsertNavTask`2.ExecuteCommand()
   在 SqlSugar.InsertNavTask`2.<<ExecuteCommandAsync>b__23_0>d.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 SqlSugar.InsertNavTask`2.<ExecuteCommandAsync>d__23.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.Business.tb_PurEntryController`1.<BaseSaveOrUpdateWithChild>d__16`1.MoveNext()
		采购入库单	RUINORERP.UI.PSI.PUR.UCPurEntry	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:18:54	ERROR	RUINORERP.Business.BaseController	保存失败，请重试;或联系管理员。违反了 PRIMARY KEY 约束 'PK_TB_PURENTRY'。不能在对象 'dbo.tb_PurEntry' 中插入重复键。
语句已终止。			采购入库单	RUINORERP.UI.PSI.PUR.UCPurEntry	提交	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:19:14	ERROR	RUINORERP.Extensions.SqlsugarSetup	SQL 执行错误：违反了 UNIQUE KEY 约束 'AK_KEY_TB_PURENTRY_TB_PUREN'。不能在对象 'dbo.tb_PurEntry' 中插入重复键。
语句已终止。, SQL: INSERT INTO [tb_PurEntry]  
           ([PurEntryID],[PurEntryNo],[CustomerVendor_ID],[DepartmentID],[ProjectGroup_ID],[Employee_ID],[Paytype_ID],[PurOrder_ID],[PurOrder_NO],[PayStatus],[IsCustomizedOrder],[Currency_ID],[ExchangeRate],[TotalQty],[ForeignTotalAmount],[TotalAmount],[TotalTaxAmount],[TotalUntaxedAmount],[EntryDate],[Notes],[isdeleted],[Created_at],[Created_by],[Modified_at],[Modified_by],[ApprovalOpinions],[ApprovalStatus],[ApprovalResults],[DataStatus],[Approver_by],[Approver_at],[PrintStatus],[IsIncludeTax],[KeepAccountsType],[ForeignShipCost],[ShipCost],[TaxDeductionType],[ReceiptInvoiceClosed],[GenerateVouchers],[VoucherNO])
     VALUES
           (2043529416541343744,'PIR260413251',1743467637846970368,1740610089557037056,,1740614448411971584,1740969004765417472,2039933874380869632,'PO7EA43035',1,0,1746556578401751040,1,1000,0,1200.0000,0,1200.0000,'2026/4/13 11:18:50','',0,'2026/4/13 11:19:14',1740614448411971584,'',,'',0,NULL,1,,'',0,0,,0,0,,NULL,NULL,'') ;			采购入库单	RUINORERP.UI.PSI.PUR.UCPurEntry	保存	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:19:19	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	回滚请求但无事务上下文			采购入库单	RUINORERP.UI.PSI.PUR.UCPurEntry	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:19:19	ERROR	RUINORERP.Business.tb_FM_ReceivablePayableController	{
  "应收付款单": 2043529437168930816,
  "单据编号": "AP260413126",
  "来源业务": 4,
  "来源单据": 2043529416541343744,
  "来源单号": "PIR260413251",
  "单据日期": "2026-04-13T11:19:14.488748+08:00",
  "业务日期": "2026-04-13T11:18:50.1203542+08:00",
  "往来单位": 1743467637846970368,
  "币别": 1746556578401751040,
  "费用单据": false,
  "用于佣金": false,
  "平台单": false,
  "收款信息": 1897171161398251520,
  "汇率": 1.0,
  "收付类型": 2,
  "运费": 0.0,
  "总金额外币": 0.0,
  "总金额本币": 1200.0000,
  "已核销外币": 0.0,
  "已核销本币": 0.0,
  "已对账外币": 0.0,
  "已对账本币": 0.0,
  "未核销外币": 0.0,
  "未核销本币": 1200.0000,
  "到期日": "2026-04-13T23:59:59.9999999+08:00",
  "部门": 1740610089557037056,
  "经办人": 1740614448332279808,
  "已开票": false,
  "允许加入对账": true,
  "含税": false,
  "税额总计": 0.0,
  "未税总计": 0.0,
  "支付状态": 2,
  "备注": "采购入库单：PIR260413251的应付款",
  "创建时间": "2026-04-13T11:19:19.4130297+08:00",
  "创建人": 1740614448411971584,
  "逻辑删除": false,
  "审批意见": "由采购入库单确认PIR260413251,系统自动审核",
  "审批状态": 1,
  "审批结果": true,
  "打印状态": 0,
  "tb_FM_ReceivablePayableDetails": [
    {
      "应收付明细": 2043529437244428288,
      "应收付款单": 2043529437168930816,
      "产品": 1901580893479374848,
      "含税": false,
      "汇率": 1.0,
      "单价": 1.8000,
      "数量": 500.0,
      "税率": 0.0,
      "税额": 0.0,
      "金额小计": 900.0000
    },
    {
      "应收付明细": 2043529437244428289,
      "应收付款单": 2043529437168930816,
      "产品": 1895717701708550144,
      "属性": "中英文",
      "含税": false,
      "汇率": 1.0,
      "单价": 0.6000,
      "数量": 500.0,
      "税率": 0.0,
      "税额": 0.0,
      "金额小计": 300.0000
    }
  ]
}	类型: System.NullReferenceException
消息: 未将对象引用设置到对象的实例。
堆栈:    在 RUINORERP.Business.tb_FM_ReceivablePayableController`1.<ApprovalAsync>d__40.MoveNext()
		采购入库单	RUINORERP.UI.PSI.PUR.UCPurEntry	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:19:19	ERROR	RUINORERP.Business.CommService.FMAuditLogService	获取财务审计对象信息失败	类型: System.ArgumentNullException
消息: 值不能为 null。
参数名: entity
堆栈:    在 RUINORERP.Business.BizMapperService.EntityMappingService.GetIdAndName(Object entity)
   在 RUINORERP.Business.CommService.FMAuditLogService.CreateAuditLog[T](String action, T entity, String description)
		采购入库单	RUINORERP.UI.PSI.PUR.UCPurEntry	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	覃道孔	2026-04-13 11:20:37	WARN	RUINORERP.UI.Network.Services.FileBusinessService	关联字段 VoucherImage 的值为 null		-3.0.0.0-2025/11/25 14:28:52-http://smarterp.7766.org:1688/test/updaterFiler/	销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	新增	192.168.0.43
	F8-BC-12-6E-3A-35	-USER-20190708YI-Administrator
	龙成华	2026-04-13 11:27:10	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043529416541343744, 原因: 单据未被锁定或不存在			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:28:19	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-d3f86b88-d2d1-4da3-8afc-5db6fc69f942] 事务对象已为空，跳过回滚			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:28:19	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-d3f86b88-d2d1-4da3-8afc-5db6fc69f942] 长事务警告：544.85秒			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:28:19	ERROR	RUINORERP.Business.tb_PurEntryController	违反了 UNIQUE KEY 约束 'AK_KEY_TB_PURENTRY_TB_PUREN'。不能在对象 'dbo.tb_PurEntry' 中插入重复键。
语句已终止。	类型: System.Data.SqlClient.SqlException
消息: 违反了 UNIQUE KEY 约束 'AK_KEY_TB_PURENTRY_TB_PUREN'。不能在对象 'dbo.tb_PurEntry' 中插入重复键。
语句已终止。
堆栈:    在 SqlSugar.AdoProvider.ExecuteCommand(String sql, SugarParameter[] parameters)
   在 SqlSugar.InsertableProvider`1.ExecuteCommand()
   在 SqlSugar.InsertNavProvider`2.SetValue[TChild](EntityColumnInfo pkColumn, List`1 insertData, Func`1 value)
   在 SqlSugar.InsertNavProvider`2.InitData[TChild](EntityColumnInfo pkColumn, List`1 insertData)
   在 SqlSugar.InsertNavProvider`2.InsertDatas[TChild](List`1 children, EntityColumnInfo pkColumn, EntityColumnInfo NavColumn)
   在 SqlSugar.InsertNavProvider`2.GetRootList[Type](List`1 datas)
   在 SqlSugar.InsertNavProvider`2.InitParentList()
   在 SqlSugar.InsertNavProvider`2._ThenInclude[TChild](Expression`1 expression)
   在 SqlSugar.InsertNavProvider`2.ThenInclude[TChild](Expression`1 expression)
   在 SqlSugar.InsertNavTaskInit`2.<>c__DisplayClass16_0`1.<Include>b__0()
   在 SqlSugar.InsertNavTask`2.<AsNav>b__24_0()
   在 SqlSugar.InsertNavTask`2.<>c__DisplayClass13_0`1.<ThenInclude>b__0()
   在 SqlSugar.InsertNavTask`2.ExecuteCommand()
   在 SqlSugar.InsertNavTask`2.<<ExecuteCommandAsync>b__23_0>d.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter.GetResult()
   在 SqlSugar.InsertNavTask`2.<ExecuteCommandAsync>d__23.MoveNext()
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   在 System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
   在 RUINORERP.Business.tb_PurEntryController`1.<BaseSaveOrUpdateWithChild>d__16`1.MoveNext()
		生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:28:19	ERROR	RUINORERP.Business.BaseController	保存失败，请重试;或联系管理员。违反了 UNIQUE KEY 约束 'AK_KEY_TB_PURENTRY_TB_PUREN'。不能在对象 'dbo.tb_PurEntry' 中插入重复键。
语句已终止。			生产领料单	RUINORERP.UI.MRP.MP.UCMaterialRequisition	审核	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	覃道孔	2026-04-13 11:29:38	WARN	RUINORERP.UI.MainForm	欢迎流程验证超时,但连接已建立		-3.0.0.0-2025/11/25 14:28:52-http://smarterp.7766.org:1688/test/updaterFiler/				192.168.0.43
	F8-BC-12-6E-3A-35	-USER-20190708YI-Administrator
	覃道孔	2026-04-13 11:29:50	WARN	RUINORERP.UI.Network.Services.FileBusinessService	关联字段 VoucherImage 的值为 null		-3.0.0.0-2025/11/25 14:28:52-http://smarterp.7766.org:1688/test/updaterFiler/	行为菜单=>销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	新增	192.168.0.43
	F8-BC-12-6E-3A-35	-USER-20190708YI-Administrator
	覃道孔	2026-04-13 11:29:52	WARN	RUINORERP.UI.MainForm	检查锁状态失败: 单据ID无效 0		-3.0.0.0-2025/11/25 14:28:52-http://smarterp.7766.org:1688/test/updaterFiler/	行为菜单=>销售订单	RUINORERP.UI.PSI.SAL.UCSaleOrder	关闭	192.168.0.43
	F8-BC-12-6E-3A-35	-USER-20190708YI-Administrator
	龙成华	2026-04-13 11:34:40	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043532301605933056, 原因: 单据未被锁定或不存在			采购入库单	RUINORERP.UI.PSI.PUR.UCPurEntry	新增	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	龙成华	2026-04-13 11:46:32	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043534268243775488, 原因: 单据未被锁定或不存在			采购入库单	RUINORERP.UI.PSI.PUR.UCPurEntry	新增	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	樊仕雯	2026-04-13 11:48:35	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043536256746524672, 原因: 单据未被锁定或不存在			生产计划单	RUINORERP.UI.MRP.MP.UCProductionPlan	刷新	192.168.1.250
		-CHINAMI-3LUO0MU-Administrator
	樊仕雯	2026-04-13 11:48:35	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043536128501485568, 原因: 单据未被锁定或不存在			生产工作台	RUINORERP.UI.MRP.UCProdWorkbench	刷新	192.168.1.250
		-CHINAMI-3LUO0MU-Administrator
	樊仕雯	2026-04-13 11:49:59	ERROR	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-9348b14f-428b-41a8-adad-f1a10de0e23c] 事务提交失败	类型: System.Data.SqlClient.SqlException
消息: 无法执行该事务操作，因为有挂起请求正在此事务上运行。
堆栈:    在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.TdsParser.Run(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj)
   在 System.Data.SqlClient.TdsParser.TdsExecuteTransactionManagerRequest(Byte[] buffer, TransactionManagerRequestType request, String transactionName, TransactionManagerIsolationLevel isoLevel, Int32 timeout, SqlInternalTransaction transaction, TdsParserStateObject stateObj, Boolean isDelegateControlRequest)
   在 System.Data.SqlClient.SqlInternalConnectionTds.ExecuteTransactionYukon(TransactionRequest transactionRequest, String transactionName, IsolationLevel iso, SqlInternalTransaction internalTransaction, Boolean isDelegateControlRequest)
   在 System.Data.SqlClient.SqlInternalTransaction.Commit()
   在 System.Data.SqlClient.SqlTransaction.Commit()
   在 SqlSugar.AdoProvider.CommitTran()
   在 RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage.CommitTranInternal()
		制令单	RUINORERP.UI.MRP.MP.UCManufacturingOrder	提交	192.168.1.250
		-CHINAMI-3LUO0MU-Administrator
	樊仕雯	2026-04-13 11:49:59	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-Unknown] 强制回滚：事务连接已关闭或无效			制令单	RUINORERP.UI.MRP.MP.UCManufacturingOrder	提交	192.168.1.250
		-CHINAMI-3LUO0MU-Administrator
	樊仕雯	2026-04-13 11:49:59	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[CommitTran] 提交请求但无事务上下文			制令单	RUINORERP.UI.MRP.MP.UCManufacturingOrder	提交	192.168.1.250
		-CHINAMI-3LUO0MU-Administrator
	樊仕雯	2026-04-13 11:50:01	WARN	RUINORERP.Repository.UnitOfWorks.UnitOfWorkManage	[Transaction-62a2b7e0-cfd2-45fd-9a55-20ad68e02ebf] 事务连接已关闭或无效，无法提交			制令单	RUINORERP.UI.MRP.MP.UCManufacturingOrder	审核	192.168.1.250
		-CHINAMI-3LUO0MU-Administrator
	龙成华	2026-04-13 11:54:58	WARN	RUINORERP.UI.Network.Services.ClientLockManagementService	解锁失败，已恢复本地锁状态: 单据ID=2043537166679805952, 原因: 单据未被锁定或不存在			采购入库单	RUINORERP.UI.PSI.PUR.UCPurEntry	新增	192.168.1.4
	00-00-00-00-00-00-00-E0	-OUWYRT93Q0I7RJZ-Administrator
	黄利华	2026-04-13 11:56:07	WARN	RUINORERP.UI.MainForm	欢迎流程验证超时,但连接已建立			异常日志管理	RUINORERP.UI.BI.UCLogsList	查询	192.168.0.99
	08-BF-B8-6F-C9-AA	-WATSONNEWPC-Administrator