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
        private Bitmap baseBitmap, seaPixelsBitmap, outputBitmap;
        public Bitmap Bitmap { get { return this.outputBitmap; } }
        public enum WorkingStates { Idle, WorkingOnSelection };
        private WorkingStates workingState = WorkingStates.Idle;
        public WorkingStates WorkingState { get { return workingState; } }

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
                switch (selection.Kind)
                {
                    case Selection.SelectionKinds.Rectangular:
                        if (selection.isFinished == false)
                        {
                            outputGraphics.DrawRectangle(new Pen(Color.Aquamarine, 2), selection.RectangularOutline);
                        }
                        else if (selection == this.selections[this.currentSelectionIndex])
                        {
                            foreach (Point landPoint in selection.ListOfLand)
                                outputBitmap.SetPixel(landPoint.X, landPoint.Y, Color.LightGreen);
                            outputGraphics.DrawRectangle(new Pen(Color.Aquamarine, 2), selection.RectangularOutline);
                            if (this.imageCenter != new Point(-1, -1))
                            {
                                Pen bluePen = new Pen(Color.DarkBlue, 2), whitePen = new Pen(Color.White, 4);
                                outputGraphics.DrawLine(whitePen, this.imageCenter, selection.LandCenter);
                                outputGraphics.DrawLine(bluePen, this.imageCenter, selection.LandCenter);
                                bluePen = new Pen(Color.DarkBlue, 3);
                                whitePen = new Pen(Color.White, 5);
                                outputGraphics.DrawLine(whitePen, selection.LandCenter.X, selection.LandCenter.Y - 9, selection.LandCenter.X, selection.LandCenter.Y + 10);
                                outputGraphics.DrawLine(whitePen, selection.LandCenter.X - 9, selection.LandCenter.Y, selection.LandCenter.X + 10, selection.LandCenter.Y);
                                outputGraphics.DrawLine(bluePen, selection.LandCenter.X, selection.LandCenter.Y - 9, selection.LandCenter.X, selection.LandCenter.Y + 10);
                                outputGraphics.DrawLine(bluePen, selection.LandCenter.X - 9, selection.LandCenter.Y, selection.LandCenter.X + 10, selection.LandCenter.Y);
                            }
                        }
                        else
                        {

                        }
                        break;
                    case Selection.SelectionKinds.Arbitrary:
                        if (selection.isFinished == false || selection == this.selections[this.currentSelectionIndex])
                        {
                            
                        }
                        else if (selection == this.selections[this.currentSelectionIndex])
                        {
                            if (this.imageCenter != new Point(-1, -1))
                            {
                                Pen bluePen = new Pen(Color.DarkBlue, 2), whitePen = new Pen(Color.White, 4);
                                outputGraphics.DrawLine(whitePen, this.imageCenter, selection.LandCenter);
                                outputGraphics.DrawLine(bluePen, this.imageCenter, selection.LandCenter);
                                bluePen = new Pen(Color.DarkBlue, 3);
                                whitePen = new Pen(Color.White, 5);
                                outputGraphics.DrawLine(whitePen, selection.LandCenter.X, selection.LandCenter.Y - 9, selection.LandCenter.X, selection.LandCenter.Y + 10);
                                outputGraphics.DrawLine(whitePen, selection.LandCenter.X - 9, selection.LandCenter.Y, selection.LandCenter.X + 10, selection.LandCenter.Y);
                                outputGraphics.DrawLine(bluePen, selection.LandCenter.X, selection.LandCenter.Y - 9, selection.LandCenter.X, selection.LandCenter.Y + 10);
                                outputGraphics.DrawLine(bluePen, selection.LandCenter.X - 9, selection.LandCenter.Y, selection.LandCenter.X + 10, selection.LandCenter.Y);
                            }
                        }
                        else
                        {

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
                        this.selections.Last().DeleteAPoint();
                    }
                    break;

                case MouseButtons.Right:
                    if (this.selections.Last().Kind == Selection.SelectionKinds.Arbitrary)    //Add point or finish the arbitrary selection
                    {
                        this.selections.Last().AddAPoint(mouseLocation);
                    }
                    else    //Delete the rectangular selection
                    {
                        this.selections.Last().DeleteAPoint();
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
            if (this.workingState == WorkingStates.WorkingOnSelection)
                this.selections.Last().UpdateNextPointLocation(mouseLocation);
            /*switch (this.workingState)
            {
                case WorkingStates.Idle:
                    //Do nothing?
                    break;

                case WorkingStates.WorkingOnSelection:
                    //Calculate an outline for a selection;
                    this.selections.Last().UpdateNextPointLocation(mouseLocation);
                    break;
            }*/
        }

        private void CalculateSelectionCost(Selection selection)
        {
            switch (selection.Kind)
            {
                case Selection.SelectionKinds.Rectangular:
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
                        selection.Cost = (int)(Math.Floor((selection.ListOfLand.Count * Math.Pow(this.pixelRatio, 2)) / (0.685 * distanceFromCenter)));
                    }
                    selection.LandArea = (int)Math.Floor(selection.ListOfLand.Count * Math.Pow(this.pixelRatio, 2));
                    break;

                case Selection.SelectionKinds.Arbitrary:

                    break;
            }
            //selectionInfo = "Area Found! Action: " + currentAct + "; Total Area = " + area + "; Land Area = " + landArea + "; Cost = " + cost + ";";
        }

        private void CalculateSelectionArea(Selection selection)
        {
            selection.ListOfLand.Clear();
            selection.Area = 0;
            switch (selection.Kind)
            {
                case Selection.SelectionKinds.Rectangular:
                    for (int x = selection.TopLeftPoint.X; x < selection.BottomRightPoint.X; x++)
                    {
                        for (int y = selection.TopLeftPoint.Y; y < selection.BottomRightPoint.Y; y++)
                        {
                            if (this.seaPixelsBitmap.GetPixel(x, y) != Color.FromArgb(255, 16, 89))
                                selection.ListOfLand.Add(new Point(x, y));
                            selection.Area++;
                        }
                    }
                    break;

                case Selection.SelectionKinds.Arbitrary:

                    break;
            }
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
    }

}