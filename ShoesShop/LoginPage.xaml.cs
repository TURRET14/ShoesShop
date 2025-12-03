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
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Button_Login_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (String.IsNullOrEmpty(TextBox_Login.Text)) 
            {
                errors.AppendLine("Логин не может быть пустым!");
            }
            if (String.IsNullOrEmpty(PasswordBox_Password.Password))
            {
                errors.AppendLine("Пароль не может быть пустым!");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ShopUser user = Emelyanenko_ShoesShopEntities.GetInstance().ShopUser.FirstOrDefault(entry => entry.UserLogin == TextBox_Login.Text && entry.UserPassword == PasswordBox_Password.Password);
            if (user != null)
            {
                NavigationService.Navigate(new ProductsPage(user));
                ((MainWindow)Application.Current.MainWindow).TextBlock_FIO.Text = user.UserFIO;
            }
            else
            {
                MessageBox.Show("Введен неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductsPage());
        }
    }
}
