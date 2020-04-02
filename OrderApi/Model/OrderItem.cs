using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Model
{
    public class OrderItem
    {
        public long Id { get; set; }
        public string CustomerName { get; set; }
        public string ItemName { get; set; }
        public int ItemValue { get; set; }
        public int ItemQuantity { get; set; }
    }
}
