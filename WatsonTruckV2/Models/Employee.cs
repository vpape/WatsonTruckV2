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
    
    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.Beneficiaries = new HashSet<Beneficiary>();
            this.Deductions = new HashSet<Deduction>();
            this.Family_Info = new HashSet<Family_Info>();
            this.Group_Health = new HashSet<Group_Health>();
            this.InsurancePlans = new HashSet<InsurancePlan>();
            this.Life_Insurance = new HashSet<Life_Insurance>();
            this.JobApplicants = new HashSet<JobApplicant>();
        }
    
        public int Employee_id { get; set; }
        public string CurrentEmployer { get; set; }
        public string PreviousEmployer { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string SSN { get; set; }
        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        public string MailingAddress { get; set; }
        public string PhysicalAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string CellPhone { get; set; }
        public string County { get; set; }
        public string CityLimits { get; set; }
        public Nullable<System.DateTime> HireDate { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<System.DateTime> EligibilityDate { get; set; }
        public string HoursWorkedPerWeek { get; set; }
        public string JobTitle { get; set; }
        public string AnnualSalary { get; set; }
        public string Department { get; set; }
        public string EnrollmentType { get; set; }
        public string Payroll_id { get; set; }
        public string Class { get; set; }
        public string PObox { get; set; }
        public string CityTwo { get; set; }
        public string StateTwo { get; set; }
        public string ZipCodeTwo { get; set; }
        public string Active { get; set; }
        public string Retired { get; set; }
        public string CobraStateContinuation { get; set; }
        public string UserRole { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Beneficiary> Beneficiaries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Deduction> Deductions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Family_Info> Family_Info { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Group_Health> Group_Health { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InsurancePlan> InsurancePlans { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Life_Insurance> Life_Insurance { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobApplicant> JobApplicants { get; set; }
    }
}
