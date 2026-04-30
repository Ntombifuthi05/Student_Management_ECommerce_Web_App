using System.ComponentModel.DataAnnotations;

namespace ASPNETCore_DB.Models
{
    public class Consumer
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? ImagePath { get; set; }
    }
}

