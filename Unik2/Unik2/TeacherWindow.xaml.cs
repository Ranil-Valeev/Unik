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
using Unik2.Pages;

namespace Unik2
{
    /// <summary>
    /// Логика взаимодействия для TeacherWindow.xaml
    /// </summary>
    public partial class TeacherWindow : Window
    {
        public TeacherWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Pages.TeacherPage());
        }
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Pages.LoadPage());
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (!(MainFrame.Content is TeacherPage))
            {
                MainFrame.Navigate(new Pages.TeacherPage());
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
