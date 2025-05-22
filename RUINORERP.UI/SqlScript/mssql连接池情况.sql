 
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