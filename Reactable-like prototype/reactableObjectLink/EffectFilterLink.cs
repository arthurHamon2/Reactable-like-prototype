using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using WpfApplication2.reactableObjects;

namespace WpfApplication2.reactableObjectLink
{
    public class EffectFilterLink : ReactableObjectLink
    {
        public EffectFilterLink()
		{
			linkConnection = new Line();
		}


        public override void checkingConnection(InputReactableObject effectFilter)
        {
            // Copy and paste the list of object in the radius of the generator in a local list.
            List<ReactableObject> objectList = new List<ReactableObject>();
            effectFilter.ObjectsInRadius.ForEach(objectList.Add);

            //Takes the nearest object of the list of object. 
            ReactableObject nearestObject = effectFilter.nearestObject(objectList);
            bool connectionCheck = false;

            // Condition: the generator has got an object ,at least one, in its radius.
            //if (nearestObject != null)
            //{
            // Condition: If the nearest object is an effect filter which is already connected.
            if (nearestObject.InputObject[0] != null && nearestObject is EffectFilter
                && !effectFilter.Equals(nearestObject.InputObject[0])
                || effectFilter.distanceCalculation(SmartBoard.output) < nearestObject.distanceCalculation(SmartBoard.output))
            {
                // To leave: a connection has been established or every objects in the generator's radius are not available to be connected.
                while (!connectionCheck) //&& nearestObject != null && objectList.Count>0)
                {
                    // If the nearest object is the output the object must to be connected with it.
                    if (!(nearestObject is Output))
                    {
                        // Check if the nearest object is before the generator.
                        if (effectFilter.distanceCalculation(SmartBoard.output) > nearestObject.distanceCalculation(SmartBoard.output))
                        {
                            // If a connection is available with the filter or if the filter is already connected with the generator 
                            if (nearestObject.InputObject[0] == null || nearestObject.InputObject[0].Equals(effectFilter))
                            {
                                // Creates or updates the connection
                                connection(effectFilter, nearestObject);
                                connectionCheck = true;
                            }
                            // If the nearest object has already got a connected object, we check if the connected object is more distant of the generator or not 
                            if (nearestObject.distanceCalculation(nearestObject.InputObject[0]) > effectFilter.distanceCalculation(nearestObject))
                            {
                                // If the generator is more close, a new connection is established.
                                connection(effectFilter, nearestObject);
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
                            nearestObject = effectFilter.nearestObject(objectList);
                        }
                    }
                    else
                    {
                        connection(effectFilter, nearestObject);
                        connectionCheck = true;
                    }
                }

            }
            // Condition: If the nearest object is not an effect filter.
            else
            {
                //The connection can be established with the given object
                connection(effectFilter, nearestObject);
            }
            // }
            // Saves the previous object which has been connected.
            previousConnectedObject = nearestObject;
        }
        
        
        
        
        
        /*public override void checkingConnection(InputStuff effectFilter)
        {
            // Copy and paste the list of object in the radius of the generator in a local list.
			List<ReactableObject> objectList = new List<ReactableObject>();
			effectFilter.ObjectsInRadius.ForEach(objectList.Add);

            //Takes the nearest object of the list of object. 
            ReactableObject nearestObject = effectFilter.nearestObject(objectList);
            bool connectionCheck = false;
            
            // Condition: the effect or the filter has got an object ,at least one, in its radius.
            if (nearestObject != null)
            {
                // Condition: If the nearest object is an effect filter which is already connected.
                    if (nearestObject.InputObject != null && nearestObject is EffectFilter
                       // && !effectFilter.Equals(nearestObject.InputObject[0]) 
                        )
                    {
                        // To leave: a connection has been established or every objects in the effect or filter's radius are not available to be connected.
                        while (!connectionCheck && nearestObject != null && objectList.Count > 0)
                        {
                            // If a connection is available with the filter or if the filter is already connected with the generator 
                            if (nearestObject.InputObject[0] == null || nearestObject.InputObject[0].Equals(effectFilter))
                            {
                                // Creates or updates the connection
                                connection(effectFilter, nearestObject);
                                connectionCheck = true;
                            }
                            // If the nearest object has already got a connected object, we check if the connected object is more distant of the generator or not 
                            if (nearestObject.distanceCalculation(nearestObject.InputObject[0]) > effectFilter.distanceCalculation(nearestObject))
                            {
                                // If the generator is more close, a new connection is established.
                                connection(effectFilter, nearestObject);
                                connectionCheck = true;
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
                                nearestObject = effectFilter.nearestObject(objectList);
                            }
                        }
                        // If the connection has not been established.
                        if (nearestObject == null && !connectionCheck)
                        {
                            // The connection will be done with the output.
                            connection(effectFilter, SmartBoard.output);
                        }
                    }
                    // Condition: If the nearest object is not connected.
                    else
                    {
                        //The connection can be established with the given object
                        connection(effectFilter, nearestObject);
                    }
                }
            
            // Condition: the generator has not got an object near its. 
            else if (!connectionCheck)
            {
                // Then the connection is established with the output
                connection(effectFilter, SmartBoard.output);
            }

            // Saves the previous object which has been connected.
            previousConnectedObject = null;
            if (nearestObject != null)
            {
                previousConnectedObject = nearestObject;
            }
        }*/

        public void debug(EffectFilter f1)
        {
            Console.WriteLine("hello, Im the filter X={0} Y={1}", f1.getX(), f1.getY());
            if (f1.ConnectedObjects != null)
            {
                foreach (EffectFilter effectFilter in f1.ConnectedObjects)
                {
                    Console.WriteLine(effectFilter.getX());
                }
            }
        }
    }
}

