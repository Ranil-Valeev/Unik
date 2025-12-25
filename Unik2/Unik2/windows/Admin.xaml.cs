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
using System.Windows.Shapes;
using Unik2.Admin_Pages;
using Unik2.Pages;

namespace Unik2.windows
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        public Admin()
        {
            InitializeComponent();
            MainFrame.Navigate(new Admin_Pages.AddTeachers());
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (!(MainFrame.Content is AddTeachers))
            {
                MainFrame.Navigate(new Admin_Pages.AddTeachers());
            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }
    }
}
