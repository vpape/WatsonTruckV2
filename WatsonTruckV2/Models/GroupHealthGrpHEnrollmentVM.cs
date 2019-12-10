using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using WatsonTruckV2.Models;

namespace WatsonTruckV2.Models
{
    public class GroupHealthGrpHEnrollmentVM
    {
        public Employee employee { get; set; }
        public List<Family_Info> family { get; set; }
        public Family_Info spouse { get; set; }
        public Group_Health grpHealth { get; set; }
        public List<Other_Insurance> otherIns { get; set; }
        public Other_Insurance spouseInsurance { get; set; }
        public Deduction deduction { get; set; }
    }
}