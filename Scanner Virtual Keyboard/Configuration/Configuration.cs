using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Scanner_Virtual_Keyboard.Configuration
{
    [Serializable]
    public class Configuration
    {
        public KeyboardConfiguration Keyboard { get; set; }
        public PortConfiguration Port { get; set; }

        public Configuration()
        {
            Keyboard = new KeyboardConfiguration();
            Port = new PortConfiguration();
        }

        #region Serialization

        public void Save()
        {
            FileStream fs = null;
            if (File.Exists(GetConfigurationFilename()))
                fs = new FileStream(GetConfigurationFilename(), FileMode.Truncate, FileAccess.Write, FileShare.None);
            else
                fs = new FileStream(GetConfigurationFilename(), FileMode.Create, FileAccess.Write, FileShare.None);
            Debug.Assert(fs != null);
            XmlSerializer xs = new XmlSerializer(typeof(Configuration));
            xs.Serialize(fs, this);
            fs.Close();
        }

        public static Configuration Load()
        {
            Configuration conf = null;
            if (File.Exists(GetConfigurationFilename()))
            {
                FileStream fs = new FileStream(GetConfigurationFilename(), FileMode.Open, FileAccess.Read, FileShare.None);
                XmlSerializer xs = new XmlSerializer(typeof(Configuration));
                conf = (Configuration)xs.Deserialize(fs);
                fs.Close();
            }
            else
            {
                conf = new Configuration();
            }
            Debug.Assert(conf != null);
            return conf;
        }

        #endregion

        public static string GetConfigurationFilename(string name = "Configuration.xml")
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easyiteam", "Scanner Virtual Keyboard");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return Path.Combine(path, name);
        }

    }
}
