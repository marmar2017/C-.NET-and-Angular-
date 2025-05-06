using System.ComponentModel.DataAnnotations;

namespace coffeeOrder.Models
{
    public class CoffeeOrderPayload
    {
        [Required]
        public string Size { get; set; }  // Options: Small, Medium, Large
        [Required]
        public string CoffeeType { get; set; }  // Options: Latte, Espresso, Cappuccino, etc.
        [Required]
        public string MilkType { get; set; } // Options: Whole, Skim, Oat, Almond, Soy, None
        [Range(0, 10)]
        public int SugarCount { get; set; } = 1; // Number of sugar units
        [Required]
        public bool ExtraShot { get; set; } // Whether to add an extra espresso shot
        
        public string Temperature { get; set; } // Options: Hot, Iced
        public string? Notes { get; set; }  // Any special instructions
    }
}
