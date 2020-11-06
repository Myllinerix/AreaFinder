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
        public Bitmap innitialBitmap;
        public Bitmap baseBitmap;
        public Bitmap outputBitmap;
        //public int Width, Height;

        public WorkingImage(string _fileName)
        {
            baseBitmap = new Bitmap(_fileName);
            outputBitmap = (Bitmap)baseBitmap.Clone();
            
            /*Rectangle transparentRectangle = new Rectangle(0, 0, outputBitmap.Width, outputBitmap.Height);
            Brush transparentBrush = new SolidBrush(Color.FromArgb(0, 0, 0, 0));
            using (Graphics g = Graphics.FromImage(foregroundOutput))
                g.FillRectangle(transparentBrush, transparentRectangle);
            foregroundOutput.MakeTransparent();*/
        }

        public void Draw(Graphics g_canvas)
        {
            g_canvas.DrawImage(outputBitmap, 0, 0, outputBitmap.Width, outputBitmap.Height);
        }

        /*public void RedrawForeground()
        {

        }*/

        public int Width()
        {
            return outputBitmap.Width;
        }
        public int Height()
        {
            return outputBitmap.Height;
        }

        public void SetPixelRatio(double pixelRatio)
        {

        }

        public string SelectionInfo()
        {
            return "";
        }

        public void UpdateOutputBitmap(Point mouseLocation)
        {

        }

        public void SetRectanglePoint()
        {

        }

        public void SetArbitraryPoint()
        {

        }

        public void AddSeaColor()
        {
            Color seaColor = baseBitmap.GetPixel(_mouseXY.X, _mouseXY.Y);
            for (int x = 0; x < baseBitmap.Width; x++)
                for (int y = 0; y < baseBitmap.Height; y++)
                    if ((baseBitmap.GetPixel(x, y).R >= seaColor.R - 15 && baseBitmap.GetPixel(x, y).R <= seaColor.R + 15) && (baseBitmap.GetPixel(x, y).G >= seaColor.G - 15 && baseBitmap.GetPixel(x, y).G <= seaColor.G + 15) && (baseBitmap.GetPixel(x, y).B >= seaColor.B - 15 && baseBitmap.GetPixel(x, y).B <= seaColor.B + 15))
                        baseBitmap.SetPixel(x, y, Color.FromArgb(255, 16, 89));
        }

        public void SetCenterPoint()
        {

        }
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