using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Threading;
using WpfApplication2.reactableObjectLink;

namespace WpfApplication2.reactableObjects
{
    public abstract class Controller : InputReactableObject
    {
		/// <summary>
		/// Constructs an object of type Controller. 
		/// </summary>
		/// <param name="_canvas">canvas where the oscillator is on it</param>
		/// <param name="x">Coordonne x</param>
		/// <param name="y">Coordonne y</param>
		/// <param name="_width">The size of each object's side</param>
		protected Controller(Canvas _canvas, int _x, int _y, double _width)
		{
			Canvas = _canvas;
			x = _x;
			y = _y;
			height = _width;
			width = _width;

			#region Create the main form.

			// The shape is created depending of the input.
			Rect rect = new Rect(new Size(width, height));
			EllipseGeometry rectangleGeometry = new EllipseGeometry(rect);
			geometricShape = new Path();
			geometricShape.Data = rectangleGeometry;
			geometricShape.Width = width;
			geometricShape.Height = height;

			Canvas.SetTop(geometricShape, y - height / 2);
			Canvas.SetLeft(geometricShape, x - width / 2);

			// set Z index for the geometric shape for hidding connection line 
			Canvas.SetZIndex(geometricShape, 1);

			#endregion Create the main form.

			// RenderTransform part
			initializationInputStuff();

			// Add the object on the canvas.
			Canvas.Children.Add(geometricShape);
			drawRightArc();
			drawLeftArc();
			drawPointerArcLeft();
			drawPointerArcRight();
			drawingCollisionSurface(300);

            drawSubtypeMenubButton();
			// Initialization of his output.
			outputLink = new ControllerLink();

			// Initialization of the list.
			checkRadius(ReactableObject.ReactableObjectList);

			// Set the opacity of the collision surface, it will be appear when the user click on the object
			collisionSurface.Opacity = 0;

            // The dispatcher check the connection.
            connectionChecker = new DispatcherTimer(new TimeSpan(1000 * 100), //time interval, timespam( 10^-7/init)
                        DispatcherPriority.Normal,//priority
                        delegate
                        {
                            outputLink.checkingConnection(this);
                        },
                        geometricShape.Dispatcher);

		}

		/// <summary>
		/// Allow to update the list objects in the radius.
		/// </summary>
		/// <param name="listAllObjects">The list of objects to check</param>
		public override void checkRadius(List<ReactableObject> listAllObjects)
		{
			foreach (InputReactableObject reactableObject in listAllObjects)
			{
				if (distanceCalculation(reactableObject) < collisionSurfaceRadius
					&& (reactableObject is EffectFilter || reactableObject is Generator)
					&& !reactableObject.Equals(this))
				{
					if (!(objectsInRadius.Contains(reactableObject)))
					{
						// Update the list of the current object.
						objectsInRadius.Add(reactableObject);
					}
				}
				else
				{
					if (!reactableObject.Equals(this) && objectsInRadius.Contains(reactableObject))
					{
						// Update the list of the current object.
						objectsInRadius.Clear();
						checkRadius(listAllObjects);
					}
				}
			}
		}
    }

}
