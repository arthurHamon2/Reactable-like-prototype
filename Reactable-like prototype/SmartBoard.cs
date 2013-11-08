using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Diagnostics;
using SBSDKComWrapperLib;
using System.Windows.Threading;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using WpfApplication2.reactableObjects;
using System.Windows.Shapes;


namespace WpfApplication2
{
    class SmartBoard
    {
        #region Variables declaration.
        private Canvas canvas;
        private static int traceCount = 0; // Used for the debugging

        // Menu of the reactable interface
        private RectangleFocusMenu reactableMenu;

        private Oscillator osci;

        private List<Brush> colorList;

        // values of the norms of vector from one finger to another when pinching
        private double fingersAngle, secondFingersAngle;
        // Space between fingers
        private Vector gap, previousGap, secondGap, previousSecondGap, firstTouchRotationCenter, secondTouchRotationCenter;

        // SDK related.
        private ISBSDKBaseClass2 sbsdk;
        private _ISBSDKBaseClass2Events_Event sbsdkEvents;
        private _ISBSDKBaseClass2HoverEvents_Event sbsdkHoverEvents;

        // Some coordinates are stored in Vectors :
        private Vector firstTouch, secondTouch, // the coordinates of the contact point of the fingers
            firstMove, secondMove, // same but contact point when moving fingers
            previousFirstMove, previousSecondMove; // same as firstMove/secondMove but previous
        private Boolean itemSelected = false;

        private InputReactableObject objectSelected = null;
		public static Output output;

        /// <summary>
        /// default value to put the focusMenu on the right position at starting (TO DO: make a constant of this value!)
        /// </summary>
        public static Point LeftClosePoint = new Point(-30, 150);

        #endregion Variables declaration.


        public SmartBoard(Canvas _canvas)
        {
            #region Instantation of the SDK
            try
            {
                sbsdk = new SBSDKBaseClass2();
                Trace("[SDK] SBSDKBaseClass2Class instantiated.");
            }
            catch (Exception)
            {
                Trace("[SDK] SBSDKBaseClass2Class instantation FAILED !");
            }
            if (sbsdk != null)
            {
                try
                {
                    sbsdkEvents = (_ISBSDKBaseClass2Events_Event)sbsdk;
                    sbsdkHoverEvents = (_ISBSDKBaseClass2HoverEvents_Event)sbsdk;
                }
                catch (Exception)
                {
                    Trace("[SDK] Event/Hover instantation FAILED");
                }
            }
            #endregion Instantiation of the SDK


            #region Initialization.
            canvas = _canvas;
            Trace("SmartBoard(canvas) entered.");

            output = new Output(canvas);
            // Here, initialization of the first set of object.
            reactableMenu = new RectangleFocusMenu(canvas, LeftClosePoint, new Path(), true);
            reactableMenu.addImageFocusMenu();
            //reactableMenu.Show();


            // new reactable object just to see what happened 
			Filter fil1 = new Filter(canvas, 750, 400, 75);
			Filter fil2 = new Filter(canvas, 850, 500, 75);
            Filter fil3 = new Filter(canvas, 1000, 550, 75);
            Filter fil4 = new Filter(canvas, 350, 300, 75);
            
            Oscillator osc1 = new Oscillator(canvas, 200, 150, 75);
            Oscillator osc2 = new Oscillator(canvas, 1200, 600, 75);

            OscillatorController control = new OscillatorController(canvas, 75, 75, 75);
            OscillatorController control2 = new OscillatorController(canvas, 75, 200, 75);
           /* OscillatorController control3 = new OscillatorController(canvas, 75, 200, 75);
            OscillatorController control4 = new OscillatorController(canvas, 75, 200, 75);
            OscillatorController control5 = new OscillatorController(canvas, 75, 200, 75);*/

            //ReactableObject.ReactableObjectList.Add(fil1);

            #endregion Initialization.


            canvas.MouseDown += new System.Windows.Input.MouseButtonEventHandler(canvas_MouseDown);



            #region Event Handlers section.
            if (sbsdkEvents != null)
            {
                // Adding the handlers for the touchscreen
                sbsdkEvents.OnXYDown += new SBSDKComWrapperLib._ISBSDKBaseClass2Events_OnXYDownEventHandler(OnXYDown);
                sbsdkEvents.OnXYMove += new SBSDKComWrapperLib._ISBSDKBaseClass2Events_OnXYMoveEventHandler(OnXYMove);
                sbsdkEvents.OnXYUp += new SBSDKComWrapperLib._ISBSDKBaseClass2Events_OnXYUpEventHandler(OnXYUp);
            }
            #endregion Event Handlers section.

            #region SDK COM Stuff
            if (sbsdk != null)
            {
                HwndSource hwndSource = PresentationSource.FromVisual(canvas) as HwndSource;
                sbsdk.SBSDKAttach(hwndSource.Handle.ToInt32(), false);
                sbsdk.SBSDKSetSendMouseEvents(hwndSource.Handle.ToInt32(), SBSDKComWrapperLib._SBCSDK_MOUSE_EVENT_FLAG.SBCME_NEVER, -1);
                Trace("SmartBoard(canvas) entered2.");
            }
            #endregion SDK COM Stuff
        }

        private void canvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Trace("[WPF] canvas_MouseDown() is called.");
        }

        /// <summary>
        /// This function calls the OnXYDownUI function on the main thread when
        /// a finger touches the screen.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="PointerID"></param>
        private void OnXYDown(int x, int y, int z, int PointerID)
        {
            // This handler is raised when a finger touches the screen. The PointerID is 0 if there is no touch,
            // 1 if there is one touch, and above if there are two touches (windows gives the value 257)
            String s = String.Format("[SDK] OnXYDown() called:(x = {0}, y = {1}, z = {2}), " + "pointer Id = {3} [thread: {4}]", x, y, z, PointerID, Thread.CurrentThread.ManagedThreadId);
            Trace(s);
            // The following code will run in the main thread so the variables are shared
            canvas.Dispatcher.Invoke(DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                OnXYDownUI(x, y, z, PointerID);
                            }
                                                ));
        }

        /// <summary>
        /// This function calls the OnXYMoveUI function on the main thread
        /// when a finger moves on the screen.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="PointerID"></param>
        private void OnXYMove(int x, int y, int z, int PointerID)
        {
            // raised when a finger moves (that means all the time since your finger is always moving
            // and the DViT technology very sensitive.
            String s = String.Format("[SDK] OnXYMove() called:(x = {0}, y = {1}, z = {2}), " + "pointer Id = {3} [thread: {4}]", x, y, z, PointerID, Thread.CurrentThread.ManagedThreadId);
            Trace(s);
            canvas.Dispatcher.Invoke(DispatcherPriority.Normal,
                            new Action(
                                                    () =>
                                                    {
                                                        OnXYMoveUI(x, y, z, PointerID);
                                                    }
                                                    ));
        }

        /// <summary>
        /// The function is called when a touch is released,
        /// when a finger doesn't touch the screen anymore.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="PointerID"></param>
        private void OnXYUp(int x, int y, int z, int PointerID)
        {
            canvas.Dispatcher.Invoke(DispatcherPriority.Normal,
                            new Action(
                                                    () =>
                                                    {
                                                        OnXYUpUI(x, y, z, PointerID);
                                                    }
                                                    ));
        }

        private void OnXYDownUI(int x, int y, int z, int PointerID)
        {
            firstTouch.X = x;
            firstTouch.Y = y;
            previousFirstMove.X = firstTouch.X;
            previousFirstMove.Y = firstTouch.Y;
            Point fingerPosition = new Point(firstTouch.X, firstTouch.Y);

            #region menu events when the finger is down
            // colours re-initialisation for segmentPath
            foreach (Path segmentPath in reactableMenu.segmentPaths)
            {
                segmentPath.Fill = Brushes.Blue; // the original colour   
            }
            // re-initialize the item selected
            itemSelected = false;

            //if the menu was hitting and its state was "close" (the menu must be closed here)
            if (HitTestPoint(reactableMenu.getActiveMenuSurface(), fingerPosition)
                && reactableMenu.menuIsOpen == false)
            {
                reactableMenu.openMenu(); // open the menu
                Trace("Reactable menu is opened now");
            }
            //if the menu was not hitting or the user hit something else, the menu will be closed (the menu must be open here)
            else if (!HitTestPoint(reactableMenu.getActiveMenuSurface(), fingerPosition)
                && reactableMenu.menuIsOpen == true)
            {
                reactableMenu.closeMenu(); //close the menu
                Trace("Reactable menu is closed now");
            }
            // this statement check if the user would like to select an item in the menu
            else if ((HitTestPoint(reactableMenu.getActiveMenuSurface(), fingerPosition)
                && reactableMenu.menuIsOpen == true))
            {
                //to select an item is summarize to highlight it which was the user's choice
                foreach (Path segmentPath in reactableMenu.segmentPaths)
                {
                    if (HitTestPoint(segmentPath, fingerPosition))//if the segment is touched...
                    {
                        segmentPath.Fill = Brushes.RoyalBlue; // ... the colour's path will change    
                        itemSelected = true;
                        Trace("SELECTED!");
                    }
                }
            }
            #endregion menu events when the finger is down

            #region reactable objects events
            // We check for each reactable objects on the canvas...
            foreach (InputReactableObject reactableObject in ReactableObject.ReactableObjectList)
                //... if the user try to select an object
                if (reactableObject.hitTestPoint((Point)(fingerPosition - new Point(Canvas.GetLeft(reactableObject.geometricShape),
                    Canvas.GetTop(reactableObject.geometricShape)))))
              // if(HitTestPoint(reactableObject.geometricShape,(Point)(fingerPosition - new Point(Canvas.GetLeft(reactableObject.geometricShape),
                //    Canvas.GetTop(reactableObject.geometricShape)))))
                {
                    reactableObject.firstFingerOnObject = true;
                    objectSelected = reactableObject;
                    objectSelected.collisionSurface.Opacity = 1;
                    // if an object is selected then continuing the hit test point is useless.
                    if (objectSelected != null)
                        break;

                    Trace("a reactable object is selected");
                }

            #endregion reactable objects events
        }

        private void OnXYMoveUI(int x, int y, int z, int PointerID)
        {
            // Coordinates of the move are stored in the appropriate Vector.
            firstMove.X = x;
            firstMove.Y = y;

            #region reactable objects events
            if (objectSelected != null)
            {
                // Implementation of a "level-off" so the shape doesn't move just because the
                // finger(s) is (are) a bit shaking.
                if (objectSelected.doubleClickObject == false)
                {
                    if (firstMove.X - previousFirstMove.X > 1 || firstMove.X - previousFirstMove.X < -1 ||
                        firstMove.Y - previousFirstMove.Y > 1 || firstMove.Y - previousFirstMove.Y < -1)
                    {
                        objectSelected.translateWholeObject(objectSelected.getX() + (firstMove.X - previousFirstMove.X),
                        objectSelected.getY() + (firstMove.Y - previousFirstMove.Y));
                        //objectSelected.drawingConnectionWithOutput(output);
                        foreach (InputReactableObject reactableObject in ReactableObject.ReactableObjectList)
                        {
                            if (objectSelected != reactableObject)
                            {
                                //canvas.Children.Remove(reactableObject.objectLink);
                                //objectSelected.drawingConnectionWithObject(reactableObject);
                            }
                                
                        }
                    }
                }
                else
                {
                    if (firstMove.Y - previousFirstMove.Y > 1 || firstMove.Y - previousFirstMove.Y < -1)
                    {
                        objectSelected.rotateGeometricShape(firstMove.Y - previousFirstMove.Y);
                    }
                }
            }
            // previousMove is resfreshed.
            previousFirstMove.X = firstMove.X;
            previousFirstMove.Y = firstMove.Y;

            #endregion reactable objects events
        }

        private InputReactableObject previousObject;

        private void OnXYUpUI(int x, int y, int z, int PointerID)
        {
            // re-initialization of the finger on the shape
            foreach (InputReactableObject reactableObject in ReactableObject.ReactableObjectList)
            {
                reactableObject.firstFingerOnObject = false;
                reactableObject.doubleClickObject = false;
                reactableObject.collisionSurface.Opacity = 0;
            }
            previousObject = objectSelected;
            objectSelected = null;
        }


        /// <summary>
        /// The Trace function enables to easily write a Debug message
        /// that don't appears when running in "Release" mode.
        /// </summary>
        /// <param name="_message">The debug message.</param>
        private static void Trace(String _message)
        {
            Debug.WriteLine(String.Format("{0}: {1}", traceCount, _message));
        }

        /// <summary>
        /// This function is basically a HitTest function working with renderTransform.
        /// </summary>
        /// <param name="reference">The object you want to HitTest.</param>
        /// <param name="point">The contact point on the screen.</param>
        /// <returns> boolean value indicate if the shape is hit or not</returns>
        public static bool HitTestPoint(FrameworkElement shape, Point point)
        {
            Console.WriteLine("{0}{1}", point.X, point.Y);
            Point myPoint = shape.RenderTransform.Inverse.Transform(point);
            Console.WriteLine("{0}{1}", myPoint.X, myPoint.Y);
            HitTestResult hitTestResult = VisualTreeHelper.HitTest(shape, myPoint);
            if (hitTestResult != null) return true;
            else return false;
        }

        /*public static List<ReactableObject> getreactableObjectList()
        {
            return reactableObjectList;
        }*/
    }
}
