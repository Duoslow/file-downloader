using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace downloader
{
    public partial class Form1 : Form
    {

        WebClient webClient;
        Stopwatch sw = new Stopwatch();
        public Form1()
        {
            InitializeComponent();
            
        }
        public void hee()
        {
            textBox1.Text = "";
            file.Text = "";
            labelSpeed.Text = "";
            labelPerc.Text = "";
            labelDownloaded.Text = "";
            progressBar.Value = 0;
        }
        public Uri URL;
        public void DownloadFile(string urlAddress, string location)
        {
           
                   using (webClient = new WebClient())
                   {
                       webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                       webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                Uri address = new Uri(urlAddress);
                if (address.Scheme == Uri.UriSchemeHttp)
                {
                    Console.WriteLine("HTTP");
                    if (checkBox1.Checked)
                    {
                        webClient.Credentials = new NetworkCredential(username.Text, password.Text);
                    }
                    URL = urlAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? new Uri(urlAddress) : new Uri("http://" + urlAddress);
                }
                if (address.Scheme == Uri.UriSchemeHttps)
                {
                    
                    Console.WriteLine("Https");
                    if (checkBox1.Checked)
                    {
                        webClient.Credentials = new NetworkCredential(username.Text, password.Text);
                    }
                    URL = urlAddress.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ? new Uri(urlAddress) : new Uri("https://" + urlAddress);
                }
                if (address.Scheme == Uri.UriSchemeFtp)
                {
                    Console.WriteLine("ftp");
                    if (checkBox1.Checked)
                    {
                        webClient.Credentials = new NetworkCredential(username.Text, password.Text);
                    }
                    
                    URL = urlAddress.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase) ? new Uri(urlAddress) : new Uri("ftp://" + urlAddress);
                }
                sw.Start();
               
                       try
                       {
                           webClient.DownloadFileAsync(URL, location);
                       }
                       catch (Exception ex)
                       {
                           MessageBox.Show(ex.Message);
                       }
                   }
        }


       

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            labelSpeed.Text = string.Format("{0} mb/s", (e.BytesReceived / 1024d / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));
            progressBar.Value = e.ProgressPercentage;
            labelPerc.Text = e.ProgressPercentage.ToString() + "%";
            labelDownloaded.Text = string.Format("{0} MB / {1} MB",
                (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));

        }
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            sw.Reset();
            if (e.Cancelled == true)
            {
                MessageBox.Show("Download has been canceled.");
                File.Delete(za);
                hee();
            }
            else
            {
                MessageBox.Show("Download completed!");
                Process.Start("explorer.exe", "/select," + za + "");
                hee();
            }
        }
        public string za;
        private  void button1_Click(object sender, EventArgs e)
        {
            var lastPart = textBox1.Text.Split('/').Where(x => !string.IsNullOrWhiteSpace(x)).LastOrDefault();
            file.Text = lastPart;
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                 za = folderBrowserDialog1.SelectedPath+"\\"+lastPart;
                DownloadFile(textBox1.Text, za);
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webClient.CancelAsync();
            hee();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                groupBox1.Visible = true;
            }
            else
            {
                groupBox1.Visible = false;
            }
        }
    }
}
