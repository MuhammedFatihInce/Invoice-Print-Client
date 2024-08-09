using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace partsSoftClient.Helpers
{
	public class FileHelper
	{
        public static Dictionary<string, string> ReadConfigFile(string filePath)
        {
            var configDictionary = new Dictionary<string, string>();
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(baseDirectory).Parent.Parent.FullName;
            string absoluteFilePath = Path.Combine(projectDirectory, filePath);
            // Dosyayı satır satır oku
            foreach (var line in File.ReadLines(absoluteFilePath))
            {
                // Satırı key=value olarak ayır
                var parts = line.Split(new char[] { '=' }, 2);
                if (parts.Length == 2)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Trim();


                    configDictionary[key] = value;
                }
            }

            return configDictionary;
        }
        public static string getFolderPath(string fileName)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string uploadPath = Path.Combine(desktopPath, "uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            string filePath = Path.Combine(uploadPath, fileName);

            return filePath;
        }
    }
}
