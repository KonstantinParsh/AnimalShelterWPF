using Lab4ParshArestGol.Models;
using System;
using System.Collections.Generic;
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
        private List<Animal> allAnimals = new List<Animal>();
        private int currentPage = 0;
        private const int pageSize = 3;

        public CardsPage()
        {
            InitializeComponent();
        }

        public void LoadData()
        {
            
        }

        public void UpdateCards()
        {
            var pageData = allAnimals
                .Skip(currentPage * pageSize)
                .Take(pageSize)
                .ToList();

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
    }
}
