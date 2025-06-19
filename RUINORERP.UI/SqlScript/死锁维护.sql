下面


--检测死锁
--如果发生死锁了，我们怎么去检测具体发生死锁的是哪条SQL语句或存储过程？
--这时我们可以使用以下存储过程来检测，就可以查出引起死锁的进程和SQL语句。SQL Server自带的系统存储过程sp_who和sp_lock也可以用来查找阻塞和死锁, 但没有这里介绍的方法好用。
use master
go
create procedure sp_who_lock
as
begin
declare @spid int,@bl int,
 @intTransactionCountOnEntry int,
  @intRowcount int,
  @intCountProperties int,
  @intCounter int
 create table #tmp_lock_who (
 id int identity(1,1),
 spid smallint,
 bl smallint)
 IF @@ERROR<>0 RETURN @@ERROR
 insert into #tmp_lock_who(spid,bl) select 0 ,blocked
 from (select * from sysprocesses where blocked>0 ) a 
 where not exists(select * from (select * from sysprocesses where blocked>0 ) b 
 where a.blocked=spid)
 union select spid,blocked from sysprocesses where blocked>0
 IF @@ERROR<>0 RETURN @@ERROR 
-- 找到临时表的记录数
 select @intCountProperties = Count(*),@intCounter = 1
 from #tmp_lock_who
 IF @@ERROR<>0 RETURN @@ERROR 
 if @intCountProperties=0
 select '现在没有阻塞和死锁信息' as message
-- 循环开始
while @intCounter <= @intCountProperties
begin
-- 取第一条记录
 select @spid = spid,@bl = bl
 from #tmp_lock_who where Id = @intCounter 
 begin
 if @spid =0 
   select '引起数据库死锁的是: '+ CAST(@bl AS VARCHAR(10)) + '进程号,其执行的SQL语法如下'
 else
   select '进程号SPID：'+ CAST(@spid AS VARCHAR(10))+ '被' + '进程号SPID：'+ CAST(@bl AS VARCHAR(10)) +'阻塞,其当前进程执行的SQL语法如下'
 DBCC INPUTBUFFER (@bl )
 end
-- 循环指针下移
 set @intCounter = @intCounter + 1
end
drop table #tmp_lock_who
return 0
end



-----------------------上面是检测死锁的存储过程----------------------
--执行这句。显示一些相关的信息。
exec sp_who_lock


--在没有解决死锁的更好方法前。为了不影响系统使用。可以强制重启SQL服务。
--：SQL server配置管理器-》sql server服务-》右键sql server（实例名）-》重新启动。

或杀死进程号

--=================


exec sp_who 'active'--查看系统内所有的活动进程 BLK不为0的为死锁

exec sp_lock 60 --返回某个进程对资源的锁定情况
 

DBCC INPUTBUFFER (269)

------------

--查询死锁进程语句
 use master
 go

use erpnew
go
select
request_session_id spid, 
OBJECT_NAME(resource_associated_entity_id) tableName ,HOST_NAME() as hostname
from
sys.dm_tran_locks 
where
resource_type='OBJECT'

		
exec sp_who2 1245
DBCC INPUTBUFFER (1245)   --显示从客户端发送到服务器的最后一个语句 

--查询正在运行的进程SQL

 
SELECT   spid,
         blocked,
         DB_NAME(sp.dbid) AS DBName,
         program_name,
         waitresource,
         lastwaittype,
         sp.loginame,
         sp.hostname,
         a.[Text] AS [TextData],
         SUBSTRING(A.text, sp.stmt_start / 2,
         (CASE WHEN sp.stmt_end = -1 THEN DATALENGTH(A.text) ELSE sp.stmt_end
         END - sp.stmt_start) / 2) AS [current_cmd]
FROM     sys.sysprocesses AS sp OUTER APPLY sys.dm_exec_sql_text (sp.sql_handle) AS A
WHERE    spid > 50
ORDER BY blocked DESC, DB_NAME(sp.dbid) ASC, a.[text];


 
 

 

execute sp_who

execute sp_lock;

根据spid 查询造成死锁的语句

DBCC INPUTBUFFER(80)

结束死锁进程

KILL 80
————————————————

                          
                        
原文链接：https://blog.csdn.net/feritylamb/article/details/107691610



--可以查出一些信息


WITH BlockingChain AS (
    SELECT 
        wt.session_id AS blocked_session,
        wt.blocking_session_id,
        er.blocking_session_id AS blocking_chain_id,
        er.status,
        er.wait_type,
        er.wait_time,
        er.command,
        DB_NAME(er.database_id) AS database_name,
        OBJECT_NAME(p.object_id, er.database_id) AS TableName,
        l.request_mode,
        l.resource_type,
        l.resource_description,
        st.text AS SQLText,
        es.host_name,
        es.program_name,
        ROW_NUMBER() OVER(PARTITION BY wt.session_id ORDER BY l.request_session_id) AS rn
    FROM sys.dm_os_waiting_tasks wt
    INNER JOIN sys.dm_exec_requests er ON wt.session_id = er.session_id
    INNER JOIN sys.dm_tran_locks l ON er.session_id = l.request_session_id
    LEFT JOIN sys.partitions p ON l.resource_associated_entity_id = p.hobt_id
    INNER JOIN sys.dm_exec_sessions es ON er.session_id = es.session_id
    CROSS APPLY sys.dm_exec_sql_text(er.sql_handle) st
    WHERE wt.wait_type LIKE 'LCK%_%'
    AND er.database_id = DB_ID()
)
SELECT *
FROM BlockingChain
WHERE rn = 1
ORDER BY wait_time DESC;