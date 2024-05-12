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
    public partial class ManipulationPage : Page
    {
        public ManipulationPage()
        {
            InitializeComponent();
            SetUp();
            WorldSkillsContext worldSkillsContext = new WorldSkillsContext();
            if (worldSkillsContext.Requests.Count() == 0)
            {
                RequestIdTextBox.Text = "1";
            }
            else
            {
                RequestIdTextBox.Text = (worldSkillsContext.Requests.Count() + 1).ToString();
            }
        }
        public ManipulationPage(Request request)
        {
            InitializeComponent();
            SetUp();
            if (User.ActiveUser.RoleId == 2)
            {
                TitleTextBox.IsEnabled = LevelTextBox.IsEnabled = ExecutorComboBox.IsEnabled
                    = ClientCompleteNameTextBox.IsEnabled = ClientPhoneTextBox.IsEnabled = DeleteExecutorButton.IsEnabled = false;
            }
            RequestIdTextBox.Text = request.RequestId.ToString();
            TitleTextBox.Text = request.Title.ToString();
            ClientCompleteNameTextBox.Text = request.ClientCompleteName;
            ClientPhoneTextBox.Text = request.ClientPhone;
            LevelTextBox.Text = request.Level;
            AdditionalTextBox.Text = request.Additional;
            StatusComboBox.SelectedIndex = (request.StatusId ?? 0) - 1;
            ExecutorComboBox.SelectedItem = request.ExecutorId;
        }
        private void SetUp()
        {
            WorldSkillsContext worldSkillsContext = new WorldSkillsContext();
            StatusComboBox.ItemsSource = worldSkillsContext.Statuses.OrderBy(s => s.StatusId).Select(s => s.Title).ToList();
            ExecutorComboBox.ItemsSource = worldSkillsContext.Users.Where(u => u.RoleId == 2).Select(u => u.UserId).ToList();
        }
        private void DeleteExecutorButton_Click(object sender, RoutedEventArgs e)
        {
            ExecutorComboBox.SelectedIndex = -1;
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (StatusComboBox.SelectedIndex != -1 && ClientCompleteNameTextBox.Text.Length < 150 && ClientPhoneTextBox.Text.Length <= 11
                && TitleTextBox.Text.Length < 100 && LevelTextBox.Text.Length < 100 && AdditionalTextBox.Text.Length < 100)
            {
                WorldSkillsContext worldSkillsContext = new WorldSkillsContext();
                Request? request = worldSkillsContext.Requests.FirstOrDefault(r => r.RequestId == Convert.ToInt32(RequestIdTextBox.Text));
                if (request != null)
                {
                    worldSkillsContext.Requests.Remove(request);
                }
                request = new Request(
                    Convert.ToInt32(RequestIdTextBox.Text),
                    StatusComboBox.SelectedIndex + 1,
                    ExecutorComboBox.SelectedItem != null ? Convert.ToInt32(ExecutorComboBox.SelectedItem.ToString()) : null,
                    ClientCompleteNameTextBox.Text,
                    ClientPhoneTextBox.Text,
                    TitleTextBox.Text,
                    LevelTextBox.Text,
                    AdditionalTextBox.Text);
                worldSkillsContext.Requests.Add(request);
                worldSkillsContext.SaveChanges();
                MainWindow.Frame.Content = new ViewPage();
            }
            else
            {
                MessageBox.Show("Данные содержат ошибку");
            }
        }
    }
}
