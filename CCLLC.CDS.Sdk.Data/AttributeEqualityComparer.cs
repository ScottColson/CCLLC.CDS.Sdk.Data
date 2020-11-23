using System;
using Microsoft.Xrm.Sdk;

namespace CCLLC.CDS.Sdk
{
    public class AttributeEqualityComparer : IAttributeEqualityComparer
    {
        public new bool Equals(object x, object y)
        {
            if ((x is null || (x.GetType() == typeof(string) && string.IsNullOrEmpty(x as string)))
                && (y is null || (y.GetType() == typeof(string) && string.IsNullOrEmpty(y as string))))
                return true;

            if (x is null && y != null
                || x != null && y is null)
                return false;

            if (x.GetType() != y.GetType())
                return false;

            if (x.GetType() == typeof(OptionSetValue))
                return ((OptionSetValue)x).Value == ((OptionSetValue)y).Value;

            if (x.GetType() == typeof(BooleanManagedProperty))
                return ((BooleanManagedProperty)x).Value == ((BooleanManagedProperty)y).Value;

            if (x.GetType() == typeof(EntityReference))
                return ((EntityReference)x).LogicalName == ((EntityReference)y).LogicalName
                    && ((EntityReference)x).Id == ((EntityReference)y).Id;

            if (x.GetType() == typeof(Money))
                return (((Money)x).Value == ((Money)y).Value);
            
            if (x.GetType() == typeof(DateTime) || x.GetType() == typeof(DateTime?))
                return Math.Abs(((DateTime)x - (DateTime)y).TotalSeconds) < 1;

            return x.Equals(y);
        }

        public int GetHashCode(object obj)
        {
            return obj.GetHashCode();
        }
    }
}
