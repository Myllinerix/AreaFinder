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
        public enum WorkingStates { Idle, WorkingOnSelection };
        private WorkingStates workingState = WorkingStates.Idle;
        public WorkingStates WorkingState { get { return workingState; } }

        private Bitmap baseBitmap, seaPixelsBitmap, outputBitmap;
        public Bitmap Bitmap { get { return this.outputBitmap; } }

        private bool readyToNextUpdate = true;
        public bool ReadyToNextUpdate { get => readyToNextUpdate; set => readyToNextUpdate = value; }

        private List<Selection> selections = new List<Selection>();
        private Point imageCenter = new Point(-1, -1);
        private double pixelRatio = 11.24;
        private int currentSelectionIndex = 0;

        public WorkingImage(string _fileName)
        {
            this.baseBitmap = new Bitmap(_fileName);
            this.seaPixelsBitmap = (Bitmap)this.baseBitmap.Clone();
            this.outputBitmap = (Bitmap)this.seaPixelsBitmap.Clone();
        }

        public void SetPixelRatio(double received_Ratio)
        {
            this.pixelRatio = received_Ratio;
        }

        public string SelectionInfo()
        {
            return "Something definetely happened";
        }

        public void AddSeaColor(Point mouseLocation)
        {
            Color seaColor = this.seaPixelsBitmap.GetPixel(mouseLocation.X, mouseLocation.Y);
            for (int x = 0; x < this.seaPixelsBitmap.Width; x++)
                for (int y = 0; y < this.seaPixelsBitmap.Height; y++)
                    if ((this.seaPixelsBitmap.GetPixel(x, y).R >= seaColor.R - 15 && this.seaPixelsBitmap.GetPixel(x, y).R <= seaColor.R + 15) &&
                        (this.seaPixelsBitmap.GetPixel(x, y).G >= seaColor.G - 15 && this.seaPixelsBitmap.GetPixel(x, y).G <= seaColor.G + 15) &&
                        (this.seaPixelsBitmap.GetPixel(x, y).B >= seaColor.B - 15 && this.seaPixelsBitmap.GetPixel(x, y).B <= seaColor.B + 15))
                        this.seaPixelsBitmap.SetPixel(x, y, Color.FromArgb(255, 16, 89));
            this.RecalculateSelections();
        }

        public void SetCenterPoint(Point mouseLocation)
        {
            this.imageCenter = mouseLocation;
            this.RecalculateSelections();
        }

        public void OutputBitmap()
        {
            this.outputBitmap = (Bitmap)this.seaPixelsBitmap.Clone();
            Graphics outputGraphics = Graphics.FromImage(this.outputBitmap);

            foreach (Selection selection in this.selections)
            {
                Point previousArbitraryPoint = new Point(-1, -1);
                switch (selection.Kind)
                {
                    case Selection.SelectionKinds.Rectangular:
                        if (selection.IsFinished == false || selection == this.selections[this.currentSelectionIndex])
                        {
                            outputGraphics.DrawRectangle(new Pen(Color.Aquamarine, 2), selection.RectangularOutline);
                        }

                        if (selection.IsFinished == true && selection == this.selections[this.currentSelectionIndex])
                        {
                            foreach (Point landPoint in selection.ListOfLand)
                                outputBitmap.SetPixel(landPoint.X, landPoint.Y, Color.LightGreen);
                            if (this.imageCenter != new Point(-1, -1))
                            {
                                Pen whitePen = new Pen(Color.White, 4), bluePen = new Pen(Color.DarkBlue, 2);
                                outputGraphics.DrawLine(whitePen, this.imageCenter, selection.landCenter);
                                outputGraphics.DrawLine(bluePen, this.imageCenter, selection.landCenter);
                                bluePen = new Pen(Color.DarkBlue, 3);
                                whitePen = new Pen(Color.White, 5);
                                outputGraphics.DrawLine(whitePen, selection.landCenter.X, selection.landCenter.Y - 9, selection.landCenter.X, selection.landCenter.Y + 10);
                                outputGraphics.DrawLine(whitePen, selection.landCenter.X - 9, selection.landCenter.Y, selection.landCenter.X + 10, selection.landCenter.Y);
                                outputGraphics.DrawLine(bluePen, selection.landCenter.X, selection.landCenter.Y - 9, selection.landCenter.X, selection.landCenter.Y + 10);
                                outputGraphics.DrawLine(bluePen, selection.landCenter.X - 9, selection.landCenter.Y, selection.landCenter.X + 10, selection.landCenter.Y);
                            }
                        }
                        break;

                    case Selection.SelectionKinds.Arbitrary:
                        if (selection.IsFinished == false || selection == this.selections[this.currentSelectionIndex])
                        {
                            outputGraphics.DrawLine(new Pen(Color.Aquamarine, 2), selection.ListOfPoints.Last(), selection.NextArbitraryPoint);
                            foreach (Point arbitraryPoint in selection.ListOfPoints)
                            {
                                if (previousArbitraryPoint != new Point(-1, -1))
                                {
                                    outputGraphics.DrawLine(new Pen(Color.Aquamarine, 2), previousArbitraryPoint, arbitraryPoint);
                                }
                                previousArbitraryPoint = arbitraryPoint;
                            }
                        }

                        if (selection.IsFinished == true && selection == this.selections[this.currentSelectionIndex])
                        {
                            if (this.imageCenter != new Point(-1, -1))
                            {
                                Pen whitePen = new Pen(Color.White, 4), bluePen = new Pen(Color.DarkBlue, 2);
                                outputGraphics.DrawLine(whitePen, this.imageCenter, selection.landCenter);
                                outputGraphics.DrawLine(bluePen, this.imageCenter, selection.landCenter);
                                bluePen = new Pen(Color.DarkBlue, 3);
                                whitePen = new Pen(Color.White, 5);
                                outputGraphics.DrawLine(whitePen, selection.landCenter.X, selection.landCenter.Y - 9, selection.landCenter.X, selection.landCenter.Y + 10);
                                outputGraphics.DrawLine(whitePen, selection.landCenter.X - 9, selection.landCenter.Y, selection.landCenter.X + 10, selection.landCenter.Y);
                                outputGraphics.DrawLine(bluePen, selection.landCenter.X, selection.landCenter.Y - 9, selection.landCenter.X, selection.landCenter.Y + 10);
                                outputGraphics.DrawLine(bluePen, selection.landCenter.X - 9, selection.landCenter.Y, selection.landCenter.X + 10, selection.landCenter.Y);
                            }
                        }
                        break;
                }
            }

            if (this.imageCenter != new Point(-1, -1))
            {
                Pen bluePen = new Pen(Color.DarkBlue, 3), whitePen = new Pen(Color.White, 5);

                outputGraphics.DrawLine(whitePen, imageCenter.X, imageCenter.Y - 9, imageCenter.X, imageCenter.Y + 10);
                outputGraphics.DrawLine(whitePen, imageCenter.X - 9, imageCenter.Y, imageCenter.X + 10, imageCenter.Y);
                outputGraphics.DrawLine(bluePen, imageCenter.X, imageCenter.Y - 9, imageCenter.X, imageCenter.Y + 10);
                outputGraphics.DrawLine(bluePen, imageCenter.X - 9, imageCenter.Y, imageCenter.X + 10, imageCenter.Y);
            }
        }

        public void ChangeCurrentSelection(Point mouseLocation, MouseButtons mouseButton)
        {
            Selection selection = this.selections.Last();
            switch (mouseButton) //Find out what the control wants to do with the selection
            {
                case MouseButtons.Left:
                    if (selection.Kind == Selection.SelectionKinds.Rectangular)    //Finish the rectangular selection
                    {
                        selection.AddAPoint(mouseLocation);
                        this.CalculateSelectionArea(selection);
                        this.CalculateSelectionCost(selection);
                        this.workingState = WorkingStates.Idle;
                    }
                    else    //Delete point or completely delete the arbitrary selection
                    {
                        if (this.selections.Last().DeleteAPoint())
                        {
                            this.selections.RemoveAt(this.selections.Count - 1);
                            this.workingState = WorkingStates.Idle;
                            if (this.selections.Count > 0)
                                this.currentSelectionIndex = this.selections.Count - 1;
                            else
                                this.currentSelectionIndex = 0;
                        }
                    }
                    break;

                case MouseButtons.Right:
                    if (this.selections.Last().Kind == Selection.SelectionKinds.Arbitrary)    //Add point or finish the arbitrary selection
                    {
                        this.selections.Last().AddAPoint(mouseLocation);
                        if (this.selections.Last().IsFinished == true)
                        {
                            this.CalculateSelectionArea(selection);
                            this.CalculateSelectionCost(selection);
                            this.workingState = WorkingStates.Idle;
                        }
                    }
                    else    //Delete the rectangular selection
                    {
                        if (this.selections.Last().DeleteAPoint())
                        {
                            this.selections.RemoveAt(this.selections.Count - 1);
                            this.workingState = WorkingStates.Idle;
                            if (this.selections.Count > 0)
                                this.currentSelectionIndex = this.selections.Count - 1;
                            else
                                this.currentSelectionIndex = 0;
                        }
                    }
                    break;
            }
        }

        public void AddASelection(Point mouseLocation, Selection.SelectionKinds selectionKind)
        {
            if (selectionKind == Selection.SelectionKinds.Rectangular)
                this.selections.Add(new RectangularSelection(mouseLocation)); //Create a rectangular selection
            else
                this.selections.Add(new ArbitrarySelection(mouseLocation));  //Create an arbitrary selection
            this.workingState = WorkingStates.WorkingOnSelection;
            this.currentSelectionIndex = this.selections.Count - 1;
        }

        public void RedrawCurrentSelection(Point mouseLocation)
        {
            this.ReadyToNextUpdate = false;
            if (this.workingState == WorkingStates.WorkingOnSelection)
                this.selections.Last().UpdateNextPointLocation(mouseLocation);
        }

        private void CalculateSelectionArea(Selection selection)
        {
            selection.ListOfLand.Clear();
            selection.Area = 0;
            switch (selection.Kind)
            {
                case Selection.SelectionKinds.Rectangular:
                    for (int x = selection.TopLeftPoint.X; x < selection.BottomRightPoint.X; x++)
                        for (int y = selection.TopLeftPoint.Y; y < selection.BottomRightPoint.Y; y++)
                            if (this.seaPixelsBitmap.GetPixel(x, y) != Color.FromArgb(255, 16, 89))
                                selection.ListOfLand.Add(new Point(x, y));
                    break;

                case Selection.SelectionKinds.Arbitrary:

                    foreach (Point innerPoint in selection.InnerPoints)
                        if (this.seaPixelsBitmap.GetPixel(innerPoint.X, innerPoint.Y) != Color.FromArgb(255, 16, 89))
                            selection.ListOfLand.Add(innerPoint);
                    break;
            }
        }

        private void CalculateSelectionCost(Selection selection)
        {
            if (this.imageCenter.X != -1 && selection.ListOfLand.Count > 0)
            {
                Point pixelsSum = new Point(0, 0);
                foreach (Point pixel in selection.ListOfLand)
                {
                    pixelsSum.X += pixel.X;
                    pixelsSum.Y += pixel.Y;
                }
                selection.landCenter.X = pixelsSum.X / selection.ListOfLand.Count;
                selection.landCenter.Y = pixelsSum.Y / selection.ListOfLand.Count;

                double distanceFromCenter = Math.Sqrt(Math.Pow(Math.Abs(selection.landCenter.X - this.imageCenter.X), 2)
                    + Math.Pow(Math.Abs(selection.landCenter.Y - this.imageCenter.Y), 2)) * this.pixelRatio;
                selection.Cost = (int)(Math.Floor((selection.ListOfLand.Count * Math.Pow(this.pixelRatio, 2)) / (0.685 * distanceFromCenter)));
            }
            selection.LandArea = (int)Math.Floor(selection.ListOfLand.Count * Math.Pow(this.pixelRatio, 2));
            //selectionInfo = "Area Found! Action: " + currentAct + "; Total Area = " + area + "; Land Area = " + landArea + "; Cost = " + cost + ";";
        }

        private void RecalculateSelections()
        {
            foreach (Selection selection in this.selections)
            {
                this.CalculateSelectionArea(selection);
                this.CalculateSelectionCost(selection);
            }
        }

        public void RevertSeaColors()
        {
            this.seaPixelsBitmap = (Bitmap)this.baseBitmap.Clone();
            this.RecalculateSelections();
        }

        private void CheckIfPointIsInner(Point recieved_point, List<Point> outlinePoints, Point topLeft, Point bottomRight)
        {

        }
    }

}