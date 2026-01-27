using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 列映射配置管理器
    /// 用于管理列映射配置的保存、加载和管理
    /// </summary>
    public class ColumnMappingManager
    {
        private readonly string _configPath;
        private readonly XmlSerializer _serializer;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ColumnMappingManager()
        {
            // 设置配置文件保存路径
            _configPath = ColumnMappingConstants.GetConfigFilePath();
            if (!Directory.Exists(_configPath))
            {
                Directory.CreateDirectory(_configPath);
            }

            // 初始化XmlSerializer
            _serializer = new XmlSerializer(typeof(ColumnMappingCollection));
        }

        /// <summary>
        /// 保存列映射配置
        /// </summary>
        /// <param name="mappings">列映射配置集合</param>
        /// <param name="mappingName">映射配置名称</param>
        /// <param name="entityType">实体类型名称</param>
        /// <exception cref="ArgumentException">当映射配置名称为空时抛出</exception>
        /// <exception cref="Exception">当保存过程中发生错误时抛出</exception>
        public void SaveMapping(ColumnMappingCollection mappings, string mappingName, string entityType = null)
        {
            if (string.IsNullOrEmpty(mappingName))
            {
                throw new ArgumentException("映射配置名称不能为空");
            }

            try
            {
                // 更新映射配置的元数据
                foreach (var mapping in mappings)
                {
                    mapping.MappingName = mappingName;
                    mapping.EntityType = entityType;
                    mapping.UpdateTime = DateTime.Now;
                    if (mapping.CreateTime == DateTime.MinValue)
                    {
                        mapping.CreateTime = DateTime.Now;
                    }
                }

                // 保存到XML文件（包含实体类型信息）
                string filePath = Path.Combine(_configPath, $"{mappingName}.xml");
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    _serializer.Serialize(writer, mappings);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"保存列映射配置失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 加载列映射配置
        /// </summary>
        /// <param name="mappingName">映射配置名称</param>
        /// <param name="entityType">目标实体类型（可选，用于运行时获取字段元信息）</param>
        /// <returns>加载的列映射配置集合</returns>
        /// <exception cref="ArgumentException">当映射配置名称为空时抛出</exception>
        /// <exception cref="FileNotFoundException">当配置文件不存在时抛出</exception>
        /// <exception cref="Exception">当加载过程中发生错误时抛出</exception>
        public ColumnMappingCollection LoadMapping(string mappingName, Type entityType = null)
        {
            if (string.IsNullOrEmpty(mappingName))
            {
                throw new ArgumentException("映射配置名称不能为空");
            }

            string filePath = Path.Combine(_configPath, $"{mappingName}.xml");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("指定的列映射配置文件不存在", filePath);
            }

            try
            {
                using StreamReader reader = new StreamReader(filePath);
                return (ColumnMappingCollection)_serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw new Exception($"加载列映射配置失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取所有保存的映射配置名称
        /// </summary>
        /// <returns>映射配置名称列表</returns>
        public List<string> GetAllMappingNames()
        {
            List<string> mappingNames = new List<string>();

            try
            {
                string[] files = Directory.GetFiles(_configPath, "*.xml");
                foreach (string file in files)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    mappingNames.Add(fileName);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"获取映射配置列表失败: {ex.Message}", ex);
            }

            return mappingNames;
        }

        /// <summary>
        /// 删除列映射配置
        /// </summary>
        /// <param name="mappingName">映射配置名称</param>
        /// <exception cref="ArgumentException">当映射配置名称为空时抛出</exception>
        /// <exception cref="FileNotFoundException">当配置文件不存在时抛出</exception>
        public void DeleteMapping(string mappingName)
        {
            if (string.IsNullOrEmpty(mappingName))
            {
                throw new ArgumentException("映射配置名称不能为空");
            }

            string filePath = Path.Combine(_configPath, $"{mappingName}.xml");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("指定的列映射配置文件不存在", filePath);
            }

            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"删除列映射配置失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 检查映射配置是否存在
        /// </summary>
        /// <param name="mappingName">映射配置名称</param>
        /// <returns>是否存在</returns>
        public bool MappingExists(string mappingName)
        {
            if (string.IsNullOrEmpty(mappingName))
            {
                return false;
            }

            string filePath = Path.Combine(_configPath, $"{mappingName}.xml");
            return File.Exists(filePath);
        }
    }
}