using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;

namespace Scanner_Virtual_Keyboard
{
    public class SerialMonitor
    {
        private Thread m_thread;
        private bool active_monitor;
        private Configuration.PortConfiguration config;
        private SerialPort m_port;

        public event Action<string> OnReceived;
        public event Action<string> OnStatusChanged;

        public SerialMonitor(Configuration.PortConfiguration Configuration)
        {
            config = Configuration;
        }

        public void Start()
        {
            active_monitor = true;
            Debug.Assert(m_thread == null);
            m_thread = new Thread(new ThreadStart(MonitorThread));
            m_thread.Start();
        }

        private void M_port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
#if DEBUG
            Console.WriteLine("ErrorReceived : {0}", e.ToString());
#endif
            m_port = null;
        }

        private void M_port_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
#if DEBUG
            Console.WriteLine("PinChanged: ", e.ToString());
#endif
        }

        private void M_port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
#if DEBUG
            Console.WriteLine("DataReceived: {0}", e.ToString());
#endif
            SerialPort port = (SerialPort)sender;
            try
            {
                string line = port.ReadLine().Trim();
                if (line != string.Empty && OnReceived != null)
                    ReceivedEvent(line);
            }
            catch(Exception err)
            {
#if DEBUG
                Console.WriteLine(err.Message);
#endif
                
            }
        }

        public void Stop()
        {
            active_monitor = false;
            if (m_thread != null)
            {
                m_thread.Join();
                m_thread = null;
            }
        }


        private void ReceivedEvent(string data)
        {
            if (OnReceived == null) return;
            OnReceived.Invoke(data);
        }

        private void StatusChangeEvent(string message)
        {
            if (OnStatusChanged == null) return;
            OnStatusChanged(message);
        }


        private void MonitorThread()
        {
            while (active_monitor)
            {
                if (config.isPortReady())
                {
                    StatusChangeEvent(string.Format("Device {0} connected.", config.portName));
                    try
                    {
                        if (m_port == null)
                        {
                            m_port = config.CreatePort();
                            m_port.DataReceived += new SerialDataReceivedEventHandler(M_port_DataReceived);
                            m_port.PinChanged += new SerialPinChangedEventHandler(M_port_PinChanged);
                            m_port.ErrorReceived += new SerialErrorReceivedEventHandler(M_port_ErrorReceived);
                            m_port.DtrEnable = true;
                            m_port.RtsEnable = true;
                        }

                        if (!m_port.IsOpen)
                        {
                            m_port.Open();
                            continue;
                        }
                        else
                        {
                            StatusChangeEvent(string.Format("Device {0} ready.", config.portName));
                        }
                    }
                    catch (ThreadAbortException)
                    {
                        m_port.Close();
                        m_port.Dispose();
                        break;
                    }
                    catch (TimeoutException)
                    {
                        Thread.Sleep(50);
                        continue;
                    }
                    catch (Exception err)
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine(err);
                    }
                    finally
                    {
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    StatusChangeEvent(string.Format("Device not found. ({0})",active_monitor.ToString()));
                    Thread.Sleep(1000);
                }
            }

            try
            {
                if (m_port != null)
                {
                    if(m_port.IsOpen) m_port.Close();
                    m_port.Dispose();
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
        }
    }
}
