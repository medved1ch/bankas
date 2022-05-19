using HandyControl.Tools;
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
using LiveCharts;
using LiveCharts.Wpf;
using bankas.Classes;

namespace bankas
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
            LoadDateLog();
        }
        public void LoadDateLog() //Загрузка счетов клиента
        {
            txtLog.Text = IdSave.Login;
            txtDate.Text = IdSave.Date;
            txtDay.Text = IdSave.Day;
        }

            private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            StartUpWindow start = new StartUpWindow();
            start.Show();
            this.Close();
        }
    }
}
