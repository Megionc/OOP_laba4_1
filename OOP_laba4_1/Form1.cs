using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_laba4_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
             

     

        Graphics g;
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            g = CreateGraphics();

        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Point click;
            click = e.Location;
            Pen blackPen = new Pen(Color.Black, 3);
            g.DrawEllipse(blackPen, click.X, click.Y, 100, 100);
        }
    }



}
