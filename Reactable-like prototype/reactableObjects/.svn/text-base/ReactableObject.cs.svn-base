using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace WpfApplication2
{
    public abstract class ReactableObject
    {
        /// <summary>
        /// The canvas where the object is on it.
        /// </summary>
        private Canvas canvas;

        public Canvas Canvas
        {
            get { return canvas; }
            set { canvas = value; }
        }

        /// <summary>
        /// X coordonne of the object's center.
        /// </summary>
        protected double x;

        public double getX()
        {
            return x;
        }

        /// <summary>
        /// U coordonne of the object's center.
        /// </summary>
        protected double y;

        public double getY()
        {
            return y;
        }

        /// <summary>
        /// The heigth of the object.
        /// </summary>
        protected double height;

        /// <summary>
        /// The width of the object.
        /// </summary>
        protected double width;

        /// <summary>
        ///  This list will contain all object on reactable-like.
        /// </summary>
        protected static List<ReactableObject> reactableObjectList = new List<ReactableObject>();

        public static List<ReactableObject> ReactableObjectList
        {
            get { return ReactableObject.reactableObjectList; }
            set { ReactableObject.reactableObjectList = value; }
        }


        /// <summary>
        /// The connection of the object with another one.
        /// </summary>
        public Boolean connected = false;

        /// <summary>
        /// Array of reactable objects which are connected on this reactable object
        /// </summary>
        protected ReactableObject[] inputObject;
        
        /// <summary>
        /// Array of reactable objects which are connected on this reactable object
        /// </summary>
        public ReactableObject[] InputObject
        {
            get { return inputObject; }
            set { inputObject = value; }
        }

        /// <summary>
        /// Convert from degrees to radians via multiplication by PI/180  
        /// </summary>
        /// <param name="_radius">Radius of the element</param>
        /// <param name="_angleInDegrees">The angle of point</param>
        /// <param name="_origin">The origin of the element</param>
        /// <returns></returns>
        public Point PointOnCircle(double _radius, double _angleInDegrees, Point _origin)
        {     
            double x = (_radius * Math.Cos(_angleInDegrees * Math.PI / 180.0)) + _origin.X;
            double y = (_radius * Math.Sin(_angleInDegrees * Math.PI / 180.0)) + _origin.Y;

            return new Point(x, y);
        }

		/// <summary>
		/// Give the nearest object.
		/// </summary>
		/// <param name="objectList"></param>
		/// <returns></returns>
		public ReactableObject nearestObject(List<ReactableObject> objectList)
		{
			ReactableObject nearestObject = null;
			double distance = 0;
			foreach (ReactableObject reactableObject in objectList)
			{
				// The first object in the radius of the current object is the nearest.
				if (nearestObject == null)
				{
					nearestObject = reactableObject;
					distance = distanceCalculation(reactableObject);
				}
				// Check if the others object in the radius is the nearest.
				if (distanceCalculation(reactableObject) < distance)
				{
					nearestObject = reactableObject;
					distance = distanceCalculation(reactableObject);
				}
			}

			return nearestObject;
		}

		public double distanceCalculation(ReactableObject a)
		{
			return Math.Sqrt((a.x - x) * (a.x - x) + (a.y - y) * (a.y - y));
		}


			
    }
}
