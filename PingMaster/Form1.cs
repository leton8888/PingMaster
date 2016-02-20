using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PingMaster
{

    public partial class Form1 : Form
    {


        private DataTable dt = new DataTable("IPTable");
        private static Hashtable ht = new Hashtable();
        private static int ListSize = 0;
        private static bool isPingOver = false;
        public Form1()
        {

            InitializeComponent();

            button1.Enabled = false;
            //读配置文件
            StreamReader sr = new StreamReader("../../list.txt", Encoding.Default);
            String line;
            String[] splitStr;



            dt.Columns.Add("名称", typeof(string));
            dt.Columns.Add("地址", typeof(string));
            dt.Columns.Add("Ping", typeof(long));
            dt.Columns.Add("丢包", typeof(int));
            DataRow dr;
            while ((line = sr.ReadLine()) != null)
            {
                dr = dt.NewRow();
                splitStr = line.Split(',');
                dr[0] = splitStr[0];
                dr[1] = splitStr[1];
                dt.Rows.Add(dr);
            }

            dataGridView1.DataSource = dt;
            ListSize = dataGridView1.Rows.Count;
            button1.Enabled = true;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            isPingOver = false;
            ht.Clear();
            Worker w;
            Thread t;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                //Console.WriteLine(dataGridView1[1, i].Value);
                w = new Worker(dataGridView1[1, i].Value.ToString());
                //w = new Worker(dataGridView1, i);
                t = new Thread(w.DoWork);
                t.Start();
            }

            while (!isPingOver)
            {

            }
           
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                PingHelper.PingResult result = (PingHelper.PingResult)ht[dataGridView1[1, i].Value.ToString()];
                dataGridView1[2, i].Value = result.GetAvePing().ToString();
                dataGridView1[3, i].Value = result.GetTimeOut().ToString();
            }
            
            button1.Enabled = true;
        }



        public class Worker
        {
            private string address;
            //private DataGridView dgv;
            private int index;
            /*
            public Worker(DataGridView dgv, int rowIndex)
            {
                this.dgv = dgv;
                this.index = rowIndex;
                this.address = dgv[1, rowIndex].Value.ToString();
            }
            */
            public Worker(string address)
            {
                this.address = address;
            }
            public void DoWork()
            {
                PingHelper ph = new PingHelper();
                PingHelper.PingResult result = ph.MultiPing(address, 10);



                ht.Add(address, result);
                //dgv[2, index].Value = result.GetAvePing().ToString();
                //dgv[3, index].Value = result.GetTimeOut().ToString();
                //hs[address] = result;
                if (ht.Count == ListSize)
                {
                    isPingOver = true;

                }
            }
        }

    }


}
