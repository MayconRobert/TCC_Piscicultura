using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Site_piscicultura.Models
{
    public class ModeloPiscicultura
    {
        private int id, qtd_Tanques;
        private string nome, local, clima, tamanhoLocal, cpf_usuario;
        //private List<String> lista;

        //public List<String> Lista
        //{
        //    get { return lista; }
        //    set { lista = value; }
        //}

        static SqlConnection con =
           new SqlConnection(ConfigurationManager.ConnectionStrings["BCD"].ConnectionString);
        [DisplayName("ID")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [DisplayName("CPF")]
        public String Cpf_usuario
        {
            get { return cpf_usuario; }
            set { cpf_usuario = value; }
        }

        [DisplayName("Quantidade de Tanques")]
        public int Qtd_Tanques
        {
            get { return qtd_Tanques; }
            set { qtd_Tanques = value; }
        }

        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

        public string Local
        {
            get { return local; }
            set { local = value; }
        }

        public string Clima
        {
            get { return clima; }
            set { clima = value; }
        }

        [DisplayName("Tamanho do Local")]
        public string TamanhoLocal
        {
            get { return tamanhoLocal; }
            set { tamanhoLocal = value; }
        }

        //MÉTODO DE CRIAR PISCICULTURA
        internal string CriarPiscicultura()
        {
            //TESTAR A CONEXÃO PARA VER SE ESTÁ CERTO
            try
            {

                con.Open();
                SqlCommand query = new SqlCommand("INSERT INTO Pisciculturas values (@nome, @local, @clima, @qtd_tanques, @tamanho_local, @cpf)", con);
                //query.Parameters.AddWithValue("@id", id);
                query.Parameters.AddWithValue("@nome", nome);
                query.Parameters.AddWithValue("@qtd_tanques", qtd_Tanques);
                query.Parameters.AddWithValue("@local", local);
                query.Parameters.AddWithValue("@clima", clima);
                query.Parameters.AddWithValue("tamanho_local", tamanhoLocal);
                query.Parameters.AddWithValue("@cpf", cpf_usuario);
 
                query.ExecuteNonQuery();
            }


            catch (Exception e)
            {
                return e.Message;
            }
            //VERIFICA SE A CONEXÃO ESTÁ ABERTO
            if (con.State == ConnectionState.Open)
                //SE TIVER ABERTA, FECHA CONEXÃO
                con.Close();
            //RETORNA UMA MENSAGEM SE INSERIU OU NÃO
            return "Piscicultura registrada com sucesso!"; 

        }



        //--METODO PARA EDITAR PISCICULTURA
        internal string EditarPiscicultura()
        {

            try
            {
                //Abrir conexão
                con.Open();

                //Comando para fazer atualização de informaçao no BD
                SqlCommand atualizar = new SqlCommand("UPDATE Pisciculturas SET qtd_Tanques = @qtd_tanque, nome = @nome, local=@local, clima=@clima,Tam_local=@tamanhoLocal where Id = @Id", con);

                atualizar.Parameters.AddWithValue("@cpf_usuario", cpf_usuario);
                atualizar.Parameters.AddWithValue("@qtd_tanque", qtd_Tanques);
                atualizar.Parameters.AddWithValue("@nome", nome);
                atualizar.Parameters.AddWithValue("@local", local);
                atualizar.Parameters.AddWithValue("@clima", clima);
                atualizar.Parameters.AddWithValue("@tamanholocal", tamanhoLocal);
                atualizar.Parameters.AddWithValue("@id", id);

                atualizar.ExecuteNonQuery();



            }
            catch (Exception m)
            {

                return m.Message;
            }

            if (con.State == ConnectionState.Open)
                con.Close();


            return "Editado com Sucesso";

        }


        //---MÉTODO para EXCLUIR PISCICULTURA---//

        internal string RemoverPiscicultura()
        {
            string res = "";
            try
            {
                con.Open();
                //Deletar a chave primária da tabela
                SqlCommand query =
                    new SqlCommand("DELETE FROM Pisciculturas WHERE id = @id", con);
                query.Parameters.AddWithValue("@id", id);

                query.ExecuteNonQuery();
                res = "Removido com sucesso!";
            }
            catch (Exception e)
            {
                res = e.Message.ToString();
            }

            if (con.State == ConnectionState.Open)
                con.Close();
            return res;//

        }

       

        //--CRIAR A LISTA DE Pisciculturas CADASTRADOS---//

        public static List<ModeloPiscicultura> ListarPiscicultura(string cpf)
        {

            //Criar um array para guardar as informações que virão do BD
            List<ModeloPiscicultura> lista = new List<ModeloPiscicultura>();
            //ModeloUsuario u = new ModeloUsuario();
            try
            {
                con.Open();

                //Comando para selecionar os itens da tabela Pisciculturas
                SqlCommand listar = new SqlCommand("SELECT * FROM Pisciculturas where Fk_Usuarios_Cpf = @cpf", con);
                listar.Parameters.AddWithValue("@cpf", cpf);
                SqlDataReader leitor = listar.ExecuteReader();

                while (leitor.Read())
                {
                    ModeloPiscicultura p = new ModeloPiscicultura();
                    p.id = (int)leitor["id"];
                    p.cpf_usuario = leitor["FK_Usuarios_CPF"].ToString();
                    p.qtd_Tanques =(int)leitor["Qtd_Tanques"];
                    p.nome = leitor["Nome"].ToString();
                    p.local = leitor["Local"].ToString();
                    p.clima = leitor["Clima"].ToString();
                    p.tamanhoLocal = leitor["Tam_Local"].ToString();
                

                    lista.Add(p);
                }
            }
            catch (Exception e)
            {
                lista = null;
            }

            if (con.State == ConnectionState.Open)
                con.Close();

            return lista;
        }

        //---MÉTODO PARA BUSCAR UM PEIXE ESPECIFICO NA TABELA---//

        public static ModeloPiscicultura BuscaPiscicultura(int id)
        {
            ModeloPiscicultura l = new ModeloPiscicultura();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BCD"].ConnectionString);
            try
            {
                con.Open();
                SqlCommand query =
                    new SqlCommand("SELECT * FROM Pisciculturas WHERE id = @id", con);
                query.Parameters.AddWithValue("@id",id);
                SqlDataReader leitor = query.ExecuteReader();

                while (leitor.Read())
                {
                    l.id = (int)leitor["id"];
                    l.cpf_usuario = leitor["fk_usuarios_cpf"].ToString();
                    l.qtd_Tanques = (int)leitor["Qtd_Tanques"];
                    l.nome = leitor["Nome"].ToString();
                    l.local = leitor["Local"].ToString();
                    l.clima = leitor["Clima"].ToString();
                    l.tamanhoLocal = leitor["Tam_Local"].ToString();

                }
            }
            catch (Exception e)
            {
                l = null;
                string resposta = e.Message;
            }

            if (con.State == ConnectionState.Open)
                con.Close();

            return l;
        }

        public int PesquisaNumeroPisciculturas(string cpf)
        {
            int numeroDePisciculturas = 0;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BCD"].ConnectionString);
            try
            {
                con.Open();
                SqlCommand query =
                    new SqlCommand("SELECT count (*) pisciculturasRegistradas FROM Pisciculturas WHERE FK_Usuarios_CPF = @cpf", con);
                query.Parameters.AddWithValue("@cpf", cpf);
                SqlDataReader leitor = query.ExecuteReader();

                while (leitor.Read())
                {
                    //m.cpf = leitor["cpf"].ToString();
                    numeroDePisciculturas = (int)leitor["pisciculturasRegistradas"];

                }
            }
            catch (Exception e)
            {
                numeroDePisciculturas = 0;
                string resposta = e.Message;

            }

            if (con.State == ConnectionState.Open)
                con.Close();
            return numeroDePisciculturas;
        }

        public string AdicionaNumeroTanques(string cpf, int idPiscicultura)
        {
            try
            {
                //Abrir conexão
                con.Open();

                //Comando para fazer atualização de informaçao no BD
                SqlCommand atualizar = new SqlCommand("UPDATE Pisciculturas SET qtd_Tanques = qtd_Tanques + 1 where FK_Usuarios_CPF = @cpf and Id = @Id", con);

                atualizar.Parameters.AddWithValue("@cpf", cpf);
                atualizar.Parameters.AddWithValue("@Id", idPiscicultura);
                //atualizar.Parameters.AddWithValue("@qtd_tanque", qtd_Tanques);

                atualizar.ExecuteNonQuery();



            }
            catch (Exception m)
            {

                return m.Message;
            }

            if (con.State == ConnectionState.Open)
                con.Close();


            return "Editado com Sucesso";
        }

        public string DiminuiNumeroTanques(string cpf, int idPiscicultura)
        {
            try
            {
                //Abrir conexão
                con.Open();

                //Comando para fazer atualização de informaçao no BD
                SqlCommand atualizar = new SqlCommand("UPDATE Pisciculturas SET qtd_Tanques = qtd_Tanques -1 where FK_Usuarios_CPF = @cpf and Id = @Id", con);

                atualizar.Parameters.AddWithValue("@cpf", cpf);
                atualizar.Parameters.AddWithValue("@Id", idPiscicultura);
                //atualizar.Parameters.AddWithValue("@qtd_tanque", qtd_Tanques);

                atualizar.ExecuteNonQuery();



            }
            catch (Exception m)
            {

                return m.Message;
            }

            if (con.State == ConnectionState.Open)
                con.Close();


            return "Editado com Sucesso";
        }





    }
}