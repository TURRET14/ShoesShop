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

namespace ShoesShop
{
    /// <summary>
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        public ProductsPage(ShopUser user)
        {
            InitializeComponent();
            if (user.UserRole.Name == "Авторизованный клиент")
            {

            }
            else if (user.UserRole.Name == "Администратор")
            {

            }
            else if (user.UserRole.Name == "Менеджер")
            {

            }
            else
            {

            }
        }
        public ProductsPage()
        {
            InitializeComponent();
        }
    }
}
