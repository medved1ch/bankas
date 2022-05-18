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
    internal class Registration
    {
        static BankEntities db = new BankEntities();

        static public void Reg(TextBox TbUN, TextBox TbMailUp, PasswordBox PbPassUp, Window StartUpWindow)
        {
            if (String.IsNullOrEmpty(TbMailUp.Text) || String.IsNullOrEmpty(PbPassUp.Password) || String.IsNullOrEmpty(TbUN.Text))
            {
                MessageBox.Show("Заполните поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (TbUN.Text.Length < 4)
            {
                MessageBox.Show(" Логин должен быть больше 3", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (PbPassUp.Password.Length < 6)
            {
                MessageBox.Show(" Пароль должен быть больше 5", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else 
            {
                SimpleCommand sc = new SimpleCommand();
                String host = System.Net.Dns.GetHostName();
                // Получение ip-адреса.
                System.Net.IPAddress IPReg0 = System.Net.Dns.GetHostByName(host).AddressList[0];
                Accounts usr = new Accounts
                
                {
                    Login = TbUN.Text,
                    Email = TbMailUp.Text,
                    Pass = sc.GetHash(PbPassUp.Password),
                    IPReg = IPReg0.ToString()
                };
                try
                {
                    db.Accounts.Add(usr);
                    db.SaveChanges();
                    MessageBox.Show("Регистрация прошла успешно!");
                    TbMailUp.Clear();
                    TbUN.Clear();
                    PbPassUp.Clear();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException)
                {
                    db.Accounts.Remove(usr);
                    db.SaveChanges();
                    MessageBox.Show("Имя пользователя занято, или пользователь с такой почтой существует");
                    TbUN.Clear();
                    TbMailUp.Clear();
                    return;
                }
            }
            }
        }
    }

