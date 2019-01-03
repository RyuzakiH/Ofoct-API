using Ofoct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static Ofoct.AudioToText;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var audioToText = new AudioToText();


            var wochitmar = audioToText.UploadFile(@"C:\Users\Ryuzaki\Desktop\wochitmar_txt.mp3");
            var wochitmar_baidu = audioToText.Convert(wochitmar, AudioToText.Engine.Baidu);
            var wochitmar_cmu = audioToText.Convert(wochitmar, AudioToText.Engine.CMU_Sphinx);

            var wochitmar_baidu = audioToText.Convert(@"C:\Users\Ryuzaki\Desktop\wochitmar_txt.mp3", AudioToText.Engine.Baidu);

            var wochitmar_cmu = audioToText.Convert(@"C:\Users\Ryuzaki\Desktop\wochitmar_txt.mp3", AudioToText.Engine.CMU_Sphinx);




            var application = audioToText.UploadFile(@"C:\Users\Ryuzaki\Desktop\application.ogg");
            var application_baidu = audioToText.Convert(application, AudioToText.Engine.Baidu);
            var application_cmu = audioToText.Convert(application, AudioToText.Engine.CMU_Sphinx);

            var application_cmu = audioToText.Convert(@"C:\Users\Ryuzaki\Desktop\application.ogg", AudioToText.Engine.CMU_Sphinx);

            var application_baidu = audioToText.Convert(@"C:\Users\Ryuzaki\Desktop\application.ogg", AudioToText.Engine.Baidu);




            var congratulations = audioToText.UploadFile(@"congratulations.mp3");
            var congratulations_baidu = audioToText.Convert(congratulations, AudioToText.Engine.Baidu);
            var congratulations_cmu = audioToText.Convert(congratulations, AudioToText.Engine.CMU_Sphinx);

            var congratulations_cmu = audioToText.Convert(@"C:\Users\Ryuzaki\Desktop\congratulations.mp3", AudioToText.Engine.CMU_Sphinx);

            var congratulations_baidu = audioToText.Convert(@"C:\Users\Ryuzaki\Desktop\congratulations.mp3", AudioToText.Engine.Baidu);



        }
    }
}
