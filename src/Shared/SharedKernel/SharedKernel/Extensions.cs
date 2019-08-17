using System;
using System.Linq;

namespace SharedKernel
{
    public static class Extensions
    {
        public static Address ToAddress(this string address)
        {
            if(string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            var parts = address.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            if(parts.Length < 3)
            {
                throw new InvalidOperationException("Address must contain at least 3 lines representing line1, line2 and post code");
            }

            return new Address(parts.ElementAt(0), parts.ElementAt(1), parts.ElementAt(2));
        }

        public static string Stringify(this Address address)
        {
            if(address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            return string.Join(Environment.NewLine, address.Line1, address.Line2, address.PostCode);
        }
    }
}
