using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Cart
    {
        [Key]
        public Guid CartId { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}