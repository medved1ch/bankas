using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using bankas.Database;
using bankas.Classes;
using PasswordBox = HandyControl.Controls.PasswordBox;
using TextBox = HandyControl.Controls.TextBox;

namespace bankas.Classes
{
    internal class Authorisation
    {
        static BankEntities db = new BankEntities();
        static public void Authoris(TextBox TbMailIn, PasswordBox PbPassIn, Window StartUpWindow)
        {
            if (String.IsNullOrEmpty(TbMailIn.Text) || String.IsNullOrEmpty(PbPassIn.Password))
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {


                Accounts usr = db.Accounts.SingleOrDefault(c => c.Email == TbMailIn.Text);
                if (usr == null)
                {
                    MessageBox.Show("Такого пользователя не существует");
                    return;
                }
                SimpleCommand sc = new SimpleCommand();
                if (sc.CheckHashEquil(usr.Pass, sc.GetHash(PbPassIn.Password)))
                {
                    IdSave.Login = usr.Login;
                    IdSave.Date = DateTime.Now.ToString("d");
                    IdSave.Day = DateTime.Now.ToString("ddd");
                    MessageBox.Show($"Здравствуйте, {usr.Login}");
                    MainMenu menu = new MainMenu();
                    menu.Show();
                    StartUpWindow.Close();
                }
                else
                {
                    MessageBox.Show("Пароль неверный!");
                }

            }
        }
    }
}
