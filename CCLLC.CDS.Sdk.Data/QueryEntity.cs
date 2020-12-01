using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Query;
using System.Linq.Expressions;
using System.Linq;

namespace CCLLC.CDS.Sdk
{
    public abstract class QueryEntity<P,E> : Filterable<P>, IQueryEntity<P,E> where P : IQueryEntity<P,E> where E : Entity, new()
    {   
        protected IList<IList<string>> Columnsets { get; }
        protected IList<IJoinedEntity> JoinedEntities { get; }
        protected IList<OrderExpression> OrderExpressions { get; }
                
        protected string RecordType { get; }

        protected QueryEntity() : base()
        {
            var record = new E();
            RecordType = record.LogicalName;
            Columnsets = new List<IList<string>>();
            JoinedEntities = new List<IJoinedEntity>();
            OrderExpressions = new List<OrderExpression>();
            this.Parent = this;
        }

        public P LeftJoin<RE>(string fromAttributeName, string toAttributeName, Action<IJoinedEntity<P, E, RE>> expression) where RE : Entity, new()
        {
            _ = expression ?? throw new ArgumentNullException(nameof(expression));

            var relatedRecordType = new RE().LogicalName;

            var joinEntity = new JoinedEntity<P,E, RE>(JoinOperator.LeftOuter, RecordType, fromAttributeName, relatedRecordType, toAttributeName);
            expression(joinEntity);
            JoinedEntities.Add(joinEntity);
            return (P)Parent;
        }

       

        public P InnerJoin<RE>(string fromAttributeName, string toAttributeName, Action<IJoinedEntity<P, E, RE>> expression) where RE : Entity, new()
        {
            _ = expression ?? throw new ArgumentNullException(nameof(expression));

            var relatedRecordType = new RE().LogicalName;

            var joinEntity = new JoinedEntity<P, E ,RE>(JoinOperator.Inner, RecordType, fromAttributeName, relatedRecordType, toAttributeName);
            expression(joinEntity);
            JoinedEntities.Add(joinEntity);
            return (P)Parent;
        }        

        public P Select(params string[] columns)
        {            
            Columnsets.Add(new List<string>(columns));            
            return (P)Parent;          
        }

        public P Select(Expression<Func<E, object>> anonymousTypeInitializer)
        {
            var columns = anonymousTypeInitializer.GetAttributeNamesArray<E>();
            return Select(columns);
        }

        public P SelectAll()
        {
            return Select(new string[] { "*" });            
        }

        public P OrderByAsc(params string[] columns)
        {
            foreach (var c in columns)
            {
                OrderExpressions.Add(new OrderExpression(c, OrderType.Ascending));
            }

            return (P)Parent;
        }

        public P OrderByDesc(params string[] columns)
        {
            foreach (var c in columns)
            {
                OrderExpressions.Add(new OrderExpression(c, OrderType.Descending));
            }

            return (P)Parent;
        }

        protected FilterExpression GetFilterExpression()
        {
            if (Filters.Count == 0) return new FilterExpression();

            if (Filters.Count == 1) return Filters[0].ToFilterExpression();

            // Wrap multiple filters in an AND filter.
            var filterExpression = new FilterExpression(LogicalOperator.And);
            foreach(var f in Filters)
            {
                filterExpression.Filters.Add(f.ToFilterExpression());
            }
            

            return filterExpression;
        }
           
        protected virtual ColumnSet GetColumnSet()
        {
            var uniqueColumns = new List<string>();
                      

            foreach (var cs in this.Columnsets)
            {
                if (isSelectAllColumnSet(cs))
                    return new ColumnSet(true);

                foreach (var c in cs)
                {
                    if (!uniqueColumns.Contains(c))
                    {
                        uniqueColumns.Add(c);
                    }
                }
            }

            return new ColumnSet(uniqueColumns.ToArray());
        }
           
        private static bool isSelectAllColumnSet(IList<string> columns)
        {
            return (columns.Where(v => v.Equals("*", StringComparison.Ordinal)).FirstOrDefault() != null);
        }

       

       
    }
}
