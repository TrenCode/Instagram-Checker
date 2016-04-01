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
using System.IO;
using System.Threading;
namespace FlatUIRemake
{
    public partial class Form1 : Form
    {
        int cracks = 0;
        ListBox ls = new ListBox();
        public Form1()
        {
            InitializeComponent();
        }
       // CookieContainer session; 'for something else lol

        bool tos = false;
        void StartChecking()
        {
            var ig = new InstagramChecker();
            
            //ls.Items.Add("fkn"); 'Had this shit for testing 
            if (tos)
            {
                foreach (string username in ls.Items)
                {
                    bool av = ig.IsUsernameAvailable(username);
                    if (av)
                    {
                        AddAccount(username, "Available");
                    }
                    else
                    {
                        AddAccount(username, "Taken");
                    }
                }
            }
            else
            {
                MessageBox.Show("Agree to the ToS", "Alert");
            }
        }


        void AddAccount(string username, string status)
        {
            ListViewItem itm = new ListViewItem(username);
            itm.SubItems.Add(status);
            //itm.SubItems.Add(capture);
            listView1.Items.Add(itm);
            cracks = cracks + 1;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                tos = true;
            }
            else
            {
                tos = false;
            }
        }

       

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Thread thr = new Thread(new ThreadStart(StartChecking));
            thr.Start();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            if (f.ShowDialog() == DialogResult.OK)
            {
                ls.Items.Clear();

                List<string> lines = new List<string>();
                using (StreamReader r = new StreamReader(f.OpenFile()))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        ls.Items.Add(line);
                        
                       

                    }
                }
            }
            var count = ls.Items.Count;
            MessageBox.Show("Total Items Loaded : " + count);
        }

        private void formSkin1_Click(object sender, EventArgs e)
        {

        }

        
    }
}
