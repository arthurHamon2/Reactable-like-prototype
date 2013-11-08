using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication2.reactableObjects;
using System.Windows.Shapes;

namespace WpfApplication2.reactableObjectLink
{
    public class GeneratorLink : ReactableObjectLink
    {
        
		public GeneratorLink()
		{
			linkConnection = new Line();
		}
        
        public override void checkingConnection(InputReactableObject generator)
        {
            // Copy and paste the list of object in the radius of the generator in a local list.
			List<ReactableObject> objectList = new List<ReactableObject>();
			generator.ObjectsInRadius.ForEach(objectList.Add);

            //Takes the nearest object of the list of object. 
            ReactableObject nearestObject = generator.nearestObject(objectList);
            bool connectionCheck = false;
            
            // Condition: the generator has got an object ,at least one, in its radius.
            //if (nearestObject != null)
            //{
                // Condition: If the nearest object is an effect filter which is already connected.
                if (nearestObject.InputObject[0] != null && nearestObject is EffectFilter 
                    && !generator.Equals(nearestObject.InputObject[0])
					|| generator.distanceCalculation(SmartBoard.output) < nearestObject.distanceCalculation(SmartBoard.output))
                {
                    // To leave: a connection has been established or every objects in the generator's radius are not available to be connected.
                    while (!connectionCheck) //&& nearestObject != null && objectList.Count>0)
                    {
						// If the nearest object is the output the object must to be connected with it.
						if (!(nearestObject is Output))
						{
							// Check if the nearest object is before the generator.
							if (generator.distanceCalculation(SmartBoard.output) > nearestObject.distanceCalculation(SmartBoard.output))
							{
								// If a connection is available with the filter or if the filter is already connected with the generator 
								if (nearestObject.InputObject[0] == null || nearestObject.InputObject[0].Equals(generator))
								{
									// Creates or updates the connection
									connection(generator, nearestObject);
									connectionCheck = true;
								}
								// If the nearest object has already got a connected object, we check if the connected object is more distant of the generator or not 
								if (nearestObject.distanceCalculation(nearestObject.InputObject[0]) > generator.distanceCalculation(nearestObject))
								{
									// If the generator is more close, a new connection is established.
									connection(generator, nearestObject);
									connectionCheck = true;
								}
							}

							// If a connection is not available with the given nearest object.
							if (!connectionCheck)
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
								nearestObject = generator.nearestObject(objectList);
							}
						}
						else
						{
							connection(generator, nearestObject);
							connectionCheck = true;
						}
                    }

                }
                // Condition: If the nearest object is not an effect filter.
                else
                {
                    //The connection can be established with the given object
                    connection(generator, nearestObject);
                }
           // }
            // Saves the previous object which has been connected.
            previousConnectedObject = nearestObject; 
        }

    }
}
