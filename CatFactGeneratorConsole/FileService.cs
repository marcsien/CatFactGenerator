using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFactGeneratorConsole
{
    public class FileService
    {
        private readonly string _filePath;
        public FileService(string filePath)
        {
            _filePath = filePath;
        }

        public void AppendToFile(string content)
        {
            using (StreamWriter writer = File.AppendText(_filePath))
            {
                writer.WriteLine(content);
            }
        }
    }
}
