using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ntier.DAL.Entities
{
    public partial class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string? CreateAt { get; set; }
        public string? Address { get; set; }

        public virtual AppUser? User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
