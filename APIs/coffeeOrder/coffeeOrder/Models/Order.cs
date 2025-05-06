using System;


namespace coffeeOrder.Models
{
    public enum OrderStatus
{
    Pending = 0,
    InProgress = 1,
    Ready = 2, 
    Failed = 3
}

    public class Order
    { 
        public Guid Id { get; set; }
        public required string PayloadJson { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int RetryCount { get; set; }


    }
}
