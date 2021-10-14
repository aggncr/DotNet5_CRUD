using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DotNet5_CRUD.Models
{
    public class CreateOrUpdateBookViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Page Size")]
        [Range(50, 1000)]
        public int PageSize { get; set; }

        [BindProperty]
        public List<SelectListItem> Authors { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Author")]
        public int? AuthorId { get; set; }

        public CreateOrUpdateBookViewModel()
        {
            Authors = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Choose" }
            };
        }
    }
}
