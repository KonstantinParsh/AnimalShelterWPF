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
using System.Windows.Shapes;

namespace Lab4ParshArestGol.Pages
{
    /// <summary>
    /// Логика взаимодействия для ConfirmRequestWindow.xaml
    /// </summary>
    public partial class ConfirmRequestWindow : Window
    {
        private int _requestId;
        public int IsConfirmed = 0;

        public ConfirmRequestWindow(int requestId)
        {
            InitializeComponent();
            _requestId = requestId;
            AppointmentDatePicker.SelectedDate = DateTime.Now.AddDays(1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (AppointmentDatePicker.SelectedDate == null) return;

            string query = "UPDATE Requests SET status = N'Одобрено', request_date = @NewDate WHERE request_id = @RequestId";

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NewDate", AppointmentDatePicker.SelectedDate.Value);
                    command.Parameters.AddWithValue("@RequestId", _requestId);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        IsConfirmed = 1;
                        Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
