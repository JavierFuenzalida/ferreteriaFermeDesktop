using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FermeDatos;

namespace FermeNegocio
{
    public class Detalle_boleta
    {
        public decimal Id_producto { get; set; }
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio_uni { get; set; }
        public decimal Precio_total { get; set; }


        public Detalle_boleta()
        {

            this.Init();
        }


        private void Init()
        {
            Id_producto = 0;
            Descripcion = string.Empty;
            Cantidad = 0;
            Precio_uni = 0;
            Precio_total = 0;

        }

        public List<Detalle_boleta> ListaDetalleBoleta(decimal v_id_boleta)
        {
            List<Detalle_boleta> Detalles = new List<Detalle_boleta>();

            FermeEntities bd = new FermeEntities();
            
            var listaDetalleBoleta = from d in bd.DETALLE_BOLETA
                                     join p in bd.PRODUCTO
                                     on d.ID_PRODUCTO equals p.ID_PRODUCTO
                                     where d.ID_BOLETA == v_id_boleta
                                     select new {d.CANTIDAD_PROD, d.PRECIO_UNI, d.PRECIO_TOTAL, p.DESCRIPCION, d.ID_PRODUCTO};


            foreach (var item in listaDetalleBoleta)
            {
                Detalle_boleta deta = new Detalle_boleta();

                deta.Id_producto = item.ID_PRODUCTO;
                deta.Descripcion = item.DESCRIPCION;
                deta.Cantidad = item.CANTIDAD_PROD;
                deta.Precio_uni = item.PRECIO_UNI;
                deta.Precio_total = item.PRECIO_TOTAL;
                

                Detalles.Add(deta);
            }
            return Detalles;
        }

    }
}
