﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;




namespace FunnyJoke
{
    public partial class Form1 : Form
    {
        String Msg;
        public Form1(String msg)
        {
            InitializeComponent();
      
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();           
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
         
        }        

    }
}
