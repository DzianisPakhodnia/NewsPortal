using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NewsPortal.Models
{
    public class News
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string? Subtitle { get; set; }
        public string Text { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }

}
