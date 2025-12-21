using RUINORERP.Model;
using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RUINORERP.UI.ProductEAV.Core
{
    /// <summary>
    /// 产品属性服务
    /// 职责：属性和属性值的加载、缓存管理
    /// </summary>
    public class ProductAttrService
    {
        private static Dictionary<string, object> _cache = new Dictionary<string, object>();
        private static Dictionary<string, DateTime> _cacheExpiry = new Dictionary<string, DateTime>();
        private const int CacheMinutes = 10;

        /// <summary>
        /// 获取所有产品（支持缓存）
        /// </summary>
        public async Task<List<tb_Prod>> GetAllProductsAsync()
        {
            return await Task.Run(() =>
            {
                const string cacheKey = "AllProducts";
                if (TryGetCache(cacheKey, out List<tb_Prod> cached))
                {
                    return cached;
                }

                var products = MainForm.Instance.AppContext.Db
                    .Queryable<tb_Prod>()
                    .OrderBy(p => p.ProductNo)
                    .ToList();

                SetCache(cacheKey, products);
                return products;
            });
        }

        /// <summary>
        /// 获取所有属性（支持缓存）
        /// </summary>
        public async Task<List<tb_ProdProperty>> GetAllPropertiesAsync()
        {
            return await Task.Run(() =>
            {
                const string cacheKey = "AllProperties";
                if (TryGetCache(cacheKey, out List<tb_ProdProperty> cached))
                {
                    return cached;
                }

                var properties = MainForm.Instance.AppContext.Db
                    .Queryable<tb_ProdProperty>()
                    .OrderBy(p => p.Property_ID)
                    .ToList();

                SetCache(cacheKey, properties);
                return properties;
            });
        }

        /// <summary>
        /// 根据属性ID获取属性值（支持缓存）
        /// </summary>
        public async Task<List<tb_ProdPropertyValue>> GetPropertyValuesAsync(long propertyId)
        {
            return await Task.Run(() =>
            {
                var cacheKey = $"PropertyValues_{propertyId}";
                if (TryGetCache(cacheKey, out List<tb_ProdPropertyValue> cached))
                {
                    return cached;
                }

                var values = MainForm.Instance.AppContext.Db
                    .Queryable<tb_ProdPropertyValue>()
                    .Where(v => v.Property_ID == propertyId)
                    .OrderBy(v => v.SortOrder)
                    .ToList();

                SetCache(cacheKey, values);
                return values;
            });
        }

        /// <summary>
        /// 获取产品的所有SKU详情
        /// </summary>
        public async Task<List<tb_ProdDetail>> GetProductDetailsAsync(long prodBaseId)
        {
            return await Task.Run(() =>
            {
                return MainForm.Instance.AppContext.Db
                    .Queryable<tb_ProdDetail>()
                    .Where(d => d.ProdBaseID == prodBaseId)
                    .OrderBy(d => d.SKU)
                    .ToList();
            });
        }

        /// <summary>
        /// 获取产品的属性关系
        /// </summary>
        public async Task<List<tb_Prod_Attr_Relation>> GetAttrRelationsAsync(long prodBaseId)
        {
            return await Task.Run(() =>
            {
                return MainForm.Instance.AppContext.Db
                    .Queryable<tb_Prod_Attr_Relation>()
                    .Where(r => r.ProdBaseID == prodBaseId)
                    .ToList();
            });
        }

        /// <summary>
        /// 检查属性关系是否存在
        /// </summary>
        public async Task<bool> CheckAttributeRelationExistsAsync(long prodDetailId, long propertyId, long propertyValueId)
        {
            return await Task.Run(() =>
            {
                return MainForm.Instance.AppContext.Db
                    .Queryable<tb_Prod_Attr_Relation>()
                    .Any(r => r.ProdDetailID == prodDetailId &&
                              r.Property_ID == propertyId &&
                              r.PropertyValueID == propertyValueId);
            });
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public void ClearCache()
        {
            _cache.Clear();
            _cacheExpiry.Clear();
        }

        /// <summary>
        /// 清除特定缓存
        /// </summary>
        public void ClearCache(string key)
        {
            _cache.Remove(key);
            _cacheExpiry.Remove(key);
        }

        // 缓存辅助方法
        private bool TryGetCache<T>(string key, out T value) where T : class
        {
            value = null;

            if (!_cache.ContainsKey(key))
            {
                return false;
            }

            if (_cacheExpiry.ContainsKey(key) && DateTime.Now > _cacheExpiry[key])
            {
                _cache.Remove(key);
                _cacheExpiry.Remove(key);
                return false;
            }

            value = _cache[key] as T;
            return value != null;
        }

        private void SetCache<T>(string key, T value) where T : class
        {
            _cache[key] = value;
            _cacheExpiry[key] = DateTime.Now.AddMinutes(CacheMinutes);
        }
    }
}
