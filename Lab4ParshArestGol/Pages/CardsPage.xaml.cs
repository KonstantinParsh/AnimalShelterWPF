using Lab4ParshArestGol.Core;
using Lab4ParshArestGol.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
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
    /// Логика взаимодействия для CardsPage.xaml
    /// </summary>
    public partial class CardsPage : Page
    {
        public List<Animal> allAnimals = new List<Animal>();
        private int currentPage = 0;
        private const int pageSize = 4;

        public CardsPage()
        {
            InitializeComponent();
            LoadData();    
            UpdateCards();
        }

        public void LoadData()
        {
            string query = "SELECT animalId, name, species, gender, breed, color, age_months, an_weight, description, photopath, vaccinations FROM AnimalCard";

            try
            {
                allAnimals.Clear();

                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Animal animal = new Animal
                            {
                                Id = Convert.ToInt32(reader["animalId"]),
                                Name = reader["name"].ToString(),
                                Species = reader["species"].ToString(),
                                Gender = Convert.ToInt32(reader["gender"]),
                                Breed = reader["breed"].ToString(),
                                Color = reader["color"].ToString(),
                                AgeMonths = Convert.ToInt32(reader["age_months"]),
                                Weight = Convert.ToDecimal(reader["an_weight"]),
                                Description = reader["description"].ToString(),
                                PhotoPath = reader["photopath"].ToString(),

                                Vaccinations = reader["vaccinations"] != DBNull.Value ? reader["vaccinations"].ToString() : ""
                            };

                            allAnimals.Add(animal);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка БД: {ex.Message}", "Ошибка подключения",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void UpdateCards()
        {
            var pageData = allAnimals
                .Skip(currentPage * pageSize)
                .Take(pageSize)
                .ToList();

            AnimalsControl.ItemsSource = null;
            AnimalsControl.ItemsSource = pageData;
            UpdateNavigateButtons();
        }
        public void UpdateNavigateButtons()
        {
            int totalPages = (int)Math.Ceiling((double)allAnimals.Count / pageSize);
            BtnBack.Visibility = (currentPage == 0) ? Visibility.Hidden : Visibility.Visible;
            BtnNwxt.Visibility = (currentPage >= totalPages - 1) ? Visibility.Hidden : Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Animal selectedAnimal)
            {
                this.NavigationService.Navigate(new AnimalInfo(selectedAnimal.Id));
            };
        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 0)
            {
                currentPage--;
                UpdateCards();
            }
        }

        private void BtnNwxt_Click(object sender, RoutedEventArgs e)
        {
            int totalPages = (int)Math.Ceiling((double)allAnimals.Count / pageSize);
            if (currentPage < totalPages - 1)
            {
                currentPage++;
                UpdateCards();
            }
        }

        private void BckToMnBtn_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void MkARequestBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!UserSession.IsAuthorized())
            {
                MessageBox.Show("Вы должны войти в аккаунт, чтобы подать заявку!", "Доступ ограничен", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                if (UserSession.CurrentRoleId == 1 || UserSession.CurrentRoleId == 3)
                {
                    this.NavigationService.Navigate(new AdminApplicationPage());
                }
                else
                {
                    this.NavigationService.Navigate(new ApplicationPage());
                }
            }
        }
    }
}
