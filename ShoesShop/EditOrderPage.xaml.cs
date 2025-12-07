using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    /// Логика взаимодействия для EditOrderPage.xaml
    /// </summary>
    public partial class EditOrderPage : Page
    {
        private ShopOrder selected;
        private bool isNewOrder;
        public EditOrderPage(ShopOrder selected)
        {
            InitializeComponent();
            this.selected = selected;
            DataContext = selected;
            isNewOrder = false;
            LoadData();
        }

        public EditOrderPage()
        {
            InitializeComponent();
            this.selected = new ShopOrder();
            DataContext = selected;
            isNewOrder = true;
            LoadData();
        }

        private void LoadData()
        {
            ComboBox_Status.ItemsSource = Emelyanenko_ShoesShopEntities.GetInstance().OrderStatus.ToList();
            ComboBox_User.ItemsSource = Emelyanenko_ShoesShopEntities.GetInstance().ShopUser.ToList();
            ComboBox_PickupPoint.ItemsSource = Emelyanenko_ShoesShopEntities.GetInstance().PickupPoint.ToList();
            Column_Product.ItemsSource = Emelyanenko_ShoesShopEntities.GetInstance().Product.ToList();

            ObservableCollection<ShopOrderDetail> collection = new ObservableCollection<ShopOrderDetail>(selected.ShopOrderDetailList);
            collection.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (ShopOrderDetail item in e.NewItems)
                    {
                        item.ShopOrder = selected;
                        Emelyanenko_ShoesShopEntities.GetInstance().ShopOrderDetail.Add(item);
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (ShopOrderDetail item in e.OldItems)
                    {
                        Emelyanenko_ShoesShopEntities.GetInstance().ShopOrderDetail.Remove(item);
                    }
                }
            };
            DataGrid_Details.ItemsSource = collection;
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (selected.OrderDate == null)
            {
                errors.AppendLine("Укажите дату заказа.");
            }
            if (selected.DeliveryDate == null)
            {
                errors.AppendLine("Укажите дату доставки.");
            }
            if (selected.PickupPoint == null)
            {
                errors.AppendLine("Укажите пункт выдачи.");
            }
            if (selected.ShopUser == null)
            {
                errors.AppendLine("Укажите клиента.");
            }
            if (selected.OrderStatus == null)
            {
                errors.AppendLine("Укажите статус заказа.");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (isNewOrder)
                {
                    Emelyanenko_ShoesShopEntities.GetInstance().ShopOrder.Add(selected);
                }
                Emelyanenko_ShoesShopEntities.GetInstance().SaveChanges();
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при работе с базой данных. Скорее всего, детали заказа не прошли валидацию.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
