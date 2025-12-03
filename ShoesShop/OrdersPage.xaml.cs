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
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        private ShopUser user;
        public OrdersPage(ShopUser user)
        {
            InitializeComponent();
            this.user = user;
            ListBox_Data.ItemsSource = Emelyanenko_ShoesShopEntities.GetInstance().ShopOrder.ToList();
            if (user.UserRole.Name == "Администратор")
            {
                Grid_Add_Delete.Visibility = Visibility.Visible;
            }
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (user.UserRole.Name == "Администратор")
            {
                if (sender != null)
                {
                    NavigationService.Navigate(new EditOrderPage(((ListBoxItem)sender).DataContext as ShopOrder));
                }
            }
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить выбранные записи?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    List<ShopOrder> orders = ListBox_Data.SelectedItems.Cast<ShopOrder>().ToList();
                    Emelyanenko_ShoesShopEntities.GetInstance().ShopOrder.RemoveRange(orders);
                    Emelyanenko_ShoesShopEntities.GetInstance().SaveChanges();
                    ListBox_Data.ItemsSource = Emelyanenko_ShoesShopEntities.GetInstance().ShopOrder.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при работе с базой данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditOrderPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ListBox_Data.ItemsSource = Emelyanenko_ShoesShopEntities.GetInstance().ShopOrder.ToList();
        }
    }
}
