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
        bool dialogResultOK = false;
        WorkingImage workingImage;
        Point mouseLocation = new Point();
     
        public Form1()
        {
            InitializeComponent();

            if (open.ShowDialog() == DialogResult.OK)
            {   
                this.Size = new Size(new Bitmap(open.FileName).Width + 14, new Bitmap(open.FileName).Height + 39);
                workingImage = new WorkingImage(open.FileName);
                dialogResultOK = true;
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
                    this.Text = workingImage.SelectionInfo();
                    break;

                case "Z":
                    workingImage.RevertSeaColors();
                    this.Text = workingImage.SelectionInfo();
                    break;

                case "R":
                    if (!numericUpDown1.Enabled)
                        ActivateNumeric();
                    break;

                case "A":
                    workingImage.SwitchShowingAllSelectionsAtOnce();
                    break;

                case "D":
                    workingImage.DeleteASelection();
                    break;

                case "Down":
                    workingImage.PreviousSelection();
                    this.Text = workingImage.SelectionInfo();
                    break;

                case "Up":
                    workingImage.NextSelection();
                    this.Text = workingImage.SelectionInfo();
                    break;

                case "T":
                    workingImage.TriangulateSelection();
                    break;

                case "S":
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
            panel1.BringToFront();
            for (int y = -36; y <= 0; y++)
            {
                panel1.Location = new Point(0, y);
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
            this.Text = workingImage.SelectionInfo();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (dialogResultOK)
            {
                pictureBox1.Image = workingImage.Bitmap;
                workingImage.ReadyToNextUpdate = true;
            }
            else
                this.Close();
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
            this.Text = workingImage.SelectionInfo();
        }
    }
}