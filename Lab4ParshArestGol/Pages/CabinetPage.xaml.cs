using Lab4ParshArestGol.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace Lab4ParshArestGol.Pages
{
    /// <summary>
    /// Логика взаимодействия для CabinetPage.xaml
    /// </summary>
    public partial class CabinetPage : Page
    {
        public CabinetPage()
        {
            InitializeComponent();
            LoadUserData();
            ConfigureAccess();
        }

        private void LoadUserData()
        {
            string query = "SELECT fullName, phone, password, login FROM Users WHERE userId = @UserId";

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", UserSession.CurrentUserId);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string fullName = reader["fullName"].ToString();
                                PhoneNumber.Text = reader["phone"].ToString();
                                Password.Text = reader["password"].ToString(); 

                                string[] parts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                                FirstName.Text = parts.Length > 0 ? parts[0] : "—";
                                SecondName.Text = parts.Length > 1 ? parts[1] : "—";
                                ThirdName.Text = parts.Length > 2 ? parts[2] : "—";

                                Login.Text = "@" + reader["login"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Пользователь не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при загрузке профиля из БД: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void ConfigureAccess()
        {
            int currentRole = UserSession.CurrentRoleId;

            MedRecordsBtn.Visibility = Visibility.Collapsed;
            RequestsBtn.Visibility = Visibility.Collapsed;
            RecordBtn.Visibility = Visibility.Collapsed;

            if (currentRole == 1)
            {
                RequestsBtn.Visibility = Visibility.Visible;
                RecordBtn.Visibility = Visibility.Visible;
            }
            else if (currentRole == 3)
            {
                RequestsBtn.Visibility = Visibility.Visible;
            }
            else if (currentRole == 4)
            {
                MedRecordsBtn.Visibility = Visibility.Visible;
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение выхода",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                UserSession.Logout();
                NavigationService.Navigate(new StartPage());
            }
            else if (result == MessageBoxResult.No)
            {
                return;
            }
        }

        private void RecordBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RequestsBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new RequestPage());
        }

        private void MedRecordsBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
