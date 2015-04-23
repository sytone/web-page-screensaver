using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using WebPageScreensaver.Properties;
using Timer = System.Windows.Forms.Timer;

namespace WebPageScreensaver
{
    public partial class ScreensaverForm : Form
    {
        private DateTime StartTime = DateTime.Now;

        public ScreensaverForm()
        {
            GlobalUserEventHandler gueh = new GlobalUserEventHandler();
            gueh.Event += new GlobalUserEventHandler.UserEvent(CloseAfter1Second);
            Application.AddMessageFilter(gueh);

            InitializeComponent();

            reloadTimer = new Timer();

            webBrowser.ScriptErrorsSuppressed = true;

            ////RegistryKey zoomReg = Registry.LocalMachine.CreateSubKey("Software\\Microsoft\\Internet Explorer\\Zoom");
            ////Trace.Write("Zoom Setting: ");
            ////Trace.WriteLine(zoomReg.GetValue("ZoomDisabled"));
            ////zoomReg.SetValue("ZoomDisabled", 0, RegistryValueKind.DWord);
            
            
        }

        private Timer reloadTimer;
        private readonly List<Dashboard> dashboards = new List<Dashboard>();
        private Dashboard currentDashboard;

        private void ScreensaverForm_Load(object sender, EventArgs e)
        {
            string dataFile;

            try
            {
                RegistryKey reg = Registry.LocalMachine.CreateSubKey(Program.KEY);
                dataFile = (string)reg.GetValue("DataPath", ".\\dashboarddata.csv");
                reg.Close();
            }
            catch (Exception)
            {
                dataFile = ".\\dashboarddata.csv";
            }
            if (!String.IsNullOrWhiteSpace(dataFile))
            {
                LoadDashboardsFromFile(dataFile);
                reloadTimer.Interval = 2000; // Inital load after 2000
                reloadTimer.Tick += (o, args) => 
                {
                    reloadTimer.Stop();
                    Trace.WriteLine("Timer Ticked!");
                    currentDashboard = dashboards.Next(currentDashboard);
                    Trace.WriteLine(currentDashboard);

                    if ((currentDashboard.StartHour == 0 && currentDashboard.EndHour == 0) ||
                        (DateTime.Now.Hour > currentDashboard.StartHour && DateTime.Now.Hour < currentDashboard.EndHour))
                    {
                        webBrowser.Navigate(currentDashboard.Url);
                        while (webBrowser.IsBusy) { }
                        Trace.Write("Requested Scale: ");
                        Trace.WriteLine(currentDashboard.Scale);
                        this.Focus();
                        webBrowser.Focus();
                        switch (currentDashboard.Scale)
                        {
                            case -2:
                                SendKeys.Send("^0");
                                SendKeys.Send("^+");
                                SendKeys.Send("^+");
                                break;
                            case -1:
                                SendKeys.Send("^0");
                                SendKeys.Send("^+");
                                break;
                            case 0:
                                SendKeys.Send("^0");
                                break;
                            case 1:
                                SendKeys.Send("^0");
                                SendKeys.Send("^-");
                                break;
                            case 2:
                                SendKeys.Send("^0");
                                SendKeys.Send("^-");
                                SendKeys.Send("^-");
                                break;
                        }
                        //((SHDocVw.WebBrowser)webBrowser.ActiveXInstance).ExecWB(SHDocVw.OLECMDID.OLECMDID_OPTICAL_ZOOM,
                        //    SHDocVw.OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER, currentDashboard.Scale, IntPtr.Zero);
                    }
                    Trace.WriteLine(currentDashboard.Period);
                    reloadTimer.Interval = currentDashboard.Period * 1000;
                    Trace.WriteLine(reloadTimer.Interval);
                    Trace.Write("Timer reset: ");
                    Trace.WriteLine(reloadTimer.Interval);
                    if (currentDashboard == dashboards.Last())
                    {
                        LoadDashboardsFromFile(dataFile);
                    }
                    reloadTimer.Start();
                };
                reloadTimer.Start();
            }
            else
            {
                webBrowser.Navigate("about:blank");
                Thread.Sleep(10000);
            }
            //webBrowser.Navigate((string)reg.GetValue("UncPath", ""));
            
        }


        private void LoadDashboardsFromFile(string dataFile)
        {
            string line;
            dashboards.Clear();
            FileStream aFile = new FileStream(dataFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(aFile);

            // read data in line by line
            while ((line = sr.ReadLine()) != null)
            {
                Trace.WriteLine(line);
                if (line.StartsWith("http") || line.StartsWith("file"))
                {
                    dashboards.Add(new Dashboard(line.Split(',')));
                    Trace.WriteLine(dashboards.Last());
                }
            }
            sr.Close();
        }

        private void CloseAfter1Second()
        {
            if (StartTime.AddSeconds(1) < DateTime.Now)
                Close();
        }

    }
}