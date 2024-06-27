using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class CartItem
    {
        [Key]
        public Guid CartItemId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public Guid CartId { get; set; }

        [ForeignKey("CartId")]
        public Cart Cart { get; set; }
    }
}