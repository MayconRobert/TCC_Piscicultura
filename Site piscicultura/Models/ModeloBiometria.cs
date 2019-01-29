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
    public class ModeloBiometria
    {
        private int id, qtd_PeixesTanque, qtd_PeixesAmostra, id_Tanque;
        private double peso_Amostra, ganho_PesoAmostra;
        private string data, peso_Medio, ganho_PesoMedio;

        /******CONEXÃO COM BANCO DE DADOS******/
        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BCD"].ConnectionString);
        //aaaaa commit
        [DisplayName("ID")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [DisplayName("Quantidade de Peixes")]
        public int Qtd_peixesTanque
        {
            get { return qtd_PeixesTanque; }
            set { qtd_PeixesTanque = value; }
        }
        [DisplayName("Quantidade de Peixes")]
        public int Qtd_peixesAmostra
        {
            get { return qtd_PeixesAmostra; }
            set { qtd_PeixesAmostra = value; }
        }

        [DisplayName("Massa da amostra")]
        public double Peso_Amostra
        {
            get { return peso_Amostra; }
            set { peso_Amostra = value; }
        }

        [DisplayName("Massa Média")]
        public String Peso_Medio
        {
            get { return peso_Medio; }
            set { peso_Medio = value; }
        }

        [DisplayName("Ganho de Massa")]
        public double Ganho_PesoAmostra
        {
            get { return ganho_PesoAmostra; }
            set { ganho_PesoAmostra = value; }
        }

        [DisplayName("Ganho de Massa")]
        public String Ganho_PesoMedio
        {
            get { return ganho_PesoMedio; }
            set { ganho_PesoMedio = value; }
        }

        public String Data
        {
            get { return data; }
            set { data = value; }
        }
        [DisplayName("Tanque")]
        public int Id_Tanque
        {
            get { return id_Tanque; }
            set { id_Tanque = value; }
        }


        internal string RegistrarBiometria()
        {
            //TESTAR A CONEXÃO PARA VER SE ESTÁ CERTO
            try
            {

                con.Open();
                SqlCommand query = new SqlCommand("INSERT INTO Biometria values (@qtd_PeixesTanque, @qtd_PeixesAmostra, @peso_Amostra, @peso_Medio, @ganho_PesoAmostra, @ganho_PesoMedio, @data,  @id_tanque)", con);
                query.Parameters.AddWithValue("@id", id);
                query.Parameters.AddWithValue("@qtd_PeixesTanque", qtd_PeixesTanque);
                query.Parameters.AddWithValue("@qtd_PeixesAmostra", qtd_PeixesAmostra);
                query.Parameters.AddWithValue("@peso_Amostra", peso_Amostra);
                query.Parameters.AddWithValue("@peso_Medio", peso_Medio);
                query.Parameters.AddWithValue("@ganho_PesoAmostra", ganho_PesoAmostra);
                query.Parameters.AddWithValue("@ganho_PesoMedio", ganho_PesoMedio);
                query.Parameters.AddWithValue("@data", data);
                query.Parameters.AddWithValue("@id_tanque", id_Tanque);



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
            return "Biometria registrada com sucesso!"; // Retorna a mensagem

        }

      

        //--CRIAR A LISTA DE PEIXES CADASTRADOS---//

        public static List<ModeloBiometria> ListarBiometria(int idTanque)
        {

            //Criar um array para guardar as informações que virão do BD
            List<ModeloBiometria> lista = new List<ModeloBiometria>();

            try
            {
                con.Open();

                //Comando para selecionar os itens da tabela peixes
                SqlCommand listar = new SqlCommand("SELECT * FROM Biometria where FK_Tanques_ID = @Id", con);
                listar.Parameters.AddWithValue("@Id", idTanque);
                SqlDataReader leitor = listar.ExecuteReader();

                while (leitor.Read())
                {
                   

                    ModeloBiometria b = new ModeloBiometria();
                    b.Id =(int) leitor["id"];
                    b.Qtd_peixesTanque = (int) leitor["Qtd_PeixesTanque"];
                    b.Qtd_peixesAmostra= (int) leitor["qtd_PeixesAmostra"];
                    b.Id_Tanque =  (int) leitor["FK_Tanques_ID"];
                    b.Peso_Amostra = (float) leitor["peso_Amostra"];
                    b.Peso_Medio = leitor["peso_Medio"].ToString();
                    b.Ganho_PesoMedio = leitor["ganho_PesoMedio"].ToString();
                    b.Ganho_PesoAmostra = (float) leitor["ganho_PesoAmostra"];
                    b.Data = leitor["data"].ToString();
                    lista.Add(b);
                }
            }
            catch (Exception e)
            {
                lista = null;
                e.ToString();
            }

            if (con.State == ConnectionState.Open)
                con.Close();

            return lista;
        }

        public static List<ModeloBiometria> BuscaIdTanques()
        {
            //Criar um array para guardar as informações que virão do BD
            List<ModeloBiometria> lista = new List<ModeloBiometria>();

            try
            {
                con.Open();

                //Comando para selecionar os itens da tabela peixes
                SqlCommand listar = new SqlCommand("SELECT ID FROM Tanques", con);
                SqlDataReader leitor = listar.ExecuteReader();

                while (leitor.Read())
                {
                    ModeloBiometria bio = new ModeloBiometria();
                    bio.Id_Tanque = (int)leitor["ID"];
                    lista.Add(bio);
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


        //---MÉTODO PARA BUSCAR BIOMETRIA ESPECIFICO NA TABELA---//

        public static ModeloBiometria BuscaBiometria(int id)
        {

            ModeloBiometria bio = new ModeloBiometria();

            try
            {
                con.Open();
                SqlCommand query =
                    new SqlCommand("SELECT * FROM Biometria WHERE ID = @id", con);
                query.Parameters.AddWithValue("@id", id);
                SqlDataReader leitor = query.ExecuteReader();

                while (leitor.Read())
                {
                    //ModeloBiometria b = new ModeloBiometria();
                    bio.Id = (int)leitor["id"];
                    bio.Qtd_peixesTanque = (int)leitor["Qtd_PeixesTanque"];
                    bio.Qtd_peixesAmostra = (int)leitor["qtd_PeixesAmostra"];
                    bio.Id_Tanque = (int)leitor["FK_Tanques_ID"];
                    bio.Peso_Amostra = (float)leitor["peso_Amostra"];
                    bio.Peso_Medio = leitor["peso_Medio"].ToString();
                    bio.Ganho_PesoMedio = leitor["ganho_PesoMedio"].ToString();
                    bio.Ganho_PesoAmostra = (float)leitor["ganho_PesoAmostra"];
                    bio.Data = leitor["data"].ToString();

                }
            }
            catch (Exception e)
            {
                bio = null;
            }

            if (con.State == ConnectionState.Open)
                con.Close();

            return bio;
        }


        internal string EditarBiometria(int id)
        {

            try
            {
                //Abrir conexão
                con.Open();

                //Comando para fazer atualização de informaçao no BD
                SqlCommand atualizar = new SqlCommand("UPDATE Biometria SET Qtd_PeixesTanque = @qtd_PeixesTanque, Qtd_PeixesAmostra = @qtd_PeixesAmostra, peso_Amostra=@peso_Amostra, peso_Medio=@peso_Medio, ganho_PesoAmostra=@ganho_PesoAmostra, ganho_PesoMedio = @ganho_PesoMedio, data = @Data where Id = @ID", con);

                atualizar.Parameters.AddWithValue("@ID", id);
                atualizar.Parameters.AddWithValue("@qtd_PeixesTanque", qtd_PeixesTanque);
                atualizar.Parameters.AddWithValue("@qtd_PeixesAmostra", qtd_PeixesAmostra);
                atualizar.Parameters.AddWithValue("@peso_Amostra", peso_Amostra);
                atualizar.Parameters.AddWithValue("@peso_Medio", peso_Medio);
                atualizar.Parameters.AddWithValue("@ganho_PesoAmostra", ganho_PesoAmostra);
                atualizar.Parameters.AddWithValue("@ganho_PesoMedio", ganho_PesoMedio);
                atualizar.Parameters.AddWithValue("data", data);
                //atualizar.Parameters.AddWithValue("idTanques", id_Tanque);


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

        internal string RemoverBiometria(int id)
        {
            string res = "";
            try
            {
                con.Open();
                //Deletar a chave primária da tabela
                SqlCommand query =
                    new SqlCommand("DELETE FROM Biometria WHERE id = @id", con);
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
            return res;

        }

        public int PesquisaNumeroPeixes(int id)
        {
            ModeloTanque mt = new ModeloTanque();
            try
            {
            
                con.Open();
                SqlCommand select = new SqlCommand("SELECT Quantidade_Peixes from Tanques where ID = @Id", con);
                select.Parameters.AddWithValue("@Id", id);
                SqlDataReader leitor = select.ExecuteReader();


                while (leitor.Read())
                {
     
                    mt.QuantidadePeixes = (int)leitor["Quantidade_Peixes"];
                    
                }
            }

            catch (Exception e)
            {
                mt.QuantidadePeixes = 0;
            }
            if (con.State == ConnectionState.Open)
                con.Close();

            return mt.QuantidadePeixes;
        }

    }
}