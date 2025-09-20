using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingDAL.Domains
{
    public class Cart
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }

        public ApplicationUser Customer { get; set; } = null!;
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    }



}



