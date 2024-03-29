﻿using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Query;

namespace CCLLC.CDS.Sdk
{   
    public interface ICondition
    {
        ConditionExpression ToConditionExpression();
    }

    public interface ICondition<P> : ICondition where P : IFilterable
    {
        IFilter<P> Parent { get; }
        IFilter<P> Is<T>(Microsoft.Xrm.Sdk.Query.ConditionOperator conditionOperator, params T[] values);
        IFilter<P> IsNull();
        IFilter<P> IsNotNull();
        IFilter<P> IsEqualTo<T>(params T[] values);
        IFilter<P> IsNotEqualTo<T>(params T[] values);
        IFilter<P> IsGreaterThanOrEqualTo<T>(T value);
        IFilter<P> IsGreaterThan<T>(T value);
        IFilter<P> IsLessThanOrEqualTo<T>(T value);
        IFilter<P> IsLessThan<T>(T value);
        IFilter<P> IsLike(params string[] values);
        
    }

}
