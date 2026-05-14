using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
using System.Windows.Shapes;

namespace Lab4ParshArestGol.Pages
{
    public partial class RegWindow : Window
    {
        public RegWindow()
        {
            InitializeComponent();
        }

        public void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null && tb.Text == tb.Tag.ToString())
            {
                tb.Text = "";
            }
        }

        public void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = tb.Tag.ToString();
            }
        }

        private void RegBut_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            string firstName = FirstNameField.Text.Trim();
            string secondName = SecondNameField.Text.Trim();
            string phoneNumber = PhoneNumberField.Text.Trim();

            string thirdNameRaw = ThirdNameField.Text.Trim();
            
            string login = LoginField.Text.Trim();
            string password = PasswordField.Text.Trim();
            string passwordRepeat = PasswordRepeatField.Text.Trim();
            var selectedRole = (RoleField.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (firstName == "" || firstName == FirstNameField.Tag?.ToString() ||
                secondName == "" || secondName == SecondNameField.Tag?.ToString() ||
                phoneNumber == "" || phoneNumber == PhoneNumberField.Tag?.ToString() ||
                login == "" || login == LoginField.Tag?.ToString() ||
                password == "" || password == PasswordField.Tag?.ToString() ||
                passwordRepeat == "" || passwordRepeat == PasswordRepeatField.Tag?.ToString() ||
                selectedRole == null || selectedRole == "ВЫБЕРИТЕ РОЛЬ*")
            {
                MessageBox.Show("Все поля со звездочкой должны быть заполнены!", "Ошибка ввода данных");
                return;
            }
            if (password != passwordRepeat)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка ввода данных");
                return;
            }

            object thirdNameValue;
            if (thirdNameRaw == ThirdNameField.Tag?.ToString() || string.IsNullOrWhiteSpace(thirdNameRaw))
            {
                thirdNameValue = DBNull.Value;
            }
            else
            {
                thirdNameValue = thirdNameRaw;
            }
            string fullName = $"{firstName} {secondName}";
            if (thirdNameValue != DBNull.Value)
            {
                fullName += $" {thirdNameValue}";
            }

            int roleId = 2;
            if (selectedRole == "Администратор") roleId = 1;
            else if (selectedRole == "Волонтер") roleId = 3;
            else if (selectedRole == "Ветеринарный врач") roleId = 4;
            
            string query = @"INSERT INTO Users (fullName, phone, role_id, login, password) VALUES (@FullName, @Phone, @RoleId, @Login, @Password)";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FullName", fullName);
                        command.Parameters.AddWithValue("@Phone", phoneNumber);
                        command.Parameters.AddWithValue("@RoleId", roleId);
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Password", password);

                        connection.Open();
                        command.ExecuteNonQuery();

                        MessageBox.Show("Peгистрация завершена. Пользователь добавлен в базу.");
                        this.Close();
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Ошибка при сохранении в базу: {ex.Message}");
            }
        }

        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LogInBut_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow();
            this.Close();
            authWindow.Show();
        }

        private void PhoneNumberField_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }
    }
}
