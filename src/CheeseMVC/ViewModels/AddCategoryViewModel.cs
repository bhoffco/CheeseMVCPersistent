﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace CheeseMVC.ViewModels
{
 
   public class AddCategoryViewModel
    {
        [Required]
        [Display (Name = "Category Name")]
        public string Name { get; set; }
    }

}
