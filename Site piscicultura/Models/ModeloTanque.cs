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
    public class ModeloTanque
    {
        private int id, piscicultura_id, racoes_id, peixes_cod, quantidadePeixes;
        private string nome, tipoAgua, nomePiscicultura, racao, especiePeixe;
        float volumeAgua;

        static SqlConnection con =
           new SqlConnection(ConfigurationManager.ConnectionStrings["BCD"].ConnectionString);
        [DisplayName("Ração")]
        public string Racao
        {
            get { return racao; }
            set { racao = value; }
        }
       
        [DisplayName("Espécie")]
        public string EspeciePeixe
        {
            get { return especiePeixe; }
            set { especiePeixe = value; }
        }

        [DisplayName("ID do Tanque")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [DisplayName("Quantidade de Peixes")]
        public int QuantidadePeixes
        {
            get { return quantidadePeixes; }
            set { quantidadePeixes = value; }
        }

        [DisplayName("Volume de água")]
        public float VolumeAgua
        {
            get { return volumeAgua; }
            set { volumeAgua = value; }
        }

        [DisplayName("Nome da piscicultura")]
        public string NomePiscultura
        {
            get { return nomePiscicultura; }
            set { nomePiscicultura = value; }
        }

        [DisplayName("Piscicultura")]
        public int Piscicultura_id
        {
            get { return piscicultura_id; }
            set { piscicultura_id = value; }
        }
        [DisplayName("Ração")]
        public int Racoes_id
        {
            get { return racoes_id; }
            set { racoes_id = value; }
        }
       [DisplayName("Espécie do peixe")]
        public int Peixes_cod
        {
            get { return peixes_cod; }
            set { peixes_cod = value; }
        }
        [DisplayName("Nome")]
        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }


        [DisplayName("Tipo da Água")]
        public string TipoAgua
        {
            get { return tipoAgua; }
            set { tipoAgua = value; }
        }


        public int PegaIdPiscicultura(string cpf)
        {
            ModeloPiscicultura mp = new ModeloPiscicultura();
            bool res = false;
            try
            {
                // ABRE CONEXÃO
                con.Open();
                //SELECIONA A TABELA
                SqlCommand query =
                    new SqlCommand("SELECT id from Pisciculturas WHERE Fk_Usuarios_CPF = @cpf", con);
                query.Parameters.AddWithValue("@cpf", cpf);
                //COMPARA OS VALORES
                query.Parameters.AddWithValue("@id", id);
                SqlDataReader leitor = query.ExecuteReader();
                //PERCORRE LINHA POR LINHA
                res = leitor.HasRows;
                if (leitor.Read())
                {
                    mp.Id = int.Parse(leitor["id"].ToString());
                }

                if (res == false)
                {
                    // FECHA LEITOR CASO ESTEJA ABERTO
                    leitor.Close();

                }
            }
            //CASO NÃO ENCONTRE
            catch (Exception e)
            {
                res = false;
               
            }
            //VERIFICA SE A CONEXÃO ESTÁ ABERTO
            if (con.State == ConnectionState.Open)
                //SE TIVER ABERTA, FECHA CONEXÃO
                con.Close();
            //CONFERE AS RESPOSTAS CASO FOR FALSO

            return mp.Id;
        }

        public ModeloPeixes PegaIdPeixes()
        {
            ModeloPeixes mpe = new ModeloPeixes();
            bool res = false;
            try
            {
                // ABRE CONEXÃO
                con.Open();
                //SELECIONA A TABELA
                SqlCommand query =
                    new SqlCommand("SELECT cod from Peixes WHERE cod = @cod", con);
                //COMPARA OS VALORES
                query.Parameters.AddWithValue("@cod", peixes_cod);
                SqlDataReader leitor = query.ExecuteReader();
                //PERCORRE LINHA POR LINHA
                res = leitor.HasRows;
                if (leitor.Read())
                {
                    mpe.Cod = int.Parse(leitor["Cod"].ToString());
                }

                if (res == false)
                {
                    // FECHA LEITOR CASO ESTEJA ABERTO
                    leitor.Close();

                }
            }
            //CASO NÃO ENCONTRE
            catch (Exception e)
            {
                res = false;

            }
            //VERIFICA SE A CONEXÃO ESTÁ ABERTO
            if (con.State == ConnectionState.Open)
                //SE TIVER ABERTA, FECHA CONEXÃO
                con.Close();
            //CONFERE AS RESPOSTAS CASO FOR FALSO

            return mpe;
        }

        public ModeloRacao PegaIdRacao()
        {
            ModeloRacao mr = new ModeloRacao();
            bool res = false;
            try
            {
                // ABRE CONEXÃO
                con.Open();
                //SELECIONA A TABELA
                SqlCommand query =
                    new SqlCommand("SELECT id from Racoes", con);
                //COMPARA OS VALORES
           
                SqlDataReader leitor = query.ExecuteReader();
                //PERCORRE LINHA POR LINHA
                res = leitor.HasRows;
                if (leitor.Read())
                {
                    mr.Id = int.Parse(leitor["Id"].ToString());
                }

                if (res == false)
                {
                    // FECHA LEITOR CASO ESTEJA ABERTO
                    leitor.Close();

                }
            }
            //CASO NÃO ENCONTRE
            catch (Exception e)
            {
                res = false;

            }
            //VERIFICA SE A CONEXÃO ESTÁ ABERTO
            if (con.State == ConnectionState.Open)
                //SE TIVER ABERTA, FECHA CONEXÃO
                con.Close();
            //CONFERE AS RESPOSTAS CASO FOR FALSO

            return mr;
        }


        //MÉTODO DE CRIAR TANQUE
        internal string CriarTanque()
        {
            //TESTAR A CONEXÃO PARA VER SE ESTÁ CERTO
            try
            {

                con.Open();
                SqlCommand query = new SqlCommand("INSERT INTO Tanques values (@nome, @quantidadePeixes, @VolumeAgua, @tipo_Agua, @pisciculturas_Id, @Peixe_cod, @racoes_Id)", con);
                query.Parameters.AddWithValue("@nome", nome);
                query.Parameters.AddWithValue("@quantidadePeixes", quantidadePeixes);
                query.Parameters.AddWithValue("@volumeAgua", volumeAgua);
                query.Parameters.AddWithValue("@tipo_Agua", tipoAgua);
                query.Parameters.AddWithValue("@pisciculturas_Id", piscicultura_id);
                query.Parameters.AddWithValue("@racoes_Id", racoes_id);
                query.Parameters.AddWithValue("@peixe_Cod", peixes_cod);

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
            return "Tanque registrado com sucesso!"; // Retorna a mensagem

        }

        //--METODO PARA EDITAR TANQUE
        internal string EditarTanque()
        {
            try
            {
                //Abrir conexão
                con.Open();
                //Comando para fazer atualização de informaçao no BD
                SqlCommand atualizar = new SqlCommand("UPDATE Tanques SET  nome = @nome, Quantidade_Peixes = @qtdPeixes, tipo_Agua = @tipoAgua, Volum_Agua = @Volume_Agua where ID = @id", con);
                atualizar.Parameters.AddWithValue("@nome", nome);
                atualizar.Parameters.AddWithValue("@qtdPeixes", quantidadePeixes);
                atualizar.Parameters.AddWithValue("@tipoAgua", tipoAgua);
                atualizar.Parameters.AddWithValue("@Volume_Agua", volumeAgua);
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

        //MÉTODO PARA REMOVER TANQUE
        internal string RemoverTanque(int id)
        {
            string res = "";

            try
            {
                con.Open();

                //Deletar a chave primária da tabela
                SqlCommand query =
                    new SqlCommand("DELETE FROM Tanques WHERE ID = @id", con);

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

        //LISTAR OS TANQUES

        public static List<ModeloTanque> ListarTanques(int idPiscicultura)
        {

            //Criar um array para guardar as informações que virão do BD
            List<ModeloTanque> lista = new List<ModeloTanque>();


            try
            {
                con.Open();

                //Comando para selecionar os itens da tabela PRODUTOS
                SqlCommand listar = new SqlCommand("SELECT *, A.Nome as 'NomePiscicultura', R.Nome as 'NomeRaçao' FROM Tanques T, Racoes R, Peixes P, Pisciculturas A "
                +"where T.FK_Peixes_Cod = P.Cod "
                +"and T.FK_Racoes_ID = R.ID "
                +"and T.FK_Pisciculturas_Id = A.ID "
                +"and A.ID = @IdPiscicultura ", con);
                listar.Parameters.AddWithValue("@IDPiscicultura", idPiscicultura);
                SqlDataReader leitor = listar.ExecuteReader();

                while (leitor.Read())
                {
                    ModeloTanque p = new ModeloTanque();
                    p.Id = (int)leitor["Id"];
                    p.Nome = leitor["Nome"].ToString();
                    p.TipoAgua = leitor["Tipo_Agua"].ToString();
                    p.VolumeAgua = (float)leitor["Volum_Agua"];
                    p.QuantidadePeixes = (int)leitor["Quantidade_Peixes"];
                    p.Piscicultura_id = (int)leitor["FK_Pisciculturas_ID"];
                    p.Peixes_cod = (int)leitor["FK_Peixes_Cod"];
                    p.Racoes_id = (int)leitor["FK_Racoes_ID"];
                    p.Racao = leitor["NomeRaçao"].ToString();
                    p.EspeciePeixe = leitor["Especie"].ToString();
                    p.NomePiscultura = leitor["NomePiscicultura"].ToString();
                    

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

        public List<ModeloPiscicultura> SelectNomePisciculturas(string cpf)
        {
            List<ModeloPiscicultura> lista = new List<ModeloPiscicultura>();
            try
            {
                con.Open();
                SqlCommand select = new SqlCommand("SELECT Nome, Id from Pisciculturas where FK_Usuarios_CPF = @Cpf", con);
                select.Parameters.AddWithValue("@cpf", cpf);

                SqlDataReader leitor = select.ExecuteReader();


                while (leitor.Read())
                {
                    ModeloPiscicultura m = new ModeloPiscicultura();

                    m.Nome = leitor["Nome"].ToString();
                    m.Id = (int)leitor["Id"];
                    lista.Add(m);
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

        public List<ModeloPeixes> SelectNomePeixes()
        {
            List<ModeloPeixes> lista = new List<ModeloPeixes>();
            try
            {
                con.Open();
                SqlCommand select = new SqlCommand("SELECT Especie, Cod from Peixes", con);
              

                SqlDataReader leitor = select.ExecuteReader();


                while (leitor.Read())
                {
                    ModeloPeixes m = new ModeloPeixes();

                    m.Especie = leitor["Especie"].ToString();
                    m.Cod = (int)leitor["Cod"];
                    lista.Add(m);
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

        public List<ModeloRacao> SelectNomeRacoes()
        {
            List<ModeloRacao> lista = new List<ModeloRacao>();
            try
            {
                con.Open();
                SqlCommand select = new SqlCommand("SELECT Nome, Id from Racoes", con);


                SqlDataReader leitor = select.ExecuteReader();


                while (leitor.Read())
                {
                    ModeloRacao m = new ModeloRacao();

                    m.Nome = leitor["Nome"].ToString();
                    m.Id = (int)leitor["Id"];
                    lista.Add(m);
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

        public List<ModeloTanque> SelectNomeTanques(string cpf)
        { 

            List<ModeloTanque> lista = new List<ModeloTanque>();
            try
            {
                con.Open();
                SqlCommand select = new SqlCommand("SELECT *, T.Nome as 'NomeTanque', T.ID as 'IdTanque' from Tanques T, Usuarios U, Pisciculturas P "+
                "where T.FK_Pisciculturas_ID = P.Id "+
                "and P.FK_Usuarios_CPF = U.Cpf "+
                "and U.CPF = @cpf;", con);
                select.Parameters.AddWithValue("@cpf", cpf);
               


                SqlDataReader leitor = select.ExecuteReader();


                while (leitor.Read())
                {
                    ModeloTanque m = new ModeloTanque();

                    m.nome = leitor["NomeTanque"].ToString();
                    m.Id = (int)leitor["IDTanque"];
                    m.QuantidadePeixes = (int)leitor["Quantidade_Peixes"];
                    lista.Add(m);
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

        public int PesquisaNumeroTanques(string cpf, int idPiscicultura)
        {
            int numeroDeTanques = 0;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BCD"].ConnectionString);
            try
            {
                con.Open();
                SqlCommand query =
                    new SqlCommand("SELECT  U.cpf, count(*) qtd From Tanques T, Pisciculturas P, Usuarios U "+
                                    "where "+
                                    "T.FK_Pisciculturas_ID = @idPiscicultura and "+
                                    "P.FK_Usuarios_CPF = @cpf "+
                                    "group by u.cpf", con);
                query.Parameters.AddWithValue("@cpf", cpf);
                query.Parameters.AddWithValue("@idPiscicultura", idPiscicultura);
                SqlDataReader leitor = query.ExecuteReader();

                while (leitor.Read())
                {
                    //m.cpf = leitor["cpf"].ToString();
                    numeroDeTanques = (int)leitor["qtd"];

                }
            }
            catch (Exception e)
            {
               numeroDeTanques = 0;
                string resposta = e.Message;

            }

            if (con.State == ConnectionState.Open)
                con.Close();
            return numeroDeTanques;
        }

        public static ModeloTanque BuscaTanque(int id)
        {
            ModeloTanque l = new ModeloTanque();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BCD"].ConnectionString);
            try
            {
                con.Open();
                SqlCommand query = new SqlCommand("SELECT *, A.Nome as 'NomePiscicultura', R.Nome as 'NomeRaçao' FROM Tanques T, Racoes R, Peixes P, Pisciculturas A "
               + "where T.FK_Peixes_Cod = P.Cod "
               + "and T.FK_Racoes_ID = R.ID "
               + "and T.FK_Pisciculturas_Id = A.ID "
               + "and T.ID = @Id ", con);
                //SqlCommand query = new SqlCommand("Select * from Tanques where Id = @ID", con);
                query.Parameters.AddWithValue("@ID", id);
              
              
                SqlDataReader leitor = query.ExecuteReader();

                while (leitor.Read())
                {
                    l.id = (int)leitor["Id"];
                    l.nome = leitor["Nome"].ToString();
                    l.quantidadePeixes = (int)leitor["Quantidade_Peixes"];
                    l.volumeAgua = (float)leitor["Volum_Agua"];
                    l.tipoAgua = leitor["Tipo_Agua"].ToString();
                    l.racao = leitor["NomeRaçao"].ToString();
                    l.nomePiscicultura = leitor["NomePiscicultura"].ToString();
                    l.especiePeixe = leitor["Especie"].ToString();



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

    }
}