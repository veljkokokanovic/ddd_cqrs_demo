using System;
using System.Collections.Generic;
using System.Text;

namespace ReadModel.Delivery
{
    public class Address
    {
        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string PostCode { get; set; }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Line1, Line2, PostCode);
        }
    }
}
