﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace Calc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int sum = 0;

        public void Form1_Load(object sender, EventArgs e)
        {
            loadMemory();
        }

        public void Form1_Save(object sender, EventArgs e)
        {
            saveMemory();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button0_Click(object sender, EventArgs e)
        {
            this.textBox1.Text += "0";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.textBox1.Text += "1";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox1.Text += "2";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.textBox1.Text += "3";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.textBox1.Text += "4";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.textBox1.Text += "5";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.textBox1.Text += "6";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.textBox1.Text += "7";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.textBox1.Text += "8";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.textBox1.Text += "9";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonMinus_Click(object sender, EventArgs e)
        {
            try
            {
                int num = Int32.Parse(this.textBox1.Text.ToString());
                this.sum -= num;
                string line = this.sum.ToString();

                ListViewItem lvi = new ListViewItem("-");
                lvi.SubItems.Add(this.textBox1.Text);
                lvi.SubItems.Add(line);
                this.listView1.Items.Add(lvi);

            }
            catch (FormatException ev)
            {
                Debug.WriteLine("FormatException");
            }

            this.textBox1.Text = "";
            this.textBox1.Focus();

            this.saveMemory();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPlus_Click(object sender, EventArgs e)
        {
            try
            {
                int num = Int32.Parse(this.textBox1.Text.ToString());
                this.sum += num;
                string line = this.sum.ToString();

                ListViewItem lvi = new ListViewItem("+");
                lvi.SubItems.Add(this.textBox1.Text);
                lvi.SubItems.Add(line);
                this.listView1.Items.Add(lvi);
            }
            catch (FormatException ev)
            {
                Debug.WriteLine("FormatException");
            }

            this.textBox1.Text = "";
            this.textBox1.Focus();

            this.saveMemory();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEqual_Click(object sender, EventArgs e)
        {
            this.buttonPlus_Click(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.sum = 0;
            this.textBox1.Text = "";

            this.clearListView();

            this.textBox1.Focus();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.clearMemory();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.clearMemory();
        }


        /// <summary>
        /// leave the header
        /// </summary>
        private void clearListView()
        {
            int count = this.listView1.Items.Count;
            while (count >= 1)
            {
                this.listView1.Items.RemoveAt(0);
                count--;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void clearMemory()
        {
            this.sum = 0;
            this.textBox1.Text = "";

            this.clearListView();

            this.textBox1.Focus();
        }


        //
        private char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
        private char delimiter = ',';

        /// <summary>
        /// 
        /// </summary>
        public void loadMemory()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalcStorageFormat));

            CalcStorageFormat memory;

            FileStream fs = new FileStream("calcmemory.xml", FileMode.Open, FileAccess.Read);
            XmlReader reader = XmlReader.Create(fs);
            try
            {
                memory = (CalcStorageFormat)serializer.Deserialize(reader);
            }
            finally
            {
                reader.Dispose();
                fs.Close();
            }

            //
            this.sum = memory.sum;

            //
            this.textBox1.Text = "";

            //
            this.clearListView();
            for (int i = 0; i < memory.history.Count;i++)
            {
                string line = memory.history[i];

                string[] els = line.Split(this.delimiter);

                try
                {
                    ListViewItem lvi = new ListViewItem(els[0]);
                    lvi.SubItems.Add(els[1]);
                    lvi.SubItems.Add(els[2]);
                    this.listView1.Items.Add(lvi);
                } catch(IndexOutOfRangeException ex)
                {

                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void saveMemory()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalcStorageFormat));
            CalcStorageFormat memory = new CalcStorageFormat();
            memory.sum = sum;

            memory.history = new List<string>();
            foreach (ListViewItem item in listView1.Items)
            {
                string line = item.SubItems[0].Text + "," + item.SubItems[1].Text + "," + item.SubItems[2].Text;
                memory.history.Add(line);
            }

            /*
            using (FileStream fs = new FileStream("memory", FileMode.Create, FileAccess.Write)
            {
                  serializer.Serialize(fs, memory);
            }
            */

            FileStream fs = new FileStream("calcmemory.xml", FileMode.Create, FileAccess.Write);
            try
            {
                serializer.Serialize(fs, memory);
            }
            finally
            {
                fs.Dispose();
            }

        }
    }

}

/// <summary>
/// 
/// </summary>
public class CalcStorageFormat
{
    public int sum;
    public List<string> history;
}
