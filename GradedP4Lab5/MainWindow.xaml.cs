using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GradedP4Lab5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //try make it static later 
        private   Point currentPoint = new Point();
        //default red (initial point color)
        private  bool color = false; 

        public MainWindow()
        {
            InitializeComponent();
        }


        private void Canvas_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            Ellipse el = new Ellipse();

            //Choosing color
            if (!color)
                mySolidColorBrush.Color = Colors.Red; // red
            else 
                mySolidColorBrush.Color = Colors.Blue; //Blue
            
            color = !color;

            if (e.ChangedButton == MouseButton.Left) {
                //getting coordinates of elipce with respect to canvas
                currentPoint = e.GetPosition(Canvas1);
                
                
                el.Fill = mySolidColorBrush;

                // Set the width and height of the Ellipse.
                el.Width = 10;
                el.Height = 10;

               
                //check if we do not go outside canvas
                if (currentPoint.X + el.Width / 2 <= Canvas1.Width && currentPoint.Y + el.Height / 2 <= Canvas1.Height && currentPoint.X - el.Width / 2 >= 0 && currentPoint.Y - el.Height / 2 >= 0) {
                    Canvas1.Children.Add(el);
                    Canvas.SetLeft(el, currentPoint.X  - el.Width/2);
                    Canvas.SetTop(el, currentPoint.Y - el.Height/2);
                }
             }
         }

        //Closing appication on Exit
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void sizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //
        }

       

       


   

    }
}
