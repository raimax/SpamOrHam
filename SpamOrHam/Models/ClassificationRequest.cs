using System.ComponentModel.DataAnnotations;

namespace SpamOrHam.Models
{
    public class ClassificationRequest
    {
        [Required]
        public string Content { get; set; }
    }
}
