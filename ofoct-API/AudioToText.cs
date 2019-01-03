using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Ofoct
{
    public class AudioToText
    {

        public enum Engine
        {
            Baidu,
            CMU_Sphinx
        }

        public class AudioFile
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string Path { get; set; }
            public string TempName { get; set; }

            public AudioFile(string filename)
            {
                Path = filename;
                Name = new FileInfo(filename).Name;
                Type = new FileInfo(filename).Extension.Replace(".", "");
            }
        }

        private const string main_url = "https://www.ofoct.com/audio-converter/audio-to-text.html";
        private const string upload_url = "https://{0}/upload.php";

        private string host;


        public AudioToText()
        {
            LoadPage();
        }

        private void LoadPage()
        {
            using (var client = CreateHttpClient())
            {
                client.Headers.Set("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");

                var res = client.DownloadString(main_url);

                this.host = DetermineHost(res);
            }
        }

        public string Convert(string audioFile, Engine engine = Engine.Baidu)
        {
            var file = new AudioFile(audioFile);

            UploadFile(string.Format(upload_url, this.host), file);

            var converted = ConvertFile(file, engine);

            if (!converted)
                ConvertFile(file, (engine == Engine.Baidu) ? Engine.CMU_Sphinx : Engine.Baidu);

            return DownloadFile(file);
        }


        public string Convert(AudioFile audioFile, Engine engine = Engine.Baidu)
        {
            var converted = ConvertFile(audioFile, engine);

            if (!converted)
                ConvertFile(audioFile, (engine == Engine.Baidu) ? Engine.CMU_Sphinx : Engine.Baidu);

            return DownloadFile(audioFile);
        }




        public AudioFile UploadFile(string fileName)
        {
            var file = new AudioFile(fileName);

            using (var Client = CreateHttpClient())
            {
                var temp = DateTime.Now.Ticks.ToString("X");

                Client.Headers.Set("Content-Type", "multipart/form-data; boundary=----WebKitFormBoundary" + temp);
                Client.Headers.Set("Host", this.host);
                Client.Headers.Set("Referer", main_url);

                var data = Encoding.ASCII.GetBytes(string.Format("------WebKitFormBoundary{0}\r\nContent-Disposition: form-data; name=\"myfile\"; filename=\"{1}\"\r\nContent-Type: audio/{2}\r\n\r\n", temp, new FileInfo(fileName).Name, new FileInfo(fileName).Extension)).ToList();
                data.AddRange(File.ReadAllBytes(fileName));
                data.AddRange(Encoding.ASCII.GetBytes("\r\n------WebKitFormBoundary" + temp));

                Client.Timeout = 300000;

                var res = Encoding.ASCII.GetString(Client.UploadData(string.Format(upload_url, this.host), "POST", data.ToArray()));

                var files = new JavaScriptSerializer().Deserialize<string[]>(res);

                file.TempName = files[0];

                return file;
            }
        }

        private void UploadFile(string url, AudioFile audioFile)
        {
            using (var Client = CreateHttpClient())
            {
                var temp = DateTime.Now.Ticks.ToString("X");

                Client.Headers.Set("Content-Type", "multipart/form-data; boundary=----WebKitFormBoundary" + temp);
                Client.Headers.Set("Host", this.host);
                Client.Headers.Set("Referer", main_url);

                var data = Encoding.ASCII.GetBytes(string.Format("------WebKitFormBoundary{0}\r\nContent-Disposition: form-data; name=\"myfile\"; filename=\"{1}\"\r\nContent-Type: audio/{2}\r\n\r\n", temp, audioFile.Name, audioFile.Type)).ToList();
                data.AddRange(File.ReadAllBytes(audioFile.Path));
                data.AddRange(Encoding.ASCII.GetBytes("\r\n------WebKitFormBoundary" + temp));

                Client.Timeout = 300000;

                var res = Encoding.ASCII.GetString(Client.UploadData(url, "POST", data.ToArray()));

                var files = new JavaScriptSerializer().Deserialize<string[]>(res);

                audioFile.TempName = files[0];
            }
        }

        private bool ConvertFile(AudioFile audioFile, Engine engine)
        {
            var Client = CreateHttpClient();
            Client.Headers.Set("Host", this.host);
            Client.Headers.Set("Referer", main_url);

            Client.Timeout = 150000;

            var res = Client.DownloadString(string.Format(@"https://" + this.host + "/convert-file_v2.php?cid=audio2txt&output=txt&tmpfpath={0}&row=file1&sourcename={1}&sdk={2}&rowid=file1", audioFile.TempName, audioFile.Name, engine.ToString().ToLower()));

            return (res.Split('|')[1] == "SUCCESS");
        }

        private string DownloadFile(AudioFile audioFile)
        {
            using (var client = CreateHttpClient())
            {
                var txt = client.DownloadString("https://" + this.host + "/get-file.php?type=get&genfpath=/tmp/" + audioFile.TempName + ".txt&downloadsavename=tempfile.txt");
                return txt.Split(',')[0];
            }
        }

        private string DetermineHost(string sourceCode)
        {
            return Regex.Match(sourceCode, @"ct\d").Value + ".ofoct.com";
        }

        private HttpClient CreateHttpClient(WebHeaderCollection Headers = null)
        {
            return new HttpClient()
            {
                DefaultHeaders = new WebHeaderCollection()
                {
                    { "Accept", "*/*" },
                    { "Accept-Encoding", "gzip, deflate" },
                    { "Accept-Language", "en-US,en" },
                    { "User-Agent", "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36" },
                    { "Origin", "https://www.ofoct.com" }
                }
            };
        }


    }
}
