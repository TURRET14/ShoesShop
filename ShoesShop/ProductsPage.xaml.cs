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
        private ProductSupplier allSuppliersOption = new ProductSupplier() { Name = "Все поставщики" };
        private ShopUser user;
        public ProductsPage(ShopUser user)
        {
            InitializeComponent();
            this.user = user;
            ListBox_Data.ItemsSource = Emelyanenko_ShoesShopEntities.GetInstance().Product.ToList();
            List<ProductSupplier> suppliers = Emelyanenko_ShoesShopEntities.GetInstance().ProductSupplier.ToList();
            suppliers.Add(allSuppliersOption);
            ComboBox_FilterSupplier.ItemsSource = suppliers;
            if (user.UserRole.Name == "Администратор" || user.UserRole.Name == "Менеджер")
            {
                Grid_Filters.Visibility = Visibility.Visible;
                Grid_Sorting.Visibility = Visibility.Visible;
                Button_Orders.Visibility = Visibility.Visible;
            }
            if (user.UserRole.Name == "Администратор")
            {
                Grid_Add_Delete.Visibility = Visibility.Visible;
            }
        }
        public ProductsPage()
        {
            InitializeComponent();
            ListBox_Data.ItemsSource = Emelyanenko_ShoesShopEntities.GetInstance().Product.ToList();
            List<ProductSupplier> suppliers = Emelyanenko_ShoesShopEntities.GetInstance().ProductSupplier.ToList();
            suppliers.Add(allSuppliersOption);
            ComboBox_FilterSupplier.ItemsSource = suppliers;
        }

        private void TextBox_Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterAndSort();
        }

        private void ComboBox_Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAndSort();
        }

        private void FilterAndSort()
        {
            List<Product> products = Emelyanenko_ShoesShopEntities.GetInstance().Product.ToList();
            if (!String.IsNullOrEmpty(TextBox_FilterArticle.Text))
            {
                products = products.Where(entry => entry.ProductArticle.ToUpper().Contains(TextBox_FilterArticle.Text.ToUpper())).ToList();
            }

            if (!String.IsNullOrEmpty(TextBox_FilterName.Text))
            {
                products = products.Where(entry => entry.ProductName.ToUpper().Contains(TextBox_FilterName.Text.ToUpper())).ToList();
            }

            if (!String.IsNullOrEmpty(TextBox_FilterDescription.Text))
            {
                products = products.Where(entry => entry.ProductDescription.ToUpper().Contains(TextBox_FilterDescription.Text.ToUpper())).ToList();
            }

            if (ComboBox_FilterSupplier.SelectedItem != null)
            {
                if (!(ComboBox_FilterSupplier.SelectedItem == allSuppliersOption))
                {
                    products = products.Where(entry => entry.ProductSupplier == ComboBox_FilterSupplier.SelectedItem).ToList();
                }
            }

            if (ComboBox_Sort.SelectedItem.ToString() == "По возрастанию")
            {
                products = products.OrderBy(entry => entry.ProductInStock).ToList();
            }
            else if (ComboBox_Sort.SelectedItem.ToString() == "По убыванию")
            {
                products = products.OrderBy(entry => entry.ProductInStock).Reverse().ToList();
            }
            if (ListBox_Data != null)
            {
                ListBox_Data.ItemsSource = products;
            }
        }

        private void ListBox_Data_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (user.UserRole.Name == "Администратор")
            {
                if (sender != null)
                {
                    NavigationService.Navigate(new EditProductPage(((ListBoxItem)sender).DataContext as Product));
                }
            }
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить выбранные записи?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    List<Product> products = ListBox_Data.SelectedItems.Cast<Product>().ToList();
                    foreach (Product product in products)
                    {
                        if (Emelyanenko_ShoesShopEntities.GetInstance().ShopOrderDetail.FirstOrDefault(entry => entry.ProductID == product.ID) != null)
                        {
                            MessageBox.Show("Один или более из выбранных товаров используется в заказе. Его удаление невозможно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    Emelyanenko_ShoesShopEntities.GetInstance().Product.RemoveRange(products);
                    Emelyanenko_ShoesShopEntities.GetInstance().SaveChanges();
                    ListBox_Data.ItemsSource = Emelyanenko_ShoesShopEntities.GetInstance().Product.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при работе с базой данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditProductPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ListBox_Data.ItemsSource = Emelyanenko_ShoesShopEntities.GetInstance().Product.ToList();
        }

        private void Button_Orders_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrdersPage(user));
        }
    }
}
