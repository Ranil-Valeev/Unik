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
using Unik2.model;
using Unik2.windows;

namespace Unik2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new Unik2Entities())
            {
                var users = db.Users.FirstOrDefault(u => u.Login == TbLogin.Text && u.Password == Tbpassword.Text);
                if (users == null)
                {
                    MessageBox.Show("Неверный логин или пароль.");
                    return;
                } else if (users.Id_role == 1)
                {
                    TeacherWindow teachers = new TeacherWindow();
                    teachers.Show();
                    this.Close();
                } else if (users.Id_role == 2)
                {
                    Admin admin = new Admin();
                    admin.Show();
                    this.Close();
                }
            }
        }

        private void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            windows.Reg reg = new windows.Reg();
            reg.Show();
            this.Close();
        }
    }
}
