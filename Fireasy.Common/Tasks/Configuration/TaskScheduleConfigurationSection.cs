﻿// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Fireasy.Common.Configuration;
using Fireasy.Common.Extensions;
using System;
using System.Xml;
using System.Collections.Generic;
using Fireasy.Common.Serialization;
#if NETSTANDARD
using Microsoft.Extensions.Configuration;
#endif

namespace Fireasy.Common.Tasks.Configuration
{
    /// <summary>
    /// 提供对任务调度管理器的配置管理。对应的配置节为 fireasy/taskSchedulers(.net framework) 或 fireasy:taskSchedulers(.net core)。
    /// </summary>
    [ConfigurationSectionStorage("fireasy/taskSchedulers")]
    public sealed class TaskScheduleConfigurationSection : ManagableConfigurationSection<TaskScheduleConfigurationSetting>
    {
        /// <summary>
        /// 使用配置节点对当前配置进行初始化。
        /// </summary>
        /// <param name="section">对应的配置节点。</param>
        public override void Initialize(XmlNode section)
        {
            InitializeNode(
                section,
                "scheduler",
                func: node => new TaskScheduleConfigurationSetting
                {
                    Name = node.GetAttributeValue("name"),
                    SchedulerType = Type.GetType(node.GetAttributeValue("type"), false, true)
                });

            //取默认实例
            DefaultInstanceName = section.GetAttributeValue("default");

            base.Initialize(section);
        }

#if NETSTANDARD
        /// <summary>
        /// 使用配置节点对当前配置进行初始化。
        /// </summary>
        /// <param name="configuration">对应的配置节点。</param>
        public override void Bind(IConfiguration configuration)
        {
            Bind(configuration,
                "settings",
                func: c => new TaskScheduleConfigurationSetting
                {
                    Name = c.Key,
                    SchedulerType = Type.GetType(c.GetSection("type").Value, false, true)
                });

            //取默认实例
            DefaultInstanceName = configuration.GetSection("default").Value;

            base.Bind(configuration);
        }
#endif
    }
}
