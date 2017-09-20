using System.Threading;


namespace HueVoice
{
    class Program
    {
        static ManualResetEvent _completed = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            new Services.SpeechRecognitionService(_completed);
        }
         
    }
}
