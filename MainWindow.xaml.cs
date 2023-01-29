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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp8
{
    public partial class MainWindow : Window
    {
        GroceryStoreDatabasesEntities groceryStoreDatabases = new GroceryStoreDatabasesEntities();

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
            ChartCanvas1.Width = ChartCanvas.Width;
            ChartCanvas1.Height = ChartCanvas.Height;

        }
        private void CreateChart()
        {
            ChartCanvas.Children.Clear();
            ChartCanvas1.Children.Clear();

            Товар selectedProduct = groceryStoreDatabases.Товар.Where(x => x.Наименование.Equals(ProductComboBox.Text)).FirstOrDefault();
            List<ТоварПоставка> deliverySelectedProductList = groceryStoreDatabases.ТоварПоставка.Where(x => x.КодТовара.Equals(selectedProduct.Код)).ToList();
            if(deliverySelectedProductList.Count == 0)
            {
                return;
            }

            List<Поставка> deliveryList = groceryStoreDatabases.Поставка.ToList().Where(x => x.ДатаПоставки.Year.Equals(Convert.ToInt16(ShowYearComboBox.Text))).ToList();
            //List<Поставка> deliveryProductList = new List<Поставка>();
            //for (int i = 0; i < deliverySelectedProductList.Count; i++)
            //{
            //    deliveryProductList.Add(deliveryList.Where(x => x.Код.Equals(deliverySelectedProductList[i].КодПоставки)).FirstOrDefault());
            //}

            int max = deliverySelectedProductList.Max(x => x.Количество);
            int step = 1;
            while (max >10)
            {
                max = max / 10;
                step = step * 10;
            }
            step = step / 10;
            int quantityInSegment = max*step;

            GenerateCanvas();
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
                ChartCanvas.Children.Add(vertical);
            }
           
           

            for (int i = 0; i < deliveryList.Count; i++)
            {
                InfoTextBlock.Text = step.ToString();
                
                
                var b = deliverySelectedProductList.Where(x => x.КодПоставки.Equals(deliveryList[i].Код)).FirstOrDefault();
                if(b == null)
                {
                    MessageBox.Show("Внимание");
                    return;
                }

               
                StackPanel grid = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    RenderTransform = new TranslateTransform(
                        indient * (deliveryList[i].ДатаПоставки.Month)* 0 
                                                                          ,


                    ((indient * quantity) - b.Количество * indient / quantityInSegment)* 0 ) ,

                    ToolTip = $"Доставлено{b.Количество}\\ {selectedProduct.Наименование}\n Остаток с поставки {b.Остаток}",
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Background = Brushes.Tan,
                    //LayoutTransform = new RotateTransform(180, -indient * (deliveryList[i].ДатаПоставки.Month - quantity),
                    //  -(quantity) * indient),

                };
                Rectangle rectangle = new Rectangle()
                {
                    Width = indient,
                    Height = b.Количество * indient/ quantityInSegment,
                    Fill = Brushes.Violet,
                    VerticalAlignment = VerticalAlignment.Bottom,

                    //RenderTransform = new TranslateTransform(0, (b.Количество * indient / quantityInSegment)),
                    LayoutTransform = new RotateTransform(180, 0, -(b.Количество * indient / quantityInSegment))
                };
                Rectangle rectangle1 = new Rectangle()
                {
                    Width = indient,
                    Height = b.Остаток * indient / quantityInSegment,
                    Fill = Brushes.DarkCyan,
                    VerticalAlignment = VerticalAlignment.Top,
                };
                rectangle1.LayoutTransform = new RotateTransform(180,0, -(b.Остаток * indient / quantityInSegment));

                    //(rectangle.Height - rectangle1.Height));

                InfoTextBlock.Text = grid.RenderTransform.Value.ToString();

                DoubleAnimation buttonAnimation = new DoubleAnimation();              
                buttonAnimation.From =0;
                buttonAnimation.To = b.Количество * indient / quantityInSegment;          
                buttonAnimation.Duration = TimeSpan.FromSeconds(3);

                rectangle.BeginAnimation(HeightProperty, buttonAnimation);
                buttonAnimation.To = b.Остаток * indient / quantityInSegment;
                rectangle1.BeginAnimation(HeightProperty, buttonAnimation);
                grid.Children.Add(rectangle);
                grid.Children.Add(rectangle1);
                ChartCanvas1.Children.Add(grid);
            }
        }

        private void GenerateCanvas()
        {
            for (int i = 0; i < quantity; i++)
            {

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
                    X1 = indient*0,
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

               
                ChartCanvas.Children.Add(horizontal);
                ChartCanvas.Children.Add(horizontalLine);
                ChartCanvas.Children.Add(verticalLLine);

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
            CreateChart();
        }

        private void ShowYearComboBox_DropDownClosed(object sender, EventArgs e)
        {
            CreateChart();
        }

        private void ChartCanvas_MouseMove(object sender, MouseEventArgs e)
        {
           int xMousePosition = Convert.ToInt16(e.GetPosition(ChartCanvas1).X);
           int  yMousePosition = Convert.ToInt16(e.GetPosition(ChartCanvas1).Y);
            txtbX.Text = xMousePosition.ToString();
            txtbY.Text = yMousePosition.ToString();
        }
    }
}
