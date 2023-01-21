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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp8
{
    public partial class MainWindow : Window
    {
        GroceryStoreDatabasesEntities groceryStoreDatabasesEntities = new GroceryStoreDatabasesEntities();

        List<Поставка> purveyanceProductList = new List<Поставка>()
        {
            
        };
        List<Списание> withdrawalProductList = new List<Списание>();

        int quantity = 10;
        int indent = 50;


        public MainWindow()
        {
            InitializeComponent();
            CreateChart();
        }
        private void CreateChart()
        {

            for (int i = 0; i < quantity; i++)
            {
                TextBlock vertical = new TextBlock() 
                {
                    Text = (quantity - i).ToString(),
                    RenderTransform = new TranslateTransform(indent, indent * i),
                    FontSize = 24,
                };
                TextBlock horizontal = new TextBlock()
                {
                    Text = (i+1).ToString(),
                    RenderTransform = new TranslateTransform(indent * (i+2), indent * quantity),
                    FontSize = 24,
                };
                Line line = new Line()
                {
                    X1 = indent,
                    X2 = indent * (i + 2),
                    Y1 = indent * i,
                    Y2 = indent * quantity,
                    Fill = Brushes.Black,
                    StrokeThickness = 1,
                    Stroke = Brushes.Black
                };
                ChartCanvas.Children.Add(vertical);
                ChartCanvas.Children.Add(horizontal);
                ChartCanvas.Children.Add(line);
            }


            for (int i = 0; i < purveyanceProductList.Count; i++)
            {
                Rectangle rectangle = new Rectangle()
                {
                    Width = 50,
                    Height = 100,
                    Fill = Brushes.Green,
                    RenderTransform = new TranslateTransform(30, 30),
                };
                ChartCanvas.Children.Add(rectangle);
            }
        }
    }
}
