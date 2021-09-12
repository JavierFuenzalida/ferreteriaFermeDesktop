using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FermeDatos;

namespace FermeNegocio
{
    public class Carrito
    {
        
        
        
        public decimal Id_producto { get; set; }
        public string  Descripcion { get; set; }
        public decimal Cantidad_prod { get; set; }
        public decimal precio { get; set; }
        public decimal precioTotal { get; set; }



        public Carrito()
        {

            this.Init();
        }


        private void Init()
        {
            
            
            
            Id_producto = 0;
            Descripcion = string.Empty;
            Cantidad_prod = 0;
            precio = 0;
            precioTotal = 0;

        }


        public List<Carrito> ListaCarrito(string username)
        {
            List<Carrito> Carritos = new List<Carrito>();

            FermeEntities bd = new FermeEntities();

            var salida = new System.Data.Objects.ObjectParameter("v_id_usuario", typeof(decimal));
            bd.SP_OBTE_ID_USUARIO(username, salida);


            decimal id = int.Parse(salida.Value.ToString());

            var listaCarrito = from c in bd.CARRITO
                               join p in bd.PRODUCTO
                               on c.ID_PRODUCTO equals p.ID_PRODUCTO
                               where c.ID_USUARIO == id
                               select new {c.CANTIDAD_PROD, c.ID_PRODUCTO, p.DESCRIPCION, p.PRECIO};


            foreach (var item in listaCarrito)
            {
                Carrito cart = new Carrito();
                
                cart.Cantidad_prod = item.CANTIDAD_PROD;
                cart.Id_producto = item.ID_PRODUCTO;
                cart.Descripcion = item.DESCRIPCION;
                cart.precio = item.PRECIO;
                cart.precioTotal = item.PRECIO*item.CANTIDAD_PROD;

                Carritos.Add(cart);
            }
            return Carritos;
        }

        public decimal AgregarCarrito(string v_username, decimal v_id_producto, decimal v_cantidad)
        {
            try
            {
                FermeEntities bd = new FermeEntities();

                var salida = new System.Data.Objects.ObjectParameter("v_salida", typeof(decimal));
                bd.SP_ADD_CARRITO_ESCRI(v_username, v_id_producto, v_cantidad, salida);

                decimal result = int.Parse(salida.Value.ToString());

                return result;
            }
            catch (Exception)
            {
                decimal result = 0;
                return result;
            }
        }

        public List<decimal> cantidadSubtotalyTotal(string v_username)
        {
            List<decimal> valores = new List<decimal>();

            try
            {
                FermeEntities bd = new FermeEntities();
                var cantidad = new System.Data.Objects.ObjectParameter("v_cantidad", typeof(decimal));
                var sub_total = new System.Data.Objects.ObjectParameter("v_sub_total", typeof(decimal));
                var total = new System.Data.Objects.ObjectParameter("v_total", typeof(decimal));
                bd.SP_OBTENER_CANTIDAD_SUBTOTAL_Y_TOTAL_ESCRITORIO(v_username, cantidad, sub_total, total);

                decimal _cantidad = int.Parse(cantidad.Value.ToString());
                decimal _sub_total = int.Parse(sub_total.Value.ToString());
                decimal _total = int.Parse(total.Value.ToString());

                valores.Add(_cantidad);
                valores.Add(_sub_total);
                valores.Add(_total);

                return valores;
            }
            catch (Exception)
            {
                decimal _cantidad = 0;
                decimal _sub_total = 0;
                decimal _total = 0;

                valores.Add(_cantidad);
                valores.Add(_sub_total);
                valores.Add(_total);

                return valores;
            }
        }

        public decimal BorrarRegistro(string v_username, decimal v_id_producto)
        {
            FermeEntities bd = new FermeEntities();

            try
            {
                var salida = new System.Data.Objects.ObjectParameter("v_salida", typeof(decimal));
                bd.SP_DELETE_CARRITO_ESCRIT(v_username, v_id_producto, salida);

                decimal result = int.Parse(salida.Value.ToString());

                return result;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public decimal LimpiarCarrito(string v_username)
        {
            FermeEntities bd = new FermeEntities();

            try
            {
                var salida = new System.Data.Objects.ObjectParameter("v_salida", typeof(decimal));
                bd.SP_CLEAN_CARRITO_ESCRIT(v_username, salida);

                decimal result = int.Parse(salida.Value.ToString());

                return result;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public decimal RegistrarCompra(decimal v_id_usuario)
        {
            FermeEntities bd = new FermeEntities();

            try
            {
                var salida = new System.Data.Objects.ObjectParameter("v_salida", typeof(decimal));
                bd.SP_REGISTRAR_COMPRA_ESCRIT(v_id_usuario, salida);

                decimal result = int.Parse(salida.Value.ToString());

                return result;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
