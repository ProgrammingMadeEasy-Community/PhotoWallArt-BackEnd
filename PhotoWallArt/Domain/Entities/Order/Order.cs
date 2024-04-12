using System;
using System.Collections.Generic;
using Domain.Common;
using Domain.Entities.Order;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Orders
{
    public class Order : BaseEntity
    {
        public int OrderId { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime PlacedAt { get; set; }
        public string Status { get; set; }
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

        public Order()
        {
            PlacedAt = DateTime.Now; 
            Status = "pending";      
            OrderItems = new List<OrderItem>(); 
        }
    }

   

}
