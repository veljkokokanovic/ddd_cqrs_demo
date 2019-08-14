using System;
using System.Collections.Generic;
using domainD;

namespace SharedKernel
{
    public class Delivery : ValueObject
    {
        public Delivery(Address deliveryAddress, DateTime deliveryDate, string phoneNumber)
        {
            DeliveryAddress = deliveryAddress;
            DeliveryDate = deliveryDate;
            PhoneNumber = phoneNumber;
        }

        public Address DeliveryAddress { get; }

        public DateTime DeliveryDate { get; }

        public string PhoneNumber { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return DeliveryAddress;
            yield return DeliveryDate;
            yield return PhoneNumber;
        }
    }
}
