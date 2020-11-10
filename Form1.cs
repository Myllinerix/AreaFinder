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
        OpenFileDialog open = new OpenFileDialog();
        WorkingImage workingImage;
        Point mouseLocation = new Point();
     
        public Form1()
        {
            InitializeComponent();

            if (open.ShowDialog() == DialogResult.OK)
            {   
                this.Size = new Size(new Bitmap(open.FileName).Width + 14, new Bitmap(open.FileName).Height + 39);
                pictureBox1.Dock = DockStyle.Fill;
                workingImage = new WorkingImage(open.FileName);
                //selection.RecreateArbBitmap();
                //pictureBox1.Image = workingImage.baseBitmap;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Activate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData.ToString() == "C")
            {
                workingImage.SetCenterPoint(mouseLocation);
                
                /*select.SetCenter(mouseXY);
                select.CalculateCost();
                pictureBox1.Image = select.outputBitmap;
                this.Text = select.selectionInfo;*/
            }

            if (e.KeyData.ToString() == "E")
                Graphics.FromImage(workingImage.Bitmap).FillRectangle(new SolidBrush(Color.Blue), mouseLocation.X, mouseLocation.Y, 200, 200);

            if (e.KeyData.ToString() == "R" && !numericUpDown1.Enabled)
                ActivateNumeric();
        }

        private void numericUpDown1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData.ToString() == "R" || e.KeyData.ToString() == "Return")
                DeactivateNumeric();
        }

        private void pictureBox111_MouseClick(object sender, MouseEventArgs e)
        {
            //Left

            /*if (select.currentAct == Selection.CurrentAction.NextPointArb)
                RecreateEverything();
            else
            {
                if (select.currentAct != Selection.CurrentAction.SecondPointRect)
                    select.currentAct = Selection.CurrentAction.FirstPointRect;
                select.SetPointRect(mouseXY);
                pictureBox1.Image = select.outputBitmap;
                this.Text = select.selectionInfo;
            }*/

            //Right

            /*if (select.currentAct == Selection.CurrentAction.SecondPointRect)
                RecreateEverything();
            else
            {
                if (select.currentAct != Selection.CurrentAction.NextPointArb)
                    select.currentAct = Selection.CurrentAction.FirstPointArb;
                select.SetPointArb(mouseXY);
                pictureBox1.Image = select.outputBitmap;
                this.Text = select.selectionInfo;
            }*/

            //Middle

            /*select.AddSeaColor(mouseXY);
            RecreateEverything();*/
        }

        /*private void RecreateEverything()
        {
            select.currentAct = Selection.CurrentAction.StayingStill;
            select.RecreateOutputBitmapFromMain();
            select.RecreateArbBitmap();
            pictureBox1.Image = select.outputBitmap;
            select.selectionInfo = "Area Finder";
            this.Text = select.selectionInfo;
        }*/

        private void ActivateNumeric()
        {
            for (int i = -36; i <= 0; i++)
            {
                panel1.Location = new Point(0, i);
                Thread.Sleep(5);
            }
            numericUpDown1.Enabled = true;
            numericUpDown1.Focus();
        }

        private void DeactivateNumeric()
        {
            numericUpDown1.Enabled = false;
            for (int i = 0; i >= -36; i--)
            {
                panel1.Location = new Point(0, i);
                Thread.Sleep(5);
            }
            this.Focus();
            workingImage.SetPixelRatio((double)numericUpDown1.Value);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            pictureBox1.Image = workingImage.Bitmap;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
            mouseLocation.X = e.X;
            mouseLocation.Y = e.Y;
            //workingImage.UpdateOutputBitmap(mouseLocation);

            //pictureBox1.Image = workingImage.outputBitmap;
            this.Text = workingImage.SelectionInfo();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    workingImage.SetRectanglePoint();
                    break;
                case MouseButtons.Right:
                    workingImage.SetArbitraryPoint();
                    break;
                case MouseButtons.Middle:
                    workingImage.AddSeaColor(mouseLocation);
                    MessageBox.Show("123");
                    break;
            }
        }
    }
}