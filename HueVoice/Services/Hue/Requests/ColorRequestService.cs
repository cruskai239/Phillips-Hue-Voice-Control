using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueVoice.Services.Hue.Requests
{
    class LightStatePutRequestService  : HueRequestService
    {
        public const string path = "lights/{0}/state";
        private string json = "";

        private Dictionary<string, int> ColorMap;

        public LightStatePutRequestService()
        {
            ColorMap = new Dictionary<string, int>()
            {
                {"red" , 0 },
                {"orange", 6375 },
                {"yellow", 12750 },
                {"green", 25500 },
                {"blue", 46920 },
                {"purple", 56227 }
            };
        }


        public void ValidateParameter(Dictionary<string, object> extraParams)
        {
            if (extraParams.ContainsKey("hue"))
            {
                extraParams["hue"] = GetColorCode(extraParams["hue"].ToString());
            }

            if (extraParams.ContainsKey("bri"))
            {
                double scaleModifier = 0;
                double.TryParse(extraParams["bri"].ToString(), out scaleModifier);

                extraParams["bri"] = Math.Floor((scaleModifier / 100) * 254);
            }
        }
        public string GetJson(Dictionary<string, object> extraParams)
        {
            foreach (KeyValuePair<string, object> extraParam in extraParams)
            {
                json += "\"" + extraParam.Key + "\":";
                var output = 0;
                if (int.TryParse(extraParam.Value.ToString(), out output))
                {
                    json += extraParam.Value;
                }
                else
                {
                    json += "\"" + extraParam.Value + "\"";
                }
            }
            return json;
        }

        public void MakeRequest(string path, Dictionary<string, object> extraParams)
        {
            ValidateParameter(extraParams);
            string json = GetJson(extraParams);
           
            for(int i = 1; i < 4; i++)
            {
                Console.WriteLine("{" + json + "}");
                Console.WriteLine(base.MakeRequest(String.Format(path, i), "{" + json + "}", extraParams));
            }
        }

        public int GetColorCode(string color)
        {
            int result = 0;
            ColorMap.TryGetValue(color, out result);
            return result;
        }
    }
}
