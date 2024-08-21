drop proc Proc_WorkCenterSale
go

create proc Proc_WorkCenterSale
@Employees nvarchar(1000),
@sqlOutput VARCHAR(8000) OUT -- 定义 OUT 参数来输出 @sql 的值
as 

begin


DECLARE @WhereClause varchar(800) = ' and 1=1 ';-- 用于存储要拼接的 WHERE 子句

-- 根据 @Employees 参数决定是否拼接 WHERE 子句
if isnull(@Employees,'')!='' set @WhereClause+=' and Employee_ID IN (' + @Employees + ')' -- 拼接 WHERE 子句
 PRINT @WhereClause;

 declare  @sql varchar(8000)
 

 set @sql= 'SELECT 
        CASE 
            WHEN DataStatus = 1 THEN ''未提交'' 
            ELSE '' '' 
        END AS 订单状态, COUNT(SOrderNo) AS 数量 
     FROM tb_SaleOrder 
     WHERE DataStatus = 1 '+@WhereClause + '     GROUP BY DataStatus
     UNION ALL
     SELECT 
        CASE 
            WHEN DataStatus = 2 THEN ''未审核'' 
            ELSE ''未审核'' 
        END AS 订单状态, COUNT(SOrderNo) AS 数量 
     FROM tb_SaleOrder 
     WHERE (DataStatus = 2 OR tb_SaleOrder.ApprovalStatus = 0) ' +@WhereClause + '     GROUP BY DataStatus
     UNION ALL
     SELECT 
        CASE 
            WHEN DataStatus = 4 THEN ''未出库'' 
            ELSE '' '' 
        END AS 订单状态, COUNT(SOrderNo) AS 数量 
     FROM tb_SaleOrder 
     WHERE DataStatus = 4 ' +@WhereClause + '     GROUP BY DataStatus;'
;

exec(@sql);
-- 输出 @sql 的值到输出参数 @sqlOutput
 SET @sqlOutput = @sql;
 PRINT @sqlOutput;
end

go


DECLARE @sqlOutput nvarchar(1000)

exec Proc_WorkCenterSale 897897878977, @sqlOutput   


--------------------------------------------------------------------采购订单

drop proc Proc_WorkCenterPUR
go
create proc Proc_WorkCenterPUR
@Employees nvarchar(1000),
@sqlOutput VARCHAR(8000) OUT -- 定义 OUT 参数来输出 @sql 的值
as 

begin


DECLARE @WhereClause varchar(800) = ' and 1=1 ';-- 用于存储要拼接的 WHERE 子句

-- 根据 @Employees 参数决定是否拼接 WHERE 子句
if isnull(@Employees,'')!='' set @WhereClause+=' and Employee_ID IN (' + @Employees + ')' -- 拼接 WHERE 子句
 PRINT @WhereClause;

 declare  @sql varchar(8000)
 

 set @sql= 'SELECT 
        CASE 
            WHEN DataStatus = 1 THEN ''未提交'' 
            ELSE '' '' 
        END AS 订单状态, COUNT(PurOrderNo) AS 数量 
     FROM tb_PurOrder 
     WHERE DataStatus = 1 '+@WhereClause + '     GROUP BY DataStatus
     UNION ALL
     SELECT 
        CASE 
            WHEN DataStatus = 2 THEN ''未审核'' 
            ELSE ''未审核'' 
        END AS 订单状态, COUNT(PurOrderNo) AS 数量 
     FROM tb_PurOrder 
     WHERE (DataStatus = 2) ' +@WhereClause + '     GROUP BY DataStatus
     UNION ALL
    SELECT
     CASE 
            WHEN DataStatus = 5 THEN ''部分入库'' 
            ELSE ''部分入库'' 
        END AS 订单状态, COUNT(PurOrderNo) AS 数量 
        FROM 
(

SELECT PurOrderNo,tb_PurOrder.PurOrder_ID,5 as DataStatus,tb_PurOrderDetail.Quantity,tb_PurOrderDetail.DeliveredQuantity 
FROM tb_PurOrder
JOIN tb_PurOrderDetail ON tb_PurOrder.PurOrder_ID = tb_PurOrderDetail.PurOrder_ID
WHERE 
tb_PurOrderDetail.DeliveredQuantity < tb_PurOrderDetail.Quantity 
and tb_PurOrderDetail.DeliveredQuantity>0
AND tb_PurOrder.DataStatus=4 ' +@WhereClause + '

)  AS TT GROUP BY DataStatus 

 UNION ALL
        SELECT
     CASE 
            WHEN DataStatus = 6 THEN ''未入库'' 
            ELSE ''未入库'' 
        END AS 订单状态, COUNT(DataStatus) 
        FROM 
(

		SELECT
DataStatus, PurOrderNo 
        FROM 
(

SELECT PurOrderNo,tb_PurOrder.PurOrder_ID,6 as DataStatus,tb_PurOrderDetail.Quantity,tb_PurOrderDetail.DeliveredQuantity 
FROM tb_PurOrder
JOIN tb_PurOrderDetail ON tb_PurOrder.PurOrder_ID = tb_PurOrderDetail.PurOrder_ID
WHERE 
tb_PurOrderDetail.DeliveredQuantity < tb_PurOrderDetail.Quantity 
and tb_PurOrderDetail.DeliveredQuantity=0
AND tb_PurOrder.DataStatus=4 ' +@WhereClause + '

)  AS TT GROUP BY PurOrderNo ,DataStatus


)  AS TT GROUP BY DataStatus'
;

exec(@sql);
-- 输出 @sql 的值到输出参数 @sqlOutput
 SET @sqlOutput = @sql;
 PRINT @sqlOutput;
end

go


DECLARE @sqlOutput nvarchar(1000)

exec Proc_WorkCenterPUR '', @sqlOutput 

---------------------------------------------------------------------------
---------------------------------------------------------------------------其它出入库
---其它出库

﻿drop proc Proc_WorkCenterOtherOut
go

create proc Proc_WorkCenterOtherOut
@Employees nvarchar(1000),
@sqlOutput VARCHAR(8000) OUT -- 定义 OUT 参数来输出 @sql 的值
as 

begin


DECLARE @WhereClause varchar(800) = ' and 1=1 ';-- 用于存储要拼接的 WHERE 子句

-- 根据 @Employees 参数决定是否拼接 WHERE 子句
if isnull(@Employees,'')!='' set @WhereClause+=' and Employee_ID IN (' + @Employees + ')' -- 拼接 WHERE 子句
 PRINT @WhereClause;

 declare  @sql varchar(8000)
 

 set @sql= 'SELECT 
        CASE 
            WHEN DataStatus = 1 THEN ''未提交'' 
            ELSE '' '' 
        END AS 单据状态, COUNT(BillNo) AS 数量 
     FROM tb_StockOut 
     WHERE DataStatus = 1 '+@WhereClause + '     GROUP BY DataStatus
     UNION ALL
     SELECT 
        CASE 
            WHEN DataStatus = 2 THEN ''未审核'' 
            ELSE ''未审核'' 
        END AS 单据状态, COUNT(BillNo) AS 数量 
     FROM tb_StockOut 
     WHERE (DataStatus = 2 ) ' +@WhereClause + '     GROUP BY DataStatus
     UNION ALL
     SELECT 
        CASE 
            WHEN DataStatus = 4 THEN ''未结案'' 
            ELSE '' '' 
        END AS 单据状态, COUNT(BillNo) AS 数量 
     FROM tb_StockOut 
     WHERE DataStatus = 4 ' +@WhereClause + '     GROUP BY DataStatus;'
;

exec(@sql);
-- 输出 @sql 的值到输出参数 @sqlOutput
 SET @sqlOutput = @sql;
 PRINT @sqlOutput;
end

go


DECLARE @sqlOutput nvarchar(1000)

exec Proc_WorkCenterOtherOut null, @sqlOutput   


--------------------------------------------------------------------其它入库单
drop proc Proc_WorkCenterOtherIn
go
create proc Proc_WorkCenterOtherIn
@Employees nvarchar(1000),
@sqlOutput VARCHAR(8000) OUT -- 定义 OUT 参数来输出 @sql 的值
as 

begin


DECLARE @WhereClause varchar(800) = ' and 1=1 ';-- 用于存储要拼接的 WHERE 子句

-- 根据 @Employees 参数决定是否拼接 WHERE 子句
if isnull(@Employees,'')!='' set @WhereClause+=' and Employee_ID IN (' + @Employees + ')' -- 拼接 WHERE 子句
 PRINT @WhereClause;

 declare  @sql varchar(8000)
 

 set @sql= 'SELECT 
        CASE 
            WHEN DataStatus = 1 THEN ''未提交'' 
            ELSE '' '' 
        END AS 单据状态, COUNT(BillNo) AS 数量 
     FROM tb_StockIn 
     WHERE DataStatus = 1 '+@WhereClause + '     GROUP BY DataStatus
     UNION ALL
     SELECT 
        CASE 
            WHEN DataStatus = 2 THEN ''未审核'' 
            ELSE ''未审核'' 
        END AS 单据状态, COUNT(BillNo) AS 数量 
     FROM tb_StockIn 
     WHERE (DataStatus = 2) ' +@WhereClause + '     GROUP BY DataStatus
     UNION ALL
     SELECT 
        CASE 
            WHEN DataStatus = 4 THEN ''未结案'' 
            ELSE '' '' 
        END AS 单据状态, COUNT(BillNo) AS 数量 
     FROM tb_StockIn 
     WHERE DataStatus = 4 ' +@WhereClause + '     GROUP BY DataStatus;'
;

exec(@sql);
-- 输出 @sql 的值到输出参数 @sqlOutput
 SET @sqlOutput = @sql;
 PRINT @sqlOutput;
end

go


DECLARE @sqlOutput nvarchar(1000)

exec Proc_WorkCenterOtherIn '', @sqlOutput   