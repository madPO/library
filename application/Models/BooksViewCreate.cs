using System.ComponentModel.DataAnnotations;

namespace application.Models
{
    public class BooksViewCreate
    {
        [Required]
        [Display(Name = "Название книги")]
        public virtual string Name { get; set; }
        [Required]
        [Display(Name = "Автора книги")]
        public virtual string Author { get; set; }
        [Required]
        [Display(Name = "Издательство")]
        public virtual string Publisher { get; set; }
        [Required]
        [Display(Name = "Дата публикации")]
        public virtual string Date { get; set; }
        [Display(Name = "Описание")]
        public virtual string Description { get; set; }
        [Display(Name = "Количество книг")]
        public virtual int Count { get; set; }
    }
}