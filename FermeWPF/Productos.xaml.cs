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
using System.Text.RegularExpressions;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using FermeNegocio;

namespace FermeWPF
{
    /// <summary>
    /// Lógica de interacción para Productos.xaml
    /// </summary>
    public partial class Productos : MetroWindow
    {
        public Productos()
        {
            InitializeComponent();
            llenarGrid();
        }

        private void llenarGrid()
        {
            Producto producto = new Producto();

            dg_productos.ItemsSource = producto.ListaProductos();
        }
    }
}
