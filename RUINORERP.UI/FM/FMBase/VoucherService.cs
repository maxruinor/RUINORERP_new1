using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.FM.FMBase
{
    public class VoucherService
    {
        private readonly ISqlSugarClient _db;

        public void CreateVoucher(VoucherBizType bizType, int bizId,
            List<VoucherEntry> entries, string description)
        {
            using (var tran = _db.Ado.BeginTransaction())
            {
                // 生成凭证号（规则：FJ+年月+序号）
                string voucherNo = "FJ" + DateTime.Now.ToString("yyyyMM") + "-" +
                    _db.Queryable<AccountingVoucher>()
                        .Where(v => v.VoucherNo.StartsWith("FJ" + DateTime.Now.ToString("yyyyMM")))
                        .Count().ToString("D3");

                // 插入主表
                var voucher = new AccountingVoucher
                {
                    VoucherNo = voucherNo,
                    VoucherDate = DateTime.Now,
                    BizType = bizType,
                    BizId = bizId,
                    Description = description,
                    CreatedBy = CurrentUser.UserId
                };
                _db.Insertable(voucher).ExecuteCommand();

                // 插入分录
                foreach (var entry in entries)
                {
                    entry.VoucherId = voucher.VoucherId;
                    _db.Insertable(entry).ExecuteCommand();
                }

                tran.Commit();
            }
        }
    }
}
