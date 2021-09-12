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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using FermeNegocio;

namespace FermeWPF
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : MetroWindow
    {
        public Login()
        {
            InitializeComponent();
        }


    private async void Btn_login_Click(object sender, RoutedEventArgs e)
        {
            Usuario us = new Usuario();

            bool result = us.ValidarVendedor(txt_username.Text, txt_contrasena.Password.ToString());

            if (result == true)
            {
                Inicio home = new Inicio(txt_username.Text);
                this.Hide();
                App.Current.MainWindow.Hide(); //esto me permite cerrar la ventana main ya que con la intruccion hide() por si sola no lo logra.
                home.Closed += (s, args) => App.Current.Shutdown(); //Detiene la app. //opci....this.close();//
                home.Show();
            }
            else
            {
                await this.ShowMessageAsync("aviso", "No valido");
            }

        }

        private void MetroWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }

}
