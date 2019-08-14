using System;
using System.Collections.Generic;
using domainD;

namespace SharedKernel
{
    public class Address : ValueObject
    {
        public Address(string line1, string line2, string postCode)
        {
            Line1 = line1;
            Line2 = line2;
            PostCode = postCode;
        }

        public string Line1 { get; }

        public string Line2 { get; }

        public string PostCode { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Line1;
            yield return Line2;
            yield return PostCode;
        }
    }
}
