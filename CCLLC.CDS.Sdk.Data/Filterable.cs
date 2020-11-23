using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk.Query;

namespace CCLLC.CDS.Sdk
{
    public abstract class Filterable<P> : IFilterable<P> where P : IFilterable
    {
        public IList<FilterExpression> Filters { get; }
        public IList<ConditionExpression> Conditions { get; }
        public virtual IFilterable<P> Parent { get; protected set; }

        protected Filterable()
        {
            Filters = new List<FilterExpression>();
            Conditions = new List<ConditionExpression>();           
        }
       

        public virtual P WhereAll(Action<IFilter<P>> expression)
        {
            _ = expression ?? throw new ArgumentNullException(nameof(expression));

            var filter = new Filter<P>(Parent, LogicalOperator.And);
            expression(filter);
            this.Filters.Add(filter.ToFilterExpression());
            return (P)Parent;
        }

        public virtual P WhereAny(Action<IFilter<P>> expression)
        {
            _ = expression ?? throw new ArgumentNullException(nameof(expression));

            var filter = new Filter<P>(Parent, LogicalOperator.Or);
            expression(filter);
            this.Filters.Add(filter.ToFilterExpression());
            return (P)Parent;
        }

       
    }
}
