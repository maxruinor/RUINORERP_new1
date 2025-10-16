﻿using AutoUpdateTools;
using CacheManager.Core;
using FastReport.DevComponents.DotNetBar.Controls;
using FastReport.Table;
using HLH.Lib.Helper;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebSockets;
using System.Windows.Forms;
using System.Windows.Media;

namespace RUINORERP.UI.SysConfig
{
    [MenuAttrAssemblyInfo("缓存管理", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统工具)]
    public partial class UCCacheManage : UserControl
    {
        private readonly IEntityCacheManager _cacheManager;
        private readonly TableSchemaManager _tableSchemaManager;

        public UCCacheManage()
        {
            InitializeComponent();
            // 通过依赖注入获取缓存管理器
            _cacheManager = Startup.GetFromFac<IEntityCacheManager>();
            _tableSchemaManager = TableSchemaManager.Instance;
        }

        private void 请求缓存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBoxTableList.SelectedItem is SuperValue kv)
            {
                string tableName = kv.superDataTypeName;
                UIBizSrvice.RequestCache(tableName);
            }
        }

        private void UCCacheManage_Load(object sender, EventArgs e)
        {
            //这里添加所有缓存
            LoadCacheToUI();
        }

        private void LoadCacheToUI()
        {
            //加载所有缓存的表
            listBoxTableList.Items.Clear();

            // 获取所有可缓存的表名
            List<string> list = new List<string>();
            // 从TableSchemaManager获取所有表信息
            foreach (var schemaInfo in _tableSchemaManager.GetAllSchemaInfo())
            {
                list.Add(schemaInfo.TableName);
            }
            list.Sort();

            foreach (var tableName in list)
            {
                try
                {
                    // 获取实体类型
                    var entityType = _tableSchemaManager.GetEntityType(tableName);
                    if (entityType != null)
                    {
                        // 使用反射调用泛型方法
                        var method = typeof(IEntityCacheManager).GetMethod("GetEntityList", new Type[] { typeof(string) })
                            .MakeGenericMethod(entityType);
                        var cacheList = method.Invoke(_cacheManager, new object[] { tableName }) as System.Collections.IEnumerable;

                        int count = 0;
                        if (cacheList != null)
                        {
                            count = cacheList.Cast<object>().Count();
                        }

                        SuperValue kv = new SuperValue(tableName + "[" + count + "]", tableName);
                        listBoxTableList.Items.Add(kv);
                    }
                }
                catch (Exception ex)
                {
                    // 记录错误但继续处理其他表
                    Console.WriteLine($"处理表 {tableName} 时发生错误: {ex.Message}");
                }
            }

            // 添加锁定信息（如果需要）
            // 注意：这里需要根据新系统的锁定管理器实现进行调整
        }

        private void btnRefreshCache_Click(object sender, EventArgs e)
        {
            LoadCacheToUI();
        }

        /// <summary>
        /// 所有实体表都在这个命名空间下，不需要每次都反射
        /// </summary>
        Assembly assembly = System.Reflection.Assembly.LoadFrom("RUINORERP.Model.dll");
        private void listBoxTableList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxTableList.SelectedItem is SuperValue kv)
            {
                string tableName = kv.superDataTypeName;

                // 处理锁定信息列表（如果需要）
                if (tableName == "锁定信息列表")
                {
                    this.dataGridView1.SetUseCustomColumnDisplay(false);
                    // 这里需要根据新系统的锁定管理器实现进行调整
                    dataGridView1.DataSource = null;
                }
                else
                {
                    try
                    {
                        // 获取实体类型
                        Type type = assembly.GetType("RUINORERP.Model." + tableName);
                        if (type == null)
                        {
                            dataGridView1.DataSource = null;
                            return;
                        }

                        // 使用反射调用泛型方法获取实体列表
                        var method = typeof(IEntityCacheManager).GetMethod("GetEntityList", new Type[] { typeof(string) })
                            .MakeGenericMethod(type);
                        var cacheList = method.Invoke(_cacheManager, new object[] { tableName });

                        if (cacheList == null)
                        {
                            dataGridView1.DataSource = null;
                            return;
                        }

                        // 设置DataGridView属性
                        dataGridView1.NeedSaveColumnsXml = true;
                        dataGridView1.FieldNameList = UIHelper.GetFieldNameColList(true, type);
                        dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView1.XmlFileName = "UCCacheManage" + tableName;
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = cacheList;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"加载表 {tableName} 数据时发生错误: {ex.Message}");
                        dataGridView1.DataSource = null;
                    }
                }
            }
            else
            {
                dataGridView1.DataSource = null;
                return;
            }
        }

        private void 清空选中缓存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBoxTableList.SelectedItem is SuperValue kv)
            {
                string tableName = kv.superDataTypeName;
                
                try
                {
                    // 获取实体类型
                    var entityType = _tableSchemaManager.GetEntityType(tableName);
                    if (entityType != null)
                    {
                        // 创建空列表来更新缓存（清空效果）
                        var emptyListType = typeof(List<>).MakeGenericType(entityType);
                        var emptyList = Activator.CreateInstance(emptyListType);
                        
                        // 调用更新方法来清空缓存
                        _cacheManager.UpdateEntityList(tableName, emptyList);
                        
                        // 刷新UI
                        LoadCacheToUI();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"清空表 {tableName} 缓存时发生错误: {ex.Message}");
                }
            }
        }
    }
}
