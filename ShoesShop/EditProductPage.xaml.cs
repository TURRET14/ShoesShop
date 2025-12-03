using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Логика взаимодействия для EditProductPage.xaml
    /// </summary>
    public partial class EditProductPage : Page
    {
        private Product selected;
        private bool isNewObject;
        public EditProductPage(Product selected)
        {
            InitializeComponent();
            LoadDataIntoComboBoxes();
            this.selected = selected;
            isNewObject = false;
            this.DataContext = selected;
        }
        public EditProductPage()
        {
            InitializeComponent();
            LoadDataIntoComboBoxes();
            selected = new Product();
            isNewObject = true;
            this.DataContext = selected;
        }

        private void LoadDataIntoComboBoxes()
        {
            ComboBox_Category.ItemsSource = Emelyanenko_ShoesShopEntities.GetInstance().ProductCategory.ToList();
            ComboBox_Manufacturer.ItemsSource = Emelyanenko_ShoesShopEntities.GetInstance().ProductManufacturer.ToList();
            ComboBox_Supplier.ItemsSource = Emelyanenko_ShoesShopEntities.GetInstance().ProductSupplier.ToList();
        }

        private void Button_SelectPhotoPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Изображения (*.png;*.jpg)|*.png;*.jpg";
            if (dialog.ShowDialog() == true)
            {
                BitmapImage img = new BitmapImage(new Uri(dialog.FileName));
                if (img.PixelWidth > 300 || img.PixelHeight > 200)
                {
                    MessageBox.Show("Разрешение изображения слишком большое! Максимум: 300X200!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                selected.ProductPhotoPath = dialog.FileName;
                Image_Icon.GetBindingExpression(Image.SourceProperty).UpdateTarget();
            }
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (String.IsNullOrEmpty(selected.ProductName))
            {
                errors.AppendLine("Наименование не может быть пустым!");
            }
            if (selected.ProductCategory == null)
            {
                errors.AppendLine("Категория не может быть пустой!");
            }
            if (String.IsNullOrEmpty(selected.ProductDescription))
            {
                errors.AppendLine("Описание не может быть пустым!");
            }
            if (selected.ProductManufacturer == null)
            {
                errors.AppendLine("Производитель не может быть пустым!");
            }
            if (selected.ProductSupplier == null)
            {
                errors.AppendLine("Поставщик не может быть пустым!");
            }
            if (selected.ProductPrice < 0)
            {
                errors.AppendLine("Цена не может быть отрицательной!");
            }
            if (String.IsNullOrEmpty(selected.ProductUnit))
            {
                errors.AppendLine("Единица измерения не может быть пустой!");
            }
            if (selected.ProductInStock < 0)
            {
                errors.AppendLine("Количество на складе не может быть отрицательной!");
            }
            if (selected.ProductDiscount < 0 || selected.ProductDiscount > 100)
            {
                errors.AppendLine("Скидка не может быть отрицательной или больше 100!");
            }
            if (String.IsNullOrEmpty(selected.ProductArticle))
            {
                errors.AppendLine("Артикул не может быть пустой!");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (selected.ProductPhotoPath != null && selected.ProductPhotoPath.Contains(":\\"))
            {
                try
                {
                    if (File.Exists(selected.ProductPhotoPath))
                    {
                        if (!File.Exists(Environment.CurrentDirectory + "\\" + selected.ProductPhotoPath.Split('\\').Last()))
                        {
                            File.Copy(selected.ProductPhotoPath, Environment.CurrentDirectory + "\\" + selected.ProductPhotoPath.Split('\\').Last(), overwrite: true);
                        }
                        else
                        {
                            MessageBox.Show("Изображение с таким названием уже существует. Будет использовано уже существующее изображение с таким названием.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        selected.ProductPhotoPath = selected.ProductPhotoPath.Split('\\').Last();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении изображения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            

            if (isNewObject)
            {
                Emelyanenko_ShoesShopEntities.GetInstance().Product.Add(selected);
            }
            Emelyanenko_ShoesShopEntities.GetInstance().SaveChanges();

            NavigationService.GoBack();
        }
    }
}
