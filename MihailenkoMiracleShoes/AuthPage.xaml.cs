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

namespace MihailenkoMiracleShoes
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        private MihailenkoMiracleShoesEntities _db = new MihailenkoMiracleShoesEntities();
        private MessageHelper _mh = new MessageHelper();

        public AuthPage()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text;

            var user = _db.Client
                .Where(u => u.Login == login)
                .FirstOrDefault();


            if (user == null)
            {
                _mh.ShowError("Введён неправильный логин!");
                return;
            }


            CurrentSession.CurrentUser = user;


            _mh.ShowInfo($"Добро пожаловать, {user.Login}!");


            Manager.MainFrame.Navigate(new ProductPage(user));
        }
    }
}
