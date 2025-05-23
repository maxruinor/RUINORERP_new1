﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINOR.Framework.Core.Common.Model
{
    /// <summary>
    /// 分页
    /// </summary>
    public class Pagination
    {
        public Pagination()
        {
            PageIndex = 1;
            PageSize = 10;
            SortFields = new List<string> { "id desc" };
            TotalElements = 0;
        }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页行数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 排序列
        /// </summary>
        public List<string> SortFields { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalElements { get; set; }
    }
}
