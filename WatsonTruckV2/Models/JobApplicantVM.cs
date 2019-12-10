using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WatsonTruckV2.Models
{
    public class JobApplicantVM
    {
        public List<JobApplicant> jobApplicants { get; set; }
        //public List<JobList> jobLists { get; set; }
        //public List<JobDescription> jobDescriptions  { get; set; }

        //add ListOfJobs table in db.WatsonTruck
    }
}