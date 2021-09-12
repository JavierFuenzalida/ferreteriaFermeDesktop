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

using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Geom;
using Microsoft.Win32;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using Table = iText.Layout.Element.Table;
using iText.Layout.Properties;
using TextAlignment = iText.Layout.Properties.TextAlignment;
using System.IO;

namespace FermeWPF
{
    /// <summary>
    /// Lógica de interacción para Inicio.xaml
    /// </summary>
    public partial class Inicio : MetroWindow
    {
        String _username;
        decimal _id_boleta;

        public Inicio()
        {

            InitializeComponent();

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void MetroWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        public Inicio(string username)
        {

            InitializeComponent();

            _username = username;
            txt_username.Text = "Bienvenido " + _username;

            Carrito c = new Carrito();
            dg_carrito.ItemsSource = c.ListaCarrito(_username);
            CalcularValores(_username);

            Boleta b = new Boleta();
            ListaBoletas.ItemsSource = b.listaBoleta(_username);
        }


        private void CalcularValores(string username)
        {
            Carrito c = new Carrito();
            List<decimal> valor = new List<decimal>();

            valor = c.cantidadSubtotalyTotal(username);

            foreach (var i in valor)
            {
                txt_cantidad_mostrar.Text = valor[0].ToString();
                txt_subtotal.Text = valor[1].ToString();
                txt_total.Text = valor[2].ToString();
            }
        }

        private async void Btn_agregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txt_id_producto.Text == "")
                {
                    await this.ShowMessageAsync("aviso", "Ingrese Un Codigo de Producto");
                }
                else
                {
                    Carrito c = new Carrito();
                    decimal result = c.AgregarCarrito(_username, Decimal.Parse(txt_id_producto.Text), Decimal.Parse(txt_cantidad.Text));
                    Console.WriteLine("result: " + result);

                    if (result != 0)
                    {
                        CalcularValores(_username);
                        dg_carrito.ItemsSource = c.ListaCarrito(_username);

                    }
                    else
                    {
                        await this.ShowMessageAsync("aviso", "Producto no Encontrado");

                    }
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Producto no Encontrado");
            }
        }

        private void Txt_salgo_recibido_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txt_salgo_recibido.Text == "")
                {
                    txt_salgo_recibido.Text = "";
                    txt_cambio.Text = "";
                    btn_finalizar.IsEnabled = false;

                }
                else
                {
                    if (Decimal.Parse(txt_salgo_recibido.Text) >= Decimal.Parse(txt_total.Text))
                    {
                        txt_cambio.Text = (Decimal.Parse(txt_salgo_recibido.Text) - Decimal.Parse(txt_total.Text)).ToString();
                        btn_finalizar.IsEnabled = true;
                    }
                    else
                    {
                        txt_cambio.Text = (Decimal.Parse(txt_salgo_recibido.Text) - Decimal.Parse(txt_total.Text)).ToString();
                        btn_finalizar.IsEnabled = false;
                    }

                    
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        private void Dg_carrito_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = dg_carrito.SelectedItem;
            string ID = (dg_carrito.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            txt_id_producto.Text = ID;

        }

        private async void Btn_borrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Carrito c = new Carrito();
                decimal result = c.BorrarRegistro(_username, Decimal.Parse(txt_id_producto.Text));

                if (result != 0)
                {
                    CalcularValores(_username);
                    dg_carrito.ItemsSource = c.ListaCarrito(_username);
                }
                else
                {
                    await this.ShowMessageAsync("aviso", "no Se a podido quitar el producto de el carrito.");
                }
                
            }
            catch (Exception)
            {

                Console.WriteLine("no Se a podido quitar el producto de el carrito.");
            }
        }

        private void Btn_limpiar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Carrito c = new Carrito();
                decimal result = c.LimpiarCarrito(_username);
                CalcularValores(_username);
                dg_carrito.ItemsSource = c.ListaCarrito(_username);

            }
            catch (Exception)
            {

                Console.WriteLine("Exception al limpiar carrito");
            }
        }

        private async void Btn_finalizar_Click(object sender, RoutedEventArgs e)
        {
            MessageDialogResult result = await this.ShowMessageAsync("Confirmación", "¿Desea Finalizar Compra?", MessageDialogStyle.AffirmativeAndNegative);

            if (result == MessageDialogResult.Affirmative)
            {
                try
                {
                    Usuario u = new Usuario();
                    Carrito c = new Carrito();
                    Boleta b = new Boleta();
                    decimal id = u.ObtenerIdUsuario(_username);
                    decimal respuesta = c.RegistrarCompra(id);
                    
                    if (respuesta != 0)
                    {
                        await this.ShowMessageAsync("Aviso", "Compra Realizada Correrctamente.");
                        CalcularValores(_username);
                        dg_carrito.ItemsSource = c.ListaCarrito(_username);
                        ListaBoletas.ItemsSource = b.listaBoleta(_username);
                        txt_salgo_recibido.Text = "";
                        txt_cambio.Text = "";
                    }
                    else
                    {
                        await this.ShowMessageAsync("Aviso", "Error, La compra no pudo ser Procesada.");
                    }
                }
                catch (Exception)
                {

                    Console.WriteLine("Exception: Error, La compra no pudo ser Procesada.");
                }


                
            }
        }

        private void ListaBoletas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ListBox)sender;
            var boleta = (FermeNegocio.Boleta)item.SelectedItem;

            if (boleta != null)
            {
                _id_boleta = boleta.Id_boleta;
                btn_imprimir.IsEnabled = true;
            }
            else
            {
                btn_imprimir.IsEnabled = false;
            }


        }

        private void Btn_imprimir_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog SD = new SaveFileDialog();
            SD.Filter = "PDF file (*.pdf)|*.pdf";
            if (SD.ShowDialog() == true)
            {
                string rutaSinExtencion = SD.FileName.Remove(SD.FileName.Length - 4);
                CrearPDF(rutaSinExtencion);
                Console.WriteLine(rutaSinExtencion);


            }
        }


        private async void CrearPDF(string ruta)
        {
            try
            {
                PdfWriter PdfWriter = new PdfWriter(@ruta);
                PdfDocument pdf = new PdfDocument(PdfWriter);
                Document documento = new Document(pdf, PageSize.LETTER);


                documento.SetMargins(240, 20, 50, 20);

                PdfFont fontColumnas = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                PdfFont fontContenido = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);


                string[] columnas = { "Codigo", "Producto", "Cant", "Precio uni", "Precio total" };

                float[] tamanios = { 4, 9, 1, 2, 2 };

                Table tabla = new Table(UnitValue.CreatePercentArray(tamanios));
                tabla.SetWidth(UnitValue.CreatePercentValue(100));

                foreach (string columna in columnas)
                {
                    tabla.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph(columna).SetFont(fontColumnas)));
                }

                Detalle_boleta detalle = new Detalle_boleta();

                List<Detalle_boleta> listaDetalle = detalle.ListaDetalleBoleta(_id_boleta);

                foreach (var i in listaDetalle)
                {
                    tabla.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(i.Id_producto.ToString()).SetFont(fontContenido)));
                    tabla.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(i.Descripcion.ToString()).SetFont(fontContenido)));
                    tabla.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(i.Cantidad.ToString()).SetFont(fontContenido)));
                    tabla.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(i.Precio_uni.ToString()).SetFont(fontContenido)));
                    tabla.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(i.Precio_total.ToString()).SetFont(fontContenido)));
                }

                documento.Add(tabla);
                documento.Close();


                string _fecha_ingreso = "";
                string _subTotal = "";
                string _total = "";
                string _iva = "";

                Boleta b = new Boleta();
                List<Boleta> queryBoleta = b.obtBoleta(_id_boleta);
                foreach (var boleta in queryBoleta)
                {
                    _fecha_ingreso = boleta.Fecha_ingreso.ToString("dd-MM-yyyy");
                    _subTotal = boleta.Sub_total.ToString();
                    _total = boleta.Total.ToString();
                    _iva = boleta.Iva.ToString();

                }

                var logo = new iText.Layout.Element.Paragraph("FERME").SetWidth(UnitValue.CreatePercentValue(100)).SetFont(fontColumnas);
                logo.SetFontSize(28);

                var titulo = new iText.Layout.Element.Paragraph("Boleta Electronica");
                titulo.SetTextAlignment(TextAlignment.CENTER);
                titulo.SetFontSize(20);

                var fecha = new iText.Layout.Element.Paragraph("Fecha de Compra: " + _fecha_ingreso);
                fecha.SetFontSize(12);

                var subTotal = new iText.Layout.Element.Paragraph("Sub-Total: $" + _subTotal);
                subTotal.SetFontSize(12);

                var total = new iText.Layout.Element.Paragraph("Total a Pagar (IVA INCLUIDO): $" + _total);
                total.SetFontSize(12);

                var iva = new iText.Layout.Element.Paragraph("IVA: " + _iva);
                iva.SetFontSize(12);

                var id_bole = new iText.Layout.Element.Paragraph("N° de Boleta:" + _id_boleta);
                id_bole.SetFontSize(12);

                var separador = new iText.Layout.Element.Paragraph("=============================================================");
                separador.SetFontSize(16);
                separador.SetWidth(UnitValue.CreatePercentValue(100));

                PdfDocument pdfDoc = new PdfDocument(new PdfReader(ruta), new PdfWriter
                    (ruta + "-BoletaNumero- " + _id_boleta + ".pdf"));
                Document doc = new Document(pdfDoc);

                int nroPaginas = pdfDoc.GetNumberOfPages();

                for (int i = 1; i <= nroPaginas; i++)
                {
                    PdfPage pagina = pdfDoc.GetPage(i);

                    float y = (pdfDoc.GetPage(i).GetPageSize().GetTop() - 15);

                    doc.ShowTextAligned(logo, 66, y, i, TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);
                    doc.ShowTextAligned(fecha, 510, y, i, TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);
                    doc.ShowTextAligned(titulo, 300, y - 30, i, TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);
                    doc.ShowTextAligned(id_bole, 300, y - 50, i, TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);

                    doc.ShowTextAligned(separador, 300, y - 80, i, TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);

                    doc.ShowTextAligned(subTotal, 20, y - 130, i, TextAlignment.LEFT, iText.Layout.Properties.VerticalAlignment.TOP, 0);
                    doc.ShowTextAligned(total, 20, y - 150, i, TextAlignment.LEFT, iText.Layout.Properties.VerticalAlignment.TOP, 0);
                    doc.ShowTextAligned(iva, 20, y - 170, i, TextAlignment.LEFT, iText.Layout.Properties.VerticalAlignment.TOP, 0);

                    doc.ShowTextAligned(separador, 300, y - 200, i, TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);


                    doc.ShowTextAligned(new iText.Layout.Element.Paragraph(string.Format("pagina {0} de {1}", i, nroPaginas)), pdfDoc.GetPage
                        (i).GetPageSize().GetWidth() / 2, pdfDoc.GetPage(i).GetPageSize().GetBottom() + 30, i,
                        TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);
                }
                doc.Close();
                File.Delete(ruta);

                await this.ShowMessageAsync("Aviso", "Boleta Generada Corractamente.");
            }
            catch (Exception)
            {

                await this.ShowMessageAsync("Aviso", "Error, No se Pudo Generar la Boleta.");
            }




        }

        private void Btn_buscar_Click(object sender, RoutedEventArgs e)
        {
            Productos ventana_productos = new Productos();
            ventana_productos.Show();
        }
    }
}

