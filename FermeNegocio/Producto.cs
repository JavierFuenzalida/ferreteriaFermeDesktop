using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FermeDatos;

namespace FermeNegocio
{
    public class Producto
    {
        public decimal Id_producto { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public decimal Stock { get; set; }
        public decimal Activo{ get; set; }



        public Producto()
        {

            this.Init();
        }


        private void Init()
        {

            Id_producto = 0;
            Descripcion = string.Empty;
            Precio = 0;
            Stock = 0;
            Activo = 0;

        }


        public List<Producto> ListaProductos()
        {
            List<Producto> productos = new List<Producto>();

            FermeEntities bd = new FermeEntities();

            var listaProductos = from p in bd.PRODUCTO
                                 where p.ACTIVO == 1
                                 select new { p.ID_PRODUCTO, p.DESCRIPCION, p.PRECIO, p.STOCK, p.ACTIVO};


            foreach (var item in listaProductos)
            {
                Producto pro = new Producto();

                pro.Id_producto = item.ID_PRODUCTO;
                pro.Descripcion = item.DESCRIPCION;
                pro.Precio = item.PRECIO;
                pro.Stock = item.STOCK;
                pro.Activo = decimal.Parse(item.ACTIVO.ToString());

                productos.Add(pro);
            }
            return productos;
        }
    }
}
