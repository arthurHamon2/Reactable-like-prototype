using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace WpfApplication2.LinkObject
{
    public abstract class Link
    {

        protected ReactableObject output;

        protected ReactableObject input;

        protected Line connection;

        protected Canvas canvas;

        protected Boolean connected = false;

        public Link(ReactableObject _input, ReactableObject _output, Canvas _canvas)
        {
            connection = new Line();
            input = _input;
            output = _output;
            canvas = _canvas;
        }

        public abstract void drawingConnection();

    }
}
