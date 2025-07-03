--采购入库统计依采购--------总进总出

drop proc Proc_PurEntryStatisticsByEmployee
go

CREATE PROC Proc_PurEntryStatisticsByEmployee
    @GroupByField NVARCHAR(50) = '', -- 默认为''，表示同时，可以是 'Employee_ID', 'ProjectGroup_ID' ,或其中一个
    @ProjectGroups NVARCHAR(1000),
    @Employees NVARCHAR(1000),
    @Start VARCHAR(80), -- 时间注意到日期格式，不然数据会不准 2024-02-01 16:26 这样时间上午的没算到
    @End VARCHAR(80),
    @sqlOutput VARCHAR(8000) OUT -- 定义 OUT 参数来输出 @sql 的值
AS
BEGIN
    DECLARE @WhereClause VARCHAR(800) = ' WHERE 1=1 '; -- 用于存储要拼接的 WHERE 子句
    DECLARE @GroupByClause VARCHAR(800) = ''; -- 用于存储 GROUP BY 子句
    DECLARE @GroupByClauseHead VARCHAR(800) = ''; --  

    -- 根据 @Employees 参数决定是否拼接 WHERE 子句
    IF ISNULL(@Employees, '') <> '' 
        SET @WhereClause += ' AND Employee_ID IN (' + @Employees + ')'; -- 拼接 WHERE 子句
    
    IF ISNULL(@ProjectGroups, '') <> '' 
        SET @WhereClause += ' AND ProjectGroup_ID IN (' + @ProjectGroups + ')'; -- 拼接 WHERE 子句

    -- 根据 @GroupByField 参数决定 GROUP BY 子句
    IF @GroupByField = 'Employee_ID' 
    BEGIN
        SET @GroupByClause = 'GROUP BY Employee_ID';
        SET @GroupByClauseHead = 'Employee_ID,';
    END
    ELSE IF @GroupByField = 'ProjectGroup_ID' 
    BEGIN
        SET @GroupByClause = 'GROUP BY ProjectGroup_ID';
        SET @GroupByClauseHead = 'ProjectGroup_ID,';
    END
    ELSE IF @GroupByField = 'Both' 
    BEGIN
        SET @GroupByClause = 'GROUP BY Employee_ID, ProjectGroup_ID';
        SET @GroupByClauseHead = 'Employee_ID, ProjectGroup_ID,';
    END
    ELSE
    BEGIN
        SET @GroupByClause = ''; -- 如果没有指定有效的分组字段，相当于不分组
        SET @GroupByClauseHead = '';
    END

    PRINT @WhereClause;
    PRINT @GroupByClause;
 
    DECLARE @sql VARCHAR(8000);

    SET @sql = '
SELECT 
' + @GroupByClauseHead + '
    SUM(总采购入库数量) AS 总采购入库数量,
    SUM(入库成交金额) AS 入库成交金额,
    SUM(退货数量) AS 退货数量,
    SUM(退货金额) AS 退货金额,
    SUM(采购税额) AS 采购税额,
    SUM(退货税额) AS 退货税额,
    SUM(总采购入库数量 - 退货数量) AS 实际成交数量,
    SUM(入库成交金额 - 退货金额 - 佣金返点 + 佣金返还 - 采购税额) AS 实际成交金额
  
FROM (
    -- 采购数据
    SELECT 
        Employee_ID,
        ProjectGroup_ID,
        SUM(c.Quantity) AS 总采购入库数量,
        SUM(c.TransactionPrice * c.Quantity) AS 入库成交金额,
        SUM(c.SubtotalTaxAmount) AS 采购税额,
        0 AS 退货数量,
        0 AS 退货金额,
        0 AS 退货税额
    FROM tb_PurEntry m 
    LEFT JOIN tb_PurEntryDetail c ON m.PurEntryID = c.PurEntryID
    WHERE (m.DataStatus = 4 OR m.DataStatus = 8) 
        AND m.ApprovalResults = 1 
        AND m.ApprovalStatus = 1
        AND m.EntryDate >= ''' + @Start + '''
        AND m.EntryDate < DATEADD(DAY, 1, ''' + @End + ''')
    GROUP BY Employee_ID, ProjectGroup_ID
    
    UNION ALL
    
    -- 退货数据（取负值以便直接相减）
    SELECT 
        Employee_ID,
        ProjectGroup_ID,
        0 AS 总采购入库数量,
        0 AS 入库成交金额,
        0 AS 采购税额,
        SUM(c.Quantity) AS 退货数量,
        SUM(c.TransactionPrice * c.Quantity) AS 退货金额,
        SUM(c.SubtotalTaxAmount) AS 退货税额,
    FROM tb_PurEntryRe m 
    LEFT JOIN tb_PurEntryReDetail c ON m.PurEntryRe_ID = c.PurEntryRe_ID
    WHERE (m.DataStatus = 4 OR m.DataStatus = 8) 
        AND m.ApprovalStatus = 1 
        AND m.ApprovalResults = 1 
        AND m.ReturnDate >= ''' + @Start + '''
        AND m.ReturnDate < DATEADD(DAY, 1, ''' + @End + ''')
    GROUP BY Employee_ID, ProjectGroup_ID
) AS A
' + @WhereClause + '
' + @GroupByClause + ';';

    PRINT @sql;
    EXEC(@sql);
    
    -- 输出 @sql 的值到输出参数 @sqlOutput
    SET @sqlOutput = @sql;
END
GO    


DECLARE @sqlOutput VARCHAR(MAX)
exec Proc_PurEntryStatisticsByEmployee '','','', '2025-05-01','2025-05-31', @sqlOutput
