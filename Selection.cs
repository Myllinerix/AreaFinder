using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.CodeDom;

namespace Area_Finder_Too
{
    abstract class Selection
    {
        public enum SelectionKinds { Rectangular, Arbitrary };
        protected SelectionKinds selectionKind;
        public SelectionKinds Kind { get { return selectionKind; } }

        protected Point topLeftPoint, bottomRightPoint;
        protected Size sizeOfOutline;
        protected Rectangle rectangularOutline;

        public Point TopLeftPoint { get { return topLeftPoint; } }
        public Point BottomRightPoint { get { return bottomRightPoint; } }
        public Size SizeOfOutline { get { return sizeOfOutline; } }
        public Rectangle RectangularOutline { get { return rectangularOutline; } }
        

        public List<Point> listOfPoints = new List<Point>();                     //Main list of singular points (for rectangular max count = 2)
        //protected List<Point> listOfOutPointPixels = new List<Point>();             //List of pixels belonging to outer vertices of selection
        //protected List<Point> listOfOutLinePixels = new List<Point>();              //List of pixels belonging to outer sides of selection
        //protected List<Point> rawListOfLand = new List<Point>();                    //Raw list of land pixels not excluding sea pixels (area = listOfLand.Count)
        public bool isFinished = false;

        public Selection(Point mousePosition)
        {
            this.listOfPoints.Add(mousePosition);
        }

        public abstract void UpdateNextPointLocation(Point mousePosition);

        public abstract void AddAPoint(Point mousePosition);

        public abstract bool DeleteAPoint();

        /*public void SetCenter(Point _mouseXY = new Point())
        {
            List<Bitmap> bitmaps = new List<Bitmap>();
            bitmaps.Add(mainBitmap);
            bitmaps.Add(arbitraryBitmap);
            bitmaps.Add(outputBitmap);
            foreach (Bitmap _bitmap in bitmaps)
            {
                Graphics gr = Graphics.FromImage(_bitmap);
                Pen bluePen = new Pen(Color.DarkBlue, 3), whitePen = new Pen(Color.White, 5);
                gr.DrawLine(whitePen, _mouseXY.X, _mouseXY.Y - 9, _mouseXY.X, _mouseXY.Y + 10);
                gr.DrawLine(whitePen, _mouseXY.X - 9, _mouseXY.Y, _mouseXY.X + 10, _mouseXY.Y);
                gr.DrawLine(bluePen, _mouseXY.X, _mouseXY.Y - 9, _mouseXY.X, _mouseXY.Y + 10);
                gr.DrawLine(bluePen, _mouseXY.X - 9, _mouseXY.Y, _mouseXY.X + 10, _mouseXY.Y);
            }
            imageCenter = _mouseXY;
        }*/

        /*public void SetPointRect(Point _mouseXY = new Point())
        {
            landArea = 0;
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
            }
        }*/

        /*public void SetPointArb(Point _mouseXY = new Point())
        {
            if (currentAct == CurrentAction.NextPointArb || currentAct == CurrentAction.FirstPointArb)
            {
                if (currentAct == CurrentAction.FirstPointArb)
                {
                    currentAct = Selection.CurrentAction.NextPointArb;
                    RecreateArbBitmap();
                    landArea = 0;
                    listOfLand.Clear();
                }
                listOfArbPoints.Add(_mouseXY);
                for (int x = _mouseXY.X - 2; x <= _mouseXY.X + 2; x++)
                {
                    for (int y = _mouseXY.Y - 2; y <= _mouseXY.Y + 2; y++)
                    {
                        base.arbitraryBitmap.SetPixel(x, y, Color.DarkCyan);
                        if (x == listOfArbPoints[0].X && y == listOfArbPoints[0].Y && listOfArbPoints.Count > 2)
                            currentAct = CurrentAction.LastPointArb;
                    }
                }
                selectionInfo = "Area'll be find! Action: " + currentAct + "; Previous Point = " + listOfArbPoints[listOfArbPoints.Count() - 1] + "; Current Point = " + _mouseXY + ";";
                if (currentAct == CurrentAction.LastPointArb)
                {
                    CalculateArb(_mouseXY);
                }
            }
        }*/

        /*private void CalculateArb(Point _mouseXY = new Point())
        {
            Pen cyanPen = new Pen(Color.Cyan, 1);
            using (Graphics gr = Graphics.FromImage(base.arbitraryBitmap))
                gr.DrawLine(cyanPen, listOfArbPoints[listOfArbPoints.Count - 2], listOfArbPoints[listOfArbPoints.Count - 1]);
            Point topLeft = new Point(99999, 99999), bottomRight = new Point(0, 0);
            foreach (Point _curArbPoint in listOfArbPoints)
            {
                if (_curArbPoint.X < topLeft.X)
                    topLeft.X = Math.Abs(_curArbPoint.X - 5);
                if (_curArbPoint.Y < topLeft.Y)
                    topLeft.Y = Math.Abs(_curArbPoint.Y - 5);
                if (_curArbPoint.X > bottomRight.X)
                    bottomRight.X = _curArbPoint.X + 5;
                if (_curArbPoint.Y > bottomRight.Y)
                    bottomRight.Y = _curArbPoint.Y + 5;
                while (bottomRight.Y > mainBitmap.Height)
                    bottomRight.Y--;
                while (bottomRight.X > mainBitmap.Width)
                    bottomRight.X--;
            }
            for (int x = topLeft.X; x <= bottomRight.X; x++)
            {
                ArbPixelState pixelState = ArbPixelState.Outside;
                int y = topLeft.Y;
                while (y <= bottomRight.Y)
                {
                    switch (pixelState)
                    {
                        case ArbPixelState.Outside:
                            if (base.arbitraryBitmap.GetPixel(x, y) == Color.FromArgb(0, 255, 255))
                            {
                                pixelState = ArbPixelState.BorderToInside;
                            }
                            break;

                        case ArbPixelState.BorderToInside:
                            if (base.arbitraryBitmap.GetPixel(x, y) != Color.FromArgb(0, 255, 255))
                            {
                                pixelState = ArbPixelState.Inside;
                                if (base.arbitraryBitmap.GetPixel(x, y) != Color.FromArgb(255, 16, 89))
                                {
                                    base.arbitraryBitmap.SetPixel(x, y, Color.LightGreen);
                                    listOfLand.Add(new Point(x, y));
                                    landArea++;
                                }
                            }
                            break;

                        case ArbPixelState.Inside:
                            if (base.arbitraryBitmap.GetPixel(x, y) != Color.FromArgb(0, 255, 255))
                            {
                                if (base.arbitraryBitmap.GetPixel(x, y) != Color.FromArgb(255, 16, 89))
                                {
                                    base.arbitraryBitmap.SetPixel(x, y, Color.LightGreen);
                                    listOfLand.Add(new Point(x, y));
                                    landArea++;
                                }
                            }
                            else
                                pixelState = ArbPixelState.BorderToOutside;
                            break;

                        case ArbPixelState.BorderToOutside:
                            if (base.arbitraryBitmap.GetPixel(x, y) != Color.FromArgb(0, 255, 255))
                                pixelState = ArbPixelState.Outside;
                            break;

                        default:
                            break;
                    }
                    y++;
                }
            }
            RecreateOutputBitmapFromArbitrary();
            CalculateCost(_mouseXY);
            currentAct = CurrentAction.StayingStill;
        }*/

        /*public void UpdateOutputBitmap(Point _mouseXY)
        {
            Pen pen2 = new Pen(Color.Cyan, 2);
            Pen pen1 = new Pen(Color.Cyan, 1);
            if (currentAct == CurrentAction.SecondPointRect)
            {
                RecreateOutputBitmapFromMain();
                Rectangle rect;
                if (_mouseXY.X > constPoint.X && _mouseXY.Y > constPoint.Y)
                {
                    rect = new Rectangle(constPoint.X, constPoint.Y, _mouseXY.X - constPoint.X, _mouseXY.Y - constPoint.Y);
                    topLeftPoint.X = constPoint.X;
                    topLeftPoint.Y = constPoint.Y;
                }
                else if (_mouseXY.X > constPoint.X)
                {
                    rect = new Rectangle(constPoint.X, _mouseXY.Y, _mouseXY.X - constPoint.X, constPoint.Y - _mouseXY.Y);
                    topLeftPoint.X = constPoint.X;
                    topLeftPoint.Y = _mouseXY.Y;
                }
                else if (_mouseXY.Y > constPoint.Y)
                {
                    rect = new Rectangle(_mouseXY.X, constPoint.Y, constPoint.X - _mouseXY.X, _mouseXY.Y - constPoint.Y);
                    topLeftPoint.X = _mouseXY.X;
                    topLeftPoint.Y = constPoint.Y;
                }
                else
                {
                    rect = new Rectangle(_mouseXY.X, _mouseXY.Y, constPoint.X - _mouseXY.X, constPoint.Y - _mouseXY.Y);
                    topLeftPoint.X = _mouseXY.X;
                    topLeftPoint.Y = _mouseXY.Y;
                }
                using (Graphics gr = Graphics.FromImage(base.outputBitmap))
                {
                    gr.DrawRectangle(pen2, rect);
                }
                area = (int)Math.Floor(Math.Abs(constPoint.X - _mouseXY.X) * Math.Abs(constPoint.Y - _mouseXY.Y) * Math.Pow(base.pixelRatio, 2));
                selectionInfo = "Area Found! Action: " + currentAct + "; Point 1 = " + constPoint + "; Point 2 = " + _mouseXY + "; Total Area = " + area + "; Land Area = " + landArea + "; Cost = " + cost + ";";
            }
            else if (currentAct == CurrentAction.NextPointArb)
            { 
                if (listOfArbPoints.Count > 1)
                    using (Graphics gr = Graphics.FromImage(base.arbitraryBitmap))
                        gr.DrawLine(pen1, listOfArbPoints[listOfArbPoints.Count - 2], listOfArbPoints[listOfArbPoints.Count - 1]);
                RecreateOutputBitmapFromArbitrary();
                using (Graphics gr = Graphics.FromImage(base.outputBitmap))
                    gr.DrawLine(pen2, listOfArbPoints[listOfArbPoints.Count - 1], _mouseXY);
                selectionInfo = "Area'll be find! Action: " + currentAct + "; Previous Point = " + listOfArbPoints[listOfArbPoints.Count() - 1] + "; Current Point = " + _mouseXY + ";";
            }
        }*/

        /*public void RecreateArbBitmap()
        {
            listOfArbPoints.Clear();
            arbitraryBitmap = (Bitmap)mainBitmap.Clone();
        }*/

        /*public void CalculateCost(Point _mouseXY = new Point())
        {
            if (imageCenter.X != -1 && listOfLand.Count() > 0)
            {
                Point pixelsSum = new Point(0, 0);
                foreach (Point pixel in listOfLand)
                {
                    pixelsSum.X += pixel.X;
                    pixelsSum.Y += pixel.Y;
                }
                landCenter.X = pixelsSum.X / listOfLand.Count();
                landCenter.Y = pixelsSum.Y / listOfLand.Count();
                double distance = Math.Sqrt(Math.Pow(Math.Abs(landCenter.X - base.imageCenter.X), 2) + Math.Pow(Math.Abs(landCenter.Y - base.imageCenter.Y), 2)) * base.pixelRatio;
                cost = (int)(Math.Floor((landArea * Math.Pow(base.pixelRatio, 2)) / (0.685 * distance)));
                using (Graphics gr = Graphics.FromImage(base.outputBitmap))
                {
                    Pen bluePen = new Pen(Color.DarkBlue, 2), whitePen = new Pen(Color.White, 4);
                    gr.DrawLine(whitePen, base.imageCenter, landCenter);
                    gr.DrawLine(bluePen, base.imageCenter, landCenter);
                    bluePen = new Pen(Color.DarkBlue, 3);
                    whitePen = new Pen(Color.White, 5);
                    gr.DrawLine(whitePen, landCenter.X, landCenter.Y - 9, landCenter.X, landCenter.Y + 10);
                    gr.DrawLine(whitePen, landCenter.X - 9, landCenter.Y, landCenter.X + 10, landCenter.Y);
                    gr.DrawLine(bluePen, landCenter.X, landCenter.Y - 9, landCenter.X, landCenter.Y + 10);
                    gr.DrawLine(bluePen, landCenter.X - 9, landCenter.Y, landCenter.X + 10, landCenter.Y);
                }     
            }
            landArea = (int)Math.Floor(landArea * Math.Pow(base.pixelRatio, 2));
            area = (int)Math.Floor(Math.Abs(constPoint.X - _mouseXY.X) * Math.Abs(constPoint.Y - _mouseXY.Y) * Math.Pow(base.pixelRatio, 2));
            selectionInfo = "Area Found! Action: " + currentAct + "; Total Area = " + area + "; Land Area = " + landArea + "; Cost = " + cost + ";";
        }*/
    }

    class RectangularSelection : Selection
    {
        public RectangularSelection(Point mouseLocation) : base (mouseLocation)
        {
            base.selectionKind = SelectionKinds.Rectangular;
        }

        public override void UpdateNextPointLocation(Point mousePosition)
        {
            base.topLeftPoint = new Point(Math.Min(mousePosition.X, base.listOfPoints.Last().X),
                Math.Min(mousePosition.Y, base.listOfPoints.Last().Y));
            base.bottomRightPoint = new Point(Math.Max(mousePosition.X, base.listOfPoints.Last().X),
                Math.Max(mousePosition.Y, base.listOfPoints.Last().Y));

            base.sizeOfOutline = new Size(base.bottomRightPoint.X - base.topLeftPoint.X, base.bottomRightPoint.Y - base.topLeftPoint.Y);
            base.rectangularOutline = new Rectangle(base.topLeftPoint, base.sizeOfOutline);
        }

        public override void AddAPoint(Point mousePosition)
        {
            base.listOfPoints.Add(mousePosition);
        }

        public override bool DeleteAPoint()
        {
            base.listOfPoints.RemoveAt(listOfPoints.Count - 1);
            return true;
        }
    }

    class ArbitrarySelection : Selection
    {
        public ArbitrarySelection(Point mouseLocation) : base(mouseLocation)
        {
            base.selectionKind = SelectionKinds.Arbitrary;
        }

        public override void UpdateNextPointLocation(Point mousePosition)
        {
            
        }

        public override void AddAPoint(Point mousePosition)
        {

        }

        public override bool DeleteAPoint()
        {
            base.listOfPoints.RemoveAt(listOfPoints.Count - 1);
            if (base.listOfPoints.Count == 0) 
                return true;
            else 
                return false;
        }
    }
}