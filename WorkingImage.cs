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
        protected Bitmap innitialBitmap;
        public Bitmap backgroundOutput, foregroundOutput;

        public void InitializeBitmaps(string _fileName, PaintEventArgs e)
        {
            innitialBitmap = new Bitmap(_fileName);
            backgroundOutput = (Bitmap)innitialBitmap.Clone();
            e.Graphics.DrawImage(backgroundOutput, 0, 0, backgroundOutput.Width, backgroundOutput.Height);
            Rectangle transparentRectangle = new Rectangle(0, 0, backgroundOutput.Width, backgroundOutput.Height);
            Brush transparentBrush = new SolidBrush(Color.FromArgb(0, 0, 0, 0));
            using (Graphics g = Graphics.FromImage(foregroundOutput))
                g.FillRectangle(transparentBrush, transparentRectangle);
            foregroundOutput.MakeTransparent();
        }

        public void RedrawBackground()
        {

        }

        public void RedrawForeground()
        {

        }

        public void AddSeaColor(Point _mouseXY = new Point())
        {
            Color seaColor = mainBitmap.GetPixel(_mouseXY.X, _mouseXY.Y);
            for (int x = 0; x < mainBitmap.Width; x++)
                for (int y = 0; y < mainBitmap.Height; y++)
                    if ((mainBitmap.GetPixel(x, y).R >= seaColor.R - 15 && mainBitmap.GetPixel(x, y).R <= seaColor.R + 15) && (mainBitmap.GetPixel(x, y).G >= seaColor.G - 15 && mainBitmap.GetPixel(x, y).G <= seaColor.G + 15) && (mainBitmap.GetPixel(x, y).B >= seaColor.B - 15 && mainBitmap.GetPixel(x, y).B <= seaColor.B + 15))
                        mainBitmap.SetPixel(x, y, Color.FromArgb(255, 16, 89));
        }
    }

    interface IFigure
    {
        int area { get; set; }
        int landArea { get; set; }
        int cost { get; set; }
        string selectionInfo { get; set; }

        Point landCenter { get; set; }
    }

    class RectangularFigure : IFigure
    {
        int IFigure.area { get; set; }
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
    }
}