using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NML.Core
{
    public class ConfigurationHelper
    {
        public static string GetConfiguration(string AppKey)
        {
            try
            {
                return System.IO.File.ReadAllText(GetPath(AppKey));
            }
            catch { return null; }
        }

        private static string GetPath(string AppKey)
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\NML";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            return directory+"\\" + AppKey;
        }

        public static void SetConfiguration(string AppKey, string configuration)
        {
            System.IO.File.WriteAllText(GetPath(AppKey), configuration);
        }
    }
}
