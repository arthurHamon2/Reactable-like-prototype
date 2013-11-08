using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

using System.Windows.Input;

namespace WpfApplication2
{
	/// <summary>
	/// The focus menu class pops up a 'focus + context' menu (initially an 'active surface' which is a circle with a number 
	/// of segments, one for each menu item). The whole of the window is covered by a semitransparent 'film' so that all visual 
	/// elements except the focus item appear dimmed and are effectively inactive.
	/// The focus item's drawing path (if any) may be supplied and is displayed above the active surface.
	/// </summary>
	class CercleFocusMenu : FocusMenu
	{

	/*	protected override void ReduceFocusMenuMode()
		{
			if (focusCanvas.Children.Contains(activeMenuSurface)) focusCanvas.Children.Remove(activeMenuSurface);
			focusCanvas.Children.Add(activeMenuSurface);
			removeSegment(numberMaxReduceSegment, numberOfMenuItemsShow);
		}


		protected override void ExtendFocusMenuMode()
		{
			showSegments(numberOfMenuItemsShow, numberOfMenuItems);
			numberOfMenuItemsShow = numberOfMenuItems;
		}
		*/
		
		public CercleFocusMenu(Canvas _focusCanvas, Point _focusPoint, Path _focusPath)
			: base(_focusCanvas, _focusPoint, _focusPath)
	   {
		}
		/// <summary>
		/// The radius of a simple circular surface (in this example case).
		/// </summary>
		private const double activeSurfaceRadius = 50; // Default value.

		public override void SetUpMenu()
		{
		   #region Create the menu's active surface drawing path.
			activeMenuSurface = new Path();
			activeMenuSurface.Data = new EllipseGeometry(focusPoint, activeSurfaceRadius + 5, activeSurfaceRadius + 5);
			activeMenuSurface.Stroke = Brushes.Black;
			activeMenuSurface.StrokeThickness = 5;
			activeMenuSurface.Fill = Brushes.LightBlue; // So we will notice any gaps between segments (for example).
			activeMenuSurface.Opacity = 0.1;

			//Size activeSurfaceSize = new Size(activeSurfaceRadius, activeSurfaceRadius);
			#endregion Create the menu's active surface drawing path.

			#region Create the segments.
			// Must have at least two segments otherwise (a) we would need a segment of more 
			// than 180 degrees, and(b) it's not a menu if there's only one choice!
			if (segmentImagesInfo.Count < 2)
			  throw new ArgumentOutOfRangeException("FocusMenu must have a minimum of two menu items.");
			else
				numberOfMenuItems = segmentImagesInfo.Count;

			// Calculate the angle subtended by each segment.
			double segmentAngle = 360 / numberOfMenuItems; // Note number of menu items must be at least 2.

			// Set up segment drawing for each menu item.
			for (int segmentNumber = 0; segmentNumber < numberOfMenuItems; segmentNumber++)
			{
				// Set up the collection that holds the line and arc that define a segment.
				PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();

				// Need a separate path figure for each segment.
				PathFigure segmentPathFigure = new PathFigure();

				// Calculate the start position and stop position of the segment's arc. Note that the segments are rotated
				// anticlockwise by half of their subtended angle so that the menu item's image is centered under its 
				// segment (and therefore hit testing area).
				Point arcStartPoint = PointOnCircle(activeSurfaceRadius, (segmentNumber - 0.5) * segmentAngle, focusPoint);
				Point arcStopPoint = PointOnCircle(activeSurfaceRadius,	 (segmentNumber + 0.5) * segmentAngle, focusPoint);

				// The segment drawing starts at the focus point of the focus item.
				segmentPathFigure.StartPoint = focusPoint;

				// Line from focus point to start of this segment's arc.
				LineSegment lineSegment = new LineSegment(arcStartPoint, false);

				// Arc from this segment's arc start point to its arc stop point.
				ArcSegment arcSegment = new ArcSegment(arcStopPoint, new Size(activeSurfaceRadius, activeSurfaceRadius),
																														 0, false, SweepDirection.Clockwise, true);

				// Add the line and the arc to this segment's collection. 
				pathSegmentCollection.Add(lineSegment);
				pathSegmentCollection.Add(arcSegment);

				// Set the path segment collection into the path figure for this segment.
				segmentPathFigure.Segments = pathSegmentCollection;

				// Force the path figure to draw a line from the end of the arc to the focus item's focus point.
				segmentPathFigure.IsClosed = true;

				PathFigureCollection segmentPathFigureCollection = new PathFigureCollection();
				segmentPathFigureCollection.Add(segmentPathFigure);

				PathGeometry segmentPathGeometry = new PathGeometry();
				segmentPathGeometry.Figures = segmentPathFigureCollection;

				// Set up the segment's drawing (and hit-testing) path and its Tag.
				Path segmentPath = new Path();
				segmentPath.Stroke = Brushes.Black;
				segmentPath.StrokeThickness = 2;
				segmentPath.Fill = Brushes.Blue;
				segmentPath.Opacity = segmentOpacicity;
				segmentPath.Data = segmentPathGeometry;

				// The selection's value to be passed back to the focus item via an event.
				segmentPath.Tag = segmentNumber;

				// Add this segment's path into the collection.
				segmentPaths.Add(segmentPath);

				// Calculate the position of the segment's image.
				Point segmentImagePoint = PointOnCircle(activeSurfaceRadius * 0.67, segmentNumber * segmentAngle, focusPoint);

				Canvas.SetLeft(segmentImagesInfo[segmentNumber].image, 
											 segmentImagePoint.X +  segmentImagesInfo[segmentNumber].translateX);
				Canvas.SetTop(segmentImagesInfo[segmentNumber].image,
											segmentImagePoint.Y + segmentImagesInfo[segmentNumber].translateY);
			}
			#endregion Create the segments.
		}

		Point PointOnCircle(double _radius, double _angleInDegrees, Point _origin)
		{
			// Convert from degrees to radians via multiplication by PI/180         
			double x = /*(double)*/(_radius * Math.Cos(_angleInDegrees * Math.PI / 180.0)) + _origin.X;
			double y = /*(double)*/(_radius * Math.Sin(_angleInDegrees * Math.PI / 180.0)) + _origin.Y;

			return new Point(x, y);
		}
	}
		
}
