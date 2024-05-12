using Microsoft.EntityFrameworkCore;
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
using WorldSkills.Models;

namespace WorldSkills
{
    /// <summary>
    /// Interaction logic for ViewPage.xaml
    /// </summary>
    public partial class ViewPage : Page
    {
        private List<Request> requests;
        private List<Request> viewRequests = new List<Request>();
        public ViewPage()
        {
            InitializeComponent();
            WorldSkillsContext worldSkillsContext = new WorldSkillsContext();
            RequestsCountLabel.Content = worldSkillsContext.Requests.Count().ToString();
            StatusComboBox.ItemsSource = worldSkillsContext.Statuses.OrderBy(s => s.StatusId).Select(s => s.Title).ToList();
            if (User.ActiveUser.RoleId == 1)
            {
                requests = worldSkillsContext.Requests
                    .Include(r => r.Status)
                    .ToList();
            }
            else
            {
                requests = worldSkillsContext.Requests
                    .Where(r => r.ExecutorId == User.ActiveUser.UserId)
                    .Include(r => r.Status)
                    .ToList();
                CreateButton.IsEnabled = false;
            }
            UpdateView();
        }

        private void UpdateView()
        {
            viewRequests.Clear();
            foreach(Request request in requests)
            {
                if (request.Title.ToLower().Contains(SearchTextBox.Text.ToLower()) &&
                    (StatusComboBox.SelectedIndex + 1 == request.StatusId || StatusComboBox.SelectedIndex == -1))
                {
                    viewRequests.Add(request);
                }
            }
            ViewRequestsCountLabel.Content = viewRequests.Count().ToString();
            MainDataGrid.ItemsSource = null;
            MainDataGrid.ItemsSource = viewRequests;
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateView();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Frame.Content = new ManipulationPage((sender as FrameworkElement).DataContext as Request);
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Frame.Content = new ManipulationPage();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
            StatusComboBox.SelectedIndex = -1;
            UpdateView();
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateView();
        }
    }
}
