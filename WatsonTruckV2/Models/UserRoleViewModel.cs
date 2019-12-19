using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WatsonTruckV2.Models
{
    public class UserRoleViewModel
    {
        public string RoleId { get; set; }
        public string userId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public bool isSelected { get; set; }
    }
}