using System;
using System.Linq.Expressions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CCLLC.CDS.Sdk
{
    public class QueryExpressionBuilder<E> : FluentQuery<IQueryExpressionBuilder<E>,E>, IQueryExpressionBuilder<E> where E : Entity, new()
    {
        protected string SearchValue { get; private set; }
        public QueryExpressionBuilder() : base()
        {
            SearchValue = null;
        }

        public QueryExpression Build()
        {
            return this.GetQueryExpression(SearchValue);
        }

        public IQueryExpressionBuilder<E> WithSearchValue(string searchValue)
        {
            SearchValue = searchValue;
            return this;
        }
    }
}
