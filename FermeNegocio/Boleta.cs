using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FermeDatos;

namespace FermeNegocio
{
    public class Boleta
    {
        public decimal Id_boleta { get; set; }
        public DateTime Fecha_ingreso { get; set; }
        public decimal Iva { get; set; }
        public decimal Sub_total { get; set; }
        public decimal Total { get; set; }
        public decimal Id_usuario { get; set; }
        public decimal Id_estado { get; set; }
        public decimal Id_pago { get; set; }
        public string Despacho { get; set; }

        public Boleta()
        {

            this.Init();
        }


        private void Init()
        {
            Id_boleta = 0;
            Fecha_ingreso = DateTime.Today;
            Iva = 0;
            Sub_total = 0;
            Total = 0;
            Id_estado = 0;
            Id_usuario = 0;
            Id_pago = 0;
            Despacho = string.Empty;

        }

        public List<Boleta> listaBoleta(string username)
        {
            List<Boleta> Boletas = new List<Boleta>();

            FermeEntities bd = new FermeEntities();

            var salida = new System.Data.Objects.ObjectParameter("v_id_usuario", typeof(decimal));
            bd.SP_OBTE_ID_USUARIO(username, salida);


            decimal id = int.Parse(salida.Value.ToString());

            var listaBoleta = from b in bd.BOLETA
                               where b.ID_USUARIO == id
                               orderby b.FECHA_INGRESO descending
                               select new { b.ID_BOLETA, b.FECHA_INGRESO, b.IVA, b.SUB_TOTAL, b.TOTAL, b.ID_USUARIO, b.ID_ESTADO, b.ID_PAGO, b.DESPACHO};


            foreach (var item in listaBoleta)
            {
                Boleta bole = new Boleta();

                bole.Id_boleta = item.ID_BOLETA;
                bole.Fecha_ingreso = item.FECHA_INGRESO;
                bole.Iva = item.IVA;
                bole.Sub_total = item.SUB_TOTAL;
                bole.Total = item.TOTAL;
                bole.Id_usuario = item.ID_USUARIO;
                bole.Id_estado = item.ID_ESTADO;
                bole.Id_pago = item.ID_PAGO;
                bole.Despacho = item.DESPACHO;

                Boletas.Add(bole);
            }
            return Boletas;
        }

        public List<Boleta> obtBoleta(decimal v_id_boleta)
        {
            List<Boleta> boleta = new List<Boleta>();

            FermeEntities bd = new FermeEntities();

            var queryBoleta = from b in bd.BOLETA
                                     where b.ID_BOLETA == v_id_boleta
                                     select new {b.ID_BOLETA, b.FECHA_INGRESO, b.IVA, b.SUB_TOTAL, b.TOTAL, b.ID_USUARIO, b.ID_ESTADO, b.ID_PAGO, b.DESPACHO};


            foreach (var item in queryBoleta)
            {
                Boleta bole = new Boleta();

                bole.Id_boleta = item.ID_BOLETA;
                bole.Fecha_ingreso = item.FECHA_INGRESO;
                bole.Iva = item.IVA;
                bole.Sub_total = item.SUB_TOTAL;
                bole.Total = item.TOTAL;
                bole.Id_usuario = item.ID_USUARIO;
                bole.Id_estado = item.ID_ESTADO;
                bole.Id_pago = item.ID_PAGO;
                bole.Despacho = item.DESPACHO;

                boleta.Add(bole);
            }
            return boleta;
        }

    }
}
