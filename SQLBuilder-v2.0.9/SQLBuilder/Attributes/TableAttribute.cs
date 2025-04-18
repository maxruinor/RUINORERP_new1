﻿#region License
/***
 * Copyright © 2018-2021, 张强 (943620963@qq.com).
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * without warranties or conditions of any kind, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#endregion

using System;

namespace SQLBuilder.Attributes
{
    /// <summary>
    /// 指定表名
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">数据库表名</param>
        public TableAttribute(string name = null)
        {
            if (name != null)
                this.Name = name;
        }

        /// <summary>
        /// 数据库表名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 数据库模式
        /// </summary>
        public string Schema { get; set; }
    }
}
