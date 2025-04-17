using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Windows;
using System.Windows.Controls;

namespace PR18
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var s = sender as MenuItem;
            switch (s.Header)
            {
                case "Добавить":
                    AddItem();
                    break;
                case "Редактировать":
                    EditItem();
                    break;
                case "Удалить":
                    DelItem();
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            switch (s.Content)
            {
                case "Найти":
                    Find();
                    break;
                case "Фильтр":
                    Filter();
                    break;
                case "SizeBatch Больше 50":
                    SQL(1);
                    break;
                case "SizeBatch Меньше 50":
                    SQL(2);
                    break;
                case "Обновление SizeSellBatch":
                    SQL(3);
                    break;
                case "Удаление Из DateSellBatch":
                    SQL(4);
                    break;
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDBInDataGrid();
        }

        private void LoadDBInDataGrid()
        {
            using (OptSalesContext _db = new OptSalesContext())//Контекст
            {
                int selectedIndex = dataGrid.SelectedIndex;
                dataGrid.ItemsSource = _db.OptSales.ToList();//Привязываем 
                if (selectedIndex != -1)
                {
                    if (selectedIndex == dataGrid.Items.Count) selectedIndex--;
                    dataGrid.SelectedIndex = selectedIndex;
                    dataGrid.ScrollIntoView(dataGrid.SelectedIndex);
                }
                dataGrid.Focus();
            }
        }

        private void AddItem()
        {
            Data.optSales = null;
            AddItemDataGrid addItem = new AddItemDataGrid();
            addItem.Owner = this;
            addItem.ShowDialog();
            LoadDBInDataGrid();
        }

        private void EditItem()
        {
            Data.optSales = (OptSale)dataGrid.SelectedItem;
            AddItemDataGrid editItem = new AddItemDataGrid();
            editItem.Owner = this;
            editItem.ShowDialog();
            LoadDBInDataGrid();
        }

        private void DelItem()
        {
            if (dataGrid.SelectedItem != null)
            {
                MessageBoxResult result;
                result = MessageBox.Show("Удалить Запись?", " Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        OptSale row = (OptSale)dataGrid.SelectedItem;
                        if (row != null)
                        {
                            using (OptSalesContext _db = new OptSalesContext())
                            {
                                _db.OptSales.Remove(row);
                                _db.SaveChanges();
                                LoadDBInDataGrid();
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка удаления");
                    }
                }
                else dataGrid.Focus();
            }
            else MessageBox.Show("Выбери запись");
        }

        private void Find()
        {
            //Получаем коллекцию элементов из dataGrid
            List<OptSale> listItem = (List<OptSale>)dataGrid.ItemsSource;
            //Используем метод Whee для фильтрации по заданному критерию 
            var filtered = listItem.Where(p => p.NameProduct.Contains(tbFind.Text));
            if (filtered.Count() > 0)
            {
                var item = filtered.First();
                dataGrid.SelectedItem = item;
                dataGrid.ScrollIntoView(item);
                dataGrid.Focus();
            }
            else MessageBox.Show("Нету");
        }

        private void Filter()
        {
            if (tbFindFilter.Text.IsNullOrEmpty() == false)
            {
                using (OptSalesContext _db = new OptSalesContext())
                {
                    var filter = _db.OptSales.Where(p => p.NameProduct == tbFindFilter.Text);
                    dataGrid.ItemsSource = filter.ToList();
                }
            }
            else LoadDBInDataGrid();
        }

        private void SQL(int select)
        {
            using (OptSalesContext _db = new OptSalesContext())
            {
                switch (select)
                {
                    case 1:
                        var fromSQLOne = _db.OptSales.FromSql($"SELECT * FROM OptSales WHERE SizeBatch > '50'");
                        dataGrid.ItemsSource = fromSQLOne.ToList();
                        break;
                    case 2:
                        var fromSQLTwo = _db.OptSales.FromSql($"SELECT * FROM OptSales WHERE SizeBatch < '50'");
                        dataGrid.ItemsSource = fromSQLTwo.ToList();
                        break;
                    case 3:
                        var fromSQLThree = _db.OptSales.FromSql($"UPDATE OptSales.dbo.OptSales SET SizeSellBatch = 10 WHERE SizeSellBatch = ''");
                        dataGrid.ItemsSource = _db.OptSales.ToList();
                        break;
                    case 4:
                        var fromSQLFour = _db.OptSales.FromSql($"DELETE FROM OptSales WHERE DateSellBatch = NULL");
                        dataGrid.ItemsSource = fromSQLFour.ToList();
                        break;
                }
            }
        }

        private void WindowDataGrid_Initialized(object sender, EventArgs e)
        {
            Autorization authorization = new Autorization();
            authorization.ShowDialog();
            if (Data.Login == false) Close();
            if (Data.Right == "Admin") ;
            else miDelite.IsEnabled = false;
            this.Title = this.Title + " " + Data.UserSurname + " " + Data.UserName + " (" + Data.Right + ")";
        }
    }
}