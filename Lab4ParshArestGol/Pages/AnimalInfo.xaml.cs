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
    /// Логика взаимодействия для AnimalInfo.xaml
    /// </summary>
    public partial class AnimalInfo : Page
    {
        private int _animalId;

        public AnimalInfo(int animalId)
        {
            InitializeComponent();
            _animalId = animalId;
            LoadAnimalDetails();
        }

        private void LoadAnimalDetails()
        {
            string query = "SELECT * FROM AnimalCard WHERE animalId = @Id";

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", _animalId);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Animal animal = new Animal
                            {
                                Id = Convert.ToInt32(reader["animalId"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                PhotoPath = reader["PhotoPath"].ToString(),
                                Species = reader["Species"].ToString(),
                                Gender = Convert.ToInt32(reader["Gender"]),
                                Breed = reader["Breed"].ToString(),
                                Color = reader["Color"].ToString(),
                                AgeMonths = Convert.ToInt32(reader["age_months"]),
                                Weight = Convert.ToDecimal(reader["an_weight"]),
                                Vaccinations = reader["Vaccinations"].ToString()
                            };
                            this.DataContext = animal;
                        }
                        else
                        {
                            MessageBox.Show("Животное не найдено в базе данных!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при работе с базой данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void TakeHmBt_Click(object sender, RoutedEventArgs e)
        {
            if (!UserSession.IsAuthorized())
            {
                MessageBox.Show("Вы должны войти в аккаунт, чтобы забрать животное!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (this.DataContext is Animal currentAnimal)
            {
                string query = @"INSERT INTO Requests (userid, animal_id, request_date, request_type, TempName, TempSpecies, TempGender, TempBreed, TempColor, TempAgeMonths, TempWeight, TempDescription, TempPhotoPath) 
VALUES (@userid, @animal_id, @request_date, @request_type, @name, @species, @gender, @breed, @color, @age_months, @weight, @description, @photopath)";

                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userid", UserSession.CurrentUserId);
                        command.Parameters.AddWithValue("@animal_id", currentAnimal.Id);
                        command.Parameters.AddWithValue("@request_date", DateTime.Now);
                        command.Parameters.AddWithValue("@request_type", "Адопция");
                        command.Parameters.AddWithValue("@name", currentAnimal.Name);
                        command.Parameters.AddWithValue("@species", currentAnimal.Species);
                        command.Parameters.AddWithValue("@gender", currentAnimal.Gender);
                        command.Parameters.AddWithValue("@breed", currentAnimal.Breed);
                        command.Parameters.AddWithValue("@color", currentAnimal.Color);
                        command.Parameters.AddWithValue("@age_months", currentAnimal.AgeMonths);
                        command.Parameters.AddWithValue("@weight", currentAnimal.Weight);
                        command.Parameters.AddWithValue("@description", currentAnimal.Description);
                        command.Parameters.AddWithValue("@photopath", currentAnimal.PhotoPath);
                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            MessageBox.Show($"Заявка на питомца {currentAnimal.Name} успешно отправлена! Админ свяжется с вами.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка отправки заявки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }
    }
}
