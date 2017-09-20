using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HueVoice.Services.Hue.Requests;

namespace HueVoice.Services
{
    class SpeechRecognitionService
    {

        ManualResetEvent _completed;

        public SpeechRecognitionService(ManualResetEvent completed)
        {
            this._completed = completed;
            this.InitializeService();
        }

        public void InitializeService()
        {
            SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine();
            List<string> brightnessChoices = new List<string>();
            for (int i = 0; i < 105; i += 5)
            {
                brightnessChoices.Add(String.Format("brightness {0} percent", i));
            }
            var brightnessGrammar = new Grammar(new Choices(brightnessChoices.ToArray()));
            

            _recognizer.LoadGrammar(new Grammar(new Choices("color red", "color blue", "color green", "color yellow", "color orange", "color purple", "computer exit")) { Name = "colors" });
            _recognizer.LoadGrammar(brightnessGrammar);
            _recognizer.SpeechRecognized += _recognizer_SpeechRecognized;
            _recognizer.SetInputToDefaultAudioDevice(); // set the input of the speech recognizer to the default audio device
            _recognizer.RecognizeAsync(RecognizeMode.Multiple); // recognize speech asynchronous

            _completed.WaitOne(); // wait until speech recognition is completed

            _recognizer.Dispose();
        }

        void _recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            
            
            Console.WriteLine(e.Result.Text);
            if (e.Result.Text == "computer exit")
            {
                _completed.Set();
                return;
            }
            else if(e.Result.Text.StartsWith("color"))
            {
                string color = e.Result.Text.Replace("color ", "");
                var colorRequestService = new LightStatePutRequestService();
                var extraParams = new Dictionary<string, object>();
                extraParams.Add("hue", color);                
                colorRequestService.MakeRequest(LightStatePutRequestService.path, extraParams);
            }
            else if(e.Result.Text.StartsWith("brightness")){
                string brightness = e.Result.Text.Replace("brightness ", "").Replace(" percent", "");
                var colorRequestService = new LightStatePutRequestService();
                var extraParams = new Dictionary<string, object>();
                extraParams.Add("bri", brightness);
                colorRequestService.MakeRequest(LightStatePutRequestService.path, extraParams);
            }
            
        }
    }
}
