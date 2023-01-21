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

        List<Поставка> purveyanceProductList = new List<Поставка>();
        List<Списание> withdrawalProductList = new List<Списание>();

        public MainWindow()
        {
            InitializeComponent();
            Create();
        }
        private void Create()
        {

        }
    }
}
