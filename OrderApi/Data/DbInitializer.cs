using OrderApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Data
{
    public class DbInitializer
    {
        public static void Initialize(OrderContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.OrderItems.Any())
            {
                return;   // DB has been seeded
            }

            var orders = new OrderItem[]
            {
            new OrderItem{CustomerName="Carson",ItemName="Book-Godan",ItemQuantity=1, ItemValue=150},
             new OrderItem{CustomerName="James",ItemName="T-Shirt",ItemQuantity=2, ItemValue=450},
            new OrderItem{CustomerName="Hales",ItemName="Snacks",ItemQuantity=3, ItemValue=50}
          
            };
            foreach (OrderItem s in orders)
            {
                context.OrderItems.Add(s);
            }
            context.SaveChanges();

           

        }
    }
}
