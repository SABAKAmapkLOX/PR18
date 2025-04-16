using Microsoft.EntityFrameworkCore;
using PR18;
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
using System.Windows.Shapes;

namespace PR18
{
    public class Data
    {
        public static OptSale? optSales;
        public static bool Login = false;
        public static string UserSurname;
        public static string UserName;
        public static string UserPartonymic;
        public static string Right;
    }

    public partial class AddItemDataGrid : Window
    {
        OptSalesContext _db = new OptSalesContext();
        OptSale _OptSales;

        public AddItemDataGrid()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Data.optSales == null)
            {
                this.Title = "Добавление записи";
                btnAddItem.Content = "Добавить";
                _OptSales = new OptSale();
            }
            else
            {
                this.Title = "Редактирование записи";
                btnAddItem.Content = "Изменить";
                _OptSales = _db.OptSales.Find(Data.optSales.Id);
            }
            this.DataContext = _OptSales;
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();

            //проверки всякие
            if (tbNameProduct.Text.Length == 0 || tbNameProduct.Text.Length >= 50) error.AppendLine("Введите Название продуктов");
            if (tbSizeBatch.Text.Length == 0 || tbSizeBatch.Text.Length >= 50) error.AppendLine("Введите размер партии");
            if (tbNameCompany.Text.Length == 0 || tbNameCompany.Text.Length >= 50) error.AppendLine("Введите название компании");
            if (tbNumberBatch.Text.Length == 0 || int.TryParse(tbNumberBatch.Text, out int numberBatch) == false) error.AppendLine("Введите номер партии");
            if (tbPriceProduct.Text.Length == 0 || decimal.TryParse(tbPriceProduct.Text, out decimal priceProduct) == false) error.AppendLine("Введите цену продукта");
            if (tbPriceBatch.Text.Length == 0 || decimal.TryParse(tbPriceBatch.Text, out decimal priceBatch) == false) error.AppendLine("Введите цену партии");
            if (tbSIzeSellBatch.Text.Length == 0) tbSIzeSellBatch.Text = "";


            //Если есть ошибки то выводим MessageBox и не сохраняем
            if (error.Length > 0)
            {
                MessageBox.Show(error.ToString());
                return;
            }

            _OptSales.DateOfReceipt = dpSelectDate.SelectedDate;
            _OptSales.DateSellBatch = dpDateSellBatch.SelectedDate;

            try
            {
                if (Data.optSales == null)
                {
                    _db.OptSales.Add(_OptSales);
                    _db.SaveChanges();
                }
                else
                {
                    _db.SaveChanges();
                }
                MessageBox.Show("Все хорошо");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
