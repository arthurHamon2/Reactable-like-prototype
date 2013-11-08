using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using WpfApplication2.reactableObjects;

namespace WpfApplication2.reactableObjectLink
{
    public class ControllerLink : ReactableObjectLink
    {
        public ControllerLink()
		{
			linkConnection = new Line();
		}

        public override void checkingConnection(InputReactableObject controller)
        {
            // Copy and paste the list of object in the radius of the generator in a local list.
			List<ReactableObject> objectList = new List<ReactableObject>();
			controller.ObjectsInRadius.ForEach(objectList.Add);

            //Takes the nearest object of the list of object. 
            ReactableObject nearestObject = controller.nearestObject(objectList);

            if (nearestObject is Controller)
            {
                while (nearestObject is Controller && nearestObject == null)
                {
                    // Creates a temporary list which stores the current list of object 
                    List<ReactableObject> temp = new List<ReactableObject>();
                    objectList.ForEach(temp.Add);
                    // Clear the old list
                    objectList.Clear();

                    // Updates the list of object which has got the objects in the generator's radius,
                    // without the previous nearest object. 
                    foreach (ReactableObject reactableObject in temp)
                    {
                        if (!reactableObject.Equals(nearestObject))
                            objectList.Add(reactableObject);
                    }
                    // Gets the new nearest object
                    nearestObject = controller.nearestObject(objectList);
                }
            }

            // Condition: the generator has got an object ,at least one, in its radius.
            if (nearestObject != null)
            {
                //The connection can be established with the given object
                connection(controller, nearestObject);
                previousConnectedObject = nearestObject;
            }
            else
            {
                disconnection(controller, previousConnectedObject);
            }
        }

    }
}

