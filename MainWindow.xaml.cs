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
        GroceryStoreDatabasesEntities groceryStoreDatabases = new GroceryStoreDatabasesEntities();

        List<Поставка> purveyanceProductList = new List<Поставка>()
        {
        };
        List<Списание> withdrawalProductList = new List<Списание>();

        DateTime month = DateTime.Now;

        int quantity = 12;
        int indient = 50;
        int[] year;

        string[] monthsName = new string[] { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
        public MainWindow()
        {
            InitializeComponent();
            ProductComboBox.ItemsSource = groceryStoreDatabases.Товар.ToList();
            ProductComboBox.DisplayMemberPath = "Наименование";
            ProductComboBox.SelectedIndex = 0;
            CreateRangeYears();
            CreateChart();

        }
        private void CreateChart()
        {
            ChartCanvas.Children.Clear();

            Товар selectedProduct = groceryStoreDatabases.Товар.Where(x => x.Наименование.Equals(ProductComboBox.Text)).FirstOrDefault();
            List<ТоварПоставка> deliverySelectedProductList = groceryStoreDatabases.ТоварПоставка.Where(x => x.КодТовара.Equals(selectedProduct.Код)).ToList();
            if(deliverySelectedProductList.Count == 0)
            {
                return;
            }
            int max = deliverySelectedProductList.Max(x => x.Количество);
            int step = 1;
            int f = deliverySelectedProductList.Max(x => x.Количество);
            while (max >10)
            {
                max = max / 10;
                step = step * 10;
            }
            step = step / 10;
            int quantityInSegment = max*step;

            List<Поставка> deliveryList = groceryStoreDatabases.Поставка.ToList().Where(x => x.ДатаПоставки.Year.Equals(Convert.ToInt16(ShowYearComboBox.Text))).ToList();

            for (int i = 0; i < quantity; i++)
            {
                TextBlock vertical = new TextBlock()
                {
                    Text = (max * i* step).ToString(),
                    RenderTransform = new TranslateTransform(0, indient * (quantity - i) - indient / 2),
                    FontSize = 18,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextAlignment = TextAlignment.Right,
                    Width = 95,
                };

                TextBlock horizontal = new TextBlock()
                {
                    Text = (monthsName[quantity - i - 1]).ToString(),
                    RenderTransform = new TranslateTransform(indient * (i + 1) * 2, indient * quantity),
                    FontSize = 18,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                    Width = indient * 2,
                    LayoutTransform = new RotateTransform(45),
                };
                Line horizontalLine = new Line()
                {
                    X1 = indient,
                    X2 = indient * quantity * 2 + indient * 2,
                    Y1 = indient * i + indient,
                    Y2 = indient * i + indient,
                    Fill = Brushes.Beige,
                    StrokeThickness = 1,
                    Stroke = Brushes.Beige
                };
                Line verticalLLine = new Line()
                {
                    Y1 = indient - indient,
                    Y2 = indient * quantity + indient,
                    X1 = indient * i * 2 + indient * 2,

                    X2 = indient * i * 2 + indient * 2,
                    Fill = Brushes.Aquamarine,
                    StrokeThickness = 1,
                    Stroke = Brushes.Firebrick
                };

                ChartCanvas.Children.Add(vertical);
                ChartCanvas.Children.Add(horizontal);
                ChartCanvas.Children.Add(horizontalLine);
                ChartCanvas.Children.Add(verticalLLine);

            }


            for (int i = 0; i < deliveryList.Count; i++)
            {
                InfoTextBlock.Text = step.ToString();
                var b = deliverySelectedProductList.Where(x => x.КодПоставки.Equals(deliveryList[i].Код)).FirstOrDefault();
                StackPanel grid = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    RenderTransform = new TranslateTransform(indient * (13 - deliveryList[i].ДатаПоставки.Month) * 2,(quantityInSegment*(quantity-2))- b.Количество * indient / quantityInSegment),
                    ToolTip = $"Доставлено{b.Количество}\\ {selectedProduct.Наименование}\n Остаток с поставки {b.Остаток}",
                    //VerticalAlignment = VerticalAlignment.Bottom,
                    
                };
                Rectangle rectangle = new Rectangle()
                {
                    Width = indient,
                    Height = b.Количество * indient/ quantityInSegment,
                    Fill = Brushes.Violet,
                    VerticalAlignment = VerticalAlignment.Top
                };
                Rectangle rectangle1 = new Rectangle()
                {
                    Width = indient,
                    Height = b.Остаток* indient / quantityInSegment,
                    Fill = Brushes.DarkCyan,
                    VerticalAlignment = VerticalAlignment.Top
                };
                rectangle1.RenderTransform = new TranslateTransform(0, rectangle.Height - rectangle1.Height);
                InfoTextBlock.Text = grid.RenderTransform.Value.ToString();

                grid.Children.Add(rectangle);
                grid.Children.Add(rectangle1);
                ChartCanvas.Children.Add(grid);
            }
        }

        private void CreateRangeYears()
        {
            

            DateTime firstDeliveryDate = groceryStoreDatabases.Поставка.Min(x => x.ДатаПоставки);
            int timeRange = month.Year - firstDeliveryDate.Year;
            InfoTextBlock.Text = timeRange.ToString();
            year = new int[timeRange + 1];
            for (int i = 0; i < year.Length; i++)
            {
                year[i] = firstDeliveryDate.Year + i;
            }
            ShowYearComboBox.ItemsSource = year;
            ShowYearComboBox.SelectedIndex = 0;
        }

        private void ProductComboBox_DropDownClosed(object sender, EventArgs e)
        {
            CreateRangeYears();
        }

        private void ShowYearComboBox_DropDownClosed(object sender, EventArgs e)
        {
            CreateChart();
        }
    }
}
