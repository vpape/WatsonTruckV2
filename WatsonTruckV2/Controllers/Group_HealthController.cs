using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using SelectPdf;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;
using System.Dynamic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Http;
//using System.Web.Http;
using System.Web.Mvc;
using WatsonTruckV2.Models;


namespace WatsonTruckV2.Controllers
{
    //[System.Web.Mvc.Authorize = "Employee, Manager"]
    //[Authorize(Roles = "Employee, Manager")]
    public class Group_HealthController : Controller
    {
        private WatsonTruckEntities db = new WatsonTruckEntities();
        private static Group_Health grpHealth = new Group_Health();
        private static Employee employee = new Employee();
        private static List<Family_Info> family = new List<Family_Info>();
        private static List<Other_Insurance> otherIns = new List<Other_Insurance>();

        public Group_HealthController()
        {

        }

        //GroupHealth-Start---------------------------------------------------------------------------------------

        public ActionResult GrpHealthInsPremiums()
        {
            return View();
        }

        //Create-InsPrem---Finish Html page for Admin. Employee will only view Ins Cost and Vision and Dental pdf
        public JsonResult GrpHealthInsPremiumNew(int Employee_id, int InsurancePlan_id, int InsurancePremium_id, string EmployeeOnly, string EmployeeAndSpouse,
            string EmployeeAndDependent, string EmployeeAndFamily, decimal YearlyPremiumCost, string InsMECPlan, string InsStndPlan, string InsBuyUpPlan)
        {

            InsurancePremium insPremium = new InsurancePremium();

            insPremium.EmployeeOnly = EmployeeOnly;
            insPremium.EmployeeAndSpouse = EmployeeAndSpouse;
            insPremium.EmployeeAndDependent = EmployeeAndDependent;
            insPremium.EmployeeAndFamily = EmployeeAndFamily;
            insPremium.YearlyPremiumCost = YearlyPremiumCost;

            ViewBag.insPremium = insPremium;

            Employee e = db.Employees
            .Where(i => i.Employee_id == Employee_id)
            .Single();

            db.InsurancePremiums.Add(insPremium);

            db.SaveChanges();

            InsurancePlan insPlan = new InsurancePlan();

            //insPlan.InsurancePlan_id = InsurancePlan_id;
            insPlan.MECPlan = InsMECPlan;
            insPlan.StandardPlan = InsStndPlan;
            insPlan.BuyUpPlan = InsBuyUpPlan;

            ViewBag.insPlan = insPlan;

            int result = e.Employee_id;

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
        //----------------------------------------------------------------------------------------

        //Edit-InsPrem
        public ActionResult EditGrpHealthInsPremium(int? InsurancePremium_id)
        {
            if (InsurancePremium_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            InsurancePremium insPremium = db.InsurancePremiums.Find(InsurancePremium_id);
            if (insPremium == null)
            {
                return HttpNotFound();
            }

            ViewBag.InsurancePremium = insPremium.InsurancePremium_id;

            return View(insPremium);
        }

        //EditUpdate-InsPrem
        public JsonResult GrpHealthInsPremiumEditUpdate(int Employee_id, int InsurancePremium_id, string EmployeeOnly,
            string EmployeeAndSpouse, string EmployeeAndDependent, string EmployeeAndFamily, decimal YearlyPremiumCost)
        {
            var e = db.Employees
                .Where(i => i.Employee_id == Employee_id)
                .Single();

            InsurancePremium insPremium = db.InsurancePremiums
                .Where(i => i.InsurancePremium_id == InsurancePremium_id)
                .Single();

            insPremium.EmployeeOnly = EmployeeOnly;
            insPremium.EmployeeAndSpouse = EmployeeAndSpouse;
            insPremium.EmployeeAndDependent = EmployeeAndDependent;
            insPremium.EmployeeAndFamily = EmployeeAndFamily;
            insPremium.YearlyPremiumCost = YearlyPremiumCost;

            ViewBag.insPremium = insPremium;

            if (ModelState.IsValid)
            {
                db.Entry(insPremium).State = System.Data.Entity.EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception err)
                {
                    Console.WriteLine(err);
                }

                RedirectToAction("EmpOverview", new { e.Employee_id });
            }

            int result = e.Employee_id;

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }


        //----------------------------------------------------------------------------------------

        public ActionResult GrpHealthInsSupplement()
        {
            return View();
        }

        //Create-InsSupplment
        public JsonResult GrpHealthInsSupplementNew(int InsurancePlanDetail_id, string CalendarYearDeductible, string WaivedForPreventive,
            string AnnualMaximum, string Preventive, string Basic, string Major, string UCRpercentage, string EndoPeridontics,
            string Orthodontia, string OrthodontiaLifetimeMax, string WaitingPeriod, string DentalNetWork, string Exams,
            string Materials, string LensesSingleVision, string BiFocal, string TriFocal, string Lenticular,
            string ContactsMedicallyNecessary, string ContactsElective, string Frames, string Network, string DentalNetwork,
            string RateGuarantee, string Item, string Detail)
        {
            InsurancePlanDetail insPlanDetail = new InsurancePlanDetail();

            //"EmpDentalCost": EmpDentalCost,
            //"EmpVisionCost": EmpVisionCost,
            //"EmpSpDentalCost": EmpSpDentalCost,
            //"EmpSpVisionCost": EmpSpVisionCost,
            //"EmpDepDentalCost": EmpDepDentalCost,
            //"EmpDepVisionCost": EmpDepVisionCost,
            //"EmpFamDentalCost": EmpFamDentalCost,
            //"EmpFamVisionCost": EmpFamVisionCost,

            insPlanDetail.CalendarYearDeductible = CalendarYearDeductible;
            insPlanDetail.WaivedForPreventive = WaivedForPreventive;
            insPlanDetail.AnnualMaximum = AnnualMaximum;
            insPlanDetail.Preventive = Preventive;
            insPlanDetail.Basic = Basic;
            insPlanDetail.Major = Major;
            insPlanDetail.UCRPercentage = UCRpercentage;
            insPlanDetail.EndodonticsOrPeriodontics = EndoPeridontics;
            insPlanDetail.Orthodontia = Orthodontia;
            insPlanDetail.OrthodontiaLifeTimeMaximum = OrthodontiaLifetimeMax;
            insPlanDetail.WaitingPeriod = WaitingPeriod;
            insPlanDetail.DentalNetwork = DentalNetWork;

            insPlanDetail.Exams = Exams;
            insPlanDetail.Materials = Materials;
            insPlanDetail.LensesSingleVision = LensesSingleVision;
            insPlanDetail.Bifocal = BiFocal;
            insPlanDetail.Trifocal = TriFocal;
            insPlanDetail.Lenticular = Lenticular;
            insPlanDetail.ContactMedicallyNecessary = ContactsMedicallyNecessary;
            insPlanDetail.ContactElective = ContactsElective;
            insPlanDetail.Frames = Frames;
            insPlanDetail.Network = Network;
            insPlanDetail.RateGuarantee = RateGuarantee;

            insPlanDetail.Item = Item;
            insPlanDetail.Detail = Detail;

            int result = insPlanDetail.InsurancePlanDetail_id;

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        //----------------------------------------------------------------------------------------

        //Edit-InsSupplment
        public ActionResult EditGrpHealthSupplement(int? InsurancePlanDetail_id)
        {
            if (InsurancePlanDetail_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            InsurancePlanDetail insPlanDetail = db.InsurancePlanDetails.Find(InsurancePlanDetail_id);
            if (insPlanDetail == null)
            {
                return HttpNotFound();
            }

            ViewBag.InsurancePlanDetail = insPlanDetail.InsurancePlan_id;

            return View(insPlanDetail);
        }

        //EditUpdate-InsSupplment
        public JsonResult GrpHealthInsSupplementEditUpdate(int InsurancePlanDetail_id, string CalendarYearDeductible, string WaivedForPreventive,
            string AnnualMaximum, string Preventive, string Basic, string Major, string UCRpercentage, string EndoPeridontics,
            string Orthodontia, string OrthodontiaLifetimeMax, string WaitingPeriod, string DentalNetWork, string Exams,
            string Materials, string LensesSingleVision, string BiFocal, string TriFocal, string Lenticular,
            string ContactsMedicallyNecessary, string ContactsElective, string Frames, string Network, string DentalNetwork,
            string RateGuarantee, string Item, string Detail)
        {
            var insPlanDetail = db.InsurancePlanDetails
                .Where(i => i.InsurancePlanDetail_id == InsurancePlanDetail_id)
                .Single();

            //"EmpDentalCost": EmpDentalCost,
            //"EmpVisionCost": EmpVisionCost,
            //"EmpSpDentalCost": EmpSpDentalCost,
            //"EmpSpVisionCost": EmpSpVisionCost,
            //"EmpDepDentalCost": EmpDepDentalCost,
            //"EmpDepVisionCost": EmpDepVisionCost,
            //"EmpFamDentalCost": EmpFamDentalCost,
            //"EmpFamVisionCost": EmpFamVisionCost,

            insPlanDetail.CalendarYearDeductible = CalendarYearDeductible;
            insPlanDetail.WaivedForPreventive = WaivedForPreventive;
            insPlanDetail.AnnualMaximum = AnnualMaximum;
            insPlanDetail.Preventive = Preventive;
            insPlanDetail.Basic = Basic;
            insPlanDetail.Major = Major;
            insPlanDetail.UCRPercentage = UCRpercentage;
            insPlanDetail.EndodonticsOrPeriodontics = EndoPeridontics;
            insPlanDetail.Orthodontia = Orthodontia;
            insPlanDetail.OrthodontiaLifeTimeMaximum = OrthodontiaLifetimeMax;
            insPlanDetail.WaitingPeriod = WaitingPeriod;
            insPlanDetail.DentalNetwork = DentalNetWork;

            insPlanDetail.Exams = Exams;
            insPlanDetail.Materials = Materials;
            insPlanDetail.LensesSingleVision = LensesSingleVision;
            insPlanDetail.Bifocal = BiFocal;
            insPlanDetail.Trifocal = TriFocal;
            insPlanDetail.Lenticular = Lenticular;
            insPlanDetail.ContactMedicallyNecessary = ContactsMedicallyNecessary;
            insPlanDetail.ContactElective = ContactsElective;
            insPlanDetail.Frames = Frames;
            insPlanDetail.Network = Network;
            insPlanDetail.RateGuarantee = RateGuarantee;

            insPlanDetail.Item = Item;
            insPlanDetail.Detail = Detail;

            ViewBag.insPlanDetail = insPlanDetail;

            if (ModelState.IsValid)
            {
                db.Entry(insPlanDetail).State = System.Data.Entity.EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception err)
                {
                    Console.WriteLine(err);
                }

                RedirectToAction("EmpOverview", new { insPlanDetail.InsurancePlan_id });
            }

            int result = insPlanDetail.InsurancePlan_id;

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }


        //====================================
        // GET: CreateGrpHPDF PDF 
        //====================================  

        public ActionResult CreateGrpHPDF(int? Employee_id, int? GroupHealthInsurance_id, FormCollection collection)
        {
            ViewBag.GroupHealthInsurance_id = GroupHealthInsurance_id;
            ViewBag.Employee_id = Employee_id;

            GroupHealthGrpHEnrollmentVM groupHGrpHEnrollmentVM = new GroupHealthGrpHEnrollmentVM();


            groupHGrpHEnrollmentVM.employee = db.Employees.FirstOrDefault(i => i.Employee_id == Employee_id);
            groupHGrpHEnrollmentVM.grpHealth = db.Group_Health.FirstOrDefault(i => i.Employee_id == Employee_id);

            groupHGrpHEnrollmentVM.spouse = db.Family_Info.FirstOrDefault(i => i.Employee_id == Employee_id && i.RelationshipToInsured == "Spouse");
            groupHGrpHEnrollmentVM.family = db.Family_Info.Where(i => i.Employee_id == Employee_id && i.RelationshipToInsured != "Spouse").ToList();
            if (groupHGrpHEnrollmentVM.spouse != null)
            {
                groupHGrpHEnrollmentVM.spouseInsurance = db.Other_Insurance.FirstOrDefault(i => i.Employee_id == Employee_id && i.FamilyMember_id == groupHGrpHEnrollmentVM.spouse.FamilyMember_id);
                groupHGrpHEnrollmentVM.otherIns = db.Other_Insurance.Where(i => i.Employee_id == Employee_id && i.FamilyMember_id != groupHGrpHEnrollmentVM.spouse.FamilyMember_id).ToList();
            }
            else
            {
                groupHGrpHEnrollmentVM.spouseInsurance = null;
                groupHGrpHEnrollmentVM.otherIns = db.Other_Insurance.Where(i => i.Employee_id == Employee_id).ToList();
            }

            return View(groupHGrpHEnrollmentVM);
        }


        //====================================
        // Get: GroupHealthEnrollment
        //====================================
        public ActionResult GrpHealthEnrollment(int? Employee_id, int? GroupHealthInsurance_id)
        {
            ViewBag.GroupHealthInsurance_id = GroupHealthInsurance_id;
            ViewBag.Employee_id = Employee_id;

            GroupHealthGrpHEnrollmentVM groupHGrpHEnrollmentVM = new GroupHealthGrpHEnrollmentVM();

            groupHGrpHEnrollmentVM.employee = db.Employees.FirstOrDefault(i => i.Employee_id == Employee_id);
            groupHGrpHEnrollmentVM.grpHealth = db.Group_Health.FirstOrDefault(i => i.Employee_id == Employee_id);

            groupHGrpHEnrollmentVM.spouse = db.Family_Info.FirstOrDefault(i => i.Employee_id == Employee_id && i.RelationshipToInsured == "Spouse");
            groupHGrpHEnrollmentVM.family = db.Family_Info.Where(i => i.Employee_id == Employee_id && i.RelationshipToInsured != "Spouse").ToList();
            if (groupHGrpHEnrollmentVM.spouse != null)
            {
                groupHGrpHEnrollmentVM.spouseInsurance = db.Other_Insurance.FirstOrDefault(i => i.Employee_id == Employee_id && i.FamilyMember_id == groupHGrpHEnrollmentVM.spouse.FamilyMember_id);
                groupHGrpHEnrollmentVM.otherIns = db.Other_Insurance.Where(i => i.Employee_id == Employee_id && i.FamilyMember_id != groupHGrpHEnrollmentVM.spouse.FamilyMember_id).ToList();
            }
            else
            {
                groupHGrpHEnrollmentVM.spouseInsurance = null;
                groupHGrpHEnrollmentVM.otherIns = db.Other_Insurance.Where(i => i.Employee_id == Employee_id).ToList();
            }

            return View(groupHGrpHEnrollmentVM);

        }

        //====================================
        // Post: Create EmploymentInfoGrpH
        //====================================
        public JsonResult EmploymentInfoGrpHealthEnrollment(int? Employee_id, string GroupName, string IMSGroupNumber, string Department, string EnrollmentType,
             string Payroll_id, string Class, string AnnualSalary, string JobTitle, DateTime HireDate, DateTime EffectiveDate, string HoursWorkedPerWeek)
        {
            //Employee emp = new Employee();
            Employee emp = db.Employees
             .Where(i => i.Employee_id == Employee_id)
             .Single();

            emp.Department = Department;
            emp.EnrollmentType = EnrollmentType;
            emp.Payroll_id = Payroll_id;
            emp.Class = Class;
            emp.AnnualSalary = AnnualSalary;
            emp.JobTitle = JobTitle;
            emp.HireDate = HireDate;
            emp.EffectiveDate = EffectiveDate;
            emp.HoursWorkedPerWeek = HoursWorkedPerWeek;

            Group_Health g = db.Group_Health
                .Where(i => i.Employee_id == Employee_id)
                .Single();

            g.GroupName = GroupName;
            g.IMSGroupNumber = IMSGroupNumber;

            db.SaveChanges();

            int result = g.Employee_id;

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        //====================================
        //Post-GrpHealthEnrollment
        //====================================

        //DEMO LICENSE KEY for Selectpdf Html to PDF API:
        //"7df5f5a6-4672-4cd7-9277-3de3615ffdfc";

        public JsonResult GrpHealthEnrollmentNew(int Employee_id,/*DateTime? CafeteriaPlanYear,*/ string empCoveredByOtherIns,
            string empInsCarrier, string empInsPolicyNumber, string empInsPhoneNumber, string NoMedical, string MECPlan,
            string StandardPlan, string BuyUpPlan, string GrpHEnrollmentEmpSignature, DateTime? GrpHEnrollmentEmpSignatureDate, string Myself, string Spouse,
            string Dependent, string OtherCoverageSelection, string OtherReasonSelection, string ReasonForGrpCoverageRefusal, string GrpHRefusalEmpSignature,
            DateTime? GrpHRefusalEmpSignatureDate, FormCollection collection)
        {
            string response = "";

            int record = (from grpH in db.Group_Health
                          where grpH.Employee_id == Employee_id
                          select grpH).Count();

            if (record > 0)
            {
                response = "Record already exists.";
            }
            else
            {

                Group_Health g = db.Group_Health
                    .Where(i => i.Employee_id == Employee_id)
                    .Single();

                //Group_Health g = new Group_Health();

                g.Employee_id = Employee_id;
                //g.OtherInsuranceCoverage = empCoveredByOtherIns;
                //g.InsuranceCarrier = empInsCarrier;
                //g.PolicyNumber = empInsPolicyNumber;
                //g.PhoneNumber = empInsPhoneNumber;

                //g.CafeteriaPlanYear = CafeteriaPlanYear;
                g.NoMedicalPlan = NoMedical;
                g.MECPlan = MECPlan;
                g.StandardPlan = StandardPlan;
                g.BuyUpPlan = BuyUpPlan;

                g.GrpHEnrollmentEmpSignature = GrpHEnrollmentEmpSignature;
                g.GrpHEnrollmentEmpSignatureDate = GrpHEnrollmentEmpSignatureDate;
                g.Myself = Myself;
                g.Spouse = Spouse;
                g.Dependent = Dependent;
                g.OtherCoverage = OtherCoverageSelection;
                g.OtherReason = OtherReasonSelection;
                g.ReasonForGrpCoverageRefusal = ReasonForGrpCoverageRefusal;
                g.GrpHRefusalEmpSignature = GrpHRefusalEmpSignature;
                g.GrpHRefusalEmpSignatureDate = GrpHRefusalEmpSignatureDate;

                db.SaveChanges();
            }

            int result = Employee_id;

            return Json(new { data = result, error = response }, JsonRequestBehavior.AllowGet);
        }

        //====================================
        //Edit-GrpHealthEnrollment
        //====================================

        public ActionResult EditGroupHealthIns(int? Employee_id, int? GroupHealthInsurance_id)
        {
            ViewBag.GroupHealthInsurance_id = GroupHealthInsurance_id;
            ViewBag.Employee_id = Employee_id;

            GroupHealthGrpHEnrollmentVM groupHGrpHEnrollmentVM = new GroupHealthGrpHEnrollmentVM();

            groupHGrpHEnrollmentVM.employee = db.Employees.FirstOrDefault(i => i.Employee_id == Employee_id);
            groupHGrpHEnrollmentVM.grpHealth = db.Group_Health.FirstOrDefault(i => i.Employee_id == Employee_id);

            groupHGrpHEnrollmentVM.spouse = db.Family_Info.FirstOrDefault(i => i.Employee_id == Employee_id && i.RelationshipToInsured == "Spouse");
            groupHGrpHEnrollmentVM.family = db.Family_Info.Where(i => i.Employee_id == Employee_id && i.RelationshipToInsured != "Spouse").ToList();
            if (groupHGrpHEnrollmentVM.spouse != null)
            {
                groupHGrpHEnrollmentVM.spouseInsurance = db.Other_Insurance.FirstOrDefault(i => i.Employee_id == Employee_id && i.FamilyMember_id == groupHGrpHEnrollmentVM.spouse.FamilyMember_id);
                groupHGrpHEnrollmentVM.otherIns = db.Other_Insurance.Where(i => i.Employee_id == Employee_id && i.FamilyMember_id != groupHGrpHEnrollmentVM.spouse.FamilyMember_id).ToList();
            }
            else
            {
                groupHGrpHEnrollmentVM.spouseInsurance = null;
                groupHGrpHEnrollmentVM.otherIns = db.Other_Insurance.Where(i => i.Employee_id == Employee_id).ToList();
            }

            return View(groupHGrpHEnrollmentVM);

        }

        //====================================
        //EditUpdate-GrpHealthEnrollment
        //====================================

        public static string apiEndpoint = "https://selectpdf.com/api2/convert/";
        public static string apiKey = "7df5f5a6-4672-4cd7-9277-3de3615ffdfc";
        public static string GrpHInsURL = "http://localhost:57772/Group_Health/EditGroupHealthIns?Employee_id=";

        public static void Main(string[] args)
        {
            // POST JSON example using WebClient (and Newtonsoft for JSON serialization)
            SelectPdfPostWithWebClient();
        }

        // POST JSON example using WebClient (and Newtonsoft for JSON serialization)
        public static void SelectPdfPostWithWebClient()
        {

        }

        public JsonResult GrpHealthInsEditUpdate(int? Employee_id, int? InsurancePlan_id, /*DateTime? CafeteriaPlanYear,*/ string NoMedical, string MECPlan,
                string StandardPlan, string BuyUpPlan, string GrpHEnrollmentEmpSignature, DateTime? GrpHEnrollmentEmpSignatureDate, string Myself, string Spouse,
                string Dependent, string OtherCoverageSelection, string OtherReasonSelection, string ReasonForGrpCoverageRefusal, string GrpHRefusalEmpSignature,
                DateTime? GrpHRefusalEmpSignatureDate, FormCollection collection)
        {

            Group_Health g = db.Group_Health
                 .Where(i => i.Employee_id == Employee_id)
                 .Single();

            //g.CafeteriaPlanYear = CafeteriaPlanYear;
            g.NoMedicalPlan = NoMedical;
            g.MECPlan = MECPlan;
            g.StandardPlan = StandardPlan;
            g.BuyUpPlan = BuyUpPlan;

            g.GrpHEnrollmentEmpSignature = GrpHEnrollmentEmpSignature;
            g.GrpHEnrollmentEmpSignatureDate = GrpHEnrollmentEmpSignatureDate;
            g.Myself = Myself;
            g.Spouse = Spouse;
            g.Dependent = Dependent;
            g.OtherCoverage = OtherCoverageSelection;
            g.OtherReason = OtherReasonSelection;
            g.ReasonForGrpCoverageRefusal = ReasonForGrpCoverageRefusal;
            g.GrpHRefusalEmpSignature = GrpHRefusalEmpSignature;
            g.GrpHRefusalEmpSignatureDate = GrpHRefusalEmpSignatureDate;

            //InsurancePlan insPlan = db.InsurancePlans
            //  .Where(i => i.InsurancePlan_id == InsurancePlan_id)
            //  .Single();

            //insPlan.MECPlan = InsMECPlan;
            //insPlan.StandardPlan = InsStndPlan;
            //insPlan.BuyUpPlan = InsBuyUpPlan;

            //ViewBag.insPlan = insPlan;

            if (ModelState.IsValid)
            {
                db.Entry(g).State = System.Data.Entity.EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception err)
                {
                    Console.WriteLine(err);
                }
            }

            int result = g.Employee_id;

            CreateGrpHealthPDF(g.Employee_id);

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        public void CreateGrpHealthPDF(int Employee_id)
        {
            System.Console.WriteLine("Starting conversion with WebClient ...");

            // set parameters
            SelectPdfParameters parameters = new SelectPdfParameters();
            parameters.key = apiKey;
            parameters.url = GrpHInsURL + Employee_id;

            // JSON serialize parameters
            string jsonData = JsonConvert.SerializeObject(parameters);
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

            // create WebClient object
            WebClient webClient = new WebClient();
            webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");

            // POST parameters (if response code is not 200 OK, a WebException is raised)
            try
            {
                byte[] result = webClient.UploadData(apiEndpoint, "POST", byteData);

                // all ok - read PDF and write on disk (binary read!!!!)
                MemoryStream ms = new MemoryStream(result);

                // write to file
                FileStream file = new FileStream("test2.pdf", FileMode.Create, FileAccess.Write);
                ms.WriteTo(file);
                file.Close();

            }
            catch (WebException webEx)
            {
                // an error occurred
                System.Console.WriteLine("Error: " + webEx.Message);

                HttpWebResponse response = (HttpWebResponse)webEx.Response;
                Stream responseStream = response.GetResponseStream();

                // get details of the error message if available (text read!!!)
                StreamReader readStream = new StreamReader(responseStream);
                string message = readStream.ReadToEnd();
                responseStream.Close();

                System.Console.WriteLine("Error Message: " + message);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error: " + ex.Message);
            }

            System.Console.WriteLine("Finished.");

            // return resulted pdf document
            //FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            //fileResult.FileDownloadName = "GrpHealth_Insurance.pdf";
            //return fileResult;

        }

        //SalaryRedirect-Start----------------------------------------------------------------------------------

        public ActionResult SalaryRedirection(int Employee_id)
        {

            GroupHealthGrpHEnrollmentVM groupHGrpHEnrollmentVM = new GroupHealthGrpHEnrollmentVM();

            groupHGrpHEnrollmentVM.employee = db.Employees.FirstOrDefault(i => i.Employee_id == Employee_id);

            ViewBag.Employee_id = groupHGrpHEnrollmentVM.employee.Employee_id;


            return View(groupHGrpHEnrollmentVM);
        }

        //Create-SalaryRedirect
        public JsonResult SalaryRedirectionUpdate(int Employee_id, int? Deductions_id, string MedicalInsProvider, string EEelectionPreTaxMedIns,
            string PremiumPreTaxMedIns, string EEelectionPostTaxMedIns, string PremiumPostTaxMedIns, string DentalInsProvider, string EEelectionPreTaxDentalIns,
            string PremiumPreTaxDentalIns, string EEelectionPostTaxDentalIns, string PremiumPostTaxDentalIns, string VisionInsProvider, string EEelectionPreTaxVisionIns,
            string PremiumPreTaxVisionIns, string EEelectionPostTaxVisionIns, string PremiumPostTaxVisionIns, string StDisabilityProvider, string EEelectionPreTaxStDisability, string PremiumPreTaxStDisability,
            string EEelectionPostTaxStDisability, string PremiumPostTaxStDisability, string HospitalIndemProvider, string EEelectionPreTaxHospitalIndem,
            string PremiumPreTaxHospitalIndem, string EEelectionPostTaxHospitalIndem, string PremiumPostTaxHospitalIndem, string TermLifeInsProvider,
            string EEelectionPreTaxTermLifeIns, string PremiumPreTaxTermLifeIns, string EEelectionPostTaxTermLifeIns, string PremiumPostTaxTermLifeIns,
            string WholeLifeInsProvider, string EEelectionPreTaxWholeLifeIns, string PremiumPreTaxWholeLifeIns, string EEelectionPostTaxWholeLifeIns,
            string PremiumPostTaxWholeLifeIns, string OtherInsProvider, string EEelectionPreTaxOtherIns, string PremiumPreTaxOtherIns, string EEelectionPostTaxOtherIns,
            string PremiumPostTaxOtherIns, string AccidentProvider, string EEelectionPreTaxAccidentIns, string PremiumPreTaxAccidentIns, string EEelectionPostTaxAccidentIns,
            string PremiumPostTaxAccidentIns, string CancerProvider, string EEelectionPreTaxCancerIns, string PremiumPreTaxCancerIns, string EEelectionPostTaxCancerIns,
            string PremiumPostTaxCancerIns, string TotalPreTax, string TotalPostTax, string empInitials1, string PreTaxBenefitWaiverinitials, string empSignature,
            DateTime empSignatureDate)
        {

            Deduction d = new Deduction();

            d.Employee_id = Employee_id;
            d.MedicalInsProvider = MedicalInsProvider;
            d.EEelectionPreTaxMedIns = EEelectionPreTaxMedIns;
            d.PremiumPreTaxMedIns = PremiumPreTaxMedIns;
            d.EEelectionPostTaxMedIns = EEelectionPostTaxMedIns;
            d.PremiumPostTaxMedIns = PremiumPostTaxMedIns;

            d.DentalInsProvider = DentalInsProvider;
            d.EEelectionPreTaxDentalIns = EEelectionPreTaxDentalIns;
            d.PremiumPreTaxDentalIns = PremiumPreTaxDentalIns;
            d.EEelectionPostTaxDentalIns = EEelectionPostTaxDentalIns;
            d.PremiumPostTaxDentalIns = PremiumPostTaxDentalIns;

            d.VisionInsProvider = VisionInsProvider;
            d.EEelectionPreTaxVisionIns = EEelectionPreTaxVisionIns;
            d.PremiumPreTaxVisionIns = PremiumPreTaxVisionIns;
            d.EEelectionPostTaxVisionIns = EEelectionPostTaxVisionIns;
            d.PremiumPostTaxVisionIns = PremiumPostTaxVisionIns;



            d.StDisabilityProvider = StDisabilityProvider;
            d.EEelectionPreTaxStDisability = EEelectionPreTaxStDisability;
            d.PremiumPreTaxStDisability = PremiumPreTaxStDisability;
            d.EEelectionPostTaxStDisability = EEelectionPostTaxStDisability;
            d.PremiumPostTaxStDisability = PremiumPostTaxStDisability;

            d.HospitalIndemProvider = HospitalIndemProvider;
            d.EEelectionPreTaxHospitalIndem = EEelectionPreTaxHospitalIndem;
            d.PremiumPreTaxHospitalIndem = PremiumPreTaxHospitalIndem;
            d.EEelectionPostTaxHospitalIndem = EEelectionPostTaxHospitalIndem;
            d.PremiumPostTaxHospitalIndem = PremiumPostTaxHospitalIndem;

            d.TermLifeInsProvider = TermLifeInsProvider;
            d.EEelectionPreTaxTermLifeIns = EEelectionPreTaxTermLifeIns;
            d.PremiumPreTaxTermLifeIns = PremiumPreTaxTermLifeIns;
            d.EEelectionPostTaxTermLifeIns = EEelectionPostTaxTermLifeIns;
            d.PremiumPostTaxTermLifeIns = PremiumPostTaxTermLifeIns;

            d.WholeLifeInsProvider = WholeLifeInsProvider;
            d.EEelectionPreTaxWholeLifeIns = EEelectionPreTaxWholeLifeIns;
            d.PremiumPreTaxWholeLifeIns = PremiumPreTaxWholeLifeIns;
            d.EEelectionPostTaxWholeLifeIns = EEelectionPostTaxWholeLifeIns;
            d.PremiumPostTaxWholeLifeIns = PremiumPostTaxWholeLifeIns;

            d.AccidentProvider = AccidentProvider;
            d.EEelectionPreTaxAccidentIns = EEelectionPreTaxAccidentIns;
            d.PremiumPreTaxAccidentIns = PremiumPreTaxAccidentIns;
            d.EEelectionPostTaxAccidentIns = EEelectionPostTaxAccidentIns;
            d.PremiumPostTaxAccidentIns = PremiumPostTaxAccidentIns;

            d.CancerProvider = CancerProvider;
            d.EEelectionPreTaxCancerIns = EEelectionPreTaxCancerIns;
            d.PremiumPreTaxCancerIns = PremiumPreTaxCancerIns;
            d.EEelectionPostTaxCancerIns = EEelectionPostTaxCancerIns;
            d.PremiumPostTaxCancerIns = PremiumPostTaxCancerIns;

            d.OtherInsProvider = OtherInsProvider;
            d.EEelectionPreTaxOtherIns = EEelectionPreTaxOtherIns;
            d.PremiumPreTaxOtherIns = PremiumPreTaxOtherIns;
            d.EEelectionPostTaxOtherIns = EEelectionPostTaxOtherIns;
            d.PremiumPostTaxOtherIns = PremiumPostTaxOtherIns;

            d.TotalPreTax = TotalPreTax;
            d.TotalPostTax = TotalPostTax;
            d.EmployeeSignature = empSignature;
            d.EmployeeSignatureDate = empSignatureDate;
            d.EmployeeInitials = empInitials1;
            d.PreTaxBenefitWaiverinitials = PreTaxBenefitWaiverinitials;


            db.Deductions.Add(d);
            db.SaveChanges();

            int result = d.Deductions_id;

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        //----------------------------------------------------------------------------------------

        public ActionResult EditSalaryRedirection(int? Employee_id, int? Deductions_id)
        {
            GroupHealthGrpHEnrollmentVM grpHGrpEnrollmentVM = new GroupHealthGrpHEnrollmentVM();

            grpHGrpEnrollmentVM.employee = db.Employees.FirstOrDefault(i => i.Employee_id == Employee_id);
            grpHGrpEnrollmentVM.deduction = db.Deductions.FirstOrDefault(i => i.Employee_id == Employee_id);

            //ViewBag.Deductions_id = grpHGrpEnrollmentVM.deduction.Deductions_id;
            ViewBag.Employee_id = grpHGrpEnrollmentVM.employee.Employee_id;

            //    if (Employee_id == null)
            //    {
            //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //    }

            //    Group_Health g = db.Group_Health.Find(Employee_id);
            //    if (g == null)
            //    {
            //        return HttpNotFound();
            //    }

            return View(grpHGrpEnrollmentVM);
        }

        //EditUpdate-SalaryRedirect
        public JsonResult SalaryRedirectionEditUpdate(int Employee_id, int? Deductions_id, string MedicalInsProvider, string EEelectionPreTaxMedIns,
            string PremiumPreTaxMedIns, string EEelectionPostTaxMedIns, string PremiumPostTaxMedIns, string DentalInsProvider, string EEelectionPreTaxDentalIns,
            string PremiumPreTaxDentalIns, string EEelectionPostTaxDentalIns, string PremiumPostTaxDentalIns, string VisionInsProvider, string EEelectionPreTaxVisionIns,
            string PremiumPreTaxVisionIns, string EEelectionPostTaxVisionIns, string PremiumPostTaxVisionIns, string StDisabilityProvider, string EEelectionPreTaxStDisability, string PremiumPreTaxStDisability,
            string EEelectionPostTaxStDisability, string PremiumPostTaxStDisability, string HospitalIndemProvider, string EEelectionPreTaxHospitalIndem,
            string PremiumPreTaxHospitalIndem, string EEelectionPostTaxHospitalIndem, string PremiumPostTaxHospitalIndem, string TermLifeInsProvider,
            string EEelectionPreTaxTermLifeIns, string PremiumPreTaxTermLifeIns, string EEelectionPostTaxTermLifeIns, string PremiumPostTaxTermLifeIns,
            string WholeLifeInsProvider, string EEelectionPreTaxWholeLifeIns, string PremiumPreTaxWholeLifeIns, string EEelectionPostTaxWholeLifeIns,
            string PremiumPostTaxWholeLifeIns, string OtherInsProvider, string EEelectionPreTaxOtherIns, string PremiumPreTaxOtherIns, string EEelectionPostTaxOtherIns,
            string PremiumPostTaxOtherIns, string AccidentProvider, string EEelectionPreTaxAccidentIns, string PremiumPreTaxAccidentIns, string EEelectionPostTaxAccidentIns,
            string PremiumPostTaxAccidentIns, string CancerProvider, string EEelectionPreTaxCancerIns, string PremiumPreTaxCancerIns, string EEelectionPostTaxCancerIns,
            string PremiumPostTaxCancerIns, string TotalPreTax, string TotalPostTax, string empInitials1, string PreTaxBenefitWaiverinitials, string empSignature,
            DateTime empSignatureDate)
        {
            Deduction d = db.Deductions
                .Where(i => i.Deductions_id == Deductions_id)
                .Single();

            d.Employee_id = Employee_id;
            d.MedicalInsProvider = MedicalInsProvider;
            d.EEelectionPreTaxMedIns = EEelectionPreTaxMedIns;
            d.PremiumPreTaxMedIns = PremiumPreTaxMedIns;
            d.EEelectionPostTaxMedIns = EEelectionPostTaxMedIns;
            d.PremiumPostTaxMedIns = PremiumPostTaxMedIns;

            d.DentalInsProvider = DentalInsProvider;
            d.EEelectionPreTaxDentalIns = EEelectionPreTaxDentalIns;
            d.PremiumPreTaxDentalIns = PremiumPreTaxDentalIns;
            d.EEelectionPostTaxDentalIns = EEelectionPostTaxDentalIns;
            d.PremiumPostTaxDentalIns = PremiumPostTaxDentalIns;

            d.VisionInsProvider = VisionInsProvider;
            d.EEelectionPreTaxVisionIns = EEelectionPreTaxVisionIns;
            d.PremiumPreTaxVisionIns = PremiumPreTaxVisionIns;
            d.EEelectionPostTaxVisionIns = EEelectionPostTaxVisionIns;
            d.PremiumPostTaxVisionIns = PremiumPostTaxVisionIns;

            d.StDisabilityProvider = StDisabilityProvider;
            d.EEelectionPreTaxStDisability = EEelectionPreTaxStDisability;
            d.PremiumPreTaxStDisability = PremiumPreTaxStDisability;
            d.EEelectionPostTaxStDisability = EEelectionPostTaxStDisability;
            d.PremiumPostTaxStDisability = PremiumPostTaxStDisability;

            d.HospitalIndemProvider = HospitalIndemProvider;
            d.EEelectionPreTaxHospitalIndem = EEelectionPreTaxHospitalIndem;
            d.PremiumPreTaxHospitalIndem = PremiumPreTaxHospitalIndem;
            d.EEelectionPostTaxHospitalIndem = EEelectionPostTaxHospitalIndem;
            d.PremiumPostTaxHospitalIndem = PremiumPostTaxHospitalIndem;

            d.TermLifeInsProvider = TermLifeInsProvider;
            d.EEelectionPreTaxTermLifeIns = EEelectionPreTaxTermLifeIns;
            d.PremiumPreTaxTermLifeIns = PremiumPreTaxTermLifeIns;
            d.EEelectionPostTaxTermLifeIns = EEelectionPostTaxTermLifeIns;
            d.PremiumPostTaxTermLifeIns = PremiumPostTaxTermLifeIns;

            d.WholeLifeInsProvider = WholeLifeInsProvider;
            d.EEelectionPreTaxWholeLifeIns = EEelectionPreTaxWholeLifeIns;
            d.PremiumPreTaxWholeLifeIns = PremiumPreTaxWholeLifeIns;
            d.EEelectionPostTaxWholeLifeIns = EEelectionPostTaxWholeLifeIns;
            d.PremiumPostTaxWholeLifeIns = PremiumPostTaxWholeLifeIns;

            d.AccidentProvider = AccidentProvider;//data shows null
            d.EEelectionPreTaxAccidentIns = EEelectionPreTaxAccidentIns;
            d.PremiumPreTaxAccidentIns = PremiumPreTaxAccidentIns;
            d.EEelectionPostTaxAccidentIns = EEelectionPostTaxAccidentIns;
            d.PremiumPostTaxAccidentIns = PremiumPostTaxAccidentIns;

            d.CancerProvider = CancerProvider; //data shows null
            d.EEelectionPreTaxCancerIns = EEelectionPreTaxCancerIns;
            d.PremiumPreTaxCancerIns = PremiumPreTaxCancerIns;
            d.EEelectionPostTaxCancerIns = EEelectionPostTaxCancerIns;
            d.PremiumPostTaxCancerIns = PremiumPostTaxCancerIns;

            d.OtherInsProvider = OtherInsProvider;
            d.EEelectionPreTaxOtherIns = EEelectionPreTaxOtherIns;
            d.PremiumPreTaxOtherIns = PremiumPreTaxOtherIns;
            d.EEelectionPostTaxOtherIns = EEelectionPostTaxOtherIns;
            d.PremiumPostTaxOtherIns = PremiumPostTaxOtherIns;

            d.TotalPreTax = TotalPreTax;
            d.TotalPostTax = TotalPostTax;
            d.EmployeeSignature = empSignature;
            d.EmployeeSignatureDate = empSignatureDate;
            d.EmployeeInitials = empInitials1;
            d.PreTaxBenefitWaiverinitials = PreTaxBenefitWaiverinitials;

            if (ModelState.IsValid)
            {
                db.Entry(d).State = System.Data.Entity.EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception err)
                {
                    Console.WriteLine(err);
                }

                RedirectToAction("EmpOverview", new { d.Employee_id });
            }

            int result = d.Deductions_id;

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        //AuthorizationForm-Start-----------------------------------------------------------------------------

        public ActionResult AuthorizationForm(int? Employee_id, int? GroupHealthInsurance_id)
        {
            ViewBag.Employee_id = Employee_id;
            ViewBag.GroupHealthInsurance_id = GroupHealthInsurance_id;

            EmployeeAndInsuranceVM employeeAndInsVM = new EmployeeAndInsuranceVM();

            employeeAndInsVM.employee = db.Employees.FirstOrDefault(i => i.Employee_id == Employee_id);
            employeeAndInsVM.grpHealth = db.Group_Health.FirstOrDefault(i => i.Employee_id == Employee_id);


            return View(employeeAndInsVM);
        }

        //Create-AuthorizationForm
        public JsonResult AuthorizationFormNew(int? GroupHealthInsurance_id, int Employee_id, string PersonOneReleaseInfoTo, string PersonOneRelationship,
            string PersonTwoReleaseInfoTo, string PersonTwoRelationship, string PolicyHolderSignature, DateTime? PolicyHolderSignatureDate,
            string PersonOneSignature, DateTime? PersonOneSignatureDate, string PersonTwoSignature, DateTime? PersonTwoSignatureDate)
        {

            Group_Health g = db.Group_Health
                .Where(i => i.GroupHealthInsurance_id == GroupHealthInsurance_id)
                .SingleOrDefault();

            g.Employee_id = Employee_id;
            g.NameOfPersonOneReleaseInfoTo = PersonOneReleaseInfoTo;
            g.PersonOneRelationship = PersonOneRelationship;
            g.NameOfPersonTwoReleaseInfoTo = PersonTwoReleaseInfoTo;
            g.PersonTwoRelationship = PersonTwoRelationship;
            g.AuthorizationFormPolicyHolderSignature = PolicyHolderSignature;
            g.AuthorizationFormPolicyHolderSignatureDate = PolicyHolderSignatureDate;
            g.PersonOneSignature = PersonOneSignature;
            g.PersonOneSignatureDate = PersonOneSignatureDate;
            g.PersonTwoSignature = PersonTwoSignature;
            g.PersonTwoSignatureDate = PersonTwoSignatureDate;

            db.SaveChanges();

            int result = g.GroupHealthInsurance_id;

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        //----------------------------------------------------------------------------------------

        //Edit-AuthorizationForm
        public ActionResult EditAuthorizationForm(int? Employee_id, int? GroupHealthInsurance_id)
        {
            ViewBag.Employee_id = Employee_id;
            ViewBag.GroupHealthInsurance_id = GroupHealthInsurance_id;

            EmployeeAndInsuranceVM employeeAndInsVM = new EmployeeAndInsuranceVM();

            employeeAndInsVM.employee = db.Employees.FirstOrDefault(i => i.Employee_id == Employee_id);
            employeeAndInsVM.grpHealth = db.Group_Health.FirstOrDefault(i => i.Employee_id == Employee_id);

            //if (Employee_id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

            //Group_Health g = db.Group_Health.Find(Employee_id);
            //if (g == null)
            //{
            //    return HttpNotFound();
            //}


            return View(employeeAndInsVM);
        }

        //EditUpdate-AuthorizationForm
        public JsonResult AuthorizationFormEditUpdate(int? Employee_id, int? GroupHealthInsurance_id, string PersonOneReleaseInfoTo, string PersonOneRelationship,
            string PersonTwoReleaseInfoTo, string PersonTwoRelationship, string PolicyHolderSignature, DateTime PolicyHolderSignatureDate,
            string PersonOneSignature, DateTime PersonOneSignatureDate, string PersonTwoSignature, DateTime PersonTwoSignatureDate)
        {

            Group_Health g = db.Group_Health
                .Where(i => i.GroupHealthInsurance_id == GroupHealthInsurance_id)
                .Single();

            g.NameOfPersonOneReleaseInfoTo = PersonOneReleaseInfoTo;
            g.PersonOneRelationship = PersonOneRelationship;
            g.NameOfPersonTwoReleaseInfoTo = PersonTwoReleaseInfoTo;
            g.PersonTwoRelationship = PersonTwoRelationship;
            g.AuthorizationFormPolicyHolderSignature = PolicyHolderSignature;
            g.AuthorizationFormPolicyHolderSignatureDate = PolicyHolderSignatureDate;
            g.PersonOneSignature = PersonOneSignature;
            g.PersonOneSignatureDate = PersonOneSignatureDate;
            g.PersonTwoSignature = PersonTwoSignature;
            g.PersonTwoSignatureDate = PersonTwoSignatureDate;

            if (ModelState.IsValid)
            {
                db.Entry(g).State = System.Data.Entity.EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception err)
                {
                    Console.WriteLine(err);
                }

                RedirectToAction("EmpOverview", new { g.Employee_id });
            }

            int result = g.Employee_id;

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteGrpHealthIns(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Group_Health groupHealth = db.Group_Health.Find(id);
            if (groupHealth == null)
            {
                return HttpNotFound();
            }

            return View(groupHealth);
        }

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("DeleteGrpHealthIns")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGroupHealth(int id)
        {
            Group_Health groupHealth = db.Group_Health.Find(id);
            db.Group_Health.Remove(groupHealth);
            db.SaveChanges();

            db.DeleteEmployeeAndDependents(id);

            return RedirectToAction("GrpHealthEnrollment");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
