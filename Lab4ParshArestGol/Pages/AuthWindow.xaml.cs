using Lab4ParshArestGol.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
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
using System.Windows.Threading;

namespace Lab4ParshArestGol.Pages
{
    public partial class AuthWindow : Window
    {
        private int _failedAttempts = 0;
        private int _cooldownSeconds = 30;
        private DispatcherTimer _cooldownTimer;

        public AuthWindow()
        {
            InitializeComponent();
            InitializeTimer();
        }

        private void PasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordTextBox.Text == "Password") PasswordTextBox.Text = "";
        }

        private void PasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordTextBox.Text == "") PasswordTextBox.Text = "Password";
        }

        private void LogInTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LogInTextBox.Text == "Login") LogInTextBox.Text = "";
        }

        private void LogInTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LogInTextBox.Text == "") LogInTextBox.Text = "Login";
        }

        private void InitializeTimer()
        {
            _cooldownTimer = new DispatcherTimer();
            _cooldownTimer.Interval = TimeSpan.FromSeconds(1);
            _cooldownTimer.Tick += CooldownTimer_Tick;
        }

        private void StartCooldown()
        {
            LogInBtn.IsEnabled = false;
            LogInTextBox.IsEnabled = false;
            PasswordTextBox.IsEnabled = false;

            PasswordTextBox.Text = "Password";
            LogInTextBox.Text = "Login";

            _cooldownSeconds = 30;

            TimerText.Text = $"Слишком много попыток. Подождите {_cooldownSeconds} с";
            TimerText.Visibility = Visibility.Visible;

            _cooldownTimer.Start();
        }

        private void CooldownTimer_Tick(object sender, EventArgs e)
        {
            _cooldownSeconds--;
            TimerText.Text = $"Слишком много попыток. Подождите {_cooldownSeconds} с";

            if (_cooldownSeconds <= 0)
            {
                _cooldownTimer.Stop();
                LogInBtn.IsEnabled = true;
                TimerText.Visibility = Visibility.Collapsed;
                _failedAttempts = 0;
            }
        }

        private void LogInBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LogInTextBox.Text;
            string password = PasswordTextBox.Text;

            bool isUsernameEmpty = string.IsNullOrWhiteSpace(login) || login == "Login";
            bool isPasswordEmpty = string.IsNullOrWhiteSpace(password) || password == "Password";

            if (isUsernameEmpty || isPasswordEmpty)
            {
                MessageBox.Show("Пожалуйста, заполните все поля. Подсказки не являются данными для входа", "Забывашка!");
                return;
            }

            bool isLogInSuccessful = false;

            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT userId, fullName, role_id FROM Users WHERE Login = @login AND Password = @pass";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@pass", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isLogInSuccessful = true;

                            UserSession.CurrentUserId = Convert.ToInt32(reader["userId"]);
                            UserSession.CurrentUserFullName = reader["fullName"].ToString();
                            UserSession.CurrentRoleId = Convert.ToInt32(reader["role_id"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка БД: " + ex.Message);
                return;
            }

            if (isLogInSuccessful)
            {
                MessageBox.Show($"Добро пожаловать, {UserSession.CurrentUserFullName}!");
                _failedAttempts = 0;

                this.DialogResult = true;
                this.Close();
            }
            else
            {
                _failedAttempts++;

                if (_failedAttempts >= 3)
                {
                    StartCooldown();
                }
                else
                {
                    TimerText.Text = "Неверный логин или пароль. Повторите попытку";
                    TimerText.Visibility = Visibility.Visible;
                }
            }
        }

        private void BackToMainBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            RegWindow regwin = new RegWindow();
            regwin.ShowDialog();
            this.Close();
        }

        private void NoSpaces_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
