using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NewsPortal.Models
{
    public class News
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Заголовок обязателен")]
        public string Title { get; set; }


        [Required(ErrorMessage = "Подзаголовок обязателен")]
        public string Subtitle { get; set; }
        

        [Required(ErrorMessage = "Текст обязателен")]
        public string Text { get; set; }


        [Required(ErrorMessage = "Изображение обязателено")]
        [Url(ErrorMessage = "Некорректный URL изображения")]
        public string ImageUrl { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [NotMapped]
        public IFormFile? ImageFile { get; set; }




    }
}
