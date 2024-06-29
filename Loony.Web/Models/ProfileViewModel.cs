using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace Loony.Web.Models
{
    public class ProfileViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Required field")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Required field")]
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        [Required(ErrorMessage = "Required field")]
        public string Email { get; set; }
        [DataType(DataType.Password), StringLength(20, MinimumLength = 4)]
        public string Password1 { get; set; }
        [Compare(nameof(Password1), ErrorMessage = "Passwords mismatch")]
        [DataType(DataType.Password), StringLength(20, MinimumLength = 4)]
        public string Password2 { get; set; }
        public string Phone { get; set; }
        public int Hit { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string LastLoginIP { get; set; }
        public byte[] Avatar { get; set; }
        [Required(ErrorMessage = "Required field")]
        public int LanguageId { get; set; }
        public SelectList Languages { get; set; }


        //public List<SelectListItem> Genders { get; } = new List<SelectListItem>
        //{
        //    new SelectListItem { Value = "M", Text = "Male" },
        //    new SelectListItem { Value = "F", Text = "Female" },
        //};

        public enum Gender { Male, Female }

    }
}
