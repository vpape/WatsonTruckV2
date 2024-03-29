//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WatsonTruckV2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Beneficiary
    {
        public int Beneficiary_id { get; set; }
        public int Employee_id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SSN { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Address { get; set; }
        public string CIty { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string RelationshipToEmployee { get; set; }
        public string PercentageOfBenefits { get; set; }
        public string PrimaryBeneficiary { get; set; }
        public string ContingentBeneficiary { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
