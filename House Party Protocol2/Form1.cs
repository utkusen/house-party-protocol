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
using System.Net;
using System.Threading;
using Microsoft.VisualBasic;
using Microsoft.Win32;



namespace House_Party_Protocol2
{
    public partial class Form1 : Form
    {
        string pathToRemove;
        string readUrl;
        int connectionStatus;
        string userMessage;
        int tempCheckbox3 = 0;
        int tempform2;
        string directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            tempCheckbox3 = 0;

            //load settings
            readOptions();

            //add program to windows startup
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue("House Party Protocol", "\"" + Application.ExecutablePath + "\"");
            }   

        }

 

        public void readOptions()
        {
            //load settings from txt files
            if (File.Exists(Path.Combine(directory, "path.txt")))
            {
                TextReader tr = new StreamReader(Path.Combine(directory, "path.txt"));

                pathToRemove = tr.ReadLine().ToString();
                textBox1.Text = pathToRemove;

                
                tr.Close();
            }

            if (File.Exists(Path.Combine(directory, "url.txt")))
            {
                TextReader tr = new StreamReader(Path.Combine(directory, "url.txt"));
                readUrl = tr.ReadLine().ToString();
                textBox2.Text = readUrl;
               

                
                tr.Close();
            }

            if (File.Exists(Path.Combine(directory, "connect.txt")))
            {
                TextReader tr = new StreamReader(Path.Combine(directory, "connect.txt"));

                connectionStatus = Int32.Parse(tr.ReadLine());

                
                tr.Close();

                
            }

            if (File.Exists(Path.Combine(directory, "usermessage.txt")))
            {
                TextReader tr = new StreamReader(Path.Combine(directory, "usermessage.txt"));

                userMessage = tr.ReadLine().ToString();

               
                tr.Close();
            }

            if (File.Exists(Path.Combine(directory, "options.txt")))
            {
                TextReader tr = new StreamReader(Path.Combine(directory, "options.txt"));

                if (tr.ReadLine().ToString() == "1")
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
                if (tr.ReadLine().ToString() == "1")
                {
                    checkBox2.Checked = true;
                }
                else
                {
                    checkBox2.Checked = false;
                }
                if (tr.ReadLine().ToString() == "1")
                {
                    checkBox3.Checked = true;
                }
                else
                {
                    checkBox3.Checked = false;
                }
                if (tr.ReadLine().ToString() == "1")
                {
                    checkBox4.Checked = true;
                }
                else
                {
                    checkBox4.Checked = false;
                }
                

                
                tr.Close();
            }

            if (connectionStatus == 1)
            {
                label2.Text = "Status: Listening";
                timer1.Enabled = true;
            }
            tempCheckbox3 = 1;
        }

 
        //AES encryption algorithm
        public byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        //created random ciphers for AES algorithm
        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*!=?&/";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        //encrypts target directory
        public void DestroyDirectory(string location)
        {
            
           string[] files = Directory.GetFiles(location);
           string[] childDirectories = Directory.GetDirectories(location);

           for (int i = 0; i < files.Length; i++)
           {
               EncryptFile(files[i]);
           }

           for (int i = 0; i < childDirectories.Length; i++)
           {
               DestroyDirectory(childDirectories[i]);
           }

            //removes windows event logs
           if (checkBox1.Checked == true)
           {
               foreach (var eventLog in EventLog.GetEventLogs())
               {
                   eventLog.Clear();
                   eventLog.Dispose();
               }
           }

            //shutdowns the pc
           if (checkBox2.Checked == true)
           {
               ProcessStartInfo psi = new ProcessStartInfo();
               psi.FileName = "shutdown.exe";
               psi.Arguments = "-s -f -t 0";
               psi.CreateNoWindow = true;
               Process p = Process.Start(psi);
           }

           if (checkBox4.Checked == true)
           {
               this.Hide();
           }

            //shows message
           if (checkBox3.Checked == true && tempform2 == 0)
           {
               Form2 frm2 = new Form2();
               frm2.Show();
               tempform2 = 1;
           }

  
        }

        //runs AES encryption
        public void EncryptFile(string filelocation)
        {
            string file = filelocation;
            
            byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(CreatePassword(25));

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            string fileEncrypted = filelocation;

            File.WriteAllBytes(fileEncrypted, bytesEncrypted);


        }

        //starts wiping. encrypts every file for 3 times
        public void startParty()
        {
            disconnect();
            timer1.Enabled = false;
            for (int i = 0; i < 4; i++)
            {
                
                DestroyDirectory(pathToRemove);
                
            }

            
        }


        //saves settings to txt files
        public void saveOptions()
        {
            // directory path
            TextWriter tw = new StreamWriter(Path.Combine(directory, "path.txt"));
            tw.WriteLine(pathToRemove);
            tw.Close();
            // other options
            TextWriter ta = new StreamWriter(Path.Combine(directory, "options.txt"));
            if (checkBox1.Checked)
            {
                ta.WriteLine(1);
            }
            else
            {
                ta.WriteLine(0);
            }

            if (checkBox2.Checked)
            {
                ta.WriteLine(1);
            }
            else
            {
                ta.WriteLine(0);
            }
            if (checkBox3.Checked)
            {
                ta.WriteLine(1);
            }
            else
            {
                ta.WriteLine(0);
            }
            if (checkBox4.Checked)
            {
                ta.WriteLine(1);
            }
            else
            {
                ta.WriteLine(0);
            }
            ta.Close();

            TextWriter ts = new StreamWriter(Path.Combine(directory, "url.txt"));
            ts.WriteLine(textBox2.Text.ToString());
            ts.Close();

            TextWriter tb = new StreamWriter(Path.Combine(directory, "usermessage.txt"));
            tb.WriteLine(userMessage);
            tb.Close();
            MessageBox.Show("Saved");
        }

        //checks the url for command. if the txt's value is "1" operation starts
        public void listener(string url)
        {
            label2.Text = "Status: Listening";
            WebClient client = new WebClient();
            
            string s = client.DownloadString(url);
            

            if (s == "1")
            {
                label2.Text = "Status: Read done";
                startParty();
            }
            

        }


        //checks the url 
        public void websiteCheck(string url)
        {
            try
            {
                System.Net.WebClient client = new WebClient();
                client.DownloadData(url);
                MessageBox.Show("Address is reachable");
            }
            catch
            {
                MessageBox.Show("Address is unreachable");
            }
        }

        public void disconnect()
        {
            TextWriter tw = new StreamWriter(Path.Combine(directory, "connect.txt"));
            tw.WriteLine("0");
            tw.Close();
            timer1.Enabled = false;
            label2.Text = "Status: Idle";
            
        }


        private void button1_Click(object sender, EventArgs e)
        {
            
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                pathToRemove = folder.SelectedPath;
                textBox1.Text = pathToRemove;

                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("This is irreversible. Are you sure?", "Folder Deletion", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                startParty();
            }
            
            
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label2.Text = "Status: Listening";
            readUrl = textBox2.Text.ToString();
            timer1.Enabled = true;

            TextWriter tw = new StreamWriter(Path.Combine(directory, "connect.txt"));
            tw.WriteLine("1");
            tw.Close();
            
           

        }

        private void button4_Click(object sender, EventArgs e)
        {
            saveOptions();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            listener(readUrl);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            disconnect();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            
            if (checkBox3.Checked == true && tempCheckbox3 == 1)
            {
                userMessage = Interaction.InputBox("Enter Your Message", "Your Message", "Too late :(", 10, 10);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form3 frm3 = new Form3();
            frm3.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form4 frm4 = new Form4();
            frm4.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            websiteCheck(textBox2.Text.ToString());
        }

     
       
      
    }
}
