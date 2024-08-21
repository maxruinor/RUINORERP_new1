using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model
{
    public class QueryParameter<M>
    {

        private Expression<Func<M, object>> _QueryFieldExpression;
        public Expression<Func<M, object>> QueryFieldExpression
        {
            get { return _QueryFieldExpression; }
            set
            {
                _QueryFieldExpression = value;
                ExpressionHelper.
            }
        }



        public string QueryField { get; set; }
        public Expression<Func<dynamic, bool>> FieldValueExpCondition { get; set; }
        public Expression<Func<C, bool>> SetFieldValueExpCondition<C>(Expression<Func<C, bool>> expression)
        {
            FieldValueExpCondition = expression as Expression<Func<dynamic, bool>>;
            return expression;
        }


    }

    public class QueryParameter
    {
        public string QueryField { get; set; }


    }

}
