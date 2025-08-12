using RUINORERP.Global.EnumExt;
using RUINORERP.Model.ReminderModel.ReminderResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.ReminderService
{
    public class ReminderResultManager
    {
        private readonly List<IReminderResult> _results = new List<IReminderResult>();
        private readonly object _lock = new object();

        public void AddResult(IReminderResult result)
        {
            lock (_lock)
            {
                _results.Add(result);
                // 触发新结果事件
                OnNewResult?.Invoke(this, result);
            }
        }

        public IEnumerable<IReminderResult> GetResults(ReminderBizType? type = null)
        {
            lock (_lock)
            {
                return type.HasValue
                    ? _results.Where(r => r.BusinessType == type.Value).ToList()
                    : _results.ToList();
            }
        }

        public IEnumerable<IReminderResult> GetUnreadResults()
        {
            lock (_lock)
            {
                return _results.Where(r => !r.IsRead).ToList();
            }
        }

        public IReminderResult MarkAsRead(string resultId)
        {
            lock (_lock)
            {
                var result = _results.FirstOrDefault(r => r.ResultId == resultId);
                if (result != null)
                {
                    result.IsRead = true;
                }
                return result;
            }
        }

        public void ArchiveOldResults(TimeSpan maxAge)
        {
            lock (_lock)
            {
                var cutoff = DateTime.Now - maxAge;
                foreach (var result in _results.Where(r => r.TriggerTime < cutoff))
                {
                    result.IsArchived = true;
                }
            }
        }

        // 新结果事件
        public event EventHandler<IReminderResult> OnNewResult;
    }
}
