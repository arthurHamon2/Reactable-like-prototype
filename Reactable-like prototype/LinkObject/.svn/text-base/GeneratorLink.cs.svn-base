using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication2.reactableObjects;
using System.Windows.Controls;

namespace WpfApplication2.LinkObject
{
    public class GeneratorLink: Link
    {

        public GeneratorLink(ReactableObject _input, ReactableObject _output, Canvas _canvas) :base(_input, _output, _canvas)
        {
            if (typeof(Filter).IsAssignableFrom(input.GetType()) )
            {
                drawingConnectionWithFiltrer();
            }
            else if (typeof(Output).IsAssignableFrom(input.GetType()) )
            {
            
            }
        }
        
       
        
        public override void drawingConnection()
        {
            
        }

        public void drawingConnectionWithFiltrer()
        {
            // take the center of the shape to start
            connection.X1 = output.getX();
            connection.Y1 = output.getY();
            // take the center of the output to finish
           // connection.X2 = Canvas.GetLeft((Output)output.outputCircle) + output.getHeight() / 2;
           // connection.Y2 = Canvas.GetTop(output.outputCircle) + output.getWidth() / 2;
            connection.X2 = input.getX();
            connection.Y2 = input.getY();

            connected = true;

            canvas.Children.Add(connection);
        }

        public void refreshConnection()
        {
            canvas.Children.Remove(connection);

            // take the center of the shape to start
            connection.X1 = output.getX();
            connection.Y1 = output.getY();
            // take the center of the output to finish
            // connection.X2 = Canvas.GetLeft((Output)output.outputCircle) + output.getHeight() / 2;
            // connection.Y2 = Canvas.GetTop(output.outputCircle) + output.getWidth() / 2;
            connection.X2 = input.getX();
            connection.Y2 = input.getY();

            connected = true;

            canvas.Children.Add(connection);
        }
    }
}
