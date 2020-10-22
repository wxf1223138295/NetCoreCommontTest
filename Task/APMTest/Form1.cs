using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace APMTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public delegate string Mydele(string para);

        public Mydele mydele;
        private void Form1_Load(object sender, EventArgs e)
        {
            mydele = new Mydele(GetText);

            var s1s = Thread.CurrentThread.ManagedThreadId;
            mydele.BeginInvoke("2", Callbacs, null);


            Thread.Sleep(1000);
            this.textBox1.Text = "call back之前";

            var s3s = Thread.CurrentThread.ManagedThreadId;

        }

        public void Callbacs(IAsyncResult result)
        {
            var resd = mydele.EndInvoke(result);
            var ss = Thread.CurrentThread.ManagedThreadId;
            Action<string> act = (str) => { this.textBox1.Text = str; };
            this.textBox1.Invoke(act, resd);
        }
        public string GetText(string name)
        {
           
            Action<string> act = (str) => { this.textBox2.Text = str; };
            this.textBox2.Invoke(act, name);
            Thread.Sleep(2000);
            return name;
        }


    }
}
