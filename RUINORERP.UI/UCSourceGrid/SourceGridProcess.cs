using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global.Model;
using RUINORERP.UI.Common;
namespace RUINORERP.UI.UCSourceGrid
{

    /// <summary>
    /// 特殊情况时，比方 ，一个grid中要选择两个产品时。
    /// 要手动指定列的值及来源。
    /// 如：产品转换单中，来源的列有SKU,name，目标也有。 选择一个列进行编辑时。会从产品查询UI中选择一个产品。则要指定目标列的值来源。并且可以指定多个关联的。从SKU可以传过来 产品产品名称 属性规则 等。
    /// 这时就要有一个固定的映射关系。
    /// </summary>
    public class SourceToTargetMatchCol
    {
        string _SourceColName;

        string _TargetToColName;

        public string SourceColName { get => _SourceColName; set => _SourceColName = value; }

        public string TargetToColName { get => _TargetToColName; set => _TargetToColName = value; }

        Type _SourceType;
        public Type SourceType { get => _SourceType; set => _SourceType = value; }

        Type _TargetType;
        public Type TargetType { get => _TargetType; set => _TargetType = value; }

        public SourceToTargetMatchCol GetSourceToTargetMatchCol<Source, Target>(Expression<Func<Source, object>> colNameSourceExp, Expression<Func<Target, object>> colNameTargetExp)
        {
            var sourceColName = colNameSourceExp.GetMemberInfo().Name;
            var targetColName = colNameTargetExp.GetMemberInfo().Name;
            SourceToTargetMatchCol sourceToTargetMatch = new SourceToTargetMatchCol();
            sourceToTargetMatch.SourceColName = sourceColName;
            sourceToTargetMatch.TargetToColName = targetColName;
            sourceToTargetMatch.SourceType = typeof(Source);
            sourceToTargetMatch.TargetType = typeof(Target);
            return sourceToTargetMatch;
        }

    }
}
