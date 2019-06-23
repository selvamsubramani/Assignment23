using System.ComponentModel.DataAnnotations;

namespace FSE.Assignment23.MVC.Models
{
    public class Supplier
    {
        [Display(Name = "Supplier Code")]
        public string Code { get; set; }
        [Display(Name = "Supplier Name")]
        public string Name { get; set; }
        [Display(Name = "Supplier Address")]
        public string Address { get; set; }
    }
}