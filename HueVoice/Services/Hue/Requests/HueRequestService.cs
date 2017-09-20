using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.IO;


namespace HueVoice.Services.Hue.Requests
{

    class HueRequestService
    {
        protected string _username = "rW1fZ60PxHFZrihSTyyVbLz9hChKIgVAeSZrzpGv";
        protected string _baseUrl = "http://10.0.0.3/api/";

        

        public string MakeRequest(string path, string paramsJson, Dictionary<string, object> extraParams = null)
        {

            var request = (HttpWebRequest)WebRequest.Create(_baseUrl + _username + "/" + path);
            request.ContentType = "application/json";
            request.Method = "PUT";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(paramsJson);
            }

            var response = (HttpWebResponse)request.GetResponse();
            using(var streamReader= new StreamReader(response.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }

            
        }


       



    }
}
