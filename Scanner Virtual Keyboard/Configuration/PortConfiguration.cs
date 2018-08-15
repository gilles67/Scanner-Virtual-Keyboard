using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Xml.Serialization;

namespace Scanner_Virtual_Keyboard.Configuration
{
    [Serializable]
    public class PortConfiguration
    {
        public string portName { get; set; }
        public int baudRate { get; set; }
        public Parity parity { get; set; }
        public int dataBits { get; set; }
        public StopBits stopBits { get; set; }
        public ReturnChar eolChar { get; set; }

        public PortConfiguration()
        {
            portName = "COM1";
            baudRate = 9600;
            parity = Parity.None;
            dataBits = 8;
            stopBits = StopBits.One;
        }

        public SerialPort CreatePort()
        {
            SerialPort port = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            switch (eolChar)
            {
                case ReturnChar.CarriageReturn:
                    port.NewLine = "\r";
                    break;
                case ReturnChar.LfCr:
                    port.NewLine = "\r\n";
                    break;
                case ReturnChar.LineFeed:
                default:
                    port.NewLine = "\n";
                    break;
            }
            return port;
        }

        public bool isPortReady()
        {
            List<string> ports = new List<string>(SerialPort.GetPortNames());
            return ports.Contains(this.portName);
        }

        [XmlIgnore]
        public List<string> LsPortNames
        {
            get
            {
                return SerialPort.GetPortNames().ToList<string>();
            }
        }
        [XmlIgnore]
        public List<int> LsBaudRates
        {
            get { return new List<int> { 75, 110, 134, 150, 300, 600, 1200, 1800, 2400, 4800, 7200, 9600, 14400, 19200, 38400, 57600, 115200, 128000 }; }
        }
        [XmlIgnore]
        public List<int> LsDataBits
        {
            get { return new List<int> { 4, 5, 6, 7, 8 }; }
        }
        [XmlIgnore]
        public List<Parity> LsParity
        {
            get { return new List<Parity> { Parity.Even, Parity.Mark, Parity.None, Parity.Odd, Parity.Space }; }
        }
        [XmlIgnore]
        public List<StopBits> LsStopBits
        {
            get { return new List<StopBits> { StopBits.One, StopBits.OnePointFive, StopBits.Two }; }
        }
        [XmlIgnore]
        public List<ReturnChar> LsReturnChar
        {
            get { return new List<ReturnChar> { ReturnChar.LineFeed, ReturnChar.CarriageReturn, ReturnChar.LfCr }; }
        }

    }

    public enum ReturnChar
    {
        LineFeed = 1,
        CarriageReturn = 2,
        LfCr = 3
    }
    

}
