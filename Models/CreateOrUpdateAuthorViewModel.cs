using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DotNet5_CRUD.Models
{
    public class CreateOrUpdateAuthorViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
    }
}
