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
using System.Data.Entity;
namespace Unik2.Admin_Pages
{
    /// <summary>
    /// Логика взаимодействия для AddTeachers.xaml
    /// </summary>
    public partial class AddTeachers : Page
    {
       
            private Unik2Entities _context = Unik2Entities.GetContext();
            private Teachers _currentTeacher;

            public AddTeachers()
            {
                InitializeComponent();
                LoadData();
            }

            private void LoadData()
            {
                _context.Teachers.Load();
                _context.Category.Load();
                _context.Users.Load();

                // ComboBox выбора преподавателя
                TeachersComboBox.ItemsSource = _context.Teachers.Local;

                // ComboBox категорий
                CategoryCombobox.ItemsSource = _context.Category.Local;
            }

            private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                _currentTeacher = TeachersComboBox.SelectedItem as Teachers;

                if (_currentTeacher != null)
                {
                    TbFio.Text = _currentTeacher.Fio;
                    TbStreet.Text = _currentTeacher.Street;
                    TbHouse.Text = _currentTeacher.House;
                    TbApartament.Text = _currentTeacher.Apartment;
                    CategoryCombobox.SelectedValue = _currentTeacher.Id_category;

                    if (_currentTeacher.Users != null)
                    {
                        TbLogin.Text = _currentTeacher.Users.Login;
                        TbPassword.Text = _currentTeacher.Users.Password;
                    }
                }
            }

            private void SaveButton_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(TbFio.Text))
                    {
                        MessageBox.Show("Введите ФИО");
                        return;
                    }

                    if (_currentTeacher == null)
                    {
                        _currentTeacher = new Teachers();
                        _context.Teachers.Add(_currentTeacher);

                        // создаём пользователя
                        Users user = new Users
                        {
                            Login = TbLogin.Text.Trim(),
                            Password = TbPassword.Text.Trim(),
                            Id_role = 2 // роль преподавателя
                        };

                        _context.Users.Add(user);
                        _currentTeacher.Users = user;
                    }
                    else
                    {
                        // обновление пользователя
                        _currentTeacher.Users.Login = TbLogin.Text.Trim();
                        _currentTeacher.Users.Password = TbPassword.Text.Trim();
                    }

                    _currentTeacher.Fio = TbFio.Text.Trim();
                    _currentTeacher.Street = TbStreet.Text.Trim();
                    _currentTeacher.House = TbHouse.Text.Trim();
                    _currentTeacher.Apartment = TbApartament.Text.Trim();
                    _currentTeacher.Id_category = (int)CategoryCombobox.SelectedValue;

                    _context.SaveChanges();
                    LoadData();

                    MessageBox.Show("Преподаватель сохранён!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }

            private void NewButton_Click(object sender, RoutedEventArgs e)
            {
                TeachersComboBox.SelectedIndex = -1;
                _currentTeacher = null;

                TbFio.Clear();
                TbStreet.Clear();
                TbHouse.Clear();
                TbApartament.Clear();
                TbLogin.Clear();
                TbPassword.Clear();
                CategoryCombobox.SelectedIndex = -1;
            }

            private void DeleteButton_Click(object sender, RoutedEventArgs e)
            {
                if (_currentTeacher == null)
                {
                    MessageBox.Show("Выберите преподавателя для удаления!");
                    return;
                }

                if (MessageBox.Show(
                    $"Удалить преподавателя {_currentTeacher.Fio}?",
                    "Подтверждение",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    // удаляем пользователя
                    if (_currentTeacher.Users != null)
                        _context.Users.Remove(_currentTeacher.Users);

                    _context.Teachers.Remove(_currentTeacher);
                    _context.SaveChanges();

                    LoadData();
                    NewButton_Click(null, null);

                    MessageBox.Show("Преподаватель удалён!");
                }
            }
        }
    }

