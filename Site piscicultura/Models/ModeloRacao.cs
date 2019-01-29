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
    public class ModeloRacao
    {

        static SqlConnection con =
         new SqlConnection(ConfigurationManager.ConnectionStrings["BCD"].ConnectionString);

        private int id;
        private string nome, fabricante, peso, tamanho, proteina, ingredientePrincipal, tipoRacao, cpfUsuario;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

        public string Fabricante
        {
            get { return fabricante; }
            set { fabricante = value; }
        }

        public string Peso
        {
            get { return peso; }
            set { peso = value; }
        }

        public string Tamanho
        {
            get { return tamanho; }
            set { tamanho = value; }
        }

        [DisplayName("Proteína")]
        public string Proteina
        {
            get { return proteina; }
            set { proteina = value; }
        }

        [DisplayName("Ingrediente Principal")]
        public string IngredientePrincipal
        {
            get { return ingredientePrincipal; }
            set { ingredientePrincipal = value; }
        }

        [DisplayName("Tipo da Ração")]
        public string TipoRacao
        {
            get { return tipoRacao; }
            set { tipoRacao = value; }
        }

        
        public string CpfUsuario
        {
            get { return cpfUsuario; }
            set { cpfUsuario = value; }
        }


        internal string AdicionarRacao()
        {
            //TESTAR A CONEXÃO PARA VER SE ESTÁ CERTO
            try
            {

                con.Open();
                SqlCommand query = new SqlCommand("INSERT INTO Racoes values (@nome, @fabricante, @peso, @tamanho, @proteina, @ingrediente, @tipo, @cpfUsuario)", con);
             
                query.Parameters.AddWithValue("@nome", nome);
                query.Parameters.AddWithValue("@fabricante", fabricante);
                query.Parameters.AddWithValue("@peso", peso);
                query.Parameters.AddWithValue("@tamanho", tamanho);
                query.Parameters.AddWithValue("@proteina", proteina);
                query.Parameters.AddWithValue("@ingrediente", ingredientePrincipal);
                query.Parameters.AddWithValue("@tipo", tipoRacao);
                query.Parameters.AddWithValue("@cpfUsuario", cpfUsuario);

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
            return "Ração registrada com sucesso!";

        }
        //----MÉTODO PARA EDITAR RAÇÕES
        internal string EditarRacao()
        {

            try
            {
                //Abrir conexão
                con.Open();

                //Comando para fazer atualização de informaçao no BD
                SqlCommand query = new SqlCommand("UPDATE Racoes SET Nome = @nome ,Fabricante = @fabricante ,Peso = @peso ,Tamanho = @tamanho ,Proteina = @proteina ,Ingrediente_Principal=@ingredientes ,Tipo_Racao=@tipo where Id = @id", con);

                query.Parameters.AddWithValue("@id", id);
                query.Parameters.AddWithValue("@nome", nome);
                query.Parameters.AddWithValue("@fabricante", fabricante);
                query.Parameters.AddWithValue("@peso", peso);
                query.Parameters.AddWithValue("@tamanho", tamanho);
                query.Parameters.AddWithValue("@proteina", proteina);
                query.Parameters.AddWithValue("@ingredientes", ingredientePrincipal);
                query.Parameters.AddWithValue("@tipo", tipoRacao);
                query.ExecuteNonQuery();



            }
            catch (Exception m)
            {

                return m.Message;
            }

            if (con.State == ConnectionState.Open)
                con.Close();


            return "Editado com Sucesso";

        }

        //--CRIAR A LISTA DE RAÇÕES CADASTRADOS---//

        public static List<ModeloRacao> ListarRacao(string cpf)
        {

            //Criar um array para guardar as informações que virão do BD
            List<ModeloRacao> lista = new List<ModeloRacao>();

            try
            {
                con.Open();

                //Comando para selecionar os itens da tabela RAÇOES
                SqlCommand listar = new SqlCommand("SELECT * FROM Racoes where FK_Usuarios_CPF = @cpf ", con);
                listar.Parameters.AddWithValue("@cpf", cpf);
                SqlDataReader leitor = listar.ExecuteReader();

                while (leitor.Read())
                {
                    ModeloRacao p = new ModeloRacao();
                    p.Id = (int)leitor["Id"];
                    p.Nome = leitor["Nome"].ToString();
                    p.Fabricante = leitor["Fabricante"].ToString();
                    p.Peso = leitor["Peso"].ToString();
                    p.Tamanho = leitor["Tamanho"].ToString();
                    p.Proteina = leitor["Proteina"].ToString();
                    p.IngredientePrincipal = leitor["Ingrediente_Principal"].ToString();
                    p.TipoRacao = leitor["Tipo_Racao"].ToString();

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

        //---MÉTODO PARA BUSCAR UMA RAÇÃO ESPECIFICO NA TABELA---//

        public static ModeloRacao BuscaRacao(int codigo)
        {
            ModeloRacao p = new ModeloRacao();

            try
            {
                con.Open();
                SqlCommand query =
                    new SqlCommand("SELECT * FROM Racoes WHERE Id = @cod", con);
                query.Parameters.AddWithValue("@cod", codigo);
                SqlDataReader leitor = query.ExecuteReader();

                while (leitor.Read())
                {
                    p.Id = (int)leitor["Id"];
                    p.Nome = leitor["Nome"].ToString();
                    p.Fabricante = leitor["Fabricante"].ToString();
                    p.Peso = leitor["Peso"].ToString();
                    p.Tamanho = leitor["Tamanho"].ToString();
                    p.Proteina = leitor["Proteina"].ToString();
                    p.IngredientePrincipal = leitor["Ingrediente_Principal"].ToString();
                    p.TipoRacao = leitor["Tipo_Racao"].ToString();

                }
            }
            catch (Exception e)
            {
                p = null;
            }

            if (con.State == ConnectionState.Open)
                con.Close();

            return p;
        }

        //---MÉTODO PARA EXCLUIR RACÕES---//

        internal string Remover(int id)
        {
            string res = "";
            try
            {
                con.Open();
                //Deletar a chave primária da tabela
                SqlCommand query =
                    new SqlCommand("DELETE FROM Racoes WHERE Id = @cod", con);
                query.Parameters.AddWithValue("@cod", id);

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
        public int pesquisaNumeroRacoes(string cpf)
        {
            int numeroDeRacoes = 0;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BCD"].ConnectionString);
            try
            {
                con.Open();
                SqlCommand query =
                    new SqlCommand("SELECT count (*) racoesRegistradas FROM Racoes WHERE FK_Usuarios_CPF = @cpf", con);
                query.Parameters.AddWithValue("@cpf", cpf);
                SqlDataReader leitor = query.ExecuteReader();

                while (leitor.Read())
                {
                   
                    numeroDeRacoes = (int)leitor["racoesRegistradas"];

                }
            }
            catch (Exception e)
            {
                numeroDeRacoes= 0;
                string resposta = e.Message;

            }

            if (con.State == ConnectionState.Open)
                con.Close();
            return numeroDeRacoes;
        }

    }
}
    
