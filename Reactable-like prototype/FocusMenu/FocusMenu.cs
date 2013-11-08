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
	abstract class FocusMenu
	{

		#region Context.
		/// <summary>
		/// The canvas that the focus item (if any) has been drawn on and that the menu is to be drawn on.
		/// </summary>
		protected Canvas focusCanvas;

		/// <summary>
		/// The (x, y) coordinates upon which the menu is to center its focus. This is the focus point associated 
		/// with the focus item right-clicked upon (if any) or it is the coordinates of a right-click on the focus canvas.
		/// </summary>
		protected Point focusPoint;

		/// <summary>
		/// The drawing path of the item right-clicked upon (if any).
		/// </summary>
		protected Path focusPath;

		/// <summary>
		/// Simple class to hold a segment's image and the translation required to be used by the caller.
		/// </summary>
		internal class SegmentImageInfo
		{
			public Image image;
			public double translateX;
			public double translateY;

			public SegmentImageInfo(Image _image, double _translateX, double _translateY)
			{
				image = _image;
				translateX = _translateX;
				translateY = _translateY;
			}
		}

		/// <summary>
		/// The images to be displayed (one per menu segment).
		/// </summary>
		protected List<SegmentImageInfo> segmentImagesInfo;

		/// <summary>
		/// Calculated from the number of menu segment images supplied.
		/// </summary>
		protected int numberOfMenuItems = -1; // Illegal value.
		protected int numberOfMenuItemsShow = -1; // Illegal value.
		#endregion Context.

		#region Menu active surface (and masking film) drawing information.

		/// <summary>
		/// Drawing and hit-testing path for the menu's active surface.
		/// </summary>
		protected Path activeMenuSurface;

		/// <summary>
		/// Part-transparent film to partially mask everything except the subject item.
		/// The user clicking on the film tells us to exit the focus menu mode.
		/// </summary>
		private Rectangle film;

		// One path per segment.
		public List<Path> segmentPaths;

		protected double segmentOpacicity = 0.3;

		#endregion Menu active surface (and masking film) drawing information.

		/// <summary>
		/// Drawing and interaction logic for the focus menu that appears when the subject element is right-clicked.
		/// </summary>
		/// <param name="_focusCanvas">The canvas that the focus item (if any) has been drawn on and that the menu is to be drawn on.</param>
		/// <param name="_focusPoint">The (x, y) coordinates upon which the menu is to center its focus. This is the focus point associated 
		/// with the focus item right-clicked upon (if any) or it is the coordinates of a right-click on the focus canvas.</param>
		/// <param name="_focusPath">The path of the item right-clicked upon.</param>
		public FocusMenu(Canvas _focusCanvas, Point _focusPoint, Path _focusPath)
		{
			//Debug.WriteLine(string.Format("FocusMenu constructor - focusItemPath.Tag: {0}",
			//                              _partStaveTimeLineItem.Path.Tag));
			Debug.WriteLine("AbstractFocusMenu constructor");

			#region Context.
			focusCanvas = _focusCanvas;
			focusPoint = _focusPoint;
			focusPath = _focusPath;
			#endregion Context.

			// Set up storage for the drawing paths and images for each menu segment.
			segmentPaths = new List<Path>();
			segmentImagesInfo = new List<SegmentImageInfo>();
		}

		public void Show()
		{
			Debug.WriteLine("AbstractFocusMenu show");
			SetUpMenu();
			EnterFocusMenuMode();
		}

		/// <summary>
		/// Caller supplies an image to be displayed in a menu segment.
		/// </summary>
		/// <param name="_segmentImage">The image to be displayed in a menu segment.</param>
		public void AddSegmentImage(Image _segmentImage, double _translateX, double _translateY)
		{
            Canvas.SetZIndex(_segmentImage, 1);
			segmentImagesInfo.Add(new SegmentImageInfo(_segmentImage, _translateX, _translateY));
		}

		/// <summary>
		/// Creates the active menu surface, segments and sets up the associated hit testing.
		/// </summary>
		abstract public void SetUpMenu();

		protected const int numberMaxReduceSegment = 5;  //the number of segment in the reduce menu

		#region Enter and Exit Focus Menu Mode.
		/// <summary>
		/// Attach mouse handlers to segment paths, focus path (if any) and active menu surface and add all these paths to the canvas.
		/// </summary>
		protected void EnterFocusMenuMode()
		{

			if (numberOfMenuItemsShow == -1)
			{
				numberOfMenuItemsShow = numberOfMenuItems;
			}
			showSegments(numberOfMenuItemsShow);
		}

		protected void showSegments(int _max)
		{

			numberOfMenuItemsShow = _max;
			// Show active surface.
			if (focusCanvas.Children.Contains(activeMenuSurface)) focusCanvas.Children.Remove(activeMenuSurface);
			focusCanvas.Children.Add(activeMenuSurface);
			// Show segments after attaching handlers.
			for (int segmentIndex = 0; segmentIndex < _max; segmentIndex++)
			{
				// Show segment's image (before adding the segment's hit testing path on top of the image).
				focusCanvas.Children.Add(segmentImagesInfo[segmentIndex].image);

				Path segmentPath = segmentPaths[segmentIndex];

				// Set up handler for any mouse down event.
				segmentPath.MouseDown += new MouseButtonEventHandler(segmentPath_MouseDown);
				segmentPath.MouseEnter += new MouseEventHandler(segmentPath_MouseEnter);
				segmentPath.MouseLeave += new MouseEventHandler(segmentPath_MouseLeave);
				// Ensure that each segment path is higher in the z-order than the active menu surface and segment images.
				

				// Place each of the path segments onto the focus item's canvas to have it drawn.
                Canvas.SetZIndex(segmentPath, 1); // now, we can click on the whole segment even if you click on the drawing note
                focusCanvas.Children.Add(segmentPath);
			}

			// If we have a focus path then arrange for hits on it to be handled.
			if (focusPath != null)
			{
				// Create and connect up a handler for the focus item's path so that we stop mouse messages from reaching it.
				focusPath.PreviewMouseDown += new MouseButtonEventHandler(focusPath_PreviewMouseDown);
				focusPath.PreviewMouseUp += new MouseButtonEventHandler(focusPath_PreviewMouseDown);

				// Ensure the subject item appears above the film layer, the active menu surface and the segments.
				focusCanvas.Children.Remove(focusPath);
				focusCanvas.Children.Add(focusPath);
			}
		}

		/// <summary>
		/// Detach mouse handlers from film path, segment paths, focus path (if any) and active menu surface 
		/// and remove all of these paths from the canvas.
		/// </summary>
		public void ExitFocusMenuMode()
		{
			removeSegments();

			// Remove the active surface from the canvas.
			focusCanvas.Children.Remove(activeMenuSurface);

			// Remove the film from the canvas.
			focusCanvas.Children.Remove(film);

			// Disconnect the focus menu's active surface and film mouse down handlers. 
			//film.MouseDown -= film_MouseDown;
			activeMenuSurface.MouseDown -= activeMenuSurface_MouseDown;

			// If we have a focus path then disconect its handlers.
			if (focusPath != null)
			{
				// Remove our mouse down and up handlers so that the focus item (if any) can respond to these messages again.
				focusPath.PreviewMouseDown -= focusPath_PreviewMouseDown;
				focusPath.PreviewMouseUp -= focusPath_PreviewMouseDown;
			}
		}

        /// <summary>
        /// remove all the segments on the canvas
        /// </summary>
		protected void removeSegments()
		{
			// Remove the segments paths and images from the canvas.
			for (int segmentIndex = 0; segmentIndex < segmentPaths.Count; segmentIndex++)
			{
				// Do I have to cast off all of the inner objects to get them garbaged collected... [Jay ToDo!]
				// Remove the segment's path.
				focusCanvas.Children.Remove(segmentPaths[segmentIndex]);

				// Remove the segment's image;
				focusCanvas.Children.Remove(segmentImagesInfo[segmentIndex].image);
			}
		}

        /// <summary>
        /// fake function to generate some blank image
        /// </summary>
        public void addImageFocusMenu()
        {
            // fake adding image to the menu for the moment
            for (int imageNumber = 0; imageNumber < 4; imageNumber++)
            {
                // Add the display paths for the menu items.
                AddSegmentImage(new Image(), -11, -61);
            }
        }
		#endregion Enter and Exit Focus Menu Mode.

		#region Handlers.
		/// <summary>
		/// Handler for mouse down events from any of the segment paths (the Tag field 
		/// of the sender path holds that path's index in the segment path array).
		/// </summary>
		/// <param name="sender">The path of the menu segment clicked upon.</param>
		/// <param name="e">Not used.</param>
		protected void segmentPath_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Path segmentPath = sender as Path;

			Debug.WriteLine(String.Format("segmentPath_MouseDown - Tag: {0}", (int)segmentPath.Tag));

			RaiseFocusMenuSelectionEvent((int)segmentPath.Tag);

			ExitFocusMenuMode();

		}

		protected void segmentPath_MouseEnter(object sender, EventArgs e)
		{
			Path segmentPath = sender as Path;

			segmentPath.Opacity = segmentOpacicity + 0.2;
		}

		protected void segmentPath_MouseLeave(object sender, EventArgs e)
		{
			Path segmentPath = sender as Path;

			segmentPath.Opacity = segmentOpacicity;
		}

		void film_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Debug.WriteLine("film_MouseDown");

			ExitFocusMenuMode();

			// Inform the focus item that the menu has been exited but no selection has been made (value -1).
			RaiseFocusMenuSelectionEvent(-1);
		}

		/// <summary>
		/// A mouse down on the menu's active surface should not happen (provided that the menu item 
		/// segments cover this surface completely--could be used for navigation controls...?).
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void activeMenuSurface_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Debug.WriteLine("activeMenuSurface_MouseDown: TAG {0}");

			ExitFocusMenuMode();

			// Inform the subject item that the menu has been exited and pass it the selected item's index.
			RaiseFocusMenuSelectionEvent(-1);
		}

		protected void focusPath_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			//Debug.WriteLine("focusPath_PreviewMouseDown");

			e.Handled = true;
		}
		#endregion Handlers.

		#region Focus menu event.
		/// <summary>
		/// The EventHandler type is the .NET predefined 'default' delegate type except that we use our own 
		/// FocusMenuEventArgs class (derived from the EventArgs class) to pass data to the subscriber's handler.
		/// </summary>
		public event EventHandler<FocusMenuSelectionEventArgs> FocusMenuSelectionEvent;

		/// <summary>
		/// When this focus menu's interaction logic has determined which menu item has been
		/// selected it calls this routine to raise the event and thereby inform the focus item.
		/// </summary>
		/// <param name="_selection">The one-based index of the selected menu item; zero means no selection made.</param>
		public void RaiseFocusMenuSelectionEvent(int _selection)
		{
			if (FocusMenuSelectionEvent != null) // Ensure that there are one or more event handlers to call.
			{
				// Create an object of our FocusMenuEventArgs class with the value to be 
				// passed back to the subscriber as its constructor argument.
				FocusMenuSelectionEventArgs focusMenuSelectionEventArgs = new FocusMenuSelectionEventArgs(_selection);

				// Invoke all of the subscriber event handlers.
				FocusMenuSelectionEvent(this, focusMenuSelectionEventArgs);
			}
		}
		#endregion Focus menu event.

        /// <summary>
        /// Getter for the Active Menu Surface
        /// </summary>
        /// <returns> The current active surface of the menu </returns>
        public Path getActiveMenuSurface()
        {
            return activeMenuSurface;
        }

        /// <summary>
        /// Getter for the segment paths
        /// </summary>
        /// <returns> The current segment paths </returns>
        public List<Path> getSegmentPaths()
        {
            return segmentPaths;
        }
	}
	#region Focus menu selection event arguments.
	/// <summary>
	/// The .NET class EventArgs does not contain data. We derive from it and 'add' the data fields
	/// we require using one (or more) constructor parameter(s) as shown: 
	/// </summary>
	public class FocusMenuSelectionEventArgs : EventArgs
	{
		// Our data field.
		protected int selectedFocusMenuItem;
		public int SelectedFocusMenuItem
		{
			get { return selectedFocusMenuItem; }
			//set { selectedFocusMenuItem = value; }
		}

		// Construct a FocusMenuEventArgs instance around our data.
		public FocusMenuSelectionEventArgs(int _selectedFocusMenuItem)
		{
			selectedFocusMenuItem = _selectedFocusMenuItem;
		}
	}
	#endregion Focus menu selection event arguments.


}
