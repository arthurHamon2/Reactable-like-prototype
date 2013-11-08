using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using WpfApplication2.drawer;

using System.Timers;
using System.Windows.Threading;
using System.Threading;
using System.Windows.Input;
using WpfApplication2.reactableObjectLink;

namespace WpfApplication2.reactableObjects
{
    public abstract class InputReactableObject : ReactableObject
    {
        #region Variables declaration.

        /// <summary>
        /// The path of the central shape.
        /// </summary>
        public Path geometricShape;

        public Path getGeometricShape()
        {
            return geometricShape;
        }

        /// <summary>
        /// The path of the left arc.
        /// </summary>
        protected Path arcLeft;

        /// <summary>
        /// The path of the right arc.
        /// </summary>
        protected Path arcRight;

		/// <summary>
		/// The path of the subtypes menu button.
		/// </summary>
		protected Path subtypesMenuButton;

        /// <summary>
        /// The image on the object.
        /// </summary>
        protected Image image;

        /// <summary>
        /// The path of the pointer of the left arc.
        /// </summary>
        public Path pointerArcLeft;

        /// <summary>
        /// The collision surface allows to know if some objects can communicate or not each others
        /// </summary>
        public Path collisionSurface;
        public double collisionSurfaceRadius;

        /// <summary>
        /// The maximum value that the pointer can take.
        /// </summary>
        protected double pointerArcLeftMax;

		/// <summary>
		/// The minimum value that the pointer can take.
		/// </summary>
		protected double pointerArcLeftMin;

        /// <summary>
        /// The position of the pointer relative to its value.
        /// </summary>
        protected double pointerPositionArcLeft;

        /// <summary>
        /// Path of the left arc.
        /// </summary>
        public Path pointerArcRight;

        /// <summary>
        /// The maximum value that the pointer can take.
        /// </summary>
        protected const double pointerArcRightMax = 2000;

        /// <summary>
        /// The position of the pointer relative to its value.
        /// </summary>
        protected double pointerPositionArcRight = 0;

        /// <summary>
        /// The TranformGroup of central Shape
        /// </summary>
        protected TransformGroup geometricShapeTransformGroup; 
        
        /// <summary>
        /// The TransformGroup of the pointer of the left arc.
        /// </summary>
        protected TransformGroup pointerArcLeftTransformGroup;
        
        /// <summary>
        /// The TransformGroup of the pointer of the right arc.
        /// </summary>
        protected TransformGroup pointerArcRightTransformGroup;

        /// <summary>
        /// The RotateTransform of the central shape.
        /// </summary>
        protected RotateTransform geometricShapeRotateTransform; 
        
        /// <summary>
        /// The RotateTransform of the pointer of the left arc.
        /// </summary>
        protected RotateTransform pointerArcLeftRotateTransform; 
        
        /// <summary>
        /// The RotateTransform of the pointer of the right arc.
        /// </summary>
        protected RotateTransform pointerArcRightRotateTransform;

        /// <summary>
        /// Constant which difines the angle where is the down of the left arc from the center of the central shape.
        /// </summary>
        protected const double arcLeft_angleLow = 110;

        /// <summary>
        /// Constant which difines the angle where is the top of the left arc from the center of the central shape.
        /// </summary>
        protected const double arcLeft_angleHigh = 250;

        /// <summary>
        /// Constant which difines the angle where is the down of the right arc from the center of the central shape.
        /// </summary>
        protected const double arcRight_angleLow = 70;

        /// <summary>
        /// Constant which difines the angle where is the top of the right arc from the center of the central shape.
        /// </summary>
        protected const double arcRight_angleHigh = 290;

        /// <summary>
        /// Allows to know if there is a finger on the object.
        /// </summary>
        public bool firstFingerOnObject = false;

        /// <summary>
        /// Allow to know if a click on a object is a double or a simple click.
        /// </summary>
        public Boolean doubleClickObject = false;

        /// <summary>
        /// Timer which increase the time between two click.
        /// </summary>
        private DispatcherTimer timerDoubleClickObject;

        /// <summary>
        /// Count the time between two click.
        /// </summary>
		private int timeDoubleClickObject = 0;

		/// <summary>
		/// Stocks all object in radius.
		/// </summary>
		protected List<ReactableObject> objectsInRadius = new List<ReactableObject>();

		public List<ReactableObject> ObjectsInRadius
		{
			get { return objectsInRadius; }
			//set { objectsInRadius = value; }
		}

		/// <summary>
		/// DispatcherTimer to check connection between object all time.
		/// </summary>
		protected DispatcherTimer connectionChecker;

		/// <summary>
		/// Output of the object.
		/// </summary>
		protected ReactableObjectLink outputLink;

		public ReactableObjectLink OutputLink
		{
			get { return outputLink; }
			set { outputLink = value; }
		}


        #endregion Variables declaration.


        /// <summary>
        /// Allows to initialize the renderTransform part.
        /// </summary>
        protected void initializationInputStuff()
        {
            geometricShapeRotateTransform = new RotateTransform();
            pointerArcLeftRotateTransform = new RotateTransform();
            pointerArcRightRotateTransform = new RotateTransform();
            geometricShapeTransformGroup = new TransformGroup();
            geometricShapeTransformGroup.Children.Add(geometricShapeRotateTransform);
            pointerArcLeftTransformGroup = new TransformGroup();
            pointerArcLeftTransformGroup.Children.Add(pointerArcLeftRotateTransform);
            pointerArcRightTransformGroup = new TransformGroup();
            pointerArcRightTransformGroup.Children.Add(pointerArcRightRotateTransform);

            // Adding the created object into the main list.
            ReactableObjectList.Add(this);

            geometricShape.MouseLeftButtonDown += MouseLeftButtonDown;
            geometricShape.MouseMove += MouseMove;
            geometricShape.MouseUp += MouseUp;

        }

        #region Transforming functions.


        /// <summary>
        /// This function allows to rotate the central shape.
        /// </summary>
        /// <param name="angle">The angle of rotate of the central shape.</param>
        public void rotateGeometricShape(double angle)
        {
            //Rotate the central shape.
            geometricShapeRotateTransform.Angle += angle;
            geometricShapeRotateTransform.CenterX = height / 2;
            geometricShapeRotateTransform.CenterY = width / 2;
            geometricShape.RenderTransform = geometricShapeTransformGroup;

            double anglePointerPerAngleShape = (arcLeft_angleHigh - arcLeft_angleLow) / pointerArcLeftMax;
            double unitPositionPerAngleShape = pointerArcLeftMax / (arcLeft_angleHigh - arcLeft_angleLow);

            // If the pointer is beyond of the left arc then the software does nothing
			if (pointerPositionArcLeft + angle * unitPositionPerAngleShape * anglePointerPerAngleShape <= pointerArcLeftMax && pointerPositionArcLeft + angle * unitPositionPerAngleShape * anglePointerPerAngleShape >= pointerArcLeftMin)
            {
                pointerArcLeftRotateTransform.Angle += angle * anglePointerPerAngleShape;
                pointerArcLeftRotateTransform.CenterX = x - Canvas.GetLeft(pointerArcLeft);
                pointerArcLeftRotateTransform.CenterY = y - Canvas.GetTop(pointerArcLeft);
                pointerArcLeft.RenderTransform = pointerArcLeftTransformGroup;

                pointerPositionArcLeft = pointerPositionArcLeft + angle * unitPositionPerAngleShape * anglePointerPerAngleShape;
            }


        }


        /// <summary>
        /// This function allows to translate all the object.
        /// </summary>
        /// <param name="x">X coordonne</param>
        /// <param name="y">Y coordonne</param>
        public void translateWholeObject(double _x, double _y)
        {
            double lastX = x;
            double lastY = y;
            x = _x;
            y = _y;

            //translation of the central shape.
            Canvas.SetLeft(geometricShape, _x - height / 2);
            Canvas.SetTop(geometricShape, _y - width / 2);

            //translation of the arcs.
            Canvas.SetLeft(arcLeft, Canvas.GetLeft(arcLeft) + (x - lastX));
            Canvas.SetTop(arcLeft, Canvas.GetTop(arcLeft) + (y - lastY));
            Canvas.SetLeft(arcRight, Canvas.GetLeft(arcRight) + (x - lastX));
            Canvas.SetTop(arcRight, Canvas.GetTop(arcRight) + (y - lastY));

            //translation of the pointer of the left arc.
            Canvas.SetLeft(pointerArcLeft, Canvas.GetLeft(pointerArcLeft) + (x - lastX));
            Canvas.SetTop(pointerArcLeft, Canvas.GetTop(pointerArcLeft) + (y - lastY));

            //translation of the pointer of the right arc.
            Canvas.SetLeft(pointerArcRight, Canvas.GetLeft(pointerArcRight) + (x - lastX));
            Canvas.SetTop(pointerArcRight, Canvas.GetTop(pointerArcRight) + (y - lastY));

            //translation of the collision surface
            Canvas.SetLeft(collisionSurface, Canvas.GetLeft(collisionSurface) + (x - lastX));
            Canvas.SetTop(collisionSurface, Canvas.GetTop(collisionSurface) + (y - lastY));

			//translation of the subtypes menu button.
			Canvas.SetLeft(subtypesMenuButton, Canvas.GetLeft(collisionSurface) + (x - lastX));
			Canvas.SetTop(subtypesMenuButton, Canvas.GetTop(collisionSurface) + (y - lastY));
        }

        #endregion Transforming functions.

        #region Drawing functions of arcs.

        /// <summary>
        /// This function allows to draw the left arc of the object.
        /// </summary>
        protected void drawLeftArc()
        {

            // The radius of the circumcercle.
            double radius = Math.Sqrt(height * height + width * width) / 2;

            PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
            PathFigure segmentPathFigure = new PathFigure();

            Point arcStartPoint = PointOnCircle(radius, arcLeft_angleLow, new Point(x, y));
            Point arcStopPoint = PointOnCircle(radius, arcLeft_angleHigh, new Point(x, y));
            segmentPathFigure.StartPoint = arcStartPoint;

            ArcSegment arcSegment = new ArcSegment(arcStopPoint, new Size(radius, radius), 0, false, SweepDirection.Clockwise, true);

            pathSegmentCollection.Add(arcSegment);
            segmentPathFigure.Segments = pathSegmentCollection;
            PathFigureCollection segmentPathFigureCollection = new PathFigureCollection();
            segmentPathFigureCollection.Add(segmentPathFigure);
            PathGeometry segmentPathGeometry = new PathGeometry();
            segmentPathGeometry.Figures = segmentPathFigureCollection;

            // Set up the segment's drawing (and hit-testing) path.
            arcLeft = new Path();
            arcLeft.Stroke = Brushes.Black;
            arcLeft.StrokeThickness = 1;
            arcLeft.Fill = Brushes.Transparent;
            arcLeft.Data = segmentPathGeometry;
            Canvas.SetLeft(arcLeft, 0);
            Canvas.SetTop(arcLeft, 0);
			Canvas.SetZIndex(arcLeft, 1);
            Canvas.Children.Add(arcLeft);
        }

        /// <summary>
        /// This function allows to draw the right arc of the object.
        /// </summary>
        protected void drawRightArc()
        {
            // The radius of the circumcercle.
            double radius = Math.Sqrt(height * height + width * width) / 2;

            PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
            PathFigure segmentPathFigure = new PathFigure();

            Point arcStartPoint = PointOnCircle(radius, arcRight_angleLow, new Point(x, y));
            Point arcStopPoint = PointOnCircle(radius, arcRight_angleHigh, new Point(x, y));
            segmentPathFigure.StartPoint = arcStartPoint;

            ArcSegment arcSegment = new ArcSegment(arcStopPoint, new Size(radius, radius), 0, false, SweepDirection.Counterclockwise, true);

            pathSegmentCollection.Add(arcSegment);
            segmentPathFigure.Segments = pathSegmentCollection;
            PathFigureCollection segmentPathFigureCollection = new PathFigureCollection();
            segmentPathFigureCollection.Add(segmentPathFigure);
            PathGeometry segmentPathGeometry = new PathGeometry();
            segmentPathGeometry.Figures = segmentPathFigureCollection;

            // Set up the segment's drawing (and hit-testing) path.
            arcRight = new Path();
            arcRight.Stroke = Brushes.Black;
            arcRight.StrokeThickness = 1;
            arcRight.Fill = Brushes.Transparent;
            arcRight.Data = segmentPathGeometry;
            Canvas.SetLeft(arcRight, 0);
            Canvas.SetTop(arcRight, 0);
			Canvas.SetZIndex(arcRight, 1);
            Canvas.Children.Add(arcRight);
        }

        #endregion Drawing functions of arcs.

        #region Drawing functions of arc pointers.

        /// <summary>
        /// Allows to draw the point of the left arc.
        /// </summary>
        protected void drawPointerArcLeft()
        {
            // The radius of the circumcercle.
            double radius = Math.Sqrt(height * height + width * width) / 2;

            PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
            PathFigure segmentPathFigure = new PathFigure();

            Point StartPoint = PointOnCircle(radius, arcLeft_angleLow, new Point(0, 0));
            Point lineStopPoint1 = PointOnCircle(radius + 6, arcLeft_angleLow + 3, new Point(0, 0));
            Point lineStopPoint2 = PointOnCircle(radius + 6, arcLeft_angleLow - 3, new Point(0, 0));
            segmentPathFigure.StartPoint = StartPoint;
            segmentPathFigure.IsClosed = true;

            LineSegment lineSegment1 = new LineSegment(lineStopPoint1, true);
            LineSegment lineSegment2 = new LineSegment(lineStopPoint2, true);

            pathSegmentCollection.Add(lineSegment1);
            pathSegmentCollection.Add(lineSegment2);
            segmentPathFigure.Segments = pathSegmentCollection;
            PathFigureCollection segmentPathFigureCollection = new PathFigureCollection();
            segmentPathFigureCollection.Add(segmentPathFigure);
            PathGeometry segmentPathGeometry = new PathGeometry();
            segmentPathGeometry.Figures = segmentPathFigureCollection;

            // Set up the segment's drawing (and hit-testing) path and its Tag.
            pointerArcLeft = new Path();
            pointerArcLeft.Data = segmentPathGeometry;
            pointerArcLeft.Stroke = Brushes.Black;
            pointerArcLeft.StrokeThickness = 1;
            pointerArcLeft.Fill = Brushes.White;
            Canvas.SetLeft(pointerArcLeft, x);
            Canvas.SetTop(pointerArcLeft, y);
			Canvas.SetZIndex(pointerArcLeft, 1);
            Canvas.Children.Add(pointerArcLeft);
        }

        /// <summary>
        /// Allows to draw the pointer of the right arc.
        /// </summary>
        protected void drawPointerArcRight()
        {
            // The radius of the circumcercle.
            double radius = Math.Sqrt(height * height + width * width) / 2;

            EllipseGeometry ellipseGeometry = new EllipseGeometry(new Point(0,0), 7, 7);

            pointerArcRight = new Path();
            pointerArcRight.Data = ellipseGeometry;
            pointerArcRight.Stroke = Brushes.Black;
            pointerArcRight.StrokeThickness = 1;
            pointerArcRight.Fill = Brushes.White;
            Canvas.SetLeft(pointerArcRight, PointOnCircle(radius, arcRight_angleLow, new Point(x, y)).X);
            Canvas.SetTop(pointerArcRight, PointOnCircle(radius, arcRight_angleLow, new Point(x, y)).Y);
            Canvas.SetZIndex(pointerArcRight, 1);
            Canvas.Children.Add(pointerArcRight);
        }


        
        #endregion Drawing functions of arc pointers.

		/// <summary>
		/// Allows to draw the subtype menu button.
		/// </summary>
		protected void drawSubtypeMenubButton()
		{
			// The radius of the circumcercle.
			double radius = Math.Sqrt(height * height + width * width) / 2;

			PathFigureCollection segmentPathFigureCollection = new PathFigureCollection();

			// more's horizontal line 
			PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
			PathFigure segmentPathFigure1 = new PathFigure();

			Point StartPoint = PointOnCircle(radius, 90 - 4, new Point(0, 0));
			Point lineStopPoint1 = PointOnCircle(radius, 90 + 4, new Point(0, 0));
			segmentPathFigure1.StartPoint = StartPoint;
			LineSegment lineSegment1 = new LineSegment(lineStopPoint1, true);	
			pathSegmentCollection.Add(lineSegment1);
			segmentPathFigure1.Segments = pathSegmentCollection;	
			segmentPathFigureCollection.Add(segmentPathFigure1);

			// more's vertical line 
			pathSegmentCollection = new PathSegmentCollection();
			PathFigure segmentPathFigure2 = new PathFigure();

			StartPoint = PointOnCircle(radius - 4, 90, new Point(0, 0));
			lineStopPoint1 = PointOnCircle(radius + 4, 90, new Point(0, 0));
			segmentPathFigure2.StartPoint = StartPoint;
			lineSegment1 = new LineSegment(lineStopPoint1, true);
			pathSegmentCollection.Add(lineSegment1);
			segmentPathFigure2.Segments = pathSegmentCollection;
			segmentPathFigureCollection.Add(segmentPathFigure2);

			// button's cercle
			EllipseGeometry ellipseGeometry = new EllipseGeometry(PointOnCircle(radius, 90, new Point(0, 0)), 7, 7);

			PathGeometry segmentPathGeometry = new PathGeometry();		
			segmentPathGeometry.Figures = segmentPathFigureCollection;
			segmentPathGeometry.AddGeometry(ellipseGeometry);

			// Set up the segment's drawing (and hit-testing) path and its Tag.
			subtypesMenuButton = new Path();
			subtypesMenuButton.Data = segmentPathGeometry;
			subtypesMenuButton.Stroke = Brushes.Black;
			subtypesMenuButton.StrokeThickness = 1;
			subtypesMenuButton.Fill = Brushes.White;
			Canvas.SetLeft(subtypesMenuButton, x);
			Canvas.SetTop(subtypesMenuButton, y);
			Canvas.SetZIndex(subtypesMenuButton, 1);
			Canvas.Children.Add(subtypesMenuButton);
            // adding an handler (for debugging only)
            subtypesMenuButton.MouseDown += new MouseButtonEventHandler(subtypesMenuButton_MouseDown);
            
		}

		#region MouseRegion - DEBUGGING ONLY

		bool objectSelected = false;

        protected virtual void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            objectSelected = true;
            collisionSurface.Opacity = 1;
        }    
        protected virtual void MouseMove(object sender, MouseEventArgs e)
        {
            if (objectSelected)
            {
                Point position = Mouse.GetPosition(Canvas);
                translateWholeObject(position.X , position.Y);

                checkRadius(ReactableObject.ReactableObjectList);             
            }
        }
        protected virtual void MouseUp(object sender, MouseEventArgs e)
        {
            objectSelected = false;
            collisionSurface.Opacity = 0;
        }
        protected virtual void subtypesMenuButton_MouseDown(object sender, MouseEventArgs e)
        {
            CercleFocusMenu reactableMenu = new CercleFocusMenu(Canvas, new Point(x,y), new Path());
            reactableMenu.AddSegmentImage(Drawer.drawingSineWave(), -15, -10);
            reactableMenu.AddSegmentImage(Drawer.drawingSquareWave(), -15, -10);
            reactableMenu.AddSegmentImage(Drawer.drawingSawtoothWave(), -15, -10);
            reactableMenu.AddSegmentImage(Drawer.drawingWhiteNoise(), -15, -10);
            reactableMenu.Show();
            
        }
                
        #endregion MouseRegion - DEBUGGING ONLY

        /// <summary>
        /// This function test if a point touch the central shape.
        /// </summary>
        /// <param name="point">The contact point on the screen.</param>
        /// <returns> boolean value indicate if the shape is hit or not</returns>
        public bool hitTestPoint(Point point)
        {
            bool result = false;
            Point myPoint = geometricShape.RenderTransform.Inverse.Transform(point);
            HitTestResult hitTestResult = VisualTreeHelper.HitTest(geometricShape, myPoint);
            if (hitTestResult != null)
            {
                result = true;
                firstFingerOnObject = true;
                if (timeDoubleClickObject == 0)
                {
                    timeDoubleClickObject = 1;
                    timerDoubleClickObject = new DispatcherTimer(new TimeSpan(1000 * 1000 ), //time interval, timespam( 10^-7/init)
                                            DispatcherPriority.Normal,//priority
                                            delegate
                                            {
                                                timeDoubleClickObject++;
                                                if (timeDoubleClickObject > 6)
                                                {
                                                    timeDoubleClickObject = 0;
                                                    timerDoubleClickObject.Stop();
                                                    timerDoubleClickObject = null;
                                                }
                                            },
                                            geometricShape.Dispatcher);
                }
                else
                {
                    doubleClickObject = true;
                    timeDoubleClickObject = 0;
                }

            }
            return result;
        }

        /// <summary>
        /// This function draw the "collision surface" 
        /// which is able to know when a connection between object is possible
        /// </summary>
        protected void drawingCollisionSurface(int radius)
        {
            EllipseGeometry collisionEllipse = new EllipseGeometry(new Point(0, 0), radius, radius);
            collisionSurfaceRadius = collisionEllipse.RadiusX;
            
            collisionSurface = new Path();
            collisionSurface.Data = collisionEllipse;
            collisionSurface.StrokeThickness = 1;
            collisionSurface.Stroke = Brushes.Black;
            // draw a circle with dash stroke
            collisionSurface.StrokeDashArray = new DoubleCollection(
            new double[] { 4, 3 });

            // the collision surface take the center of the reactable object
            Canvas.SetLeft(collisionSurface, x);
            Canvas.SetTop(collisionSurface, y);

            Canvas.Children.Add(collisionSurface);
        }

		/// <summary>
		/// Allow to update the list objects in the radius.
		/// </summary>
		/// <param name="listAllObjects">The list of objects to check</param>
		public abstract void checkRadius(List<ReactableObject> listAllObjects);


    }
}
