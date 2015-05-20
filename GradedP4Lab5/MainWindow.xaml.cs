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

        //declaring Golfers instances 
        Golfers.DataIO dataio = new Golfers.DataIO();
        Golfers.Algorithm algorithm = new Golfers.Algorithm();
        Golfers.Validator validator = new Golfers.Validator();



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
                el.Width = 5;
                el.Height = 5;

               
                //check if we do not go outside canvas
                if (currentPoint.X + el.Width / 2 <= Canvas1.Width && currentPoint.Y + el.Height / 2 <= Canvas1.Height && currentPoint.X - el.Width / 2 >= 0 && currentPoint.Y - el.Height / 2 >= 0) {
                    Canvas1.Children.Add(el);
                    Canvas.SetLeft(el, currentPoint.X  - el.Width/2);
                    Canvas.SetTop(el, currentPoint.Y - el.Height/2);
                }
             }//if left press end
        }


        //Closing appication on Exit shit
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        Random rand = new Random();

        private void sizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsInitialized)
                return;
            Canvas1.LayoutTransform = new ScaleTransform(sizeSlider.Value, sizeSlider.Value);
            double height = scrollViewer.ScrollableHeight;
            double width = scrollViewer.ScrollableWidth;


            scrollViewer.ScrollToVerticalOffset(height / 2);            
            scrollViewer.ScrollToHorizontalOffset(width / 2);

            SolidColorBrush mySolidColorBrush =new SolidColorBrush();

           // Random r1 = new Random(255);
            int R = (int)rand.Next(0, 255);
            int G = (int)rand.Next(0, 255);
            int B = (int)rand.Next(0, 255);

            mySolidColorBrush.Color = System.Windows.Media.Color.FromRgb((byte)R, (byte)G, (byte)B);
            Canvas1.Background = mySolidColorBrush;
            //From  previous xaml
            // <Canvas.LayoutTransform>
            //       <ScaleTransform ScaleX="{Binding ElementName=sizeSlider, Path=Value, Mode=TwoWay}" ScaleY="{Binding ElementName=sizeSlider, Path=Value, Mode=TwoWay}"></ScaleTransform>
            //   </Canvas.LayoutTransform>

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
                //get current mouse polition with relpect to scroll view dat shit
                mousePos = e.GetPosition(scrollViewer);

                double X = mousePos.X - drag_point.X;
                double Y = mousePos.Y - drag_point.Y;

                drag_point = mousePos;


                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - X);
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - Y);

            }
        }
        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_Mouse_Enter(object sender, MouseEventArgs e)
        {

        }

        

        private void Canvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            scrollViewer.Cursor = Cursors.Arrow;
            Canvas1.Cursor = Cursors.Arrow;
            scrollViewer.ReleaseMouseCapture();
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
            if(sizeSlider.Value != sizeSlider.Maximum)
                sizeSlider_MouseWheel(sender, e);
            //sizeSlider_ValueChanged(sender, null);
        }


        //NEw game menu item clicked 
        private void new_Game(object sender, RoutedEventArgs e)
        {
            golfpoints.Clear();
            Canvas1.Children.Clear();
        }

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //list for storing list of golfers points
            List<Golfers.Point> temp_points = new List<Golfers.Point>();

            temp_points.AddRange(golfpoints.GetRange(0,golfpoints.Count));

            List<Line> lines = Canvas1.Children.OfType<Line>().ToList();

            if (validator.IsDataValid(temp_points))
            {

                List<Tuple<Golfers.Point, Golfers.Point>> solution = algorithm.Solve(temp_points);
                if (validator.IsSolutionValid(solution))
                {
                    foreach (Line l in lines)
                        Canvas1.Children.Remove(l);

                    foreach (Tuple<Golfers.Point, Golfers.Point> t in solution)
                    {
                        Line l = new Line();
                        l.Stroke = new SolidColorBrush(Colors.Black);
                        l.StrokeThickness = 0.2;

                        if (t.Item1.Y > t.Item2.Y)
                        {
                            l.Y1 = t.Item1.Y - 5/2;
                            l.Y2 = t.Item2.Y + 5/2;
                        }
                        else
                        {
                            l.Y1 = t.Item1.Y + 5/2;
                            l.Y2 = t.Item2.Y - 5/2;
                        }

                        l.X1 = t.Item1.X;
                        l.X2 = t.Item2.X;

                        Canvas1.Children.Add(l);

                    }


                }


            }
            else { 
                   //Show error message about invalid solution 
                   MessageBox.Show("Points are invalid", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
              }
         }


        //Load solution from chosen file 

        private void Load_File(object sender, RoutedEventArgs e)
        {
            //List of Golfer points to load
            List<Golfers.Point> temp_points = new List<Golfers.Point>();
            Microsoft.Win32.OpenFileDialog fileChooser = new Microsoft.Win32.OpenFileDialog();
            //title filter default
            fileChooser.Title = "Load solution from File";
            fileChooser.DefaultExt = ".txt";
            fileChooser.Filter = "Text file (*.txt) | *.txt";
            
            //if opens read to golfer points

            if (fileChooser.ShowDialog() == true) {
                try
                {
                    golfpoints.Clear();
                    golfpoints = dataio.Read(fileChooser.FileName);
                    Button_Click(null, null);
                }
                catch { MessageBox.Show("Incorrect File", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
            }
        }

        private void Save_File(object sender, RoutedEventArgs e) {
            Microsoft.Win32.SaveFileDialog fileSave = new Microsoft.Win32.SaveFileDialog();
            fileSave.Title = "Save solution";
            fileSave.Filter = "Text Files (*.txt) | *.txt";

            List<Tuple<Golfers.Point, Golfers.Point>> list = new List<Tuple<Golfers.Point, Golfers.Point>>();
            /////////////////
            List<Golfers.Point> copy_golfpoints = new List<Golfers.Point>(golfpoints);
            if (validator.IsDataValid(copy_golfpoints)) { 
                list = algorithm.Solve(copy_golfpoints);
            }
           
            if (fileSave.ShowDialog() == true)
            {
                if (list.Count == 0)
                    MessageBox.Show("No files in the solution", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else {
                    if (validator.IsSolutionValid(list))
                        dataio.Write(fileSave.FileName, list);
                    else {
                        MessageBox.Show("Solution is not valid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            }
        }
      }
}
