using Lab4ParshArestGol.Core;
using Microsoft.Win32;
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
    /// Логика взаимодействия для ApplicationPage.xaml
    /// </summary>
    public partial class ApplicationPage : Page
    {
        private string _finalPhotoPath = "/Images/default.png";
        private int _currentUserId = UserSession.CurrentUserId;

        public ApplicationPage()
        {
            InitializeComponent();
            ConfigurePlaceholders();
        }

        private void ConfigurePlaceholders()
        {
            TextBox[] fields = {
                NameField, SpeciesField, BreedField, AgeMonthsField,
                WeightField, VaccinationsField, ColorField, PhotoField, DescriptionField
            };

            foreach (var tb in fields)
            {
                tb.Tag = tb.Text;
                tb.GotFocus += TextBox_GotFocus;
                tb.LostFocus += TextBox_LostFocus;
            }
        }

        public void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null && tb.Tag != null && tb.Text == tb.Tag.ToString())
            {
                tb.Text = "";
            }
        }

        public void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null && tb.Tag != null && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = tb.Tag.ToString();
            }
        }

        private void PhotoField_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Изображения (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string sourceFile = openFileDialog.FileName;
                    string targetFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

                    if (!System.IO.Directory.Exists(targetFolder))
                    {
                        System.IO.Directory.CreateDirectory(targetFolder);
                    }

                    string extension = System.IO.Path.GetExtension(sourceFile);
                    string uniqueName = Guid.NewGuid().ToString() + extension;
                    string destinationFile = System.IO.Path.Combine(targetFolder, uniqueName);

                    System.IO.File.Copy(sourceFile, destinationFile, true);

                    _finalPhotoPath = "/Images/" + uniqueName;
                    PhotoField.Text = _finalPhotoPath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке фото: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SumbitBtn_Click(object sender, RoutedEventArgs e)
        {
            TextBox[] requiredTextFields = { NameField, SpeciesField, BreedField, AgeMonthsField, WeightField, VaccinationsField, ColorField, DescriptionField };
            foreach (var tb in requiredTextFields)
            {
                if (string.IsNullOrWhiteSpace(tb.Text) || tb.Text == tb.Tag.ToString())
                {
                    MessageBox.Show($"Пожалуйста, заполните поле: {tb.Tag}", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            if (GenderField.SelectedIndex == 0 || GenderField.SelectedIndex == -1)
            {
                MessageBox.Show("Пожалуйста, выберите ПОЛ животного!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int ageMonths;
            if (!int.TryParse(AgeMonthsField.Text, out ageMonths) || ageMonths < 0)
            {
                MessageBox.Show("Возраст должен быть целым положительным числом (кол-во месяцев)!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string weightInput = WeightField.Text.Replace('.', ',');
            decimal weight;
            if (!decimal.TryParse(weightInput, out weight) || weight <= 0)
            {
                MessageBox.Show("Вес должен быть положительным числом (например: 5,2 или 12)!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (DescriptionField.Text.Length > 100)
            {
                MessageBox.Show("Описание не должно превышать 100 символов!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int genderCode = 0;
            if (GenderField.SelectedIndex == 1)
            {
                genderCode = 1;
            }

            string query = @"INSERT INTO Requests (userid, request_type, TempName, TempSpecies, TempGender, TempBreed, TempColor, TempAgeMonths, TempWeight, TempDescription, TempPhotoPath, TempVaccinations, status) 
                             VALUES (@userId, @requestType, @name, @species, @gender, @breed, @color, @age, @weight, @desc, @photo, @vacc, N'В обработке');";

            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", _currentUserId);
                        command.Parameters.AddWithValue("@requestType", "Прием");
                        command.Parameters.AddWithValue("@name", NameField.Text.Trim());
                        command.Parameters.AddWithValue("@species", SpeciesField.Text.Trim());
                        command.Parameters.AddWithValue("@gender", genderCode);
                        command.Parameters.AddWithValue("@breed", BreedField.Text.Trim());
                        command.Parameters.AddWithValue("@color", ColorField.Text.Trim());
                        command.Parameters.AddWithValue("@age", ageMonths);
                        command.Parameters.AddWithValue("@weight", weight);
                        command.Parameters.AddWithValue("@desc", DescriptionField.Text.Trim());
                        command.Parameters.AddWithValue("@photo", _finalPhotoPath);
                        command.Parameters.AddWithValue("@vacc", VaccinationsField.Text.Trim());

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Заявка успешно отправлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при работе с базой данных: {ex.Message}", "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BckToMnBtn_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}
