using CacheManager.Core;
using F23.StringSimilarity;
using RUINORERP.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.LogicaService
{
    //重复单位检测算法设计
    /// <summary>
    /// 重复检测结果
    /// </summary>
    public class DuplicateResult
    {
        public bool IsDuplicate { get; set; }
        public tb_CustomerVendor DuplicateEntity { get; set; }
        public double Score { get; set; }
    }

    public interface IDuplicateCheckService
    {
        Task<DuplicateResult> CheckAsync(tb_CustomerVendor input);
    }

    public class DuplicateCheckService : IDuplicateCheckService
    {
        private readonly ISqlSugarClient _db;
        private readonly ICacheManager<object> _cache;

        public DuplicateCheckService(ISqlSugarClient db, ICacheManager<object> cache)
        {
            _db = db;
            _cache = cache;
        }

        public async Task<DuplicateResult> CheckAsync(tb_CustomerVendor input)
        {
            //大于0的为修改时不检测了，如果先随便建。名称再修改为雷同的。这个人为情况。后面再看
            if (input.CustomerVendor_ID > 0)
            {
                return new DuplicateResult { IsDuplicate = false };
            }



            // 1. 轻量级候选集：同城市、同电话、同邮箱等任意一项相同
            var candidates = await _db.Queryable<tb_CustomerVendor>()
                                      .Where(c => c.isdeleted == false &&
                                                  (c.Area == input.Area ||
                                                   c.MobilePhone == input.MobilePhone ||
                                                   c.Email == input.Email ||
                                                   c.Phone == input.Phone))
                                      .ToListAsync();

            if (!candidates.Any())
                return new DuplicateResult { IsDuplicate = false };

            // 2. 权重打分
            double bestScore = 0;
            tb_CustomerVendor best = null;

            foreach (var c in candidates)
            {
                double score = CalcScore(input, c);
                if (score > bestScore)
                {
                    bestScore = score;
                    best = c;
                }
            }

            const double threshold = 0.75;
            return bestScore >= threshold
                ? new DuplicateResult { IsDuplicate = true, DuplicateEntity = best, Score = bestScore }
                : new DuplicateResult { IsDuplicate = false };
        }

        private double CalcScore(tb_CustomerVendor a, tb_CustomerVendor b)
        {
            var jw = new JaroWinkler();   // 只需 new 一次

            double score = 0;

            // 名称 Jaro-Winkler
            score += 0.35 * jw.Similarity(a.CVName, b.CVName);

            // 简称
            if (!string.IsNullOrWhiteSpace(a.ShortName) && !string.IsNullOrWhiteSpace(b.ShortName))
                score += 0.20 * jw.Similarity(a.ShortName, b.ShortName);

            // 电话/手机/邮箱/传真/地址
            score += AddEqualBonus(a.MobilePhone, b.MobilePhone, 0.15);
            score += AddEqualBonus(a.Phone, b.Phone, 0.10);
            score += AddEqualBonus(a.Email, b.Email, 0.10);
            score += AddEqualBonus(a.Fax, b.Fax, 0.05);
            score += AddEqualBonus(a.Address, b.Address, 0.05);

            return Math.Min(score, 1.0);
        }

        private double AddEqualBonus(string x, string y, double weight)
            => string.Equals(x, y, StringComparison.OrdinalIgnoreCase) ? weight : 0;
    }
}
