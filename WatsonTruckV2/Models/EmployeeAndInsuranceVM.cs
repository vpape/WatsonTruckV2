using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WatsonTruckV2.Models
{
    public class EmployeeAndInsuranceVM
    {
        public Employee employee { get; set; }
        public Group_Health grpHealth { get; set; }
        public Life_Insurance lifeIns { get; set; }
        public Other_Insurance otherIns { get; set; }
        public Family_Info spouse { get; set; }
        public List<Family_Info> family { get; set; }
        public Beneficiary beneficiary { get; set; }
        public List<Beneficiary> benefiList { get; set; }
    }
}