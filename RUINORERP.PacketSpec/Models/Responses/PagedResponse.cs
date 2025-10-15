using MessagePack;
using System;
using System.Collections.Generic;
using RUINORERP.PacketSpec.Models.Requests;

namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 分页响应基类 - 专门用于分页查询结果的响应
    /// </summary>
    /// <typeparam name="TEntity">实体数据类型</typeparam>
    [MessagePackObject]
    public class PagedResponse<TEntity> : ResponseBase<List<TEntity>>
    {
        /// <summary>
        /// 当前页索引（从0开始）
        /// </summary>
        [Key(13)]
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        [Key(14)]
        public int PageSize { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        [Key(15)]
        public int TotalPages { get; set; }

        /// <summary>
        /// 是否有下一页
        /// </summary>
        [Key(16)]
        public bool HasNextPage { get; set; }

        /// <summary>
        /// 是否有上一页
        /// </summary>
        [Key(17)]
        public bool HasPreviousPage { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public PagedResponse() : base()
        {
        }

        /// <summary>
        /// 创建分页成功响应
        /// </summary>
        /// <param name="data">当前页数据</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="message">成功消息</param>
        /// <returns>分页响应实例</returns>
        public static PagedResponse<TEntity> CreateSuccess(
            List<TEntity> data, 
            int totalCount, 
            int pageIndex, 
            int pageSize, 
            string message = "分页查询成功")
        {
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            
            return new PagedResponse<TEntity>
            {
                Data = data,
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                HasNextPage = pageIndex < totalPages - 1,
                HasPreviousPage = pageIndex > 0,
                IsSuccess = true,
                Message = message,
                Timestamp = DateTime.Now
            };
        }

        /// <summary>
        /// 创建分页失败响应
        /// </summary>
        /// <param name="message">失败消息</param>
        /// <param name="code">错误代码</param>
        /// <param name="extraData">扩展错误信息</param>
        /// <returns>分页响应实例</returns>
        public static PagedResponse<TEntity> CreateError(string message, int code = 500, Dictionary<string, object> extraData = null)
        {
            return new PagedResponse<TEntity>
            {
                IsSuccess = false,
                Message = message,
                Timestamp = DateTime.Now,
                ExtraData = extraData ?? new Dictionary<string, object>()
            };
        }

        /// <summary>
        /// 创建空分页响应
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>空分页响应实例</returns>
        public static PagedResponse<TEntity> CreateEmpty(string message = "暂无数据")
        {
            return new PagedResponse<TEntity>
            {
                Data = new List<TEntity>(),
                TotalCount = 0,
                PageIndex = 0,
                PageSize = 0,
                TotalPages = 0,
                HasNextPage = false,
                HasPreviousPage = false,
                IsSuccess = true,
                Message = message,
                Timestamp = DateTime.Now
            };
        }
    }

    /// <summary>
    /// 分页查询请求类 - 与分页响应配套使用
    /// </summary>
    [MessagePackObject]
    public class PagedRequest : RequestBase
    {
        /// <summary>
        /// 当前页索引（从0开始）
        /// </summary>
        [Key(5)]
        public int PageIndex { get; set; } = 0;

        /// <summary>
        /// 每页大小
        /// </summary>
        [Key(6)]
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// 排序字段
        /// </summary>
        [Key(7)]
        public string SortField { get; set; } = "";

        /// <summary>
        /// 排序方向（ASC/DESC）
        /// </summary>
        [Key(8)]
        public string SortDirection { get; set; } = "ASC";

        /// <summary>
        /// 是否获取总记录数
        /// </summary>
        [Key(9)]
        public bool IncludeTotalCount { get; set; } = true;

        /// <summary>
        /// 创建默认分页请求
        /// </summary>
        public static PagedRequest CreateDefault()
        {
            return new PagedRequest
            {
                PageIndex = 0,
                PageSize = 20,
                SortField = "",
                SortDirection = "ASC",
                IncludeTotalCount = true
            };
        }

        /// <summary>
        /// 创建分页请求
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="sortDirection">排序方向</param>
        public static PagedRequest Create(int pageIndex, int pageSize, string sortField = "", string sortDirection = "ASC")
        {
            return new PagedRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SortField = sortField,
                SortDirection = sortDirection,
                IncludeTotalCount = true
            };
        }

        /// <summary>
        /// 跳过的记录数
        /// </summary>
        [MessagePack.IgnoreMember]
        public int SkipCount => PageIndex * PageSize;

        /// <summary>
        /// 获取分页参数
        /// </summary>
        /// <returns>分页参数元组</returns>
        public (int skip, int take) GetPagingParameters()
        {
            return (SkipCount, PageSize);
        }
    }
}
