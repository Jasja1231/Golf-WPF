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
        private  bool golfer = false; 

        //List of all golfer points
        List<Golfers.Point> golfpoints = new List<Golfers.Point>();

        private Point drag_point = new Point();


        private static Point mousePos = new Point();


        public MainWindow()
        {
            InitializeComponent();
        }


        private void Canvas_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            Ellipse el = new Ellipse();
            Golfers.PointType Golfer_point = new Golfers.PointType();


            //Choosing color  & point type
            if (!golfer)
            {
                mySolidColorBrush.Color = Colors.Red; // red
                Golfer_point = Golfers.PointType.Hole;
            }
            else { 
                mySolidColorBrush.Color = Colors.Blue; //Blue
                Golfer_point = Golfers.PointType.Golfer;
            }
            golfer = !golfer;

            if (e.ChangedButton == MouseButton.Left) {
                //getting coordinates of elipce with respect to canvas
                currentPoint = e.GetPosition(Canvas1);
                
                //Add new pont to list of all Golfer points with special position
                golfpoints.Add(new Golfers.Point(currentPoint.X, currentPoint.Y, Golfer_point)); // - clean it after pressing NEW GAME


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
             }//if left press end
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


        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            drag_point.X = Mouse.GetPosition(scrollViewer).X;
            drag_point.Y = Mouse.GetPosition(scrollViewer).Y;
            Canvas1.Cursor = Cursors.SizeAll;     
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed) {
                //get current mouse polition with relpect to scroll view
                mousePos = e.GetPosition(scrollViewer);

                double X = mousePos.X - drag_point.X;
                double Y = mousePos.Y - drag_point.Y;

                drag_point = mousePos;


                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + X);
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + Y);

            }
        }

        private void Canvas_Mouse_Enter(object sender, MouseEventArgs e)
        {

        }

        

        private void Canvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            scrollViewer.Cursor = Cursors.Arrow;
            Mouse.Capture(null);
            scrollViewer.ReleaseMouseCapture();
            //Mouse.Capture(this);
        }


        private void sizeSlider_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                Canvas1.Width = Canvas1.Height = Canvas1.Height + e.Delta * 0.03;
                sizeSlider.Value += e.Delta * 0.03;
            }

            else if (e.Delta < 0 && sizeSlider.Value + e.Delta * 0.03 >= 1)
            {
                Canvas1.Width = Canvas1.Height = Canvas1.Height + e.Delta*0.03;
                sizeSlider.Value += e.Delta * 0.03;
                if (sizeSlider.Value < (uint)(e.Delta * 0.03) + 1)
                    sizeSlider.Value = 1;
            }
        }


        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            sizeSlider_MouseWheel(sender, e);
        }


        //NEw game menu item clicked 
        private void new_Game(object sender, RoutedEventArgs e)
        {
            golfpoints.Clear();
            Canvas1.Children.Clear();
        }



        



       
       

       


   

    }
}
