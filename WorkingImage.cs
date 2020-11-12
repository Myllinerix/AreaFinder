using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Area_Finder_Too
{
    class WorkingImage
    {
        private Bitmap baseBitmap, outputBitmap;
        public Bitmap Bitmap
        {
            get
            {
                return this.outputBitmap;
            }
        }

        List<Selection> selections = new List<Selection>();

        //private List

        private Point imageCenter = new Point(-1, -1);
        private double pixelRatio = 11.24;
        private enum WorkingStates { Idle, WorkingOnSelection };
        private WorkingStates workingState = WorkingStates.Idle;

        //Graphics received_canvas = null;
        //public int Width, Height;

        public WorkingImage(string _fileName)
        {
            this.baseBitmap = new Bitmap(_fileName);
            this.outputBitmap = (Bitmap)this.baseBitmap.Clone();
            
            /*Rectangle transparentRectangle = new Rectangle(0, 0, outputBitmap.Width, outputBitmap.Height);
            Brush transparentBrush = new SolidBrush(Color.FromArgb(0, 0, 0, 0));
            using (Graphics g = Graphics.FromImage(foregroundOutput))
                g.FillRectangle(transparentBrush, transparentRectangle);
            foregroundOutput.MakeTransparent();*/
        }

        /*public void RedrawForeground()
        {

        }*/

        public void SetPixelRatio(double received_Ratio)
        {
            this.pixelRatio = received_Ratio;
        }

        public string SelectionInfo()
        {
            return "Something definetely happened";
        }

        private void UpdateOutputBitmap()
        {

        }

        public void SetRectanglePoint()
        {

        }

        public void SetArbitraryPoint()
        {

        }

        public void AddSeaColor(Point mouseLocation)
        {
            Color seaColor = this.baseBitmap.GetPixel(mouseLocation.X, mouseLocation.Y);
            for (int x = 0; x < this.baseBitmap.Width; x++)
                for (int y = 0; y < this.baseBitmap.Height; y++)
                    if ((this.baseBitmap.GetPixel(x, y).R >= seaColor.R - 15 && this.baseBitmap.GetPixel(x, y).R <= seaColor.R + 15) &&
                        (this.baseBitmap.GetPixel(x, y).G >= seaColor.G - 15 && this.baseBitmap.GetPixel(x, y).G <= seaColor.G + 15) &&
                        (this.baseBitmap.GetPixel(x, y).B >= seaColor.B - 15 && this.baseBitmap.GetPixel(x, y).B <= seaColor.B + 15))
                        this.baseBitmap.SetPixel(x, y, Color.FromArgb(255, 16, 89));
        }

        public void SetCenterPoint(Point mouseLocation)
        {
            /*List<Bitmap> bitmaps = new List<Bitmap>();
            bitmaps.Add(mainBitmap);
            bitmaps.Add(arbitraryBitmap);
            bitmaps.Add(outputBitmap);*/
            //foreach (Bitmap _bitmap in bitmaps)

            /*if (this.imageCenter != new Point(-1, -1))
            {

            }*/
            
            Pen bluePen = new Pen(Color.DarkBlue, 3), whitePen = new Pen(Color.White, 5);
            //r.DrawImage(this.outputBitmap, 0, 0, this.outputBitmap.Width, this.outputBitmap.Height);
            Graphics outputGraphics = Graphics.FromImage(this.outputBitmap);
            outputGraphics.DrawLine(whitePen, mouseLocation.X, mouseLocation.Y - 9, mouseLocation.X, mouseLocation.Y + 10);
            outputGraphics.DrawLine(whitePen, mouseLocation.X - 9, mouseLocation.Y, mouseLocation.X + 10, mouseLocation.Y);
            outputGraphics.DrawLine(bluePen, mouseLocation.X, mouseLocation.Y - 9, mouseLocation.X, mouseLocation.Y + 10);
            outputGraphics.DrawLine(bluePen, mouseLocation.X - 9, mouseLocation.Y, mouseLocation.X + 10, mouseLocation.Y);
            this.imageCenter = mouseLocation;
        }

        public void MouseClick(Point mouseLocation, MouseButtons mouseButton)
        {   
            switch (mouseButton)
            {
                case MouseButtons.Middle:
                    AddSeaColor(mouseLocation);
                    break;
                case MouseButtons.Left:
                    switch (this.workingState)  //Find out what the control wants to do with the selection (presumably rectangular)
                    {
                        case WorkingStates.Idle:
                            this.selections.Add(new RectangularSelection(mouseLocation));   //Create a rectangular selection
                            this.workingState = WorkingStates.WorkingOnSelection;
                            break;
                        case WorkingStates.WorkingOnSelection:  
                            if (this.selections.Last().selectionKind == Selection.SelectionKinds.Rectangular)    //Finish the rectangular selection
                            {
                                this.selections.Last().AddAPoint(mouseLocation);
                                this.workingState = WorkingStates.Idle;
                            }
                            else    //Delete point or completely delete the arbitrary selection
                            {
                                this.selections.Last().DeleteAPoint();
                            }
                            break;
                    }
                    break;
                case MouseButtons.Right:
                    switch (this.workingState)  //Find out what control wants to do with the selection (presumably arbitrary)
                    {
                        case WorkingStates.Idle:
                            this.selections.Add(new ArbitrarySelection(mouseLocation));  //Create an arbitrary selection
                            break;
                        case WorkingStates.WorkingOnSelection:
                            if (this.selections.Last().selectionKind == Selection.SelectionKinds.Arbitrary)    //Add point or finish the arbitrary selection
                            {
                                this.selections.Last().AddAPoint(mouseLocation);
                            }
                            else    //Delete the rectangular selection
                            {
                                this.selections.Last().DeleteAPoint();
                            }
                            break;
                    }
                    break;
            }
            
            //Last selection in Selections is selection in work

            //Make to them corresponding changes

            //UpdateOutputBitmap

            /*landArea = 0;
            listOfLand.Clear();
            switch (currentAct)
            {
                case CurrentAction.FirstPointRect:
                    constPoint.X = _mouseXY.X; constPoint.Y = _mouseXY.Y;
                    for (int x = constPoint.X - 1; x <= constPoint.X + 1; x++)
                        for (int y = constPoint.Y - 1; y <= constPoint.Y + 1; y++)
                            base.outputBitmap.SetPixel(x, y, Color.Cyan);
                    currentAct = CurrentAction.SecondPointRect;
                    break;

                case CurrentAction.SecondPointRect:
                    currentAct = CurrentAction.StayingStill;
                    for (int x = topLeftPoint.X; x < topLeftPoint.X + Math.Abs(constPoint.X - _mouseXY.X); x++)
                    {
                        for (int y = topLeftPoint.Y; y < topLeftPoint.Y + Math.Abs(constPoint.Y - _mouseXY.Y); y++)
                        {
                            if (base.mainBitmap.GetPixel(x, y) != Color.FromArgb(255, 16, 89))
                            {
                                base.outputBitmap.SetPixel(x, y, Color.LightGreen);
                                listOfLand.Add(new Point(x, y));
                                landArea++;
                            }
                        }
                    }
                    CalculateCost(_mouseXY);
                    break;

                default:
                    break;
            }*/
        }

        public void MouseMove(Point mouseLocation)
        {

        }
    }

}