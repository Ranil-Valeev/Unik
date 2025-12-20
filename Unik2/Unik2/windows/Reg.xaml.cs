using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Unik2.model;

namespace Unik2.windows
{
    /// <summary>
    /// Логика взаимодействия для Reg.xaml
    /// </summary>
    public partial class Reg : Window
    {
        private Unik2Entities _context = Unik2Entities.GetContext();
        //private Category _currentCat;
        public Reg()
        {
            InitializeComponent();
            LoadCombobox();
        }
        private void LoadCombobox()
        {
            _context.Category.Load();
            CbCategory.ItemsSource = _context.Category.Local;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(RegLogin.Text) || string.IsNullOrWhiteSpace(RegPass.Password) || CbCategory.SelectedItem == null) {
                    MessageBox.Show("Заполните логин, пароль и выберите категорию!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                string passwords = RegPass.Password;
                var passwordregex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&/.,#])[A-Za-z\d@$!%*?&/.,#]{8,}$"); 
                if (!passwordregex.IsMatch(passwords))
                {
                    MessageBox.Show("Пароль не соответствует требованиям: нужны заглавные и строчные буквы, цифры и спецсимволы!", "Слабый пароль", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                Users newUser = new Users();
                newUser.Login = RegLogin.Text.Trim();
                newUser.Password = passwords;
                newUser.Id_role = 1;

                Teachers newTeacher = new Teachers();
                newTeacher.Fio = RegFio.Text.Trim();
                newTeacher.Street = RegStreet.Text.Trim();
                newTeacher.House = RegHouse.Text.Trim();
                newTeacher.Apartment = RegApartment.Text.Trim();

                var selectedCat = CbCategory.SelectedItem as Category;
                newTeacher.Id_category = selectedCat.Id_category;

                newTeacher.Users = newUser;

                _context.Users.Add(newUser);
                _context.Teachers.Add(newTeacher);

                _context.SaveChanges();

                MessageBox.Show("Регистрация успешно завершена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
