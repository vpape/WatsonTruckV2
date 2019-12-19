using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WatsonTruckV2.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        public string UserRole { get; set; }
        public string RoleId { get; set; }
        public string Name { get; set; }
    }
}