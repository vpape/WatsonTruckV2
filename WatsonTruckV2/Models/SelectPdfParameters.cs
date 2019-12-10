using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Text;

namespace WatsonTruckV2.Models
{
    public class SelectPdfParameters
    {
        public string key { get; set; }
        public string url { get; set; }
        public string html { get; set; }
        public string base_url { get; set; }

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
            System.Console.WriteLine("Starting conversion with WebClient ...");

            // set parameters
            SelectPdfParameters parameters = new SelectPdfParameters();
            parameters.key = apiKey;
            parameters.url = GrpHInsURL;

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
        }
    }
}
