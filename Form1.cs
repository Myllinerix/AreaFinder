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
            switch (e.KeyData.ToString())
            {
                case "C":
                    workingImage.SetCenterPoint(mouseLocation);
                    break;

                case "Z":
                    workingImage.RevertSeaColors();
                    break;

                case "E":
                    Random random = new Random();
                    Color randomColor = new Color();
                    randomColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                    Graphics.FromImage(workingImage.Bitmap).FillRectangle(new SolidBrush(randomColor), mouseLocation.X, mouseLocation.Y, 2000, 2000);
                    break;

                case "R":
                    if (!numericUpDown1.Enabled)
                        ActivateNumeric();
                    break;
            }
            
            workingImage.OutputBitmap();
            pictureBox1.Image = workingImage.Bitmap;
        }

        private void numericUpDown1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData.ToString() == "R" || e.KeyData.ToString() == "Return")
                DeactivateNumeric();
        }

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
            workingImage.ReadyToNextUpdate = true;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (workingImage.ReadyToNextUpdate == true)
            {   
                mouseLocation.X = e.X;
                mouseLocation.Y = e.Y;
                workingImage.RedrawCurrentSelection(mouseLocation);
                workingImage.OutputBitmap();
                pictureBox1.Invalidate();
            }
            //this.Text = workingImage.SelectionInfo();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Middle:
                    workingImage.AddSeaColor(mouseLocation);
                    break;
                case MouseButtons.Left:
                    switch (workingImage.WorkingState)
                    {
                        case WorkingImage.WorkingStates.Idle:
                            workingImage.AddASelection(mouseLocation, Selection.SelectionKinds.Rectangular);  //Create a rectangular selection
                            break;
                        case WorkingImage.WorkingStates.WorkingOnSelection:
                            workingImage.ChangeCurrentSelection(mouseLocation, MouseButtons.Left); //Change existing selection
                            break;
                    }
                    break;
                case MouseButtons.Right:
                    switch (workingImage.WorkingState)
                    {
                        case WorkingImage.WorkingStates.Idle:
                            workingImage.AddASelection(mouseLocation, Selection.SelectionKinds.Arbitrary);  //Create an arbitrary selection
                            break;
                        case WorkingImage.WorkingStates.WorkingOnSelection:
                            workingImage.ChangeCurrentSelection(mouseLocation, MouseButtons.Right); //Change existing selection
                            break;
                    }
                    break;
            }
            workingImage.OutputBitmap();
            pictureBox1.Invalidate();
        }
    }
}