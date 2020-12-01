using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CCLLC.CDS.Sdk
{
    public class JoinedEntity<P, E, RE> : QueryEntity<IJoinedEntity<P, E, RE>, RE>, IJoinedEntity<P, E, RE>, IJoinedEntitySettings<P, E, RE> where P : IQueryEntity<P, E> where E : Entity, new() where RE : Entity, new()
    {
        protected string JoinAlias { get; private set; }
        protected JoinOperator JoinOperator { get; }
        protected string ParentEntity { get; }
        protected string ParentAttribute { get; }
        protected string RelatedEntity { get; }
        protected string RelatedAttribute { get; }

        public IJoinedEntitySettings<P, E, RE> With => this;

        public JoinedEntity(JoinOperator joinOperator, string parentEntityName, string parentAttributeName, string relatedEntityName, string relatedAttributeName) : base() 
        {
            this.JoinOperator = joinOperator;
            this.ParentEntity = parentEntityName;
            this.ParentAttribute = parentAttributeName;
            this.RelatedEntity = relatedEntityName;
            this.RelatedAttribute = relatedAttributeName;
        }

        public LinkEntity ToLinkEntity()
        {
            var linkEntity = new LinkEntity(ParentEntity, RelatedEntity, ParentAttribute, RelatedAttribute, JoinOperator)
            {
                EntityAlias = string.IsNullOrEmpty(JoinAlias) ? RelatedEntity : JoinAlias,
                Columns = GetColumnSet(),
                LinkCriteria = GetFilterExpression()
            };

            foreach(var je in JoinedEntities)
            {
                linkEntity.LinkEntities.Add(je.ToLinkEntity());
            }   
            
            linkEntity.Orders.AddRange(OrderExpressions);

            return linkEntity;
            
        }


        public IJoinedEntity<P, E, RE> Alias(string aliasName)
        {
            this.JoinAlias = aliasName;
            return this;
        }
    }
}
