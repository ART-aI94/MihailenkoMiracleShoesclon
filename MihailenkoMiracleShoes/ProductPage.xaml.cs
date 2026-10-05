using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MihailenkoMiracleShoes
{
    public partial class ProductPage : Page
    {
        private List<Product> _products;
        private Client _currentUser;
        public ProductPage(Client user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadUser();
            LoadProducts();
            LoadCategories();
        }

        private void LoadUser()
        {
            if (_currentUser != null)
            {
                FIOTB.Text =
                    _currentUser.LastName + " " +
                    _currentUser.FirstName + " " +
                    _currentUser.Patrynomic;


                if (_currentUser.Roles != null)
                {
                    RoleTB.Text = _currentUser.Roles.RoleName;
                }
                else
                {
                    RoleTB.Text = "Роль не указана";
                }
            }
            else
            {
                FIOTB.Text = "Гость";
                RoleTB.Text = "";
            }
        }

        private void LoadProducts()
        {
            _products =
                MihailenkoMiracleShoesEntities
                .GetContext()
                .Product
                .ToList();
            ProductList.ItemsSource = _products;
            TBCount.Text =
                "Количество: " + _products.Count;
        }

        private void LoadCategories()
        {
            FilteringCB.Items.Clear();
            FilteringCB.Items.Add("Все категории");
            var categories =
                MihailenkoMiracleShoesEntities
                .GetContext()
                .Category
                .ToList();
            foreach (var category in categories)
            {
                FilteringCB.Items.Add(category.CategoryName);
            }
            FilteringCB.SelectedIndex = 0;
        }

        private void SearchingTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void FilteringCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();
        }


        private void SortUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProduct();
        }

        private void SortDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProduct();
        }


        private void UpdateProduct()
        {
            if (_products == null)
                return;

            var result = _products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchingTB.Text))
            {
                string text =
                    SearchingTB.Text.ToLower();


                result =
                    result.Where(x =>
                    x.ProductName
                    .ToLower()
                    .Contains(text));
            }

            if (FilteringCB.SelectedIndex > 0)
            {
                string category =
                    FilteringCB.SelectedItem.ToString();


                result =
                    result.Where(x =>
                    x.Category.CategoryName == category);
            }


            if (SortUp.IsChecked == true)
            {
                result =
                    result.OrderBy(x => x.Price);
            }



            if (SortDown.IsChecked == true)
            {
                result =
                    result.OrderByDescending(x => x.Price);
            }

            ProductList.ItemsSource =
                result.ToList();

            TBCount.Text =
                "Количество: " + result.Count();

        }
        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentSession.CurrentUser = null;


            Manager.MainFrame.Navigate(
                new AuthPage());
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(
                new AddEditPage());
        }

    }
}