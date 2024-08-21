using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.WF.BizOperation.Condition
{
    public class ConditionRegistry
    {
        private readonly Dictionary<string, Type> _conditionTypes = new Dictionary<string, Type>();

        public void RegisterCondition(string key, Type conditionType)
        {
            _conditionTypes[key] = conditionType;
        }

        public IWorkflowCondition GetCondition(string key)
        {
            if (_conditionTypes.TryGetValue(key, out var conditionType))
            {
                return (IWorkflowCondition)Activator.CreateInstance(conditionType);
            }
            throw new ArgumentException("Condition type not found.", nameof(key));
        }
    }
}
