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


        //Universal fields:
        protected List<Point> listOfPoints = new List<Point>();                     //Main list of singular points (for rectangular max count = 2)
        protected Point topLeftPoint, bottomRightPoint;                             //Corner points of selection
        private List<Point> listOfLand = new List<Point>();                         //List of land pixels (filled by the workingImage object)
        protected bool isFinished = false;
        protected int area;
        
        public List<Point> ListOfPoints { get { return listOfPoints; } }
        public Point TopLeftPoint { get { return topLeftPoint; } }
        public Point BottomRightPoint { get { return bottomRightPoint; } }
        public List<Point> ListOfLand { get => listOfLand; set => listOfLand = value; }
        public bool IsFinished { get { return isFinished; } }
        public int Area { get => area; set => area = value; }


        //Exclusively rectangular fields:
        protected Size sizeOfOutline;
        protected Rectangle rectangularOutline;

        public Rectangle RectangularOutline { get { return rectangularOutline; } }


        //Exclusively arbitrary fields:
        protected List<Point> outlinePoints = new List<Point>();                    //List of pixels, that belong to an otline of the selection
        protected List<Point> innerPoints = new List<Point>();                      //List of pixels inside the selection
        protected Point nextArbitraryPoint;

        public Point NextArbitraryPoint { get { return nextArbitraryPoint; } }
        public List<Point> OutlinePoints { get { return outlinePoints; } }
        public List<Point> InnerPoints { get { return innerPoints; } }


        //Public fields:
        public int Cost, LandArea;
        public Point LandCenter = new Point(-1, -1);                                //Mean point between all landPixels
        public int PointsChecked = 0;
        public string SelectionInfo = "Area Finder";


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
            base.isFinished = true;
        }

        public override bool DeleteAPoint()
        {
            base.listOfPoints.RemoveAt(listOfPoints.Count - 1);
            return true;
        }
    }

    class ArbitrarySelection : Selection
    {
        private List<Point> somePoints = new List<Point>();             //Points that could be an inner or outer points (unknown at that moment)
        private List<Point> enlargedOutlinePoints = new List<Point>();  //Point of an enlarged outline of selection 
                                                                        //It is used to find out, if the points are inner or outer. 
                                                                        //If the "while" has reached any of such points, then they're outer and vice versa
        
        private List<Point> pointsToBeChecked = new List<Point>();      //List of points, that should be checked later
        private bool[,] checkedPointsMatrix, pointsToBeCheckedMatrix;
        private Point enlargedTopLeft;

        public ArbitrarySelection(Point mouseLocation) : base(mouseLocation)
        {
            base.selectionKind = SelectionKinds.Arbitrary;
            base.nextArbitraryPoint = mouseLocation;
        }

        public override void UpdateNextPointLocation(Point mousePosition)
        {
            if (mousePosition.X > base.listOfPoints[0].X - 12 && mousePosition.X < base.listOfPoints[0].X + 12
                && mousePosition.Y > base.listOfPoints[0].Y - 12 && mousePosition.Y < base.listOfPoints[0].Y + 12 && base.listOfPoints.Count >= 3)
                base.nextArbitraryPoint = base.listOfPoints[0];
            else
                base.nextArbitraryPoint = mousePosition;
        }

        public override void AddAPoint(Point mousePosition)
        {
            //Checking if the user is trying to finish the selection:
            if (base.nextArbitraryPoint == base.listOfPoints[0])
            {
                base.listOfPoints.Add(base.nextArbitraryPoint);
                base.topLeftPoint = mousePosition; base.bottomRightPoint = new Point(0, 0); //Corner points of rectangle, that contains arbitrary selection
                Point previousPoint = new Point(-100, -100);
                
                //Find outline pixels and pit them into outlinePoints:
                foreach (Point arbitraryPoint in base.listOfPoints)                         
                {
                    if (base.topLeftPoint.X > arbitraryPoint.X) base.topLeftPoint.X = arbitraryPoint.X;
                    if (base.topLeftPoint.Y > arbitraryPoint.Y) base.topLeftPoint.Y = arbitraryPoint.Y;
                    if (base.bottomRightPoint.X < arbitraryPoint.X) base.bottomRightPoint.X = arbitraryPoint.X;
                    if (base.bottomRightPoint.Y < arbitraryPoint.Y) base.bottomRightPoint.Y = arbitraryPoint.Y;
                    if (previousPoint != new Point(-100, -100))
                    {
                        if (previousPoint.X == arbitraryPoint.X)
                            previousPoint.X++;
                        Point min = new Point(), max = new Point();
                        min.X = Math.Min(previousPoint.X, arbitraryPoint.X);
                        max.X = Math.Max(previousPoint.X, arbitraryPoint.X);
                        min.Y = Math.Min(previousPoint.Y, arbitraryPoint.Y);
                        max.Y = Math.Max(previousPoint.Y, arbitraryPoint.Y);
                        double tangent = (double)(previousPoint.Y - arbitraryPoint.Y) / (double)(previousPoint.X - arbitraryPoint.X);
                        double step = Math.Abs(1 / tangent) / 1.1;
                        if (step > 1)
                            step = 1;
                        for (double x = 0; x <= max.X - min.X; x += step)
                        {
                            int y;
                            if (tangent > 0)
                                y = (int)Math.Round(x * tangent) + min.Y;
                            else
                                y = (int)Math.Round(x * tangent) + max.Y;
                            for (int i = (int)Math.Round(x) + min.X - 1; i < (int)Math.Round(x) + min.X + 1; i++)
                                for (int j = y - 1; j < y + 1; j++)
                                    if (base.outlinePoints.Contains(new Point(i, j)) == false)
                                        base.outlinePoints.Add(new Point(i, j));
                        }
                    }
                    previousPoint = arbitraryPoint;
                }

                //Find EnlargedOutline to calculate innerPoints:
                for (int x = base.topLeftPoint.X - 15; x <= base.bottomRightPoint.X + 15; x++)
                {
                    this.enlargedOutlinePoints.Add(new Point(x, base.topLeftPoint.Y - 15));
                    this.enlargedOutlinePoints.Add(new Point(x, base.bottomRightPoint.Y + 15));
                }
                for (int y = base.topLeftPoint.Y - 14; y <= base.bottomRightPoint.Y + 14; y++)
                {
                    this.enlargedOutlinePoints.Add(new Point(base.topLeftPoint.X - 15, y));
                    this.enlargedOutlinePoints.Add(new Point(base.bottomRightPoint.X + 15, y));
                }

                //Create matrices of right size:
                int bitmapWidth = (base.bottomRightPoint.X + 15) - (base.topLeftPoint.X - 15) + 1;
                int bitmapHeight = (base.bottomRightPoint.Y + 15) - (base.topLeftPoint.Y - 15) + 1;
                this.checkedPointsMatrix = new bool[bitmapWidth, bitmapHeight];
                this.pointsToBeCheckedMatrix = new bool[bitmapWidth, bitmapHeight];
                this.enlargedTopLeft = new Point(base.topLeftPoint.X - 15, base.topLeftPoint.Y - 15);

                //Find any point, that is definetely not in the outline (inner or outer):
                Point randomNotAnOutlinePoint = new Point(-100, -100);
                while (randomNotAnOutlinePoint == new Point(-100, -100))
                {
                    Random random = new Random();
                    Point randomHopefullyNotAnOutlinePoint;
                    Point randomOutlinePoint = base.ListOfPoints[random.Next(base.ListOfPoints.Count)];
                    randomHopefullyNotAnOutlinePoint = new Point(randomOutlinePoint.X + random.Next(-9, 10),
                                                                        randomOutlinePoint.Y + random.Next(-9, 10));
                    if (base.OutlinePoints.Contains(randomHopefullyNotAnOutlinePoint) == false)
                        randomNotAnOutlinePoint = randomHopefullyNotAnOutlinePoint;
                }

                //Checking all points, that the algorithm will be ably to achive:
                bool somePointsIsAnOuterPoints = false, everythingIsChecked = false;
                somePointsIsAnOuterPoints = this.CheckAPoint(randomNotAnOutlinePoint, somePointsIsAnOuterPoints);
                //MessageBox.Show("somePoints: " + somePoints.Count + " checkedPointsArr:" + checkedPointsMatrix.Length + " pointsToBeChecked: " + pointsToBeChecked.Count);
                while (!everythingIsChecked)
                {
                    Random random = new Random();
                    if (pointsToBeChecked.Count > 0)
                        somePointsIsAnOuterPoints = this.CheckAPoint(pointsToBeChecked.Last(), somePointsIsAnOuterPoints);
                    else
                        everythingIsChecked = true;
                }
                //MessageBox.Show("somePoints: " + somePoints.Count + " checkedPointsArr:" + checkedPointsMatrix.Length + " pointsToBeChecked: " + pointsToBeChecked.Count);

                //If the points is outer, then filling base.innerPoints with all points exept known, if inner, then simply coping them:
                if (somePointsIsAnOuterPoints)
                {
                    for (int x = base.topLeftPoint.X - 14; x <= base.bottomRightPoint.X + 14; x++)
                    {
                        for (int y = base.topLeftPoint.Y - 14; y <= base.bottomRightPoint.Y + 14; y++)
                        {
                            if ((this.somePoints.Contains(new Point(x, y)) || base.outlinePoints.Contains(new Point(x, y))) == false)
                            {
                                base.innerPoints.Add(new Point(x, y));
                            }
                        }
                    }
                }
                else
                    base.innerPoints = this.somePoints;

                //Declaring, that the selection is finished (finally...):
                base.isFinished = true;
            }
            else //If not, then simply adding the next point:
                base.listOfPoints.Add(mousePosition);
        }

        public override bool DeleteAPoint()
        {
            base.listOfPoints.RemoveAt(listOfPoints.Count - 1);
            if (base.listOfPoints.Count == 0) 
                return true;
            else 
                return false;
        }

        private bool CheckAPoint(Point recieved_point, bool isDefinetelyAnOuterPoint)
        {
            base.PointsChecked++;

            //Changing all matrices and list:

            this.pointsToBeCheckedMatrix[recieved_point.X - this.enlargedTopLeft.X, recieved_point.Y - this.enlargedTopLeft.Y] = false;
            this.pointsToBeChecked.Remove(new Point(recieved_point.X, recieved_point.Y));
            this.checkedPointsMatrix[recieved_point.X - this.enlargedTopLeft.X, recieved_point.Y - this.enlargedTopLeft.Y] = true;
            
            if (this.enlargedOutlinePoints.Contains(recieved_point)) //In EnlargedOutline
                return true;

            if (base.outlinePoints.Contains(recieved_point)) //In outline
                return isDefinetelyAnOuterPoint;
            
            this.somePoints.Add(recieved_point);

            //Adding 4 surrounding points to matrix be checked later, if they are not already in pointsToBeCheckedMatrix
            //And adding them after that to list corresponding to matrix
            //Matrix is for easy access to exact point (to check, if the point is already going to be checked)
            //Such check is need to ensure that no redundant points will be added to list
            //List if for actual checking of points

            if (this.checkedPointsMatrix[recieved_point.X - this.enlargedTopLeft.X + 1, recieved_point.Y - this.enlargedTopLeft.Y] == false)
            {
                if (!this.pointsToBeCheckedMatrix[recieved_point.X - this.enlargedTopLeft.X + 1, recieved_point.Y - this.enlargedTopLeft.Y])
                {
                    this.pointsToBeCheckedMatrix[recieved_point.X - this.enlargedTopLeft.X + 1, recieved_point.Y - this.enlargedTopLeft.Y] = true;
                    this.pointsToBeChecked.Add(new Point(recieved_point.X + 1, recieved_point.Y));
                }
            }

            if (this.checkedPointsMatrix[recieved_point.X - this.enlargedTopLeft.X - 1, recieved_point.Y - this.enlargedTopLeft.Y] == false)
            {
                if (!this.pointsToBeCheckedMatrix[recieved_point.X - this.enlargedTopLeft.X - 1, recieved_point.Y - this.enlargedTopLeft.Y])
                {
                    this.pointsToBeCheckedMatrix[recieved_point.X - this.enlargedTopLeft.X - 1, recieved_point.Y - this.enlargedTopLeft.Y] = true;
                    this.pointsToBeChecked.Add(new Point(recieved_point.X - 1, recieved_point.Y));
                }
            }

            if (this.checkedPointsMatrix[recieved_point.X - this.enlargedTopLeft.X, recieved_point.Y - this.enlargedTopLeft.Y + 1] == false)
            {
                if (!this.pointsToBeCheckedMatrix[recieved_point.X - this.enlargedTopLeft.X, recieved_point.Y - this.enlargedTopLeft.Y + 1])
                {
                    this.pointsToBeCheckedMatrix[recieved_point.X - this.enlargedTopLeft.X, recieved_point.Y - this.enlargedTopLeft.Y + 1] = true;
                    this.pointsToBeChecked.Add(new Point(recieved_point.X, recieved_point.Y + 1));
                }
            }

            if (this.checkedPointsMatrix[recieved_point.X - this.enlargedTopLeft.X, recieved_point.Y - this.enlargedTopLeft.Y - 1] == false)
            {
                if (!this.pointsToBeCheckedMatrix[recieved_point.X - this.enlargedTopLeft.X, recieved_point.Y - this.enlargedTopLeft.Y - 1])
                {
                    this.pointsToBeCheckedMatrix[recieved_point.X - this.enlargedTopLeft.X, recieved_point.Y - this.enlargedTopLeft.Y - 1] = true;
                    this.pointsToBeChecked.Add(new Point(recieved_point.X, recieved_point.Y - 1));
                }
            }

            return isDefinetelyAnOuterPoint;
        }
    }
}