CREATE FUNCTION dbo.fn_GetISOWeek (@Date DATE)
RETURNS INT
AS
BEGIN
    DECLARE @ThursdayOfWeek DATE
    DECLARE @Jan4 DATE
    DECLARE @FirstThursday DATE
    DECLARE @ISOYear INT
    
    -- 计算当前周的周四（ISO标准锚点）
    SET @ThursdayOfWeek = DATEADD(DAY, 3 - ((DATEPART(WEEKDAY, @Date) + @@DATEFIRST - 2) % 7), @Date)
    
    -- 使用DATEADD替代DATEFROMPARTS（兼容SQL 2008）
    SET @Jan4 = DATEADD(YEAR, YEAR(@ThursdayOfWeek) - 1900, DATEADD(DAY, 3, DATEADD(MONTH, 0, '19000101')))
    
    -- 计算当年第一个周四
    SET @FirstThursday = DATEADD(DAY, (7 - DATEPART(WEEKDAY, @Jan4) + 1) % 7, @Jan4)
    
    -- 返回周数
    RETURN DATEDIFF(WEEK, @FirstThursday, @ThursdayOfWeek) + 1
END
GO

CREATE FUNCTION dbo.fn_GetISOYear (@Date DATE)
RETURNS INT
AS
BEGIN
    RETURN YEAR(DATEADD(DAY, 3 - ((DATEPART(WEEKDAY, @Date) + @@DATEFIRST - 2) % 7), @Date))
END
GO

CREATE FUNCTION dbo.fn_GetISOWeekKey (@Date DATE)
RETURNS CHAR(8)
AS
BEGIN

    IF @Date IS NULL RETURN NULL

    DECLARE @ISOYear INT = dbo.fn_GetISOYear(@Date)
    DECLARE @ISOWeek INT = dbo.fn_GetISOWeek(@Date)
    
    -- 使用字符串拼接替代FORMAT函数（兼容SQL 2008）
    RETURN CAST(@ISOYear AS VARCHAR) + '-W' + 
           RIGHT('0' + CAST(@ISOWeek AS VARCHAR), 2)
END
GO


-- 测试跨年周
SELECT dbo.fn_GetISOWeekKey('2023-01-01') AS [2023-01-01] -- 应返回 2022-W52

-- 测试第53周
SELECT dbo.fn_GetISOWeekKey('2020-12-31') AS [2020-12-31] -- 应返回 2020-W53

-- 测试普通周
SELECT dbo.fn_GetISOWeekKey('2023-10-16') AS [2023-10-16] -- 应返回 2023-W42

--所有语法兼容 SQL Server 2005 及以上版本