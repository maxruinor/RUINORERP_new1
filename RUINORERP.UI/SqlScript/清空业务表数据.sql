﻿ 
-- 执行 TRUNCATE 语句
TRUNCATE TABLE tb_SaleOrderDetail;
TRUNCATE TABLE tb_PurOrderDetail;
TRUNCATE TABLE tb_StocktakeDetail;
TRUNCATE TABLE tb_FM_OtherExpenseDetail;
TRUNCATE TABLE tb_FM_ExpenseClaimDetail;
TRUNCATE TABLE tb_ProdBorrowingDetail;
TRUNCATE TABLE tb_FM_PriceAdjustmentDetail;
TRUNCATE TABLE tb_StockTransferDetail;
TRUNCATE TABLE tb_ProdReturningDetail;
TRUNCATE TABLE tb_FM_PaymentDetail;
TRUNCATE TABLE tb_FM_PaymentRequestDetail;
TRUNCATE TABLE tb_FM_ReceivablePayableDetail;
TRUNCATE TABLE tb_PurReturnEntryDetail
TRUNCATE TABLE tb_FM_PreReceivedPayment;
TRUNCATE TABLE tb_FM_PaymentRecordDetail;
TRUNCATE TABLE tb_SaleOutReDetail
TRUNCATE TABLE tb_PurEntryDetail
TRUNCATE TABLE tb_SaleOutDetail
TRUNCATE TABLE tb_PriceRecord
DELETE  from tb_ProdReturning;
DELETE  from tb_FM_PaymentRecord;
DELETE  from tb_FM_PaymentRequest;
DELETE  from tb_FM_PriceAdjustment;
DELETE  from tb_FM_ReceivablePayable;
DELETE  from tb_FM_Statement;
DELETE  from tb_FM_PaymentSettlement;
DELETE  from tb_SaleOrder;
DELETE  from tb_PurOrder;
DELETE  from  tb_Inventory;
DELETE  from tb_Stocktake;
DELETE  from tb_FM_OtherExpense;
DELETE  from tb_FM_ExpenseClaim;
DELETE  from tb_SaleOutRe
DELETE  from tb_PurReturnEntry;
DELETE  from tb_PurEntryReDetail;
DELETE  from tb_SaleOut
DELETE  from tb_ProdBorrowing;
DELETE  from  tb_PurEntryRe;
DELETE  from tb_PurEntry
DELETE  from  tb_FM_PriceAdjustment;
DELETE  from  tb_StockTransfer;
DELETE  from  tb_StockOutDetail;
DELETE  from  tb_StockOut;



--下面这个禁用是有效果的，一禁用后。要恢复
-- 禁用所有外键约束
--EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT ALL";

-- 执行 TRUNCATE 语句
--TRUNCATE TABLE ...;

-- 启用所有外键约束
--EXEC sp_msforeachtable "ALTER TABLE ? CHECK CONSTRAINT ALL";