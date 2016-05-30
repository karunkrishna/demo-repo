using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;


namespace HtmlDOM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate(textBox1.Text);




        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
                textBox2.Text = textBox2.Text + Environment.NewLine + (e.Url.OriginalString.ToString() + " has been loaded");

            
            //OriginalString
        }
    }
}
