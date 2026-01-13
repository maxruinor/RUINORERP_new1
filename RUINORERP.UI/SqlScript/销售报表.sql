  ----------=======================================================以下是按订单来统计

  --销售订单统计依业务
drop proc Proc_SaleOrderStatisticsByEmployee
go

create proc Proc_SaleOrderStatisticsByEmployee
@GroupByField NVARCHAR ( 50 )= '', -- 默认为''，表示同时，可以是 'Employee_ID', 'ProjectGroup_ID' ,或其中一个
@ProjectGroups nvarchar(1000),
@Employees nvarchar(1000),
@Start varchar(80),--时间注意到日期格式，不然数据会不准 2024-02-01 16:26 这样时间上午的没算到
@End varchar(80),
@sqlOutput VARCHAR(8000) OUT -- 定义 OUT 参数来输出 @sql 的值
as 

begin


DECLARE @WhereClause varchar(800) = ' WHERE 1=1 ';-- 用于存储要拼接的 WHERE 子句

DECLARE		@GroupByClause VARCHAR ( 800 ) = '';-- 用于存储 GROUP BY 子句
DECLARE		@GroupByClauseHead VARCHAR ( 800 ) = '';--  
 

-- 根据 @Employees 参数决定是否拼接 WHERE 子句
if isnull(@Employees,'')<>'' set @WhereClause+=' and Employee_ID IN (' + @Employees + ')' -- 拼接 WHERE 子句
if isnull(@ProjectGroups,'')<>'' set @WhereClause+=' and ProjectGroup_ID IN (' + @ProjectGroups + ')' -- 拼接 WHERE 子句
	-- 根据 @GroupByField 参数决定 GROUP BY 子句
	IF
		@GroupByField = 'Employee_ID' 
		BEGIN
		SET @GroupByClause = 'GROUP BY Employee_ID'		;
		SET @GroupByClauseHead='Employee_ID,'  
		END
	ELSE
	IF
		@GroupByField = 'ProjectGroup_ID' 
		BEGIN
		SET @GroupByClause = 'GROUP BY ProjectGroup_ID' 
		SET @GroupByClauseHead='ProjectGroup_ID,'  
		END
		ELSE
	IF
		@GroupByField = 'Both' 
		BEGIN
			SET @GroupByClause = 'GROUP BY Employee_ID, ProjectGroup_ID'  
			SET @GroupByClauseHead='Employee_ID, ProjectGroup_ID,'  
		END
	ELSE
		BEGIN
			SET @GroupByClause = '' -- 如果没有指定有效的分组字段，相当于不分组
			SET @GroupByClauseHead=''  
		END

 PRINT @WhereClause;
 PRINT @GroupByClause;
 
 declare  @sql varchar(8000)
 

 set @sql='

SELECT 
' + @GroupByClauseHead + '
sum(总销售订单数量) as 总销售订单数量,
sum(订单成交金额) as 订单成交金额,
sum(isnull(退货数量,0)) as 退货数量,
sum(isnull(退货金额,0)) as 退货金额,
sum(销售税额) as 销售税额,
sum(佣金返点) as 佣金返点,
sum(总销售订单数量-isnull(退货数量,0)) as 实际成交数量,
sum(订单成交金额-isnull(退货金额,0)-佣金返点-销售税额) as 实际成交金额

  from (

SELECT A.Employee_ID,A.ProjectGroup_ID,总销售订单数量,订单成交金额,销售税额,佣金返点,退货数量,退货金额 from 

(
 SELECT Employee_ID,ProjectGroup_ID,sum(c.Quantity) as  总销售订单数量, sum(c.TransactionPrice*c.Quantity)  as 订单成交金额 ,sum(c.SubtotalTaxAmount) as [销售税额] ,sum(c.CommissionAmount) as 佣金返点,sum(c.TotalReturnedQty) as 退货数量, sum(c.TransactionPrice*c.TotalReturnedQty)  as 退货金额 from  tb_SaleOrder m RIGHT JOIN  tb_SaleOrderDetail c on  m.SOrder_ID=c.SOrder_ID

WHERE (m.DataStatus=4 or m.DataStatus=8) and m.ApprovalStatus=1 and m.ApprovalResults=1
 and  Convert(varchar(10),m.SaleDate,120) >=''' + @Start + '''
 and  Convert(varchar(10),m.SaleDate,120) <= ''' + @End + '''
GROUP BY Employee_ID,m.ProjectGroup_ID
 ) as A
 )

as SO   
' + @WhereClause + '
' + @GroupByClause + ';'
;

exec(@sql);
-- 输出 @sql 的值到输出参数 @sqlOutput
 SET @sqlOutput = @sql;
 PRINT @sqlOutput;
end

go

--====


DECLARE @sqlOutput VARCHAR(MAX)
exec Proc_SaleOrderStatisticsByEmployee 'Employee_ID',null,'1740611989660635136,1740611989706772480', '2024-01-01','2024-03-01', @sqlOutput


 