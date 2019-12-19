using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace WatsonTruckV2.Models
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>(); /*Roles = new List<string>();*/
        }


        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        public List<string> Claims { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
        //public List<string> Roles { get; set; }



    }
}