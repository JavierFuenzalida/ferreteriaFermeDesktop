using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

using FermeDatos;

namespace FermeNegocio
{
    public class Usuario
    {
        public decimal Id_usuario { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public Usuario()
        {

            this.Init();
        }


        private void Init()
        {
            Id_usuario = 0;
            username = string.Empty;
            password = string.Empty;

        }

        public List<Usuario> ListaProductos()
        {
            List<Usuario> users = new List<Usuario>();

            FermeEntities bd = new FermeEntities();
            var ListaP = from d in bd.USUARIO
                         select new { d.ID_USUARIO, d.USERNAME, d.PASSWORD };


            foreach (var item in ListaP)
            {
                Usuario usu = new Usuario();
                usu.Id_usuario = item.ID_USUARIO;
                usu.username = item.USERNAME;
                usu.password = item.PASSWORD;

                users.Add(usu);
            }

            return users;
        }

        public bool ValidarVendedor(string v_username, string v_password)
        {

            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder new_pass = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(v_password));
            for (int i = 0; i < stream.Length; i++) new_pass.AppendFormat("{0:x2}", stream[i]);


            FermeEntities bd = new FermeEntities();

            var salida = new System.Data.Objects.ObjectParameter("v_salida", typeof(decimal));
            bd.SP_VALIDAR_VENDEDOR(v_username, new_pass.ToString(), salida);
            Console.WriteLine("====================================");
            Console.WriteLine(salida.Value);

            if (int.Parse(salida.Value.ToString()) != 0)
            {
                return true;
            }

            return false;
        }

        public decimal ObtenerIdUsuario(string v_username)
        {
            try
            {
                FermeEntities bd = new FermeEntities();

                var salida = new System.Data.Objects.ObjectParameter("v_id_usuario", typeof(decimal));
                bd.SP_OBTE_ID_USUARIO(v_username, salida);

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
