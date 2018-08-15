using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Scanner_Virtual_Keyboard.Configuration
{
    public class KeyboardConfiguration
    {
        public bool? logKeystorke { get; set; }
        public bool? appendReturn { get; set; }
        public int delayKeystorke { get; set; }

        public KeyboardConfiguration()
        {
            appendReturn = true;
            logKeystorke = false;
            delayKeystorke = 0;
        }

        [XmlIgnore]
        public List<int> LsDelayKeystorke
        {
            get { return new List<int> { 0, 20, 40 }; }
        }
    }
}
