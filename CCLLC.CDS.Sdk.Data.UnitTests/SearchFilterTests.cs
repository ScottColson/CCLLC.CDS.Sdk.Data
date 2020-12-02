using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CCLLC.CDS.Sdk.Data.UnitTest
{
    using Proxy;

    [TestClass]
    public class SearchFilterTests
    {
        [TestMethod]
        public void Test_WithSearchFields_Should_CreateOrFilter()
        {
            new WithSearchFields_Should_CreateOrFilter().Test();
        }

        private class WithSearchFields_Should_CreateOrFilter : TestMethodClassBase
        {           
            protected override void Test(IOrganizationService service)
            {
                var qryExpression = new QueryExpressionBuilder<Account>()                    
                       .WhereAll(a => a
                            .IsActive()
                            .WithSearchFields<Account>(cols => new { cols.Name }))                            
                       .WithSearchValue("mytestvalue")
                       .Build();

                var criteria = qryExpression.Criteria;

                Assert.AreEqual(LogicalOperator.And, criteria.FilterOperator);
                Assert.AreEqual(1, criteria.Filters.Count);
                Assert.AreEqual(1, criteria.Conditions.Count);

                var searchFilter = criteria.Filters[0];
                Assert.AreEqual(LogicalOperator.Or, searchFilter.FilterOperator);
                Assert.AreEqual(1, searchFilter.Conditions.Count);
            }
        }

    }
}
