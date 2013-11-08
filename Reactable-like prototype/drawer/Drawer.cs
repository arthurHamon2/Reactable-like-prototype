using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WpfApplication2.drawer
{
	public class Drawer : Page
	{
		public static Image drawingSineWave()
		{
			PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
			PathFigureCollection segmentPathFigureCollection = new PathFigureCollection();

			PathFigure segmentPathFigure = new PathFigure();

			segmentPathFigure = new PathFigure();
			segmentPathFigure = new PathFigure();
			segmentPathFigure.IsFilled = false;
			segmentPathFigure.IsClosed = false;

			Point StartPoint = new Point(0, 20);
			Point point1 = new Point(15, 20);
			Point point2 = new Point(30, 20);

			segmentPathFigure.StartPoint = StartPoint;

			ArcSegment lineSegment1 = new ArcSegment(point1, new Size(7.5, 7.5), 0, false, SweepDirection.Clockwise, true);
			ArcSegment lineSegment2 = new ArcSegment(point2, new Size(7.5, 7.5), 0, false, SweepDirection.Counterclockwise, true);

			pathSegmentCollection.Add(lineSegment1);
			pathSegmentCollection.Add(lineSegment2);
			segmentPathFigure.Segments = pathSegmentCollection;
			segmentPathFigureCollection.Add(segmentPathFigure);

			PathGeometry segmentPathGeometry = new PathGeometry();
			segmentPathGeometry.Figures = segmentPathFigureCollection;

			//
			// Create the Geometry to draw.
			//
			GeometryGroup geometryGroup = new GeometryGroup();
			geometryGroup.Children.Add(segmentPathGeometry);

			//
			// Create a GeometryDrawing.
			//
			GeometryDrawing aGeometryDrawing = new GeometryDrawing();
			aGeometryDrawing.Geometry = geometryGroup;

			// Paint the drawing with a gradient.
			aGeometryDrawing.Brush =
				new LinearGradientBrush(
					Colors.Blue,
					Color.FromRgb(204, 204, 255),
					new Point(0, 0),
					new Point(1, 1));

			// Outline the drawing with a solid color.
			aGeometryDrawing.Pen = new Pen(Brushes.Black, 2);

			//
			// Use a DrawingImage and an Image control
			// to display the drawing.
			//
			DrawingImage geometryImage = new DrawingImage(aGeometryDrawing);

			// Freeze the DrawingImage for performance benefits.
			geometryImage.Freeze();

			Image anImage = new Image();
			anImage.Source = geometryImage;
			anImage.HorizontalAlignment = HorizontalAlignment.Left;

			return anImage;
		}

		public static Image drawingSawtoothWave()
		{
			PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
			PathFigureCollection segmentPathFigureCollection = new PathFigureCollection();

			PathFigure segmentPathFigure = new PathFigure();

			segmentPathFigure = new PathFigure();
			segmentPathFigure.IsFilled = false;
			segmentPathFigure.IsClosed = false;

			Point StartPoint = new Point(0, 20);
			Point point1 = new Point(20, 0);
			Point point2 = new Point(20, 20);

			segmentPathFigure.StartPoint = StartPoint;

			LineSegment lineSegment1 = new LineSegment(point1, true);
			LineSegment lineSegment2 = new LineSegment(point2, true);

			pathSegmentCollection.Add(lineSegment1);
			pathSegmentCollection.Add(lineSegment2);
			segmentPathFigure.Segments = pathSegmentCollection;
			segmentPathFigureCollection.Add(segmentPathFigure);

			PathGeometry segmentPathGeometry = new PathGeometry();
			segmentPathGeometry.Figures = segmentPathFigureCollection;

			//
			// Create the Geometry to draw.
			//
			GeometryGroup geometryGroup = new GeometryGroup();
			geometryGroup.Children.Add(segmentPathGeometry);

			//
			// Create a GeometryDrawing.
			//
			GeometryDrawing aGeometryDrawing = new GeometryDrawing();
			aGeometryDrawing.Geometry = geometryGroup;

			// Paint the drawing with a gradient.
			aGeometryDrawing.Brush =
				new LinearGradientBrush(
					Colors.Blue,
					Color.FromRgb(204, 204, 255),
					new Point(0, 0),
					new Point(1, 1));

			// Outline the drawing with a solid color.
			aGeometryDrawing.Pen = new Pen(Brushes.Black, 2);

			//
			// Use a DrawingImage and an Image control
			// to display the drawing.
			//
			DrawingImage geometryImage = new DrawingImage(aGeometryDrawing);

			// Freeze the DrawingImage for performance benefits.
			geometryImage.Freeze();

			Image anImage = new Image();
			anImage.Source = geometryImage;
			anImage.HorizontalAlignment = HorizontalAlignment.Left;

			return anImage;
		}

		public static Image drawingSquareWave()
		{
			PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
			PathFigureCollection segmentPathFigureCollection = new PathFigureCollection();

			PathFigure segmentPathFigure = new PathFigure();

			segmentPathFigure = new PathFigure();
			segmentPathFigure.IsFilled = false;
			segmentPathFigure.IsClosed = false;

			Point StartPoint = new Point(0, 20);
			Point point1 = new Point(0, 0);
			Point point2 = new Point(20, 0);
			Point point3 = new Point(20, 20);
			Point point4 = new Point(25, 20);

			segmentPathFigure.StartPoint = StartPoint;
			

			LineSegment lineSegment1 = new LineSegment(point1, true);
			LineSegment lineSegment2 = new LineSegment(point2, true);
			LineSegment lineSegment3 = new LineSegment(point3, true);
			LineSegment lineSegment4 = new LineSegment(point4, true);

			pathSegmentCollection.Add(lineSegment1);
			pathSegmentCollection.Add(lineSegment2);
			pathSegmentCollection.Add(lineSegment3);
			pathSegmentCollection.Add(lineSegment4);
			segmentPathFigure.Segments = pathSegmentCollection;
			segmentPathFigureCollection.Add(segmentPathFigure);

			PathGeometry segmentPathGeometry = new PathGeometry();
			segmentPathGeometry.Figures = segmentPathFigureCollection;

			//
			// Create the Geometry to draw.
			//
			GeometryGroup geometryGroup = new GeometryGroup();
			geometryGroup.Children.Add(segmentPathGeometry);

			//
			// Create a GeometryDrawing.
			//
			GeometryDrawing aGeometryDrawing = new GeometryDrawing();
			aGeometryDrawing.Geometry = geometryGroup;

			// Paint the drawing with a gradient.
			aGeometryDrawing.Brush =
				new LinearGradientBrush(
					Colors.Blue,
					Color.FromRgb(204, 204, 255),
					new Point(0, 0),
					new Point(1, 1));

			// Outline the drawing with a solid color.
			aGeometryDrawing.Pen = new Pen(Brushes.Black, 2);

			//
			// Use a DrawingImage and an Image control
			// to display the drawing.
			//
			DrawingImage geometryImage = new DrawingImage(aGeometryDrawing);

			// Freeze the DrawingImage for performance benefits.
			geometryImage.Freeze();

			Image anImage = new Image();
			anImage.Source = geometryImage;
			anImage.HorizontalAlignment = HorizontalAlignment.Left;

			return anImage;
		}

        public static Image drawingWhiteNoise()
        {
            PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
            PathFigureCollection segmentPathFigureCollection = new PathFigureCollection();

            PathFigure segmentPathFigure = new PathFigure();

            segmentPathFigure = new PathFigure();
            segmentPathFigure.IsFilled = false;
            segmentPathFigure.IsClosed = false;

            Point StartPoint = new Point(0, 5);
            Point point1 = new Point(5, 0);
            Point point2 = new Point(10, 10);
            Point point3 = new Point(15, -10);
            Point point4 = new Point(20, 0);

            segmentPathFigure.StartPoint = StartPoint;


            LineSegment lineSegment1 = new LineSegment(point1, true);
            LineSegment lineSegment2 = new LineSegment(point2, true);
            LineSegment lineSegment3 = new LineSegment(point3, true);
            LineSegment lineSegment4 = new LineSegment(point4, true);

            pathSegmentCollection.Add(lineSegment1);
            pathSegmentCollection.Add(lineSegment2);
            pathSegmentCollection.Add(lineSegment3);
            pathSegmentCollection.Add(lineSegment4);
            segmentPathFigure.Segments = pathSegmentCollection;
            segmentPathFigureCollection.Add(segmentPathFigure);

            PathGeometry segmentPathGeometry = new PathGeometry();
            segmentPathGeometry.Figures = segmentPathFigureCollection;

            //
            // Create the Geometry to draw.
            //
            GeometryGroup geometryGroup = new GeometryGroup();
            geometryGroup.Children.Add(segmentPathGeometry);

            //
            // Create a GeometryDrawing.
            //
            GeometryDrawing aGeometryDrawing = new GeometryDrawing();
            aGeometryDrawing.Geometry = geometryGroup;

            // Paint the drawing with a gradient.
            aGeometryDrawing.Brush =
                new LinearGradientBrush(
                    Colors.Blue,
                    Color.FromRgb(204, 204, 255),
                    new Point(0, 0),
                    new Point(1, 1));

            // Outline the drawing with a solid color.
            aGeometryDrawing.Pen = new Pen(Brushes.Black, 2);

            //
            // Use a DrawingImage and an Image control
            // to display the drawing.
            //
            DrawingImage geometryImage = new DrawingImage(aGeometryDrawing);

            // Freeze the DrawingImage for performance benefits.
            geometryImage.Freeze();

            Image anImage = new Image();
            anImage.Source = geometryImage;
            anImage.HorizontalAlignment = HorizontalAlignment.Left;

            return anImage;
        }
	}
}
