using System;
using Microsoft.Xrm.Sdk.Query;

namespace CCLLC.CDS.Sdk
{
    public partial class Filter<P> : Filterable<P>, IFilter<P> where P : IFilterable
    {
        public LogicalOperator Operator { get; }      
     
        public Filter(IFilterable<P> parent, LogicalOperator logicalOperator) : base()
        {
            this.Parent = parent;
            this.Operator = logicalOperator;           
        }

        public IFilter<P> IsActive(bool value = true)
        {
            var condition = new Condition<P>(this, "statecode");
            condition.IsEqualTo<int>((value == true) ? 0 : 1);            
            return (IFilter<P>)this;
        }

        public IFilter<P> HasStatus(params int[] status)
        {
            var condition = new Condition<P>(this, "statuscode");
            condition.IsEqualTo<int>(status);            
            return this;
        }

        public IFilter<P> HasStatus<T>(params T[] status) where T : Enum
        {
            if(status.Length > 0)
            {
                var statusAsInt =  Array.ConvertAll(status, value => (int)(object)value);
                return HasStatus(statusAsInt);
            }
            return this;            
        }

        public FilterExpression ToFilterExpression()
        {
            var filterExpression = new FilterExpression(Operator);
            foreach(var c in Conditions)
            {
                filterExpression.AddCondition(c.ToConditionExpression());
            }            
            foreach(var f in Filters)
            {
                filterExpression.AddFilter(f.ToFilterExpression());
            }           
            
            return filterExpression;            
        }

        public ICondition<P> Attribute(string name)
        {
            return new Condition<P>(this, name);
        }
    }

}
