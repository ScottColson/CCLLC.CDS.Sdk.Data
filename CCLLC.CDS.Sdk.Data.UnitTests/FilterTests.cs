﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CCLLC.CDS.Sdk.Data.UnitTest
{
    using Proxy;

    [TestClass]
    public class FilterTests
    {
        #region WhereAny_Should_AddOrFilterToQueryExpression

        [TestMethod]
        public void Test_WhereAny_Should_AddOrFilterToQueryExpression()
        {
            new WhereAny_Should_AddOrFilterToQueryExpression().Test();
        }

        private class WhereAny_Should_AddOrFilterToQueryExpression : TestMethodClassBase
        {
           
            protected override void Test(IOrganizationService service)
            {
                var qryExpression = new QueryExpressionBuilder<Account>()
                       .WhereAny(a => a
                            .IsActive()
                            .Attribute("accountnumber").IsEqualTo("test"))
                       .Build();

                var criteria = qryExpression.Criteria;

                Assert.AreEqual(LogicalOperator.Or, criteria.FilterOperator);
                Assert.AreEqual(2, criteria.Conditions.Count);
            }
        }

        #endregion WhereAny_Should_AddOrFilterToQueryExpression

        #region WhereAll_Should_AddAndFilterToQueryExpression

        [TestMethod]
        public void Test_WhereAll_Should_AddAndFilterToQueryExpression()
        {
            new WhereAll_Should_AddAndFilterToQueryExpression().Test();
        }

        private class WhereAll_Should_AddAndFilterToQueryExpression : TestMethodClassBase
        {
            protected override void Test(IOrganizationService service)
            {
                var qryExpression = new QueryExpressionBuilder<Account>()
                        .WhereAll(a => a     
                             .QuickFind()
                             .IsActive()
                             .Attribute("accountnumber").IsEqualTo("test")
                             .WhereAll(a1 => a1
                                .Attribute("name").IsEqualTo("testname")))
                        .Build();

                var criteria = qryExpression.Criteria;

                Assert.AreEqual(LogicalOperator.And, criteria.FilterOperator);
                Assert.IsTrue(criteria.IsQuickFindFilter);
                Assert.AreEqual(2, criteria.Conditions.Count);
            }
        }

        #endregion WhereAll_Should_AddAndFilterToQueryExpression

        #region NestedWhereAll_Should_CreateChildAndFilter

        [TestMethod]
        public void Test_NestedWhereAll_Should_CreateChildAndFilter()
        {
            new NestedWhereAll_Should_CreateChildAndFilter().Test();
        }

        private class NestedWhereAll_Should_CreateChildAndFilter : TestMethodClassBase
        {
            
            protected override void Test(IOrganizationService service)
            {
                var qryExpression = new QueryExpressionBuilder<Account>()
                       .WhereAny(a => a
                            .IsActive()
                            .Attribute("accountnumber").IsEqualTo("test")
                            .WhereAll(a1 => a1
                                .HasStatus<Account.eStatusCode>(Account.eStatusCode.Active)
                                .Attribute("name").IsNotNull()))
                       .Build();

                var criteria = qryExpression.Criteria;

                Assert.AreEqual(LogicalOperator.Or, criteria.FilterOperator);
                Assert.AreEqual(2, criteria.Conditions.Count);
                Assert.AreEqual(1, criteria.Filters.Count);

                criteria = criteria.Filters[0];
                Assert.AreEqual(LogicalOperator.And, criteria.FilterOperator);
                Assert.AreEqual(2, criteria.Conditions.Count);
            }
        }

        #endregion NestedWhereAll_Should_CreateChildAndFilter

        #region MultipleConditionValues_Should_CreateNestedOrFilter

        [TestMethod]
        public void Test_MultipleConditionValues_Should_CreateNestedOrFilter()
        {
            new MultipleConditionValues_Should_CreateNestedOrFilter().Test();
        }

        private class MultipleConditionValues_Should_CreateNestedOrFilter : TestMethodClassBase
        {       
            protected override void Test(IOrganizationService service)
            {
                var qryExpression = new QueryExpressionBuilder<Account>()
                        .WhereAll(a => a
                             .IsActive()
                             .Attribute("accountnumber").IsEqualTo("test","test2","test3"))
                        .Build();

                var criteria = qryExpression.Criteria;

                Assert.AreEqual(LogicalOperator.And, criteria.FilterOperator);
                Assert.AreEqual(1, criteria.Conditions.Count);
                Assert.AreEqual(1, criteria.Filters.Count);

                criteria = criteria.Filters[0];
                Assert.AreEqual(LogicalOperator.Or, criteria.FilterOperator);
                Assert.AreEqual(3, criteria.Conditions.Count);
            }
        }

        #endregion MultipleConditionValues_Should_CreateNestedOrFilter

        #region ConditionalAttribute_Should_BeIgnored

        [TestMethod]
        public void Test_ConditionalAttribute_Should_BeIgnored()
        {
            new ConditionalAttribute_Should_BeIgnored().Test();
        }

        private class ConditionalAttribute_Should_BeIgnored : TestMethodClassBase
        {
            protected override void Test(IOrganizationService service)
            {
                var qryExpression = new QueryExpressionBuilder<Account>()
                        .WhereAll(a => a
                             .QuickFind()
                             .IsActive()
                             .Attribute(false, "accountnumber").IsEqualTo("test")
                             .WhereAll(a1 => a1
                                .Attribute("name").IsEqualTo("testname")))
                        .Build();

                var criteria = qryExpression.Criteria;

                Assert.AreEqual(LogicalOperator.And, criteria.FilterOperator);
                Assert.IsTrue(criteria.IsQuickFindFilter);
                Assert.AreEqual(1, criteria.Conditions.Count);
            }
        }

        #endregion WhereAll_Should_AddAndFilterToQueryExpression
    }
}
