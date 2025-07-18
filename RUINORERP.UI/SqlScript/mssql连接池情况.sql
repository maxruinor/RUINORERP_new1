 
SELECT COUNT(*) AS ConnectionCount
FROM sys.dm_exec_connections;
-- 查看当前活跃连接
SELECT 
 
    status,
    login_time,
    host_name,
    program_name,
    client_interface_name,
    login_name,
    last_request_start_time,
    last_request_end_time

FROM sys.dm_exec_sessions
WHERE is_user_process = 1;

-- 查看连接详细信息
SELECT 
    session_id,
    connect_time,
    net_transport,
    protocol_type,
    protocol_version,
    endpoint_id,
    encrypt_option,
    auth_scheme,
    node_affinity,
    num_reads,
    num_writes,
    last_read,
    last_write,
    net_packet_size,
    client_net_address,
    client_tcp_port,
    local_net_address,
    local_tcp_port,
    connection_id,
    parent_connection_id
FROM sys.dm_exec_connections;

--查看连接是否来自连接池：
SELECT 
    login_name,
    host_name,
    program_name,
    COUNT(*) AS connection_count
FROM sys.dm_exec_sessions
WHERE is_user_process = 1
GROUP BY login_name, host_name, program_name
ORDER BY connection_count DESC;


--查看是否有长时间未活动的连接：
SELECT 
    session_id,
    login_time,
    last_request_end_time,
    DATEDIFF(minute, last_request_end_time, GETDATE()) AS idle_minutes
FROM sys.dm_exec_sessions
WHERE status = 'sleeping'
  AND is_user_process = 1
ORDER BY idle_minutes DESC;












---查看死锁的日志  可用：
SELECT 
    x.value('@timestamp', 'datetime') AS DeadlockTime,
    x.query('.') AS DeadlockGraph
FROM (
    SELECT CAST(target_data AS XML) AS TargetData
    FROM sys.dm_xe_session_targets st
    JOIN sys.dm_xe_sessions s ON s.address = st.event_session_address
    WHERE s.name = 'system_health'
      AND st.target_name = 'ring_buffer'
) AS Data
CROSS APPLY TargetData.nodes('RingBufferTarget/event[@name="xml_deadlock_report"]') AS X(x)
ORDER BY DeadlockTime DESC;