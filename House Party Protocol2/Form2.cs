using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using OpenQA.Selenium.Chrome;
using System.Net;

namespace House_Party_Protocol2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            BackColor = Color.Lime;
            TransparencyKey = Color.Lime;
            FormBorderStyle = FormBorderStyle.None;
            if (File.Exists("usermessage.txt"))
            {
                TextReader tr = new StreamReader("usermessage.txt");

                label1.Text = tr.ReadLine().ToString();
                

                // close the stream
                tr.Close();
            }

        }
    }
}
