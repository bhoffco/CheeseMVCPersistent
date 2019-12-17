using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CheeseMVC.ViewModels
{
    public class AddMenuViewModel
    {
        [Required(ErrorMessage = "Please add a name")]
        [Display(Name = "Menu Name")]
        public string Name { get; set; }
        public AddMenuViewModel()
        { }
    }
}

