using System.IO;

namespace Ofoct
{
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
}
