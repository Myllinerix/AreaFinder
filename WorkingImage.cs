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

        private float triangulationResConst
        {
            get
            {
                return TriangulationResConst;
            }
            set
            {
                if (value > 0.05f && value <= 0.33f)
                    TriangulationResConst = value;
            }
        }
        public float TriangulationResConst;

        private List<Selection> selections = new List<Selection>();
        private Point imageCenter = new Point(-1, -1);
        private double pixelRatio = 11.24;
        private int currentSelectionIndex = 0;
        private bool showAllSelections = false;

        public WorkingImage(string _fileName)
        {
            this.baseBitmap = new Bitmap(_fileName);
            this.seaPixelsBitmap = (Bitmap)this.baseBitmap.Clone();
            this.outputBitmap = (Bitmap)this.seaPixelsBitmap.Clone();
            this.TriangulationResConst = 0.099f;
        }

        public void SetPixelRatio(double received_Ratio)
        {
            this.pixelRatio = received_Ratio;
            this.RecalculateSelections();
        }

        public string SelectionInfo()
        {
            if (this.selections.Count > 0)
                return this.selections[this.currentSelectionIndex].SelectionInfo;
            else
                return "Area Finder";
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

        public void TriangulateSelection()
        {
            if(this.selections[this.currentSelectionIndex].IsFinished)
            {
                this.selections[this.currentSelectionIndex].Triangulate(this.TriangulationResConst);
            }
        }

        public void SwitchShowingAllSelectionsAtOnce()
        {
            if (!this.showAllSelections)
                this.showAllSelections = true;
            else
                this.showAllSelections = false;
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

            //Output or try to output each and every selection:
            foreach (Selection selection in this.selections) 
            {
                Point previousArbitraryPoint = new Point(-1, -1);
                switch (selection.Kind)
                {
                    case Selection.SelectionKinds.Rectangular: //Output of a rectangular selection:
                        //Of an unfinished selection:
                        if (selection.IsFinished == false)
                        {
                            //Draw an outline:
                            outputGraphics.DrawRectangle(new Pen(Color.Aquamarine, 2), selection.RectangularOutline);
                        }

                        //Of a current finished selection or ALL finished selection, if this.showAllSelections == true:
                        if (selection.IsFinished == true && (selection == this.selections[this.currentSelectionIndex] || this.showAllSelections))
                        {
                            //Fill all land pixels with color:
                            foreach (Point landPoint in selection.ListOfLand)
                                outputBitmap.SetPixel(landPoint.X, landPoint.Y, Color.LightGreen);
                            
                            //Draw a line from the the center of land points to the center of an image:
                            if (this.imageCenter != new Point(-1, -1))
                                this.DrawALineToTheCenterOfSelection(outputGraphics, selection);

                            //Draw an outline:
                            if(selection == this.selections[this.currentSelectionIndex] && this.showAllSelections)
                                outputGraphics.DrawRectangle(new Pen(Color.Crimson, 2), selection.RectangularOutline);
                            else
                                outputGraphics.DrawRectangle(new Pen(Color.DarkBlue, 2), selection.RectangularOutline);
                        }
                        break;

                    case Selection.SelectionKinds.Arbitrary: //Output of an arbitrary selection:
                        //Of an unfinished selection:
                        if (selection.IsFinished == false) 
                        {
                            //Draw a line to the next arbitrary point:
                            outputGraphics.DrawLine(new Pen(Color.Aquamarine, 2), selection.ListOfPoints.Last(), selection.NextArbitraryPoint);
                            
                            //Draw an outline:
                            foreach (Point arbitraryPoint in selection.ListOfPoints)
                            {
                                if (previousArbitraryPoint != new Point(-1, -1))
                                    outputGraphics.DrawLine(new Pen(Color.Aquamarine, 2), previousArbitraryPoint, arbitraryPoint);
                                previousArbitraryPoint = arbitraryPoint;
                            }
                        }

                        //Of a current finished selection or ALL finished selection, if this.showAllSelections == true:
                        if (selection.IsFinished == true && (selection == this.selections[this.currentSelectionIndex] || this.showAllSelections))
                        {
                            //Fill all land pixels with color:
                            foreach (Point landPoint in selection.ListOfLand)
                                outputBitmap.SetPixel(landPoint.X, landPoint.Y, Color.LightGreen);

                            //Draw a line from the the center of land points to the center of an image:
                            if (this.imageCenter != new Point(-1, -1))
                                this.DrawALineToTheCenterOfSelection(outputGraphics, selection);

                            //Draw an outline:
                            if (selection == this.selections[this.currentSelectionIndex] && this.showAllSelections)
                                foreach (Point outlinePoint in selection.OutlinePoints)
                                    outputBitmap.SetPixel(outlinePoint.X, outlinePoint.Y, Color.Crimson);
                            else
                                foreach (Point outlinePoint in selection.OutlinePoints)
                                    outputBitmap.SetPixel(outlinePoint.X, outlinePoint.Y, Color.Indigo);
                        }
                        break;
                }
            }

            //Draw a cross in the center of the image, if such specified:
            if (this.imageCenter != new Point(-1, -1))
            {
                Pen outerPen = new Pen(Color.White, 5), innerPen = new Pen(Color.DarkBlue, 3);
                if (this.showAllSelections)
                    innerPen.Color = Color.Crimson;
                outputGraphics.DrawLine(outerPen, imageCenter.X, imageCenter.Y - 9, imageCenter.X, imageCenter.Y + 10);
                outputGraphics.DrawLine(outerPen, imageCenter.X - 9, imageCenter.Y, imageCenter.X + 10, imageCenter.Y);
                outputGraphics.DrawLine(innerPen, imageCenter.X, imageCenter.Y - 9, imageCenter.X, imageCenter.Y + 10);
                outputGraphics.DrawLine(innerPen, imageCenter.X - 9, imageCenter.Y, imageCenter.X + 10, imageCenter.Y);
            }
        }

        public void ChangeCurrentSelection(Point mouseLocation, MouseButtons mouseButton)
        {
            Selection selection = this.selections.Last();
            
            //Find out what the control wants to do with the selection:
            switch (mouseButton) 
            {
                case MouseButtons.Left:
                    if (selection.Kind == Selection.SelectionKinds.Rectangular)    //Finish the rectangular selection
                    {
                        selection.AddAPoint(mouseLocation);
                        this.CalculateSelectionArea(selection);
                        this.CalculateSelectionCost(selection);
                        this.workingState = WorkingStates.Idle;
                    }
                    else    //Delete a point or completely delete the arbitrary selection
                    {
                        if (this.selections.Last().DeleteAPoint())
                        {
                            this.DeleteASelection();
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
                            //MessageBox.Show(this.selections.Last().PointsChecked.ToString() + " " + this.selections.Last().OutlinePoints.Count.ToString());
                        }
                    }
                    else    //Delete the rectangular selection
                    {
                        if (this.selections.Last().DeleteAPoint())
                        {
                            this.DeleteASelection();
                        }
                    }
                    break;
            }
        }

        private void DrawALineToTheCenterOfSelection(Graphics outputGraphics, Selection selection)
        {
            Pen outerPen = new Pen(Color.White, 4), innerPen = new Pen(Color.DarkBlue, 2);
            if (selection == this.selections[this.currentSelectionIndex] && this.showAllSelections)
                innerPen.Color = Color.Crimson;
            if (selection == this.selections[this.currentSelectionIndex])
            {
                outputGraphics.DrawLine(outerPen, this.imageCenter, selection.LandCenter);
                outputGraphics.DrawLine(innerPen, this.imageCenter, selection.LandCenter);
                outerPen.Width = 5;
                innerPen.Width = 3;
                outputGraphics.DrawLine(outerPen, selection.LandCenter.X, selection.LandCenter.Y - 9, selection.LandCenter.X, selection.LandCenter.Y + 10);
                outputGraphics.DrawLine(outerPen, selection.LandCenter.X - 9, selection.LandCenter.Y, selection.LandCenter.X + 10, selection.LandCenter.Y);
                outputGraphics.DrawLine(innerPen, selection.LandCenter.X, selection.LandCenter.Y - 9, selection.LandCenter.X, selection.LandCenter.Y + 10);
                outputGraphics.DrawLine(innerPen, selection.LandCenter.X - 9, selection.LandCenter.Y, selection.LandCenter.X + 10, selection.LandCenter.Y);
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

        public void DeleteASelection()
        {
            if(this.selections.Count > 0)
                this.selections.RemoveAt(this.currentSelectionIndex);
            this.workingState = WorkingStates.Idle;
            if (this.currentSelectionIndex == this.selections.Count && this.selections.Count > 0)
                this.currentSelectionIndex--;
            else if (this.selections.Count == 0)
                this.currentSelectionIndex = 0;
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
            switch (selection.Kind)
            {
                case Selection.SelectionKinds.Rectangular:
                    int totalArea = 0;
                    for (int x = selection.TopLeftPoint.X; x < selection.BottomRightPoint.X; x++)
                    {
                        for (int y = selection.TopLeftPoint.Y; y < selection.BottomRightPoint.Y; y++)
                        {
                            totalArea++;
                            if (this.seaPixelsBitmap.GetPixel(x, y) != Color.FromArgb(255, 16, 89))
                            {
                                selection.ListOfLand.Add(new Point(x, y));
                            }
                        }
                    }

                    selection.Area = (int)Math.Round(selection.ListOfLand.Count * Math.Pow(this.pixelRatio, 2));
                    selection.LandArea = (int)Math.Round(totalArea * Math.Pow(this.pixelRatio, 2));
                    break;

                case Selection.SelectionKinds.Arbitrary:
                    foreach (Point innerPoint in selection.InnerPoints)
                        if (this.seaPixelsBitmap.GetPixel(innerPoint.X, innerPoint.Y) != Color.FromArgb(255, 16, 89))
                            selection.ListOfLand.Add(innerPoint);

                    selection.Area = (int)Math.Round(selection.InnerPoints.Count * Math.Pow(this.pixelRatio, 2));
                    selection.LandArea = (int)Math.Round(selection.ListOfLand.Count * Math.Pow(this.pixelRatio, 2));
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
                selection.LandCenter.X = pixelsSum.X / selection.ListOfLand.Count;
                selection.LandCenter.Y = pixelsSum.Y / selection.ListOfLand.Count;

                double distanceFromCenter = Math.Sqrt(Math.Pow(Math.Abs(selection.LandCenter.X - this.imageCenter.X), 2)
                    + Math.Pow(Math.Abs(selection.LandCenter.Y - this.imageCenter.Y), 2)) * this.pixelRatio;
                selection.Cost = (int)(Math.Round((selection.ListOfLand.Count * Math.Pow(this.pixelRatio, 2)) / (0.685 * distanceFromCenter)));
            }
            
            selection.SelectionInfo = "Area Found! Total Area = " + selection.Area + "; Land Area = " + selection.LandArea + "; Cost = " + selection.Cost + ";";
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

        public void PreviousSelection()
        {
            if (this.currentSelectionIndex > 0)
                currentSelectionIndex--;
        }

        public void NextSelection()
        {
            if (this.currentSelectionIndex + 1 < this.selections.Count)
                currentSelectionIndex++;
        }
    }

}