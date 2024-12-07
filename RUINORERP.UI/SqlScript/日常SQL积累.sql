--一个订单多次出库时，运费都保存到了出库单中。修复，将保存第一次出库。后面的全清0.


	UPDATE tb_SaleOut
SET ShipCost = 0
WHERE SaleOut_MainID IN (
    -- 子查询，找出多次出库的订单号，并为每个订单号的出库记录分配一个行号
    SELECT SaleOut_MainID
    FROM (
        SELECT
            SaleOut_MainID,SaleOrderNo,
            ROW_NUMBER() OVER (PARTITION BY SaleOrderNo ORDER BY SaleOrderNo) AS RowNum
        FROM
            tb_SaleOut
        WHERE
            ShipCost > 0
    ) AS SubQuery
    WHERE SubQuery.RowNum > 1
);


---上面开始用的订单号。把rownum的也更新为0.通过订单再修改回来再按ID更新

UPDATE tb_SaleOut set ShipCost=B.ShipCost
from tb_SaleOut A,tb_SaleOrder B WHERE a.SOrder_ID=b.SOrder_ID
and B.SOrderNo  in (
SELECT SOrderNo FROM tb_SaleOrder   WHERE ShipCost>0  and SOrderNo in ( SELECT SaleOrderNo FROM tb_SaleOut GROUP BY SaleOrderNo HAVING COUNT ( SaleOrderNo ) > 1 )
)

