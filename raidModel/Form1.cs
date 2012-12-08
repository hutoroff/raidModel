using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace raidModel
{
    public partial class Form1 : Form
    {
        int writeTime;
        int readTime;

        #region BusinesLogic
        private bool write(raid0 array)
        {       //true - in case of failure; false - in case of success
            List<sbyte> toWrite = new List<sbyte>(textBox1.Text.Length);
            foreach (char ch in textBox1.Text)
                toWrite.Add(Convert.ToSByte(ch));

            writeTime = array.writeToArray(toWrite);
            if (writeTime == -1)
            {
                writeTime = 0;
                return true;
            }
            textBox3.Text = Convert.ToString(writeTime);
            return false;
        }

        //private bool write(raid1 array)
        //{       //true - in case of failure; false - in case of success
        //    List<sbyte> toWrite = new List<sbyte>(textBox1.Text.Length);
        //    foreach (char ch in textBox1.Text)
        //        toWrite.Add(Convert.ToSByte(ch));

        //    writeTime = array.writeToArray(toWrite);
        //    if (writeTime == -1)
        //    {
        //        writeTime = 0;
        //        return true;
        //    }
        //    textBox3.Text = Convert.ToString(writeTime);
        //    return false;
        //}

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox4.Text = "1024";
            numericUpDown1.Minimum = 0;
            numericUpDown1.Value = 2;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                textBox4.ReadOnly = true;
                generateStringBut1.Enabled = false;
                textBox4.Text = Convert.ToString(textBox1.Text.Length);
            }
            else
            {
                textBox4.ReadOnly = false;
                generateStringBut1.Enabled = true;
                textBox4.Text = "1024";
            }

        }

        private void generateStringBut1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            int length = Convert.ToInt32(textBox4.Text);
            Random rand = new Random();
            String toPut;
            toPut = "";
            progressBar1.Maximum = length;
            progressBar1.Value = 0;
            for(int i=0;i<length;i++)
            {
                toPut += Convert.ToChar(rand.Next(1, 128));
                progressBar1.Value++;
            }
            textBox1.Text += toPut;
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            disk hdd;
            if(radioButton7.Checked)
            {
                hdd = new disk(78812649881.6, 16777216, 3.6f, 4);
            }
            else
            {
                if(radioButton6.Checked)
                    hdd = new disk(536870912000,16777216,8.5f,9.5f);
                else
                    hdd = new disk(193273528320,0,1.5f,2f);
            }

            if(radioButton0.Checked)
            {
                raid0 array = new raid0();
                for (int i = 0; i < numericUpDown1.Value; i++)
                    array.addDisk(hdd);
                if (write(array))
                    MessageBox.Show("Ошибка записи в массив!");
            }
            else
            {
                //raid1 array = new raid1();
                //for (int i = 0; i < numericUpDown1.Value; i++)
                //    array.addDisk(hdd);
                //if (write(array))
                //    MessageBox.Show("Ошибка записи в массив!");
            }
        }

        private void radioButton0_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton0.Checked)
                numericUpDown1.Increment = 1;
            else
                numericUpDown1.Increment = 2;
        }
    }
}
