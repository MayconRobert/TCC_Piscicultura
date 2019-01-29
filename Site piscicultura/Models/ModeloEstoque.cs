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
    public class ModeloEstoque
    {
        private int id, quantidade, id_Piscicultura;
        private string produto, categoria, validade;

        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BCD"].ConnectionString);

        [DisplayName("ID")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [DisplayName("Piscicultura")]
        public int Id_Piscicultura
        {
            get { return id_Piscicultura; }
            set { id_Piscicultura = value; }
        }
        [DisplayName("Quantidade")]
        public int Quantidade
        {
            get { return quantidade; }
            set { quantidade = value; }
        }
        [DisplayName("Produto")]
        public string Produto
        {
            get { return produto; }
            set { produto = value; }
        }
        [DisplayName("Categoria")]
        public string Categoria
        {
            get { return categoria; }
            set { categoria = value; }
        }
        [DisplayName("Validade")]
        public string Validade
        {
            get { return validade; }
            set { validade = value; }
        }
        
       
        //---MÉTODO PARA CADASTRAR PRODUTOS NO SISTEMA

        internal string InserirEstoque()
        {

            try
            {
                //Verificar se a conexao está aberta
            if (con.State == System.Data.ConnectionState.Open)

                con.Close();

                //Abrir conexão
                con.Open();

                //Comando para fazer a inserção das informações no BD
                SqlCommand inserir = new SqlCommand("INSERT INTO Produtos_Estoque VALUES( @produto, @categoria, @validade, @quantidade,  @id_Piscicultura)", con);

                //Atribuir os valores das variavéis do sistema para as colunas no BD
                inserir.Parameters.AddWithValue("@quantidade", quantidade);
                inserir.Parameters.AddWithValue("@produto", produto);
                inserir.Parameters.AddWithValue("@categoria", categoria);
                inserir.Parameters.AddWithValue("@validade", validade);
                inserir.Parameters.AddWithValue("@id_Piscicultura", id_Piscicultura);

                //Executar o comando no BD
                inserir.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                //Caso ocorra um erro retorna o tipo de erro para o ususario
                return e.Message;

            }


            //Verificar se a conexao está aberta
            if (con.State == System.Data.ConnectionState.Open)

                con.Close();

            //Retorna mensagem se cadastrou no banco

            return "Produto cadastrado com Sucesso!";



        }

        //----MÉTODO PARA EDITAR PRODUTOS
        internal string EditarEstoque(int id)
        {

            try
            {
                //Abrir conexão
                con.Open();

                //Comando para fazer atualização de informaçao no BD
                SqlCommand atualizar = new SqlCommand("UPDATE Produtos_Estoque SET Quantidade = @quantidade, Produto = @produto ,Categoria = @categoria ,Validade = @validade where id = @id", con);

                atualizar.Parameters.AddWithValue("@id", id);
                atualizar.Parameters.AddWithValue("@quantidade", quantidade);
                atualizar.Parameters.AddWithValue("@produto", produto);
                atualizar.Parameters.AddWithValue("@categoria", categoria);
                atualizar.Parameters.AddWithValue("@validade", validade);
              

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

        //--CRIAR A LISTA DE PRODUTOS CADASTRADOS---//

        public static List<ModeloEstoque> ListarEstoque(int idPisci)
        {

            //Criar um array para guardar as informações que virão do BD
            List<ModeloEstoque> lista = new List<ModeloEstoque>();


           
            try
            {
                con.Open();

                //Comando para selecionar os itens da tabela PRODUTOS
                SqlCommand listar = new SqlCommand("SELECT * FROM Produtos_Estoque where FK_Pisciculturas_ID = @piscicultura", con);
                listar.Parameters.AddWithValue("@piscicultura", idPisci);
                SqlDataReader leitor = listar.ExecuteReader();

                while (leitor.Read())
                {
                    ModeloEstoque p = new ModeloEstoque();
                    p.Id = (int)leitor["Id"];
                    p.Quantidade = (int)leitor["Quantidade"];
                    p.Produto = leitor["Produto"].ToString();
                    p.Categoria = leitor["Categoria"].ToString();
                    p.Validade = leitor["Validade"].ToString();
                  

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

        //---MÉTODO PARA BUSCAR UM PRODUTOS ESPECIFICO NA TABELA---//

        public static ModeloEstoque BuscaEstoque(int codigo)
        {
            ModeloEstoque p = new ModeloEstoque();
            
            try
            {
                con.Open();
                SqlCommand query =
                    new SqlCommand("SELECT * FROM Produtos_Estoque WHERE Id = @cod", con);
                query.Parameters.AddWithValue("@cod", codigo);
                SqlDataReader leitor = query.ExecuteReader();

                while (leitor.Read())
                {

                    p.Quantidade = (int)leitor["Quantidade"];
                    p.Produto = leitor["Produto"].ToString();
                    p.Categoria = leitor["Categoria"].ToString();
                    p.Validade = leitor["Validade"].ToString();
                
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

        //---MÉTODO PARA EXCLUIR PRODUTOS---//

        internal string Remover(int id)
        {
            string res = "";
            try
            {
                con.Open();
                //Deletar a chave primária da tabela
                SqlCommand query =
                    new SqlCommand("DELETE FROM Produtos_Estoque WHERE ID = @cod", con);
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

        public ModeloPiscicultura PegaIdPiscicultura()
        {
            ModeloPiscicultura mp = new ModeloPiscicultura();
            bool res = false;
            try
            {
                // ABRE CONEXÃO
                con.Open();
                //SELECIONA A TABELA
                SqlCommand query =
                    new SqlCommand("SELECT id from Pisciculturas WHERE id = @id", con);
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

            return mp;
        }

    }
}