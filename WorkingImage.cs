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

        private Point imageCenter = new Point(-1, -1);
        private double pixelRatio = 11.24;
        public enum WorkingStates { Idle, WorkingOnSelection };
        private WorkingStates workingState = WorkingStates.Idle;
        public WorkingStates WorkingState { get { return workingState; } }

        public WorkingImage(string _fileName)
        {
            this.baseBitmap = new Bitmap(_fileName);
            this.outputBitmap = (Bitmap)this.baseBitmap.Clone();
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
            this.imageCenter = mouseLocation;
        }

        public void OutputBitmap()
        {
            this.outputBitmap = (Bitmap)this.baseBitmap.Clone();
            Graphics outputGraphics = Graphics.FromImage(this.outputBitmap);

            foreach (Selection selection in selections)
            {
                switch (selection.Kind)
                {
                    case Selection.SelectionKinds.Rectangular:
                        if (selection.isFinished)
                        {

                        }
                        else
                        {
                            outputGraphics.DrawRectangle(new Pen(Color.Aquamarine, 2), selection.RectangularOutline);
                        }
                        break;
                    case Selection.SelectionKinds.Arbitrary:
                        if (selection.isFinished)
                        {

                        }
                        else
                        {

                        }
                        break;
                }
            }

            if (imageCenter != new Point(-1, -1))
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
            switch (mouseButton) //Find out what the control wants to do with the selection
            {
                case MouseButtons.Left:
                    if (this.selections.Last().Kind == Selection.SelectionKinds.Rectangular)    //Finish the rectangular selection
                    {
                        this.selections.Last().AddAPoint(mouseLocation);
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
            switch (selectionKind)
            {
                case Selection.SelectionKinds.Rectangular:
                    this.selections.Add(new RectangularSelection(mouseLocation)); //Create a rectangular selection
                    this.workingState = WorkingStates.WorkingOnSelection;
                    break;
                case Selection.SelectionKinds.Arbitrary:
                    this.selections.Add(new ArbitrarySelection(mouseLocation));  //Create an arbitrary selection
                    this.workingState = WorkingStates.WorkingOnSelection;
                    break;
            }
        }

        public void MouseClick(Point mouseLocation, MouseButtons mouseButton)
        {

            //Last selection in Selections is selection in work

            //Make to them corresponding changes

            //UpdateOutputBitmap

        }

        public void RedrawCurrentSelection(Point mouseLocation)
        {
            switch (this.workingState)
            {
                case WorkingStates.Idle:
                    //Do nothing?
                    break;

                case WorkingStates.WorkingOnSelection:
                    //Calculate an outline for a selection;
                    this.selections.Last().UpdateNextPointLocation(mouseLocation);
                    this.selections.Last();
                    break;
            }
        }
    }

}