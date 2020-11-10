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

        //private List

        private Point imageCenter = new Point(-1, -1);
        private double pixelRatio = 11.24;

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

        /*public void InvalidateCanvas(List<Point> pointsToInvalidate)
        {
            //g_canvas.
        }

        public void InvalidateCanvas(Region regionToInvalidate)
        {
            
        }

        public void InvalidateCanvas()
        {

        }*/
    }


    /*public interface IFigure
    {
        int area { get; set; }
        int landArea { get; set; }
        int cost { get; set; }
        string selectionInfo { get; set; }

        Point landCenter { get; set; }
    }

    public class RectangularFigure : IFigure
    {
        public int area { get; set; }
        area = 0;
        int IFigure.landArea { get; set; }
        int IFigure.cost { get; set; }
        string IFigure.selectionInfo { get; set; }
        Point IFigure.landCenter { get; set; }

        private Point constPoint = new Point(0, 0), topLeftPoint = new Point(0, 0);
    }

    class ArbitraryFigure : IFigure
    {
        private int area = 0, landArea = 0, cost = 0;
        private List<Point> listOfArbPoints = new List<Point>();
    }*/
}