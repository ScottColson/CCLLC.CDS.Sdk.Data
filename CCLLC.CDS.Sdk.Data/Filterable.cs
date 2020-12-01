using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk.Query;

namespace CCLLC.CDS.Sdk
{
    public abstract class Filterable<P> : IFilterable<P> where P : IFilterable
    {
        public IList<IFilter> Filters { get; }
        public IList<ICondition> Conditions { get; }
        protected IList<string> SearchFields { get; }
        public virtual IFilterable<P> Parent { get; protected set; }

        protected Filterable()
        {
            SearchFields = new List<string>();
            Filters = new List<IFilter>();
            Conditions = new List<ICondition>();           
        }
       

        public virtual P WhereAll(Action<IFilter<P>> expression)
        {
            _ = expression ?? throw new ArgumentNullException(nameof(expression));

            var filter = new Filter<P>(Parent, LogicalOperator.And);
            expression(filter);
            this.Filters.Add(filter);
            return (P)Parent;
        }

        public virtual P WhereAny(Action<IFilter<P>> expression)
        {
            _ = expression ?? throw new ArgumentNullException(nameof(expression));

            var filter = new Filter<P>(Parent, LogicalOperator.Or);
            expression(filter);
            this.Filters.Add(filter);
            return (P)Parent;
        }

        protected virtual FilterExpression GenerateSearchFilter(string searchValue)
        {
            if (SearchFields.Count <= 0 || searchValue is null)
                return null;

            var searchFilter = new FilterExpression(LogicalOperator.Or);
            foreach (var sf in SearchFields)
            {
                searchFilter.AddCondition(new ConditionExpression(sf, ConditionOperator.Like, searchValue));
            }

            return searchFilter;
        }
    }
}
