using System.ComponentModel.DataAnnotations;

namespace FSE.Assignment23.MVC.Models
{
    public class Item
    {
        [Display(Name = "Item Code")]
        public string Code { get; set; }
        [Display(Name = "Item Description")]
        public string Description { get; set; }
        [Display(Name = "Item Rate")]
        public decimal Rate { get; set; }
    }
}