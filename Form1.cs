using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Area_Finder_Too
{
    public partial class Form1 : Form
    {
        readonly OpenFileDialog open = new OpenFileDialog();
        readonly Selection select = new Selection();
        Point mouseXY = new Point();
     
        public Form1()
        {
            if (open.ShowDialog() == DialogResult.OK)
            {
                select.OpenImage(open.FileName);
                select.RecreateOutputBitmapFromMain();
                select.RecreateArbBitmap();
                InitializeComponent();
                pictureBox1.Width = select.outputBitmap.Width;
                pictureBox1.Height = select.outputBitmap.Height;
                this.Size = new Size(select.outputBitmap.Width + 14, select.outputBitmap.Height + 39);
                pictureBox1.Image = select.outputBitmap;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Activate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData.ToString() == "C")
            {
                select.SetCenter(mouseXY);
                select.CalculateCost();
                pictureBox1.Image = select.outputBitmap;
                this.Text = select.selectionInfo;
            }

            if (e.KeyData.ToString() == "R" && !numericUpDown1.Enabled)
            {
                for (int i = -36; i <= 0; i++)
                {
                    panel1.Location = new Point(0, i);
                    Thread.Sleep(5);
                }
                numericUpDown1.Enabled = true;
                numericUpDown1.Focus();
            }
        }

        private void numericUpDown1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData.ToString() == "R" || e.KeyData.ToString() == "Return")
            {
                numericUpDown1.Enabled = false;
                for (int i = 0; i >= -36; i--)
                {
                    panel1.Location = new Point(0, i);
                    Thread.Sleep(5);
                }
                this.Focus();
                select.pixelRatio = (double)numericUpDown1.Value;
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (select.currentAct == Selection.CurrentAction.NextPointArb)
                    RecreateEverything();
                else
                {
                    if (select.currentAct != Selection.CurrentAction.SecondPointRect)
                        select.currentAct = Selection.CurrentAction.FirstPointRect;
                    select.SetPointRect(mouseXY);
                    pictureBox1.Image = select.outputBitmap;
                    this.Text = select.selectionInfo;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (select.currentAct == Selection.CurrentAction.SecondPointRect)
                    RecreateEverything();
                else
                {
                    if (select.currentAct != Selection.CurrentAction.NextPointArb)
                        select.currentAct = Selection.CurrentAction.FirstPointArb;
                    select.SetPointArb(mouseXY);
                    pictureBox1.Image = select.outputBitmap;
                    this.Text = select.selectionInfo;
                }   
            }
            else if (e.Button == MouseButtons.Middle)
            {
                select.AddSeaColor(mouseXY);
                RecreateEverything();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
            mouseXY.X = e.X;
            mouseXY.Y = e.Y;
            select.UpdateOutputBitmap(mouseXY);
            pictureBox1.Image = select.outputBitmap;
            this.Text = select.selectionInfo;                
        }

        private void RecreateEverything()
        {
            select.currentAct = Selection.CurrentAction.StayingStill;
            select.RecreateOutputBitmapFromMain();
            select.RecreateArbBitmap();
            pictureBox1.Image = select.outputBitmap;
            select.selectionInfo = "Area Finder";
            this.Text = select.selectionInfo;
        }
    }
}