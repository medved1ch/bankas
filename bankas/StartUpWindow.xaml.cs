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
using System.Windows.Navigation;
using System.Windows.Shapes;
using bankas.Connection;
using bankas.Classes;

namespace bankas
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class StartUpWindow : Window
    {
        public StartUpWindow()
        {
            InitializeComponent();
        }

        private void BtnCloseApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void BtnSignIn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(TbMailIn.Text) || String.IsNullOrEmpty(PbPass.Password))
            {
                MessageBox.Show("Заполните поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(ConnectionDB.conn))
                    try
                    {
                        byte[] Pass;
                        Encryption f = new Encryption();
                        Pass = f.GetHashPassword(PbPass.Password);
                        connection.Open();
                        string query = $@"SELECT  COUNT(*) FROM Accounts WHERE Email=@Mail AND Pass=@Pass";
                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@Mail", TbMail.Text.ToLower());
                        cmd.Parameters.AddWithValue("@Pass", Pass);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count == 1)
                        {
                            string query2 = $@"SELECT id FROM Accounts WHERE Email=@Mail";
                            SqlCommand cmd2 = new SqlCommand(query2, connection);
                            cmd2.Parameters.AddWithValue("@Mail", TbMail.Text.ToLower());
                            int countID = Convert.ToInt32(cmd2.ExecuteScalar());
                            connection.Close();
                            MessageBox.Show("Добро пожаловать!");
                            MainMenu menu = new MainMenu();
                            menu.Show();
                            this.Close();

                        }
                        else
                        {
                            MessageBox.Show("Неверное имя пользователя или пароль");
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
            }
        }

        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDB.conn))

            {
                if (String.IsNullOrEmpty(TbUN.Text) || String.IsNullOrEmpty(PbPass.Password) || String.IsNullOrEmpty(TbMail.Text))
                {
                    MessageBox.Show("Заполните поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (TbUN.Text.Length < 4)
                {
                    MessageBox.Show(" Логин должен быть больше 3", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (PbPass.Password.Length < 6)
                {
                    MessageBox.Show(" Пароль должен быть больше 5", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    connection.Open();
                    string query = $@"SELECT  COUNT(*) FROM Accounts WHERE Login=@Login";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Login", TbUN.Text.ToLower());
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 1)
                    {
                        MessageBox.Show("Логин занят", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    string query2 = $@"SELECT  COUNT(*) FROM Accounts WHERE Email=@Mail";
                    SqlCommand cmd2 = new SqlCommand(query2, connection);
                    cmd2.Parameters.AddWithValue("@Mail", TbMail.Text.ToLower());
                    int count2 = Convert.ToInt32(cmd2.ExecuteScalar());
                    if (count2 == 1)
                    {
                        MessageBox.Show("Пользователь с такой почтой уже зарегистрирован", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    if (count == 0 & count2 == 0)
                    {
                        string query1 = $@"INSERT INTO Accounts ('Login','Email','Pass') VALUES (@Login, @Email, @Pass)";
                        SqlCommand cmd1 = new SqlCommand(query1, connection);
                        try
                        {
                            byte[] Pass;
                            Encryption f = new Encryption();
                            Pass = f.GetHashPassword(PbPass.Password);
                            cmd1.Parameters.AddWithValue("@Login", TbUN.Text.ToLower());
                            cmd1.Parameters.AddWithValue("@Pass", Pass);
                            cmd1.Parameters.AddWithValue("@Mail", TbMail.Text.ToLower());
                            cmd1.ExecuteNonQuery();
                            MessageBox.Show("Проверка пройдена. Аккаунт зарегистрирован.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.Close();
                        }

                        catch (SqlException ex)
                        {
                            MessageBox.Show("Ошибка" + ex);

                        }
                    }

                }
            }
        }
    }
}
                

