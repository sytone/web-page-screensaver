using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WebPageScreensaver
{
    public partial class PreferencesForm : Form
    {
        public PreferencesForm()
        {
            InitializeComponent();
        }

        private void PreferencesForm_Load(object sender, EventArgs e)
        {
            try
            {
                RegistryKey reg = Registry.LocalMachine.CreateSubKey(Program.KEY);
                textBox1.Text = (string)reg.GetValue("DataPath", "\\\\Machine\\path");
                reg.Close();
            }
            catch (Exception)
            {
                RegistryKey reg = Registry.LocalMachine.OpenSubKey(Program.KEY);
                textBox1.Text = (string)reg.GetValue("DataPath", "\\\\Machine\\path");
                textBox1.ReadOnly = true;
                reg.Close();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                try
                {
                    RegistryKey reg = Registry.LocalMachine.CreateSubKey(Program.KEY);
                    reg.SetValue("DataPath", textBox1.Text);
                    reg.Close();
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }

            base.OnClosed(e);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
