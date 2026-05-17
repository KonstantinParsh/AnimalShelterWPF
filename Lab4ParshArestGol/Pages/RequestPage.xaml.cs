using Lab4ParshArestGol.Core;
using Lab4ParshArestGol.Models;
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
    /// Логика взаимодействия для RequestPage.xaml
    /// </summary>
    public partial class RequestPage : Page
    {
        private List<RequestItem> allRequests = new List<RequestItem>();
        private int currentPage = 0;
        private const int pageSize = 4;
        CardsPage cardsPage;

        public RequestPage()
        {
            InitializeComponent();
            LoadRequests();
        }

        private void LoadRequests()
        {
            allRequests.Clear();
            string query = "SELECT * FROM Requests WHERE status = N'В обработке';";

            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                allRequests.Add(new RequestItem
                                {
                                    AnimalId = reader["animal_id"] != DBNull.Value ? Convert.ToInt32(reader["animal_id"]) : 0,
                                    RequestId = reader["request_id"] != DBNull.Value ? Convert.ToInt32(reader["request_id"]) : 0,
                                    UserId = reader["userid"] != DBNull.Value ? Convert.ToInt32(reader["userid"]) : 0,
                                    ClientName = reader["userid"] != DBNull.Value ? $"Пользователь {reader["userid"]}" : "—",
                                    AnimalName = reader["TempName"] != DBNull.Value && !string.IsNullOrWhiteSpace(reader["TempName"].ToString()) ? reader["TempName"].ToString() : "—",
                                    AnimalType = reader["TempSpecies"] != DBNull.Value && !string.IsNullOrWhiteSpace(reader["TempSpecies"].ToString()) ? reader["TempSpecies"].ToString() : "—",
                                    AnimalBreed = reader["TempBreed"] != DBNull.Value && !string.IsNullOrWhiteSpace(reader["TempBreed"].ToString()) ? reader["TempBreed"].ToString() : "—",
                                    RequestDate = reader["request_date"] != DBNull.Value ? Convert.ToDateTime(reader["request_date"]).ToString("yyyy-MM-dd") : "—",
                                    RequestType = reader["request_type"] != DBNull.Value ? reader["request_type"].ToString() : "—"
                                });
                            }
                        }
                    }
                }

                if (currentPage * pageSize >= allRequests.Count && currentPage > 0)
                {
                    currentPage--;
                }

                DisplayCurrentPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка БД: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayCurrentPage()
        {
            RequestsControl.ItemsSource = allRequests.Skip(currentPage * pageSize).Take(pageSize).ToList();

            if (BtnBack != null)
            {
                BtnBack.Visibility = (currentPage == 0) ? Visibility.Collapsed : Visibility.Visible;
            }

            if (BtnNwxt != null)
            {
                BtnNwxt.Visibility = ((currentPage + 1) * pageSize >= allRequests.Count) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            RequestItem item = btn.DataContext as RequestItem;
            if (item == null) return;

            int requestId = item.RequestId;
            string requestType = item.RequestType;
            int animalId = item.AnimalId;

            string query = "";

            if (requestType == "Прием")
            {
                query = @"
            INSERT INTO AnimalCard (name, species, gender, breed, color, age_months, an_weight, description, photopath, vaccinations)
            SELECT TempName, TempSpecies, TempGender, TempBreed, TempColor, TempAgeMonths, TempWeight, TempDescription, TempPhotoPath, TempVaccinations
            FROM Requests
            WHERE request_id = @ReqId;

            UPDATE Requests 
            SET status = N'Одобрено' 
            WHERE request_id = @ReqId;";
            }
            else if (requestType == "Адопция")
            {
                query = @"
    DELETE FROM Requests 
    WHERE animal_id = @AnimalId;

    DELETE FROM AnimalCard 
    WHERE animalId = @AnimalId;";
            }
            else
            {
                query = "UPDATE Requests SET status = N'Одобрено' WHERE request_id = @ReqId;";
            }

            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReqId", requestId);
                        command.Parameters.AddWithValue("@AnimalId", animalId);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                LoadRequests();
                
                if (currentPage * pageSize >= allRequests.Count && currentPage > 0)
                {
                    currentPage--;
                }

                DisplayCurrentPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обработке заявки ({requestType}): {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RejectBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            RequestItem item = btn.DataContext as RequestItem;
            if (item == null) return;

            int requestId = item.RequestId;

            string query = "DELETE FROM Requests WHERE request_id = @ReqId;";

            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReqId", requestId);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                LoadRequests();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отклонении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 0)
            {
                currentPage--;
                DisplayCurrentPage();
            }
        }

        private void BtnNwxt_Click(object sender, RoutedEventArgs e)
        {
            if ((currentPage + 1) * pageSize < allRequests.Count)
            {
                currentPage++;
                DisplayCurrentPage();
            }
        }
    }
}
