using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using WpfApplication2.reactableObjects;
using System.Windows.Media;

namespace WpfApplication2.reactableObjectLink
{
    public abstract class ReactableObjectLink
    {
        /// <summary>
        /// The drawing line (the connection) between two objects.
        /// </summary>
        protected Line linkConnection;

        /// <summary>
        /// The previous connected object.
        /// </summary>
        protected ReactableObject previousConnectedObject;

        /// <summary>
        /// Draws a line (a connection) between two objects.
        /// </summary>
        /// <param name="outPut"> The object which will be connected to another object. </param>
        /// <param name="inPut"> The object which receives the connection. </param>
        public void connection(ReactableObject outPut, ReactableObject inPut)
        {
            // Removes the previous connection.
            disconnection(outPut, inPut);

            // Creating the connection between the output and the input.
            linkConnection = new Line();
            if(outPut is Generator)
                linkConnection.Stroke = Brushes.Green;
            else if(outPut is EffectFilter)
                linkConnection.Stroke = Brushes.Orange;
            else if(outPut is Controller)
                linkConnection.Stroke = Brushes.Yellow;
            //listEffectFilter((EffectFilter) outPut, (EffectFilter) inPut);
			linkConnection.StrokeThickness = 2;
            
            linkConnection.X1 = outPut.getX();
            linkConnection.Y1 = outPut.getY();

            linkConnection.X2 = inPut.getX();
            linkConnection.Y2 = inPut.getY();

			outPut.Canvas.Children.Add(linkConnection);
			outPut.connected = true;

            // If the input is a filter, its input must be store the output.
            if (inPut is EffectFilter && !(outPut is Controller))
            {
                inPut.InputObject[0] = outPut;
            }
           /* if (inPut is EffectFilter && outPut is EffectFilter)
            {

                if ((previousConnectedObject != null && previousConnectedObject!= inPut)&& previousConnectedObject is EffectFilter)
                {
                    deleteInList((EffectFilter)inPut, (EffectFilter)previousConnectedObject);
                }
                if (previousConnectedObject == null || !inPut.Equals(previousConnectedObject))
                {
                   // deleteInList((EffectFilter)inPut, (EffectFilter)previousConnectedObject);
                    listEffectFilter((EffectFilter)inPut, (EffectFilter)outPut);
                }
            }*/
        }

        /// <summary>
        /// Deletes the connection.
        /// </summary>
        /// <param name="outPut"> The object which will be connected to another object.</param>
        /// <param name="inPut"> The object which receives the connection. </param>
        public void disconnection(ReactableObject outPut, ReactableObject inPut)
        {
            outPut.connected = false;

            if (linkConnection != null)
            {
                // Removes the previous connection on the canvas.
                outPut.Canvas.Children.Remove(linkConnection);
            }

            if (previousConnectedObject != null && inPut != null 
                && !(outPut is Controller))
            {
                // Only if the input is not already connected with its previous output.
                if (!outPut.Equals(previousConnectedObject))
                {
                    // Sets the filter's input at null.
                    previousConnectedObject.InputObject[0] = null;
                }
            }
            
            
        }

        /// <summary>
        /// Checking the connections available
        /// </summary>
        /// <param name="outPut"> The object which will be connected to another object. </param>
        public abstract void checkingConnection(InputReactableObject outPut);

    }
}
