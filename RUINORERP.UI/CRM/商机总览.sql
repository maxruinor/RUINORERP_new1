SELECT
    CONVERT(varchar, YEAR(Created_at)) + '-' + RIGHT('0' + CONVERT(varchar, MONTH(Created_at)), 2) AS TimeGroup,
    COUNT(1) AS Count
FROM
    tb_CRM_Customer
WHERE
    Created_at >= '2025-01-01 00:00:00' AND Created_at < '2026-01-01 00:00:00'
GROUP BY
    YEAR(Created_at), MONTH(Created_at)
