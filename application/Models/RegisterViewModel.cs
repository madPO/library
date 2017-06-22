using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace application.Models
{
    public class RegisterViewModel
    {

        public RegisterViewModel()
        {
            Groups = new List<SelectListItem>();
        }

        [Required]
        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "ФИО")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        public Groups Group { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "Пароли не совпадают!")]
        public string ConfirmPassword { get; set; }

        public IEnumerable<SelectListItem> Groups { get; set; }

        [Required]
        [Display(Name = "Группа пользователя")]
        public string group_id { get; set; }
    }
}