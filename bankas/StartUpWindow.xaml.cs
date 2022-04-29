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
using System.Data;

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
            if (String.IsNullOrEmpty(TbMailIn.Text) || String.IsNullOrEmpty(PbPassIn.Password))
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
                        Pass = f.GetHashPassword(PbPassIn.Password);
                        connection.Open();
                        string query = $@"SELECT  COUNT(*) FROM Accounts WHERE Email=@Mail AND Pass=@Pass";
                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@Mail", TbMailIn.Text.ToLower());
                        cmd.Parameters.AddWithValue("@Pass", Pass);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count == 1)
                        {
                            string query2 = $@"SELECT id FROM Accounts WHERE Email=@Mail";
                            SqlCommand cmd2 = new SqlCommand(query2, connection);
                            cmd2.Parameters.AddWithValue("@Mail", TbMailIn.Text.ToLower());
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

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionDB.conn))
                {
                    connection.Open();
                    string Login = TbUN.Text;
                    string Mail = TbMailUp.Text.ToLower();
                    string query = $@"SELECT COUNT(*) from Accounts where Login = '{Login}' and Email = '{Mail}'";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    object result = cmd.ExecuteScalar();
                    int a = Convert.ToInt32(result);
                    if (a != 0)
                    {
                        MessageBox.Show("Такой пользователь уже существует!");
                    }
                    else
                    {
                        try
                        {
                            
                            Encryption f = new Encryption();
                            byte[] Pass = f.GetHashPassword(PbPassUp.Password);

                            string query1 = $@"insert into Accounts(Login, Email, Pass) values ('{Login}','{Mail}', '@BinData')";
                            cmd = new SqlCommand(query1, connection);
                            SqlParameter param = cmd.Parameters.AddWithValue("@BinData", Pass);
                            param.DbType = DbType.Binary;
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Успешная регистрация!");
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("Ошибка" + ex);
                        }
                    }

                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
                

